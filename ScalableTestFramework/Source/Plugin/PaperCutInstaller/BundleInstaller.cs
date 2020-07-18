using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework.Plugin;
using HtmlAgilityPack;

namespace HP.ScalableTest.Plugin.PaperCutInstaller
{
    public class BundleInstaller
    {
        protected string CsrfToken;
        private const string OmniDeviceSignInUrl = "https://{0}/hp/device/SignIn/Index";
        private const string OmniDeviceSolutionUrl = "https://{0}/hp/device/SolutionInstaller";
        private const string OmniDeviceSolutionSaveUrl = "https://{0}/hp/device/SolutionInstaller/Save?jsAnchor=SolutionInstallViewSectionId";

        private const string AcceptString = "text/html, application/xhtml+xml, image/jxr, image/gif, image/jpeg, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, application/msword, application/vnd.ms-powerpoint, application/vnd.ms-excel, */*";
        protected readonly JediOmniDevice Device;
        protected HttpClientHandler Handler;

        public BundleInstaller(JediOmniDevice device)
        {
            Device = device;
            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.Expect100Continue = false;
        }

        public BundleInstaller()
        {
            Handler = new HttpClientHandler() { AllowAutoRedirect = true, UseCookies = false };
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.Expect100Continue = false;
        }

        public string SignIn(string sessionId)
        {
            CsrfToken = GetCsrfToken(string.Format(CultureInfo.CurrentCulture, OmniDeviceSignInUrl, Device.Address), ref sessionId);

            var cookieContainer = new CookieContainer();
            HttpWebRequest signinRequest = (HttpWebRequest)WebRequest.Create(string.Format(CultureInfo.CurrentCulture, OmniDeviceSignInUrl, Device.Address));
            cookieContainer.Add(new Cookie("sessionId", sessionId) { Domain = signinRequest.RequestUri.Host });
            signinRequest.CookieContainer = cookieContainer;
            signinRequest.Method = "POST";
            signinRequest.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            signinRequest.ContentType = "application/x-www-form-urlencoded";
            signinRequest.Headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");
            signinRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            signinRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");

            string postData;
            if (string.IsNullOrEmpty(CsrfToken))
            {
                postData = $"agentIdSelect=hp_EmbeddedPin_v1&DomainDropDown=&UserNameTextBox=&PinDropDown=AdminItem&PasswordTextBox={Device.AdminPassword}&signInOk=Sign+In";
            }
            else
            {
                postData = $"agentIdSelect=hp_EmbeddedPin_v1&CSRFToken={CsrfToken}&DomainDropDown=&UserNameTextBox=&PinDropDown=AdminItem&PasswordTextBox={Device.AdminPassword}&signInOk=Sign+In";
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
                    sessionId = signinRequest.Headers.GetValues("Cookie")?.FirstOrDefault()?.Split(';').FirstOrDefault();
                    sessionId = sessionId?.Replace("sessionId=", string.Empty);
                    CsrfToken = GetCsrfToken(signinResponse.ResponseUri.AbsoluteUri, ref sessionId);
                }

                return sessionId;
            }
            catch (WebException)
            {
                return string.Empty;
            }
        }

        private string GetCsrfToken(string urlAddress, ref string sessionId)
        {
            string csrfToken = string.Empty;

            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };
            using (HttpClient client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.Add("Accept", AcceptString);
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");

                if (!string.IsNullOrEmpty(sessionId))
                {
                    client.DefaultRequestHeaders.Add("Cookie", $"sessionId={sessionId}");
                }

                var csrfResponse = client.GetAsync(urlAddress);
                if (csrfResponse.Result.IsSuccessStatusCode)
                {
                    if (string.IsNullOrEmpty(sessionId))
                    {
                        sessionId = csrfResponse.Result.Headers.GetValues("Set-Cookie")?.FirstOrDefault()?.Split(';').FirstOrDefault();
                        sessionId = sessionId?.Replace("sessionId=", string.Empty);
                    }

                    var responseBodyTask = csrfResponse.Result.Content.ReadAsStringAsync();
                    var responseBodyString = responseBodyTask.Result;
                    int startIndex = responseBodyString.IndexOf("name=\"CSRFToken\" value=\"",
                        StringComparison.OrdinalIgnoreCase);
                    if (startIndex != -1)
                    {
                        responseBodyString = responseBodyString.Substring(startIndex);
                        int endIndex = responseBodyString.IndexOf("\" />", StringComparison.OrdinalIgnoreCase);
                        csrfToken = responseBodyString.Substring("name=\"CSRFToken\" value=\"".Length,
                            endIndex - "name=\"CSRFToken\" value=\"".Length);
                    }
                }
            }

            return csrfToken;
        }

        public PluginExecutionResult RemoveSolution(string sessionId, string solutionName)
        {
            var solutionId = GetSolutionId(sessionId, solutionName);
            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };
            using (var client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Cookie", $"sessionId={sessionId}");
                client.Timeout = TimeSpan.FromMinutes(10);

                using (var multipartFormData = new MultipartFormDataContent())
                {
                    multipartFormData.Add(new StringContent(CsrfToken), "\"CSRFToken\"");
                    multipartFormData.Add(new StringContent(solutionId), "\"Solutions\"");

                    var fileContent = new StreamContent(Stream.Null);
                    fileContent.Headers.Add("Content-Disposition", "form-data; name=\"bundleFile\"; filename=\"\"");
                    multipartFormData.Add(fileContent, "bundleFile", "\"\"");

                    multipartFormData.Add(new StringContent("Remove..."), "\"RemoveSolutionButton\"");
                    multipartFormData.Add(new StringContent("SolutionInstallViewSectionId"), "\"StepBackAnchor\"");
                    multipartFormData.Add(new StringContent("SolutionInstallViewSectionId"), "\"jsAnchor\"");

                    var message = client.PostAsync(string.Format(CultureInfo.CurrentCulture, OmniDeviceSolutionSaveUrl, Device.Address),
                        multipartFormData);

                    if (message.Result.IsSuccessStatusCode)
                    {
                        //confirm the message prompt
                        var formData = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("CSRFToken", CsrfToken),
                            new KeyValuePair<string, string>("OperationIdentifier", "BundleList.Remove.DialogId"),
                            new KeyValuePair<string, string>("DialogButtonYes", "Remove")
                        });

                        var removeResponse = client.PostAsync(string.Format(CultureInfo.CurrentCulture, OmniDeviceSolutionSaveUrl, Device.Address), formData);

                        if (removeResponse.Result.IsSuccessStatusCode)
                        {
                            var removeResponseBodyString = removeResponse.Result.Content.ReadAsStringAsync();
                            HtmlDocument hDoc = new HtmlDocument();
                            hDoc.LoadHtml(removeResponseBodyString.Result);

                            var summaryNode = hDoc.DocumentNode.SelectSingleNode("//div[@id='Summary']");
                            if (summaryNode.InnerText.Contains("restarting"))
                            {
                                return new PluginExecutionResult(PluginResult.Passed);
                            }
                        }
                        return new PluginExecutionResult(PluginResult.Failed, removeResponse.Exception?.InnerException);
                    }

                    return new PluginExecutionResult(PluginResult.Failed, message.Exception);
                }
            }
        }

        protected string GetSolutionId(string sessionId, string solutionName)
        {
            try
            {
                //Getting the session ID by making request to /hp/device/SolutionInstaller
                HttpWebRequest httpRequest = WebRequest.CreateHttp(new Uri(string.Format(CultureInfo.CurrentCulture, OmniDeviceSolutionUrl, Device.Address)));
                httpRequest.Method = "GET";
                WebHeaderCollection headers = new WebHeaderCollection();
                if (!string.IsNullOrEmpty(Device.AdminPassword))
                {
                    var plainTextBytes = Encoding.UTF8.GetBytes("admin:" + Device.AdminPassword);
                    string authorization = Convert.ToBase64String(plainTextBytes);
                    headers.Add(HttpRequestHeader.Authorization, "Basic " + authorization + "");
                }

                if (!string.IsNullOrEmpty(sessionId))
                {
                    headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");
                }
                httpRequest.Headers = headers;
                var sessionResponse = HttpMessenger.ExecuteRequest(httpRequest);

                //Getting the session ID from the response received
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(sessionResponse.Body);

                HtmlNode table = doc.DocumentNode.SelectSingleNode("//table[@id='SolutionsTable']");
                if (table == null)
                {
                    throw new WebException("Solution not installed");
                }
                foreach (var cell in table.SelectNodes(".//tr/td")) // **notice the .**
                {
                    if (cell.InnerText.StartsWith(solutionName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (cell.Id.EndsWith("_Name", StringComparison.OrdinalIgnoreCase))
                        {
                            return cell.Id.Substring(0, cell.Id.Length - 5);
                        }
                    }
                }
                return string.Empty;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new DeviceCommunicationException("Exception occurred while fetching the Solution Id ", ex.InnerException);
            }
        }

        public PluginExecutionResult InstallSolution(string sessionId, string bundleFileName)
        {
            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };
            using (var client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Cookie", $"sessionId={sessionId}");
                client.Timeout = TimeSpan.FromMinutes(10);

                //find out if the device supports bundle installer
                var solutionInstaller = client.GetAsync(string.Format(CultureInfo.CurrentCulture, OmniDeviceSolutionUrl, Device.Address));
                if (!solutionInstaller.Result.IsSuccessStatusCode)
                    return new PluginExecutionResult(PluginResult.Failed, "Solution Installer not available");


                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(CsrfToken), "\"CSRFToken\"");

                    var fileContent = new StreamContent(File.OpenRead(bundleFileName));
                    fileContent.Headers.Add("Content-Type", "application/octet-stream");
                    fileContent.Headers.Add("Content-Disposition",
                        "form-data; name=\"bundleFile\"; filename=\"" + Path.GetFileName(bundleFileName) + "\"");

                    formData.Add(fileContent, "bundleFile", Path.GetFileName(bundleFileName));

                    formData.Add(new StringContent("Install"), "\"InstallButton\"");
                    formData.Add(new StringContent("SolutionInstallViewSectionId"), "\"StepBackAnchor\"");
                    formData.Add(new StringContent("SolutionInstallViewSectionId"), "\"jsAnchor\"");

                    var message = client.PostAsync(string.Format(CultureInfo.CurrentCulture, OmniDeviceSolutionSaveUrl, Device.Address), formData);

                    if (message.Result.IsSuccessStatusCode)
                    {
                        var bodyString = message.Result.Content.ReadAsStringAsync();
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(bodyString.Result);
                        var summaryNode = doc.DocumentNode.SelectSingleNode("//div[@id='Summary']");
                        if (summaryNode.HasAttributes)
                        {
                            if (summaryNode.Attributes["class"].Value == "message message-warning" && !summaryNode.InnerText.Contains("installed"))
                            {
                                return new PluginExecutionResult(PluginResult.Failed, $"Solution not installed: {summaryNode.InnerText}");
                            }
                        }
                        HtmlNode table = doc.DocumentNode.SelectSingleNode("//table[@id='SolutionsTable']");
                        if (table == null)
                        {
                            return new PluginExecutionResult(PluginResult.Failed, $"Solution not installed: {summaryNode.InnerText}");
                        }
                        return new PluginExecutionResult(PluginResult.Passed, summaryNode.InnerText);
                    }

                    return new PluginExecutionResult(PluginResult.Failed, message.Exception?.InnerException);
                }
            }
        }
    }

    public class SafeComBundleInstaller : BundleInstaller
    {
        private const string OmniSafeComConfigUrl = "https://{0}/hp/device/dk.safecom.hp.web.ScConfigPage/Index";
        private const string OmniSafeComRegisterUrl = "https://{0}/hp/device/dk.safecom.hp.web.ScRegisterPage/Index";

        public SafeComBundleInstaller(JediOmniDevice device) : base(device)
        {
        }

        public PluginExecutionResult ConfigureSafeCom(string sessionId, NameValueCollection formDataCollection)
        {
            var solutionId = GetSolutionId(sessionId, "SafeCom");
            if (string.IsNullOrEmpty(solutionId))
            {
                throw new WebException("SafeCom not Installed");
            }

            formDataCollection.Add("CSRFToken", CsrfToken);
            Handler = new HttpClientHandler {AllowAutoRedirect = true, UseCookies = false};

            using (var client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Cookie", $"sessionId={sessionId}");
                client.Timeout = TimeSpan.FromMinutes(10);
                using (var multipartFormData = new MultipartFormDataContent())
                {
                    foreach (var formDataKey in formDataCollection.AllKeys)
                    {
                        multipartFormData.Add(new StringContent(formDataCollection[formDataKey]), $"\"{formDataKey}\"");
                    }

                    var configureResponse = client.PostAsync(string.Format(OmniSafeComConfigUrl, Device.Address),
                        multipartFormData);

                    if (configureResponse.Result.IsSuccessStatusCode)
                    {
                        return new PluginExecutionResult(PluginResult.Passed, "Safecom configured");
                    }

                    return new PluginExecutionResult(PluginResult.Failed, configureResponse.Exception);
                }
            }
        }

        public PluginExecutionResult RegisterDevice(string sessionId, NameValueCollection formDataCollection)
        {
            formDataCollection.Add("CSRFToken", CsrfToken);
            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };

            using (var client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Cookie", $"sessionId={sessionId}");
                client.Timeout = TimeSpan.FromMinutes(10);
                using (var multipartFormData = new MultipartFormDataContent())
                {
                    foreach (var formDataKey in formDataCollection.AllKeys)
                    {
                        multipartFormData.Add(new StringContent(formDataCollection[formDataKey]), $"\"{formDataKey}\"");
                    }

                    var configureResponse = client.PostAsync(string.Format(OmniSafeComRegisterUrl, Device.Address),
                        multipartFormData);

                    if (configureResponse.Result.IsSuccessStatusCode)
                    {
                        var configureResponseBody = configureResponse.Result.Content.ReadAsStringAsync();
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(configureResponseBody.Result);

                        HtmlNode table = doc.DocumentNode.SelectSingleNode("//div[@id='LogSectionId']/div[2]/table/tr[5]/td[1]/h1");
                        if (table != null && table.Attributes["style"].Value.Contains("red") &&
                            !string.IsNullOrEmpty(table.InnerText))
                        {
                            return new PluginExecutionResult(PluginResult.Failed,
                                $"Device: {Device.GetDeviceInfo().ModelName} - {Device.Address} did not register, {table.InnerText}");
                        }
                        return new PluginExecutionResult(PluginResult.Passed, "Safecom Registred");
                    }

                    return new PluginExecutionResult(PluginResult.Failed, configureResponse.Exception);
                }
            }
        }
    }
}