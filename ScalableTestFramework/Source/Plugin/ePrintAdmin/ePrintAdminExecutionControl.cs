using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Web;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ePrintAdmin
{
    [ToolboxItem(false)]
    public partial class ePrintAdminExecutionControl : UserControl, IPluginExecutionEngine
    {
        private ePrintAdminActivityData _activityData;

        //private PluginExecutionData _executionData = null;
        private NetworkCredential _credential;

        private HP.DeviceAutomation.IDevice _device;
        private string _userDnsName;
        private string _ePrintServerIp;
        private string _ePrintServerType = "http";
        public ePrintAdminExecutionControl()
        {
            InitializeComponent();
            activityStatus_dataGridView.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Sets up the scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            bool activityFailed = false;
            StringBuilder failedMessages = new StringBuilder();
            _activityData = executionData.GetMetadata<ePrintAdminActivityData>();
            _credential = executionData.Credential;

            PrintDeviceInfo printDeviceInfo = (PrintDeviceInfo)executionData.Assets.First();
            _device = DeviceConstructor.Create(printDeviceInfo);

            _userDnsName = executionData.Environment.UserDnsDomain;
            _ePrintServerIp = executionData.Servers.First().Address;
            CookieCollection loginCookie = new CookieCollection();
            if (Login(loginCookie).Result == PluginResult.Failed)
            {
                return new PluginExecutionResult(PluginResult.Failed, "Unable to login to eprint server");
            }

            foreach (var eprintTask in _activityData.ePrintAdminTasks)
            {
                activityStatus_dataGridView.DataSource = null;
                try
                {
                    switch (eprintTask.Operation)
                    {
                        case EprintAdminToolOperation.AddPrinteripv4:
                        case EprintAdminToolOperation.AddPrinterHpac:
                        case EprintAdminToolOperation.AddPrinterPJL:
                            {
                                ePrintAddPrinter(eprintTask, loginCookie);
                            }
                            break;

                        case EprintAdminToolOperation.DeletePrinter:
                            {
                                ePrintDeletePrinter(eprintTask, loginCookie);
                            }
                            break;

                        case EprintAdminToolOperation.ImportPrinter:
                            {
                                ePrintImportPrinter(eprintTask, loginCookie);
                            }
                            break;

                        case EprintAdminToolOperation.RegularUser:
                        case EprintAdminToolOperation.GuestUser:
                            {
                                AddUser(eprintTask, loginCookie);
                            }
                            break;
                        case EprintAdminToolOperation.SendPrintJob:
                            {
                                ePrintSendPrintJob(eprintTask, loginCookie);
                            }
                            break;
                    }

                    activityStatus_dataGridView.Visible = false;

                    activityStatus_dataGridView.DataSource = _activityData.ePrintAdminTasks;
                    activityStatus_dataGridView.Visible = true;
                }
                catch (Exception ex)
                {
                    failedMessages.AppendLine($"Failed for ActivityTask:{eprintTask.Operation} with Exception: {ex.Message}");
                    activityFailed = true;
                }
            }
            return activityFailed ? new PluginExecutionResult(PluginResult.Failed, string.Join(",", failedMessages)) : new PluginExecutionResult(PluginResult.Passed, "All test cases passed");
        }

        private PluginExecutionResult Login(CookieCollection loginCookie)
        {
            HttpWebResult loginResult;
            Uri eprintLoginUrl =
                new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/ePrintAuthentication.asmx/Login");
            try
            {
                loginResult = ExecuteLoginWebService(eprintLoginUrl);
                if (loginResult.StatusCode != HttpStatusCode.OK)
                {
                    ExecutionServices.SystemTrace.LogError("Unable to Login to ePrint . Please check Credentials");
                    return new PluginExecutionResult(PluginResult.Failed, "Unable to Login to ePrint . Please check Credentials");
                }
            }
            catch (WebException ex)
            {
                if (ex.Message.Contains("403"))
                {
                    //---------------my changes----------------
                    _ePrintServerType = "https";
                    eprintLoginUrl = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/ePrintAuthentication.asmx/Login");
                    loginResult = ExecuteLoginWebService(eprintLoginUrl);
                    if (loginResult.StatusCode != HttpStatusCode.OK)
                    {
                        ExecutionServices.SystemTrace.LogError("Unable to Login to ePrint . Please check Credentials");
                        return new PluginExecutionResult(PluginResult.Failed, "Unable to Login to ePrint . Please check Credentials");
                    }
                }
                else
                {
                    ExecutionServices.SystemTrace.LogError("Unable to Login to ePrint . Please check Credentials");
                    return new PluginExecutionResult(PluginResult.Failed, "Unable to Login to ePrint . Please check Credentials");
                }
            }

            string setCookie = loginResult.Headers.Get("Set-Cookie");
            string trimmedcookie = setCookie.Substring(12, setCookie.IndexOf(";", StringComparison.Ordinal) - 12);
            loginCookie.Add(new Cookie("LoginCookie", trimmedcookie) { Domain = _ePrintServerIp });
            return new PluginExecutionResult(PluginResult.Passed);
        }

        private HttpWebResult ExecuteLoginWebService(Uri eprintLoginUrl)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(eprintLoginUrl);
            webRequest.Accept = "*/*";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.ContentType = "application/json; charset=UTF-8";
            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            string postData = $"{{\"userName\":\"{_activityData.ePrintAdminUser}\",\"password\":\"{_activityData.ePrintAdminPassword}\",\"createPersistentCookie\":false}}";
            HttpWebResult loginResult = HttpWebEngine.Post(webRequest, postData);
            return loginResult;
        }

        private void AddUser(EprintAdminTask eprintTask, CookieCollection loginCookie)
        {
            //GET previous response to extract view state
            string postData;
            Uri getAddUser = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/users/Edit.aspx");
            HttpWebRequest getAddUserReq = (HttpWebRequest)WebRequest.Create(getAddUser);
            getAddUserReq.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            getAddUserReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            getAddUserReq.Headers.Add("Accept-Encoding", "gzip, deflate");
            getAddUserReq.CookieContainer = new CookieContainer();
            getAddUserReq.CookieContainer.Add(loginCookie);
            getAddUserReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            HttpWebResult importResponse = HttpWebEngine.Get(getAddUserReq);
            ExecutionServices.SystemTrace.LogInfo($"Requesting to add {Environment.UserName} as a {eprintTask.Operation} ");
            //POST
            CookieCollection adminCookie = new CookieCollection();
            string setCookie = importResponse.Headers.Get("Set-Cookie");
            string trimmedcookie = setCookie.Substring(12, setCookie.IndexOf(";", StringComparison.Ordinal) - 12);
            adminCookie.Add(new Cookie("AdminCookie", trimmedcookie) { Domain = _ePrintServerIp });
            Uri addUserUrl = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/users/Edit.aspx");
            HttpWebRequest addUserReq = (HttpWebRequest)WebRequest.Create(addUserUrl);
            addUserReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            addUserReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            addUserReq.ContentType = "application/x-www-form-urlencoded";
            addUserReq.CookieContainer = new CookieContainer();
            addUserReq.CookieContainer.Add(loginCookie);
            addUserReq.CookieContainer.Add(adminCookie);
            addUserReq.Headers.Add("Accept-Encoding", "gzip, deflate");
            addUserReq.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");

            if (eprintTask.Operation.Equals(EprintAdminToolOperation.GuestUser))
            {
                //guest user

                postData = string.Format(Properties.Resources.GuestUserPostData, HttpUtility.UrlEncode(ExtractViewState(importResponse.Response)), _credential.UserName, _userDnsName, DateTime.Today.ToString("MM-dd-yyyy"), DateTime.Today.AddMonths(1).ToString("MM-dd-yyyy"));
            }
            else
            {
                //regular user
                postData = string.Format(Properties.Resources.RegularUserPostData, HttpUtility.UrlEncode(ExtractViewState(importResponse.Response)), _credential.UserName, _userDnsName, DateTime.Today.ToString("MM-dd-yyyy"), DateTime.Today.AddDays(30).ToString("MM-dd-yyyy"));

            }
            HttpWebResult addUserResponse = HttpWebEngine.Post(addUserReq, postData);


            if (addUserResponse.StatusCode != HttpStatusCode.OK)
            {
                ExecutionServices.SystemTrace.LogError($"Failed to  add { (object)Environment.UserName} as a { (object)eprintTask.Operation} ");
                eprintTask.Status = "Failed";
            }
            else
            {
                ExecutionServices.SystemTrace.LogInfo($"Successfully added {Environment.UserName} as a {eprintTask.Operation} ");
                eprintTask.Status = "Passed";
            }
        }

        private void ePrintImportPrinter(EprintAdminTask eprintTask, CookieCollection loginCookie)
        {
            //GET previous response to extract view state
            Uri getImport = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/printers/Import.aspx");
            HttpWebRequest importReq = (HttpWebRequest)WebRequest.Create(getImport);
            importReq.Accept = "text/html, application/xhtml+xml, */*";
            importReq.Headers.Add("Accept-Encoding", "gzip, deflate");
            importReq.CookieContainer = new CookieContainer();
            importReq.CookieContainer.Add(loginCookie);
            importReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            HttpWebResult importPrinterResponse = HttpWebEngine.Get(importReq);

            string file = eprintTask.TargetObject;

            ExecutionServices.SystemTrace.LogInfo($"Requesting to upload file {file}");
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("ctl00$BodyContentPlaceHolder$PrinterImportMessagePanel$ctl00$ImportButton", "Proceed");

            //POST request to upload file

            Uri importPrinterUrl = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/printers/Import.aspx");
            HttpWebRequest importPrinterReq = (HttpWebRequest)WebRequest.Create(importPrinterUrl);
            importPrinterReq.Method = "POST";
            importPrinterReq.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, */*";
            importPrinterReq.Headers.Add("Accept-Encoding", "gzip, deflate");
            importPrinterReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            importPrinterReq.CookieContainer = new CookieContainer();
            importPrinterReq.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            importPrinterReq.CookieContainer.Add(loginCookie);

            //Forming postdata
            HttpWebResult tempResult;
            string contentType = "application/vnd.ms-excel";
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            ServicePointManager.Expect100Continue = false;
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            importPrinterReq.ContentType = "multipart/form-data; boundary=" + boundary;
            using (Stream requestStream = importPrinterReq.GetRequestStream())
            {
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                string headerTemplate = string.Format(Properties.Resources.ImportPrinterHeaderTemplate, boundary, ExtractViewState(importPrinterResponse.Response), Path.GetFileName(file), contentType);

                byte[] headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);
                ExecutionServices.SystemTrace.LogDebug("Writing file bytes to Stream");
                if (string.IsNullOrEmpty(file))
                {
                    byte[] buffer = new byte[4096];
                    requestStream.Write(buffer, 0, 0);
                }
                else
                {
                    using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            requestStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                    requestStream.Write(formitembytes, 0, formitembytes.Length);
                }

                byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                requestStream.Write(trailer, 0, trailer.Length);
                tempResult = HttpWebEngine.Execute(importPrinterReq);
            }
            eprintTask.Status = tempResult.StatusCode == HttpStatusCode.OK ? "Passed" : "Failed";
        }

        private string ExtractViewState(string response)
        {
            string valueDelimiter = "value=\"";
            //gets first occurance of value=" after __VIEWSTATE
            int viewStateValuePosition = response.IndexOf(valueDelimiter, response.IndexOf("__VIEWSTATE", StringComparison.Ordinal), StringComparison.Ordinal);
            int viewStateStartPosition = viewStateValuePosition + valueDelimiter.Length;
            //retrieving viewstate discarding the string value="
            int viewStateEndPosition = response.IndexOf("\"", viewStateStartPosition, StringComparison.Ordinal);
            string result = response.Substring(viewStateStartPosition, viewStateEndPosition - viewStateStartPosition);
            return result;

        }

        private void ePrintDeletePrinter(EprintAdminTask eprintTask, CookieCollection loginCookie)
        {
            // get the list of printers and extract the unique id to get the unique internal id of the required printer
            Uri getPrinterList = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/printers/List.aspx");
            HttpWebRequest getPrintersReq = (HttpWebRequest)WebRequest.Create(getPrinterList);
            getPrintersReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            getPrintersReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            getPrintersReq.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            getPrintersReq.CookieContainer = new CookieContainer();
            getPrintersReq.CookieContainer.Add(loginCookie);
            getPrintersReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            HttpWebResult getPrintersRes = HttpWebEngine.Get(getPrintersReq);
            string printersListResponse = getPrintersRes.Response;
            // TraceFactory.Logger.Info("Load the printer list");
            ExecutionServices.SystemTrace.LogInfo("Load the printer list");

            //get the request id for the printer from the response
            string substringFromPrinterIp = printersListResponse.Substring(printersListResponse.IndexOf(_device.Address, StringComparison.Ordinal));
            string printerCode = substringFromPrinterIp.Substring(substringFromPrinterIp.IndexOf("PrinterListGrid", StringComparison.OrdinalIgnoreCase) + "PrinterListGrid".Length, 7);

            ExecutionServices.SystemTrace.LogInfo("Retrieving the printer list");
            // POST the unique id to get the redirect url of the selected printer
            Uri postPrinterDetails = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/printers/List.aspx");
            HttpWebRequest postPrinterDetReq = (HttpWebRequest)WebRequest.Create(postPrinterDetails);
            postPrinterDetReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            postPrinterDetReq.ContentType = "application/x-www-form-urlencoded";
            postPrinterDetReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            postPrinterDetReq.Headers.Add("Accept-Encoding", "gzip, deflate");
            postPrinterDetReq.CookieContainer = new CookieContainer();
            postPrinterDetReq.CookieContainer.Add(loginCookie);
            //postdata
            string postData = string.Format(Properties.Resources.DeletePrinterRedirectURL, HttpUtility.UrlEncode(printerCode), HttpUtility.UrlEncode(ExtractViewState(printersListResponse)));

            postPrinterDetReq.AllowAutoRedirect = false;
            HttpWebResult postPrinterDetResponse = HttpWebEngine.Post(postPrinterDetReq, postData);
            if (postPrinterDetResponse.StatusCode != HttpStatusCode.Redirect)
            {
                ExecutionServices.SystemTrace.LogDebug($"Unable to find printer {_device.Address} ERROR: Status code {postPrinterDetResponse.StatusCode}");
                eprintTask.Status = "Failed";
            }

            //GET details of printer by sending the unique url with printers ID to get viewstate
            Uri getPrinterDetails = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}{postPrinterDetResponse.Headers.Get("Location")}");
            HttpWebRequest getPrinterDetReq = (HttpWebRequest)WebRequest.Create(getPrinterDetails);
            getPrinterDetReq.Headers.Add(HttpRequestHeader.CacheControl, " max-age=0");
            getPrinterDetReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            getPrinterDetReq.ContentType = "application/x-www-form-urlencoded";
            getPrinterDetReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            getPrinterDetReq.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            getPrinterDetReq.CookieContainer = new CookieContainer();
            getPrinterDetReq.CookieContainer.Add(loginCookie);
            getPrinterDetReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            HttpWebResult getPrinterDetResponse = HttpWebEngine.Get(getPrinterDetReq);

            ExecutionServices.SystemTrace.LogInfo($"Redirecting to URL:{postPrinterDetResponse.Headers.Get("Location")}");
            //delete selected printer
            Uri deletePrinter = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}{postPrinterDetResponse.Headers.Get("Location")}");
            HttpWebRequest delPrinterReq = (HttpWebRequest)WebRequest.Create(deletePrinter);
            delPrinterReq.Headers.Add(HttpRequestHeader.CacheControl, " max-age=0");
            delPrinterReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            delPrinterReq.ContentType = "application/x-www-form-urlencoded";
            delPrinterReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            delPrinterReq.Headers.Add("Accept-Encoding", "gzip, deflate");
            delPrinterReq.CookieContainer = new CookieContainer();
            delPrinterReq.CookieContainer.Add(loginCookie);
            //postdata
            string delpostData = string.Format(Properties.Resources.DeletePrinterPostData, HttpUtility.UrlEncode(ExtractViewState(getPrinterDetResponse.Response)));
            HttpWebResult delPrinterResponse = HttpWebEngine.Post(delPrinterReq, delpostData);

            if (delPrinterResponse.StatusCode != HttpStatusCode.OK)
            {
                ExecutionServices.SystemTrace.LogInfo($"Failed to Delete the printer ERROR:{delPrinterResponse.StatusCode}");

                eprintTask.Status = "Failed";
            }
            else
            {
                ExecutionServices.SystemTrace.LogInfo($"Successfully deleted the printer {_device.Address}");

                eprintTask.Status = "Passed";
            }
        }

        private void ePrintSendPrintJob(EprintAdminTask eprintTask, CookieCollection loginCookie)
        {
            //Get Request
            Uri getsendPrintUrl = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}//cloudprintadmin/services/SubmitTest.aspx");
            HttpWebRequest getsendPrint = (HttpWebRequest)WebRequest.Create(getsendPrintUrl);
            getsendPrint.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            getsendPrint.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            getsendPrint.CookieContainer = new CookieContainer();
            getsendPrint.CookieContainer.Add(loginCookie);
            getsendPrint.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            getsendPrint.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            HttpWebResult getsendPrintResponse = HttpWebEngine.Get(getsendPrint);
            ExecutionServices.SystemTrace.LogInfo($"Requesting to Print {Environment.UserName} as a {eprintTask.Operation} ");
            //get admin cookie
            CookieCollection adminCookie = new CookieCollection();
            string setCookie = getsendPrintResponse.Headers.Get("Set-Cookie");
            string trimmedcookie = setCookie.Substring(12, setCookie.IndexOf(";", StringComparison.Ordinal) - 12);
            adminCookie.Add(new Cookie("AdminCookie", trimmedcookie) { Domain = _ePrintServerIp });
            //Post request
            Uri sendPrintUrl = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/services/SubmitTest.aspx");
            HttpWebRequest sendPrintReq = (HttpWebRequest)WebRequest.Create(sendPrintUrl);
            sendPrintReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            sendPrintReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            sendPrintReq.ContentType = "application/x-www-form-urlencoded";
            sendPrintReq.CookieContainer = new CookieContainer();
            sendPrintReq.CookieContainer.Add(loginCookie);
            sendPrintReq.CookieContainer.Add(adminCookie);
            sendPrintReq.Headers.Add("Accept-Encoding", "gzip, deflate");
            string postData = string.Format(Properties.Resources.SendPrintJobPostData, (HttpUtility.UrlEncode(ExtractViewState(getsendPrintResponse.Response))), _device.Address);
            getsendPrintResponse = HttpWebEngine.Post(sendPrintReq, postData);
            if (getsendPrintResponse.StatusCode.Equals(HttpStatusCode.OK))
            {
                ExecutionServices.SystemTrace.LogInfo($"Successfully sent print job to { (object)_device.Address} ");
                eprintTask.Status = "Passed";
            }
            else
            {
                ExecutionServices.SystemTrace.LogError($"Could not send print job to IP {_device.Address} ERROR:Status Code:{getsendPrintResponse.StatusCode} ");

                eprintTask.Status = "Failed";
            }
        }

        private void ePrintAddPrinter(EprintAdminTask eprintTask, CookieCollection loginCookie)
        {
            string assetId = eprintTask.TargetObject;
            //GET previous response to extract viewstate
            Uri getaddPrinterUrl = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/printers/Insert.aspx");
            HttpWebRequest getaddPrinter = (HttpWebRequest)WebRequest.Create(getaddPrinterUrl);
            getaddPrinter.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            getaddPrinter.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            getaddPrinter.CookieContainer = new CookieContainer();
            getaddPrinter.CookieContainer.Add(loginCookie);
            getaddPrinter.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            getaddPrinter.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            HttpWebResult getaddPrinterResponse = HttpWebEngine.Get(getaddPrinter);

            if (getaddPrinterResponse.StatusCode != HttpStatusCode.OK)
            {
                ExecutionServices.SystemTrace.LogError("Unable to Login to ePrint . Please check Credentials");
                //throw new PluginExecutionResult(PluginResult.Failed, "Unable to Login to ePrint . Please check Credentials");
            }
            CookieCollection adminCookie = new CookieCollection();
            string setCookie = getaddPrinterResponse.Headers.Get("Set-Cookie");
            string trimmedcookie = setCookie.Substring(12, setCookie.IndexOf(";", StringComparison.Ordinal) - 12);
            adminCookie.Add(new Cookie("AdminCookie", trimmedcookie) { Domain = _ePrintServerIp });
            //Add the Printer: POST
            Uri addPrinterUrl = new Uri($@"{_ePrintServerType}://{_ePrintServerIp}/cloudprintadmin/printers/Insert.aspx");
            HttpWebRequest addPrinterReq = (HttpWebRequest)WebRequest.Create(addPrinterUrl);
            HttpWebResult addPrinterResponse;
            //ipv4 printer or HPAC printer
            if (eprintTask.Operation.Equals(EprintAdminToolOperation.AddPrinteripv4))
            {
                addPrinterReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
                addPrinterReq.Accept = "text/html, application/xhtml+xml, */*";
                addPrinterReq.ContentType = "application/x-www-form-urlencoded";
                addPrinterReq.CookieContainer = new CookieContainer();
                addPrinterReq.CookieContainer.Add(adminCookie);
                addPrinterReq.CookieContainer.Add(loginCookie);
                addPrinterReq.Headers.Add("Accept-Encoding", "gzip, deflate");
                addPrinterReq.Headers.Add(HttpRequestHeader.Pragma, "no-cache");

                string postData = string.Format(Properties.Resources.AddPrinterIPV4PostData, (HttpUtility.UrlEncode(ExtractViewState(getaddPrinterResponse.Response))), assetId, _device.GetDeviceInfo().ModelName, _device.Address);

                addPrinterResponse = HttpWebEngine.Post(addPrinterReq, postData);
                if (addPrinterResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    ExecutionServices.SystemTrace.LogInfo($"Successfully added printer { (object)assetId} IP { (object)_device.Address} ");
                    eprintTask.Status = "Passed";
                }
                else
                {
                    ExecutionServices.SystemTrace.LogError($"Could not add printer { (object)assetId} IP { (object)_device.Address} ERROR:Status Code:{ (object)addPrinterResponse.StatusCode} ");

                    eprintTask.Status = "Failed";
                }
            }
            else if (eprintTask.Operation.Equals(EprintAdminToolOperation.AddPrinterHpac))
            {
                addPrinterReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
                addPrinterReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                addPrinterReq.ContentType = "application/x-www-form-urlencoded";
                addPrinterReq.CookieContainer = new CookieContainer();
                addPrinterReq.CookieContainer.Add(loginCookie);
                addPrinterReq.Headers.Add("Accept-Encoding", "gzip, deflate");
                addPrinterReq.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");

                string postData = string.Format(Properties.Resources.AddPrinterHPACPostdata, (HttpUtility.UrlEncode(ExtractViewState(getaddPrinterResponse.Response))), eprintTask.HpacInputValue.PrinterName, eprintTask.HpacInputValue.NetworkAddress, eprintTask.HpacInputValue.QueueName, eprintTask.HpacInputValue.DomainUser, eprintTask.HpacInputValue.DomainPassword);
                addPrinterResponse = HttpWebEngine.Post(addPrinterReq, postData);
                if (addPrinterResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    ExecutionServices.SystemTrace.LogInfo($"Successfully added printer using HPAC { (object)eprintTask.HpacInputValue.NetworkAddress} and Queue { (object)eprintTask.HpacInputValue.QueueName} ");
                    eprintTask.Status = "Passed";
                }
                else
                {
                    ExecutionServices.SystemTrace.LogInfo($"Could not add printer { (object)eprintTask.HpacInputValue.PrinterName} ERROR:Status Code:{ (object)addPrinterResponse.StatusCode} ");
                    eprintTask.Status = "Failed";
                }
            }
            else
            {
                addPrinterReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
                addPrinterReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                addPrinterReq.ContentType = "application/x-www-form-urlencoded";
                addPrinterReq.CookieContainer = new CookieContainer();
                addPrinterReq.CookieContainer.Add(adminCookie);
                addPrinterReq.CookieContainer.Add(loginCookie);
                addPrinterReq.Headers.Add("Accept-Encoding", "gzip, deflate");
                addPrinterReq.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");

                string postData = string.Format(Properties.Resources.AddPrinterSAFECOMPostdata, (HttpUtility.UrlEncode(ExtractViewState(getaddPrinterResponse.Response))), eprintTask.HpacInputValue.PrinterName, eprintTask.HpacInputValue.NetworkAddress, eprintTask.HpacInputValue.QueueName, eprintTask.HpacInputValue.DomainUser, eprintTask.HpacInputValue.DomainPassword);
                addPrinterResponse = HttpWebEngine.Post(addPrinterReq, postData);
                if (addPrinterResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    ExecutionServices.SystemTrace.LogInfo($"Successfully added printer using HPAC { (object)eprintTask.HpacInputValue.NetworkAddress} and Queue { (object)eprintTask.HpacInputValue.QueueName} ");
                    eprintTask.Status = "Passed";
                }
                else
                {
                    ExecutionServices.SystemTrace.LogInfo($"Could not add printer {eprintTask.HpacInputValue.PrinterName} ERROR:Status Code:{addPrinterResponse.StatusCode} ");
                    eprintTask.Status = "Failed";
                }
            }
        }
    }
}