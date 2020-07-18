using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using HP.DeviceAutomation;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Communicator for Phoenix devices
    /// </summary>
    internal class PhoenixCommunicator : EwsCommunicatorBase
    {
        const string PhoenixDeviceSignInUrl = "https://{0}/";
        public PhoenixCommunicator(IDevice device, string password)
            : base(device, password, "Phoenix")
        {
        }


        protected override string GetSessionId(PayloadDefinition definition)
        {
            try
            {

                //Getting the session ID by making request to PhoenixDeviceSignInURL
                var signInUri = new Uri(string.Format(CultureInfo.CurrentCulture, PhoenixDeviceSignInUrl, Device.Address));
                HttpWebRequest httpRequest = BuildRequest(signInUri, HttpVerb.Get, definition, null);
                var sessionResponse = HttpMessenger.ExecuteRequest(httpRequest);

                //Getting the session ID from the response recieved
                string sessionId = string.Empty;
                if (sessionResponse.Headers.GetValues("Set-Cookie") != null)
                {
                    sessionId = (sessionResponse.Headers.GetValues("Set-Cookie")?.FirstOrDefault()?.Split(';').FirstOrDefault());
                }
                return sessionId;
            }
            catch (WebException)
            {
                return string.Empty;
            }
        }

        protected override HttpWebRequest BuildRequest(Uri url, HttpVerb method, PayloadDefinition payloadDefinition, string sessionId, string payload = null, string hideId = null)
        {
            HttpMessenger messenger = new HttpMessenger
            {
                ContentType = "application/x-www-form-urlencoded",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                AllowAutoRedirect = false,
                Expect100Continue = false
            };

            WebHeaderCollection headers = new WebHeaderCollection();
            foreach (var header in payloadDefinition.Headers.Keys)
            {
                headers.Add(header, payloadDefinition.Headers[header]);
            }

            if (!string.IsNullOrEmpty(Password))
            {
                var plainTextBytes = Encoding.UTF8.GetBytes("admin:" + Password);
                string authorization = Convert.ToBase64String(plainTextBytes);
                headers.Add(HttpRequestHeader.Authorization, "Basic " + authorization + "");
            }

            CookieContainer cookieContainer = null;
            if (!string.IsNullOrEmpty(sessionId))
            {
                cookieContainer = new CookieContainer();
                cookieContainer.Add(new Cookie("sessionId", sessionId) { Domain = url.Host });
            }

            HttpWebRequest request = messenger.BuildRequest(url, method, headers, payload);
            request.CookieContainer = cookieContainer;
            return request;
        }


        protected override void FillRemoveSolutionData(HttpWebRequest webRequest, string solutionName, string sessionId)
        {
            throw new NotImplementedException();
        }
    }
}
