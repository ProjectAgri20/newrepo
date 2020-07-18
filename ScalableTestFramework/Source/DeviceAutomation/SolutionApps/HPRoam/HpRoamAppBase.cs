using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam;
using System;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam
{
    /// <summary>
    /// HpRoam Base Class
    /// </summary>
    public abstract class HpRoamAppBase : DeviceWorkflowLogSource, IHpRoamApp
    {
        private readonly string[] _deleteButtonId = { "job-delete" };
        private readonly string[] _printButtonId = { "print-btn" };

        private Lazy<bool> _newVersion;

        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        public const string SolutionButtonTitle = "Print Release";

        /// <summary>
        /// Initializes a new instance of the <see cref="HpRoamAppBase" /> class.
        /// </summary>
        /// <param name="snmp">The SNMP.</param>_
        /// <param name="controlPanel">The control panel.</param>
        protected HpRoamAppBase(Snmp snmp, IJavaScriptExecutor controlPanel)
        {
            Snmp = snmp;
            OxpdEngine = new OxpdBrowserEngine(controlPanel, HpRoamResource.HpRoamJavaScript);

            _newVersion = new Lazy<bool>(false);
        }

        /// <summary>
        /// The OXPd browser engine.
        /// </summary>
        protected OxpdBrowserEngine OxpdEngine { get; private set; }

        /// <summary>
        /// The Snmp object.
        /// </summary>
        protected Snmp Snmp { get; private set; }

        /// <summary>
        /// Presses a button via the Oxpd Engine.
        /// </summary>
        /// <param name="buttonId">The id of the button to press</param>
        protected void PressButton(string buttonId)
        {
            OxpdEngine.PressElementById(buttonId);
        }

        /// <summary>
        /// Wait specific value for available
        /// </summary>
        /// <param name="value">Value for waiting</param>
        /// <param name="time">Waiting time</param>
        /// <returns></returns>
        protected bool WaitForHtmlContains(string value, TimeSpan time)
        {
            return OxpdEngine.WaitForHtmlContains(value, time);
        }

        /// <summary>
        /// Presses the PrintAll button.
        /// </summary>
        public void PrintAll()
        {
            TimeSpan timeOut = TimeSpan.FromSeconds(30);
            OxpdEngine.PressElementByClassIndex(_printButtonId[0], 0);
            if (!StartedProcessingWork(timeOut))
            {
                throw new DeviceWorkflowException($"HpRoam PrintAll did not start printing documents within {timeOut.TotalSeconds} seconds.");
            }
        }

        /// <summary>
        /// Presses the Print-Delete button.
        /// </summary>
        public void Print()
        {
            OxpdEngine.PressElementByClassIndex(_printButtonId[0], 0);
        }

        /// <summary>
        /// Determines whether the HPAC solution is the newest version.
        /// </summary>
        /// <returns>System.Boolean.</returns>
        public bool IsNewVersion() => _newVersion.Value;

        /// <summary>
        /// Launches HP Roam with the specified authenticator using the given authentication mode
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
        /// Presses the Delete button.
        /// </summary>
        public void Delete()
        {
            PressButton(_deleteButtonId[0]);           
        }

        /// <summary>
        /// Returns true when finished processing the current print job
        /// </summary>
        /// <returns>true if job(s) are finished printing.</returns>
        public bool FinishedProcessingJob()
        {
            bool parseSuccess = false;
            string javascriptResult = string.Empty;
            int processingCount = -1;

            javascriptResult = OxpdEngine.ExecuteFunction("getJobProcessingCount");
            System.Diagnostics.Debug.WriteLine($"Jobs processing = {javascriptResult}");
            parseSuccess = int.TryParse(javascriptResult, out processingCount);
            if (parseSuccess)
            {
                return processingCount == 0;
            }

            return false;
        }

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public abstract bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Enumeration of methods by which to check Job Status.
        /// </summary>
        public enum JobStatusCheckBy
        {
            /// <summary>
            /// Check by OID
            /// </summary>
            Oid,

            /// <summary>
            /// Check by control panel
            /// </summary>
            ControlPanel
        }

        /// <summary>
        /// Selects the first document ID.
        /// </summary>
        public void SelectFirstDocument()
        {
            if (OxpdEngine.ExecuteFunction("isElementSelected", "circle-check").Equals("true"))
            {
                OxpdEngine.PressElementByClassIndex("select-all-circle-check", 0);
            }

            if (OxpdEngine.WaitToExistElementId("circle-check", TimeSpan.FromSeconds(1)))
            {
                OxpdEngine.PressElementByClassIndex("circle-check", 0);
            }
        }

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetDocumentCount()
        {
            int count = 0;
            if (int.TryParse(OxpdEngine.ExecuteFunction("getDocumentCount"), out count) && count > 0)
            {
                return count;
            }

            return 0;
        }

        /// <summary>
        /// Gets the count of selected documents.
        /// </summary>
        /// <returns></returns>
        public int GetSelectedDocumentCount()
        {
            int count = 0;
            if (int.TryParse(OxpdEngine.ExecuteFunction("getSelectedDocumentCount"), out count) && count > 0)
            {
                return count;
            }

            return 0;
        }


        /// <summary>
        /// Signals the app to staky awake for additional functions
        /// </summary>
        public abstract void KeepAwake();

        /// <summary>
        /// Waits for app to finish processing a single delete
        /// </summary>
        public virtual bool FinishProcessDelete(int initialJobCount)
        {
            bool success = false;
            int finishCount = -1;
            bool parseSuccess = int.TryParse(OxpdEngine.ExecuteFunction("getDocumentCount"), out finishCount);
            if (parseSuccess && finishCount < initialJobCount)
            {
                success = true;
            }
            else
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Returns Roam Alert text.
        /// </summary>
        public string GetAlertText()
        {
            string rawText = OxpdEngine.ExecuteFunction("getRoamAlertText");
            return rawText.Trim('"');
        }

        /// <summary>
        /// Presses the OK button on the Roam Alert.
        /// </summary>
        public void HandleAlert()
        {
            OxpdEngine.PressElementById("ok-btn");
        }

    }
}
