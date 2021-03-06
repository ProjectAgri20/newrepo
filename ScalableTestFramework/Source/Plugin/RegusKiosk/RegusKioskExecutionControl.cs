﻿using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.DeviceAutomation.LinkApps.Kiosk.RegusKiosk;
using HP.ScalableTest.Utility;
using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.RegusKiosk
{
    public partial class RegusKioskExecutionControl : UserControl, IPluginExecutionEngine
    {
        private RegusKioskActivityData _data = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="RegusKioskExecutionControl" /> class.
        /// </summary>
        public RegusKioskExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _data = executionData.GetMetadata<RegusKioskActivityData>();

            if (_data.JobType.Equals(RegusKioskJobType.Copy))
            {
                return ExecuteCopy(executionData);
            }
            else if (_data.JobType.Equals(RegusKioskJobType.Print))
            {
                return ExecutePrint(executionData);
            }
            else if (_data.JobType.Equals(RegusKioskJobType.Scan))
            {
                return ExecuteScan(executionData);
            }
            return new PluginExecutionResult(PluginResult.Failed, $"Unrecognized Connector Job Type: {_data.JobType}", ConnectorExceptionCategory.FalseAlarm.GetDescription());
        }

        /// <summary>
        /// Execute Copy Job.
        /// </summary>
        /// <returns></returns>
        public PluginExecutionResult ExecuteCopy(PluginExecutionData executionData)
        {
            using (RegusKioskCopyManager manager = new RegusKioskCopyManager(executionData, _data.CopyOptions, _data.LockTimeouts))
            {
                UpdateStatus("Starting execution to copy");
                manager.ActivityStatusChanged += UpdateStatus;
                manager.DeviceSelected += UpdateDevice;
                manager.AppNameSelected += UpdateJobType;
                return manager.RunLinkScanActivity();
            }
        }

        /// <summary>
        /// Execute Print Job.
        /// </summary>
        /// <returns></returns>
        public PluginExecutionResult ExecutePrint(PluginExecutionData executionData)
        {
            using (RegusKioskPrintManager manager = new RegusKioskPrintManager(executionData, _data.PrintOptions, _data.LockTimeouts))
            {
                UpdateStatus("Starting execution to print");
                manager.ActivityStatusChanged += UpdateStatus;
                manager.DeviceSelected += UpdateDevice;
                manager.AppNameSelected += UpdateJobType;
                return manager.RunLinkPrintActivity();
            }
        }

        /// <summary>
        /// Execute Scan Job.
        /// </summary>
        /// <returns></returns>
        public PluginExecutionResult ExecuteScan(PluginExecutionData executionData)
        {
            using (RegusKioskScanManager manager = new RegusKioskScanManager(executionData, _data.ScanOptions, _data.LockTimeouts))
            {
                UpdateStatus("Starting execution to scan");
                manager.ActivityStatusChanged += UpdateStatus;
                manager.DeviceSelected += UpdateDevice;
                manager.AppNameSelected += UpdateJobType;
                return manager.RunLinkScanActivity();
            }
        }

        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void UpdateStatus(string status)
        {
            string statusLine = $"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")} {status}";
            status_RichTextBox.InvokeIfRequired(n => n.AppendText(statusLine + Environment.NewLine));
        }

        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        protected void UpdateStatus(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }

        /// <summary>
        /// Updates the device displayed in the control.
        /// </summary>
        /// <param name="device">The device.</param>
        protected void UpdateDevice(string device)
        {
            activeDeviceLabel.InvokeIfRequired(n => n.Text = device);
        }

        /// <summary>
        /// Updates the device displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        protected void UpdateDevice(object sender, StatusChangedEventArgs e)
        {
            UpdateDevice(e.StatusMessage);
        }

        /// <summary>
        /// Updates the Cloud Type displayed in the control.
        /// </summary>
        /// <param name="appName">The cloud type.</param>
        protected void UpdateJobType(string jobType)
        {
            activejobTypelabel.InvokeIfRequired(n => n.Text = jobType);
        }

        /// <summary>
        /// Updates the Cloud Type displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        protected void UpdateJobType(object sender, StatusChangedEventArgs e)
        {
            UpdateJobType(e.StatusMessage);
        }
    }
}
