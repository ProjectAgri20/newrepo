using System;
using System.Collections.Generic;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;
using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeQ
{
    /// <summary>
    /// SafeQApp Base class.
    /// </summary>
    public abstract class SafeQAppBase : DeviceWorkflowLogSource, ISafeQApp
    {
        /// <summary>
        /// Updates the status area information 
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusMessageUpdate;
        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeQAppBase" /> class.
        /// </summary>
        /// <param name="controlPanel"></param>
        protected SafeQAppBase(IJavaScriptExecutor controlPanel)
        {
            _engine = new OxpdBrowserEngine(controlPanel, SafeQResource.SafeQJavaScript);
        }
        /// <summary>
        /// Gets the solution button title for pullprinting.
        /// </summary>
        public const string SolutionButtonPrintTitle = "SafeQ Print";

        /// <summary>
        /// Gets the solution button title for scan.
        /// </summary>
        public const string SolutionButtonScanTitle = "SafeQ Scan";
        /// <summary>
        /// Presses the Select All button
        /// </summary>
        public abstract void SelectAll();

        /// <summary>
        /// Presses the Print button
        /// </summary>
        public abstract void Print();

        /// <summary>
        /// Presses the Print B/W button
        /// </summary>
        public abstract void PrintAll();

        /// <summary>
        /// Presses the Delete button
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Un-selects all documents except the first.
        /// </summary>
        /// <param name="documentValue">The javascript values (dynamically generated) for toggling the checkbox selection.</param>
        public virtual void SelectFirstDocument (string documentValue)
        {
            _engine.PressElementById(documentValue);            
        }

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>Document count</returns>
        public abstract int GetDocumentCount();


        /// <summary>
        /// Get all documents Ids by JavaScript.
        /// The Ids separated by "$"
        /// </summary>
        /// <returns></returns>
        public abstract List<string> GetDocumentIds();
        
        /// <summary>
        /// Launches SafeQ with the specified authenticator using given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy</param>
        /// <param name="parameters">The authentication parameters</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode, Dictionary<string,object> parameters);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator"></param>
        public abstract bool SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        public abstract bool BannerErrorState();
        /// <summary>
        /// Change App name by job
        /// </summary>
        public abstract void ChangeAppName(string appName);

        /// <summary>
        /// Checks that the current screen is home
        /// </summary>
        public abstract bool IsHomeScreen();

        /// <summary>
        /// Checks the device job status by control panel.
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckDeviceJobStatusByControlPanel()
        {
            return true;
        }

        /// <summary>
        /// Presses a button on the control panel.
        /// </summary>
        /// <param name="buttonId">The id of the button to press</param>
        public void PressButton(string buttonId)
        {
            _engine.PressElementById(buttonId);
        }
        
        /// <summary>
        /// Wait specific value for available
        /// </summary>
        /// <param name="value">Value for waiting</param>
        /// <param name="time">Waiting time</param>
        /// <returns></returns>
        public bool WaitObjectForAvailable(string value, TimeSpan time)
        {   
           return _engine.WaitForHtmlContains(value, time);

        }

        /// <summary>
        /// Dismisses the popup that displays after a Print or Delete operation.
        /// </summary>
        public abstract void DismissPostPrintOperation();

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public abstract bool StartedProcessingWork(TimeSpan ts);
        
        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public abstract bool FinishedProcessingWork();

        /// <summary>
        /// To scan Sided Job in a SafeQScan
        /// </summary>
        public abstract void ScanOption_SidedJob(int ScanCount);

        /// <summary>
        /// Wait for job completed
        /// </summary>
        public abstract bool WaitForScanning(int scanCount);

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopie">The number copie.</param>
        public abstract void SetCopyCount(int numCopie);

        /// <summary>
        /// Select the SetColorMode.
        /// </summary>
        /// <param name="colorType">The number copie.</param>
        public abstract void SetColorMode(SafeQPrintColorMode colorType);

        /// <summary>
        /// Sets the Sides Option.
        /// </summary>
        /// <param name="sidedMode">The number copies.</param>
        public abstract void SetSides(SafeQPrintSides sidedMode);

        /// <summary>
        /// Press Option button
        /// </summary>
        public abstract void SelectOption();

        /// <summary>
        /// Select Job on the SafeQScan
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="scanCount">The scan count.</param>
        public abstract void SelectJob(string filename, int scanCount);

        /// <summary>
        /// Scan Job
        /// </summary>
        public void Scan(int numberOfcopies)
        {
            bool result = true;

            ScanOption_SidedJob(numberOfcopies);

            result = WaitForScanning(numberOfcopies);
            if (result && numberOfcopies > 1)
            {
                RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
            }
            else if(!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Execution Sided Scan Job failed");
                throw e;
            }
        }
        
        /// <summary>
        /// Press Save button
        /// </summary>
        public abstract void SaveOption();

        /// <summary>
        /// Press Element by Text
        /// </summary>
        protected void PressElementByText(string parentId, string tag, string text)
        {            
            string elementWithText = $"getElementByText('{parentId}','{tag}','{text}')";
            BoundingBox boundingBox = _engine.GetElementBoundingArea(elementWithText);
            _engine.PressElementByBoundingArea(boundingBox);
        }
    }

    /// <summary>
    /// Defines the color-mode option of basic settings
    /// </summary>
    public enum SafeQPrintColorMode
    {
        /// <summary>
        /// Black and white
        /// </summary>
        [Description("B/W")]
        BW,
        /// <summary>
        /// Color
        /// </summary>
        [Description("Color")]
        Color
    }

    /// <summary>
    /// Defines the sides option of basic settings
    /// </summary>
    public enum SafeQPrintSides
    {
        /// <summary>
        /// One-Sided
        /// </summary>
        [Description("One-sided")]
        Onesided,
        /// <summary>
        /// Two-Sided
        /// </summary>
        [Description("Two-sided")]
        Twosided
    }
}