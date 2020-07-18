using System.Collections.Generic;

namespace HP.ScalableTest.Core
{
    /// <summary>
    /// Provides extension methods for various attributes of each <see cref="VirtualResourceType" />.
    /// </summary>
    public static class VirtualResourceTypeAttributes
    {
        #region Attribute Sets

        private static readonly HashSet<VirtualResourceType> _pluginResources = new HashSet<VirtualResourceType>
        {
            VirtualResourceType.OfficeWorker,
            VirtualResourceType.AdminWorker,
            VirtualResourceType.CitrixWorker,
            VirtualResourceType.SolutionTester,
            VirtualResourceType.LoadTester
        };

        private static readonly HashSet<VirtualResourceType> _indefiniteResources = new HashSet<VirtualResourceType>
        {
            VirtualResourceType.PerfMonCollector,
            VirtualResourceType.EventLogCollector
        };

        #endregion

        /// <summary>
        /// Indicates whether the virtual resource type uses plugins to execute.
        /// </summary>
        /// <param name="resourceType">The <see cref="VirtualResourceType" />.</param>
        /// <returns><c>true</c> if the virtual resource type uses plugins, <c>false</c> otherwise.</returns>
        public static bool UsesPlugins(this VirtualResourceType resourceType)
        {
            return _pluginResources.Contains(resourceType);
        }

        /// <summary>
        /// Indicates whether the virtual resource type runs for a specified duration and then completes.
        /// </summary>
        /// <param name="resourceType">The <see cref="VirtualResourceType" />.</param>
        /// <returns><c>true</c> if the virtual resource type runs to completion, <c>false</c> otherwise.</returns>
        public static bool RunsToCompletion(this VirtualResourceType resourceType)
        {
            return !_indefiniteResources.Contains(resourceType);
        }

        /// <summary>
        /// Indicates whether the virtual resource type runs indefinitely,
        /// e.g. a monitor resources that continues running until the rest of the session completes.
        /// </summary>
        /// <param name="resourceType">The <see cref="VirtualResourceType" />.</param>
        /// <returns><c>true</c> if the virtual resource type runs indefinitely, <c>false</c> otherwise.</returns>
        public static bool RunsIndefinitely(this VirtualResourceType resourceType)
        {
            return _indefiniteResources.Contains(resourceType);
        }
    }
}
