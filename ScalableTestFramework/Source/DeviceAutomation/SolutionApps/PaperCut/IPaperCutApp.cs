using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCut
{
    /// <summary>
    /// Base methods for all PaperCutApp classes
    /// </summary>
    public interface IPaperCutApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches PaperCut with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        bool SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Presses the Print button.
        /// </summary>
        void Print();

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        void Delete();

        /// <summary>
        /// Selects the first document.
        /// </summary>
        /// <param name="documentIds">All the document Ids in the list.</param>
        void SelectFirstDocument(Collection<string> documentIds);

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetDocumentCount();

        /// <summary>
        /// Gets the document ids.
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        Dictionary<string, string> GetDocumentIds();

        /// <summary>
        /// Checks to see if the processing of work has started.
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>
        /// bool
        /// </returns>
        bool FinishedProcessingWork();

        /// <summary>
        /// Dismisses the popup that displays after a Print or Delete operation.
        /// </summary>
        void DismissPostOperationPopup();

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        bool BannerErrorState();

        /// <summary>
        /// Exists the PaperCut print popup message.
        /// </summary>
        /// <returns>bool</returns>
        bool ExistPrintPopupMessage();

    }
}
