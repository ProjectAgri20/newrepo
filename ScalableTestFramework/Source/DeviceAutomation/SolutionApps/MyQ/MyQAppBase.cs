using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.MyQ
{
    /// <summary>
    /// MyQ Base class.
    /// </summary>
    public abstract class MyQAppBase : DeviceWorkflowLogSource, IMyQApp
    {
        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;
        private readonly string[] _deleteButtonId = { "cancel-job-button" };
        private readonly string[] _printButtonId = { "release-job-button" };
        private readonly string[] _selectAllButtonId = { "page-title-select-job-details", "select-job-checkbox" };

        /// <summary>
        /// Initializes a new instance of the <see cref="MyQAppBase" /> class.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        protected MyQAppBase(IJavaScriptExecutor controlPanel)
        {
            _engine = new OxpdBrowserEngine(controlPanel, MyQResource.MyQJavaScript);
        }

        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        public const string SolutionButtonTitle = "MyQ";

        /// <summary>
        /// Presses the PrintAll button.
        /// </summary>
        public abstract void PrintAll();

        /// <summary>
        /// Presses the Print button.
        /// </summary>
        public abstract void Print();

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Press the PullPrinting
        /// </summary>
        public abstract void PressPullPrinting();

        /// <summary>
        /// Press back to go to main page
        /// </summary>
        public abstract void GotoMainPage();

        /// <summary>
        /// Selects first documents.
        /// </summary>
        public abstract void SelectFirstDocument();

        /// <summary>
        /// Selects all documents.
        /// </summary>        
        public abstract void SelectAllDocuments();

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>Document count</returns>
        public abstract int GetDocumentCount();

        /// <summary>
        /// Press Easy Scan Folder
        /// </summary>
        public abstract void PressEasyScanFolder();

        /// <summary>
        /// Press Easy Scan Email
        /// </summary>
        public abstract void PressEasyScanEmail();

        /// <summary>
        /// Launches HPAC with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        public abstract bool BannerErrorState();

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
        /// Presses a button on the control panel.
        /// </summary>
        /// <param name="id">The id of the button to press</param>
        /// <param name="index">Then index of the button to press</param>
        public void PressButtonByClassIndex(string id, int index)
        {
            _engine.PressElementByClassIndex(id, index);
        }

        /// <summary>
        /// Execute Function
        /// </summary>
        /// <param name="function">function name</param>
        public string ExecuteFunction(string function)
        {
           return  _engine.ExecuteFunction(function);
        }

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

        
    }
}
