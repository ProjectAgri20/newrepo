using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni SafeCom Authenticator class.  Provides authentication for SafeCom on Jedi Omni devices.
    /// </summary>
    internal class JediOmniSafeComUCAuthenticator : JediOmniPinAuthenticator
    {
        private readonly OxpdBrowserEngine _engine;
        private const string _codeTextBoxClassname = "inputField";
        private const string _loginButtonId = "btnLogin";

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniSafeComUCAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniSafeComUCAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {            
            _engine = new OxpdBrowserEngine(controlPanel);
        }

        /// <summary>
        /// Enters credentials on the device.
        /// If the device prompts for username and password, enters them.
        /// If the device prompts for PIN, enters PIN.
        /// </summary>
        public override void EnterCredentials()
        {
            if (UseSafeComLdap())
            {
                EnterUserNamePassword();                
            }
            else
            {   
                EnterPin();                
            }
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            bool check = true;
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime + TimeSpan.FromSeconds(10);

            while (check)
            {
                if (ExistElementIdOxpd(_loginButtonId))
                {
                    PressElementOxpd(_loginButtonId);
                    check = false;
                }
                else if(DateTime.Now > endTime)
                {
                    throw new DeviceWorkflowException($"Fail to find login button {DateTime.Now - startTime}");
                }
                else
                {
                    Thread.Sleep(250);
                }
            }
        }

        /// <summary>
        /// Enters the user name, password (and domain if applicable) on the device control panel.
        /// Executes in a different order than the base class.
        /// </summary>
        protected override void EnterUserNamePassword()
        {
            EnterUserName();
            EnterPassword();
            EnterDomain();
        }

        protected override void EnterUserName()
        {
            try
            {
                PressElementOxpdByClassIndex(_codeTextBoxClassname, 0);
                TypeOnVirtualKeyboard(Credential.UserName);
                Pacekeeper.Sync();
            }
            catch (DeviceInvalidOperationException ex)
            {
                throw new DeviceWorkflowException("User Name textbox was not found.", ex);
            }
        }

        protected override void EnterPassword()
        {
            try
            {
                PressElementOxpdByClassIndex(_codeTextBoxClassname, 1);
                TypeOnVirtualKeyboard(Credential.Password);
                Pacekeeper.Sync();
            }
            catch (DeviceInvalidOperationException ex)
            {
                throw new DeviceWorkflowException("Password textbox was not found.", ex);
            }
        }

        protected override void EnterDomain()
        {
            bool noOpException = true;
            try
            {
                if (WaitForHtmlContains("Domain", TimeSpan.FromMilliseconds(500)))
                {
                    string script = $"document.getElementsByClassName('{_codeTextBoxClassname}')[2].value";

                    if (_engine.ExecuteJavaScript(script).Trim('"') == "")
                    {
                        noOpException = false;
                        PressElementOxpdByClassIndex(_codeTextBoxClassname, 2);
                        TypeOnVirtualKeyboard(Credential.Domain);
                        Pacekeeper.Sync();
                    }
                }
            }
            catch (DeviceInvalidOperationException ex)
            {
                ExecutionServices.SystemTrace.LogDebug($"Unable to determine the existence of 'Domain' textbox.  {ex.ToString()}");
                // The intention here is to only throw a DeviceWorkflowException if the Domain textbox was detected AND failure occurred in entering the domain name.
                // All other failures in different locations (even while trying to detect if the domain textbox even exists) can just no-op.
                if (!noOpException)
                {
                    throw new DeviceWorkflowException("Domain textbox was not found.", ex);
                }
            }
        }

        protected override void EnterPin()
        {
            PressElementOxpdByClassIndex(_codeTextBoxClassname, 0);
            TypeOnVirtualKeyboard(Credential.Pin);
            Pacekeeper.Sync();
        }

        private bool UseSafeComLdap()
        {
            JediOmniMasthead masthead = new JediOmniMasthead(ControlPanel);

            DateTime start = DateTime.Now;
            if (ControlPanel.CheckState(masthead.MastheadSpinner, OmniElementState.Exists))
            {
                if (!Wait.ForTrue(() => masthead.BusyStateActive() == false, TimeSpan.FromSeconds(30)))
                {
                    throw new DeviceWorkflowException("SafeCom UC Authentication screen did not display within 30 seconds.");
                }
            }
            
                if (_engine.HtmlContains("Code"))
                {
                    return false;
                }
                return true;           
        }

        /// <summary>
        /// This method checks for an overlay that displays after successful login when the Print All tile was selected for Lazy Auth.
        /// 12/13/2019: Created for validating lazy auth "PrintAll" from the home screen.  If Lazy auth is not supported by SafeCom UC, this method can be removed.
        /// </summary>
        /// <param name="signedIn"></param>
        /// <returns></returns>
        private bool IsPrintingAll(out bool signedIn)
        {
            signedIn = _engine.ExistElementId("lblPleaseWait");
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: JediOmniSafeComUCAuthenticator::IsPrintingAll::Found valid signin notification: {signedIn}.");

            return signedIn;
        }

    }
}
