using System.Collections.Generic;
using System.Reflection;
using Monk.Core.Attributes;

namespace Monk.Core.Helpers
{
    internal class PriorityComparer : IComparer<Assembly>
    {
        /// <summary>
        /// Compares priorirties of two asseblies based on ModuleLoadPriorityAttribute
        /// </summary>
        public int Compare(Assembly x, Assembly y)
        {
            var xPriority = ReadPriority(x);
            var yPriority = ReadPriority(y);
            return xPriority.CompareTo(yPriority);
        }

        /// <summary>
        /// ModuleLoadPriorityAttribute value from assable if exists
        /// </summary>
        public static int ReadPriority(Assembly assembly)
        {
            var orderPriorityList = assembly.GetCustomAttributes(
                typeof(ModuleLoadPriorityAttribute), false);
            if (orderPriorityList.Length > 0)
                return ((ModuleLoadPriorityAttribute)orderPriorityList[0]).Priority;
            return 100;
        }
    }
}
