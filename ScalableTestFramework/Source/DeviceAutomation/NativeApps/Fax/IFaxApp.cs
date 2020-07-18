using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Fax
{
    /// <summary>
    /// Interface for an embedded Fax application.
    /// </summary>
    public interface IFaxApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Launches the Fax application on the device.
        /// </summary>
        void Launch();

        /// <summary>
        /// Launches the FAX solution with the specified authenticator using the given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Adds the recipient.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        void AddRecipient(string recipient);

        /// <summary>
        /// Adds the specified recipient for the fax.
        /// </summary>
        /// <param name="recipients">The recipient.</param>
        /// <param name="useSpeedDial">The useSpeedDial.</param>
        void AddRecipients(Dictionary<string, string> recipients, bool useSpeedDial);

        /// <summary>
        /// Traverse to Fax Report  on Control Panel and Fetch the Html Fax Report
        /// </summary>
        /// <returns></returns>
        string RetrieveFaxReport();

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        bool ExecuteJob(ScanExecutionOptions executionOptions);

        /// <summary>
        /// Gets the <see cref="IFaxJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        IFaxJobOptions Options { get; }

       
    }
}
