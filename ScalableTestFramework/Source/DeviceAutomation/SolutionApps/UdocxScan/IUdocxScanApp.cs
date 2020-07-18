using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.UdocxScan
{
    /// <summary>
    /// Base methods for all UdocxScanApp classes
    /// </summary>
    public interface IUdocxScanApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches The UdocxScan solution with the given authenticator with either eager or lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        bool SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Start Scan Job and finish
        /// </summary>
        void Scan(string emailAddress);

        /// <summary>
        /// Start Scan Job and finish
        /// </summary>
        void Scan();

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        void SetCopyCount(int numCopies);

        /// <summary>
        /// Launches Udocx Scan App in Udocx
        /// </summary>
        /// <param name="destination">The authenticator.</param>
        void SelectApp(string destination);

    }
}
