using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Watches for changes to one or more values in the system registry.
    /// </summary>
    public sealed class RegistryValueEventWatcher : RegistryEventWatcher
    {
        private readonly string _keyPath;
        private readonly IEnumerable<string> _valueNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryValueEventWatcher" /> class.
        /// </summary>
        /// <param name="hive">The <see cref="RegistryHive" /> that will be monitored.</param>
        /// <param name="keyPath">The key path to monitor.  (Backslashes must be escaped.)</param>
        /// <param name="valueName">The value to monitor.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keyPath" /> is null.</exception>
        /// <exception cref="ArgumentException">
        /// The specified <see cref="RegistryHive" /> does not support event watching.
        /// <para>or</para>
        /// <paramref name="keyPath" /> is an empty string.
        /// </exception>
        public RegistryValueEventWatcher(RegistryHive hive, string keyPath, string valueName)
            : this(hive, keyPath, new[] { valueName })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryValueEventWatcher" /> class.
        /// </summary>
        /// <param name="hive">The <see cref="RegistryHive" /> that will be monitored.</param>
        /// <param name="keyPath">The key path to monitor.  (Backslashes must be escaped.)</param>
        /// <param name="valueNames">The values to monitor.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keyPath" /> is null.
        /// <para>or</para>
        /// <paramref name="valueNames" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The specified <see cref="RegistryHive" /> does not support event watching.
        /// <para>or</para>
        /// <paramref name="keyPath" /> is an empty string.
        /// <para>or</para>
        /// <paramref name="valueNames" /> is empty.
        /// </exception>
        public RegistryValueEventWatcher(RegistryHive hive, string keyPath, IEnumerable<string> valueNames)
            : base(hive, "RegistryValueChangeEvent")
        {
            if (keyPath == null)
            {
                throw new ArgumentNullException(nameof(keyPath));
            }

            if (valueNames == null)
            {
                throw new ArgumentNullException(nameof(valueNames));
            }

            if (string.IsNullOrWhiteSpace(keyPath))
            {
                throw new ArgumentException("Key path cannot be empty.", nameof(keyPath));
            }

            if (!valueNames.Any())
            {
                throw new ArgumentException("At least one value name must be specified.", nameof(valueNames));
            }

            _keyPath = keyPath;
            _valueNames = valueNames;
        }


        /// <summary>
        /// When overridden in a derived class, builds a list of conditions that should be applied to the WMI query used by this instance.
        /// The condition for the registry hive is handled in the based class and should not be included in this list.
        /// </summary>
        /// <returns>A list of <see cref="RegistryMonitorCondition" /> objects that will be appended to the WMI query string.</returns>
        private protected override IEnumerable<RegistryMonitorCondition> BuildConditions()
        {
            yield return new RegistryMonitorCondition("KeyPath", _keyPath);
            yield return new RegistryMonitorCondition("ValueName", _valueNames);
        }
    }
}
