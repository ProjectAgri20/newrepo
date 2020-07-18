using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using System.Windows.Forms;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.PluginSupport.Connectivity
{

    /// <summary>
    /// Provides generic methods for test execution and reporting.
    /// </summary>
    public class CtcBaseTests
    {
        #region Constants

        const string START_TAG = "Test case execution started: ";
        const string END_TAG = "Test case execution completed: ";
        const string LOGS_DIR = "Logs";
        const string LOG_FILE_ADMIN = "{0}-{1}.log";

#pragma warning disable 1591

        public const string TEST_PREREQUISITES = "*************************** Pre-requisites ***************************";
        public const string TEST_EXECUTION = "***************************** Execution ******************************";
        public const string TEST_POSTREQUISITES = "************************** Post-requisites ***************************";
        public const string ROUTER_PREREQUISITES = "************************ Router Pre-requisites ***********************";
        public const string ROUTER_POSTREQUISITES = "************************ Router Post-requisites **********************";
        public const string PACKET_VALIDATION = "********************** Packet Validation *****************************";
        public const string LINUX_PREREQUISITES = "************************ Linux Pre-requisites ************************";
        public const string STEP_DELIMETER = " @@@ ";
        public const string FAILURE_PREFIX = "Fail : ";
        public const string INTERFACE_HIGHLIGHTER = "********************* {0} ****************************";
        public const string STEP_POSTREQUISITES = "************************** STEP Post-requisites ***************************";

#pragma warning restore 1591

        #endregion

        #region Local Variables

        private string _productFamily;

        private string _productName;

        private string _sliver;

        private Dictionary<int, bool> _executedTests;

        private string _firmwareVersion;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the Product Family
        /// </summary>
        public string ProductFamily
        {
            set
            {
                _productFamily = value;
            }

            get
            {
                return _productFamily;
            }
        }

        /// <summary>
        /// Gets or Sets the Product Name
        /// </summary>
        public string ProductName
        {
            set
            {
                _productName = value;
            }

            get
            {
                return _productName;
            }
        }

        /// <summary>
        /// Gets or Sets the Sliver
        /// </summary>
        public string Sliver
        {
            set
            {
                _sliver = value;
            }

            get
            {
                return _sliver;
            }
        }

        /// <summary>
        /// Gets or Sets the Wired/Wireless Network Connectivity.
        /// </summary>
        public ConnectivityType NetworkConnectivity { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the base test class
        /// </summary>
        /// <param name="productName">Product Name</param>
        public CtcBaseTests(string productName)
        {
            _productName = productName;

            if (string.IsNullOrEmpty(_productName))
            {
                _productName = "UnknownProdName";
            }

            _executedTests = new Dictionary<int, bool>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs the test from the derived class which has the test number
        /// </summary>
        /// <param name="executionData">Plugin execution data</param>
        /// <param name="testNumber">test case number to be executed</param>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="productFamily"><see cref=" ProductFamilies"/></param>
        public void RunTest(PluginExecutionData executionData, string testNumber, IPAddress ipAddress, ProductFamilies productFamily)
        {
            int number;

            if (int.TryParse(testNumber, out number))
            {
                RunTest(executionData, number, ipAddress, productFamily);
            }
        }

        /// <summary>
        /// Runs the test from the derived class which has the test number
        /// </summary>
        /// <param name="executionData">Plugin execution data</param>
        /// <param name="testNumber">test case number to be executed</param>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="productFamily"><see cref=" ProductFamilies"/></param>
        public void RunTest(PluginExecutionData executionData, int testNumber, IPAddress ipAddress, ProductFamilies productFamily)
        {
            bool previousResult;

            _executedTests.TryGetValue(testNumber, out previousResult);

            // Run only if the test is failed in the previous run
            if (!previousResult)
            {
                TestDetailsAttribute testDetails = null;

                DateTime startTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                bool result = false;

                try
                {
                    // extract the method from the current executing assembly and class
                    MethodInfo info = GetTestMethod(testNumber, ref testDetails);

                    if (null != info)
                    {
                        TraceFactory.Logger.Info(START_TAG + testNumber);

                        _firmwareVersion = GetPrinterFirmwareVersion(ipAddress, productFamily);

                        // get the start time
                        startTime = DateTime.Now;

                        UpdateStatus("Test {0} started".FormatWith(testNumber));

                        result = (bool)info.Invoke(this, null);

                        UpdateStatus("Test {0} completed".FormatWith(testNumber));

                        // get the end time
                        endTime = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    // Normally, we do not like catching all exceptions, but in this situation we don't know
                    // what kind of exception will be thrown by the plug-ins.

                    TraceFactory.Logger.Info("Test case thrown exception : " + testNumber);
                    TraceFactory.Logger.Info("Error Information : " + ex.Message);
                    TraceFactory.Logger.Debug(ex.InnerException?.StackTrace == null ? string.Empty : ex.InnerException.StackTrace);

                    endTime = DateTime.Now;
                    result = false;
                }
                finally
                {
                    TraceFactory.Logger.Info(END_TAG + testNumber);
                    TraceFactory.Logger.Debug("Test result: {0}".FormatWith(result ? "Passed" : "Failed"));

                    // get log data from the log file.
                    var logData = GetLogData(testNumber, executionData.SessionId);

                    _executedTests.Remove(testNumber);
                    _executedTests.Add(testNumber, result);

                    // Tests for Wired and Wireless remain same for Print and IPconfiguratio plug-in; hence based on 'NetworkConnectivity' selection on plug-in, wired/ wireless mode is updated on the reports.
                    // For other plug-in's where NetworkConnectivity is not set, wired/ wireless mode is taken from the 'TestDetailsAttribute'
                    if (NetworkConnectivity == ConnectivityType.None)
                    {
                        // save the report after every test run, this will enable to the live report data
                        SaveReport(executionData, testNumber, testDetails.Category, testDetails.Description, startTime, endTime, result, logData, testDetails.Protocol.ToString(),
                            testDetails.Connectivity.ToString(), _firmwareVersion, testDetails.PrintProtocol.ToString());
                    }
                    else
                    {
                        SaveReport(executionData, testNumber, testDetails.Category, testDetails.Description, startTime, endTime, result, logData, testDetails.Protocol.ToString(),
                            NetworkConnectivity.ToString(), _firmwareVersion, testDetails.PrintProtocol.ToString());
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the Method Info from the derived class methods which is having the test number
        /// </summary>
        /// <param name="testNumber">test case number</param>
        /// <param name="testDetails">If the method is found the corresponding method test details will be extracted</param>
        /// <returns></returns>
        private MethodInfo GetTestMethod(int testNumber, ref TestDetailsAttribute testDetails)
        {
            // walk thru all the methods and return the method which is having the test number as user requested
            foreach (MethodInfo method in GetType().GetMethods())
            {
                object[] attrs = method.GetCustomAttributes(new TestDetailsAttribute().GetType(), false);

                if (attrs.Length > 0)
                {
                    // since test methods are having only the TestDetails type custom attributes, so we can cast to this type
                    testDetails = (TestDetailsAttribute)attrs[0];

                    if (testDetails.Id == testNumber)
                    {
                        return method;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Save the report data based on the report type
        /// </summary>
        /// <param name="executionData">Plugin execution data</param>
        /// <param name="testNumber">Test case number</param>
        /// <param name="category">Test Category</param>
        /// <param name="description">Test Description</param>
        /// <param name="startTime">Start Time</param>
        /// <param name="endTime">End Time</param>
        /// <param name="result">Test Result</param>
        /// <param name="logData">Log Data of the Test</param>
        /// <param name="connectivity">IPv4/IPv6 Connectivity</param>
        /// <param name="mode">Wired/WireLess Mode</param>
        /// <param name="firmwareVersion">Firmware version of the printer</param>
        /// <param name="printProtocol">Print protocol RAW, LPD, IPP etc.</param>
        private void SaveReport(PluginExecutionData executionData, int testNumber, string category, string description, DateTime startTime, DateTime endTime, bool result, string logData, string connectivity, string mode, string firmwareVersion, string printProtocol = null)
        {
            // use the Data logger component to add the row to the ConnectivityTestDetails table in ScalableTestDataLog DB
            ConnectivityTestDetailLog log = new ConnectivityTestDetailLog(executionData);

            log.TestId = testNumber;
            log.ProductFamily = _productFamily;
            log.ProductName = _productName;
            log.TestName = "Empty"; // TODO: Currently CTC tests doesn't have the Name
            log.Category = category;
            log.Description = description;
            log.StartTime = startTime;
            log.EndTime = endTime;
            log.Result = result;
            log.ErrorDetails = logData;
            log.Sliver = _sliver;
            log.Connectivity = connectivity;
            log.Mode = mode;
            log.PrintProtocol = printProtocol;
            log.FirmwareVersion = firmwareVersion;

            // Submit the data to the service
            ExecutionServices.DataLogger.Submit(log);
        }

        /// <summary>
        /// Gets the log data to the reports based on the log style
        /// </summary>
        /// <param name="testNumber">Test number</param>
        /// <returns>Log data to be included in the reports</returns>
        protected string GetLogData(int testNumber, string sessionId = "")
        {
            string logData = GetTestLogFromFile(testNumber, sessionId);

            // replace the new line characters with "|" so that it can be displayed in reports with proper format
            logData = logData.Replace(Environment.NewLine, "|");

            return UpdateLog(logData);
        }

        /// <summary>
        /// It will fetch the log information for the corresponding test case.
        /// If the log file contains more than one entry for the test, it will fetch latest log information.
        /// </summary>
        /// <param name="testNumber">Test case number for which log information will be fetched</param>
        /// <returns>Returns the log information for the test case</returns>
        protected string GetTestLogFromFile(int testNumber, string sessionId = "")
        {
            // MessageBox.Show("In GetTestLogFrom File");

            string basePath = string.Empty;
            string logFilePath = string.Empty;
            //This is not working so changing it to old way 
             string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
          basePath = Path.Combine(location, "Logs");
          //TraceFactory.Logger.Info("Base Path : {0}".FormatWith(basePath));
                       
            //-------------------------------------------------------------
            //Old method is working
         //  string basePath_old = LogFileReader.DataLogPath();
          // TraceFactory.Logger.Info("basepath : {0}".FormatWith(basePath_old));
            TraceFactory.Logger.Info("Username : {0}".FormatWith(Environment.UserName));
            //// First look for the file in the ST Office Worker location
            string logFileName = LOG_FILE_ADMIN.FormatWith(Environment.UserName, TraceFactory.GetThreadContextProperty("ProcessId"));
            TraceFactory.Logger.Info("logfilename : {0}".FormatWith(logFileName));
            // logFilePath = Path.Combine(basePath, logFileName);
            //comments but working
            //logFilePath = Path.Combine(basePath_old, logFileName);
           TraceFactory.Logger.Info("STF logfilepath : {0}".FormatWith(logFilePath));
           
            if (!File.Exists(logFilePath))
            {
                TraceFactory.Logger.Info("For STB Results");
                // Not found there - use the STB Solution Tester location instead
              
                string STBLogFilePath = Path.Combine(basePath, sessionId);
                TraceFactory.Logger.Info("STB-logFilePath without filename: {0}".FormatWith(STBLogFilePath));
              
                string[] stbLogFilename = Directory.GetFiles(STBLogFilePath, "*.log");
                logFilePath = stbLogFilename[0];
                TraceFactory.Logger.Info("STB Log File path new:{0}".FormatWith(logFilePath));
            }

            // check whether log exists or not
            if (File.Exists(logFilePath))
            {
                StringBuilder testLog = new StringBuilder();

                try
                {
                    int startLinePos = -1;
                    int currentPosition = -1;

                    // read all the lines from log file into string array
                    Collection<string> lines = GetLogLines(logFilePath);
                    TraceFactory.Logger.Info("No of lines : {0}".FormatWith(lines));
                    // if the Repeat session is executed then there are multiple start tags may appear in the log file, always pick the latest log
                    // find the line number from where it should start the logs.
                    foreach (string line in lines)
                    {
                        currentPosition++;

                        if (line.Contains(START_TAG + testNumber))
                        {
                            startLinePos = currentPosition;
                            // Move the pointer to the next 
                            // start reading the log data from the next line onwards
                            ++startLinePos;
                        }
                    }

                    if (-1 == startLinePos)
                    {
                        startLinePos = 0;
                        testLog.AppendLine("Test {0} logs has exceeded file size.{1}Showing logs from the available log data.".FormatWith(testNumber, Environment.NewLine));
                    }

                    for (int i = startLinePos; i < lines.Count; i++)
                    {
                        string line = lines[i];

                        if (line.Contains(END_TAG + testNumber))
                        {
                            break;
                        }
                        else
                        {
                            testLog.AppendLine(line);
                        }
                    }

                    return testLog.ToString();
                }
                catch (IndexOutOfRangeException)
                {
                    // suppress the exception
                    return testLog.ToString();
                }
            }
            else
            {
                TraceFactory.Logger.Info("Log File not found at the location or is Empty");
                return string.Empty;
            }
        }

        /// <summary>
        /// Read all the files content into collection
        /// </summary>
        /// <param name="fileName">Log file name with path</param>
        /// <returns>Collection </returns>
        private Collection<string> GetLogLines(string fileName)
        {
            FileStream fs = null;

            try
            {
                Collection<string> lines = new Collection<string>();

                // load the log file
                fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        lines.Add(sr.ReadLine());
                    }

                    fs = null;
                }

                return lines;
            }
            finally
            {
                fs?.Dispose();
            }
        }

        /// <summary>
        /// Updates the status message on the management console.
        /// </summary>
        /// <param name="status">status message</param>
        private void UpdateStatus(string status)
        {
            //TODO: Migration issue
            //EngineEventBus.OnActivityStatusMessageChanged(this, status);
        }

        /// <summary>
        /// Returns Printer firmware version
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="productFamilies"><see cref=" ProductFamilies"/></param>
        /// <returns></returns>
        private string GetPrinterFirmwareVersion(IPAddress ipAddress, ProductFamilies productFamilies)
        {
            try
            {
                if (string.IsNullOrEmpty(_firmwareVersion))
                {
                    Printer.PrinterFamilies family = (Printer.PrinterFamilies)Enum.Parse(typeof(Printer.PrinterFamilies), Enum<ProductFamilies>.Value(productFamilies));
                    Printer.Printer printer = Printer.PrinterFactory.Create(family, ipAddress);

                    _firmwareVersion = printer.FirmwareVersion;
                }
            }
            catch (Exception ex)
            {
                _firmwareVersion = "Not Available";
                TraceFactory.Logger.Info("Unable to get Firmware version");
                TraceFactory.Logger.Debug("Exception details: {0}".FormatWith(ex.JoinAllErrorMessages()));
            }

            return _firmwareVersion;
        }

        /// <summary>
        /// Update the logs based on the plug-in specific requirements.
        /// Firewall: All the logs between Validation to Validation Summary will be changed from Info to Debug [I] to [D]
        /// </summary>
        /// <param name="logData">The log data</param>
        /// <returns>Updated log data based on the plug-in requirements</returns>
        private string UpdateLog(string logData)
        {
            // Update the logs for Firewall Plug-in
            if (base.ToString().EndsWith("FirewallTests"))
            {
                StringBuilder updatedLogData = new StringBuilder();

                // This log data contains the '|' as new line character
                // split with '|' and walk thru

                bool startUpdate = false;

                foreach (string line in logData.Split('|'))
                {
                    if (line.Contains("*************** Validating Services"))
                    {
                        startUpdate = true;
                    }

                    if (line.Contains("****************** Validation Summary"))
                    {
                        startUpdate = false;
                    }

                    if (startUpdate)
                    {
                        updatedLogData.AppendLine(line.Replace("[I]", "[D]"));
                    }
                    else
                    {
                        updatedLogData.AppendLine(line);
                    }
                }

                return updatedLogData.ToString().Replace(Environment.NewLine, "|");
            }

            return logData;
        }

        #endregion
    }
}

