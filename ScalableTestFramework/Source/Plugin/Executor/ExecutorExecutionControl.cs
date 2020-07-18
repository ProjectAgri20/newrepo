using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation;

namespace HP.ScalableTest.Plugin.Executor
{
    [ToolboxItem(false)]
    public partial class ExecutorExecutionControl : UserControl, IPluginExecutionEngine
    {
        private bool _setupDone = false;
        private Collection<Executable> _exectuables;
        private ExecutorActivityData _activityData;
        private readonly Collection<ExecutionResult> _executionResults;

        public ExecutorExecutionControl()
        {
            InitializeComponent();
            dataGridView_Results.AutoGenerateColumns = false;
            _executionResults = new Collection<ExecutionResult>();
        }

        /// <summary>
        /// Installs any dependent software via a DOS command file before executing the configured file list.
        /// </summary>
        /// <param name="executionData"></param>
        public void Setup(PluginExecutionData executionData)
        {
            _activityData = executionData?.GetMetadata<ExecutorActivityData>();

            if (!string.IsNullOrEmpty(_activityData?.SetupFileName))
            {
                SystemSetup.Run(_activityData.SetupFileName, string.Empty, executionData?.Credential, _activityData.CopyDirectory);
            }
        }

        /// <summary>
        /// Execute the plugin operation.
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            if (!_setupDone)
            {
                Setup(executionData);
                _setupDone = true;
            }

            _activityData = executionData?.GetMetadata<ExecutorActivityData>();
            _exectuables = _activityData?.Executables;
            TimeSpan timeout = TimeSpan.FromMinutes(5);

            if (_exectuables == null)
            {
                return new PluginExecutionResult(PluginResult.Skipped, "No files were configured to execute.");
            }

            foreach (Executable exe in _exectuables)
            {
                string finalArgument = exe.Arguments + (exe.PassSessionId ? $" {executionData?.SessionId}" : string.Empty);
                DateTime executionStart = DateTime.Now;

                ProcessExecutionResult result = ProcessUtil.Execute(exe.FilePath, finalArgument, timeout);

                if (result.SuccessfulExit)
                {
                    //Framework.ExecutionServices.SystemTrace.LogDebug($"Success. Output: {result.StandardOutput}");
                    RefreshGrid(new ExecutionResult { FileName = exe.FileName, Result = "Succeeded", ExecutionDateTime = executionStart });
                }
                else
                {
                    RefreshGrid(new ExecutionResult { FileName = exe.FileName, Result = "Failed", ExecutionDateTime = executionStart });

                    StringBuilder errorDescription = new StringBuilder("Error: ");
                    errorDescription.Append(GetProcessError(result, executionStart, timeout));
                    errorDescription.Append(Environment.NewLine);
                    errorDescription.Append("Output: ");
                    errorDescription.Append(result.StandardOutput);
                    errorDescription.Append(Environment.NewLine);
                    errorDescription.Append("File: ");
                    errorDescription.Append(exe.FileName);
                    Framework.ExecutionServices.SystemTrace.LogDebug(errorDescription.ToString());

                    return new PluginExecutionResult(PluginResult.Failed, errorDescription.ToString());
                }
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        private void RefreshGrid(ExecutionResult executionResult)
        {
            dataGridView_Results.Visible = false;
            dataGridView_Results.DataSource = null;
            _executionResults.Add(executionResult);
            dataGridView_Results.DataSource = _executionResults;
            dataGridView_Results.Refresh();
            dataGridView_Results.Visible = true;
        }

        /// <summary>
        /// ProcessExecutionResult does not indicate whether the process timed out.
        /// This method detects a timeout and returns a message inticating so.
        /// </summary>
        /// <param name="processResult">The ProcessExecutionResult from the execution attempt.</param>
        /// <param name="startDateTime">The Date/Time when the process was started.</param>
        /// <param name="timeout">The timeout TimeSpan.</param>
        /// <returns></returns>
        private string GetProcessError(ProcessExecutionResult processResult, DateTime startDateTime, TimeSpan timeout)
        {
            if (string.IsNullOrEmpty(processResult.StandardError))
            {
                //No standard error.  Check to see if it timed out.
                TimeSpan execTime = DateTime.Now - startDateTime;
                if (Math.Round(execTime.TotalSeconds, 2) >= Math.Round(timeout.TotalSeconds, 2))  //Round to the nearest second.
                {
                    return $"Process did not return within {timeout.TotalMinutes} minutes.";
                }
            }

            //Otherwise, just return the standard error as received.
            return processResult.StandardError;
        }
    }

    /// <summary>
    /// This class is a placeholder for result display, we cannot create a dictionary and databind to a datagrid,
    /// hence using this class
    /// </summary>

    public class ExecutionResult
    {
        /// <summary>
        /// the executable filename
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// the result of execution
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// The Date/Time of the execution.
        /// </summary>
        public DateTime ExecutionDateTime { get; set; }
    }
}
