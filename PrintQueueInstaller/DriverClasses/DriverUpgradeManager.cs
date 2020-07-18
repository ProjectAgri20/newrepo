using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using System.Security;
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// This class manages the process of upgrading a driver on the system.  It executes
    /// the upgrade and watches the registry for each queue being upgraded.
    /// </summary>
    public class DriverUpgradeManager : IDisposable
    {
        private QueueManager _queueManager = null;
        private SortableBindingList<DriverUpgradeData> _upgradeData;
        private string _currentDriverName = string.Empty;
        private PrintDeviceDriver _newDriver = null;
        private Thread _upgradeThread = null;
        private Thread _uiTimerThread = null;
        private DriverUpgradeData _currentData = null;
        private AutoResetEvent _timerEvent = new AutoResetEvent(false);

        private const string _queueRegistryPath = @"SYSTEM\CurrentControlSet\Control\Print\Printers\{0}";

        internal event EventHandler DataUpdated;
        internal event EventHandler UpgradeCompleted;
        internal event EventHandler<StatusEventArgs> StatusChange;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverUpgradeManager"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public DriverUpgradeManager(QueueManager manager)
        {
            _queueManager = manager;
            _upgradeData = new SortableBindingList<DriverUpgradeData>();
        }

        /// <summary>
        /// Gets or sets the current driver.
        /// </summary>
        /// <value>
        /// The current driver.
        /// </value>
        public string CurrentDriverName
        {
            get { return _currentDriverName; }
            set { _currentDriverName = value; }
        }

        /// <summary>
        /// Aborts the upgrade.
        /// </summary>
        public void AbortUpgrade()
        {
            try
            {
                lock (this)
                {
                    // Kill the two currently running threads.
                    if (_uiTimerThread != null)
                    {
                        _uiTimerThread.Abort();
                    }

                    if (_upgradeThread != null)
                    {
                        _upgradeThread.Abort();
                    }
                }

                if (_currentData != null)
                {
                    _currentData.Status = "Aborted";
                    TriggerDataUpdateEvent();
                }

                UpdateStatus("Upgrades aborted", StatusEventType.StatusChange);
            }
            catch (SecurityException ex)
            {
                TraceFactory.Logger.Error("Failed to abort upgrade thread", ex);
            }
            catch (ThreadAbortException ex)
            {
                TraceFactory.Logger.Error("Failed to abort upgrade thread", ex);
            }
            catch (ThreadStateException ex)
            {
                TraceFactory.Logger.Error("Failed to abort upgrade thread", ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Upgrades the specified driver name.
        /// </summary>
        /// <param name="upgradeData">The upgrade data.</param>
        /// <param name="newDriver">The new driver.</param>
        public void Upgrade(SortableBindingList<DriverUpgradeData> upgradeData, PrintDeviceDriver newDriver)
        {
            _queueManager.Reset();

            _upgradeData = upgradeData;
            _newDriver = newDriver;

            TriggerDataUpdateEvent();

            if (_currentDriverName.Equals(_newDriver.Name, StringComparison.OrdinalIgnoreCase))
            {
                foreach (DriverUpgradeData data in _upgradeData)
                {
                    data.Status = "Not Monitored";
                    data.StartTimeFormatted = string.Empty;
                    data.EndTimeFormatted = string.Empty;
                    data.Duration = string.Empty;
                }
                                
                lock (this)
                {
                    _upgradeThread = new Thread(UpgradeSameVersion);
                    _upgradeThread.Start();
                }
            }
            else
            {
                foreach (DriverUpgradeData data in _upgradeData)
                {
                    data.Status = string.Empty;
                    data.StartTimeFormatted = string.Empty;
                    data.EndTimeFormatted = string.Empty;
                    data.Duration = string.Empty;
                }

                lock (this)
                {
                    _upgradeThread = new Thread(UpgradeToNewVersion);
                    _upgradeThread.Start();
                }
            }
        }

        private void UpdateStatus(string message, StatusEventType eventType)
        {
            if (StatusChange != null)
            {
                StatusChange(this, new StatusEventArgs(message, eventType));

            }
        }

        private void UpgradeSameVersion(object state)
        {
            // Going to the same version will typically mean just upgrading the queues which
            // will be under the control of Microsoft.  So by just installing the new driver
            // things will automatically kick in for all the queues.
            try
            {
                DateTime start = DateTime.Now;
                UpdateStatus("Upgrading {0}...".FormatWith(_newDriver.Name), StatusEventType.StatusChange);
                _queueManager.InstallDriver(true);
                UpdateStatus("Upgrading {0}...complete".FormatWith(_newDriver.Name), StatusEventType.StatusChange);
                DateTime end = DateTime.Now;

                TimeSpan totalTime = end.Subtract(start);

                CalculateStatistics(_upgradeData.Count, start, end, totalTime);
            }
            catch (Win32Exception ex)
            {
                UpdateStatus("Install failed...{0}".FormatWith(ex.Message), StatusEventType.StatusChange);
                FireUpgradeCompleted();
                return;
            }
        }

        private void TimerStatus(object state)
        {
            DriverUpgradeData upgradeData = state as DriverUpgradeData;

            TimeSpan delta = new TimeSpan(0, 0, 1);
            TimeSpan timeSpan = new TimeSpan();
            while (!_timerEvent.WaitOne(1000))
            {
                timeSpan = timeSpan.Add(delta);
                upgradeData.Status = Resource.DurationFormat.FormatWith
                (
                    timeSpan.Hours,
                    timeSpan.Minutes,
                    timeSpan.Seconds,
                    timeSpan.Milliseconds
                );

                TriggerDataUpdateEvent();
            }
        }

        private void TriggerDataUpdateEvent()
        {
            if (DataUpdated != null)
            {
                DataUpdated(this, new EventArgs());
            }
        }

        private void FireUpgradeCompleted()
        {
            if (UpgradeCompleted != null)
            {
                UpgradeCompleted(this, new EventArgs());
            }
        }

        private void UpgradeToNewVersion(object state)
        {
            try
            {
                UpdateStatus("Installing {0}...".FormatWith(_newDriver.Name), StatusEventType.StatusChange);
                _queueManager.InstallDriver();
                UpdateStatus("Installing {0}...done".FormatWith(_newDriver.Name), StatusEventType.StatusChange);

                int queueCount = 0;
                TimeSpan totalTime = new TimeSpan();

                DateTime start = DateTime.Now;
                foreach (var upgradeData in _upgradeData)
                {
                    UpdateStatus("Upgrading {0}".FormatWith(upgradeData.QueueName), StatusEventType.StatusChange);

                    _currentData = upgradeData;

                    if (upgradeData.Include)
                    {
                        queueCount++;
                        lock (this)
                        {
                            _uiTimerThread = new Thread(TimerStatus);
                            _uiTimerThread.Start(upgradeData);
                        }

                        upgradeData.StartTime = DateTime.Now;
                        _queueManager.UpgradeDriver(upgradeData.QueueName);
                        upgradeData.EndTime = DateTime.Now;

                        // Release the timer prompt so it stops
                        _timerEvent.Set();

                        TimeSpan upgradeDuration = upgradeData.EndTime.Subtract(upgradeData.StartTime);
                        upgradeData.Duration = Resource.DurationFormat.FormatWith
                        (
                            upgradeDuration.Hours,
                            upgradeDuration.Minutes,
                            upgradeDuration.Seconds,
                            upgradeDuration.Milliseconds
                        );
                        upgradeData.Status = "Complete";

                        totalTime = totalTime.Add(upgradeData.EndTime.Subtract(upgradeData.StartTime));

                        TriggerDataUpdateEvent();
                    }
                    else
                    {
                        upgradeData.Status = "Skipping";
                    }
                }
                DateTime end = DateTime.Now;

                CalculateStatistics(queueCount, start, end, totalTime);
            }
            catch (Win32Exception ex)
            {
                UpdateStatus("Install failed...{0}".FormatWith(ex.Message), StatusEventType.StatusChange);
                FireUpgradeCompleted();
                return;
            }
        }

        private void CalculateStatistics(int queueCount, DateTime start, DateTime end, TimeSpan totalTime)
        {
            if (queueCount > 0)
            {
                UpdateStatus(end.ToString(Resource.DateTimeFormat, CultureInfo.CurrentCulture), StatusEventType.UpgradeEndTime);
                UpdateStatus(start.ToString(Resource.DateTimeFormat, CultureInfo.CurrentCulture), StatusEventType.UpgradeStartTime);

                long avgQueueCreationTime = totalTime.Ticks / (long)queueCount;
                TimeSpan avgQueueUpgradeTimeSpan = new TimeSpan(avgQueueCreationTime);
                string queueUpgradeDurationValue = Resource.DurationFormat.FormatWith
                    (
                        avgQueueUpgradeTimeSpan.Hours,
                        avgQueueUpgradeTimeSpan.Minutes,
                        avgQueueUpgradeTimeSpan.Seconds,
                        avgQueueUpgradeTimeSpan.Milliseconds
                    );
                UpdateStatus(queueUpgradeDurationValue, StatusEventType.AverageQueueUpgradeDuration);

                string durationValue = Resource.LongDurationFormat.FormatWith
                    (
                        totalTime.Days,
                        totalTime.Hours,
                        totalTime.Minutes,
                        totalTime.Seconds,
                        totalTime.Milliseconds
                    );
                UpdateStatus(durationValue, StatusEventType.TotalUpgradeTime);

                PrinterInstallStatus status = _queueManager.Installer.InstallStatus;
                string key = "Overall Driver Upgrade Process";
                status.Create(key);
                status[key].Record("OVERALL UPGRADE START", start);
                status[key].Record("OVERALL UPGRADE END", end);
                status[key].Record("AVERAGE QUEUE UPGRADE TIME", avgQueueUpgradeTimeSpan);
                status[key].Record("TOTAL QUEUE UPGRADE TIME", totalTime);
            }

            FireUpgradeCompleted();
        }

        /// <summary>
        /// Returns a list of installed queues by driver and architecture.
        /// </summary>
        /// <param name="driverName">Name of the driver.</param>
        /// <param name="architectureName">Name of the architecture.</param>
        /// <returns></returns>
        public static Collection<DriverUpgradeData> SelectQueues(string driverName, string architectureName)
        {
            if (string.IsNullOrEmpty(architectureName))
            {
                throw new ArgumentNullException("architectureName");
            }

            Collection<DriverUpgradeData> queues = new Collection<DriverUpgradeData>();

            ProcessorArchitecture selectedArch = (architectureName.Equals("x64", StringComparison.OrdinalIgnoreCase))
                ? ProcessorArchitecture.NTAMD64 : ProcessorArchitecture.NTx86;

            var processors = PrintProcessor.ProcessorsByArchitecture;

            string basePath = @"System\CurrentControlSet\Control\Print\Printers";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(basePath))
            {
                foreach (string queueName in key.GetSubKeyNames())
                {
                    using (RegistryKey queueKey = Registry.LocalMachine.OpenSubKey(Path.Combine(basePath, queueName)))
                    {
                        if (queueKey != null)
                        {
                            // Check that the driver name matches for this queue.  If it does, keep going.
                            object driverModel = queueKey.GetValue("Printer Driver");
                            if (driverModel != null && driverModel.ToString().Equals(driverName, StringComparison.OrdinalIgnoreCase))
                            {
                                // Find the processor name, see if it has the same architecture as the requested driver
                                // If it does, then add the associated queue to the list.
                                object processorValue = queueKey.GetValue("Print Processor");
                                if (processorValue != null)
                                {
                                    string processorName = (string)processorValue;

                                    // Determine if the given print processor is associated with the current
                                    // architecture for any queues referencing the given driver name.  If
                                    // this queue's driver maps to a print processor for this architecture
                                    // then add it to the list of queues using the given driver...
                                    var any =
                                        (
                                            from archKey in processors.Keys
                                            from value in processors[archKey]
                                            where processorName.Equals(value, StringComparison.OrdinalIgnoreCase) 
                                                && archKey == selectedArch.ToDriverArchitecture()
                                            select archKey
                                        ).Any();

                                    if (any)
                                    {
                                        DriverUpgradeData data = new DriverUpgradeData(queueName);
                                        data.PrintProcessor = processorName;
                                        queues.Add(data);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return queues;
        }

        /// <summary>
        /// Gets the log text defined for the upgrade process.
        /// </summary>
        public string LogText
        {
            get { return _queueManager.Installer.LogText; }
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
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_timerEvent != null)
                {
                    _timerEvent.Dispose();
                }
            }
        }

        #endregion
    }

    internal enum StatusEventType
    {
        UpgradeStartTime,
        UpgradeEndTime,
        StatusChange,
        AverageQueueUpgradeDuration,
        TotalUpgradeTime,
        Reset,
    }
}
