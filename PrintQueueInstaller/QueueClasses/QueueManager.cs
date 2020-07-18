using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Print.Drivers;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class QueueManager : IDisposable
    {
        private Collection<QueueInstallationData> _physicalQueues = new Collection<QueueInstallationData>();
        private Collection<QueueInstallationData> _virtualQueues = new Collection<QueueInstallationData>();
        private DriverPackagePathSet _driverSetPaths = new DriverPackagePathSet();
        private SerializableDictionary<string, PrintDeviceDriver> _printDrivers = new SerializableDictionary<string, PrintDeviceDriver>();
        private string _currentDriverKey = string.Empty;
        private QueueInstallationDataFactory _factory = new QueueInstallationDataFactory();
        private QueueInstaller _installer = new QueueInstaller();
        private PrintDeviceDriverCollection _propertiesSet = new PrintDeviceDriverCollection();
        private bool _generateNewPropertiesSet = false;


        /// <summary>
        /// Gets or sets the additional description.
        /// </summary>
        /// <value>
        /// The additional description.
        /// </value>
        public string AdditionalDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the CFM configuration file.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the CFM file should be used; otherwise, <c>false</c>.
        /// </value>
        public bool UseConfigurationFile { get; set; }

        /// <summary>
        /// Gets or sets the configuration file.
        /// </summary>
        /// <value>
        /// The configuration file.
        /// </value>
        public string ConfigurationFile { get; set; }

        /// <summary>
        /// Gets or sets the current driver.
        /// </summary>
        /// <value>
        /// The current driver.
        /// </value>
        public PrintDeviceDriver CurrentDriver { get; set; }

        /// <summary>
        /// Gets the installer.
        /// </summary>
        public QueueInstaller Installer
        {
            get { return _installer; }
        }

        /// <summary>
        /// Gets the print driver packages.
        /// </summary>
        public PrintDeviceDriverCollection PrintDrivers
        {
            get 
            {
                // This gets set to true whenever new items are added.  Once it is generated
                // which also sorts the list, it goes to false to avoid regenerating every time.
                if (_generateNewPropertiesSet)
                {
                    _propertiesSet.Clear();
                    _propertiesSet = new PrintDeviceDriverCollection(_printDrivers.Values.ToList<PrintDeviceDriver>());
                    _generateNewPropertiesSet = false;
                }

                return _propertiesSet;
            }
        }

        /// <summary>
        /// Adds the specified print driver package to the list of available packages
        /// </summary>
        /// <param name="printDriver">The print driver.</param>
        public void AddDriver(PrintDeviceDriver printDriver)
        {
            if (printDriver == null)
            {
                throw new ArgumentNullException("printDriver");
            }

            _currentDriverKey = "{0} [{1}]".FormatWith(printDriver.Name, printDriver.InfPath);

            if (!_printDrivers.ContainsKey(_currentDriverKey))
            {
                _printDrivers.Add(_currentDriverKey, printDriver);
                _generateNewPropertiesSet = true;
            }

            CurrentDriver = printDriver;
        }

        /// <summary>
        /// Loads the driver package and saves it in the cache.
        /// </summary>
        /// <param name="driversDirectory">Location of the driver distribution.</param>
        /// <param name="includeAllArchitectures">if set to <c>true</c> all architectures are included.</param>
        public void LoadDrivers(string driversDirectory, bool includeAllArchitectures)
        {
            // Clear out the print drivers when loading a new set of drivers.
            _printDrivers.Clear();
            _propertiesSet.Clear();

            var drivers = DriverController.LoadFromDirectory(driversDirectory, includeAllArchitectures);

            foreach (DriverDetails driver in drivers)
            {
                AddDriver(new PrintDeviceDriver(driver));
            }
        }

        /// <summary>
        /// Runs the driver configuration utility.
        /// </summary>
        public static void RunDriverConfigurationUtility()
        {
            TraceFactory.Logger.Debug("Entering...");
            string exeDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HPBCFGAP");
            string exePath = Path.Combine(exeDir, "hpbcfgap.exe");

            // If the directory and main executable is there, then assume that everything is there
            if (!Directory.Exists(exeDir) || !File.Exists(exePath))
            {

                // Expand out the binaries and return the path to the EXE
                Directory.CreateDirectory(exeDir);

                string path = Path.Combine(exeDir, "cfgapp.ico");
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    TraceFactory.Logger.Debug("Copying {0}".FormatWith(path));
                    Resource.cfgapp.Save(stream);
                }
                path = Path.Combine(exeDir, "hpbcfgre.dll");
                File.WriteAllBytes(path, Resource.hpbcfgre);
                File.WriteAllBytes(Path.Combine(exeDir, "hpbcfgui.dll"), Resource.hpbcfgui);
                File.WriteAllBytes(Path.Combine(exeDir, "HPCDMC32.dll"), Resource.HPCDMC32);
                File.WriteAllBytes(exePath, Resource.hpbcfgap);
            }

            Process.Start(exePath);
        }

        /// <summary>
        /// Refreshes the current package.
        /// </summary>
        public void RefreshCurrentPackage()
        {
            if (!_printDrivers.ContainsKey(_currentDriverKey))
            {
                _currentDriverKey = string.Empty;
            }
        }

        /// <summary>
        /// Finds the current package path.
        /// </summary>
        /// <returns></returns>
        public string FindCurrentPackagePath()
        {
            string path = string.Empty;
            if (!string.IsNullOrEmpty(_currentDriverKey) && _printDrivers.ContainsKey(_currentDriverKey))
            {
                path = _printDrivers[_currentDriverKey].InfPath;
            }

            return path;
        }

        /// <summary>
        /// Gets a value indicating whether the package is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the package is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool PrintDriverSelected
        {
            get { return !string.IsNullOrEmpty(_currentDriverKey); }
        }

        /// <summary>
        /// Constructs the queue definitions.
        /// </summary>
        /// <param name="printIds">The print ids.</param>
        /// <param name="queueCount">The queue count.</param>
        public void ConstructQueueDefinitions(Collection<string> printIds, int queueCount, bool fullName)
        {
            _physicalQueues.Clear();
            Collection<QueueInstallationData> items = _factory.Create
                (
                    printIds,
                    _printDrivers[_currentDriverKey],
                    CurrentDriver,
                    AdditionalDescription,
                    queueCount,
                    fullName
                );

            foreach (QueueInstallationData item in items)
            {
                item.UseConfigurationFile = UseConfigurationFile;
                if (UseConfigurationFile)
                {
                    item.ConfigurationFilePath = ConfigurationFile;
                }
                else
                {
                    item.ConfigurationFilePath = string.Empty;
                }

                _physicalQueues.Add(item);
            }
        }

        /// <summary>
        /// Installs the queues.
        /// </summary>
        /// <param name="threadCount">The thread count.</param>
        /// <param name="queueDefinitions">The queue definitions.</param>
        /// <param name="timeout">The timeout.</param>
        public void InstallQueues(int threadCount, Collection<QueueInstallationData> queueDefinitions, TimeSpan timeout)
        {
            _installer.ResetLog();
            _installer.InstallQueues(threadCount, queueDefinitions, timeout);
        }

        /// <summary>
        /// Installs the currently selected driver.
        /// </summary>
        /// <returns></returns>
        public bool InstallDriver(bool forceInstall = false)
        {
            bool status = false;

            if (_printDrivers.ContainsKey(_currentDriverKey))
            {
                _installer.InstallDriver(CurrentDriver, forceInstall);
                status = true;
            }

            return status;
        }

        /// <summary>
        /// Upgrades the driver.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public void UpgradeDriver(string queueName)
        {
            if (_printDrivers.ContainsKey(_currentDriverKey))
            {
                _installer.UpgradeDriver(CurrentDriver, queueName);
            }
        }

        /// <summary>
        /// Aborts the installation.
        /// </summary>
        public void AbortInstallation()
        {
            _installer.Abort();
        }

        /// <summary>
        /// Clears the definitions.
        /// </summary>
        public void ClearDefinitions()
        {
            _physicalQueues.Clear();
            _virtualQueues.Clear();
        }

        /// <summary>
        /// Gets the queue definitions.
        /// </summary>
        public IEnumerable<QueueInstallationData> QueueDefinitions
        {
            get
            {
                foreach (QueueInstallationData item in (from p in _physicalQueues select p))
                {
                    yield return item;
                }

                foreach (QueueInstallationData item in (from p in _virtualQueues select p))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Gets the pending physical queue count.
        /// </summary>
        public int PendingPhysicalQueueCount
        {
            get { return _physicalQueues.Count; }
        }

        /// <summary>
        /// Gets the pending virtual queue count.
        /// </summary>
        public int PendingVirtualQueueCount
        {
            get { return _virtualQueues.Count; }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            _factory.Reset();
            _installer.ResetLog();
        }

        /// <summary>
        /// Constructs the queue definitions.
        /// </summary>
        /// <param name="ipStartValue">The start value of the last IP octet.</param>
        /// <param name="ipEndValue">The end value of the last IP octet.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="numberOfQueues">The number of queues.</param>
        /// <param name="addressCode">The address code.</param>
        /// <param name="incrementIP">if set to <c>true</c> [increment IP octet value].</param>
        /// <param name="enableSnmp">if set to <c>true</c> [enable SNMP].</param>
        /// <param name="renderOnClient">if set to <c>true</c> [render on client].</param>
        /// <param name="shareQueues">if set to <c>true</c> [share queues].</param>
        public void ConstructQueueDefinitions
            (
                int ipStartValue,
                int ipEndValue,
                string hostName,
                int numberOfQueues,
                string addressCode,
                bool incrementIP,
                bool enableSnmp,
                bool renderOnClient,
                bool shareQueues
            )
        {
            _virtualQueues.Clear();
            Collection<QueueInstallationData> items = _factory.Create
                (
                    CurrentDriver,
                    AdditionalDescription,
                    ipStartValue,
                    ipEndValue,
                    hostName,
                    numberOfQueues,
                    addressCode,
                    incrementIP,
                    enableSnmp,
                    renderOnClient,
                    shareQueues
                );

            foreach (QueueInstallationData item in items)
            {
                item.UseConfigurationFile = UseConfigurationFile;
                if (UseConfigurationFile)
                {
                    item.ConfigurationFilePath = ConfigurationFile;
                }
                else
                {
                    item.ConfigurationFilePath = string.Empty;
                }

                _virtualQueues.Add(item);
            }
        }

        /// <summary>
        /// Gets the package paths.
        /// </summary>
        public DriverPackagePathSet DriverPackagePaths
        {
            get { return _driverSetPaths; }
        }

        /// <summary>
        /// Opens the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public Collection<QueueInstallationData> Open(string path)
        {
            SaveData saveData = null;
            try
            {
                saveData = LegacySerializer.DeserializeXml<SaveData>(File.ReadAllText(path));
            }
            catch (ArgumentException ex)
            {
                throw new QueueInstallException("Failed top open file", ex);
            }
            catch (IOException ex)
            {
                throw new QueueInstallException("Failed top open file", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new QueueInstallException("Failed top open file", ex);
            }
            catch (NotSupportedException ex)
            {
                throw new QueueInstallException("Failed top open file", ex);
            }
            catch (SecurityException ex)
            {
                throw new QueueInstallException("Failed top open file", ex);
            }

            _printDrivers.Clear();
            foreach (string key in saveData.LoadedPackages.Keys)
            {
                _printDrivers.Add(key, saveData.LoadedPackages[key]);
            }

            UseConfigurationFile = saveData.PreConfigure;
            AdditionalDescription = saveData.PreConfigureText;

            return saveData.InstallData;
        }

        /// <summary>
        /// Saves data to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="items">The items.</param>
        public void Save(string path, Collection<QueueInstallationData> items)
        {
            SaveData saveData = new SaveData();
            saveData.InstallData = items;
            saveData.LoadedPackages = _printDrivers;
            saveData.PreConfigure = UseConfigurationFile;
            saveData.PreConfigureText = AdditionalDescription;
            saveData.Save(path);
        }

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
                _installer.Dispose();
            }
        }
    }
}
