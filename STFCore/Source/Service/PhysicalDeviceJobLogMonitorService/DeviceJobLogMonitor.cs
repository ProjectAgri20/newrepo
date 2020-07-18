using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using HP.Epr.WebServicesResponder;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    public class DeviceJobLogMonitor
    {
        private readonly string SIGNIFICAND = "Significand";

        private string _sessionId;
        private string _deviceId;
        private string _deviceIpAddress;
        private Guid _activityId;
        private Guid _transactionId;
        private List<string> _documentIds = new List<string>();
        private List<PhysicalDeviceJobLogger> _logData = new List<PhysicalDeviceJobLogger>();
        private readonly DataLogger _dataLogger;

        public event EventHandler<StatusChangedEventArgs> StatusChanged;        

        public DeviceJobLogMonitor(string sessionId, Guid activityId, Guid transactionId, string deviceId, string deviceIpAddress)
        {
            _activityId = activityId;
            _transactionId = transactionId;
            _sessionId = sessionId;
            _deviceId = deviceId;
            _deviceIpAddress = deviceIpAddress;
            _dataLogger = new DataLogger(GlobalSettings.WcfHosts[WcfService.DataLog]);
        }

        /// <summary>
        /// Monitors the device for job log entries pertaining to the requested document ids
        /// </summary>
        /// <param name="documentIdentifiers">The document identifiers.</param>
        /// <param name="role">The role.</param>
        /// <param name="password">The password.</param>
        /// <param name="minutesToMonitorBeforeTimeout">The minutes to monitor before timeout.</param>
        public void Monitor(List<string> documentIdentifiers, string role, string password, int minutesToMonitorBeforeTimeout = 30)
        {
            if (documentIdentifiers != null)
            {
                _documentIds = documentIdentifiers;
            }

            try
            {
                LogInitialEntries();
                QueryDeviceJobLog(role, password, minutesToMonitorBeforeTimeout);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error getting job log data from jobaccounting service, aborting monitoring", ex);
            }
        }

        private void LogInitialEntries()
        {
            foreach (var docId in _documentIds)
            {
                var logItem = CreateDataLogItem(docId);
                _logData.Add(logItem);
            }
        }

        private PhysicalDeviceJobLogger CreateDataLogItem(string docId)
        {
            var logItem = new PhysicalDeviceJobLogger()
            {
                SessionId = _sessionId,
                DeviceId = _deviceId,
                ActivityExecutionId = _transactionId,
                JobName = docId,
            };
            _dataLogger.Submit(logItem);
            return logItem;
        }

        /// <summary>
        /// Logs the entry via datalogger - either creating a new entry or updating an existing
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        private void LogOutput(string docId, JobLogItem logEntry)
        {
            PhysicalDeviceJobLogger logger = _logData.FirstOrDefault(x => x.JobName.Contains(docId, StringComparison.OrdinalIgnoreCase));
            if (logger == null)
            {
                // Create new entry if we don't already have one
                logger = CreateDataLogItem(docId);
                if (logEntry == null)
                {
                    return;
                }
            }

            if (logger != null &&  logEntry != null)
            {
                // Update the existing log entry with the new information
                logger.JobApplicationName = logEntry.ApplicationName;
                logger.JobCategory = logEntry.JobCategory;
                logger.JobEndStatus = logEntry.JobDoneStatus;
                logger.JobEndDateTime = Convert.ToDateTime(logEntry.JobDoneTimestamp);
                logger.JobId = logEntry.UUID;
                logger.JobName = logEntry.DeviceJobName;
                logger.JobStartDateTime = Convert.ToDateTime(logEntry.JobStartedTimestamp);
                logger.MonitorEndDateTime = DateTime.Now;
                _dataLogger.Update(logger);
            }
        }
        /// <summary>
        /// If the job name wasn't found and timeout threshold was exceeded update the monitor end field for the record.
        /// </summary>
        /// <param name="docId">string</param>
        private void LogFailedOutput(string docId)
        {
            try
            {
                PhysicalDeviceJobLogger logger = _logData.FirstOrDefault(x => x.JobName.Contains(docId, StringComparison.OrdinalIgnoreCase) && !x.MonitorEndDateTime.HasValue);
                if (logger != null)
                {
                    logger.MonitorEndDateTime = DateTime.Now;
                    _dataLogger.Update(logger);
                }
            }
            catch(Exception)
            {
                TraceFactory.Logger.Error("Failed to mark MonitorEnd time for {0}".FormatWith(docId));
            }
        }

        /// <summary>
        /// Queries the device job log.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="password">The password.</param>
        /// <param name="minutesToWait">The minutes to wait.</param>
        private void QueryDeviceJobLog(string role, string password, int minutesToWait)
        {
            var processedDocIds = new List<string>();
            var docIdsRemainingToProcess = new List<string>(_documentIds);
            DateTime endTime = DateTime.Now.AddMinutes(minutesToWait);

            while (docIdsRemainingToProcess.Any() && DateTime.Now < endTime)
            {
                TraceFactory.Logger.Debug("Getting log data from device {0}, session={1}".FormatWith(_deviceIpAddress, _sessionId));
                try
                {
                    var deviceJobLog = GetJobLog(role, password);
                    foreach (var docId in docIdsRemainingToProcess)
                    {
                        var matchingLogEntries = deviceJobLog.Where(x=>x.JobInfo.DeviceJobName.Contains(docId, StringComparison.OrdinalIgnoreCase)).ToList();
                        if (matchingLogEntries.Any())
                        {
                            TraceFactory.Logger.Debug("Logging data for {0}".FormatWith(docId));
                            // Add this doc id to the list of processed
                            processedDocIds.Add(docId);
                            var logEntry = matchingLogEntries.Last().JobInfo;
                            LogOutput(docId, logEntry);
                            OnStatusUpdate("Job entry for {0} found on device".FormatWith(logEntry.ToString()));
                        }
                    }
                }
                catch (System.TimeoutException)
                {
                    TraceFactory.Logger.Debug("JobAccounting service did not respond within default message timeout");
                }

                // adjust the list of remaining doc ids to process
                docIdsRemainingToProcess = docIdsRemainingToProcess.Except(processedDocIds).ToList();

                // if we still have more to process then go to sleep for a bit before we try again
                if (docIdsRemainingToProcess.Any())
                {
                    TraceFactory.Logger.Debug("{0} doc ids left to look for".FormatWith(docIdsRemainingToProcess.Count()));
                    Thread.Sleep(30000);
                }
                else
                {
                    TraceFactory.Logger.Debug("Found them all");
                }
            }

            if (docIdsRemainingToProcess.Any())
            {
                TraceFactory.Logger.Error("Timed out before all monitored jobs were found");
                foreach(var docId in docIdsRemainingToProcess)
                {
                    LogFailedOutput(docId);
                }
            }
            TraceFactory.Logger.Info("Finished monitoring");
        }
        /// <summary>
        /// Uses the retrieved job log to retrieve information on the following types
        /// of device jobs: JobInfo, HTTPInfo, PrintInfo, FolderInfo, and ScanInfo
        /// </summary>
        /// <param name="role">string</param>
        /// <param name="password">string</param>
        /// <returns>List<JobAccountingLogItem></returns>
        public List<JobAccountingLogItem> GetJobLog(string role, string password)
        {
            List<JobAccountingLogItem> JobAccountList = new List<JobAccountingLogItem>();
           
            var jobAccountingTable = GetJobLogAsElement(role, password);
            var jobAccountingData = GetElements(jobAccountingTable, "JobAccountingData");

            JobAccountingLogItem jali = null;

            foreach (XElement jad in jobAccountingData.Nodes())
            {
                XName name = jad.Name;

                switch(name.LocalName)
                {
                    case "JobInfo":
                        jali = AddJobAccountingItemToList(JobAccountList, jali);
                        GetJobInfoData(jali, jad);
                        break;
                    case "UserInfo": GetUserInfoData(jali, jad);
                        break;
                    case "HTTPInfo": GetHttpInfoData(jali, jad);
                        break;
                    case "PrintInfo": GetPrintInfoData(jali, jad);
                        break;
                    case "FolderInfoList": GetFolderInfoData(jali, jad);
                        break;
                    case "ScanInfo": GetScanInfoData(jali, jad);
                        break;
                    case "DriverInfo": GetDriverInfoData(jali, jad);
                        break;
                }
                
            }
            JobAccountList.Add(jali);

            return JobAccountList;
        }
        /// <summary>
        /// Retrieves the driver information of the job data
        /// </summary>
        /// <param name="jali">JobAccountingLogItem</param>
        /// <param name="jad">XElement</param>
        private void GetDriverInfoData(JobAccountingLogItem jali, XElement jad)
        {
            var clientInfo = GetElements(jad, "ClientInfo");
            var ApplicationInfo = GetElements(jad, "ApplicationInfo");

            // Some DriverInfo does not have any data other than a few paired values
            if (clientInfo.Count > 0 && ApplicationInfo.Count > 0)
            {
                var ciLog = clientInfo.Select(x => new DriverLogItem()
                    {
                        ClientInfo_Hostname = GetChildElementValue(x, "Hostname"),
                        ClientInfo_IPAddress = GetChildElementValue(x, "IPAddress")
                    }).First();
                var aiLog = ApplicationInfo.Select(y => new DriverLogItem()
                    {
                        ApplicationName = GetChildElementValue(y, "ApplicationName"),
                        FileName = GetChildElementValue(y, "FileName")
                    }).First();

                jali.DriverInfo = ciLog;
                jali.DriverInfo.ApplicationName = aiLog.ApplicationName;
                jali.DriverInfo.FileName = aiLog.FileName;
                jali.DriverInfo.JobUUID = GetChildElementValue(jad, "JobUUID");
            }
        }
        /// <summary>
        /// Retrieves the user information about the job data
        /// </summary>
        /// <param name="jali">JobAccountingLogItem</param>
        /// <param name="jad">XElement</param>
        private void GetUserInfoData(JobAccountingLogItem jali, XElement jad)
        {
            var ui = GetElementsSelf(jad, "UserInfo");

            var uiLog = ui.Select(x => new UserLogItem()
                {
                    AuthenticationAgentIdentifier = GetChildElementValue(x, "AuthenticationAgentIdentifier"),
                    AuthenticationAgentName = GetChildElementValue(x, "AuthenticationAgentName"),
                    AuthenticationAgentUUID = GetChildElementValue(x, "AuthenticationAgentUUID"),
                    AuthenticationCategory = GetChildElementValue(x, "AuthenticationCategory"),
                    
                    AuthorizationAgentName = GetChildElementValue(x, "AuthorizationAgentName"),
                    AuthorizationAgentUUID = GetChildElementValue(x, "AuthorizationAgentUUID"),
                    
                    Department = GetChildElementValue(x, "Department"),
                    DepartmentAccessCode = GetChildElementValue(x, "DepartmentAccessCode"),
                    DisplayName = GetChildElementValue(x, "DisplayName"),
                    
                    FullyQualifiedUserName = GetChildElementValue(x, "FullyQualifiedUserName"),
                    
                    UserDomain = GetChildElementValue(x, "UserDomain"),
                    UserName = GetChildElementValue(x, "UserName")
                }).First();

            jali.UserInfo = uiLog;
        }
        /// <summary>
        /// Retrieves the scan information for the Job log and stores in JobAccountingLogItem
        /// </summary>
        /// <param name="jali">JobAccountingLogItem</param>
        /// <param name="jad">XElement</param>
        private void GetScanInfoData(JobAccountingLogItem jali, XElement jad)
        {
            var counters = GetElementsFirst(jad, "Counter");
            var fss = GetElements(jad, "FirstScannedSheet");
            if (fss.Count > 0 && counters.Count > 0)
            {
                var fssLog = fss.Select(fs => new ScanLogItem()
                    {
                        MediaInputID = GetChildElementValue(fs, "MediaInputID"),
                        MediaSizeID = GetChildElementValue(fs, "MediaSizeID"),
                        Plex = GetChildElementValue(fs, "Plex"),
                    });

                jali.ScanInfo = fssLog.First();
                GetScanerInfoData(jali.ScanInfo, counters);
            }
        }
        /// <summary>
        /// Retrieves the values for the following elements of Scan Info job:
        /// Total, Simplex, Flatbed or ADF, sheets Total, Flatbed or ADF, images and
        /// stores in the given scan log item.
        /// </summary>
        /// <param name="sli">ScanLogItem</param>
        /// <param name="counters">List of XElement</param>
        private void GetScanerInfoData(ScanLogItem sli, List<XElement> counters)
        {
            // <Counter><CounterName /> <FixedPointNumbers /> </Counter>

            for(int idx = 0; idx < counters.Count; idx++)
            {           
                // Retrieve the nodes of CounterName and FixedPointNubmers
                var cnt = counters[idx].Elements();
                string name = cnt.First().Value;
                switch(name)
                {
                    case "TotalSheets": sli.TotalSheets = GetChildElementValue(cnt.Last(),SIGNIFICAND);
                        break;
                    case "SimplexSheets": sli.SimplexSheets = GetChildElementValue(cnt.Last(), SIGNIFICAND);
                        break;
                    case "FlatbedSheets": sli.FlatbedSheets = GetChildElementValue(cnt.Last(), SIGNIFICAND);
                        break;
                    case "FlatbedImages": sli.FlatbedImages = GetChildElementValue(cnt.Last(), SIGNIFICAND);
                        break;
                    case "TotalImages": sli.TotalImages = GetChildElementValue(cnt.Last(), SIGNIFICAND);
                        break;
                    case "ADFSheets": sli.ADFSheets = GetChildElementValue(cnt.Last(), SIGNIFICAND);
                        break;
                    case "ADFSimplexImages": sli.ADFSimplexImages = GetChildElementValue(cnt.Last(), SIGNIFICAND);
                        break;
                    case "ADFImages": sli.ADFImages = GetChildElementValue(cnt.Last(), SIGNIFICAND);
                        break;
                }
            }
        }
        /// <summary>
        /// Retrieves and adds the Folder information
        /// </summary>
        /// <param name="jali">JobAccountingLogItem</param>
        /// <param name="jad">XElement</param>
        private void GetFolderInfoData(JobAccountingLogItem jali, XElement jad)
        {
            var folder = GetElements(jad, "FolderInfo");
            var file = GetElements(folder.FirstOrDefault(), "File");

            if (folder.Count > 0 && file.Count > 0)
            {
                var folderLog = folder.Select(fld => new FolderLogItem()
                    {
                        UNCPath = GetChildElementValue(fld, "UNCPath"),
                        CompletionStatus = GetChildElementValue(fld, "CompletionStatus"),
                    });

                var fileLog = file.Select(f => new FolderLogItem()
                    {
                        Filename = GetChildElementValue(f, "FileName"),
                        DataSize = GetChildElementValue(f, "DataSize"),
                    });

                jali.FolderInfo = folderLog.First();
                jali.FolderInfo.Filename = fileLog.First().Filename;
                jali.FolderInfo.DataSize = fileLog.First().DataSize;
            }
        }
        /// <summary>
        /// Retrieves and adds the Print job information
        /// </summary>
        /// <param name="jali">JobAccountingLogItem</param>
        /// <param name="jad">XElement</param>
        private void GetPrintInfoData(JobAccountingLogItem jali, XElement jad)
        {
            var counters = GetPICounters(jad);
            var ps = GetElements(jad, "PrintSettings");
            var fps = GetElements(jad, "FirstPrintedSheet");

            if (fps.Count > 0 && ps.Count > 0)
            {
                var fpsLog = fps.Select(f => new PrintLogItem()
                    {
                        MediaSizeID = GetChildElementValue(f, "MediaSizeID"),
                        MediaTypeID = GetChildElementValue(f, "MediaTypeID"),
                        MediaInputID = GetChildElementValue(f, "MediaInputID"),
                        Plex = GetChildElementValue(f, "Plex"),
                    });

                jali.PrintInfo = fpsLog.First();
                jali.PrintInfo.PrintQuality = GetPrintQuality(ps);

                GetCounterData(jali.PrintInfo, counters);
            }
        }
        /// <summary>
        /// Retrieves the print quality for the give list of XElemnts. The list is comprised of only one XElement.
        /// </summary>
        /// <param name="ps">List of XElement</param>
        /// <returns>string</returns>
        private string GetPrintQuality(List<XElement> ps)
        {
            var psLog = ps.Select(p => new PrintLogItem()
            {
                PrintQuality = GetChildElementValue(p, "PrintQuality"),
            });

            return psLog.First().PrintQuality;
        }
        /// <summary>
        /// Retrieves the print counts from the list of XElements
        /// </summary>
        /// <param name="pli">PrintLogItem</param>
        /// <param name="counters">List of XElements</param>
        private void GetCounterData(PrintLogItem pli, List<XElement> counters)
        {
            for(int idx = 0; idx < counters.Count; idx++)
            {
                string name = counters[idx++].Value;
                switch(name)
                {
                    case "MonochromeImpressions": pli.MonochromeImpressions = GetChildElementValue(counters[idx], SIGNIFICAND);
                        break;
                    case "ColorImpressions": pli.ColorImpressions = GetChildElementValue(counters[idx], SIGNIFICAND);
                        break;
                    case "TotalImpressions": pli.TotalImpressions = GetChildElementValue(counters[idx], SIGNIFICAND);
                        break;
                    case "SimplexSheets": pli.SimplexSheets = GetChildElementValue(counters[idx], SIGNIFICAND);
                        break;
                }
                
            }
        }
        /// <summary>
        /// Retrieves the count nodes from the given list
        /// </summary>
        /// <param name="jad">XElement</param>
        /// <returns>List XElement</returns>
        private List<XElement> GetPICounters(XElement jad)
        {
            List<XElement> ps = GetElements(jad, "PrintSummary");
            List<XElement> cgList = new List<XElement>();

            foreach (XElement node in ps.Nodes())
            {
                string name = node.Name.LocalName;
                if (name.Equals("Counter"))
                {
                    List<XElement> cLst = GetElementsSelf(node, "Counter");
                    foreach (XElement xe in cLst.Nodes())
                    {
                        cgList.Add(xe);
                    }
                }
            }

            return cgList;
        }
        /// <summary>
        /// Retrieves the HTTP job info from the given XElement and stores in JobAcountingLogItem
        /// </summary>
        /// <param name="jali">JobAccountingLogItem</param>
        /// <param name="jad">XElement</param>
        private void GetHttpInfoData(JobAccountingLogItem jali, XElement jad)
        {
            // get the URI path and completion status
            var log = GetHttpInfo(jad).First();
            var fs = GetElementsSelf(jad, "FileSummaries");             

            // <HTTPInfo><DeliveredFilesInfo></FileSummaries></DelivededFilesInfo></HTTPInfo>

            if (fs.Count > 0)
            {
                // retrieve inner node info
                var fsLog = fs.Select(f => new HTTPLogItem()
                {
                    FileNameBase = GetChildElementValue(f, "FileNameBase"),
                    TotalDataSize = GetChildElementValue(f, "TotalDataSize"),
                    FileNameExtension = GetChildElementValue(f, "FileNameExtension"),
                }).First();

                // now add in the retrieved child elements.
                log.FileNameBase = fsLog.FileNameBase;
                log.FileNameExtension = fsLog.FileNameExtension;
                log.TotalDataSize = fsLog.TotalDataSize;

                jali.HttpInfo = log;
            }
        }
        /// <summary>
        /// Retrieves the HTTP URI path and the completion status of the job Info from the given job log data
        /// </summary>
        /// <param name="logData">XElement</param>
        /// <returns>IEnumerable<HTTPLogItem></returns>
        private IEnumerable<HTTPLogItem> GetHttpInfo(XElement logData)
        {
            var httpInfos = GetElementsSelf(logData, "HTTPInfo");

            var httpInfo = httpInfos.Select(x => new HTTPLogItem()
            {
                URI = GetChildElementValue(x, "URI"),
                CompletionStatus = GetChildElementValue(x, "CompletionStatus")
            });

            return httpInfo;
        }
        /// <summary>
        /// Creates a new JobAcountingLogItem. If the passed in value isn't null adds to list.
        /// Only time it should be null is first time through.
        /// </summary>
        /// <param name="JobAccountList"></param>
        /// <param name="jali">JobAccountingLogItem</param>
        /// <returns>JobAccountingLogItem</returns>
        private JobAccountingLogItem AddJobAccountingItemToList(List<JobAccountingLogItem> JobAccountList, JobAccountingLogItem jali)
        {
            if (jali != null)
            {
                JobAccountList.Add(jali);
            }
            jali = new JobAccountingLogItem();
            return jali;
        }
        /// <summary>
        /// Retrieves the job info data retrieved form the device job log
        /// </summary>
        /// <param name="jali">XElement</param>
        /// <param name="jad">JobAccountingLogItem</param>
        private void GetJobInfoData(JobAccountingLogItem jali, XElement jad)
        {
            var ji = GetElementsSelf(jad, "JobInfo");

            var jobLog = ji.Select(x => new JobLogItem()
            {
                UUID = GetChildElementValue(x, "UUID"),
                ApplicationName = GetChildElementValue(x, "ApplicationName"),
                DeviceJobName = GetChildElementValue(x, "DeviceJobName"),
                JobCategory = GetChildElementValue(x, "JobCategory"),
                JobDoneStatus = GetChildElementValue(x, "JobDoneStatus"),
                JobDoneTimestamp = GetChildElementValue(x, "JobDoneTimeStamp"),
                JobStartedTimestamp = GetChildElementValue(x, "JobStartedTimeStamp"),
            });
            foreach (var jl in jobLog)
            {
                jali.JobInfo = jl;
            }
        }
        /// <summary>
        /// Gets the device job log data and returns as string.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
        public string GetJobLogAsString(string role, string password)
        {
            var elem = GetJobLogAsElement(role, password);
            return (elem == null ? string.Empty : elem.ToString());
        }

        private XElement GetJobLogAsElement(string role, string password)
        {
            var ip = _deviceIpAddress;
            var jobAccountServiceUri = new Uri(CreateUri(ip, "/jobaccounting"));
            var jobAccountingUrn = new Uri("urn:hp:imaging:OXPd:service:jobaccounting:JobAccountingService:JobAccountingTable");
            var _client = new WSTransferClient(role, password);
            var logData = _client.Get(jobAccountServiceUri, jobAccountingUrn);

            TraceFactory.Logger.Debug("Account Log: \n{0}".FormatWith( logData));
            return logData;
        }

        protected void OnStatusUpdate(string status)
        {
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }

        private static string GetChildElementValue(XElement parent, string childLocalName)
        {
            string result = string.Empty;
            if (parent.HasElements)
            {
                var childElem = parent.Elements().FirstOrDefault(x => x.Name.LocalName.Equals(childLocalName, StringComparison.OrdinalIgnoreCase));
                if (childElem != null)
                {
                    result = childElem.Value;
                }
            }
            return result;
        }

        private static List<XElement> GetElements(XElement root, string localName)
        {
            var result = (
                        from el in root.Descendants().Where(x => x.Name.LocalName == localName)
                        select el
                        ).ToList();
            return result;
        }

        private static List<XElement> GetElementsSelf(XElement root, string localName)
        {
            var result = (
                        from el in root.DescendantsAndSelf().Where(x => x.Name.LocalName == localName)
                        select el
                        ).ToList();
            return result;
        }
        private static List<XElement>GetElementsFirst(XElement root, string localName)
        {
            var result = (from el in root.Elements().Where(x => x.Name.LocalName.Equals(localName))
                          select el).ToList();

            return result;
        }

        private static string CreateUri(string hostname, string serviceName)
        {
            if (hostname == null)
                throw new Exception("hostname cannot be null");

            bool ipv6 = hostname.Contains(":");
            StringBuilder sb = new StringBuilder();
            sb.Append("https://");
            if (ipv6)
            {
                // this must be an IPV6 address, so put brackets around it.
                sb.Append("[");
            }
            sb.Append(hostname);
            if (ipv6)
            {
                sb.Append("]");
            }
            sb.Append(":");
            sb.Append(7627);
            sb.Append(serviceName);
            return sb.ToString();
        }
    }
}


