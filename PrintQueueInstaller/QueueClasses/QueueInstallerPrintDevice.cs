using System;
using System.IO;
using System.Linq;
using System.Printing;
using HP.ScalableTest.Framework.Automation.OfficeWorker;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Used to access the protected members of the LocalPrintDeviceInstaller
    /// </summary>
    internal class InstallerPrintDevice
    {
        private static readonly object _fileCopyLock = new object();

        /// <summary>
        /// Gets or sets the name of the print queue for this print device.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets a target driver to use from the collection.
        /// </summary>
        /// <value>The <see cref="DriverDetails"/> associated with this device instance.</value>
        public PrintDeviceDriver Driver { get; set; }

        /// <summary>
        /// Gets or sets the port information this print device will use.
        /// </summary>
        /// <value>An <see cref="TcpIPPortInstaller"/> object representing the port for the queue associated with this device.</value>
        public TcpIPPortInstaller Port { get; set; }

        /// <summary>
        /// Gets or sets the printer configuration file (CFM) that will be associated with this device and queue
        /// </summary>
        /// <value>
        /// The configuration file name.
        /// </value>
        public string ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether jobs will render EMF data on the client. Only applies to Vista and above.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is rendered on the client; otherwise, <c>false</c>.
        /// </value>
        public bool IsRenderedOnClient { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the queue for this device is shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is shared; otherwise, <c>false</c>.
        /// </value>
        public bool IsSharedQueue { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerPrintDevice" /> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="port">The port.</param>
        public InstallerPrintDevice(PrintDeviceDriver driver, TcpIPPortInstaller port)
        {
            Driver = driver;
            Port = port;
            QueueName = "{0} ({1})".FormatWith(driver.Name, port.PortName);
        }

        /// <summary>
        /// Creates the print queue.
        /// </summary>
        public void CreatePrintQueue(TimeSpan timeout)
        {
            // TODO: This method of setting up the configuration file changes the configuration for ALL print queues
            // that are subsequently installed using this driver.  To configure a particular queue with a CFM without changing the
            // default configuration for the driver, this method should be modified to call install.exe in the UPD driver
            // directory instead of using printui.dll.  Non-UPD drivers do not have an installer and should continue to use
            // the existing method.

            if (!string.IsNullOrEmpty(ConfigFile))
            {
                SetupConfigFile();
            }

            var printUI = Resource.InstallQueuePrintUI.FormatWith
                (
                    QueueName,
                    Driver.InfPath,
                    Port.PortName,
                    Driver.Name
                );

            TraceFactory.Logger.Debug("{0}".FormatWith(printUI));
            var result = ProcessUtil.Execute("cmd.exe", "/C {0}".FormatWith(printUI), timeout);
            if (!result.SuccessfulExit || result.ExitCode > 0)
            {
                throw new InvalidOperationException(result.StandardError);
            }
        }

        private void SetupConfigFile()
        {
            // Derive the name of the CFM file by getting the name of the CFG file from the distribution
            // and giving it the same base name.
            TraceFactory.Logger.Debug("ConfigFile: {0}".FormatWith(ConfigFile));
            string location = string.IsNullOrEmpty(Driver.InfPath) ? string.Empty : Path.GetDirectoryName(Driver.InfPath);
            string[] driverConfigFiles = Directory.GetFiles(location, "*.cfg");
            string defaultConfigFileName = Path.GetFileNameWithoutExtension(driverConfigFiles.Single());

            // Note that this will need to be more dynamic when we starting installing v4 drivers.
            string destination = Path.Combine(DriverController.SystemVersion3DriverDirectory, defaultConfigFileName + ".cfm");

            TraceFactory.Logger.Debug("Destination: {0}".FormatWith(destination));

            lock (_fileCopyLock)
            {
                if (!File.Exists(destination))
                {
                    try
                    {
                        File.Copy(ConfigFile, destination, true);
                    }
                    catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
                    {
                        TraceFactory.Logger.Error("Failed to copy CFM File", ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the client rendering mode for Vista and above.
        /// </summary>
        public void EnableClientRendering()
        {
            PrintQueue queue = PrintQueueController.GetPrintQueue(QueueName);
            PrintQueueController.SetJobRenderLocation(queue, IsRenderedOnClient ? PrintJobRenderLocation.Client : PrintJobRenderLocation.Server);
        }

        /// <summary>
        /// Sets up the printer as a share
        /// </summary>
        public void EnableSharedQueue()
        {
            using (LocalPrintServer server = new LocalPrintServer(PrintSystemDesiredAccess.AdministrateServer))
            {
                using (PrintQueue queue = new PrintQueue(server, QueueName, PrintSystemDesiredAccess.AdministratePrinter))
                {
                    queue.ShareName = QueueName;
                    queue.Commit();

                    PrintQueueController.ChangeAttributes(queue, PrintQueueAttributes.Shared, true);
                }
            }
        }
    }
}
