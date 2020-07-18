using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using RegistryHive = HP.ScalableTest.WindowsAutomation.Registry.RegistryHive;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// This class represents a set of registry key data keyed off the registry path
    /// </summary>
    public class RegistrySnapshot : Dictionary<string, RegistrySnapshotKeys>
    {
        private RegistryHive _hive = RegistryHive.LocalMachine;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrySnapshot"/> class.
        /// </summary>
        /// <param name="hive">The hive.</param>
        public RegistrySnapshot(RegistryHive hive)
            : base()
        {
            _hive = hive;
        }

        /// <summary>
        /// Gets the hive.
        /// </summary>
        public RegistryHive Hive
        {
            get { return _hive; }
        }

        /// <summary>
        /// Compares one snapshot to another and returns the differences
        /// </summary>
        /// <param name="newSnapshot">The snapshot to compare.</param>
        /// <returns>a <see cref="RegistrySnapshot"/> that contains both the added and removed items.</returns>
        public RegistrySnapshot CompareTo(RegistrySnapshot newSnapshot)
        {
            RegistrySnapshot snapshot = new RegistrySnapshot(this.Hive);

            // compare "this" to what is provided, and if what is provided
            // does not have an entry, the consider it deleted.
            foreach (string key in this.Keys)
            {
                if (!newSnapshot.ContainsKey(key))
                {
                    snapshot.Add(key, this[key]);
                    snapshot[key].State = RegistrySnapshotState.Removed;
                }
            }

            // Now compare what was provided to "this" and if entries
            // are missing, then consider then as newly added.
            foreach (string key in newSnapshot.Keys)
            {
                if (!this.ContainsKey(key))
                {
                    snapshot.Add(key, newSnapshot[key]);
                    snapshot[key].State = RegistrySnapshotState.Added;
                }
            }

            return snapshot;
        }

        public string ToStringCsv(bool includeHeader = true)
        {
            StringBuilder builder = new StringBuilder();

            if (includeHeader)
            {
                builder.AppendLine("Registry Path,State,Name,Type,Data");
            }

            foreach (string key in Keys)
            {
                foreach (RegistrySnapshotKey entry in this[key].Entries)
                {
                    builder.Append(key);
                    builder.Append(",");
                    builder.Append(this[key].State);
                    builder.Append(",");
                    builder.Append(entry.ToStringCsv());
                    builder.Append(Environment.NewLine);
                }
            }

            return builder.ToString();
        }
    }
}
