using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.Utility;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.ComplexPrint
{
    /// <summary>
    /// This is a demo execution controller for an Activity Plug-in
    /// </summary>
    public class ComplexPrintExecutionEngine : IPluginExecutionEngine
    {
        ComplexPrintTests _printTests = null;
        private object installationLock = new object();

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            ComplexPrintActivityData activityData = executionData.GetMetadata<ComplexPrintActivityData>(CtcMetadataConverter.Converters);

            /* Complex printing plugin might use more than 1 client machine based on user scenario.
			 * All inputs related to printer are maintained in a configuration file in share location (etlhubrepo\boi\CTC\<Product_Family>)
			 * Since multiple machines are trying to access the config file, file is copied to local machine.
			 * */

            // Get details from configuration file
            string configFilePath = Path.Combine(CtcSettings.ConnectivityShare, activityData.ProductFamily.ToString(), "CNP.xml");
            TraceFactory.Logger.Debug("Config File path: {0}".FormatWith(configFilePath));
            FileStream file = null;

            try
            {
                XmlDocument configFile = new XmlDocument();
                string tempFilePath = Path.Combine(Path.GetTempPath(), "CNP.xml");
                File.Copy(configFilePath, tempFilePath, true);

                file = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read);
                configFile.Load(file);

                activityData.Ipv4Address = configFile.DocumentElement.SelectSingleNode("IPv4Address").InnerText;
                activityData.DriverPackagePath = configFile.DocumentElement.SelectSingleNode("DriverPath").InnerText;
                activityData.DriverModel = configFile.DocumentElement.SelectSingleNode("DriverModel").InnerText;
                activityData.ProductName = configFile.DocumentElement.SelectSingleNode("ProductName").InnerText;
                activityData.DocumentsPath = configFile.DocumentElement.SelectSingleNode("FilesPath").InnerText;
                activityData.SitemapsVersion = configFile.DocumentElement.SelectSingleNode("Sitemaps").InnerText;
                activityData.PrinterConnectivity = ConnectivityType.Wired;
            }
            catch (Exception exception)
            {
                TraceFactory.Logger.Fatal("Failed to read configuration from xml document. Exception details: {0}".FormatWith(exception.JoinAllErrorMessages()));
            }
            finally
            {
                if (null != file)
                {
                    file.Close();
                }
            }

            /* If a WS Print test is selected in user scenario, a pop-up needs to be shown to 'Add the printer'.			 
			 * Read through all selected tests and keep track if WSP pop-up needs to be shown up.
			 * */

            // Check if WSP test is selected by checking the test attributes for each test
            bool isWSPSelected = false;

            foreach (int testNumber in activityData.SelectedTests)
            {
                // Fetch the TestDetailsAttribute of the test method matching the testNumber
                var attributes = typeof(ComplexPrintTests).GetMethods().Where(item => item.GetCustomAttributes(new TestDetailsAttribute().GetType(), false).Length > 0
                        && ((TestDetailsAttribute)item.GetCustomAttributes(new TestDetailsAttribute().GetType(), false)[0]).Id.Equals(testNumber))
                        .Select(x => x.GetCustomAttributes(new TestDetailsAttribute().GetType(), false)[0]);

                TestDetailsAttribute testAttributes = (TestDetailsAttribute)attributes.FirstOrDefault();

                if (testAttributes.Category.Contains("WSP", StringComparison.CurrentCultureIgnoreCase))
                {
                    isWSPSelected = true;
                    break;
                }
            }

            if (isWSPSelected)
            {
                TraceFactory.Logger.Debug("Please add WS Printer on client machine: {0}".FormatWith(Environment.MachineName));
            }

            /* To keep track of whether a WSP test is added in any of the user scenario, a common file is maintained under shared location and all the clients need to update this file on WSP status.
			 * Folder structure: etlhubrepo\boi\CTC\<Product_Family>temp\<Session_ID>.xml
			 * Since many clients will be trying to write data to the xml file, check if xml is already created (by some other client), create if not.
			 * If a WSP test is selected, an entry is made with 'Yes' as the value, else 'No' with machine hostname as the unique identifier for the node.
			 * Status for the WS Print addition is maintained in 'Status' attribute. If scenario doesn't have a WSP test selected, 'Completed' status is added by default. Otherwise 'In Progress' is added.
			 * 
			 * Locking mechanism:			 
			 * Since its a shared file, a lock is applied when a file is in use by a machine. Lock file for writing the data and release once completed.
			 * Acquire resource for max of 20 seconds to perform write operation. Wait for max of 3 minutes to use resource for writing into the file. (Waiting in Queue to write into the file)
			 * */

            string configFolder = Path.Combine(CtcSettings.ConnectivityShare, activityData.ProductFamily.ToString(), "temp");

            // Check if folder exists already.
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }

            string xmlFilePath = Path.Combine(configFolder, executionData.SessionId + ".xml");
            XElement machines = null;

            // Check if file exists already. Create root node 'Machines' if file is not created else load the file
            if (!File.Exists(xmlFilePath))
            {
                machines = new XElement("Machines");
            }
            else
            {
                XDocument xmlDocument = XDocument.Load(xmlFilePath);
                machines = xmlDocument.Element("Machines");
            }

            ReaderWriterLock fileLock = new ReaderWriterLock();
            bool isFileWritten = false;
            DateTime currentTime = DateTime.Now;
            TimeSpan waitTime = TimeSpan.FromMinutes(3);
            Random randomTime = new Random();

            do
            {
                // Since many clients are trying to access the file, generate a random timeout for acquiring lock on the file.
                int timeOut = randomTime.Next(10, 20);

                if (!fileLock.IsWriterLockHeld)
                {
                    fileLock.AcquireWriterLock(TimeSpan.FromSeconds(timeOut));

                    XElement machine = new XElement("Machine", new XAttribute("VM_Name", Environment.MachineName),
                                                            new XAttribute("WSP_Print", isWSPSelected ? "Yes" : "No"),
                                                            new XAttribute("Status", isWSPSelected ? "In Progress" : "Completed"));

                    machines.Add(machine);
                    machines.Save(xmlFilePath);

                    // Wait for sometime to release the lock after saving file.
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    isFileWritten = true;
                    fileLock.ReleaseWriterLock();
                }

            } while (!isFileWritten && DateTime.Now.Subtract(waitTime) <= currentTime);

            /* Check if printer is accessible with IPv4 address and IPv6 addresses: Linklocal, Stateless and Stateful address.
			 * Pop-up a message box if printer address is not pinging.
			 * */

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.Ipv4Address));

            // If printer is not available, assign default IPAddress
            if (printer.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), 1))
            {
                // Get All Ipv6 Addresses 
                activityData.Ipv6LinkLocalAddress = printer.IPv6LinkLocalAddress == null ? string.Empty : printer.IPv6LinkLocalAddress.ToString();
                activityData.Ipv6StateFullAddress = printer.IPv6StateFullAddress == null ? string.Empty : printer.IPv6StateFullAddress.ToString();
                activityData.Ipv6StatelessAddress = printer.IPv6StatelessAddresses.Count == 0 ? string.Empty : printer.IPv6StatelessAddresses[0].ToString();
            }
            else
            {
                activityData.Ipv6LinkLocalAddress = string.Empty;
                activityData.Ipv6StateFullAddress = string.Empty;
                activityData.Ipv6StatelessAddress = string.Empty;
            }

            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromSeconds(10)))
            {
                MessageBox.Show(string.Concat("Printer IPv4 Address is not accessible\n\n",
                                                "IPv4 address: {0}\n".FormatWith(activityData.Ipv4Address)),
                                                "IPv4 Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new PluginExecutionResult(PluginResult.Failed, "Printer IPv4 Address is not accessible");
            }

            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv6LinkLocalAddress), TimeSpan.FromSeconds(10)))
            {
                MessageBox.Show(string.Concat("Printer Link Local Address is not accessible\n\n",
                                                "Link local address: {0}\n".FormatWith(activityData.Ipv6LinkLocalAddress),
                                                "Check if Stateless and Stateful address are pinging if you have selected it.\n",
                                                "Stateless: {0}, Stateful: {1}".FormatWith(activityData.Ipv6StatelessAddress, activityData.Ipv6StateFullAddress)),
                                                "Link Local Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new PluginExecutionResult(PluginResult.Failed, "Printer Link Local Address is not accessible");
            }

            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv6StatelessAddress), TimeSpan.FromSeconds(10)))
            {
                MessageBox.Show(string.Concat("Printer Stateless Address is not accessible\n\n",
                                                "Stateless address: {0}\n".FormatWith(activityData.Ipv6StatelessAddress),
                                                "Check if Stateful address is pinging if you have selected it.\n",
                                                "Stateful: {0}".FormatWith(activityData.Ipv6StateFullAddress)),
                                                "Stateless Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new PluginExecutionResult(PluginResult.Failed, "Printer Stateless Address is not accessible");
            }

            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv6StateFullAddress), TimeSpan.FromSeconds(10)))
            {
                MessageBox.Show(string.Concat("Printer Stateful Address is not accessible\n\n",
                                                "Stateful address: {0}\n".FormatWith(activityData.Ipv6StateFullAddress)),
                                                "Stateful Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new PluginExecutionResult(PluginResult.Failed, "Printer Stateful Address is not accessible");
            }

            /* Since multiple clients can access drivers, print driver is copied to local machine so as to avoid 'File in use' exception.
			 * Driver path is updated to local temp directory for further use.
			 * */

            string tempDriverDirectory = Path.Combine(Path.GetTempPath(), "PrintDriver");
            TraceFactory.Logger.Debug("Local printer driver location: {0}".FormatWith(tempDriverDirectory));

            if (Directory.Exists(tempDriverDirectory))
            {
                Directory.Delete(tempDriverDirectory, true);
            }

            foreach (string dirPath in Directory.GetDirectories(activityData.DriverPackagePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(activityData.DriverPackagePath, tempDriverDirectory));
            }

            foreach (string filePath in Directory.GetFiles(activityData.DriverPackagePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(filePath, filePath.Replace(activityData.DriverPackagePath, tempDriverDirectory), true);
            }

            activityData.DriverPackagePath = tempDriverDirectory;

            // By default, adding P9100 printer so that driver installation is complete while adding WS Printer.
            printer.Install(IPAddress.Parse(activityData.Ipv4Address), Printer.Printer.PrintProtocol.RAW, activityData.DriverPackagePath, activityData.DriverModel, 9100);

            // If WSP test is selected, Show a pop-up to Add the printer.
            if (isWSPSelected)
            {
                /* If a WSP test is selected in any scenario created by user, pop a message box to add the printer.
				 * Status on whether a WSP test is selected are maintained in the config file (etlhubrepo\boi\CTC\<Product_Family>temp\<Session_ID>.xml) with attribute 'WSP_Print'.
				 * In case this scenario is added with WSP test, Pop-up message. Once the Ok button is clicked, updated the 'Status' on config file to 'Completed'.
				 * */

                XElement localMachine = null;
                XDocument document = XDocument.Load(xmlFilePath);
                XElement element = document.Element("Machines");

                foreach (var vm in element.Elements("Machine"))
                {
                    if (vm.Attribute("VM_Name").Value.Equals(Environment.MachineName))
                    {
                        localMachine = vm;
                        TraceFactory.Logger.Info("Machine: {0}".FormatWith(localMachine.Attribute("VM_Name").Value));
                        break;
                    }
                }

                MessageBox.Show("Add WS Printer and click 'OK'.", "Add WS Printer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                localMachine.SetAttributeValue("Status", "Completed");
                element.Save(xmlFilePath);
            }

            /* Wait till all clients are ready for execution: If a client is configured with WSP test, wait till user adds the printer.
			 * This case is handled with updation of the config file on status of WS Printer installation. 
			 * Traverse through all Elements and check for 'Status' attribute to know the status on installation of WS Printer.
			 * */

            bool isWSPrinterInstalled = true;

            do
            {
                TraceFactory.Logger.Debug("Waiting for all clients to complete WS Printer installation.");

                // In case isWSPrinterInstalled is set to false while traversing through the config file, set it back to true for next iteration
                isWSPrinterInstalled = true;

                // Wait for some time so that other clients update the status on WS Printer installation
                Thread.Sleep(TimeSpan.FromSeconds(10));

                // Reload the file to get the most updated data
                XDocument xDocument = XDocument.Load(xmlFilePath);
                XElement xElement = xDocument.Element("Machines");

                foreach (var vm in xElement.Elements("Machine"))
                {
                    isWSPrinterInstalled &= "Completed".Equals(vm.Attribute("Status").Value);
                }

            } while (!isWSPrinterInstalled);

            if (null == _printTests)
            {
                _printTests = new ComplexPrintTests(activityData);
            }

            foreach (int testNumber in activityData.SelectedTests)
            {
                ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                _printTests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.Ipv4Address), activityData.ProductFamily);
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }
    }
}