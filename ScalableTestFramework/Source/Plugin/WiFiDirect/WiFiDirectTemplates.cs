using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using HP.DeviceAutomation.AccessPoint;
using HP.ScalableTest.DeviceAutomation.Wireless;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Discovery;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Wireless;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.WiFiDirect
{
    public static class WiFiDirectTemplates
    {
        #region Constants
        private const string WpaPassphrase = "12345678";
        private const string WpaPresharedkey = "485BFE660D5F61003E79CD7EBB485BFE660D5F61003E79CD7EBBAA8465E6A4A";
        private const string WifiautoPassword = "12345678";
        private const string WifiadvancedPassword = "directpassword";
        private const string Wifidirectip = "192.168.223.1";
        private const string WifiadminPassword = "WiFiAdmin@123";

        #endregion

        #region Wifi-Direct

        /// <summary>
        /// 756470 Verify that wireless direct or Wifi Direct security settings cannot be changed when system password is set.	
        /// Enable wireless direct with security.
        /// Set password for printer.
        /// Try to change wireless direct security settings.
        /// Note : Perform this test case for Wifi Direct if supported on printer.
        /// Send a print job after every successful connection.
        /// Expected: Wireless diect security settings should be available for editing only if printer password is provided.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool ValidateWiFiWithSystemPassword(WiFiDirectActivityData activityData, int testNo)
        {
            if (!TestWiFiPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                if (!AssociateWiFiDirect(activityData, WiFiDirectConnectionMode.Auto, WifiautoPassword, testNo))
                {
                    return false;
                }

                //Setting Printer Password            
                if (!EwsWrapper.Instance().SetAdminPassword(WifiadminPassword))
                {
                    return false;
                }

                EwsWrapper.Instance().StopAdapter();
                EwsWrapper.Instance().Start();

                TraceFactory.Logger.Info(" After enabling Printer Password, updating  wireless direct security settings");
                if (!EwsWrapper.Instance().ValidateAdminPasswordinWiFiPage(WifiadminPassword))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().Login(WifiadminPassword))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SearchTextInPage("Connected Clients", "WiFiDirect"))
                {
                    TraceFactory.Logger.Info("Fail to edit the wireless direct security settings even after providing printer password");
                    return false;
                }
                TraceFactory.Logger.Info("Able to edit the wireless direct security settings after providing printer password");
                return true;
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info(wirelessException.Message);
                return false;
            }
            finally
            {
                EwsWrapper.Instance().Login(WifiadminPassword);
                EwsWrapper.Instance().DeleteAdminPassword(WifiadminPassword);
                Printer printer = PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(Wifidirectip));
                printer.PowerCycle();
                TestWiFiPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 756503 Wifi Direct status on Restore factory default
        ///1.Configure Wifi direct for different settings.
        ///2. Perform Restore Factory default on printer.
        ///3. Verify Wifi Direct Status from EWS page.
        /// Expected: Any clients connected to the WFD will get disconnected immediately on Restore default.
        /// Input from B&M: The updated SSID will go off and default SSID exists after restore default
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool ValidateWiFiWithRestoreFactory(WiFiDirectActivityData activityData, int testNo)
        {
            if (!TestWiFiPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                Printer printer = PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.PrimaryInterfaceAddress));
                string UpdatedSSID = "WiFiDirect123";

                TraceFactory.Logger.Info("Changing the default wifi direct settings for SSID");
                if (!EwsWrapper.Instance().SetWiFiSSID(UpdatedSSID))
                {
                    return false;
                }
                if (activityData.ProductFamily != ProductFamilies.InkJet)
                {
                    if (!EwsWrapper.Instance().RestoreSecurityDefaults())
                    {
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("For Ink Product restore default setting is not clearing the wifi settings, hence power cycling");
                    printer.PowerCycle();
                }

                Thread.Sleep(TimeSpan.FromSeconds(10));
                TraceFactory.Logger.Info("Validating for the default value of SSID after restore default");

                if (UpdatedSSID == EwsWrapper.Instance().GetWiFiSSID())
                {
                    TraceFactory.Logger.Info("The Updated SSID still exists in Printer even after Restore Defaults");
                    return false;
                }
                TraceFactory.Logger.Info("The Default SSID exists after Restore Defaults");
                return true;
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info(wirelessException.Message);
                return false;
            }
            finally
            {
                TestWiFiPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 756409 Wireless Direct or Wifi Direct -Channel configuration- AP running on default channel
        ///Step:Verify AP is configured with the default channel
        ///Go to Networking menu -> Wireless Setup -> Disable STA[Printer Wifi]
        ///Go to Networking menu -> Wireless Direct and enable wireless direct
        ///Verify the AP channel
        /// Expected: AP to run on default Channel 6 when sta is disabled
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool ValidateChannelWithWirelessSta(WiFiDirectActivityData activityData, int testNo)
        {
            if (!TestWiFiPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Disabling STA and Validating Channel in WiFiDirect");
                if (!EwsWrapper.Instance().SetWirelessSTA(false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating the Channel after disabling Wireless STA");
                if (!SnmpWrapper.Instance().GetWIFIChannel().EqualsIgnoreCase("6"))
                {
                    TraceFactory.Logger.Info("The Micro AP is not running on the default channel 6");
                    return false;
                }
                TraceFactory.Logger.Info("The Micro AP is running on the default channel 6");
                return true;
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info(wirelessException.Message);
                return false;
            }
            finally
            {
                EwsWrapper.Instance().SetWirelessSTA(true);
                TestWiFiPostRequisites(activityData);
            }
        }

        ///// <summary>
        ///// 756408 Wireless Direct or Wifi Direct -Channel configuration- AP channel to be  accordingly with STA channel
        /////Go to Networking menu -> Wireless Setup -> Associate STA with an AP running in channel 1 
        /////Go to Networking menu -> Wireless Direct and enable wireless direct Verify the AP channel 
        /////Change the channel of the STA associated AP from 1-13
        ///// Expected: AP to run on the same channel as sta , in this case it is changed according to the new STA channel
        ///// </summary>
        ///// <param name="activityData"></param>
        ///// <param name="testNo"></param>
        ///// <returns></returns>
        //public static bool ValidateWiFiChannelWithSta(WiFiDirectActivityData activityData, int testNo)
        //{
        //    if (!TestWiFiPreRequisites(activityData))
        //    {
        //        return false;
        //    }
        //    try
        //    {
        //        TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
        //        int channelNumber = activityData.AccessPointDetails.FirstOrDefault().Vendor == AccessPointManufacturer.DLink ? 15 : 1;

        //        TraceFactory.Logger.Info("Associating Printer with Wireless AcessPoint and updating the channel no in accesspoint to :{0}".FormatWith(channelNumber));
        //        Profile accessPointProfile = GetAccessPointProfile(activityData);

        //        WirelessSettings settings = new WirelessSettings()
        //        { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

        //        WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();

        //        if (!TestWireless(activityData, accessPointProfile, settings, securitySettings, testNo))
        //        {
        //            TraceFactory.Logger.Info("Failed to associate the printer with AccessPoint");
        //            return false;
        //        }

        //        TraceFactory.Logger.Info("Successfully updated the channel number in Accesspoint");
        //        Thread.Sleep(TimeSpan.FromMinutes(1));

        //        TraceFactory.Logger.Info("Validating the Channel number in WiFI Direct");

        //        if (!SnmpWrapper.Instance().GetWIFIChannel().Equals(channelNumber))
        //        {
        //            TraceFactory.Logger.Info("The Micro AP has not updated with the Accesspoint Channel Number");
        //            return false;
        //        }
        //        TraceFactory.Logger.Info("The Micro AP has updated with the Accesspoint Channel Number");
        //        return true;
        //    }
        //    catch (Exception wirelessException)
        //    {
        //        TraceFactory.Logger.Info(wirelessException.Message);
        //        return false;
        //    }
        //    finally
        //    {
        //        TestWiFiPostRequisites(activityData);
        //    }
        //}

        /// <summary>
        /// 756510 Wifi Direct Association in Advanced connection type.
        ///Step1: 1. Enable Wifi Direct in advanced conenction mode.
        ///2. Configure PAssword for Wifi Direct network. (Min 8 to max 63)
        ///3 Disable checkboxes : 
        ///a.Do not broadcast the Wifi Direct name
        ///b.Do not printer's Wifi Direct name on reports on or printer control panel.
        ///c.Do not show wifi direct password on reports or on printer control panel.
        ///4. Apply settings and try to associate Client device (WFD/WFDS device)
        ///5. Verify Client gets associated using correct Password.
        ///6. Verify Wifi Direct Name is broadcasted.Name shoukd be<HP-Printer-XY- ....>
        ///7. Verify Password is visible on EWS page/CP
        ///8. Verify SSID is shown on Reports and CP.
        ///4. Verify client can not use Wifi Direct Services to print but remain associated to the Printer.
        ///Note : Verify This test case with different Passwords manually confired from EWS min 8 to max 63 chars.
        ///5. Verify Legacy client can print successfully with printer using Wireless direct
        /// Expected: 1. User should be able to configure customised settings in Advanced mode.
        ///2. Printer's SSID is changed to HP-Print-XY-**** whre is XY is last two chars from Wireless MAC address. 
        ///3. Printer's Wifi Direct name is shown on Reports and CP
        ///4. Printer WIFI direct name/SSID is broadcasted and gets listed in the client's Wfif connection search list.
        ///5. Printer's Wifi Direct Password is visible on Reports and CP.
        ///6. Clients should be able to print successfully using APP /in OS
        ///Step2: 1. Enable Wifi Direct in advanced conenction mode.
        ///2. Configure PAssword for Wifi Direct network. (Min 8 to max 63)
        ///3 Enable checkboxes : 
        ///a.Do not broadcast the Wifi Direct name
        ///b.Do not printer's Wifi Direct name on reports on or printer control panel.
        ///c.Do not show wifi direct password on reports or on printer control panel.
        ///4. Apply settings and try to associate Client device (WFD/WFDS/Legacy device)
        ///5. Verify Client gets associated using correct Password. (Associate client to Printer's SSID by seaching it manually with seacrh option from Device since broadcast in above step is disabled. )
        ///6. Verify Wifi Direct Name is broadcasted.Name shoukd be<HP-Printer-XY- ....>
        ///7. Verify Password is visible on EWS page/CP
        ///8. Verify SSID is shown on Reports and CP.
        ///9. Verify Legacy client can not use Wifi Direct Services to print but remain associated to the Printer.
        ///10. Verify WFD/WFDS clients get connected using PAssword prints successfully.
        ///Note : Verify This test case with different Passwords manually confired from EWS min 8 to max 63 chars.
        ///Verify Legacy client can print successfully with printer using Wireless direct
        ///Expected: .Printer's Wifi direct name in Adavanced mode will be changed to HP-Print-XY-****.
        ///2. Since all Do not broadcast Printer's name is checked, Printer's Wifi direct SSID should be broadcasted and should not get listed in client's search list.
        ///3. Using Manual Search option to search printer's SSID should be able to locate printer and associate client to printer with correct PAssword.
        ///4. Printers' SSID and password is not shown in Reports or CP.
        ///5. Max 5 clients can be connected at a time.
        ///6. Printer from the clients using WFD/WFDS/Wireless direct should be successful depending on type of the connected client.(using App/In OS)
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool ValidateWiFiAdvancedOptions(WiFiDirectActivityData activityData, int testNo)
        {
            if (!TestWiFiPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                bool flag = false;
                var conn = new Wifi();

                TraceFactory.Logger.Info("Enabling Advanced Option with Password in Printer");
                if (!AssociateWiFiDirect(activityData, WiFiDirectConnectionMode.Advanced, WifiadvancedPassword, testNo))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step1:Validating the Wifi Direct behaviour after disabling all WiFi Direct Advanced Options"));

                TraceFactory.Logger.Info("Validating the Wifi Direct name on Reports");
                var ssid = SnmpWrapper.Instance().GetWIFISSID();
                if (!EwsWrapper.Instance().ValidateSSIDName(ssid))
                {
                    TraceFactory.Logger.Info("Even after disabling the option 'Do not show the name on Reports', still SSID is not displayed");
                    return false;
                }
                TraceFactory.Logger.Info("Since the option 'Do not show the name on Reports' is disabled, able to view the SSID Name in the configuration page of the Printer");

                TraceFactory.Logger.Info("Validating the Wifi Direct name broadcasting");
                var wirelessNetworks = conn.GetWirelessNetworkConnections();
                foreach (var wirelessNetwork in wirelessNetworks)
                {
                    if (wirelessNetwork.Name == ssid)
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    TraceFactory.Logger.Info("Even after disabling the option 'Do not broadcast', still SSID is not able access from Client");
                    return false;
                }
                TraceFactory.Logger.Info("Since the option 'Do not broadcast' is disabled, able to view the SSID Name in the configuration page of the Printer");

                if (activityData.ProductFamily == ProductFamilies.TPS)
                {
                    TraceFactory.Logger.Info("Validating the Wifi Direct Password on Reports");
                    if (!EwsWrapper.Instance().ValidateSSIDName(WifiadvancedPassword))
                    {
                        TraceFactory.Logger.Info("Even after disabling the option 'Do not show Password on Reports', still Password is not displayed");
                        return false;
                    }
                    TraceFactory.Logger.Info("Since the option 'Do not show Password on Reports' is disabled, able to view the Password in the configuration page of the Printer");
                }
                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step2:Validating the Wifi Direct behaviour after enabling all WiFi Direct Advanced Options"));

                TraceFactory.Logger.Info("Enabling the advanced options");
                if (!EwsWrapper.Instance().SetWiFiAdvancedOptions())
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));
                if (activityData.ProductFamily != ProductFamilies.VEP)
                {
                    TraceFactory.Logger.Info("Validating the Wifi Direct name on Reports");
                    if (EwsWrapper.Instance().ValidateSSIDName(ssid))
                    {
                        TraceFactory.Logger.Info("Even after enabling the option 'Do not show the name on Reports', still SSID is displayed");
                        return false;
                    }
                    TraceFactory.Logger.Info("Since the option 'Do not show the name on Reports' is enabled, SSID Name is not displayed in the configuration page of the Printer");
                }
                else
                {
                    //TODO: Need to validate the same from control panel for VEP
                }

                TraceFactory.Logger.Info("Validating the Wifi Direct name broadcasting");
                string clientNetworkName = CtcUtility.GetClientNetworkName("192.168.223.100");
                NetworkUtil.DisableNetworkConnection(clientNetworkName);
                NetworkUtil.EnableNetworkConnection(clientNetworkName);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                wirelessNetworks = conn.GetWirelessNetworkConnections();
                flag = false;
                foreach (WirelessNetwork wirelessNetwork in wirelessNetworks)
                {
                    if (wirelessNetwork.Name == ssid)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    TraceFactory.Logger.Info("Even after enabling the option 'Do not broadcast', still SSID is able access from Client");
                    return false;
                }
                TraceFactory.Logger.Info("Since the option 'Do not broadcast' is enabled, SSID Name is not displayed in the configuration page of the Printer");

                if (activityData.ProductFamily != ProductFamilies.VEP)
                {
                    TraceFactory.Logger.Info("Validating the Wifi Direct Password on Reports");
                    if (EwsWrapper.Instance().ValidateSSIDName(WifiadvancedPassword))
                    {
                        TraceFactory.Logger.Info("Even after enabling the option 'Do not show Password on Reports', still Password is displayed");
                        return false;
                    }
                    TraceFactory.Logger.Info("Since the option 'Do not show Password on Reports' is enabled, Password is not displayed in the configuration page of the Printer");
                }
                else
                {
                    //TODO: Need to validate the same from control panel for VEP
                }
                return true;
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info(wirelessException.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info("As part of the postrequiste, disabling all advanced options");
                EwsWrapper.Instance().SetWiFiAdvancedOptions(false, false, false);
                EwsWrapper.Instance().ConfigureWiFiDirect(WiFiDirectConnectionMode.Auto.ToString(), WifiautoPassword);
                TestWiFiPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 756405 Wireless Direct or Wifi Direct -SSID-Functionality
        ///Step1: Verify AP ssid is not displayed in STA SSID list
        /// Go to Networking menu -> Wireless Setup and refresh SSID list
        ///Note : Perform this test case for Wifi Direct if supported on printer.
        ///DIRECT-xy-HP<model name>
        /// Expected: AP SSID[Self SSID] should not be listed in the SSID list
        /// Step2: Verify AP ssid is not displayed in STA SSID list
        /// Go to Networking menu -> Wireless Setup wizard and refresh SSID list
        ///Note : Perform this test case for Wifi Direct if supported on printer.
        ///DIRECT-xy-HP<model name>
        ///Expected: AP SSID should not be listed in the SSID list
        ///Step3: Error message displayed for manually entered AP SSID
        ///Go to Networking menu -> Wireless Setup and Enter AP mode SSID of the printer
        ///Note : Perform this test case for Wifi Direct if supported on printer.
        ///DIRECT-xy-HP<model name>
        ///Expected: STA should not associate with self AP ssid and throw an error message
        ///Step4: Error message displayed for manually entered AP SSID
        ///Go to Networking menu -> Wireless Setup wizard and enter AP mode SSID of the printer
        ///Note : Perform this test case for Wifi Direct if supported on printer.
        ///DIRECT-xy-HP<model name>
        ///Expected: STA should not associate with self AP ssid and throw an error message
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool ValidateConnectionWithSelfSSID(WiFiDirectActivityData activityData, int testNo)
        {
            if (!TestWiFiPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string printerMacaAdress = EwsWrapper.Instance().GetMacAddress();
                // INetworkSwitch networkSwitch = SwitchFactory.Create(activityData.SwitchIPAddress);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step1: Verifying the SSID is not displayed in STA SSID List"));
                if (ProductFamilies.InkJet != activityData.ProductFamily)
                {
                    EwsWrapper.Instance().SetWirelessSTA(true);
                    if (EwsWrapper.Instance().GetSSIDList().Contains(activityData.WiFiSsid))
                    {
                        if (ProductFamilies.VEP == activityData.ProductFamily)
                        {
                            TraceFactory.Logger.Info("Since Europa device is connected, Self SSID exists in Wireless STA SSID List");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Self SSID exists in Wireless STA SSID List");
                            return false;
                        }
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("This step is not applicable to inkjet");
                }
                TraceFactory.Logger.Info("The Self SSID is not displayed in the Wireless STA SSSID List");

                if (ProductFamilies.VEP == activityData.ProductFamily || ProductFamilies.InkJet == activityData.ProductFamily)
                {
                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step2: Verifying the SSID is not displayed in STA Wireless Wizard SSID List"));
                    if (EwsWrapper.Instance().GetWirelessWizardSSIDList().Contains(activityData.WiFiSsid))
                    {
                        if (ProductFamilies.VEP == activityData.ProductFamily)
                        {
                            TraceFactory.Logger.Info("Since Europa device is connected with Printer, Self SSID exists in Wireless Wizard STA SSID List");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Self SSID exists in Wireless Wizard STA SSID List");
                            return false;
                        }
                    }
                    TraceFactory.Logger.Info("The Self SSID is not displayed in the Wireless Wizard STA SSSID List");
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step3: Validating the error message displayed for manually entered SSID"));
                WirelessSettings settings = new WirelessSettings()
                { SsidName = activityData.WiFiSsid, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings(new WPAPersonalSettings() { Encryption = WPAEncryptions.AUTO, passphrase = WpaPassphrase, Version = WPAVersions.Auto });


                if (!EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, activityData.PrinterInterfaceType, false))
                {
                    TraceFactory.Logger.Info("Manually entered SSID is not configured and is throwing error message");
                }

                // For TPS to enable wireless need to remove LAN Cable, hence disabling port
                if (ProductFamilies.VEP != activityData.ProductFamily)
                {
                    TraceFactory.Logger.Info("Disabling port to enable wireless connection");
                    //if (!networkSwitch.DisablePort(activityData.PrimaryInterfaceAddressPortNumber))
                    //{
                    //    return false;
                    //}
                    if (CtcUtility.GetPrinterIPAddress(activityData.WirelessMacAddress) != "")
                    {
                        TraceFactory.Logger.Info("For Manually entered SSID, printer is getting configured and ip has been assigned which is not expected");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("For Manually entered SSID, printer is not configured and ip not assigned as expected");
                    }
                    TraceFactory.Logger.Info("Enabling back the port");
                    //if (!networkSwitch.EnablePort(activityData.PrimaryInterfaceAddressPortNumber))
                    //{
                    //    return false;
                    //}
                }
                else
                {
                    if (EwsWrapper.Instance().SearchTextInPage(printerMacaAdress, "WiFiDirect"))
                    {
                        TraceFactory.Logger.Info("The Self SSID is associated with the printer");
                        return false;
                    }
                    TraceFactory.Logger.Info("Result of Step3: Manually entered SSID is not configured as expected");
                }

                if (ProductFamilies.VEP == activityData.ProductFamily)
                {
                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step4: Validating the error message displayed for manually entered SSID using Wireless Wizard"));
                    if (!EwsWrapper.Instance().ConfigureWirelessWizard(activityData.WiFiSsid))
                    {
                        TraceFactory.Logger.Info("Manually entered SSID is not configured and is throwing error message");
                    }

                    if (EwsWrapper.Instance().SearchTextInPage(printerMacaAdress, "WiFiDirect"))
                    {
                        TraceFactory.Logger.Info("The Self SSID is associated with the printer");
                        return false;
                    }
                    TraceFactory.Logger.Info("Result of Step4: Manually entered SSID is not configured as expected");
                }
                return true;
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info(wirelessException.Message);
                return false;
            }
            finally
            {
                TestWiFiPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 756469 Verify Wireless Direct or Wifi Direct AP behavior with frequent Connect and disconnect Ethernet cable
        ///Go to Networking menu -> Wireless Direct and enable wireless direct
        ///Plug in and plug out Ethernet cable and check for HTTPS and network status
        /// Expected: EWS page should be opened using Https and network icon to be update accordingly
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool ValidateWiFiWithEthernetCablePluggedInAndOut(WiFiDirectActivityData activityData, int testNo)
        {
            INetworkSwitch networkSwitch = SwitchFactory.Create(activityData.SwitchIpAddress);

            if (!TestWiFiPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                Printer printer = PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(Wifidirectip));

                if (!AssociateWiFiDirect(activityData, WiFiDirectConnectionMode.Auto, WifiautoPassword, testNo))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Plugging out the ethernet cable");
                if (!networkSwitch.DisablePort(activityData.PrimaryInterfaceAddressPortNumber))
                {
                    TraceFactory.Logger.Info("Fail to disable port: {0}".FormatWith(activityData.PrimaryInterfaceAddressPortNumber));
                    return false;
                }

                TraceFactory.Logger.Info("Validating the accessibility of the printer using HTTPS");
                if (!printer.IsEwsAccessible("https"))
                {
                    TraceFactory.Logger.Info("Fail to access the printer using https");
                    return false;
                }
                TraceFactory.Logger.Info("Able to accesss the printer using https after plugging out ethernet cable");
                return true;
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info(wirelessException.Message);
                return false;
            }
            finally
            {
                networkSwitch.EnablePort(activityData.PrimaryInterfaceAddressPortNumber);
                Thread.Sleep(TimeSpan.FromSeconds(40));
                TestWiFiPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 756418 Wireless Direct or Wifi Direct DHCP-Client list after power cycle
        ///Enable uAP
        ///Attach 5 clients to uAP.
        ///Power cycle the printer
        ///Use telnet debug command to verify no active devices listed(or via EWS). This must be done before uAP clients try to renew.
        ///Verify that uAP clients are able to obtain a DHCP provided address when they try and renew lease.
        ///Verify that uAP clients obtain same address as previously used (since clients will ask for same address if available.)
        ///Use telnet debug command to verify active devices listed(or via EWS). 
        ///Print with all clients.
        /// Expected: Power cycling of printer causes active client table to clear. When clients renew leases printer should grant same addresses if available assuming clients asks for same.
        /// Input B&M: Store the ipaddress of the client before powercycle, after powercycle with wifi the client should retain the same ip with wifi direct enabled
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool ValidateWiFiClientWithPowerCycle(WiFiDirectActivityData activityData, int testNo)
        {
            if (!TestWiFiPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                Printer printer = PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.PrimaryInterfaceAddress));

                if (!AssociateWiFiDirect(activityData, WiFiDirectConnectionMode.Advanced, WifiadvancedPassword, testNo, false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Fetching the IPAddress of the client before power cycling the printer");
                IPAddress wifiDirectClientIPAddress = CtcUtility.GetClientIpAddress(Wifidirectip, AddressFamily.InterNetwork);

                TraceFactory.Logger.Info("Power Cycling the Printer to validate the Client ip");
                printer.PowerCycle();

                TraceFactory.Logger.Info("After PowerCycle enabling wifi direct with advanced mode");
                if (!AssociateWiFiDirect(activityData, WiFiDirectConnectionMode.Advanced, WifiadvancedPassword, testNo))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating the Client IPAddress after Power Cycling the Printer");
                if (!CtcUtility.GetClientIpAddress(Wifidirectip, AddressFamily.InterNetwork).Equals(wifiDirectClientIPAddress))
                {
                    TraceFactory.Logger.Info("The client ipaddress of the printer has changed after powercycle which is not expected");
                    return false;
                }
                TraceFactory.Logger.Info("The client ipaddress of the printer has not changed after powercycle");
                return true;
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info(wirelessException.Message);
                return false;
            }
            finally
            {
                EwsWrapper.Instance().ConfigureWiFiDirect(WiFiDirectConnectionMode.Auto.ToString(), WifiautoPassword);
                TestWiFiPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 756402  Wireless Direct or Wfifi Direct-SSID-Configuration
        ///Step1: Modify the default SSID-CP
        /// "Go to Networking menu -> Wireless direct printing and Configure the SSID to HP-Print-XY-""C""
        ///Note : Perform this test case for Wifi Direct if supported on printer.
        ///DIRECT-xy-HP<model name>
        ///Expected: "New ssid should be Configured and the same has should be advertised and displayed in the internal pages
        /// Step2: Verify Default SSID configuration from EWS
        ///"Go to Networking menu -> Wireless direct printing and check for the default SSID
        ///Note : Perform this test case for Wifi Direct if supported on printer.
        ///DIRECT-xy-HP<model name>
        ///Expected: "SSID should be HP-Print-xy- sub brand>- hp model
        ///Step3: Configure SSID with no characters-Blank
        ///"Go to Networking menu -> Wireless direct printing and Configure the SSID to HP-Print-XY-
        ///Note : Perform this test case for Wifi Direct if supported on printer.
        ///SSID for Wifi Direct will be  DIRECT-xy-HP<model name>
        ///Expected: "No error message should be displayed and SSID should be accepted with Non editable part
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool ValidateWifiSSIDConfiguration(WiFiDirectActivityData activityData, int testNo)
        {
            if (!TestWiFiPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string ssidPrefix = SnmpWrapper.Instance().GetWIFISSIDPrefix();
                string ssidSuffix = SnmpWrapper.Instance().GetWIFISSIDSuffix();
                string ssidString = activityData.ProductFamily == ProductFamilies.VEP ?
                                    "DB" : activityData.ProductFamily == ProductFamilies.TPS ? "c9" : "5c";
                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step1: Validating the Default SSID of the printer"));
                if (!(activityData.WiFiSsid.StartsWith("DIRECT-{0}-HP".FormatWith(ssidString)) && activityData.WiFiSsid.Contains(ssidSuffix)))
                {
                    TraceFactory.Logger.Info("The default SSID 'DIRECT-xy-HP <model name >' is not matching with the printer SSID");
                    TraceFactory.Logger.Info("The current SSID:{0} and Wifi Prefix:{1}".FormatWith(activityData.WiFiSsid, ssidSuffix));
                    return false;
                }
                TraceFactory.Logger.Info("The default SSID 'DIRECT-xy-HP <model name >' is matching with Printer SSID");

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step2: Modifying the WiFi Direct SSID of the printer"));
                if (!EwsWrapper.Instance().SetWiFiSSID("WIFI"))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromSeconds(20));
                TraceFactory.Logger.Info("Validating the updated SSID in the configuration page");
                //string updatedSSID = activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.VEP.ToString()) ? "DIRECT-DB-HP WIFI" : "DIRECT-c9-HP WIFI";
                string updatedSSID = ssidPrefix + "WIFI";
                if (!EwsWrapper.Instance().ValidateSSIDName(updatedSSID))
                {
                    TraceFactory.Logger.Info("The modified SSID {0} is not displayed in printer internal page".FormatWith(updatedSSID));
                    return false;
                }
                TraceFactory.Logger.Info("The modified SSID: {0} is displayed in printer internal page".FormatWith(updatedSSID));

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step3: Validating the SSID with No Characters"));
                if (!EwsWrapper.Instance().SetWiFiSSID(""))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromSeconds(20));
                updatedSSID = ssidPrefix;
                TraceFactory.Logger.Info("Validating the updated SSID in the configuration page");
                if (!EwsWrapper.Instance().ValidateSSIDName(updatedSSID))
                {
                    TraceFactory.Logger.Info("The modified SSID:{0} with character blank is not displayed in printer internal page".FormatWith(updatedSSID));
                    return false;
                }
                TraceFactory.Logger.Info("The modified SSID:{0} with character blank is displayed in printer internal page".FormatWith(updatedSSID));
                return true;
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info(wirelessException.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info("To bring back the Default SSID, as part of postrequisites,performing restore defaults");
                EwsWrapper.Instance().RestoreSecurityDefaults();
                TestWiFiPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 756423 Verify discovery operation with Wired and AP active-NHOT
        /// Wireless Direct or Wifi Direct -NHOT- Multicast and Discovery NHOT-All Accessories need to coonect[serpent and europa]
        ///Step1:Enable wifi direct AP and associate devices to microAP Connect an Ethernet cable Perform WSD and SLP discovery on both interfaces
        ///Note : Perform this test case for Wifi Direct if supported on printer.
        ///Expected: Discovery should work on both the interface
        ///Step2: Verify discovery operation with Wireless STA and AP active
        ///Expected: "Enable AP and associate devices to AP
        ///Associate STA to and access point
        ///Perform WSD and SLP discovery on both interfaces
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool ValidateWiFiDiscoveryWithAccessories(WiFiDirectActivityData activityData, int testNo)
        {
            Printer printer = PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.PrimaryInterfaceAddress));
            if (!TestWiFiPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!AssociateWiFiDirect(activityData, WiFiDirectConnectionMode.Auto, WifiautoPassword, testNo, false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating the Wifi Connection in other interface");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondaryInterfaceAddress);
                if (!EwsWrapper.Instance().GetWiFiConnectionMode().ToLower().Contains(WiFiDirectConnectionMode.Auto.ToString().ToLower()))
                {
                    TraceFactory.Logger.Info("The Wifi Direct Changes in Primary interface is not updated to secondary Interface");
                    return false;
                }
                TraceFactory.Logger.Info("The Wifi Direct Changes in Primary interface is updated to secondary Interface");

                TraceFactory.Logger.Info("Validating the Discovery in all interfaces");
                PluginSupport.Connectivity.Discovery.DeviceInfo deviceInfo;
                if (!PrinterDiscovery.Discover(IPAddress.Parse(activityData.PrimaryInterfaceAddress), out deviceInfo))
                {
                    TraceFactory.Logger.Info("Discovery Failed for Primary Interface after enabling Wifi direct");
                    return false;
                }
                TraceFactory.Logger.Info("Even after enabling wifi direct, able to discover the primary interface ip");

                if (!PrinterDiscovery.Discover(IPAddress.Parse(activityData.SecondaryInterfaceAddress), out deviceInfo))
                {
                    TraceFactory.Logger.Info("Discovery Failed for Secondary Interface after enabling Wifi direct");
                    return false;
                }
                TraceFactory.Logger.Info("Even after enabling wifi direct, able to discover the secondary interface ip");
                return true;
            }
            catch (Exception wirelessException)
            {
                TraceFactory.Logger.Info(wirelessException.Message);
                return false;
            }
            finally
            {
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryInterfaceAddress);
                EwsWrapper.Instance().ConfigureWiFiDirect(WiFiDirectConnectionMode.Auto.ToString(), WifiautoPassword);
                printer.PowerCycle();
                TestWiFiPostRequisites(activityData);
            }
        }

        #endregion

        #region Private methods

        public static bool AssociateWiFiDirect(WiFiDirectActivityData activityData, WiFiDirectConnectionMode connectionMode, string password, int testNo, bool isPrint = true)
        {
            Wifi wifiConnection = new Wifi();

            TraceFactory.Logger.Info("Enabling wifi direct with security in Printer");
            if (!EwsWrapper.Instance().ConfigureWiFiDirect(connectionMode.ToString(), password))
            {
                return false;
            }

            TraceFactory.Logger.Info("Associating Client with Wireless Direct with Security");

            Thread.Sleep(TimeSpan.FromSeconds(10));
            string SSID = SnmpWrapper.Instance().GetWIFISSID();
            Thread.Sleep(TimeSpan.FromMinutes(2));
            if (!HP.ScalableTest.Utility.Retry.UntilTrue(() => wifiConnection.Connect(SSID, password), 10, TimeSpan.FromSeconds(30)))
            {
                TraceFactory.Logger.Info("Client fails to associate WIFIDirect with Security");
                return false;
            }
            TraceFactory.Logger.Info("Client successfully associates the WIFIDirect with Security");

            Thread.Sleep(TimeSpan.FromMinutes(1));
            if (!HP.ScalableTest.Utility.Retry.UntilTrue(() => NetworkUtil.PingUntilTimeout(IPAddress.Parse("192.168.223.1"), TimeSpan.FromMinutes(1)), 3, TimeSpan.FromSeconds(30)))
            {
                TraceFactory.Logger.Info("Fail to ping the IPAddress-192.168.223.1 after associating WIFIDirect");
                return false;
            }
            else
            {
                TraceFactory.Logger.Info("Able to Ping the IPAddress of the Printer-192.168.223.1 after associating WIFIDirect");
            }

            if (isPrint)
            {
                TraceFactory.Logger.Info("Validate Print after successfull association");

                Printer printer = PrinterFactory.Create(activityData.ProductFamily.ToString(), Wifidirectip);

                if (!printer.Install(IPAddress.Parse(Wifidirectip), Printer.PrintProtocol.RAW, activityData.DriverPath, activityData.DriverModel))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));
                return printer.Print(CtcUtility.CreateFile("Wireless_Test_{0}_{1}".FormatWith(activityData.SessionId, testNo)));
            }
            else
            {
                return true;
            }
        }

        private static bool TestWiFiPreRequisites(WiFiDirectActivityData activityData, bool isWps = false)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);

            if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryInterfaceAddress), TimeSpan.FromSeconds(30)))
            {
                TraceFactory.Logger.Info("Ping with IP address: {0} is successful.".FormatWith(activityData.PrimaryInterfaceAddress));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Ping with IP address: {0} failed.".FormatWith(activityData.PrimaryInterfaceAddress));
                return false;
            }
        }

        private static bool TestWiFiPostRequisites(WiFiDirectActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
            Thread.Sleep(TimeSpan.FromMinutes(1));
            return true;
        }

        private static Profile GetAccessPointProfile(WiFiDirectActivityData activityData)
        {
            return new Profile()
            {
                RadioSettings = new RadioSettings()
                {
                    SSIDConfiguration = new SSIDSettings()
                    {
                        SSID0 = Guid.NewGuid().ToString().Substring(0, 4),
                        SSID0State = SsidBroadCast.Enabled
                    },
                    WirelessNetworkModes = WirelessNetworkModes.BG,
                    WirelessChannel = WirelessChannel.Auto,
                    Radio = Radio._2dot4Ghz
                },

                SecuritySettings = new SecuritySettings()
                {
                    ClientTypes = Modes.NoSecurity,
                    WirelessIsolation_Between_SSID = WirelessIsolationBetweenSsid.Enable,
                    WirelessIsolation_WithIn_SSID = WirelessIsolationWithInSsid.Enable,
                }
            };
        }

        private static bool CreateAccessPointProfile(AccessPointInfo accessPointInfo, Profile accessPointProfile)
        {
            IAccessPoint accessPoint = AccessPointFactory.Create(accessPointInfo.Address, accessPointInfo.Vendor, accessPointInfo.Model);
            //TraceFactory.Logger.Info("Creating access point profile with {0}".FormatWith(accessPointProfile.ToString()));
            TraceFactory.Logger.Info($"Creating profile: {accessPointProfile.RadioSettings.SSIDConfiguration.SSID0} on {accessPointInfo.Vendor}-{accessPointInfo.Model} Access Point with IP address: {accessPointInfo.Address}");

            TraceFactory.Logger.Info($"Radio Settings:- Radio: {accessPointProfile.RadioSettings.Radio}, Mode: {accessPointProfile.RadioSettings.WirelessNetworkModes}, Channel: {accessPointProfile.RadioSettings.WirelessChannel}");

            switch (accessPointProfile.SecuritySettings.ClientTypes)
            {
                case Modes.NoSecurity:
                    break;
                case Modes.WepPersonal:
                    TraceFactory.Logger.Info($"Key Type: {accessPointProfile.SecuritySettings.WepKeyType}, Index: {(int)accessPointProfile.SecuritySettings.TransmitKeySelect}, Key1:{accessPointProfile.SecuritySettings.WepKeys.Key1}, Key2:{accessPointProfile.SecuritySettings.WepKeys.Key2}, Key3:{accessPointProfile.SecuritySettings.WepKeys.Key3}, Key4:{accessPointProfile.SecuritySettings.WepKeys.Key4}");
                    break;
                case Modes.WpaPersonal:
                    TraceFactory.Logger.Info($"Version: {accessPointProfile.SecuritySettings.WPAVersion}, Encryption: {accessPointProfile.SecuritySettings.WPAAlgorithm}, Key: {accessPointProfile.SecuritySettings.WpaPersonalSettings.Pre_shared_Key}");
                    break;
                case Modes.WepEnterprise:
                    TraceFactory.Logger.Info($"Radius Server: {accessPointProfile.SecuritySettings.WepEnterpriseSettings.Primary_RADIUS_Server}, Key: {accessPointProfile.SecuritySettings.WepEnterpriseSettings.Primary_Shared_Secret}");
                    break;
                case Modes.WpaEnterprise:
                    TraceFactory.Logger.Info($"Version: {accessPointProfile.SecuritySettings.WPAVersion}, Encryption: {accessPointProfile.SecuritySettings.WPAAlgorithm}, Radius Server: {accessPointProfile.SecuritySettings.WpaEnterpriceSettings.Primary_RADIUS_Server}, Key: {accessPointProfile.SecuritySettings.WpaEnterpriceSettings.Primary_Shared_Secret}");
                    break;
            }
            try
            {
                accessPoint.Login("admin", "1iso*help");
                accessPoint.CreateProfile(accessPointProfile);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Failed to create access point profile.");
                TraceFactory.Logger.Debug(ex.Message);
                return false;
            }
            finally
            {
                accessPoint.Logout();
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }

            TraceFactory.Logger.Info("Successfully created access point profile.");
            return true;
        }

        #endregion
    }
}