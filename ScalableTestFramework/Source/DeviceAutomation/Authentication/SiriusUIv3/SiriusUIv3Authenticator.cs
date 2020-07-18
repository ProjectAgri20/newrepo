using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.Utility;
using RobotClient;
using RobotClient.Endpoints;
using RobotClient.Endpoints.Data;
using HP.ScalableTest.Framework.Assets;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv3
{
    /// <summary>
    /// Sirius UIv3 Authenticator.  Handles all possible authentication scenarios for Sirius UIv3 devices.
    /// </summary>
    public class SiriusUIv3Authenticator : DeviceWorkflowLogSource, IAuthenticator
    {

        /// <summary>
        /// Gets or sets the authentication initialization method.
        /// </summary>
        /// <value>
        /// The authentication initialization method.
        /// </value>
        public AuthenticationInitMethod InitializationMethod { get; set; }
        /// <summary>
        /// Maps the providers to text that is used to set and retrieve authentication methods on the device.
        /// </summary>
        protected Dictionary<AuthenticationProvider, ProviderMapItem> ProviderMap { get; } = new Dictionary<AuthenticationProvider, ProviderMapItem>();

        private SiriusUIv3ControlPanel ControlPanel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3Authenticator"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="provider">The provider.</param>
        public SiriusUIv3Authenticator(SiriusUIv3Device device, AuthenticationCredential credential, AuthenticationProvider provider)
        {
            Credential = credential;
            Provider = provider;
            ControlPanel = device.ControlPanel;

            ProviderMap.Add(AuthenticationProvider.Windows, new ProviderMapItem("Windows", "Windows"));
            ProviderMap.Add(AuthenticationProvider.DSS, new ProviderMapItem("DSS", "DSS"));
            ProviderMap.Add(AuthenticationProvider.LocalDevice, new ProviderMapItem("Local Device", "Local Device"));
            ProviderMap.Add(AuthenticationProvider.SafeCom, new ProviderMapItem("SafeCom", "SafeCom"));
            ProviderMap.Add(AuthenticationProvider.HpacDra, new ProviderMapItem("DRA Authentication", "DRA Authentication"));
            ProviderMap.Add(AuthenticationProvider.HpacIrm, new ProviderMapItem("IRM Authentication", "IRM Authentication"));
            ProviderMap.Add(AuthenticationProvider.Equitrac, new ProviderMapItem("Equitrac Authentication Agent", "Equitrac Authentication Agent"));
        }

        /// <summary>
        /// Gets the authentication Credential.
        /// </summary>
        public AuthenticationCredential Credential { get; protected set; }

        /// <summary>
        /// Gets the authentication Provider.
        /// </summary>
        public AuthenticationProvider Provider { get; protected set; }

        /// <summary>
        /// Gets or sets the Pace keeper for this authenticator which is used to control pacing of the workflow.
        /// </summary>
        /// <value>The Pace keeper.</value>
        public Pacekeeper Pacekeeper { get; set; } = new Pacekeeper(TimeSpan.Zero);

        /// <summary>
        /// Gets or sets a value indicating whether [lazy authentication only].
        /// This requires that the provider by set with the desired solution.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [lazy authentication only]; otherwise, <c>false</c>.
        /// </value>
        public bool LazyAuthOnly { get; set; } = false;

        /// <summary>
        /// Enters credentials on the device control panel and initiates the authentication process.
        /// </summary>
        public void Authenticate()
        {
            Authenticate(null);
        }

        /// <summary>
        /// Enters credentials on the device control panel and initiates the authentication process.
        /// Applies the specified parameters before submitting the authentication request.
        /// </summary>
        public void Authenticate(Dictionary<string, object> parameters)
        {
            AuthenticationInitMethod authMode = GetAuthenticationInitMethod();

            RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);
            AuthenticationProvider deviceDefaultProvider = GetDefaultProvider(ControlPanel.GetScreenInfo());

            // Eager Auth allows for the setting of a provider.  Lazy Auth just prompts for credentials,
            // so we only want to allow the provider to be set if auth mode is Eager.
            if (authMode == AuthenticationInitMethod.SignInButton)
            {
                if (Provider == AuthenticationProvider.Auto)
                {
                    // Use the device default
                    Provider = deviceDefaultProvider;
                }

                if (Provider != deviceDefaultProvider)
                {
                    // Set the device provider to the requested provider
                    SetDeviceProvider(Provider);
                }

                ControlPanel.PressByValue("Continue");
            }

            //At this point this.Provider should be correctly set, so we should use it from now on.
            IAppAuthenticator appAuthenticator = GetAppAuthenticator(Provider);

            if (Provider != AuthenticationProvider.Card && parameters?.Count > 0)
            {
                appAuthenticator.ApplyParameters(parameters);
            }

            RecordInfo(DeviceWorkflowMarker.AuthType, Provider.GetDescription());
            RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);
            appAuthenticator.EnterCredentials();
            RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);

            if (Provider != AuthenticationProvider.Card)
            {
                appAuthenticator.SubmitAuthentication();
            }
            RecordEvent(DeviceWorkflowMarker.AuthenticationEnd);
        }

        /// <summary>
        /// Gets the app authenticator for the specified <see cref="AuthenticationProvider" />.
        /// </summary>
        /// <returns><see cref="IAppAuthenticator" /></returns>
        protected virtual IAppAuthenticator GetAppAuthenticator(AuthenticationProvider provider)
        {
            return SiriusUIv3AppAuthenticatorFactory.Create(provider, ControlPanel, Credential, Pacekeeper);
        }

        /// <summary>
        /// Gets the default authentication setting from the device.
        /// </summary>
        /// <returns><see cref="AuthenticationProvider" /></returns>
        protected virtual AuthenticationProvider GetDefaultProvider(ScreenInfo screenInfo)
        {
            AuthenticationProvider result = AuthenticationProvider.Auto;

            if (this.Provider == AuthenticationProvider.Card)
            {
                // Requested Provider is Card.  No need to try and check the device.
                result = AuthenticationProvider.Card;
            }
            else
            {
                //Check the device for the authenticate method.
                string deviceSetting = GetDeviceAuthSetting(screenInfo);

                if (!string.IsNullOrEmpty(deviceSetting))
                {
                    if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.Windows].DisplayText))
                    {
                        result = AuthenticationProvider.Windows;
                    }
                    else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.LocalDevice].DisplayText))
                    {
                        result = AuthenticationProvider.LocalDevice;
                    }
                    else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.DSS].DisplayText))
                    {
                        result = AuthenticationProvider.DSS;
                    }
                    else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.SafeCom].DisplayText))
                    {
                        result = AuthenticationProvider.SafeCom;
                    }
                    else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.HpacDra].DisplayText))
                    {
                        result = AuthenticationProvider.HpacDra;
                    }
                    else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.HpacIrm].DisplayText))
                    {
                        result = AuthenticationProvider.HpacIrm;
                    }
                    else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.Equitrac].DisplayText))
                    {
                        result = AuthenticationProvider.Equitrac;
                    }
                }
            }

            if (result == AuthenticationProvider.Auto)
            {
                throw new DeviceInvalidOperationException("Unable to detect provider.");
            }

            return result;
        }

        /// <summary>
        /// Applies the Provider to the authentication method on the device.
        /// </summary>
        /// <param name="provider"></param>
        protected virtual void SetDeviceProvider(AuthenticationProvider provider)
        {
            WidgetCollection widgets = ControlPanel.GetScreenInfo().Widgets;
            Widget widget = widgets.FirstOrDefault(w => w.Values.Keys.Contains("text") && w.Values["text"] == ProviderMap[provider].DisplayText);

            if (widget != null)
            {
                // The item exists, select the value
                ControlPanel.PerformAction(WidgetAction.Select, widget.Id);
            }
            else
            {
                throw new DeviceInvalidOperationException($"Unable to set SignIn method to {EnumUtil.GetDescription(provider)}.");
            }
        }

        private AuthenticationInitMethod GetAuthenticationInitMethod()
        {
            // "flow_auth::st_auth_alternate_login_selection"
            if (ControlPanel.WaitForScreenId("login", StringMatch.Contains, TimeSpan.FromSeconds(2)))
            {
                // The Login Selection Screen is being displayed
                return AuthenticationInitMethod.SignInButton;
            }

            // No Login Selection screen.
            return AuthenticationInitMethod.ApplicationButton;
        }

        /// <summary>
        /// Gets the current Authentication setting on the device.
        /// </summary>
        /// <returns>The setting as a string.</returns>
        private string GetDeviceAuthSetting(ScreenInfo screenInfo)
        {
            //First, check to see if we are on the screen with radio buttons
            Widget widget = screenInfo.Widgets.FirstOrDefault(w => w.Values.Keys.Contains("subtext") && w.Values["subtext"] == "SELECTED");

            if (widget != null)
            {
                return widget.Values["text"];
            }

            //No radio buttons, so check to see if it is prompting for a user code (HPAC IRM Auth)
            // if there is a desire to use DRA then will need to check the value of the widget ...
            widget = screenInfo.Widgets.FirstOrDefault(w => w.HasValue("Code Expected"));
            if (widget != null)
            {
                return ProviderMap[AuthenticationProvider.HpacIrm].DisplayText;
            }

            widget = screenInfo.Widgets.FirstOrDefault(w => w.HasValue("User Name", StringMatch.Contains));
            if (widget != null)
            {
                return ProviderMap[AuthenticationProvider.Windows].DisplayText;
            }

            // If we end up here, we couldn't determine the setting.
            return string.Empty;
        }
    }

    internal enum ScreenSizes
    {
        SmallScreen = 0,
        LargeScreen = 1
    };
}
