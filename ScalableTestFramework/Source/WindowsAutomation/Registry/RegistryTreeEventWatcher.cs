using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Watches for changes to one or more trees (recursively) in the system registry.
    /// </summary>
    public sealed class RegistryTreeEventWatcher : RegistryEventWatcher
    {
        private readonly IEnumerable<string> _rootPaths;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryTreeEventWatcher" /> class.
        /// </summary>
        /// <param name="hive">The <see cref="RegistryHive" /> that will be monitored.</param>
        /// <param name="rootPath">The root path to monitor.  (Backslashes must be escaped.)</param>
        /// <exception cref="ArgumentException">The specified <see cref="RegistryHive" /> does not support event watching.</exception>
        public RegistryTreeEventWatcher(RegistryHive hive, string rootPath)
            : this(hive, new[] { rootPath })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryTreeEventWatcher" /> class.
        /// </summary>
        /// <param name="hive">The <see cref="RegistryHive" /> that will be monitored.</param>
        /// <param name="rootPaths">The root paths to monitor.  (Backslashes must be escaped.)</param>
        /// <exception cref="ArgumentNullException"><paramref name="rootPaths" /> is null.</exception>
        /// <exception cref="ArgumentException">
        /// The specified <see cref="RegistryHive" /> does not support event watching.
        /// <para>or</para>
        /// <paramref name="rootPaths" /> is empty.
        /// </exception>
        public RegistryTreeEventWatcher(RegistryHive hive, IEnumerable<string> rootPaths)
            : base(hive, "RegistryTreeChangeEvent")
        {
            if (rootPaths == null)
            {
                throw new ArgumentNullException(nameof(rootPaths));
            }

            if (!rootPaths.Any())
            {
                throw new ArgumentException("At least one root path must be specified.", nameof(rootPaths));
            }

            _rootPaths = rootPaths;
        }

        /// <summary>
        /// When overridden in a derived class, builds a list of conditions that should be applied to the WMI query used by this instance.
        /// The condition for the registry hive is handled in the based class and should not be included in this list.
        /// </summary>
        /// <returns>A list of <see cref="RegistryMonitorCondition" /> objects that will be appended to the WMI query string.</returns>
        private protected override IEnumerable<RegistryMonitorCondition> BuildConditions()
        {
            yield return new RegistryMonitorCondition("RootPath", _rootPaths);
        }
    }
}
