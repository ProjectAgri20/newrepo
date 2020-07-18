using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Service.Monitor.DigitalSend;

namespace HP.ScalableTest.Service.Monitor.AutoStore
{
    /// <summary>
    /// AutoStore Monitoring Service 
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Service.Monitor.StfMonitor" />
    public class AutoStoreMonitor : StfMonitor
    {
        /// <summary>
        /// Gets the monitor location.
        /// </summary>
        /// <value>
        /// The monitor location.
        /// </value>
        public override string MonitorLocation
        {
            get { return Configuration.MonitorLocation; }
        }

        /// <summary>
        /// The configuration needed to start an output monitor.
        /// </summary>
        protected StfMonitorConfig Configuration { get; private set; }

        private string _fileNamePath = string.Empty;
        private readonly string _autoStoreExePath;
        private readonly string _filePattern;
        private FileSystemWatcher _watcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoStoreMonitor"/> class.
        /// </summary>
        /// <param name="monitorConfig">The file name path.</param>
        public AutoStoreMonitor(MonitorConfig monitorConfig) : base(monitorConfig)
        {
            RefreshConfig(monitorConfig);
            _autoStoreExePath = @"C:\Program Files (x86)\Notable Solutions\AutoStore 7\APD.exe";
            
            _filePattern = "*.xml";

            _watcher = new FileSystemWatcher(MonitorLocation);
            _watcher.Filter = _filePattern;
            _watcher.Created += new FileSystemEventHandler(OnFileCreated);
        }
        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            // When the Created event fires, the file may still be writing to the disk
            // Wait a moment to let the file finish writing
            Thread.Sleep(TimeSpan.FromSeconds(1));

            ProcessFile(e.FullPath, DateTime.Now);
        }
        /// <summary>
        /// Processes existing files in the destination being monitored.
        /// </summary>
        private void ProcessExisting()
        {
            Console.WriteLine("Processing existing files in '{0}'.".FormatWith(Configuration.MonitorLocation));
            try
            {
                foreach (string file in System.IO.Directory.GetFiles(Configuration.MonitorLocation, _filePattern))
                {
                    ProcessFile(file);
                }
            }
            catch (IOException ex)
            {
                TraceFactory.Logger.Error(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                TraceFactory.Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Searches the AutoStore XML file for the DigitalSendServerJob required data
        /// </summary>
        protected virtual void ProcessFile(string filePath, DateTime? createdEventTime = null )
        {
            TraceFactory.Logger.Debug("Found file: {0}".FormatWith(filePath));

            try
            {
                XElement root = XElement.Load(filePath);
                IEnumerable<XElement> xmlData = from el in root.Elements(ParentNodes.JobData)
                                                select el;

                string autoStoreJobId = GetAttributeValue(xmlData, ChildNodes.JobId);
                Guid jobId = Guid.Parse(autoStoreJobId);

                DigitalSendServerJobLogger dssLog = new DigitalSendServerJobLogger(jobId);
                dssLog.ProcessedBy = GetAttributeValue(xmlData, ParentNodes.ServerData, ChildNodes.ServerName); 
                dssLog.JobType = dssLog.ProcessedBy + "-" + GetAttributeValue(xmlData, ChildNodes.FormName);

                dssLog.FileName = GetAttributeValue(xmlData, ParentNodes.FileData, ChildNodes.FileName);
                dssLog.FileSizeBytes = long.Parse(GetAttributeValue(xmlData, ParentNodes.FileData, ChildNodes.FileSize));
                dssLog.ScannedPages = short.Parse(GetAttributeValue(xmlData, ParentNodes.FileData, ChildNodes.PageCount));
                dssLog.FileType = GetAttributeValue(xmlData, ParentNodes.FileData, ChildNodes.FileExt);

                dssLog.DeviceModel = GetAttributeValue(xmlData,ParentNodes.DeviceData, ChildNodes.ModelProduct);
                string endingDT = GetAttributeValue(xmlData, ParentNodes.DeviceData, ChildNodes.DeviceTime);
                int offset = GetOffset(endingDT);

                string completionDateTime = GetJobTypeCompletionTime(xmlData, dssLog.JobType);


                dssLog.CompletionDateTime = ParseAutoStoreDateTime(completionDateTime).AddHours(offset); 
                dssLog.SessionId = GetSessionId(dssLog.FileName);

                dssLog.DssVersion = GetAutoStoreVersion();


                _fileNamePath = filePath;

                CreateDatalogEntry(dssLog);
                MoveAutoStoreXmlFile();
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

        /// <summary>
        /// Creates datalog entry from record information
        /// </summary>
        private void CreateDatalogEntry(DigitalSendServerJobLogger log)
        {
            TraceFactory.Logger.Debug("DSS Log Entry: SessionId={0}, JobId={1}, ProcessedBy={2}, DeviceModel={3}, FileName={4}, FileType={5}, ScannedPages={6}, FileSizeBytes={7}, CompletionDateTime={8}"
                                .FormatWith(log.SessionId, log.DigitalSendJobId.ToString(), log.ProcessedBy, log.DeviceModel, log.FileName, log.FileType, log.ScannedPages.ToString(), log.FileSizeBytes.ToString(), log.CompletionDateTime.ToString()));

            DataLogger dataLogger = new DataLogger(GlobalSettings.WcfHosts[WcfService.DataLog]);

            if (!string.IsNullOrEmpty(log.SessionId))
            {                
                log.CompletionStatus = "Success";                
                dataLogger.Submit(log);
            }
        }
        /// <summary>
        /// Gets the offset.
        /// 09:28:55-06:00
        /// </summary>
        /// <param name="deviceTime">The device time.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private int GetOffset(string deviceTime)
        {
            int offset = 0;

            if (!string.IsNullOrEmpty(deviceTime))
            {
                int startPos = deviceTime.IndexOf('-');
                int endPos = deviceTime.LastIndexOf(':');

                string value = deviceTime.Substring(startPos, endPos - startPos);

                offset = int.Parse(value);
            }

            return offset;
        }
        DateTime ParseAutoStoreDateTime(string strDt)
        {
            DateTime dt = DateTime.Parse("01/01/1800 12:00:00 AM");

            string[] values = strDt.Split(':');

            strDt = values[0] + "-" + values[1] + "-" + values[2] + "T" + FormatTimePortion(values[3]) + ":" + FormatTimePortion(values[4]) + ":" + FormatTimePortion(values[5]) + "." + values[6];

            TraceFactory.Logger.Debug("Re-formated CompletionDateTime = '{0}'".FormatWith(strDt));

            dt = DateTime.Parse(strDt);
            return dt;
        }

        private string FormatTimePortion(string timeValue)
        {
            string timePart = timeValue;

            if (timePart.Length == 1)
            {
                timePart = "0" + timeValue;
            }

            return timePart;
        }

        /// <summary>
        /// Moves the AutoStore XML file to the "Processed" folder. If the folder doesn't exist, will create.
        /// </summary>
        private void MoveAutoStoreXmlFile()
        {
            string moveToPath = Path.GetDirectoryName(_fileNamePath) + @"\Processed";
            string fileName = Path.GetFileName(_fileNamePath);

            System.IO.Directory.CreateDirectory(moveToPath);
            moveToPath += @"\" + fileName;

            TraceFactory.Logger.Debug("Move {0} to {1}".FormatWith(_fileNamePath, moveToPath));

            File.Move(_fileNamePath, moveToPath);
        }
        private string GetAutoStoreVersion()
        {
            string version = "Unknown";

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(_autoStoreExePath);
            if (versionInfo != null)
            {
                version = versionInfo.ProductVersion;
            }

            return version;
        }
        private string GetSessionId(string fileName)
        {
            string sessionId = string.Empty;
            string[] data = fileName.Split('-');

            if (data.Count() > 0)
            {
                sessionId = data[0];
            }

            return sessionId;
        }

        private string GetJobTypeCompletionTime(IEnumerable<XElement> xmlData, string jobType)
        {
            string parentElement = string.Empty;
            string childElement = string.Empty;

            if (jobType.Contains(AutoStoreJobType.Email))
            {
                parentElement = ChildNodes.EmailConnector;
                childElement = ChildNodes.EmailEndDt;

            }
            else if (jobType.Contains(AutoStoreJobType.Folder))
            {
                parentElement = ChildNodes.FolderConnector;
                childElement = ChildNodes.FolderEndDt;
            }
            else if (jobType.Contains(AutoStoreJobType.SharePoint))
            {
                parentElement = ChildNodes.SharePointConnector;
                childElement = ChildNodes.SharePointEndDt;
            }

            else if (jobType.Contains(AutoStoreJobType.LanFAX))
            {
                parentElement = ChildNodes.LanFaxConnector;
                childElement = ChildNodes.LanFaxEndDt;
            }

            return GetAttributeValue(xmlData, parentElement, childElement);
        }
        private string GetAttributeValue(IEnumerable<XElement> xmlData, string attributeName)
        {
            string attributeValue = string.Empty;

            var data = from n in xmlData.Elements(attributeName)
                       select n.Value;

            if (data.Count() > 0)
            {
                attributeValue = data.First();
            }

            return attributeValue;
        }
        private string GetAttributeValue(IEnumerable<XElement> xmlData, string parentElement, string childElement)
        {
            string attributeValue = string.Empty;

            var data = from n in xmlData.Elements(parentElement).Descendants(childElement)
                       select n.Value;

            if (data.Count() > 0)
            {
                attributeValue = data.First();
            }

            return attributeValue;
        }

        /// <summary>
        /// Starts this instance of AutoStore on the server.
        /// </summary>
        public override void StartMonitoring()
        {
            ProcessExisting();
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Stops this instance of the AutoStore Instance.
        /// </summary>
        public override void StopMonitoring()
        {
            _watcher.EnableRaisingEvents = false;
        }

        /// <summary>
        /// Refreshes monitor configuration for this instance.
        /// </summary>
        /// <param name="monitorConfig"></param>
        public override void RefreshConfig(MonitorConfig monitorConfig)
        {
            Configuration = LegacySerializer.DeserializeXml<StfMonitorConfig>(monitorConfig.Configuration);
        }

        private static void LogProcessFileError(string fileName, Exception ex)
        {
            TraceFactory.Logger.Error("{0} could not be processed.".FormatWith(fileName), ex);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Directory Monitor for {Configuration.MonitorLocation}";

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_watcher != null)
                {
                    _watcher.Dispose();
                    _watcher = null;
                }
            }
        }

        #endregion
    }


}
