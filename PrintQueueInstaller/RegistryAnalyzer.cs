using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using Microsoft.Win32;
using RegistryHive = HP.ScalableTest.WindowsAutomation.Registry.RegistryHive;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// The class provides methods to determine any changes in the registry as well
    /// as determine the size of the registry.  It will be used to see growth in the
    /// registry when new printers are installed.
    /// </summary>
    public class RegistryAnalyzer
    {
        private RegistryHive _targetHive = RegistryHive.LocalMachine;
        private string _registryPath = string.Empty;

        private Dictionary<RegistryHive, RegistryKey> _registryHives = new Dictionary<RegistryHive, RegistryKey>()
            {
                { RegistryHive.LocalMachine, Registry.LocalMachine },
                { RegistryHive.ClassesRoot, Registry.ClassesRoot },
                { RegistryHive.CurrentUser, Registry.CurrentUser },
                { RegistryHive.Users, Registry.Users }
            };

        public RegistryHive TargetHive
        {
            get { return _targetHive; }
        }

        public string RegistryPath
        {
            get { return _registryPath; }
        }

        public string Key
        {
            get { return "{0}_{1}".FormatWith(_targetHive, _registryPath); }
        }

        public static int RegistryPrinterCount
        {
            get
            {
                int queueCount = 0;
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Print\Printers");
                if (key != null)
                {
                    queueCount = key.GetSubKeyNames().Length;
                }

                return queueCount;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to log errors when crawling the registry.
        /// </summary>
        /// <value>
        ///   <c>true</c> if error are to be logged; otherwise, <c>false</c>.
        /// </value>
        public bool LogErrors { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryAnalyzer"/> class.
        /// </summary>
        /// <param name="hive">The hive.</param>
        /// <param name="subTree">The sub tree.</param>
        public RegistryAnalyzer(RegistryHive hive, string subTree)
        {
            if (string.IsNullOrEmpty(subTree))
            {
                throw new ArgumentNullException("subTree");
            }

            _targetHive = hive;
            _registryPath = subTree;
            LogErrors = false;
        }

        /// <summary>
        /// Takes a snapshot of the registry.
        /// </summary>
        /// <returns></returns>
        public RegistrySnapshot TakeSnapshot()
        {
            RegistrySnapshot snapshot = new RegistrySnapshot(_targetHive);
            using (RegistryKey baseKey = GetRegistryKey(_targetHive))
            {
                CrawlRegistry(snapshot, baseKey, _registryPath);
            }

            return snapshot;
        }

        /// <summary>
        /// Gets the size of the registry in bytes
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>The size of the registry in bytes</returns>
        public int GetRegistrySize(string userName, string password)
        {
            var analyzer = new WindowsAutomation.Registry.RegistryAnalyzer(TargetHive, _registryPath);
            return analyzer.GetRegistrySize();
        }

        private RegistryKey GetRegistryKey(RegistryHive hive)
        {
            // Return the actual RegistryKey object for the given hive
            if (!_registryHives.ContainsKey(hive))
            {
                throw new ArgumentException("Invalid Hive: {0}".FormatWith(hive));
            }

            return _registryHives[hive];
        }

        private void CrawlRegistry(RegistrySnapshot snapshot, RegistryKey baseKey, string subTree)
        {
            Collection<string> subKeyNames = null;
            RegistrySnapshotKeys snapshotData = null;

            if (!snapshot.TryGetValue(subTree, out snapshotData))
            {
                snapshotData = new RegistrySnapshotKeys(RegistrySnapshotState.Existed);
                snapshot.Add(subTree, snapshotData);
            }

            try
            {
                using (RegistryKey key = baseKey.OpenSubKey(subTree))
                {
                    if (key != null)
                    {
                        foreach (string valueName in key.GetValueNames())
                        {
                            RegistrySnapshotKey data = new RegistrySnapshotKey();
                            data.Name = valueName;
                            data.Kind = key.GetValueKind(valueName);
                            data.Value = key.GetValue(valueName);

                            snapshotData.Entries.Add(data);
                        }

                        subKeyNames = new Collection<string>(key.GetSubKeyNames());
                    }
                }
            }
            catch (SecurityException ex)
            {
                // If logging is enabled, capture the error, otherwise just continue
                if (LogErrors)
                {
                    Console.WriteLine("{0}:{1}", ex.Message, subTree);
                }
            }

            if (subKeyNames != null)
            {
                // For any subkeys at this level, call recursively to process them
                foreach (string subKey in subKeyNames)
                {
                    string subTreePath = !string.IsNullOrEmpty(subTree) ? subTree + @"\" + subKey : subKey;
                    CrawlRegistry(snapshot, baseKey, subTreePath);
                }
            }
        }
    }

    /// <summary>
    /// A list of possible states for any registry snapshot data
    /// </summary>
    public enum RegistrySnapshotState
    {
        /// <summary>
        /// No state is defined for this entry
        /// </summary>
        None,

        /// <summary>
        /// This indicates the entry already existed when the initial snapshot was taken
        /// </summary>
        Existed,

        /// <summary>
        /// This indicates the registry entry was added between snapshots
        /// </summary>
        Added,

        /// <summary>
        /// This indicates the entry was removed between snapshots
        /// </summary>
        Removed
    }
}
