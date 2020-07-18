using System;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.JetAdvantage
{
    /// <summary>
    /// Interface for creating and running the JetAdvantage plugin
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.IDeviceWorkflowLogSource" />
    public interface IJetAdvantageApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// check whether documents are available for print
        /// </summary>
        bool DocumentPrinted { get; }

        /// <summary>
        /// Launch the HP Jet Advantage PullPrinting App
        /// </summary>
        void Launch();

        /// <summary>
        /// Run the Pull Printing Task in HP Jet Advantage
        /// </summary>
        /// <param name="printAll">if set to <c>true</c> [print all].</param>
        /// <param name="deleteDocuments">if set to <c>true</c> [delete].</param>
        void RunHPJetAdvantage(bool printAll, bool deleteDocuments);

        /// <summary>
        /// Sign In to HP Jet Advantage App
        /// </summary>
        /// <param name="jetLoginId">The jet login identifier.</param>
        /// <param name="jetPassword">The jet password.</param>
        /// <param name="useJetPin">if set to <c>true</c> [use jet pin].</param>
        /// <param name="jetPin">The jet pin.</param>
        void SignIn(string jetLoginId, string jetPassword, bool useJetPin, string jetPin);

        /// <summary>
        /// Logout of HP Jet Advantage App
        /// </summary>
        void Logout();

        /// <summary>
        /// Occurs when [activity status change].
        /// </summary>
        event EventHandler<StatusChangedEventArgs> ActivityStatusChange;
    }
}
