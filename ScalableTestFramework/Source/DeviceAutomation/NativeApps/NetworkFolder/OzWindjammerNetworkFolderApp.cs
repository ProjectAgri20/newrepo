using System;
using System.Globalization;
using System.Linq;
using System.Net;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Oz;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.OzWindjammer;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder
{
    /// <summary>
    /// Implementation of <see cref="INetworkFolderApp" /> for an <see cref="OzWindjammerDevice" />.
    /// </summary>
    public sealed class OzWindjammerNetworkFolderApp : DeviceWorkflowLogSource, INetworkFolderApp
    {
        private const int _networkFolderMain = 37;
        private const int _networkFolderKeyboard = 38;
        private const int _homeScreen = 567;
        private const int _signInScreen = 18;

        private readonly OzWindjammerDevice _device;
        private readonly OzWindjammerControlPanel _controlPanel;
        private readonly OzWindjammerNetworkFolderJobOptions _optionsManager;
        private readonly OzWindjammerJobExecutionManager _executionManager;
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
        /// Initializes a new instance of the <see cref="OzWindjammerNetworkFolderApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public OzWindjammerNetworkFolderApp(OzWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new OzWindjammerNetworkFolderJobOptions(device);
            _executionManager = new OzWindjammerJobExecutionManager(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Network Folder application on the device.
        /// </summary>
        public void Launch()
        {
            Widget appButton = _controlPanel.ScrollToItem("Title", "Network Folder");
            if (appButton != null)
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress);
                _controlPanel.Press(appButton);
                bool success = _controlPanel.WaitForScreen(_networkFolderMain, TimeSpan.FromSeconds(30));
                if (!success)
                {
                    int activeScreen = _controlPanel.ActiveScreenId();
                    if (activeScreen == _signInScreen)
                    {
                        throw new DeviceWorkflowException("Sign-in required to launch the Network Folder application.");
                    }
                    else
                    {
                        throw new DeviceWorkflowException($"Could not launch Network Folder application. Active screen: {activeScreen}");
                    }
                }
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Sync();
            }
            else
            {
                if (_controlPanel.ActiveScreenId() == _homeScreen)
                {
                    throw new DeviceWorkflowException("Network Folder application button was not found on device home screen.");
                }
                else
                {
                    throw new DeviceWorkflowException("Cannot launch the Network Folder application: Not at device home screen.");
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
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                Widget appButton = _controlPanel.ScrollToItem("Title", "Network Folder");
                if (appButton != null)
                {
                    RecordEvent(DeviceWorkflowMarker.AppButtonPress);
                    _controlPanel.Press(appButton);
                    bool promptedForCredentials = _controlPanel.WaitForScreen(_signInScreen, TimeSpan.FromSeconds(10));
                    if (promptedForCredentials)
                    {
                        Pacekeeper.Sync();
                        authenticator.Authenticate();
                    }

                    bool success = _controlPanel.WaitForScreen(_networkFolderMain, TimeSpan.FromSeconds(30));
                    if (!success)
                    {
                        throw new DeviceWorkflowException($"Could not launch Network Folder application. Active screen: {_controlPanel.ActiveScreenId()}");
                    }
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                    Pacekeeper.Sync();
                }
                else
                {
                    if (_controlPanel.ActiveScreenId() == _homeScreen)
                    {
                        throw new DeviceWorkflowException("Network Folder application button was not found on device home screen.");
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Cannot launch the Network Folder application: Not at device home screen.");
                    }
                }
            }
            else  // AuthenticationMode.Eager
            {
                throw new NotImplementedException("Eager authentication has not been implemented for Oz Network Folder.");
            }
        }

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void EnterFileName(string fileName)
        {
            Widget fileNameBox = _controlPanel.GetWidgets().FindByLabel("File Name", StringMatch.Contains, WidgetType.StringBox, WidgetType.EditBox);
            _controlPanel.Press(fileNameBox);
            bool success = _controlPanel.WaitForScreen(_networkFolderKeyboard, TimeSpan.FromSeconds(5));

            if (success)
            {
                Pacekeeper.Sync();

                _controlPanel.TypeOnVirtualKeyboard(fileName);
                Pacekeeper.Sync();
                _controlPanel.Press("OK");
                _controlPanel.WaitForScreen(_networkFolderMain, TimeSpan.FromSeconds(5));
                Pacekeeper.Sync();
            }
            else
            {
                throw new DeviceWorkflowException("File name is not user-editable.");
            }
        }

        /// <summary>
        /// Adds the specified network folder path as a destination for the scanned file.
        /// </summary>
        /// <param name="path">The network folder path.</param>
        /// <param name="credential">The network credential parameter is being added to provide the network credentials to access folder path </param>
        /// /// <param name="applyCredentials"><value><c>true</c> if apply credentials on verification is checked;otherwise, <c>false</c>.</value></param>
        public void AddFolderPath(string path, NetworkCredential credential, bool applyCredentials)//network credential for folder access is supported for Omni device. Ozwindjammer yet to be supported
        {
            Widget folderPathBox = _controlPanel.GetWidgets().FindByLabel("Network Folder Path", StringMatch.Contains, WidgetType.StringBox, WidgetType.EditBox);
            _controlPanel.Press(folderPathBox);
            bool success = _controlPanel.WaitForScreen(_networkFolderKeyboard, TimeSpan.FromSeconds(5));

            if (success)
            {
                Pacekeeper.Sync();

                _controlPanel.TypeOnVirtualKeyboard(path);
                Pacekeeper.Sync();
                _controlPanel.Press("OK");
                _controlPanel.WaitForScreen(_networkFolderMain, TimeSpan.FromSeconds(5));
                Pacekeeper.Sync();
            }
            else
            {
                throw new DeviceWorkflowException("Cannot enter a network folder path without sign-in.");
            }
        }


        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// </summary>
        /// <param name="quicksetName">The quickset name.</param>
        public void SelectQuickset(string quicksetName)
        {
            WidgetCollection quicksetWidgets = _controlPanel.GetWidgets().OfType(WidgetType.HelpListItem);
            string bestMatch = StringMatcher.FindBestMatch(quicksetName, quicksetWidgets.Select(n => n.Text),
                StringMatch.StartsWith, ignoreCase: true, ignoreWhiteSpace: true, allowPartialMatch: true);

            if (bestMatch != null)
            {
                Widget quickset = _controlPanel.ScrollToItem("StringContent1", bestMatch);
                _controlPanel.Press(quickset);
                Pacekeeper.Sync();
            }
            else
            {
                throw new DeviceWorkflowException($"Could not find the quickset '{quicksetName}'.");
            }
        }

        /// <summary>
        /// Gets the number of folder destinations currently selected.
        /// </summary>
        /// <returns>The number of folder destinations.</returns>
        public int GetDestinationCount()
        {
            Widget folderPathBox = _controlPanel.GetWidgets().FindByLabel("Network Folder Path", StringMatch.Contains, WidgetType.StringBox, WidgetType.EditBox);

            if (string.IsNullOrEmpty(folderPathBox.Text))
            {
                return 0;
            }
            else
            {
                // Look for text that indicates multiple folders
                string endText = " FOLDERS)";
                if (folderPathBox.Text.EndsWith(endText, StringComparison.OrdinalIgnoreCase))
                {
                    int start = folderPathBox.Text.Length - (endText.Length + 1);
                    return (int.Parse(folderPathBox.Text.Substring(start, 1), CultureInfo.InvariantCulture));
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool completedJob = false;
            _executionManager.WorkflowLogger = WorkflowLogger;
            completedJob = _executionManager.ExecuteScanJob(executionOptions);
            return completedJob;
        }

        /// <summary>
        /// Gets the <see cref="INetworkFolderJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public INetworkFolderJobOptions Options => _optionsManager;

        private class OzWindjammerNetworkFolderJobOptions : OzWindjammerJobOptionsManager, INetworkFolderJobOptions
        {
            public OzWindjammerNetworkFolderJobOptions(OzWindjammerDevice device)
                : base(device)
            {
            }
        }
    }
}
