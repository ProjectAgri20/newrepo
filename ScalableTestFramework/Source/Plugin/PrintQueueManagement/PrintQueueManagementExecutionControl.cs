using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Printing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Plugins.PrintQueueManagement.UIMap;
using HP.ScalableTest.PluginSupport.TopCat;
using HP.ScalableTest.Print;
using HP.ScalableTest.Utility;
using Microsoft.Win32;
using TopCat.TestApi.GUIAutomation;

namespace HP.ScalableTest.Plugin.PrintQueueManagement
{
    /// <summary>
    /// Print queue management execution control
    /// </summary>
    [ToolboxItem(false)]
    public partial class PrintQueueManagementExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PrintQueueManagementActivityData _activityData;

        private int _taskCounter;
        private TopCatScript _configureScript;
        private DocumentCollection _documentCollection;
        private PrintQueue _defaultPrintQueue;
        private PrintQueueInfo _printQueueInfo;
        private DynamicLocalPrintQueueInfo _localPrintQueueInfo;
        private int shortTimeout = 5;
        private readonly TimeSpan _humanTimespan = TimeSpan.FromSeconds(1);
        /// <summary>
        /// constructor
        /// </summary>
        public PrintQueueManagementExecutionControl()
        {
            InitializeComponent();

            activityStatus_dataGridView.AutoGenerateColumns = false;
            TopCatUIAutomation.Initialize();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _activityData = executionData?.GetMetadata<PrintQueueManagementActivityData>();
            _documentCollection = executionData?.Documents;

            _localPrintQueueInfo = executionData?.PrintQueues.First() as DynamicLocalPrintQueueInfo;

            _taskCounter = 0;

            foreach (var pqmTask in _activityData.PrintQueueTasks)
            {
                activityStatus_dataGridView.DataSource = null;

                switch (pqmTask.Operation)
                {
                    case PrintQueueOperation.Install:
                        {
                            pqmTask.Status = InstallPrintQueue(executionData);
                        }
                        break;

                    case PrintQueueOperation.Upgrade:
                        {
                            UpgradePrintQueue(pqmTask, executionData.Environment.PluginSettings);
                        }
                        break;

                    case PrintQueueOperation.Uninstall:
                        {
                            pqmTask.Status = UninstallPrintQueue();
                        }
                        break;

                    case PrintQueueOperation.Print:
                        {
                            pqmTask.Status = PrintDocument(pqmTask.TargetObject.ToString());
                        }
                        break;

                    case PrintQueueOperation.Cancel:
                        {
                            pqmTask.Status = CancelDocument(pqmTask.TargetObject.ToString(), pqmTask.Delay);
                        }
                        break;

                    case PrintQueueOperation.Configure:
                        {
                            ConfigureQueueForm(pqmTask);
                        }
                        break;
                }
                activityStatus_dataGridView.Visible = false;

                activityStatus_dataGridView.DataSource = _activityData.PrintQueueTasks;
                activityStatus_dataGridView.Visible = true;

                _taskCounter++;
            }

            return _activityData.PrintQueueTasks.All(x => x.Status == Status.Passed) ? new PluginExecutionResult(PluginResult.Passed) : new PluginExecutionResult(PluginResult.Failed);
        }

        #region InstallPrintQueue

        /// <summary>
        /// Installs the print queue
        /// </summary>
        private Status InstallPrintQueue(PluginExecutionData executionData)
        {
            if (executionData.PrintQueues.Count == 0)
            {
                return Status.Skipped;
            }


            _printQueueInfo = executionData.PrintQueues.First();
            _defaultPrintQueue = PrintQueueController.Connect(_printQueueInfo);

            // Log the device/server that was used for this job, if one was specified
            LogDevice(executionData, _printQueueInfo);

            //To check count of paper size,type and trays
            try
            {
                int paperSize = GetCountFromRegistry("MediaSize");
                int paperType = GetCountFromRegistry("MediaType");
                int inputtrays =
                    ((string[])
                        Registry.LocalMachine.OpenSubKey(
                            $@"System\CurrentControlSet\Control\Print\Printers\{ (object)_defaultPrintQueue.FullName}\PrinterDriverData").GetValue("InputSlot")).Count(
                                    x => x.StartsWith("Tray", StringComparison.OrdinalIgnoreCase));

                ExecutionServices.SystemTrace.LogInfo($"Paper Sizes available for { (object)_defaultPrintQueue.FullName} are { (object)paperSize}");
                ExecutionServices.SystemTrace.LogInfo($"Paper Types available for { (object)_defaultPrintQueue.FullName} are { (object)paperType}");
                ExecutionServices.SystemTrace.LogInfo($"Trays available  for {_defaultPrintQueue.Name}  are {inputtrays}");
            }
            catch (NullReferenceException nullRefEx)
            {
                ExecutionServices.SystemTrace.LogInfo($"Something went wrong when trying to read values from registry, Please manually validate the supported paper sizes, types and trays supported. Exception: { (object)nullRefEx.Message}");
            }
            if (ValidatePrintingShortcut(_defaultPrintQueue.FullName))
            {
                try
                {
                    //this is a fix for defaultprintqueue.connected not working in windows vista due to UAC
                    foreach (string printerName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (_defaultPrintQueue.FullName.Equals(printerName))
                        {
                            if (_activityData.IsDefaultPrinter)
                            {
                                PrintQueueController.SetDefaultQueue(_defaultPrintQueue);
                            }

                            ExecutionServices.SystemTrace.LogDebug($"Printer installed: { (object)printerName}");
                            return Status.Passed;
                        }
                    }
                }
                catch
                {
                    return Status.Failed;
                }
            }
            else
            {
                ExecutionServices.SystemTrace.LogInfo(
                    $"Failed to install the device: {_printQueueInfo.AssociatedAssetId}");
                return Status.Failed;
            }

            return Status.Passed;
        }

        private static void LogDevice(PluginExecutionData executionData, PrintQueueInfo printer)
        {
            if (!string.IsNullOrEmpty(printer.AssociatedAssetId))
            {
                var log = new ActivityExecutionAssetUsageLog(executionData, printer.AssociatedAssetId);
                ExecutionServices.DataLogger.Submit(log);
            }
        }

        /// <summary>
        /// Validate the install of printing shortcuts
        /// </summary>
        /// <param name="portName">the portname</param>
        /// <returns></returns>
        private bool ValidatePrintingShortcut(string portName)
        {
            if (!string.IsNullOrEmpty(_localPrintQueueInfo.PrintDriverConfiguration.ConfigurationFile) &&
                !string.IsNullOrEmpty(_localPrintQueueInfo.PrintDriverConfiguration.DefaultShortcut))
            {
                int shortcutConfig = GetShortcutCountFromConfig(_localPrintQueueInfo.PrintDriverConfiguration.ConfigurationFile);
                int shortcutRegistry = GetShortcutCountFromRegistry(portName);

                if (shortcutConfig == shortcutRegistry)
                {
                    ExecutionServices.SystemTrace.LogInfo(
                        $"All Shortcuts found in configuration were installed successfully for { (object)portName}");
                    return true;
                }
                ExecutionServices.SystemTrace.LogInfo(
                    $"All the shortcuts {shortcutRegistry}/{shortcutConfig} were not installed for {portName}, Please validate Manually");
                return false;
            }

            return true;
        }

        /// <summary>
        /// gets the device attribute count from the registry
        /// </summary>
        /// <param name="deviceAttribute">paper size, media count, etc.</param>
        /// <returns></returns>
        private int GetCountFromRegistry(string deviceAttribute)
        {
            var stringToCount = Registry.LocalMachine.OpenSubKey(
               $@"System\CurrentControlSet\Control\Print\Printers\{_defaultPrintQueue.FullName}\PrinterDriverData\JCTData").GetValue(deviceAttribute).ToString();
            return string.IsNullOrEmpty(stringToCount) ? 0 : stringToCount.Split(';').Length;
        }

        /// <summary>
        /// Gets the number of printing shortcuts available
        /// </summary>
        /// <param name="printQueueName">the print queue</param>
        /// <returns></returns>
        private static int GetShortcutCountFromRegistry(string printQueueName)
        {
            return
                Registry.LocalMachine.OpenSubKey(
                    $@"System\CurrentControlSet\Control\Print\Printers\{printQueueName}\HPPresetRoot\PresetPoolData").ValueCount;
        }

        /// <summary>
        /// Gets the shortcut present in the CFM file
        /// </summary>
        /// <param name="configFile">CFM file</param>
        /// <returns></returns>
        private static int GetShortcutCountFromConfig(string configFile)
        {
            if (string.IsNullOrEmpty(configFile))
            {
                return 0;
            }

            var xRoot = XElement.Load(configFile);
            var xCurrent = xRoot.Descendants("current").First().Element("printing");
            var xFeatures = xCurrent?.Elements("feature");
            var shortcut = xFeatures?.First(x => x.Attribute("resource_id").Value.Equals("3250"));
            //3250 indicates printing shortcuts in CFM file
            var shortcuts = shortcut?.Elements("option");
            return shortcuts.Count();
        }

        #endregion InstallPrintQueue

        /// <summary>
        /// upgrades the print queue with the new driver
        /// </summary>
        /// <param name="pqmTask">contains the info about the driver to be upgraded to</param>
        private void UpgradePrintQueue(PrintQueueManagementTask pqmTask, SettingsDictionary pluginSettings)
        {
            var upgradeDriverInfo = (PrintDriverInfo)pqmTask.TargetObject;

            string printDriverPath = Path.Combine(pluginSettings["PrintDriverServer"], Environment.Is64BitOperatingSystem ? upgradeDriverInfo.InfX64 : upgradeDriverInfo.InfX86);

            StringBuilder upgradeDriverArgs = new StringBuilder();
            upgradeDriverArgs.Append(
                $"/ia /v \"Type 3 - User Mode\" /m \"{_localPrintQueueInfo.PrintDriver.DriverName}\"");
            upgradeDriverArgs.Append($" /n \"{_defaultPrintQueue.FullName}\"");
            upgradeDriverArgs.Append($" /f \"{printDriverPath}\"");
            upgradeDriverArgs.Append($" /l \"{Path.GetDirectoryName(printDriverPath)}\"");
            var action = new Action(() =>
            {
                var result = ProcessUtil.Execute("rundll32", $"printui.dll,PrintUIEntry { (object)upgradeDriverArgs.ToString()}");
                if (result.ExitCode != 0)
                {
                    pqmTask.Status = Status.Failed;
                }

                pqmTask.Status = Status.Passed;
                ExecutionServices.SystemTrace.LogInfo(
                    $"Upgraded the driver for the device: {_printQueueInfo.AssociatedAssetId}");
                device_textBox.Text = _printQueueInfo.AssociatedAssetId + @", " +
                                      GetDriverInfo(_defaultPrintQueue.FullName);
            });
            var token = new LocalLockToken(_defaultPrintQueue.FullName, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
            ExecutionServices.CriticalSection.Run(token, action);
        }

        /// <summary>
        /// uninstalls the print queue
        /// </summary>
        private Status UninstallPrintQueue()
        {
            foreach (string printerName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (_defaultPrintQueue.FullName.Equals(printerName))
                {
                    var result = ProcessUtil.Execute("rundll32", $"printui.dll,PrintUIEntry /dl /n\"{_defaultPrintQueue.FullName}\"");
                    if (result.ExitCode != 0)
                    {
                        return Status.Failed;
                    }
                }
            }

            //sleepig 5 seconds to check if uninstall worked
            Thread.Sleep(TimeSpan.FromSeconds(5));
            //check if the printer is uninstalled or not

            if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().Any(printerName => _defaultPrintQueue.FullName.Equals(printerName)))
            {
                return Status.Failed;
            }

            ExecutionServices.SystemTrace.LogInfo($"Uninstalled the device: {_printQueueInfo.AssociatedAssetId}");

            return Status.Passed;
        }

        /// <summary>
        /// Prints the document to the print queue
        /// </summary>
        private Status PrintDocument(string docIdString)
        {
            string jobFileName;

            Guid docId;
            if (Guid.TryParse(docIdString, out docId))
            {
                jobFileName = ExecutionServices.FileRepository.GetFile(_documentCollection.First(x => x.DocumentId == docId)).FullName;
            }
            else
            {
                jobFileName = ExecutionServices.FileRepository.GetFile(_documentCollection.First()).FullName;
            }

            try
            {
                PrintingEngine engine = new PrintingEngine();
                var result = engine.Print(jobFileName, _defaultPrintQueue);
                ExecutionServices.SystemTrace.LogDebug(result.JobEndTime.LocalDateTime);
                _activityData.PrintQueueTasks.ElementAt(_taskCounter).Status = Status.Passed;
            }
            catch (Exception genericException)
            {
                ExecutionServices.SystemTrace.LogDebug(genericException.Message);
                _activityData.PrintQueueTasks.ElementAt(_taskCounter).Status = Status.Failed;
                return Status.Failed;
            }
            return Status.Passed;
        }

        /// <summary>
        /// starts the print and cancels it before it gets to be printed
        /// </summary>
        /// <param name="docIdString"></param>
        /// <param name="delay"></param>
        private Status CancelDocument(string docIdString, int delay)
        {
            //DocumentLibraryManager manager = new DocumentLibraryManager();
            Guid docId;
            var jobFileName = Guid.TryParse(docIdString, out docId) ? ExecutionServices.FileRepository.GetFile(_documentCollection.First(x => x.DocumentId == docId)).FullName : ExecutionServices.FileRepository.GetFile(_documentCollection.First()).FullName;

            PrintingEngine engine = new PrintingEngine();
            var result = engine.Print(jobFileName, _defaultPrintQueue);
            Thread.Sleep(TimeSpan.FromSeconds(delay));
            return CancelJob(_defaultPrintQueue, result.UniqueFileName);
        }

        private static Status CancelJob(PrintQueue printQueue, string uniqueFileName)
        {
            // If you do not "refresh" the print queue, then getting information about the jobs will fail.
            printQueue.Refresh();
            PrintJobInfoCollection jobs = printQueue.GetPrintJobInfoCollection();

            // Extension is pulled out because some applications omit the file extension when it creates the job name.
            string fileName = Path.GetFileNameWithoutExtension(uniqueFileName);
            PrintSystemJobInfo jobInfo = jobs.FirstOrDefault(j => j.Name.Contains(fileName));
            jobInfo?.Cancel();

            //wait for 20 seconds to check if the job got deleted
            DateTime expireTime = DateTime.Now + TimeSpan.FromSeconds(20);
            while (jobInfo.IsDeleting && DateTime.Now < expireTime)
            {
                Thread.Sleep(100);
            }

            if (jobInfo.IsDeleted)
            {
                return Status.Passed;
            }
            return Status.Failed;
        }

        /// <summary>
        /// Configures the print preferences of the print queue
        /// </summary>
        /// <param name="pqmTask">contains the info about print queue</param>
        private void ConfigureQueueForm(PrintQueueManagementTask pqmTask)
        {
            string queueName = _defaultPrintQueue.FullName;
            var action = new Action(() =>
            {
                PrintPreferences preferences =
                    new PrintPreferences(TopCat.TestApi.GUIAutomation.Enums.UIAFramework.ManagedUIA)
                    {
                        DriverModel = _localPrintQueueInfo.PrintDriver.DriverName
                    };

                ProcessUtil.Execute("cmd.exe", $"/c rundll32 printui.dll,PrintUIEntry /p /n \"{queueName}\"", TimeSpan.FromSeconds(shortTimeout));
                preferences.PrintPreferencesWindow.WaitForAvailable(shortTimeout);
                preferences.DeviceSettingsTabItem.Select(shortTimeout);
                preferences.InstallableOptionTreeItem.Select(shortTimeout);

                if (preferences.DuplexUnitfor2STreeItem.IsAvailable())
                {
                    preferences.DuplexUnitfor2STreeItem.Select(shortTimeout);
                }
                else
                {
                    preferences.DuplexUnitfor2SNTreeItem.Select(shortTimeout);
                }

                preferences.SelectListItem(pqmTask.Preference.Duplexer);
                preferences.OKButton1Button.Click(shortTimeout);

                ProcessUtil.Execute("cmd.exe", $"/c rundll32 printui.dll,PrintUIEntry /e /n \"{queueName}\"", TimeSpan.FromSeconds(shortTimeout));

                preferences.PrintPreferencesWindow.WaitForAvailable(shortTimeout);
                preferences.PrintingShortcutTabItem.Select(shortTimeout);


                preferences.SelectListItem(pqmTask.Preference.PaperSize);
                Thread.Sleep(_humanTimespan);
                preferences.SelectListItem(pqmTask.Preference.InputTray);
                Thread.Sleep(_humanTimespan);
                preferences.SelectListItem(pqmTask.Preference.PaperType);
                Thread.Sleep(_humanTimespan);
                preferences.SelectListItem(pqmTask.Preference.Orientation);
                Thread.Sleep(_humanTimespan);
                preferences.SelectListItem(pqmTask.Preference.DuplexValue);

                preferences.AdvancedTabItem.Select(shortTimeout);

                preferences.CopyCountmsctlsSpinner.SetValue(pqmTask.Preference.Copies);

                preferences.OKButton1Button.Click(shortTimeout);

                pqmTask.Status = Status.Passed;
            });

            //var tempScriptDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "TopCatScript"));
            //File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "CustomPrintSettings.tcx"),
            //    ScriptResource.CustomPrintSettings);
            //File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "printPreferences.tcc"),
            //    ScriptResource.printPreferences);

            //_configureScript = new TopCatScript
            //(
            //    Path.Combine(tempScriptDirectory.FullName, "CustomPrintSettings.tcx"),
            //    "CustomPrintSettings"
            //);

            //_configureScript.Properties.Add
            //(
            //    "QueueName",
            //    _defaultPrintQueue.FullName
            //);
            //_configureScript.Properties.Add
            //(
            //    "Copies",
            //    pqmTask.Preference.Copies.ToString(System.Globalization.CultureInfo.InvariantCulture)
            //);
            //_configureScript.Properties.Add
            //(
            //    "PaperSize",
            //    pqmTask.Preference.PaperSize
            //);
            //_configureScript.Properties.Add
            //(
            //    "PaperSource",
            //    pqmTask.Preference.InputTray
            //);
            //_configureScript.Properties.Add
            //(
            //    "Orientation",
            //    pqmTask.Preference.Orientation
            //);
            //_configureScript.Properties.Add
            //(
            //    "DuplexValue",
            //    pqmTask.Preference.DuplexValue
            //);
            //_configureScript.Properties.Add
            //(
            //    "Duplexer",
            //    pqmTask.Preference.Duplexer
            //);
            //_configureScript.Properties.Add
            //(
            //    "DriverModel",
            //    _localPrintQueueInfo.PrintDriver.DriverName
            //);
            //TopCatExecutionController tcExecutionController = new TopCatExecutionController(_configureScript);
            //tcExecutionController.InstallPrerequisites(GlobalSettings.Items["TopCatSetup"]);
            //tcExecutionController.ExecuteTopCatTest();

            //string resultFile = tcExecutionController.GetResultFilePath(GlobalSettings.Items["DomainAdminUserName"]);
            //if (!string.IsNullOrEmpty(resultFile))
            //{
            //    XDocument resultDoc = XDocument.Load(resultFile);
            //    var successTests = resultDoc.Descendants("SuccessfulTests").First().Descendants("test");
            //    pqmTask.Status = successTests.Any() ? Status.Passed : Status.Failed;
            //}
            //else
            //{
            //    pqmTask.Status = Status.Failed;
            //}


            var token = new LocalLockToken(_defaultPrintQueue.FullName, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
            ExecutionServices.CriticalSection.Run(token, action);
        }

        /// <summary>
        /// gets the driver info such as name, print processor
        /// </summary>
        /// <param name="printerName">the print queue name</param>
        /// <returns>info about print driver</returns>
        private static string GetDriverInfo(string printerName)
        {
            StringBuilder queueProperty = new StringBuilder();
            string query = $"SELECT * from Win32_Printer WHERE Name LIKE '%{printerName}'";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                ManagementObjectCollection coll = searcher.Get();

                foreach (ManagementObject printer in coll)
                {
                    foreach (PropertyData property in printer.Properties)
                    {
                        if (property.Name.Equals("DriverName"))
                        {
                            queueProperty.Append(property.Value);
                        }
                        if (property.Name.Equals("PrintProcessor"))
                        {
                            queueProperty.AppendFormat(CultureInfo.InvariantCulture, ", {0}", property.Value);
                        }
                    }
                }
            }
            return queueProperty.ToString();
        }

        /// <summary>
        /// Performs any teardown activities for plugin which implements this interface
        /// </summary>
        public void Teardown()
        {
            var action = new Action(() =>
            {
                _configureScript = new TopCatScript(Path.Combine(Path.GetTempPath(), "TopCatScript", "CustomPrintSettings.tcx"), "CustomPrintSettings");
                TopCatExecutionController topCatExecutionController = new TopCatExecutionController(_configureScript);
                topCatExecutionController.Cleanup();
            });

            var token = new LocalLockToken(_defaultPrintQueue.FullName, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
            ExecutionServices.CriticalSection.Run(token, action);
        }


    }


}