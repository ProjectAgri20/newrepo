using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpec
{
    /// <summary>
    /// Interface for generating the HPEC solution
    /// </summary>
    public interface IHpecApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches HPEC with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="documentName">The document name.</param>
        bool ExecuteJob(string workflow, string documentName);

        /// <summary>
        /// Hpec's the start scan.
        /// </summary>
        void HpecStartScan(int pageCount);

        /// <summary>
        /// Processes the job after scan.
        /// </summary>
        bool ProcessJobAfterScan();

        /// <summary>
        /// Jobs the finished.
        /// </summary>
        void JobFinished();

        /// <summary>
        /// Gets or sets the reported hpec page count.
        /// </summary>
        /// <value>
        /// The reported hpec page count.
        /// </value>
        int ReportedHpecPageCount { get; set; }
    }
}
