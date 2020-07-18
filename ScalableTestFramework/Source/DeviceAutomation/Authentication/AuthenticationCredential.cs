using System;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.Authentication
{
    /// <summary>
    /// Holds credential information in a device authentication context.
    /// </summary>
    public class AuthenticationCredential
    {
        private readonly NetworkCredential _credential = null;
        private readonly string _pin = null;
        private readonly BadgeBoxInfo _badgeBoxInfo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationCredential"/> class.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <exception cref="System.ArgumentException">
        /// Credential cannot be null.
        /// and
        /// UserName must have a value.
        /// </exception>
        public AuthenticationCredential(NetworkCredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentException("Credential cannot be null.", nameof(credential));
            }

            if (string.IsNullOrEmpty(credential.UserName))
            {
                throw new ArgumentException("UserName must have a value.", nameof(credential));
            }

            // Intentionally Not Checking for Password value.  Could be running in a Workgroup
            // Environment with no credentials set up on the device.

            _credential = credential;
            _pin = ParsePin(UserName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationCredential"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain.</param>
        /// <exception cref="System.ArgumentException">
        /// User name must have a value.
        /// and
        /// Domain must have a value.
        /// </exception>
        public AuthenticationCredential(string userName, string password, string domain)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("User name must have a value.", nameof(userName));
            }

            // Intentionally Not Checking for Password value.  Could be running in a Workgroup environment
            // with no credentials set up on the device.

            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentException("Domain must have a value.", nameof(domain));
            }

            _credential = new NetworkCredential(userName, password);
            if (!string.IsNullOrEmpty(domain))
            {
                _credential.Domain = domain;
            }
            _pin = ParsePin(userName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationCredential"/> class.
        /// </summary>
        /// <param name="pin">The pin.</param>
        /// <exception cref="System.ArgumentException">PIN must have a value.</exception>
        public AuthenticationCredential(string pin)
        {
            if (string.IsNullOrEmpty(pin))
            {
                throw new ArgumentException("PIN must have a value.", nameof(pin));
            }

            _pin = pin;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationCredential"/> class.
        /// Handles setting of <see cref="BadgeBoxInfo" />.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="deviceAddress">The device address.</param>
        /// <exception cref="System.ArgumentException">No badge box associated with the specified device.</exception>
        public AuthenticationCredential(PluginExecutionData pluginExecutionData, string deviceId, string deviceAddress) : this(pluginExecutionData.Credential)
        {
            _badgeBoxInfo = GetBadgeBoxInfo(pluginExecutionData.Assets, deviceId, deviceAddress);
        }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        /// <value>The user name.</value>
        public string UserName => _credential?.UserName;

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password => _credential?.Password;

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain => _credential?.Domain;

        /// <summary>
        /// Gets the pin.
        /// </summary>
        /// <value>The pin.</value>
        public string Pin => _pin;

        /// <summary>
        /// Gets the badge box configuration information.
        /// </summary>
        /// <value>The badge box information.</value>
        public BadgeBoxInfo BadgeBoxInfo => _badgeBoxInfo;

        /// <summary>
        /// Parses the PIN value from the username.
        /// If the userName length is greater than 2 chars (char or digit),
        /// returns the userName string without the 1st character.
        /// This fits RDL's pattern: example "u05432".  If the userName length is 2 or less,
        /// returns the entire userName passed in.
        /// </summary>
        /// <param name="userName">The user name to parse.</param>
        /// <returns></returns>
        private string ParsePin(string userName)
        {
            /*
            Logically, a PIN is an all-numeric value.  However, some third-party
            solutions allow alpha characters in their PIN values (HPAC).
            So for now, we can't implement the RegEx pattern below.
            First char alpha with rest of string numeric, any length.
            if (Regex.IsMatch(userName, @"^[a-zA-Z][0-9]+$"))
            {
                return userName.Substring(1);
            }
            */

            if (userName.Length > 2)
            {
                return userName.Substring(1);
            }

            return userName;
        }

        private static BadgeBoxInfo GetBadgeBoxInfo(AssetInfoCollection availableAssets, string deviceId, string deviceAddress)
        {
            ExecutionServices.SystemTrace.LogInfo($"Total number of assets: {availableAssets.Count}");
            BadgeBoxInfo badgeBoxAsset = null;
            if (availableAssets.OfType<BadgeBoxInfo>().Count() > 0)
            {
                badgeBoxAsset = availableAssets.OfType<BadgeBoxInfo>().Where(n => n.PrinterId == deviceId).FirstOrDefault();                
            }
            if (badgeBoxAsset == null)
            {
                throw new ArgumentException($"No Badge Box is associated with device {deviceId}, {deviceAddress}.");
            }

            ExecutionServices.SystemTrace.LogInfo($"Printer ID of badge box: {badgeBoxAsset.PrinterId}");
            return badgeBoxAsset;
        }
    }
}
