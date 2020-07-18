using System;
using System.Collections.Generic;
using System.Management;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Watches for changes to the system registry.
    /// </summary>
    public abstract class RegistryEventWatcher : IDisposable
    {
        private readonly string _hiveName;
        private readonly string _eventClassName;
        private readonly ManagementEventWatcher _watcher = new ManagementEventWatcher();

        private static readonly Dictionary<RegistryHive, string> _hiveNames = new Dictionary<RegistryHive, string>
        {
            [RegistryHive.LocalMachine] = "HKEY_LOCAL_MACHINE",
            [RegistryHive.Users] = "HKEY_USERS",
            [RegistryHive.CurrentConfig] = "HKEY_CURRENT_CONFIG"
        };

        /// <summary>
        /// Occurs when this <see cref="RegistryEventWatcher" /> is notified of a change in the registry.
        /// </summary>
        public event EventHandler<RegistryChangeEventArgs> RegistryChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryEventWatcher" /> class.
        /// </summary>
        /// <param name="hive">The <see cref="RegistryHive" /> that will be monitored.</param>
        /// <param name="eventClassName">The WMI event class name that will be monitored.</param>
        /// <exception cref="ArgumentException">The specified <see cref="RegistryHive" /> does not support event watching.</exception>
        protected RegistryEventWatcher(RegistryHive hive, string eventClassName)
        {
            if (!_hiveNames.ContainsKey(hive))
            {
                throw new ArgumentException($"Registry hive {hive} does not support event watching.", nameof(hive));
            }

            _hiveName = _hiveNames[hive];
            _eventClassName = eventClassName;

            _watcher.Query.QueryLanguage = "WQL";
            _watcher.Scope.Path.NamespacePath = @"root\default";
            _watcher.EventArrived += (s, e) => RegistryChanged?.Invoke(this, new RegistryChangeEventArgs(e.NewEvent));
        }

        /// <summary>
        /// When overridden in a derived class, builds a list of conditions that should be applied to the WMI query used by this instance.
        /// The condition for the registry hive is handled in the based class and should not be included in this list.
        /// </summary>
        /// <returns>A list of <see cref="RegistryMonitorCondition" /> objects that will be appended to the WMI query string.</returns>
        private protected abstract IEnumerable<RegistryMonitorCondition> BuildConditions();

        /// <summary>
        /// Starts monitoring the registry for changes.
        /// </summary>
        public void Start()
        {
            string query = $"SELECT * FROM {_eventClassName} WHERE Hive = '{_hiveName}'";
            foreach (RegistryMonitorCondition condition in BuildConditions())
            {
                query += $" AND ({condition.ToString()})";
            }
            _watcher.Query.QueryString = query;

            _watcher.Start();
        }

        /// <summary>
        /// Stops monitoring the registry for changes.
        /// </summary>
        public void Stop()
        {
            _watcher.Stop();
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _watcher.Stop();
                _watcher.Dispose();
            }
        }

        #endregion
    }
}
