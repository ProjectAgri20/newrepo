using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Framework.PluginService;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Manages the startup and communication channel into a <see cref="SessionProxy"/> process.
    /// </summary>
    [DataContract]
    public class SessionProxyController : IDisposable
    {
        private readonly SessionProxy _sessionProxy = null;
        private AutoResetEvent _waitHandle = null;
        private readonly Uri _endpoint = null;

        [DataMember]
        private bool _proxyStarted = false;

        [DataMember]
        private bool _sessionClosing = false;

        [DataMember]
        private readonly string _sessionId = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionProxyController"/> class.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        public SessionProxyController(string sessionId)
        {
            _sessionId = sessionId;

            string extension = "{0}-{1}".FormatWith(sessionId, Guid.NewGuid().ToShortString());
            _endpoint = WcfService.SessionProxy.GetLocalHttpUri(extension);

            // Instatiate the proxy and start the backend service for STF Lite
            if (GlobalSettings.IsDistributedSystem == false)
            {
                FrameworkServicesInitializer.InitializeExecution();
                _sessionProxy = new SessionProxy(sessionId);
                _sessionProxy.StartBackendService();
                
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the session proxy process has been started.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the proxy process is started; otherwise, <c>false</c>.
        /// </value>
        public bool ProxyProcessStarted
        {
            get { return _proxyStarted; }

            set
            {
                _proxyStarted = value;

                // This property is used to unblock the proxy process startup if the 
                // proxy isn't already started.
                if (_proxyStarted && _waitHandle != null)
                {
                    TraceFactory.Logger.Debug("Wait handle set for {0}".FormatWith(_sessionId));
                    _waitHandle.Set();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the shutdown process has begun.
        /// The intent being to not allow the shutdown process to be called more than once.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the proxy process is shutting down; otherwise, <c>false</c>.
        /// </value>
        public bool SessionClosing
        {
            get { return _sessionClosing; }
            set { _sessionClosing = value; }
        }

        /// <summary>
        /// Get the Endpoint used for this controller
        /// </summary>
        public Uri Endpoint
        {
            get { return _endpoint; }
        }

        /// <summary>
        /// Gets the channel used to communicate with this proxy process.
        /// </summary>
        public ISessionProxy Channel
        {
            get
            {
                if (GlobalSettings.IsDistributedSystem)
                {
                    return new WcfClient<ISessionProxy>(MessageTransferType.Http, _endpoint).Channel;
                }
                else
                {
                    return _sessionProxy;
                }
            }
        }

        /// <summary>
        /// Gets the session proxy instance.
        /// </summary>
        /// <value>The proxy instance.</value>
        public SessionProxy Proxy
        {
            get { return _sessionProxy; }
        }

        /// <summary>
        /// Gets the session unique identifier.
        /// </summary>
        public string SessionId
        {
            get { return _sessionId; }
        }

        /// <summary>
        /// Starts the Session Proxy process if not already running.
        /// </summary>
        public void StartProxyProcess()
        {
            if (GlobalSettings.IsDistributedSystem)
            {
                try
                {
                    if (ProxyProcessStarted)
                    {
                        TraceFactory.Logger.Debug("Proxy service already started for {0}, returning...".FormatWith(_sessionId));
                        return;
                    }

                    _waitHandle = new AutoResetEvent(false);
                    ThreadPool.QueueUserWorkItem(t => StartProxyProcessHandler());

                    _waitHandle.WaitOne();
                    TraceFactory.Logger.Debug("Signal received for {0}, wait handle released".FormatWith(_sessionId));

                    _waitHandle.Dispose();
                    _waitHandle = null;
                    TraceFactory.Logger.Debug("Wait handle set to null for {0}".FormatWith(_sessionId));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error(ex);
                    throw;
                }
            }
            else
            {
                // Since the proxy is not started as a standalone process, then simply
                // set the flag to true to indicate that it has been started.
                ProxyProcessStarted = true;
            }
        }

        private void StartProxyProcessHandler()
        {
            var dir = Path.GetDirectoryName(Directory.GetCurrentDirectory());            
            var file = Path.Combine(dir, "SessionProxy", "sessionproxy.exe");
            var args = "{0} {1} {2}".FormatWith(GlobalSettings.Database, _sessionId, _endpoint.AbsoluteUri);

            TraceFactory.Logger.Debug("{0} {1}".FormatWith(file, args));

            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = args;
            info.FileName = file;
            info.WorkingDirectory = Path.GetDirectoryName(file);

            Process.Start(info);

            TraceFactory.Logger.Debug("Proxy process started for {0}".FormatWith(_sessionId));
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_waitHandle != null)
                {
                    _waitHandle.Dispose();
                }
               
                if (_sessionProxy != null)
                {
                    _sessionProxy.Dispose();
                } 
            }
        }

        #endregion
    }
}
