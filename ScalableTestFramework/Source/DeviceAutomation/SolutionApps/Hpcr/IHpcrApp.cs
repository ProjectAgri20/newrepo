using System;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr
{
    /// <summary>
    /// Interface for an HPCR Scan To application.
    /// </summary>
    public interface IHpcrApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Occurs when there is a change in the executing job status.
        /// </summary>
        event EventHandler<StatusChangedEventArgs> StatusMessageUpdate;

        /// <summary>
        /// Gets or sets the HPCR final status.
        /// </summary>
        /// <value>
        /// The HPCR final status.
        /// </value>
        string HpcrFinalStatus { get; }

        /// <summary>
        /// Launches The Hpcr solution with the given authenticator with either eager or lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        bool ExecuteJob(ScanExecutionOptions executionOptions);

        /// <summary>
        /// Jobs the finished.
        /// </summary>
        void JobFinished();
    }
}
