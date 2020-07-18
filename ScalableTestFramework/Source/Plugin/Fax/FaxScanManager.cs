using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.NativeApps.Fax;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using HtmlAgilityPack;

namespace HP.ScalableTest.Plugin.Fax
{
    public class FaxScanManager : ScanActivityManager
    {
        private readonly FaxActivityData _data;
        private IFaxApp _faxApp = null;

        private readonly Pacekeeper _pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(5));

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; }

        protected override string ScanType => "Fax";

        public FaxScanManager(PluginExecutionData executionData)
            : base(executionData)
        {
            _data = executionData.GetMetadata<FaxActivityData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = _data.ScanOptions;
        }

        public FaxScanManager(PluginExecutionData executionData, string serverName)
            : base(executionData, serverName)
        {
            _data = executionData.GetMetadata<FaxActivityData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = _data.ScanOptions;
        }

        protected override void SetupJob(IDevice device)
        {
            if (_data.FaxOperation == FaxTask.SendFax)
            {
                SendFaxSetupJob(device);
            }
            else if (_data.FaxOperation == FaxTask.ReceiveFax)
            {
                ReceiveFaxSetupJob(device);
            }
        }
        /// <summary>
        /// Sets up the Fax job.
        /// </summary>
        /// <param name="device">The device.</param>
        private void SendFaxSetupJob(IDevice device)
        {
            UpdateStatus("Setting up Fax Send job...");
            ScanLog.JobEndStatus = "Failed";
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            UpdateStatus(string.Format("Setting up device at address {0} for user {1}", device.Address, ExecutionData.Credential.UserName));

            InitializeAuthenticator(_data.AuthProvider, device, ExecutionData);
            // Load the fax application
            _faxApp = FaxAppFactory.Create(device);

            // need to add the ability for user to set eager or lazy authentication
            AuthenticationMode am = (_data.ApplicationAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            _faxApp.WorkflowLogger = Authenticator.WorkflowLogger = WorkflowLogger;
            _faxApp.Pacekeeper = Authenticator.Pacekeeper = new Pacekeeper(_data.AutomationPause);
            _faxApp.Launch(Authenticator, am);

            if (!_data.UseSpeedDial)
            {
                if (string.IsNullOrEmpty(_data.FaxNumber) || string.IsNullOrWhiteSpace(_data.FaxNumber))
                {
                    // Apply settings from configuration
                    _faxApp.AddRecipient(FilePrefix.ToFaxCode());
                }
                else
                {
                    _faxApp.AddRecipient(_data.FaxNumber);
                }
            }
            else
            {
                Dictionary<string, string> recipients = new Dictionary<string, string>();
                string[] FaxNums;
                if (_data.FaxNumber == null || string.IsNullOrWhiteSpace(_data.FaxNumber))
                {
                    // Apply settings from configuration
                    FaxNums = FilePrefix.ToFaxCode().Split(',');
                    foreach (string FaxNum in FaxNums)
                    {
                        recipients[FaxNum] = string.Empty;
                    }
                    _faxApp.AddRecipients(recipients, false);
                }
                else
                {
                    FaxNums = _data.FaxNumber.Split(',');
                    string[] PINs = new string[FaxNums.Length];
                    _data.PIN.Split(',', (char)StringSplitOptions.None).CopyTo(PINs, 0);
                    for (int i = 0; i < FaxNums.Count(); i++)
                    {
                        recipients[FaxNums[i]] = PINs[i];
                    }

                    _faxApp.AddRecipients(recipients, _data.UseSpeedDial);
                }
            }       

            if (_data.EnableNotification)
            {
                EmailBuilder emailAddress = new EmailBuilder(_data.NotificationEmail, ExecutionData);
                //sending false parameter for thumbNail as it is unchecked bydefault             
                _faxApp.Options.EnableEmailNotification(NotifyCondition.Always, emailAddress.ToString(), false);
            }

            //Sets the scan job options
            SetOptions(_data.ScanOptions, _faxApp.Options.GetType(), _faxApp.GetType(), device);
            _faxApp.Options.SetJobBuildState(this.UseJobBuild);

            // OCR is not applicable for fax jobs
            ScanLog.Ocr = false;
            UpdateStatus("Job setup complete");
            _pacekeeper.Pause();
        }

        private void ReceiveFaxSetupJob(IDevice device)
        {
            UpdateStatus("Setting up Fax Receive job...");
            ScanLog.JobEndStatus = "Failed";
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            UpdateStatus(string.Format("Setting up device at address {0} for user {1}", device.Address, ExecutionData.Credential.UserName));

            if (_data.EnableNotification)
            {
                EmailBuilder emailAddress = new EmailBuilder(_data.NotificationEmail, ExecutionData);
                //sending false parameter for thumbNail as it is unchecked bydefault              
                _faxApp.Options.EnableEmailNotification(NotifyCondition.Always, emailAddress.ToString(), false);
            }

            UpdateStatus("Job setup complete");

            UpdateStatus(string.Format("Waiting for Fax Receiving at address {0} for {1}", device.Address, _data.FaxReceiveTimeout));
            Thread.Sleep(_data.FaxReceiveTimeout);
            //TODO: check for fax notification (Answering call) while Fax Receiving check report once received
        }

        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            _pacekeeper.Pause(); // Sync for Limo
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            try
            {
                if (_data.FaxOperation == FaxTask.SendFax)
                {
                    if (SendFaxFinishJob(device))
                    {
                        result = new PluginExecutionResult(PluginResult.Passed);
                    }
                }
                else if (_data.FaxOperation == FaxTask.ReceiveFax)
                {
                    if (ReceiveFaxFinishJob(device))
                    {
                        result = new PluginExecutionResult(PluginResult.Passed);
                    }
                    else
                    {
                        result = new PluginExecutionResult(PluginResult.Failed, "Fax Receive Result Failed ", "Fax Receive Error");
                    }
                }
            }
            finally
            {
                // We got far enough to start the scan job, so submit the log
                SetJobEndStatus(result);
                ExecutionServices.DataLogger.Submit(ScanLog);
            }

            return result;
        }

        private bool SendFaxFinishJob(IDevice device)
        {
            // Start the job
            ScanExecutionOptions options = new ScanExecutionOptions();
            if (this.UseJobBuild)
            {
                options.JobBuildSegments = _data.ScanOptions.PageCount;
            }
            return _faxApp.ExecuteJob(options);      
        }

        private bool ReceiveFaxFinishJob(IDevice device)
        {
            bool done = true;
            List<FaxReport> faxReports = null;
            FaxReport faxData = null;

            UpdateStatus(string.Format("Retrieving Fax report"));
            string FaxReportHtml = _faxApp.RetrieveFaxReport();
            if (!string.IsNullOrEmpty(FaxReportHtml))
            {
                UpdateStatus(string.Format("Consolidating Fax report"));
                faxReports = ConsolidateFaxReport(FaxReportHtml);
            }
            if (faxReports != null)
            {
                faxData = faxReports.Where(p => p.Identification == _data.FaxNumber)
                                            .OrderByDescending(p => p.DateTime)
                                            .FirstOrDefault();
            }
            if (faxData == null || faxData.Result != "Success")
            {
                done = false;
            }               

            return done;
        }

        /// <summary>
        /// Consolidating the Fax Report from HTml to list format
        /// </summary>
        /// <param name="HtmlString"></param>
        /// <returns></returns>
        private List<FaxReport> ConsolidateFaxReport(string HtmlString)
        {
            List<FaxReport> Report = new List<FaxReport>();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HtmlString);
            var metaTags = htmlDoc.DocumentNode.SelectNodes("//tr");
            if (metaTags != null)
            {
                foreach (var tag in metaTags)
                {
                    FaxReport reportnode = new FaxReport();
                    HtmlNode Job = tag.FirstChild;
                    reportnode.JobID = Job.InnerHtml;

                    HtmlNode DateTime = Job.NextSibling;
                    reportnode.DateTime = DateTime.InnerHtml;

                    HtmlNode User = DateTime.NextSibling;
                    reportnode.User = User.InnerHtml;

                    HtmlNode Type = User.NextSibling;
                    reportnode.Type = Type.InnerHtml;

                    HtmlNode Identification = Type.NextSibling;
                    reportnode.Identification = Identification.InnerHtml;

                    HtmlNode Duration = Identification.NextSibling;
                    reportnode.Duration = Duration.InnerHtml;

                    HtmlNode Pages = Duration.NextSibling;
                    reportnode.Pages = Pages.InnerHtml;

                    HtmlNode Result = Pages.NextSibling;
                    reportnode.Result = Result.InnerHtml;

                    Report.Add(reportnode);
                }
            }
            return Report;
        }
    }

    /// <summary>
    /// Holds Fax Report Data. 
    /// This data would be retrieved from the Fax Report on Device Control Panel
    /// </summary>
    public class FaxReport
    {
        /// <summary>
        /// Unique Job Id for each fax Job 
        /// </summary>
        public string JobID { get; set; }
        /// <summary>
        /// Date and Time of Fax Receival
        /// </summary>
        public string DateTime { get; set; }
        /// <summary>
        /// Fax Sender Name
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Fax Type Received/Sent
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Fax Sender Unique Identifier
        /// </summary>
        public string Identification { get; set; }
        /// <summary>
        /// Fax Receival Duration
        /// </summary>
        public string Duration { get; set; }
        /// <summary>
        /// Number of pages Fax sent/received
        /// </summary>
        public string Pages { get; set; }
        /// <summary>
        ///  Fax Result
        /// </summary>
        public string Result { get; set; }
    }
}
