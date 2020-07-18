using HP.ScalableTest.DeviceAutomation.Authentication;
using System;
using System.Collections.Generic;
using static HP.ScalableTest.DeviceAutomation.SolutionApps.Blueprint.BlueprintAppBase;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Blueprint
{
    /// <summary>
    /// Base methods for all BlueprintApp classes
    /// </summary>
    public interface IBlueprintApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches HPAC with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Presses the Print All button.
        /// </summary>
        void PrintAll();

        /// <summary>
        /// Presses the Print-Delete button.
        /// </summary>
        void Print();

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
        /// Selects the first document.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        void SelectFirstDocument(string documentId);


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
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        bool BannerErrorState();
    }
}
