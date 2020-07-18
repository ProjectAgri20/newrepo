using HP.ScalableTest.DeviceAutomation.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Contacts
{
    /// <summary>
    /// Interface for an embedded contacts application.
    /// </summary>
    public interface IContactsApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Creates a speed dial contact
        /// </summary>
        int CreateSpeedDial(string DisplayName, string SpeedDial, string FaxNumbers);

        /// <summary>
        /// Opens Contacts screen with the specified authenticator using the given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);
    }
}
