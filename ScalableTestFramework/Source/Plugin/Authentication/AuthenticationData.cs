using System;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Authentication
{
    /// <summary>
    /// Contains data needed to execute a authentication activity.
    /// </summary>
    [DataContract]
    public class AuthenticationData
    {
        /// <summary>
        /// Gets or sets the button on the device used to initiate the authentication activity.
        /// </summary>
        /// <value>The initiation button.</value>
        [DataMember]
        public string InitiationButton { get; set; }

        /// <summary>
        /// Gets or sets the authentication provider.
        /// </summary>
        /// <value>The authentication provider.</value>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets the authentication method.
        /// </summary>
        /// <value>
        /// The authentication method.
        /// </value>
        [DataMember]
        public AuthenticationMethod AuthMethod { get; set; }

        /// <summary>
        /// Gets or sets the un-authenticate method.
        /// </summary>
        /// <value>
        /// The un authenticate method.
        /// </value>
        [DataMember]
        public string UnAuthenticateMethod { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationData"/> class.
        /// </summary>
        public AuthenticationData()
        {
            InitiationButton = AuthenticationInitMethod.SignInButton.GetDescription();

            AuthProvider = AuthenticationProvider.Auto;
            AuthMethod = AuthenticationMethod.Auto;

            UnAuthenticateMethod = "Press Sign Out";
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (LockTimeouts == null)
            {
                LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
            }
        }
    }
}