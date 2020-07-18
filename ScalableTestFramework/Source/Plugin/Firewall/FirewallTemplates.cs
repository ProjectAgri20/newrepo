using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.Utility;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.Firewall
{
    /// <summary>
    /// Firewall Templates
    /// </summary>
    internal static class FirewallTemplates
    {
        private const string SERVICE_TEMPLATE_NAME = "ServiceTemplate-";

        #region Private Enum

        /// <summary>
        /// List of services
        /// </summary>
        private enum Services
        {
            None = 1,
            Print_9100 = 2,
            Print_LPD = 4,
            HTTP = 8,
            HTTPS = 16,
            SNMP = 32,
            Telnet = 64,
            All = Print_9100 | Print_LPD | HTTP | SNMP | Telnet
        }

        #endregion enum

        #region Firewall Templates

        #region All IP Address

        /// <summary>
        /// Template for All IPAddress and All Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPAddress_AllService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation

                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false, isMessageBoxChecked: activityData.Debug);
                    if (activityData.Debug)
                    {
                        MessageBox.Show("User has enabled Debug option.\nAfter verification of Firewall results please press OK to continue with execution. ");
                    }
                }

                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    if (activityData.Debug)
                    {
                        MessageBox.Show("User has enabled Debug option. \n After verification of Firewall results please press OK to continue with execution. ");
                    }
                }

                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    if (activityData.Debug)
                    {
                        MessageBox.Show("User has enabled Debug option. \n After verification of Firewall results please press OK to continue with execution. ");
                    }
                }


                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    if (activityData.Debug)
                    {
                        MessageBox.Show("User has enabled Debug option. \n After verification of Firewall results please press OK to continue with execution. ");
                    }
                }


                // Drop Rule, Default: Allow
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isMessageBoxChecked: activityData.Debug, isPingRequiredP9100Install: false);

                    if (activityData.Debug)
                    {
                        MessageBox.Show("User has enabled Debug option.\nAfter verification of Firewall results please press OK to continue with execution. ");
                    }
                }

                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    if (activityData.Debug)
                    {
                        MessageBox.Show("User has enabled Debug option.\nAfter verification of Firewall results please press OK to continue with execution. ");
                    }
                }

                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

                    if (activityData.Debug)
                    {
                        MessageBox.Show("User has enabled Debug option.\nAfter verification of Firewall results please press OK to continue with execution. ");
                    }
                }

                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    if (activityData.Debug)
                    {
                        MessageBox.Show("User has enabled Debug option.\nAfter verification of Firewall results please press OK to continue with execution. ");
                    }
                }

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

		/// <summary>
		/// Template for All IPAddress and All Print Service
		/// </summary>
		/// <param name="activityData"></param>
		/// <returns></returns>
		public static bool Template_AllIPAddress_AllPrintService(FirewallActivityData activityData)
		{
			try
			{
				bool result = true;
				BitArray resultArray = new BitArray(8, true);
				PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
				Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
				bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
                bool IsLFP = PrinterFamilies.LFP.ToString().EqualsIgnoreCase(family.ToString());
				  bool IsTPS = activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString());
                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllPrintServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);

                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                if (activityData.IPv4Enable)
                {
                    // Result validation
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));

                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    // TODO: Removing Validation for P9100 since it has some issue while printing.Discuss with Manual Team
                }

                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                            p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                , isPingRequiredP9100Install: false);
                }

                if (activityData.Stateless)
                {
                    // TODO: P9100 Validation is removed since it is not working even manually also for some network setup  issue, need to validate and add
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                 p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                , isPingRequiredP9100Install: false);
                }

                if (activityData.Stateful)
                {
                    // TODO: P9100 Validation is removed since it is not working even manually also for some network setup  issue, need to validate and add
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                , isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow



                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Add custom rule for WS print:Allow for WS discovery to work");
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;
                CreateWSPrintServiceRule(activityData, addressTemplate, securitySettings);

                securitySettings.Action = IPsecFirewallAction.DropTraffic;
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllPrintServices;
                securitySettings.ServiceTemplate = serviceTemplate;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                if (activityData.IPv4Enable)
                {
                    // Result validation
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, telnet: IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail,
                                                                lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

                }

                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail,
                                                                lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail,
                                                                lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPAddress and All Management Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPAddress_AllManagementService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
				bool IsTPS = activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString());
                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");

                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllManagementServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);

                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);


                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, telnet: IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }


                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                EwsWrapper.Instance().DeleteAllRules();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllManagementServices;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.DropTraffic;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, telnet: IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                if (activityData.LinkLocal)
                {
                    // TODO: Removing Validation for P9100 since it has some issue while printing.Manual Team will come back on this
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                 p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                // TODO: Removing Validation for P9100 since it has some issue while printing.Manual Team will come back on this

                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                // TODO: Removing Validation for P9100 since it has some issue while printing.Manual Team will come back on this

                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }


                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPAddress and All Discovery Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPAddress_AllWSDiscoveryService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
				 bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());
                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllDiscoveryServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                CreateWSPrintServiceRule(activityData, addressTemplate, securitySettings);

                
			   // CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				// Result validation
				if (activityData.IPv4Enable)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
					resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, telnet: IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
																https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass,
																printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}
				if (activityData.LinkLocal)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
					resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}

				if (activityData.Stateless)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
					resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}

				if (activityData.Stateless)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
					resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}

                // Drop Rule, Default: Allow
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllDiscoveryServices;
                securitySettings.ServiceTemplate = serviceTemplate;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				// Result validation
				if (activityData.IPv4Enable)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
					resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
																https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
																printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}
				if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
				{
					if (activityData.LinkLocal)
					{
						TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
						resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
																	p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
																	, isPingRequiredP9100Install: false);
					}

                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                    , isPingRequiredP9100Install: false);
                    }

                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail , snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                    , isPingRequiredP9100Install: false);
                    }
                }

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPAddress and All Microsoft Web Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPAddress_AllMicrosoftWebService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
				bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());
                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllWebServicesPrint;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //	CreateSNMPServiceRule(addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                , isPingRequiredP9100Install: false);
                }

                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                , isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                , isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllWebServicesPrint;
                securitySettings.ServiceTemplate = serviceTemplate;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPAddress and All Service to test maximun rule creation
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPAddress_AllService_MaximumRule(FirewallActivityData activityData)
        {
            try
            {
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

				try
				{
					if (PrinterFamilies.VEP.ToString().EqualsIgnoreCase(activityData.ProductFamily) ||
                        PrinterFamilies.LFP.ToString().EqualsIgnoreCase(activityData.ProductFamily))
					{
						// Creating Maximum number of Rules
						for (int i = 0; i <= 5; i++)
						{
							EwsWrapper.Instance().CreateRule(securitySettings, true, true);
						}
					}
					else
					{
						for (int i = 0; i <= 10; i++)
						{
							EwsWrapper.Instance().CreateRule(securitySettings, true, true);
						}
					}

                }
                catch (Exception maximumRuleException)
                {
                    TraceFactory.Logger.Debug(maximumRuleException.Message);
                }

				if ((PrinterFamilies.VEP.ToString().EqualsIgnoreCase(activityData.ProductFamily)) ||
                    (PrinterFamilies.LFP.ToString().EqualsIgnoreCase(activityData.ProductFamily)))
				{
					if (!EwsWrapper.Instance().SearchTextInPage("The maximum number of rules have been added"))
					{
						return false;
					}
                    else
                    {
                        return true;
                    }
				}
				else if ((PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily)))
				{
					if (EwsWrapper.Instance().CheckMaxRules())
					{
						return true;
					}
                    else
                    {
                        return false;
                    }					
				}
                return false;				
			}
			finally
			{
				EwsWrapper.Instance().DeleteAllRules();
			}
		}

        #endregion All IP Address

        #region All IPv4 Address

        /// <summary>
        /// Template for All IPv4Address and All Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv4Address_AllService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");

                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv4Addresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                               https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                               https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                               https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                               https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                               https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                               https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv4Address and All Print Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv4Address_AllPrintService(FirewallActivityData activityData, DefaultAddressTemplates defaultAddressTemplate)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
				 bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());
                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv4Addresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllPrintServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);

                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);


                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Pass, telnet: IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                , isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                , isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                , isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow
                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Add custom rule for WS print:Allow for WS discovery to work");
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;
                CreateWSPrintServiceRule(activityData, addressTemplate, securitySettings);
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllPrintServices;
                securitySettings.ServiceTemplate = serviceTemplate;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, telnet: IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Pass,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass,
                                                                lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass,
                                                                lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass,
                                                                lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

		/// <summary>
		/// Template for All IPv4Address and All Management Service
		/// </summary>
		/// <param name="activityData"></param>
		/// <returns></returns>
		public static bool Template_AllIPv4Address_AllManagementService(FirewallActivityData activityData, DefaultAddressTemplates defaultAddressTemplate)
		{
			try
			{
				bool result = true;
				BitArray resultArray = new BitArray(8, true);
				PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
				Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
				bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
                bool IsLFP = PrinterFamilies.LFP.ToString().EqualsIgnoreCase(family.ToString());
				bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());
                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv4Addresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllManagementServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                // CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);
                securitySettings.ServiceTemplate = serviceTemplate;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, IsTPS? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow
                // Creating 1st SNMP custom rule ->Allow and then All IPv4 + All Managment ->DROP , Default -> Allow
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                EwsWrapper.Instance().DeleteAllRules();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllManagementServices;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);


                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.DropTraffic;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				// Result validation
				if (activityData.IPv4Enable)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
					resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Pass, http: DeviceServiceState.Fail,
													https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
													printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Pass :DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv4Address and All Discovery Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv4Address_AllDiscoveryService(FirewallActivityData activityData, DefaultAddressTemplates defaultAddressTemplate)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
			 	bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv4Addresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllDiscoveryServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                TraceFactory.Logger.Info("Add custom rule for WS print:Allow for WS discovery to work");
                CreateWSPrintServiceRule(activityData, addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				// Result validation
				TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
				resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
																https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass,
																printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
				resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass, 
																p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
				resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
				resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);                

                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;
                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllDiscoveryServices;
                securitySettings.ServiceTemplate = serviceTemplate;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				// Result validation
				if (activityData.IPv4Enable)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
					resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
																https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
																printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

                }
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                    isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                        isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                        isPingRequiredP9100Install: false);
                    }
                }

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv4Address and All Microsoft Web Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv4Address_AllMicrosoftWebService(FirewallActivityData activityData, DefaultAddressTemplates defaultAddressTemplate)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv4Addresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllWebServicesPrint;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("Validating Services for IPv4 Address {0}".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, DeviceServiceState.Fail, DeviceServiceState.Pass, http: DeviceServiceState.Fail,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("Validating Services for IPv6 Link Local Address {0}".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("Validating Services for IPv6 Stateless Address {0}".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("Validating Services for IPv6 Stateful Address {0}".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow
                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                securitySettings.Action = IPsecFirewallAction.DropTraffic;
                securitySettings.ServiceTemplate = serviceTemplate;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass,
                                                                    lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass,
                                                                    lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass,
                                                                    lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();

            }
        }

        #endregion All IPv4 Address

        #region All IPv6 Address

        /// <summary>
        /// Template for All IPv6Address and All Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6Address_AllService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6Addresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;
                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

		/// <summary>
		/// Template for All IPv6Address and All Print Service
		/// </summary>
		/// <param name="activityData"></param>
		/// <returns></returns>
		public static bool Template_AllIPv6Address_AllPrintService(FirewallActivityData activityData)
		{
			try
			{
				bool result = true;
				BitArray resultArray = new BitArray(8, true);
				PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
				Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
				bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
                bool IsLFP = PrinterFamilies.LFP.ToString().EqualsIgnoreCase(family.ToString());
				  bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());
                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6Addresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllPrintServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                         isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    // TODO: P9100 Validation is removed since it is not working even manually also for some network setup  issue, need to validate and add
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    // TODO: P9100 Validation is removed since it is not working even manually also for some network setup  issue, need to validate and add
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    // TODO: P9100 Validation is removed since it is not working even manually also for some network setup  issue, need to validate and add
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                securitySettings.ServiceTemplate = serviceTemplate;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                        isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

		/// <summary>
		/// Template for All IPv6Address and All Management Service
		/// </summary>
		/// <param name="activityData"></param>
		/// <returns></returns>
		public static bool Template_AllIPv6Address_AllManagementService(FirewallActivityData activityData)
		{
			try
			{
				bool result = true;
				BitArray resultArray = new BitArray(8, true);
				PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
				Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
				bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
                bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6Addresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllManagementServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				// Result validation
				TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
				resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																	p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
				resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
																	p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
				resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
																	p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
				resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
																	p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);


                // Drop Rule, Default: Allow

                EwsWrapper.Instance().DeleteAllRules();
                //Create 1st SNMP custom rule -> Allow and then All IPv6 + Managment -> DROP , Default -> DROP
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                securitySettings.Action = IPsecFirewallAction.AllowTraffic;
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);


                securitySettings.Action = IPsecFirewallAction.DropTraffic;
                securitySettings.ServiceTemplate = serviceTemplate;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                    isPingRequiredP9100Install: false);

                TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

                TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

                TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv6Address and All Discovery Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6Address_AllDiscoveryService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6Addresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllDiscoveryServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        // Workaround : As WSD with Stateless adress is not possible, printer discovers always with Link local. removing validation for WSD here
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }

                    if (activityData.Stateful)
                    {
                        // Workaround : As WSD with Stateless adress is not possible, printer discovers always with Link local. removing validation for WSD here
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("VEP Does not support Discovery over IPv6. Skipping this step.");
                }

                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("VEP Does not support Discovery over IPv6. Skipping this step.");
                }

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv6Address and All Microsoft Web Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6Address_AllMicrosoftWebService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6Addresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllWebServicesPrint;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("VEP does not support Discovery over IPv6 addresses. Skipping this step");
                }
                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                        , isPingRequiredP9100Install: false);
                }
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                            , isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                            , isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                           , isPingRequiredP9100Install: false);
                    }

                }
                else
                {
                    TraceFactory.Logger.Info("VEP does not supprort Discovery over IPv6 addresses. Skipping this step");
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        #endregion All IPv6 Address

        #region All IPv6 Link Local Address

        /// <summary>
        /// Template for All IPv6 Link Local Address and All Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6LinkLocalAddress_AllService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
				   bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());
                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6LinkLocal;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                        isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                        printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                        isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                        isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv6 Link Local Address and All Print Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6LinkLocalAddress_AllPrintService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6LinkLocal;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllPrintServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                        isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    // TODO: Removing Validation for P9100 since it has some issue while printing.Manual Team will come back on this
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                // Drop Rule, Default: Allow
                EwsWrapper.Instance().DeleteAllRules();
                securitySettings.Action = IPsecFirewallAction.DropTraffic;
                securitySettings.ServiceTemplate = serviceTemplate;

                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

		/// <summary>
		/// Template for All IPv6 Link Local Address and All Management Service
		/// </summary>
		/// <param name="activityData"></param>
		/// <returns></returns>
		public static bool Template_AllIPv6LinkLocalAddress_AllManagementService(FirewallActivityData activityData)
		{
			try
			{
				bool result = true;
				BitArray resultArray = new BitArray(8, true);
				PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
				Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
				bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
                bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6LinkLocal;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllManagementServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				// Result validation
				if (activityData.IPv4Enable)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
					resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																		p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}
				if (activityData.LinkLocal)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
					resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
																		p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}
				if (activityData.Stateless)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
					resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																		p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}
				if (activityData.Stateful)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
					resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																		p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}
				// Drop Rule, Default: Allow
				securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                securitySettings.Action = IPsecFirewallAction.AllowTraffic;
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);


                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.DropTraffic;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				// Result validation
				TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
				resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
																	p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
																	isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
				resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																 p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
				resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
																	p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

                TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
				resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv6 Link Local Address and All Discovery Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6LinkLocalAddress_AllDiscoveryService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
                bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());
                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6LinkLocal;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllDiscoveryServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("VEP Does not support Discovery Services over IPv6 addresses. Skipping this step");
                }

                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                            isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                            isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                            p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                            isPingRequiredP9100Install: false);
                    }
                }

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv6 Link Local Address and All Microsoft Web Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6LinkLocalAddress_AllMicrosoftWebService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6LinkLocal;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllWebServicesPrint;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("VEP does not support Discovery Services over IPv6 addresses. Skipping this step");
                }
                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                        , isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                        , isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                        , isPingRequiredP9100Install: false);
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("VEP does not suuport Discovery services over IPv6 addresses. Skipping this step");
                }

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        #endregion All IPv6 Link Local Address

        #region All IPv6 Non Link Local Address

        /// <summary>
        /// Template for All IPv6 Non Link Local Address and All Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6NonLinkLocalAddress_AllService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());
                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6NonLinkLocal;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv6 Non Link Local Address and All Print Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6NonLinkLocalAddress_AllPrintService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6NonLinkLocal;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllPrintServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                    isPingRequiredP9100Install: false);
                }

                if (activityData.Stateless)
                {
                    // TODO: P9100 Validation is removed since it is not working even manually also for some network setup  issue, need to validate and add
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    // TODO: P9100 Validation is removed since it is not working even manually also for some network setup  issue, need to validate and add
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                securitySettings.ServiceTemplate = serviceTemplate;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.LinkLocal)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                    resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                    isPingRequiredP9100Install: false);
                }
                if (activityData.Stateless)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                    resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                    isPingRequiredP9100Install: false);
                }
                if (activityData.Stateful)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                    resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

		/// <summary>
		/// Template for All IPv6 Non Link Local Address and All Management Service
		/// </summary>
		/// <param name="activityData"></param>
		/// <returns></returns>
		public static bool Template_AllIPv6NonLinkLocalAddress_AllManagementService(FirewallActivityData activityData)
		{
			try
			{
				bool result = true;
				BitArray resultArray = new BitArray(8, true);
				PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
				Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
				bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
                bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6NonLinkLocal;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllManagementServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				// Result validation
				TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
				resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail,
																https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
																printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
				resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
				resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
																p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

				TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
				resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
																p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);

                // Drop Rule, Default: Allow
                //securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                securitySettings.Action = IPsecFirewallAction.AllowTraffic;
                CreateSNMPServiceRule(activityData, addressTemplate, securitySettings);


                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.DropTraffic;
                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				// Result validation
				if (activityData.IPv4Enable)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
					resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
																	https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
																	printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}
				if (activityData.LinkLocal)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
					resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
																	p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}
				if (activityData.Stateless)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
					resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																	p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}
				if (activityData.Stateful)
				{
					TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
					resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, telnet: IsTPS ? DeviceServiceState.Fail : DeviceServiceState.Skip, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
																	p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
				}
				foreach (bool item in resultArray)
				{
					result &= item;
				}

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv6 Non Link Local Address and All Discovery Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6NonLinkLocalAddress_AllDiscoveryService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6NonLinkLocal;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllDiscoveryServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation

                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                    p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("VEP does not support Discovery services over IPv6 addresses. Skipping this stpe");
                }

                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                        isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                        isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel,
                                                                        isPingRequiredP9100Install: false);
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("VEP does not support Discovery services over IPv6 addresses. Skipping this step");
                }

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Template for All IPv6 Non Link Local Address and All Microsoft Web Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv6NonLinkLocalAddress_AllMicrosoftWebService(FirewallActivityData activityData)
        {
            try
            {
                bool result = true;
                BitArray resultArray = new BitArray(8, true);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop

                TraceFactory.Logger.Info("Checking Firewall option for Allow ::: Drop");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPv6NonLinkLocal;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllWebServicesPrint;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[0] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail, DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? DeviceServiceState.Pass : DeviceServiceState.Fail,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[1] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[2] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[3] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("VEP does not support Discovery services. Skipping this step");
                }
                // Drop Rule, Default: Allow
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Checking Firewall option for Drop ::: Allow");

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //CreateSNMPServiceRule(activityData,addressTemplate, securitySettings);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Result validation
                if (activityData.IPv4Enable)
                {
                    TraceFactory.Logger.Info("*************** Validating Services for IPv4 Address {0} ***************".FormatWith(activityData.IPv4Address));
                    resultArray[4] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                                    https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
                                                                    printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
                }
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (activityData.LinkLocal)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Link Local Address {0} ***************".FormatWith(activityData.IPv6LinkLocalAddress));
                        resultArray[5] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6LinkLocalAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                        , isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateless)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateless Address {0} ***************".FormatWith(activityData.IPv6StatelessAddress));
                        resultArray[6] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatelessAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                        , isPingRequiredP9100Install: false);
                    }
                    if (activityData.Stateful)
                    {
                        TraceFactory.Logger.Info("*************** Validating Services for IPv6 Stateful Address {0} ***************".FormatWith(activityData.IPv6StatefulAddress));
                        resultArray[7] = CtcUtility.ValidateDeviceServices(printer, activityData.IPv6StatefulAddress, DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, http: DeviceServiceState.Pass, https: DeviceServiceState.Pass,
                                                                        p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel
                                                                        , isPingRequiredP9100Install: false);
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("VEP does not support Discovery services over IPv6. Skipping this step");
                }
                foreach (bool item in resultArray)
                {
                    result &= item;
                }

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        #endregion All IPv6 Link Local Address

        #region Print cross Plug-in Templates

        /// <summary>        
        /// -	Create the rule on the printer and enable the firewall
        /// -	Install the printer (all Protocols)
        /// -	Send few print jobs
        /// -	Printing should be successful.
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Template_AllIPv4Address_AllServices(FirewallActivityData activityData, DefaultAddressTemplates defaultAddressTemplate, bool isIPV6 = false)
        {
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
            try
            {
                bool result = true;
                string address = null;
                BitArray resultArray = new BitArray(8, true);
                string outHostName = string.Empty;

                // Perform pre-requisites
                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = defaultAddressTemplate;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

				if(isIPV6)
				{
                    if(activityData.IPv6StatefulAddress != string.Empty)
                    {
                        address = activityData.IPv6StatefulAddress;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to retrieve Printer IPV6 address, hence failing the test case");
                        return false;
                    }					
				}
				else
				{
					address = activityData.IPv4Address;
				}
				// Validating Print for all protocols
				TraceFactory.Logger.Info("Installing driver with P9100 and validating print");
				resultArray[0] = InstallandPrint(IPAddress.Parse(address), Printer.Printer.PrintProtocol.RAW, activityData, 9100);
				TraceFactory.Logger.Info("Installing driver with LPD and validating print");
				resultArray[1] = InstallandPrint(IPAddress.Parse(address), Printer.Printer.PrintProtocol.LPD, activityData);				

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

				TraceFactory.Logger.Info("****************** Validation Summary for Print Protocols *****************");
				TraceFactory.Logger.Info("---------------------------------------------------------------------------");
				TraceFactory.Logger.Info("Services   * P9100  *   LPD  *  WSP  * IPP-80  *  IPP-631  * IPPS * ");
				TraceFactory.Logger.Info("---------------------------------------------------------------------------");
				TraceFactory.Logger.Info("Expected   *  " + string.Join("  *  ", "True") + "  * " + string.Join("  *  ", "True"));
				TraceFactory.Logger.Info("---------------------------------------------------------------------------");
				TraceFactory.Logger.Info("Actual     *  " + string.Join("  *  ", resultArray[0]) + "  * " + string.Join("  *  ", resultArray[1]));
				TraceFactory.Logger.Info("---------------------------------------------------------------------------");                

                return result;
            }
            finally
            {
                EwsWrapper.Instance().DeleteAllRules();
            }
        }
        #endregion

        #endregion Firewall Templates

        #region Advanced Firewall Templates

        /// <summary>        
        /// Step1:
        /// -	Create the rule on the printer and enable the firewall        
        /// -   PowerCycle the Printer
        /// -	Validate the existence of Rules
        /// -   Rules should exist
        /// Step2:                
        /// -   ColdReset the Printer
        /// -	Validate the existence of Rules
        /// -   Rules should not exist
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool ValidateRuleWithPowerCycleAndColdReset(FirewallActivityData activityData, DefaultAddressTemplates defaultAddressTemplate)
        {
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
            try
            {
                // Allow Rule, Default: Drop
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = defaultAddressTemplate;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

				EwsWrapper.Instance().CreateRule(securitySettings, true, true);
				EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
				EwsWrapper.Instance().SetIPsecFirewall(true);
                Thread.Sleep(TimeSpan.FromMinutes(1));
				printer.PowerCycle();

                if (!EwsWrapper.Instance().IsRuleExists())
                {
                    return false;
                }

                printer.ColdReset();
                return !EwsWrapper.Instance().IsRuleExists();
            }
            finally
            {
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// -	Create the rule on the printer with Custom Service P9100 and enable the firewall  
        /// -   Enable AllowTraffic Option and Default Policy to Drop
        /// -   Validate Print by P9100
        /// -   Print through P9100 should be successful
        /// -	Validate the other services
        /// -   All other services should fail.
        /// -   Enable Drop Traffic Option and Default Rule is Drop
        /// -   Validate Print by 9100
        /// -   Validation should be unsuccessful since the Drop Traffic option is enabled,.
        /// -   All other services should not work
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool ValidateCustomService(FirewallActivityData activityData, string customService, int testNo)
        {
            TraceFactory.Logger.Info("Validation of Custom Service- {0}".FormatWith(customService));

            try
            {
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());

                Prerequisite(printer, activityData);

                // Allow Rule, Default: Drop
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;
                Collection<Service> firewallService = new Collection<Service>();

                Service p9100Service = new Service();
                p9100Service.IsDefault = true;
                p9100Service.Name = "P9100";
                p9100Service.Protocol = ServiceProtocolType.TCP;
                p9100Service.PrinterPort = "9100";
                p9100Service.ServiceType = ServiceType.Printer;
                p9100Service.RemotePort = "Any";

                Service snmpService = new Service();
                snmpService.IsDefault = true;
                snmpService.Name = "SNMP";
                snmpService.Protocol = ServiceProtocolType.UDP;
                snmpService.PrinterPort = "161";
                snmpService.ServiceType = ServiceType.Printer;
                snmpService.RemotePort = "Any";

                Service httpService = new Service();
                httpService.IsDefault = true;
                httpService.Name = "HTTP";
                httpService.Protocol = ServiceProtocolType.TCP;
                httpService.PrinterPort = "80";
                httpService.ServiceType = ServiceType.Printer;
                httpService.RemotePort = "Any";

                Service wsDiscoverService = new Service();
                wsDiscoverService.IsDefault = true;
                // Added this condition because of discrepancy in WS Discovery in EWS page of Ink and other printers
                if (IsInk)
                {
                    wsDiscoverService.Name = "WS Discovery";
                }
                else
                {
                    wsDiscoverService.Name = "WS-Discovery";
                }
                wsDiscoverService.Protocol = ServiceProtocolType.UDP;
                wsDiscoverService.PrinterPort = "3702";
                wsDiscoverService.ServiceType = ServiceType.Printer;
                wsDiscoverService.RemotePort = "Any";

                Service ICMPService = new Service();
                ICMPService.IsDefault = true;
                ICMPService.Name = "ICMPv4";
                ICMPService.Protocol = ServiceProtocolType.ICMPv4;
                // For Ink, only available Printer port for ICMPv4 in EWS page is "None". For VEP None, 8 and 0 are available
                if (IsInk)
                {
                    ICMPService.PrinterPort = "None";
                }
                else
                {
                    ICMPService.PrinterPort = "8";
                }
                ICMPService.ServiceType = ServiceType.Printer;
                ICMPService.RemotePort = "None";

                if (customService == "P9100")
                {
                    firewallService.Add(p9100Service);
                    firewallService.Add(snmpService);
                    firewallService.Add(ICMPService);
                }
                else if (customService == "SNMP")
                {
                    firewallService.Add(snmpService);
                }
                else if (customService == "HTTP")
                {
                    firewallService.Add(httpService);
                }
                else if (customService == "WS-Discovery")
                {
                    EwsWrapper.Instance().SetSecurityWSDiscovery(false);
                    firewallService.Add(wsDiscoverService);
                }

                //ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(SERVICE_TEMPLATE_NAME + testNo, firewallService);                
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(SERVICE_TEMPLATE_NAME + testNo, firewallService);

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;

                TraceFactory.Logger.Info("Default: Drop, Traffic: AllowTraffic");
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                //Add custom rule for WS-print for WS-discovery to work
                if (customService == "WS-Discovery")
                {
                    
                    CreateWSPrintServiceRule(activityData, addressTemplate, securitySettings);
                }
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                if (!ValidateCustomService(activityData, customService))
                {
                    return false;
                }

                // Drop Rule, Default: Drop    
                TraceFactory.Logger.Info("Default: Allow, Traffic: DropTraffic");
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);
                securitySettings.Action = IPsecFirewallAction.DropTraffic;

                return !ValidateCustomService(activityData, customService);
            }
            finally
            {
                EwsWrapper.Instance().SetSecurityWSDiscovery(true);
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// -	Create the rule on the printer with Print Services
        /// -   Enable AllowTraffic Option and Default Policy to Drop
        /// -   Create the rule on the Printer with Management Services
        /// -   Enable AllowTraffic Option and Default Policy to Drop
        /// -   Validate Print by P9100 and SNMP
        /// -   Print and SNMP should be successful
        /// -	Validate the other services [exclude Print and Management]
        /// -   All other services should fail.
        /// -   Enable Drop Traffic Option and Default Rule is Drop
        /// -   Print and SNMP should be Unsuccessful
        /// -	Validate the other services [exclude Print and Management]
        /// -   All other Services should pass
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool ValidateCustomServiceWithMultipleRules(FirewallActivityData activityData, int testNo)
        {
            TraceFactory.Logger.Info("Validation of Print and Management Services by creating multiple Rules in Printer");

			try
			{
				PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
				Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
				bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily);
				bool IsVEP = PrinterFamilies.VEP.ToString().EqualsIgnoreCase(activityData.ProductFamily);
				bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily);
                bool IsLFP = PrinterFamilies.LFP.ToString().EqualsIgnoreCase(activityData.ProductFamily);


                Prerequisite(printer, activityData);

                if (!CreateMultipleRule(activityData, DefaultAction.Drop, IPsecFirewallAction.AllowTraffic))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating Print and Management Services with multiple rules, Default: Drop, Rules: Allow");
                if (!CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Pass, DeviceServiceState.Pass, http: DeviceServiceState.Pass,
                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass, wsd: IsVEP ? DeviceServiceState.Fail : DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false))
                {
                    return false;
                }

                EwsWrapper.Instance().DeleteAllRules();

                if (!CreateMultipleRule(activityData, DefaultAction.Allow, IPsecFirewallAction.DropTraffic))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating Print and Management Services with multiple rules, Default: Allow, Rules: Drop");
                return CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail,
                                                https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false);
            }
            finally
            {
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Delete All existing rules, cold rest printer
        /// </summary>
        /// <param name="printer"></param>
        private static void Prerequisite(Printer.Printer printer, FirewallActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);

            EwsWrapper.Instance().Stop();
            EwsWrapper.Instance().Start("https");
            EwsWrapper.Instance().DeleteAllRules();

            if (activityData.ProductFamily.EqualsIgnoreCase("TPS"))
            {
                ColdReset(activityData, printer);
            }

            EwsWrapper.Instance().SetAdvancedOptions();
            EwsWrapper.Instance().SetDHCPv6OnStartup(true);
            //Only for VEP 
            EwsWrapper.Instance().EnableDisablev6();
            // Disable WebEncryption option to stop navigation forcefull
            EwsWrapper.Instance().SetEncryptWebCommunication(false);
        }

        /// <summary>
        /// Install and Print for specified PrintProtocol and Port no
        /// </summary>
        /// <param name="ipAddress">Ip Address of printer</param>
        /// <param name="protocol"><see cref="Printer.Printer.PrintProtocol"/></param>
        /// <param name="activityData"><see cref=" FirewallActivityData"/></param>
        /// <param name="portNo">Print protocol port no</param>
        /// <returns>true if installation and print is successful, false otherwise</returns>
        private static bool InstallandPrint(IPAddress ipAddress, Printer.Printer.PrintProtocol protocol, FirewallActivityData activityData, int portNo = 515, string hostName = null)
        {
            bool result = false;

            PrinterFamilies printerFamily = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(printerFamily, ipAddress);

            if (protocol == Printer.Printer.PrintProtocol.WSP)
            {
                printer.NotifyWSPrinter += CtcUtility.printer_NotifyWSPAddition;
                if (printer.Install(ipAddress, Printer.Printer.PrintProtocol.WSP, activityData.PrintDriver, activityData.PrintDriverModel))
                {
                    MessageBox.Show("WS Printer was added successfully.", "WS Printer Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("WS Printer was not added successfully. All WS Print related tests will fail.", "WS Printer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (printer.Install(ipAddress, protocol, activityData.PrintDriver, activityData.PrintDriverModel, portNo, hostName))
            {
                result = printer.Print(CtcUtility.CreateFile("Test File for Firewall Security."));
            }
            else
            {
                TraceFactory.Logger.Info("Printer Installation is unsuccessful for {0} IP Address with {1} Protocol.".FormatWith(ipAddress, protocol));
            }

            if (result)
            {
                TraceFactory.Logger.Info("Print Job is successful for {0} IP Address with {1} Protocol.".FormatWith(ipAddress, protocol));
            }
            else
            {
                TraceFactory.Logger.Info("Print Job is unsuccessful for {0} IP Address with {1} Protocol.".FormatWith(ipAddress, protocol));
                printer.DeleteAllPrintQueueJobs();
            }

            return result;
        }

        /// <summary>
        /// Validate Service
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool ValidateCustomService(FirewallActivityData activityData, string customService)
        {
            PrinterFamilies printerFamily = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(printerFamily, IPAddress.Parse(activityData.IPv4Address));
            bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily);
            bool IsTPS = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily);

			TraceFactory.Logger.Info("Validation: {0} should pass and all other services should fail, since we have created Custom Service for {0}".FormatWith(customService));
			if (customService == "P9100")
			{
				if(!CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, DeviceServiceState.Pass, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Pass, http: DeviceServiceState.Fail,
											   https: DeviceServiceState.Pass, p9100: DeviceServiceState.Pass, lpd: DeviceServiceState.Fail, wsd: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false))
				{
					return false;
				}			
			}
			else if (customService == "SNMP")
			{
				if (!CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Pass, http: DeviceServiceState.Fail,
											  https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: IsTPS? DeviceServiceState.Pass:DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false))
				{
					return false;
				}
			}
			else if (customService == "HTTP")
			{
				if (!CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Pass,
											  https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false))
				{
					return false;
				}
			}
			else if (customService == "WS-Discovery")
			{
				if (!CtcUtility.ValidateDeviceServices(printer, activityData.IPv4Address, IsTPS ? DeviceServiceState.Pass : DeviceServiceState.Fail, IsInk ? DeviceServiceState.Skip : DeviceServiceState.Fail, DeviceServiceState.Fail, http: DeviceServiceState.Fail,
											  https: DeviceServiceState.Pass, p9100: DeviceServiceState.Fail, lpd: DeviceServiceState.Fail, wsd: DeviceServiceState.Pass, printerDriver: activityData.PrintDriver, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false))
				{
					return false;
				}
			}
			return true;
		}

        /// <summary>
        /// Creating Multiple Rules
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool CreateMultipleRule(FirewallActivityData activityData, DefaultAction action, IPsecFirewallAction ruleAction)
        {
            Collection<Service> firewallService = new Collection<Service>();
            try
            {
                TraceFactory.Logger.Info("Creating First Rule with Print Service, DefaultAction: {0} and DefaultRule: {1}".FormatWith(action, ruleAction));
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllPrintServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = ruleAction;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(action);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                TraceFactory.Logger.Info("Creating Second Rule with Management Service, DefaultAction: {0} and DefaultRule: {1}".FormatWith(action, ruleAction));
                addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;

                serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllManagementServices;

                securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = ruleAction;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(action);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                Service ICMPService = new Service();
                ICMPService.IsDefault = true;
                ICMPService.Name = "ICMPv4";
                ICMPService.Protocol = ServiceProtocolType.ICMPv4;
                // For Ink, only available Printer port for ICMPv4 in EWS page is "None". For VEP None, 8 and 0 are available
                if (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    ICMPService.PrinterPort = "None";
                }
                else
                {
                    ICMPService.PrinterPort = "8";
                }
                ICMPService.ServiceType = ServiceType.Printer;
                ICMPService.RemotePort = "None";

                firewallService.Add(ICMPService);
                //	serviceTemplate = new ServiceTemplateSettings(SERVICE_TEMPLATE_NAME + 12, firewallService);
                serviceTemplate = new ServiceTemplateSettings(SERVICE_TEMPLATE_NAME + 12, firewallService);

                addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;

                securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = ruleAction;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(action);
                EwsWrapper.Instance().SetIPsecFirewall(true);
                return true;
            }
            catch (Exception ruleException)
            {
                TraceFactory.Logger.Info("Exception:{0}".FormatWith(ruleException.Message));
                return false;
            }
        }

        /// <summary>
        /// Get SecurityRuleSettings structure for PSK Authentication type
        /// </summary>
        /// <param name="testNo">Test Number</param>
        /// <param name="address">Address Template</param>
        /// <param name="service">Service Template</param>
        /// <param name="strength">IKE Security Strength</param>
        /// <param name="phase1Settings">Phase 1 settings for Custom strength</param>
        /// <param name="phase2Settings">Phase 2 settings for Custom strength</param>
        /// <returns>Security Rule Settings <see cref="SecurityRuleSettings"/></returns>
        public static SecurityRuleSettings GetPSKRuleSettings(int testNo, AddressTemplateSettings address, ServiceTemplateSettings service, IKESecurityStrengths strength = IKESecurityStrengths.LowInteroperabilityHighsecurity,
                                                               IKEPhase1Settings phase1Settings = null, IKEPhase2Settings phase2Settings = null)
        {
            IKEv1Settings ikeV1Settings = null;

            // For Custom strength, provide Phase 1 and Phase 2 settings
            if (IKESecurityStrengths.Custom == strength)
            {
                if (null == phase1Settings)
                {
                    throw new ArgumentNullException("phase1Settings", "Invalid phase 1 settings.");
                }

                if (null == phase2Settings)
                {
                    throw new ArgumentNullException("phase2Settings", "Invalid phase 2 settings.");
                }

                ikeV1Settings = new IKEv1Settings("123", phase1Settings, phase2Settings);
            }
            else
            {
                ikeV1Settings = new IKEv1Settings("123");
            }

            DynamicKeysSettings dynamicSettings = new DynamicKeysSettings(strength, ikeV1Settings);
            IPsecTemplateSettings ipsecTemplate = new IPsecTemplateSettings("IPSECTEMPLATE".FormatWith(testNo), dynamicSettings);

            SecurityRuleSettings securitySettings = new SecurityRuleSettings("CLIENTRULE".FormatWith(testNo), address, service, IPsecFirewallAction.ProtectedWithIPsec, ipsecTemplate);

            return securitySettings;
        }

		/// <summary>
		/// Creating Rule for SNMP Service
		/// </summary>
		/// <param name="testNo">Test Number</param>
		/// <param name="address">Address Template</param>
		/// <param name="service">Service Template</param>
		/// <param name="strength">IKE Security Strength</param>
		/// <param name="phase1Settings">Phase 1 settings for Custom strength</param>
		/// <param name="phase2Settings">Phase 2 settings for Custom strength</param>
		/// <returns>Security Rule Settings <see cref="SecurityRuleSettings"/></returns>
		public static void CreateSNMPServiceRule(FirewallActivityData activityData, AddressTemplateSettings addressTemplate, SecurityRuleSettings securitySettings)
		{
			PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
			bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
            bool IsLFP = PrinterFamilies.LFP.ToString().EqualsIgnoreCase(family.ToString());
            // Creating third rule with printing service 9100                
            Service defaultCustomServiceSNMPUDP = new Service();
            defaultCustomServiceSNMPUDP.IsDefault = true;
            defaultCustomServiceSNMPUDP.Name = "SNMP";
            defaultCustomServiceSNMPUDP.Protocol = ServiceProtocolType.UDP;
            defaultCustomServiceSNMPUDP.PrinterPort = "161";
            defaultCustomServiceSNMPUDP.ServiceType = ServiceType.Printer;
            defaultCustomServiceSNMPUDP.RemotePort = "Any";

            Service defaultCustomServiceSNMPTCP = new Service();
            defaultCustomServiceSNMPTCP.IsDefault = true;
            defaultCustomServiceSNMPTCP.Name = "SNMP";
            defaultCustomServiceSNMPTCP.Protocol = ServiceProtocolType.TCP;
            defaultCustomServiceSNMPTCP.PrinterPort = "161 - 162";
            defaultCustomServiceSNMPTCP.ServiceType = ServiceType.Printer;
            defaultCustomServiceSNMPTCP.RemotePort = "Any";

			Collection<Service> ipSecService = new Collection<Service>();
			ipSecService.Add(defaultCustomServiceSNMPUDP);
			// In InkJet, there is no service with SNMP-TCP            
			if (!IsInk && !IsLFP)
			{
				ipSecService.Add(defaultCustomServiceSNMPTCP);
			}

            // Setting default Address and custom Service Templates                

            ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(SERVICE_TEMPLATE_NAME + 1, ipSecService);

            // create rule with Custom Service Template settings

            securitySettings.ServiceTemplate = serviceTemplate;
            EwsWrapper.Instance().CreateRule(securitySettings, true);

        }

  			/// <summary>
        ///Create WSP Print custom rule
        /// </summary>
        /// <param name="activityData">ActivityData</param>
        /// <param name="printer">/param>
        /// <returns></returns>
        public static void CreateWSPrintServiceRule(FirewallActivityData activityData, AddressTemplateSettings addressTemplate, SecurityRuleSettings securitySettings)
        {
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(family.ToString());
            // Creating third rule with printing service 9100                
            Service defaultCustomServiceWSPrintRemote = new Service();
            defaultCustomServiceWSPrintRemote.IsDefault = true;
            defaultCustomServiceWSPrintRemote.Name = "Web Services Print";
            defaultCustomServiceWSPrintRemote.Protocol = ServiceProtocolType.TCP;
            defaultCustomServiceWSPrintRemote.PrinterPort = "5226";
            defaultCustomServiceWSPrintRemote.ServiceType = ServiceType.Remote;
            defaultCustomServiceWSPrintRemote.RemotePort = "Any";

            Service defaultCustomServiceWSPrintPrinter = new Service();
            defaultCustomServiceWSPrintPrinter.IsDefault = true;
            defaultCustomServiceWSPrintPrinter.Name = "Web Services Print";
            defaultCustomServiceWSPrintPrinter.Protocol = ServiceProtocolType.TCP;
            defaultCustomServiceWSPrintPrinter.PrinterPort = "3910";
            defaultCustomServiceWSPrintPrinter.ServiceType = ServiceType.Printer;
            defaultCustomServiceWSPrintPrinter.RemotePort = "Any";

            Collection<Service> ipSecService = new Collection<Service>();
            ipSecService.Add(defaultCustomServiceWSPrintRemote);
            // In InkJet, there is no service with SNMP-TCP            
            if (!IsInk)
            {
                ipSecService.Add(defaultCustomServiceWSPrintPrinter);
            }

            // Setting default Address and custom Service Templates                

            ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(SERVICE_TEMPLATE_NAME + 1, ipSecService);

            // create rule with Custom Service Template settings

            securitySettings.ServiceTemplate = serviceTemplate;
            EwsWrapper.Instance().CreateRule(securitySettings, true);

        }

        /// <summary>
        /// ColdReset the product
        /// </summary>
        /// <param name="activityData">ActivityData</param>
        /// <param name="printer">printer object to call cold reset function</param>
        /// <returns>true/false</returns>
        private static bool ColdReset(FirewallActivityData activityData, Printer.Printer printer, int timeOut = 0)
        {
            if (!(PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily) || (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))))
            {
                if (0 == timeOut)
                {
                    return printer.ColdReset();
                }
                else
                {
                    return printer.ColdReset(timeOut);
                }
            }
            else
            {
                IPAddress linkLocalAddress = printer.IPv6LinkLocalAddress;

                if (PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    if (0 == timeOut)
                    {
                        printer.ColdReset();
                    }
                    else
                    {
                        printer.ColdReset(timeOut);
                    }
                }
                else // Since Cold reset cannot be done through code 
                {
                    EwsWrapper.Instance().SetFactoryDefaults(false);
                }

                // Wireless configuration will be removed on cold reset, configuring wireless using wired ip address.
                if (activityData.PrinterConnectivity.Equals(ConnectivityType.Wireless))
                {
                    INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

                    #region Enable Switch Port

                    if (networkSwitch.EnablePort(activityData.PortNo))
                    {
                        TraceFactory.Logger.Info("Enabled Port# {0} to fetch wired ip address to configure wireless back after cold reset".FormatWith(activityData.PortNo));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Make sure valid switch port is provided");
                        TraceFactory.Logger.Info("Failed to enable port# {0} to fetch wired ip address to configure wireless back after cold reset".FormatWith(activityData.PortNo));
                        return false;
                    }

                    string wiredAddress = string.Empty;
                    string hostName = string.Empty;

                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    if (!BacaBodWrapper.DiscoverDevice(activityData.WiredMacAddress, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.All, ref wiredAddress, ref hostName))
                    {
                        BacaBodWrapper.DiscoverDevice(activityData.WiredMacAddress, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.All, ref wiredAddress, ref hostName);
                    }

                    if (!printer.PingUntilTimeout(IPAddress.Parse(wiredAddress), TimeSpan.FromMinutes(1)))
                    {
                        return false;
                    }

                    // To make sure all stack is up and active, waiting for a while
                    Thread.Sleep(TimeSpan.FromMinutes(1));

                    #endregion

                    #region Configure Wireless

                    // Changing device address to wired ip to configure wireless
                    EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(wiredAddress));

                    WirelessSettings settings = new WirelessSettings() { WirelessRadio = true, SsidName = activityData.SsidName };
                    if (EwsWrapper.Instance().ConfigureWireless(settings, new WirelessSecuritySettings()))
                    {
                        TraceFactory.Logger.Info("Successfully Re-configured Wireless on the device after cold reset using the SSID:{0}".FormatWith(activityData.SsidName));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to Re-configure Wireless on the device after cold reset using the SSID:{0}".FormatWith(activityData.SsidName));
                        return false;
                    }

                    #endregion

                    #region Disable Switch Port

                    if (networkSwitch.DisablePort(activityData.PortNo))
                    {
                        TraceFactory.Logger.Info("Disabled Port# {0} to retrieve back wireless ip address after cold reset".FormatWith(activityData.PortNo));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Please make sure that you are configured a managed switch with proper port number as input through UI");
                        TraceFactory.Logger.Info("Failed to disable port# {0} to retrieve back wireless ip address after cold reset".FormatWith(activityData.PortNo));
                        return false;
                    }

                    if (!printer.PingUntilTimeout(IPAddress.Parse(activityData.IPv4Address), TimeSpan.FromMinutes(1)))
                    {
                        return false;
                    }

                    EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(activityData.IPv4Address));
                    #endregion
                }

                // Telnet option will be disabled after performing cold reset, enabling the same.
                if (!PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    EwsWrapper.Instance().SetTelnet();
                }
            }

            return true;
        }
        #endregion Private Functions
    }
}
