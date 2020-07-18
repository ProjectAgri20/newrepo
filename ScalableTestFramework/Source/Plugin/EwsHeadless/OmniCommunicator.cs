using HP.DeviceAutomation;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Communicator for Omni devices
    /// </summary>
    internal class OmniCommunicator : EwsCommunicatorBase
    {
        protected string CsrfToken;
        private const string OmniDeviceSignInUrl = "https://{0}/hp/device/SignIn/Index";
        private const string OmniDeviceHpacSolutionUrl = "https://{0}/hp/device/SolutionInstaller";

        private const string AcceptString =
            "text/html, application/xhtml+xml, image/jxr, image/gif, image/jpeg, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, application/msword, application/vnd.ms-powerpoint, application/vnd.ms-excel, */*";
        public OmniCommunicator(IDevice device, string password)
            : base(device, password, "Omni")
        {
        }

        protected override string GetSessionId(PayloadDefinition definition)
        {
            try
            {
                //[Kelly]:the signin Uri might need to be modified to have the port 4242 appended if you are targetting the simulator
                var signInUri = new Uri(string.Format(CultureInfo.CurrentCulture, OmniDeviceSignInUrl, Device.Address));
                string sessionId = string.Empty;
                CsrfToken = GetCsrfToken(signInUri.ToString(), ref sessionId);
                return sessionId;
            }
            catch (WebException)
            {
                return string.Empty;
            }
        }

        protected override string SignIn(string sessionId)
        {
            string postData;
            ServicePointManager.Expect100Continue = false;
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
                    //The below IF condition is included to use the CSRF token using the actual URL of the request and not the landing response URI. For unknown reasons the CSRF from "/hp/device/index" does not work for Webservice and Networking
                    //The implementation of CSRF is in the inherited class , a cleanup of this code may need to be looked upon 
                    if (alternateCSRFUrl.Equals(string.Empty))
                        CsrfToken = GetCsrfToken(signinResponse.ResponseUri.AbsoluteUri, ref sessionId);
                    else
                        CsrfToken = GetCsrfToken(alternateCSRFUrl, ref sessionId);
                }

                return sessionId;
            }
            catch (WebException)
            {
                return string.Empty;
            }
        }

        protected string GetCsrfToken(string urlAddress, ref string sessionId)
        {
            string csrfToken = string.Empty;
          

            HttpClientHandler handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };
            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("Accept", AcceptString);
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");

                if (!string.IsNullOrEmpty(sessionId))
                {
                    client.DefaultRequestHeaders.Add("Cookie", $"sessionId={sessionId}");
                    // signinCsrfRequest.Headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");
                }

                var csrfResponse = client.GetAsync(urlAddress);
                if (csrfResponse.Result.IsSuccessStatusCode)
                {
                    if (string.IsNullOrEmpty(sessionId))
                    {
                        sessionId =
                            csrfResponse.Result.Headers.GetValues("Set-Cookie")?
                                .FirstOrDefault()?.Split(';')
                                .FirstOrDefault();
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
                        if (endIndex == -1)
                        {
                            //While using the alternate URL for webservice and networking , the CSRF is contained inside an <input> without a trailing '/' which results in a exception . This piece of code gets the correct CSRF for it .
                            endIndex = responseBodyString.IndexOf("\">", StringComparison.OrdinalIgnoreCase);
                        }                     
                        csrfToken = responseBodyString.Substring("name=\"CSRFToken\" value=\"".Length,
                            endIndex - "name=\"CSRFToken\" value=\"".Length);
                    }
                }
            }

         
            return csrfToken;
        }

        protected override HttpWebRequest BuildRequest(Uri url, HttpVerb method, PayloadDefinition payloadDefinition, string sessionId, string payload = null, string hideId = null)
        {
           

            HttpMessenger messenger = new HttpMessenger()
            {
                ContentType = "application/x-www-form-urlencoded",
                Accept = "text/html, application/xhtml+xml, image/jxr, image/gif, image/jpeg, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, application/msword, application/vnd.ms-powerpoint, application/vnd.ms-excel, */*",
                AllowAutoRedirect = true,
                // Encoding = Encoding.GetEncoding("iso-8859-1"),
                Expect100Continue = false,
            };
            
            WebHeaderCollection headers = new WebHeaderCollection();
            foreach (var header in payloadDefinition.Headers.Keys)
            {
                headers.Add(header, payloadDefinition.Headers[header]);
            }

            if (!string.IsNullOrEmpty(Password) && method != HttpVerb.Get )
            {
                var plainTextBytes = Encoding.UTF8.GetBytes("admin:" + Password);
                string authorization = Convert.ToBase64String(plainTextBytes);
                headers.Add(HttpRequestHeader.Authorization, "Basic " + authorization + "");
            }

            var cookieContainer = new CookieContainer();
            if (!string.IsNullOrEmpty(sessionId))
            {
                headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");
                cookieContainer.Add(new Cookie("sessionId", sessionId) { Domain = url.Host });
            }
            if (!string.IsNullOrEmpty(hideId))
            {
                headers.Add(HttpRequestHeader.Cookie, $"ipsecwizardid={hideId}");
                cookieContainer.Add(new Cookie("ipsecwizardid", hideId) { Domain = url.Host });
            }
            HttpWebRequest request;
            if (string.IsNullOrEmpty(payload))
            {
                request = messenger.BuildRequest(url, method, headers);
            }
            else
            {
                if (string.IsNullOrEmpty(CsrfToken))
                {
                    request = messenger.BuildRequest(url, method, headers, payload);
                }
                else
                {
                    request = messenger.BuildRequest(url, method, headers, $"CSRFToken={HttpUtility.UrlEncode(CsrfToken)}&{payload}");
                }
            }
            request.CookieContainer = cookieContainer;
            return request;

            
        }
      
            
        protected string GetSolutionId(string sessionId, string solutionName)
        {
            try
            {
                PayloadDefinition payload = new PayloadDefinition { Headers = new Dictionary<string, string>() };
                //Getting the session ID by making request to /hp/device/SolutionInstaller
                HttpWebRequest httpRequest = BuildRequest(new Uri(string.Format(CultureInfo.CurrentCulture, OmniDeviceHpacSolutionUrl, Device.Address)), HttpVerb.Get, payload, sessionId);
                var sessionResponse = HttpMessenger.ExecuteRequest(httpRequest);
                //Getting the session ID from the response recieved

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(sessionResponse.Body);
                CsrfToken = doc.GetElementbyId("CSRFToken").Attributes["value"].Value;

                //var nodes = doc.g
                HtmlNode table = doc.DocumentNode.SelectSingleNode("//table[@id='SolutionsTable']");
                if (table == null)
                {
                    throw new DeviceInvalidOperationException("Solution not installed");
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

        protected override void FillRemoveSolutionData(HttpWebRequest webRequest, string solutionName, string sessionId)
        {
            var solutionId = GetSolutionId(sessionId, solutionName);

            webRequest.ServicePoint.Expect100Continue = false;
            string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x8", CultureInfo.CurrentCulture);
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            using (Stream requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                string headerTemplate = $"Content-Disposition: form-data; name=\"CSRFToken\"\r\n\r\n{CsrfToken}";
                byte[] headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                headerTemplate = "Content-Disposition: form-data; name=\"bundleFile\"; filename=\"\"\r\n";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);
                headerTemplate = "Content-Type: application/octet-stream\r\n\r\n";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                headerTemplate = $"Content-Disposition: form-data; name=\"Solutions\"\r\n\r\n{solutionId}";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                headerTemplate = "Content-Disposition: form-data; name=\"RemoveSolutionButton\"\r\n\r\nRemove...";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                headerTemplate = "Content-Disposition: form-data; name=\"StepBackAnchor\"\r\n\r\nSolutionListViewSectionId";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                headerTemplate = "Content-Disposition: form-data; name=\"jsAnchor\"\r\n\r\nSolutionListViewSectionId";
                headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

                byte[] footerBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                requestStream.Write(footerBytes, 0, footerBytes.Length);
                webRequest.UserAgent = "IE";
            }
        }
    }
}