using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Authentication
{
    internal sealed class SiriusUIv3AuthenticationDriver : AuthenticationDriverBase// DeviceWorkflowPerformanceSource, IAuthenticationDriver
    {
        private SiriusUIv3Device _device;
        private SiriusUIv3ControlPanel _controlPanel;
        private SiriusUIv3PreparationManager _preparationManager;
        private InitiationMethod _initMethod;
        private string _solutionButton;

        /// <summary>
        /// Gets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated { get; private set; } = false;
        /// <summary>
        /// Gets a value indicating whether this instance is un authenticated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is un authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnAuthenticated { get; private set; } = false;
        /// <summary>
        /// Gets a value indicating whether [on home screen].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [on home screen]; otherwise, <c>false</c>.
        /// </value>
        public bool OnHomeScreen { get; private set; } = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="OmniAuthenticatorDriver"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="solutionButton">The solution button.</param>
        /// <param name="maxWaitTime">The maximum wait time.</param>
        public SiriusUIv3AuthenticationDriver(SiriusUIv3Device device, string solutionButton, DeviceWorkflowLogger workflowLogger)
        {
            _device = device;
            _controlPanel = device.ControlPanel;

            _preparationManager = new SiriusUIv3PreparationManager(_device);
            _solutionButton = solutionButton;

            _initMethod = AuthInitMethod.GetInitiationMethod(solutionButton);

            PrepareDevice(workflowLogger);
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
                PressSignInButton();
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
                    case InitiationMethod.Equitrac:
                    case InitiationMethod.HPAC:
                    case InitiationMethod.SafeCom:
                        PressSolutionButton();
                        Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
                        OnHomeScreen = false;
                        IsAuthenticated = true;
                        IsUnAuthenticated = false;
                        break;
                    default:
                        throw new ControlNotFoundException("The '" + _solutionButton + "' is not defined for use by the Authentication plugin at this time.");
                }
            }
        }
        public override void Launch(IAuthenticator authenticator, AuthenticationInitMethod authenticationInitMethod)
        {
            if (authenticationInitMethod.Equals(AuthenticationInitMethod.SignInButton))
            {
                Launch(authenticator, AuthenticationMode.Eager);
            }
            else
            {
                Launch(authenticator, AuthenticationMode.Lazy);
            }
        }
        private void PressSignInButton()
        {
            _controlPanel.PressByValue("Sign In");
        }

        private void PressSolutionButton()
        {
            TimeSpan waitTimeSpan = TimeSpan.FromSeconds(20);
            TimeSpan pause = TimeSpan.FromSeconds(2);

            if (_controlPanel.WaitForScreenLabel("Home", waitTimeSpan))
            {
                //Scroll to Apps
                _controlPanel.ScrollPressByValue("sfolderview_p", "Apps");

                _controlPanel.WaitForWidgetByValue(_solutionButton, waitTimeSpan);

                _controlPanel.ScrollToItemByValue("sfolderview_p", _solutionButton);
                Thread.Sleep(pause);
                //Press HP AC
                _controlPanel.PressByValue(_solutionButton);
                Thread.Sleep(pause);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to navigate to Home Screen.");
            }
        }

        /// <summary>
        /// Authenticates using the given authenticator and waits for the given form name.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The wait form.</param>
        /// <exception cref="DeviceWorkflowException">Timed out waiting for Sign In screen.</exception>
        public override void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            if (_controlPanel.WaitForScreenLabel("Home"))
            {
                authenticator.Authenticate();
                RecordEvent(DeviceWorkflowMarker.AppShown);
                IsAuthenticated = true;
            }
            else
            {
                throw new DeviceWorkflowException("Timed out waiting for Sign In screen.");
            }
        }

        private void PrepareDevice(DeviceWorkflowLogger workflowLogger)
        {
            _preparationManager.InitializeDevice(_initMethod != InitiationMethod.DoNotSignIn);
            _preparationManager.WorkflowLogger = workflowLogger;

            OnHomeScreen = true;
            IsUnAuthenticated = true;
            IsAuthenticated = false;
        }

    }
}
