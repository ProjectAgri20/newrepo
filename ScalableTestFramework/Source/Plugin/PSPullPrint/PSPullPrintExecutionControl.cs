using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.PSPullPrint
{
    [ToolboxItem(false)]
    public partial class PSPullPrintExecutionControl : UserControl, IPluginExecutionEngine
    {
        public PSPullPrintExecutionControl()
        {
            InitializeComponent();
        }

        private PSPullPrintActivityData _activityData;

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _activityData = executionData.GetMetadata<PSPullPrintActivityData>();

            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

            var printDeviceInfo = (PrintDeviceInfo)executionData.Assets.First();
            var printer = DeviceConstructor.Create(printDeviceInfo);

            LockToken assetToken = new AssetLockToken(executionData.Assets.First(), _activityData.LockTimeouts ?? new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10)));
            ExecutionServices.CriticalSection.Run(assetToken, () =>
            {
                if (printer is JediWindjammerDevice)
                {
                    result = ExecuteJedi(printer, executionData.Credential);
                }
                else if (printer is SiriusUIv3Device)
                {
                    result = ExecuteSiriusTriptane(printer, executionData.Credential);
                }
                else if (printer is SiriusDevice)
                {
                    result = ExecuteSirius(printer, executionData.Credential);
                }
                else
                {
                    result = new PluginExecutionResult(PluginResult.Skipped, "Activity not supported on this device");
                }
            });

            return result;
        }

        private PluginExecutionResult ExecuteSiriusTriptane(IDevice device, NetworkCredential credential)
        {
            if (_activityData.PullPrintingSolution.Equals((PullPrintingSolution.HPAC)))
            {
                try
                {
                    //HPAC for Triptane
                    HpacSiriusTriptane hpacTriptane = new HpacSiriusTriptane(device, credential);
                    UpdateStatus("Signing in user and Navigating to HPAC");
                    //Navigate to HPAC Screen
                    hpacTriptane.NavigateToHpac();
                    UpdateStatus("User signed in, HPAC Launched");
                    foreach (var pmtasks in _activityData.PullPrintTasks)
                    {
                        UpdateStatus($"Executing {pmtasks.Description}");
                        //Performing the specified tasks in the Control panel
                        hpacTriptane.PerformHpacTasksOnCp(pmtasks.Operation);
                        UpdateStatus($"Waiting for {_activityData.ActivityPacing.TotalSeconds} seconds");
                        Thread.Sleep(_activityData.ActivityPacing);
                    }
                }
                catch (Exception ex)
                {
                    return new PluginExecutionResult(PluginResult.Failed, ex.Message);
                }
            }
            else if (_activityData.PullPrintingSolution.Equals((PullPrintingSolution.SafeCom)))
            {
                try
                {
                    //HPAC for Triptane
                    SafecomSiriusTriptane safecomTriptane = new SafecomSiriusTriptane(device, credential);
                    UpdateStatus("Signing in user and Navigating to HPAC");
                    //Navigate to HPAC Screen
                    safecomTriptane.AuthenticateSafecom(_activityData.SafecomPin);
                    UpdateStatus("User signed in, HPAC Launched");
                    foreach (var pmtasks in _activityData.PullPrintTasks)
                    {
                        UpdateStatus($"Executing {pmtasks.Description}");
                        //Performing the specified tasks in the Control panel
                        safecomTriptane.DoSafecomAction(pmtasks.Operation);
                        UpdateStatus($"Waiting for {_activityData.ActivityPacing.TotalSeconds} seconds");
                        Thread.Sleep(_activityData.ActivityPacing);
                    }

                }
                catch (Exception ex)
                {
                    return new PluginExecutionResult(PluginResult.Failed, ex.Message);
                }
            }
            UpdateStatus("All activities executed successfully");
            return new PluginExecutionResult(PluginResult.Passed);
        }

        private PluginExecutionResult ExecuteSirius(IDevice device, NetworkCredential credential)
        {
            if (_activityData.PullPrintingSolution.Equals((PullPrintingSolution.HPAC)))
            {
                try
                {
                    //HPAC for Sirius
                    HpacSirius hpacSirius = new HpacSirius(device, credential);
                    UpdateStatus("Signing in user and Navigating to HPAC");
                    //Navigate to HPAC Screen
                    hpacSirius.NavigateToHpac();
                    UpdateStatus("User signed in, HPAC Launched");
                    foreach (var pmtasks in _activityData.PullPrintTasks)
                    {
                        UpdateStatus($"Executing {pmtasks.Description}");
                        //Performing the specified tasks in the Control panel
                        hpacSirius.PerformHpacTasksOnCp(pmtasks.Operation);
                        UpdateStatus($"Waiting for {_activityData.ActivityPacing.TotalSeconds} seconds");
                        Thread.Sleep(_activityData.ActivityPacing);
                    }
                }
                catch (Exception ex)
                {
                    return new PluginExecutionResult(PluginResult.Failed, ex.Message);
                }
            }
            else
            {
                //Safecom for Sirius
                SafecomSirius safecomSirius = new SafecomSirius(device, credential);
                //Creating the sirius device for getting the control panel access
                try
                {
                    UpdateStatus("Signing in user and Navigating to Safecom");
                    safecomSirius.AuthenticateSafecom(_activityData.SafecomPin);
                    UpdateStatus("User signed in, Safecom Launched");
                    foreach (var pmtasks in _activityData.PullPrintTasks)
                    {
                        UpdateStatus($"Executing {pmtasks.Description}");
                        //Performing the specified tasks in the Control panel
                        safecomSirius.DoSafecomAction(pmtasks.Operation);
                        UpdateStatus($"Waiting for {_activityData.ActivityPacing.TotalSeconds} seconds");
                        Thread.Sleep(_activityData.ActivityPacing);
                    }
                }
                catch (Exception ex)
                {
                    return new PluginExecutionResult(PluginResult.Failed, ex.Message);
                }
            }
            UpdateStatus("All activities executed successfully");
            return new PluginExecutionResult(PluginResult.Passed);
        }

        private PluginExecutionResult ExecuteJedi(IDevice device, NetworkCredential credential)
        {
            if (_activityData.PullPrintingSolution.Equals((PullPrintingSolution.HPAC)))
            {
                //HPAC for Jedi
                HpacJedi hpacJedi = new HpacJedi(device, credential);

                try
                {
                    //Navigate to HPAC Screen
                    UpdateStatus("Signing in user and Navigating to HPAC");
                    hpacJedi.NavigateToJediHpac();
                    UpdateStatus("User signed in, HPAC Launched");
                    foreach (var pmtasks in _activityData.PullPrintTasks)
                    {
                        UpdateStatus($"Executing {pmtasks.Description}");
                        //Performing the specified tasks in the Control panel
                        hpacJedi.PerformJediHpacTasksOnCp(pmtasks.Operation);
                        UpdateStatus($"Waiting for {_activityData.ActivityPacing.TotalSeconds} seconds");
                        Thread.Sleep(_activityData.ActivityPacing);
                    }
                }
                catch (Exception ex)
                {
                    return new PluginExecutionResult(PluginResult.Failed, ex.Message);
                }
            }
            else
            {
                //Safecom for Jedi
                SafecomJedi safecomJedi = new SafecomJedi(device, credential);
                //Creating the sirius device for getting the control panel access
                try
                {
                    UpdateStatus("Signing in user and Navigating to Safecom");
                    safecomJedi.AuthenticateSafecom(string.Empty);
                    UpdateStatus("User signed in, Safecom Launched");
                }
                catch (Exception ex)
                {
                    return new PluginExecutionResult(PluginResult.Failed, ex.Message);
                }
            }
            UpdateStatus("All activities executed successfully");
            return new PluginExecutionResult(PluginResult.Passed);
        }

        protected virtual void UpdateStatus(string statusMsg)
        {
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });
            Application.DoEvents();
        }
    }
}