using System.Collections.Generic;
using System.Security;
using Microsoft.Win32;
using Win32Registry = Microsoft.Win32.Registry;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Provides methods for analyzing the size and contents of the system registry.
    /// </summary>
    public sealed class RegistryAnalyzer
    {
        private static readonly Dictionary<RegistryHive, RegistryKey> _registryHives = new Dictionary<RegistryHive, RegistryKey>
        {
            [RegistryHive.ClassesRoot] = Win32Registry.ClassesRoot,
            [RegistryHive.CurrentUser] = Win32Registry.CurrentUser,
            [RegistryHive.LocalMachine] = Win32Registry.LocalMachine,
            [RegistryHive.Users] = Win32Registry.Users,
            [RegistryHive.CurrentConfig] = Win32Registry.CurrentConfig
        };

        /// <summary>
        /// Gets the <see cref="Registry.RegistryHive" /> this <see cref="RegistryAnalyzer" /> will analyze.
        /// </summary>
        public RegistryHive RegistryHive { get; }

        /// <summary>
        /// Gets the registry path this <see cref="RegistryAnalyzer" /> will analyze.
        /// </summary>
        public string RegistryPath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryAnalyzer" /> class.
        /// </summary>
        /// <param name="hive">The <see cref="Registry.RegistryHive" /> this instance will analyze.</param>
        /// <param name="registryPath">The registry path this instance will analyze.</param>
        public RegistryAnalyzer(RegistryHive hive, string registryPath)
        {
            RegistryHive = hive;
            RegistryPath = registryPath;
        }

        /// <summary>
        /// Gets the approximate size of the specified registry path in the specified hive.
        /// </summary>
        /// <returns>The approximate size of the specified registry path, in bytes.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is expensive.")]
        public int GetRegistrySize()
        {
            return DuregUtility.GetRegistrySize(RegistryHive, RegistryPath);
        }

        /// <summary>
        /// Creates a <see cref="RegistrySnapshot" /> object representing the current state of the specified registry path.
        /// </summary>
        /// <returns>A <see cref="RegistrySnapshot" /> object.</returns>
        public RegistrySnapshot TakeSnapshot()
        {
            RegistrySnapshot snapshot = new RegistrySnapshot(RegistryHive, RegistryPath);
            using (RegistryKey baseKey = _registryHives[RegistryHive])
            {
                CrawlRegistry(snapshot, baseKey, RegistryPath);
            }
            return snapshot;
        }

        private void CrawlRegistry(RegistrySnapshot snapshot, RegistryKey baseKey, string subTree)
        {
            List<string> subKeyNames = new List<string>();

            try
            {
                using (RegistryKey key = baseKey.OpenSubKey(subTree))
                {
                    if (key != null)
                    {
                        snapshot.Keys.Add(new RegistrySnapshotKey(key));
                        subKeyNames.AddRange(key.GetSubKeyNames());
                    }
                }
            }
            catch (SecurityException)
            {
                // Ignore and keep going
            }

            foreach (string subKey in subKeyNames)
            {
                string subTreePath = !string.IsNullOrEmpty(subTree) ? subTree + @"\" + subKey : subKey;
                CrawlRegistry(snapshot, baseKey, subTreePath);
            }
        }
    }
}
