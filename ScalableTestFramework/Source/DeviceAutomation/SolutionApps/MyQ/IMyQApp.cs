using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.MyQ
{
    /// <summary>
    /// Base methods for all MyQ classes
    /// </summary>
    public interface IMyQApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches MyQ with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Presses the PrintAll button.
        /// </summary>
        void PrintAll();

        /// <summary>
        /// Presses the Print button.
        /// </summary>
        void Print();

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        void Delete();

        /// <summary>
        /// Presses the PullPrinting
        /// </summary>
        void PressPullPrinting();

        /// <summary>
        /// Go to Main Page();
        /// </summary>
        void GotoMainPage();

        /// <summary>
        /// Selects the first document.
        /// </summary>
        void SelectFirstDocument();

        /// <summary>
        /// Selects All Jobs.
        /// </summary>        
        void SelectAllDocuments();

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetDocumentCount();

        /// <summary>
        /// Press Easy Scan Folder
        /// </summary>
        void PressEasyScanFolder();

        /// <summary>
        /// Press Easy Scan Email
        /// </summary>
        void PressEasyScanEmail();

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
