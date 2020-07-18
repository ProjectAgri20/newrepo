using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Developer
{
    public partial class DeveloperPluginExecControl : UserControl, IPluginExecutionEngine
    {
        private readonly Random _random = new Random();

        public DeveloperPluginExecControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            Log("Starting activity.");
            DeveloperPluginActivityData activityData = executionData.GetMetadata<DeveloperPluginActivityData>();

            if (string.IsNullOrEmpty(sessionIdTextBox.Text))
            {
                sessionIdTextBox.InvokeIfRequired(n => n.Text = executionData.SessionId);
            }

            ProcessAssets(executionData.Assets);
            ProcessDocuments(executionData.Documents);
            ProcessPrintQueues(executionData.PrintQueues);

            try
            {
                return ProcessResult(activityData);
            }
            finally
            {
                Log($"Finished activity.{Environment.NewLine}");
            }
        }

        private void ProcessAssets(AssetInfoCollection assets)
        {
            Log($"Found {assets.Count} assets: {string.Join(", ", assets.Select(n => n.AssetId))}");
        }

        private void ProcessDocuments(DocumentCollection documents)
        {
            Log($"Found {documents.Count} documents: {string.Join(", ", documents.Select(n => n.FileName))}");
        }

        private void ProcessPrintQueues(PrintQueueInfoCollection printQueues)
        {
            Log($"Found {printQueues.Count} print queues: {string.Join(", ", printQueues.Select(n => n.QueueName))}");
        }

        private PluginExecutionResult ProcessResult(DeveloperPluginActivityData activityData)
        {
            DeveloperPluginResult selectedResult = activityData.Result;
            Log($"Specified outcome: {selectedResult}");

            if (selectedResult == DeveloperPluginResult.ThrowException)
            {
                throw new Exception("Intentional exception.");
            }

            if (selectedResult == DeveloperPluginResult.Random)
            {
                selectedResult = (DeveloperPluginResult)_random.Next(1, 5);
            }

            PluginResult pluginResult = EnumUtil.Parse<PluginResult>(selectedResult.ToString());
            string message = (pluginResult == PluginResult.Passed ? string.Empty : $"{pluginResult} deliberately.");
            Log($"Execution result: {pluginResult}");

            return new PluginExecutionResult(pluginResult, message);
        }

        private void Log(string message)
        {
            ExecutionServices.SystemTrace.LogDebug(message);

            outputTextBox.InvokeIfRequired(n =>
            {
                n.AppendText($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {message}{Environment.NewLine}");
                n.Select(n.Text.Length, 0);
                n.ScrollToCaret();
            });
        }
    }
}
