using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Authentication
{
    public interface IAuthenticationDriver : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches the specified authenticator utilizing the given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Launches the specified authenticator utilizing the given authentication initialization method.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationInitMethod">The authentication initialize method.</param>
        void Launch(IAuthenticator authenticator, AuthenticationInitMethod authenticationInitMethod);

        /// <summary>
        /// Authenticates using the given authenticator and waits for the given form name.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The wait form.</param>
        void Authenticate(IAuthenticator authenticator, string waitForm);
        /// <summary>
        /// The sign-out operation desired.
        /// </summary>
        /// <param name="signOut">The sign out.</param>
        void SignOut(DeviceSignOutMethod signOut, IDevice device);
    }
}
