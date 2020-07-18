using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class FtpServer : IDisposable
    {
        public const int DefaultTcpIpPort = 21;
        public event EventHandler<StatusChangedEventArgs> UpdateStatus;

        private bool _disposed = false;
        private bool _listening = false;

        private TcpListener _listener;
        private List<ClientConnection> _activeConnections;

        private IPEndPoint _localEndPoint;
        private UserStore _userStore;

        public FtpServer(string ipAddress, int port)
        {
            _localEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        }

        public string EndpointAddress
        {
            get
            {
                if (_localEndPoint.Port == DefaultTcpIpPort)
                {
                    return $"ftp://{_localEndPoint.Address}";
                }
                return $"ftp://{_localEndPoint.Address}:{_localEndPoint.Port}";
            }
        }

        public UserStore UserStore
        {
            get
            {
                if (_userStore == null)
                {
                    _userStore = new UserStore();
                }

                return _userStore;
            }
        }

        public void Start()
        {
            _listener = new TcpListener(_localEndPoint);

            UpdateStatus?.Invoke(this, new StatusChangedEventArgs("#Fields: date time c-ip c-port cs-username cs-method cs-uri-stem sc-status sc-bytes cs-bytes s-name s-port"));

            _listening = true;
            _listener.Start();

            _activeConnections = new List<ClientConnection>();

            _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);
        }

        public void Stop()
        {
            UpdateStatus?.Invoke(this, new StatusChangedEventArgs("Stopping FtpServer"));

            _listening = false;
            _listener.Stop();

            _listener = null;
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            if (_listening)
            {
                _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);

                TcpClient client = _listener.EndAcceptTcpClient(result);

                ClientConnection connection = new ClientConnection(this, client);
                connection.UpdateStatus += Connection_UpdateStatus;

                _activeConnections.Add(connection);

                ThreadPool.QueueUserWorkItem(connection.HandleClient, client);
            }
        }

        private void Connection_UpdateStatus(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus?.Invoke(sender, e);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Stop();

                    foreach (ClientConnection conn in _activeConnections)
                    {
                        conn.Dispose();
                    }
                }
            }

            _disposed = true;
        }
    }
}
