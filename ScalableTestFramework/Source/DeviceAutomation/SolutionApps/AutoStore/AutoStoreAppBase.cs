using System;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.AutoStore
{
    /// <summary>
    /// A3 Solution for RDL testing environment
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowLogSource" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.AutoStore.IAutoStoreApp" />
    public abstract class AutoStoreAppBase : DeviceWorkflowLogSource, IAutoStoreApp
    {
        /// <summary>
        /// Gets or sets the button title.
        /// </summary>
        /// <value>
        /// The button title.
        /// </value>
        protected string ButtonTitle { get; set; }
        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        /// <value>
        /// The name of the document.
        /// </value>
        protected string DocumentName { get; set; }
        /// <summary>
        /// Gets or sets the width of the screen.
        /// </summary>
        /// <value>
        /// The width of the screen.
        /// </value>
        protected int ScreenWidth { get; set; }

        /// <summary>
        /// The execution options
        /// </summary>
        /// <value>
        /// The execution options.
        /// </value>
        protected AutoStoreExecutionOptions ExecutionOptions { get; set; }

        /// <summary>
        /// Gets or sets the email to address.
        /// </summary>
        /// <value>
        /// The email to address.
        /// </value>
        protected string EmailToAddress { get; set; }

        /// <summary>
        /// ID for the 'Hide the Options Button'
        /// </summary>
        /// <value>
        /// The hide more options BTN identifier.
        /// </value>
        protected string HideMoreOptionsBtnId { get; private set; }

        /// <summary>
        /// The more options BTN identifier
        /// </summary>
        protected string MoreOptionsBtnId { get; set; }        

        /// <summary>
        /// The more options image preview
        /// </summary>
        protected string MoreOptionsImagePreview { get; set; }

        /// <summary>
        /// The more options job build
        /// </summary>
        protected string MoreOptionsJobBuild { get; set; }

        /// <summary>
        /// The more options up arrow BTN identifier
        /// </summary>
        protected string MoreOptionsUpArrowBtnId { get; set; }

        /// <summary>
        /// The more options down arrow BTN identifier
        /// </summary>
        protected string MoreOptionsDownArrowBtnId { get; set; }

        /// <summary>
        /// The more options yes BTN identifier
        /// </summary>
        protected string PreviewModeOnId { get; set; }

        /// <summary>
        /// Gets the preview mode off identifier.
        /// </summary>
        /// <value>
        /// The preview mode off identifier.
        /// </value>
        protected string PreviewModeOffId { get; set; }


        /// <summary>
        /// Gets the job build mode on identifier.
        /// </summary>
        /// <value>
        /// The job build mode on identifier.
        /// </value>
        protected string JobBuildModeOnId { get; set; }

        /// <summary>
        /// Gets the job build mode off identifier.
        /// </summary>
        /// <value>
        /// The job build mode off identifier.
        /// </value>
        protected string JobBuildModeOffId { get; set; }

        /// <summary>
        /// The more options no BTN identifier
        /// </summary>
        protected string MoreOptionsNoBtnId { get; private set; }

        /// <summary>
        /// The more options OKAY identifier
        /// </summary>
        protected string MoreOptionsOkId { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoStoreAppBase"/> class.
        /// </summary>
        protected AutoStoreAppBase()
        {
            HideMoreOptionsBtnId = "HPOXPd-hide-options-button";
            MoreOptionsBtnId = "OptionsButton";  

            MoreOptionsImagePreview = "HPOXPd-PreviewModeOption";
            MoreOptionsJobBuild = "HPOXPd-JobAssemblyModeOption";

            JobBuildModeOnId = "li0-" + MoreOptionsJobBuild;
            JobBuildModeOffId = "li1-" + MoreOptionsJobBuild;

            PreviewModeOnId = "li1-" + MoreOptionsImagePreview;
            PreviewModeOffId = "li0-" + MoreOptionsImagePreview;

            // Probably Windjammer only
            MoreOptionsOkId = "HPOXPDDROPDOWNID#1DROPDOWNOKOXPD";
        }

        /// <summary>
        /// Occurs when there is a change in the executing job status.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusMessageUpdate;

        /// <summary>
        /// Launches The AutoStore solution with the given authenticator with either eager or lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        /// <returns></returns>
        public abstract bool ExecuteJob(AutoStoreExecutionOptions executionOptions);

        /// <summary>
        /// Method used to determine or run the end results of the job
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public abstract void JobFinished(AutoStoreExecutionOptions executionOptions);

        /// <summary>
        /// Presses the workflow button as defined in the AutoStore Execution Options.
        /// </summary>
        /// <returns>bool</returns>
        protected abstract bool PressWorkflowButton();

        /// <summary>
        /// Updates the status area with job / solution information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        protected void UpdateStatus(string message, params object[] args)
        {
            var formattedMessage = string.Format(message, args);
            UpdateStatus(formattedMessage);
        }

        /// <summary>
        /// Updates the status area with job / solution information.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void UpdateStatus(string message)
        {
            OnStatusMessageChanged(this, new StatusChangedEventArgs(message));
        }

        /// <summary>
        /// Called when [status message changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
        private void OnStatusMessageChanged(object sender, StatusChangedEventArgs e)
        {
            StatusMessageUpdate?.Invoke(sender, e);
        }
    }
}
