using System;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class EndpointManager
    {
        public event EventHandler<StatusChangedEventArgs> UpdateStatus;
        private PluginEnvironment _environment;
        public const int HttpPort = 48650;
        public const int HttpsPort = 48660;

        public EndpointManager(PathManager pathManager, PluginEnvironment environment)
        {
            PathManager = pathManager;
            _environment = environment;
        }

        public PathManager PathManager { get; }

        protected FtpServer _ftpServer;
        public FtpServer FtpServer
        {
            get
            {
                if (_ftpServer == null)
                {
                    string localIP = NetUtil.GetAddressForLocalHostIpv4().ToString();
                    _ftpServer = new FtpServer(PathManager.FtpPath.EndpointServerIP, 21);
                    _ftpServer.UpdateStatus += OnUpdateStatus;
                    _ftpServer.UserStore.Add(_environment.PluginSettings["DomainAdminUserName"], _environment.PluginSettings["DomainAdminPassword"], PathManager.FtpPath.CorrespondingFileSystemPath);
                    _ftpServer.Start();
                }
                return _ftpServer;
            }
        }

        protected HttpListenerScanReceiver _httpScanReceiver;
        public HttpListenerScanReceiver HttpScanReceiver
        {
            get
            {
                if (_httpScanReceiver == null)
                {
                    _httpScanReceiver = new HttpListenerScanReceiver(PathManager.HttpPath.GetPathAsUrl());
                    _httpScanReceiver.UpdateStatus += OnUpdateStatus;
                }
                return _httpScanReceiver;
            }
        }

        private void OnUpdateStatus(object sender, Utility.StatusChangedEventArgs e)
        {
            UpdateStatus?.Invoke(sender, e);
        }

        protected HttpListenerServerSim _httpListenerServerSim;
        public HttpListenerServerSim HttpServerSim
        {
            get
            {
                if (_httpListenerServerSim == null)
                {
                    _httpListenerServerSim = new HttpListenerServerSim(PathManager.HttpPath.EndpointServerIP);
                    _httpListenerServerSim.UpdateStatus += OnUpdateStatus;
                }
                return _httpListenerServerSim;
            }
        }

        protected NetworkFolderHandler _networkFolder;
        public NetworkFolderHandler NetworkFolder
        {
            get
            {
                if (_networkFolder == null)
                {
                    _networkFolder = new NetworkFolderHandler();
                    _networkFolder.EndpointAddress = PathManager.FileSharePath.GetPathAsUrl();
                }

                return _networkFolder;
            }
        }
    }
}