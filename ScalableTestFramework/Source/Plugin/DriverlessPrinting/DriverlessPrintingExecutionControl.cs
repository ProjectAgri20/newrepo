using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DriverlessPrinting
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
    public partial class DriverlessPrintingExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PluginExecutionData _executionData;
        private DeviceWorkflowLogger _performanceLogger;
        private DriverlessPrintingActivityData _activityData;
        private PluginExecutionResult _result = new PluginExecutionResult(PluginResult.Passed);

        /// <summary>
        ///
        /// </summary>
        public DriverlessPrintingExecutionControl()
        {
            InitializeComponent();
        }

        #region IPluginExecutionEngine implementation

        /// <summary>
        /// Executes this plug-in's workflow using the specified <see cref="PluginExecutionData"/>.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult"/> indicating the outcome of the
        /// execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _executionData = executionData;
            _performanceLogger = new DeviceWorkflowLogger(_executionData);
            _activityData = _executionData.GetMetadata<DriverlessPrintingActivityData>();
            var printer = executionData.Assets.OfType<PrintDeviceInfo>().FirstOrDefault();
            if (printer == null)
                return new PluginExecutionResult(PluginResult.Skipped, "No assets available for execution.");
            if(!printer.Attributes.HasFlag(AssetAttributes.Printer))
                return new PluginExecutionResult(PluginResult.Skipped, "The device has no print capability.");
            var address = printer.Address;

            var iteratorMode = _activityData.ShuffleDocuments
                ? CollectionSelectorMode.ShuffledRoundRobin
                : CollectionSelectorMode.Random;
            var documentIterator = new DocumentCollectionIterator(iteratorMode);
            var document = documentIterator.GetNext(executionData.Documents);
            FileInfo localFile = ExecutionServices.FileRepository.GetFile(document);

            if (_activityData.PinProtected)
            {
                AddPinProtection(localFile, _activityData.Pin);
            }

            if (_activityData.PrintMethod == PrintMethod.Random)
            {
                Random newRandom = new Random(4);
                var randomInt = newRandom.Next(0, 999) % 4;
                _activityData.PrintMethod = (PrintMethod)randomInt;
            }

            _performanceLogger.RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
            UpdateStatus($"Printing {document.FileName} via {_activityData.PrintMethod}.");
            switch (_activityData.PrintMethod)
            {
                default:
                    Print9100(address, localFile);
                    break;

                case PrintMethod.Ftp:
                    PrintFtp(address, "admin", printer.AdminPassword, localFile, true);
                    break;

                case PrintMethod.Ipp:
                    PrintIpp(address, localFile);
                    break;

                case PrintMethod.Ews:
                    PrintEws(address, printer.AdminPassword, localFile);
                    break;
            }
            _performanceLogger.RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
            _performanceLogger.RecordExecutionDetail(DeviceWorkflowMarker.PrintJobEnd, _activityData.PrintMethod.ToString());
            ActivityExecutionDocumentUsageLog documentLog = new ActivityExecutionDocumentUsageLog(executionData, document);
            ExecutionServices.DataLogger.Submit(documentLog);

            if (_activityData.PrintJobSeparator)
            {
                UpdateStatus("Printing Job Separator.");
                PrintJobSeparator(address);
            }
            localFile.Delete();
            return _result;
        }

        private void AddPinProtection(FileInfo localFile, int pin)
        {
            var prnFile = File.ReadAllText(localFile.FullName, Encoding.Default);
            var prnFileLines = prnFile.Split('\n').ToList();

            int insertLine;
            var jobStorageTag = prnFileLines.FindIndex(x => x.Contains("SET HOLD"));
            if (jobStorageTag != -1)
            {
                insertLine = jobStorageTag;
                prnFileLines.RemoveAt(jobStorageTag);
                int holdTypeLine = prnFileLines.FindIndex(x => x.Contains("SET HOLDTYPE="));
                if (holdTypeLine >= 0)
                {
                    prnFileLines.RemoveAt(holdTypeLine);
                }

                int holdKeyLine = prnFileLines.FindIndex(x => x.Contains("SET HOLDKEY="));
                if (holdKeyLine >= 0)
                    prnFileLines.RemoveAt(holdKeyLine);
            }
            else
            {
                //prnFileLines.Insert();
                insertLine = prnFileLines.IndexOf("@PJL ENTER LANGUAGE=PCLXL");
            }
            prnFileLines.Insert(insertLine, "@PJL SET HOLD=ON");
            prnFileLines.Insert(insertLine + 1, "@PJL SET HOLDTYPE=Private");
            prnFileLines.Insert(insertLine + 2, $"@PJL SET HOLDKEY={pin:D4}");

            //we have to modify the username tag so that we have consistent folder name where these files will be dropped to
            //this will allow us to use printfromjobstorage plugin
            //we will set the user name to EDTSTB
            insertLine = prnFileLines.FindIndex(x => x.Contains("SET USERNAME"));
            if (insertLine > 0)
                prnFileLines[insertLine] = "@PJL SET USERNAME=\"EDTSTB\"";

            var modifiedText = string.Join("\n", prnFileLines);

            File.WriteAllText(localFile.FullName, modifiedText, Encoding.Default);
        }

        /// <summary>
        /// Prints document using IPP Protocol with IPP tool
        /// </summary>
        /// <param name="address"></param>
        /// <param name="localFileDocument"></param>

        private void PrintIpp(string address, FileInfo localFileDocument)
        {
            string ipptoolpath = @"C:\Program Files (x86)\ipptool\ipptool.exe";

            if (!File.Exists(ipptoolpath))
            {
                _result = new PluginExecutionResult(PluginResult.Failed, "IPP Tool is not installed, IPP Print cannot proceed");
                return;
            }

            var testFileData = GetTestFile(localFileDocument.Name);
            string testFilePath = Path.Combine(Path.GetTempPath(), "ipptest.test");
            using (var testFile = File.Create(testFilePath))
            {
                testFile.Write(testFileData, 0, testFileData.Length);
                testFile.Flush();
            }

            string args = $"-f \"{localFileDocument.FullName}\" http://{address}:631/ipp \"{testFilePath}\"";

            var ippInfo =
                new ProcessStartInfo(ipptoolpath, args)
                {
                    WorkingDirectory = Path.GetDirectoryName(ipptoolpath),
                    UseShellExecute = true
                };
            var ippProcess = Process.Start(ippInfo);
            if (ippProcess != null && ippProcess.WaitForExit((int)TimeSpan.FromMinutes(1).TotalMilliseconds))
            {
                _result = new PluginExecutionResult(PluginResult.Passed);
            }
            else
            {
                _result = new PluginExecutionResult(PluginResult.Failed, "IPP Print Job failed");
            }
        }

       

        /// <summary>
        /// Prints document through FTP using webclient
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="localFileDocument"></param>
        /// <param name="isPassiveMode"></param>
        private void PrintFtp(string ipAddress, string userName, string password, FileInfo localFileDocument, bool isPassiveMode = false)
        {
            // Enclose address with square brackets if IPv6 address
            if (ipAddress.Contains(":"))
            {
                ipAddress = $"[{ipAddress}]";
            }

            var ftpWebRequest = (FtpWebRequest)WebRequest.Create($"ftp://{ipAddress}/{localFileDocument.Name}");
            ftpWebRequest.Credentials = new NetworkCredential(userName, password);
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.UsePassive = isPassiveMode;
            ftpWebRequest.KeepAlive = true;
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpWebRequest.Timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
            using (var ftpStream = ftpWebRequest.GetRequestStream())
            {
                using (Stream stream = new FileStream(localFileDocument.FullName, FileMode.Open, FileAccess.Read))
                {
                    stream.CopyTo(ftpStream);
                }
            }

            FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse();
            _result = response.StatusCode == FtpStatusCode.ClosingData ? new PluginExecutionResult(PluginResult.Passed) : new PluginExecutionResult(PluginResult.Failed, "FTP Printing failed");
            response.Close();
            ftpWebRequest.Abort();
        }

        /// <summary>
        /// Prints document using raw port
        /// </summary>
        /// <param name="address"></param>
        /// <param name="localFileDocument"></param>
        private void Print9100(string address, FileInfo localFileDocument)
        {
            using (TcpClient client = new TcpClient(AddressFamily.InterNetwork))
            {
                client.Connect(IPAddress.Parse(address), 9100);
                if (client.Connected)
                {
                    using (var printStream = client.GetStream())
                    {
                        using (Stream stream = new FileStream(localFileDocument.FullName, FileMode.Open, FileAccess.Read))
                        {
                            stream.CopyTo(printStream);
                        }
                    }
                }
                else
                {
                    _result = new PluginExecutionResult(PluginResult.Failed, "Could not connect to printer.");
                }
            }
        }

        /// <summary>
        /// Prints document using EWS
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="password"></param>
        /// <param name="localFileDocument"></param>
        private void PrintEws(string ipAddress, string password, FileInfo localFileDocument)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            string sessionId = SignInOmni(ipAddress, password);
            var csrfToken = GetCsrfToken($"https://{ipAddress}/hp/device/SignIn/Index", ref sessionId);
            var authenticatorRequest = (HttpWebRequest)WebRequest.Create($"https://{ipAddress}/hp/device/Print/Print");
            authenticatorRequest.Method = "POST";

            authenticatorRequest.Accept = "text/html, application/xhtml+xml, image/jxr, */*";

            string boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x16", CultureInfo.CurrentCulture);
            authenticatorRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            authenticatorRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            authenticatorRequest.KeepAlive = true;

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(new Cookie("sessionId", sessionId) { Domain = authenticatorRequest.RequestUri.Host });
            authenticatorRequest.CookieContainer = cookieContainer;
            authenticatorRequest.Headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");

            using (Stream requestStream = authenticatorRequest.GetRequestStream())
            {
                boundary = "--" + boundary;
                var boundarybytes = Encoding.ASCII.GetBytes(boundary + "\r\n");
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"CSRFToken\"\r\n\r\n";
                byte[] headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                headerBytes = Encoding.UTF8.GetBytes(csrfToken + "\r\n");
                requestStream.Write(headerBytes, 0, headerBytes.Length);
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                headerTemplate = "Content-Disposition: form-data; name=\"fileToPrint\"; filename=\"" + localFileDocument.Name + "\"\r\n";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                headerTemplate = "Content-Type: application/octet-stream\r\n\r\n";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                using (Stream stream = new FileStream(localFileDocument.FullName, FileMode.Open, FileAccess.Read))
                {
                    stream.CopyTo(requestStream);
                }
                headerTemplate = "\r\n";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                headerTemplate = "Content-Disposition: form-data; name=\"NumberOfCopies\"\r\n\r\n" + "1\r\n";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                headerTemplate = "Content-Disposition: form-data; name=\"EnableCollatedCopies\"\r\n\r\n" + "on\r\n";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                headerTemplate = "Content-Disposition: form-data; name=\"FormButtonSubmit\"\r\n\r\n" + "Print\r\n";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                var footerBoundary = boundary + "--\r\n";
                boundarybytes = Encoding.ASCII.GetBytes(footerBoundary);
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
            }

            var configResponse = HttpMessenger.ExecuteRequest(authenticatorRequest);

            if (configResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Printing file through EWS failed.");
            }
        }

        /// <summary>
        /// Gets the control file required for IPP printing based on the media size
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private byte[] GetTestFile(string fileName)
        {
            var fileAttribs = fileName.Split('_');
            string mediaSizeIndicator = fileAttribs.ElementAt(1);

            if (mediaSizeIndicator.StartsWith("A3", StringComparison.OrdinalIgnoreCase))
            {
                return Properties.Resources.Print_pdf_Media_A3;
            }

            if (mediaSizeIndicator.StartsWith("A4", StringComparison.OrdinalIgnoreCase))
            {
                return Properties.Resources.Print_pdf_Media_A4;
            }

            if (mediaSizeIndicator.StartsWith("Exec", StringComparison.OrdinalIgnoreCase))
            {
                return Properties.Resources.Print_pdf_Media_Exec;
            }

            if (mediaSizeIndicator.StartsWith("Ldg", StringComparison.OrdinalIgnoreCase))
            {
                return Properties.Resources.Print_pdf_Media_Ledger;
            }

            if (mediaSizeIndicator.StartsWith("Lgl", StringComparison.OrdinalIgnoreCase))
            {
                return Properties.Resources.Print_pdf_Media_Legal;
            }

            if (mediaSizeIndicator.StartsWith("Ltr", StringComparison.OrdinalIgnoreCase))
            {
                return Properties.Resources.Print_pdf_Media_Letter;
            }

            //send letter as default
            return Properties.Resources.Print_pdf_Media_Letter;
        }

        /// <summary>
        /// Prints a job seperator
        /// </summary>
        /// <param name="address"></param>
        private void PrintJobSeparator(string address)
        {
            StringBuilder strFileContent = PrintTag();
            string tagfileName = Path.Combine(Path.GetTempPath(), _executionData.ActivityExecutionId + ".txt");

            File.WriteAllText(tagfileName, strFileContent.ToString(), Encoding.ASCII);

            FileInfo tagFileInfo = new FileInfo(tagfileName);
            Print9100(address, tagFileInfo);
            File.Delete(tagfileName);
        }

        /// <summary>
        /// Contents of Job Separator
        /// </summary>
        /// <returns></returns>
        private StringBuilder PrintTag()
        {
            StringBuilder strFileContent = new StringBuilder();
            strFileContent.AppendLine();
            strFileContent.AppendLine();
            strFileContent.AppendLine($"UserName: {Environment.UserName}");
            strFileContent.AppendLine($"Session ID: {_executionData.SessionId}");
            strFileContent.AppendLine($"Activity ID:{_executionData.ActivityExecutionId}");
            strFileContent.AppendLine($"Date: {DateTime.UtcNow.ToShortDateString()}");
            strFileContent.AppendLine($"Time: {DateTime.UtcNow.ToShortTimeString()}");
            return strFileContent;
        }

        protected virtual void UpdateStatus(string statusMsg)
        {
            pluginStatus_TextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });

            ExecutionServices.SystemTrace.LogInfo(statusMsg);
        }

        private string SignInOmni(string address, string password)
        {
            string sessionId = string.Empty;// = GetSessionId(address);
            string postData;

            var csrfToken = GetCsrfToken($"https://{address}/hp/device/SignIn/Index", ref sessionId);
            password = Uri.EscapeDataString(password);
            HttpWebRequest signinRequest = (HttpWebRequest)WebRequest.Create($"https://{address}/hp/device/SignIn/Index");
            signinRequest.CookieContainer = new CookieContainer();
            signinRequest.CookieContainer.Add(new Cookie("sessionId", sessionId) { Domain = address });
            signinRequest.Method = "POST";
            signinRequest.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            signinRequest.ContentType = "application/x-www-form-urlencoded";
            signinRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            signinRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");

            if (string.IsNullOrEmpty(csrfToken))
            {
                postData = $"agentIdSelect=hp_EmbeddedPin_v1&DomainDropDown=&UserNameTextBox=&PinDropDown=AdminItem&PasswordTextBox={password}&signInOk=Sign+In";
            }
            else
            {
                postData = $"CSRFToken={csrfToken}&agentIdSelect=hp_EmbeddedPin_v1&DomainDropDown=&UserNameTextBox=&PinDropDown=AdminItem&PasswordTextBox={password}&signInOk=Sign+In";
            }
            byte[] buffer = Encoding.ASCII.GetBytes(postData);
            signinRequest.ContentLength = buffer.Length;
            using (Stream stream = signinRequest.GetRequestStream())
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }

            try
            {
                var signinResponse = (HttpWebResponse)signinRequest.GetResponse();

                if (signinResponse.StatusCode == HttpStatusCode.OK || signinResponse.StatusCode == HttpStatusCode.Moved || signinResponse.StatusCode == HttpStatusCode.Found)
                {
                    signinResponse.Close();

                    sessionId = signinRequest.Headers["Cookie"];
                    sessionId = sessionId.Substring(0, sessionId.IndexOf(";", StringComparison.Ordinal));
                    sessionId = sessionId.Replace("sessionId=", string.Empty);
                }

                return sessionId;
            }
            catch (WebException webException)
            {
                throw new Exception("Failed to login", webException);
            }
        }

        private static string GetCsrfToken(string urlAddress, ref string sessionId)
        {
            string csrfToken = string.Empty;
            HttpWebRequest signinCsrfRequest =
                (HttpWebRequest)WebRequest.Create(urlAddress);
            signinCsrfRequest.Method = "GET";
            if (!string.IsNullOrEmpty(sessionId))
            {
                signinCsrfRequest.Headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");
            }
            //get the CSRF token from the response
            var signinResponse = (HttpWebResponse)signinCsrfRequest.GetResponse();
            if (signinResponse.StatusCode == HttpStatusCode.OK)
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    sessionId = signinResponse.Headers.GetValues("Set-Cookie")?.FirstOrDefault()?.Split(';').FirstOrDefault();
                    sessionId = sessionId?.Replace("sessionId=", string.Empty);
                }

                using (var responseStream = signinResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            string responseBodyString = reader.ReadToEnd();
                            int startIndex = responseBodyString.IndexOf("name=\"CSRFToken\" value=\"",
                                StringComparison.OrdinalIgnoreCase);
                            if (startIndex != -1)
                            {
                                responseBodyString = responseBodyString.Substring(startIndex);
                                int endIndex = responseBodyString.IndexOf("\" />", StringComparison.OrdinalIgnoreCase);
                                csrfToken = responseBodyString.Substring("name=\"CSRFToken\" value=\"".Length,
                                    endIndex - "name=\"CSRFToken\" value=\"".Length);
                                //csrfToken = HttpUtility.UrlEncode(csrfToken);
                            }
                        }

                        responseStream.Close();
                    }
                }
            }
            signinResponse.Close();
            return csrfToken;
        }

        #endregion IPluginExecutionEngine implementation
    }
}