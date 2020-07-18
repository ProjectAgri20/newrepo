using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HtmlAgilityPack;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Equitrac
{
    /// <summary>
    /// Base class for the Equitrac solution
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowLogSource" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.Equitrac.IEquitracApp" />
    public abstract class EquitracAppBase : DeviceWorkflowLogSource, IEquitracApp
    {
        private readonly OxpdBrowserEngine _engine;

        private const string _deleteButtonId = "deletebtn";
        private const string _printSaveButtonId = "printkeepbtn";
        private const string _printButtonId = "printbtn";
        private const string _refreshButtonId = "refreshbtn";

        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        protected const string SolutionButtonTitle = "Follow-You Printing";

        /// <summary>
        /// Initializes a new instance of the <see cref="EquitracAppBase"/> class.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        protected EquitracAppBase(IJavaScriptExecutor controlPanel)
        {
            _engine = new OxpdBrowserEngine(controlPanel, EquitracResource.EquitracJavaScript);
        }

        /// <summary>
        /// Gets the list of document ids currently displayed
        /// The expectation is that each document/job name coming from STF will contain at least a partial GUID.  
        /// This function parses looking for this pattern and extracting the GUIDs.
        /// </summary>
        /// <returns>List of document ids</returns>
        public IEnumerable<string> GetDocumentIds()
        {
            string docListXpath = "//table[@id='scrollingContent']//div[@class='labelOXPd']";
            var result = new List<string>();
            string rawHtml = string.Empty;

            rawHtml = _engine.GetBrowserHtml();
            var doc = new HtmlDocument();
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
        /// Gets the first document identifier.
        /// </summary>
        /// <returns>
        /// System.String.
        /// </returns>
        public string GetFirstDocumentId() => GetDocumentIds().FirstOrDefault();

        /// <summary>
        /// Equitrac Delete button.
        /// </summary>
        public void Delete()
        {
            PressButton(_deleteButtonId);
        }

        /// <summary>
        /// Equitrac print button
        /// </summary>
        public void Print()
        {
            PressButton(_printButtonId);
        }

        /// <summary>
        /// Equitrac Print and Save button.
        /// </summary>
        public void PrintSave()
        {
            PressButton(_printSaveButtonId);
        }

        /// <summary>
        /// Refreshes the Equitrac screen
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Refresh()
        {
            PressButton(_refreshButtonId);
        }

        /// <summary>
        /// Selects all the documents.
        /// </summary>
        public void SelectAll()
        {
            _engine.ExecuteFunction("selectAllDocuments");
        }

        /// <summary>
        /// Selects the first document in the document list.
        /// </summary>
        public void SelectFirstDocument()
        {
            _engine.ExecuteFunction("selectFirstDocument");
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
        /// Retrieves the current document/job count.
        /// </summary>
        /// <returns>
        /// int
        /// </returns>
        public int GetDocumentCount()
        {
            int jobCount = 0;

            Wait.ForTrue(() =>
            {
                string value = _engine.ExecuteFunction("getDocumentCount");
                int.TryParse(value, out jobCount);

                return jobCount > 0;
            }

            , TimeSpan.FromSeconds(10));

            return jobCount;
        }

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public abstract bool FinishedProcessingWork();

        /// <summary>
        /// Launches Equitrac with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public abstract bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopie">The number copie.</param>
        public abstract void SetCopyCount(int numCopie);

        /// <summary>
        /// processes the copy count.
        /// </summary>
        public void ProcessCopyCount()
        {
            PressButton("txtnumcopies");
        }

    }
}
