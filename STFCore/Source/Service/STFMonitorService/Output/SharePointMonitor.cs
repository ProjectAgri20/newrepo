using System;
using System.Net;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using HP.ScalableTest.FileAnalysis;
using HP.ScalableTest.SharePoint;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.DataLog;

namespace HP.ScalableTest.Service.Monitor.Output
{
    /// <summary>
    /// Monitors a SharePoint document library for digital send output files.
    /// </summary>
    public class SharePointMonitor : OutputMonitorBase
    {
        private SharePointDocumentLibrary _library;
        private SharePointDocumentQuery _query;
        private string _tempPath;
        private string _fileExtension = "pdf";

        private Timer _timer;
        private TimeSpan _timerInterval = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Gets the location to monitor.
        /// </summary>
        public override string MonitorLocation
        {
            get { return Configuration.MonitorLocation; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointMonitor"/> class.
        /// </summary>
        /// <param name="monitorConfig">The monitor configuration.</param>
        public SharePointMonitor(MonitorConfig monitorConfig) 
            : base(monitorConfig)
        {
            _library = new SharePointDocumentLibrary(MonitorLocation);
            _timer = new Timer(new TimerCallback(TimerFire));

            // Set up a temporary path and make sure it exists
            _tempPath = Path.Combine(Path.GetTempPath(), "SharePointMonitor", _library.Name);
            System.IO.Directory.CreateDirectory(_tempPath);

            // Initialize the document criteria
            _query = new SharePointDocumentQuery()
            {
                DocumentLimit = 50,
                CreationDelay = TimeSpan.FromMinutes(2)
            };
        }

        private void TimerFire(object notUsed)
        {
            _timer.Change(-1, -1);
            ProcessExisting();
            _timer.Change(_timerInterval, _timerInterval);
        }

        /// <summary>
        /// Processes existing files in the destination being monitored.
        /// </summary>
        private void ProcessExisting()
        {
            SharePointDocumentCollection documents = new SharePointDocumentCollection();
            SharePointDocumentCollection retrieved = null;
            int retrievedCount = -1;
            // Download the documents in batches, to avoid overloading the server
            do
            {

                // Retrieve document information and add it to the master list
                retrievedCount = -1; //Reset the counter
                try
                {
                    retrieved = _library.Retrieve(_query);
                    retrievedCount = retrieved.Count;
                }
                catch (WebException webEx)
                {
                    TraceFactory.Logger.Error(webEx);
                    return; //Unable to connect to the Sharepoint server.
                }
                catch (Win32Exception winEx)
                {
                    TraceFactory.Logger.Error(winEx);
                    return; //Unable to connect to the Sharepoint server.
                }

                // Download each document
                foreach (SharePointDocument document in retrieved)
                {
                    try
                    {
                        _library.Download(document, _tempPath);
                        documents.Add(document);
                    }
                    catch (WebException webEx)
                    {
                        LogDownloadError(document.FileName, webEx);
                    }
                    catch (NotSupportedException ex)
                    {
                        LogDownloadError(document.FileName, ex);
                    }
                }

                // Remove the documents from the server so we don't pick them up again
                try
                {
                    _library.Delete(retrieved);
                }
                catch (WebException webEx)
                {
                    TraceFactory.Logger.Error("Error removing downloaded files from Sharepoint.  Cleaning up.", webEx);
                    CleanupUnprocessedFiles(documents);
                    return;
                }


                // Repeat until we get a partial batch, which means we have downloaded
                // the remainder of the documents on the server.
            } while (retrievedCount == _query.DocumentLimit);

            TraceFactory.Logger.Debug("Downloaded {0} documents from {1}.".FormatWith(documents.Count, _library.Name));

            // Process the documents.  Note that this step must occur after all the documents are downloaded
            // so that any metadata files will be present in the directory with their corresponding documents.
            var selectedDocuments = documents.Where(n => n.FileName.EndsWith(_fileExtension, StringComparison.OrdinalIgnoreCase));
            foreach (SharePointDocument document in selectedDocuments)
            {
                ProcessDocument(document, Path.Combine(_tempPath, document.FileName));
            }
        }

        private static void LogDownloadError(string fileUrl, Exception ex)
        {
            TraceFactory.Logger.Error("Error downloading {0} from Sharepoint.".FormatWith(fileUrl), ex);
        }

        private void ProcessDocument(SharePointDocument document, string filePath)
        {
            TraceFactory.Logger.Debug("Found file: " + document.FileName);

            try
            {
                string fileName = Path.GetFileName(filePath);
                ScanFilePrefix filePrefix = ScanFilePrefix.Parse(fileName);

                // Create the log for this file
                DigitalSendJobOutputLogger log = new DigitalSendJobOutputLogger(fileName, filePrefix.ToString(), filePrefix.SessionId);
                log.FileSentDateTime = document.Created.LocalDateTime;
                log.FileLocation = $@"{_library.SiteUrl.Host}\{_library.Name}";

                // Validate and analyze the file
                OutputProcessor processor = new OutputProcessor(filePath);
                ValidationResult result = null;
                Retry.WhileThrowing(
                    () => result = processor.Validate(Configuration),
                    10,
                    TimeSpan.FromSeconds(2),
                    new List<Type>() { typeof(IOException) });

                // If the validation failed, there is a small chance that the HPS file arrived
                // a little later than the PDF and didn't get downloaded at the same time.
                // Check the server to see if that's the case - if so, run the validation again
                if (!result.Succeeded && result.Message.Contains("metadata", StringComparison.OrdinalIgnoreCase))
                {
                    if (FindMetadataFile(document))
                    {
                        result = processor.Validate(Configuration);
                    }
                }

                DocumentProperties properties = processor.GetProperties();
                log.FileSizeBytes = properties.FileSize;
                log.PageCount = properties.Pages;
                log.SetErrorMessage(result);

                // Clean up the file
                processor.ApplyRetention(Configuration, result.Succeeded);

                // Send the output log
                new DataLogger(GetLogServiceHost(filePrefix.SessionId)).Submit(log);
            }
            catch (IOException ex)
            {
                LogProcessFileError(filePath, ex);
            }
            catch (FormatException ex)
            {
                LogProcessFileError(filePath, ex);
            }
        }

        private bool FindMetadataFile(SharePointDocument document)
        {
            string fileName = Path.GetFileNameWithoutExtension(document.FileName);
            SharePointDocumentQuery criteria = new SharePointDocumentQuery();
            criteria.FileName = fileName;
            var retrieved = _library.Retrieve(criteria);

            if (retrieved.Any())
            {
                foreach (SharePointDocument doc in retrieved)
                {
                    _library.Download(doc, _tempPath);
                }
                _library.Delete(retrieved);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void LogProcessFileError(string fileName, Exception ex)
        {
            TraceFactory.Logger.Error("{0} could not be processed.".FormatWith(fileName), ex);
        }

        private void CleanupUnprocessedFiles(SharePointDocumentCollection documents)
        {
            foreach (SharePointDocument document in documents)
            {
                try
                {
                    File.Delete(Path.Combine(_tempPath, document.FileName));
                }
                catch (IOException ex)
                {
                    TraceFactory.Logger.Error(ex);
                }
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void StartMonitoring()
        {
            ProcessExisting();
            _timer.Change(_timerInterval, _timerInterval);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void StopMonitoring()
        {
            _timer.Change(-1, -1);
        }

        /// <summary>
        /// String representation of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"SharePoint Monitor for {_library.SiteUrl}";

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        #endregion
    }
}
