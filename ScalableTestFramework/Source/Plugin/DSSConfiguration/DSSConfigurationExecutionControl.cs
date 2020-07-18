using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using TopCat.TestApi.GUIAutomation;

namespace HP.ScalableTest.Plugin.DSSConfiguration
{
    /// <summary>
    /// Used to execute the activity of the DSSConfiguration plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class DssConfigurationExecutionControl : UserControl, IPluginExecutionEngine
    {
        private DssConfigurationActivityData _activityData;

        /// <summary>
        /// Initializes a new instance of the DSSConfigurationExecutionControl class.
        /// </summary>
        public DssConfigurationExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute the task of the DSSConfiguration activity.
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _activityData = executionData.GetMetadata<DssConfigurationActivityData>();
            TopCatUIAutomation.Initialize();
            UpdateStatus($"Launching the DSS Configuration Task : {_activityData.TaskName}");
            try
            {
                List<Object> parameterList = new List<object>();
                DssConfigurationTask dssConfigurationTask = new DssConfigurationTask(executionData.SessionId, executionData.Credential.UserName);
                var method = typeof(DssConfigurationTask).GetMethod(_activityData.TaskName);
                foreach (var parameterInfo in method.GetParameters())
                {
                    object data;
                    _activityData.ParameterValueDictionary.TryGetValue(parameterInfo.ParameterType.Name, out data);
                    parameterList.Add(data);
                }

                method.Invoke(dssConfigurationTask, parameterList.ToArray());
            }
            catch (Exception exception)
            {
                return new PluginExecutionResult(PluginResult.Failed, exception.InnerException ?? exception);
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        /// <summary>
        /// Updates the status text in the execution control display.
        /// </summary>
        /// <param name="statusMsg"></param>
        protected virtual void UpdateStatus(string statusMsg)
        {
            statusRichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now:MM/dd/yyyy HH:mm:ss.fff}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });
        }
    }
}