using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HP.DeviceAutomation;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    /// <summary>
    /// Base class for custom http listener classes
    /// </summary>
    public class HttpListenerBase : AgentCallbackListenerBase
    {
        public event EventHandler<StatusChangedEventArgs> UpdateStatus;
        private Guid _instanceID;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpListenerBase() : base()
        {
            _instanceID = Guid.NewGuid();
        }

        #endregion Constructor

        #region Destructor

        /// <summary>
        /// Destructor, closes callback channel
        /// </summary>
        ~HttpListenerBase()
        {
            _listener.Prefixes.Clear();
            _listener.Close();
            // This was throwing "An unhandled exception of type 'System.InvalidOperationException' occurred in mscorlib.dll; A task may only be disposed if it is in a completion state(RanToCompletion, Faulted or Canceled)"
            // Line is commented out per https://blogs.msdn.microsoft.com/pfxteam/2012/03/25/do-i-need-to-dispose-of-tasks/
            //_listenerTask.Dispose();
        }

        #endregion Destructor

        #region Members

        public Guid InstanceID
        {
            get
            {
                return _instanceID;
            }
        }

        /// <summary>
        /// Object used for listener thread lock
        /// </summary>
        protected object _listenerLock = new object();

        /// <summary>
        /// the HttpListener
        /// </summary>
        protected HttpListener _listener;

        /// <summary>
        /// The task (thread) that the listener is running on
        /// </summary>
        protected Task _listenerTask;

        /// <summary>
        /// A list of all http requests received by the listener
        /// </summary>
        public List<RequestInstance> RequestsReceived = new List<RequestInstance>();

        /// <summary>
        /// ResetEvent to start listening for next request
        /// </summary>
        private AutoResetEvent _resetEvent = new AutoResetEvent(true);

        /// <summary>
        /// The endpoint address of the listener, using ip address
        /// </summary>
        protected string _endpointAddress = null;
        /// <summary>
        /// The endpoint address of the listener, using ip address
        /// </summary>
        public override string EndpointAddress
        {
            get { return _endpointAddress; }
            protected set
            {
                lock (_listenerLock)
                {
                    _endpointAddress = value;
                    _listener.Prefixes.Clear();
                    _listener.Prefixes.Add(_endpointAddress);
                }
            }
        }

        private NetworkCredential _credential = null;
        /// <summary>
        /// Credentials required to access the listener endpoint
        /// </summary>
        private NetworkCredential Credential
        {
            get
            {
                return _credential;
            }
            set
            {
                _credential = value;
                if (_credential != null) _listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
            }
        }

        private bool isHttpListenerStopping = false;

        /// <summary>
        /// How the listener will respond to the given context
        /// </summary>
        /// <param name="context"></param>
        protected virtual void ListenerResponse(HttpListenerContext context)
        {
            throw new NotImplementedException("Not implemented in HttpListenerBase");
        }

        #endregion Members

        #region Public methods

        public string SetEndpointAddress(string address)
        {
            EndpointAddress = address;

            return _endpointAddress;
        }

        /// <summary>
        /// Configures the listener to require the given credentials 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        public void UseCredentials(string user, string password, string domain)
        {
            Credential = new NetworkCredential(user, password, domain);
        }

        /// <summary>
        /// Waits for the listener to receive the given number of cumulative requests 
        /// </summary>
        /// <param name="numberOfRequests"></param>
        /// <param name="timeout"></param>
        public void WaitForRequests(int numberOfRequests, TimeSpan timeout)
        {
            if (!Wait.ForTrue(() => RequestsReceived.Count >= numberOfRequests, timeout))
            {
                throw new ActualVsExpectedException($"Expected number of uploads not received before timeout. Expected: {numberOfRequests}; Actual: {RequestsReceived.Count}; {nameof(timeout)}: {timeout}");
            }
        }

        #endregion Public methods

        #region Private helpers

        /// <summary>
        /// Sends the content as a response
        /// </summary>
        /// <param name="context">The current http context</param>
        /// <param name="contenttype">The content type of the response</param>
        /// <param name="content">The content to be returned</param>
        protected void SendContent(HttpListenerContext context, string contenttype, byte[] content)
        {
            context.Response.ContentType = contenttype;
            context.Response.ContentLength64 = content.LongLength;
            int bufferSize = 2048;
            using (BinaryWriter bw = new BinaryWriter(context.Response.OutputStream))
            {
                for (int p = 0; p < content.LongLength; p += bufferSize)
                {
                    int size = (int)content.LongLength - p;
                    if (size > bufferSize)
                    {
                        size = bufferSize;
                    }
                    bw.Write(content, p, size);
                    bw.Flush();
                }
            }
            context.Response.OutputStream.Close();
        }

        /// <summary>
        /// Adds the received request to the list of received requests
        /// </summary>
        /// <param name="httpListenerContext"></param>
        protected void AddRequest(HttpListenerContext httpListenerContext)
        {
            RequestInstance newRequest = new RequestInstance();

            newRequest.TimeStamp = DateTime.Now;
            newRequest.Headers = GetDictionaryFromCollection(httpListenerContext.Request.Headers);

            newRequest.Method = httpListenerContext.Request.HttpMethod;

            // calling HttpUtility.ParseQueryString due to a known defect in context.Request.QueryString
            newRequest.GetQueryString = GetDictionaryFromCollection(HttpUtility.ParseQueryString(httpListenerContext.Request.Url.Query));

            newRequest.PostQueryString = new Dictionary<string, string>();
            newRequest.Files = new List<UploadedFile>();

            newRequest = ProcessPost(newRequest, httpListenerContext);

            newRequest.Uri = httpListenerContext.Request.Url.AbsoluteUri.Substring(EndpointAddress.Length);
            if (newRequest.Uri.Contains("?"))
            {
                newRequest.Uri = newRequest.Uri.Substring(0, newRequest.Uri.IndexOf('?'));
            }

            RequestsReceived.Add(newRequest);
        }

        /// <summary>
        /// Converts a NameValueCollection to a Dictionary
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        protected Dictionary<string, string> GetDictionaryFromCollection(NameValueCollection collection)
        {
            Dictionary<string, string> retval = new Dictionary<string, string>();
            foreach (string key in collection.AllKeys)
            {
                if (key != null) retval.Add(key, collection[key]);
            }
            return retval;
        }

        /// <summary>
        /// Is the current request authorized?
        /// </summary>
        /// <param name="context">current http context</param>
        /// <returns>the authorization status</returns>
        protected bool IsAuthorized(HttpListenerContext context)
        {
            bool retval = true;

            if (Credential != null)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (identity.Name != null && identity.Password != null)
                {
                    if (!Credential.UserName.Equals(identity.Name, StringComparison.OrdinalIgnoreCase) || !Credential.Password.Equals(identity.Password, StringComparison.Ordinal))
                    {
                        retval = false;
                    }
                }
                else
                {
                    retval = false;
                }
            }
            return retval;
        }

        /// <summary>
        /// starts the listener thread
        /// </summary>
        protected void startListener()
        {
            lock (_listenerLock)
            {
                _listener.Start();
            }
            Thread.Sleep(new TimeSpan(0, 0, 2));

            while (_listener != null && _listener.IsListening)
            {
                _resetEvent.WaitOne();

                if (isHttpListenerStopping == true)
                {
                    // release signal or a deadlock.
                    _resetEvent.Set();
                    break;
                }

                IAsyncResult result = _listener.BeginGetContext(ProcessRequest, _listener);
            }
            if (isHttpListenerStopping)
            {
                isHttpListenerStopping = false;
            }
        }

        /// <summary>
        /// Create and start the listener
        /// </summary>
        protected void DoStart()
        {
            if (_listener == null)
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add(EndpointAddress);
                if (Credential != null) _listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
            }
            if (!_listener.IsListening)
            {
                _listenerTask = Task.Factory.StartNew(() => startListener(), TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
            }
        }

        private void ProcessRequest(IAsyncResult result)
        {
            HttpListener mListener = (HttpListener)result.AsyncState;
            try
            {
                if (mListener.IsListening)
                {
                    HttpListenerContext context = mListener.EndGetContext(result);
                    _resetEvent.Set();
                    ListenerResponse(context);
                }
            }
            catch (HttpListenerException ex)
            {
                UpdateStatus?.Invoke(this, new StatusChangedEventArgs("ProcessRequest caught an exception: " + ex));
            }
        }

        /// <summary>
        /// Converts post contents of received HttpListenerRequest to RequestInstance
        /// (Includes PostQueryString and uploaded files)
        /// </summary>
        /// <returns>RequestInstance containing post contents of HttpListenerRequest</returns>
        /// <param name="newRequest">The RequestInstance to put post contents into</param>
        /// <param name="context">the HttpListenerContext on listener</param>
        protected virtual RequestInstance ProcessPost(RequestInstance newRequest, HttpListenerContext context)
        {
            throw new NotImplementedException($"Not implemented in {GetType().Name}.");
        }

        #endregion Private helpers
    }
}