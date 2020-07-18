using System;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.AutoStore
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.IDeviceWorkflowLogSource" />
    public interface IAutoStoreApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Occurs when there is a change in the executing job status.
        /// </summary>
        event EventHandler<StatusChangedEventArgs> StatusMessageUpdate;

        /// <summary>
        /// Launches The AutoStore solution with the given authenticator with either eager or lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        /// <returns></returns>
        bool ExecuteJob(AutoStoreExecutionOptions executionOptions);

        /// <summary>
        /// Jobs the finished.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        void JobFinished(AutoStoreExecutionOptions executionOptions);
    }
}
