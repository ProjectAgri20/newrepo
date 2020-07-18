using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.Utility;
using OpenQA.Selenium;

namespace HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer
{
    /// <summary>
    /// The Ciphers categorised based on the encryption strength
    /// </summary>
    public enum Ciphers
    {
        None,

        /// <summary>
        /// The low encryption strenth selectedeCiphers
        /// </summary>
        [EnumValue("AES256-GCM-SHA384|AES256-SHA256|AES256-SHA|AES128-GCM-SHA256|AES128-SHA256|AES128-SHA|DES-CBC3-SHA|RC4-SHA|RC4-MD5|DES-CBC-SHA")]
        LowEncryptionStrength,

        /// <summary>
        /// The medium encryption strenth selectedeCiphers
        /// </summary>
        [EnumValue("AES256-GCM-SHA384|AES256-SHA256|AES256-SHA|AES128-GCM-SHA256|AES128-SHA256|AES128-SHA|DES-CBC3-SHA|RC4-SHA|RC4-MD5")]
        MediumEncryptionStrenth,

        /// <summary>
        /// The high encryption strenth selectedeCiphers //removed this from below |DES-CBC3-SHA
        /// </summary>
        [EnumValue("AES256-GCM-SHA384|AES256-SHA256|AES256-SHA|AES128-GCM-SHA256|AES128-SHA256|AES128-SHA")]
        HighEncryptionStrength
    }

    /// <summary>
    /// EWS wrapper class contains security related functions
    /// </summary>
    public sealed partial class EwsWrapper
    {
        #region Private Variables

        private string HOMESCREENTABS_BEFORE_LOGIN = "Information";

        #endregion Private Variables

        #region public methods

        #region IP Security/ Firewall

        /// <summary>
        /// Get Action: <see cref="DefaultAction"/> for Default rule
        /// </summary>
        /// <returns><see cref=" DefaultAction"/></returns>
        public DefaultAction GetDefaultRuleAction()
        {
            TraceFactory.Logger.Debug(GET_DEBUG_LOG.FormatWith("Default Rule action"));

            int[] columnIndex = { 0 };
            string ruleAction = string.Empty;
            _adapter.Navigate("IPsec_Firewall", "https");

            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                ruleAction = _adapter.GetValue("Default_Action");
            }
            else
            {
                if (_adapter.IsElementPresent("Rules_Table"))
                {
                    Collection<Collection<string>> rulesTable = _adapter.GetTable("Rules_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                    foreach (Collection<string> tableRow in rulesTable)
                    {
                        if (tableRow[0].Trim().Equals("Default"))
                        {
                            ruleAction = tableRow[3].Trim().ToString();
                        }
                    }
                }
            }

            TraceFactory.Logger.Debug(GET_SUCCESS_LOG.FormatWith("Default Rule action", ruleAction));

            return ruleAction.EqualsIgnoreCase(Enum<DefaultAction>.Value(DefaultAction.Allow)) ? DefaultAction.Allow : DefaultAction.Drop;
        }

        /// <summary>
        /// Set Action: <see cref=" DefaultAction"/> for Default rule
        /// </summary>
        /// <param name="action"><see cref=" DefaultAction"/></param>
        /// <returns><see cref=" DefaultAction"/></returns>
        public bool SetDefaultRuleAction(DefaultAction action)
        {
            TraceFactory.Logger.Debug(SET_DEBUG_LOG.FormatWith("Default rule action", action));
            int[] columnIndex = { 0 };

            _adapter.Navigate("IPsec_Firewall");

            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                _adapter.SelectDropDown("Default_Action", action.ToString());
                _adapter.Click("IPSec_Apply");
            }
            else
            {
                if (_adapter.IsElementPresent("Rules_Table"))
                {
                    Collection<Collection<string>> rulesTable = _adapter.GetTable("Rules_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                    foreach (Collection<string> tableRow in rulesTable)
                    {
                        if (tableRow[0].Trim().Equals("Default"))
                        {
                            if (!tableRow[3].Trim().Equals(action.ToString()))
                            {
                                if (action.ToString().Equals("Allow"))
                                {
                                    _adapter.Click("Default_Action_Allow");
                                }
                                else
                                {
                                    _adapter.Click("Default_Action_Drop");
                                }
                                _adapter.Click("Default_Action_Ok");
                            }
                        }
                    }

                }
            }

            // Wait for rule to get updated
            Thread.Sleep(TimeSpan.FromSeconds(5));

            if (GetDefaultRuleAction().Equals(action))
            {
                TraceFactory.Logger.Info(SUCCESS_LOG.FormatWith("Default rule action", action));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_LOG.FormatWith("Default rule action", action));
                return false;
            }
        }

        /// <summary>
        /// Get IPSec Firewall option value
        /// </summary>
        /// <returns>true if enabled, false otherwise</returns>
        public bool GetIPsecFirewall()
        {
            TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("IP Sec/Firewall"));

            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                StopAdapter();
                _adapter.Start("https");
            }

            _adapter.Navigate("IPsec_Firewall", "https");

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("IP Sec/Firewall", _adapter.IsChecked("Enable_IPsec_Firewall") ? "enable" : "disable"));

            return _adapter.IsChecked("Enable_IPsec_Firewall");
        }

        /// <summary>
        /// Check if any IpSec Rule is Present
        /// </summary>
        /// <returns>true when rules are present, false otherwise</returns>
        public bool IsRuleExists()
        {
            int[] columnIndex = { 0 };

            // For InkJet the rules table is in the IPSec/Firewall home page
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                _adapter.Navigate("Delete_Rules", "https");
                Thread.Sleep(TimeSpan.FromMinutes(2));
            }
            else
            {
                _adapter.Navigate("IPsec_Firewall", "https");
            }


            if (_adapter.IsElementPresent("Rules_Table"))
            {
                Collection<Collection<string>> rulesTable = _adapter.GetTable("Rules_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                foreach (Collection<string> tableRow in rulesTable)
                {
                    // if any rule exist then returns true otherwise false
                    // For Inkjet, Default rule will be the first row in the table if no rules present
                    if ((!string.IsNullOrEmpty(tableRow[2].Trim())) && (!tableRow[0].Trim().Equals("Default")))
                    {
                        TraceFactory.Logger.Info("Rules are Present");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("No Rules");
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Set IPSec Firewall option
        /// Note: Validation for this option is not performed since web UI page might be inaccessible after enabling IPsec/Firewall option. GetIPsecFirewall() can be used if required
        /// </summary>
        /// <param name="enable">true to enable, false otherwise</param>
        /// <returns>true if successfully set, false otherwise</returns>
        public void SetIPsecFirewall(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("IP Sec/Firewall", state));

			try
			{
				_adapter.Navigate("IPsec_Firewall", "https");
				
				if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(2));
                }

                Thread.Sleep(TimeSpan.FromSeconds(10));
				if (enable)
				{
                    if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                    {
                        _adapter.Check("Enable_1");
                    }
                    _adapter.Check("Enable_IPsec_Firewall");
                }
                else
                {
                    _adapter.Uncheck("Enable_IPsec_Firewall");
                }

                _adapter.Click("IPSec_Apply");
                Thread.Sleep(TimeSpan.FromSeconds(10));
                //TraceFactory.Logger.Info("Successfully set IPSec Firewall option to {0}".FormatWith(state));
                TraceFactory.Logger.Info("Successfully set ipsec firewall option to {0}".FormatWith(enable));
            }
            catch
            {
                // Do Nothing
            }
            finally
            {
                StopAdapter();
                _adapter.Start("https");
            }
        }

        /// <summary>
        /// Get Fail Safe option value
        /// </summary>
        /// <returns>true if set, false otherwise</returns>
        public bool GetSecurityFailsafe()
        {
            // Inkjet will have failsafe option in Options tab
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                _adapter.Navigate("IPsec_Firewall");
            }

            // In InkJet, the Failsafe's Id is changing.
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                StopAdapter();
                _adapter.Start();
            }

            // VEP/ LFP will have fail safe option in Advanced settings
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                _adapter.Navigate("IPsec_Advanced");
            }

            TraceFactory.Logger.Debug(GET_SUCCESS_LOG.FormatWith("Fail safe", _adapter.IsChecked("Enable_Failsafe") ? "enable" : "disable"));
            if (_adapter.IsChecked("Enable_Failsafe"))
            {
                _adapter.Navigate("IPsec_Firewall");
                return true;
            }
            else
            {
                TraceFactory.Logger.Debug("Failesafe option is not checked.");
                return false;
            }

        }

        /// <summary>
        /// Get WS-Discovery option value
        /// </summary>
        /// <returns>true if set, false otherwise</returns>
        public bool GetSecurityWSDiscovery()
        {
            TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("WS Discovery"));
            // Inkjet will have WS Discovery in Options tab
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("IPsec_Firewall");
                _adapter.Click("Advanced_Option");
            }

            // VEP/ LFP will have fail safe option in Advanced settings
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                _adapter.Navigate("IPsec_Advanced");
            }

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("WS Discovery", _adapter.IsChecked("Ws_Discovery_Option") ? "enable" : "disable"));

            return _adapter.IsChecked("Ws_Discovery_Option");
        }

        /// <summary>
        /// Set Fail Safe option value
        /// </summary>
        /// <param name="enable">true to enable, false otherwise</param>
        /// <returns>true is set successfully, false otherwise</returns>
        public bool SetSecurityFailsafe(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("Fail safe", state));

            // Inkjet will have failsafe option in Options tab
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                _adapter.Navigate("IPsec_Firewall");
            }
            // In InkJet, the Failsafe's Id is changing.
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                StopAdapter();
                _adapter.Start();
            }

            // VEP/ LFP will have fail safe option in Advanced settings
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                _adapter.Navigate("IPsec_Advanced");
            }

            if (enable)
            {
                _adapter.Check("Enable_Failsafe");
            }
            else
            {
                _adapter.Uncheck("Enable_Failsafe");
            }

            _adapter.Click("IPSec_Apply");

            Thread.Sleep(TimeSpan.FromSeconds(5));

            if (GetSecurityFailsafe().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("Fail safe", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("Fail safe", state));
                return false;
            }
        }

        /// <summary>
        /// Set Security WS-Discovery option value
        /// </summary>
        /// <param name="enable">true to enable, false otherwise</param>
        /// <returns>true is set successfully, false otherwise</returns>
        public bool SetSecurityWSDiscovery(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("WSDiscovery", state));

            // Inkjet will have WS Discovery in Options tab
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("IPsec_Firewall");
                _adapter.Click("Advanced_Option");
            }
            // VEP/ LFP will have fail safe option in Advanced settings
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                _adapter.Navigate("IPsec_Advanced");
            }

            if (enable)
            {
                if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
                {
                    _adapter.Check("Ws_Discovery_Option");
                }
                else
                {
                    //For InkJets Sitemap ID's are changing continuously so Killing nad opening browser again to check if it has original ID
                    if (!_adapter.IsElementPresent("Ws_Discovery_Option"))
                    {
                        StopAdapter();
                        _adapter.Start();
                        _adapter.Navigate("IPsec_Advanced");
                        if (!_adapter.IsElementPresent("Ws_Discovery_Option"))
                        {
                            TraceFactory.Logger.Info("WS-Disovery-option not found on the page");
                        }
                        else
                        {
                            _adapter.Check("Ws_Discovery_Option");
                        }
                        
                    }
                }
            }
            else
            {
                _adapter.Uncheck("Ws_Discovery_Option");
            }
            if ((_adapter.Settings.ProductType == PrinterFamilies.TPS))
            {
                _adapter.Click("Ipsec_Advanced_Apply");
            }
            else
            {
                _adapter.Click("IPSec_Apply");
            }
            Thread.Sleep(TimeSpan.FromSeconds(5));

            if (GetSecurityWSDiscovery().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("WS Discovery", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("WS Discovery", state));
                return false;
            }
        }

        /// <summary>
        /// Enable All IPSec Firewall rules
        /// </summary>
        /// <returns>true is all rules are enabled, false otherwise</returns>
        public bool EnableAllRules()
        {
            TraceFactory.Logger.Info("Enabling Rules in Printer");
            return SetIPSecFirewallRules("IPsec_Firewall", "Rules_Table", "IPSec_Apply", FindType.ByXPath);
        }

        /// <summary>
        /// Disable All IPSec Firewall rules
        /// </summary>
        /// <returns>true is all rules are disabled, false otherwise</returns>
        public bool DisableAllRules()
        {
            TraceFactory.Logger.Info("Disabling Rule in Printer");
            return SetIPSecFirewallRules("IPsec_Firewall", "Rules_Table", "IPSec_Apply", FindType.ByXPath, false);
        }

        /// <summary>
        /// Checking the Maximum Rules created
        /// </summary>
        /// <returns>true if it is not allowing, false otherwise</returns>
        public bool CheckMaxRules()
        {
            _adapter.Navigate("IPsec_Firewall", "https");
            if (_adapter.IsElementPresent("Add_rule"))
            {
                TraceFactory.Logger.Info("Maximum rules limit not reached");
                return false;
            }
            else
            {
                TraceFactory.Logger.Info("Maximum Rules are created. No rules are allowed to create.");
                return true;
            }
        }

        /// <summary>
        /// Delete All IPSec Firewall rules
        /// </summary>
        /// <param name="deleteCustomTemplates">true to Delete Custom Address, Service and IPsec templates</param>
        /// <returns>true when no rules are present, false otherwise</returns>
        public bool DeleteAllRules(bool deleteCustomTemplates = true)
        {
            TraceFactory.Logger.Debug("Deleting all IPSec Firewall rules.");
            bool isTps = _adapter.Settings.ProductType == PrinterFamilies.TPS;
            bool isInk = _adapter.Settings.ProductType == PrinterFamilies.InkJet;
            bool isLfp = _adapter.Settings.ProductType == PrinterFamilies.LFP;

            try
            {
                // Delete Pages and the way we delete differs for TPS and VEP
                if (isTps)
                {
                    _adapter.Navigate("IPsec_Firewall", "https");
                    _adapter.Click("Restore_Defaults");
                }
                else if (isInk)
                {
                    EwsWrapper.Instance().SetIPsecFirewall(false);
                    _adapter.Navigate("IPsec_Firewall", "https");
                    if (IsRuleExists())
                    {
                        _adapter.Click("Select_AllRules");
                        _adapter.Click("Delete_Rules");
                        _adapter.Click("Delete_Rules_Confirm");
                        _adapter.Click("Delete_Rules_Ok");
                    }
                }
                else
                {
                    int[] columnIndex = { 0 };

                    EwsWrapper.Instance().SetIPsecFirewall(false);
                    if (isLfp)
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                    }

                    _adapter.Navigate("Delete_Rules", "https");

                    // Check if rules table is available. In case if there are no rules, there will be warning message.
                    if (_adapter.IsElementPresent("Rules_Table"))
                    {
                        Collection<Collection<string>> rulesTable = _adapter.GetTable("Rules_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                        foreach (Collection<string> tableRow in rulesTable)
                        {
                            // 3rd column of the row specifies the Address template field. When a rule is created, this field will be updated
                            if (!string.IsNullOrEmpty(tableRow[2].Trim()))
                            {
                                _adapter.Check(tableRow[0], false, FindType.ByName);
                            }
                            Thread.Sleep(TimeSpan.FromSeconds(10));
                        }
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        _adapter.Click("Apply");
                    }
                }
                // No alert message will come for Inkjet
                if (!isInk)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    _adapter.ClickOkonAlert();
                }

                Thread.Sleep(TimeSpan.FromMinutes(2));

                // Custom templates are created only in VEP and InkJet currently
                if (!isTps && deleteCustomTemplates)
                {
                    DeleteAllCustomRules();
                }

                TraceFactory.Logger.Info("All IPSec Firewall rules are deleted.");

                return true;
            }
            catch (Exception ruleException)
            {
                TraceFactory.Logger.Info("Exception caught : {0}".FormatWith(ruleException.Message));
                return false;
            }
        }

        /// <summary>
        /// Delete existing IPsec rule
        /// </summary>
        /// <param name="ipsecTemplateName">IPsec template name</param>
        /// <returns>true if rule is deleted, false if rule is not created or unable to delete rule</returns>
        public bool DeleteRule(string ipsecTemplateName)
        {
            _adapter.Navigate("Delete_Rules", "https");
            int ipsecColumn = 0;
            bool isInk = PrinterFamilies.InkJet == _adapter.Settings.ProductType;
            // 5th column of the row specifies the ipsec template field. 4th column for INK
            if (isInk)
            {
                ipsecColumn = 3;
                ipsecTemplateName = "IPsec Required (" + ipsecTemplateName + ")";
            }
            else
            {
                ipsecColumn = 4;
            }

            // Check if rules table is available. In case if there are no rules, there will be warning message.
            if (_adapter.IsElementPresent("Rules_Table"))
            {
                int[] columnIndex = { 0 };

                Collection<Collection<string>> rulesTable = _adapter.GetTable("Rules_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                int radioIndex = 1;
                foreach (Collection<string> tableRow in rulesTable)
                {
                    if (isInk)
                    {
                        if (tableRow[ipsecColumn].Trim().Equals(ipsecTemplateName))
                        {
                            // All radio buttons in the table are identified by a single name
                            IList<IWebElement> elements = _adapter.GetPageElements(tableRow[0], false, FindType.ByName);
                            elements[radioIndex].Click();

                            Thread.Sleep(TimeSpan.FromSeconds(10));
                        }
                        radioIndex++;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(tableRow[ipsecColumn].Trim()) && ipsecTemplateName.EqualsIgnoreCase(tableRow[ipsecColumn].Trim()))
                        {
                            _adapter.Check(tableRow[0], false, FindType.ByName);
                        }
                    }
                }
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Click("Delete_Rule");
                    _adapter.Click("Delete_Rules_Confirm");
                    _adapter.Click("Delete_Rules_Ok");
                }
                else
                {
                    _adapter.Click("Apply");
                    _adapter.ClickOkonAlert();
                }

                Thread.Sleep(TimeSpan.FromSeconds(10));

                TraceFactory.Logger.Info("Rule with ipsec template name: {0} deleted successfully.".FormatWith(ipsecTemplateName));

                return true;
            }

            TraceFactory.Logger.Info("Failed to delete rule.");

            return false;
        }

        /// <summary>
        /// Create IPSec Firewall rule.
        /// Note: Any Exception during rule creation needs to be handled by the caller
        /// Populating SecurityRuleSettings structure
        /// 1. AddressTemplateSettings:
        ///     a) Default template: fill in DefaultAddressTemplates, ignore other fields.
        ///     b) Custom template: <see cref="SetCustomAddressTemplate"/>
        /// 2. ServiceTemplateSettings:
        ///     a) Default template: fill in DefaultServiceTemplates, ignore other fields.
        ///     b) Custom template: <see cref="CreateServiceTemplate"/>
        /// 3. IPsecFirewallAction:
        ///     a) Firewall rule: Allow/Drop
        ///     b) IPsec rule: ProtectedWithIPsec
        /// 4. IPsecTemplateSettings: Need to filled in only for IPsec rule. See <see cref="CreateIPSecTemplate"/>
        /// 5. Name: Ignore this field for Printer rule creation
        /// </summary>
        /// <param name="settings"><see cref=" SecurityRuleSettings"/></param>
        /// <param name="enableRule">true to enable rules, false otherwise</param>
        /// <param name="enablePolicy">true to enable fail safe policy, false otherwise</param>
        public void CreateRule(SecurityRuleSettings settings, bool enableRule = false, bool enablePolicy = true)
        {
            // While creating IPsec rule, if Authentication Type selected is 'Certificates'; Certificates needs be installed before creating rule
            // To handle this case, both CA and ID certificates are installed prior to creating IPsec rule

            if (!CheckCertificateInstallation(settings))
            {
                TraceFactory.Logger.Info("Failed to install certificates before creating IPsec rule.");
                return;
            }

            TraceFactory.Logger.Info(" IPSec/Firewall Settings: {0}".FormatWith(settings.ToString()));

            TraceFactory.Logger.Debug("Creating IPSec/Firewall rule.");

            _adapter.Navigate("IPsec_Firewall", "https");

            // Click on Add Rule
            _adapter.Click("Add_rule");

            /* For creating Firewall rule, there are two stages: Creating Address template and creating service template. Finally Allow/Drop the rule.
			 * For creating IPsec rule, there is an additional step to create IPsec template apart from address and service templates.
			 * Flow for creating rule will be as follows:
			 * 1. Create Address template
			 * 2. Create Service template
			 * 3. Create IPsec template for IPsec rule, Allow/Drop traffic option for Firewall rule
			 * 4. Finalize Rule
			 * Additional settings after rule creation
			 * 5. Enable/Disable rules
			 * 6. Enable/Disable Failsafe option
			 * Printer View/Behavioral changes:
			 * --------------------------------------------------------------------------------------------------------
			 *   Feature    |        TPS       |         VEP            |        LFP               |     InkJet       |
			 * --------------------------------------------------------------------------------------------------------
			 *  Support     |   Only Firewall  |   Both Firewall, IPsec |   Both Firewall, IPsec   |  Only Firewall   |
			 *  Wizard      |   Single page    |    Multiple page       |   Multiple Page          |  Multiple Page   |
			 * */

            // TPS has address, service templates available in same page. For VEP and InkJet, we need to click on 'Next' button to proceed.
            //CreateAddressTemplate(settings.AddressTemplate);
            //NavigateToNextPage("Address_Next");
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                CreateServiceTemplate(settings.ServiceTemplate);
                NavigateToNextPage("Service_Next");

                CreateAddressTemplate(settings.AddressTemplate);
                NavigateToNextPage("Address_Next");
            }
            else
            {
                CreateAddressTemplate(settings.AddressTemplate);
                NavigateToNextPage("Address_Next");

                CreateServiceTemplate(settings.ServiceTemplate);
                NavigateToNextPage("Service_Next");
            }
            // Firewall rule: Only Allow or Drop traffic option are available and finally finalize the rule
            // IPsec rule: Create IPsec template and finalize rule
            if (settings.Action == IPsecFirewallAction.ProtectedWithIPsec)
            {
                CreateIPSecTemplate(settings.IPsecTemplate);
            }
            else
            {
                if (settings.Action == IPsecFirewallAction.AllowTraffic)
                {
                    _adapter.Check("Traffic_Allow");
                }
                else
                {
                    _adapter.Check("Traffic_Drop");
                }
            }

            NavigateToNextPage("Traffic_Next");

            // There is Info screen which pops up for some cases, In case if there is a screen with this Info message; click Ok and proceed further.
            if (Instance().SearchTextInPage("Selected IPsec policy requires a service template with specific port to configure a rule"))
            {
               
                NavigateToNextPage("Traffic_Next");
            }

            if (_adapter.IsElementPresent("ConfigResult_OK"))
            {
                _adapter.Click("ConfigResult_OK");
            }

            // If all Management service is selected to create rule, ews page will have warning message
            if (Instance().SearchTextInPage("Selected IPsec policy requires a service template with specific port to configure a rule"))
            {
                NavigateToNextPage("Traffic_Next");
            }

            // This will Finalize the rule created
            FinalizeRule();

            // VEP rules will be enabled by default so check only for TPS
            if (enableRule)
            {
              if (!(PrinterFamilies.VEP == _adapter.Settings.ProductType))
                {
                    TraceFactory.Logger.Info("Enabling the rules");
                    EnableAllRules();
                }
            }
            else
            {
                DisableAllRules();
            }

            // Fail safe option will make sure, the user can access printer web page with https irrespective of rule created
            // It is recommended to enable this option so that the user doesn't get locked down
            SetSecurityFailsafe(enablePolicy);

            TraceFactory.Logger.Info("## IPSec Firewall Option Enabled");
        }

        /// <summary>
        /// Create Custom service
        /// </summary>
        /// <param name="serviceSettings"><see cref="Service"/></param>
        public void CreateCustomService(Service serviceSettings)
        {
            // Navigate to Custom service page
            _adapter.Navigate("IPsec_Firewall");
            _adapter.Click("Add_rule");
            _adapter.Click("Address_Next");
            _adapter.Click("Service_New");
            _adapter.Click("Service_Manage_Service");

            // Create custom service with user specified settings
            ManageCustomService(serviceSettings.Name, serviceSettings.Protocol, serviceSettings.ServiceType, serviceSettings.PrinterPort, serviceSettings.RemotePort, serviceSettings.IcmpType);

            // After custom service is created, click on OK button to come back to main service page
            _adapter.Click("Service_Manage_Ok");

            GotoMainIPsecPage();
        }

        /// <summary>
        /// Edit Address Template
        /// </summary>
        /// <param name="templateSettings"><see cref="AddressTemplateSettings"/></param>
        /// <param name="apply"></param>
        /// <returns>true is edited successfully, false otherwise</returns>
        public void EditAddressTemplate(AddressTemplateSettings templateSettings, bool apply = false)
        {
            TraceFactory.Logger.Info("##Editing the Address Template with Settings : {0}".FormatWith(templateSettings.ToString()));

            // Default templates can't be edited
            if (DefaultAddressTemplates.Custom != templateSettings.DefaultTemplate)
            {
                TraceFactory.Logger.Info("##Default Address template : {0} can't be edited.".FormatWith(templateSettings.DefaultTemplate));
                return;
            }

            _adapter.Navigate("IPsec_Firewall", "https");
            // Click on Address template link on main ipsec page
            _adapter.ClickonLink(templateSettings.Name);
            // Click on Modify Template button
            _adapter.Click("Address_Template_Modify");
            // Create and Edit address template will take you to same page. For Edit, Address template name will be disabled
            SetCustomAddressTemplate(templateSettings, true);

            if (apply)
            {
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromMinutes(2));
                _adapter.Navigate("IPsec_Firewall");
            }

            //Verify if modification was successful
            _adapter.ClickonLink(templateSettings.Name);

            // Get the addresses for local and remote machine from the table
            Collection<Collection<string>> addressTable = _adapter.GetTable("Address_Template_Table", includeHeader: false, elementType: FindType.ByXPath);

            string localAddress, remoteAddress;
            localAddress = remoteAddress = string.Empty;

            // Assign local and remote address based on custom address template type
            switch (templateSettings.CustomTemplateType)
            {
                case CustomAddressTemplateType.IPAddress:
                    {
                        localAddress = templateSettings.LocalAddress;
                        remoteAddress = templateSettings.RemoteAddress;
                        break;
                    }

                case CustomAddressTemplateType.IPAddressPrefix:
                    {
                        // Pattern: 192.168.201.3    /    24  192.168.201.65    /    24
                        localAddress = templateSettings.LocalAddress.Split('|')[0] + "/" + templateSettings.LocalAddress.Split('|')[1];
                        remoteAddress = templateSettings.RemoteAddress.Split('|')[0] + "/" + templateSettings.RemoteAddress.Split('|')[1];
                        break;
                    }

                case CustomAddressTemplateType.IPAddressRange:
                    {
                        // Pattern: 192.168.201.23    -    192.168.201.29  192.168.201.56    -    192.168.201.60
                        localAddress = templateSettings.LocalAddress.Split('|')[0] + "-" + templateSettings.LocalAddress.Split('|')[1];
                        remoteAddress = templateSettings.RemoteAddress.Split('|')[0] + "-" + templateSettings.RemoteAddress.Split('|')[1];
                        break;
                    }

                case CustomAddressTemplateType.PredefinedAddresses:
                    {
                        // Pattern in data table:
                        // All IPv4:                    0.0.0.0    -    255.255.255.255
                        // All IPv6:                    ::    -    FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF
                        // All link local:              FE80::    -    FE80:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF

                        //PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), _adapter.Settings.ProductType);
                        PrinterFamilies family = _adapter.Settings.ProductType;

                        if (templateSettings.LocalAddress.EqualsIgnoreCase(CtcUtility.GetEnumvalue(Enum<DefaultAddressTemplates>.Value(DefaultAddressTemplates.AllIPv4Addresses), family)))
                        {
                            localAddress = "0.0.0.0-255.255.255.255";
                            remoteAddress = "0.0.0.0-255.255.255.255";
                        }
                        else if (templateSettings.LocalAddress.EqualsIgnoreCase(CtcUtility.GetEnumvalue(Enum<DefaultAddressTemplates>.Value(DefaultAddressTemplates.AllIPv6Addresses), family)))
                        {
                            localAddress = "::-FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF";
                            remoteAddress = "::-FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF";
                        }
                        else if (templateSettings.LocalAddress.EqualsIgnoreCase(CtcUtility.GetEnumvalue(Enum<DefaultAddressTemplates>.Value(DefaultAddressTemplates.AllIPv6LinkLocal), family)))
                        {
                            localAddress = "FE80::-FE80:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF";
                            remoteAddress = "FE80::-FE80:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF";
                        }
                        break;
                    }

                default: break;
            }

            // 2nd column represents local address, 3rd column represents remote address
            if (addressTable[0][1].RemoveWhiteSpace().EqualsIgnoreCase(localAddress) && addressTable[0][2].RemoveWhiteSpace().EqualsIgnoreCase(remoteAddress))
            {
                TraceFactory.Logger.Info("## Successfully edited address template.");
            }
            else
            {
                TraceFactory.Logger.Info("## Failed to edit address template.");
            }
        }

        /// <summary>
        /// Edit Service Template
        /// </summary>
        /// <param name="templateSettings"><see cref="ServiceTemplateSettings"/></param>
        /// <returns>true is edited successfully, false otherwise</returns>
        public void EditServiceTemplate(ServiceTemplateSettings templateSettings, bool apply = false)
        {
            try
            {
                // Default service templates can't be edited
                if (DefaultServiceTemplates.Custom != templateSettings.DefaultTemplate)
                {
                    TraceFactory.Logger.Info("Default Service template : {0} can't be edited.".FormatWith(templateSettings.DefaultTemplate));
                    return;
                }

                _adapter.Navigate("IPsec_Firewall");
                // Click on Service template name link
                _adapter.ClickonLink(templateSettings.Name);
                // Click on modify button
                _adapter.Click("Service_Template_Modify");
                // Click on Manage Service button to proceed to Custom service edit page
                _adapter.Click("Service_Manage_Service");
                // Select all requested services
                // Xpath table differs for Omniopus products, hence require to include headers
                if (IsOmniOpus)
                {
                    SelectMangeServices(templateSettings.Services, true);
                }
                else
                {
                    SelectMangeServices(templateSettings.Services);
                }

                if (apply)
                {
                    _adapter.Click("Apply");
                    Thread.Sleep(TimeSpan.FromMinutes(2));
                    _adapter.Navigate("IPsec_Firewall");
                    TraceFactory.Logger.Info("Successfully edited service template.");
                }
            }
            catch
            {
                TraceFactory.Logger.Info("Failed to edit service template.");
            }
            // TODO: Fetching Table Column Details is not working, so disabling the validation part
            // Click on service template and validate if service was modified
            //_adapter.ClickonLink(templateSettings.Name);

            //bool result = true;
            //// Get all services from the table
            //Collection<Collection<string>> serviceTable = _adapter.GetTable("Service_Template_Table", includeHeader: false);
            //int tableColumnCount = serviceTable[0].Count;
            //int nameColumn, protocolColumn, serviceColumn, printerportColumn, remoteportColumn;

            //// Table headers are embedded under 'tbody' and is the first row retired. Get the table headers for matching each column
            //nameColumn = serviceTable[0].IndexOf(serviceTable[0].Where(x => x.StartsWith("Service Name", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            //protocolColumn = serviceTable[0].IndexOf(serviceTable[0].Where(x => x.StartsWith("Protocol", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            //serviceColumn = serviceTable[0].IndexOf(serviceTable[0].Where(x => x.StartsWith("Service Type", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            //printerportColumn = serviceTable[0].IndexOf(serviceTable[0].Where(x => x.StartsWith("Printer", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            //remoteportColumn = serviceTable[0].IndexOf(serviceTable[0].Where(x => x.StartsWith("Remote", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());

            //// Header data is no more required and is removed to reduce looping during selection of service
            //serviceTable.RemoveAt(0);

            //string printerPort, remotePort;
            //printerPort = remotePort = string.Empty;

            //foreach (Service service in templateSettings.Services)
            //{
            //	bool validationResult = false;

            //	foreach (Collection<string> tableRow in serviceTable)
            //	{
            //		// Port value for any with be 'Empty' in the structure passed by the user and 'Any' in the table retrieved from web UI
            //		// For range: table provide with '-' and user passes '|'
            //		printerPort = service.PrinterPort == string.Empty ? "Any" : service.PrinterPort.Replace("|", " - ");
            //		remotePort = service.RemotePort == string.Empty ? "Any" : service.RemotePort.Replace("|", " - ");

            //		if (service.Name.EqualsIgnoreCase(tableRow[nameColumn]) && service.Protocol.ToString().EqualsIgnoreCase(tableRow[protocolColumn])
            //			   && tableRow[serviceColumn].StartsWith(service.ServiceType.ToString()) && printerPort.EqualsIgnoreCase(tableRow[printerportColumn])
            //			   && remotePort.EqualsIgnoreCase(tableRow[remoteportColumn]))
            //		{
            //			validationResult = true;
            //			break;
            //		}
            //	}

            //	result &= validationResult;
            //}

            //if(result)
            //{
            //	TraceFactory.Logger.Info("Successfully edited service template.");
            //}
            //else
            //{
            //	TraceFactory.Logger.Info("Failed to edit service template.");
            //}
        }

        /// <summary>
        /// Edit IPsec template
        /// </summary>
        /// <param name="templateSettings"><see cref="IPsecTemplateSettings"/></param>
        /// <param name="apply"></param>
        /// <returns>true is edited successfully, false otherwise</returns>
        public void EditIPSecTemplate(IPsecTemplateSettings templateSettings, bool apply = false)
        {
            TraceFactory.Logger.Info("Editing the IPSec Template, new Settings : {0}".FormatWith(templateSettings.ToString()));

            _adapter.Navigate("IPsec_Firewall");
            // Click on IPsec template name link
            _adapter.ClickonLink(templateSettings.Name);
            // Click on modify button
            _adapter.Click("IPsec_Template_Modify");
            CreateIPSecTemplate(templateSettings, true);

            if (apply)
            {
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromMinutes(2));
                _adapter.Navigate("IPsec_Firewall");
            }

            // Validate modification
            _adapter.ClickonLink(templateSettings.Name);

            string searchLocalAuthentication, searchRemoteAuthentication;
            searchLocalAuthentication = searchRemoteAuthentication = string.Empty;

            // Based on Authentication type, set the search string
            if (templateSettings.DynamicKeysSettings.V1Settings != null)
            {
                searchLocalAuthentication = GetAuthenticationTypeText(templateSettings.DynamicKeysSettings.V1Settings.AuthenticationTypes);
            }
            else if (templateSettings.DynamicKeysSettings.V2Settings != null)
            {
                searchLocalAuthentication = GetAuthenticationTypeText(templateSettings.DynamicKeysSettings.V2Settings.LocalAuthenticationType);
                searchRemoteAuthentication = GetAuthenticationTypeText(templateSettings.DynamicKeysSettings.V2Settings.RemoteAuthenticationType);
            }
            else if (templateSettings.ManualKeysSettings != null)
            {
                searchLocalAuthentication = "Manual Keys";
            }

            if (SearchTextInPage(searchLocalAuthentication) && SearchTextInPage(searchRemoteAuthentication))
            {
                TraceFactory.Logger.Info("Successfully edited ipsec template.");
            }
            else
            {
                TraceFactory.Logger.Info("Failed to edit ipsec template.");
            }
        }

        /// <summary>
        /// Delete Address template
        /// </summary>
        /// <param name="templateName">Address template name</param>
        /// <returns>true if address template deleted successfully, false otherwise</returns>
        public void DeleteAddressTemplate(string templateName)
        {
            // Navigate to Address template page and delete custom address template
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {

                _adapter.Navigate("IPsec_Firewall");
                _adapter.Click("Add_rule");
                DeleteListItem("Address_List", templateName);

                // If the specified address is used by in any rule, it can't be deleted
                if (SearchTextInPage("The selected template is currently in use by a rule"))
                {
                    if (!IsOmniOpus)
                    {
                        // Click on 'Ok' button on Error page and go back to main IPsec page
                        _adapter.Click("Error_Ok");
                    }

                    TraceFactory.Logger.Info("Failed to delete address template.");
                    TraceFactory.Logger.Info("Specified address template : {0} is used in rule. Delete rule before deleting address template.".FormatWith(templateName));
                }
                else
                {
                    TraceFactory.Logger.Info("## Successfully deleted address template: {0}".FormatWith(templateName));
                }

                GotoMainIPsecPage();
            }
            else
            {
                // For INKJET, deleting an address template is possible only by navigating to IPSEC Address Template tab. From the table, select the template to be deleted and confirm delete
                // action. Here, the whole table is fetched,then the macting address template is found and verifies whetehr the check box corresponding to that template is enabled or not
                // Ideally, when the rule is enabled, the check box corresponding to the address template should be disabled and enabled only when the rule is deleted.

                int[] columnIndex = { 0 };
                StopAdapter();
                _adapter.Start("https");
                _adapter.Navigate("Firewall_Address_Template");

                //Fetching the table containing address templates
                Collection<Collection<string>> rulesTable = _adapter.GetTable("Address_Template_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                int checkboxIndex = 1;

                //Parse through each row and check whether the address template to be deleted is found or not
                foreach (Collection<string> tableRow in rulesTable)
                {
                    if (tableRow[1].Trim().Equals(templateName))
                    {
                        // All radio buttons in the table are identified by a single name
                        IList<IWebElement> elements = _adapter.GetPageElements(tableRow[0], false, FindType.ByName);
                        if (!elements[checkboxIndex].Enabled)
                        {
                            // If the check box is disabled, deleting of template not possible
                            TraceFactory.Logger.Info("Failed to delete address template.");
                        }
                        else
                        {
                            //Deleting the address template
                            elements[checkboxIndex].Click();
                            _adapter.Click("Delete");
                            _adapter.Click("Delete_Confirmation");
                            _adapter.Click("Delete_Ok");
                            TraceFactory.Logger.Info("Deleted Address Template");
                        }


                        Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                }

            }
        }

        /// <summary>
        /// Delete Service template
        /// </summary>
        /// <param name="templateName">Service template name</param>
        /// <returns>true if service template deleted successfully, false otherwise</returns>
        public void DeleteServiceTemplate(string templateName)
        {
            // Navigate to Service template page and delete custom service template
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                _adapter.Navigate("IPsec_Firewall");
                _adapter.Click("Add_rule");
                _adapter.Click("Address_Next");

                DeleteListItem("Service_List", templateName);

                // If the specified service is used by in any rule, it can't be deleted
                if (SearchTextInPage("The selected template is currently in use by a rule"))
                {
                    if (_adapter.IsElementPresent("Error_Ok"))
                    {
                        // Click on 'Ok' button on Error page and go back to main IPsec page
                        _adapter.Click("Error_Ok");
                    }

                    TraceFactory.Logger.Info("Failed to delete service template.");
                    TraceFactory.Logger.Info("Specified service template : {0} is used in rule. Delete rule before deleting service template.".FormatWith(templateName));
                }
                else
                {
                    TraceFactory.Logger.Info("Successfully deleted service template: {0}".FormatWith(templateName));
                }

                GotoMainIPsecPage();
            }
            else
            {
                // For INKJET, deleting a service template is possible only by navigating to IPSEC Service Template tab. From the table, select the template to be deleted and confirm delete
                // action. Here, the whole table is fetched,then the macting address template is found and verifies whetehr the check box corresponding to that template is enabled or not
                // Ideally, when the rule is enabled, the check box corresponding to the address template should be disabled and enabled only when the rule is deleted.

                int[] columnIndex = { 0 };
                StopAdapter();
                _adapter.Start("https");
                _adapter.Navigate("Firewall_Service_Template");

                //Fetching the table containing service templates
                Collection<Collection<string>> rulesTable = _adapter.GetTable("Service_Template_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                int checkboxIndex = 1;

                //Parse through each row and check whether the service template to be deleted is found or not
                foreach (Collection<string> tableRow in rulesTable)
                {
                    if (tableRow[1].Trim().Equals(templateName))
                    {
                        // All radio buttons in the table are identified by a single name
                        IList<IWebElement> elements = _adapter.GetPageElements(tableRow[0], false, FindType.ByName);
                        if (!elements[checkboxIndex].Enabled)
                        {
                            // If the check box is disabled, deleting of template not possible
                            TraceFactory.Logger.Info("Failed to delete Service template.");
                        }
                        else
                        {
                            //Deleting the service template
                            elements[checkboxIndex].Click();
                            _adapter.Click("Delete");
                            _adapter.Click("Delete_Confirmation");
                            _adapter.Click("Delete_Ok");
                            TraceFactory.Logger.Info("Deleted Service Template");
                        }


                        Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                }

            }
        }

        /// <summary>
        /// Delete IPsec template
        /// </summary>
        /// <param name="templateName">IP Sec template name</param>
        /// <returns>true if ip sec template deleted successfully, false otherwise</returns>
        public void DeleteIPsecTemplate(string templateName)
        {
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                // Navigate to Ipsec template page and delete IPsec template
                _adapter.Navigate("IPsec_Firewall");
                _adapter.Click("Add_rule");
                _adapter.Click("Address_Next");
                _adapter.Click("Service_Next");
                _adapter.Check("Traffic_IPsec");
                _adapter.Click("Traffic_Next");

                DeleteListItem("IPsec_List", templateName);

                // If the specified service is used by in any rule, it can't be deleted
                if (SearchTextInPage("The selected template is currently in use by a rule"))
                {
                    if (_adapter.IsElementPresent("Error_Ok"))
                    {
                        // Click on 'Ok' button on Error page and go back to main IPsec page
                        _adapter.Click("Error_Ok");
                    }

                    TraceFactory.Logger.Info("Failed to delete ipsec template.");
                    TraceFactory.Logger.Info("Specified ipsec template : {0} is used in rule. Delete rule before deleting ipsec template.".FormatWith(templateName));
                }
                else
                {
                    TraceFactory.Logger.Info("Successfully deleted ipsec template: {0}".FormatWith(templateName));
                }

                GotoMainIPsecPage();
            }
            else
            {
                // For INKJET, deleting a ipsec template is possible only by navigating to IPSEC Template tab. From the table, select the template to be deleted and confirm delete
                // action. Here, the whole table is fetched,then the macting address template is found and verifies whetehr the check box corresponding to that template is enabled or not
                // Ideally, when the rule is enabled, the check box corresponding to the address template should be disabled and enabled only when the rule is deleted.

                int[] columnIndex = { 0 };
                StopAdapter();
                _adapter.Start("https");
                _adapter.Navigate("IPSec_Template");

                //Fetching the table containing service templates
                Collection<Collection<string>> rulesTable = _adapter.GetTable("IPsec_Template_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                int checkboxIndex = 1;

                //Parse through each row and check whether the service template to be deleted is found or not
                foreach (Collection<string> tableRow in rulesTable)
                {
                    if (tableRow[1].Trim().Equals(templateName))
                    {
                        // All radio buttons in the table are identified by a single name
                        IList<IWebElement> elements = _adapter.GetPageElements(tableRow[0], false, FindType.ByName);
                        if (!elements[checkboxIndex].Enabled)
                        {
                            // If the check box is disabled, deleting of template not possible
                            TraceFactory.Logger.Info("Failed to delete Service template.");
                        }
                        else
                        {
                            //Deleting the service template
                            elements[checkboxIndex].Click();
                            _adapter.Click("Delete");
                            _adapter.Click("Delete_Confirmation");
                            _adapter.Click("Delete_Ok");
                            TraceFactory.Logger.Info("Deleted Service Template");
                        }


                        Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                }

            }
        }

        /// <summary>
        /// Delete Custom service
        /// </summary>
        /// <param name="serviceName">Custom service name</param>
        /// <returns></returns>
        public void DeleteCustomService(string serviceName)
        {
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                // Navigate to Custom service page
                _adapter.Navigate("IPsec_Firewall");
                _adapter.Click("Add_rule");
                _adapter.Click("Address_Next");
                _adapter.Click("Service_New");
                _adapter.Click("Service_Manage_Service");
                _adapter.Click("Service_Manage_Custom");

                _adapter.Select("Custom_Service_Table", serviceName);
                _adapter.Click("Custom_Service_Delete");

                // If the specified service is used by in any rule, it can't be deleted
                if (SearchTextInPage("The selected service is currently in use by a rule"))
                {
                    if (_adapter.IsElementPresent("Error_Ok"))
                    {
                        // Click on 'Ok' button on Error page and go back to main IPsec page
                        _adapter.Click("Error_Ok");
                    }

                    TraceFactory.Logger.Info("Specified custom service : {0} is used in rule. Delete rule before deleting custom service.".FormatWith(serviceName));
                }

                // Come back to main service page once custom service is deleted
                _adapter.Click("Custom_Service_Ok");
                _adapter.Click("Service_Manage_Ok");

                GotoMainIPsecPage();
            }
            else
            {

                // For INKJET, deleting a ipsec service template is possible only by navigating to IPSEC service list tab. From the table, select the template to be deleted and confirm delete
                // action. Here, the whole table is fetched,then the macting address template is found and verifies whetehr the check box corresponding to that template is enabled or not
                // Ideally, when the rule is enabled, the check box corresponding to the address template should be disabled and enabled only when the rule is deleted.

                int[] columnIndex = { 0 };
                StopAdapter();
                _adapter.Start("https");
                _adapter.Navigate("IPSec_Services_List");

                //Fetching the table containing service templates
                Collection<Collection<string>> rulesTable = _adapter.GetTable("IPsec_Services_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                int checkboxIndex = 1;

                //Parse through each row and check whether the service template to be deleted is found or not
                foreach (Collection<string> tableRow in rulesTable)
                {
                    if (tableRow[1].Trim().Equals(serviceName))
                    {
                        // All radio buttons in the table are identified by a single name
                        IList<IWebElement> elements = _adapter.GetPageElements(tableRow[0], false, FindType.ByName);
                        if (!elements[checkboxIndex].Enabled)
                        {
                            // If the check box is disabled, deleting of template not possible
                            TraceFactory.Logger.Info("Failed to delete Service template.");
                        }
                        else
                        {
                            //Deleting the service template
                            elements[checkboxIndex].Click();
                            _adapter.Click("Delete");
                            _adapter.Click("Delete_Confirmation");
                            _adapter.Click("Delete_Ok");
                            TraceFactory.Logger.Info("Deleted Service Template");
                        }


                        Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                }
            }
        }

        /// <summary>
        /// Set DHCPV4 option in IPSecurity advanced options
        /// </summary>
        /// <param name="state">true to enable, false otherwise</param>
        /// <returns>true is set successfully, false otherwise</returns>
        public bool SetSecurityDHCPV4(bool state)
        {
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("DHCPV4", state));

            _adapter.Navigate("IPsec_Firewall");

            // VEP/ LFP will have fail safe option in Advanced settings
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                _adapter.Navigate("IPsec_Advanced");
            }

            if (state)
            {
                _adapter.Check("DHCPv4_BOOTP_Option");
            }
            else
            {
                _adapter.Uncheck("DHCPv4_BOOTP_Option");
            }

            _adapter.Click("IPSec_Apply");

            Thread.Sleep(TimeSpan.FromSeconds(5));

            if (GetSecurityDHCPV4().Equals(state))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("DHCPV4", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("DHCPV4", state));
                return false;
            }
        }

        /// <summary>
        /// Get DHCPV4 option state of IPSecurity Page
        /// </summary>
        /// <returns>true if set, false otherwise</returns>
        public bool GetSecurityDHCPV4()
        {
            TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("DHCPV4"));

            _adapter.Navigate("IPsec_Firewall");

            // VEP/ LFP will have fail safe option in Advanced settings
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                _adapter.Navigate("IPsec_Advanced");
            }

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("DHCPV4", _adapter.IsChecked("DHCPv4_BOOTP_Option") ? "enable" : "disable"));

            return _adapter.IsChecked("DHCPv4_BOOTP_Option");
        }

        #endregion IP Security/ Firewall

        #region 802.1x

        /// <summary>
        /// Configure 802.1x authentication through web UI.
        /// </summary>
        /// <param name="configurationDetails"><see cref="Dot1XConfigurationDetails"/></param>
        /// <returns>True if the configuration is successful. else false.</returns>
        public bool Set802Dot1XAuthentication(Dot1XConfigurationDetails configurationDetails)
        {
            if (configurationDetails.AuthenticationProtocol == AuthenticationMode.None || configurationDetails.AuthenticationProtocol.HasFlag(AuthenticationMode.MSCHAPV2))
            {
                TraceFactory.Logger.Info("The authentication mode: {0} is not supported on the printer.".FormatWith(AuthenticationMode.None | AuthenticationMode.MSCHAPV2));
                return false;
            }

            try
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Stop();
                    _adapter.Start();
                }

                if (IsOmniOpus)
                {
                    if (!ConfigureCiphers(configurationDetails.EncryptionStrength))
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Info("Configuring 802.1x Authentication mode to {0} through Web UI".FormatWith(configurationDetails.AuthenticationProtocol));
                TraceFactory.Logger.Info("EncryptionStrength:	{0}".FormatWith(configurationDetails.EncryptionStrength));
                TraceFactory.Logger.Info("User Name:	{0}".FormatWith(configurationDetails.UserName));
                TraceFactory.Logger.Info("Password:	{0}".FormatWith(configurationDetails.Password));
                TraceFactory.Logger.Info("Server Id:	{0}".FormatWith(configurationDetails.ServerId));
                TraceFactory.Logger.Info("Fail Safe:	{0}".FormatWith(configurationDetails.FailSafe));
                TraceFactory.Logger.Info("ReAuthenticate On Apply:	{0}".FormatWith(configurationDetails.ReAuthenticate ? "Enabled" : "Disabled"));

                _adapter.Navigate("802.1x_Authentication");

                if (configurationDetails.AuthenticationProtocol.HasFlag(AuthenticationMode.PEAP))
                {
                    _adapter.Check("Dot1x_PEAP");
                }
                else
                {
                    _adapter.Uncheck("Dot1x_PEAP");
                }

                if (configurationDetails.AuthenticationProtocol.HasFlag(AuthenticationMode.EAPTLS))
                {
                    _adapter.Check("Dot1x_EAPTLS");
                }
                else
                {
                    _adapter.Uncheck("Dot1x_EAPTLS");
                }

                if (!(configurationDetails.AuthenticationProtocol.HasFlag(AuthenticationMode.PEAP) | configurationDetails.AuthenticationProtocol.HasFlag(AuthenticationMode.EAPTLS)))
                {
                    TraceFactory.Logger.Info("The authentication protocol: {0} is not supported by the printer.".FormatWith(configurationDetails.AuthenticationProtocol));
                    return false;
                }

                _adapter.SetText("Dot1x_UserName", configurationDetails.UserName);
                _adapter.SetText("Dot1x_Password", configurationDetails.Password);
                _adapter.SetText("Dot1x_ConfirmPassword", configurationDetails.Password);

                if (_adapter.IsElementPresent("Dot1x_Server_Id"))
                {
                    _adapter.SetText("Dot1x_Server_Id", string.IsNullOrEmpty(configurationDetails.ServerId) ? string.Empty : configurationDetails.ServerId);

                    if (string.IsNullOrEmpty(configurationDetails.ServerId))
                    {
                        _adapter.Uncheck("Dot1x_Require_Exact_Match");
                    }
                    else
                    {
                        _adapter.Check("Dot1x_Require_Exact_Match");
                    }
                }

                if (!IsOmniOpus && _adapter.IsElementPresent("Dot1x_EncryptionStrength"))
                {
                    _adapter.SelectByValue("Dot1x_EncryptionStrength", CtcUtility.GetEnumvalue(Enum<EncryptionStrengths>.Value(configurationDetails.EncryptionStrength), _adapter.Settings.ProductType));
                }

                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    _adapter.SelectByValue("OnAuthenticationFail", Enum<FallbackOption>.Value(configurationDetails.FailSafe));
                }
                else
                {
                    if (configurationDetails.FailSafe == FallbackOption.Connect)
                    {
                        _adapter.Check("Dot1x_Connect");
                    }
                    else
                    {
                        _adapter.Check("Dot1x_Block");
                    }
                }

                // Inkjet does not support Re-authenticate on apply option.
                if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
                {
                    if (configurationDetails.ReAuthenticate)
                    {
                        _adapter.Check("Dot1x_Authenticate_on_Apply");
                    }
                    else
                    {
                        _adapter.Uncheck("Dot1x_Authenticate_on_Apply");
                    }
                    _adapter.Click("Dot1x_Apply");
                }
                else
                {
                    // Inkjet has an additional confirmation page.
                    _adapter.Click("Dot1x_Apply");
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                    if (_adapter.SearchText("Printer is processing your request"))
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                        _adapter.Click("Dot1x_Confirm");
                    }
                }
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Debug("Exception details: {0}".FormatWith(defaultException.Message));
            }
            finally
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
                // Browser becomes inactive and goes to infinite loop; trying to stop adapter and kill explicitly
                StopAdapter();

                _adapter.Start();
            }

            // When fail safe is disabled, EWS page may not be accessible after applying the settings. So validation is not performed for block n/w
            if (configurationDetails.FailSafe == FallbackOption.Block)
            {
                TraceFactory.Logger.Debug("Since Failsafe option is disabled, will not be able to access printer till port is also enabled for 802.1x");
                return true;
            }

            Dot1XConfigurationDetails actualValues = Get802Dot1XAuthentication();

            if (actualValues.AuthenticationProtocol == configurationDetails.AuthenticationProtocol &&
                    actualValues.UserName.EqualsIgnoreCase(actualValues.UserName) &&
                    ((_adapter.IsElementPresent("Dot1x_EncryptionStrength")) ? actualValues.EncryptionStrength == configurationDetails.EncryptionStrength : true)
                    && actualValues.FailSafe == configurationDetails.FailSafe && ((_adapter.IsElementPresent("Dot1x_Server_Id") && !string.IsNullOrEmpty(configurationDetails.ServerId)) ? (actualValues.ServerId == configurationDetails.ServerId && actualValues.RequireServerIdMatch) : true))

            {
                TraceFactory.Logger.Info("Successfully configured 802.1x authentication.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to configure 802.1x authentication.");
                return false;
            }
        }

        /// <summary>
        /// Get the <see cref="Dot1XConfigurationDetails"/> from web UI.
        /// </summary>
        /// <returns><see cref="Dot1XConfigurationDetails"/></returns>
        public Dot1XConfigurationDetails Get802Dot1XAuthentication()
        {
            Dot1XConfigurationDetails details = new Dot1XConfigurationDetails();

            _adapter.Navigate("802.1x_Authentication");
            details.AuthenticationProtocol = _adapter.IsChecked("Dot1x_PEAP") ? details.AuthenticationProtocol | AuthenticationMode.PEAP : details.AuthenticationProtocol & ~AuthenticationMode.PEAP;
            details.AuthenticationProtocol = _adapter.IsChecked("Dot1x_EAPTLS") ? details.AuthenticationProtocol | AuthenticationMode.EAPTLS : details.AuthenticationProtocol & ~AuthenticationMode.EAPTLS;

            details.UserName = _adapter.GetValue("Dot1x_UserName");

            if (_adapter.IsElementPresent("Dot1x_EncryptionStrength"))
            {
                details.EncryptionStrength = CtcUtility.GetEnum<EncryptionStrengths>(_adapter.GetValue("Dot1x_EncryptionStrength"), _adapter.Settings.ProductType);//Enum<EncryptionStrengths>.Parse(_adapter.GetValue("Dot1x_EncryptionStrength"));
            }

            details.FailSafe = _adapter.Settings.ProductType == PrinterFamilies.TPS ? Enum<FallbackOption>.Parse(_adapter.GetValue("OnAuthenticationFail"))
                : _adapter.IsChecked("Dot1x_Connect") ? FallbackOption.Connect : FallbackOption.Block;
            details.ReAuthenticate = (_adapter.Settings.ProductType != PrinterFamilies.InkJet && _adapter.IsChecked("Dot1x_Authenticate_on_Apply"));

            if (_adapter.IsElementPresent("Dot1x_Server_Id"))
            {
                details.ServerId = _adapter.GetValue("Dot1x_Server_Id");
                details.RequireServerIdMatch = _adapter.IsChecked("Dot1x_Require_Exact_Match");
                TraceFactory.Logger.Info(_adapter.GetValue("Dot1x_Server_Id"));
            }

            TraceFactory.Logger.Info("Authentication Mode:{0}      , UserName:{1}     , EncryptionStrength:{2}      , FailSafe: {3}".FormatWith(details.AuthenticationProtocol, details.UserName, details.EncryptionStrength, details.FailSafe));
            return details;
        }

        /// <summary>
        /// Reset the 802.1x configuration to default values.
        /// </summary>
        /// <returns>True if the configuration is successfully reset, else false.</returns>
        public bool Reset802Dot1XAuthentication()
        {
            try
            {
                TraceFactory.Logger.Debug("Restoring 802.1x default values");
                _adapter.Navigate("802.1x_Authentication");
                _adapter.Click("Dot1x_Restore_Defaults");
                Thread.Sleep(TimeSpan.FromSeconds(5));

                bool tpsProduct = _adapter.Settings.ProductType == PrinterFamilies.TPS;
                bool inkjetProduct = _adapter.Settings.ProductType == PrinterFamilies.InkJet;

                if (tpsProduct || inkjetProduct)
                {
                    ClickonConfirmation();
                }

                if (!tpsProduct)
                {
                    _adapter.Click("Dot1x_Reset_Ok");
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));

                TraceFactory.Logger.Info("## Successfully restored the 802.1x default values");
                return true;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception details: {0}".FormatWith(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Configure selectedeCiphers based on the encryption strength on the Secure Communication page. Applicable only for VEP Omni/Opus builds
        /// </summary>
        /// <param name="encryptionStrength"><see cref="EncryptionStrengths"/></param>
        /// <returns>True if the selectedeCiphers are successfully configured.</returns>
        public bool ConfigureCiphers(EncryptionStrengths encryptionStrength)
        {
            if (IsOmniOpus)
            {
                if (encryptionStrength == EncryptionStrengths.Low)
                {
                    TraceFactory.Logger.Info("Encryption strength Low is not supported on Omni/Opus.");
                    return false;
                }

                try
                {
                    _adapter.Navigate("SecureCommunication");

                    List<string> selectedeCiphers;

                    if (encryptionStrength == EncryptionStrengths.Low)
                    {
                        selectedeCiphers = Enum<Ciphers>.Value(Ciphers.LowEncryptionStrength).Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    else if (encryptionStrength == EncryptionStrengths.Medium)
                    {
                        selectedeCiphers = Enum<Ciphers>.Value(Ciphers.MediumEncryptionStrenth).Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    else
                    {
                        selectedeCiphers = Enum<Ciphers>.Value(Ciphers.HighEncryptionStrength).Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    //adding this delay to varify page loads properly
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    // Remove all from destination list.
                    foreach (var item in _adapter.GetListItems("DestinationSelect"))
                    {
                        _adapter.Select("DestinationSelect", item);
                    }

                    _adapter.Click("Remove");

                    Thread.Sleep(TimeSpan.FromSeconds(15));

                    _adapter.DeselectAll("SourceSelect");

                    // SElect the ciphers to be configured.
                    foreach (var item in selectedeCiphers)
                    {
                        _adapter.Select("SourceSelect", item);
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    _adapter.Click("Select");
                    _adapter.Click("Apply");

                    Thread.Sleep(TimeSpan.FromSeconds(15));

                    if (!_adapter.SearchText("Changes have been made successfully"))
                    {
                        TraceFactory.Logger.Info("Failed to select the required Ciphers for Encryption Strength: {0}.".FormatWith(encryptionStrength));
                        return false;
                    }

                    if (GetConfiguredCiphers().Except(selectedeCiphers).ToList().Count == 0)
                    {
                        TraceFactory.Logger.Info("Successfully configured the ciphers for encryption length: {0}.".FormatWith(encryptionStrength));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to configure the Ciphers for Encryption Strength: {0}.".FormatWith(encryptionStrength));
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Info("Failed to configure the Ciphers for Encryption Strength: {0}.".FormatWith(encryptionStrength));
                    TraceFactory.Logger.Info("Error Details: {0}".FormatWith(ex.Message));
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("Configuring Ciphers from Secure Communication page is available only for Omni/Opus UI.");
                return false;
            }
        }

        /// <summary>
        /// Gets the selected Ciphers
        /// </summary>
        /// <returns>A list of selected ciphers.</returns>
        private List<string> GetConfiguredCiphers()
        {
            _adapter.Navigate("SecureCommunication");
            Thread.Sleep(TimeSpan.FromSeconds(10));

            return _adapter.GetListItems("DestinationSelect");
        }

        /// <summary>
        /// Gets the status of 802.1x fom 802.1x authentication page. Currently can be used only for VEP
        /// </summary>
        /// <returns>True if it is set, else false.</returns>
        public bool Get802Dot1xStatus()
        {
            TraceFactory.Logger.Info("Getting the status of 802.1x Authentication from Web UI.");

            _adapter.Navigate("802.1x_Authentication");

            if (_adapter.IsChecked("Dot1x_PEAP") || _adapter.IsChecked("Dot1x_EAPTLS"))
            {
                TraceFactory.Logger.Info("802.1x Authentication is enabled on the device.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("802.1x Authentication settings are cleared on the device.");
                return false;
            }
        }

        #endregion 802.1x

        #region FIPS

        ///  <summary>
        ///  Enable/Disable FIPS on the device through WEBUI
        ///  </summary>
        ///  <param name="status">to enable/disable option</param>
        public bool SetFipsOption(bool status)
        {
            try
            {
                TraceFactory.Logger.Debug(SET_DEBUG_LOG.FormatWith("FIPS", status));
                //StopAdapter();
                //_adapter.Start();
                //Thread.Sleep(TimeSpan.FromSeconds(60));
                if (IsOmniOpus)
                {
                    _adapter.Navigate("SecureCommunication");
                }
                else
                {
                    _adapter.Navigate("MngmtProtocol_WebMngmt");
                }
                Thread.Sleep(TimeSpan.FromSeconds(30));
                if (status.Equals(true))
                {
                    _adapter.Check("EnableFIPS");
                }
                else
                {
                    _adapter.Uncheck("EnableFIPS");
                }
                _adapter.Click("ApplySecurity");
                Thread.Sleep(TimeSpan.FromSeconds(25));

                if (_adapter.SearchText("Changes have been made successfully"))
                {
                    TraceFactory.Logger.Info("Successfully configured the FIPS option to {0} state".FormatWith(status));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to configure the FIPS option to {0} state".FormatWith(status));
                    return false;
                }
            }
            catch (Exception fipsException)
            {
                TraceFactory.Logger.Info(fipsException.Message);
                return false;
            }
            finally
            {
                //Added since FIPs and TLS tests in security were failing 
                StopAdapter();
                _adapter.Start();
            }
        }

        /// <summary>
        /// Get the status of FIPS
        /// </summary>
        /// <returns>true/false</returns>
        public bool GetFipsOption()
        {
            TraceFactory.Logger.Info("Getting FIPS option status of the printer");
            _adapter.Stop();
            _adapter.Start();

            if (IsOmniOpus)
            {
                _adapter.Navigate("SecureCommunication");
            }
            else
            {
                _adapter.Navigate("MngmtProtocol_WebMngmt");
            }

            if (_adapter.IsChecked("EnableFIPS"))
            {
                TraceFactory.Logger.Info("FIPS Option is in enabled state");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("FIPS Option is in disabled state");
                return false;
            }
        }

        /// <summary>
        /// Enable/Disable TLS option on the device through WEBUI
        /// </summary>
        /// <param name="status">true/false</param>
        /// <param name="tlsMode">TLS1.2,TLS1.1,TLS1.0,SSL3.0</param>
        public bool SetTLSOption(string tlsMode, bool status)
        {
            try
            {
                TraceFactory.Logger.Info("Configuring {0} Option to {1} state".FormatWith(tlsMode, status));
                Thread.Sleep(TimeSpan.FromSeconds(30));
                if (IsOmniOpus)
                {
                    _adapter.Navigate("SecureCommunication");
                }
                else
                {
                    _adapter.Navigate("MngmtProtocol_WebMngmt");
                }

                if (status.Equals(true))
                {
                    _adapter.Check(tlsMode);
                }
                else
                {
                    _adapter.Uncheck(tlsMode);
                }

                _adapter.Click("ApplySecurity");
                Thread.Sleep(TimeSpan.FromSeconds(25));

                if (_adapter.SearchText("Changes have been made successfully"))
                {
                    TraceFactory.Logger.Info("Successfully configured the {0} option to {1} stats".FormatWith(tlsMode, status));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to configure the {0} option to {1} stats".FormatWith(tlsMode, status));
                    return false;
                }
            }
            catch (Exception tlsException)
            {
                TraceFactory.Logger.Info(tlsException.Message);
                return false;
            }
            finally
            {
                StopAdapter();
                _adapter.Start();
            }
        }

        /// <summary>
        /// Get the status of TLS/SSL3 option
        /// </summary>
        /// <returns>true/false</returns>
        public bool GetTLSOption(string option)
        {
            TraceFactory.Logger.Info("Getting {0} option of the printer".FormatWith(option));

            if (IsOmniOpus)
            {
                _adapter.Navigate("SecureCommunication");
            }
            else
            {
                _adapter.Navigate("MngmtProtocol_WebMngmt");
            }

            return _adapter.IsChecked(option);
        }

        /// <summary>
        /// Sets Encryption strength for WebManagement Protocol Page
        /// </summary>
        /// <param name="strength">EncryptionStrength Low, High, Medium etc.</param>
        ///
        public bool SetEncryptionStrength(EncryptionStrengths strength)
        {
            try
            {
                TraceFactory.Logger.Info("Setting EncryptionStrength of WebManangement page to {0}".FormatWith(strength));
                Thread.Sleep(TimeSpan.FromSeconds(30));
                if (IsOmniOpus)
                {
                    return ConfigureCiphers(strength);
                }
                else
                {
                    _adapter.Navigate("MngmtProtocol_WebMngmt");
                    _adapter.SelectDropDown("EncryptionStrength", strength.ToString());
                    _adapter.Click("ApplySecurity");
                    Thread.Sleep(TimeSpan.FromSeconds(25));

                    TraceFactory.Logger.Info("Successfully configured the EncryptionStrength of WebManangement page to {0}".FormatWith(strength));
                    return true;
                }
            }
            catch (Exception encryptionstrengthException)
            {
                TraceFactory.Logger.Info(encryptionstrengthException.Message);
                return false;
            }
        }

        /// <summary>
        /// Get the Encryption Strength Value
        /// </summary>
        /// <returns>Encryption Strength</returns>
        public string GetEncryptionStrength()
        {
            TraceFactory.Logger.Info("Getting Encryption Strength");
            StopAdapter();
            _adapter.Start();
            
            string encryption = string.Empty;
            if (IsOmniOpus)
            {
                List<string> ciphers = GetConfiguredCiphers();
                TraceFactory.Logger.Info("Number of Ciphers in Destination table : {0}".FormatWith(ciphers.Count));
                Ciphers cipher = (from item in Enum<Ciphers>.EnumValues where !ciphers.Except(item.Value.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries).ToList()).Any() select (Ciphers) item.Key).FirstOrDefault();
                switch (cipher)
                {
                    case Ciphers.None:
                        encryption = string.Empty;
                        break;

                    case Ciphers.LowEncryptionStrength:
                        encryption = EncryptionStrengths.Low.ToString();
                        break;

                    case Ciphers.MediumEncryptionStrenth:
                        encryption = EncryptionStrengths.Medium.ToString();
                        break;

                    case Ciphers.HighEncryptionStrength:
                        encryption = EncryptionStrengths.High.ToString();
                        break;
                }
            }
            else
            {
                _adapter.Navigate("MngmtProtocol_WebMngmt");
                encryption = _adapter.GetValue("EncryptionStrength");
                TraceFactory.Logger.Info("Encryption Strength is {0}".FormatWith(encryption));
            }
            TraceFactory.Logger.Info("Encryption strength value is : {0}".FormatWith(encryption));
            return encryption;
        }

        /// <summary>
        /// Validating the SNMP Page for Low encryption ciphers MD5/DES
        /// </summary>
        /// <returns>true/false</returns>
        public bool ValidateSNMPPage()
        {
            TraceFactory.Logger.Info("Validating the SNMP Page for Low encryption ciphers MD5/DES");

            _adapter.Navigate("MngmtProtocol_SNMP");
            if (!(_adapter.GetValue("AuthenticationProtocol").Contains("MD5")) && !(_adapter.GetValue("PrivacyProtocol").Contains("DES")))
            {
                TraceFactory.Logger.Info("Low Encryption ciphers MD5/DES is disabled for SNMPV3");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Low Encryption ciphers MD5/DES is enabled for SNMPV3");
                return false;
            }
        }

        /// <summary>
        /// Setting SNMPV3 Authentication Protocol
        /// </summary>
        /// <returns>true/false</returns>
        public bool SetSNMPV3AuthenticationProtocol(string authProtocol, string userName, string authPassPhrase, string privacyPassPhrase, bool enableSNMPV3)
        {
            try
            {
                TraceFactory.Logger.Info("Setting Authentication protocol of SNMP page to {0}".FormatWith(authProtocol));

                _adapter.Navigate("MngmtProtocol_SNMP");
                if (enableSNMPV3)
                {
                    _adapter.Check("Enable_SNMPv3");
                    _adapter.SetText("User_Name", userName);
                    _adapter.SelectDropDown("AuthenticationProtocol", authProtocol);
                    _adapter.SetText("AuthenticationProtocolPassphrase", authPassPhrase);
                    _adapter.SetText("PrivacyProtocolPassphrase", privacyPassPhrase);
                }
                else
                {
                    _adapter.Uncheck("Enable_SNMPv3");
                }
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(25));

                TraceFactory.Logger.Info("Successfully configured the  Authentication protocol of SNMP page to {0}".FormatWith(authProtocol));
                return true;
            }
            catch (Exception snmpv3Exception)
            {
                TraceFactory.Logger.Info(snmpv3Exception.Message);
                return false;
            }
        }

        /// <summary>
        ///Upgrade/Downgrade Firmware in the device
        /// </summary>
        /// <param name="path">Path of the firmware file</param>
        /// <returns>true/false</returns>
        public bool InstallFirmware(string path)
        {
            try
            {
                TraceFactory.Logger.Info("Upgrading/downgrading the firmware: {0} in the device".FormatWith(path));

                _adapter.Navigate("FirmwareUpgrade");

                if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
                {
                    _adapter.SetBrowseControlText("FilePath", path);

                    // clicking install button is throwing exception,since the WebUI is taking long time to respond back
                    try
                    {
                        _adapter.Click("Install");
                    }
                    catch
                    {
                        // ignored
                    }
                    finally
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(4));
                        if (_adapter.IsElementPresent("Restart"))
                        {
                            _adapter.Click("Restart");
                        }
                        Thread.Sleep(TimeSpan.FromMinutes(6));
                    }
                }
                else if (_adapter.Settings.ProductType == PrinterFamilies.LFP)
                {
                    try
                    {
                        string executablePath = @"{0}\{1}".FormatWith(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "UploadFile.exe");
                        _adapter.Click("SelectFile");
                        ProcessUtil.Execute("cmd.exe", "/C \"{0}\" {1} {1}".FormatWith(executablePath, path));
                        TraceFactory.Logger.Debug("File Upload popup is present.");
                        _adapter.Click("Update");
                        TraceFactory.Logger.Info("Firmware Upload in Progress, Please wait, it may take almost 1.5 hr to complete");
                        Thread.Sleep(TimeSpan.FromMinutes(100));                        
                        if (!_adapter.SearchText("success"))
                        {
                            TraceFactory.Logger.Info("Failed to upload firmware file");
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        TraceFactory.Logger.Debug("File Upload popup is not present.");
                    }
                }

                if (!ValidateFirmware(Path.GetFileNameWithoutExtension(path)))
                {
                    TraceFactory.Logger.Info("Failed to upgraded/downgraded the firmware");
                    return false;
                }
                return true;
            }
            catch (Exception snmpv3Exception)
            {
                TraceFactory.Logger.Info(snmpv3Exception.Message);
                return false;
            }
        }

        /// <summary>
        ///  Validate firmware in configuration page of information tab
        /// </summary>
        /// <param name="firmware"></param>
        /// <returns>true/false</returns>
        public bool ValidateFirmware(string firmware)
        {
            try
            {
                TraceFactory.Logger.Info("Validating the firmware in configuration page of Information tab");
                TraceFactory.Logger.Info("firmware:{0}".FormatWith(firmware));
                Thread.Sleep(TimeSpan.FromMinutes(3));

                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("InformationConfiguration_Page");
                Thread.Sleep(TimeSpan.FromMinutes(1));
                string firmware_trimed = firmware.Substring(0, firmware.Length-10);
                if (firmware_trimed.Contains(_adapter.GetText("FirmwareRevision")) || firmware_trimed.Contains(_adapter.GetText("FirmwareDatecode")))
                {
                    TraceFactory.Logger.Info("Successfully validated the firmware:{0} from configuration page".FormatWith(firmware));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to validate the firmware:{0} from configuration page.".FormatWith(firmware));
                    return false;
                }
            }
            catch (Exception snmpv3Exception)
            {
                TraceFactory.Logger.Info(snmpv3Exception.Message);
                return false;
            }
        }

        #endregion FIPS

        #region Password

        /// <summary>
        /// Sets the admin password through web UI.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="setPasswordAsCommunityName"></param>
        /// <returns></returns>
        public bool SetAdminPassword(string password, bool setPasswordAsCommunityName = false)
        {
            TraceFactory.Logger.Info("Setting the admin password to {0}.".FormatWith(password));

            _adapter.Navigate("Admin_Account");

            string executablePath = @"{0}\{1}".FormatWith(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "Authenticate.exe");

            _adapter.SetText("Password", password);
            _adapter.SetText("Confirm_Password", password);

            _adapter.Click("Password_Apply");

            Thread.Sleep(TimeSpan.FromSeconds(30));

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                if (_adapter.SearchText("successfully"))
                {
                    TraceFactory.Logger.Info(SUCCESS_LOG.FormatWith("Admin Password", password));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info(FAILURE_LOG.FormatWith("Admin Password", password));
                    return false;
                }
            }
            else
            {
                try
                {
                    _adapter.Click("Password_Ok");
                }
                catch
                {
                    // ignored
                }

                // Password is used for the username as it can be anything
                var result = ScalableTest.Utility.ProcessUtil.Execute("cmd.exe", "/C \"{0}\" {1} {2}".FormatWith(executablePath, password, password));
                if (result.ExitCode == 0)
                {
                    TraceFactory.Logger.Debug("Authentication popup is present.");
                    TraceFactory.Logger.Info(SUCCESS_LOG.FormatWith("admin password", password));
                    return true;
                }
                else if (result.ExitCode == 1)
                {
                    TraceFactory.Logger.Debug("No authentication popup is present.");
                    TraceFactory.Logger.Info(FAILURE_LOG.FormatWith("admin password", password));
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Delete admin password from web UI.
        /// </summary>
        /// <returns>True if the password is deleted, else false.</returns>
		public bool DeleteAdminPassword(string password)
        {
            TraceFactory.Logger.Info("Deleting the admin password.");

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                if (!SetPasswordComplexity(false, 0, true, password))
                {
                    return false;
                }
            }

            _adapter.Navigate("Admin_Account");

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                _adapter.SetText("Old_Password", password);
            }
            _adapter.SetText("Password", string.Empty);
            _adapter.SetText("Confirm_Password", string.Empty);
            _adapter.Click("Password_Apply");

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                if (_adapter.SearchText("successfully"))
                {
                    TraceFactory.Logger.Info("Successfully deleted the admin password.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to delete the admin password.");
                    return false;
                }
            }
            else
            {
                _adapter.Click("ClearPassword_Ok");

                if (string.IsNullOrEmpty(_adapter.GetText("Password")))
                {
                    TraceFactory.Logger.Info("Successfully deleted the admin password.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to delete the admin password.");
                    return false;
                }
            }
        }

        public bool Login(string password)
        {
            StopAdapter();
            _adapter.Start();

            try
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
                {
                    _adapter.Navigate("Sign_In");

                    _adapter.SetText("Password", password);
                    _adapter.Click("SignIn_Ok");

                    Thread.Sleep(TimeSpan.FromSeconds(30));

                    if (!_adapter.SearchText("Sign In failed"))
                    {
                        TraceFactory.Logger.Info("Successfully logged in to Web UI.");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to login to Web UI.");
                        return false;
                    }
                }
                else if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    try
                    {
                        //Navigate to home screen as it will ask for password
                        _adapter.ClickonLink("Sign In");
                    }
                    catch
                    {
                        // ignored
                    }

                    // Password is used for the username as it can be anything
                    string executablePath = @"{0}\{1}".FormatWith(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "Authenticate.exe");

                    var result = ScalableTest.Utility.ProcessUtil.Execute("cmd.exe", "/C \"{0}\" {1} {1}".FormatWith(executablePath, password));
                    if (result.ExitCode == 0)
                    {
                        TraceFactory.Logger.Debug("Authentication popup is present.");
                    }
                    else if (result.ExitCode == 1)
                    {
                        TraceFactory.Logger.Debug("No authentication popup is present.");
                    }

                    TraceFactory.Logger.Info("Successfully logged in to Web UI.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Not implemented for LFP and Inkjet products");
                    throw new NotImplementedException("Not implemented for LFP and Inkjet products");
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Failed to login to Web UI.");
                TraceFactory.Logger.Debug("Exception Details: {0}".FormatWith(ex.Message));
                return false;
            }
        }

        public bool ValidateLogin(string password)
        {
            if (!Login(password))
            {
                return false;
            }

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                _adapter.Navigate("Home");

                if ((_adapter.IsElementPresent("Home_UserName", retry: false) && _adapter.IsElementPresent("Home_Link", retry: false) && _adapter.IsElementPresent("SignOut_Link", retry: false)))
                {
                    TraceFactory.Logger.Info("The strings User, Home, Home | Sign Out are present on the page.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("The strings User, Home, Home | Sign Out are NOT present on the page.");
                    return false;
                }
            }

            return true;
        }

        public bool ValidateHomeScreenPages(bool checkForPresence = true, bool excludeInfoPage = false)
        {
            bool result = true;

            _adapter.Navigate("Home");

            // Information tab is excluded as for
            string[] homeScreenTabs = new string[] { HOMESCREENTABS_BEFORE_LOGIN, "Settings", "Troubleshooting", "Security", "Networking" };

            StringBuilder presentTabs = new StringBuilder();
            StringBuilder absentTabs = new StringBuilder();

            foreach (string page in homeScreenTabs)
            {
                if (excludeInfoPage && page.EqualsIgnoreCase(HOMESCREENTABS_BEFORE_LOGIN))
                {
                    continue;
                }

                if (_adapter.IsElementPresent(page, retry: false))
                {
                    presentTabs.AppendFormat("{0} ", page);
                    result &= (checkForPresence);
                }
                else
                {
                    absentTabs.AppendFormat("{0} ", page);
                    result &= (!checkForPresence);
                }
            }

            if (presentTabs.Length != 0)
            {
                TraceFactory.Logger.Info("{0} tabs are present in the Home page.".FormatWith(presentTabs));
            }

            if (absentTabs.Length != 0)
            {
                TraceFactory.Logger.Info("{0} tabs are not present in the Home page.".FormatWith(absentTabs));
            }

            return result;
        }

        public bool ValidatePasswordStatus()
        {
            _adapter.Navigate("Admin_Account");

            if (!(string.IsNullOrEmpty(_adapter.GetValue("Password")) && string.IsNullOrEmpty(_adapter.GetValue("Confirm_Password"))))
            {
                TraceFactory.Logger.Info("Admin password is set in Web UI.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Admin password is not set in Web UI.");
                return false;
            }
        }

        #endregion Password

        #region Secure Protocols

        /// <summary>
        /// Setting TLS option on the printer
        /// </summary>
        /// <param name="protocol">TLS option to be set on the printer</param>
        public void SetSecureProtocol(SecureProtocol protocol)
        {
            try
            {
                TraceFactory.Logger.Info("Enabling Secure Protocols to {0} option".FormatWith(protocol));

                if (IsOmniOpus)
                {
                    _adapter.Navigate("SecureCommunication");
                }
                else
                {
                    _adapter.Navigate("MngmtProtocol_WebMngmt");
                }

                // first uncheck all the options and enable the required options
                _adapter.Uncheck("TLS1.0");
                _adapter.Uncheck("TLS1.1");
                _adapter.Uncheck("TLS1.2");

                // select the user required option
                switch (protocol)
                {
                    case SecureProtocol.TLS10:
                        _adapter.Check("TLS1.0");
                        break;

                    case SecureProtocol.TLS11:
                        _adapter.Check("TLS1.1");
                        break;

                    case SecureProtocol.TLS12:
                        _adapter.Check("TLS1.2");
                        break;

                    case SecureProtocol.AllTLS:
                        _adapter.Check("TLS1.0");
                        _adapter.Check("TLS1.1");
                        _adapter.Check("TLS1.2");
                        break;

                    default:
                        TraceFactory.Logger.Error("Unsupported {0} option given".FormatWith(protocol));
                        break;
                }

                _adapter.Click("ApplySecurity");
                Thread.Sleep(TimeSpan.FromSeconds(25));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info(ex.Message);
            }
        }

        /// <summary>
        /// Sets Encrypt Web Communication option
        /// </summary>
        /// <param name="encryptOption">Enable/Disable option</param>
        public void SetEncryptWebCommunication(bool encryptOption)
        {
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                try
                {
                    TraceFactory.Logger.Info("Setting Encrypt Web Communication option to {0}".FormatWith(encryptOption));

                    if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                    {
                        _adapter.Stop();
                        _adapter.Start();
                        _adapter.Navigate("Enforcememt_HTTPS");
                        if (encryptOption)
                        {
                            _adapter.Check("Always_Use_HTTPS");
                        }
                        else
                        {
                            _adapter.Uncheck("Always_Use_HTTPS");
                        }

                        _adapter.Click("Apply");
                    }
                    else
                    {
                        _adapter.Navigate("MngmtProtocol_WebMngmt");


                        if (encryptOption)
                        {
                            _adapter.Check("Encrypt_All");
                        }
                        else
                        {
                            _adapter.Uncheck("Encrypt_All");
                        }

                        _adapter.Click("ApplySecurity");
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(25));
                    TraceFactory.Logger.Info("Encryption is successfully set to : {0}".FormatWith(encryptOption));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Info(ex.Message);
                }
            }


        }

        /// <summary>
        /// Gets Encrypt Web Communication option
        /// </summary>
        /// <returns></returns>
        public bool GetEncryptWebCommunication()
        {
            _adapter.Navigate("MngmtProtocol_WebMngmt");
            return _adapter.GetValue("Encrypt_All") == "on";
        }

        #endregion Secure Protocols

        public bool ResetDefaults()
        {
            _adapter.Navigate("SecuritySettings");
            _adapter.Click("RestoreSecurityDefaults");

            ClickonConfirmation();

            return true;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Create Address template for IP Sec rule
        /// </summary>
        /// <param name="templateSettings"><see cref=" AddressTemplateSettings"/></param>
        /// <returns>true if address is selected</returns>
        public void CreateAddressTemplate(AddressTemplateSettings templateSettings)
        {
            TraceFactory.Logger.Debug("Creating Address template.");
            bool isInk = _adapter.Settings.ProductType == PrinterFamilies.InkJet;

            string address;
            int[] columnIndex = { 0 };

            // In case of custom template, address template needs to be created with requested configurations.
            if (DefaultAddressTemplates.Custom == templateSettings.DefaultTemplate)
            {
                address = templateSettings.Name;
                SetCustomAddressTemplate(templateSettings);
            }
            else
            {
                // Get the item name from list box
                address = GetListItemName("Address_List", addressTemplate: templateSettings.DefaultTemplate);
            }
            // In Inkjet the address template has to be selected using radio button where as in others it is from a list 
            if (!isInk)
            {
                _adapter.Select("Address_List", address);
            }
            else
            {
                if (DefaultAddressTemplates.Custom != templateSettings.DefaultTemplate)
                {
                    Collection<Collection<string>> rulesTable = _adapter.GetTable("Address_Template_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                    int radioIndex = 0;

                    foreach (Collection<string> tableRow in rulesTable)
                    {
                        if (tableRow[1].Trim().Equals(address))
                        {
                            // All radio buttons in the table are identified by a single name
                            IList<IWebElement> elements = _adapter.GetPageElements(tableRow[0], false, FindType.ByName);
                            elements[radioIndex].Click();

                            Thread.Sleep(TimeSpan.FromSeconds(10));
                        }
                        radioIndex++;
                    }
                }
            }
            TraceFactory.Logger.Info("Created Address template.");
        }

        /// <summary>
        /// Get the List box name for the specified default template.
        /// </summary>
        /// <param name="listControlId">List box control id</param>
        /// <param name="addressTemplate">Default Address Template</param>
        /// <param name="serviceTemplate">Default Service Template</param>
        /// <param name="isAddressTemplate">true for address template, false for service template</param>
        /// <returns></returns>
        public string GetListItemName(string listControlId, DefaultAddressTemplates addressTemplate = DefaultAddressTemplates.AllIPAddresses, DefaultServiceTemplates serviceTemplate = DefaultServiceTemplates.AllDigitalSendServices, bool isAddressTemplate = true)
        {
            string templateValue;

            // Get the Enum value based on the template requested
            if (isAddressTemplate)
            {
                templateValue = CtcUtility.GetEnumvalue(Enum<DefaultAddressTemplates>.Value(addressTemplate), _adapter.Settings.ProductType);
            }
            else
            {
                templateValue = CtcUtility.GetEnumvalue(Enum<DefaultServiceTemplates>.Value(serviceTemplate), _adapter.Settings.ProductType);
            }

            // If there is no Enum value configured, return empty
            if (string.IsNullOrEmpty(templateValue))
            {
                return string.Empty;
            }

            string defaultTemplateName = templateValue;

            // If their are multiple Enum values, check which value is matching in the list box and return the matching name
            if (templateValue.Contains('|'))
            {
                List<string> templateValues = templateValue.Split('|').ToList();
                List<string> listItems = _adapter.GetListItems(listControlId);

                foreach (string listItem in templateValues)
                {
                    foreach (string item in listItems)
                    {
                        if (listItem.EqualsIgnoreCase(item))
                        {
                            defaultTemplateName = item;
                            break;
                        }
                    }
                }
            }
            return defaultTemplateName;
        }

        /// <summary>
        /// Configure Custom Address template
        /// For Range and Prefix type: provide values with '|' delimiter
        /// For Address and Predefined use single values
        /// </summary>
        /// <param name="templateSettings"><see cref="AddressTemplateSettings"/></param>
        /// <param name="isModify">true to modify existing template, false to create new</param>
        private void SetCustomAddressTemplate(AddressTemplateSettings templateSettings, bool isModify = false)
        {
            TraceFactory.Logger.Info("Creating/Editing Custom Address template");

            // Template Name needs to be configured only when a new template is created. Name field is non-editable during modification
            if (!isModify)
            {
                _adapter.Click("Address_Template_New");
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Click("Address_Next");
                }
                _adapter.SetText("Address_Template_Name", templateSettings.Name);
            }

            TraceFactory.Logger.Info("Selected Custom Address Template Type: {0}".FormatWith(templateSettings.CustomTemplateType));

            // Both Local and Remote address values will be of same combination. Configure values based on user settings.
            switch (templateSettings.CustomTemplateType)
            {
                case CustomAddressTemplateType.IPAddress:
                    {
                        // Local
                        _adapter.Click("Address_Local_IPAddress");
                        _adapter.SelectDropDown("Address_Local_IPAddress_Value", templateSettings.LocalAddress);

                        // Remote
                        _adapter.Click("Address_Remote_IPAddress");
                        _adapter.SetText("Address_Remote_IPAddress_Value", templateSettings.RemoteAddress);

                        break;
                    }
                case CustomAddressTemplateType.PredefinedAddresses:
                    {
                        // Local
                        _adapter.Click("Address_Local_Predefined");
                        _adapter.SelectDropDown("Address_Local_Predefined_Value", templateSettings.LocalAddress);

                        // Remote
                        _adapter.Click("Address_Remote_Predefined");
                        _adapter.SelectDropDown("Address_Remote_Predefined_Value", templateSettings.RemoteAddress);

                        break;
                    }
                case CustomAddressTemplateType.IPAddressRange:
                    {
                        string[] range = templateSettings.LocalAddress.Split('|');

                        // Local
                        _adapter.Click("Address_Local_Range");
                        _adapter.SetText("Address_Local_FromRange_Value", range[0]);
                        _adapter.SetText("Address_Local_ToRange_Value", range[1]);

                        TraceFactory.Logger.Debug("Local Address Range From Value: {0}".FormatWith(range[0]));
                        TraceFactory.Logger.Debug("Local Address Range To Value: {0}".FormatWith(range[1]));

                        range = templateSettings.RemoteAddress.Split('|');

                        // Remote
                        _adapter.Click("Address_Remote_Range");
                        _adapter.SetText("Address_Remote_FromRange_Value", range[0]);
                        _adapter.SetText("Address_Remote_ToRange_Value", range[1]);

                        TraceFactory.Logger.Debug("Remote Address Range From Value: {0}".FormatWith(range[0]));
                        TraceFactory.Logger.Debug("Remote Address Range To Value: {0}".FormatWith(range[1]));
                        break;
                    }
                case CustomAddressTemplateType.IPAddressPrefix:
                    {
                        string[] prefix = templateSettings.LocalAddress.Split('|');

                        // Local
                        _adapter.Click("Address_Local_Prefix");
                        _adapter.SetText("Address_Local_Prefix_Address", prefix[0]);
                        _adapter.SetText("Address_Local_Prefix_Value", prefix[1]);

                        TraceFactory.Logger.Debug("Local IPAddress Prefix: {0}".FormatWith(prefix[0]));
                        TraceFactory.Logger.Debug("Local IPAddress Prefix Value: {0}".FormatWith(prefix[1]));

                        prefix = templateSettings.RemoteAddress.Split('|');

                        // Remote
                        _adapter.Click("Address_Remote_Prefix");
                        _adapter.SetText("Address_Remote_Prefix_Address", prefix[0]);
                        _adapter.SetText("Address_Remote_Prefix_Value", prefix[1]);

                        TraceFactory.Logger.Debug("Remote IPAddress Prefix: {0}".FormatWith(prefix[0]));
                        TraceFactory.Logger.Debug("Remote IPAddress Prefix Value: {0}".FormatWith(prefix[1]));
                        break;
                    }
                default: break;
            }

            _adapter.Click("Address_Template_Ok");

            Thread.Sleep(TimeSpan.FromSeconds(20));
            if (_adapter.IsElementPresent("Address_Template_Ok") && !_adapter.IsElementPresent("apply"))
            {
                _adapter.Click("Address_Template_Ok");
            }
            Thread.Sleep(TimeSpan.FromMinutes(1));

            TraceFactory.Logger.Info("Created Custom Address template");
        }

        /// <summary>
        /// Create Service template for IP Sec rule
        /// </summary>
        /// <param name="templateSettings"><see cref=" ServiceTemplateSettings"/></param>
        /// <returns>true if address is selected</returns>
        private void CreateServiceTemplate(ServiceTemplateSettings templateSettings)
        {
            TraceFactory.Logger.Debug("Creating Service template");

            bool isInk = _adapter.Settings.ProductType == PrinterFamilies.InkJet;
            bool isLfp = _adapter.Settings.ProductType == PrinterFamilies.LFP;
            int[] columnIndex = { 0 };
            string serviceName = null;

            // For custom service, create the service first and select
            if (DefaultServiceTemplates.Custom == templateSettings.DefaultTemplate)
            {
                TraceFactory.Logger.Debug("Creating Custom Service template");
                serviceName = templateSettings.Name;

                // For VEP/TPS, Click on New Service button, provide service template name and click on Manage Service button

                _adapter.Click("Service_New");
                if (isInk)
                {
                    _adapter.Click("Service_New_Next");
                }

                _adapter.SetText("Service_Template_Name", templateSettings.Name);

                //_adapter.Click("Service_Manage_Service");

                if (!isInk)
                {
                    if (PrinterFamilies.TPS == _adapter.Settings.ProductType)
                    {
                        _adapter.Click("Service_Manage_Service_Table");
                    }
                    else
                    {
                        _adapter.Click("Select_Srv");
                    }
                }

                TraceFactory.Logger.Debug("Selcting the Service form the Manage Service Table");

                /* There are 2 sections here:
				 * 1. Few pre-defined custom service are already configured.
				 * 2. Create a custom service
				*/
                bool IsManageService = false;
                foreach (Service service in templateSettings.Services)
                {
                    // Check if the service is default custom service or need to create a custom manage service
                    if (!service.IsDefault)
                    {
                        //since the service is custom, setting this to true
                        IsManageService = true;
                        // Create custom service if it is not a default
                        ManageCustomService(service.Name, service.Protocol, service.ServiceType, service.PrinterPort, service.RemotePort, service.IcmpType);
                        TraceFactory.Logger.Debug("Creating new Manage Cstom Service");
                       
                    }
                }

                // Select all requested services
                // Xpath table differs for Omniopus products, hence require to include headers
                if ((IsOmniOpus) || (isInk))
                {
                    //SelectMangeServices(templateSettings.Services, true);
                    SelectMangeServices(templateSettings.Services, true, IsManageService);
                }
                else
                {
                    SelectMangeServices(templateSettings.Services);
                }
            }
            else
            {
                // Get the template name from list box
                serviceName = GetListItemName("Service_List", serviceTemplate: templateSettings.DefaultTemplate, isAddressTemplate: false);
            }

            if (!isInk)
            {
                _adapter.Select("Service_List", serviceName);
            }
            else
            {
                if (DefaultServiceTemplates.Custom != templateSettings.DefaultTemplate)
                {
                    Collection<Collection<string>> rulesTable = _adapter.GetTable("Service_Template_Table", includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);
                    //In Ink, all radio buttons have the same IDs/Name 
                    int radioIndex = 0;

                    foreach (Collection<string> tableRow in rulesTable)
                    {

                        if (tableRow[1].Trim().Equals(serviceName))
                        {
                            IList<IWebElement> elements = _adapter.GetPageElements(tableRow[0], false, FindType.ByName);
                            elements[radioIndex].Click();

                            Thread.Sleep(TimeSpan.FromSeconds(10));
                        }
                        radioIndex++;
                    }

                }
            }


            TraceFactory.Logger.Info("Created Service template.");

        }

        /// <summary>
        /// Create a custom service
        /// Note: Both printer and remote port no will expect these values - Any: null, Range: values separated by '|', Specific: Single value
        /// </summary>
        /// <param name="serviceName">Custom service name</param>
        /// <param name="serviceProtocol">Custom Protocol type: <see cref=" ServiceProtocolType"/></param>
        /// <param name="serviceType">Custom Service type: <see cref=" ServiceType"/></param>
        /// <param name="servicePrinterPort">Printer port no</param>
        /// <param name="serviceRemotePort">Remote port no</param>
        /// <param name="icmpType">ICMP type value</param>
        private void ManageCustomService(string serviceName, ServiceProtocolType serviceProtocol, ServiceType serviceType, string servicePrinterPort, string serviceRemotePort, string icmpType = null)
        {
            TraceFactory.Logger.Debug("Creating Custom Manage Service template.");
            bool isInk = _adapter.Settings.ProductType == PrinterFamilies.InkJet;

            // Click on Manage Custom service and configure details to add new custom service
            _adapter.Click("Service_Manage_Custom");
            if (isInk)
            {
                _adapter.Click("Service_Manage_Custom_Next");
            }

            _adapter.SetText("Custom_Service_Name", serviceName);
            _adapter.SelectByValue("Custom_Service_Protocol", Enum<ServiceProtocolType>.Value(serviceProtocol));

            _adapter.SelectByValue("Custom_Service_Type", CtcUtility.GetEnumvalue(Enum<ServiceType>.Value(serviceType), _adapter.Settings.ProductType));

            // Printer port:
            // If there is no value specified: Any port
            // If port value contains '|' ie, 2 values separated by delimiter: Port range
            // If port value is single field: Specific port
            if (string.IsNullOrEmpty(servicePrinterPort.Trim()) || servicePrinterPort == "Any")
            {
                _adapter.Click("Printer_AnyPort");
            }
            else if (servicePrinterPort.Contains('|'))
            {
                string[] portRange = servicePrinterPort.Split('|');
                _adapter.Click("Printer_PortRange");
                _adapter.SetText("Printer_Port_FromRange", portRange[0]);
                _adapter.SetText("Printer_Port_ToRange", portRange[1]);
            }
            else
            {
                _adapter.Click("Printer_SpecificPort");
                _adapter.SetText("Printer_SpecificPort_Value", servicePrinterPort);
            }

            // Remote port
            if (string.IsNullOrEmpty(serviceRemotePort.Trim()) || serviceRemotePort == "Any")
            {
                _adapter.Click("Remote_AnyPort");
            }
            else if (serviceRemotePort.Contains('|'))
            {
                string[] portRange = serviceRemotePort.Split('|');
                _adapter.Click("Remote_PortRange");
                _adapter.SetText("Remote_Port_FromRange", portRange[0]);
                _adapter.SetText("Remote_Port_ToRange", portRange[1]);
            }
            else
            {
                _adapter.Click("Remote_SpecificPort");
                _adapter.SetText("Remote_SpecificPort_Value", serviceRemotePort);
            }

            if (!string.IsNullOrEmpty(icmpType))
            {
                // For inkjet, ICMP Message Rate Text box will be inactive if the Protocol is TCP or UDP
                if ((!isInk) || (!(isInk && ((icmpType == ServiceProtocolType.TCP.ToString()) || (icmpType == ServiceProtocolType.UDP.ToString())))))
                {
                    _adapter.SetText("ICMP_Type", icmpType);
                }
            }

            _adapter.Click("Custom_Service_Add");
            _adapter.Click("Custom_Service_Ok");

            TraceFactory.Logger.Info("Created Custom Manage Service template.");
        }

        /// <summary>
        /// Create IP Sec template for IP Sec rule
        /// </summary>
        /// <param name="templateSettings"><see cref="IPsecTemplateSettings"/></param>
        /// <param name="isModify">Option to Create new/ modify existing. True for Modification, false for creating new</param>
        /// <returns></returns>
        public bool CreateIPSecTemplate(IPsecTemplateSettings templateSettings, bool isModify = false)
        {
            bool isInk = _adapter.Settings.ProductType == PrinterFamilies.InkJet;

            if (PrinterFamilies.TPS == _adapter.Settings.ProductType)
            {
                throw new NotSupportedException("IP Security is not supported for TPS products.");
            }

            TraceFactory.Logger.Debug("Creating IPsec template.");

            if (!isModify)
            {
                // Radio button to select IP sec option and Next button then click on 'New' and provide template name
                _adapter.Check("Traffic_IPsec");
                _adapter.Click("Traffic_Next");
                _adapter.Click("IPsec_New");
                if (isInk)
                {
                    _adapter.Click("IPsec_Next"); //applicable only for INKJET
                }
                _adapter.SetText("IPsec_Name", templateSettings.Name);
            }

            // Split based on Security key type: Dynamic and Manual
            if (templateSettings.KeyType == SecurityKeyType.Dynamic)
            {
                if (!isInk)
                {
                    _adapter.Check("Dynamic_Type"); // This refers to Internet Key Exchange on web page

                }

                // IKE Strength remains same for both version 1 and version 2
                // IKE Strength
                _adapter.SelectByValue("IKE_Strength", CtcUtility.GetEnumvalue(Enum<IKESecurityStrengths>.Value(templateSettings.DynamicKeysSettings.Strengths), _adapter.Settings.ProductType));


                // 2 IKEVersion: Version 1 and Version 2
                // IKE Version1
                if (templateSettings.DynamicKeysSettings.Version == IKEVersion.IKEv1)
                {
                    _adapter.SelectByValue("IKE_Version", CtcUtility.GetEnumvalue(Enum<IKEVersion>.Value(IKEVersion.IKEv1), _adapter.Settings.ProductType));


                    // Next button
                    _adapter.Click("IPsec_Next");

                    // Split between PSK, Certificates and Kerberos configuration
                    if (IKEAAuthenticationTypes.PreSharedKey == templateSettings.DynamicKeysSettings.V1Settings.AuthenticationTypes)
                    {
                        _adapter.Check("IPsec_PSK");
                        _adapter.SetText("IPsec_PSK_Value", templateSettings.DynamicKeysSettings.V1Settings.PSKValue);
                        if (isInk)
                        {
                            _adapter.Check("IPsec_PSK_Hash_None");
                        }
                    }
                    else if (IKEAAuthenticationTypes.Certificates == templateSettings.DynamicKeysSettings.V1Settings.AuthenticationTypes)
                    {
                        _adapter.Check("IPSec_Certificates");
                    }

                    // INK doesn't support KERBEROS
                    else if (IKEAAuthenticationTypes.Kerberos == templateSettings.DynamicKeysSettings.V1Settings.AuthenticationTypes && !isInk)
                    {
                        // Click on Radio button and Configure
                        _adapter.Check("IPSec_Kerberos");
                        _adapter.Click("Kerberos_Configure");

                        if (!ConfigureKerberos(templateSettings.DynamicKeysSettings.V1Settings.KerberosSettings))
                        {
                            return false;
                        }
                    }

                    _adapter.Click("IPsecTemplate_Next");

                    // Custom Profile will have additional settings
                    if (IKESecurityStrengths.Custom == templateSettings.DynamicKeysSettings.Strengths)
                    {
                        ConfigureCustomSettings(templateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase1Settings, templateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings);
                    }
                }
                // IKE Version 2
                else
                {
                    if (!isInk) // IKEv2 not supported for this release
                    {
                        _adapter.SelectByValue("IKE_Version", Enum<IKEVersion>.Value(IKEVersion.IKEv2));

                        // Next button
                        _adapter.Click("IPsec_Next");

                        // Identity Authentication Page
                        // Local Specific values
                        if (IKEAAuthenticationTypes.PreSharedKey == templateSettings.DynamicKeysSettings.V2Settings.LocalAuthenticationType)
                        {
                            _adapter.Check("IPsec_LA_PSK");
                            _adapter.SelectByValue("IPsec_LA_PSK_IdentityType", Enum<IdentityType>.Value(templateSettings.DynamicKeysSettings.V2Settings.LocalPSKIdentityType));
                            _adapter.SetText("IPsec_LA_PSK_Identity", templateSettings.DynamicKeysSettings.V2Settings.LocalPSKIdentity);
                            _adapter.Check("IPsec_LA_ASCII");
                            _adapter.SetText("IPsec_LA_PSK_Key", templateSettings.DynamicKeysSettings.V2Settings.LocalPSKKey);
                        }
                        else if (IKEAAuthenticationTypes.Certificates == templateSettings.DynamicKeysSettings.V2Settings.LocalAuthenticationType)
                        {
                            _adapter.Check("IPsec_LA_Certificates");
                            // Local supports only DistinguishedName type
                            // This needs to be selected for 'Identity' field to get loaded. (It expects a Click operation to load data)
                            _adapter.Click("IPsec_LA_Certificates_IdentityType");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Invalid Local Authentication Type: {0}".FormatWith(templateSettings.DynamicKeysSettings.V2Settings.LocalAuthenticationType));
                            return false;
                        }

                        // Remote Specific values
                        if (IKEAAuthenticationTypes.PreSharedKey == templateSettings.DynamicKeysSettings.V2Settings.RemoteAuthenticationType)
                        {
                            _adapter.Check("IPsec_Remote_PSK");
                            _adapter.SelectByValue("IPsec_Remote_PSK_IdentityType", Enum<IdentityType>.Value(templateSettings.DynamicKeysSettings.V2Settings.RemotePSKIdentityType));
                            _adapter.SetText("IPsec_Remote_PSK_Identity", templateSettings.DynamicKeysSettings.V2Settings.RemotePSKIdentity);
                            _adapter.Check("IPsec_Remote_ASCII");
                            _adapter.SetText("IPsec_Remote_PSK_Key", templateSettings.DynamicKeysSettings.V2Settings.RemotePSKKey);
                        }
                        else if (IKEAAuthenticationTypes.Certificates == templateSettings.DynamicKeysSettings.V2Settings.RemoteAuthenticationType)
                        {
                            _adapter.Check("IPsec_Remote_Certificates");
                            _adapter.SelectByValue("IPsec_Remote_Certificates_IdentityType", Enum<IdentityType>.Value(templateSettings.DynamicKeysSettings.V2Settings.RemoteCertIdentityType));
                            // Get the value from Local Certificate Identity and set the same for Remote Certificate Identity
                            _adapter.SetText("IPsec_Remote_Certificates_Identity", _adapter.GetValue("IPsec_LA_Certificates_IdentityType"));
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Invalid Remote Authentication Type: {0}".FormatWith(templateSettings.DynamicKeysSettings.V2Settings.RemoteAuthenticationType));
                            return false;
                        }

                        _adapter.Click("Identity_Authentication_Next");

                        // Custom Profile will have additional settings
                        if (IKESecurityStrengths.Custom == templateSettings.DynamicKeysSettings.Strengths)
                        {
                            ConfigureCustomSettings(templateSettings.DynamicKeysSettings.V2Settings.IKEv2Phase1Settings, templateSettings.DynamicKeysSettings.V2Settings.IKEv2Phase2Settings, IKEVersion.IKEv2);
                        }
                    }
                }
            }
            // Manual Keys
            else
            {
                _adapter.Check("Manual_Type");

                // Next button
                _adapter.Click("IPsec_Next");

                // If Encapsulation type is Tunnel, tunnel local and remote address need to be provided. Since ConfigureIPSecPhase function is generic for both Dynamic and Manual settings &
                // tunnel addresses is configured only when the IKEVersion is Version2. Configuring IKEVersion based on configuration requested.
                IKEVersion ikeVersion = templateSettings.ManualKeysSettings.BasicSettings.Encapsulation == EncapsulationType.Transport ? IKEVersion.IKEv1 : IKEVersion.IKEv2;

                ConfigureIPSecPhase2(templateSettings.ManualKeysSettings.BasicSettings.Encapsulation, templateSettings.ManualKeysSettings.BasicSettings.ESPEnable, templateSettings.ManualKeysSettings.BasicSettings.ESPEncryption, templateSettings.ManualKeysSettings.BasicSettings.ESPAuthentication,
                    templateSettings.ManualKeysSettings.BasicSettings.AHEnable, templateSettings.ManualKeysSettings.BasicSettings.AHAuthentication, ikeVersion, localAddress: templateSettings.ManualKeysSettings.BasicSettings.LocalAddress, remoteAddress: templateSettings.ManualKeysSettings.BasicSettings.RemoteAddress);

                // Phase 2 page next button
                _adapter.Click("Phase2_Next");

                ConfigureIPSecManualKeys(templateSettings.ManualKeysSettings);
            }

            // not applicable for INK. Used for selecting the template created in VEP
            if (!isModify && !isInk)
            {
                _adapter.Select("IPsec_List", templateSettings.Name);
            }

            TraceFactory.Logger.Info("Created IPsec template.");

            return true;
        }

        /// <summary>
        /// Configure IPsec manual key settings for IPsec template
        /// </summary>
        /// <param name="manualKeySettings"></param>
        private void ConfigureIPSecManualKeys(ManualKeysSettings manualKeySettings)
        {
            TraceFactory.Logger.Debug("Configuring Manual Settings.");

            // Format type are default: SPI - Decimal & Key - Hex
            _adapter.Click("SPI_Decimal");
            //_adapter.Click("Key_Hex");

            // Page settings are configured based on Basic settings done in previous page
            // SPI Format
            if (manualKeySettings.BasicSettings.ESPEnable)
            {
                _adapter.SetText("ESP_In", manualKeySettings.AdvancedSettings.ESPSPIIn.ToString());
                _adapter.SetText("ESP_Out", manualKeySettings.AdvancedSettings.ESPSPIOut.ToString());
            }
            if (manualKeySettings.BasicSettings.AHEnable)
            {
                _adapter.SetText("AH_In", manualKeySettings.AdvancedSettings.AHSPIIn.ToString());
                _adapter.SetText("AH_Out", manualKeySettings.AdvancedSettings.AHSPIOut.ToString());
            }

            // Key Format

            _adapter.Click("Key_ASCII");
            // Encryption value needs to configured only if the Encryption is selected in previous page
            if (manualKeySettings.BasicSettings.ESPEncryption.HasFlag(Encryptions.AES128) || manualKeySettings.BasicSettings.ESPEncryption.HasFlag(Encryptions.AES192) ||
               manualKeySettings.BasicSettings.ESPEncryption.HasFlag(Encryptions.AES256) || manualKeySettings.BasicSettings.ESPEncryption.HasFlag(Encryptions.DES) ||
               manualKeySettings.BasicSettings.ESPEncryption.HasFlag(Encryptions.DES3))
            {
                _adapter.SetText("Encryption_In", manualKeySettings.AdvancedSettings.EncryptionIn);
                _adapter.SetText("Encryption_Out", manualKeySettings.AdvancedSettings.EncryptionOut);
            }
            // Authentication can be configured for either ESP or AH
            if ((manualKeySettings.BasicSettings.ESPAuthentication.HasFlag(Authentications.AESXCBC) || manualKeySettings.BasicSettings.ESPAuthentication.HasFlag(Authentications.MD5) ||
                    manualKeySettings.BasicSettings.ESPAuthentication.HasFlag(Authentications.SHA1) || manualKeySettings.BasicSettings.ESPAuthentication.HasFlag(Authentications.SHA256) ||
                    manualKeySettings.BasicSettings.ESPAuthentication.HasFlag(Authentications.SHA384) || manualKeySettings.BasicSettings.ESPAuthentication.HasFlag(Authentications.SHA512)) ||
                    manualKeySettings.BasicSettings.AHAuthentication.HasFlag(Authentications.AESXCBC) || manualKeySettings.BasicSettings.AHAuthentication.HasFlag(Authentications.MD5) ||
                    manualKeySettings.BasicSettings.AHAuthentication.HasFlag(Authentications.SHA1) || manualKeySettings.BasicSettings.AHAuthentication.HasFlag(Authentications.SHA256) ||
                    manualKeySettings.BasicSettings.AHAuthentication.HasFlag(Authentications.SHA384) || manualKeySettings.BasicSettings.AHAuthentication.HasFlag(Authentications.SHA512))
            {
                _adapter.SetText("Authentication_In", manualKeySettings.AdvancedSettings.AuthenticationIn);
                _adapter.SetText("Authentication_Out", manualKeySettings.AdvancedSettings.AuthenticationOut);
            }

            //Next button
            _adapter.Click("Manual_Keys_next");

            TraceFactory.Logger.Info("Configured Manual Settings.");
        }

        /// <summary>
        /// Configure custom settings for IPsec template
        /// </summary>
        /// <param name="ikePhase1Settings"><see cref="IKEVersion "/></param>
        /// <param name="ikePhase2Settings"><see cref="IKEPhase2Settings "/></param>
        /// <param name="version"><see cref="IKEVersion "/></param>
        private void ConfigureCustomSettings(IKEPhase1Settings ikePhase1Settings, IKEPhase2Settings ikePhase2Settings, IKEVersion version = IKEVersion.IKEv1)
        {
            TraceFactory.Logger.Debug("Configuring Custom Settings.");

            // Phase 1
            ConfigureIPSecPhase1(ikePhase1Settings, version);

            // Phase 2
            ConfigureIPSecPhase2(ikePhase2Settings.Encapsulation, ikePhase2Settings.ESPEnable, ikePhase2Settings.ESPEncryption, ikePhase2Settings.ESPAuthentication, ikePhase2Settings.AHEnable,
                                 ikePhase2Settings.AHAuthentication, version, ikePhase2Settings.TunnelLocalAddress, ikePhase2Settings.TunnelRemoteAddress);

            // Below 2 values are extra in Phase 2 settings compared to Manual Key
            _adapter.SetText("Phase2_SALifeTime", ikePhase2Settings.SALifeTime.ToString());
            _adapter.SetText("Phase2_SASize", ikePhase2Settings.SASize.ToString());

            // Configure Advanced IKE options
            ConfigureAdvancedIKESettings(ikePhase2Settings.AdvancedIKESettings);

            _adapter.Click("Phase2_Next");

            TraceFactory.Logger.Info("Configured Custom Settings.");
        }

        /// <summary>
        /// Configure Advanced IKE settings
        /// </summary>
        /// <param name="advancedIKESettings"><see cref="AdvancedIKESettings"/></param>
        private void ConfigureAdvancedIKESettings(AdvancedIKESettings advancedIKESettings)
        {
            bool isInk = _adapter.Settings.ProductType == PrinterFamilies.InkJet;
            if (null == advancedIKESettings)
            {
                TraceFactory.Logger.Debug("No advance IKE settings requested to configure.");
                return;
            }

            TraceFactory.Logger.Debug("Configuring Advanced Settings.");

            // Advanced IKE Settings
            _adapter.Click("IKE_Advanced");

            if (advancedIKESettings.ReplayDetection)
            {
                _adapter.Check("Advanced_Relay");
            }
            else
            {
                _adapter.Uncheck("Advanced_Relay");
            }

            if (advancedIKESettings.KeyPFS)
            {
                _adapter.Check("Advanced_PFS");
            }
            else
            {
                _adapter.Uncheck("Advanced_PFS");
            }

            if (!isInk) // Not applicable for INK
            {
                // DiffieHellman groups selection
                _adapter.Click("DiffieHellman_Edit_IKE");

            }

            //For Ink,DH groups will be enabled only if Key PFS is checked and also the ids of DH groups are different
            if (isInk && advancedIKESettings.KeyPFS)
            {
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH1))
                {
                    _adapter.Check("Advanced_PFS_DH_1");
                }
                else
                {
                    _adapter.Uncheck("Advanced_PFS_DH_1");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH2))
                {
                    _adapter.Check("Advanced_PFS_DH_2");
                }
                else
                {
                    _adapter.Uncheck("Advanced_PFS_DH_2");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH5))
                {
                    _adapter.Check("Advanced_PFS_DH_5");
                }
                else
                {
                    _adapter.Uncheck("Advanced_PFS_DH_5");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH14))
                {
                    _adapter.Check("Advanced_PFS_DH_14");
                }
                else
                {
                    _adapter.Uncheck("Advanced_PFS_DH_14");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH15))
                {
                    _adapter.Check("Advanced_PFS_DH_15");
                }
                else
                {
                    _adapter.Uncheck("Advanced_PFS_DH_15");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH16))
                {
                    _adapter.Check("Advanced_PFS_DH_16");
                }
                else
                {
                    _adapter.Uncheck("Advanced_PFS_DH_16");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH17))
                {
                    _adapter.Check("Advanced_PFS_DH_17");
                }
                else
                {
                    _adapter.Uncheck("Advanced_PFS_DH_17");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH18))
                {
                    _adapter.Check("Advanced_PFS_DH_18");
                }
                else
                {
                    _adapter.Uncheck("Advanced_PFS_DH_18");
                }
            }
            if (!isInk)
            {
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH1))
                {
                    _adapter.Check("DH_1");
                }
                else
                {
                    _adapter.Uncheck("DH_1");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH2))
                {
                    _adapter.Check("DH_2");
                }
                else
                {
                    _adapter.Uncheck("DH_2");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH5))
                {
                    _adapter.Check("DH_5");
                }
                else
                {
                    _adapter.Uncheck("DH_5");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH14))
                {
                    _adapter.Check("DH_14");
                }
                else
                {
                    _adapter.Uncheck("DH_14");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH15))
                {
                    _adapter.Check("DH_15");
                }
                else
                {
                    _adapter.Uncheck("DH_15");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH16))
                {
                    _adapter.Check("DH_16");
                }
                else
                {
                    _adapter.Uncheck("DH_16");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH17))
                {
                    _adapter.Check("DH_17");
                }
                else
                {
                    _adapter.Uncheck("DH_17");
                }
                if (advancedIKESettings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH18))
                {
                    _adapter.Check("DH_18");
                }
                else
                {
                    _adapter.Uncheck("DH_18");
                }
            }


            if (!isInk)
            {
                _adapter.Click("DH_Ok");
                _adapter.Click("Advanced_Ok");
            }



            TraceFactory.Logger.Info("Configured Advanced Settings.");
        }

        /// <summary>
        /// Configure Phase 1 settings
        /// </summary>
        /// <param name="ikePhase1Settings"><see cref=" IKEPhase1Settings"/></param>
        /// <param name="version"><see cref=" IKEVersion"/></param>
        private void ConfigureIPSecPhase1(IKEPhase1Settings ikePhase1Settings, IKEVersion version)
        {
            bool isInk = _adapter.Settings.ProductType == PrinterFamilies.InkJet;
            TraceFactory.Logger.Debug("Configuring Phase1 Settings.");

            if (!isInk)     //This click is not applicable for InkJet
            {
                _adapter.Click("DiffieHellman_Edit");
            }


            if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH1))
            {
                _adapter.Check("DH_1");
            }
            else
            {
                _adapter.Uncheck("DH_1");
            }
            if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH2))
            {
                _adapter.Check("DH_2");
            }
            else
            {
                _adapter.Uncheck("DH_2");
            }
            if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH5))
            {
                _adapter.Check("DH_5");
            }
            else
            {
                _adapter.Uncheck("DH_5");
            }
            if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH14))
            {
                _adapter.Check("DH_14");
            }
            else
            {
                _adapter.Uncheck("DH_14");
            }
            if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH15))
            {
                _adapter.Check("DH_15");
            }
            else
            {
                _adapter.Uncheck("DH_15");
            }
            if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH16))
            {
                _adapter.Check("DH_16");
            }
            else
            {
                _adapter.Uncheck("DH_16");
            }
            if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH17))
            {
                _adapter.Check("DH_17");
            }
            else
            {
                _adapter.Uncheck("DH_17");
            }
            if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH18))
            {
                _adapter.Check("DH_18");
            }
            else
            {
                _adapter.Uncheck("DH_18");
            }

            if (IKEVersion.IKEv2 == version)
            {
                if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH19))
                {
                    _adapter.Check("DH_19");
                }
                else
                {
                    _adapter.Uncheck("DH_19");
                }
                if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH20))
                {
                    _adapter.Check("DH_20");
                }
                else
                {
                    _adapter.Uncheck("DH_20");
                }
                if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH21))
                {
                    _adapter.Check("DH_21");
                }
                else
                {
                    _adapter.Uncheck("DH_21");
                }
                if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH22))
                {
                    _adapter.Check("DH_22");
                }
                else
                {
                    _adapter.Uncheck("DH_22");
                }
                if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH23))
                {
                    _adapter.Check("DH_23");
                }
                else
                {
                    _adapter.Uncheck("DH_23");
                }
                if (ikePhase1Settings.DiffieHellmanGroup.HasFlag(DiffieHellmanGroups.DH24))
                {
                    _adapter.Check("DH_24");
                }
                else
                {
                    _adapter.Uncheck("DH_24");
                }
            }
            if (!isInk)     //This click is not applicable for InkJet
            {
                _adapter.Click("DH_Ok");
            }
            // Multiple Encryptions can be enabled
            if (ikePhase1Settings.Encryption.HasFlag(Encryptions.AES128))
            {
                _adapter.Check("Phase1_Encryption_AES128");
            }
            else
            {
                _adapter.Uncheck("Phase1_Encryption_AES128");
            }
            if (ikePhase1Settings.Encryption.HasFlag(Encryptions.AES192))
            {
                _adapter.Check("Phase1_Encryption_AES192");
            }
            else
            {
                _adapter.Uncheck("Phase1_Encryption_AES192");
            }
            if (ikePhase1Settings.Encryption.HasFlag(Encryptions.AES256))
            {
                _adapter.Check("Phase1_Encryption_AES256");
            }
            else
            {
                _adapter.Uncheck("Phase1_Encryption_AES256");
            }
            if (ikePhase1Settings.Encryption.HasFlag(Encryptions.DES))
            {
                _adapter.Check("Phase1_Encryption_DES");
            }
            else
            {
                _adapter.Uncheck("Phase1_Encryption_DES");
            }
            if (ikePhase1Settings.Encryption.HasFlag(Encryptions.DES3))
            {
                _adapter.Check("Phase1_Encryption_DES3");
            }
            else
            {
                _adapter.Uncheck("Phase1_Encryption_DES3");
            }

            // Multiple Authentications can be enabled
            if (ikePhase1Settings.Authentication.HasFlag(Authentications.MD5))
            {
                _adapter.Check("Phase1_Authentication_MD5");
            }
            else
            {
                _adapter.Uncheck("Phase1_Authentication_MD5");
            }
            if (ikePhase1Settings.Authentication.HasFlag(Authentications.SHA1))
            {
                _adapter.Check("Phase1_Authentication_SHA1");
            }
            else
            {
                _adapter.Uncheck("Phase1_Authentication_SHA1");
            }
            if (ikePhase1Settings.Authentication.HasFlag(Authentications.SHA256))
            {
                _adapter.Check("Phase1_Authentication_SHA256");
            }
            else
            {
                _adapter.Uncheck("Phase1_Authentication_SHA256");
            }
            if (ikePhase1Settings.Authentication.HasFlag(Authentications.SHA384))
            {
                _adapter.Check("Phase1_Authentication_SHA384");
            }
            else
            {
                _adapter.Uncheck("Phase1_Authentication_SHA384");
            }
            if (ikePhase1Settings.Authentication.HasFlag(Authentications.SHA512))
            {
                _adapter.Check("Phase1_Authentication_SHA512");
            }
            else
            {
                _adapter.Uncheck("Phase1_Authentication_SHA512");
            }

            if (IKEVersion.IKEv2 == version && !ikePhase1Settings.Authentication.HasFlag(Authentications.None))
            {
                if (ikePhase1Settings.Authentication.HasFlag(Authentications.AESXCBC))
                {
                    _adapter.Check("Phase1_Authentication_AESXCBC");
                }
                else
                {
                    _adapter.Uncheck("Phase1_Authentication_AESXCBC");
                }
            }

            _adapter.SetText("Phase1_SALifeTime", ikePhase1Settings.SALifeTime.ToString());
            _adapter.Click("Phase1_Next");

            TraceFactory.Logger.Info("Configured Phase1 Settings.");
        }

        /// <summary>
        /// Configure Phase 2 settings
        /// Note: This doesn't set Advanced settings and doesn't Click on 'Next' button
        /// This needs to be handled based on Dynamic/ Manual Phase 2 settings page
        /// </summary>
        /// <param name="encapsulation"><see cref="EncapsulationType"/></param>
        /// <param name="isESP">true is ESP field needs to be checked, false otherwise</param>
        /// <param name="espEncryption"><see cref="Encryptions"/></param>
        /// <param name="espAuthentication"><see cref="Authentications"/></param>
        /// <param name="isAH">true if AH field needs to be checked, false otherwise</param>
        /// <param name="ahAuthentication"><see cref="Authentications"/></param>
        /// <param name="version"><see cref="IKEVersion"/></param>
        /// <param name="localAddress">Tunnel Local Address</param>
        /// <param name="remoteAddress">Tunnel Remote Address</param>
        private void ConfigureIPSecPhase2(EncapsulationType encapsulation, bool isESP, Encryptions espEncryption, Authentications espAuthentication, bool isAH, Authentications ahAuthentication, IKEVersion version = IKEVersion.IKEv1, string localAddress = null, string remoteAddress = null)
        {
            TraceFactory.Logger.Debug("Configuring Phase2 Settings.");

            // Encapsulation Type
            if (EncapsulationType.Transport == encapsulation)
            {
                _adapter.Check("Encapsulation_Transport");
            }
            else
            {
                _adapter.Check("Encapsulation_Tunnel");
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.SetText("Tunnel_LocalAddress", localAddress);
                    _adapter.SetText("Tunnel_RemoteAddress", remoteAddress);
                }
                else
                {
                    if (IKEVersion.IKEv2 == version)
                    {
                        _adapter.SetText("Tunnel_LocalAddress", localAddress);
                        _adapter.SetText("Tunnel_RemoteAddress", remoteAddress);
                    }
                }
            }

            // Cryptographic Parameter: ESP
            if (isESP)
            {
                _adapter.Check("Cryptographic_ESP");

                // Multiple Encryptions can be enabled
                if (espEncryption.HasFlag(Encryptions.None))
                {
                    _adapter.Check("Phase2_Encryption_None");
                }
                else
                {
                    if (espEncryption.HasFlag(Encryptions.AES128))
                    {
                        _adapter.Check("Phase2_Encryption_AES128");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2_Encryption_AES128");
                    }
                    if (espEncryption.HasFlag(Encryptions.AES192))
                    {
                        _adapter.Check("Phase2_Encryption_AES192");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2_Encryption_AES192");
                    }
                    if (espEncryption.HasFlag(Encryptions.AES256))
                    {
                        _adapter.Check("Phase2_Encryption_AES256");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2_Encryption_AES256");
                    }
                    if (espEncryption.HasFlag(Encryptions.DES))
                    {
                        _adapter.Check("Phase2_Encryption_DES");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2_Encryption_DES");
                    }
                    if (espEncryption.HasFlag(Encryptions.DES3))
                    {
                        _adapter.Check("Phase2_Encryption_DES3");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2_Encryption_DES3");
                    }
                }

                // Multiple Authentications can be enabled
                if (espAuthentication.HasFlag(Authentications.None))
                {
                    _adapter.Check("Phase2_Authentication_None");
                }
                else
                {
                    if (espAuthentication.HasFlag(Authentications.MD5))
                    {
                        _adapter.Check("Phase2_Authentication_MD5");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2_Authentication_MD5");
                    }
                    if (espAuthentication.HasFlag(Authentications.SHA1))
                    {
                        _adapter.Check("Phase2_Authentication_SHA1");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2_Authentication_SHA1");
                    }
                    if (espAuthentication.HasFlag(Authentications.SHA256))
                    {
                        _adapter.Check("Phase2_Authentication_SHA256");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2_Authentication_SHA256");
                    }
                    if (espAuthentication.HasFlag(Authentications.SHA384))
                    {
                        _adapter.Check("Phase2_Authentication_SHA384");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2_Authentication_SHA384");
                    }
                    if (espAuthentication.HasFlag(Authentications.SHA512))
                    {
                        _adapter.Check("Phase2_Authentication_SHA512");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2_Authentication_SHA512");
                    }

                    if (IKEVersion.IKEv2 == version)
                    {
                        if (espAuthentication.HasFlag(Authentications.AESXCBC))
                        {
                            _adapter.Check("Phase2_Authentication_AESXCBC");
                        }
                        else
                        {
                            _adapter.Uncheck("Phase2_Authentication_AESXCBC");
                        }
                    }
                }
            }
            else
            {
                _adapter.Uncheck("Cryptographic_ESP");
            }

            // Cryptographic Parameter: AH
            if (isAH)
            {
                _adapter.Check("Cryptographic_AH");

                // Multiple Authentications can be enabled
                if (ahAuthentication.HasFlag(Authentications.MD5))
                {
                    _adapter.Check("Phase2AH_Authentication_MD5");
                }
                else
                {
                    _adapter.Uncheck("Phase2AH_Authentication_MD5");
                }
                if (ahAuthentication.HasFlag(Authentications.SHA1))
                {
                    _adapter.Check("Phase2AH_Authentication_SHA1");
                }
                else
                {
                    _adapter.Uncheck("Phase2AH_Authentication_SHA1");
                }
                if (ahAuthentication.HasFlag(Authentications.SHA256))
                {
                    _adapter.Check("Phase2AH_Authentication_SHA256");
                }
                else
                {
                    _adapter.Uncheck("Phase2AH_Authentication_SHA256");
                }
                if (ahAuthentication.HasFlag(Authentications.SHA384))
                {
                    _adapter.Check("Phase2AH_Authentication_SHA384");
                }
                else
                {
                    _adapter.Uncheck("Phase2AH_Authentication_SHA384");
                }
                if (ahAuthentication.HasFlag(Authentications.SHA512))
                {
                    _adapter.Check("Phase2AH_Authentication_SHA512");
                }
                else
                {
                    _adapter.Uncheck("Phase2AH_Authentication_SHA512");
                }

                if (IKEVersion.IKEv2 == version)
                {
                    if (ahAuthentication.HasFlag(Authentications.AESXCBC))
                    {
                        _adapter.Check("Phase2AH_Authentication_AESXCBC");
                    }
                    else
                    {
                        _adapter.Uncheck("Phase2AH_Authentication_AESXCBC");
                    }
                }
            }
            else
            {
                _adapter.Uncheck("Cryptographic_AH");
            }

            TraceFactory.Logger.Info("Configured Phase2 Settings.");
        }

        /// <summary>
        /// Configure Kerberos settings on printer web ui
        /// </summary>
        /// <param name="kerberosSettings"></param>
        /// <returns></returns>
        private bool ConfigureKerberos(KerberosSettings kerberosSettings)
        {
            // Manual and Import Configuration files options
            if (kerberosSettings.IsManual)
            {
                _adapter.Click("Kerberos_Manual");
                _adapter.Click("Kerberos_Next");
                _adapter.SetText("Kerberos_KDCServer", kerberosSettings.ManualSettings.KDCServer);
                _adapter.SetText("Kerberos_PrincipalRealm", kerberosSettings.ManualSettings.PrincipalRealm);
                _adapter.SetText("Kerberos_Password", kerberosSettings.ManualSettings.Password);

                if (!string.IsNullOrEmpty(kerberosSettings.ManualSettings.EncryptionType.ToString()))
                {
                    _adapter.SelectByValue("Kerberos_EncryptionType", Enum<KerberosEncryptionType>.Value(kerberosSettings.ManualSettings.EncryptionType));
                }
                if (kerberosSettings.ManualSettings.IterationCount != 0)
                {
                    _adapter.SetText("Kerberos_IterationCount", kerberosSettings.ManualSettings.IterationCount.ToString());
                }
                if (kerberosSettings.ManualSettings.KeyVersionNumber != 0)
                {
                    _adapter.SetText("Kerberos_KeyVersionNumber", kerberosSettings.ManualSettings.KeyVersionNumber.ToString());
                }
                if (kerberosSettings.ManualSettings.ClockSkew != 0)
                {
                    _adapter.SetText("Kerberos_ClockSkew", kerberosSettings.ManualSettings.ClockSkew.ToString());
                }
                if (kerberosSettings.ManualSettings.TimeSyncPeriod != 0)
                {
                    _adapter.SetText("Kerberos_TimeSyncPeriod", kerberosSettings.ManualSettings.TimeSyncPeriod.ToString());
                }
                if (!string.IsNullOrEmpty(kerberosSettings.ManualSettings.SNTPServer))
                {
                    _adapter.SetText("Kerberos_SNTPServer", kerberosSettings.ManualSettings.SNTPServer);
                }
                Thread.Sleep(TimeSpan.FromSeconds(40));                
                _adapter.Click("Kerberos_Manual_Next");                
            }
            else
            {
                _adapter.Click("Kerberos_Import");
                _adapter.SetBrowseControlText("Kerberos_ConfigFile", kerberosSettings.ImportSettings.ConfigurationFilePath);
                _adapter.SetBrowseControlText("Kerberos_KeytabFile", kerberosSettings.ImportSettings.KeyTabFilePath);

                // By default, a value (480) is specified on web ui
                if (0 != kerberosSettings.ImportSettings.TimeSyncPeriod)
                {
                    _adapter.SetText("Kerberos_Time", kerberosSettings.ImportSettings.TimeSyncPeriod.ToString());
                }

                if (!string.IsNullOrEmpty(kerberosSettings.ImportSettings.SNTPServer))
                {
                    _adapter.SetText("Kerberos_SNTP", kerberosSettings.ImportSettings.SNTPServer);
                }

                _adapter.Click("Kerberos_Next");
            }

            // Wait for configurations to take effect
            Thread.Sleep(TimeSpan.FromSeconds(50));

            // Validate if configuration was successful
            if (_adapter.IsElementPresent("Kerberos_View"))
            {
                _adapter.Click("Kerberos_View");
                _adapter.Click("Kerberos_Validate");
                if (SearchTextInPage("Success"))
                {
                    if (_adapter.IsElementPresent("ConfigResult_OK"))
                    {
                        _adapter.Click("ConfigResult_OK");
                    }
                    TraceFactory.Logger.Info("Kerberos configuration is successful.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Kerberos configuration is unsuccessful.");
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("Kerberos configuration is unsuccessful.");
                return false;
            }
        }

        /// <summary>
        /// Click on Finish/ Ok button for completing the IP Sec/ Firewall rule
        /// </summary>
        /// <returns>Always true</returns>
        private bool FinalizeRule()
        {
            TraceFactory.Logger.Debug("Finalizing rule.");

            try
            {
                if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
                {
                    // Click on Finish button
                    _adapter.Click("Final_Next");
                    // Click on OK button
                    //  _adapter.Click("Finalize_Rule");
                }
                else
                {
                    // Click on OK button
                    _adapter.Click("Finalize_Rule");
                }

                TraceFactory.Logger.Info("IPSec/Firewall rule created successfully.");
            }
            catch
            {
                StopAdapter();
                _adapter.Start("https");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Enable/ Disable IP Sec/ Firewall Rule
        /// </summary>
        /// <param name="pageKey">Sitemaps Page Key</param>
        /// <param name="tableKey">Sitemaps Table Key</param>
        /// <param name="applyButtonKey">Sitemaps Apply button Key</param>
        /// <param name="tableType"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        private bool SetIPSecFirewallRules(string pageKey, string tableKey, string applyButtonKey, FindType tableType = FindType.ById, bool enable = true)
        {
            try
            {
                TraceFactory.Logger.Debug("{0} IPSec/ Firewall Rules.".FormatWith(enable ? "Enabling" : "Disabling"));
                bool isInk = _adapter.Settings.ProductType == PrinterFamilies.InkJet;

                if (isInk)
                {
                    _adapter.Navigate("IPsec_Firewall", "https");
                    _adapter.Check("Enable_All_Rule");
                    _adapter.Click("IPSec_Apply");
                    TraceFactory.Logger.Info("Rules enabled");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
                else
                {
                    // Column no for check box differs between TPS & VEP
                    //int columnNo = PrinterFamilies.TPS.EqualsIgnoreCase(_adapter.Settings.ProductType) ? 0 : 1;
                    int columnNo = PrinterFamilies.TPS.Equals(_adapter.Settings.ProductType) ? 0 : 1;
                    int[] columnIndex = { columnNo };
                    int rulesCreated = 0;
                    Collection<string> rulesId = new Collection<string>();

                _adapter.Navigate(pageKey);
                Collection<Collection<string>> rulesTable = _adapter.GetTable(tableKey, includeHeader: false, columnIndex: columnIndex, elementType: FindType.ByName, returnValue: false);

                // Last 2 rows are obsolete
                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    rulesTable.RemoveAt(rulesTable.Count - 1);
                    //rulesTable.RemoveAt(rulesTable.Count - 1); Removing only one row from table as it is default row.)(TPS works)
                }

                foreach (Collection<string> tableRow in rulesTable)
                {
                    // 3rd column of the row specifies the Address template field. When a rule is created, this field will be updated
                    //if (!string.IsNullOrEmpty(tableRow[2].Trim())) //When the new rule is added it is table row 1. so changed here. (TPS)
                    if (!string.IsNullOrEmpty(tableRow[1].Trim()))
                    {
                        rulesCreated++;
                        rulesId.Add(tableRow[columnNo]);

                        if (enable)
                        {
                            _adapter.Check(tableRow[columnNo], false, FindType.ByName);
                        }
                        else
                        {
                            _adapter.Uncheck(tableRow[columnNo], false, FindType.ByName);
                        }
                    }
                }

                if (0 == rulesCreated)
                {
                    TraceFactory.Logger.Info("No rules were created to {0}.".FormatWith(enable == true ? "enable" : "disable"));
                    return false;
                }

                _adapter.Click(applyButtonKey);
                _adapter.ClickOkonAlert();

                Thread.Sleep(TimeSpan.FromSeconds(10));

                _adapter.Navigate(pageKey);

                int rulesEnabled = 0;

                foreach (string ruleId in rulesId)
                {
                    if (enable)
                    {
                        if (_adapter.IsChecked(ruleId, false, FindType.ByName))
                        {
                            rulesEnabled++;
                        }
                    }
                    else
                    {
                        if (!_adapter.IsChecked(ruleId, false, FindType.ByName))
                        {
                            rulesEnabled++;
                        }
                    }
                }

                    if (rulesCreated != rulesEnabled)
                    {
                        TraceFactory.Logger.Info("All created rules are not {0}.".FormatWith(enable ? "enabled" : "disabled"));
                        return false;
                    }
                }
            }
            catch
            {
                // Do nothing
            }
            finally
            {
                StopAdapter();
                _adapter.Start("https");
            }

            TraceFactory.Logger.Info("All created rules are {0}.".FormatWith(enable == true ? "enabled" : "disabled"));

            return true;
        }

        /// <summary>
        /// Check if requested IP Sec rule requires certificate to be installed and install both CA and ID if required
        /// </summary>
        /// <param name="settings"><see cref="SecurityRuleSettings"/></param>
        /// <returns>true if both CA and ID certificates are installed successfully/ certificates installation is not required, false if any 1 certificate installation goes bad</returns>
        public bool CheckCertificateInstallation(SecurityRuleSettings settings)
        {
            // CA and ID Certificate are required only for configuration selecting 'Certicates' as Authentication option
            if (IPsecFirewallAction.ProtectedWithIPsec != settings.Action || SecurityKeyType.Manual == settings.IPsecTemplate.KeyType ||
                (settings.IPsecTemplate.DynamicKeysSettings.V1Settings != null && IKEAAuthenticationTypes.Certificates != settings.IPsecTemplate.DynamicKeysSettings.V1Settings.AuthenticationTypes) ||
                (settings.IPsecTemplate.DynamicKeysSettings.V2Settings != null && IKEAAuthenticationTypes.Certificates != settings.IPsecTemplate.DynamicKeysSettings.V2Settings.LocalAuthenticationType))
            {
                return true;
            }

            TraceFactory.Logger.Info("Installing certificates before IP sec rule creation.");

            string caCertificatePath = string.Empty;
            string idCertificatePath = string.Empty;
            string idPassword = string.Empty;

            if (settings.IPsecTemplate.DynamicKeysSettings.V1Settings != null && IKEAAuthenticationTypes.Certificates == settings.IPsecTemplate.DynamicKeysSettings.V1Settings.AuthenticationTypes)
            {
                caCertificatePath = settings.IPsecTemplate.DynamicKeysSettings.V1Settings.CACertificatePath;
                idCertificatePath = settings.IPsecTemplate.DynamicKeysSettings.V1Settings.IDCertificatePath;
                idPassword = settings.IPsecTemplate.DynamicKeysSettings.V1Settings.IDCertificatePassword;
            }
            else if (settings.IPsecTemplate.DynamicKeysSettings.V2Settings != null && IKEAAuthenticationTypes.Certificates == settings.IPsecTemplate.DynamicKeysSettings.V2Settings.LocalAuthenticationType)
            {
                caCertificatePath = settings.IPsecTemplate.DynamicKeysSettings.V2Settings.LocalCACertificatePath;
                idCertificatePath = settings.IPsecTemplate.DynamicKeysSettings.V2Settings.LocalIDCertificatePath;
                idPassword = settings.IPsecTemplate.DynamicKeysSettings.V2Settings.LocalIDCertificatePassword;
            }

            if (string.IsNullOrEmpty(caCertificatePath) || string.IsNullOrEmpty(idCertificatePath) || string.IsNullOrEmpty(idPassword))
            {
                TraceFactory.Logger.Info("Invalid certificate details.");
                return false;
            }

            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                StopAdapter();
                _adapter.Start("https");
            }

            return InstallCACertificate(caCertificatePath) && InstallIDCertificate(idCertificatePath, idPassword);
        }

        /// <summary>
        /// Select/Check requested services from Services table
        /// </summary>
        /// <param name="manageServices">Collection of <see cref=" Service"/></param>
        private void SelectMangeServices(Collection<Service> manageServices, bool includeHeader = false, bool manageService = false)
        {
            // Column 0 has the check box control id
            int[] checkBoxColumnIndex = new int[] { 0 };

            Collection<Collection<string>> serviceTable = _adapter.GetTable("Service_Manage_Service_Table", includeHeader: includeHeader, columnIndex: checkBoxColumnIndex, elementType: FindType.ByName, returnValue: false);
            if (!(PrinterFamilies.InkJet == _adapter.Settings.ProductType))
            {
                if (PrinterFamilies.VEP == _adapter.Settings.ProductType || PrinterFamilies.LFP == _adapter.Settings.ProductType)
                {
                    serviceTable.RemoveAt(0);
                    serviceTable.RemoveAt(0);
                    //This is used to check if custom mange service is used
                    if (manageService == false)
                    {
                        serviceTable.RemoveAt(0);
                    }

                }
                else
                {
                    serviceTable.RemoveAt(0);
                    serviceTable.RemoveAt(0);
                }
            }
            else
            {
                serviceTable.RemoveAt(0);
            }
            int tableColumnCount = serviceTable[0].Count;
            int nameColumn, protocolColumn, serviceColumn, printerportColumn, remoteportColumn;
            bool isInk = _adapter.Settings.ProductType == PrinterFamilies.InkJet;

            // Table headers are embedded under 'tbody' and is the first row retrieved.
            nameColumn = serviceTable[0].IndexOf(serviceTable[0].Where(x => x.StartsWith("Name", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            protocolColumn = serviceTable[0].IndexOf(serviceTable[0].Where(x => x.StartsWith("Protocol", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            serviceColumn = serviceTable[0].IndexOf(serviceTable[0].Where(x => x.StartsWith("Service", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            printerportColumn = serviceTable[0].IndexOf(serviceTable[0].Where(x => x.StartsWith("Printer", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            remoteportColumn = serviceTable[0].IndexOf(serviceTable[0].Where(x => x.StartsWith("Remote", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());

            // Header data is no more required and is removed to reduce looping time during selection of service
            //serviceTable.RemoveAt(0);

            string printerPort, remotePort;
            printerPort = remotePort = string.Empty;
            TraceFactory.Logger.Debug("Reading the Table and Selecting the Custom service.");
            // For all service requested, check each row of service table and select the service
            foreach (Service service in manageServices)
            {
                int radioIndex = 1;
                foreach (Collection<string> tableRow in serviceTable)
                {

                    // This condition will eliminate all invalid rows
                    if (tableRow.Count == tableColumnCount)
                    {
                        // Input provided for:
                        // Any port will be 'null', data acquired from table will have 'Any'
                        // Specified port will be 'xxx|yyy', data acquired from table will have 'xxx - yyy'
                        printerPort = service.PrinterPort == string.Empty ? "Any" : service.PrinterPort.Replace("|", " - ");
                        remotePort = service.RemotePort == string.Empty ? "Any" : service.RemotePort.Replace("|", " - ");
                        //Hardcoding these values for colume check once again 
                        nameColumn = 1;
                        protocolColumn = 2;
                        serviceColumn = 3;
                        printerportColumn = 4;
                        remoteportColumn = 5;

                        // All columns except ServiceType are compared with requested data and service table data
                        // ServiceType will show Printer/MFP and Remote as the data under service table. Hence only comparing whether the data 'StartsWith' Printer or Remote

                        if (service.Name.EqualsIgnoreCase(tableRow[nameColumn]) && service.Protocol.ToString().EqualsIgnoreCase(tableRow[protocolColumn])
                            && tableRow[serviceColumn].StartsWith(service.ServiceType.ToString()) && printerPort.EqualsIgnoreCase(tableRow[printerportColumn]) && remotePort.EqualsIgnoreCase(tableRow[remoteportColumn]))
                        {
                            // In Inkjet ,all checkbox are identified by single name. Required check box is identified by using web elements
                            if (isInk)
                            {
                                IList<IWebElement> elements = _adapter.GetPageElements(tableRow[0], false, FindType.ByName);
                                if (service.IsDefault)
                                {
                                    elements[radioIndex].Click();
                                }
                                break;
                            }
                            else
                            {
                                _adapter.Check(tableRow[0], false, FindType.ByName);
                                TraceFactory.Logger.Debug("Expected Service found and checked in the Table");
                                break;
                            }
                        }

                        radioIndex++;

                    }
                }
            }


            _adapter.Click("Custom_Service_Ok");

            if ((PrinterFamilies.VEP == _adapter.Settings.ProductType) || (PrinterFamilies.LFP == _adapter.Settings.ProductType))
            {
                _adapter.Click("MngSrv_Ok");
            }
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Return text value on printer web page for Authentication type
        /// </summary>
        /// <param name="authenticationType"><see cref="IKEAAuthenticationTypes"/></param>
        /// <returns></returns>
        private string GetAuthenticationTypeText(IKEAAuthenticationTypes authenticationType)
        {
            if (IKEAAuthenticationTypes.PreSharedKey == authenticationType)
            {
                return "Pre-Shared";
            }
            else if (IKEAAuthenticationTypes.Certificates == authenticationType)
            {
                return "Certificates";
            }
            else if (IKEAAuthenticationTypes.Kerberos == authenticationType)
            {
                return "Kerberos";
            }

            return string.Empty;
        }

        /// <summary>
        /// Click on 'Next' button 
        /// </summary>
        /// <param name="nextButtonControlId">Button Control Id</param>
        private void NavigateToNextPage(string nextButtonControlId)
        {
            //bool isTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(_adapter.Settings.ProductType);

            if (!(PrinterFamilies.TPS == _adapter.Settings.ProductType))
            {
                // Click on Next button
                _adapter.Click(nextButtonControlId);

            }
        }

        /// <summary>
        /// Delete Custom Address, Service and IPsec templates
        /// </summary>
        private void DeleteAllCustomRules()
        {
            bool isInk = _adapter.Settings.ProductType == PrinterFamilies.InkJet;

            // Deleting all custom rule differs for Inkjet and other printers. Customs rules are deleted from AddressTemplate,ServiceTemplate and ServiceList page 
            if (isInk)
            {
                // Delete all custom address templates. If no custom templates are there Delete button will be inactive and it will throw exception
                try
                {
                    _adapter.Navigate("Firewall_Address_Template", "https");
                    _adapter.Click("Select_All");
                    _adapter.Click("Delete");
                    _adapter.Click("Delete_Confirmation");
                    _adapter.Click("Delete_Ok");
                }
                catch
                {
                    // Do nothing
                }

                // Delete All custom service  templates. If no custom templates are there Delete button will be inactive and it will throw exception  
                try
                {
                    _adapter.Navigate("Firewall_Service_Template", "https");
                    _adapter.Click("Select_All");
                    _adapter.Click("Delete");
                    _adapter.Click("Delete_Confirmation");
                    _adapter.Click("Delete_Ok");
                }
                catch
                {
                    // Do nothing
                }

                // Delete All custom services from ServiceList. If no custom templates are there Delete button will be inactive and it will throw exception
                try
                {
                    _adapter.Navigate("Firewall_Service_List", "https");
                    _adapter.Click("Select_All");
                    _adapter.Click("Delete");
                    _adapter.Click("Delete_Confirmation");
                    _adapter.Click("Delete_Ok");
                }
                catch
                {
                    // Do nothing
                }
                // Delete All IPSec Templates. If no custom templates are there Delete button will be inactive and it will throw exception
                try
                {
                    StopAdapter();
                    _adapter.Start("https");
                    _adapter.Navigate("IPSec_Template", "https");
                    _adapter.Click("Select_All");
                    _adapter.Click("Delete");
                    _adapter.Click("Delete_Confirmation");
                    _adapter.Click("Delete_Ok");
                }
                catch
                {
                    // Do nothing
                }
            }
            else
            {
                // Get Values for all Default Address templates
                Collection<string> defaultAddresses = new Collection<string>();
                string[] addressValues = Enum.GetNames(typeof(DefaultAddressTemplates));

                PrinterFamilies family = _adapter.Settings.ProductType;

                foreach (string defaultAddress in addressValues)
                {
                    // Avoid Custom address option from enum
                    if (!DefaultAddressTemplates.Custom.ToString().EqualsIgnoreCase(defaultAddress))
                    {
                        defaultAddresses.Add(CtcUtility.GetEnumvalue(Enum<DefaultAddressTemplates>.Value(defaultAddress), family));
                    }
                }

                // Get Values for all default Service templates
                Collection<string> defaultServices = new Collection<string>();
                string[] servicesValues = Enum.GetNames(typeof(DefaultServiceTemplates));

                foreach (string defaultService in servicesValues)
                {
                    if (!DefaultServiceTemplates.Custom.ToString().EqualsIgnoreCase(defaultService))
                    {
                        defaultServices.Add(CtcUtility.GetEnumvalue(Enum<DefaultServiceTemplates>.Value(defaultService), family));
                    }
                }

                // Navigate to firewall page to delete all custom templates if any
                _adapter.Navigate("IPsec_Firewall", "https");
                _adapter.Click("Add_rule");

                // Added delay to address the issue of popping up the failed to delete rule
                Thread.Sleep(TimeSpan.FromMinutes(1));

                // Delete all custom address templates
                DeleteCustomTemplate("Address_List", defaultAddresses);
                // After custom template is deleted, any default address needs to be selected to move forward. Selecting 'All Addresses'
                _adapter.Select("Address_List", defaultAddresses[0]);
                _adapter.Click("Address_Next");

                //Delete All custom service  templates
                DeleteCustomTemplate("Service_List", defaultServices);

                //Delete Custom Manage services
                _adapter.Click("Service_New");
                _adapter.Click("Service_Manage_Service");
                _adapter.Click("Service_Manage_Custom");

                // GetListItems will throw up exception when there are no elements availbale in the table.
                try
                {
                    List<string> listItems = _adapter.GetListItems("Custom_Service_Table");

                    foreach (string item in listItems)
                    {
                        _adapter.Select("Custom_Service_Table", item);
                        _adapter.Click("Custom_Service_Delete");
                    }
                }
                catch
                {
                    // Do nothing
                }
                finally
                {
                    // Click 'Cancel' button 3 times to go back to Service main page
                    GotoMainIPsecPage();
                }

                // Selecting 'All Services' to move forward
                _adapter.Select("Service_List", defaultServices[0]);
                _adapter.Click("Service_Next");

                _adapter.Check("Traffic_IPsec");
                _adapter.Click("Traffic_Next");

                // When there are no templates created for ipsec, GetListItems will throw up exception; ignoring ipsec template deletion in this scenario and going back to main ipsec page
                try
                {
                    // Get all IPsec rules created
                    List<string> listItems = _adapter.GetListItems("IPsec_List");
                    Collection<string> ipsecTemplate = new Collection<string>();

                    foreach (string item in listItems)
                    {
                        ipsecTemplate.Add(item);
                    }

                    DeleteCustomTemplate("IPsec_List", ipsecTemplate, isIPsecTemplate: true);
                }
                catch
                {
                    // Do nothing
                }
                finally
                {
                    GotoMainIPsecPage();
                }

                // Go back to main IPsec page by clicking Cancel button
                if (_adapter.IsElementPresent("Page_Cancel"))
                {
                    _adapter.Click("Page_Cancel");
                }
            }
        }

        /// <summary>
        /// Select specified custom template from list box and delete
        /// </summary>
        /// <param name="listBoxControlId">List box control id</param>
        /// <param name="defaultValues">Default addresses/ services list. For IPsec, this list will be populated with rules to delete</param>
        /// <param name="isIPsecTemplate">true if deletion is for ipsec template, false for Address/Service template</param>
        private void DeleteCustomTemplate(string listBoxControlId, Collection<string> defaultValues, bool isIPsecTemplate = false)
        {
            // All IPsec template will be custom templates, deleting all rules created
            if (isIPsecTemplate)
            {
                foreach (string ipsecTemplate in defaultValues)
                {
                    DeleteListItem(listBoxControlId, ipsecTemplate);
                }
            }
            else
            {
                // Added delay to address the issue of popping up the failed to delete rule
                Thread.Sleep(TimeSpan.FromSeconds(30));

                // Get all items available in list box
                List<string> listBoxItems = _adapter.GetListItems(listBoxControlId);

                // For each of the item, verify if item is default and delete if custom
                foreach (string listItem in listBoxItems)
                {
                    bool isCustom = true;

                    foreach (string defaultValue in defaultValues)
                    {
                        if (defaultValue.EqualsIgnoreCase(listItem))
                        {
                            isCustom = false;
                            break;
                        }
                    }

                    if (isCustom)
                    {
                        DeleteListItem(listBoxControlId, listItem);
                    }
                }
            }
        }

        /// <summary>
        /// Click on the list box item and Delete the template
        /// </summary>
        /// <param name="listBoxControlId">List box control id</param>
        /// <param name="listItem">List item</param>
        private void DeleteListItem(string listBoxControlId, string listItem)
        {
            _adapter.Select(listBoxControlId, listItem);
            _adapter.Click("Delete_Template");
            _adapter.Click("Delete_Confirmation");
        }

        /// <summary>
        /// Click on 'Cancel' button to goto main ipsec page. Maximum of 3 cancel buttons will be clicked if it is available
        /// Note: Please check the usage if modifying the below implementation
        /// </summary>
        public void GotoMainIPsecPage()
        {
            if (_adapter.IsElementPresent("Page_Cancel"))
            {
                _adapter.Click("Page_Cancel");

                if (_adapter.IsElementPresent("Page_Cancel"))
                {
                    _adapter.Click("Page_Cancel");

                    if (_adapter.IsElementPresent("Page_Cancel"))
                    {
                        _adapter.Click("Page_Cancel");
                    }
                }
            }
        }

        /// <summary>
        /// Clears the kerberos configuration
        /// Note: Please check the usage if modifying the below implementation
        /// </summary>
        public void ClearKerberosIPsecConfiguration()
        {
            _adapter.Navigate("IPsec_Firewall");
            _adapter.Click("Add_rule");
            NavigateToNextPage("Address_Next");
            NavigateToNextPage("Service_Next");
            _adapter.Check("Traffic_IPsec");
            _adapter.Click("Traffic_Next");
            _adapter.Click("IPsec_New");
            _adapter.SetText("IPsec_Name", "test");
            _adapter.SelectByValue("IKE_Version", Enum<IKEVersion>.Value(IKEVersion.IKEv1));
            _adapter.Click("IPsec_Next");
            if (_adapter.IsElementPresent("Kerberos_Clear", sourceType: FindType.ByName))
            {
                _adapter.Click("Kerberos_Clear", sourceType: FindType.ByName);
                TraceFactory.Logger.Info("Cleared the Kerberos configuration");
            }
            else
            {
                TraceFactory.Logger.Info("Failed to clear the Kerberos configuration");
            }
        }

        /// <summary>
        /// Sets the password complexity for Omni/Opus.
        /// </summary>
        /// <param name="status">Enable or disable password complexity</param>
        /// <param name="length">Length of the password</param>
        /// <param name="password">Password if it is set.</param>
        /// <param name="isPasswordSet">Flag to indicate if the password is set or not.</param>
        /// <returns></returns>
        public bool SetPasswordComplexity(bool status, int length, bool isPasswordSet = false, string password = "")
        {
            TraceFactory.Logger.Info(SET_DEBUG_LOG.FormatWith("Password Complexity", status));

            // If password is set, login is required to access the 'Account Policy' page
            if (isPasswordSet)
            {
                Instance().Login(password);
            }

            _adapter.Navigate("Account_Policy");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (status)
            {
                TraceFactory.Logger.Info("Enabling Password Complexity");
                _adapter.Check("PasswordComplexity");
            }
            else
            {
                TraceFactory.Logger.Info("Disabling Password Complexity");
                _adapter.Uncheck("PasswordComplexity");
            }

            //Set the minimum password length
            _adapter.SetText("MinimumPasswordLenght", length.ToString());
            _adapter.Click("AP_Apply");

            if (status)
            {
                if (_adapter.IsChecked("PasswordComplexity") && _adapter.GetText("MinimumPasswordLenght").EqualsIgnoreCase(length.ToString()))
                {
                    TraceFactory.Logger.Info(SUCCESS_LOG.FormatWith("Password Complexity", status));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("Password Complexity", status));
                    return false;
                }
            }
            else
            {
                if (!_adapter.IsChecked("PasswordComplexity"))
                {
                    TraceFactory.Logger.Info(SUCCESS_LOG.FormatWith("Password Complexity", status));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("Password Complexity", status));
                    return false;
                }
            }
        }

        #endregion Private Methods
    }
}