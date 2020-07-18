using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// User agents that can be provided to <see cref="HttpWebEngine" />.
    /// </summary>
    public enum BrowserAgent
    {
        /// <summary>
        /// Internet Explorer 10 and above
        /// </summary>
        IE,

        /// <summary>
        /// Mozilla Firefox version 31 and above
        /// </summary>
        Firefox,

        /// <summary>
        /// Google Chrome version 37 and above
        /// </summary>
        Chrome,

        /// <summary>
        /// Apple Safari
        /// </summary>
        Safari,

        /// <summary>
        /// Opera browser
        /// </summary>
        Opera
    }

    /// <summary>
    /// Provides methods for HTTP web operations.
    /// </summary>
    public static class HttpWebEngine
    {
        private static readonly Dictionary<BrowserAgent, string> _userAgents = new Dictionary<BrowserAgent, string>
        {
            [BrowserAgent.IE] = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
            [BrowserAgent.Firefox] = "Mozilla/5.0 (Windows NT 5.1; rv:31.0) Gecko/20100101 Firefox/31.0",
            [BrowserAgent.Chrome] = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2049.0 Safari/537.36",
            [BrowserAgent.Safari] = "Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25",
            [BrowserAgent.Opera] = "Opera/9.80 (Windows NT 6.0) Presto/2.12.388 Version/12.14"
        };

        /// <summary>
        /// Executes the specified <see cref="HttpWebRequest" /> and returns the result.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static HttpWebResult Execute(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return new HttpWebResult(response);
            }
        }

        /// <summary>
        /// Executes the specified HTTP GET request.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Get(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Method = "GET";
            return Execute(request);
        }

        /// <summary>
        /// Executes the specified HTTP GET request as the specified browser agent.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="agent">The <see cref="BrowserAgent" />.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Get(HttpWebRequest request, BrowserAgent agent)
        {
            SetUserAgent(request, agent);
            return Get(request);
        }

        /// <summary>
        /// Executes the specified HTTP PUT request.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="data">The data payload for the PUT request.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Put(HttpWebRequest request, string data)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Method = "PUT";
            request.ServicePoint.Expect100Continue = false;
            SetData(request, data);
            return Execute(request);
        }

        /// <summary>
        /// Executes the specified HTTP PUT request as the specified browser agent.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="data">The data payload for the PUT request.</param>
        /// <param name="agent">The <see cref="BrowserAgent" />.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Put(HttpWebRequest request, string data, BrowserAgent agent)
        {
            SetUserAgent(request, agent);
            return Put(request, data);
        }

        /// <summary>
        /// Executes the specified HTTP PUT request.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="data">The data payload for the PUT request.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Put(HttpWebRequest request, byte[] data)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Method = "PUT";
            request.ServicePoint.Expect100Continue = false;
            SetData(request, data);
            return Execute(request);
        }

        /// <summary>
        /// Executes the specified HTTP PUT request as the specified browser agent.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="data">The data payload for the PUT request.</param>
        /// <param name="agent">The <see cref="BrowserAgent" />.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Put(HttpWebRequest request, byte[] data, BrowserAgent agent)
        {
            SetUserAgent(request, agent);
            return Put(request, data);
        }

        /// <summary>
        /// Executes the specified HTTP POST request.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="data">The data payload for the POST request.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Post(HttpWebRequest request, string data)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Method = "POST";
            request.ServicePoint.Expect100Continue = false;
            SetData(request, data);
            return Execute(request);
        }

        /// <summary>
        /// Executes the specified HTTP POST request as the specified browser agent.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="data">The data payload for the POST request.</param>
        /// <param name="agent">The <see cref="BrowserAgent" />.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Post(HttpWebRequest request, string data, BrowserAgent agent)
        {
            SetUserAgent(request, agent);
            return Post(request, data);
        }

        /// <summary>
        /// Executes the specified HTTP POST request.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="data">The data payload for the POST request.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Post(HttpWebRequest request, byte[] data)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Method = "POST";
            request.ServicePoint.Expect100Continue = false;
            SetData(request, data);
            return Execute(request);
        }

        /// <summary>
        /// Executes the specified HTTP POST request as the specified browser agent.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="data">The data payload for the POST request.</param>
        /// <param name="agent">The <see cref="BrowserAgent" />.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Post(HttpWebRequest request, byte[] data, BrowserAgent agent)
        {
            SetUserAgent(request, agent);
            return Post(request, data);
        }

        /// <summary>
        /// Executes the specified HTTP DELETE request.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Delete(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Method = "DELETE";
            request.ContentLength = 0;
            return Execute(request);
        }

        /// <summary>
        /// Executes the specified HTTP DELETE request as the specified browser agent.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="agent">The <see cref="BrowserAgent" />.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult Delete(HttpWebRequest request, BrowserAgent agent)
        {
            SetUserAgent(request, agent);
            return Delete(request);
        }

        /// <summary>
        /// Uploads a file using an HTTP POST request as the specified browser agent.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest" />.</param>
        /// <param name="file">The file to upload.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="formData">The form data.</param>
        /// <param name="agent">The <see cref="BrowserAgent" />.</param>
        /// <returns>An <see cref="HttpWebResult" /> object representing the result of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public static HttpWebResult UploadFile(HttpWebRequest request, string file, string paramName, NameValueCollection formData, BrowserAgent agent)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Method = "POST";
            request.ServicePoint.Expect100Continue = false;
            SetUserAgent(request, agent);
            SetFileUploadData(request, file, paramName, formData);
            return Execute(request);
        }

        private static void SetUserAgent(HttpWebRequest request, BrowserAgent agent)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.UserAgent = _userAgents[agent];
        }

        private static void SetData(HttpWebRequest request, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(data);
                SetData(request, bytes);
            }
        }

        private static void SetData(HttpWebRequest request, byte[] bytes)
        {
            request.ContentLength = bytes.Length;

            using (Stream writeStream = request.GetRequestStream())
            {
                writeStream.Write(bytes, 0, bytes.Length);
            }
        }

        private static void SetFileUploadData(HttpWebRequest request, string file, string paramName, NameValueCollection formData)
        {
            using (Stream requestStream = request.GetRequestStream())
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                string contentType = "application/x-x509-ca-cert";
                string headerTemplate = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, Path.GetFileName(file), contentType);
                byte[] headerBytes = Encoding.UTF8.GetBytes(headerTemplate);
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

                const string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in formData.Keys)
                {
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, formData[key]);
                    byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                    requestStream.Write(formitembytes, 0, formitembytes.Length);
                }

                byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                requestStream.Write(trailer, 0, trailer.Length);
            }
        }
    }
}
