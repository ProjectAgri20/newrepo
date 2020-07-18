using System;
using System.Collections.Generic;
using System.Linq;

using NotifCodeACM = HP.ScalableTest.PluginSupport.Connectivity.Wireless.WlanNotificationCodeAcm;
using NotifCodeMSM = HP.ScalableTest.PluginSupport.Connectivity.Wireless.WlanNotificationCodeMsm;

namespace HP.ScalableTest.PluginSupport.Connectivity.Wireless
{
    /// <summary>
    /// Wrapper class on WlanClient to provide easy access of wireless network information and perform operations
    /// </summary>
	public class Wifi
    {
        #region Public Events

        /// <summary>
        /// Raises event whenever wireless network connection status changes
        /// </summary>
        public event EventHandler<WifiStatusEventArgs> ConnectionStatusChanged;

        #endregion

        #region Local Variables

        private WlanClient _client;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to create a Wifi object
        /// </summary>
        public Wifi()
        {
            _client = new WlanClient();
            IsAvailable = !_client.NoWifiAvailable;

            if (!IsAvailable)
                return;

            if (_client.Interfaces[0].InterfaceState == WlanInterfaceState.Connected)
            {
                IsConnected = true;
            }

            foreach (var inte in _client.Interfaces)
                inte.WlanNotification += inte_WlanNotification;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Wifi network interface availability
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Gets Wifi network interface connection status
        /// </summary>
        public bool IsConnected { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list over all available Wireless Network connections
        /// </summary>
        public List<WirelessNetwork> GetWirelessNetworkConnections()
        {
            List<WirelessNetwork> wirelessNetworks = new List<WirelessNetwork>();
            if (_client.NoWifiAvailable)
                return wirelessNetworks;

            foreach (WlanInterface wlanIface in _client.Interfaces)
            {
                WlanAvailableNetwork[] rawNetworks = wlanIface.GetAvailableNetworkList(0);
                List<WlanAvailableNetwork> networks = new List<WlanAvailableNetwork>();

                // Remove network entries without profile name if one exist with a profile name.
                foreach (WlanAvailableNetwork network in rawNetworks)
                {
                    bool hasProfileName = !string.IsNullOrEmpty(network.profileName);
                    bool anotherInstanceWithProfileExists = rawNetworks.Where(n => n.Equals(network) && !string.IsNullOrEmpty(n.profileName)).Any();

                    if (!anotherInstanceWithProfileExists || hasProfileName)
                        networks.Add(network);
                }

                foreach (WlanAvailableNetwork network in networks)
                {
                    wirelessNetworks.Add(new WirelessNetwork(wlanIface, network));
                }
            }

            return wirelessNetworks;
        }

        /// <summary>
        /// Connects to the given wireless network
        /// </summary>
        /// <param name="ssid">SSID to connect</param>
        /// <param name="password">wireless network connection password</param>
        public bool Connect(string ssid, string password = "")
        {
            List<WirelessNetwork> wirelessNetworks = GetWirelessNetworkConnections();

            foreach (WirelessNetwork wirelessNetwork in wirelessNetworks)
            {
                if (wirelessNetwork.Name == ssid)
                {
                    AuthSettings authSettings = new AuthSettings(wirelessNetwork);
                    authSettings.Password = password;

                    return wirelessNetwork.Connect(authSettings, true);
                }
            }

            return false;
        }

        /// <summary>
        /// Disconnect all wifi interfaces
        /// </summary>
        public void Disconnect()
        {
            if (_client.NoWifiAvailable)
                return;

            foreach (WlanInterface wlanIface in _client.Interfaces)
            {
                wlanIface.Disconnect();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Receives the event from the Wlan interface when ever there is a change in the connection status
        /// </summary>
        /// <param name="notifyData">Wireless status notification</param>
        private void inte_WlanNotification(WlanNotificationData notifyData)
        {
            if (notifyData.notificationSource == WlanNotificationSource.ACM && (NotifCodeACM)notifyData.NotificationCode == NotifCodeACM.Disconnected)
            {
                OnConnectionStatusChanged(WifiStatus.Disconnected);
            }
            else if (notifyData.notificationSource == WlanNotificationSource.MSM && (NotifCodeMSM)notifyData.NotificationCode == NotifCodeMSM.Connected)
            {
                OnConnectionStatusChanged(WifiStatus.Connected);
            }
        }

        /// <summary>
        /// Raises the event to all subscribers
        /// </summary>
        /// <param name="newStatus">Wireless connection status</param>
		private void OnConnectionStatusChanged(WifiStatus newStatus)
        {
            if (ConnectionStatusChanged != null)
            {
                ConnectionStatusChanged(this, new WifiStatusEventArgs(newStatus));
            }
        }

        #endregion
    }

    /// <summary>
    /// Raises the event when ever Wifi status changes
    /// </summary>
	public class WifiStatusEventArgs : EventArgs
    {
        public WifiStatus NewStatus { get; private set; }

        internal WifiStatusEventArgs(WifiStatus status) : base()
        {
            this.NewStatus = status;
        }

    }


}
