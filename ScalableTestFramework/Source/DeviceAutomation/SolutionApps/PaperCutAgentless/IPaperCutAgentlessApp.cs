using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCutAgentless
{
    /// <summary>
    /// Base methods for all PaperCutApp classes
    /// </summary>
    public interface IPaperCutAgentlessApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches PaperCutAgentless with the specified authenticator using the given authentication mode
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
        /// Selects All Jobs.
        /// </summary>        
        void SelectAllDocuments();

        /// <summary>
        /// Set Single Job Options.
        /// </summary>       
        void SetSingleJobOptions(string id, int copies, bool duplexMode, bool colorMode);

        /// <summary>
        /// Set copies on the Single Job Options.
        /// </summary>       
        void SetSingleJobCopies(int copies);

        /// <summary>
        /// Set duplex mode on the Single Job Options.
        /// </summary>       
        void SetSingleJobDuplexMode();

        /// <summary>
        /// Set color mode on the Single Job Options.
        /// </summary>       
        void SetSingleJobGrayScale();

        /// <summary>
        /// Set the Force 2sided option on the print job list.
        /// </summary>
        void SetForce2sided();

        /// <summary>
        /// Set the Force Grayscale option on the print job list.
        /// </summary>
        void SetForcegrayscale();

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
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        bool BannerErrorState();

    }
}
