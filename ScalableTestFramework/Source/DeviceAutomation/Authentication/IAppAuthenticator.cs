using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.Authentication
{
    /// <summary>
    /// Interface for performing authentication tasks on a device for a specific solution.
    /// </summary>
    public interface IAppAuthenticator
    {
        /// <summary>
        /// Gets the pacekeeper for this solution authenticator.
        /// </summary>
        /// <value>The pacekeeper.</value>
        Pacekeeper Pacekeeper { get; }

        /// <summary>
        /// Gets the authentication credential.
        /// </summary>
        /// <value>The credential.</value>
        AuthenticationCredential Credential { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        string ErrorMessage { get; }

        /// <summary>
        /// Enters credentials on the device control panel.
        /// </summary>
        void EnterCredentials();

        /// <summary>
        /// Applies additional authentication parameters before submission of authentication request.
        /// </summary>
        /// <param name="parameters"></param>
        void ApplyParameters(Dictionary<string, object> parameters);

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        void SubmitAuthentication();

        /// <summary>
        /// Checks the device control panel for successful authentication.
        /// </summary>
        /// <returns><c>true</c> if the authentication operation is valid, <c>false</c> otherwise.</returns>
        bool ValidateAuthentication();
    }
}
