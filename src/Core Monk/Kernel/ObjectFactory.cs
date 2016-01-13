using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Monk.Core.Exceptions;
using Monk.Core.Helpers;
using Ninject;
using Ninject.Modules;

namespace Monk.Core.Kernel
{
    public class ObjectFactory
    {
        #region Constants

        public const string APP_SETTING_KEY_AUTO_LOAD_MODULE_PATTERN = "AutoLoadModulePattern";
        public const string APP_SETTING_KEY_EXCLUDE_AUTO_LOAD_MODULE_PATTERN = "ExcludeAutoLoadModulePattern";
        private const string EVENT_VIEWER_SOURCE = "Monk";
        private const char ASSEMBLIES_SPLIT_CHAR = ',';

        #endregion     

        private static IKernel _kernel;

        public static IKernel Kernel
        {
            get
            {
                if (_kernel == null)
                    BuildKernel();
                return _kernel;
            }
        }

        public static object Get(Type type)
        {
            return Kernel.Get(type);
        }

        /// <summary>
        /// This must be used only in unit testing when setting a different kernel from the StandardKernel (ex: RhinoKernel)
        /// Returns The settings used for the Kernel
        /// </summary>
        protected static INinjectSettings GetSettings()
        {
            return new NinjectSettings() { LoadExtensions = true, InjectNonPublic = true };
        }

        /// <summary>
        /// This must be used only in unit testing when setting a different kernel from the StandardKernel (ex: RhinoKernel)
        /// </summary>
        public static void AssignKernel(IKernel kernel)
        {
            _kernel = kernel;
        }

        /// <summary>
        /// This must be used only in unit testing when setting a different kernel from the StandardKernel (ex: RhinoKernel)
        /// Resets the Kernel to null
        /// </summary>
        public static void ResetKernel()
        {
            AssignKernel(null);
        }

        public static void ResolveDependencies(object ob)
        {
            Kernel.Inject(ob);
        }

        #region Privates

        /// <summary>
        /// Builds the Kernel by activating all the ninject modules
        /// </summary>
        private static void BuildKernel()
        {
            AssignKernel(new StandardKernel(GetSettings(), new INinjectModule[] { }));
            var candidateAssembliesToLoad = GetAssembliesContainingModulesToLoad();
            var rejectedAssembliesToLoad = new List<Assembly>();
            foreach (var candidateAssembly in candidateAssembliesToLoad)
            {
                try
                {
                    _kernel.Load(candidateAssembly);
                    WriteEntryLog(String.Format("{0} has been loaded", candidateAssembly.FullName), false);
                }
                catch (FileNotFoundException ex)
                {
                    LogToEventViewer(ex, candidateAssembly);
                    rejectedAssembliesToLoad.Add(candidateAssembly);
                }
                catch (Exception ex)
                {
                    LogToEventViewer(ex, candidateAssembly);
                }
            }

            if (rejectedAssembliesToLoad.Count != 0)
            {
                FormatUnableToLoadNinjectModulesException(rejectedAssembliesToLoad);
                throw new ActivationException("An error occured during the configuration of the Kernel. Check Event Viwer for details.");
            }

        }

        /// <summary>
        /// Returns assemblies containg ninject modules that are ready to be loaded.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Assembly> GetAssembliesContainingModulesToLoad()
        {
            var assemblyNamesToLoad = GetAssemblyPatternsFromConfigToLoad();
            var fileNamesToAvoid = GetFileNamesToAvoid();
            var result = GetValidatedAssemblies(assemblyNamesToLoad, fileNamesToAvoid);
            WriteFoundAssembliesInLog(result);
            result.Sort(new PriorityComparer());
            return result;
        }

        /// <summary>
        /// Return all the patterns for loading Assemblies defined in the config
        /// </summary>
        private static string[] GetAssemblyPatternsFromConfigToLoad()
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(APP_SETTING_KEY_AUTO_LOAD_MODULE_PATTERN))
                throw new DependencyInjectionException(
                    String.Format("Cannot configure ObjectFactory: your app.config must contain an app setting key [{0}] " +
                                  "and the value must contains assembly search patterns separated by [{1}]",
                                  APP_SETTING_KEY_AUTO_LOAD_MODULE_PATTERN, ASSEMBLIES_SPLIT_CHAR));
            var assemblyNamesToLoad = ConfigurationManager.AppSettings[APP_SETTING_KEY_AUTO_LOAD_MODULE_PATTERN].Split(ASSEMBLIES_SPLIT_CHAR);
            return assemblyNamesToLoad;
        }

        /// <summary>
        /// Return all the assembly patterns that needs to be avoided from loading defined in the config
        /// </summary>
        private static IEnumerable<string> GetAssemblyPatternsFromConfigToAvoid()
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(APP_SETTING_KEY_EXCLUDE_AUTO_LOAD_MODULE_PATTERN))
                return new string[0];
            return ConfigurationManager.AppSettings[APP_SETTING_KEY_EXCLUDE_AUTO_LOAD_MODULE_PATTERN].Split(ASSEMBLIES_SPLIT_CHAR);
        }

        /// <summary>
        /// Returns a list of files that match the pattern ExcludeAutoLoadModulePattern.
        /// </summary>
        private static List<string> GetFileNamesToAvoid()
        {
            var assemblyNamesToAvoid = GetAssemblyPatternsFromConfigToAvoid();
            return (from assemblyPattern in assemblyNamesToAvoid
                    let currentAssemblyUri = new Uri(Assembly.GetExecutingAssembly().CodeBase)
                    let currenyAssemblyDirectory = new FileInfo(currentAssemblyUri.LocalPath).Directory
                    where currenyAssemblyDirectory != null
                    from file in currenyAssemblyDirectory.GetFiles(assemblyPattern)
                    select file.Name).ToList();
        }

        private static void FormatUnableToLoadNinjectModulesException(IEnumerable<Assembly> rejectedAssembliesToLoad)
        {
            var sb = new StringBuilder();
            sb.AppendLine("An error occured during the loading of some Ninject Assemblies deployed in your bin folder.");
            sb.AppendLine("** Review previous event logs for the detailed reason of failure **");
            sb.AppendLine("Probably some references are missing!");
            sb.AppendLine("The following assemblies failed to load:");
            foreach (var assembly in rejectedAssembliesToLoad)
                sb.AppendLine(assembly.FullName);
            var excpetionToLaunch = new DependencyInjectionException(sb.ToString());
            LogToEventViewer(excpetionToLaunch);
            throw excpetionToLaunch;
        }

        /// <summary>
        /// Returns assemblies contains ninject modules that can be safetly loaded and that are not excluded.
        /// </summary>
        private static List<Assembly> GetValidatedAssemblies(string[] assemblyNamesToLoad, List<string> fileNamesToAvoid)
        {
            var result = new List<Assembly>();
            foreach (var assemblyPattern in assemblyNamesToLoad)
            {
                var currentAssemblyUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
                var currenyAssemblyDirectory = new FileInfo(currentAssemblyUri.LocalPath).Directory;
                if (currenyAssemblyDirectory != null)
                {
                    var files = currenyAssemblyDirectory.GetFiles(assemblyPattern);
                    foreach (var file in files)
                        if (!fileNamesToAvoid.Contains(file.Name) && FileIsCandidateAssembly(file))
                        {
                            var candidateAssembly = LoadAssemblyByFileOrNull(file);
                            if (candidateAssembly != null && !result.Contains(candidateAssembly))
                                result.Add(candidateAssembly);
                        }
                }
            }
            return result;
        }

        /// <summary>
        /// try to load and return assemly by filename. if error occurs, a null ref is returned.
        /// </summary>
        private static Assembly LoadAssemblyByFileOrNull(FileInfo file)
        {
            Assembly assemblyToLoad = null;
            try
            {
                assemblyToLoad =
                    Assembly.Load(file.Name.ToLower()
                                      .Replace(".dll", String.Empty)
                                      .Replace(".exe", String.Empty));
            }
            catch (Exception ex)
            {
                WriteEntryLog(
                    String.Format(
                        "Assembly {0} match the provided {2} , but is not a compatible .net assembly." +
                        "If this assembly is not required, please include it in the app.config under the ExcludeAutoLoadModulePattern. Error is {1}.",
                        file.Name, ex.Message, APP_SETTING_KEY_AUTO_LOAD_MODULE_PATTERN), true);
            }
            return assemblyToLoad;
        }

        /// <summary>
        /// Check if given file is an assembly 
        /// </summary>
        private static bool FileIsCandidateAssembly(FileInfo file)
        {
            return !file.Name.ToLower().EndsWith(".vshost.exe") && (file.Name.ToLower().EndsWith(".dll") || file.Name.ToLower().EndsWith(".exe"));
        }

        /// <summary>
        /// writes a message in the Event Viewer by proving usefull infomration on what assemlbies have been loaded correctly
        /// </summary>
        private static void WriteFoundAssembliesInLog(List<Assembly> result)
        {
            if (result == null || result.Count == 0)
            {
                var messageToThrow =
                    String.Format("Cannot configure ObjectFactory: no assemblies are found with the search pattern {0}",
                                  ConfigurationManager.AppSettings[APP_SETTING_KEY_AUTO_LOAD_MODULE_PATTERN]);
                WriteEntryLog(messageToThrow, true);
                throw new DependencyInjectionException(messageToThrow);
            }

            var sb = new StringBuilder();
            sb.AppendLine("###### the kernel's object facory has started using the auto module loading ######");
            sb.AppendLine("the patter defined in config for searching modules in assemblies is: ");
            sb.AppendLine(ConfigurationManager.AppSettings[APP_SETTING_KEY_AUTO_LOAD_MODULE_PATTERN]);
            sb.AppendLine("Assemblies that matched selected patters are listed below. Please note that lower is the priority," +
                          "earlier the module is loaded. ");
            foreach (var foundAssembly in result)
                sb.AppendLine(string.Format("- {0} with module loading priority: {1} ", foundAssembly.FullName, PriorityComparer.ReadPriority(foundAssembly)));
            WriteEntryLog(sb.ToString(), false);
        }

        private static void LogToEventViewer(Exception ex, Assembly faultedAssembly = null)
        {
            var sb = new StringBuilder();
            if (faultedAssembly != null) sb.AppendLine("An error occured in loading ninject modules from assembly: " + faultedAssembly.FullName);
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            WriteInnerExceptionIfExists(sb, ex);
            WriteEntryLog(sb.ToString(), true);
        }

        /// <summary>
        /// writes a string in the event viwer
        /// </summary>
        private static void WriteEntryLog(string message, bool isError)
        {
            //EventLog.WriteEntry(EVENT_VIEWER_SOURCE, message, isError ? EventLogEntryType.Error : EventLogEntryType.Information);
        }

        private static void WriteInnerExceptionIfExists(StringBuilder sb, Exception ex)
        {
            if (ex.InnerException != null)
            {
                var innerEx = ex.InnerException;
                sb.AppendLine("Inner:");
                sb.AppendLine(innerEx.Message);
                sb.AppendLine(innerEx.StackTrace);
                WriteInnerExceptionIfExists(sb, innerEx);
            }
        }

        #endregion
       
    }
}
