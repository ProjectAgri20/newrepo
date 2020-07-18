using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Authentication
{
    internal sealed class WindjammerAuthenticatorDriver : AuthenticationDriverBase
    {
        private JediWindjammerPreparationManager _preparationManager;
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private InitiationMethod _initMethod;
        private string _solutionButton;

        public bool IsAuthenticated { get; private set; } = false;
        public bool IsUnAuthenticated { get; private set; } = false;
        public bool OnHomeScreen { get; private set; } = false;

        public WindjammerAuthenticatorDriver(JediWindjammerDevice device, string solutionButton, DeviceWorkflowLogger workflowLogger)
        {
            _device = device;
            _preparationManager = new JediWindjammerPreparationManager(_device);
            _workflowLogger = workflowLogger;

            _controlPanel = _device.ControlPanel;


            _solutionButton = solutionButton;
            _initMethod = AuthInitMethod.GetInitiationMethod(_solutionButton);

            PrepareDevice();
        }
        /// <summary>
        /// Launches the Windjammer specified authenticator utilizing the given authentication initialization method.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationInitMethod">The authentication initialize method.</param>
        /// <exception cref="DeviceWorkflowException">Device Initialization method " + authenticationInitMethod.GetDescription() + " is not supported at this time.</exception>
        public override void Launch(IAuthenticator authenticator, AuthenticationInitMethod authenticationInitMethod)
        {
            authenticator.InitializationMethod = authenticationInitMethod;

            if (authenticationInitMethod.Equals(AuthenticationInitMethod.SignInButton))
            {
                ActivateViaSignIn(authenticator);
            }
            else if (authenticationInitMethod.Equals(AuthenticationInitMethod.ApplicationButton))
            {
                ActivateViaAppButton(authenticator);
            }
            else if (authenticationInitMethod.Equals(AuthenticationInitMethod.Badge))
            {
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
            }
            else
            {
                throw new DeviceWorkflowException("Device Initialization method " + authenticationInitMethod.GetDescription() + " is not supported at this time.");
            }
           
        }

        private void ActivateViaSignIn(IAuthenticator authenticator)
        {
            if (_controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM))
            {
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
                OnHomeScreen = true;
            }

        }

        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                ActivateViaSignIn(authenticator);
            }
            else // AuthenticationMode.Lazy
            {
                ActivateViaAppButton(authenticator);
            }
        }

        private void ActivateViaAppButton(IAuthenticator authenticator)
        {
            switch (_initMethod)
            {
                case InitiationMethod.Badge:
                    Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
                    break;
                case InitiationMethod.Copy:
                    CopyLazyAuth(authenticator, _device);
                    break;
                case InitiationMethod.Email:
                    EmailLazyAuth(authenticator, _device);
                    break;
                case InitiationMethod.Fax:
                    FaxLazyAuth(authenticator, _device);
                    break;
                case InitiationMethod.NetworkFolder:
                    NetworkFolderLazyAuth(authenticator, _device);
                    break;
                case InitiationMethod.WorkFlow:
                    DssWorkflowLazyAuth(authenticator, _device);
                    break;
                case InitiationMethod.Equitrac:
                case InitiationMethod.HPAC:
                case InitiationMethod.HPCR_PersonalDistributions:
                case InitiationMethod.HPCR_PuplicDistributions:
                case InitiationMethod.HPCR_RoutingSheet:
                case InitiationMethod.HPCR_ScanToMe:
                case InitiationMethod.HPCR_ScanToMyFiles:
                case InitiationMethod.HPEC:
                case InitiationMethod.SafeCom:
                    PressSolutionButton();
                    Authenticate(authenticator, JediWindjammerLaunchHelper.OxpdFormIdentifier);
                    OnHomeScreen = false;
                    IsAuthenticated = true;
                    IsUnAuthenticated = false;
                    break;
                default:
                    throw new ControlNotFoundException("The '" + _solutionButton + "' is not defined for use by the Authentication plugin at this time.");
            }
        }

        private void PressSolutionButton()
        {
            RecordEvent(DeviceWorkflowMarker.AppButtonPress, _initMethod.GetDescription());
            _controlPanel.ScrollPressNavigate("mAccessPointDisplay", "Title", _solutionButton, JediWindjammerLaunchHelper.SIGNIN_FORM, ignorePopups: true);
        }

        public override void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            try
            {
                authenticator.Authenticate();
                Thread.Sleep(2000);
                if (_controlPanel.WaitForForm(waitForm, StringMatch.Contains, TimeSpan.FromSeconds(30)) == true)
                {
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                }

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


        private void PrepareDevice()
        {
            _preparationManager.InitializeDevice(_initMethod != InitiationMethod.DoNotSignIn);

            OnHomeScreen = true;
            IsUnAuthenticated = true;
            IsAuthenticated = false;
        }

    }
}
