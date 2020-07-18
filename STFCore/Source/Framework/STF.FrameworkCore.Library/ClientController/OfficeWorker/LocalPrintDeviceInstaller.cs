using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Print;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.WindowsAutomation;

namespace HP.ScalableTest.Framework.Automation.OfficeWorker
{
    /// <summary>
    /// This class represents a network attached device and provides capability to manage different aspects of the 
    /// driver and queue associated with the device.  This includes the device driver, the port, the
    /// shared queue setting, the render on client setting, any printer shortcuts and the driver
    /// configuration file.
    /// </summary>
    /// <remarks>
    /// This class will use different methods for installing associated print queues depending on the methods called.
    /// The default approach for creating a print queue uses the winspool.drv AddPrinter() method when the Install() method is called.
    /// If there is an indication that a printer shortcut has been
    /// provided, then a different approach is used.  If the shortcut is present, but is equal to "None", then the install utility
    /// will be used and will look something like the following example:
    /// <para>
    /// C:\Windows\system32\DriverStore\FileRepository\&lt;DriverPath>\install.exe /q /npf /smSTFVPRINT04:17800 /n"VP04 17800 UPD PCL 6 v5.2 0001"
    /// </para>
    /// If a shortcut is provided, then the command will look something like the example below:
    /// <para>
    /// C:\Windows\system32\DriverStore\FileRepository\&lt;DriverPath>\install.exe /q /npf /smSTFVPRINT04:17800 /n"VP04 17800 UPD PCL 6 v5.2 0001" /gcfm"shortcut"
    /// </para>
    /// The alternate approach used only applies if CreatePrintQueue() is specified.  In this method, the PrintUI utility is used to install the queue.
    /// Below is an example of the command line if PrintUI is used to install the queue:
    /// <para>
    /// rundll32 printui.dll,PrintUIEntry /if /b "VP04 17800 UPD PCL 6 v5.2 0001" /f "C:\Windows\system32\DriverStore\FileRepository\&lt;DriverPath>\hpcu112u.inf" /r "IP_STFVPRINT04:17800" /m "HP Universal Printing PCL 6 (v5.2)"
    /// </para>
    /// </remarks>
    [DataContract]
    internal class LocalPrintDeviceInstaller
    {
        public string QueueName { get; }
        public string DriverName { get; }
        public string DefaultShortcut { get; }

        private readonly DynamicLocalPrintQueueInfo _printQueueInfo;
        private readonly DriverDetails _driver;
        private readonly string _portName;
        private readonly string _configFile;
        private readonly string _defaultShortcut;

        public LocalPrintDeviceInstaller(DynamicLocalPrintQueueInfo printQueueInfo)
        {
            _printQueueInfo = printQueueInfo;
            if (!string.IsNullOrEmpty(printQueueInfo.PrintDriverConfiguration.FileName))
            {
                _portName = string.Format("{0}_IP_{1}:{2}", printQueueInfo.PrintDriverConfiguration.FileName, printQueueInfo.Address, printQueueInfo.PortNumber);
            }
            else
            {
                _portName = string.Format("IP_{0}:{1}", printQueueInfo.Address, printQueueInfo.PortNumber);
            }

            _driver = CreateDriver(printQueueInfo.PrintDriver, GlobalSettings.Items[Setting.PrintDriverServer]);
            _configFile = printQueueInfo.PrintDriverConfiguration.ConfigurationFile;
            _defaultShortcut = printQueueInfo.PrintDriverConfiguration.DefaultShortcut;

            QueueName = printQueueInfo.QueueName;
            DriverName = _driver.Name;
            DefaultShortcut = _defaultShortcut;
        }

        public static DriverDetails CreateDriver(PrintDriverInfo printDriverInfo, string printDriverServer)
        {
            var driver = new DriverDetails();
            driver.Name = printDriverInfo.DriverName;
            driver.PrintProcessor = printDriverInfo.PrintProcessor;

            if (!Environment.Is64BitOperatingSystem)
            {
                driver.Architecture = DriverArchitecture.NTx86;
                driver.InfPath = Path.Combine(printDriverServer, printDriverInfo.InfX86);
            }
            else
            {
                driver.Architecture = DriverArchitecture.NTAMD64;
                driver.InfPath = Path.Combine(printDriverServer, printDriverInfo.InfX64);
            }

            return driver;
        }

        private string BuildShortcut()
        {
            if (string.IsNullOrEmpty(_defaultShortcut) || _defaultShortcut.Equals("None"))
            {
                return string.Empty;
            }

            string cfmFilePath = Path.Combine(Directory.GetParent(Path.GetDirectoryName(_driver.InfPath).Trim('\\')).FullName, Path.GetFileName(_configFile));
            TraceFactory.Logger.Debug("Config File:{0} -> {1}".FormatWith(_configFile, cfmFilePath));
            File.Copy(_configFile, cfmFilePath, true);

            var root = XElement.Load(cfmFilePath, LoadOptions.PreserveWhitespace);
            var current = root.Descendants("current").First().Element("printing");
            var features = current.Elements("feature");
            var shortcuts = features.Where(x => x.Attribute("resource_id").Value.Equals("3250"));

            try
            {
                if (shortcuts.Count() == 0)
                {
                    TraceFactory.Logger.Info("No printing shortcuts found in configuration File");
                    return cfmFilePath;
                }
            }
            catch (ArgumentNullException argNullException)
            {
                TraceFactory.Logger.Info("No printing shortcuts found in configuration File, {0}".FormatWith(argNullException.Message));
            }

            var shortcutList = shortcuts.Elements("option").Where(x => x.Attribute("resource_id").Value.Equals("-1") || x.Attribute("resource_id").Value.Equals("3264"));
            var selectedShortCut = shortcutList.FirstOrDefault(x => x.FirstNode.ToString().Trim().Equals(_defaultShortcut));
            var shortcutFeatures = selectedShortCut.Elements("feature");

            foreach (var shortcutFeature in shortcutFeatures)
            {
                current.Add(shortcutFeature);
            }

            root.Save(cfmFilePath, SaveOptions.None);

            //for some stupid illogical reason, we need to add a space at the EOF for DCU and driver to read this
            using (var stream = File.OpenWrite(cfmFilePath))
            {
                stream.Position = stream.Length;
                stream.Write(System.Text.Encoding.UTF8.GetBytes(" "), 0, 1);
                stream.Flush();
            }

            return cfmFilePath;
        }

        /// <summary>
        /// Installs a local print queue that points to this print device.
        /// </summary>
        /// <param name="installCheck">if set to <c>true</c> and the driver is already installed, then this will return.</param>
        /// <example>
        /// The example below shows how to install a driver and associated queue for a given local print
        /// device.  In this example, all drivers for a given distribution are first loaded, then one
        /// driver is chosen based on the system architecture.  In addition, a port is created that points
        /// to the target device.  The port, driver and platform type for the device are given to the print
        /// device object when then proceeds to install the driver and queue on the system.
        /// <code>
        /// // Load all drivers in the driver package located in the given directory
        /// PrintDeviceDriverCollection drivers = new PrintDeviceDriverCollection();
        /// drivers.LoadFromDirectory(@"\\DriverRepository\5.7.0\UPD\pcl6\winxp_vista_x64", includeAllArchitectures: true);
        /// if (drivers.Count() &gt; 0)
        /// {
        /// // Use the first driver from the distribution that matches this architecture
        /// PrintDeviceDriver driver = drivers.Where(x =&gt; x.Architecture == ProcessorArchitectureInfo.Current).FirstOrDefault();
        /// if (driver == null)
        /// {
        /// throw new InvalidOperationException("Device driver for this architecture not found in the distribution");
        /// }
        /// // Construct a port that the print queue will use when printing jobs.
        /// var port = StandardTcpIPPort.CreateRawPortData("15.198.212.221", portName: "IP_15.198.212.221", snmpEnabled: false);
        /// // Construct a local print device using all of the data built thus far.
        /// var device = new LocalPrintDevice(driver, port, DevicePlatform.Physical);
        /// // Install the device, which includes installing the driver and creating the queue
        /// device.Install();
        /// }
        /// </code></example>
        /// <remarks>This method goes through a series of calls to incrementally build the queue.  It starts by
        /// installing the driver if not already installed.  It then creates the printer port, then
        /// sets up the configuration file (CFM) if there is one, then applies the printer shortcut
        /// if it exists.  Then it adds the actual print queue, followed by the setup of client rendering
        /// and then configures the shared queue setting.  It then waits until all these steps are complete.</remarks>
        public void Install()
        {
            if (PrintQueueInstaller.IsInstalled(QueueName))
            {
                TraceFactory.Logger.Info("Print queue is already installed");
                return;
            }

            // Install the print driver
            CopyDriver();
            DriverInstaller.Install(_driver);

            // Create the Printer Port
            CreatePort();

            // Create the print queue
            CreatePrintQueue();

            // Wait until all the queue creation activity has completed.
            PrintQueueInstaller.WaitForInstallationComplete(QueueName, _driver.Name);
        }

        private void CreatePort()
        {
            LprPrinterPortInfo lprPortInfo = _printQueueInfo.PrinterPort as LprPrinterPortInfo;
            if (lprPortInfo != null)
            {
                TraceFactory.Logger.Debug("Creating LPR port. Queue Name = {0}".FormatWith(lprPortInfo.QueueName));
                PrintPortManager.AddLprPort(_portName, _printQueueInfo.PortNumber, _printQueueInfo.Address, lprPortInfo.QueueName, _printQueueInfo.SnmpEnabled, "public", 1);
            }
            else
            {
                TraceFactory.Logger.Debug("Creating RAW port.");
                PrintPortManager.AddRawPort(_portName, _printQueueInfo.PortNumber, _printQueueInfo.Address, _printQueueInfo.SnmpEnabled, "public", 1);
            }
        }

        private void CreatePrintQueue()
        {
            //no shortcut selected, use winspool to install
            if (string.IsNullOrEmpty(_configFile))
            {
                PrintQueueInstaller.CreatePrintQueue(QueueName, _driver.Name, _portName, _driver.PrintProcessor);
            }
            else
            {
                //the installer is always in the driver directory
                string installerPath = Path.GetDirectoryName(_driver.InfPath).Trim('\\') + "\\install.exe";
                if (!File.Exists(installerPath))
                {
                    throw new PrintQueueInstallationException("UPD Installer missing");
                }

                //derive the printer IP from the Portname
                string printerIp = _portName.Substring(_portName.LastIndexOf("_") + 1);

                //use the command line arguments to install the print queue
                string cfm;
                if (string.IsNullOrEmpty(_defaultShortcut) || _defaultShortcut.Equals("None"))
                {
                    cfm = _configFile;
                }
                else
                {
                    cfm = BuildShortcut();
                }

                FileInfo installer = new FileInfo(installerPath);
                FileInfo cfmFile = new FileInfo(cfm);
                PrintQueueInstaller.InstallUpdPrinter(installer, QueueName, printerIp, cfmFile);

            }
            TraceFactory.Logger.Info("Adding print queue complete.");
        }

        private void CopyDriver()
        {
            // Need a predictable location for the driver installation.  So just use
            // the original path (which should be off a server) and trim off the leading
            // UNC characters (\\), then save the driver to the relative local location,
            // which is the current working directory
            string source = Path.GetDirectoryName(_driver.InfPath);
            string destination = Path.GetDirectoryName(_driver.InfPath).Trim('\\');

            if (!Directory.Exists(destination))
            {
                FileSystem.CopyDirectory(source, destination);
                TraceFactory.Logger.Info("Successfully copied " + destination);
            }

            // Update the InfPath, since this is where the driver will reside locally.
            _driver.InfPath = Path.Combine(destination, Path.GetFileName(_driver.InfPath));
        }

        public void ValidatePrintingShortcut()
        {
            if (!string.IsNullOrEmpty(_configFile) && !string.IsNullOrEmpty(_defaultShortcut))
            {
                int shortcutConfig = GetShortcutCountFromConfig(_configFile);
                int shortcutRegistry = GetShortcutCountFromRegistry(_driver.Name, _portName);
                if (shortcutConfig == shortcutRegistry)
                {
                    TraceFactory.Logger.Info("All Shortcuts found in configuration were installed successfully for {0} ({1})".FormatWith(_driver.Name, _portName));
                }
                else
                {
                    TraceFactory.Logger.Info("All the shortcuts {0}/{1} were not installed for {2} ({3}), Please validate Manually".FormatWith(shortcutRegistry, shortcutConfig, _driver.Name, _portName));
                }
            }
        }

        private int GetShortcutCountFromRegistry(string printDriverName, string printQueueName)
        {
            string queueName = "{0} ({1})".FormatWith(printDriverName, printQueueName);
            return Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Print\Printers\{0}\HPPresetRoot\PresetPoolData".FormatWith(queueName)).ValueCount;
        }

        private int GetShortcutCountFromConfig(string configFile)
        {
            if (string.IsNullOrEmpty(configFile))
            {
                return 0;
            }

            var xRoot = XElement.Load(configFile);
            var xCurrent = xRoot.Descendants("current").First().Element("printing");
            var xFeatures = xCurrent.Elements("feature");
            var shortcut = xFeatures.Where(x => x.Attribute("resource_id").Value.Equals("3250")).First();
            var shortcuts = shortcut.Elements("option");
            return shortcuts.Count();
        }
    }
}