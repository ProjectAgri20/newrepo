using System;
using System.IO;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    /// <summary>
    /// Handles uploading files to Jet Advantage
    /// </summary>
    public class JetAdvantageUploadEngine
    {
        private JetAdvantageUploadActivityData _activityData = null;
        private string _jetAdvantageURL = string.Empty;
        private string _jetAdvantageProxy = string.Empty;

        /// <summary>
        /// Event for when the status changes for this controller.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetAdvantageUploadExecControl"/> class.
        /// </summary>
        public JetAdvantageUploadEngine()
        {
            Initialize();
        }

        public void Initialize()
        {

        }

        /// <summary>
        /// Update the status of the controller.
        /// </summary>
        /// <param name="message">New status message.</param>
        private void UpdateStatus(string message)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this, new StatusChangedEventArgs(message));
            }
        }

        /// <summary>
        /// Start the activity.
        /// </summary>
        /// <param name="metadata">Serialized activity data.</param>
        public PluginExecutionResult ProcessActivity(PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            UpdateStatus("Begin process activity...");

            try
            {
                _activityData = executionData.GetMetadata<JetAdvantageUploadActivityData>();

                _jetAdvantageProxy = _activityData.StackProxy;
                _jetAdvantageURL = _activityData.StackURL;

                UpdateStatus("Fetching document...");

                // Throw an error if for some reason there isn't anything to upload.
                if (executionData.Documents.Count == 0)
                {
                    throw new ArgumentException("The Document Set is empty, nothing to upload");
                }

                foreach (Document document in executionData.Documents)
                {
                    UpdateStatus(string.Format("Using base document {0}", document.FileName));

                    FileInfo fileInfo = ExecutionServices.FileRepository.GetFile(document);
                    UploadUniqueFile(fileInfo, executionData);
                }

                UpdateStatus("Activity processing completed");
            }
            catch (ThreadAbortException ex)
            {
                // Need to reset the abort.
                Thread.ResetAbort();
                UpdateStatus("Error: " + ex.ToString());
                result = new PluginExecutionResult(PluginResult.Failed, ex);
            }
            catch (Exception ex)
            {
                UpdateStatus("Error: " + ex.ToString());
                result = new PluginExecutionResult(PluginResult.Failed, ex);
            }
            ExecutionServices.SystemTrace.LogDebug("Exiting...");

            return result;
        }

        /// <summary>
        /// Uploads a unique (GUID appended) copy of the file to JetAdvantage
        /// Will throw an exception if the file is not confirmed at destination
        /// </summary>
        private void UploadUniqueFile(FileInfo fileInfo, PluginExecutionData data)
        {
            JetAdvantageUploadLog log = new JetAdvantageUploadLog(data);

            TitanAPI titanApi = new TitanAPI(_jetAdvantageProxy, _jetAdvantageURL);
            TitanUser user = new TitanUser(_activityData.LoginId, _activityData.LoginPassword);

            UniqueFile uniqueFile = UniqueFile.Create(fileInfo);
            UpdateStatus(string.Format("Created file {0}", uniqueFile.FullName));

            FileInfo fi = uniqueFile.FileInfo;

            DateTime jobStart = DateTime.Now;

            log.CompletionStatus = "Starting";
            log.DestinationUrl = _jetAdvantageProxy;
            log.FileName = uniqueFile.FullName;
            log.FileSentDateTime = jobStart;
            log.FileSizeSentBytes = fi.Length;
            log.FileType = fi.Extension;
            log.LoginId = user.EmailAddress;

            ExecutionServices.DataLogger.Submit(log);
            //bool success = titanApi.CheckJetAdvantageAvailability(_jetAdvantageURL, _jetAdvantageProxy);
            // test that the server is available by trying to access the destination
            var existingDocs = titanApi.GetPrintQueue(user);
            UpdateStatus(string.Format("Verified that we can access JetAdvantage servers {0}, {1}", _jetAdvantageProxy, _jetAdvantageURL));

            try
            {
                var receivedDoc = titanApi.UploadDocument(user, uniqueFile);
                DateTime jobEnd = DateTime.Now;

                UpdateStatus(string.Format("Uploaded file {0}", uniqueFile.FullName));

                log.CompletionStatus = "Success";
                log.FileSizeReceivedBytes = receivedDoc.size;
                log.FileReceivedDateTime = jobEnd;
                log.CompletionDateTime = DateTime.Now;
                UpdateStatus("Submitting log info.");
                ExecutionServices.DataLogger.Update(log);
                UpdateStatus("After submitting log info.");
            }
            catch (Exception ex)
            {
                log.CompletionStatus = "Failed";
                log.CompletionDateTime = System.DateTime.Now;

                ExecutionServices.DataLogger.Update(log);
                throw ex;
            }
        }
    }

}