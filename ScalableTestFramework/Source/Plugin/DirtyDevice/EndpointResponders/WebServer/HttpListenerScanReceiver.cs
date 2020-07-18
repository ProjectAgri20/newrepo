using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    /// <summary>
    /// HttpListener that simulates a http server
    /// </summary>
    public class HttpListenerScanReceiver : HttpListenerBase
    {
        #region Constructor

        public HttpListenerScanReceiver(string address) : base()
        {
            _listener = new HttpListener();
            SetEndpointAddress(address);
            _listener.Prefixes.Add(EndpointAddress);
            _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            DoStart();
        }

        #endregion Constructor

        #region Public methods

        public IEnumerable<FileInfo> SaveUploadedFiles(DirectoryInfo targetFolder)
        {
            targetFolder.Create();
            foreach (RequestInstance instance in (RequestsReceived))
            {
                foreach (UploadedFile file in instance.Files)
                {
                    var path = new FileInfo(Path.Combine(targetFolder.FullName, file.Filename));
                    File.WriteAllBytes(path.FullName, file.Contents);
                    yield return path;
                }
            }
        }

        #endregion Public methods

        #region Private helpers

        /// <summary>
        /// How the listener responds when it receives a request
        /// </summary>
        /// <param name="context"></param>
        protected override void ListenerResponse(HttpListenerContext context)
        {
            Thread.Sleep(1000);

            AddRequest(context);
            context.Response.OutputStream.Close();
            context.Response.Close();
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
            if (context.Request.ContentType != null && context.Request.ContentType.ToLower().Contains("multipart"))
            {
                newRequest.Files = UploadedFileParser.ParseUploadedFiles(context.Request.InputStream);
            }

            return newRequest;
        }

        #endregion Private helpers
    }
}