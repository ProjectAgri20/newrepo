namespace HP.ScalableTest.PluginSupport.Connectivity.Wireless
{
    /// <summary>
    /// Contains the Wireless network authentication settings
    /// </summary>
    public class AuthSettings
    {
        #region Local variables

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
        /// Constructor to create authentication settings
        /// </summary>
        /// <param name="wirelessNetwork">Wireless Network object</param>
        public AuthSettings(WirelessNetwork wirelessNetwork)
        {
            _network = wirelessNetwork.Network;
            _wlanInterface = wirelessNetwork.Interface;

            // read the wifi network settings and assign local properties
            IsPasswordRequired = _network.securityEnabled && _network.dot11DefaultCipherAlgorithm != Dot11CipherAlgorithm.None;

            bool isEAPStore = _network.dot11DefaultAuthAlgorithm == Dot11AuthAlgorithm.RSNA || _network.dot11DefaultAuthAlgorithm == Dot11AuthAlgorithm.WPA;

            IsUsernameRequired = isEAPStore;
            IsDomainSupported = isEAPStore;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the password required status
        /// </summary>
        public bool IsPasswordRequired { get; }

        /// <summary>
        /// Gets the username required status
        /// </summary>
        public bool IsUsernameRequired { get; }

        /// <summary>
        /// Gets the domain supported status
        /// </summary>
        public bool IsDomainSupported { get; }



        /// <summary>
        /// Gets or sets the domain
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets the password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets the given password is valid or not
        /// </summary>
        public bool IsPasswordValid
        {
            get
            {
                // #warning Not sure that Enterprise networks have the same requirements on the password complexity as standard ones.
                return PasswordHelper.IsValid(Password, _network.dot11DefaultCipherAlgorithm);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the EAP user XML and saves
        /// </summary>
        /// <returns>Returns true if it is successfully else returns false</returns>
        private bool SaveToEAP()
        {
            if (!IsDomainSupported || !IsPasswordValid)
                return false;

            string userXML = EapUserFactory.Generate(_network.dot11DefaultCipherAlgorithm, Username, Password, Domain);
            _wlanInterface.SetEAP(_network.profileName, userXML);

            return true;
        }

        /// <summary>
        /// Crates the wireless network profile and saves
        /// </summary>
        /// <returns>Returns true if it is successfully else returns false</returns>
		internal bool Process()
        {
            if (!IsPasswordValid)
                return false;

            string profileXML = ProfileFactory.Generate(_network, Password);
            _wlanInterface.SetProfile(WlanProfileFlags.AllUser, profileXML, true);

            if (IsDomainSupported && !SaveToEAP())
                return false;

            return true;
        }

        #endregion
    }
}
