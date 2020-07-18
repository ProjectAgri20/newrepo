using System;
using System.Linq;
using System.Xml.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni Authenticator.  Handles all possible authentication scenarios for Omni devices.
    /// </summary>
    public class JediOmniAuthenticator : AuthenticatorBase
    {
        private const string AuthDropdownId = "#hpid-dropdown-agent";

        private JediOmniControlPanel ControlPanel { get; }
        private AuthenticationProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniAuthenticator"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="provider">The provider.</param>
        public JediOmniAuthenticator(JediOmniDevice device, AuthenticationCredential credential, AuthenticationProvider provider)
            : base(credential, provider)
        {
            ControlPanel = device.ControlPanel;
            _provider = provider;

            // The ProviderMap maps the text on the device, authentication drop down, to the authentication provider enum.

            ProviderMap.Add(AuthenticationProvider.Windows, new ProviderMapItem("Windows", "Windows"));
            ProviderMap.Add(AuthenticationProvider.DSS, new ProviderMapItem("DSS", "DSS"));
            ProviderMap.Add(AuthenticationProvider.LDAP, new ProviderMapItem("LDAP", "LDAP"));
            ProviderMap.Add(AuthenticationProvider.LocalDevice, new ProviderMapItem("Local Device", "Local Device"));
            ProviderMap.Add(AuthenticationProvider.SafeCom, new ProviderMapItem("SafeCom", "SafeCom"));
            ProviderMap.Add(AuthenticationProvider.SafeComUC, new ProviderMapItem("KOFAX", "KOFAX"));
            ProviderMap.Add(AuthenticationProvider.HpacDra, new ProviderMapItem("HPAC - DRA Server", "HPAC - DRA Server"));
            ProviderMap.Add(AuthenticationProvider.HpacIrm, new ProviderMapItem("HPAC - IRM Server", "HPAC - IRM Server"));
            ProviderMap.Add(AuthenticationProvider.HpacAgentLess, new ProviderMapItem("IRM Authentication", "IRM Authentication"));
            ProviderMap.Add(AuthenticationProvider.HpacPic, new ProviderMapItem("HPAC - PIC Server", "HPAC - PIC Server"));
            ProviderMap.Add(AuthenticationProvider.HpacWindows, new ProviderMapItem("HPAC-Windows", "HPAC-Windows"));
            ProviderMap.Add(AuthenticationProvider.Equitrac, new ProviderMapItem("Equitrac Authentication Agent", "Equitrac Authentication Agent"));
            ProviderMap.Add(AuthenticationProvider.EquitracWindows, new ProviderMapItem("Equitrac Authentication Agent", "Equitrac Authentication Agent"));
            ProviderMap.Add(AuthenticationProvider.Blueprint, new ProviderMapItem("Pharos Authentication Agent", "Pharos Authentication Agent"));
            ProviderMap.Add(AuthenticationProvider.PaperCut, new ProviderMapItem("PaperCut", "PaperCut"));
            ProviderMap.Add(AuthenticationProvider.ISecStar, new ProviderMapItem("User Authentication", "User Authentication"));
            ProviderMap.Add(AuthenticationProvider.Celiveo, new ProviderMapItem("Celiveo Authentication", "Celiveo Authentication"));
            ProviderMap.Add(AuthenticationProvider.SafeQ, new ProviderMapItem("YSoft SafeQ", "YSoft SafeQ"));
            ProviderMap.Add(AuthenticationProvider.AutoStore, new ProviderMapItem("AutoStore", "AutoStore"));
            ProviderMap.Add(AuthenticationProvider.PaperCutAgentless, new ProviderMapItem("PaperCut MF", "PaperCut MF"));
            ProviderMap.Add(AuthenticationProvider.UdocxScan, new ProviderMapItem("Udocx Scan", "Udocx Scan"));
            ProviderMap.Add(AuthenticationProvider.GeniusBytesGuest, new ProviderMapItem("Genius Bytes Guest Login", "Genius Bytes Guest Login"));
            ProviderMap.Add(AuthenticationProvider.GeniusBytesManual, new ProviderMapItem("Genius Bytes Manual Login", "Genius Bytes Manual Login"));
            ProviderMap.Add(AuthenticationProvider.GeniusBytesPin, new ProviderMapItem("Genius Bytes PIN Login", "Genius Bytes PIN Login"));
            ProviderMap.Add(AuthenticationProvider.HpRoamPin, new ProviderMapItem("Embedded Badge Authentication", "Embedded Badge Authentication"));
            ProviderMap.Add(AuthenticationProvider.HpId, new ProviderMapItem("HP Login", "HP Login"));
        }

        /// <summary>
        /// Gets the app authenticator for the specified <see cref="AuthenticationProvider" />.
        /// </summary>
        /// <returns><see cref="IAppAuthenticator" /></returns>
        protected override IAppAuthenticator GetAppAuthenticator(AuthenticationProvider provider)
        {
            return JediOmniAppAuthenticatorFactory.Create(provider, ControlPanel, Credential, Pacekeeper);
        }

        /// <summary>
        /// Gets the default authentication setting from the device.
        /// </summary>
        /// <returns><see cref="AuthenticationProvider" /></returns>
        protected override AuthenticationProvider GetDefaultProvider()
        {
            AuthenticationProvider result = AuthenticationProvider.Auto;
            switch (Provider)
            {
                case AuthenticationProvider.Card:
                case AuthenticationProvider.HpRoamPin:
                case AuthenticationProvider.GeniusBytesGuest:
                case AuthenticationProvider.GeniusBytesManual:
                case AuthenticationProvider.GeniusBytesPin:
                case AuthenticationProvider.MyQ:
                case AuthenticationProvider.HpacScan:
                case AuthenticationProvider.HpId:
                    // No need to try and check the device further.
                    result = Provider;
                    break;
                default:                    
                    result = MapDeviceSettingToProvider(GetDeviceAuthSetting());
                    break;
            }
            if (result == AuthenticationProvider.Auto)
            {
                throw new DeviceInvalidOperationException($"Unable to detect provider, {Provider.GetDescription()}.");
            }
            return result;
        }

        /// <summary>
        /// Gets the current Authentication setting on the device.
        /// </summary>
        /// <returns>The setting as a string.</returns>
        private string GetDeviceAuthSetting()
        {
            string value = string.Empty;

            if (ControlPanel != null)
            {
                if (!LazyAuthOnly && _provider != AuthenticationProvider.HpacWindows)
                {
                    if (!ControlPanel.WaitForAvailable(AuthDropdownId))
                    {
                        throw new DeviceWorkflowException($"Authentication screen did not appear within {Math.Round(ControlPanel.DefaultWaitTime.TotalSeconds, 0)} seconds.");
                    }
                    value = ControlPanel.GetValue(AuthDropdownId, "innerText", OmniPropertyType.Property).Trim();
                }
                else
                {
                    value = Provider.GetDescription();
                }
            }
            return value;
        }

        /// <summary>
        /// Map device setting to authentication provider
        /// </summary>
        /// <param name="deviceSetting"></param>
        /// <returns></returns>
        private AuthenticationProvider MapDeviceSettingToProvider(string deviceSetting)
        {
            AuthenticationProvider result = AuthenticationProvider.Auto;
            if (!string.IsNullOrEmpty(deviceSetting))
            {
                if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.HpacWindows].DisplayText))
                {
                    result = AuthenticationProvider.HpacWindows;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.Windows].DisplayText))
                {
                    result = AuthenticationProvider.Windows;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.LocalDevice].DisplayText))
                {
                    result = AuthenticationProvider.LocalDevice;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.LDAP].DisplayText))
                {
                    result = AuthenticationProvider.LDAP;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.DSS].DisplayText))
                {
                    result = AuthenticationProvider.DSS;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.SafeCom].DisplayText))
                {
                    result = AuthenticationProvider.SafeCom;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.SafeComUC].DisplayText))
                {
                    result = AuthenticationProvider.SafeComUC;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.HpacDra].DisplayText))
                {
                    result = AuthenticationProvider.HpacDra;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.HpacIrm].DisplayText))
                {
                    result = AuthenticationProvider.HpacIrm;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.HpacAgentLess].DisplayText))
                {
                    result = AuthenticationProvider.HpacAgentLess;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.HpacPic].DisplayText))
                {
                    result = AuthenticationProvider.HpacPic;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.Equitrac].DisplayText))
                {
                    result = AuthenticationProvider.Equitrac;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.Blueprint].DisplayText))
                {
                    result = AuthenticationProvider.Blueprint;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.PaperCutAgentless].DisplayText))
                {
                    result = AuthenticationProvider.PaperCutAgentless;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.PaperCut].DisplayText))
                {
                    result = AuthenticationProvider.PaperCut;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.ISecStar].DisplayText))
                {
                    result = AuthenticationProvider.ISecStar;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.Celiveo].DisplayText))
                {
                    result = AuthenticationProvider.Celiveo;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.SafeQ].DisplayText))
                {
                    result = AuthenticationProvider.SafeQ;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.AutoStore].DisplayText))
                {
                    result = AuthenticationProvider.AutoStore;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.UdocxScan].DisplayText))
                {
                    result = AuthenticationProvider.UdocxScan;
                }
                else if (deviceSetting.Contains(ProviderMap[AuthenticationProvider.HpRoamPin].DisplayText))
                {
                    result = AuthenticationProvider.HpRoamPin;
                }
            }
            return result;
        }

        /// <summary>
        /// Applies the Provider to the authentication method on the device.
        /// </summary>
        /// <param name="provider"></param>
        protected override void SetDeviceProvider(AuthenticationProvider provider)
        {
            switch (provider)
            {
                case AuthenticationProvider.HpacWindows:
                case AuthenticationProvider.EquitracWindows:
                case AuthenticationProvider.PaperCutAgentless:
                    break;
                default:
                    SetProvider(provider);
                    break;
            }
        }

        private void SetProvider(AuthenticationProvider provider)
        {
            //Check to see if Provider text exists in the dropdown
            XElement dropdownItem = GetDropDownItem(provider);
            if (dropdownItem == null)
            {
                throw new DeviceInvalidOperationException($"Unable to set SignIn method to '{EnumUtil.GetDescription(provider)}'.");
            }

            //Select the item value
            ControlPanel.Press(AuthDropdownId);
            ControlPanel.Press($"#{dropdownItem.FirstAttribute.Value}");
        }

        /// <summary>
        /// We need a way to compare the EXACT text of the dropdown items to the provider text.
        /// Previously, we were using .hp-listitem-text:contains({ProviderMap[provider].SetText} to select the item, 
        /// but this breaks for cases like PaperCut where we have a "PaperCut" auth type and a "PaperCut MF" auth type.
        /// .hp-listitem-text:contains returns 'true' for both cases.  That is the reason for using XElement to evaluate
        /// the HTML.  The other advantage to this design is that we can use the item's id attribute to select it later.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        private XElement GetDropDownItem(AuthenticationProvider provider)
        {
            if (! ControlPanel.CheckState(AuthDropdownId, OmniElementState.VisibleCompletely))
            {
                TimeSpan waitTime = TimeSpan.FromSeconds(10);
                if (!ControlPanel.WaitForState(AuthDropdownId, OmniElementState.VisibleCompletely, waitTime))
                {
                    throw new DeviceWorkflowException($"Authentication dropdown '{AuthDropdownId}' did not become completely visible within {waitTime} seconds.");
                }
            }

            XElement itemElement = null;
            string rawHtml = ControlPanel.GetValue(AuthDropdownId, "outerHTML", OmniPropertyType.Property);
            try
            {
                XElement dropdownElement = XElement.Parse(rawHtml);
                XElement listElement = dropdownElement.Descendants().Where(d => d.Name.LocalName.Equals("ul", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (listElement != null)
                {
                    //foreach (XElement e in listElement.Elements())
                    //{
                    //    System.Diagnostics.Debug.WriteLine($"'{e.Value}'");
                    //}

                    itemElement = listElement.Elements().Where(e => e.Value.Trim().Equals(ProviderMap[provider].SetText, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }
            }
            catch (ArgumentNullException)
            {
            }
            catch (NullReferenceException)
            {
            }

            return itemElement;
        }

    }
}
