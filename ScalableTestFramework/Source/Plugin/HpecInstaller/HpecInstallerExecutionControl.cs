using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HP.ScalableTest.Plugin.HpecInstaller
{
    /// <summary>
    /// A class that implements the execution portion of the plug-in.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="IPluginExecutionEngine"/> interface.
    ///
    /// <seealso cref="IPluginExecutionEngine"/>
    /// </remarks>
    public partial class HpecInstallerExecutionControl : UserControl, IPluginExecutionEngine
    {
        private HpecInstallerActivityData _data;
        private NetworkCredential _credential;
        private ServerInfo _serverInfo;
        private string _deviceListFile;
        private string _sessionId;

        private const string FdtFileName = @"C:\Program Files (x86)\HP\Embedded Capture Installer\FDTCommandLine.Exe";

        private const string BoundaryAes = "--Boundary93504AC0-71EE-49dd-AEB3-D3EF6D251ABE------";
        // BOUNDARY for AES Encryption without a user generated password.
        private const string BoundaryAeshk = "--Boundary93504AC0-71EE-49dd-AEB3-D3EF6D251BEA------";
        private const string SaltString = "A71CA3C4B6D1B9C410E75E8BE8F2465A";

        private readonly StringBuilder _logText = new StringBuilder();
        private readonly TimeSpan _retryTimeSpan = TimeSpan.FromSeconds(10);
        /// <summary>
        /// constructor
        /// </summary>
        public HpecInstallerExecutionControl()
        {
            InitializeComponent();
        }

        #region IPluginExecutionEngine implementation

        /// <summary>
        /// Executes this plug-in's workflow using the specified <see cref="PluginExecutionData"/>.
        /// </summary>
        /// <remarks>
        ///
        /// <seealso cref="PluginExecutionData"/>
        /// <seealso cref="PluginExecutionResult"/>
        /// <seealso cref="PluginResult"/>
        /// </remarks>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult"/> indicating the outcome of the
        /// execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _data = executionData.GetMetadata<HpecInstallerActivityData>();
            _credential = executionData.Credential;
            _serverInfo = executionData.Servers.FirstOrDefault();
            _sessionId = executionData.SessionId;
            var printer = executionData.Assets.OfType<PrintDeviceInfo>().FirstOrDefault();
            UpdateStatus("Creating Device List File.");
            _deviceListFile = CreateDeviceList(printer);
            UpdateStatus($"Created Device List File on remote server at: {_deviceListFile}");

            switch (_data.InstallerAction)
            {
                case HpecInstallerAction.InstallHpec:
                    InstallHpec();
                    break;

                case HpecInstallerAction.UninstallHpec:
                    UninstallHpec();
                    break;

                case HpecInstallerAction.UploadWorkflow:
                    UploadWorkFlow();
                    break;

                case HpecInstallerAction.RemoveWorkflow:
                    RemoveWorkFlow();
                    break;
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        protected virtual void UpdateStatus(string text)
        {
            status_RichTextBox.InvokeIfRequired(c =>
                {
                    ExecutionServices.SystemTrace.LogInfo(text);
                    _logText.Clear();
                    _logText.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                    _logText.Append("  ");
                    _logText.AppendLine(text);
                    status_RichTextBox.AppendText(_logText.ToString());
                    status_RichTextBox.Refresh();
                }
            );
        }

        private void RemoveWorkFlow()
        {
            UpdateStatus("Removing Workflows from device.");
            string taskListFile = string.Empty;
            var removeDoc = XDocument.Parse(string.Format(Properties.Resources.RemoveWorkFlow, _data.ServerVersion));
            string scheduledTaskFolder = $@"\\{_serverInfo.Address}\c$\Program Files (x86)\HP\Embedded Capture Installer\scheduledTaskFiles";
            scheduledTaskFolder = Path.Combine(scheduledTaskFolder, _sessionId);
            UserImpersonator.Execute(() => taskListFile = CreateRemoteXmlFile(scheduledTaskFolder, "tasklist.xml", removeDoc), new NetworkCredential(_credential.UserName, _credential.Password, _credential.Domain));
            UpdateStatus($"Created Task File on remote server at {taskListFile}");
            UpdateStatus("Remote Task for removing workflow scheduled, and will run in 10 seconds.");
            var removeTask = InsertRemoteTask("Remove_HPEC", FdtFileName, $"\"{_deviceListFile}\" \"{taskListFile}\"");

            if (!Retry.UntilTrue(() => IsTaskComplete(removeTask), 10, _retryTimeSpan))
            {
                UpdateStatus("Remove Task did not complete successfully. Please check server for further information.");
                throw new Exception("Remove WorkFlow Failed");
            }
            UpdateStatus("Workflow removed successfully.");

            RemoveRemoteTask(removeTask);
            UpdateStatus("Scheduled Task removed successfully.");
        }

        private void UploadWorkFlow()
        {
            string taskListFile = string.Empty;
            string workFlowfile = string.Empty;
            //copy the workflow file to the remote location
            UpdateStatus("Uploading Workflow to device.");
            var creds = new NetworkCredential(_credential.UserName, _credential.Password, _credential.Domain);
            string scheduledTaskFolder = $@"\\{_serverInfo.Address}\c$\Program Files (x86)\HP\Embedded Capture Installer\scheduledTaskFiles";
            scheduledTaskFolder = Path.Combine(scheduledTaskFolder, _sessionId);
            var workFlowStream = File.OpenRead(_data.WorkflowFile);
            UserImpersonator.Execute(() => workFlowfile = CreateRemoteFile(scheduledTaskFolder, Path.GetFileName(_data.WorkflowFile), workFlowStream), creds);
            UpdateStatus($"Created Workflow File on remote server at {workFlowfile}");
            var workFlowDoc = XDocument.Parse(string.Format(Properties.Resources.UploadWorkFlow, workFlowfile, _data.ServerVersion));
            UserImpersonator.Execute(() => taskListFile = CreateRemoteXmlFile(scheduledTaskFolder, "tasklist.xml", workFlowDoc), creds);
            UpdateStatus($"Created Task File on remote server at {taskListFile}");
            UpdateStatus("Remote Task for Uploading workflow scheduled, and will run in 10 seconds.");
            var configureTask = InsertRemoteTask("Configure_HPEC", FdtFileName, $"\"{_deviceListFile}\" \"{taskListFile}\"");
            workFlowStream.Dispose();

            if (!Retry.UntilTrue(() => IsTaskComplete(configureTask), 10, _retryTimeSpan))
            {
                UpdateStatus("Upload Workflow Task did not complete successfully. Please check server for further information.");
                throw new Exception("Uninstallation Failed");
            }
            UpdateStatus("Workflow uploaded successfully.");
            RemoveRemoteTask(configureTask);
            UpdateStatus("Scheduled Task removed successfully.");
        }

        private void UninstallHpec()
        {
            string taskListFile = string.Empty;
            var uninstallDoc = XDocument.Parse(string.Format(Properties.Resources.UninstallHpec, _data.ServerVersion));
            UpdateStatus("Uninstalling HPEC from device.");
            string scheduledTaskFolder = $@"\\{_serverInfo.Address}\c$\Program Files (x86)\HP\Embedded Capture Installer\scheduledTaskFiles";
            scheduledTaskFolder = Path.Combine(scheduledTaskFolder, _sessionId);
            UserImpersonator.Execute(() => taskListFile = CreateRemoteXmlFile(scheduledTaskFolder, "tasklist.xml", uninstallDoc), new NetworkCredential(_credential.UserName, _credential.Password, _credential.Domain));
            UpdateStatus($"Created Task File on remote server at {taskListFile}");
            UpdateStatus("Remote Task for Uninstalling HPEC, and will run in 10 seconds.");
            var uninstallTask = InsertRemoteTask("Uninstall_HPEC", FdtFileName, $"\"{_deviceListFile}\" \"{taskListFile}\"");

            if (!Retry.UntilTrue(() => IsTaskComplete(uninstallTask), 30, _retryTimeSpan))
            {
                UpdateStatus("Uninstall Task did not complete successfully. Please check server for further information.");
                throw new Exception("Uninstallation Failed");
            }
            UpdateStatus("HPEC Uninstalled successfully.");
            RemoveRemoteTask(uninstallTask);
            UpdateStatus("Scheduled Task removed successfully.");
        }

        private void InstallHpec()
        {
            string taskListFileName = string.Empty;
            var installDoc = XDocument.Parse(string.Format(Properties.Resources.InstallHpec, _data.ServerVersion));
            UpdateStatus("Installing HPEC from device.");
            string scheduledTaskFolder = $@"\\{_serverInfo.Address}\c$\Program Files (x86)\HP\Embedded Capture Installer\scheduledTaskFiles";
            scheduledTaskFolder = Path.Combine(scheduledTaskFolder, _sessionId);
            UserImpersonator.Execute(() => taskListFileName = CreateRemoteXmlFile(scheduledTaskFolder, "taskList.xml", installDoc), new NetworkCredential(_credential.UserName, _credential.Password, _credential.Domain));
            UpdateStatus($"Created Task File on remote server at {taskListFileName}");
            UpdateStatus("Remote Task for Installing HPEC, and will run in 10 seconds.");
            var installTask = InsertRemoteTask("Install_HPEC", FdtFileName, $"\"{_deviceListFile}\" \"{taskListFileName}\"");

            if (!Retry.UntilTrue(() => IsTaskComplete(installTask), 30, _retryTimeSpan))
            {
                UpdateStatus("Install Task did not complete successfully. Please check server for further information.");
                throw new Exception("Installation Failed");
            }
            UpdateStatus("HPEC Installed successfully.");
            RemoveRemoteTask(installTask);
            UpdateStatus("Scheduled Task removed successfully.");
        }

        private Task InsertRemoteTask(string taskName, string command, string arguments)
        {
            Task remoteTask;
            using (TaskService ts = new TaskService(_serverInfo.Address, _credential.UserName, _credential.Domain, _credential.Password))
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = taskName;
                td.Principal.UserId = "LOCALSERVICE";
                td.Principal.LogonType = TaskLogonType.ServiceAccount;

                td.Triggers.Add(new TimeTrigger(DateTime.Now + TimeSpan.FromSeconds(10)));
                td.Actions.Add(new ExecAction(command, arguments, Path.GetDirectoryName(command)));
                remoteTask = ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
            Thread.Sleep(TimeSpan.FromSeconds(15));
            UpdateStatus("Remote task has started.");
            return remoteTask;
        }

        private void RemoveRemoteTask(Task task)
        {
            using (TaskService ts = new TaskService(_serverInfo.Address, _credential.UserName, _credential.Domain, _credential.Password))
            {
                var targetTask = ts.GetTask(task.Path);
                ts.RootFolder.DeleteTask(targetTask.Name);
            }
        }

        private bool IsTaskComplete(Task task)
        {
            using (TaskService ts = new TaskService(_serverInfo.Address, _credential.UserName, _credential.Domain,
                _credential.Password))
            {
                var targetTask = ts.AllTasks.FirstOrDefault(x => x.Name == task.Name);
                return targetTask?.LastTaskResult == 0;
            }
        }

        private string CreateDeviceList(PrintDeviceInfo deviceInfo)
        {
            string activityUrn = "urn:hp:imaging:con:service:systemconfiguration:SystemConfigurationService";
            string endpoint = "systemconfiguration";
            string deviceListFileName = string.Empty;

            var jediDevice = new JediDevice(deviceInfo.Address, deviceInfo.AdminPassword);

            var systemConfigurationTicket = jediDevice.WebServices.GetDeviceTicket(endpoint, activityUrn);

            var deviceListDoc = XDocument.Parse(Properties.Resources.DeviceList);

            deviceListDoc.Element("DeviceList")?.Element("Name")?.SetValue($"DL#{DateTime.Now:HHmmdd}");

            var deviceGuid = Guid.NewGuid();
            var deviceElement = deviceListDoc.Element("DeviceList")?.Element("Devices")?.Elements("Device")
                .FirstOrDefault();
            if (deviceElement != null)
            {
                deviceElement.Attribute("guid")?.SetValue(deviceGuid);
                deviceElement.Attribute("deviceId")?
                    .SetValue(
                        $"{systemConfigurationTicket.FindElement("MakeAndModel").Value}#{systemConfigurationTicket.FindElement("SerialNumber").Value}");
                deviceElement.Attribute("ipAddress")?.SetValue(deviceInfo.Address);

                //we need to enter hashed password here, so let's do that later
                string hashedPassword = EncryptStringByAes(deviceInfo.AdminPassword, SaltString, BoundaryAeshk);
                deviceElement.Element("Password")?.SetValue($"{hashedPassword}");
                deviceElement.Element("Model")?.SetValue(systemConfigurationTicket.FindElement("MakeAndModel").Value);

                deviceElement.Element("Hostname")?.SetValue(HostName(jediDevice));
                deviceElement.Element("FactoryDefaultHostname")?.SetValue(HostName(jediDevice));

                deviceElement.Element("MacAddress")?.SetValue(MacId(jediDevice));
                deviceElement.Element("FwVersion")?.SetValue(jediDevice.GetDeviceInfo().FirmwareRevision);
                var fwDateTimeString = jediDevice.GetDeviceInfo().FirmwareDateCode;
                string format = "yyyyMMdd";
                DateTime fwDateTime = DateTime.ParseExact(fwDateTimeString, format, CultureInfo.InvariantCulture);
                deviceElement.Element("FwDate")?.SetValue(fwDateTime.ToString("s"));
                deviceElement.Element("Ram")?.SetValue(systemConfigurationTicket.FindElement("MemoryInstalled").Value);
                deviceElement.Element("SerialNumber")?.SetValue(systemConfigurationTicket.FindElement("SerialNumber").Value);
            }

            string scheduledTaskFolder = $@"\\{_serverInfo.Address}\c$\Program Files (x86)\HP\Embedded Capture Installer\scheduledTaskFiles";
            scheduledTaskFolder = Path.Combine(scheduledTaskFolder, _sessionId);
            UserImpersonator.Execute(() => deviceListFileName = CreateRemoteXmlFile(scheduledTaskFolder, "deviceList.xml", deviceListDoc), new NetworkCredential(_credential.UserName, _credential.Password, _credential.Domain));

            return deviceListFileName;
        }

        private string MacId(JediDevice jediDevice)
        {
            var macIdRawValue = jediDevice.Snmp.GetRaw("1.3.6.1.4.1.11.2.4.3.1.23.0");
            var macIdBytes = macIdRawValue.ToBytes();
            var macids = macIdBytes.Select(x => $"{Convert.ToInt32(x):X}");

            return string.Join(":", macids);
        }

        private string HostName(JediDevice jediDevice)
        {
            return jediDevice.Snmp.Get(".1.3.6.1.4.1.2699.1.2.1.2.1.1.2.1");
        }

        private string CreateRemoteXmlFile(string directory, string filename, XDocument xDoc)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            if (!directoryInfo.Parent.Exists)
                Directory.CreateDirectory(directoryInfo.Parent.FullName);
            
            var dirInfo = Directory.CreateDirectory(directory);
            string saveFileName = Path.Combine(dirInfo.FullName, filename);
            using (var fileStream = File.Create(saveFileName))
            {
                xDoc.Save(fileStream);
                fileStream.Flush(true);
            }

            return MakeDiskRootFromUncRoot(saveFileName);
        }

        private string CreateRemoteFile(string directory, string filename, FileStream inputFileStream)
        {
            var dirInfo = Directory.CreateDirectory(directory);
            string saveFileName = Path.Combine(dirInfo.FullName, filename);
            using (var fileStream = File.Create(saveFileName))
            {
                inputFileStream.CopyTo(fileStream);
                fileStream.Flush(true);
            }

            return MakeDiskRootFromUncRoot(saveFileName);
        }

        private static string MakeDiskRootFromUncRoot(string astrPath)
        {
            string strPath = astrPath;
            if (strPath.StartsWith("\\\\"))
            {
                strPath = strPath.Substring(2);
                int ind = strPath.IndexOf('$');
                if (ind > 1 && strPath.Length >= 2)
                {
                    string driveLetter = strPath.Substring(ind - 1, 1);
                    strPath = strPath.Substring(ind + 1);
                    strPath = driveLetter + ":" + strPath;
                }
            }
            return strPath;
        }

        private string EncryptStringByAeshk(string value)
        {
            return EncryptStringByAes(value, SaltString, BoundaryAeshk);
        }

        private string EncryptStringByAes(string value, string password, string boundary)
        {
            if (!string.IsNullOrEmpty(password)) //If password is set, enconde with AES encryption
            {
                if (value.Contains(BoundaryAes)) { return value; } //String is AES Encrypted

                SymmetricAlgorithm key;

                byte[] ivtext = { 164, 251, 9, 203, 165, 152, 52, 250, 163, 253, 19, 223, 185, 162, 54, 210 };
                byte[] passwordBytes = new byte[32]; //Force Password to be 32 bytes
                Encoding.UTF8.GetBytes(password).CopyTo(passwordBytes, 0);
                key = new AesCryptoServiceProvider
                {
                    Key = passwordBytes,
                    IV = ivtext
                };

                UTF8Encoding encoding = new UTF8Encoding();

                byte[] encBytes = encoding.GetBytes(value);

                ICryptoTransform criptoTransform = key.CreateEncryptor();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, criptoTransform, CryptoStreamMode.Write);

                cryptoStream.Write(encBytes, 0, encBytes.Length);
                cryptoStream.FlushFinalBlock();

                byte[] encrypted = memoryStream.ToArray();
                string strEncrypted = Convert.ToBase64String(encrypted);

                return string.Concat(boundary, strEncrypted);
            }

            return string.Empty;
        }

        #endregion IPluginExecutionEngine implementation
    }
}