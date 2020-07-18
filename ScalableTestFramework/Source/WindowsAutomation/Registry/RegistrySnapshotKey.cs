using System;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Represents a snapshot of a registry key.
    /// </summary>
    public sealed class RegistrySnapshotKey
    {
        /// <summary>
        /// Gets the name of the registry key.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the values stored in the registry key.
        /// </summary>
        public Collection<RegistrySnapshotValue> Values { get; } = new Collection<RegistrySnapshotValue>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrySnapshotKey" /> class.
        /// </summary>
        /// <param name="key">The <see cref="RegistryKey" /> to create a snapshot for.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        public RegistrySnapshotKey(RegistryKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            foreach (string valueName in key.GetValueNames())
            {
                Values.Add(new RegistrySnapshotValue(key, valueName));
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() => Name;
    }
}
