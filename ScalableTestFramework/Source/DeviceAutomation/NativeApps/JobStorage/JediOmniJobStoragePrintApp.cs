using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage
{
    /// <summary>
    /// Implementation of <see cref="IJobStoragePrintApp" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniJobStoragePrintApp : DeviceWorkflowLogSource, IJobStoragePrintApp
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniJobStorageJobOptions _optionsManager;
        private readonly JediOmniJobExecutionManager _executionManager;
        private static readonly TimeSpan ScreenWaitTimeout = new TimeSpan(0, 0, 0, 5);
        private static readonly TimeSpan ShortScreenWaitTimeout = new TimeSpan(0, 0, 0, 2);
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;
        private readonly TimeSpan _idleTimeoutOffset;
        private readonly string[] _jobsListId = { "#hpid-select-job-to-print-button", "#hpid-print-job-storage-setting-checkall-button" };
        private int _fwVersionIndex = FirmwareVersion.Default;

        private Pacekeeper _pacekeeper;

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper
        {
            get
            {
                return _pacekeeper;
            }
            set
            {
                _pacekeeper = value;
                _optionsManager.Pacekeeper = _pacekeeper;
                _launchHelper.Pacekeeper = _pacekeeper;
            }
        }

        /// <summary>
        /// Constructor for JediOmin Device.
        /// </summary>
        /// <param name="device">JediOmni Device </param>
        public JediOmniJobStoragePrintApp(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediOmniJobStorageJobOptions(device);
            _executionManager = new JediOmniJobExecutionManager(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            _masthead = new JediOmniMasthead(_device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);

        }

        /// <summary>
        /// Launches the Print application on the device.
        /// </summary>
        public void Launch()
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = PressPrintFromJobStorageButton("#hpid-print-job-storage-app-screen");
            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                if (_controlPanel.CheckState("#hpid-signin-app-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the Print from Job Storage application.");
                }
                else
                {
                    throw new DeviceWorkflowException("Could not launch Print from Job Storage application.");
                }
            }

            _fwVersionIndex = GetFwVersionIndex();
        }

        /// <summary>
        /// Launches the Print application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = false;
            if (authenticationMode == AuthenticationMode.Lazy)
            {
                bool signInScreenLoaded = PressPrintFromJobStorageButton("#hpid-signin-body");
                if (signInScreenLoaded)
                {
                    appLoaded = Authenticate(authenticator, "#hpid-print-job-storage-app-screen");
                }
                else if (_controlPanel.CheckState("#hpid-print-job-storage-app-screen", OmniElementState.Exists))
                {
                    // The application launched without prompting for credentials
                    appLoaded = true;
                    Pacekeeper.Reset();
                }
            }
            else
            {
                switch (authenticator.Provider)
                {
                    case AuthenticationProvider.Card:
                    case AuthenticationProvider.Skip:
                        break;
                    default:
                        _launchHelper.PressSignInButton();
                        break;
                }

                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                appLoaded = PressPrintFromJobStorageButton("#hpid-print-job-storage-app-screen");
            }

            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                throw new DeviceWorkflowException("Could not launch Print From Job Storage application.");
            }

            _fwVersionIndex = GetFwVersionIndex();
        }

        private bool PressPrintFromJobStorageButton(string expectedDestination)
        {
            return _launchHelper.PressAppButton("Print From Job Storage", "#hpid-printJobStorage-homescreen-button", "#hpid-print-homescreen-button", expectedDestination);
        }

        private bool Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            bool formLoaded = _controlPanel.WaitForAvailable(waitForm);
            Pacekeeper.Pause();
            return formLoaded;
        }

        /// <summary>
        /// Selects the specified folder in the list when left empty it treats that as “Untitled”(Default folder).
        /// </summary> 
        /// <param name="folderName">Name of the folder where file exists.</param>
        public void SelectFolder(string folderName)
        {
            //Setting the Default folder to Untitled. 
            if (string.IsNullOrEmpty(folderName))
            {
                folderName = "Untitled";
            }

            //Have changed the lines of code for [CR ID 2893 ]-Bug fix for [STB 4.9.8.0]PrintFromJobStorage plugin fails to execute on 24.5(BR) FW
            if (_device.ControlPanel.GetValue("#hpid-print-job-storage-choose-file-button", "innerText", OmniPropertyType.Property).Contains(folderName))
            {
                _device.ControlPanel.PressWait("#hpid-print-job-storage-choose-file-button", "#hpid-print-job-storage-tree-view", ScreenWaitTimeout);
            }
            else
            {
                switch (_fwVersionIndex)
                {
                    case FirmwareVersion.Default:
                        if (_controlPanel.CheckState(".hp-listitem:contains(" + folderName + ")", OmniElementState.Exists))
                        {
                            string fileTypeItemSelector = ".hp-listitem-text:contains(" + folderName + ")";
                            int matches = _controlPanel.GetCount(fileTypeItemSelector);
                            SelectFolderName(folderName, fileTypeItemSelector, matches);
                            Pacekeeper.Sync();
                        }
                        else
                        {
                            throw new DeviceWorkflowException($"Could not find folder with text '{folderName}'.");
                        }
                        break;

                    case FirmwareVersion.V24_6_4:
                        if (_controlPanel.CheckState(".hp-checkbox-listitem:contains(" + folderName + ")", OmniElementState.Exists))
                        {
                            string fileTypeItemSelector = ".hp-listitem-text:contains(" + folderName + ")";
                            int matches = _controlPanel.GetCount(fileTypeItemSelector);
                            SelectFolderName(folderName, fileTypeItemSelector, matches);
                            Pacekeeper.Sync();
                        }
                        else
                        {
                            throw new DeviceWorkflowException($"Could not find folder with text '{folderName}'.");
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Clicks on the Choose button" />.
        /// </summary>
        private void SelectChoose()
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            if (_controlPanel.WaitForState("#hpid-print-job-storage-choose-file-button", OmniElementState.Exists, ScreenWaitTimeout))
            {
                _controlPanel.Press("#hpid-print-job-storage-choose-file-button");
            }
            else
            {
                throw new JobStorageDeleteJobExeception("Unable to click Choose button");
            }
        }

        /// <summary>
        /// Clicks on Delete button" />.
        /// </summary>
        private void SelectDelete()
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            if (_controlPanel.WaitForState("#hpid-delete-stored-job-button", OmniElementState.Enabled, ScreenWaitTimeout))
            {
                _controlPanel.Press("#hpid-delete-stored-job-button");
            }
            else
            {
                throw new JobStorageDeleteJobExeception("Unable to click on Delete button");
            }
        }

        /// <summary>
        /// Deletes the printed stored job" />.
        /// </summary>
        public void DeletePrintedJob()
        {
            SelectChoose();
            SelectDelete();
            _executionManager.WorkflowLogger = WorkflowLogger;
            if (_controlPanel.WaitForState("#hpid-button-delete", OmniElementState.Useable, ScreenWaitTimeout))
            {
              _controlPanel.Press("#hpid-button-delete");
            }
            else
            {
                throw new JobStorageDeleteJobExeception("Unable to find Delete jobs button");
            }
        }

        /// <summary>
        /// Starts the current job and runs it to completion" />.
        /// </summary>
        public void ExecutePrintJob()
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            if (_controlPanel.WaitForState("#hpid-button-print-job-storage-start", OmniElementState.Useable, TimeSpan.FromSeconds(5)))
            {
                _controlPanel.Press("#hpid-button-print-job-storage-start");
            }
            else
            {
                throw new DeviceWorkflowException("Print Button does not Exist");
            }

            ProcessingJob();
        }

        /// <summary>
        /// Selects first Job.
        /// </summary>
        /// <param name="pin">Pin for the file.</param>
        /// <param name="numberCopies">Number of copies to Print.</param>
        /// <param name="folderName">Folder containing the stored job.</param>
        public void SelectFirstJob(string pin, int numberCopies, string folderName)
        {
            bool pinEnabled;
            if (_device.ControlPanel.CheckState(".hp-list-item-file", OmniElementState.Exists))
            {

                if (_device.ControlPanel.CheckState(".hp-list-item-file:first .hp-listitem-lock-icon", OmniElementState.Exists) && string.IsNullOrEmpty(pin))
                {
                    throw new DeviceWorkflowException("PIN is required for the job");
                }

                switch (_fwVersionIndex)
                {
                    case FirmwareVersion.Default:
                        _device.ControlPanel.Press(".hp-list-item-file:first");
                        if (_device.ControlPanel.CheckState(_jobsListId[_fwVersionIndex], OmniElementState.Exists))
                        {
                            pinEnabled = _device.ControlPanel.PressWait(_jobsListId[_fwVersionIndex], "#hpid-pin-to-unlock-job-popup", ShortScreenWaitTimeout);
                            if (pinEnabled)
                            {
                                PinAuthentication(pin);
                            }
                        }
                        break;

                    case FirmwareVersion.V24_6_4:
                        string privateJobsButtonControl = (string.IsNullOrEmpty(folderName)) ? "#hpid-file-list-item-private-jobs" : "#hpid-file-list-item-private-jobs" + folderName.ToLower();

                        if (!string.IsNullOrEmpty(pin))
                        {
                            pinEnabled = _device.ControlPanel.PressWait(privateJobsButtonControl, "#hpid-pin-password-to-unlock-jobs-popup", ShortScreenWaitTimeout);
                            if (pinEnabled)
                            {
                                PinAuthentication(pin);
                            }
                        }

                        string elementId = $".hp-list-item-file";
                        string locked = "hp-locked";

                        List<string> controls = _controlPanel.GetIds(elementId, OmniIdCollectionType.Self).ToList();
                        foreach (string control in controls)
                        {
                            string controlId = "#" + control;
                            var value = _controlPanel.GetValue(controlId, "class", OmniPropertyType.Property).Trim();
                            if (!value.Contains(locked))
                            {
                                if (_device.ControlPanel.CheckState(controlId, OmniElementState.Useable))
                                {
                                    _device.ControlPanel.Press(controlId);
                                    break;
                                }
                            }
                        }
                        //Check for 4.3 inch control panel which hides the buttons that are normally visible on 8 inch control panels.
                        //The "Job Storage Copies" and "Job Storage Start" buttons are hidden in the Job Selection screen on 4.3 inch control panels, and are visible only after clicking the 'Done' button
                        if (_device.ControlPanel.CheckState("#hpid-print-job-storage-setting-done-button", OmniElementState.Useable))
                        {
                            _device.ControlPanel.PressWait("#hpid-print-job-storage-setting-done-button", "#hpid-print-job-storage-start-copies");
                        }
                        break;
                }

                if (numberCopies > 0)
                {
                    _controlPanel.PressWait("#hpid-print-job-storage-start-copies", "#hpid-keypad");
                    _controlPanel.TypeOnNumericKeypad(numberCopies.ToString());
                    _controlPanel.Press("#hpid-keypad-key-close");
                }
            }
            else
            {
                throw new DeviceWorkflowException("There is no file to print");
            }
        }

        /// <summary>
        /// Selects all Jobs with Password in the list.
        /// </summary>
        /// <param name="pin">Pin for the file.</param>
        /// <param name="folderName">Folder containing the stored job.</param>
        public bool SelectAllJobs(string pin, string folderName)
        {
            switch (_fwVersionIndex)
            {
                case FirmwareVersion.Default:
                    bool allJobsSelected = SelectAllJobs();
                    if (allJobsSelected)
                    {
                        ExecutePrintJob();
                        SelectFolder(folderName);
                    }
                    if (!string.IsNullOrEmpty(pin))
                    {
                        if (_device.ControlPanel.WaitForState(".hp-list-item-allfileswithpassword", OmniElementState.Useable, ShortScreenWaitTimeout))
                        {
                            _device.ControlPanel.Press(".hp-list-item-allfileswithpassword");

                            _device.ControlPanel.PressWait(_jobsListId[_fwVersionIndex], "#hpid-pin-password-to-unlock-jobs-popup", ShortScreenWaitTimeout);
                            PinAuthentication(pin);
                            return true;
                        }
                        else
                        {
                            if (_device.ControlPanel.CheckState(".hp-list-item-file", OmniElementState.Exists))
                            {
                                if (_device.ControlPanel.CheckState(".hp-locked", OmniElementState.Exists))
                                {
                                    string elementId = ".hp-locked";
                                    _device.ControlPanel.Press(elementId);
                                    _device.ControlPanel.PressWait(_jobsListId[_fwVersionIndex], "#hpid-pin-to-unlock-job-popup", ShortScreenWaitTimeout);
                                    PinAuthentication(pin);
                                    return true;
                                }
                            }
                            else
                            {
                                throw new DeviceWorkflowException("There is no file to print");
                            }

                            return false;
                        }
                    }
                    break;

                case FirmwareVersion.V24_6_4:
                    string privateJobsButtonControl = (string.IsNullOrEmpty(folderName)) ? "#hpid-file-list-item-private-jobs" : "#hpid-file-list-item-private-jobs" + folderName.ToLower();

                    if (_device.ControlPanel.CheckState(privateJobsButtonControl, OmniElementState.Exists) && !string.IsNullOrEmpty(pin))
                    {
                        _device.ControlPanel.PressWait(privateJobsButtonControl, "#hpid-pin-password-to-unlock-jobs-popup", ShortScreenWaitTimeout);
                        PinAuthentication(pin);
                    }
                    if (_device.ControlPanel.WaitForState(_jobsListId[_fwVersionIndex], OmniElementState.Useable, ShortScreenWaitTimeout))
                    {
                        _device.ControlPanel.Press(_jobsListId[_fwVersionIndex]);
                    }
                    //Check for 4.3 inch control panel which hides the buttons that are normally visible on 8 inch control panels.
                    //The "Job Storage Copies" and "Job Storage Start buttons" are hidden in the Job Selection screen on 4.3 inch control panels, and are visible only after clicking the 'Done' button
                    if (_device.ControlPanel.CheckState("#hpid-print-job-storage-setting-done-button", OmniElementState.Useable))
                    {
                        _device.ControlPanel.Press("#hpid-print-job-storage-setting-done-button");
                        return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Selects all Jobs without Password in the list.
        /// </summary> 
        public bool SelectAllJobs()
        {
            if (_device.ControlPanel.WaitForState(".hp-list-item-allfilesnopassword", OmniElementState.Useable, ShortScreenWaitTimeout))
            {
                _device.ControlPanel.Press(".hp-list-item-allfilesnopassword");
                _device.ControlPanel.Press(_jobsListId[_fwVersionIndex]);
                return true;
            }
            else
            {
                string elementId = $".hp-list-item-file";
                string locked = "hp-locked";

                List<string> controls = _controlPanel.GetIds(elementId, OmniIdCollectionType.Self).ToList();
                foreach (string control in controls)
                {
                    string controlId = "#" + control;
                    var value = _controlPanel.GetValue(controlId, "class", OmniPropertyType.Property).Trim();
                    if (!value.Contains(locked))
                    {
                        if (_device.ControlPanel.CheckState(controlId, OmniElementState.Useable))
                        {
                            _device.ControlPanel.Press(controlId);
                            _device.ControlPanel.Press(_jobsListId[_fwVersionIndex]);
                            return true;
                        }
                        else
                        {
                            throw new DeviceWorkflowException("ControlId is not usable");
                        }
                    }
                }

                return false;
            }
        }

        private void PinAuthentication(string pin)
        {
            string textboxId;
            string okButtonId;

            if (_controlPanel.CheckState("#hpid-pin-to-unlock-job-popup", OmniElementState.Exists))
            {
                textboxId = "#hpid-pin-to-unlock-job-textbox";
                okButtonId = "#hpid-pin-to-unlock-job-ok";
            }
            else
            {
                textboxId = "#hpid-pin-password-to-unlock-jobs-textbox";
                okButtonId = "#hpid-pin-password-to-unlock-job-ok";
            }
            _device.ControlPanel.PressWait(textboxId, "#hpid-keyboard", TimeSpan.FromSeconds(2));
            _device.ControlPanel.TypeOnVirtualKeyboard(pin);

            //Check for 4.3 inch control panel where the keypad/keyboard hides the 'OK' button after entering the Pin/Password.
            //Close the numeric keypad after PIN is entered
            if (_device.ControlPanel.CheckState("#hpid-keypad-key-close", OmniElementState.Exists))
            {
                _device.ControlPanel.Press("#hpid-keypad-key-close");
            }
            //Close the virtual keyboard after PIN/Password is entered
            else if (_device.ControlPanel.CheckState("#hpid-keyboard-key-done", OmniElementState.Exists))
            {
                _device.ControlPanel.Press("#hpid-keyboard-key-done");
            }
            _device.ControlPanel.PressWait(okButtonId, ".hp-popup-modal-overlay", ShortScreenWaitTimeout);

            if (_device.ControlPanel.CheckState(".hp-popup-modal-overlay", OmniElementState.Exists))
            {
                throw new DeviceWorkflowException("Invalid Pin/Pin Not Entered");
            }
        }

        /// <summary>
        /// Checks to see if the job is still being processed. The icon, display = true, is present while processing.
        /// </summary>       
        public void ProcessingJob()
        {
            _masthead.WaitForActiveJobsButtonState(true, TimeSpan.FromSeconds(6));
            if (_device.ControlPanel.CheckState(".hp-button-active-jobs:last", OmniElementState.Exists))
            {
                _device.ControlPanel.WaitForState(".hp-button-active-jobs:last", OmniElementState.Useable, true);
                string value = _controlPanel.GetValue(".hp-button-active-jobs:last", "outerHTML", OmniPropertyType.Property);
                while (!value.Contains("display: none"))
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(250));
                    value = _controlPanel.GetValue(".hp-button-active-jobs:last", "outerHTML", OmniPropertyType.Property);
                }
            }
            _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);
        }

        /// <summary>
        /// Matches the Folder Name and presses the list item
        /// </summary>
        /// <param name="folderName">The Folder Name</param>
        /// <param name="listItemSelector">List of Items related to the passed Folder Name</param>
        /// <param name="matches">Count of matched similar Folders</param>
        private void SelectFolderName(string folderName, string listItemSelector, int matches)
        {
            for (int i = 0; i < matches; i++)
            {
                string specificListItem = $"{listItemSelector}:eq({i})";
                if (folderName.Equals(_controlPanel.GetValue(specificListItem, "innerText", OmniPropertyType.Property)))
                {
                    _controlPanel.Press(specificListItem);
                }

            }
        }

        private int GetFwVersionIndex()
        {
            for (int i = FirmwareVersion.Default; i < _jobsListId.Length; i++)
            {
                if (_device.ControlPanel.WaitForState(_jobsListId[i], OmniElementState.Exists, TimeSpan.FromSeconds(2)))
                {
                    return i;
                }
            }
            //The version could not be determined.
            return FirmwareVersion.Default;
        }

        /// <summary>
        /// Gets the <see cref="IJobStorageJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IJobStorageJobOptions Options => _optionsManager;

        private class JediOmniJobStorageJobOptions : JediOmniJobOptionsManager, IJobStorageJobOptions
        {
            public JediOmniJobStorageJobOptions(JediOmniDevice device)
                : base(device)
            {
            }
        }
    }
}
