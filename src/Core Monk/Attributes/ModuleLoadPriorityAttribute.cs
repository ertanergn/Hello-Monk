using System;

namespace Monk.Core.Attributes
{
    /// <summary>
    /// This attribute can be defined in AssemblyInfos.cs for assemblies that contain NinjectModules. Provieds delayed loading
    /// Based on the value defined in the Priority, it defines the priority for autoloading the assemblies.
    /// lowest values come's first.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public abstract class ModuleLoadPriorityAttribute : Attribute
    {
        public int Priority { get; set; }

        protected ModuleLoadPriorityAttribute(int priority)
        {
            this.Priority = priority;
        }
    }
}
