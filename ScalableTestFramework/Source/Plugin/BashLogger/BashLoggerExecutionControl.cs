using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.BashLogger.BashLog;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.BashLogger
{
    /// <summary>
    /// A class that implements the execution portion of the plug-in.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="IPluginExecutionEngine"/> interface.
    ///
    /// <seealso cref="IPluginExecutionEngine"/>
    /// </remarks>
    [ToolboxItem(false)]
    public partial class BashLoggerExecutionControl : UserControl, IPluginExecutionEngine
    {
        private BashLoggerActivityData _data;
        private BashLogCollectorClient _client;
        private readonly ConcurrentDictionary<string, string> _bashLogConcurrentDictionary = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// constructor
        /// </summary>
        public BashLoggerExecutionControl()
        {
            InitializeComponent();
        }

        #region IPluginExecutionEngine implementation

        /// <summary>
        ///Execution point for the plugin
        /// <seealso cref="PluginExecutionData"/>
        /// <seealso cref="PluginExecutionResult"/>
        /// <seealso cref="PluginResult"/>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult"/> indicating the outcome of the
        /// execution.</returns>
        /// </summary>

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            var serviceHost = executionData.Environment.PluginSettings["BashLogCollectorServiceHost"];
            if (string.IsNullOrEmpty(serviceHost))
            {
                return new PluginExecutionResult(PluginResult.Error, "Bash Logger Service Host setting missing. Please enter the value in Plugin Settings");
            }

            _client = new BashLogCollectorClient(serviceHost);
            _data = executionData.GetMetadata<BashLoggerActivityData>();

            try
            {
                Parallel.ForEach(executionData.Assets.OfType<IDeviceInfo>(),
               asset =>
               {
                   var assetId = _client.CreateLogger(asset.AssetId);
                   if (string.IsNullOrEmpty(assetId))
                   {
                       ExecutionServices.SystemTrace.LogError(
                            $"Unable to Create Logger for {asset.AssetId}, Please verify the bash logger information for the device in Asset Inventory");
                       return;
                   }
                   switch (_data.LoggerAction)
                   {
                       case LoggerAction.Start:
                           _client.StartLogging(assetId);
                           UpdateStatus($"Logging Started for device: {asset.AssetId}");
                           break;

                       case LoggerAction.Stop:
                           _client.StopLogging(assetId);
                           UpdateStatus($"Logging Stopped for device: {asset.AssetId}");
                           break;

                       case LoggerAction.CollectLog:
                           {
                               UpdateStatus($"Collecting Logs for device: {asset.AssetId}");
                               var log = _client.CollectLog(assetId);
                               UpdateStatus($"Collection of Logs completed for device: {asset.AssetId}");
                               _client.Flush(assetId);
                               UpdateStatus($"Clearing the Logs for device: {asset.AssetId}");
                               _bashLogConcurrentDictionary.AddOrUpdate(assetId, log, (key, oldvalue) => oldvalue + log);
                           }
                           break;
                   }
               });
            }
            catch (Exception e)
            {
                return new PluginExecutionResult(PluginResult.Error, e.InnerException?.Message ?? e.Message);
            }

            if (_data.LoggerAction == LoggerAction.CollectLog)
            {
                foreach (var bashLogPair in _bashLogConcurrentDictionary)
                {
                    var fileSplitSize = _data.FileSplitSize * 1024 * 1024;
                    var numOfFiles = bashLogPair.Value.Length / fileSplitSize + 1;

                    if (!Directory.Exists(_data.FolderPath))
                    {
                        UpdateStatus("Given path doesn't exist, switching to temporary location");
                        _data.FolderPath = Path.GetTempPath();
                    }

                    for (int i = 0; i < numOfFiles; i++)
                    {
                        using (
                            var logFile =
                                File.Create(
                                    Path.Combine(_data.FolderPath,
                                        $"{bashLogPair.Key}_{executionData.SessionId}_BashLog_{i}.txt"), fileSplitSize,
                                    FileOptions.None))
                        {
                            byte[] buffer = new byte[fileSplitSize];

                            //complicated math below
                            //checks if the file split size is less than total file length, check for total log size is smaller than split size, check for last chunk file to be written to avoid null character entry
                            int charCount = fileSplitSize < bashLogPair.Value.Length
                                ? fileSplitSize < bashLogPair.Value.Length - i * fileSplitSize
                                    ? fileSplitSize
                                    : bashLogPair.Value.Length - i * fileSplitSize
                                : bashLogPair.Value.Length;
                            Encoding.ASCII.GetBytes(bashLogPair.Value, i * fileSplitSize, charCount, buffer,
                                buffer.GetLowerBound(0));
                            //write the log file
                            UpdateStatus($"Writing the log file: {logFile.Name}");
                            logFile.Write(buffer, 0, charCount);
                            logFile.Flush(true);
                        }
                    }
                }
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

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

        #endregion IPluginExecutionEngine implementation
    }
}