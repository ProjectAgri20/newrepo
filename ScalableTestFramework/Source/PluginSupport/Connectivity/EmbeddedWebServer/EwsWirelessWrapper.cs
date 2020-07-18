using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using HP.ScalableTest.DeviceAutomation.Wireless;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;
using HP.ScalableTest.Utility;
using CoreUtility = HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer
{
    public sealed partial class EwsWrapper
    {
        public bool ConfigureWireless(WirelessSettings wirelessSettings, WirelessSecuritySettings securitySettings, ProductType printerInterfaceType = ProductType.None, bool validate = true, bool closeBrowser = true)
        {
            if (securitySettings == null)
            {
                TraceFactory.Logger.Info("Wireless security settings cannot be null.");
                return false;
            }

            if (securitySettings.WirelessConfigurationType == WirelessTypes.Enterprise && securitySettings.EnterpriseSecurity == null)
            {
                TraceFactory.Logger.Info("Enterprise wireless security settings cannot be null for wireless security mode:{0}.".FormatWith(securitySettings.WirelessConfigurationType));
                return false;
            }

            bool result = false;

            try
            {
                TraceFactory.Logger.Info("Configuring Wireless with Settings through printer Web UI:");
                TraceFactory.Logger.Info($"Basic Settings: {wirelessSettings}");
                TraceFactory.Logger.Info($"{securitySettings}");

                #region Install Certificates for Enterprise Wireless

                if (securitySettings.WirelessConfigurationType == WirelessTypes.Enterprise)
                {
                    if (securitySettings.EnterpriseSecurity.InstallCertificates)
                    {
                        if (!InstallCACertificate(securitySettings.EnterpriseSecurity.CACertificatePath))
                        {
                            return false;
                        }

                        if (!InstallIDCertificate(securitySettings.EnterpriseSecurity.IdCertificatePath, securitySettings.EnterpriseSecurity.IdCertificatePassword))
                        {
                            return false;
                        }
                    }

                    if (_adapter.Settings.ProductType == PrinterFamilies.VEP && printerInterfaceType != ProductType.MultipleInterface)
                    {
                        if (!ConfigureCiphers(securitySettings.EnterpriseSecurity.EnterpriseConfiguration.EncryptionStrength))
                        {
                            return false;
                        }
                    }
                }

                #endregion


                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Stop();
                    _adapter.Start();
                }

                #region Wireless Radio

                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Navigate("Wireless_Config");
                    if (!_adapter.IsChecked("WirelessEnable"))
                    {
                        _adapter.Check("WirelessEnable");
                    }
                }
                else
                {
                    SetWireless(wirelessSettings.WirelessRadio, printerInterfaceType);
                }

                #endregion

                #region WirelessBand

				// Wireless band is supported only in VEP-SI and later versions of TPS.
				if ((_adapter.Settings.ProductType == PrinterFamilies.VEP && printerInterfaceType == ProductType.SingleInterface) || _adapter.Settings.ProductType == PrinterFamilies.TPS)
				{
					string wirelessBand = "WirelessBand_2.4GHz";
					switch (wirelessSettings.WirelessBand)
					{
						case WirelessBands.TwoDotFourGHz:
							wirelessBand = "WirelessBand_2.4GHz";
							break;
						case WirelessBands.FiveGHz:
							wirelessBand = "WirelessBand_5GHz";
							break;
						case WirelessBands.Both:
							wirelessBand = "WirelessBand_2.4_5GHz";
							break;
					}

                    // For Accessories the wireless band was not present on SI. Checking the element's presence before making selection.
					if (_adapter.IsElementPresent(wirelessBand))
					{
						_adapter.Check(wirelessBand);
					}

                    CoreUtility.Retry.UntilTrue(() => (!(_adapter.Body.Contains("Retrieving wireless network") || _adapter.Body.Contains("Scanning... "))), 10, TimeSpan.FromSeconds(20));
                }

                #endregion

                #region WirelessModes

                // Wireless modes(bg, bgn etc.) and modes infrastructure and adhoc is supported only in VEP-MI
                if (_adapter.Settings.ProductType == PrinterFamilies.VEP && printerInterfaceType == ProductType.MultipleInterface)
                {
                    _adapter.SelectByValue("WirelessModes", wirelessSettings.WirelessMode.ToString().ToLowerInvariant());

                    if (wirelessSettings.Mode == WirelessStaModes.Infrastructure)
                    {
                        _adapter.Uncheck("WirelessModes_Adhoc");
                    }
                    else
                    {
                        _adapter.Check("WirelessModes_Adhoc");
                    }
                }

                #endregion

                if (wirelessSettings.WirelessConfigurationMethod == WirelessConfigMethods.ExistingNetwork)
                {
                    // Refresh the SSID list so that ssid will be displayed in the list.
                    _adapter.Click("WirelessSSID_Refresh");
                    _adapter.Check("WirelessExistingNetwork");
                    _adapter.SelectByValue("Wireless_SSIDList", wirelessSettings.SsidName);
                }
                else if (wirelessSettings.WirelessConfigurationMethod == WirelessConfigMethods.NetworkName)
                {
                    if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
                    {
                        _adapter.Check("WirelessEnterNetworkName");
                    }
                    else if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
                    {
                        if (!_adapter.IsChecked("WirelessExistingNetwork"))
                        {
                            _adapter.Check("WirelessExistingNetwork");
                        }
                    }
                }

                CoreUtility.Retry.UntilTrue(() => (!(_adapter.Body.Contains("Retrieving wireless network") || _adapter.Body.Contains("Scanning... "))), 20, TimeSpan.FromSeconds(5));

                _adapter.SetText("WirelessNetworkName", wirelessSettings.SsidName);
                Thread.Sleep(TimeSpan.FromSeconds(5));

				if (securitySettings.WirelessAuthentication == WirelessAuthentications.NoSecurity)
				{
					_adapter.Check("WirelessSecurity_None");
					try
					{
						_adapter.Click("Wireless_Apply");
					}
					finally
					{ }
				}

				if (securitySettings.WirelessConfigurationType == WirelessTypes.Personal)
				{
					if (securitySettings.WirelessAuthentication == WirelessAuthentications.Wep)
					{
						ConfigureWepPersonal(wirelessSettings, wepSettings: securitySettings.WEPPersonalSecurity, productType: printerInterfaceType);
					}
					else
					{
						ConfigureWpaPersonal(wirelessSettings, wpaSettings: securitySettings.WPAPersonalSecurity, productType: printerInterfaceType);
					}
				}
				else if (securitySettings.WirelessConfigurationType == WirelessTypes.Enterprise)
				{
					ConfigureEnterpriseWireless(wirelessSettings, securitySettings.WirelessAuthentication, securitySettings.EnterpriseSecurity, printerInterfaceType);
				}
			}
			catch (Exception ex)
			{
				TraceFactory.Logger.Debug(ex.Message);
				return false;
			}
			finally
			{
				#region Validation

                if (validate)
                {
                    //if (expectedAddress != null)
                    //{
                    //	if (NetworkUtil.PingUntilTimeout(expectedAddress, TimeSpan.FromMinutes(1)))
                    //	{
                    //		TraceFactory.Logger.Info("Successfully configured wireless settings through printer Web UI.");
                    //		result = true;
                    //	}
                    //	else
                    //	{
                    //		TraceFactory.Logger.Info("Failed to configure wireless settings through printer Web UI.");
                    //		TraceFactory.Logger.Debug("Ping with IP address: {0} failed.".FormatWith(expectedAddress));
                    //		result = false;
                    //	}
                    //}
                    //else
                    //{
                    //	if (string.IsNullOrEmpty(wirelessMacAddress))
                    //	{
                    //		TraceFactory.Logger.Info("Discovery is not possible as the mac address is not provided.");
                    //		result = false;
                    //	}
                    //	else
                    //	{
                    //		string address = Utility.GetPrinterIPAddress(wirelessMacAddress);
                    //		if (string.IsNullOrEmpty(address))
                    //		{
                    //			TraceFactory.Logger.Info("Failed to configure wireless settings through printer Web UI.");
                    //			TraceFactory.Logger.Debug("Discovery with mac address: {0} failed.".FormatWith(wirelessMacAddress));
                    //			result = false;
                    //		}
                    //		else
                    //		{
                    //			TraceFactory.Logger.Info("Successfully configured wireless settings through printer Web UI.");
                    //			TraceFactory.Logger.Debug("Ping with IP address: {0} successful.".FormatWith(address));
                    //			result = true;
                    //		}
                    //	}
                    //}
                    if (CoreUtility.Retry.UntilTrue(() => (_adapter.Body.Contains("Disconnect the network cable in order to activate the wireless connection") || _adapter.Body.Contains("successfully")), 10, TimeSpan.FromSeconds(2)))
                    {
                        TraceFactory.Logger.Info("Successfully configured wireless settings through printer Web UI.");
                        result = true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to configure wireless settings through printer Web UI.");
                    }
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromMinutes(2));

                    if (closeBrowser)
                    {
                        _adapter.Stop();
                        _adapter.Start();
                    }

                    TraceFactory.Logger.Info("Successfully configured wireless settings through printer Web UI.");
                    TraceFactory.Logger.Debug("Validation is not performed.");
                    result = true;
                }

                #endregion
            }
            return result;
        }

        public void SetWireless(bool enable, ProductType printerInterfaceType = ProductType.None)
        {
            _adapter.Navigate("Wireless_Config");

            if (printerInterfaceType != ProductType.MultipleInterface)
            {
                if (enable)
                {
                    if (!_adapter.IsChecked("WirelessEnable"))
                    {
                        TraceFactory.Logger.Info(SET_DEBUG_OPTION_LOG.FormatWith("Wireless", "enable"));
                        _adapter.Check("WirelessEnable");

                        if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                        {
                            _adapter.Check("WirelessSecurity_None");
                        }
                        _adapter.Click("Wireless_Apply");
                        ClickonConfirmation();

                        if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                        {
                            _adapter.Click("Wireless_Ok");
                        }
                    }
                }
                else
                {
                    TraceFactory.Logger.Info(SET_DEBUG_OPTION_LOG.FormatWith("Wireless", "disable"));
                    if (!_adapter.IsChecked("WirelessDisable"))
                    {
                        _adapter.Check("WirelessDisable");

                        _adapter.Click("Wireless_Apply");
                        ClickonConfirmation();

                        if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                        {
                            _adapter.Click("Wireless_Cancel");
                        }
                    }
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
                CoreUtility.Retry.UntilTrue(() => (!(_adapter.Body.Contains("Retrieving wireless network") || _adapter.Body.Contains("Scanning... "))), 20, TimeSpan.FromSeconds(5));
            }
        }

        public bool GetWireless(ProductType printerInterfaceType = ProductType.None)
        {
            TraceFactory.Logger.Info("Getting wireless radio status from web UI.");
            _adapter.Navigate("Wireless_Config");

            return _adapter.IsChecked("WirelessEnable");
        }

        public List<string> GetSSIDList()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Navigate("Wireless_SetupWizard");
                _adapter.Click("WirelessSetup_Next");

                CoreUtility.Retry.UntilTrue(() => (!_adapter.Body.Contains("Searching for wireless networks..")), 20, TimeSpan.FromSeconds(5));

                if (!_adapter.IsChecked("WirelessSSID_Select"))
                {
                    _adapter.Check("WirelessSSID_Select");
                }

                _adapter.Click("WirelessSSID_Refresh");

                CoreUtility.Retry.UntilTrue(() => (!_adapter.Body.Contains("Searching for wireless networks..")), 20, TimeSpan.FromSeconds(5));

                return _adapter.GetListItems("Wireless_SSIDList");
            }
            else
            {
                SetWireless(true);
                _adapter.Click("WirelessSSID_Refresh");
                CoreUtility.Retry.UntilTrue(() => (!(_adapter.Body.Contains("Retrieving wireless network") || _adapter.Body.Contains("Scanning... "))), 20, TimeSpan.FromSeconds(5));
                _adapter.Click("WirelessSSID_Refresh");
                CoreUtility.Retry.UntilTrue(() => (!(_adapter.Body.Contains("Retrieving wireless network") || _adapter.Body.Contains("Scanning... "))), 20, TimeSpan.FromSeconds(5));

                return _adapter.GetListItems("Wireless_SSIDList");
            }
        }

        private bool ConfigureWepPersonal(WirelessSettings wirelessSettings, WEPPersonalSettings wepSettings, ProductType productType = ProductType.None)
        {
            /*
                    TPS                     VEP-MI                          VEP-SI
             WEP    WEP Persoanl            WEP Personal, WEP Enterprise    NA
             WPA    Personal, Enterprise    Personal, Enterprise            Personal, Enterprise
             WPS    Push, Pin               NA                              NA
            */
            if (_adapter.Settings.ProductType == PrinterFamilies.VEP && productType == ProductType.SingleInterface)
            {
                TraceFactory.Logger.Info("WEP is not supported on VEP-SI.");
                return false;
            }

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP && wirelessSettings.WirelessMode == WirelessModes.Bgn)
            {
                TraceFactory.Logger.Info("WEP security doesnot not supported on BGN mode on VEP.");
                return false;
            }

            CoreUtility.Retry.UntilTrue(() => _adapter.IsElementPresent("WirelessSecurity_WEP"), 10, TimeSpan.FromSeconds(5));
            _adapter.Check("WirelessSecurity_WEP");

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                _adapter.SelectByValue("WirelessSecurity_WEPType", WirelessTypes.Personal.ToString().ToUpper());
            }


            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet || _adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.SelectByValue("WirelessSecurity_WEPKeyIndex", CtcUtility.GetEnumvalue(Enum<WEPIndices>.Value(wepSettings.WEPIndex), _adapter.Settings.ProductType));
            }
            else
            {
                _adapter.SelectDropDown("WirelessSecurity_WEPKeyIndex", CtcUtility.GetEnumvalue(Enum<WEPIndices>.Value(wepSettings.WEPIndex), _adapter.Settings.ProductType));
            }

            _adapter.SetText("WirelessSecurity_WEPKey", wepSettings.WEPKey);

            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.SetText("WirelessSecurity_WEPKey_Confirm", wepSettings.WEPKey);
            }
            try
            {
                _adapter.Click("Wireless_Apply");
            }
            finally
            { }

            return true;
        }

        private bool ConfigureWpaPersonal(WirelessSettings wirelessSettings, WPAPersonalSettings wpaSettings, ProductType productType = ProductType.None)
        {
            /*
                    TPS                     VEP-MI                          VEP-SI
             WEP    WEP Persoanl            WEP Personal, WEP Enterprise    NA
             'WPA    Personal, Enterprise    Personal, Enterprise            Personal, Enterprise
             WPS    Push, Pin               NA                              NA
            */
            CoreUtility.Retry.UntilTrue(() => _adapter.IsElementPresent("WirelessSecurity_WPA"), 20, TimeSpan.FromSeconds(5));
            _adapter.Check("WirelessSecurity_WPA");

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                _adapter.SelectByValue("Wireless_WPAVersion", Enum<WPAVersions>.Value(wpaSettings.Version));
                _adapter.SelectByValue("Wireless_WPAEncryption", Enum<WPAEncryptions>.Value(wpaSettings.Encryption));
            }

            _adapter.Check("WirelessSecurity_WPAPersonal");
            _adapter.SetText("Wireless_WPAPassphrase", wpaSettings.passphrase);

            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.SetText("Wireless_WPAConfirmPassphrase", wpaSettings.passphrase);
            }

            try
            {
                _adapter.Click("Wireless_Apply");
            }
            finally
            { }

            return true;
        }

        private bool ConfigureEnterpriseWireless(WirelessSettings wirelessSettings, WirelessAuthentications authMode, EnterpriseSecuritySettings securitySettings, ProductType productType = ProductType.None)
        {
            if (authMode == WirelessAuthentications.Wep && productType != ProductType.MultipleInterface)
            {
                TraceFactory.Logger.Info("Enterprise WEP is supported only for VEP-MI");
                return false;
            }

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP && wirelessSettings.WirelessMode == WirelessModes.Bgn)
            {
                TraceFactory.Logger.Info("WEP security doesnot not suppor on BGN mode on VEP.");
                return false;
            }

            string securityMode = (authMode == WirelessAuthentications.Wpa) ? "WPA" : "WEP";

            CoreUtility.Retry.UntilTrue(() => _adapter.IsElementPresent("WirelessSecurity_{0}".FormatWith(securityMode)), 20, TimeSpan.FromSeconds(5));
            _adapter.Check("WirelessSecurity_{0}".FormatWith(securityMode));
            _adapter.Check("WirelessSecurity_{0}Enterprise".FormatWith(securityMode));

            _adapter.Uncheck("Wireless_{0}EAPTLS".FormatWith(securityMode));
            _adapter.Uncheck("Wireless_{0}EAPTLS".FormatWith(securityMode));
            _adapter.Uncheck("Wireless_{0}LEAP".FormatWith(securityMode));

            if (securitySettings.EnterpriseConfiguration.AuthenticationProtocol.HasFlag(AuthenticationMode.EAPTLS))
            {
                _adapter.Check("Wireless_{0}EAPTLS".FormatWith(securityMode));
            }

            if (securitySettings.EnterpriseConfiguration.AuthenticationProtocol.HasFlag(AuthenticationMode.PEAP))
            {
                _adapter.Check("Wireless_{0}PEAP".FormatWith(securityMode));
            }

            if (securitySettings.EnterpriseConfiguration.AuthenticationProtocol.HasFlag(AuthenticationMode.LEAP))
            {
                _adapter.Check("Wireless_{0}LEAP".FormatWith(securityMode));
            }

            _adapter.SetText("Wireless_{0}Username".FormatWith(securityMode), securitySettings.EnterpriseConfiguration.UserName);
            _adapter.SetText("Wireless_{0}Password".FormatWith(securityMode), securitySettings.EnterpriseConfiguration.Password);
            _adapter.SetText("Wireless_{0}ConfirmPassword".FormatWith(securityMode), securitySettings.EnterpriseConfiguration.Password);

            if (_adapter.IsElementPresent("Wireless_{0}ReAuthenticate"))
            {
                if (securitySettings.EnterpriseConfiguration.ReAuthenticate)
                {
                    _adapter.Check("Wireless_{0}ReAuthenticate".FormatWith(securityMode));
                }
                else
                {
                    _adapter.Uncheck("Wireless_{0}ReAuthenticate".FormatWith(securityMode));
                }
            }

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS || _adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.SelectByValue("Wireless_WPAEncryptionStrength", CtcUtility.GetEnumvalue(Enum<EncryptionStrengths>.Value(securitySettings.EnterpriseConfiguration.EncryptionStrength), _adapter.Settings.ProductType));
            }

            try
            {
                _adapter.Click("Wireless_Apply");
            }
            finally
            { }

            return true;
        }

        public bool PerformWpsPushEnrollment(bool validateSuccessMessage = false)
        {
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Info("WPS Push method is supported only for TPS.");
                throw new NotSupportedException("WPS Push method is supported only for TPS.");
            }

            TimeSpan duration = TimeSpan.FromMinutes(3);
            DateTime startTime = DateTime.Now;
            TraceFactory.Logger.Debug($"Start Time: {startTime}");

            try
            {
                TraceFactory.Logger.Info("Starting WPS Push on Printer from Web UI.");

                SetWireless(true);

                HP.ScalableTest.Utility.Retry.UntilTrue(() => _adapter.IsElementPresent("Wireless_WPS"), 20, TimeSpan.FromSeconds(5));
                _adapter.Check("Wireless_WPS");
                _adapter.Check("Wireless_WPSPushMethod");
                _adapter.Click("Wireless_Apply");

                HP.ScalableTest.Utility.Retry.UntilTrue(() => !_adapter.Body.Contains("Connecting"), 12, TimeSpan.FromSeconds(20));

                if (validateSuccessMessage)
                {
                    if (_adapter.Body.Contains("Disconnect the network cable", StringComparison.CurrentCultureIgnoreCase) || _adapter.Body.Contains("Connected"))
                    {
                        TraceFactory.Logger.Info("WPS Push configuration is successful on Printer.");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("WPS Push configuration failed on Printer.");
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("WPS Pin configuration is completed from printer Web UI.");
                    return true;
                }
            }
            finally
            {
                while ((DateTime.Now.Subtract(startTime).TotalSeconds <= duration.TotalSeconds))
                { }

                StopAdapter();
                _adapter.Start();
                TraceFactory.Logger.Debug($"Start Time: {DateTime.Now}");
            }
        }

        public string GenerateWpsPin()
        {
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Info("WPS Pin method is supported only for TPS.");
                throw new NotSupportedException("WPS Push method is supported only for TPS.");
            }

            TraceFactory.Logger.Info("Generating WPS pin from web UI.");

            SetWireless(true);

            HP.ScalableTest.Utility.Retry.UntilTrue(() => _adapter.IsElementPresent("Wireless_WPS"), 20, TimeSpan.FromSeconds(5));

            _adapter.Check("Wireless_WPS");
            _adapter.Check("Wireless_WPSPinMethod");

            string pin = string.Empty;
            if (!HP.ScalableTest.Utility.Retry.UntilTrue(() => !string.IsNullOrEmpty(pin = _adapter.GetText("Wireless_WPSPin")), 5, TimeSpan.FromSeconds(20)))
            {
                TraceFactory.Logger.Info("WPS Pin is not generated.");
                return string.Empty;
            }
            else
            {
                pin = pin.Split(':')[1].Trim();
                TraceFactory.Logger.Info("WPS Pin: {0} is generated.".FormatWith(pin));
                return pin;
            }
        }

        public bool StartWpsPinEnrollment(bool validateSuccessMessage = false)
        {
            TimeSpan duration = TimeSpan.FromMinutes(5);
            DateTime startTime = DateTime.Now;

            TraceFactory.Logger.Debug($"Start Time: {startTime}");

            try
            {
                TraceFactory.Logger.Info("Starting WPS Pin on Printer from Web UI.");

                _adapter.Click("Wireless_Apply");
                HP.ScalableTest.Utility.Retry.UntilTrue(() => !_adapter.Body.Contains("Connecting"), 12, TimeSpan.FromSeconds(30));

                if (validateSuccessMessage)
                {
                    if (_adapter.Body.Contains("Disconnect the network cable", StringComparison.CurrentCultureIgnoreCase) || _adapter.Body.Contains("Connected"))
                    {
                        TraceFactory.Logger.Info("WPS Push configuration is successful on Printer.");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("WPS Push configuration failed on Printer.");
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("WPS Pin configuration is completed from printer EWS page.");
                    return true;
                }
            }
            finally
            {
                while ((DateTime.Now.Subtract(startTime).TotalSeconds <= duration.TotalSeconds))
                { }

                StopAdapter();
                _adapter.Start();
                TraceFactory.Logger.Debug($"Start Time: {DateTime.Now}");
            }
        }

        public string GetEnterpriseWirelessUserName()
        {
            SetWireless(true);

            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                if (!_adapter.IsChecked("WirelessExistingNetwork"))
                {
                    _adapter.Check("WirelessExistingNetwork");
                }
            }

            _adapter.SetText("WirelessNetworkName", "abcdefg");
            HP.ScalableTest.Utility.Retry.UntilTrue(() => _adapter.IsElementPresent("WirelessSecurity_WPA"), 5, TimeSpan.FromSeconds(10));
            _adapter.Check("WirelessSecurity_WPA");
            HP.ScalableTest.Utility.Retry.UntilTrue(() => _adapter.IsElementPresent("WirelessSecurity_WPA"), 5, TimeSpan.FromSeconds(10));
            _adapter.Check("WirelessSecurity_WPAEnterprise");

            return _adapter.GetValue("Wireless_WPAUsername");
        }

        public void RestoreWirelessSettings()
        {
            TraceFactory.Logger.Info("Restoring Wireless Settings.");
            _adapter.Navigate("Wireless_Config");
            _adapter.Check("WirelessSecurity_WPA");
            _adapter.Check("WirelessSecurity_WPAEnterprise");
            _adapter.Click("Wireless_Restore");
            _adapter.Click("Wireless_Restore_OK");
        }

        #region WiFiDirect
        /// <summary>
        /// Configuring WiFiDirect
        /// </summary>
        /// <param name="connectionMethod">connectionMethod</param>   
        /// <param name="password">password</param>  
        public bool ConfigureWiFiDirect(string connectionMethod = null, string password = null)
        {
            connectionMethod = ((_adapter.Settings.ProductType == PrinterFamilies.TPS || _adapter.Settings.ProductType == PrinterFamilies.InkJet) && (connectionMethod.EqualsIgnoreCase(WiFiDirectConnectionMode.Auto.ToString()))) ? WiFiDirectConnectionMode.Automatic.ToString() : connectionMethod;
            try
            {
                TraceFactory.Logger.Info("Configuring WiFiDirect with Connection: {0}".FormatWith(connectionMethod));
                _adapter.Navigate("WiFiDirect");
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Click("EditSettings");
                }
                _adapter.SelectDropDown("ConnectionMethod", connectionMethod);
                _adapter.SetText("Password", password);
                _adapter.Click("Apply");

                if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
                {
                    if (_adapter.SearchText("Success"))
                    {
                        TraceFactory.Logger.Info("Successfully configured WiFiDirect with Connection:{0}".FormatWith(connectionMethod));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Fail to configure WiFiDirect");
                        return false;
                    }
                }
                else
                {
                    if (!(_adapter.Settings.ProductType == PrinterFamilies.InkJet))
                    {
                        if (ValidateSSIDName(connectionMethod))
                        {
                            TraceFactory.Logger.Info("Successfully configured WiFiDirect with Connection:{0}".FormatWith(connectionMethod));
                            return true;
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Fail to configure WiFiDirect");
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception exceptionwifi)
            {
                TraceFactory.Logger.Debug(exceptionwifi.Message);
                return false;
            }
            finally
            {
                _adapter.Stop();
                _adapter.Start();
            }
        }

        /// <summary>
        /// Setting WiFi SSID
        /// </summary>
        /// <param name="SSID">SSID created on the access point</param>   
        public bool SetWiFiSSID(string SSID)
        {
            TraceFactory.Logger.Info("Setting WiFi SSID to: {0}".FormatWith(SSID));
            _adapter.Navigate("WiFiDirect");
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Click("EditSettings");
            }
            _adapter.SetText("SSIDName", SSID);
            _adapter.Click("Apply");

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                if (_adapter.SearchText("Success"))
                {
                    TraceFactory.Logger.Info("Successfully updated WiFiDirect SSID with :{0}".FormatWith(SSID));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Fail to update WiFiDirect SSID");
                    return false;
                }
            }
            else
            {
                Thread.Sleep(TimeSpan.FromSeconds(10));
                if (ValidateSSIDName(SSID))
                {
                    TraceFactory.Logger.Info("Successfully updated WiFiDirect SSID with :{0}".FormatWith(SSID));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Fail to update WiFiDirect SSID");
                    return false;
                }
            }
        }

        /// <summary>
        /// Get WiFi SSID
        /// </summary>        
        public string GetWiFiSSID()
        {
            TraceFactory.Logger.Info("Getting WiFi SSID");
            _adapter.Navigate("WiFiDirect");
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Click("EditSettings");
            }
            return _adapter.GetText("SSIDName");
        }

        /// <summary>
        /// Get WiFi Connection mode
        /// </summary>        
        public string GetWiFiConnectionMode()
        {
            TraceFactory.Logger.Info("Getting WiFi Connection mode");
            _adapter.Navigate("WiFiDirect");
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Click("EditSettings");
            }
            return _adapter.GetValue("ConnectionMethod");
        }

        /// <summary>
        /// Set Wireless Station
        /// </summary>
        /// <param name="SSID">SSID created on the access point</param>   
        public bool SetWirelessSTA(bool valueToSet)
        {
            TraceFactory.Logger.Info("Setting Wireless STA Mode to:{0}".FormatWith(valueToSet));
            try
            {
                _adapter.Navigate("Wireless_Config");
                if (valueToSet)
                {
                    _adapter.Check("WirelessEnable");
                    if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                    {
                        _adapter.SetText("WirelessNetworkName", "WiFIDirect");
                    }
                }
                else
                {
                    _adapter.Check("WirelessDisable");
                }
                _adapter.Click("Wireless_Apply");

                if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
                {
                    if (_adapter.SearchText("Success"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    ClickonConfirmation();
                    _adapter.Stop();
                    _adapter.Start();
                    return true;
                }
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info("Exception Caught: {0}".FormatWith(wirelessException.Message));
                return false;
            }
        }

        /// <summary>
        /// Enable WiFi Direct Advanced Options
        /// </summary> 
        public bool SetWiFiAdvancedOptions(bool broadCast = true, bool showName = true, bool showPassword = true)
        {
            TraceFactory.Logger.Info("Setting WiFi Direct Advanced Options");
            try
            {
                _adapter.Navigate("WiFiDirect");
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Click("EditSettings");
                }
                if (broadCast)
                {
                    _adapter.Check("DoNotBroadcast");
                }
                else
                {
                    _adapter.Uncheck("DoNotBroadcast");
                }

                if (showName)
                {
                    _adapter.Check("DoNotShowName");
                }
                else
                {
                    _adapter.Uncheck("DoNotShowName");
                }
                if (showPassword)
                {
                    _adapter.Check("DoNotShowPassword");
                }
                else
                {
                    _adapter.Uncheck("DoNotShowPassword");
                }
                _adapter.Click("Apply");
                if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
                {
                    if (_adapter.SearchText("Success"))
                    {
                        TraceFactory.Logger.Info("Successfully set WiFi Direct Advanced options");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to set WiFi Direct Advanced options");
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Successfully set WiFi Direct Advanced options");
                    return true;
                }
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info("Exception Caught: {0}".FormatWith(wirelessException.Message));
                return false;
            }
            finally
            {
                _adapter.Stop();
                _adapter.Start();
            }
        }


        /// <summary>
        /// Get the SSID List from Wireless Wizard page
        /// </summary>
        /// <returns></returns>
        public List<string> GetWirelessWizardSSIDList()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                _adapter.Navigate("Wireless_Config");
                _adapter.Click("WirelessWizard");
                _adapter.Click("RefreshWizard");
                Thread.Sleep(TimeSpan.FromSeconds(10));
                return _adapter.GetListItems("SSID_list_wizard");
            }
            else
            {
                _adapter.Navigate("Wireless_SetupWizard");
                _adapter.Click("WirelessSetup_Next");
                _adapter.Click("WirelessSSID_Refresh");
                Thread.Sleep(TimeSpan.FromSeconds(10));
                return _adapter.GetListItems("Wireless_SSIDList");
            }
        }

        /// <summary>
        /// Configure Wireless Wizard
        /// </summary>
        /// <returns></returns>
        public bool ConfigureWirelessWizard(string SSID, string passPhrase = "12345678")
        {
            TraceFactory.Logger.Info("Configuring Wireless Wizard");
            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                _adapter.Navigate("Wireless_Config");
                _adapter.Click("WirelessWizard");
                _adapter.Click("WirelessWizard_Mode");
                _adapter.SetText("WizardEnterName_text", SSID);
                _adapter.Click("WizardNext");
                _adapter.Click("Wizard_WPA");
                _adapter.Click("WizardNext");
                _adapter.SetText("Wizard_WPAPassphrase", passPhrase);
                _adapter.Click("WizardNext");
                _adapter.Click("WizardFinish");
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
            else
            {
                _adapter.Navigate("Wireless_SetupWizard");
                _adapter.Click("WirelessSetup_Next");
                _adapter.Click("Wireless_Mode");
                _adapter.SetText("Wireless_EnterSSID", SSID);
                _adapter.Click("Wireless_Next");
                _adapter.Click("Wireless_WPAMode");
                _adapter.SetText("Wireless_WPAPassword", passPhrase);
                _adapter.Click("Wireless_Next");
                _adapter.Click("Wireless_Finish");
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
            if (!(_adapter.SearchText("success") || _adapter.SearchText("Enabled")))
            {
                TraceFactory.Logger.Info("Fail to configure WirelessWizard");
                return false;
            }
            TraceFactory.Logger.Info("Successfully configured WirelessWizard");
            return true;
        }


        /// <summary>
        /// Validate admin password in Wifi Direct Page
        /// </summary>
        /// <returns></returns>
        public bool ValidateAdminPasswordinWiFiPage(string password)
        {
            TraceFactory.Logger.Info("Validating the admin password pop up in Wifi Direct Page");
            _adapter.Navigate("WiFiDirect");

            string userName = _adapter.Settings.ProductType != PrinterFamilies.InkJet ? password : "admin";
            string executablePath = @"{0}\{1}".FormatWith(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "Authenticate.exe");

            var result = ScalableTest.Utility.ProcessUtil.Execute("cmd.exe", "/C \"{0}\" {1} {2}".FormatWith(executablePath, userName, password));
            if (result.ExitCode == 0)
            {
                TraceFactory.Logger.Debug("Authentication popup is present.");
                return true;
            }
            else if (result.ExitCode == 1)
            {
                TraceFactory.Logger.Debug("No authentication popup is present.");
                return false;
            }
            return true;
        }
        #endregion
    }

    public class WirelessSettings
    {
        /// <summary>
        /// A flag to turn wireless on or off
        /// </summary>
        public bool WirelessRadio { get; set; }

        public WirelessBands WirelessBand { get; set; } = WirelessBands.Both;

        public WirelessModes WirelessMode { get; set; } = WirelessModes.Bg;

        public WirelessStaModes Mode { get; set; } = WirelessStaModes.Infrastructure;

        public WirelessConfigMethods WirelessConfigurationMethod { get; set; } = WirelessConfigMethods.NetworkName;

        public string SsidName { get; set; }

        public override string ToString()
        {
            return $"Basic Settings-Wireless Band: {Enum<WirelessBands>.Value(WirelessBand)}, Wireless Mode: {WirelessMode}, Mode: {Mode}, SSID: {SsidName}.";
        }
    }

    public enum WirelessConfigMethods
    {
        ExistingNetwork,
        NetworkName
    }

    public enum WPSMethods
    {
        WPSPin,
        WPSPush
    }

    #region WEP

    public enum WEPModes
    {
        [EnumValue("open")]
        Open = 1,
        [EnumValue("shared")]
        Shared = 2,
        [EnumValue("openThenShared")]
        Auto
    }

    public enum WEPIndices
    {
        [EnumValue("Key 1||1||1||index1")]
        Key1 = 0,
        [EnumValue("Key 2||2||2||index2")]
        Key2 = 1,
        [EnumValue("Key 3||3||3||index3")]
        Key3 = 2,
        [EnumValue("Key 4||4||4||index4")]
        Key4 = 3
    }

    #endregion

    #region WPA

    public enum WirelessTypes
    {
        None,
        Personal,
        Enterprise
    }

    public enum WPAVersions
    {
        [EnumValue("")]
        None = 0,
        [EnumValue("Wpa2")]
        WPA2 = 3,
        [EnumValue("Auto")]
        Auto = 4
    }

    public enum WPAEncryptions
    {
        [EnumValue("")]
        None = 0,
        [EnumValue("AES")]
        AES = 3,
        [EnumValue("AUTOENC")]
        AUTO = 4
    }

    #endregion

	public class WirelessSecuritySettings
	{
		public WirelessAuthentications WirelessAuthentication { get; set; }

		public WirelessTypes WirelessConfigurationType { get; set; }

		public WEPPersonalSettings WEPPersonalSecurity { get; set; }

		public WPAPersonalSettings WPAPersonalSecurity { get; set; }

		public EnterpriseSecuritySettings EnterpriseSecurity { get; set; }

		public WirelessSecuritySettings()
		{
			WirelessAuthentication = WirelessAuthentications.NoSecurity;
		}

		public WirelessSecuritySettings(WPAPersonalSettings wpaPersonalSettings)
		{
			WirelessAuthentication = WirelessAuthentications.Wpa;
			WirelessConfigurationType = WirelessTypes.Personal;
			WPAPersonalSecurity = wpaPersonalSettings;
		}

        public WirelessSecuritySettings(WEPPersonalSettings wepPersonalSettings)
        {
            WirelessAuthentication = WirelessAuthentications.Wep;
            WirelessConfigurationType = WirelessTypes.Personal;
            WEPPersonalSecurity = wepPersonalSettings;
        }

		public WirelessSecuritySettings(WirelessAuthentications mode, EnterpriseSecuritySettings enterpriseSettings)
		{
			WirelessAuthentication = mode;
			WirelessConfigurationType = WirelessTypes.Enterprise;
			EnterpriseSecurity = enterpriseSettings;
		}

		public override string ToString()
		{
			string wirelessSecuritySettings = string.Empty;

            switch (WirelessAuthentication)
            {
                case WirelessAuthentications.NoSecurity:
                    wirelessSecuritySettings = "No Security";
                    break;
                case WirelessAuthentications.Wpa:

					if (WirelessConfigurationType == WirelessTypes.Personal)
					{
                        wirelessSecuritySettings = "Security Settings: WPA Personal, {0}".FormatWith(WPAPersonalSecurity);
                    }
                    else
                    {
                        wirelessSecuritySettings = "Security Settings: WPA Enterprise, {0}".FormatWith(EnterpriseSecurity);
                    }

                    break;
                case WirelessAuthentications.Wep:
                    if (WirelessConfigurationType == WirelessTypes.Personal)
                    {
                        wirelessSecuritySettings = "Security Settings: WEP Personal, {0}".FormatWith(WEPPersonalSecurity);
                    }
                    else
                    {
                        wirelessSecuritySettings = "Security Settings: WEP Enterprise, {0}".FormatWith(EnterpriseSecurity);
                    }

                    break;
            }

            return wirelessSecuritySettings;
        }
    }

    public class WEPPersonalSettings
    {
        public WEPModes WEPMode { get; set; }

        public WEPIndices WEPIndex { get; set; }

        public string WEPKey { get; set; }

        public override string ToString()
        {
            return "Mode: {0}, Index: {1}, Key: {2}".FormatWith(WEPMode, WEPIndex, WEPKey);
        }
    }

    public class WPAPersonalSettings
    {
        public WPAVersions Version { get; set; }

        public WPAEncryptions Encryption { get; set; }

        public string passphrase { get; set; }

        public override string ToString()
        {
            TraceFactory.Logger.Info("Version: {0}, Encryption: {1}, {2}: {3}".FormatWith(Version, Encryption, passphrase.Length == 8 ? "Passphrase" : "Pre Shared Key", passphrase));
            return "Version: {0}, Encryption: {1}, {2}: {3}".FormatWith(Version, Encryption, passphrase.Length == 8 ? "Passphrase" : "Pre Shared Key", passphrase);
        }
    }

    public class EnterpriseSecuritySettings
    {
        public WPAVersions Version { get; set; }

        public WPAEncryptions Encryption { get; set; }

        public Dot1XConfigurationDetails EnterpriseConfiguration { get; set; }

        public string CACertificatePath { get; set; }

        public string IdCertificatePath { get; set; }

        public string IdCertificatePassword { get; set; }

        public bool InstallCertificates { get; set; } = true;

        public override string ToString()
        {
            if (Version == WPAVersions.None)
            {
                return $"{EnterpriseConfiguration}, CA certificate path: {CACertificatePath}, Id Certificate: {IdCertificatePath}, Id Certificate password: {IdCertificatePassword}";
            }
            else
            {
                return $"{EnterpriseConfiguration}, WPA Version:{Version}, WPA Encryption: {Encryption}, CA certificate path: {CACertificatePath}, Id Certificate: {IdCertificatePath}, Id Certificate password: {IdCertificatePassword}";
            }
        }
    }
}
