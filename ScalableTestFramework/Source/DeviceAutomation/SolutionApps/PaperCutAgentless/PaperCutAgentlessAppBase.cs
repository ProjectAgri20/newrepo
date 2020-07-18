using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HtmlAgilityPack;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCutAgentless
{
    /// <summary>
    /// PaperCutApp Base class.
    /// </summary>
    public abstract class PaperCutAgentlessAppBase : DeviceWorkflowLogSource, IPaperCutAgentlessApp
    {
        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;
        private readonly string[] _deleteButtonId = { "cancel-job-button" };
        private readonly string[] _printButtonId = { "release-job-button" };
        private readonly string[] _selectAllButtonId = { "page-title-select-job-details", "select-job-checkbox" };
        private readonly string[] _force2Sided = { "force-duplex-checkbox" };
        private readonly string[] _forceGrayscale = { "force-grayscale-checkbox" };
        private readonly string[] _copiesSingleJobOption = { "number-of-copies" };
        private readonly string[] _duplexSingleJobOption = { "duplex_ENABLED" };
        private readonly string[] _grayScaleSingleJobOption = { "color_GRAYSCALE" };

        /// <summary>
        /// Initializes a new instance of the <see cref="PaperCutAgentlessAppBase" /> class.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        protected PaperCutAgentlessAppBase(IJavaScriptExecutor controlPanel)
        {
            _engine = new OxpdBrowserEngine(controlPanel, PaperCutAgentlessResource.PaperCutAgentlessJavaScript);            
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

            int threshhold = 30;
            if (!StartedProcessingWork(TimeSpan.FromSeconds(threshhold)))
            {
                throw new DeviceWorkflowException($"PaperCut Print operation did not start printing documents within {threshhold} seconds.");
            }
        }

        /// <summary>
        /// Presses the Delete button.
        /// </summary>
        public void Delete()
        {
            PressButton(_deleteButtonId[0]);
        }

        /// <summary>
        /// Selects first documents.
        /// </summary>
        /// <param name="documentIds">The javascript values (dynamically generated) for toggling the checkbox selection.</param>        
        public virtual void SelectFirstDocument(Collection<string> documentIds)
        {
            PressButton(documentIds[0]);            
        }

        /// <summary>
        /// Selects all documents.
        /// </summary>        
        public virtual void SelectAllDocuments()
        {
            if (_engine.ExistElementId(_selectAllButtonId[0]))
            {
                PressButton(_selectAllButtonId[0]);
            }
            else if (_engine.ExistElementId(_selectAllButtonId[1]))
            {
                PressButton(_selectAllButtonId[1]);
            }
            else
            {
                throw new DeviceWorkflowException($"The select all ID has changed, these did not work, {_selectAllButtonId[0]} and {_selectAllButtonId[1]}.");
            }
        }

        /// <summary>
        /// Set Single Job Options.
        /// </summary>       
        public virtual void SetSingleJobOptions(string id, int copies, bool duplexMode, bool colorMode)
        {
            string idString = _engine.ExecuteFunction("getSingleJobOptionsIdbyDocumentIds", id).Trim('"', '$');

            if (idString.Equals("-1"))
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

            PressButton(idString);
            

            if(copies > 1)
            {
                SetSingleJobCopies(copies);
            }
            if (duplexMode)
            {
                SetSingleJobDuplexMode();
            }
            if (colorMode)
            {
                SetSingleJobGrayScale();
            }
        }

        /// <summary>
        /// Set copies on the Single Job Options.
        /// </summary>       
        public virtual void SetSingleJobCopies(int copies)
        {
            PressButton(_copiesSingleJobOption[0]);
            SetCopies(copies);            
        }

        /// <summary>
        /// Set duplex mode on the Single Job Options.
        /// </summary>       
        public virtual void SetSingleJobDuplexMode()
        {
            PressButton(_duplexSingleJobOption[0]);
        }

        /// <summary>
        /// Set color mode on the Single Job Options.
        /// </summary>       
        public virtual void SetSingleJobGrayScale()
        {            
            PressButton(_grayScaleSingleJobOption[0]);
        }

        /// <summary>
        /// Set the Force 2sided option on the print job list.
        /// </summary>
        public virtual void SetForce2sided()
        {
            PressButton(_force2Sided[0]);
        }

        /// <summary>
        /// Set the Force Grayscale option on the print job list.
        /// </summary>
        public virtual void SetForcegrayscale()
        {
            PressButton(_forceGrayscale[0]);
        }

        /// <summary>
        /// Gets a dictionary of document ids for all documents displayed.
        /// Key = DocumentId (format "job-button-DDD") where DDD is a digit value.
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
                result.Add(item, item);
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
        
        /// <summary>
        /// Presses a button on the control panel.
        /// </summary>
        /// <param name="buttonId">The id of the button to press</param>
        private void PressButton(string buttonId)
        {
            _engine.PressElementById(buttonId);            
        }

        /// <summary>
        /// Set the copies value to copies text box
        /// </summary>
        public abstract void SetCopies(int copies);

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
