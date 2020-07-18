using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.iSecStar
{
    /// <summary>
    /// iSecStar Base class.
    /// </summary>
    public abstract class iSecStarAppBase : DeviceWorkflowLogSource, IiSecStarApp
    {
        private readonly Snmp _snmp;
        private readonly OxpdBrowserEngine _engine;

        private readonly string[] _printKeepButtonId = { "printSaveButton" };
        private readonly string[] _printButtonId = { "okayButton" };
        private readonly string[] _deleteButtonId = { "delButton" };

        private const int Version = 0;

        private Lazy<int> _currentVersion;

        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        public const string SolutionButtonTitle = "ISecStar Secure Pull Print";

        /// <summary>
        /// Initializes a new instance of the <see cref="iSecStarAppBase" /> class.
        /// </summary>
        /// <param name="snmp">The SNMP.</param>_
        /// <param name="controlPanel">The control panel.</param>
        protected iSecStarAppBase(Snmp snmp, IJavaScriptExecutor controlPanel)
        {
            _snmp = snmp;
            _engine = new OxpdBrowserEngine(controlPanel, iSecStarResource.iSecStarJavaScript);

            _currentVersion = new Lazy<int>(Version);

        }

        /// <summary>
        /// Presses the Print button
        /// </summary>
        public void Print()
        {
            PressButton(_printButtonId[_currentVersion.Value]);
            if (!StartedProcessingWork(TimeSpan.FromSeconds(60)))
            {
                throw new DeviceWorkflowException("The operation did not complete within 60 seconds.");
            }
        }

        /// <summary>
        /// Presses the Print-Keep button.
        /// </summary>
        public void PrintKeep()
        {
            PressButton(_printKeepButtonId[_currentVersion.Value]);
            if (!StartedProcessingWork(TimeSpan.FromSeconds(60)))
            {
                throw new DeviceWorkflowException("The opertaion did not complete within 60 seconds.");
            }
        }

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        public void Delete()
        {
            PressButton(_deleteButtonId[_currentVersion.Value]);
        }

        /// <summary>
        /// Selects all documents from the document list.
        /// </summary>
        public void SelectAllDocuments(string className)
        {
            string element = _engine.ExecuteFunction("SelectAllDocuments", className);
        }

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetDocumentCount()
        {
            int count = -1;
            bool success = int.TryParse(_engine.ExecuteFunction("getDocumentCount"), out count);

            return count;
        }

        /// <summary>
        /// Selects the given document ID.
        /// </summary>
        public void SelectFirstDocument(string className)
        {
            _engine.ExecuteFunction("SelectSingleDocument", className);
        }

        /// <summary>
        /// Launches ISecStar with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public abstract bool SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        public abstract bool BannerErrorState();

        /// <summary>
        /// Checks the device job status by oid.
        /// </summary>
        /// <returns></returns>
        private bool CheckDeviceJobStatusByOid()
        {
            string deviceStatus = _snmp.Get("1.3.6.1.4.1.11.2.3.9.1.1.3.0");

            //ExecutionServices.SystemTrace.LogDebug(string.Format("Device printing status via OID = {0}, (Status={1})", printingFinished, deviceStatus));
            if (deviceStatus.StartsWith("ready", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

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
        /// <param name="printButtonId">The id of the print button to press</param>
        private void PressButton(string printButtonId)
        {
            _engine.PressElementById(printButtonId);
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
