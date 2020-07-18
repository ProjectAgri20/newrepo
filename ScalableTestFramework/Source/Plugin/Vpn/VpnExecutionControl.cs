using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DotRas;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.Vpn
{
    [ToolboxItem(false)]
    public partial class VpnExecutionControl : UserControl, IPluginExecutionEngine
    {
        //private VPNActivityData data = null;

        private string _entryName;
        private VpnConfiguration _vpnConfiguration;

        public VpnExecutionControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            var activityData = executionData.GetMetadata<VpnActivityData>();

            string vpnValue = executionData.Environment.PluginSettings[activityData.Name];
            var vpnValues = vpnValue.Split(';');
            if (vpnValues.Length != 3)
            {
                return new PluginExecutionResult(PluginResult.Error, "The plugin configuration is not defined correctly. Please contact your administrator");
            }
            _vpnConfiguration = new VpnConfiguration
            {
                Name = activityData.Name,
                ServerIp = vpnValues.ElementAt(0),
                Password = vpnValues.ElementAt(2)
            };
            if (vpnValues.ElementAt(1).Contains("\\"))
            {
                _vpnConfiguration.Domain = vpnValues.ElementAt(1).Split('\\').First();
                _vpnConfiguration.UserName = vpnValues.ElementAt(1).Split('\\').Last();
            }
            else
            {
                _vpnConfiguration.Domain = string.Empty;
                _vpnConfiguration.UserName = vpnValues.ElementAt(1);
            }
            if (_vpnConfiguration.Domain.Equals("."))
            {
                _vpnConfiguration.Domain = string.Empty;
            }
            _entryName = activityData.Name;
            try
            {
                var vpnLock = new LocalLockToken("VPN", new LockTimeoutData(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30)));
                if (activityData.Connect)
                {
                    ExecutionServices.CriticalSection.Run(vpnLock, ConnectVpn);
                }
                else
                {
                    ExecutionServices.CriticalSection.Run(vpnLock, DisconnectVpn);
                }
            }
            catch (Exception exception)
            {
                return new PluginExecutionResult(PluginResult.Failed, exception.Message);
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        private void ConnectVpn()
        {
            // Get the rest of the configuration data from the Manifest

            //first create the VPN
            RasPhoneBook rasPhoneBook = new RasPhoneBook();
            rasPhoneBook.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User));
            if (!rasPhoneBook.Entries.Contains(_vpnConfiguration.Name))
            {
                ExecutionServices.SystemTrace.LogInfo($"Creating VPN {_vpnConfiguration.Name} {_vpnConfiguration.ServerIp}");
                var currentVpn = CreateVpn(_vpnConfiguration.Name, _vpnConfiguration.ServerIp);
                rasPhoneBook.Entries.Add(currentVpn);
            }
            rasPhoneBook.Dispose();

            RasDialer rasDialer = new RasDialer
            {
                EntryName = _vpnConfiguration.Name,
                PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User),
                Credentials = new System.Net.NetworkCredential()
                {
                    UserName = _vpnConfiguration.UserName,
                    Password = _vpnConfiguration.Password,
                    Domain = _vpnConfiguration.Domain
                }
            };

            try
            {
                if (rasDialer.IsBusy)
                {
                    rasDialer.DialAsyncCancel();
                }
                rasDialer.Dial();
            }
            catch (InvalidOperationException ioExcep)
            {
                ExecutionServices.SystemTrace.LogDebug(ioExcep.Message);
                log_textBox.Text += @"Unable to connect to VPN" + Environment.NewLine;
                throw;
            }
            catch (RasException rasException)
            {
                ExecutionServices.SystemTrace.LogDebug(rasException.Message);
                log_textBox.Text += @"Unable to connect to VPN" + Environment.NewLine;
                throw;
            }

            vpnname_textBox.Text = _vpnConfiguration.Name;
            status_textBox.Text = @"Connected";
            log_textBox.Text += @"Connected to VPN" + Environment.NewLine;
        }

        private void DisconnectVpn()
        {
            var activeConnections = RasConnection.GetActiveConnections();

            if (activeConnections.Count == 0)
            {
                throw new Exception("A VPN connection is not active");
            }

            RasConnection currentConnection = activeConnections.First(x => x.EntryName == _entryName);

            if (currentConnection != null)
            {
                currentConnection.HangUp();
                status_textBox.Text = @"Disconnected";
                log_textBox.Text += @"Disconnected from VPN" + Environment.NewLine;
            }
        }

        private static RasEntry CreateVpn(string vpnName, string ipAddress)
        {
            RasEntry ras = new RasEntry(vpnName)
            {
                Device = RasDevice.Create("WAN Miniport (PPTP)", RasDeviceType.Vpn),
                DialMode = RasDialMode.None,
                EncryptionType = RasEncryptionType.Optional,
                EntryType = RasEntryType.Vpn,
                FramingProtocol = RasFramingProtocol.Ppp,
                PhoneNumber = ipAddress,
                RedialCount = 2,
                RedialPause = 60,
                VpnStrategy = RasVpnStrategy.PptpOnly,
                Options =
                {
                    DoNotNegotiateMultilink = true,
                    IPv6RemoteDefaultGateway = true,
                    ModemLights = true,
                    ReconnectIfDropped = true,
                    RemoteDefaultGateway = false,
                    RequireDataEncryption = false,
                    RequireEncryptedPassword = true,
                    RequireChap = true,
                    RequireMSChap2 = true,
                    RequireMSEncryptedPassword = true
                }
            };

            RasNetworkProtocols rasNetworkProtocols = new RasNetworkProtocols { IP = true, IPv6 = true, Ipx = false };

            ras.NetworkProtocols = rasNetworkProtocols;

            return ras;
        }
    }


}