using System;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr
{
    /// <summary>
    /// Base class for the HPCR solution
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowLogSource" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr.IHpcrApp" />
    public abstract class HpcrAppBase : DeviceWorkflowLogSource, IHpcrApp
    {
        /// <summary>
        /// The default upper bound of time to wait for a screen to appear (default = 30 seconds)
        /// </summary>
        public TimeSpan DefaultScreenWait { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Updates the status area information 
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusMessageUpdate;

        /// <summary>
        /// Gets or sets the HPCR final status.
        /// </summary>
        /// <value>
        /// The HPCR final status.
        /// </value>
        public string HpcrFinalStatus { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrAppBase"/> class.
        /// </summary>
        protected HpcrAppBase()
        {
            HpcrFinalStatus = null;
        }

        /// <summary>
        /// Launches The Hpcr solution with the given authenticator with either eager or lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        /// <returns></returns>
        public abstract bool ExecuteJob(ScanExecutionOptions executionOptions);

        /// <summary>
        /// Method used to determine or run the end results of the job
        /// </summary>
        public abstract void JobFinished();

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
