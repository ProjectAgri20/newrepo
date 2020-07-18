using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Class to retrieve counters, categories, and instances for the listbox.
    /// </summary>
    public static class PerfMonController
    {
        public const string InstanceDoesNotApply = "N/A";
        
        /// <summary>
        /// Retrieves the performance counter categories for the machine name specified
        /// </summary>
        /// <param name="machineName">the target host</param>
        /// <returns></returns>
        public static Collection<PerformanceCounterCategory> GetCategories(string machineName)
        {
            IOrderedEnumerable<PerformanceCounterCategory> categories = null;
            categories = PerformanceCounterCategory.GetCategories(machineName).OrderBy(c => c.CategoryName);

            return  new Collection<PerformanceCounterCategory>(categories.ToList());
        }

        /// <summary>
        /// Retrieves the instances available for the specified performance counter category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static Collection<string> GetInstances(PerformanceCounterCategory category)
        {
            if (category == null)
                return null;

            string[] instances = category.GetInstanceNames();

            if (instances.Length > 0)
            {
                Array.Sort(instances);
            }
            else
            {
                instances = new string[] { InstanceDoesNotApply };
            }

            return new Collection<string>(instances.ToList());
        }

        /// <summary>
        /// retrieves the list of counters available for the specified performance counter category and the instance
        /// </summary>
        /// <param name="category"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        public static Collection<PerformanceCounter> GetCounters(PerformanceCounterCategory category, string instanceName)
        {
            if (category == null || string.IsNullOrEmpty(instanceName))
                return null;

            IEnumerable<PerformanceCounter> counters = null;

            if (instanceName == InstanceDoesNotApply)
            {
                instanceName = string.Empty;
            }

            counters = category.GetCounters(instanceName).OrderBy(c => c.CounterName);

            return new Collection<PerformanceCounter>(counters.ToList());
        }
    }
}
