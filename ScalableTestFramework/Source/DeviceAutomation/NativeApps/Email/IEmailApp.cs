using HP.ScalableTest.DeviceAutomation.Authentication;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Email
{
    /// <summary>
    /// Interface for an embedded Email application.
    /// </summary>
    public interface IEmailApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Launches the Email application on the device.     
        /// </summary>
        void Launch();

        /// <summary>
        /// Launches the Email application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Launches the Email application using the specified authenticator, authentication mode, and quickset.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quick set.</param>
        void LaunchFromQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName);

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="quickSetName">Name of the Quickset if exists or else empty/null</param>
        /// </summary>  
        void SelectQuickset(string quickSetName);

        /// <summary>
        /// Enters the "To" email address.
        /// </summary>
        /// <param name="emailAddresses">The "To" email addresses.</param>
        void EnterToAddress(IEnumerable<string> emailAddresses);

        /// <summary>
        /// Enters the email subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        void EnterSubject(string subject);

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        void EnterFileName(string fileName);

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        bool ExecuteJob(ScanExecutionOptions executionOptions);

        /// <summary>
        /// Gets the <see cref="IEmailJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        IEmailJobOptions Options { get; }
    }
}