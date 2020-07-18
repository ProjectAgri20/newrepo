using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Dss
{
    /// <summary>
    /// Interface for a Workflow application provided by HP Digital Sending Software.
    /// </summary>
    public interface IDssWorkflowApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Launches the Workflow application on the device.
        /// </summary>
        void Launch();

        /// <summary>
        /// Launches DSS with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Selects the workflow with the specified name from the default workflow menu.
        /// </summary>
        /// <param name="workflowName">The workflow name.</param>
        void SelectWorkflow(string workflowName);

        /// <summary>
        /// Enters the specified value for the specified workflow prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="value">The value.</param>
        void EnterPromptValue(string prompt, string value);

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        bool ExecuteJob(ScanExecutionOptions executionOptions);

        /// <summary>
        /// Gets the <see cref="IDssWorkflowJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        IDssWorkflowJobOptions Options { get; }
    }
}
