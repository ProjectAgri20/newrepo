using System.Collections.ObjectModel;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Represents a snapshot of a registry subtree.
    /// </summary>
    public sealed class RegistrySnapshot
    {
        /// <summary>
        /// Gets the <see cref="Registry.RegistryHive" /> for this snapshot.
        /// </summary>
        public RegistryHive RegistryHive { get; }

        /// <summary>
        /// Gets the registry path for this snapshot.
        /// </summary>
        public string RegistryPath { get; }

        /// <summary>
        /// Gets a collection of <see cref="RegistrySnapshotKey" /> objects that make up the snapshot.
        /// </summary>
        public Collection<RegistrySnapshotKey> Keys { get; } = new Collection<RegistrySnapshotKey>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrySnapshot" /> class.
        /// </summary>
        /// <param name="hive">The <see cref="RegistryHive" />.</param>
        /// <param name="registryPath">The registry path.</param>
        public RegistrySnapshot(RegistryHive hive, string registryPath)
        {
            RegistryHive = hive;
            RegistryPath = registryPath;
        }
    }
}
