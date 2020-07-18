using HP.ScalableTest.DeviceAutomation.Authentication;
using System;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeQ
{
    /// <summary>
    /// Base methods for all SafeQApp classes
    /// </summary>
    public interface ISafeQApp : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Launches SafeQ with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy</param>
        /// <param name="parameters">The authentication parameters</param>
        void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode, Dictionary<string, object> parameters);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        bool SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Appname is changed to Scan
        /// </summary>
        void ChangeAppName(string appName);

        /// <summary>
        /// Presses the Select All button
        /// </summary>
        void SelectAll();

        /// <summary>
        /// Presses the Option button
        /// </summary>
        void SelectOption();

        /// <summary>
        /// Presses the Option button
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="scanCount">The scan count.</param>
        void SelectJob(string filename, int scanCount);

        /// <summary>
        /// Presses the Option button
        /// </summary>
        void SaveOption();

        /// <summary>
        /// Presses the Print button
        /// </summary>
        void Print();

        /// <summary>
        /// Presses the Print B/W button
        /// </summary>
        void PrintAll();

        /// <summary>
        /// Presses the Scan button
        /// </summary>
        void Scan(int numberOfcopies);

        /// <summary>
        /// Presses the Delete button
        /// </summary>
        void Delete();

        /// <summary>
        /// Selects the first document.
        /// </summary>
        /// <param name="documentValue"></param>
        void SelectFirstDocument(string documentValue);

        /// <summary>
        /// To scan Sided Job in a SafeQScan
        /// </summary>
        void ScanOption_SidedJob(int ScanCount);

        /// <summary>
        /// Wait for job completed
        /// </summary>
        bool WaitForScanning(int scanCount);

        /// <summary>
        /// Checks that the current screen is home
        /// </summary>
        bool IsHomeScreen();

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
        /// Checkes error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        bool BannerErrorState();
        
        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        void SetCopyCount(int numCopies);

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="colorType">The number copies.</param>
        void SetColorMode(SafeQPrintColorMode colorType);

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="sidesType">The number copies.</param>
        void SetSides(SafeQPrintSides sidesType);
    }
}
