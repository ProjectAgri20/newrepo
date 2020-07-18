using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;
using System;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Connection class used to communicate with the Session Server (Dispatcher).  The connection
    /// can be network based or class based depending on the deployment mode of the framework.
    /// </summary>
    public sealed class SessionDispatcherConnection : IDisposable
    {
        private readonly WcfClient<ISessionDispatcher> _networkConnection = null;
        private static SessionDispatcher _dispatcher = null;

        private SessionDispatcherConnection()
        {
        }

        private SessionDispatcherConnection(string serviceHost)
        {
            var endpoint = WcfService.SessionServer.GetHttpUri(serviceHost);
            _networkConnection = new WcfClient<ISessionDispatcher>(MessageTransferType.CompressedHttp, endpoint);
        }

        public ISessionDispatcher Channel
        {
            get
            {
                if (GlobalSettings.IsDistributedSystem)
                {
                    return _networkConnection.Channel;
                }
                else
                {
                    // Note:  The very first time this is referenced is when the dispatcher object will
                    // be instantiated. This only applies in the "STFLite" mode.   If we are in a distributed
                    // mode, then the dispatcher service will be running and this client will return the 
                    // network connection above.
                    if (_dispatcher == null)
                    {
                        _dispatcher = new SessionDispatcher();
                    }

                    return _dispatcher;
                }
            }
        }

        public static SessionDispatcherConnection Create(string serviceHost)
        {
            return new SessionDispatcherConnection(serviceHost);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_networkConnection != null)
            {
                _networkConnection.Close();
            }
        }

        #endregion IDisposable Members
    }
}