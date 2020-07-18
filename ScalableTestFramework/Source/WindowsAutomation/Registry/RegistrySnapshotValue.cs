using System;
using System.Linq;
using Microsoft.Win32;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Represents a snapshot of a registry key value.
    /// </summary>
    public sealed class RegistrySnapshotValue
    {
        /// <summary>
        /// Gets the name of the value.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the kind of value.
        /// </summary>
        public RegistryValueKind Kind { get; }

        /// <summary>
        /// Gets the data stored in this value.
        /// </summary>
        public object Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrySnapshotValue" /> class.
        /// </summary>
        /// <param name="key">The <see cref="RegistryKey" /> that contains this value.</param>
        /// <param name="name">The name of the value to retrieve from the <see cref="RegistryKey" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        public RegistrySnapshotValue(RegistryKey key, string name)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Name = name;
            Kind = key.GetValueKind(name);
            Data = key.GetValue(name);
        }

        /// <summary>
        /// Gets a string representation of the data stored in this value.
        /// </summary>
        /// <returns>A string representation of the data stored in this value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method creates a new object each time it is called.")]
        public string GetValue()
        {
            switch (Kind)
            {
                case RegistryValueKind.Binary:
                    byte[] binaryData = (byte[])Data;
                    return $"{binaryData.Length} bytes";

                case RegistryValueKind.DWord:
                case RegistryValueKind.QWord:
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    return Data.ToString();

                case RegistryValueKind.MultiString:
                    string[] items = (string[])Data;
                    return string.Join(", ", items.Select(n => $"[{0}]"));

                case RegistryValueKind.None:
                case RegistryValueKind.Unknown:
                default:
                    return Kind.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() => Name;
    }
}
