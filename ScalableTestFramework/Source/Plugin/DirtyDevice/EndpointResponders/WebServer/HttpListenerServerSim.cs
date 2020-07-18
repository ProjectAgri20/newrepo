using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    /// <summary>
    /// HttpListener that simulates a http server
    /// </summary>
    public class HttpListenerServerSim : HttpListenerBase
    {
        #region Constructor

        /// <summary>
        /// Constructor, creates listener and starts listening
        /// </summary>
        /// <param name="dut">The device under test</param>
        /// <param name="localIp">The ip where listener will be hosted</param>
        public HttpListenerServerSim(string localIp) : base()
        {
            _listener = new HttpListener();
            _endpointAddress = "http://" + localIp + ":" + "48650" + "/HttpListenerServerSim" + InstanceID.ToString() + "/";
            _listener.Prefixes.Add(EndpointAddress);
            _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            DoStart();
        }

        #endregion Constructor

        #region Members

        /// <summary>
        /// URL for Http requests, uses a page which includes a refresh button
        /// </summary>
        public string HttpRefreshUrl
        {
            get
            {
                return EndpointAddress + "HttpPageWithRefresh.html";
            }
        }

        private string AuthenticationLogin
        {
            get
            {
                return @"<html><head><title>" +
                        "Authentication Login" +
                        "</title></head><body>" +
                        "<form action=\"" + HttpRefreshUrl + "\" method=\"post\"\" method=\"post\">" +
                        "<p>Username:<input type=\"text\" name=\"uname\" size=\"20\"/>" +
                        "<p>Password:<input type=\"password\" name=\"password\" size=\"20\"/>" +
                        "<p>Domain:<input type=\"text\" name=\"domain\" size=\"20\"/>" +
                        "<p><input type=\"submit\" name=\"submit\" value=\"log in\"/>" +
                        "</form>" +
                        "<form><input type=\"button\"value=\"Close Window\"id=\"Close\"onClick=\"window.close()\"><form>" +
                        "</body></html>";
            }
        }

        private string AuthenticationLoginXml
        {
            get
            {
                return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                       "<cloudUiDescription name=\"" +
                       Convert.ToBase64String(BitConverter.GetBytes(DateTime.UtcNow.Ticks)) +
                       "\" defaultScreen=\"default\" xmlns=\"http://www.hp.com/schemas/imaging/cnx/sip/cloudui/2010/6/10\">" +
                       "<parameter type=\"single\" name=\"domain\" /> " +
                       "<parameter type=\"single\" name=\"uname\" />  " +
                       "<parameter type=\"single\" name=\"password\" /> " +
                       "<screen id=\"default\" allowCache=\"false\">" +
                       "  <link logical=\"submit\" labelText=\"submit\" href=\"#done\" />" +
                       "  <link logical=\"cancel\" labelText=\"Cancel\" href=\"control:end\" />" +
                       "  <static location=\"heading\" labelText=\"" +
                       "Authentication Login" +
                       "\" />" +
                       "  <form immediateAccess=\"false\">" +
                       "    <inputBox param=\"domain\" labelText=\"Domain\" labelAlign=\"left\" order=\"1\" entryPrompt=\"DOMAIN\" keyboardPrompt=\"DOMAIN\"/>" +
                       "    <inputBox param=\"uname\" labelText=\"User\" labelAlign=\"left\" order=\"2\" entryPrompt=\"username\" keyboardPrompt=\"username\"/>" +
                       "    <inputBox type=\"password\" param=\"password\" labelText=\"Password\" order=\"3\" labelAlign=\"left\" entryPrompt=\"password\" keyboardPrompt=\"password\"/>" +
                       "  </form>" +
                       "</screen>" +
                       "<screen id=\"done\" showBusy=\"true\" allowCache=\"false\">" +
                       "  <autoLink timeout=\"1\" httpMethod=\"POST\" href=\"" + HttpRefreshUrl + "\" />" +
                       "  <static location=\"info\" labelText=\"Authenticating\" />" +
                       "  <link logical=\"cancel\" labelText=\"Cancel\" href=\"control:end\" />" +
                       "</screen>" +
                       "</cloudUiDescription>";
            }
        }

        private string HttpPageWithSubmit
        {
            get
            {
                return "<html><head><title>Http Page With Submit</title></head>" +
                    @"<body onload=""javascript:OXPd.homeButtonVisible = true;
                            javascript:OXPd.homeButtonEnabled = true;	
                            javascript:OXPd.startButtonVisible = true;
                            javascript:OXPd.startButtonEnabled = true;
	                        javascript:OXPd.helpButtonVisible = true;
                            javascript:OXPd.helpButtonEnabled = true;"">" +
                        "<h1>Hello World</h1>" + "<form name=\"input\" action=\"" + EndpointAddress + "HttpPageWithSubmit.html" + "\" method=\"post\"><input type=\"text\" name=\"user\"><input type=\"submit\" value=\"Submit\" id=\"submit\"></form>" +
                        @"<form><input type=""button""value=""Close Window""onClick=""window.close()""><form>
                            </body></html>";
            }
        }

        private string HttpPageQuotaUserPrompt
        {
            get
            {
                return "<html><head><title>Quota userPrompt page</title></head>" +
                    @"<body onload=""javascript:OXPd.helpButtonVisible = true;
                            javascript:OXPd.helpButtonEnabled = true;"">" +
                        "<h1>Hello World</h1>" + "<form name=\"input\" action=\"" + EndpointAddress + "HttpPageWithSubmit.html" + "\" method=\"post\"><input type=\"text\" name=\"user\"><input type=\"submit\" value=\"Submit\" id=\"submit\"></form>" +
                        @"<form><input type=""button""value=""Close Window""onClick=""window.close()""><form>
                            </body></html>";
            }
        }

        private string HttpPageQuotaLimitReachedUserPrompt
        {
            get
            {
                return "<html><head><title>Quota LimitReached userPrompt page</title></head>" +
                    @"<body onload=""javascript:OXPd.helpButtonVisible = true;
                            javascript:OXPd.helpButtonEnabled = true;"">" +
                        "<h1>Hello World</h1>" + "<form name=\"input\" action=\"" + EndpointAddress + "HttpPageWithSubmit.html" + "\" method=\"post\"><input type=\"text\" name=\"user\"><input type=\"submit\" value=\"Submit\" id=\"submit\"></form>" +
                        @"<form><input type=""button""value=""Close Window""onClick=""window.close()""><form>
                            </body></html>";
            }
        }

        private string HttpPageWithRefresh
        {
            get
            {
                return "<html><head><title>Test</title></head>" +
                        @"<body onload=""javascript:OXPd.homeButtonVisible = true;
                            javascript:OXPd.homeButtonEnabled = true;	
                            javascript:OXPd.startButtonVisible = true;
                            javascript:OXPd.startButtonEnabled = true;
	                        javascript:OXPd.helpButtonVisible = true;
                            javascript:OXPd.helpButtonEnabled = true;"">" +
                        "<h1>Hello World</h1><a href=\"" + EndpointAddress + "HttpPageWithRefresh.html" + "\" id=\"refresh\">refresh</a>" +
                        @"<form><input type=""button""value=""Close Window""id=""Close""onClick=""window.close()""><form>
                            </body></html>";
            }
        }

        private string HttpPage
        {
            get
            {
                //return "<html><head><title>HTTP Test Page</title></head>" +
                //        "<body onload=\"javascript:OXPd.homeButtonVisible = true; javascript:OXPd.homeButtonEnabled = true; javascript:OXPd.startButtonVisible = true; javascript:OXPd.startButtonEnabled = true; javascript:OXPd.helpButtonVisible = true; javascript:OXPd.helpButtonEnabled = true;\">" +
                //        "<h1>Hello World</h1>" +
                //        "<form><input type=\"button\" value=\"Close Window\" onClick=\"window.close()\"><form>" +
                //        "</body></html>";
                return @"<html xmlns=""http://www.w3.org/1999/xhtml"" >
                            <head>
                            <title>Device In Use</title>
                            <script type=""text/javascript"">
		                        var secondsCounter = 0;
		                        var resetCounter = 0;
	
		                        function updateSeconds() {
			                        secondsCounter = secondsCounter + 1;
			                        document.getElementById(""seconds"").firstChild.nodeValue = secondsCounter;
		                        }

		                        function updateResets() {
			                        resetCounter = resetCounter + 1;
			                        document.getElementById(""resets"").firstChild.nodeValue = resetCounter;
		                        }

		                        function initialize() {
		                            if (window.OXPd != undefined) {
			                            OXPd.homeButtonVisible = true;
			                            OXPd.homeButtonEnabled = true;
				                        setInterval(""OXPd.resetUIInactivityTimer(); updateResets()"",5000);
			                        }
			                        setInterval(""updateSeconds()"", 1000);
		                        }
	                        </script>
                            </head>
                            <body onload=""initialize()"" style=""background: #FFFFFF"">
                            <!--- This document can be used when you call UIConfigurationService.ReserveUIContext() as the uri -->
                            <center>
                                (X)
                                <h1>This device is in use by a remote web application.</h1>
	                            <hr/>
	                            <h2>This device has been in use for <span id=""seconds"" style=""background-color: #000000; color: #FFFFFF"">0</span> seconds.</h2>
	                            <h2>This device's ui inactivity timer has been reset <span id=""resets"" style=""background-color: #000000; color: #FFFFFF"">0</span> times.</h2>
	                        </center>
                            </body>
                            </html>";
            }
        }

        private string HttpPageXml
        {
            get
            {
                return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                        "<cloudUiDescription xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" defaultScreen=\"HomeScreen\" name=\"ReserveUI Homescreen\" xmlns=\"http://www.hp.com/schemas/imaging/cnx/sip/cloudui/2010/6/10\">" +
                        "<screen id=\"HomeScreen\">" +
                        "<static location=\"heading\" labelText=\"UI is reserved\" />" +
                        "<list view=\"vert1ColTxtOnly\" kind=\"singleManualOk\">" +
                        "<item labelText=\"The UI is reserved.\" />" +
                        "</list></screen></cloudUiDescription>";
            }
        }

        private string HttpAuthBasicUnauthorized
        {
            get
            {
                return @"<html>
                            <head>
                                <title>Error 401 - Unauthorized: Access is denied due to invalid credentials</title>
                            </head>
                            <body>Error 401 - Unauthorized: Access is denied due to invalid credentials</body>
                        </html> ";
            }
        }

        private string HttpAuthBasicAuthorized
        {
            get
            {
                return @"<html>
                            <head>
                                <title>HTTP Basic Auth test webpage</title>
                            </head>
                            <body>HTTP Basic Auth test webpage</body>
                        </html> ";
            }
        }

        private List<CustomServerPage> CustomPages = new List<CustomServerPage>();


        #endregion Members

        #region Private Helpers

        /// <summary>
        /// How the listener responds when it receives a request
        /// </summary>
        /// <param name="context"></param>
        protected override void ListenerResponse(HttpListenerContext context)
        {
            Thread.Sleep(1000);

            string pageToUse;
            byte[] content;
            AddRequest(context);
            string pageRequested = RequestsReceived[RequestsReceived.Count - 1].Uri;
            pageRequested = pageRequested.Replace(EndpointAddress, "");
            if (IsAuthorized(context))
            {
                switch (pageRequested)
                {
                    case "HttpPage.html":
                        pageToUse = HttpPage;
                        context.Response.ContentType = "text/html";
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                        content = Encoding.UTF8.GetBytes(pageToUse);
                        break;
                    case "HttpPageWithRefresh.html":
                        pageToUse = HttpPageWithRefresh;
                        context.Response.ContentType = "text/html";
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                        content = Encoding.UTF8.GetBytes(pageToUse);
                        break;
                    case "HttpPageWithSubmit.html":
                        pageToUse = HttpPageWithSubmit;
                        context.Response.ContentType = "text/html";
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                        content = Encoding.UTF8.GetBytes(pageToUse);
                        break;
                    case "HttpPageQuotaUserPrompt.html":
                        pageToUse = HttpPageQuotaUserPrompt;
                        context.Response.ContentType = "text/html";
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                        content = Encoding.UTF8.GetBytes(pageToUse);
                        break;
                    case "HttpPageQuotaLimitReachedUserPrompt.html":
                        pageToUse = HttpPageQuotaLimitReachedUserPrompt;
                        context.Response.ContentType = "text/html";
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                        content = Encoding.UTF8.GetBytes(pageToUse);
                        break;
                    case "HttpPage.xml":
                        pageToUse = HttpPageXml;
                        context.Response.ContentType = "application/xml";
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                        content = Encoding.UTF8.GetBytes(pageToUse);
                        break;
                    case "AuthenticationLogin.html":
                        pageToUse = AuthenticationLogin;
                        context.Response.ContentType = "text/html";
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                        content = Encoding.UTF8.GetBytes(pageToUse);
                        break;
                    case "AuthenticationLogin.xml":
                        pageToUse = AuthenticationLoginXml;
                        context.Response.ContentType = "application/xml";
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                        content = Encoding.UTF8.GetBytes(pageToUse);
                        break;
                    case "HttpAuthBasic.html":
                        pageToUse = HttpAuthBasicAuthorized;
                        context.Response.ContentType = "text/html";
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                        content = Encoding.UTF8.GetBytes(pageToUse);
                        break;
                    default:
                        if (CustomPages.Exists(page => page.Name == pageRequested))
                        {
                            CustomServerPage customPage = CustomPages.Find(page => page.Name == pageRequested);
                            context.Response.ContentType = customPage.ContentType;
                            context.Response.StatusCode = 200;
                            context.Response.StatusDescription = "OK";
                            content = customPage.Content;
                            if (content == null) content = File.ReadAllBytes(customPage.FilePath);
                        }
                        else
                        {
                            pageToUse = "<html><body><h1>Error 404 Not Found</h1>";
                            context.Response.ContentType = "text/html";
                            context.Response.StatusCode = 404;
                            context.Response.StatusDescription = "Not Found";
                            content = Encoding.UTF8.GetBytes(pageToUse);
                        }
                        break;
                }

                SendContent(context, context.Response.ContentType, content);

            }
            else
            {
                if (pageRequested == "HttpAuthBasic.html")
                {
                    pageToUse = HttpAuthBasicUnauthorized;
                    context.Response.ContentType = "text/html";
                    context.Response.StatusCode = 401;
                    context.Response.StatusDescription = "Unauthorized";
                    content = Encoding.UTF8.GetBytes(pageToUse);

                    SendContent(context, context.Response.ContentType, content);
                }
                else
                {
                    context.Response.StatusCode = 401;
                    context.Response.StatusDescription = "Unauthorized";
                    context.Response.OutputStream.Close();
                }
            }
        }

        /// <summary>
        /// Converts post contents of received HttpListenerRequest to RequestInstance
        /// (Includes PostQueryString and uploaded files)
        /// </summary>
        /// <returns>RequestInstance containing post contents of HttpListenerRequest</returns>
        /// <param name="newRequest">The RequestInstance to put post contents into</param>
        /// <param name="context">the HttpListenerContext on listener</param>
        protected override RequestInstance ProcessPost(RequestInstance newRequest, HttpListenerContext context)
        {
            string rawPostQueryString = new StreamReader(context.Request.InputStream).ReadToEnd();
            newRequest.RawPostQueryString = rawPostQueryString;
            if (rawPostQueryString.Length > 0)
            {
                newRequest.PostQueryString = GetDictionaryFromCollection(HttpUtility.ParseQueryString(rawPostQueryString));
            }

            return newRequest;
        }

        #endregion Private helpers
    }
}