using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.Authentication
{
    /// <summary>
    /// Interface for performing authentication tasks on a device.
    /// </summary>
    public interface IAuthenticator : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Gets or sets the pacekeeper for this authenticator.
        /// </summary>
        /// <value>The pacekeeper.</value>
        Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Gets the authentication credential.
        /// </summary>
        /// <value>The credential.</value>
        AuthenticationCredential Credential { get; }

        /// <summary>
        /// Gets the authentication provider.
        /// </summary>
        /// <value>The provider.</value>
        AuthenticationProvider Provider { get; }

        /// <summary>
        /// Gets or sets the authentication initialization method.
        /// </summary>
        /// <value>
        /// The authentication initialization method.
        /// </value>
        AuthenticationInitMethod InitializationMethod { get; set; }

        /// <summary>
        /// Enters credentials on the device control panel and initiates the authentication process.
        /// </summary>
        void Authenticate();

        /// <summary>
        /// Enters credentials on the device control panel and initiates the authentication process.
        /// Applies the specified parameters before submitting the authentication request.
        /// </summary>
        /// <param name="parameters">Additional configuration parameters presented on the Auth screen.</param>
        void Authenticate(Dictionary<string, object> parameters);

        /// <summary>
        /// Gets or sets a value indicating whether [lazy authentication only].
        /// This requires that the provider by set with the desired solution. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if [lazy authentication only]; otherwise, <c>false</c>.
        /// </value>
        bool LazyAuthOnly { get; set; }
    }
}
