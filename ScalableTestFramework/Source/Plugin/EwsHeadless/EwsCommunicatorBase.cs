using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using HP.DeviceAutomation;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    internal abstract class EwsCommunicatorBase : IEwsCommunicator
    {
        protected IDevice Device;
        protected string Password;
        private readonly string _deviceFamily;
        protected string alternateCSRFUrl = string.Empty;
        protected string SessionId = string.Empty;

        internal EwsCommunicatorBase(IDevice device, string password, string deviceFamily)
        {
            Device = device;
            Password = password;
            _deviceFamily = deviceFamily;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        /// <summary>
        /// Submits the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>An <see cref="EwsResult" /> containing the result of the HTTP request(s).</returns>
        /// <exception cref="System.Exception">Validation failed with contract provided.</exception>
        public EwsResult Submit(EwsRequest request)
        {
            ////Create XML file and validae with the respective XML schema depending on the Filter Type
            if (request.Validate())
            {
                //Get the communication request
                List<PayloadDefinition> payloadDefinitions = EwsPayloadFactory.CreatePayloadDefinitions(request, Device.GetDeviceInfo().FirmwareRevision, _deviceFamily);

                //Applying on the device based on the ICommunicator
                return CommunicateWithDevice(payloadDefinitions, request);
            }

            throw new Exception("Validation failed with contract provided.");
        }

        /// <summary>
        /// Creates an <see cref="EwsRequest" /> of the specified type.
        /// </summary>
        /// <param name="requestType">The request type.</param>
        /// <returns>An <see cref="EwsRequest" />.</returns>
        public EwsRequest CreateRequest(string requestType)
        {
            return CreateRequest(requestType, string.Empty);
        }

        /// <summary>
        /// Creates an <see cref="EwsRequest" /> of the specified type and subtype.
        /// </summary>
        /// <param name="requestType">The request type.</param>
        /// <param name="requestSubtype">The request subtype.</param>
        /// <returns>An <see cref="EwsRequest" />.</returns>
        public EwsRequest CreateRequest(string requestType, string requestSubtype)
        {
            return new EwsRequest(requestType, requestSubtype, EwsPayloadFactory.SelectContract(_deviceFamily, requestType, Device.GetDeviceInfo().FirmwareRevision));
        }

        protected abstract string GetSessionId(PayloadDefinition definition);
        protected abstract HttpWebRequest BuildRequest(Uri url, HttpVerb method, PayloadDefinition payloadDefinition, string sessionId, string payload = null, string hideId = null);
        protected abstract void FillRemoveSolutionData(HttpWebRequest webRequest, string solutionName, string sessionId);
        protected virtual string SignIn(string sessionId)
        {
            // Nothing to do in the base class
            return sessionId;
        }

        protected virtual EwsResult CommunicateWithDevice(List<PayloadDefinition> payloadDefinitions, EwsRequest request)
        {
            List<HttpResponse> commResponseList = new List<HttpResponse>();
            try
            {
               
                string hideId = string.Empty;
                bool isSignedIn = false;

                foreach (PayloadDefinition definition in payloadDefinitions)
                {
                    bool executeRequest = true;
                    if (definition.IsSessionIdRequired && string.IsNullOrEmpty(SessionId))
                    {
                        SessionId = GetSessionId(definition);
                    }
                    if (definition.IsHideRequired && string.IsNullOrEmpty(hideId))
                    {
                        hideId = GetHideId(request, definition);
                        executeRequest = false;
                    }
                    if (request.RequestSubtype.Equals("WebProxy"))
                    {
                        alternateCSRFUrl = string.Format(CultureInfo.CurrentCulture, "https://{0}/webserviceproxy.htm", Device.Address);
                    }
                    else if (request.RequestSubtype.Equals("NetworkSettings"))
                    {
                        alternateCSRFUrl = string.Format(CultureInfo.CurrentCulture, "https://{0}/snmp_creds.html", Device.Address);
                    }
                    else if (request.RequestSubtype.Equals("ManageSupplies") || request.RequestSubtype.Equals("WithHolePunch") || request.RequestSubtype.Equals("WithoutHolePunch")
                        || request.RequestSubtype.Equals("FaxReceiveSetup"))
                    {
                        alternateCSRFUrl = string.Format(CultureInfo.CurrentCulture, definition.TargetUrl, Device.Address);
                    }      

                    // Sign in with Password provided
                    if (!isSignedIn && !string.IsNullOrEmpty(Password))
                    {
                        SessionId = SignIn(SessionId);
                        isSignedIn = true;
                    }

                    FillStateValues(request, SessionId, definition);

                    //This is the actual request with the session ID
                    string targetUrl = string.Format(CultureInfo.CurrentCulture, definition.TargetUrl, Device.Address);
                    string filledPayload = definition.PreparePayload(request);
                    if (executeRequest)
                    {
                        var commResponse = ExecuteRequest(new Uri(targetUrl), filledPayload, definition,  hideId);
                        commResponseList.Add(commResponse);
                    }
                    //let's sleep for a second
                    Delay.Wait(TimeSpan.FromSeconds(2));
                }

                return new EwsResult(commResponseList);
            }
            catch (Exception ex)
            {
                return new EwsResult(commResponseList, ex);
            }
        }

        private void FillStateValues(EwsRequest request, string sessionId, PayloadDefinition definition)
        {
            if (definition.IsViewStateRequired)
            {
                Uri viewStateUrl = null;
                //Getting the viewstate by making request to the URL provided
                //Implemented the below functionality to get the tray ID for which the setting needs to be implemented
                if (request.RequestType.Equals("ManageTrays") && request.RequestSubtype.Equals("Tray"))
                {
                    viewStateUrl = new Uri(string.Format(CultureInfo.CurrentCulture, definition.ViewStateUrl, Device.Address, request.PayloadValues["Tray"]));
                }
                else
                {
                    viewStateUrl = new Uri(string.Format(CultureInfo.CurrentCulture, definition.ViewStateUrl, Device.Address));
                }
                HttpWebRequest httpRequest = BuildRequest(viewStateUrl, HttpVerb.Get, definition, sessionId);
                var viewStateResponse = HttpMessenger.ExecuteRequest(httpRequest);

                //Getting the session ID from the response recieved
                string viewstate = GetViewState(viewStateResponse.Body);
                if (!string.IsNullOrEmpty(viewstate))
                {
                    //Manage trays expects HPViewState and not just ViewState in the payload
                    if (request.RequestType.Equals("ManageTrays") && request.RequestSubtype.Equals("Tray"))
                    {
                        viewstate = System.Web.HttpUtility.HtmlDecode(viewstate);
                        request.AddWithoutValidate("HPViewState", viewstate);
                    }
                    else
                    {
                        request.AddWithoutValidate("ViewState", viewstate);
                    }
                }
            }

            if (definition.IsWizardIdRequired)
            {
                //Getting the viewstate by making request to the URL provided
                var wizardIdUrl = new Uri(string.Format(CultureInfo.CurrentCulture, definition.WizardIdUrl, Device.Address));
                HttpWebRequest httpRequest = BuildRequest(wizardIdUrl, HttpVerb.Get, definition, sessionId);
                var wizardResponse = HttpMessenger.ExecuteRequest(httpRequest);

                //Getting the session ID from the response recieved
                string wizardId = GetWizardId(wizardResponse.Body);
                if (!string.IsNullOrEmpty(wizardId))
                {
                    request.AddWithoutValidate("WizardID", wizardId);
                }
            }
        }

        private static string GetWizardId(string response)
        {
            try
            {
                if (!string.IsNullOrEmpty(response))
                {
                    int start = response.IndexOf("WizardID\" value=\"", StringComparison.Ordinal) + 16;
                    int end = response.IndexOf("\"", start, StringComparison.Ordinal);
                    return (response.Substring(start, (end - start)));
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new DeviceCommunicationException("Exception occurred while fetching the Wizard Id", ex.InnerException);
            }
            throw new DeviceCommunicationException("Could not get the Wizard Id, please check the Wizard Id URL.");
        }

        private static string GetViewState(string response)
        {
            try
            {
                if (!string.IsNullOrEmpty(response))
                {
                    int start = response.IndexOf("HPViewState\" value=\"", StringComparison.Ordinal) + 20;
                    int end = response.IndexOf("\"", start, StringComparison.Ordinal);
                    return (response.Substring(start, (end - start)));
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new DeviceCommunicationException("Exception occurred while fetching the View State", ex.InnerException);
            }
            throw new DeviceCommunicationException("Could not get the View State, please check the View State URL.");
        }

        private string GetHideId(EwsRequest request, PayloadDefinition definition)
        {
            try
            {
                if (definition.IsHideRequired)
                {
                    //Getting the HideID by making request to the URL provided
                    string hideId = string.Empty;
                    var hideUrl = new Uri(string.Format(CultureInfo.CurrentCulture, definition.HideUrl, Device.Address));
                    HttpWebRequest httpRequest = BuildRequest(hideUrl, HttpVerb.Post, definition, SessionId, definition.Payload);
                    var hideResponse = HttpMessenger.ExecuteRequest(httpRequest);
                    if (!string.IsNullOrEmpty(hideResponse.Body))
                    {
                        int start = hideResponse.Body.IndexOf("name=\"Hide\" VALUE=\"", StringComparison.Ordinal) + 19;
                        int end = hideResponse.Body.IndexOf("\"", start, StringComparison.Ordinal);
                        hideId = (hideResponse.Body.Substring(start, (end - start)));
                    }
                    //Getting the session ID from the response recieved
                    if (!string.IsNullOrEmpty(hideId))
                    {
                        request.AddWithoutValidate("HideID", hideId);
                    }
                    return hideId;
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new DeviceCommunicationException("Exception occurred while fetching the Hide Id ", ex.InnerException);
            }
            throw new DeviceCommunicationException("Could not get the Hide Id, please check the Hide Id URL.");
        }



        protected HttpResponse ExecuteRequest(Uri url, string payload, PayloadDefinition payloadDefinition, string hideId)
        {
            string solutionName = string.Empty;
            if (payloadDefinition.IsRemoveSolution)
            {
                solutionName = payload;
                payload = string.Empty;
            }
            var httpRequest = BuildRequest(url, payloadDefinition.HttpMethod, payloadDefinition, SessionId, payload, hideId);
            if (payloadDefinition.IsUpload)
            {
                FillUploadData(httpRequest, payload, "certificate", payloadDefinition.NameValuePairs);
            }
            else if (payloadDefinition.IsRemoveSolution)
            {
                FillRemoveSolutionData(httpRequest, solutionName, SessionId);
            }
           
            return HttpMessenger.ExecuteRequest(httpRequest);
        }

        
        

        private static void FillUploadData(HttpWebRequest webRequest, string file, string paramName, NameValueCollection nvc)
        {
            string contentType = "application/x-x509-ca-cert";
            webRequest.ServicePoint.Expect100Continue = false;
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", CultureInfo.CurrentCulture);
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            using (Stream requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                string headerTemplate = $"Content-Disposition: form-data; name=\"{paramName}\"; filename=\"{Path.GetFileName(file)}\"\r\nContent-Type: {contentType}\r\n\r\n";
                byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(headerTemplate);
                requestStream.Write(headerBytes, 0, headerBytes.Length);

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
                    string formitem = string.Format(CultureInfo.CurrentCulture, formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    requestStream.Write(formitembytes, 0, formitembytes.Length);
                }

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                requestStream.Write(trailer, 0, trailer.Length);
                webRequest.UserAgent = "IE";
            }
        }


    }
}