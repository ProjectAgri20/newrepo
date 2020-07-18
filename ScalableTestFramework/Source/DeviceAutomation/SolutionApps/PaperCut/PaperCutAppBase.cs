using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HtmlAgilityPack;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCut
{
    /// <summary>
    /// PaperCutApp Base class.
    /// </summary>
    public abstract class PaperCutAppBase : DeviceWorkflowLogSource, IPaperCutApp
    {
        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;
        private readonly string[] _deleteButtonId = { "cancel-all-button" };
        private readonly string[] _printButtonId = { "print-all-button" };

        /// <summary>
        /// Initializes a new instance of the <see cref="PaperCutAppBase" /> class.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        protected PaperCutAppBase(IJavaScriptExecutor controlPanel)
        {
            _engine = new OxpdBrowserEngine(controlPanel, PaperCutResource.PaperCutJavaScript);            
        }

        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        public const string SolutionButtonTitle = "Print Release";

        /// <summary>
        /// Presses the Print button.
        /// </summary>
        public void Print()
        {
            PressButton(_printButtonId[0]);

            string exist = _engine.ExecuteFunction("ExistButtonId", "message-ok-button");
            if (exist.Equals("true"))
            {
                DismissPostOperationPopup();
            }
            else
            {
                int threshhold = 30;
                if (!StartedProcessingWork(TimeSpan.FromSeconds(threshhold)))
                {
                    throw new DeviceWorkflowException($"PaperCut Print operation did not start printing documents within {threshhold} seconds.");
                }
            }
        }

        /// <summary>
        /// Exists the print popup message.
        /// </summary>
        /// <returns></returns>
        public bool ExistPrintPopupMessage()
        {
            bool exist = false;

            if (_engine.ExecuteFunction("ExistButtonId", "message-ok-button").Equals("true"))
            {
                exist = true;
            }

            return exist;
        }

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        public void Delete()
        {
            PressButton(_deleteButtonId[0]);
        }

        /// <summary>
        /// Unselects all documents except the first.
        /// </summary>
        /// <param name="onClickValues">The javascript values (dynamically generated) for toggling the checkbox selection.</param>
        public virtual void SelectFirstDocument(Collection<string> onClickValues)
        {
            for (int i = 1; i < onClickValues.Count; i++)
            {
                _engine.ExecuteJavaScript(onClickValues[i]);               
            }
        }

        /// <summary>
        /// Gets a dictionary of document ids for all documents displayed.
        /// Key = DocumentId (format "job-list-item-DDD") where DDD is a digit value.
        /// Value = A javascript string that toggles the checkbox selection of the document.
        /// The javascript has to be used because if the document list scrolls off the display area,
        /// engine.PressElementById doesn't work.  Calling the javascript on a non-displayed documentId
        /// still toggles the "Checked" property of the checkbox.
        /// </summary>
        /// <returns>Dictionary of document ids with respective javascript strings</returns>
        public Dictionary<string, string> GetDocumentIds()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            char[] separator = {'$'};

            string idString = _engine.ExecuteFunction("getDocumentIds").Trim('"', '$');

            if (idString.Equals("-1"))
            {
                throw new DeviceWorkflowException("Error executing JavaScript getDocumentIds.");
            }


            string[] items = idString.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in items)
            {
                result.Add(ParseDocumentId(item), item);
            }

            return result;
        }

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>Document count</returns>
        public int GetDocumentCount()
        {
            return int.Parse(_engine.ExecuteFunction("getDocumentCount"));
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
        /// Checks the device job status by control panel.
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckDeviceJobStatusByControlPanel()
        {
            return true;
        }

        private static string ParseDocumentId(string onClickString)
        {
            MatchCollection matches = Regex.Matches(onClickString, @"job-list-item-\d+", RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                return matches[0].Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Presses a button on the control panel.
        /// </summary>
        /// <param name="buttonId">The id of the button to press</param>
        private void PressButton(string buttonId)
        {
            _engine.PressElementById(buttonId);
        }

        /// <summary>
        /// Dismisses the popup that displays after a Print or Delete operation.
        /// </summary>
        public void DismissPostOperationPopup()
        {
            PressButton("message-ok-button");
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
