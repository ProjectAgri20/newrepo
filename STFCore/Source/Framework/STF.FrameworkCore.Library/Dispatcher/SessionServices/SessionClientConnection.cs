using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;
using System;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Connection class used to communicate with the Session Client service.  The connection
    /// can be network based or class based depending on the proximity of the service.
    /// </summary>
    public class SessionClientConnection : IDisposable
    {
        private readonly WcfClient<ISessionClient> _networkConnection = null;

        SessionClientConnection()
        {
        }

        private SessionClientConnection(Uri endpoint)
        {
            _networkConnection = new WcfClient<ISessionClient>(MessageTransferType.CompressedHttp, endpoint);
        }

        public ISessionClient Channel
        {
            get
            {
                return (GlobalSettings.IsDistributedSystem)
                    ? _networkConnection.Channel
                    : SessionClient.Instance;
            }
        }

        public static SessionClientConnection Create(Uri endpoint)
        {
            return new SessionClientConnection(endpoint);
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