using System;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Equitrac
{
    /// <summary>
    /// Interface for users of the Equitrac solution
    /// </summary>
    public interface IEquitracApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches Equitrac with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// Gets the document ids.
        /// <summary>Gets Document Ids</summary>
        /// <returns>List of strings</returns>
        IEnumerable<string> GetDocumentIds();

        /// <summary>
        /// Equitrac print button
        /// </summary>
        void Print();

        /// <summary>
        /// Equitrac Print and Save button.
        /// </summary>
        void PrintSave();

        /// <summary>
        /// Equitrac Delete button.
        /// </summary>
        void Delete();

        /// <summary>
        /// Refreshes the Equitrac screen
        /// </summary>
        void Refresh();

        /// <summary>
        /// Selects all the documents.
        /// </summary>
        void SelectAll();

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns></returns>
        bool FinishedProcessingWork();

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Retrieves the current the document/job count.
        /// </summary>
        /// <returns>int</returns>
        int GetDocumentCount();

        /// <summary>
        /// Gets the first document identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetFirstDocumentId();

        /// <summary>
        /// Selects the first document in the document list.
        /// </summary>
        void SelectFirstDocument();

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        void SetCopyCount(int numCopies);
    }
}
