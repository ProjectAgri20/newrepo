using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage
{
    /// <summary>
    /// Interface for an embedded Save To Job Storage application.
    /// </summary>
    public interface IJobStorageScanApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Launches the Scan application on the device.
        /// </summary>
        void Launch();

        /// <summary>
        /// Launches the Scan application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Adds the specified name for the job
        /// </summary>
        /// <param name="jobname">The job name.</param>
        /// <param name="pin">Pin for the file.</param> 
        void AddJobName(string jobname, string pin);

        /// <summary>
        /// Adds the specified name for the job
        /// </summary>
        /// <param name="jobname">The job name.</param>  
        void AddJobName(string jobname);

        /// <summary>
        /// Starts the Scan job and runs it to completion" />.
        /// </summary>
        bool ExecuteScanJob(ScanExecutionOptions executionOptions);

        /// <summary>
        /// Gets the <see cref="IJobStorageJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        IJobStorageJobOptions Options { get; }
    }
}
