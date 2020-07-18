using System;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Enums;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.DeviceAutomation.HpacScan;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.HpacScan
{
    /// <summary>
    /// JediOmniSafeQApp runs JediOmniHpacScanApp automation of the Control Panel
    /// </summary>
    public class JediOmniHpacScanApp : HpacScanAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;
        private readonly TimeSpan _idleTimeoutOffset;
        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;
        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        public string AppName = "Secure Scan";

        /// <param name="device"></param>
        public JediOmniHpacScanApp(JediOmniDevice device)
            : base(device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _launchHelper = new JediOmniLaunchHelper(device);
            _masthead = new JediOmniMasthead(device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
            _engine = new OxpdBrowserEngine(_controlPanel, HpacScanResource.HpacScanJavaScript);
        }

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns>bool - true if error statement exists</returns>
        public override bool BannerErrorState()
        {
            string bannerText = _controlPanel.GetValue(".hp-masthead-title:last", "innerText", OmniPropertyType.Property);
            return bannerText.Contains("Runtime Error");
        }

        /// <summary>
        /// Launches Hpac Scan with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy</param>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _controlPanel.SignalUserActivity();
            _launchHelper.WorkflowLogger = WorkflowLogger;
            switch (authenticator.Provider)
            {                
                case AuthenticationProvider.Skip:
                    break;
                default:
                    PressSecureScanButton();
                    break;
            }

            _engine.WaitForHtmlContains("LOGIN_USER", TimeSpan.FromSeconds(30));
            RecordEvent(DeviceWorkflowMarker.AppShown);

            RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);
            Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
            RecordEvent(DeviceWorkflowMarker.AuthenticationEnd);
        }

        /// <summary>
        /// Press Secure Scan Button
        /// </summary>
        private void PressSecureScanButton()
        {
            _controlPanel.ScrollPress($"div[aria-label=\"{AppName}\"]");
            RecordEvent(DeviceWorkflowMarker.AppButtonPress, AppName);
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="authenticator">authenticator</param>
        /// <param name="waitForm">wait form</param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            _controlPanel.SignalUserActivity();
            authenticator.Authenticate();
            _controlPanel.WaitForAvailable(waitForm);
        }

        /// <summary>
        /// Simplex scan job
        /// </summary>
        /// <param name="ScanCount">number of copies</param>
        /// <param name="IsJobBuildChecked">is job build checked</param>
        public override void ScanSimplex(int ScanCount, bool IsJobBuildChecked)
        {
            bool result = true;

            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            PressScanButton();
            if (!StartedProcessingWork(_idleTimeoutOffset)){
                throw new DeviceWorkflowException($"Secure Scan operation did not start printing documents within {_idleTimeoutOffset} seconds.");
            }
            if (IsJobBuildChecked)
            {
                for (int idx = 1; idx < ScanCount; idx++)
                {
                    if (_engine.WaitForHtmlContains("command_append", _idleTimeoutOffset))
                    {
                        PressButton("command_append");
                    }
                    else
                    {
                        throw new DeviceWorkflowException($"The append button did not become available within {_idleTimeoutOffset.Seconds} seconds.");
                    }
                }
                PressButton("command_finish");
            }
            result = _engine.WaitForHtmlContains("jobdata_action_scan_print", _idleTimeoutOffset);
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);

            if (!result)
            {
                throw new DeviceWorkflowException("Fail to scan");
            }
        }

        /// <summary>
        /// Duplex scan job
        /// </summary>
        /// <param name="ScanCount">number of copies</param>
        /// <param name="IsJobBuildChecked">is job build checked</param>
        public override void ScanDuplex(int ScanCount, bool IsJobBuildChecked)
        {
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            PressScanButton();

            if (!StartedProcessingWork(_idleTimeoutOffset))
            {
                throw new DeviceWorkflowException($"Secure Scan operation did not start printing documents within {_idleTimeoutOffset} seconds.");
            }

            for (int idx = 1; idx < ScanCount; idx++)
            {
                if (_controlPanel.WaitForAvailable("#hpid-button-scan", _idleTimeoutOffset))
                {
                    _controlPanel.Press("#hpid-button-scan");
                }
                else
                {
                    throw new DeviceWorkflowException($"The scan button did not become available within {_idleTimeoutOffset.Seconds} seconds.");
                }
            }

            if (_controlPanel.WaitForAvailable("#hpid-button-done", _idleTimeoutOffset))
            {
                _controlPanel.Press("#hpid-button-done");

                if (_engine.WaitForHtmlContains("command_finish", _idleTimeoutOffset) & IsJobBuildChecked)
                {
                    PressButton("command_finish");

               }
            }
            else
            {
                throw new DeviceWorkflowException($"The done button did not become available within {_idleTimeoutOffset.Seconds} seconds.");
            }

            if (_engine.WaitForHtmlContains("Scan", _idleTimeoutOffset))
            {
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            }
            else
            {
                throw new DeviceWorkflowException($"The main page did not become available within {_idleTimeoutOffset.Seconds} seconds.");
            }
        }

        /// <summary>
        /// Press scan button
        /// </summary>
        private void PressScanButton()
        {
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            if (_engine.WaitForHtmlContains("Scan", _idleTimeoutOffset))
            {
                PressButtonByClass("scan", 0);
            }
            else
            {
                throw new DeviceWorkflowException($"The scan button did not become available within {_idleTimeoutOffset.Seconds} seconds.");
            }
        }

        /// <summary>
        /// Press Advanced button
        /// </summary>
        public override void SelectOption()
        {
            PressButton("jobdata_action_advanced");
        }

        /// <summary>
        /// Set Paper Supply
        /// </summary>
        /// <param name="paperSupplyType"></param>
        public override void SetPaperSupply(PaperSupplyType paperSupplyType)
        {
            switch (paperSupplyType)
            {
                case PaperSupplyType.Duplex:
                    PressButton("DUPLEX");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set Colormode
        /// </summary>
        /// <param name="colorModeType"></param>
        public override void SetColorMode(ColorModeType colorModeType)
        {
            switch (colorModeType)
            {
                case ColorModeType.Color:
                    PressButton("FULL_COLOR");
                    break;
                case ColorModeType.Greyscale:
                    PressButton("GREYSCALE");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set Quality
        /// </summary>
        /// <param name="qualityType"></param>
        public override void SetQuality(QualityType qualityType)
        {
            switch (qualityType)
            {
                case QualityType.Low:
                    PressButton("LOW");
                    break;
                case QualityType.High:
                    PressButton("HIGH");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set SetJobBuild
        /// </summary>
        public override void SetJobBuild()
        {
            PressButton("appendMode");
        }

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public override bool StartedProcessingWork(TimeSpan ts) => _masthead.WaitForActiveJobsButtonState(true, ts);

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns></returns>
        public override bool FinishedProcessingWork() => _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);
    }
}
