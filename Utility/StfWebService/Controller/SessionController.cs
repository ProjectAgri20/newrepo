using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HP.ScalableTest.Service.StfWebService.Controller
{
    [RoutePrefix("api/Session")]
    public class SessionController : ApiController
    {
        //private static readonly ConcurrentDictionary<string, StfSessionTicket> SessionExecutionDictionary = new ConcurrentDictionary<string, StfSessionTicket>();
        //private static string _btfMessagingServerName;
        //private static string _btfArtifactsServerName;

        public SessionController()
        {
            //NameValueCollection systems = ConfigurationManager.GetSection("Systems") as NameValueCollection;
            //_btfMessagingServerName = systems["BtfMessageServer"];
            //_btfArtifactsServerName = systems["BtfArtifactsServer"];
        }

        [Route("StartSession")]
        [HttpPost]
        public HttpResponseMessage StartSession([FromBody] StfSessionTicket sessionTicket)
        {
            int queueLength = StfWebApiService.Enqueue(sessionTicket);

            var response = Request.CreateResponse(HttpStatusCode.OK, $"Test is queued at {queueLength}");
            return response;
        }

        //[Route("Initialize")]
        //[HttpGet]
        //public HttpResponseMessage Initialize()
        //{
        //    string dispatcherHostName = "localhost";
        //    if (GlobalSettings.IsDistributedSystem)
        //    {
        //        using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
        //        {
        //            var dispatcher = context.FrameworkServers.FirstOrDefault(x => x.Active && x.ServerTypes.Any(n => n.Name == "BTF"));
        //            if (dispatcher == null)
        //            {
        //                return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Could not find any dispatchers reserved for BTF Execution." };
        //            }
        //            dispatcherHostName = dispatcher.HostName;
        //        }
        //    }

        //    SessionClient.Instance.Initialize(dispatcherHostName);
        //    SessionClient.Instance.SessionStateReceived += Instance_SessionStateReceived;
        //    return new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = $"Subscribed to {dispatcherHostName}" };
        //}



        //[Route("Initiate")]
        //[HttpPost]
        //public HttpResponseMessage InitiateScenario([FromBody] StfSessionTicket sessionTicket)
        //{
        //    var ticket = SessionTicket.Create(new[] { Guid.Parse(sessionTicket.ScenarioId) }, sessionTicket.SessionName, sessionTicket.Duration);
        //    ticket.ExpirationDate = DateTime.Now + TimeSpan.FromDays(180.0);
        //    if (GlobalSettings.IsDistributedSystem)
        //    {
        //        ticket.SessionOwner = new UserCredential(GlobalSettings.Items.DomainAdminCredential.UserName,
        //            BasicEncryption.Encrypt(GlobalSettings.Items.DomainAdminCredential.Password, Resource.Key),
        //            GlobalSettings.Items.DomainAdminCredential.Domain);
        //    }
        //    SessionClient.Instance.InitiateSession(ticket);
        //    var assetDetails = SessionClient.Instance.Reserve(ticket.SessionId);

        //    if (assetDetails.Any(x => x.Availability != AssetAvailability.Available))
        //    {
        //        SessionClient.Instance.Close(ticket.SessionId);
        //        return new HttpResponseMessage(HttpStatusCode.Gone) { ReasonPhrase = "One or more assets are unavailable." };
        //    }
        //    foreach (var printDeviceDetail in assetDetails.OfType<PrintDeviceDetail>())
        //    {
        //        printDeviceDetail.UseCrc = false;
        //    }
        //    SessionClient.Instance.Stage(ticket.SessionId, assetDetails);
        //    lock (ticket.SessionId)
        //    {
        //        SessionExecutionDictionary.TryAdd(ticket.SessionId, sessionTicket);
        //    }
        //    var response = Request.CreateResponse(HttpStatusCode.OK, ticket);
        //    return response;
        //}

        //[Route("Validate")]
        //[HttpGet]
        //public HttpResponseMessage ValidateScenario([FromUri] string sessionId, [FromUri] int waitTimeOut = 30)
        //{
        //    int retryDelay = waitTimeOut / 10;
        //    if (string.IsNullOrEmpty(sessionId))
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Session Id cannot be empty" };

        //    if (Retry.UntilTrue(
        //        (() => SessionClient.Instance.GetSessionStartupState(sessionId) ==
        //               SessionStartupTransition.ReadyToValidate), 10, TimeSpan.FromSeconds(retryDelay)))
        //    {
        //        SessionClient.Instance.Validate(sessionId);
        //    }
        //    else
        //    {
        //        var state = SessionClient.Instance.GetSessionStartupState(sessionId);
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = $"Session is in {state}" };
        //    }

        //    return new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Session has been Validated" };
        //}

        //[Route("PowerUp")]
        //[HttpGet]
        //public HttpResponseMessage PowerUp([FromUri] string sessionId, [FromUri] int waitTimeOut = 30)
        //{
        //    int retryDelay = waitTimeOut / 10;
        //    if (string.IsNullOrEmpty(sessionId))
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Session Id cannot be empty" };

        //    if (Retry.UntilTrue(
        //        () => SessionClient.Instance.GetSessionStartupState(sessionId) ==
        //              SessionStartupTransition.ReadyToPowerUp, 10, TimeSpan.FromSeconds(retryDelay)))
        //    {
        //        SessionClient.Instance.PowerUp(sessionId);
        //    }
        //    else
        //    {
        //        var state = SessionClient.Instance.GetSessionStartupState(sessionId);
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = $"Session is in {state}" };
        //    }
        //    return new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Session is being powered on" };
        //}

        //[Route("Run")]
        //[HttpGet]
        //public HttpResponseMessage Run([FromUri] string sessionId, [FromUri] int waitTimeOut = 30)
        //{
        //    int retryDelay = waitTimeOut / 10;
        //    if (string.IsNullOrEmpty(sessionId))
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Session Id cannot be empty" };

        //    if (Retry.UntilTrue(
        //        () => SessionClient.Instance.GetSessionStartupState(sessionId) ==
        //              SessionStartupTransition.ReadyToRun, 10, TimeSpan.FromSeconds(retryDelay)))
        //    {
        //        SessionClient.Instance.Run(sessionId);
        //    }
        //    else
        //    {
        //        var state = SessionClient.Instance.GetSessionState(sessionId);
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = $"Session is in {state}" };
        //    }

        //    return new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Session has begun" };
        //}

        //private void Instance_SessionStateReceived(object sender, SessionStateEventArgs e)
        //{
        //    TraceFactory.Logger.Debug($"Session State received: {e.State} for Session {e.SessionId}");
        //    switch (e.State)
        //    {
        //        case SessionState.RunComplete:
        //            {
        //                lock (e.SessionId)
        //                {
        //                    var state = SessionClient.Instance.GetSessionState(e.SessionId);
        //                    if (state != SessionState.ShuttingDown || state != SessionState.ShutdownComplete)
        //                    {
        //                        ShutdownOptions shutdownOptions = new ShutdownOptions
        //                        {
        //                            CopyLogs = true,
        //                            ReleaseDeviceReservation = true
        //                        };
        //                        TraceFactory.Logger.Debug($"Shutting down session {e.SessionId}");
        //                        SessionClient.Instance.Shutdown(e.SessionId, shutdownOptions);
        //                    }
        //                }
        //            }
        //            break;

        //        case SessionState.ShutdownComplete:
        //            {
        //                lock (e.SessionId)
        //                {
        //                    TraceFactory.Logger.Debug($"Shutdown complete for session {e.SessionId}");
        //                    StfSessionTicket sessionTicket;
        //                    SessionClient.Instance.SessionStateReceived -= Instance_SessionStateReceived;
        //                    SessionExecutionDictionary.TryGetValue(e.SessionId, out sessionTicket);
        //                    if (sessionTicket != null)
        //                    {
        //                        //this is a request coming from BTF, let's push data to the btf system
        //                        if (!string.IsNullOrEmpty(sessionTicket.TestExecutionId))
        //                        {
        //                            //post the data back
        //                            SendExecutionLogs(e.SessionId, $"artifacts/{sessionTicket.TestArtificatsUrl}");
        //                            UpdateTestResult(e.SessionId);
        //                        }

        //                        SessionExecutionDictionary.TryRemove(e.SessionId, out sessionTicket);
        //                    }
        //                }
        //            }
        //            break;
        //    }
        //}

        //private void UpdateTestResult(string sessionId)
        //{
        //    SessionSummary summary;
        //    StfSessionTicket sessionTicket;
        //    string testStatus = "Pass";
        //    string errorMessage = string.Empty;
        //    SessionExecutionDictionary.TryGetValue(sessionId, out sessionTicket);

        //    if (sessionTicket == null)
        //        return;
        //    using (DataLogContext context = DbConnect.DataLogContext())
        //    {
        //        summary = context.DbSessions.First(n => n.SessionId == sessionId);
        //        if (context.SessionData(sessionId).Activities.Any(x => x.Status == "Failed"))
        //        {
        //            testStatus = "Fail";
        //        }

        //        var errorMessages = context.SessionData(sessionId).Activities.Where(x => x.Status == "Error")
        //            .Select(x => x.ResultMessage);
        //        if (errorMessages.Any())
        //            errorMessage = string.Join(",", errorMessages);
        //    }

        //    TraceFactory.Logger.Debug($"Updating status of test: {sessionTicket.TestCaseId} as {testStatus}");
        //    var exchangeValue = string.Format(
        //        "{{u'Message': {9}, u'TestExecutionResults': [{{u'TestCaseURL': u'{0}',u'LocalTestExecutionFolder': u'{1}', u'TestCaseId': {2}, u'TestCaseName': u'{3}'," +
        //        " u'TestExecutionID': u'{4}', u'ExecutionData': [{{u'MetaData': [{{u'metadata_name': u'TestResourcesProvisioningBeginTS', u'metadata_value': u'{5}'}}," +
        //        " {{u'metadata_name': u'TestResourcesProvisioningEndTS', u'metadata_value': u'{5}'}}," +
        //        " {{u'metadata_name': u'TestResourcesDecommissioningBeginTS', u'metadata_value': u'{6}'}}," +
        //        " {{u'metadata_name': u'TestResourcesDecommissioningEndTS', u'metadata_value': u'{6}'}}]," +
        //        " u'Run': u'0', u'TestCaseID': {2}, u'BeginTime': u'{5}', u'EndTime': u'{6}', u'Result': u'{7}'}}]}}], u'Error': {8}}}",
        //        sessionTicket.ScenarioId, "STF", sessionTicket.TestCaseId, sessionTicket.SessionName, sessionTicket.TestExecutionId, summary.StartDateTime, summary.EndDateTime, testStatus, string.IsNullOrEmpty(errorMessage) ? "False" : "True",
        //        string.IsNullOrEmpty(errorMessage) ? "None" : $"u'{errorMessage}'");

        //    var exchangeMessage = "{ \"project\": \"TestResultsProcessor\", \"token\": \"process_test_results\", " +
        //                          $"\"parameter\": [{{ \"name\": \"TEST_RESULT\",\"value\": \"{exchangeValue}\"}}]}}";

        //    HttpClientHandler handler = new HttpClientHandler();
        //    HttpClient client = new HttpClient(handler);
        //    string bodyString = "{\"notification_info\": [{\"notification_type\": \"message\"," +
        //                        $"\"notification_params\":{{\"use_btf_messaging\": false, \"connection_string\": \"amqp://btfadmin:iso*help@{_btfMessagingServerName}:8080/\"," +
        //                        "\"exchange_name\": \"test_execution_results\",\"exchange_type\": \"direct\",\"routing_key\": \"ROUTINGKEY\"," +
        //                        $"\"message_info\": {exchangeMessage}}}}}]}}";
        //    var resultMessage = client.PostAsJsonAsync("https://btf-preprod-ext.psr.rd.hpicorp.net/ns/v1/submitnotificationinfo", bodyString);
        //    if (resultMessage.Result.IsSuccessStatusCode)
        //    {
        //        TraceFactory.Logger.Debug($"Published the result to {_btfMessagingServerName}");
        //    }

        //    //ConnectionFactory connectionFactory =
        //    //    new ConnectionFactory
        //    //    {
        //    //        HostName = _btfMessagingServerName,
        //    //        Port = 8080,
        //    //        UserName = "btfadmin",
        //    //        Password = "iso*help"
        //    //    };

        //    //try
        //    //{
        //    //    using (var connection = connectionFactory.CreateConnection())
        //    //    {
        //    //        using (var channel = connection.CreateModel())
        //    //        {
        //    //            IBasicProperties basicProperties = new BasicProperties
        //    //            {
        //    //                AppId = "remote-build",
        //    //                ContentType = "application/json"
        //    //            };

        //    //            channel.BasicPublish("test_execution_results", "ROUTINGKEY", basicProperties,
        //    //                Encoding.UTF8.GetBytes(exchangeMessage));
        //    //        }
        //    //    }
        //    //}
        //    //catch (BrokerUnreachableException brokerUnreachable)
        //    //{
        //    //    TraceFactory.Logger.Debug($"Unable to reach the messaging server. {brokerUnreachable.Message}");
        //    //}

        //    //TraceFactory.Logger.Debug($"Published the result to {_btfMessagingServerName}");
        //}

        //private void SendExecutionLogs(string sessionId, string path)
        //{
        //    //"btf-preprod-artifacts.psr.rd.hpicorp.net"
        //    string userName = "citester";
        //    string password = "citester!@#";

        //    //get the log data
        //    TraceFactory.Logger.Debug($"Collecting logs for Session: {sessionId}");
        //    var logData = SessionClient.Instance.GetLogData(sessionId);
        //    //we know on STB machines the log file will be present in the location of the calling assembly read that up and return the string
        //    if (string.IsNullOrEmpty(logData))
        //    {
        //        logData = GetLogData(sessionId);
        //    }

        //    TraceFactory.Logger.Debug($"Uploading logs to Artifact server: {_btfArtifactsServerName}");
        //    var connectionInfo = new ConnectionInfo(_btfArtifactsServerName, 22, userName, new PasswordAuthenticationMethod(userName, password));
        //    using (var client = new SftpClient(connectionInfo))
        //    {
        //        client.Connect();
        //        if (!client.Exists(path))
        //            CreateDirectoryRecursively(client, path);
        //        client.ChangeDirectory(path);

        //        client.WriteAllText($"{path}/{sessionId}.log", logData);
        //    }
        //    TraceFactory.Logger.Debug($"Log written: {path}/{sessionId}.log");
        //}

        //private string GetLogData(string sessionId)
        //{
        //    string logFilePath;
        //    string pattern;
        //    if (GlobalSettings.IsDistributedSystem)
        //    {
        //        logFilePath = @"C:\VirtualResource\Distribution\SessionProxy\Logs";
        //        pattern = "SessionProxy-{0}".FormatWith(sessionId);
        //    }
        //    else
        //    {
        //        logFilePath = LogFileReader.DataLogPath();
        //        pattern = "StfWebService.exe.log";
        //    }

        //    StringBuilder builder = new StringBuilder();
        //    var logFiles = LogFileDataCollection.Create(logFilePath);

        //    foreach (var file in logFiles.Items.Where(x => x.FileName.StartsWith(pattern, StringComparison.OrdinalIgnoreCase)))
        //    {
        //        builder.AppendLine(file.FileData);
        //    }

        //    return builder.ToString();
        //}

        //private void CreateDirectoryRecursively(SftpClient client, string path)
        //{
        //    string current = "";

        //    if (path[0] == '/')
        //    {
        //        path = path.Substring(1);
        //    }

        //    while (!string.IsNullOrEmpty(path))
        //    {
        //        int p = path.IndexOf('/');
        //        current += '/';
        //        if (p >= 0)
        //        {
        //            current += path.Substring(0, p);
        //            path = path.Substring(p + 1);
        //        }
        //        else
        //        {
        //            current += path;
        //            path = "";
        //        }

        //        try
        //        {
        //            SftpFileAttributes attrs = client.GetAttributes(current);
        //            if (!attrs.IsDirectory)
        //            {
        //                throw new Exception("not directory");
        //            }
        //        }
        //        catch (SftpPathNotFoundException)
        //        {
        //            client.CreateDirectory(current);
        //        }
        //    }
        //}

        //[Route("Shutdown")]
        //[HttpGet]
        //public HttpResponseMessage Shutdown([FromUri] string sessionId)
        //{
        //    if (string.IsNullOrEmpty(sessionId))
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Session Id cannot be empty" };

        //    SessionClient.Instance.Close(sessionId);

        //    return new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Session is being shutdown" };
        //}
    }
}