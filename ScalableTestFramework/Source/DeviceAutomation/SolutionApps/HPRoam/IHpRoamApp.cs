using HP.ScalableTest.DeviceAutomation.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam.HpRoamAppBase;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam
{
    /// <summary>
    /// Base methods for all HpRoamApp classes
    /// </summary>
    public interface IHpRoamApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches HP Roam with the specified authenticator using the given authentication mode
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
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Selects the first document.
        /// </summary>
        void SelectFirstDocument();

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetDocumentCount();

        /// <summary>
        /// Gets the count of selected documents.
        /// </summary>
        /// <returns></returns>
        int GetSelectedDocumentCount();

        //string GetFirstDocumentId();

        /// <summary>
        /// Signals to the device to stay awake during timely processes. (prevents going back to home screen)
        /// </summary>
        void KeepAwake();

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        bool BannerErrorState();

        /// <summary>
        /// Checks to see if the processing of a pull print related action has finished.
        /// </summary>
        /// <returns>true if job(s) are finished printing.</returns>
        bool FinishedProcessingJob();

        /// <summary>
        /// Checks to see if a delete job has completed.
        /// </summary>
        /// <param name="initialJobCount"></param>
        bool FinishProcessDelete(int initialJobCount);

        /// <summary>
        /// Returns Roam Alert Text.
        /// </summary>
        /// <returns>The alert text.</returns>
        string GetAlertText();

        /// <summary>
        /// Presses the OK button on the Roam Alert.
        /// </summary>
        void HandleAlert();
    }

    /// <summary>
    /// List of available actions for HP Roam Pull Printing
    /// </summary>
    public enum HpRoamPullPrintAction
    {
        /// <summary>
        /// Print All selection for HP Roam
        /// </summary>
        [Description("Print all")]
        PrintAll,
        /// <summary>
        /// Print a single (first) document from the list of print Jobs if possible.
        /// </summary>
        [Description("Print")]
        Print,
        /// <summary>
        /// Delete a single (first) document from the list of print jobs if possible
        /// </summary>
        [Description("Delete")]
        Delete
    }

    /// <summary>Enumeration that defines how the document is to be sent to the HP Roam Cloud</summary>
    public enum DocumentSendAction
    {
        /// <summary>The windows
        /// driver</summary>
        [Description("Windows Driver")]
        Windows,

        /// <summary>The HP Roam application located on the android
        /// phone</summary>
        [Description("Android Phone")]
        Android,

        /// <summary>The HP Roam web client</summary>
        [Description("Web Client")]
        WebClient
    }

}
