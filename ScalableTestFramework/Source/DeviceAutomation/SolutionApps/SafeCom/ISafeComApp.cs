using System;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom
{
    /// <summary>
    /// Contains all shared methods for SafeComApp classes
    /// </summary>
    public interface ISafeComApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches SafeCom with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        void SignInReleaseAll(IAuthenticator authenticator);

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
        /// Determines whether the SafeCom solution is the new Omni-fication [is new version].
        /// </summary>
        /// <returns><c>true</c> if [is new version]; otherwise, <c>false</c>.</returns>
        bool IsNewVersion();

        /// <summary>
        /// Determines whether this instance is an Omni device.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is Omni; otherwise, <c>false</c>.
        /// </returns>
        bool IsOmni();

        /// <summary>
        /// Selects the first document.
        /// </summary>
        /// <param name="documentValue">The document value.</param>
        void SelectFirstDocument(string documentValue);

        /// <summary>
        /// Determines whether the device is printing.
        /// </summary>
        /// <returns><c>true</c> if the device is printing; otherwise, <c>false</c>.</returns>
        bool IsPrinting(JobStatusCheckBy checkBy);

        /// <summary>
        /// Selects the first document in the document list.
        /// </summary>
        void SelectFirstDocument();

        /// <summary>
        /// Selects all documents in the document list.
        /// </summary>
        void SelectAllDocuments();

        /// <summary>
        /// Selects the documents in the given list
        /// </summary>
        /// <param name="documentIds">The document ids.</param>
        void SelectDocuments(IEnumerable<string> documentIds);

        /// <summary>
        /// Checks to see if the processing of work has started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        bool FinishedProcessingWork();

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
        /// Gets the document name by identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns>System.String.</returns>
        string GetDocumentNameById(string documentId);

        /// <summary>
        /// Gets the document ids.
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        IEnumerable<string> GetDocumentIds();

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        void SetCopyCount(int numCopies);
    }
}
