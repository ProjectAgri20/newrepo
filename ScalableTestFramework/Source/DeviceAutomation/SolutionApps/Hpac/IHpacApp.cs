using System;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpac
{
    /// <summary>
    /// Base methods for all HpacApp classes
    /// </summary>
    public interface IHpacApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches HPAC with the specified authenticator using the given authentication mode
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
        /// Presses the Print All button.
        /// </summary>
        void PrintAll();

        /// <summary>
        /// Presses the Print-Delete button.
        /// </summary>
        void PrintDelete();

        /// <summary>
        /// Presses the Print-Keep button.
        /// </summary>
        void PrintKeep();

        /// <summary>
        /// Presses the Refresh button.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        void Delete();

        /// <summary>
        /// Determines whether the HPAC solution is the new Omni-fication [is new version].
        /// </summary>
        /// <returns><c>true</c> if [is new version]; otherwise, <c>false</c>.</returns>
        bool IsNewVersion();

        /// <summary>
        /// Determines whether the device is printing.
        /// </summary>
        /// <returns><c>true</c> if the device is printing; otherwise, <c>false</c>.</returns>
        bool IsPrinting(JobStatusCheckBy checkBy);

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>
        /// bool
        /// </returns>
        bool FinishedProcessingWork();

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Selects the first document in the document list.
        /// </summary>
        void SelectFirstDocument(string checkboxId, string onchangeValue);

        /// <summary>
        /// Selects the first document.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        void SelectFirstDocument(string documentId);

        /// <summary>
        /// Selects all documents in the document list.
        /// </summary>
        void SelectAllDocuments(string onchangeValue);

        /// <summary>
        /// Selects all documents.
        /// </summary>
        /// <param name="documentIds">The document ids.</param>
        void SelectAllDocuments(IEnumerable<string> documentIds);

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetDocumentCount();

        /// <summary>
        /// Gets the first document identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetFirstDocumentId();

        /// <summary>
        /// Gets the document ids.
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        IEnumerable<string> GetDocumentIds();

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        bool BannerErrorState();

        /// <summary>
        /// Gets the checkbox onchange value.
        /// </summary>
        /// <returns></returns>
        HpacInput GetCheckboxOnchangeValue();
    }
}
