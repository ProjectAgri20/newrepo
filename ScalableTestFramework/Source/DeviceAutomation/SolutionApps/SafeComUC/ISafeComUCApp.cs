using System;
using System.Collections.Generic;
using System.ComponentModel;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeComUC
{
    /// <summary>
    /// Contains all shared methods for SafeComUCApp classes
    /// </summary>
    public interface ISafeComUCApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Device Inactivity Timeout setting.
        /// </summary>
        TimeSpan DeviceInactivityTimeout { get; }

        /// <summary>
        /// Launches SafeComUC with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Launches SafeComUC Print All with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        void LaunchPrintAll(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        void SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Presses the Menu button.
        /// </summary>
        void Menu();

        /// <summary>
        /// Presses the Print All button.
        /// </summary>
        void PrintAll();

        /// <summary>
        /// Presses the Print-Delete button.
        /// </summary>
        void Print();

        /// <summary>
        /// Presses the Retain button.
        /// </summary>
        void Retain();

        /// <summary>
        /// Presses the Unretain button.
        /// </summary>
        void Unretain();

        /// <summary>
        /// Presses the Refresh button.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        void Delete();

        /// <summary>
        /// Wait for Job list restored
        /// </summary>
        void WaitForJobList(int jobCount, int waitingSeconds);

        /// <summary>
        /// Wait for Retain icon
        /// </summary>
        bool WaitForRetainIcon(int waitingSeconds);

        /// <summary>
        /// Wait for disappear Retain icon
        /// </summary>
        bool WaitForRetainIconDisappeared(int waitingSeconds);

        /// <summary>
        /// Selects the first document in the document list.
        /// </summary>
        void SelectFirstDocument();

        /// <summary>
        /// Selects all documents in the document list.
        /// </summary>
        void SelectAllDocuments();

        /// <summary>
        /// Checks to see if the processing of work has started, or timeout was reached.
        /// </summary>
        /// <param name="action">The pull print action</param>
        /// <param name="waitTime">Time to continue checking</param>
        /// <returns>bool</returns>
        bool StartedProcessingWork(SafeComUCPullPrintAction action, TimeSpan waitTime);

        /// <summary>
        /// Returns true when finished processing the current work, or timeout was reached.
        /// </summary>
        /// <param name="action">The pull print action</param>
        /// <returns>bool</returns>
        bool FinishedProcessingWork(SafeComUCPullPrintAction action);

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
        /// Gets the first document name.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetFirstDocumentName();

        /// <summary>
        /// Gets the document ids.
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        IEnumerable<string> GetDocumentIds();

        /// <summary>
        /// Gets the document names.
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        IEnumerable<string> GetDocumentNames();
        
        /// <summary>
        /// Checking first job checked
        /// </summary>
        bool IsFirstJobCheckBoxChecked();

        /// <summary>
        /// Checking Select All checked
        /// </summary>
        bool IsSelectAllCheckBoxChecked();

        /// <summary>
        /// Press the copy count.
        /// </summary>        
        void PressCopyCount();

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        void SetCopyCount(int numCopies);
    }

    /// <summary>
    /// List of available actions for SafeCom Unified Client Pull Printing
    /// </summary>
    public enum SafeComUCPullPrintAction
    {
        /// <summary>
        /// Print All from the document list menu.
        /// </summary>
        [Description("Print All")]
        PrintAll,
        /// <summary>
        /// Print the selected document
        /// </summary>
        [Description("Print")]
        Print,
        /// <summary>
        /// Delete all documents.
        /// </summary>
        [Description("Delete All")]
        DeleteAll,
        /// <summary>
        /// Delete the selected document.
        /// </summary>
        [Description("Delete")]
        Delete,
        /// <summary>
        /// Print all documents using "retain".
        /// </summary>
        [Description("Print Retain All")]
        PrintRetainAll,
        /// <summary>
        /// Print the selected document using "retain".
        /// </summary>
        [Description("Print Retain")]
        PrintRetain,
        /// <summary>
        /// Print all documents, unretaining all.
        /// </summary>
        [Description("Print Unretain All")]
        PrintUnretainAll,
        /// <summary>
        /// Print the selected document unretaining it.
        /// </summary>
        [Description("Print Unretain")]
        PrintUnretain,
        /// <summary>
        /// Refresh the document list.
        /// </summary>
        [Description("Refresh")]
        Refresh,
        /// <summary>
        /// Print All by pressing the "Print All" tile from the device home screen.
        /// </summary>
        [Description("Print All App - Release All")]
        PrintAllApp
    }
}
