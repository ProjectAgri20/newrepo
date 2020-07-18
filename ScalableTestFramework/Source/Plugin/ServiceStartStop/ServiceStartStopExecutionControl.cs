using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ServiceStartStop
{
    /// <summary>
    /// Used to execute the activity of the ServiceStartStop plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ServiceStartStopExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PluginExecutionData _executionData = null;




        /// <summary>
        /// Initializes a new instance of the ServiceStartStopExecutionControl class.
        /// </summary>
        public ServiceStartStopExecutionControl()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Execute the task of the ServiceStartStop activity.
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _executionData = executionData;
            ServiceStartStopActivityData activityData = executionData.GetMetadata<ServiceStartStopActivityData>();

            return ServiceActionPerform(activityData.serv.Address, activityData.task, activityData.services, executionData.Environment);

        }

        /// <summary>
        /// Updates the status text in the execution control display.
        /// </summary>
        /// <param name="statusMsg"></param>
        protected virtual void UpdateStatus(string statusMsg)
        {
            status_TextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });
        }


        /// <summary>
        /// based on the task, either starts/stops/restarts the services @hostname
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="task"></param>
        /// <param name="services"></param>

        private PluginExecutionResult ServiceActionPerform(string hostAddress, int task, List<string> services, PluginEnvironment environment)
        {

            //Using Managemnent Scope. Instantiate the Win32_Service application and sned the appropriate command to the service.
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                //GlobalSettings.Items.DomainAdminCredential.UserName
                options.Username = environment.UserDomain + @"\" + environment.PluginSettings["DomainAdminUserName"];
                options.Password = environment.PluginSettings["DomainAdminPassword"];

                ManagementScope scope = new ManagementScope(@"\\" + hostAddress + @"\root\cimv2");
                scope.Options = options;
                scope.Connect();


                foreach (string service in services)
                {
                    ManagementPath path = new ManagementPath(string.Format("Win32_Service.Name='{0}'", service));
                    ManagementObject obj = new ManagementObject(scope, path, new ObjectGetOptions());
                    ManagementBaseObject outParams;


                    switch (task)
                    {
                        case 0:
                            outParams = obj.InvokeMethod("StopService", null, null);
                            break;
                        case 1:
                            outParams = obj.InvokeMethod("StartService", null, null);
                            break;
                        case 2:
                            outParams = obj.InvokeMethod("StopService", null, null);
                            System.Threading.Thread.Sleep(1000);
                            outParams = obj.InvokeMethod("StartService", null, null);

                            break;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                    uint returnCode = System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);

                    Logger.LogDebug($"Code Value; {returnCode}");
                    //If 0, success, if not, failed.
                    if (returnCode != 0)
                    {
                        throw new DeviceWorkflowException("Failed to successfuly modify service.");
                    }


                }
                return new PluginExecutionResult(PluginResult.Passed);
            }
            catch (Exception e)
            {
                if (e.Message == "The RPC server is unavailable. (Exception from HRESULT: 0x800706BA)")
                {
                    return new PluginExecutionResult(PluginResult.Failed, "Could Not Connect to Server");
                }
                else
                {
                    return new PluginExecutionResult(PluginResult.Failed, e);

                }
            }
        }
    }
}
