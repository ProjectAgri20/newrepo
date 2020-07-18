using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.GeniusBytes
{
    /// <summary>
    /// Base methods for all GeniusBytesApp classes
    /// </summary>
    public interface IGeniusBytesApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Occurs when there is a change in the executing job status.
        /// </summary>
        event EventHandler<StatusChangedEventArgs> StatusMessageUpdate;

        /// <summary>
        /// Launches GeniusBytes with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        void Launch(IAuthenticator authenticator);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        bool SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Presses the Print all
        /// </summary>
        void PrintAll();

        /// <summary>
        /// Presses the PullPrinting
        /// </summary>
        void PullPrinting();

        /// <summary>
        /// Press App Name
        /// </summary>
        void PressAppName(string appname);

        /// <summary>
        /// Presses the Print all and Delete button
        /// </summary>
        void PrintAllandDelete();

        /// <summary>
        /// Presses the Print button
        /// </summary>
        void Print();

        /// <summary>
        /// Press the Start-Scan button
        /// </summary>
        void StartScan(string sides, int pageCount);

        /// <summary>
        /// Presses the Print and Delete button
        /// </summary>
        void PrintandDelete();

        /// <summary>
        /// Presses the Delete button
        /// </summary>
        void Delete();

        /// <summary>
        /// Presses the Delete All button
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Selects the first document.
        /// </summary>
        void SelectFirstDocument();
        
        /// <summary>
        /// Selects the first email.
        /// </summary>
        void SelectFirstEmail();

        /// <summary>
        /// Sets the email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        void SetEmailAddress(string emailAddress);

        /// <summary>
        /// Add Document
        /// </summary>
        void AddDocument(TimeSpan timeOut);

        /// <summary>
        /// Add Document for OCR
        /// </summary>
        void AddDocumentforOCR();

        /// <summary>
        /// Add Email
        /// </summary>
        void AddEmail();

        /// <summary>
        /// Confirm Email
        /// </summary>
        void ConfirmEmail();

        /// <summary>
        /// Set Color Mode As Print
        /// </summary>
        void SetColorModeAsPrint();

        /// <summary>
        /// Set Color Mode As Print Black and White
        /// </summary>
        void SetColorModeAsPrintBW();

        /// <summary>
        /// Set FileType
        /// </summary>
        void SetFileType(GeniusByteScanFileTypeOption fileType, TimeSpan timeOut);

        /// <summary>
        /// Set FileName
        /// </summary>
        void SetFileName(string filename, GeniusBytesScanType scanType);

        /// <summary>
        /// Sets the image preview.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        void SetImagePreview(bool enabled);

        /// <summary>
        /// Set color options for the scan job.
        /// </summary>
        /// <returns>The result of the scan.</returns>
        void SelectColorOption(GeniusByteScanColorOption value, TimeSpan timeOut);

        /// <summary>
        /// Set sides options for the scan job.
        /// </summary>
        /// <returns>The result of the scan.</returns>
       void SelectSidesOption(GeniusByteScanSidesOption value, TimeSpan timeOut);

        /// <summary>
        /// Set resolution options for the scan job.
        /// </summary>
        /// <returns>The result of the scan.</returns>
        void SelectResolutionOption(GeniusByteScanResolutionOption value, TimeSpan timeOut);

        /// <summary>
        /// Press Back Key.
        /// </summary>
        void PressBackKey();

        /// <summary>
        /// Press OK Key.
        /// </summary>
        void PressOKKey();

        /// <summary>
        /// Press Close Key.
        /// </summary>
        void PressCloseKey();

        /// <summary>
        /// Press Cancel Key.
        /// </summary>
        void PressCancelKey();

        /// <summary>
        /// Sign Out for Genius Bytes
        /// </summary>
        void SignOut();

        /// <summary>
        /// Scroll To Object
        /// </summary>
        bool ScrollToObject(string value, bool isDialog);

        /// <summary>
        /// Press Down-Arrow Button
        /// </summary>
        /// <returns></returns>
        void ArrowDown(string arrowName);

        /// <summary>
        /// Press Confirm Button
        /// </summary>
        /// <returns></returns>
        void Confirm();

        /// <summary>
        /// Press Confirm Button on the popup
        /// </summary>
        /// <returns></returns>
        void PressConfirmonPopup();

        /// <summary>
        /// Press Print Button on the popup
        /// </summary>
        /// <returns></returns>
        void PressPrintonPopup();

        /// <summary>
        /// Verify Home for Genius Bytes
        /// </summary>
        bool VerifySignOut();

        /// <summary>
        /// Get Document IDs.
        /// </summary>
        List<string> GetDocumentIds();

        /// <summary>
        /// Get Inner HTML by Text Contains.
        /// </summary>
        int GetDocumentsCount();

        /// <summary>
        /// Checks to see if the processing of work has started.
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns>bool</returns>
        bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns></returns>
        bool FinishedProcessingWork();

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        bool BannerErrorState();

        /// <summary>
        /// Checks if the given element ID exists on the current page
        /// </summary>
        /// <param name="elementText">Name of the element.</param>
        /// <return>true if exists</return>
        bool ExistElementText(string elementText);

        /// <summary>
        /// Waits for the given time for the element to become ready for use.
        /// </summary>
        /// <param name="element">The element text.</param>
        /// <param name="time">time to wait.</param>
        /// <returns>true if usable</returns>
        bool WaitForElementReady(string element, TimeSpan time);

        /// <summary>
        /// Wait Object For Available.
        /// </summary>
        /// <returns></returns>
        bool WaitObjectForAvailable(string value, TimeSpan time);

        /// <summary>
        /// Waits for object to be available up to the given time
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="time">The time.</param>
        /// <returns>true if available</returns>
        bool WaitForObjectAvailable(string objectId, TimeSpan time);

        /// <summary>
        /// Run Click the option value for each options.
        /// </summary>
        /// <param name="value">Option Value.</param>
        void SelectOptionValue(string value);
    }
}
