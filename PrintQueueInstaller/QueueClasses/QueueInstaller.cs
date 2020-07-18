using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.ServiceProcess;
using System.Threading;
using HP.ScalableTest.Framework.Automation.OfficeWorker;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Manages the process of installing print drivers and queues on a system
    /// </summary>
    public class QueueInstaller : IDisposable
    {
        private PrinterInstallStatus _installStatus = new PrinterInstallStatus();
        private Collection<string> _installedDrivers = new Collection<string>();
        private TimeSpan _installationTimeout = TimeSpan.FromMinutes(5);

        internal delegate void InstallStatusHandler(object sender, StatusEventArgs e);
        internal event InstallStatusHandler OnStatusUpdate;

        internal delegate void ErrorEventHandler(object sender, ErrorEventArgs e);
        internal event ErrorEventHandler OnError;

        internal event EventHandler OnComplete;

        private Collection<Thread> _installThreads = new Collection<Thread>();
        private Thread _mainInstallerThread = null;

        private Semaphore _threadSemaphore = null;
        private static object _serviceLock = new object();
        private static object _driverLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueInstaller"/> class.
        /// </summary>
        public QueueInstaller()
        {
            _threadSemaphore = new Semaphore(20, 20);
        }

        private void UpdateStatus(string message)
        {
            if (OnStatusUpdate != null)
            {
                StatusEventArgs args = new StatusEventArgs(message, StatusEventType.StatusChange);
                OnStatusUpdate(this, args);
            }
        }

        private void FireComplete()
        {
            if (OnComplete != null)
            {
                OnComplete(this, new EventArgs());
            }
        }

        internal PrinterInstallStatus InstallStatus
        {
            get { return _installStatus; }
        }

        /// <summary>
        /// Aborts the installation process
        /// </summary>
        public void Abort()
        {
            foreach (Thread thread in _installThreads)
            {
                if (thread != null)
                {
                    thread.Abort();
                }
            }

            if (_mainInstallerThread != null)
            {
                _mainInstallerThread.Abort();
            }

            _installStatus.Create("ABORT");
            _installStatus["ABORT"].Record("Abort received...");
        }

        /// <summary>
        /// Installs the queues.
        /// </summary>
        public void InstallQueues(int threadCount, Collection<QueueInstallationData> queues, TimeSpan installTimeout)
        {
            _installationTimeout = installTimeout;

            _installThreads = new Collection<Thread>();
            _mainInstallerThread = null;

            // Define the Semaphore count
            if (_threadSemaphore != null)
            {
                _threadSemaphore.Dispose();
            }

            _threadSemaphore = new Semaphore(threadCount, threadCount);

            _installStatus.Clear();

            // Start the worker thread that will create the queues
            ThreadPool.QueueUserWorkItem(CreateQueues, queues);
        }

        /// <summary>
        /// Upgrades the driver.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="queueName">Name of the queue.</param>
        public void UpgradeDriver(PrintDeviceDriver driver, string queueName)
        {
            if (driver == null)
            {
                throw new ArgumentNullException("driver");
            }

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            InstallStatusData status = _installStatus.Create(queueName);

            lock (_driverLock)
            {
                UpdateStatus("Upgrading print driver... " + driver.Name);
                status.Record("DRIVER UPGRADE START", out start);

                // Upgrade the driver, but do not check to see if the driver is installed,
                // this has already been done.
                DriverInstaller.Upgrade(driver.CreateDetail(), queueName);

                status.Record("DRIVER UPGRADE END", out end);
                status.Record("DRIVER UPGRADE TOTAL", end.Subtract(start));
            }
        }

        /// <summary>
        /// Installs the designated driver.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="forceInstall">if set to <c>true</c> then an install is forced.</param>
        public void InstallDriver(PrintDeviceDriver driver, bool forceInstall = false)
        {
            if (driver == null)
            {
                throw new ArgumentNullException("driver");
            }
            
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            InstallStatusData status = _installStatus.Create(driver.Name);

            lock (_driverLock)
            {
                UpdateStatus("Installing print driver... " + driver.Name);
                status.Record("DRIVER INSTALL START", out start);
                DriverInstaller.Install(driver.CreateDetail(), forceInstall);
                status.Record("DRIVER INSTALL END", out end);
                _installedDrivers.Add(driver.Name);
                status.Record("DRIVER INSTALL TOTAL", end.Subtract(start));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void CreateQueues(object state)
        {
            try
            {
                _mainInstallerThread = Thread.CurrentThread;

                Collection<QueueInstallationData> queues = (Collection<QueueInstallationData>)state;

                TraceFactory.Logger.Debug("PROCESS START");

                // Now install each print queue that is listed
                foreach (QueueInstallationData queueData in queues)
                {
                    if (queueData.QueueIsInstalled)
                    {
                        UpdateStatus("Queue already installed");
                    }
                    else
                    {
                        _threadSemaphore.WaitOne();
                        ThreadPool.QueueUserWorkItem(CreatePrintQueue, queueData);
                    }
                }

                // Wait for the last queue to finish
                _threadSemaphore.WaitOne();
                UpdateStatus("Queue Creation Completed");
                FireComplete();
            }
            catch (ThreadAbortException ex)
            {
                TraceFactory.Logger.Debug(ex.Message);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                if (OnError != null)
                {
                    ErrorEventArgs args = new ErrorEventArgs(ex);
                    OnError(this, args);
                }
            }
        }

        private void RemoveConfigFiles(InstallStatusData status)
        {
            foreach (string file in Directory.GetFiles(DriverController.SystemVersion3DriverDirectory, "*.cfm"))
            {
                try
                {
                    string message = "Deleting {0}".FormatWith(file);
                    TraceFactory.Logger.Debug(message);
                    UpdateStatus(message);
                    File.Delete(file);
                }
                catch (ArgumentException ex)
                {
                    LogDeleteError(status, file, ex);
                }
                catch (IOException ex)
                {
                    LogDeleteError(status, file, ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    LogDeleteError(status, file, ex);
                }
                catch (NotSupportedException ex)
                {
                    LogDeleteError(status, file, ex);
                }
            }
        }

        private static void UnableToCopyError(InstallStatusData status, Exception ex)
        {
            status.Record("Unable to copy CFM file");
            TraceFactory.Logger.Error("Unable to copy CFM file", ex);
        }

        /// <summary>
        /// When a .cfm file is applied to a driver installation, the settings propagate to the .cfg file.  So, even when the .cfm file
        /// is removed from the driver install directory, the settings persist.  To complicate things further, sometimes additional .cfg
        /// files are created which have a higher usage priority during queue creation.  For example, if the default config file is hpcpu140.cfg,
        /// in the driver installation directory there may also be hpcpu140_p6.cfg.  The _p6 file has priority over the original .cfg file
        /// during queue creation.  Therefore, when restoring the original default .cfg file, it is necessary to remove all files that start with
        /// "hpcpu140".
        /// The intent of this method is to copy the original .cfg file from the driver location chosen by the user into the driver installation
        /// directory.  We can't just do a File.Copy here because there may be other files related to the .cfg filename that need to be removed.
        /// So the restore is done in 2 phases - delete and copy.
        /// </summary>
        /// <param name="driver">The driver properties</param>
        /// <param name="status">The install status data</param>
        private static void RestoreDriverDefaults(PrintDeviceDriver driver, InstallStatusData status)
        {
            string defaultConfigFile = GetDefaultConfigFile(driver);

            //Remove all cfg files that match the default .cfg file name.
            foreach (string file in Directory.GetFiles(DriverController.SystemVersion3DriverDirectory, "{0}*.cfg".FormatWith(defaultConfigFile)))
            {
                try
                {
                    TraceFactory.Logger.Debug("Deleting {0}".FormatWith(file));
                    File.Delete(file);
                }
                catch (ArgumentException ex)
                {
                    LogDeleteError(status, file, ex);
                }
                catch (IOException ex)
                {
                    LogDeleteError(status, file, ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    LogDeleteError(status, file, ex);
                }
                catch (NotSupportedException ex)
                {
                    LogDeleteError(status, file, ex);
                }
            }

            // Copy the original .cfg file over to the driver installation location
            string source = @"{0}\{1}.cfg".FormatWith(driver.Location, defaultConfigFile);
            string destination = @"{0}\{1}.cfg".FormatWith(DriverController.SystemVersion3DriverDirectory, defaultConfigFile);

            if (!File.Exists(destination))
            {
                try
                {
                    TraceFactory.Logger.Debug("Source: {0}".FormatWith(source));
                    TraceFactory.Logger.Debug("Destination: {0}".FormatWith(destination));
                    File.Copy(source, destination, true);
                }
                catch (ArgumentException ex)
                {
                    LogFileCopyError("CFG", ex);
                    throw;
                }
                catch (IOException ex)
                {
                    LogFileCopyError("CFG", ex);
                    throw;
                }
                catch (UnauthorizedAccessException ex)
                {
                    LogFileCopyError("CFG", ex);
                    throw;
                }
                catch (NotSupportedException ex)
                {
                    LogFileCopyError("CFG", ex);
                    throw;
                }
            }

        }

        private static string GetDefaultConfigFile(PrintDeviceDriver driver)
        {
            string location = string.IsNullOrEmpty(driver.InfPath) ? string.Empty : Path.GetDirectoryName(driver.InfPath);

            string[] driverConfigFiles = Directory.GetFiles(location, "*.cfg");
            int count = driverConfigFiles.Count();

            TraceFactory.Logger.Debug("{0} .cfg file(s) found: {1}".FormatWith(count, location));
            if (count == 1)
            {
                return Path.GetFileNameWithoutExtension(driverConfigFiles.First());
            }

            //An invalid number of config files were found.
            throw new InvalidOperationException("{0} driver config files found in {1}".FormatWith(driverConfigFiles.Count(), location));
        }

        private void CreatePrintQueue(object state)
        {
            _installThreads.Add(Thread.CurrentThread);

            QueueInstallationData queueData = state as QueueInstallationData;
            InstallStatusData status = _installStatus.Create(queueData);

            try
            {
                if (queueData.QueueIsInstalled)
                {
                    // This queue is already installed, so return
                    return;
                }

                if (string.IsNullOrEmpty(queueData.Address))
                {
                    UpdateStatus("NO ADDRESS - SKIPPING");
                    status.Record("NO ADDRESS - SKIPPING");
                    return;
                }

                // Install the initial Print Driver for this queue if needed
                PrintDeviceDriver driver = queueData.Driver;

                InstallDriver(driver);

                DateTime queueInstallStart = DateTime.Now;
                status.Record("NEW ENTRY START", out queueInstallStart);

                TcpIPPortInstaller port = TcpIPPortInstaller.CreateRawPortManager
                    (
                        queueData.Address,
                        portNumber: queueData.Port,
                        portName: "IP_{0}:{1}".FormatWith(queueData.Address, queueData.Port),
                        snmpEnabled: queueData.SnmpEnabled
                    );

                queueData.Progress = "Working...";
                UpdateStatus("Installing... " + queueData.QueueName);

                TraceFactory.Logger.Debug("UseConfigurationFile: {0}".FormatWith(queueData.UseConfigurationFile));
                if (!queueData.UseConfigurationFile)
                {
                    // Make sure there are no CFM files sitting in the driver directory
                    RemoveConfigFiles(status);
                    RestoreDriverDefaults(driver, status);
                }

                InstallerPrintDevice printDevice = new InstallerPrintDevice(driver, port);
                printDevice.ConfigFile = (queueData.UseConfigurationFile) ? queueData.ConfigurationFilePath : string.Empty;
                printDevice.QueueName = queueData.QueueName;
                printDevice.IsSharedQueue = queueData.Shared;
                printDevice.IsRenderedOnClient = queueData.ClientRender;

                bool queueCreated = true;
                StatusRecord record = new StatusRecord(status);
                while(true)
                {
                    if (!SetupPort(printDevice, record, queueData))
                    {
                        queueCreated = false;
                        break;
                    }

                    if (!InstallQueue(printDevice, record, queueData))
                    {
                        queueCreated = false;
                        break;
                    }

                    if (!SetupClientRendering(printDevice, record, queueData))
                    {
                        queueCreated = false;
                        break;
                    }

                    if (!SetupSharing(printDevice, record, queueData))
                    {
                        queueCreated = false;
                        break;
                    }

                    break;

                }

                DateTime queueInstallEnd = DateTime.Now;
                status.Record("NEW ENTRY END", out queueInstallEnd);
                TimeSpan totalTime = queueInstallEnd.Subtract(queueInstallStart);
                status.Record("NEW ENTRY TOTAL", totalTime);

                if (queueCreated)
                {
                    queueData.Progress = "{0:D2}:{1:D2}.{2:D3}".FormatWith(totalTime.Minutes, totalTime.Seconds, totalTime.Milliseconds);
                    UpdateStatus("1");
                    UpdateStatus("Queue creation complete.");
                }
                else
                {
                    queueData.Progress = "ERROR";
                    UpdateStatus("1");
                    UpdateStatus("Queue creation failed.");
                }

                _threadSemaphore.Release();
            }
            catch (Win32Exception ex)
            {
                status.Record("FAILED: " + ex.Message);
                string message = new Win32Exception(ex.NativeErrorCode).Message;
                UpdateStatus("Queue creation failed: {0}".FormatWith(message));
                FireComplete();
                return;
            }
            finally
            {
                _installThreads.Remove(Thread.CurrentThread);
            }
        }

        #region Abort, Retry and Error Methods

        private static void LogDeleteError(InstallStatusData status, string fileName, Exception ex)
        {
            TraceFactory.Logger.Error("File delete failed", ex);
            status.Record("ERR: DELETE FAILED: {0}".FormatWith(Path.GetFileName(fileName)));
        }

        private static void LogFileCopyError(string fileType, Exception ex)
        {
            TraceFactory.Logger.Error("Failed to copy {0} File".FormatWith(fileType), ex);
        }

        private void RetryOnError(StatusRecord record, Exception ex)
        {
            UpdateStatus("Error retrying... " + ex.Message);
            record.Post("ERR - RETRY: {0}".FormatWith(ex.Message));
            RestartSpooler(record);
        }

        private void RecordError(StatusRecord record, QueueInstallationData queueData, Exception ex)
        {
            UpdateStatus("Error, aborting... " + ex.Message);
            record.Post("ERR - ABORT: {0}".FormatWith(ex.Message));
            queueData.Progress = "ERROR";
        }

        #endregion

        #region Setup Port

        private bool SetupPort(InstallerPrintDevice printDevice, StatusRecord record, QueueInstallationData queueData)
        {
            bool status = true;
            try
            {
                UpdateStatus("Creating port... " + printDevice.Port.PortName);
                record.Start("PORT");

                Retry.WhileThrowing
                (
                    () => SetupPortAction(printDevice, record),
                    3,
                    TimeSpan.FromSeconds(3),
                    new List<Type>() { typeof(Win32Exception) }
                );

                record.End();
            }
            catch (PrintQueueException ex)
            {
                RecordError(record, queueData, ex);
                status = false;
            }

            return status;
        }

        private void SetupPortAction(InstallerPrintDevice printDevice, StatusRecord record)
        {
            try
            {
                printDevice.Port.CreatePort();
            }
            catch (Win32Exception ex)
            {
                RetryOnError(record, ex);
                throw;
            }
        }

        #endregion

        #region Install Queue

        private bool InstallQueue(InstallerPrintDevice printDevice, StatusRecord record, QueueInstallationData queueData)
        {
            bool status = true;
            try
            {
                UpdateStatus("Creating queue... " + queueData.QueueName);
                record.Start("QUEUE");

                Retry.WhileThrowing
                (
                    () => InstallQueueAction(printDevice, record),
                    3,
                    TimeSpan.FromSeconds(2),
                    new List<Type>() { typeof(Win32Exception) }
                );

                record.End();
            }
            catch (PrintQueueException ex)
            {
                RecordError(record, queueData, ex);
                status = false;
            }
            catch (Win32Exception ex)
            {
                RecordError(record, queueData, ex);
                status = false;
            }
            catch (InvalidOperationException ex)
            {
                RecordError(record, queueData, ex);
                status = false;
            }

            return status;
        }

        private void InstallQueueAction(InstallerPrintDevice printDevice, StatusRecord record)
        {
            try
            {
                printDevice.CreatePrintQueue(_installationTimeout);
            }
            catch (Win32Exception ex)
            {
                RetryOnError(record, ex);
                throw;
            }
        }

        #endregion

        #region Setup Client Rendering

        private bool SetupClientRendering(InstallerPrintDevice printDevice, StatusRecord record, QueueInstallationData queueData)
        {
            bool status = true;
            try
            {
                UpdateStatus(@"Configuring ""Render on Client"" setting...");
                record.Start("RENDERING");

                Retry.WhileThrowing
                (
                    () => SetupClientRenderingAction(printDevice, record),
                    3,
                    TimeSpan.FromSeconds(3),
                    new List<Type>() 
                        { 
                            typeof(ArgumentNullException), 
                            typeof(SecurityException),
                            typeof(ObjectDisposedException),
                            typeof(UnauthorizedAccessException),
                            typeof(IOException)
                        }
                );

                record.End();
            }
            catch (PrintQueueException ex)
            {
                RecordError(record, queueData, ex);
                status = false;
            }

            return status;
        }

        private void SetupClientRenderingAction(InstallerPrintDevice printDevice, StatusRecord record)
        {
            try
            {
                printDevice.EnableClientRendering();
            }
            catch (Win32Exception ex)
            {
                RetryOnError(record, ex);
                throw;
            }
        }

        #endregion

        #region Setup Sharing

        private bool SetupSharing(InstallerPrintDevice printDevice, StatusRecord record, QueueInstallationData queueData)
        {
            bool status = true;
            try
            {
                UpdateStatus("Configuring sharing...");
                record.Start("SHARING");

                Retry.WhileThrowing
                (
                    () => SetupSharingAction(printDevice, record),
                    3,
                    TimeSpan.FromSeconds(3),
                    new List<Type>() { typeof(Win32Exception) }
                );

                record.End();
            }
            catch (PrintQueueException ex)
            {
                RecordError(record, queueData, ex);
                status = false;
            }

            return status;
        }

        private void SetupSharingAction(InstallerPrintDevice printDevice, StatusRecord record)
        {
            try
            {
                printDevice.EnableSharedQueue();
            }
            catch (Win32Exception ex)
            {
                RetryOnError(record, ex);
                throw;
            }
        }

        #endregion

        private void RestartSpooler(StatusRecord record)
        {
            lock (_serviceLock)
            {
                using (ServiceController serviceController = new ServiceController("spooler"))
                {
                    record.Post("SPOOLER STATUS: {0}".FormatWith(serviceController.Status));

                    if (serviceController.Status != ServiceControllerStatus.Running)
                    {
                        bool restarted = false;

                        UpdateStatus("Spooler stopped, trying to restart...".FormatWith(serviceController.Status.ToString()));
                        record.Post("SPOOLER STATE IS {0}, RESTARTING".FormatWith(serviceController.Status));
                        try
                        {
                            // Attempt to restart
                            serviceController.Start();
                            UpdateStatus("Spooler restarted");
                            record.Post("SPOOLER RESTARTED");
                            restarted = true;
                        }
                        catch (InvalidOperationException exception)
                        {
                            TraceFactory.Logger.Error("Failed to start service", exception);
                            record.Post("SPOOLER NOT STARTED");
                        }
                        catch (Win32Exception exception)
                        {
                            TraceFactory.Logger.Error("Failed to start service", exception);
                            record.Post("SPOOLER NOT STARTED");
                        }

                        if (!restarted)
                        {
                            throw new PrintQueueException("Unable to restart the spooler");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resets the log.
        /// </summary>
        public void ResetLog()
        {
            _installStatus.Reset();
        }

        /// <summary>
        /// Gets the log text.
        /// </summary>
        public string LogText
        {
            get { return _installStatus.ToString(); }
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
                if (_threadSemaphore != null)
                {
                    _threadSemaphore.Dispose();
                }
            }
        }

        #endregion
    }
}
