using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using HP.DeviceAutomation;


namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Communicator for Jedi devices
    /// </summary>
    internal class JediCommunicator : EwsCommunicatorBase
    {
        public JediCommunicator(IDevice device, string password)
            : base(device, password, "Jedi")
        {
        }
        const string JediDeviceSignInUrl = "https://{0}/hp/device/SignIn/Index";

        protected override string GetSessionId(PayloadDefinition definition)
        {
            try
            {
                var signInUri = new Uri(string.Format(CultureInfo.CurrentCulture, JediDeviceSignInUrl, Device.Address));
                //Getting the session ID by making request to /hp/device/DeviceStatus/Index
                HttpWebRequest httpRequest = BuildRequest(signInUri, HttpVerb.Get, definition, null);
                var sessionResponse = HttpMessenger.ExecuteRequest(httpRequest);


                //Getting the session ID from the response recieved
                string sessionId = sessionResponse.Headers.GetValues("Set-Cookie")?.FirstOrDefault()?.Split(';').FirstOrDefault();
                sessionId = sessionId?.Replace("sessionId=", string.Empty);
                return sessionId;
            }
            catch (WebException)
            {
                return string.Empty;
            }
        }

        protected override string SignIn(string sessionId)
        {
            PayloadDefinition definition = EwsPayloadFactory.GetPayLoad(Device.GetDeviceInfo().FirmwareRevision, "Jedi", "SignIn", string.Empty, 1);

            EwsRequest signInRequest = new EwsRequest("SignIn", "SignIn", null);
            signInRequest.Add("Password", Password.Replace("admin:", string.Empty));
            string signInPayload = definition.PreparePayload(signInRequest);
            string targetUrl = string.Format(CultureInfo.CurrentCulture, JediDeviceSignInUrl, Device.Address);
            var signInResponse = ExecuteRequest(new Uri(targetUrl), signInPayload, definition, null);

            sessionId = signInResponse.Headers.GetValues("Cookie")?.FirstOrDefault()?.Split(';').FirstOrDefault();
            sessionId = sessionId?.Replace("sessionId=", string.Empty);

            return sessionId;

        }

        protected override HttpWebRequest BuildRequest(Uri url, HttpVerb method, PayloadDefinition payloadDefinition, string sessionId, string payload = null, string hideId = null)
        {
            HttpMessenger messenger = new HttpMessenger()
            {
                ContentType = "application/x-www-form-urlencoded",
                Accept = "text/html, application/xhtml+xml, image/jxr, */*",
                AllowAutoRedirect = true,
                Encoding = Encoding.GetEncoding("iso-8859-1"),
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

            HttpWebRequest request = messenger.BuildRequest(url, method, headers, payload);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            return request;
        }



        protected override void FillRemoveSolutionData(HttpWebRequest webRequest, string solutionName, string sessionId)
        {
            throw new NotImplementedException();
        }
    }
}
