using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.TraySettings
{
    /// <summary>
    /// Implementation of <see cref="ITraySettingsApp" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniTraySettingsApp : DeviceWorkflowLogSource, ITraySettingsApp
    {

        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private Pacekeeper _pacekeeper;
        private readonly TimeSpan _activeWaitTimeOut = TimeSpan.FromSeconds(3);
        private JediOmniPreparationManager _preparationManager;

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
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniTraySettingsApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniTraySettingsApp(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _preparationManager = new JediOmniPreparationManager(device);
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Opens Contacts screen with the specified authenticator using the given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            bool signInScreenLoaded = false;

            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                if (_controlPanel.CheckState("#hpid-settings-homescreen-button", OmniElementState.Exists))
                {
                    RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Settings");
                    signInScreenLoaded = _controlPanel.ScrollPressWait("#hpid-settings-homescreen-button", "#hpid-signin-body", TimeSpan.FromSeconds(10));
                    Pacekeeper.Pause();
                }
                else
                {
                    throw new DeviceWorkflowException("Settings button was not found on device home screen.");
                }
                if (!signInScreenLoaded)
                {
                    if (_controlPanel.CheckState("#hpid-settings-homescreen-button", OmniElementState.Exists))
                    {
                        // The application launched without prompting for credentials
                        Pacekeeper.Reset();
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Could not open Settings");
                    }
                }
                else
                {
                    Authenticate(authenticator, "#hpid-settings-homescreen-button");
                }
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                throw new NotImplementedException("Eager authentication has not been implemented for this solution.");
            }
        }

        /// <summary>
        /// Sets the Tray setting for Managed Trays
        /// <param name="traySettings">Container to hold values of the Tray setting</param>
        /// </summary>
        public void ManageTraySettings(TraySettings traySettings)
        {
            _controlPanel.PressWait("#hpid-tree-node-listitem-copyprint", "#hpid-tree-node-listitem-managetrays", _activeWaitTimeOut);
            Pacekeeper.Sync();
            Pacekeeper.Pause();
            _controlPanel.PressWait("#hpid-tree-node-listitem-managetrays", "#hpid-tree-node-listitem-userequestedtray", _activeWaitTimeOut);
            Pacekeeper.Sync();
            Pacekeeper.Pause();
            if (traySettings.IsUseRequesetedTraySet)
            {
                _controlPanel.PressWait("#hpid-tree-node-listitem-userequestedtray", "#hpid-setting-userequestedtray-selection-true", _activeWaitTimeOut);
                _controlPanel.Press(traySettings.UseRequesetedTray ? "#hpid-setting-userequestedtray-selection-true" : "#hpid-setting-userequestedtray-selection-false");
            }
            if (traySettings.IsManualFeedPromptSet)
            {
                _controlPanel.PressWait("#hpid-tree-node-listitem-manuallyfeedprompt", "#hpid-setting-manuallyfeedprompt-selection-true", _activeWaitTimeOut);
                _controlPanel.Press(traySettings.ManualFeedPrompt ? "#hpid-setting-manuallyfeedprompt-selection-true" : "#hpid-setting-manuallyfeedprompt-selection-false");
            }
            if (traySettings.IsSizeTypePromptSet)
            {
                _controlPanel.PressWait("#hpid-tree-node-listitem-sizetypeprompt", "#hpid-setting-sizetypeprompt-selection-display", _activeWaitTimeOut);
                _controlPanel.Press(traySettings.SizeTypePrompt ? "#hpid-setting-sizetypeprompt-selection-display" : "#hpid-setting-sizetypeprompt-selection-donotdisplay");
            }
            if (traySettings.IsUseAnotherTraySet)
            {
                _controlPanel.PressWait("#hpid-tree-node-listitem-useanothertray", "#hpid-setting-useanothertray-selection-false", _activeWaitTimeOut);
                _controlPanel.Press(traySettings.UseAnotherTray ? "#hpid-setting-useanothertray-selection-false" : "#hpid-setting-useanothertray-selection-true");
            }
            if (traySettings.IsAlternativeLetterheadModeSet)
            {
                _controlPanel.PressWait("#hpid-tree-node-listitem-alternativeletterheadmode", "#hpid-setting-alternativeletterheadmode-selection-true", _activeWaitTimeOut);
                _controlPanel.Press(traySettings.AlternativeLetterheadMode ? "#hpid-setting-alternativeletterheadmode-selection-false" : "#hpid-setting-alternativeletterheadmode-selection-true");
            }
            if (traySettings.IsDuplexBlankPagesSet)
            {
                _controlPanel.PressWait("#hpid-tree-node-listitem-duplexblankpages", "#hpid-setting-duplexblankpages-selection-auto", _activeWaitTimeOut);
                _controlPanel.Press(traySettings.DuplexBlankPages ? "#hpid-setting-duplexblankpages-selection-auto" : "#hpid-setting-duplexblankpages-selection-yes");
            }
            if (traySettings.IsImageRotationSet)
            {
                if (_controlPanel.CheckState("#hpid-tree-node-listitem-imagerotation", OmniElementState.Exists))
                {
                    if (traySettings.ImageRotation == ImageRoationType.LeftToRight)
                    {
                        _controlPanel.PressWait("#hpid-tree-node-listitem-imagerotation", "#hpid-setting-imagerotation-selection", _activeWaitTimeOut);
                        _controlPanel.Press("#hpid-setting-imagerotation-selection-standard");
                    }
                    else if (traySettings.ImageRotation == ImageRoationType.RightToLeft)
                    {
                        _controlPanel.PressWait("#hpid-tree-node-listitem-imagerotation", "#hpid-setting-imagerotation-selection", _activeWaitTimeOut);
                        _controlPanel.Press("#hpid-setting-imagerotation-selection-righttoleft");
                    }
                    else
                    {
                        _controlPanel.PressWait("#hpid-tree-node-listitem-imagerotation", "#hpid-setting-imagerotation-selection", _activeWaitTimeOut);
                        _controlPanel.Press("#hpid-setting-imagerotation-selection-alternate");
                    }
                }
            }
            if (traySettings.IsOverrideA4LetterSet)
            {
                _controlPanel.PressWait("#hpid-tree-node-listitem-overridea4letter", "#hpid-setting-overridea4letter-selection-no", _activeWaitTimeOut);
                _controlPanel.Press(traySettings.OverrideA4Letter ? "#hpid-setting-overridea4letter-selection-no" : "#hpid-setting-overridea4letter-selection-yes");
            }
            _preparationManager.NavigateHome();
        }

        /// <summary>
        /// Authenticates the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The wait form.</param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            _controlPanel.WaitForAvailable(waitForm);
            Pacekeeper.Pause();
        }
    }
}
