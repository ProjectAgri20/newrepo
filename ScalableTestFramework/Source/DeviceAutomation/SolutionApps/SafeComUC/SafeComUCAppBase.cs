using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeComUC
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowLogSource" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom.ISafeComApp" />
    public abstract class SafeComUCAppBase : DeviceWorkflowLogSource, ISafeComUCApp
    {
        private readonly Snmp _snmp;
        private readonly string[] _menuButtonId = { "btnMenu" };
        private readonly string[] _deleteButtonId = { "menuDelete" };
        private readonly string[] _printAllButtonId = { "menuPrintAll" };
        private readonly string[] _printButtonId = { "btnPrint" };
        private readonly string[] _retainButtonId = { "menuRetain" };
        private readonly string[] _unRetainButtonId = { "menuUnretain" };
        private readonly string[] _refreshButtonId = { "menuRefresh" };
        private readonly string[] _inputCopies = { "numOfCopy" };
        private readonly string[] _selectAll = { "checkboxSelectAll" };
        private readonly string[] _retainIconId = { "_retained" };

        /// <summary>
        /// Oxpd Browser Engine
        /// </summary>
        protected readonly OxpdBrowserEngine _engine;

        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        public const string SolutionButtonTitle = "Pull Print";

        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        public const string SolutionPrintAllButtonTitle = "Print All";

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComUCAppBase" /> class
        /// </summary>
        /// <param name="snmp">The SNMP.</param>
        /// <param name="controlPanel">The control panel.</param>
        protected SafeComUCAppBase(Snmp snmp, IJavaScriptExecutor controlPanel)
        {
            _snmp = snmp;
            _engine = new OxpdBrowserEngine(controlPanel, SafeComUCResource.SafeComUCJavaScript);
        }

        /// <summary>
        /// Device Inactivity Timeout setting.
        /// </summary>
        public TimeSpan DeviceInactivityTimeout
        {
            get; protected set;
        }

        /// <summary>
        /// Presses the Copy Count area.
        /// </summary>
        public void PressCopyCount()
        {
            if (_engine.WaitForHtmlContains(_inputCopies[0], TimeSpan.FromSeconds(3)))
            {
                PressButton(_inputCopies[0]);
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
            }
            else
            {
                throw new DeviceWorkflowException("Fail to find Copies textbox");
            }            
        }

        /// <summary>
        /// Presses the Menu button.
        /// </summary>
        public virtual void Menu()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            PressButton(_menuButtonId[0]);
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
        }

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        public virtual void Delete()
        {
            PressButton(_deleteButtonId[0]);            
        }

        /// <summary>
        /// Presses the Print All button.
        /// </summary>
        public virtual void PrintAll()
        {   
            PressButton(_printAllButtonId[0]);
        }

        /// <summary>
        /// Presses the Print button.
        /// </summary>
        public virtual void Print()
        {
            PressButton(_printButtonId[0]);
        }

        /// <summary>
        /// Presses the Retain button.
        /// </summary>
        public virtual void Retain()
        {
            PressButton(_retainButtonId[0]);         
        }

        /// <summary>
        /// Presses the Unretain button.
        /// </summary>
        public virtual void Unretain()
        {            
            PressButton(_unRetainButtonId[0]);
        }

        /// <summary>
        /// Presses the Refresh button.
        /// </summary>
        public virtual void Refresh()
        {
            PressButton(_refreshButtonId[0]);            
        }

        /// <summary>
        /// Wait for job list restored
        /// </summary>
        public virtual void WaitForJobList(int jobCount, int waitingSeconds)
        {   
            DateTime endTime = DateTime.Now.AddSeconds(waitingSeconds);
            int currentJobCount = int.Parse(_engine.ExecuteFunction("getDocumentCounts", "fyp_jobContentEventArea"));

            while (jobCount != currentJobCount)
            {
                if (DateTime.Now > endTime)
                {
                    throw new DeviceWorkflowException($"Job list is not restored - Job Count: Previous ({jobCount}), Current ({currentJobCount})");
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                currentJobCount = int.Parse(_engine.ExecuteFunction("getDocumentCounts", "fyp_jobContentEventArea"));
            }
        }

        /// <summary>
        /// Wait for Retain icon on the job
        /// </summary>
        public virtual bool WaitForRetainIcon(int waitingSeconds)
        {
            string retainIcon = GetFirstDocumentId() + _retainIconId[0];
            
            if(!_engine.WaitForHtmlContains(retainIcon, TimeSpan.FromSeconds(waitingSeconds)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Wait for disappear Retain icon
        /// </summary>
        public virtual bool WaitForRetainIconDisappeared(int waitingSeconds)
        {
            string retainIcon = GetFirstDocumentId() + _retainIconId[0];
            
            DateTime endTime = DateTime.Now.AddSeconds(waitingSeconds);

            while(endTime > DateTime.Now)
            {
                if (!_engine.HtmlContains(retainIcon))
                {
                    return true;
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
            }

            return false;
        }

        /// <summary>
        /// Selects all documents in the document list.
        /// </summary>
        public virtual void SelectAllDocuments()
        {
            PressButton(_selectAll[0]);
        }
        
        /// <summary>
        /// Selects the first document in the document list.
        /// </summary>
        public virtual void SelectFirstDocument()
        {
            _engine.ExecuteFunction("selectFirstDocument", "fyp_jobContentEventArea");
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
            count = int.Parse(_engine.ExecuteFunction("getDocumentCounts", "fyp_jobContentEventArea"));
            return count;
        }

        /// <summary>
        /// Gets the document ids.
        /// </summary>
        /// <returns>
        /// IList&lt;System.String&gt;.
        /// </returns>
        public IEnumerable<string> GetDocumentIds()
        {
            char[] separator = { '"', ';' };

            string ids = _engine.ExecuteFunction("getDocumentIds", "fyp_jobItem_tpl ellipsis jobRow").Trim();

            if (ids.Equals("-1"))
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

            string[] values = ids.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            return values;
        }

        /// <summary>
        /// Gets the document names.
        /// </summary>
        /// <returns>
        /// IList&lt;System.String&gt;.
        /// </returns>
        public IEnumerable<string> GetDocumentNames()
        {
            char[] separator = { '"', ';' };

            string names = _engine.ExecuteFunction("getDocumentNames", "fyp_jobTitle_tpl").Trim();

            if (names.Equals("-1"))
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

            string[] values = names.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            return values;
        }

        /// <summary>
        /// Checking first job checked
        /// </summary>
        public bool IsFirstJobCheckBoxChecked()
        {            
            string checkBoxId = GetFirstDocumentId() + "_checkbox";            
            return Boolean.Parse(_engine.ExecuteFunction("isCheckBoxChecked", checkBoxId).Trim());
        }

        /// <summary>
        /// Checking Select All checked
        /// </summary>
        public bool IsSelectAllCheckBoxChecked()
        {
            string checkBoxId = "checkboxSelectAll";
            return Boolean.Parse(_engine.ExecuteFunction("isCheckBoxChecked", checkBoxId).Trim());
        }

        /// <summary>
        /// Launches SafeComUC Pull Print app with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

        /// <summary>
        /// Launches SafeComUC Print All app with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public abstract void LaunchPrintAll(IAuthenticator authenticator, AuthenticationMode authenticationMode);

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
        /// Gets the first document name.
        /// </summary>
        /// <returns>
        /// System.String.
        /// </returns>
        public string GetFirstDocumentName() => GetDocumentNames().First();

        /// <summary>
        /// Presses a button on the control panel.
        /// </summary>
        /// <param name="printButtonId">The id of the print button to press</param>
        public void PressButton(string printButtonId)
        {
            _engine.PressElementById(printButtonId);
        }

        /// <summary>
        /// Checks to see if the processing of work has started, or timeout was reached.
        /// </summary>
        /// <param name="action">The pull print action</param>
        /// <param name="waitTime">Time to continue checking</param>
        /// <returns>bool</returns>
        public abstract bool StartedProcessingWork(SafeComUCPullPrintAction action, TimeSpan waitTime);

        /// <summary>
        /// Returns true when finished processing the current work, or timeout was reached.
        /// </summary>
        /// <param name="action">The pull print action</param>
        /// <returns>bool</returns>
        public abstract bool FinishedProcessingWork(SafeComUCPullPrintAction action);

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopie">The number copies.</param>
        public abstract void SetCopyCount(int numCopie);
    }
}
