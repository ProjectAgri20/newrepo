using System;
using System.Net;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder
{
    /// <summary>
    /// Implementation of <see cref="INetworkFolderApp" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniNetworkFolderApp : DeviceWorkflowLogSource, INetworkFolderApp
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniNetworkFolderJobOptions _optionsManager;
        private readonly JediOmniJobExecutionManager _executionManager;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniPopupManager _popupManager;

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
        /// Initializes a new instance of the <see cref="JediOmniNetworkFolderApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniNetworkFolderApp(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediOmniNetworkFolderJobOptions(device);
            _executionManager = new JediOmniJobExecutionManager(device);
            _popupManager = new JediOmniPopupManager(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Network Folder application on the device.
        /// </summary>
        public void Launch()
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = PressScanToNetworkFolderButton("#hpid-network-folder-landing-page");
            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                if (_controlPanel.CheckState("#hpid-signin-app-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the Network Folder application.");
                }
                else
                {
                    throw new DeviceWorkflowException("Could not launch Network Folder application.");
                }
            }
        }

        /// <summary>
        /// Launches Scan to Network Folder using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = false;
            if (authenticationMode == AuthenticationMode.Lazy)
            {
                bool signInScreenLoaded = PressScanToNetworkFolderButton("#hpid-signin-body");
                if (signInScreenLoaded)
                {
                    appLoaded = Authenticate(authenticator, "#hpid-network-folder-landing-page");
                }
                else if (_controlPanel.CheckState("#hpid-network-folder-app-screen", OmniElementState.Exists))
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
                appLoaded = PressScanToNetworkFolderButton("#hpid-network-folder-app-screen");
            }

            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                throw new DeviceWorkflowException("Could not launch Network Folder application.");
            }
        }

        private bool PressScanToNetworkFolderButton(string expectedDestination)
        {
            return _launchHelper.PressAppButton("Network Folder", "#hpid-networkFolder-homescreen-button", "#hpid-scan-homescreen-button", expectedDestination);
        }

        private bool Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            bool formLoaded = _controlPanel.WaitForAvailable(waitForm);
            Pacekeeper.Pause();
            return formLoaded;
        }

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void EnterFileName(string fileName)
        {
            // Wait for a moment, since other operations (like entering a folder) may leave this field temporarily unusable
            if (_controlPanel.WaitForState("#hpid-network-folder-landing-page", OmniElementState.Useable, TimeSpan.FromSeconds(120)))
            {
                if (_controlPanel.WaitForAvailable("#hpid-file-name-textbox"))
                {
                    _controlPanel.ScrollPressWait("#hpid-file-name-textbox", "#hpid-keyboard", TimeSpan.FromSeconds(10));

                    // Workaround for an issue in firmware.  Depending on flow, pressing the text box might highlight
                    // the whole field OR just place the cursor at the end.  In the latter case, pressing the text box
                    // again will highlight it again.  To work around this, we tap the text box and press "Delete",
                    // and then do it again in case the whole thing was not highlighted the first time.
                    _controlPanel.Type(SpecialCharacter.Backspace);
                    _controlPanel.Press("#hpid-file-name-textbox");
                    _controlPanel.Type(SpecialCharacter.Backspace);

                    _controlPanel.TypeOnVirtualKeyboard(fileName);
                }
                else
                {
                    throw new DeviceWorkflowException("The filename textbox could not be found.");
                }
                Pacekeeper.Sync();
            }
        }

        /// <summary>
        /// Adds the specified network folder path as a destination for the scanned file.
        /// </summary>
        /// <param name="path">The network folder path.</param>
        /// <param name="credential">The network credential parameter is being added to provide the network credentials to access folder path</param>
        /// <param name="applyCredentials"><value><c>true</c> if apply credentials on verification is checked;otherwise, <c>false</c>.</value></param>
        public void AddFolderPath(string path, NetworkCredential credential, bool applyCredentials)
        {
            // Used for 24.5 firmware
            if (_controlPanel.CheckState("#hpid-add-destination-button", OmniElementState.Useable))
            {
                _controlPanel.ScrollPressWait("#hpid-add-destination-button", "#hpid-destination-textbox");
                _controlPanel.PressWait("#hpid-destination-textbox", "#hpid-keyboard");

                Pacekeeper.Sync();
                string escapedPath = path.Replace(@"\", @"\\");
                _controlPanel.TypeOnVirtualKeyboard(escapedPath);

                //Click Ok
                _device.ControlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-save-destination-button");
                _controlPanel.Press("#hpid-save-destination-button");
            }
            // used for older Omni firmware
            else if (_controlPanel.CheckState("#hpid-network-folder-recipient", OmniElementState.Useable))
            {
                _controlPanel.ScrollPressWait("#hpid-network-folder-recipient", "#hpid-keyboard");
                Pacekeeper.Sync();
                string escapedPath = path.Replace(@"\", @"\\");
                _controlPanel.TypeOnVirtualKeyboard(escapedPath);

                //Pressing a coordinate to shift the control out of the text box so that sign in form or verify access is displayed for the scenario where the folder access needs to be verified. 
                _controlPanel.PressScreen(new Coordinate(400, 10));
            }
            else
            {
                throw new DeviceWorkflowException("ScanToNetwork Folder Error: Unknown or new workflow destination.");
            }

            if (_popupManager.WaitForPopup("Verifying access", TimeSpan.FromSeconds(6)))
            {
                _popupManager.HandleTemporaryPopup("Verifying access", TimeSpan.FromSeconds(30));
            }

            Pacekeeper.Sync();

            if (applyCredentials == true)
            {
                // when the device is configured to verify folder access,Sign In form is displayed to enter folder credentials.
                AuthenticateFolderPath(credential);
            }
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// </summary>
        /// <param name="quicksetName">The quickset name.</param>
        public void SelectQuickset(string quicksetName)
        {
            TimeSpan ts = TimeSpan.FromSeconds(6);
            _controlPanel.WaitForAvailable(".hp-load-quickset-button", ts);
            _controlPanel.ScrollPressWait(".hp-load-quickset-button", "#hpid-quicksets-list");
            Pacekeeper.Sync();

            string listItemSelector = ".hp-listitem-text:contains(" + quicksetName + ")";
            if (_controlPanel.WaitForState(listItemSelector, OmniElementState.Exists, true, ts))
            {
                int matches = _controlPanel.GetCount(listItemSelector);
                if (matches == 1)
                {
                    _controlPanel.Press(listItemSelector);
                }
                else
                {
                    PressListItem(quicksetName, listItemSelector, matches);
                }
                Pacekeeper.Sync();

                _controlPanel.ScrollPress("#hpid-quicksets-load-button");
            }
            else
            {
                throw new DeviceWorkflowException("Quickset destination list for " + quicksetName + " not loaded or set within " + ts.ToString() + ".");
            }
            Pacekeeper.Sync();
        }

        private void PressListItem(string quicksetName, string listItemSelector, int matches)
        {
            for (int i = 0; i < matches; i++)
            {
                string specificListItem = $"{listItemSelector}:eq({i})";
                if (quicksetName == _controlPanel.GetValue(specificListItem, "innerText", OmniPropertyType.Property))
                {
                    _controlPanel.Press(specificListItem);
                    return;
                }
            }
            throw new DeviceWorkflowException($"Could not find quickset with text '{quicksetName}'.");
        }

        /// <summary>
        /// Gets the number of folder destinations currently selected.
        /// </summary>
        /// <returns>The number of folder destinations.</returns>
        public int GetDestinationCount()
        {
            return _controlPanel.GetCount(".hp-unit-bubble");
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool completedExecution = false;
            _executionManager.WorkflowLogger = WorkflowLogger;
            completedExecution = _executionManager.ExecuteScanJob(executionOptions, "#hpid-button-folder-start");
            return completedExecution;
        }

        /// <summary>
        /// Authenticates the folder path with credentials
        /// </summary>
        /// <param name="credential">The network credential parameter is being added to provide the network credentials to access folder path </param>
        private void AuthenticateFolderPath(NetworkCredential credential)//network credential for folder access is supported for Jedi Omni device for now 
        {
            if (_controlPanel.WaitForAvailable("#hpid-signin-body", TimeSpan.FromSeconds(5))) //_controlPanel.WaitForState("#hpid-signin-body", OmniElementState.Useable))
            {
                //Enter the user name 
                _device.ControlPanel.PressWait(".hp-credential-control:first:text", "#hpid-keyboard", TimeSpan.FromSeconds(2));
                _device.ControlPanel.TypeOnVirtualKeyboard(credential.UserName);

                //Enter the Password
                _device.ControlPanel.ScrollPressWait(".hp-credential-control:password", "#hpid-keyboard", TimeSpan.FromSeconds(2));
                _device.ControlPanel.TypeOnVirtualKeyboard(credential.Password);

                //Enter the user name Domain
                _device.ControlPanel.ScrollPressWait(".hp-credential-control:last:text", "#hpid-keyboard", TimeSpan.FromSeconds(2));
                _device.ControlPanel.TypeOnVirtualKeyboard(credential.Domain);

                if (_device.ControlPanel.WaitForState("#hpid-keyboard-key-done", OmniElementState.Exists))
                {
                    _device.ControlPanel.WaitForAvailable("#hpid-keyboard-key-done");
                    _device.ControlPanel.Press("#hpid-keyboard-key-done");
                }

            }
            // Verification of network folder access,for successful validation continue the workflow or else show device workflow exception with custom message
            Retry.UntilTrue(() => IsErrorShown(), 10, TimeSpan.FromSeconds(2));
        }

        /// <summary>
        /// Returns the bool value based on the error popup shown
        /// </summary>
        /// <returns><value><c>true</c> if popup message is not available;otherwise,<c>false</c>.</value></returns>
        private bool IsErrorShown()
        {
            if (_popupManager.WaitForPopup(TimeSpan.FromSeconds(5)))
            {
                string value = _popupManager.GetPopupText();
                if (value.Contains("Invalid User Name,Password") || value.Contains("The network folder path"))
                {
                    throw new DeviceWorkflowException(value);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Gets the <see cref="INetworkFolderJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public INetworkFolderJobOptions Options => _optionsManager;

        private class JediOmniNetworkFolderJobOptions : JediOmniJobOptionsManager, INetworkFolderJobOptions
        {
            public JediOmniNetworkFolderJobOptions(JediOmniDevice device)
                : base(device)
            {
            }
        }
    }
}
