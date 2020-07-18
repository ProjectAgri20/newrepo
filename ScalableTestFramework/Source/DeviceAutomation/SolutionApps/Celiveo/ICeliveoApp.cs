using HP.ScalableTest.DeviceAutomation.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Celiveo
{
    /// <summary>
    /// Base methods for all CeliveoApp classes
    /// </summary>
    public interface ICeliveoApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches Celiveo with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        bool SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Presses the Select All button
        /// </summary>
        void SelectAll();

        /// <summary>
        /// Presses the Print button
        /// </summary>
        void Print();

        /// <summary>
        /// Presses the Print B/W button
        /// </summary>
        void PrintBW();

        /// <summary>
        /// Presses the Delete button
        /// </summary>
        void Delete();

        /// <summary>
        /// Verify delete job
        /// </summary>
        /// <returns></returns>
        bool VerifyDeleteJob();

        /// <summary>
        /// Selects the first document.
        /// </summary>
        /// <param name="documentValue"></param>
        void SelectFirstDocument(string documentValue);

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetDocumentCount();

        /// <summary>
        /// Gets the document ids.
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        List<string> GetDocumentIds();

        /// <summary>
        /// Sets the current version as determined from the Celiveo server, 8.4 or 8.5.
        /// </summary>
        void SetCurrentVersion();

        /// <summary>
        /// Checks to see if the processing of work has started.
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns>bool</returns>
        bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Returns true when finished procesing the current work.
        /// </summary>
        /// <returns></returns>
        bool FinishedProcessingWork();

        /// <summary>
        /// Dismisses the popup that displays after a Delete operation.
        /// </summary>
        void DismissPostPrintOperation();

        /// <summary>
        /// Dismisses the popup that displays after a Delete operation.
        /// </summary>
        void DismissPostDeleteOperation();

        /// <summary>
        /// Checkes error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        bool BannerErrorState();
        
        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        void SetCopyCount(int numCopies);
    }
}
