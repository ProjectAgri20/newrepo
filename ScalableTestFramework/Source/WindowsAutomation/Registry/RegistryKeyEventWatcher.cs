using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Watches for changes to one or more keys in the system registry.
    /// </summary>
    public sealed class RegistryKeyEventWatcher : RegistryEventWatcher
    {
        private readonly IEnumerable<string> _keyPaths;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryKeyEventWatcher" /> class.
        /// </summary>
        /// <param name="hive">The <see cref="RegistryHive" /> that will be monitored.</param>
        /// <param name="keyPath">The key path to monitor.  (Backslashes must be escaped.)</param>
        /// <exception cref="ArgumentException">The specified <see cref="RegistryHive" /> does not support event watching.</exception>
        public RegistryKeyEventWatcher(RegistryHive hive, string keyPath)
            : this(hive, new[] { keyPath })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryKeyEventWatcher" /> class.
        /// </summary>
        /// <param name="hive">The <see cref="RegistryHive" /> that will be monitored.</param>
        /// <param name="keyPaths">The key paths to monitor.  (Backslashes must be escaped.)</param>
        /// <exception cref="ArgumentNullException"><paramref name="keyPaths" /> is null.</exception>
        /// <exception cref="ArgumentException">
        /// The specified <see cref="RegistryHive" /> does not support event watching.
        /// <para>or</para>
        /// <paramref name="keyPaths" /> is empty.
        /// </exception>
        public RegistryKeyEventWatcher(RegistryHive hive, IEnumerable<string> keyPaths)
            : base(hive, "RegistryKeyChangeEvent")
        {
            if (keyPaths == null)
            {
                throw new ArgumentNullException(nameof(keyPaths));
            }

            if (!keyPaths.Any())
            {
                throw new ArgumentException("At least one key path must be specified.", nameof(keyPaths));
            }

            _keyPaths = keyPaths;
        }

        /// <summary>
        /// When overridden in a derived class, builds a list of conditions that should be applied to the WMI query used by this instance.
        /// The condition for the registry hive is handled in the based class and should not be included in this list.
        /// </summary>
        /// <returns>A list of <see cref="RegistryMonitorCondition" /> objects that will be appended to the WMI query string.</returns>
        private protected override IEnumerable<RegistryMonitorCondition> BuildConditions()
        {
            yield return new RegistryMonitorCondition("KeyPath", _keyPaths);
        }
    }
}
