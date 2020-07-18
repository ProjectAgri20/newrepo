using HP.DeviceAutomation;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// Configuration task for the HpacServerConfiguration plugin.
    /// </summary>
    public class HpacServerConfigurationTask
    {
        private HpacServerConfigurationController _hpacController = null;
        private HpacServerConfigurationActivityData _taskConfig = null;

        public NetworkCredential Networkcredential { get; private set; }

        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="HpacServerConfigurationTask" /> class.
        /// </summary>
        /// <param name="controller">controller.</param>
        /// <param name="activityData">HpacServer Configuration ActivityData.</param>
        public HpacServerConfigurationTask(HpacServerConfigurationController controller, HpacServerConfigurationActivityData activityData)
            : base()
        {
            _hpacController = controller;
            _taskConfig = activityData;
        }

        /// <summary>
        /// Processes the activity.
        /// </summary>
        public void Execute()
        {
            if (_taskConfig.HpacConfigTile == HpacTile.IRM)
            {
                if (_taskConfig.IRMData.IrmOperation == IrmOperation.GeneralSettings)
                {
                    ConfigureIrm();
                }
                else if (_taskConfig.IRMData.IrmOperation == IrmOperation.LDAPServerConfigure)
                {
                    ConfigureLdapIrm();
                }
                else if (_taskConfig.IRMData.IrmOperation == IrmOperation.CodeandorCardAttribute)
                {
                    if (_taskConfig.IRMData.AuthenticationMode == HpacAuthenticationMode.Card || _taskConfig.IRMData.AuthenticationMode == HpacAuthenticationMode.CodeAndCard || _taskConfig.IRMData.AuthenticationMode == HpacAuthenticationMode.CodeOrCard)
                    {
                        ConfigureCardAttribute();
                    }
                    if (_taskConfig.IRMData.AuthenticationMode == HpacAuthenticationMode.Code || _taskConfig.IRMData.AuthenticationMode == HpacAuthenticationMode.CodeAndCard || _taskConfig.IRMData.AuthenticationMode == HpacAuthenticationMode.CodeOrCard)
                    {
                        ConfigureCodeAttribute();
                    }
                }
            }
            else if (_taskConfig.HpacConfigTile == HpacTile.Devices)
            {
                var printdeviceinfo = _taskConfig.DeviceData.Asset;
                if (_taskConfig.DeviceData.DeviceOperation == DeviceOperation.Add)
                {
                    AddDevice(printdeviceinfo);
                }
                else if (_taskConfig.DeviceData.DeviceOperation == DeviceOperation.InstallHPAgent)
                {
                    InstallHPAgent(printdeviceinfo);
                }
                else
                {
                    UpdateStatus("Configuring HPAC agent on the device.");
                    ConfigureDevice((PrintDeviceInfo)printdeviceinfo, _taskConfig.DeviceData.Configuration);
                    UpdateStatus("Updating authenticators on the device.");
                    UpdateAuthenticators((PrintDeviceInfo)printdeviceinfo, _taskConfig.DeviceData.Authenticators);
                    UpdateStatus("Updated authenticators.");
                }
            }
            else if (_taskConfig.HpacConfigTile == HpacTile.PrintServer)
            {
                ConfigurePrintDevice();
            }
            else if (_taskConfig.HpacConfigTile == HpacTile.Settings)
            {
                if (_taskConfig.SettingsData.SettingsOperation == SettingsOperation.AddPrintQueue)
                {
                    AddQueue();
                }
                else if (_taskConfig.SettingsData.SettingsOperation == SettingsOperation.DeletePrintQueue)
                {
                    DeleteQueue();
                }
                else if (_taskConfig.SettingsData.SettingsOperation == SettingsOperation.Protocol)
                {
                    _hpacController.SelectProtocolOption(_taskConfig.SettingsData.ProtocolOptions);
                    _hpacController.EnableEncryption(_taskConfig.SettingsData.Encryption);
                    _hpacController.AddMultipleServer(_taskConfig.SettingsData.ServerURI);
                    _hpacController.AddServerToIIS(_taskConfig.SettingsData.ServerURI, _taskConfig.SettingsData.ServerIPAddress);
                }
                else
                {
                    QuotaSettings();
                }
            }
            else
            {
                if (_taskConfig.JobAccountingData.JobAccountingOperation == JobAccountingOperation.Quota)
                {
                    QuotaOperation();
                }
                else
                {
                    ScheduleReport();
                }
            }
        }

        private void CheckAndWaitForDeviceReboot(Framework.Assets.IDeviceInfo printdeviceinfo)
        {
            int maxRetries = 20;
            var device = DeviceFactory.Create(printdeviceinfo.Address, printdeviceinfo.AdminPassword);

            if (Retry.UntilTrue(() => HasDeviceRebooted(device), maxRetries / 2, TimeSpan.FromSeconds(10)))
            {
                UpdateStatus("Device has rebooted...");
                if (Retry.UntilTrue(() => IsDeviceRunning(device), maxRetries, TimeSpan.FromSeconds(10)))
                {
                    UpdateStatus("Device is in Running status...");
                    if (Retry.UntilTrue(() => IsJetDirectUp(device), maxRetries / 4, TimeSpan.FromSeconds(10)))
                    {
                        UpdateStatus("JetDirect Initialised...");
                    }

                    if (Retry.UntilTrue(() => IsWebServicesUp(device), (maxRetries / 4), TimeSpan.FromSeconds(10)))
                    {
                        UpdateStatus("Device is in Ready state.");
                    }
                }
            }
            else
            {
                UpdateStatus("Device has not rebooted.");
            }
        }

        private void AddDevice(Framework.Assets.IDeviceInfo device)
        {
            _hpacController.AddDevice((PrintDeviceInfo)device);
        }

        private void InstallHPAgent(Framework.Assets.IDeviceInfo device)
        {
            _hpacController.InstallAgent(device.Address);
        }

        private void QuotaOperation()
        {
            _hpacController.SetQuota(_taskConfig.JobAccountingData.Username);
        }

        private void ScheduleReport()
        {
            _hpacController.GetReport(_taskConfig.JobAccountingData.Username, _taskConfig.JobAccountingData.ReportEmailTo, _taskConfig.JobAccountingData.OutputFormat.ToString());
        }

        private void ConfigureDevice(PrintDeviceInfo printDeviceInfo, List<HpacConfiguration> configuration)
        {
            _hpacController.ConfigureDevice(printDeviceInfo, configuration);
            UpdateStatus("Configured the device on HPAC server.");
            //check if the device has rebooted and wait if it is so
            UpdateStatus("Checking if the device has rebooted due to deployment.");
            CheckAndWaitForDeviceReboot(printDeviceInfo);
            UpdateStatus("Checking the server for deployment status.");
            _hpacController.CheckForDeployment(printDeviceInfo);
            UpdateStatus("Deployment completed.");
        }

        private void UpdateAuthenticators(PrintDeviceInfo printDeviceInfo, List<HpacAuthenticators> authenticators)
        {
            _hpacController.UpdateAuthenticators(printDeviceInfo, authenticators);
        }

        private void ConfigureIrm()
        {
            _hpacController.ConfigureIrm(_taskConfig.IRMData.AuthenticationMode.ToString(), _taskConfig.IRMData.DataStorage.ToString());
        }

        private void ConfigureLdapIrm()
        {
            _hpacController.ConfigureLdapIrm(_taskConfig.IRMData.LDAPServer, _taskConfig.IRMData.LDAPServerUserName, _taskConfig.IRMData.LDAPServerPassword);
        }

        private void ConfigureCardAttribute()
        {
            _hpacController.ConfigureCardAttribute(_taskConfig.IRMData.IRMUserCardNumber);
        }

        private void ConfigureCodeAttribute()
        {
            _hpacController.ConfigureCodeAttribute(_taskConfig.IRMData.IRMUserCodeNumber);
        }

        private void UpdateStatus(string message)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }

        private void AddQueue()
        {
            _hpacController.AddQueue(_taskConfig.SettingsData.QueueName);
        }

        private void DeleteQueue()
        {
            _hpacController.DeleteQueue(_taskConfig.SettingsData.QueueName);
        }

        private void QuotaSettings()
        {
            if (_taskConfig.SettingsData.PurgedJobs)
            {
                _hpacController.EnablePurge(_taskConfig.SettingsData.PurgedJobs);
            }
            if (_taskConfig.SettingsData.EnableQuota)
            {
                _hpacController.EnableQuotaCopies(_taskConfig.SettingsData.EnableQuota);
            }
            foreach (var item in _taskConfig.SettingsData.Tracking)
            {
                if (item == SNMPTracking.Copies)
                {
                    _hpacController.EnableCopiesTracking(true);
                }
                else
                {
                    _hpacController.EnableDSTracking(true);
                }
            }
        }

        private void ConfigurePrintDevice()
        {
            _hpacController.ConfigurePrintServer(_taskConfig.PrintServerData.QueueName, _taskConfig.PrintServerData.Configuration);
        }

        private static bool HasDeviceRebooted(IDevice device)
        {
            var status = device.GetDeviceStatus();
            Application.DoEvents();

            return (status == DeviceStatus.None || status == DeviceStatus.Unknown);
        }

        private static bool IsDeviceRunning(IDevice device)
        {
            var status = device.GetDeviceStatus();
            Application.DoEvents();

            //the device can be in Down status (Error) if a jam door or front door is open and thus it is ignored and considered as "Running"
            return (status == DeviceStatus.Running || status == DeviceStatus.Warning || status == DeviceStatus.Down);
        }

        private bool IsJetDirectUp(IDevice device)
        {
            string bodyString;

            WebClient client = new WebClient();

            try
            {
                bodyString = client.DownloadString($"https://{device.Address}/hp/jetdirect");
                //while the device is still initialising it will hit 404 webexception and won't trigger this
            }
            catch (Exception)
            {
                bodyString = string.Empty;
            }

            return !string.IsNullOrEmpty(bodyString);
        }

        private bool IsWebServicesUp(IDevice device)
        {
            string bodyString;

            WebClient client = new WebClient();
            try
            {
                bodyString = client.DownloadString($"https://{device.Address}:7627/controlpanel");
                //while the device is still initialising it will hit 404 webexception and won't trigger this
                if (!bodyString.Contains("FimService"))
                {
                    bodyString = string.Empty;
                }
            }
            catch (Exception)
            {
                bodyString = string.Empty;
            }

            return !string.IsNullOrEmpty(bodyString);
        }
    }
}