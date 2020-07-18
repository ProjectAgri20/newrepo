using System;
using System.Threading;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni UdocxScan Authenticator class.  Provides authentication for UdocxScan on Jedi Omni devices.
    /// </summary>
    internal class JediOmniUdocxScanAuthenticator : JediOmniWindowsAuthenticator
    {
        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly string[] _differentUser = { "different-user" };
        private readonly string[] _inputId = { "email-textbox" };
        private readonly string[] _inputPw = { "password-textbox" };
        private readonly string[] _logoutButton = { "logout-btn" };
        private readonly string[] _loginButton = { "login-button" };
        private TimeSpan _idleTimeoutOffset;
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniUdocxScanAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniUdocxScanAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
            _engine = new OxpdBrowserEngine(controlPanel);
            _controlPanel = controlPanel;
            _idleTimeoutOffset = TimeSpan.FromMilliseconds(1000);
        }

        /// <summary>
        /// Enters credentials on the device.
        /// If the device prompts for username and password, enters them.
        /// If the device prompts for PIN, enters PIN.
        /// </summary>
        public override void EnterCredentials()
        {
            Login(Credential.UserName, Credential.Password);
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            PressButton(_loginButton[0]);
            Pacekeeper.Pause();
        }
        
        /// <summary>
        /// SignOut from Udocx
        /// </summary>
        private void LogOut()
        {
            OxpdBrowserEngine engine = new OxpdBrowserEngine(ControlPanel);
            if (engine.WaitForHtmlContains(_logoutButton[0], TimeSpan.FromMilliseconds(5000)))
            {
                PressButton(_logoutButton[0]);
            }
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
        /// Signin in Udocx
        /// </summary>
        /// <param name="id">id for signin</param>
        /// <param name="pw">pw for signin</param>
        private void Login(string id, string pw)
        {
            try
            {
                if (!_engine.HtmlContains(_differentUser[0]))
                {
                    PressButton(_inputId[0]);
                    _controlPanel.WaitForAvailable("#hpid-keyboard-key-done", _idleTimeoutOffset);
                    _controlPanel.TypeOnVirtualKeyboard(id);
                    Pacekeeper.Sync();
                    PressButton(_inputPw[0]);
                    _controlPanel.WaitForAvailable("#hpid-keyboard-key-done", _idleTimeoutOffset);
                    _controlPanel.TypeOnVirtualKeyboard(pw);
                    Pacekeeper.Sync();
                }
                else
                {
                    PressButton(_differentUser[0]);
                    _engine.WaitForHtmlContains(_inputId[0], TimeSpan.FromMilliseconds(20000));
                    PressButton(_inputId[0]);
                    _controlPanel.WaitForAvailable("#hpid-keyboard-key-done", _idleTimeoutOffset);
                    _controlPanel.TypeOnVirtualKeyboard(id);
                    Pacekeeper.Sync();
                    PressButton(_inputPw[0]);
                    _controlPanel.WaitForAvailable("#hpid-keyboard-key-done", _idleTimeoutOffset);
                    _controlPanel.TypeOnVirtualKeyboard(pw);
                    Pacekeeper.Sync();
                }
                _controlPanel.Press("#hpid-keyboard-key-done");
                Pacekeeper.Pause();
            }
            catch (Exception ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException("Fail to signIn", ex);
                throw e;
            }
        }

        /// <summary>
        /// Checks the device control panel for successful authentication.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the authentication operation is valid, <c>false</c> otherwise.
        /// </returns>
        public override bool ValidateAuthentication()
        {
            if (_engine.WaitForHtmlContains("apps_list_app_title", TimeSpan.FromMilliseconds(20000)))
            {
                Thread.Sleep(2000);
                Pacekeeper.Pause();
                return true;
            }
            return false;
        }
    }
}
