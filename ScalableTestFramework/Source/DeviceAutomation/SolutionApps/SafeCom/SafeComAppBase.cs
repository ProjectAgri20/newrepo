using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowLogSource" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom.ISafeComApp" />
    public abstract class SafeComAppBase : DeviceWorkflowLogSource, ISafeComApp
    {
        private readonly Snmp _snmp;
        protected readonly OxpdBrowserEngine _engine;

        private readonly string[] _deleteButtonId = { "btnDelete", "button-delete" };
        private readonly string[] _printAllButtonId = { "btnPrintAll", "button-printAll" };
        private readonly string[] _printDeleteButtonId = { "btnPrint", "button-print" };
        private readonly string[] _printKeepButtonId = { "btnRetain", "button-retain" };
        private readonly string[] _refreshButtonId = { "btnRefresh", "button-refresh" };
        private readonly string[] _inputCopies = { "buttonText", "inputCopies" };

        private const int SafeComOld = 0;
        private const int SafeComGO = 1;

        /// <summary>
        /// The new version
        /// </summary>
        private Lazy<bool> _newVersion;

        /// <summary>
        /// The is an Omni device
        /// </summary>
        protected bool _isOmni = true;

        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        public const string SolutionButtonTitle = "Pull Print";

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComAppBase" /> class
        /// </summary>
        /// <param name="snmp">The SNMP.</param>
        /// <param name="controlPanel">The control panel.</param>
        protected SafeComAppBase(Snmp snmp, IJavaScriptExecutor controlPanel)
        {
            _snmp = snmp;
            _engine = new OxpdBrowserEngine(controlPanel, SafeComResource.SafeComJavaScript);
            _newVersion = new Lazy<bool>(IsSafeComGoOrNewer);
            
        }

        /// <summary>
        /// Determines whether [is safe COM go or newer].
        /// </summary>
        /// <returns>System.Boolean.</returns>
        private bool IsSafeComGoOrNewer()
        {
            bool isNewSafeComGo = true;

            var isNew = _engine.ExecuteFunction("ExistButtonId", _inputCopies[SafeComGO]);
            isNewSafeComGo = bool.Parse(isNew.ToString());

            return isNewSafeComGo;
        }

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        public void Delete()
        {
            if (IsOmni() && _newVersion.Value)
            {
                PressButton(_deleteButtonId[SafeComGO]);
            }
            else
            {
                PressButton(_deleteButtonId[SafeComOld]);
            }
        }

        /// <summary>
        /// Presses the Print All button.
        /// </summary>
        public void PrintAll()
        {
            if (IsOmni() && _newVersion.Value)
            {
                PressButton(_printAllButtonId[SafeComGO]);
            }
            else
            {
                PressButton(_printAllButtonId[SafeComOld]);
            }
        }

        /// <summary>
        /// Presses the Print-Delete button.
        /// </summary>
        public void PrintDelete()
        {
            if (IsOmni() && _newVersion.Value)
            {
                PressButton(_printDeleteButtonId[SafeComGO]);
            }
            else
            {
                PressButton(_printDeleteButtonId[SafeComOld]);
            }
        }

        /// <summary>
        /// Presses the Print-Keep button.
        /// </summary>
        public void PrintKeep()
        {
            if (IsOmni() && _newVersion.Value)
            {
                PressButton(_printKeepButtonId[SafeComGO]);
            }
            else
            {
                PressButton(_printKeepButtonId[SafeComOld]);
            }
        }

        /// <summary>
        /// Presses the Refresh button.
        /// </summary>
        public void Refresh()
        {
            if (IsOmni() && _newVersion.Value)
            {
                PressButton(_refreshButtonId[SafeComGO]);
            }
            else
            {
                PressButton(_refreshButtonId[SafeComOld]);
            }
        }

        /// <summary>
        /// Selects all documents in the document list.
        /// </summary>
        public void SelectAllDocuments()
        {
            if (IsOmni() && _newVersion.Value)
            {
                throw new DeviceWorkflowException("SafeCom GO requires the method call: SelectDocuments(List<string> documentIds)");
            }
            else
            {
                _engine.ExecuteFunction("selectAllDocuments", "checkbox");
            }
        }

        /// <summary>
        /// Selects the documents in the given list
        /// </summary>
        /// <param name="documentIds">The document ids.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SelectDocuments(IEnumerable<string> documentIds)
        {
            foreach (string docId in documentIds)
            {
                _engine.PressElementById(docId);
            }
        }

        /// <summary>
        /// Selects the first document in the document list.
        /// </summary>
        public void SelectFirstDocument()
        {
                _engine.ExecuteFunction("selectFirstDocument", "checkbox");
        }

        /// <summary>
        /// Selects the given document ID.
        /// </summary>
        public void SelectFirstDocument(string documentValue)
        {
            _engine.PressElementById(documentValue);
        }
        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>
        /// System.Int32.
        /// </returns>
        public virtual int GetDocumentCount()
        {
            int count = 0;

            if (IsOmni() && _newVersion.Value)
            {
                count = GetSafeComGoDocumentCount();
            }
            else
            {
                 count = int.Parse(_engine.ExecuteFunction("getDocumentCount"));
            }
            return count;
        }

        private int GetSafeComGoDocumentCount()
        {            
            char[] separator = { '"', ';' };

            string[] values = GetSafeComGoIds(separator);

            return values.Count();
        }

        /// <summary>
        /// Gets the HPAV version 15.4 or newer document ids.
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        private IEnumerable<string> GetSafeComGODocumentIds()
        {
            char[] separator = { '"', ';' };

            string[] values = GetSafeComGoIds(separator);

            for (int idx = 0; idx < values.Count(); idx++)
            {
                string name = _engine.ExecuteFunction("getDocumentNameById", values[idx]);

                name = name.Replace("\\n", "").Replace("\"", ""); ;

                values[idx] += "-" + name.Trim();
            }

            return values;
        }

        private string[] GetSafeComGoIds(char[] separator)
        {
            string ids = _engine.ExecuteFunction("getDocumentIds", "listitem").Trim();

            if (ids.Equals("-1"))
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

            string[] values = ids.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            if (values.Count() == 0)
            {
                ids = _engine.ExecuteFunction("getDocumentIds", "hp-listitem").Trim();
                values = ids.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            }

            return values;
        }

        /// <summary>
        /// Gets the document ids.
        /// </summary>
        /// <returns>
        /// IList&lt;System.String&gt;.
        /// </returns>
        public virtual IEnumerable<string> GetDocumentIds()
        {

            if (IsOmni() && _newVersion.Value)
            {
                return GetSafeComGODocumentIds();
            }
            else
            {
                string idList = _engine.ExecuteFunction("getDocumentIds", "checkbox");
                return idList.Split(';').Distinct();
            }
           
        }

        /// <summary>
        /// Gets the document name by identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns>System.String.</returns>
        public string GetDocumentNameById(string documentId) => _engine.ExecuteFunction("getDocumentNameById", documentId);
        /// <summary>
        /// Launches SafeCom with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public abstract void SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Gets the first document identifier.
        /// </summary>
        /// <returns>
        /// System.String.
        /// </returns>
        public string GetFirstDocumentId() => GetDocumentIds().First();

        /// <summary>
        /// Checks whether the device is currently printing via CP or oid
        /// </summary>
        /// <param name="checkBy"></param>
        /// <returns>bool</returns>
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
        /// Presses a button on the control panel.
        /// </summary>
        /// <param name="printButtonId">The id of the print button to press</param>
        private void PressButton(string printButtonId)
        {
            //OnMarkEvent("PrintButtonPress");
            _engine.PressElementById(printButtonId);
        }

        /// <summary>
        /// Checks the device job status by SNMP OID.
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckDeviceJobStatusByOid()
        {
            string deviceStatus = _snmp.Get("1.3.6.1.4.1.11.2.3.9.1.1.3.0");

            //ExecutionServices.SystemTrace.LogDebug("Device printing status via OID = {0}, (Status={1})".FormatWith(printingFinished, deviceStatus));
            if (string.IsNullOrEmpty(deviceStatus) || deviceStatus.StartsWith("ready", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks the device job status by control panel.
        /// </summary>
        /// <returns>bool</returns>
        protected abstract bool CheckDeviceJobStatusByControlPanel();

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
        /// <exception cref="NotImplementedException"></exception>
        public abstract bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopie">The number copies.</param>
        public abstract void SetCopyCount(int numCopie);

        /// <summary>
        /// processes the copy count.
        /// </summary>
        public void ProcessCopyCount(string copyArea = "inputCopies")
        {
            PressButton(copyArea);
        }
        /// <summary>
        /// Determines whether the SafeCom solution is the newest version.
        /// </summary>
        /// <returns>System.Boolean.</returns>
        public bool IsNewVersion() => _newVersion.Value;

        /// <summary>
        /// Determines whether this instance is an Omni device.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is Omni; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOmni() => _isOmni;

    }

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
}
