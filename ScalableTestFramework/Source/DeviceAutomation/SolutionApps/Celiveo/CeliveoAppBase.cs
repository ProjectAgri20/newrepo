using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Celiveo
{
    /// <summary>
    /// CeliveoApp Base class.
    /// </summary>
    public abstract class CeliveoAppBase : DeviceWorkflowLogSource, ICeliveoApp
    {
        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;

        private readonly string[] _deleteButtonId = { "menuButton", "menuItemBUTTONDELETE", "buttonBUTTONDELETE" };
        private readonly string[] _printBWButtonId = { "buttonBUTTONPRINTBW" };
        private readonly string[] _printButtonId = { "buttonBUTTONPRINT" };
        private readonly string[] _selectAllButtonId = { "buttonBUTTONSELECT" };
        private readonly string[] _okButtonId = { "buttonBUTTONOK" };

        
        private string _currentVersion = "8.4";
        private readonly string[] _celiveoVersions = { "8.4", "8.5" };


        /// <summary>
        /// Initializes a new instance of the <see cref="CeliveoAppBase" /> class.
        /// </summary>
        /// <param name="controlPanel"></param>
        protected CeliveoAppBase(IJavaScriptExecutor controlPanel)
        {
            _engine = new OxpdBrowserEngine(controlPanel, CeliveoResource.CeliveoJavaScript);
        }

        /// <summary>
        /// Gets the solution button title.
        /// </summary>
        public const string SolutionButtonTitle = "My print jobs";

        /// <summary>
        /// Presses the Select All button
        /// </summary>
        public void SelectAll()
        {
            PressButton(_selectAllButtonId[0]);
        }

        /// <summary>
        /// Presses the Print button
        /// </summary>
        public void Print()
        {
            PressButton(_printButtonId[0]);

            int threshhold = 30;
            if (!StartedProcessingWork(TimeSpan.FromSeconds(threshhold)))
            {
                throw new DeviceWorkflowException($"Celiveo Print operation did not start printing documents within {threshhold} seconds.");
            }
        }

        /// <summary>
        /// Presses the Print B/W button
        /// </summary>
        public void PrintBW()
        {
            PressButton(_printBWButtonId[0]);

            int threshhold = 30;
            if (!StartedProcessingWork(TimeSpan.FromSeconds(threshhold)))
            {
                throw new DeviceWorkflowException($"Celiveo Print B/W operation did not start printing documents within {threshhold} seconds.");
            }
        }

        /// <summary>
        /// Presses the Delete button
        /// </summary>
        public void Delete()
        {
            if (_currentVersion.Equals(_celiveoVersions[0])) // 8.4
            {
                PressButton(_deleteButtonId[0]);
                PressButton(_deleteButtonId[1]);
            }
            else if (_currentVersion.Equals(_celiveoVersions[1])) // 8.5
            {
                PressButton(_deleteButtonId[2]);
            }
            else
            {
                throw new DeviceWorkflowException($"Celiveo version is unknown.");
            }
        }

        /// <summary>
        /// Verify delete job
        /// </summary>
        /// <returns></returns>
        public bool VerifyDeleteJob()
        {
            if(WaitObjectForAvailable("Job(s) deleted successfully", TimeSpan.FromSeconds(40)))
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Unselects all documents except the first.
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
        public int GetDocumentCount()
        {
            int docCount = -1;

            try
            {
                docCount = int.Parse(_engine.ExecuteFunction("getDocumentCount"));
            }
            catch (JavaScriptExecutionException jsee)
            {
                if (jsee.Message.Contains("Can't find variable: getDocumentCount"))
                {
                    Framework.Logger.LogDebug("Retrying JavaScript 'getDocumentCount' at " + DateTime.Now.ToString());
                    Thread.Sleep(3000);
                    docCount = int.Parse(_engine.ExecuteFunction("getDocumentCount"));
                }
                else
                {
                    throw;
                }
            }
            return docCount;
        }

        /// <summary>
        /// Get all documents Ids by JavaScript.
        /// The Ids separated by "$"
        /// </summary>
        /// <returns></returns>
        public List<string> GetDocumentIds()
        {
            List<string> result = new List<string>();

            char[] separator = { '$' };

            string idString = string.Empty;

            try
            {
                idString = _engine.ExecuteFunction("getDocumentIds").Trim('"', '$');
            }
            catch (JavaScriptExecutionException)
            {
                Framework.Logger.LogDebug("Retrying JavaScript 'getDocumentIds at " + DateTime.Now.ToString());
                Thread.Sleep(3000);
                idString = _engine.ExecuteFunction("getDocumentIds").Trim('"', '$');
            }
            if (idString.Equals("-1"))
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

            string[] items = idString.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in items)
            {
                result.Add(item);
            }
           
            return result;
        }
        /// <summary>
        /// Sets the current version as determined from the Celiveo server, 8.4 or 8.5.
        /// </summary>
        public void SetCurrentVersion()
        {
            if (bool.Parse(_engine.ExecuteFunction("ExistButtonId", _deleteButtonId[0])))
            {
                _currentVersion = _celiveoVersions[0];
            }
            else if (bool.Parse(_engine.ExecuteFunction("ExistButtonId", _deleteButtonId[2])))
            {
                _currentVersion = _celiveoVersions[1];
            }
        }
        /// <summary>
        /// Launches Celiveo with the specified authenticator using given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy</param>
        public abstract void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode);

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
        /// Checks the device job status by control panel.
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckDeviceJobStatusByControlPanel()
        {
            return true;
        }

        // TODO:How can I do it?
        private static string ParseDocumentId(string onClickString)
        {
            return string.Empty;
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
        public void DismissPostPrintOperation()
        {
            WaitObjectForAvailable("Selected print jobs have been released", TimeSpan.FromSeconds(40));
            PressButton(_okButtonId[0]);
        }

        /// <summary>
        /// Dismisses the popup that displays after a Print or Delete operation.
        /// </summary>
        public void DismissPostDeleteOperation()
        {            
            PressButton(_okButtonId[0]);
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
                
        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopie">The number copie.</param>
        public abstract void SetCopyCount(int numCopie);
    }
}
