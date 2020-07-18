using System;
using System.Linq;
using System.Text;

namespace HP.ScalableTest.PluginSupport.Connectivity.Wireless
{
    public class WirelessNetwork
    {
        #region Local Variables

        /// <summary>
        /// Wifi network interface
        /// </summary>
        private WlanInterface _wlanInterface;

        /// <summary>
        /// Win32 structure for wireless network
        /// </summary>
		private WlanAvailableNetwork _network;

        #endregion

        #region Constructor

        /// <summary>
        /// Internal constructor to create wireless network
        /// </summary>
        /// <param name="wlanInterfac">Wifi network interface</param>
        /// <param name="network">Win32 structure for wireless network</param>
        internal WirelessNetwork(WlanInterface wlanInterfac, WlanAvailableNetwork network)
        {
            _wlanInterface = wlanInterfac;
            _network = network;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the wireless network
        /// </summary>
        public string Name
        {
            get
            {
                return Encoding.ASCII.GetString(_network.dot11Ssid.SSID, 0, (int)_network.dot11Ssid.SSIDLength);
            }
        }

        /// <summary>
        /// Gets the signal strength of the wireless network
        /// </summary>
		public uint SignalStrength
        {
            get
            {
                return _network.wlanSignalQuality;
            }
        }

        /// <summary>
        /// If the computer has a connection profile stored for this wireless network
        /// </summary>
        public bool HasProfile
        {
            get
            {
                try
                {
                    return _wlanInterface.GetProfiles().Where(p => p.profileName == Name).Any();
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the network security status
        /// </summary>
        public bool IsSecure
        {
            get
            {
                return _network.securityEnabled;
            }
        }

        /// <summary>
        /// Gets the connection status
        /// </summary>
		public bool IsConnected
        {
            get
            {
                try
                {
                    var a = _wlanInterface.CurrentConnection; // This prop throws exception if not connected, which forces me to this try catch. Refactor plix.
                    return a.profileName == _network.profileName;
                }
                catch
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// Get the underlying network object.
        /// </summary>
        internal WlanAvailableNetwork Network
        {
            get
            {
                return _network;
            }
        }


        /// <summary>
        /// Gets the underlying interface object.
        /// </summary>
        internal WlanInterface Interface
        {
            get
            {
                return _wlanInterface;
            }
        }

        /// <summary>
        /// Checks that the password format matches this wireless network's encryption method.
        /// </summary>
        public bool IsValidPassword(string password)
        {
            return PasswordHelper.IsValid(password, _network.dot11DefaultCipherAlgorithm);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Connect synchronous to the wireless network.
        /// </summary>
        public bool Connect(AuthSettings request, bool overwriteProfile = true)
        {
            // No point to continue with the connect if the password is not valid if overwrite is true or profile is missing.
            if (!request.IsPasswordValid && (!HasProfile || overwriteProfile))
                return false;

            // If we should create or overwrite the profile, do so.
            if (!HasProfile || overwriteProfile)
            {
                if (HasProfile)
                    _wlanInterface.DeleteProfile(Name);

                request.Process();
            }

            return _wlanInterface.ConnectSynchronously(WlanConnectionMode.Profile, _network.dot11BssType, Name, 20000);
        }

        /// <summary>
        /// Connect asynchronous to the wireless network.
        /// </summary>
        public void ConnectAsync(AuthSettings request, bool overwriteProfile = false, Action<bool> onConnectComplete = null)
        {
            //// TODO: Refactor -> Use async connect in wlaninterface.
            //ThreadPool.QueueUserWorkItem(new WaitCallback((o) => {
            //	bool success = false;

            //	try
            //	{
            //		success = Connect(request, overwriteProfile);
            //	}
            //	catch (Win32Exception)
            //	{					
            //		success = false;
            //	}

            //	if (onConnectComplete != null)
            //		onConnectComplete(success);
            //}));

            // If we should create or overwrite the profile, do so.
            if (!HasProfile || overwriteProfile)
            {
                if (HasProfile)
                    _wlanInterface.DeleteProfile(Name);

                request.Process();
            }
            _wlanInterface.WlanConnectionNotification += _wlanInterface_WlanConnectionNotification;
            _wlanInterface.Connect(WlanConnectionMode.Profile, _network.dot11BssType, Name);

        }

        private void _wlanInterface_WlanConnectionNotification(WlanNotificationData notifyData, WlanConnectionNotificationData connNotifyData)
        {
            System.Diagnostics.Debug.WriteLine(notifyData.NotificationCode);
        }

        /// <summary>
        /// Returns the profile in XML format if exists else returs empty string
        /// </summary>
        /// <returns>Profile in XML format</returns>
        public string GetProfileXML()
        {
            if (HasProfile)
            {
                return _wlanInterface.GetProfileXml(Name);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Deletes the stored profile in the current system
        /// </summary>
		public void DeleteProfile()
        {
            try
            {
                if (HasProfile)
                {
                    _wlanInterface.DeleteProfile(Name);
                }
            }
            catch { }
        }

        /// <summary>
        /// Returns the wireless network details in string format
        /// </summary>
        /// <returns>Wireless network details in string format</returns>
		public override sealed string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Interface: " + _wlanInterface.InterfaceName);
            info.AppendLine("Auth algorithm: " + _network.dot11DefaultAuthAlgorithm);
            info.AppendLine("Cipher algorithm: " + _network.dot11DefaultCipherAlgorithm);
            info.AppendLine("BSS type: " + _network.dot11BssType);
            info.AppendLine("Connectable: " + _network.networkConnectable);

            if (!_network.networkConnectable)
            {
                info.AppendLine("Reason to false: " + _network.wlanNotConnectableReason);
            }

            return info.ToString();
        }

        #endregion
    }
}