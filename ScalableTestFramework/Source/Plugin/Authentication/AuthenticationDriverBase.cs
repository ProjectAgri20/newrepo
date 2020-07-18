using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.NativeApps.Copy;
using HP.ScalableTest.DeviceAutomation.NativeApps.Email;
using HP.ScalableTest.DeviceAutomation.NativeApps.Fax;
using HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder;
using HP.ScalableTest.DeviceAutomation.SolutionApps.Dss;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Authentication
{
    internal abstract class AuthenticationDriverBase : DeviceWorkflowLogSource, IAuthenticationDriver, IDisposable
    {
        protected int DEFAULT_UNAUTH_WAIT_SECONDS = 15;
        protected int DEFAULT_UNAUTH_TIMEOUT_MINUTES = 10;
        protected bool _disposed = false;

        /// <summary>
        /// Occurs when [status message update].
        /// </summary>
        //public event EventHandler<StatusChangedEventArgs> StatusMessageUpdate;
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Launches the specified authenticator with the specified authentication initialization method.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="IAuthenticator">The i authenticator.</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationInitMethod IAuthenticator);

        /// <summary>
        /// Authenticates using the given authenticator and waits for the given form name.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The wait form.</param>
        public abstract void Authenticate(IAuthenticator authenticator, string waitForm);

        protected DeviceWorkflowLogger _workflowLogger;



        /// <summary>
        /// The sign-out operation desired.
        /// </summary>
        /// <param name="signOut">The sign out.</param>
        public void SignOut(DeviceSignOutMethod signOut, IDevice device)
        {
            IDevicePreparationManager dpm = DevicePreparationManagerFactory.Create(device);
            dpm.WorkflowLogger = _workflowLogger;
            dpm.SignOut(signOut);
        }
        /// <summary>
        /// Faxes via the lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="device">The device.</param>
        protected void FaxLazyAuth(IAuthenticator authenticator, IDevice device)
        {
            IFaxApp faxApp = FaxAppFactory.Create(device);
            faxApp.Launch(authenticator, AuthenticationMode.Lazy);
        }
        /// <summary>
        /// Emails via the lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="device">The device.</param>
        protected void EmailLazyAuth(IAuthenticator authenticator, IDevice device)
        {
            IEmailApp emailApp = EmailAppFactory.Create(device);
            emailApp.Launch(authenticator, AuthenticationMode.Lazy);
        }
        /// <summary>
        /// Copies via the lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="device">The device.</param>
        protected void CopyLazyAuth(IAuthenticator authenticator, IDevice device)
        {
            ICopyApp copyApp = CopyAppFactory.Create(device);
            copyApp.Launch(authenticator, AuthenticationMode.Lazy);
        }
        /// <summary>
        /// Networks the folder lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="device">The device.</param>
        protected void NetworkFolderLazyAuth(IAuthenticator authenticator, IDevice device)
        {
            INetworkFolderApp networkFolderApp = NetworkFolderAppFactory.Create(device);
            networkFolderApp.Launch(authenticator, AuthenticationMode.Lazy);
        }
        /// <summary>
        /// DSSs the workflow lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="device">The device.</param>
        protected void DssWorkflowLazyAuth(IAuthenticator authenticator, IDevice device)
        {
            IDssWorkflowApp dssWorkflowApp = DssWorkflowAppFactory.Create(device);
            dssWorkflowApp.Launch(authenticator, AuthenticationMode.Lazy);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
        }

    }
}
