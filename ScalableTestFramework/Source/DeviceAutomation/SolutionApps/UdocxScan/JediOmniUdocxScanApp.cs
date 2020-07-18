using System;
using System.Threading;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.UdocxScan
{
    /// <summary>
    /// Configuration to prepare job for UdocxScan Apps.
    /// </summary>
    public class JediOmniUdocxScanApp : UdocxScanAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;
        private readonly TimeSpan _idleTimeoutOffset;
        private readonly string[] _differentUser = { "different-user" };
        private readonly string[] _inputId = { "email-textbox" };
        private readonly string[] _inputPw = { "password-textbox" };
        private readonly string[] _loginButton = { "login-button" };
        private readonly string[] _inputEmailAddress = { "targetemailtext" };
        private readonly string[] _scanButton = { "scan-btn" };
        private readonly string[] _logoutButton = { "logout-btn" };
        private readonly string[] _emailReady = { "nice-form-field-input required-field ready" };

        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;

        /// <summary>
        /// Gets or sets the pacekeeper for this options manager.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; } = new Pacekeeper(TimeSpan.Zero);

        /// <param name="device"></param>
        public JediOmniUdocxScanApp(JediOmniDevice device)
            : base(device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _launchHelper = new JediOmniLaunchHelper(device);
            _masthead = new JediOmniMasthead(device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
            _engine = new OxpdBrowserEngine(_controlPanel, UdocxScanResource.UdocxScanJavaScript);
        }
        
        /// <summary>
        /// Launches The AutoStore solution with the given authenticator with either eager or lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="DeviceWorkflowException">
        /// Login screen not found.
        /// or
        /// Eager authentication is not allowed by the AutoStore application at this time.
        /// </exception>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                try
                {
                    UpdateStatus("Launch App: Udocx Scan");
                    _controlPanel.ScrollPress($"div[aria-label=\"Udocx\"]");
                    RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Udocx");
                    IsSignInScreen();
                    authenticator.LazyAuthOnly = true;
                    authenticator.Authenticate();
                }
                catch (Exception ex)
                {
                    DeviceWorkflowException e = new DeviceWorkflowException("Fail to Signin Udocx Scan App", ex);
                    throw e;
                }
            }
            else // AuthenticationMode.Eager
            {
                throw new DeviceWorkflowException("Eager authentication is not allowed by the UdocxScan application at this time.");
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Launches Udocx Scan App in Udocx
        /// </summary>
        /// <param name="destination">Selected App Name</param>
        public override void SelectApp(string destination)
        {
            UpdateStatus($"Launch Selected App: {destination}");
            try
            {
                PressElementByText("apps_list_app_title", destination);
            }
            catch (Exception ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to Luanch Udocx Scan App : {destination}", ex);
                throw e;
            }
        }
        
        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public override bool SignInReleaseAll(IAuthenticator authenticator)
        {
            bool result = false;

            return result;
        }

        /// <summary>
        /// Start Scan Job and finish
        /// </summary>
        public override void Scan(string emailAddress)
        {
            _engine.WaitForHtmlContains(_inputEmailAddress[0], TimeSpan.FromMilliseconds(5000));
            PressButton(_inputEmailAddress[0]);
            Pacekeeper.Sync();
            _controlPanel.TypeOnVirtualKeyboard(emailAddress);
            _controlPanel.Press("#hpid-keyboard-key-done");
            Pacekeeper.Pause();
            _engine.WaitForHtmlContains(_emailReady[0], TimeSpan.FromMilliseconds(10000));
            PressButton( _scanButton[0]);
            ScanJobFinish();
        }

        /// <summary>
        /// Start Scan Job and finish
        /// </summary>
        public override void Scan()
        {
            _engine.WaitForHtmlContains(_emailReady[0], TimeSpan.FromMilliseconds(10000));
            PressButton(_scanButton[0]);
            ScanJobFinish();
        }

        /// <summary>
        /// Check the scan job status
        /// </summary>
        private void ScanJobFinish()
        {
            int count = 0;
            bool result = false;
            while (count < 100)
            {
                if(_engine.HtmlContains("Successfully"))
                {
                    result = true;
                    break;
                }
                count++;
            }
            if (result == false)
            {
                DeviceWorkflowException e = new DeviceWorkflowException("Fail Job Finish");
                throw e;
            }
        }
        
        /// <summary>
        /// Check that App Launch is completed
        /// </summary>
        private void IsSignInScreen()
        {
            int count = 0;
            _engine.WaitForHtmlContains("Card reader is active", TimeSpan.FromMilliseconds(20000));
            while (count < 50)
            {
                if (_engine.HtmlContains(_differentUser[0]) || _engine.HtmlContains(_inputId[0]))
                {
                    Thread.Sleep(2000);
                    break;
                }
            }
            if (count == 50)
            {
                DeviceWorkflowException e = new DeviceWorkflowException("App Launch is failed");
                throw e;
            }
        }
    }
}
