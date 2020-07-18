using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HtmlAgilityPack;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpac
{
    /// <summary>
    /// HpacApp Base class.
    /// </summary>
    public abstract class HpacAppBase : DeviceWorkflowLogSource, IHpacApp
    {
        private readonly Snmp _snmp;
        private readonly OxpdBrowserEngine _engine;

        private readonly string[] _deleteButtonId = { "deleteRefreshButton", "ButtonDelete", "b_delRef" };
        private readonly string[] _printAllButtonId = { "printAllQueueButton", "ButtonPrintAll", "b_prAll" };
        private readonly string[] _printDeleteButtonId = { "printButton", "ButtonSelect", "b_pr" };
        private readonly string[] _printKeepButtonId = { "printKeepButton", "ButtonRetain", "b_prKp" };
        private readonly string[] _refreshButtonId = { "deleteRefreshButton", "ButtonDelete", "b_delRef" };  // Same as delete - the same button is used with the text changed

        private const int Version15_4 = 0;
        private const int VersionOld = 1;
        private const int Version16_2 = 2;

        private Lazy<int> _currentVersion;


        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        public const string SolutionButtonTitle = "HP AC Secure Pull Print";

        /// <summary>
        /// Initializes a new instance of the <see cref="HpacAppBase" /> class.
        /// </summary>
        /// <param name="snmp">The SNMP.</param>_
        /// <param name="controlPanel">The control panel.</param>
        protected HpacAppBase(Snmp snmp, IJavaScriptExecutor controlPanel)
        {
            _snmp = snmp;
            _engine = new OxpdBrowserEngine(controlPanel, HpacResource.HpacJavaScript);

            _currentVersion = new Lazy<int>(IsHpac15_4orNewer);

        }
        private int IsHpac15_4orNewer()
        {
            int currentVersion = 0;
            for (currentVersion = 0; currentVersion <= Version16_2; currentVersion++)
            {
                if (CheckHpacVersion(currentVersion))
                {
                    break;
                }
            }

            return currentVersion;
        }

        private bool CheckHpacVersion(int versionIndex)
        {
            var isNew = _engine.ExecuteFunction("ExistButtonId", _refreshButtonId[versionIndex]);
            bool version = bool.Parse(isNew.ToString());
            return version;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is windjammer.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is windjammer; otherwise, <c>false</c>.
        /// </value>
        protected bool IsWindJammer { get; set; }

        /// <summary>
        /// Presses the PrintAll button.
        /// </summary>
        public void PrintAll()
        {
            if (_currentVersion.IsValueCreated && _currentVersion.Value <= Version16_2)
            {
                PressButton(_printAllButtonId[_currentVersion.Value]);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to determine HPAC Server Version.");
            }
            if (!StartedProcessingWork(TimeSpan.FromSeconds(30)))
            {
                throw new DeviceWorkflowException("HPAC PrintAll did not start printing documents within 30 seconds.");
            }
        }

        /// <summary>
        /// Presses the Print-Delete button.
        /// </summary>
        public void PrintDelete()
        {
            if (_currentVersion.IsValueCreated && _currentVersion.Value <= Version16_2)
            {
                PressButton(_printDeleteButtonId[_currentVersion.Value]);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to determine HPAC Server Version.");
            }
            if (!StartedProcessingWork(TimeSpan.FromSeconds(30)))
            {
                throw new DeviceWorkflowException("HPAC PrintAll did not start printing documents within 30 seconds.");
            }
        }

        /// <summary>
        /// Presses the Print-Keep button.
        /// </summary>
        public void PrintKeep()
        {
            if (_currentVersion.IsValueCreated && _currentVersion.Value <= Version16_2)
            {
                PressButton(_printKeepButtonId[_currentVersion.Value]);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to determine HPAC Server Version.");
            }
            if (!StartedProcessingWork(TimeSpan.FromSeconds(30)))
            {
                throw new DeviceWorkflowException("HPAC PrintAll did not start printing documents within 30 seconds.");
            }
        }

        /// <summary>
        /// Presses the Refresh button.
        /// </summary>
        public void Refresh()
        {
            if (_currentVersion.IsValueCreated && _currentVersion.Value <= Version16_2)
            {
                PressButton(_refreshButtonId[_currentVersion.Value]);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to determine HPAC Server Version.");
            }
            if (!StartedProcessingWork(TimeSpan.FromSeconds(30)))
            {
                throw new DeviceWorkflowException("HPAC PrintAll did not start printing documents within 30 seconds.");
            }
        }

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        public void Delete()
        {
            if (_currentVersion.IsValueCreated && _currentVersion.Value <= Version16_2)
            {
                PressButton(_deleteButtonId[_currentVersion.Value]);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to determine HPAC Server Version.");
            }
        }

        /// <summary>
        /// Selects the first document in the document list.
        /// </summary>
        public void SelectFirstDocument(string checkboxId, string onchangeValue)
        {
            string docInfo = string.Empty;

            docInfo = "function selectDocument(){var elements = document.getElementsByTagName(\"input\"); for(var i = 0; i < elements.length; i++){ if(elements[i].type == \"checkbox\"){if(elements[i].id == \"" +
                checkboxId + "\"){elements[i].checked=\"checked\";" +
                onchangeValue + "} } } }selectDocument();";
            _engine.ExecuteJavaScript(docInfo);
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
        /// Selects all documents in the document list.
        /// </summary>
        public void SelectAllDocuments(string onchangeValue)
        {
            string documents = "function selectAllDocuments()" +
                   "{" +
                   "var elements = document.getElementsByTagName(\"input\");" +
                   "for (var i = 0; i < elements.length; i++)" +
                   "{" +
                   "if (elements[i].type == \"checkbox\")" +
                   "{" +
                        "elements[i].checked = \"checked\";" +
                        onchangeValue +
                   "}" +
                   "}" +
                   "}" +
                   "selectAllDocuments(); ";

            _engine.ExecuteJavaScript(documents);
        }

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetDocumentCount()
        {           
            int count = 0;
            if (_currentVersion.Value == 0 || _currentVersion.Value == 2)
            {
                count = GetHpacDocumentIds().Count();
            }
            else
            {
                count = int.Parse(_engine.ExecuteFunction("getDocumentCount"));
            }
            return count;
        }
        /// <summary>
        /// Gets the HPAV version 15.4 or newer document ids.
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        private IEnumerable<string> GetHpacDocumentIds()
        {
            string className = (IsWindJammer == false) ? "hp-listitem" : "checkbox";
            char[] separator = { '"', ';' };
            string ids = _engine.ExecuteFunction("getDocumentIds", className).Trim();
            string[] values = ids.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            return values;
        }

        /// <summary>
        /// Selects the given document ID.
        /// </summary>
        public void SelectFirstDocument(string documentValue)
        {
            _engine.PressElementById(documentValue);
        }

        /// <summary>
        /// Determines whether the HPAC solution is the newest version.
        /// </summary>
        /// <returns>System.Boolean.</returns>
        public bool IsNewVersion() => _currentVersion.Value != 1;

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
        /// Launches HPAC with the specified authenticator using the given authentication mode
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
        /// Gets the first document identifier found.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetFirstDocumentId() => GetDocumentIds().FirstOrDefault();

        /// <summary>
        /// Gets the list of document ids currently displayed
        /// The expectation is that each document/job name coming from STF will contain at least a partial GUID.  
        /// This function parses looking for this pattern and extracting the GUIDs.
        /// </summary>
        /// <returns>List of document ids</returns>
        public IEnumerable<string> GetDocumentIds()
        {
            IEnumerable<string> result = new List<string>();

            if (_currentVersion.Value == 0 || _currentVersion.Value == 2)
            {
                result = GetHpacDocumentIds();
            }
            else
            {
                result = GetDocumentIdsOldStyle();
            }

            return result;
        }

        private IEnumerable<string> GetDocumentIdsOldStyle()
        {
            var result = new List<string>();

            string docListXpath = "//table[@id='scrollingContent']//div[@class='labelOXPd']";
            string rawHtml = string.Empty;
            rawHtml = _engine.GetBrowserHtml();
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(rawHtml);

            var nodes = doc.DocumentNode.SelectNodes(docListXpath);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var id = GetDocumentIdFromNode(node);
                    result.Add(id);
                }
            }

            return result.Distinct();
        }

        /// <summary>
        /// Gets the checkbox on change values.
        /// </summary>
        /// <returns></returns>
        private List<HpacInput> GetCheckboxOnChangeValues()
        {
            List<HpacInput> onchangeValues = new List<HpacInput>();
            string docListXpath = "//input[@type='checkbox']";
            string rawHtml = string.Empty;

            rawHtml = _engine.GetBrowserHtml();
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(rawHtml);
            var nodes = doc.DocumentNode.SelectNodes(docListXpath);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    HpacInput hi = new HpacInput();
                    foreach (var attribute in node.Attributes)
                    {
                        hi.CheckboxId = node.Id;
                        if (attribute.Name.Equals("onchange"))
                        {
                            hi.OnchangeValue = attribute.Value;
                            onchangeValues.Add(hi);
                        }
                    }
                }
            }
            return onchangeValues;
        }

        /// <summary>
        /// Gets the checkbox ID for further processing
        /// </summary>
        /// <returns>HpacInput</returns>
        public HpacInput GetCheckboxOnchangeValue()
        {
            HpacInput checkboxId = new HpacInput();
            List<HpacInput> checkboxIds = GetCheckboxOnChangeValues();
            if (checkboxIds.Count > 0)
            {
                checkboxId = checkboxIds[0];
            }

            return checkboxId;
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

        /// <summary>
        /// Presses a button on the control panel.
        /// </summary>
        /// <param name="buttonId">The id of the print button to press</param>
        private void PressButton(string buttonId)
        {
            _engine.PressElementById(buttonId);
            
        }

        /// <summary>
        /// Gets the document identifier from html node.  Looks for Guid (or portion thereof) but falls back to inner text if no discernible guid found
        /// </summary>
        /// <param name="node">The html node.</param>
        /// <returns>System.String.</returns>
        private static string GetDocumentIdFromNode(HtmlNode node)
        {
            string result = string.Empty;

            result = GetDocumentGuidFromText(node.InnerText, true).LastOrDefault();

            // if we couldn't find a GUID then let's go with the document name in the button text
            if (string.IsNullOrEmpty(result) && node != null)
            {
                result = node.InnerText.Replace("...", "");
            }
            return result;
        }

        /// <summary>
        /// Gets the document guids.
        /// </summary>
        /// <param name="textToSearch">The text to search.</param>
        /// <param name="distinctOnly">if set to <c>true</c> [distinct only].</param>
        /// <returns>List{System.String}.</returns>
        private static List<string> GetDocumentGuidFromText(string textToSearch, bool distinctOnly = true)
        {
            var result = new List<string>();
            // look for GUIDs bracketed by underscores (or partial GUIDs of at least <8 char>-<4 char>)
            var matches = Regex.Matches(textToSearch, @"(?<guid>[A-F0-9]{8}(?:-[A-F0-9]{4}){1,3}(?:-[A-F0-9]{0,12})?)", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                result.Add(match.Groups["guid"].Value);
            }

            if (distinctOnly)
            {
                result = result.Distinct().ToList();
            }
            return result;
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
    /// Class utilized in a container object for matching the checkbox ID with its JavaScript
    /// label on change event.
    /// </summary>
    public class HpacInput
    {
        /// <summary>
        /// Checkbox id as a string
        /// </summary>
        public string CheckboxId { get; set; }

        /// <summary>
        /// Value that was in the Hpac input box
        /// </summary>
        public string OnchangeValue { get; set; }

        /// <summary>
        /// Defaults value of Checkbox and OnchangeValue to empty strings
        /// </summary>
        public HpacInput()
        {
            CheckboxId = string.Empty;
            OnchangeValue = string.Empty;
        }
    }
}
