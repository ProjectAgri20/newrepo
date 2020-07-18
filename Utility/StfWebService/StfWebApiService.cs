using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Timers;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.AssetInventory.Reservation;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DataLog.Model;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;


namespace HP.ScalableTest.Service.StfWebService
{
    public partial class StfWebApiService : SelfInstallingServiceBase
    {
        private readonly string _baseAddress = "http://*:9000/";
        private IDisposable _server;
        private static readonly Queue<StfSessionTicket> SessionQueue = new Queue<StfSessionTicket>();
        private readonly Timer _executionTimer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
        private bool _isSessionExecuting;
        private static readonly ConcurrentDictionary<string, StfSessionTicket> SessionExecutionDictionary = new ConcurrentDictionary<string, StfSessionTicket>();
        private static string _btfMessagingServerName;
        private static string _btfArtifactsServerName;
        private bool _revalidated;
        private readonly ShutdownOptions _shutdownOptions;

        public StfWebApiService() : base("StfWebService", "STF Web Service")
        {
            Description = "Service to host Web Services for triggering STF Sessions";
            _executionTimer.Elapsed += _executionTimer_Elapsed;
            _executionTimer.Enabled = false;
            NameValueCollection systems = ConfigurationManager.GetSection("Systems") as NameValueCollection;
            _btfMessagingServerName = systems["BtfMessageServer"];
            _btfArtifactsServerName = systems["BtfArtifactsServer"];
            _shutdownOptions = new ShutdownOptions
            {
                AllowWorkerToComplete = false,
                CopyLogs = false,
                PowerOff = true,
                PowerOffOption = VMPowerOffOption.PowerOff
            };
        }

        private void _executionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_isSessionExecuting || SessionQueue.Count == 0)
                return;

            StartSession(SessionQueue.Dequeue());
        }

        public void StartSession(StfSessionTicket sessionTicket)
        {
            _isSessionExecuting = true;
            try
            {
                var ticket = SessionTicket.Create(new[] { Guid.Parse(sessionTicket.ScenarioId) }, sessionTicket.SessionName, sessionTicket.Duration);
                ticket.ExpirationDate = DateTime.Now + TimeSpan.FromDays(180.0);
                if (GlobalSettings.IsDistributedSystem)
                {
                    ticket.SessionOwner = new UserCredential(GlobalSettings.Items.DomainAdminCredential.UserName,GlobalSettings.Items.DomainAdminCredential.Password, GlobalSettings.Items.DomainAdminCredential.Domain);
                }
                SessionClient.Instance.InitiateSession(ticket);
                var assetDetails = SessionClient.Instance.Reserve(ticket.SessionId);

                if (assetDetails.Any(x => x.Availability != AssetAvailability.Available))
                {
                    SessionClient.Instance.Close(ticket.SessionId);
                    TraceFactory.Logger.Error($"One or more assets are unavailable for session {ticket.SessionId}");
                    _isSessionExecuting = false;
                    return;
                }
                foreach (var printDeviceDetail in assetDetails.OfType<PrintDeviceDetail>())
                {
                    printDeviceDetail.UseCrc = false;
                }
                SessionClient.Instance.Stage(ticket.SessionId, assetDetails);
                lock (ticket.SessionId)
                {
                    SessionExecutionDictionary.TryAdd(ticket.SessionId, sessionTicket);
                }

            }
            catch (Exception e)
            {
                TraceFactory.Logger.Error(e.JoinAllErrorMessages());
                _isSessionExecuting = false;
            }
            
        }

        public void RunDebug(string[] args)
        {
            OnStart(args);
            Console.ReadLine();
            OnStop();
        }

        protected override void StartService(CommandLineArguments args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            FrameworkServiceHelper.LoadSettings(args, autoRefresh: true);
            using (EnterpriseTestContext dataContext = new EnterpriseTestContext())
            {
                GlobalSettings.IsDistributedSystem = dataContext.VirtualResources.Any(r => r.ResourceType == "OfficeWorker");
            }
            string dispatcherHostName = "localhost";
            if (GlobalSettings.IsDistributedSystem)
            {
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    var dispatcher = context.FrameworkServers.FirstOrDefault(x => x.Active && x.ServerTypes.Any(n => n.Name == "BTF"));
                    if (dispatcher == null)
                    {
                        TraceFactory.Logger.Error("Could not find any dispatchers reserved for BTF Execution." );
                        return;
                    }
                    
                }
            }

            SessionClient.Instance.Initialize(dispatcherHostName);
            SessionClient.Instance.SessionStartupTransitionReceived += Instance_SessionStartupTransitionReceived;
            SessionClient.Instance.SessionStateReceived += Instance_SessionStateReceived; 
            _server = WebApp.Start<Startup>(url: _baseAddress);
            _executionTimer.Start();
        }

        protected override void StopService()
        {
            _server?.Dispose();
            _executionTimer.Stop();
            _executionTimer.Dispose();
            SessionClient.Instance.SessionStateReceived -= Instance_SessionStateReceived;
            SessionClient.Instance.SessionStartupTransitionReceived -= Instance_SessionStartupTransitionReceived;
        }

        //protected void OnStart(string[] args)
        //{
        //    if (args == null)
        //    {
        //        throw new ArgumentNullException("args");
        //    }

        //    GlobalSettings.Load(args[0]);
        //    using (EnterpriseTestContext dataContext = new EnterpriseTestContext())
        //    {
        //        GlobalSettings.IsDistributedSystem = dataContext.VirtualResources.Any(r => r.ResourceType == "OfficeWorker");
        //    }
        //    string dispatcherHostName = "localhost";
        //    if (GlobalSettings.IsDistributedSystem)
        //    {
        //        using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
        //        {
        //            var dispatcher = context.FrameworkServers.FirstOrDefault(x => x.Active && x.ServerTypes.Any(n => n.Name == "BTF"));
        //            if (dispatcher == null)
        //            {
        //                TraceFactory.Logger.Error("Could not find any dispatchers reserved for BTF Execution.");
        //                return;
        //            }

        //        }
        //    }

        //    SessionClient.Instance.Initialize(dispatcherHostName);
        //    SessionClient.Instance.SessionStartupTransitionReceived += Instance_SessionStartupTransitionReceived;
        //    SessionClient.Instance.SessionStateReceived += Instance_SessionStateReceived;
        //    _server = WebApp.Start<Startup>(url: _baseAddress);
        //    _executionTimer.Start();
        //}

        //protected void OnStop()
        //{
        //    _server.Dispose();
        //    _executionTimer.Stop();
        //    _executionTimer.Dispose();
        //    SessionClient.Instance.SessionStateReceived -= Instance_SessionStateReceived;
        //    SessionClient.Instance.SessionStartupTransitionReceived -= Instance_SessionStartupTransitionReceived;
        //}

        private void Instance_SessionStartupTransitionReceived(object sender, SessionStartupTransitionEventArgs e)
        {
            TraceFactory.Logger.Debug($"Session Startup State received: {e.Transition} for Session {e.SessionId}");
            switch (e.Transition)
            {
                case SessionStartupTransition.ReadyToValidate:
                    SessionClient.Instance.Validate(e.SessionId);
                    break;

                case SessionStartupTransition.ReadyToPowerUp:
                {
                    _revalidated = false;
                    SessionClient.Instance.PowerUp(e.SessionId);
                    }
                    break;

                case SessionStartupTransition.ReadyToRun:
                {
                    
                    SessionClient.Instance.Run(e.SessionId);
                }
                    break;

                case SessionStartupTransition.StartupComplete:
                {
                    TraceFactory.Logger.Debug("Running");
                }
                    break;

                case SessionStartupTransition.ReadyToRevalidate:
                {
                    if (_revalidated)
                    {
                        SessionClient.Instance.Shutdown(e.SessionId, _shutdownOptions);
                    }
                    else
                    {
                        TraceFactory.Logger.Error("Error, check VM and retry");
                        TraceFactory.Logger.Info("Waiting one minute before retrying, please fix the errors in the meantime");
                        Delay.Wait(TimeSpan.FromMinutes(1));
                        SessionClient.Instance.Revalidate(e.SessionId);
                        _revalidated = true;
                    }
                }
                    break;
            }
        }

        private void Instance_SessionStateReceived(object sender, SessionStateEventArgs e)
        {
            TraceFactory.Logger.Debug($"Session State received: {e.State} for Session {e.SessionId}");
            switch (e.State)
            {
                case SessionState.RunComplete:
                    {
                        lock (e.SessionId)
                        {
                            var state = SessionClient.Instance.GetSessionState(e.SessionId);
                            if (state != SessionState.ShuttingDown || state != SessionState.ShutdownComplete)
                            {
                                ShutdownOptions shutdownOptions = new ShutdownOptions
                                {
                                    CopyLogs = true,
                                    ReleaseDeviceReservation = true
                                };
                                TraceFactory.Logger.Debug($"Shutting down session {e.SessionId}");
                                SessionClient.Instance.Shutdown(e.SessionId, shutdownOptions);
                            }
                        }
                    }
                    break;

                case SessionState.ShutdownComplete:
                    {
                        lock (e.SessionId)
                        {
                            TraceFactory.Logger.Debug($"Shutdown complete for session {e.SessionId}");
                            StfSessionTicket sessionTicket;
                            SessionExecutionDictionary.TryGetValue(e.SessionId, out sessionTicket);
                            if (sessionTicket != null)
                            {
                                //this is a request coming from BTF, let's push data to the btf system
                                if (!string.IsNullOrEmpty(sessionTicket.TestExecutionId))
                                {
                                    //post the data back
                                    SendExecutionLogs(e.SessionId, $"artifacts/{sessionTicket.TestArtificatsUrl}");
                                    UpdateTestResult(e.SessionId);
                                }

                                SessionExecutionDictionary.TryRemove(e.SessionId, out sessionTicket);
                                _isSessionExecuting = false;
                            }
                        }
                    }
                    break;
            }
        }

       

        public static int Enqueue(StfSessionTicket ticket)
        {
            SessionQueue.Enqueue(ticket);
            return SessionQueue.Count;
        }

        private void UpdateTestResult(string sessionId)
        {
            SessionSummary summary;
            StfSessionTicket sessionTicket;
            string testStatus = "Pass";
            string errorMessage = string.Empty;
            SessionExecutionDictionary.TryGetValue(sessionId, out sessionTicket);

            if (sessionTicket == null)
                return;
            using (DataLogContext context = DbConnect.DataLogContext())
            {
                summary = context.DbSessions.First(n => n.SessionId == sessionId);
                if (context.SessionData(sessionId).Activities.Any(x => x.Status == "Failed"))
                {
                    testStatus = "Fail";
                }

                var errorMessages = context.SessionData(sessionId).Activities.Where(x => x.Status == "Error")
                    .Select(x => x.ResultMessage);
                if (errorMessages.Any())
                    errorMessage = string.Join(",", errorMessages);
            }

            TraceFactory.Logger.Debug($"Updating status of test: {sessionTicket.TestCaseId} as {testStatus}");
            var exchangeValue = string.Format(
                "{{u'Message': {9}, u'TestExecutionResults': [{{u'TestCaseURL': u'{0}',u'LocalTestExecutionFolder': u'{1}', u'TestCaseId': {2}, u'TestCaseName': u'{3}'," +
                " u'TestExecutionID': u'{4}', u'ExecutionData': [{{u'MetaData': [{{u'metadata_name': u'TestResourcesProvisioningBeginTS', u'metadata_value': u'{5}'}}," +
                " {{u'metadata_name': u'TestResourcesProvisioningEndTS', u'metadata_value': u'{5}'}}," +
                " {{u'metadata_name': u'TestResourcesDecommissioningBeginTS', u'metadata_value': u'{6}'}}," +
                " {{u'metadata_name': u'TestResourcesDecommissioningEndTS', u'metadata_value': u'{6}'}}]," +
                " u'Run': u'0', u'TestCaseID': {2}, u'BeginTime': u'{5}', u'EndTime': u'{6}', u'Result': u'{7}'}}]}}], u'Error': {8}}}",
                sessionTicket.ScenarioId, "STF", sessionTicket.TestCaseId, sessionTicket.SessionName, sessionTicket.TestExecutionId, summary.StartDateTime, summary.EndDateTime, testStatus, string.IsNullOrEmpty(errorMessage) ? "False" : "True",
                string.IsNullOrEmpty(errorMessage) ? "None" : $"u'{errorMessage}'");

            var exchangeMessage = "{ \"project\": \"TestResultsProcessor\", \"token\": \"process_test_results\", " +
                                  $"\"parameter\": [{{ \"name\": \"TEST_RESULT\",\"value\": \"{exchangeValue}\"}}]}}";

            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string bodyString = "{\"notification_info\": [{\"notification_type\": \"message\"," +
                                $"\"notification_params\":{{\"use_btf_messaging\": false, \"connection_string\": \"amqp://btfadmin:iso*help@{_btfMessagingServerName}:8080/\"," +
                                "\"exchange_name\": \"test_execution_results\",\"exchange_type\": \"direct\",\"routing_key\": \"ROUTINGKEY\"," +
                                $"\"message_info\": {exchangeMessage}}}}}]}}";
            var resultMessage = client.PostAsJsonAsync("https://btf-preprod-ext.psr.rd.hpicorp.net/ns/v1/submitnotificationinfo", bodyString);
            if (resultMessage.Result.IsSuccessStatusCode)
            {
                TraceFactory.Logger.Debug($"Published the result to {_btfMessagingServerName}");
            }

            //ConnectionFactory connectionFactory =
            //    new ConnectionFactory
            //    {
            //        HostName = _btfMessagingServerName,
            //        Port = 8080,
            //        UserName = "btfadmin",
            //        Password = "iso*help"
            //    };

            //try
            //{
            //    using (var connection = connectionFactory.CreateConnection())
            //    {
            //        using (var channel = connection.CreateModel())
            //        {
            //            IBasicProperties basicProperties = new BasicProperties
            //            {
            //                AppId = "remote-build",
            //                ContentType = "application/json"
            //            };

            //            channel.BasicPublish("test_execution_results", "ROUTINGKEY", basicProperties,
            //                Encoding.UTF8.GetBytes(exchangeMessage));
            //        }
            //    }
            //}
            //catch (BrokerUnreachableException brokerUnreachable)
            //{
            //    TraceFactory.Logger.Debug($"Unable to reach the messaging server. {brokerUnreachable.Message}");
            //}

            //TraceFactory.Logger.Debug($"Published the result to {_btfMessagingServerName}");
        }

        private void SendExecutionLogs(string sessionId, string path)
        {
            //"btf-preprod-artifacts.psr.rd.hpicorp.net"
            string userName = "citester";
            string password = "citester!@#";

            //get the log data
            TraceFactory.Logger.Debug($"Collecting logs for Session: {sessionId}");
            var logData = SessionClient.Instance.GetLogData(sessionId);
            //we know on STB machines the log file will be present in the location of the calling assembly read that up and return the string
            if (string.IsNullOrEmpty(logData))
            {
                logData = GetLogData(sessionId);
            }

            TraceFactory.Logger.Debug($"Uploading logs to Artifact server: {_btfArtifactsServerName}");
            var connectionInfo = new ConnectionInfo(_btfArtifactsServerName, 22, userName, new PasswordAuthenticationMethod(userName, password));
            using (var client = new SftpClient(connectionInfo))
            {
                client.Connect();
                if (!client.Exists(path))
                    CreateDirectoryRecursively(client, path);
                client.ChangeDirectory(path);

                client.WriteAllText($"{path}/{sessionId}.log", logData);
            }
            TraceFactory.Logger.Debug($"Log written: {path}/{sessionId}.log");
        }

        private string GetLogData(string sessionId)
        {
            string logFilePath;
            string pattern;
            if (GlobalSettings.IsDistributedSystem)
            {
                logFilePath = @"C:\VirtualResource\Distribution\SessionProxy\Logs";
                pattern = "SessionProxy-{0}".FormatWith(sessionId);
            }
            else
            {
                logFilePath = LogFileReader.DataLogPath();
                pattern = "StfWebService.exe.log";
            }

            StringBuilder builder = new StringBuilder();
            var logFiles = LogFileDataCollection.Create(logFilePath);

            foreach (var file in logFiles.Items.Where(x => x.FileName.StartsWith(pattern, StringComparison.OrdinalIgnoreCase)))
            {
                builder.AppendLine(file.FileData);
            }

            return builder.ToString();
        }

        private void CreateDirectoryRecursively(SftpClient client, string path)
        {
            string current = "";

            if (path[0] == '/')
            {
                path = path.Substring(1);
            }

            while (!string.IsNullOrEmpty(path))
            {
                int p = path.IndexOf('/');
                current += '/';
                if (p >= 0)
                {
                    current += path.Substring(0, p);
                    path = path.Substring(p + 1);
                }
                else
                {
                    current += path;
                    path = "";
                }

                try
                {
                    SftpFileAttributes attrs = client.GetAttributes(current);
                    if (!attrs.IsDirectory)
                    {
                        throw new Exception("not directory");
                    }
                }
                catch (SftpPathNotFoundException)
                {
                    client.CreateDirectory(current);
                }
            }
        }
    }

    /// <summary>
    /// Class for holding incoming data from BTF
    /// </summary>
    public class StfSessionTicket
    {
        /// <summary>
        /// The scenario to be executed
        /// </summary>
        public string ScenarioId { get; set; }
        /// <summary>
        /// Name of the test
        /// </summary>
        public string SessionName { get; set; }

        /// <summary>
        /// Probable duration of the test
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// The test case id from BTF
        /// </summary>
        public int TestCaseId { get; set; }
        /// <summary>
        /// The test execution id which denotes the instances
        /// </summary>
        public string TestExecutionId { get; set; }
        /// <summary>
        /// the post back destination for logs and status
        /// </summary>
        public string TestArtificatsUrl { get; set; }
    }
}