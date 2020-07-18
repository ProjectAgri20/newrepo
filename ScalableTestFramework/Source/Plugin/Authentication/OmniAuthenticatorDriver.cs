using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Plugin.Authentication
{
    internal sealed class OmniAuthenticatorDriver : AuthenticationDriverBase
    {
        private JediOmniDevice _omniDevice;
        private JediOmniControlPanel _controlPanel;
        private JediOmniLaunchHelper _launchHelper;
        private JediOmniPreparationManager _preparationManager;
        private InitiationMethod _initMethod;
        private string _solutionButton;

        public bool IsAuthenticated { get; private set; } = false;
        public bool IsUnAuthenticated { get; private set; } = false;
        public bool OnHomeScreen { get; private set; } = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="OmniAuthenticatorDriver"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="solutionButton">The solution button.</param>
        public OmniAuthenticatorDriver(JediOmniDevice device, string solutionButton, DeviceWorkflowLogger workflowLogger)
        {
            _omniDevice = device;
            _controlPanel = device.ControlPanel;
            _launchHelper = new JediOmniLaunchHelper(device);
            _launchHelper.WorkflowLogger = workflowLogger;

            _preparationManager = new JediOmniPreparationManager(_omniDevice);
            _preparationManager.WorkflowLogger = workflowLogger;

            _solutionButton = solutionButton;
            _workflowLogger = workflowLogger;

            _initMethod = AuthInitMethod.GetInitiationMethod(solutionButton);

            PrepareDevice();

        }
        /// <summary>
        /// Launches the specified authenticator with the desired authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                _launchHelper.PressSignInButton();
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                OnHomeScreen = IsAuthenticated;
            }
            else // AuthenticationMode.Lazy
            {
                switch (_initMethod)
                {
                    case InitiationMethod.Badge:
                        Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                        break;
                    case InitiationMethod.Copy:
                        CopyLazyAuth(authenticator, _omniDevice);
                        break;
                    case InitiationMethod.Email:
                        EmailLazyAuth(authenticator, _omniDevice);
                        break;
                    case InitiationMethod.Fax:
                        FaxLazyAuth(authenticator, _omniDevice);
                        break;
                    case InitiationMethod.NetworkFolder:
                        NetworkFolderLazyAuth(authenticator, _omniDevice);
                        break;
                    case InitiationMethod.WorkFlow:
                        DssWorkflowLazyAuth(authenticator, _omniDevice);
                        break;
                    case InitiationMethod.Equitrac:
                    case InitiationMethod.HPAC:
                    case InitiationMethod.HPCR_PersonalDistributions:
                    case InitiationMethod.HPCR_PuplicDistributions:
                    case InitiationMethod.HPCR_RoutingSheet:
                    case InitiationMethod.HPCR_ScanToMe:
                    case InitiationMethod.HPCR_ScanToMyFiles:
                    case InitiationMethod.HPEC:
                    case InitiationMethod.HPRoam:
                    case InitiationMethod.SafeCom:
                        PressSolutionButton();
                        Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
                        OnHomeScreen = false;
                        IsAuthenticated = true;
                        IsUnAuthenticated = false;
                        break;
                    case InitiationMethod.DoNotSignIn:
                        break;
                    default:
                        throw new ControlNotFoundException("The '" + _solutionButton + "' is not defined for use by the Authentication plugin at this time.");
                }
            }
        }

        public override void Launch(IAuthenticator authenticator, AuthenticationInitMethod authenticationInitMethod )
        {
            authenticator.InitializationMethod = authenticationInitMethod;

            
            if (authenticationInitMethod.Equals(AuthenticationInitMethod.SignInButton))
            {
                _launchHelper.PressSignInButton();
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                OnHomeScreen = IsAuthenticated;
            }
            else if (authenticationInitMethod.Equals(AuthenticationInitMethod.ApplicationButton)) 
            {
                switch (_initMethod)
                {
                    case InitiationMethod.Badge:
                        Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                        break;
                    case InitiationMethod.Copy:
                        CopyLazyAuth(authenticator, _omniDevice);
                        break;
                    case InitiationMethod.Email:
                        EmailLazyAuth(authenticator, _omniDevice);
                        break;
                    case InitiationMethod.Fax:
                        FaxLazyAuth(authenticator, _omniDevice);
                        break;
                    case InitiationMethod.NetworkFolder:
                        NetworkFolderLazyAuth(authenticator, _omniDevice);
                        break;
                    case InitiationMethod.WorkFlow:
                        DssWorkflowLazyAuth(authenticator, _omniDevice);
                        break;
                    case InitiationMethod.Equitrac:
                    case InitiationMethod.HPAC:
                    case InitiationMethod.HPCR_PersonalDistributions:
                    case InitiationMethod.HPCR_PuplicDistributions:
                    case InitiationMethod.HPCR_RoutingSheet:
                    case InitiationMethod.HPCR_ScanToMe:
                    case InitiationMethod.HPCR_ScanToMyFiles:
                    case InitiationMethod.HPEC:
                    case InitiationMethod.HPRoam:
                    case InitiationMethod.SafeCom:
                        PressSolutionButton();
                        Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
                        OnHomeScreen = false;
                        IsAuthenticated = true;
                        IsUnAuthenticated = false;
                        break;
                    case InitiationMethod.DoNotSignIn:
                        break;
                    default:
                        throw new ControlNotFoundException("The '" + _solutionButton + "' is not defined for use by the Authentication plugin at this time..");
                }
            }
            else if(authenticationInitMethod.Equals(AuthenticationInitMethod.Badge))
            {
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
            }
        }

        private void PressSolutionButton()
        {
            string elementId = ".hp-homescreen-button:contains(" + _solutionButton + "):first";
            _controlPanel.ScrollPressWait(elementId, JediOmniLaunchHelper.SignInForm);
        }

        public override void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            if (_controlPanel.WaitForAvailable(waitForm))
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
                IsAuthenticated = true;
            }
        }

        private void PrepareDevice()
        {
            _preparationManager.InitializeDevice(_initMethod != InitiationMethod.DoNotSignIn);           

            OnHomeScreen = true;
            IsUnAuthenticated = true;
            IsAuthenticated = false;
        }
        /// <summary>
        /// Presses one of the front page buttons.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        private bool PressFrontPageButton(string elementId)
        {
            bool bSuccess = true;

            if (_controlPanel.WaitForAvailable(elementId))
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, _solutionButton);
                bSuccess = _controlPanel.PressWait(elementId, JediOmniLaunchHelper.LazySuccessScreen);
            }
            else
            {
                bSuccess = false;
            }
            return bSuccess;
        }

        /// <summary>
        /// Swipes to button.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns>bool</returns>
        private bool SwipeToButton(string elementId)
        {
            bool success = true;
            List<string> controls = _controlPanel.GetIds(elementId, OmniIdCollectionType.Self).ToList();

            if (controls.Count == 1)
            {
                elementId = "#" + controls[0];
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, _solutionButton);
                success = _controlPanel.ScrollPressWait(elementId, JediOmniLaunchHelper.LazySuccessScreen);
            }
            else
            {
                success = false;
            }
            return success;
        }

    }
}
