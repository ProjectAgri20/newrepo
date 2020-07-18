using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Blueprint
{
    /// <summary>
    /// Blueprint Base Class
    /// </summary>
    public abstract class BlueprintAppBase : DeviceWorkflowLogSource, IBlueprintApp
    {
        private readonly Snmp _snmp;
        private readonly OxpdBrowserEngine _engine;

        //See HPACAppBase for details
        private readonly string[] _deleteButtonId = { "deleteButton" };
        private readonly string[] _printAllButtonId = { "printAllButton" };
        private readonly string[] _printButtonId = { "printButton" };


        private Lazy<bool> _newVersion;

        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        public const string SolutionButtonTitle = "Print Release";

        /// <summary>
        /// Initializes a new instance of the <see cref="BlueprintAppBase" /> class.
        /// </summary>
        /// <param name="snmp">The SNMP.</param>_
        /// <param name="controlPanel">The control panel.</param>
        protected BlueprintAppBase(Snmp snmp, IJavaScriptExecutor controlPanel)
        {
            _snmp = snmp;
            _engine = new OxpdBrowserEngine(controlPanel, BlueprintResource.BlueprintJavaScript);

            _newVersion = new Lazy<bool>(false);
        }

        /// <summary>
        /// Presses the PrintAll button.
        /// </summary>
        public void PrintAll()
        {

            PressButton(_printAllButtonId[0]);
            if (!StartedProcessingWork(TimeSpan.FromSeconds(30)))
            {
                throw new DeviceWorkflowException("Blueprint PrintAll did not start printing documents within 30 seconds.");
            }
        }

        /// <summary>
        /// Presses the Print-Delete button.
        /// </summary>
        public void Print()
        {
            PressButton(_printButtonId[0]);
        }

        /// <summary>
        /// Determines whether the HPAC solution is the newest version.
        /// </summary>
        /// <returns>System.Boolean.</returns>
        public bool IsNewVersion() => _newVersion.Value;

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
        /// Gets the HPAV version 15.4 or newer document ids.
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public string GetFirstDocumentId()
        {
            //char[] separator = { '"', ';' };
            int value = -1;
            bool success = int.TryParse(_engine.ExecuteFunction("getDocumentCountUsingImgTag"), out value);
            //string[] values = id;
            if (success && value > 0)
            {
                return "j1";
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// Presses the Refresh button.
        /// </summary>
        public void Delete()
        {
            PressButton(_deleteButtonId[0]);
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
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public abstract bool FinishedProcessingWork();

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
        /// Selects the given document ID.
        /// </summary>
        public void SelectFirstDocument(string documentValue)
        {
            if (Convert.ToInt32(_engine.ExecuteFunction("CheckElementUsingClassIndex", "checkbox")) > 0)
            {
                _engine.PressElementByClassIndex("checkbox", 0);
            }
            else if(Convert.ToInt32(_engine.ExecuteFunction("CheckElementUsingClassIndex", "hp-listitem-icon hp-listitem-image lg")) > 0)
            {
                _engine.PressElementByClassIndex("hp-listitem-icon hp-listitem-image lg", 0);
            }
            else if(Convert.ToInt32(_engine.ExecuteFunction("CheckElementUsingClassIndex", "hp-listitem-icon hp-listitem-image xl")) > 0)
            {
                _engine.PressElementByClassIndex("hp-listitem-icon hp-listitem-image xl", 0);
            }
            else if(Convert.ToInt32(_engine.ExecuteFunction("CheckElementUsingClassIndex", "hp-listitem hp-listitem-with-title")) > 0)
            {
                _engine.PressElementByClassIndex("hp-listitem hp-listitem-with-title", 0);
            }
        }


        /// <summary>
        /// Selects all documents.
        /// </summary>
        /// <param name="documentIds">The document ids.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SelectAllDocuments(IEnumerable<string> documentIds)
        {
            foreach (string documentId in documentIds)
            {
                PressButton(documentId);
            }
        }



        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetDocumentCount()
        {
            int count = -1;
            string numDocs = _engine.ExecuteFunction("getDocumentCountUsingImgTag");
            bool success = int.TryParse(numDocs, out count);

            return count;
        }


        /// <summary>
        /// Determines whether the device is printing.
        /// </summary>
        /// <param name="checkBy">Check by Oid, Control Panel, etc.</param>
        /// <returns><c>true</c> if the device is printing; otherwise, <c>false</c>.</returns>
        public bool IsPrinting(JobStatusCheckBy checkBy)
        {
            bool result = false;

            switch (checkBy)
            {
                case JobStatusCheckBy.Oid:
                    result = CheckDeviceJobStatusByOid();
                    break;
                case JobStatusCheckBy.ControlPanel:
                    result = CheckDeviceJobStatusByControlPanel();
                    break;
                default:
                    result = true;
                    break;
            }

            return result;
        }

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

    }
}
