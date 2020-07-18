using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Jedi Windjammer HPAC Agentless Authenticator class.  Provides authentication for agentless HPAC on Jedi Windjammer devices.
    /// </summary>
    internal class JediWindjammerHpacAgentlessAuthenticator : JediWindjammerAppAuthenticatorBase
    {
        private OxpdBrowserEngine _engine = null;

        private readonly bool _hpacVersion16_6 = true;

        private readonly string[] _codeTextBox = { "userCode", "authentication_screen-request-code_userCode" };
        private readonly string[] _footerOkay = { "screen-request-code.accept", "authentication_screen-request-code_accept" };
        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerHpacAgentlessAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediWindjammerHpacAgentlessAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
            _engine = new OxpdBrowserEngine(ControlPanel);
            CreateExistElementFunction();
            _hpacVersion16_6 = IsVersion16_6();
        }

        /// <summary>
        /// Enters credentials on the device by entering the PIN.
        /// </summary>
        public override void EnterCredentials()
        {
            string codeTextboxId = (_hpacVersion16_6 == true) ? _codeTextBox[1] : _codeTextBox[0];
            BoundingBox box = _engine.GetBoundingAreaById(codeTextboxId);

            _engine.PressElementByBoundingArea(box);

            ControlPanel.Type(Credential.Pin);
            ControlPanel.WaitForControl("ok", TimeSpan.FromSeconds(5));
            ControlPanel.Press("ok");
        }

        /// <summary>
        /// HPAC Server 16.6 changed some of the ID names. If the index 1 of the checked array exists, the server
        /// should be on version 16.6
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is version16 6]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsVersion16_6()
        {
            bool newVersion = true;

            if (!ExistElementId(_codeTextBox[1]))
            {
                newVersion = false;
            }
            return newVersion;
        }

        private void CreateExistElementFunction()
        {
            _engine.ExecuteJavaScript(@"function ExistElementId(elementId) {
	            var buttonElement = document.getElementById(elementId), existElement;
	            existElement = true;
	            if (buttonElement == null) {
		            existElement = false;
	            }
	            return existElement;
                }");
        }

        private bool ExistElementId(string elementId)
        {
            bool exist = false;

            string existElementId = "ExistElementId('" + elementId + "'); ";
            var result = _engine.ExecuteJavaScript(existElementId);
            bool.TryParse(result, out exist);

            return exist;
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            try
            {
                string okayBtn = (_hpacVersion16_6 == true) ? _footerOkay[1] : _footerOkay[0];
                // Press the Oxpd AgentLess done button
                BoundingBox box = _engine.GetBoundingAreaById(okayBtn);
                _engine.PressElementByBoundingArea(box);

                ControlPanel.WaitForForm("HomeScreenForm", TimeSpan.FromSeconds(10));

                Pacekeeper.Pause();
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (ControlPanel.CurrentForm())
                {
                    case "OneButtonMessageBox":
                        try
                        {
                            string message = ControlPanel.GetProperty("mMessageLabel", "Text");
                            if (message.StartsWith("Invalid"))
                            {
                                throw new DeviceWorkflowException($"Could not sign in: {message}", ex);
                            }
                            else
                            {
                                throw new DeviceInvalidOperationException($"Could not sign in: {message}", ex);
                            }
                        }
                        catch (ControlNotFoundException)
                        {
                            throw new DeviceInvalidOperationException($"Could not sign in: {ex.Message}", ex);
                        }

                    default:
                        throw new DeviceInvalidOperationException($"Could not sign in: {ex.Message}", ex);
                }
            }
        }

    }
}
