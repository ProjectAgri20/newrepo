using System;
using System.IO;
using System.Net;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// The response returned from an HTTP call made via <see cref="HttpWebEngine" />.
    /// </summary>
    public sealed class HttpWebResult
    {
        /// <summary>
        /// Gets the response status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the response cookies.
        /// </summary>
        public CookieCollection Cookies { get; }

        /// <summary>
        /// Gets the response headers.
        /// </summary>
        public WebHeaderCollection Headers { get; }

        /// <summary>
        /// Gets the response body.
        /// </summary>
        public string Response { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebResult" /> class.
        /// </summary>
        /// <param name="webResponse">The <see cref="HttpWebResponse" /> to wrap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="webResponse" /> is null.</exception>
        internal HttpWebResult(HttpWebResponse webResponse)
        {
            if (webResponse == null)
            {
                throw new ArgumentNullException(nameof(webResponse));
            }

            StatusCode = webResponse.StatusCode;
            Cookies = webResponse.Cookies;
            Headers = webResponse.Headers;
            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
                Response = reader.ReadToEnd();
            }
        }
    }
}
