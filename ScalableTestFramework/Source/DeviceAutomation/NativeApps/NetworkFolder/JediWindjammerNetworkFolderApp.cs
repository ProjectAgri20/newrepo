using System;
using System.Linq;
using System.Net;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder
{
    /// <summary>
    /// Implementation of <see cref="INetworkFolderApp" /> for a <see cref="JediWindjammerDevice" />.
    /// </summary>
    public sealed class JediWindjammerNetworkFolderApp : DeviceWorkflowLogSource, INetworkFolderApp
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly JediWindjammerNetworkFolderJobOptions _optionsManager;
        private readonly JediWindjammerJobExecutionManager _executionManager;

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
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerNetworkFolderApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerNetworkFolderApp(JediWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediWindjammerNetworkFolderJobOptions(device);
            _executionManager = new JediWindjammerJobExecutionManager(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Network Folder application on the device.
        /// </summary>
        public void Launch()
        {
            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "NetworkFolder");
                _controlPanel.ScrollPressNavigate("mAccessPointDisplay", "FolderApp", "FolderAppMainForm", ignorePopups: true);
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Pause();
            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Network Folder application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Network Folder application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "FolderAppMainForm":
                        // The application launched successfully. This happens sometimes.
                        RecordEvent(DeviceWorkflowMarker.AppShown);
                        Pacekeeper.Reset();
                        return;

                    case "SignInForm":
                        throw new DeviceWorkflowException("Sign-in required to launch the Network Folder application.", ex);

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Could not launch Network Folder application: {message}", ex);

                    default:
                        throw new DeviceWorkflowException($"Could not launch Network Folder application: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Launches Scan to Network Folder using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                if (PressScanToNetworkFolderButton("SignInForm"))
                {
                    Authenticate(authenticator, "FolderAppMainForm");
                }
            }
            else // AuthenticationMode.Eager
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
                PressScanToNetworkFolderButton("FolderAppMainForm");
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Presses the scan to network folder button.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DeviceWorkflowException">
        /// Network Folder application button was not found on device home screen.
        /// </exception>
        private bool PressScanToNetworkFolderButton(string expectedForm)
        {
            bool signInScreenLoaded = false;
            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "NetworkFolder");
                _controlPanel.ScrollPressNavigate("mAccessPointDisplay", "FolderApp", expectedForm, ignorePopups: true);
                signInScreenLoaded = _controlPanel.CurrentForm().Equals("SignInForm");

                Pacekeeper.Pause();
            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Network Folder application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Network Folder application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "FolderAppMainForm":
                        // The application launched without prompting for credentials.
                        Pacekeeper.Reset();
                        return false;

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Could not launch Network Folder application: {message}", ex);

                    default:
                        throw new DeviceWorkflowException($"Could not launch Network Folder application: {ex.Message}", ex);
                }
            }

            return signInScreenLoaded;
        }

        /// <summary>
        /// Authenticates the user for DSS.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">desired form after action</param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            try
            {
                authenticator.Authenticate();
                _controlPanel.WaitForForm(waitForm, StringMatch.Contains, TimeSpan.FromSeconds(30));
            }
            catch (WindjammerInvalidOperationException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                switch (currentForm)
                {
                    case "OxpUIAppMainForm":
                        // The application launched successfully. This happens sometimes.
                        RecordEvent(DeviceWorkflowMarker.AppShown);
                        break;
                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        if (message.StartsWith("Invalid", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new DeviceWorkflowException(string.Format("Could not launch application: {0}", message), ex);
                        }
                        else
                        {
                            throw new DeviceInvalidOperationException(string.Format("Could not launch application: {0}", message), ex);
                        }

                    default:
                        throw new DeviceInvalidOperationException(string.Format("Could not launch application: {0}", ex.Message), ex);
                }
            }
        }

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void EnterFileName(string fileName)
        {
            try
            {
                _controlPanel.PressToNavigate("mFileNameTextBox", "FolderKeyboardForm", ignorePopups: false);
                Pacekeeper.Pause();
                _controlPanel.TypeOnVirtualKeyboard("mKeyboard", fileName);
                Pacekeeper.Sync();
                _controlPanel.PressToNavigate("ok", "FolderAppMainForm", ignorePopups: false);
                Pacekeeper.Pause();
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "FolderAppMainForm":
                        if (_controlPanel.GetProperty<bool>("mFileNameTextBox", "Enabled") == false)
                        {
                            throw new DeviceWorkflowException("File name is not user-editable.", ex);
                        }
                        else
                        {
                            throw;
                        }

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Error entering file name: {message}", ex);

                    default:
                        throw;
                }
            }
        }

        /// <summary>
        /// Adds the specified network folder path as a destination for the scanned file.
        /// </summary>
        /// <param name="path">The network folder path.</param>
        /// <param name="credential">The network credential parameter is being added to provide the network credentials to access folder path </param>
        /// /// <param name="applyCredentials"><value><c>true</c> if apply credentials on verification is checked;otherwise, <c>false</c>.</value></param>
        public void AddFolderPath(string path, NetworkCredential credential, bool applyCredentials)//network credential for folder access is supported for Omni device. windjammer yet to be supported
        {
            try
            {
                _controlPanel.PressToNavigate("mAddPathButton", "FolderKeyboardForm", ignorePopups: false);
                Pacekeeper.Pause();
                _controlPanel.TypeOnVirtualKeyboard("mKeyboard", path);
                Pacekeeper.Sync();
                _controlPanel.PressToNavigate("ok", "FolderAppMainForm", ignorePopups: true);
                Pacekeeper.Pause();
            }
            catch (WindjammerInvalidOperationException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                switch (currentForm)
                {
                    case "SignInForm":
                        throw new DeviceWorkflowException("Cannot access the specified network folder without additional credentials.", ex);

                    case "ConflictDialog":
                        string message = _controlPanel.GetProperty("mDetailsBrowser", "PageContent");
                        throw new DeviceWorkflowException($"Error entering folder path: {message}", ex);

                    default:
                        throw;
                }
            }
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// </summary>
        /// <param name="quicksetName">The quickset name.</param>
        public void SelectQuickset(string quicksetName)
        {
            try
            {
                // If a folder is already selected, there will be a prompt when the quickset button is pushed
                if (string.IsNullOrEmpty(_controlPanel.GetProperty("mFolderPathsListBox_Item0", "Text")))
                {
                    _controlPanel.ScrollPress("mQuickSetsListBox", "Text", quicksetName);
                    Pacekeeper.Sync();
                }
                else
                {
                    _controlPanel.ScrollPressNavigate("mQuickSetsListBox", "Text", quicksetName, "TwoButtonMessageBox", ignorePopups: false);
                    Pacekeeper.Pause();
                    _controlPanel.PressToNavigate("m_OKButton", "FolderAppMainForm", ignorePopups: false);
                    Pacekeeper.Pause();
                }

                // If the quickset points to a "personal shared folder" there will probably be a "please wait" dialog
                if (_controlPanel.CurrentForm() == "HPProgressPopup")
                {
                    _controlPanel.WaitForForm("FolderAppMainForm", ignorePopups: true);
                }
            }
            catch (ControlNotFoundException ex)
            {
                if (_controlPanel.CurrentForm() == "FolderAppMainForm")
                {
                    throw new DeviceWorkflowException($"Could not find the quickset '{quicksetName}'.", ex);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the number of folder destinations currently selected.
        /// </summary>
        /// <returns>The number of folder destinations.</returns>
        public int GetDestinationCount()
        {
            int currentCount = 0;

            var folderListItems = _controlPanel.GetControls().Where(n => n.StartsWith("mFolderPathsListBox_Item", StringComparison.OrdinalIgnoreCase)).OrderBy(n => n);
            foreach (string folderListItem in folderListItems)
            {
                if (string.IsNullOrEmpty(_controlPanel.GetProperty(folderListItem, "Text")))
                {
                    break;
                }
                currentCount++;
            }

            return currentCount;
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            bool complete = _executionManager.ExecuteScanJob(executionOptions, "FolderAppMainForm");
            if (complete && _controlPanel.CurrentForm() == "BaseJobStartPopup")
            {
                _controlPanel.PressToNavigate("m_OKButton", "FolderAppMainForm", ignorePopups: true);
            }
            return complete;
        }

        /// <summary>
        /// Gets the <see cref="INetworkFolderJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public INetworkFolderJobOptions Options => _optionsManager;

        private class JediWindjammerNetworkFolderJobOptions : JediWindjammerJobOptionsManager, INetworkFolderJobOptions
        {
            public JediWindjammerNetworkFolderJobOptions(JediWindjammerDevice device)
                : base(device, "FolderAppMainForm")
            {
            }
        }
    }
}
