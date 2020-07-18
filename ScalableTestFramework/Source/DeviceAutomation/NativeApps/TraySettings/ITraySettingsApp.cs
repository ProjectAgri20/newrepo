using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.TraySettings
{
    /// <summary>
    /// Interface for the tray settings application.
    /// </summary>
    public interface ITraySettingsApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Sets the Tray setting for Managed Trays
        /// <param name="traySettings">Container to hold values of the Tray setting</param>
        /// </summary>
        void ManageTraySettings(TraySettings traySettings);

        /// <summary>
        /// Opens Contacts screen with the specified authenticator using the given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);
    }
}
