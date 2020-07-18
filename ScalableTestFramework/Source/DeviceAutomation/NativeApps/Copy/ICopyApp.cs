using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Copy
{
    /// <summary>
    /// Interface for an embedded copy application.
    /// </summary>
    public interface ICopyApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Launches the Copy application on the device.        
        /// </summary>
        void Launch();

        /// <summary>
        /// Launches the Copy application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Launches the Copy application using the specified authenticator, authentication mode, and quickset.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quick set.</param>
        void LaunchFromQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName);

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="quickSetName">Name of the Quickset</param>
        /// </summary> 
        void SelectQuickSet(string quickSetName);

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>        
        bool ExecuteJob(ScanExecutionOptions executionOptions);

        /// <summary>
        /// Gets the <see cref="ICopyJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        ICopyJobOptions Options { get; }
    }
}
