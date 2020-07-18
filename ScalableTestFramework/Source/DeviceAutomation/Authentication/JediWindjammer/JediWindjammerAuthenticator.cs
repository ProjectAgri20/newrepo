using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using HP.ScalableTest.Utility;
using RobotClient;
using RobotClient.Endpoints;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Jedi Windjammer Authenticator.  Handles all possible authentication scenarios for Windjammer devices.
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.Authentication.AuthenticatorBase" />
    public class JediWindjammerAuthenticator : AuthenticatorBase
    {
        private const string SignInFormId = "SignInForm";
        private AuthenticationProvider _provider;

        private JediWindjammerControlPanel ControlPanel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerAuthenticator"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="provider">The provider.</param>
        public JediWindjammerAuthenticator(JediWindjammerDevice device, AuthenticationCredential credential, AuthenticationProvider provider) : base(credential, provider)
        {
            ControlPanel = device.ControlPanel;
            _provider = provider;

            ProviderMap.Add(AuthenticationProvider.Windows, new ProviderMapItem("Windows", "Windows"));
            ProviderMap.Add(AuthenticationProvider.DSS, new ProviderMapItem("DSS", "DSS"));
            ProviderMap.Add(AuthenticationProvider.LDAP, new ProviderMapItem("LDAP", "LDAP"));
            ProviderMap.Add(AuthenticationProvider.LocalDevice, new ProviderMapItem("Local Device", "Local Device"));
            ProviderMap.Add(AuthenticationProvider.SafeCom, new ProviderMapItem("SafeCom", "SafeCom"));
            ProviderMap.Add(AuthenticationProvider.HpacDra, new ProviderMapItem("HPAC - DRA Server", "HPAC - DRA Server"));
            ProviderMap.Add(AuthenticationProvider.HpacIrm, new ProviderMapItem("HPAC - IRM Server", "HPAC - IRM Server"));
            ProviderMap.Add(AuthenticationProvider.HpacWindows, new ProviderMapItem("HPAC-Windows", "HPAC-Windows"));
            ProviderMap.Add(AuthenticationProvider.HpacAgentLess, new ProviderMapItem("Code Expected", "Code Expected"));  // HPAC AgentLess first screen 
            ProviderMap.Add(AuthenticationProvider.Equitrac, new ProviderMapItem("Equitrac Embedded", "Equitrac Authentication Agent"));
            ProviderMap.Add(AuthenticationProvider.EquitracWindows, new ProviderMapItem("Equitrac-Windows", "Equitrac-Windows"));
            ProviderMap.Add(AuthenticationProvider.Blueprint, new ProviderMapItem("Pharos Authentication Service", "Pharos Authentication Service"));
            ProviderMap.Add(AuthenticationProvider.AutoStore, new ProviderMapItem("AutoStore", "AutoStore"));
            ProviderMap.Add(AuthenticationProvider.HpRoamPin, new ProviderMapItem("Embedded Badge Authentication", "Embedded Badge Authentication"));
            ProviderMap.Add(AuthenticationProvider.PaperCut, new ProviderMapItem("PaperCut", "PaperCut"));
        }

        /// <summary>
        /// Gets the app authenticator for the specified <see cref="AuthenticationProvider" />.
        /// </summary>
        /// <returns><see cref="IAppAuthenticator" /></returns>
        protected override IAppAuthenticator GetAppAuthenticator(AuthenticationProvider provider)
        {
            return JediWindjammerAppAuthenticatorFactory.Create(provider, ControlPanel, Credential, Pacekeeper);
        }

        /// <summary>
        /// Detects the provider setting from the device and sets it in this instance.
        /// </summary>
        /// <returns><see cref="AuthenticationProvider" /></returns>
        protected override AuthenticationProvider GetDefaultProvider()
        {
            AuthenticationProvider result = AuthenticationProvider.Auto;
            string deviceTitle = string.Empty;

            if (this.Provider == AuthenticationProvider.Card)
            {
                // Requested Provider is Card.  No need to try and check the device.
                result = AuthenticationProvider.Card;
            }
            else
            {
                CheckForSignInForm();
                deviceTitle = GetDeviceTitle();

                if (!string.IsNullOrEmpty(deviceTitle))
                {
                    if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.HpacWindows].DisplayText))
                    {
                        result = AuthenticationProvider.HpacWindows;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.EquitracWindows].DisplayText))
                    {
                        result = AuthenticationProvider.EquitracWindows;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.Windows].DisplayText, StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.Windows;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.LDAP].DisplayText, StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.LDAP;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.LocalDevice].DisplayText, StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.LocalDevice;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.DSS].DisplayText, StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.DSS;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.SafeCom].DisplayText, StringComparison.OrdinalIgnoreCase) || deviceTitle.Contains("tap OK", StringComparison.OrdinalIgnoreCase) || deviceTitle.Contains("enter code", StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.SafeCom;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.HpacDra].DisplayText, StringComparison.OrdinalIgnoreCase)||deviceTitle.Contains("Code Required", StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.HpacDra;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.HpacIrm].DisplayText, StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.HpacIrm;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.Equitrac].DisplayText, StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.Equitrac;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.HpacAgentLess].DisplayText))
                    {
                        result = AuthenticationProvider.HpacAgentLess;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.Blueprint].DisplayText, StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.Blueprint;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.AutoStore].DisplayText, StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.AutoStore;
                    }
                    else if (deviceTitle.Contains(ProviderMap[AuthenticationProvider.PaperCut].DisplayText) || deviceTitle.Contains("Log In", StringComparison.OrdinalIgnoreCase))
                    {
                        result = AuthenticationProvider.PaperCut;
                    }

                }
            }

            if (result == AuthenticationProvider.Auto)
            {
                throw new DeviceInvalidOperationException("Unable to detect the provider in form message: " + deviceTitle + ".");
            }

            return result;
        }

        /// <summary>
        /// Applies the Provider to the authentication method on the device.
        /// </summary>
        /// <param name="provider"></param>
        protected override void SetDeviceProvider(AuthenticationProvider provider)
        {
            string settingsButtonId = "mAdvancedButton";
            bool success = false;

            if (ControlPanel.GetProperty<bool>(settingsButtonId, "Visible"))
            {
                //Navigate to the settings dialog
                ControlPanel.PressToNavigate(settingsButtonId, "AdvancedDialog", false);

                ControlPanel.WaitForForm("AdvancedDialog", false);

                ControlPanel.ScrollPress("mAgentRadioButtonList", "Text", ProviderMap[provider].SetText);
                success = ControlPanel.PressWait("m_OKButton", SignInFormId, TimeSpan.FromSeconds(10));                
            }

            if (!success)
            {
                //If we end up here, we were not able to set the authentication method on the device.
                throw new DeviceInvalidOperationException($"Unable to set SignIn method to {EnumUtil.GetDescription(provider)}.");
            }
        }

        private void CheckForSignInForm()
        {
            string currentForm = ControlPanel.CurrentForm();
            if (!LazyAuthOnly && !currentForm.EqualsIgnoreCase(SignInFormId))
            {
                throw new ControlNotFoundException($"Expecting form {SignInFormId} but found {currentForm} instead");
            }
        }

        /// <summary>
        /// Gets the current title on the device SignIn screen.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="HP.DeviceAutomation.DeviceInvalidOperationException">Unable to retrieve current title.</exception>
        private string GetDeviceTitle()
        {
            string value;

            try
            {
                if (!LazyAuthOnly && (_provider != AuthenticationProvider.HpacWindows && _provider != AuthenticationProvider.EquitracWindows))
                {
                    value = ControlPanel.GetProperty("mAppTitleLabel", "Text");
                }
                else
                {
                    value = Provider.GetDescription();
                }
            }
            catch (ControlNotFoundException ex)
            {
                throw new DeviceInvalidOperationException("Unable to retrieve current title.", ex);
            }

            return value;
        }

    }
}
