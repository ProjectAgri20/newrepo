using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;

namespace Plugin.SdkGeneralExample
{
    /// <summary>
    /// Control used to perform and monitor device control panel execution of the OXPd PullPrint demo solution.
    /// </summary>
    public partial class SdkGeneralExampleExecutionControl : UserControl, IPluginExecutionEngine
    {
        private SdkGeneralExampleActivityData _data = null;

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public SdkGeneralExampleExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes the activity workflow.
        /// </summary>
        /// <param name="executionData">Information used in the execution of this workflow.</param>
        /// <returns>The result of executing the workflow.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Unknown");
            UpdateStatus("Starting execution");

            // Get the activity data specific to this plugin
            _data = executionData.GetMetadata<SdkGeneralExampleActivityData>();
            UpdateLabel(_data.Label);

            // Get virtual worker account information
            var userCredential = executionData.Credential;
            UpdateStatus($"Executing as user [{userCredential.UserName}] within the domain [{userCredential.Domain}]");

            // Get the plugin settings if applicable
            var settings = executionData.Environment.PluginSettings;
            if (settings != null)
            {
                UpdateStatus($"Found {settings.Count} plugin settings values");
                foreach(var key in settings.Keys)
                {
                    UpdateStatus($"... Setting key={key}, value={settings[key]}");
                }
            }

            // Demonstrate getting, locking and using an asset
            result = DemonstrateAssetLockAndUsage(executionData);
            if (result != null)
            {
                return result;
            }

            // Demonstrate getting and using a document
            result = DemonstrateDocumentUsage(executionData);
            if (result != null)
            {
                return result;
            }

            // return "passed" if we made it this far
            result = new PluginExecutionResult(PluginResult.Passed);

            return result;
        }

        /// <summary>
        /// Demonstrate document library usage.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>PluginExecutionResult.</returns>
        private PluginExecutionResult DemonstrateDocumentUsage(PluginExecutionData executionData)
        {
            PluginExecutionResult result = null;

            // Return "skipped" if no documents found
            if (executionData.Documents.Count == 0)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, "No documents specified", "No documents");
            }
            else
            {
                // List all available documents in the selection
                UpdateStatus("List all documents in the selection...");
                var enumerator = executionData.Documents.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var doc = enumerator.Current;
                    UpdateStatus($"Document {doc.FileName}, Pages={doc.Pages}, ColorMode={doc.ColorMode}");
                }
                enumerator.Reset();

                // Get a document at random from available
                UpdateStatus("Retrieving a document at random...");
                Document document = executionData.Documents.GetRandom();
                if (document != null)
                {
                    // Use the FileRepository execution service to retrieve the file from the document library and put a copy locally
                    FileInfo fileInfo = ExecutionServices.FileRepository.GetFile(document);
                    UpdateStatus($"Retrieved document at random: {fileInfo.FullName}, length={fileInfo.Length}");
                }
            }       
            return result;
        }

        /// <summary>
        /// Demonstrate asset locking and usage.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>PluginExecutionResult.</returns>
        private PluginExecutionResult DemonstrateAssetLockAndUsage(PluginExecutionData executionData)
        {
            PluginExecutionResult result = null;            

            // Get a random device from available
            IAssetInfo assetInfo = executionData.Assets.GetRandom();
            if (assetInfo != null)
            {
                try
                {
                    Action lockAction = () => LogDevice(assetInfo);
                    AssetLockToken lockToken = new AssetLockToken(assetInfo, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20));
                    UpdateStatus($"Acquiring lock token and executing for device {assetInfo.AssetId}");
                    ExecutionServices.CriticalSection.Run(lockToken, lockAction);
                }
                catch (AcquireLockTimeoutException)
                {
                    result = new PluginExecutionResult(PluginResult.Skipped, "Asset lock could not be acquired.", "Asset unavailable.");
                }
            }

            return result;
        }

        private void LogDevice(IAssetInfo asset)
        {
            ExecutionServices.SystemTrace.LogDebug($"Using device {asset.AssetId}");
        }

        private void UpdateLabel(string label)
        {
            Action displayText = new Action(() =>
            {
                textBoxActivity.Text = label;
            });

            if (textBoxActivity.InvokeRequired)
            {
                textBoxActivity.Invoke(displayText);
            }
            else
            {
                displayText();
            }
        }


        /// <summary>
        /// Logs and displays status messages.
        /// </summary>
        /// <param name="text">The status message to be logged and displayed.</param>
        protected virtual void UpdateStatus(string text)
        {
            Action displayText = new Action(() =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + text);
                var msg = $"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {text}\n";
                status_RichTextBox.AppendText(msg);
                status_RichTextBox.Refresh();
            });

            if (status_RichTextBox.InvokeRequired)
            {
                status_RichTextBox.Invoke(displayText);
            }
            else
            {
                displayText();
            }
        }
    }
}
