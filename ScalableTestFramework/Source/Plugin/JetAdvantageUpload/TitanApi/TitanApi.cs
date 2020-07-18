using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    //this is development
    //string proxyRootUrl = @"https://mfp-micro-staging.hpbizapps.com";
    //string pullPrintUrl = @"https://pp-micro-staging.hpfoghorn.com";

    // this is production
    //_jetAdvantageProxy = @"https://mfp.hpbizapps.com";
    //_jetAdvantageURL = @"https://pp.hpondemand.com";
    /// <summary>
    /// Class for interacting with Titan via APIs
    /// Refer to Travis Torkelson and/or Brent Foley if questions
    /// </summary>
    public class TitanAPI
    {
        private WebProxy _proxy;
        public string ProxyRootUrl { get; private set; }
        public string PullPrintUrl { get; private set; }
        private const string LoginTemplate = "{0}/api/v2/authn/login/unamepwd{1}";
        private const string DocumentContentBase = "/api/v2/documents/{0}/content/{1}";
        private const string PullPrintShortcut = "print";
        private const string PrintQueueDocumentsUrlTemplate = "{0}/api/v2/folders/{1}/documents";
        private const string PrintQueueUploadUrlTemplate = "{0}/api/v2/documents/{1}";
        private const string DeleteDocumentUrlTemplate = "{0}/api/v2/documents/{1}";

        private string DeleteDocumentUrl(string docId)
        {
            return String.Format(DeleteDocumentUrlTemplate, PullPrintUrl, docId);
        }

        private string PrintQueueDocumentsUrl()
        {
            return String.Format(PrintQueueDocumentsUrlTemplate, PullPrintUrl, PullPrintShortcut);
        }

        private string PrintQueueUploadUrl()
        {
            return String.Format(PrintQueueUploadUrlTemplate, PullPrintUrl, PullPrintShortcut);
        }

        private string LoginUrl(string tokenName)
        {
            return String.Format(LoginTemplate, ProxyRootUrl, "?returnTokenName=" + tokenName);
        }

        public TitanAPI()
            : this(null, null)
        {
        }

        public TitanAPI(string proxyrootUrl, string pullprintUrl)
        {
            // Set default web proxy so that we can talk to the Titan servers
            _proxy = new WebProxy("web-proxy.corp.hp.com", 8088);
            WebRequest.DefaultWebProxy = _proxy;

            // Default to test stack URLs
            ProxyRootUrl = "https://www-testhp.hpfoghorn.com/ProxyRoot";
            if (!String.IsNullOrEmpty(proxyrootUrl))
            {
                ProxyRootUrl = proxyrootUrl;
            }
            PullPrintUrl = "https://www-testhp.hpfoghorn.com/PullPrint/";
            if (!String.IsNullOrEmpty(pullprintUrl))
            {
                PullPrintUrl = pullprintUrl;
            }
        }

        public string Login(TitanUser requestor)
        {
            string result = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(LoginUrl("X-Auth-Token"));

            //http://blog.kowalczyk.info/article/at3/Forcing-basic-http-authentication-for-HttpWebReq.html
            string authInfo = requestor.EmailAddress + ":" + requestor.Password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers.Add("Authorization", "Basic " + authInfo);
            request.Accept = "x-auth-token/header";
            request.Method = "GET";
            request.Headers.Add("Version", "1");
            request.Headers.Add("X-Client-Id", "482a16dc-a6a6-4e28-8ff5-cb050731d933");

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                result = response.Headers["X-Auth-Token"];
                if (String.IsNullOrEmpty(result))
                {
                    //This is only tested because
                    String cookie = response.Headers["Set-Cookie"];
                    if (cookie.StartsWith("X-Auth-Token="))
                    {
                        result = cookie.Substring(13);
                    }
                }
            }

            if (result == null)
            {
                throw CreateException("Failed to login the user " + requestor);
            }

            return result;
        }

        private JetAdvantageException CreateException(string message, Exception ex = null)
        {
            JetAdvantageException result = null;
            if (ex == null)
            {
                result = new JetAdvantageException(message);
            }
            else
            {
                result = new JetAdvantageException(message, ex);
            }

            result.ProxyRootUrl = ProxyRootUrl;
            result.PullPrintUrl = PullPrintUrl;
            return result;
        }

        public List<TitanDocument> GetPrintQueue(TitanUser requestor)
        {
            try
            {
                return GetPrintQueue(Login(requestor));
            }
            catch (Exception)
            {
                throw CreateException("Unable to retrieve queued docs for " + requestor);
            }
        }

        public List<TitanDocument> GetPrintQueue(String authToken)
        {
            List<TitanDocument> docs = new List<TitanDocument>();
            HttpWebRequest crsGetRequest = (HttpWebRequest)WebRequest.Create(PrintQueueDocumentsUrl());
            crsGetRequest.Accept = "application/json";
            crsGetRequest.Method = "GET";
            crsGetRequest.Headers.Add("Version", "1");
            crsGetRequest.Headers.Add("X-Client-Id", "482a16dc-a6a6-4e28-8ff5-cb050731d933");
            crsGetRequest.Headers.Add("X-Auth-Token", authToken);
            WebResponse crsGetResponse = null;

            using (crsGetResponse = crsGetRequest.GetResponse())
            {
                using (Stream stream = crsGetResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    using (MemoryStream payload = new MemoryStream(Encoding.UTF8.GetBytes(reader.ReadToEnd())))
                    {
                        DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(PrintQueueResponse));
                        PrintQueueResponse getResponse = (PrintQueueResponse)dcs.ReadObject(payload);
                        foreach (TitanDocument doc in getResponse.data)
                        {
                            docs.Add(doc);
                        }
                    }
                }
            }
            return docs;
        }

        public TitanDocument UploadDocument(TitanUser requestor, FileInfo file)
        {
            return UploadDocument(Login(requestor), file);
        }

        public TitanDocument UploadDocument(String authToken, FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (!file.Exists)
                throw new FileNotFoundException("The file \"" + file.FullName + "\" does not exist.");

            HttpWebRequest crsPostRequest = (HttpWebRequest)WebRequest.Create(PrintQueueUploadUrl());
            crsPostRequest.Accept = "application/json";
            crsPostRequest.Method = "POST";
            crsPostRequest.Headers.Add("Version", "1");
            crsPostRequest.Headers.Add("X-Client-Id", "482a16dc-a6a6-4e28-8ff5-cb050731d933");
            crsPostRequest.Headers.Add("X-Auth-Token", authToken);
            Int64 boundary = DateTime.Now.Ticks;
            crsPostRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            Stream requestStream = crsPostRequest.GetRequestStream();
            StringBuilder postData = new StringBuilder();
            postData.Append("--" + boundary + "\r\n");
            postData.Append("Content-Disposition: form-data; name=\"file\"; filename=\"" + file.FullName + "\"\r\n");
            postData.Append("Content-Type: application/pdf\r\n\r\n");

            byte[] postBytes = Encoding.UTF8.GetBytes(postData.ToString());
            requestStream.Write(postBytes, 0, postBytes.Length);

            byte[] buffer = new byte[4096];
            int read;

            using (FileStream fs = File.Open(file.FullName, FileMode.Open))
            {
                while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    requestStream.Write(buffer, 0, read);
                    requestStream.Flush();
                }

                byte[] boundaryEnd = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                requestStream.Write(boundaryEnd, 0, boundaryEnd.Length);
                requestStream.Close();

                using (WebResponse crsPostResponse = crsPostRequest.GetResponse())
                {
                    using (Stream responseStream = crsPostResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(fs, Encoding.UTF8);
                        reader.ReadToEnd();
                    }
                }
            }
            return waitForDocument(authToken, file.Name);
        }

        private TitanDocument waitForDocument(String authToken, String fileName)
        {
            Stopwatch timer = Stopwatch.StartNew();
            List<TitanDocument> docs = GetPrintQueue(authToken);

            while (!docs.Exists(x => x.name == fileName) && timer.Elapsed.Seconds < 30)
            {
                Thread.Sleep(1000);
                docs = GetPrintQueue(authToken);
            }

            TitanDocument doc = docs.FirstOrDefault(x => x.name == fileName);

            if (doc == null)
                throw CreateException("Unable to verify document was uploaded after 30 seconds.");

            return doc;
        }

        public void DeleteDocuments(TitanUser requestor)
        {
            DeleteDocuments(Login(requestor));
        }

        public void DeleteDocuments(String authToken)
        {
            foreach (TitanDocument doc in GetPrintQueue(authToken))
            {
                DeleteDocument(authToken, doc);
            }
        }

        public string DeleteDocument(TitanUser requestor, TitanDocument document)
        {
            return DeleteDocument(Login(requestor), document);
        }

        public string DeleteDocument(string authToken, TitanDocument document)
        {
            if (String.IsNullOrEmpty(authToken))
                throw new ArgumentException("authToken cannot be null or empty.");
            if (document == null)
                throw new ArgumentNullException("document");

            string result = null;
            HttpWebRequest crsGetRequest = (HttpWebRequest)WebRequest.Create(DeleteDocumentUrl(document.documentId));
            crsGetRequest.Accept = "application/json";
            crsGetRequest.Method = "DELETE";
            crsGetRequest.Headers.Add("Version", "1");
            crsGetRequest.Headers.Add("X-Client-Id", "482a16dc-a6a6-4e28-8ff5-cb050731d933");
            crsGetRequest.Headers.Add("X-Auth-Token", authToken);

            using (WebResponse crsGetResponse = crsGetRequest.GetResponse())
            {
                using (Stream stream = crsGetResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }
    }
}




