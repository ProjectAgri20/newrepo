using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Plugin.TwainDriverConfiguration.UIMaps;
using HP.ScalableTest.Utility;
using TopCat.TestApi.GUIAutomation;
using TopCat.TestApi.GUIAutomation.Enums;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.TwainDriverConfiguration
{
    /// <summary>
    /// Execution control for the TwainDriverConfiguration plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class TwainDriverConfigurationExecutionControl : UserControl, IPluginExecutionEngine
    {
        private TwainDriverActivityData _activityData;
        private PluginExecutionData _executionData;
        private const int ShortTimeout = 5;
        private const int WindowTimeout = 40;
        private DeviceWorkflowLogger _workflowLogger;
        private List<IDeviceInfo> _deviceAssets;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwainDriverConfigurationExecutionControl"/> class.
        /// </summary>
        public TwainDriverConfigurationExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Entrypoint for the Plugin Task Execution for TwainDriverConfiguration Plugin
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            var acquireTimeout = TimeSpan.FromMinutes(5);
            var holdTimeout = TimeSpan.FromMinutes(5);
            _executionData = executionData;
            _deviceAssets = executionData.Assets.OfType<IDeviceInfo>().ToList();
            try
            {
                _workflowLogger = new DeviceWorkflowLogger(executionData);
                UpdateStatus("Starting task engine");
                List<AssetLockToken> tokens = _deviceAssets.Select(n => new AssetLockToken(n, acquireTimeout, holdTimeout)).ToList();
                _workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                ExecutionServices.CriticalSection.Run(tokens, selectedToken =>
                {
                    IDeviceInfo deviceInfo = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;

                    UpdateStatus($"Using device {deviceInfo.AssetId} ({deviceInfo.Address})");
                    using (IDevice device = DeviceConstructor.Create(deviceInfo))
                    {
                        var retryManager = new PluginRetryManager(executionData, UpdateStatus);
                        result = retryManager.Run(() => RunTwain(device,executionData));
                    }
                });

                _workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
            }
            catch (DeviceCommunicationException ex)
            {
                return new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device communication error.");
            }
            catch (DeviceInvalidOperationException ex)
            {
                return new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device automation error.");
            }
            catch (DeviceWorkflowException ex)
            {
                return new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
            }

            return result;
        }

        private PluginExecutionResult RunTwain(IDevice device,PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            _activityData = _executionData.GetMetadata<TwainDriverActivityData>();
            var preparationManager = DevicePreparationManagerFactory.Create(device);
            preparationManager.InitializeDevice(true);
            TopCatUIAutomation.Initialize();

            if (_activityData.TwainOperation == TwainOperation.Install)
            {
                InstallTwainDriver(device);
            }
            else if (_activityData.TwainOperation == TwainOperation.DeviceAddition)
            {
                DeviceAddition(device);
            }
            else
            {
                ConfigureSettings(device,executionData.SessionId, executionData.Credential.UserName);
            }

            preparationManager.Reset();
            return result;
        }

        private void InstallTwainDriver(IDevice device)
        {
            string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"HP\HP Scan Twain\bin"), "HPScan.exe");
            if (File.Exists(path))
            {
                UpdateStatus("Twain Driver is already installed");
            }
            else
            {
                if (!File.Exists(_activityData.SetupFileName))
                {
                    throw new Exception("Could not find the setup file");
                }
                else
                {
                    UpdateStatus("Starting setup");
                    //Install Twain Driver
                    Process process = new Process
                    {
                        StartInfo =
                        {
                            FileName = "msiexec.exe",
                            Arguments = $"/qb /i \"{_activityData.SetupFileName}\" ALLUSERS=1",
                            WindowStyle = ProcessWindowStyle.Hidden
                        }
                    };
                    process.Start();
                    UpdateStatus("Installing Twain Driver");
                    process.WaitForExit();
                    UpdateStatus("Installation Completed");
                    //By default the Device addition Pop Up Appears
                    DeviceAddition(device);
                }
            }
        }

        private void DeviceAddition(IDevice device)
        {
            // Get the common desktop directory
            var shortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
                "HP TWAIN Device Selection Tool.lnk");
            if (File.Exists(shortcut))
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(shortcut)
                {
                    UseShellExecute = true,
                    Verb = "runas"
                };

                Process.Start(processInfo);
                Thread.Sleep(TimeSpan.FromSeconds(ShortTimeout));
            }
            else
            {
                throw new Exception("Desktop shortcut not found");
            }
            UpdateStatus("Adding Device");
            HPTwainDevice deviceSelection = new HPTwainDevice(UIAFramework.ManagedUIA);
            HPTwainDevicePopUp popUp = new HPTwainDevicePopUp(UIAFramework.ManagedUIA);


            //deviceSelection.
            deviceSelection.Edit289Edit.WaitForAvailable(WindowTimeout);
            deviceSelection.Edit289Edit.PerformHumanAction(x => x.EnterText(device.Address, WindowTimeout));
            deviceSelection.SearchButton291Button.WaitForEnabled(WindowTimeout);
            deviceSelection.SearchButton291Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
            if (deviceSelection.HPLaserJetMFPM5List.IsAvailable(WindowTimeout))
            {
                deviceSelection.HPLaserJetMFPM5List.ClickWithMouse(MouseButton.Left, ShortTimeout);
            }
            else
            {
                throw new Exception("Device not found");
            }

            if (deviceSelection.ApplyButton2539Button.IsEnabled(WindowTimeout))
            {
                deviceSelection.ApplyButton2539Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                popUp.CloseButton1164Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                deviceSelection.OKButton1839Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                UpdateStatus("Device Added");
            }
            else
            {
                deviceSelection.CancelButton112Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                UpdateStatus("Same device is already selected.");
            }
        }

        private void ConfigureSettings(IDevice device,string sessionId,string userName)
        {
            UpdateStatus("Configuring the Settings");
            bool isReservation = TwainDriverConfigurationTask.ConfigureSettings(device, _activityData);
            if (_activityData.TwainConfigurations != TwainConfiguration.NewScanShortCut)
            {
                if (isReservation)
                {
                    UpdateStatus("Performing Device end operation for Reservation");
                    TwainDriverDeviceOperation.ExecuteJob(device, _activityData);
                }
                TwainDriverConfigurationTask.ScanOperation(_activityData,sessionId,userName);
            }
        }

        /// <summary>
        /// Updates the status text in the execution control display.
        /// </summary>
        /// <param name="statusMsg"></param>
        protected virtual void UpdateStatus(string statusMsg)
        {
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });
        }
    }
}
