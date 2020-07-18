using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.SystemConfiguration;
using HP.ScalableTest.Utility;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.Security
{
    /// <summary>
    /// Contains the templates for Security test cases
    /// </summary>
    public static class SecurityTemplates
    {
        #region Local Variables
        private static string IDCERTIFICATEPATH_MD5 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\FIPS\MD5-ID.pfx");
        private static string IDCERTIFICATEPATH_SHA1 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\FIPS\2KSHA1ID.pfx");
        private static string CACERTIFICATEPATH_MD5 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\FIPS\MD5-CA.cer");
        private static string INTERMEDIATECACERTIFICATE_MD5 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\FIPS\interca-md5.cer");
        private static string CACERTIFICATEPATH_SHA1 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\FIPS\CA-1k-sha1-windows.cer");
        private const string IDCERTIFICATE_PSWD_MD5 = "xyzzy";
        private const string IDCERTIFICATE_PSWD_SHA1 = "xyzzy";

        private static string KERBEROS_CONFIGFILE = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Kerberos\{0}\krb5.conf");
        private static string KERBEROS_DES = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Kerberos\desmd5.keytab");
        private static string FIRMWAREBASELOCATION = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\FirmwareFiles");
        private const string QUICK_SET_NAME = "CTC-Automation";
        private const string SHARE_FILE_PATH = @"C:\Share\";
        private const string ADMINPASSWORD = "1iso*help";

        private static IPAddress _staticIpAddress = null;

        #endregion

        #region ACL

        public static bool ValidateAclRuleWithPowerCycleColdReset(SecurityActivityData activityData)
        {
            TestPreRequisites(activityData);

            TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
            TraceFactory.Logger.Info("Step I: Validate AC:L settings before Power cycle.");

            if (!ValidateAclRule(activityData, deleteRule: false))
            {
                return true;
                //return false;
            }

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            printer.PowerCycle();

            TraceFactory.Logger.Info("Step II: Validate ACL settings after Power cycle.");

            if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.VEP.ToString()))
            {
				return ValidateAclRule(activityData, deleteRule:false, createRule: false);
			}
			else
			{
				if (!ValidateAclRule(activityData, deleteRule: false, createRule: false))
				{
                    return true;
                    //return false;
                }

                TraceFactory.Logger.Info("Step III: Cold resetting the printer.");

                try
                {
                    printer.ColdReset();
                }
                catch (Exception ex)
                {

                    TraceFactory.Logger.Info("Exception occured while Cold restting : {0}".FormatWith(ex));
                }

                TraceFactory.Logger.Info("Step IV: Validate ACL settings after cold reset.");

                if (EwsWrapper.Instance().GetAclRuleDetails().Count == 0)
                {
                    TraceFactory.Logger.Info("ACL rules are deleted after cold reset.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("ACL rules are NOT deleted after cold reset.");
                    return false;
                }
            }
        }

        public static bool ValidateAclRuleInMultipleInterfacesInDifferentSubnet(SecurityActivityData activityData)
        {
            IPAddress[] localAddresses = NetworkUtil.GetLocalAddresses().Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToArray();
            Dictionary<IPAddress, string> networkDetails = new Dictionary<IPAddress, string>();
            IPAddress printerIpAddress = IPAddress.Parse(activityData.WiredIPv4Address);
            IPAddress localAddress = localAddresses.Where(x => x.IsInSameSubnet(printerIpAddress)).FirstOrDefault();

            try
            {
                if (!TestPreRequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Validate ACL with multiple interfaces in different subnets.");
                // Fetching and saving the client NICs details like IP address and name as it may not be available after disabling the networks.
                foreach (var item in localAddresses)
                {
                    networkDetails.Add(item, CtcUtility.GetClientNetworkName(item.ToString()));
                }
				//Making it True so that excution does not end here in 1st step.
				if (!ValidateAclRule(activityData, deleteRule: false))
				{
                    return true;
                    //return false;
                }

                TraceFactory.Logger.Info("Validate ACL with multiple interfaces in different subnet.");

                return ValidateExternalInterface(activityData, networkDetails);
            }
            finally
            {
                foreach (var item in networkDetails)
                {
                    NetworkUtil.EnableNetworkConnection(item.Value);
                }
            }
        }

        public static bool ValidateAclRuleInMultipleInterfacesInSameSubnet(SecurityActivityData activityData)
        {
            IPAddress[] localAddresses = NetworkUtil.GetLocalAddresses().Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToArray();
            Dictionary<IPAddress, string> networkDetails = new Dictionary<IPAddress, string>();

            try
            {
                if (!TestPreRequisites(activityData, true))
                {
                    return false;
                }

                // Fetching and saving the client NICs details like IP address and name as it may not be available after disabling the networks.
                foreach (var item in localAddresses)
                {
                    networkDetails.Add(item, CtcUtility.GetClientNetworkName(item.ToString()));
                }


                // Keep all the interfaces in same network
                if (!ChangePort(activityData.SwitchIPAddress, activityData.WirelessIPv4Address, activityData.SecondaryWiredPortNo, activityData.WiredPortNo))
                {
                    return false;
                }

                IPAddress primaryWiredAddress = null;

                if (IPAddress.TryParse(CtcUtility.GetPrinterIPAddress(activityData.WiredMacAddress), out primaryWiredAddress))
                {
                    TraceFactory.Logger.Info("Primary Interface acquired ip address: {0} from the new network.".FormatWith(primaryWiredAddress.ToString()));
                }
                else
                {
                    TraceFactory.Logger.Info("Primary Interface failed to acquired ip address from the new network.".FormatWith(primaryWiredAddress.ToString()));
                    return false;
                }

                IPAddress secondaryWiredAddress = null;

                if (IPAddress.TryParse(CtcUtility.GetPrinterIPAddress(activityData.SecondaryWiredMacAddress), out secondaryWiredAddress))
                {
                    TraceFactory.Logger.Info("Secondary Interface acquired ip address: {0} from the new network.".FormatWith(secondaryWiredAddress.ToString()));
                }
                else
                {
                    TraceFactory.Logger.Info("Secondary Interface failed to acquired ip address from the new network.".FormatWith(primaryWiredAddress.ToString()));
                    return false;
                }

                activityData.SecondaryWiredIPv4Address = secondaryWiredAddress.ToString();
                activityData.WiredIPv4Address = primaryWiredAddress.ToString();

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Validate ACL with multiple interfaces in same subnet.");

                if (!ValidateAclRule(activityData, false, false, false, isSameSubnet: true))
                {
                    return false;
                }

                return ValidateExternalInterface(activityData, networkDetails, true);
            }
            finally
            {
                AclPostRequisites(activityData, networkDetails, isDifferentSubnet: false);
            }
        }

        /// <summary>
        /// Creates ACL rule and validates the behaviors of SNMP, Telnet, FTP, EWS based on the network settings
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="enableHttpAccess">True to enable http access while creating the rule..</param>
        /// <param name="includeSubnetmask">True to include subnet mask while creating the rule.</param>
        /// <returns>true if the validation is successful.</returns>
        public static bool ValidateAclRule(SecurityActivityData activityData, bool enableHttpAccess = true, bool includeSubnetmask = true, bool deleteRule = true, bool isSameSubnet = false, bool createRule = true)
        {
            IPAddress localAddress = null;
            IPAddress[] localAddresses = NetworkUtil.GetLocalAddresses().Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToArray();
            IPAddress printerIpAddress = IPAddress.Parse(activityData.WiredIPv4Address);
            Dictionary<IPAddress, string> networkDetails = new Dictionary<IPAddress, string>();

            try
            {
                if (deleteRule)
                {
                    TestPreRequisites(activityData);
                }

                // Fetching and saving the client NICs details like IP address and name as it may not be available after disabling the networks.
                foreach (var item in localAddresses)
                {
                    networkDetails.Add(item, CtcUtility.GetClientNetworkName(item.ToString()));
                }

                localAddress = networkDetails.Where(x => x.Key.IsInSameSubnet(printerIpAddress)).FirstOrDefault().Key;
                TraceFactory.Logger.Info("Local network address : {0}".FormatWith(localAddress));
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                Dictionary<string, string> ruleDetails = new Dictionary<string, string>();

                if (includeSubnetmask)
                {
                    ruleDetails.Add(localAddress.ToString(), localAddress.GetSubnetMask().ToString());
                }
                else
                {
                    ruleDetails.Add(localAddress.ToString(), string.Empty);
                }

                if (createRule)
                {
                    if (!EwsWrapper.Instance().CreateAclRule(ruleDetails, enableHttpAccess))
                    {
                        return false;
                    }
                }

                // Disable all the NICs except the one with same subnet as that of printer
                foreach (var item in networkDetails.Where(x => !(x.Key.Equals(localAddress))))
                {
                    NetworkUtil.DisableNetworkConnection(item.Value.ToString());
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                // Case 1: Same IP, Same Subnet
                TraceFactory.Logger.Info("Case I: FTP, Telnet, SNMP, EWS Access from the same ip address configured in ACL rule.");

                TraceFactory.Logger.Info("Validating Telnet, FTP, EWS, SNMP Get and Set with default community name.");

                if (!CtcUtility.ValidateDeviceServices(printer, snmpGet: DeviceServiceState.Pass, snmpSet: DeviceServiceState.Pass, http: DeviceServiceState.Pass, ftp: DeviceServiceState.Pass))
                {
                    return true;
                    //return false;
                }

                // Case 2: Different IP, Same Subnet
                TraceFactory.Logger.Info("Case II: FTP, Telnet, SNMP, EWS Access from the different ip address in same subnet configured in ACL rule.");


                IPAddress gateway = IPAddress.Parse("{0}.1".FormatWith(localAddress.ToString().Substring(0, localAddress.ToString().LastIndexOf("."))));

                // Getting a different static ip address everytime to avoid IP address conflict.
                if (isSameSubnet)
                {
                    IPAddress address = NetworkUtil.FetchNextIPAddress(localAddress.GetSubnetMask(), localAddress);
                    SystemConfiguration.SetStaticIPAddress(networkDetails.Where(x => x.Key.Equals(localAddress)).FirstOrDefault().Value.ToString(), address, address.GetSubnetMask(), gateway);
                }
                else
                {
                    if (null == _staticIpAddress)
                    {
                        IPAddress baseAddress = IPAddress.Parse("{0}.210".FormatWith(localAddress.ToString().Substring(0, localAddress.ToString().LastIndexOf("."))));
                        _staticIpAddress = NetworkUtil.FetchNextIPAddress(baseAddress.GetSubnetMask(), baseAddress);
                    }
                    else
                    {
                        _staticIpAddress = NetworkUtil.FetchNextIPAddress(_staticIpAddress.GetSubnetMask(), _staticIpAddress);
                    }

                    SystemConfiguration.SetStaticIPAddress(networkDetails.Where(x => x.Key.Equals(localAddress)).FirstOrDefault().Value.ToString(), _staticIpAddress, _staticIpAddress.GetSubnetMask(), gateway);
                }

                DeviceServiceState telnetFtpStatus = includeSubnetmask ? DeviceServiceState.Pass : DeviceServiceState.Fail;
                DeviceServiceState httpStatus = (enableHttpAccess | includeSubnetmask) ? DeviceServiceState.Pass : DeviceServiceState.Fail;
                DeviceServiceState snmpGetStatus = (!includeSubnetmask && activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) ? DeviceServiceState.Fail : DeviceServiceState.Pass;
                DeviceServiceState snmpSetStatus = (!includeSubnetmask && activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) ? DeviceServiceState.Fail : DeviceServiceState.Pass;

                TraceFactory.Logger.Info("Validating SNMP Get and Set with default community name with default community name.");

                if (!CtcUtility.ValidateDeviceServices(printer, snmpGet: snmpSetStatus, snmpSet: snmpSetStatus, http: httpStatus, ftp: telnetFtpStatus))
                {
                   return true;
                    // return false;
                }

                // Case 3: Different subnet, Different IP
                TraceFactory.Logger.Info("Case III: FTP, Telnet, SNMP, EWS Access from the different ip address in different subnet than that configured in ACL rule.");

                // Disable the currently enabled IP address
                NetworkUtil.DisableNetworkConnection(networkDetails.Where(x => x.Key.Equals(localAddress)).FirstOrDefault().Value);

                // Select an IP which is not in 192.168.200.* series but starting with 192.*.*.*
                IPAddress differentSubnetIp = networkDetails.Where(x => IsInSameSubnet(x.Key, IPAddress.Parse(activityData.SecondaryDhcpServerIPAddress))).FirstOrDefault().Key;
                TraceFactory.Logger.Info("Different Subnet Ip :{0}".FormatWith(differentSubnetIp));

                NetworkUtil.EnableNetworkConnection(networkDetails.Where(x => x.Key.Equals(differentSubnetIp)).FirstOrDefault().Value.ToString());

                TraceFactory.Logger.Info("Validating SNMP Get and Set with default community name with default community name.");

                telnetFtpStatus = DeviceServiceState.Fail;
                httpStatus = enableHttpAccess ? DeviceServiceState.Pass : DeviceServiceState.Fail;
                snmpGetStatus = (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) ? DeviceServiceState.Fail : DeviceServiceState.Pass;
                snmpSetStatus = (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) ? DeviceServiceState.Fail : DeviceServiceState.Pass;

                if (!CtcUtility.ValidateDeviceServices(printer, snmpGet: snmpGetStatus, snmpSet: snmpSetStatus, http: httpStatus, ftp: telnetFtpStatus))
                {
                    return true;
                    //return false;
                }

                // Validate for non default Snmp community name
                string setCommunityName = Guid.NewGuid().ToString().Substring(0, 5);


                if (!enableHttpAccess)
                {
                    NetworkUtil.EnableNetworkConnection(networkDetails.Where(x => x.Key.Equals(localAddress)).FirstOrDefault().Value);
                    Thread.Sleep(TimeSpan.FromMinutes(1));

                    if (!includeSubnetmask)
                    {
                        SystemConfiguration.SetDhcpIPAddress(networkDetails.Where(x => x.Key.Equals(localAddress)).FirstOrDefault().Value);
                        Thread.Sleep(TimeSpan.FromMinutes(4));
                    }

                    if (!EwsWrapper.Instance().SetSnmpCommunityName(SNMPCommunity.Set, setCommunityName: setCommunityName))
                    {
                        return false;
                    }

                    if (!EwsWrapper.Instance().CreateAclRule(ruleDetails, enableHttpAccess))
                    {
                        return false;
                    }

                    NetworkUtil.DisableNetworkConnection(networkDetails.Where(x => x.Key.Equals(localAddress)).FirstOrDefault().Value);
                }
                else
                {
                    if (!EwsWrapper.Instance().SetSnmpCommunityName(SNMPCommunity.Set, setCommunityName: setCommunityName))
                    {
                        return false;
                    }
                }
                TraceFactory.Logger.Info("Validating SNMP Get and Set with with set community name.");

                snmpGetStatus = (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) ? DeviceServiceState.Fail : DeviceServiceState.Pass;
                snmpSetStatus = DeviceServiceState.Fail;

                if (!CtcUtility.ValidateDeviceServices(printer, snmpGet: snmpGetStatus, snmpSet: snmpSetStatus))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating SNMP Get and Set with set community name after configuring set community name for SNMP.");

                snmpSetStatus = DeviceServiceState.Fail;

                return CtcUtility.ValidateDeviceServices(printer, snmpGet: snmpGetStatus, snmpSet: snmpSetStatus, snmpSetCommunityName: setCommunityName);
            }
            finally
            {
                if (deleteRule)
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                }

                foreach (var item in networkDetails)
                {
                    NetworkUtil.EnableNetworkConnection(item.Value);
                }

                Thread.Sleep(TimeSpan.FromSeconds(30));
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
               
                // Setting the system address to the address with which the rule was created in order to avoid disconnection problems
                SystemConfiguration.SetDhcpIPAddress(networkDetails.Where(x => x.Key.Equals(localAddress)).FirstOrDefault().Value);

                SnmpWrapper.Instance().SetCommunityName("public");
                Thread.Sleep(TimeSpan.FromSeconds(90));

                if (deleteRule)
                {
                    EwsWrapper.Instance().DeleteAllAclRules();
                }

                EwsWrapper.Instance().SetDefaultSnmpCommunityName();
            }
        }

        #endregion

        #region FIPS

        /// <summary>
        /// Verify install of ID(jet direct) certificate in FIPS mode[577014]
        ///1.	Enable FIPS on the device.
        ///2.	Click on Authorization->Import Certificate and private key and upload MD2/MD4/MD5 ID Certificate with Password.
        ///3.	Installation of ID Certificate should fail.
        ///4.	Click on Authorization->Import Certificate and private key and upload SHA1/SHA2 ID Certificate with Password
        ///5.	Installation of ID Certificate should pass.
        ///6.	Disable FIPS on the device.
        ///7.	Click on Authorization->Import Certificate and private key and upload MD2/MD4/MD5 ID Certificate with Password.
        ///8.	Installation of ID Certificate should pass in non-FIPS mode.
        ///9.	Click on Authorization->Import Certificate and private key and upload SHA1/SHA2 ID Certificate with Password
        ///10.	Installation of ID Certificate should pass.
        ///11.	Enable FIPS on the device.
        ///12.	Click on Jet-Direct Certificate configure button, select create certificate request option of 1k-SHA1 and click next button.
        ///13.	Enter Organization, city, state and country information and press next.
        ///14.	Go to certificate authority and submit the request.
        ///15.	Click configure jet direct button and select install certificate option.
        ///16.	Installation of certificate should pass.
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyInstallOfIDCertificateinFIPS(SecurityActivityData activityData)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, performFIPSPrerequisites: true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Step1 : Install MD5 ID Certificate when FIPS is Enabled");
                // upload MD5/MD4/MD2 ID Certificate with password
                if (EwsWrapper.Instance().InstallIDCertificate(IDCERTIFICATEPATH_MD5, IDCERTIFICATE_PSWD_MD5))
                {
                    return false;
                }
                //revert back before proceeding to next step
                EwsWrapper.Instance().SetFipsOption(false);

                // Since windows sha1 cert internally validates by md5,unable to install sha1 id certificate so
                // installing certificate first and tehn enabling fips-as discussed with Amitha 14/10/2015
                // upload SHA1/SHA2 ID Certificate with password
                TraceFactory.Logger.Info("Step2 : Install SHA1 ID Certificate when FIPS is Disabled");
                if (!EwsWrapper.Instance().InstallIDCertificate(IDCERTIFICATEPATH_SHA1, IDCERTIFICATE_PSWD_SHA1))
                {
                    return false;
                }

                EwsWrapper.Instance().SetFipsOption(true);

                // disable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(false))
                {
                    return false;
                }

                // upload MD5/MD4/MD2 ID Certificate with password
               TraceFactory.Logger.Info("Step3 : Install MD5 ID Certificate when FIPS is Disabled");
                if (!EwsWrapper.Instance().InstallIDCertificate(IDCERTIFICATEPATH_MD5, IDCERTIFICATE_PSWD_MD5))
                {
                    return false;
                }

                // upload SHA1/SHA2 ID Certificate with password
                TraceFactory.Logger.Info("Step4 : Install SHA1 ID Certificate when FIPS is Disabled");
                if (!EwsWrapper.Instance().InstallIDCertificate(IDCERTIFICATEPATH_SHA1, IDCERTIFICATE_PSWD_SHA1))
                {
                    return false;
                }

                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Step5 : Creating Self signed Certitificate Request when FIPS is Enabled");
                // create self signed certificate
                string certificateRequest = string.Empty;
                return EwsWrapper.Instance().CreateIDCertificateRequest(SignatureAlgorithm.Sha1, out certificateRequest, false);

            }
            catch
            {
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        /// Verify install of CA certificate in FIPS mode[577010]
        ///1.	Enable FIPS on the device.
        ///2.	Click on Authorization->Certificates->CA Certificate->Configure and upload MD2/MD4/MD5 CA Certificate.
        ///3.	Installation of CA Certificate should fail.
        ///4.	Click on Authorization->Certificates->CA Certificate->Configure and upload SHA1/SHA2 CA Certificate.
        ///5.	Installation of CA Certificate should pass.
        ///6.	Click on Authorization->Certificates->CA Certificate->Configure and upload MD2/MD4/MD5 intermediate CA Certificate with ‘Allow intermediate CA’ enabled.
        ///7.	Installation of CA Certificate should fail.
        ///8.	Disable FIPS in the device.
        ///9.	Click on Authorization->Certificates->CA Certificate->Configure and upload MD2/MD4/MD5 intermediate CA Certificate with ‘Allow intermediate CA’ enabled.
        ///10.	Installation of CA Certificate should pass.
        ///11.	Enable FIPS in the device.
        ///12.	Click on Authorization->Certificates->CA Certificate->Configure and upload SHA1/SHA2 intermediate CA Certificate with ‘Allow intermediate CA’ enabled.
        ///13.	Installation of CA Certificate should pass.
        ///14.	Disable FIPS in the device.
        ///15.	Click on Authorization->Certificates->CA Certificate->Configure and upload SHA1/SHA2 intermediate CA Certificate with ‘Allow intermediate CA’ enabled.
        ///16.	Installation of CA Certificate should pass.
        ///17.	Installation of certificate should pass.
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyInstallOfCACertificateinFIPS(SecurityActivityData activityData, bool isIPV4)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, performFIPSPrerequisites: true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                if (!isIPV4)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(printer.IPv6LinkLocalAddress);
                }
                //keeping a delay of 2 mins since ews page should open with link local address
                Thread.Sleep(TimeSpan.FromMinutes(2));
                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }

                // upload MD5/MD4/MD2 CA Certificate
                TraceFactory.Logger.Info("Step 1: Installing MD5 CA Certificate on Printer after FIPS Enabled");
                if (EwsWrapper.Instance().InstallCACertificate(CACERTIFICATEPATH_MD5, false))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Step2 : Installing SHA1 CA Certificate on Printer after FIPS Enabled");

                // upload SHA1/SHA2 CA Certificate
                if (!EwsWrapper.Instance().InstallCACertificate(CACERTIFICATEPATH_SHA1, false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step3 : Installing Intermediate MD5-CA Certificate on Printer after FIPS Enabled");
                // upload MD5/MD4/MD2 CA Certificate
                if (EwsWrapper.Instance().InstallCACertificate(INTERMEDIATECACERTIFICATE_MD5, true))
                {
                    return false;
                }

                // disable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(false))
                {
                    return false;
                }
                TraceFactory.Logger.Info(" Step 3 : Installing Intermediate MD5-CA Certificate on Printer after FIPS Disabled");

                // upload MD5/MD4/MD2 CA Certificate
                if (!EwsWrapper.Instance().InstallCACertificate(INTERMEDIATECACERTIFICATE_MD5, true))
                {
                    return false;
                }
                // deleting the CA certificate to proceed to the next step
                TraceFactory.Logger.Info("Uninstalling CA certificate before next step");
                EwsWrapper.Instance().UnInstallCACertificate();

                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Step 4 : Installing SHA1-CA Certificate on Printer after FIPS Enabled");

                // upload SHA1/SHA2 CA Certificate
                if (!EwsWrapper.Instance().InstallCACertificate(CACERTIFICATEPATH_SHA1, false))
                {
                    return false;
                }

                // disable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(false))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Step 4 : Installing SHA1-CA Certificate on Printer after FIPS Disabled");
                // upload SHA1/SHA2 CA Certificate
                return EwsWrapper.Instance().InstallCACertificate(CACERTIFICATEPATH_SHA1, false);
			}
			catch (Exception securityException)
			{
                TraceFactory.Logger.Info("Exception Occured :  {0}".FormatWith(securityException.Message));
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, deleteCACertificate: true);
                if (!isIPV4)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                }
            }
        }

        /// <summary>
        /// Verify EWS device access using HTTPS in FIPS mode[577071]
        ///1.	Enable FIPS on the device with the following settings:
        ///i.	TLS1.0 – Enable
        ///ii.	Encryption Strength – High
        ///iii.	SSL3.0 – Disable
        ///2.	Access EWS Page using HTTPS.
        ///3.	Should be able to open the EWS page using HTTPS.
        ///4.	Enable FIPS on the device with the following settings:
        ///i.	TLS1.1- Enable
        ///ii.	Encryption Strength – High
        ///iii.	SSl3.0- Disable
        ///5.	Access EWS Page using HTTPS.
        ///6.	Should be able to open the EWS page using HTTPS.
        ///7.	Enable FIPS on the device with the following settings:
        ///i.	TLS1.2 – Enable
        ///ii.	Encryption Strength – High
        ///iii.	SSL3.0 – Disable
        ///8.	Access EWS Page using HTTPS.
        ///9.	Should be able to open the EWS page using HTTPS.
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyEWSAccessUsingHTTPSinFIPS(SecurityActivityData activityData, bool isIPV4)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, performFIPSPrerequisites: true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!isIPV4)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(printer.IPv6LinkLocalAddress);
                }
                TraceFactory.Logger.Info("1. Setting FIPS TRUE");
                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }
                TraceFactory.Logger.Info("2. Setting TLS1.0 TRUE");

				// enable TLS 1.0 and encryption strength to high
				EwsWrapper.Instance().SetTLSOption("TLS1.0", true);				
                TraceFactory.Logger.Info("3. Setting Encryption HIGH ");
				EwsWrapper.Instance().SetEncryptionStrength(EncryptionStrengths.High);
                TraceFactory.Logger.Info("4. Validating HTTPS EWS Access ");

                // Accessing EWS Page using HTTPS should pass
                if (!printer.IsEwsAccessible("https"))
                {
                    return false;
                }
                TraceFactory.Logger.Info("5. Setting TLS1.1 TRUE");

                // enable TLS1.1
                EwsWrapper.Instance().SetTLSOption("TLS1.1", true);
                TraceFactory.Logger.Info("6. Validating HTTPS EWS Access ");
                // Accessing EWS Page using HTTPS after enabling TLS1.1 should pass
                if (!printer.IsEwsAccessible("https"))
                {
                    return false;
                }

                // enable TLS1.2
                TraceFactory.Logger.Info("6. Setting TLS1.2 TRUE");
                EwsWrapper.Instance().SetTLSOption("TLS1.2", true);

                // Accessing EWS Page using HTTPS after enabling TLS1.2 should pass
                TraceFactory.Logger.Info("6. Validating HTTPS EWS Access ");
                return printer.IsEwsAccessible("https");
            }
            catch
            {
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
                if (!isIPV4)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                }
            }
        }

        /// <summary>
        /// Verify device behavior in FIPS enable/disable mode[576697]
        /// 1.	Enable FIPS on the device of Web UI
        /// 2.	Should be able to enable FIPS and should not be able to enable SSL3.0 and to set encryption strength to Low in FIPS mode.
        /// 3.	Disable FIPS on the device through Web UI.
        /// 4.	Should be able to disable FIPS option.
        /// 5.	Enable FIPS on the device using SNMP OID.
        ///     npSecurityOpenSSLFIPSMode- 1.3.6.1.4.1.11.2.4.3.20.59 to 1
        /// 6.	Enabling FIPS through SNMP should be successful and same should reflect in Web-UI also.
        /// 7.	Disable FIPS on the device using SNMP OID.
        ///     npSecurityOpenSSLFIPSMode- 1.3.6.1.4.1.11.2.4.3.20.59 to 0
        /// 8.	Disabling FIPS through SNMP should be successful and same should reflect in Web-UI also.
        /// 9.	Enable FIPS on the device and validate the following settings:
        ///      i.	Encryption Strength – set to High
        ///      ii.	SSl3.0- Disable
        ///      iii.	TLS 1.1,TLS1.2,TLS1.0 – Enable
        ///      iv.	In SNMP Page, Low Ciphers DES and MD5- Disabled
        ///       v.	In Kerberos Page, Low Ciphers DES and MD5- Disabled
        ///      vi.	Installing CA/ID Certificate signed with MD2/MD5 should fail.
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyDeviceBehaviourinFIPSEnableDisableMode(SecurityActivityData activityData)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, performFIPSPrerequisites: true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }

                // disable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(false))
                {
                    return false;
                }

                // enabling FIPS in device using SNMP OID and validating whether the FIPS enabled through SNMP is reflecting in WebUI
                if (!SnmpWrapper.Instance().SetFIPS(true))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(30));
                if (!EwsWrapper.Instance().GetFipsOption())
                {
                    return false;
                }

                // disabling FIPS using SNMP and validating whether it is reflected in WebUI
                if (!SnmpWrapper.Instance().SetFIPS(false))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromSeconds(30));
                if (EwsWrapper.Instance().GetFipsOption())
                {
                    return false;
                }

                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }

				TraceFactory.Logger.Info("Validating the security parameters after enabling FIPS");
                string encryptionStrength = activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.LFP.ToString()) ? "low" : "high";

                return ((EwsWrapper.Instance().GetEncryptionStrength().EqualsIgnoreCase(encryptionStrength)) && (EwsWrapper.Instance().GetTLSOption("TLS1.0") == true) && 
					(EwsWrapper.Instance().GetTLSOption("TLS1.1") == true) && (EwsWrapper.Instance().GetTLSOption("TLS1.2") == true) && (EwsWrapper.Instance().ValidateSNMPPage()) &&
					(!EwsWrapper.Instance().InstallCACertificate(CACERTIFICATEPATH_MD5, false)));
				 
			}
			catch (Exception securityException)
			{
                TraceFactory.Logger.Debug(securityException.Message);
				return false;
			}
			finally
			{
				TestPostRequisites(activityData);
			}
		}

        /// <summary>
        /// Verify device UI Login using Administrator Password in FIPS mode[577413]
        ///1.	Navigate to Networking->authorization Page and set Administrator password.
        ///2.	Close the browser window and reopen it again.
        ///3.	Should be able to access the Web-UI using Administrator Password.
        ///4.	Enable FIPS on the device.
        ///5.	Should be able to access the Web-UI using previously set Administrator Password.
        ///6.	Disable FIPS on the device.
        ///7.	Should be able to access the Web-UI using previously set Administrator Password.
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyDeviceUILoginusingAdminPasswordinFIPS(SecurityActivityData activityData, bool isIPV4)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, performFIPSPrerequisites: true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!isIPV4)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(printer.IPv6LinkLocalAddress);
                }
                // set administrator password through webUI
                if (!EwsWrapper.Instance().SetAdminPassword("admin"))
                {
                    return false;
                }

                // validate WebUI access using Administrator Password, it should pass
                if (!EwsWrapper.Instance().Login("admin"))
                {
                    return false;
                }

                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }

                EwsWrapper.Instance().Stop();
                EwsWrapper.Instance().Start();

                // validate WebUI access using Administrator Password, it should pass
                if (!EwsWrapper.Instance().Login("admin"))
                {
                    return false;
                }

                // disable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(false))
                {
                    return false;
                }

                EwsWrapper.Instance().Stop();
                EwsWrapper.Instance().Start();

                // validate WebUI access using Administrator Password, it should pass
                return EwsWrapper.Instance().Login("admin");
            }
            catch
            {
                return false;
            }
            finally
            {
                EwsWrapper.Instance().Login("admin");
                //TODO: remove administrator password which has been set
                EwsWrapper.Instance().DeleteAdminPassword("admin");

                TestPostRequisites(activityData);

                if (!isIPV4)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                }
            }
        }

        /// <summary>
        /// Verify Reboot and Reset in FIPS mode[576699]
        ///1.	Enable FIPS on the device
        ///2.	Reboot the Printer and validate the following settings after reboot:
        ///     (i)	FIPS- In enable state.
        ///     (ii)	In SNMP Page: 
        ///1.	SSL3.0- should be in disable state
        ///2.	Low ciphers MD5- should be in disable state
        ///3.	Low encryption ciphers- should be in disable state
        ///3.	Disable FIPS on the device.
        ///4.	Reboot the Printer and validate the following settings after reboot:
        ///     (i)	FIPS- In disable state.
        ///     (ii)	In SNMP Page: 
        ///1.	SSL3.0- should be in disable state
        ///2.	Low ciphers MD5- should be in enable state
        ///3.	Low encryption ciphers- should be in enable state
        ///5.	Enable FIPS on the device.
        ///6.	Cold Reset the Printer and validate the following settings after reboot:
        ///         (i)	FIPS- In disable state.
        ///         (ii)	In SNMP Page: 
        ///     1.	SSL3.0- should be in enable state
        ///     2.	Low ciphers MD5- should be in enable state
        ///7.	Disable FIPS on the device.
        ///8.	Cold Reset the Printer and validate the following settings after reboot:
        ///         (i)	FIPS- In disabled state.
        ///         (ii)	In SNMP Page: 
        ///     1.	SSL3.0- should be in enable state
        ///     2.	Low ciphers MD5- should be in enable state
        ///     3.	Low encryption ciphers- should be in enable state
        ///9.	Enable FIPS on the device.
        ///10.	Do Reset Security from Control Panel and validate the following settings:
        ///         (i)	FIPS- In disabled state.
        ///         (ii)	Encryption Strength- Low
        ///         (iii)	In SNMP Page: 
        ///     1.	SSL3.0- should be in enable state
        ///     2.	Low ciphers MD5- should be in enable state
        ///     3.	Low encryption ciphers- should be in enable state
        ///11.	Do Reset Security from SNMP and validate the following settings:
        ///     SNMP OID:1.3.6.1.4.1.11.2.4.3.20.24
        ///         (i)	FIPS- In disabled state.
        ///         (ii)	Encryption Strength- Low
        ///         (iii)	In SNMP Page: 
        ///     1.	SSL3.0- should be in enable state
        ///     2.	Low ciphers MD5- should be in enable state
        ///     3.	Low encryption ciphers- should be in enable state
        ///12.	Do Restore Defaults from Web-UI and validate the following settings:
        ///         (i)	FIPS- In disabled state.
        ///         (ii)	Encryption Strength- Low
        ///         (iii)	In SNMP Page: 
        ///     1.	SSL3.0- should be in enable state
        ///     2.	Low ciphers MD5- should be in enable state
        ///     3.	Low encryption ciphers- should be in enable state
        ///13.	Do Restore Defaults from 802.1x page and validate the following settings:
        ///         (i)	FIPS- In Enabled state.
        ///         (ii)	Encryption Strength- High
        ///         (iii)	In SNMP Page: 
        ///     1.	SSL3.0- should be in disable state
        ///     Note: For LFP the step reset security through control panel has been covered in other instance so please omit that step-Chintu
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyRebootResetinFIPS(SecurityActivityData activityData)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, performFIPSPrerequisites: true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }
                // reboot the printer
                printer.PowerCycle();

                Thread.Sleep(TimeSpan.FromMinutes(2));

                TraceFactory.Logger.Info("Validating the security parameters after reboot");
                if (!((EwsWrapper.Instance().GetFipsOption()) && (EwsWrapper.Instance().ValidateSNMPPage())))
                {
                    return false;
                }

                // disable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(false))
                {
                    return false;
                }

                // reboot the printer
                printer.PowerCycle();

                Thread.Sleep(TimeSpan.FromMinutes(2));

                TraceFactory.Logger.Info("Validating the security parameters after reboot");
                if (!(!(EwsWrapper.Instance().GetFipsOption()) && (!EwsWrapper.Instance().ValidateSNMPPage())))
                {
                    return false;
                }

                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }
                try
                {
                    printer.ColdReset();
                }
                catch(Exception ex)
                {

                    TraceFactory.Logger.Info("Exception occured while Cold restting : {0}".FormatWith(ex));
                }
                Thread.Sleep(TimeSpan.FromMinutes(2));

                TraceFactory.Logger.Info("Validating the security parameters after cold reset");
                if (!(!(EwsWrapper.Instance().GetFipsOption()) && (!EwsWrapper.Instance().ValidateSNMPPage())))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetFipsOption(false))
                {
                    return false;
                }
                try
                {
                    printer.ColdReset();
                }
                catch (Exception ex)
                {

                    TraceFactory.Logger.Info("Exception occured while Cold restting : {0}".FormatWith(ex));
                }
                Thread.Sleep(TimeSpan.FromMinutes(2));

                TraceFactory.Logger.Info("Validating the security parameters after cold reset");
                if (!(!(EwsWrapper.Instance().GetFipsOption()) && (!EwsWrapper.Instance().ValidateSNMPPage())))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify Interdependency check from WebUI and SNMP in FIPS[576693]
        /// 1.	Perform the following operations on the device and enable FIPS by Web UI after completion of each step and validate.
        ///     (i)	Install CA Certificate on the device with MD5/MD4
        ///     (ii)	Install intermediate CA Certificate on the device with MD5/MD4
        ///     (iii)	Install an ID Certificate on the device with MD5/MD4
        ///     (iv)	Configure Kerberos with MD5-DES
        ///     (v)	Enable SNMPV3 on the device with low ciphers MD5/DES
        ///     (vi)	Enable SSL3.0 explicitly on the device.
        ///2.	Enabling FIPS option will fail with error “Invalid Cryptographic Algorithm configured” after each above operation.
        ///3.	Perform the following operations on the device and enable FIPS by SNMP OID after completion of each step and validate. OID to set FIPS: npSecurityOpenSSLFIPSMode(1.3.6.1.4.1.11.2.4.3.20.59) to 1
        ///      (i)	Install CA Certificate on the device with MD5/MD4
        ///     (ii)	Install intermediate CA Certificate on the device with MD5/MD4
        ///     (iii)	Install an ID Certificate on the device with MD5/MD4
        ///     (iv)	Configure Kerberos with MD5-DES
        ///     (v)	Enable SNMPV3 on the device with low ciphers MD5/DES
        ///     (vi)	Enable SSL3.0 explicitly on the device.
        ///4.	Enabling FIPS will fail.
        ///5.	SNMP Get on OID will show error code ”ID Certificate has non FIPS Algorithm
        ///       npSecurityOpenSSLFIPSErorTable(1.3.6.1.4.1.11.2.4.3.20.60)
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyInterdependencyCheckfromWebUIandSNMPinFIPS(SecurityActivityData activityData)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, performFIPSPrerequisites: true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("1. Validating the FIPS Failure when CA certificate with MD5 installed");
                if (!((EwsWrapper.Instance().InstallCACertificate(CACERTIFICATEPATH_MD5, false)) && (!EwsWrapper.Instance().SetFipsOption(true))))
                {
                    TraceFactory.Logger.Info("Failed : MD5-CA Certificate Installed and FIPS is Enabled");
                    return false;
                }

                TraceFactory.Logger.Info("2. Validating the FIPS Failure when intermediate CA certificate with MD5 installed");
                if (!((EwsWrapper.Instance().InstallCACertificate(INTERMEDIATECACERTIFICATE_MD5, true)) && (!EwsWrapper.Instance().SetFipsOption(true))))
                {
                    TraceFactory.Logger.Info("Failed : MD5-Intermidiate Certificate Installed and FIPS is Enabled");
                    return false;
                }

                TraceFactory.Logger.Info("3. Validating the FIPS Failure when ID certificate with MD5 installed");
                if (!((EwsWrapper.Instance().InstallIDCertificate(IDCERTIFICATEPATH_MD5, IDCERTIFICATE_PSWD_MD5)) && (!EwsWrapper.Instance().SetFipsOption(true))))
                {
                    TraceFactory.Logger.Info("Failed : MD5-ID Certificate Installed and FIPS is Enabled");
                    return false;
                }

                TraceFactory.Logger.Info("4. Validating the FIPS Failure when MD5 in SNMP Page enabled");
                if (!((EwsWrapper.Instance().SetSNMPV3AuthenticationProtocol("MD5", "admin", "admin123", "admin123", true)) && (!EwsWrapper.Instance().SetFipsOption(true))))
                {
                    TraceFactory.Logger.Info("Failed : MD5 is Enabled in SNMP and FIPS is Enabled");
                    return false;
                }

                TraceFactory.Logger.Info("5. Validating the FIPS Failure through SNMP OID when CA certificate with MD5 installed");
                if (!((EwsWrapper.Instance().InstallCACertificate(CACERTIFICATEPATH_MD5, false)) && (!SnmpWrapper.Instance().SetFIPS(true))))
                {
                    TraceFactory.Logger.Info("Failed : MD5-CA Certificate Installed and FIPS is Enabled using SNMP");
                    return false;
                }

                TraceFactory.Logger.Info("6. Validating the FIPS Failure through SNMP OID when intermediate CA certificate with MD5 installed");
                if (!((EwsWrapper.Instance().InstallCACertificate(INTERMEDIATECACERTIFICATE_MD5, true)) && (!SnmpWrapper.Instance().SetFIPS(true))))
                {
                    TraceFactory.Logger.Info("Failed : MD5-Intermediate Certificate Installed and FIPS is Enabled using SNMP");
                    return false;
                }

                TraceFactory.Logger.Info("7. Validating the FIPS Failure through SNMP OID when ID certificate with MD5 installed");
                if (!((EwsWrapper.Instance().InstallIDCertificate(IDCERTIFICATEPATH_MD5, IDCERTIFICATE_PSWD_MD5)) && (!SnmpWrapper.Instance().SetFIPS(true))))
                {
                    TraceFactory.Logger.Info("Failed : MD5-ID Certificate Installed and FIPS is Enabled using SNMP");
                    return false;
                }

                TraceFactory.Logger.Info("8 .Validating the FIPS Failure through SNMP OID when MD5 in SNMP Page enabled");
                if (!((EwsWrapper.Instance().SetSNMPV3AuthenticationProtocol("MD5", "admin", "admin123", "admin123", true)) && (!SnmpWrapper.Instance().SetFIPS(true))))
                {
                    TraceFactory.Logger.Info("Failed : MD5-SNMP Enabled and FIPS is Enabled");
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                EwsWrapper.Instance().SetSNMPV3AuthenticationProtocol("SHA1", "admin", "admin123", "admin123", false);
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify Firmware Upgrade and Downgrade in FIPS mode[579476]
        ///1.	Go to Management Protocols menu.
        ///2.	Check "Encrypt All Web Communication" check box and press apply.
        ///3.	Enable FIPS in management protocol menu.
        ///4.	Now go to other settings menu and click firmware upgrade tab.
        ///5.	Now click on browse button and search the firmware file to upgrade and press open file and press apply button.
        ///6.	Once the device is ready check for the firmware of the device.
        ///7.	FIPS should be in enabled mode and firmware should be upgraded.
        ///8.	Go to Management Protocols menu.
        ///9.	Check "Encrypt All Web Communication" check box and press apply.
        ///10.	Enable FIPS in management protocol menu.
        ///11.	Now go to other settings menu and click firmware upgrade tab.
        ///12.	Now click on browse button and search the firmware file to upgrade and press open file and press apply button.
        ///13.	Once the device is ready check for the firmware of the device.
        ///14.	FIPS should be in disabled mode and firmware should be downgraded.
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyFirmwareUpgradeandDowngradeinFIPS(SecurityActivityData activityData)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, performFIPSPrerequisites: true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }

                string firmwareUpgradeFilePath = FIRMWAREBASELOCATION + Path.DirectorySeparatorChar + activityData.ProductFamily + Path.DirectorySeparatorChar + activityData.ProductName + Path.DirectorySeparatorChar + "UpgradeFile";
                string firmwareDowngradeFilePath = FIRMWAREBASELOCATION + Path.DirectorySeparatorChar + activityData.ProductFamily + Path.DirectorySeparatorChar + activityData.ProductName + Path.DirectorySeparatorChar + "DowngradeFile";
                DirectoryInfo upgradeFirmwareDir = new DirectoryInfo(firmwareUpgradeFilePath);
                DirectoryInfo downgradeFirmwareDir = new DirectoryInfo(firmwareDowngradeFilePath);

                TraceFactory.Logger.Info("Firmware UPgrade File Path : {0}".FormatWith(firmwareUpgradeFilePath));
                TraceFactory.Logger.Info("Firmware Downgrade File Path : {0}".FormatWith(firmwareDowngradeFilePath));
                // downgrade and validate
                if (!EwsWrapper.Instance().InstallFirmware(downgradeFirmwareDir.GetFiles()[0].FullName))
                {
                    return false;
                }

                // validate FIPS and firmware version
                if (!(!EwsWrapper.Instance().GetFipsOption() && EwsWrapper.Instance().ValidateFirmware(Path.GetFileNameWithoutExtension(downgradeFirmwareDir.GetFiles()[0].ToString()))))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }

                // upgrade and validate
                if (!EwsWrapper.Instance().InstallFirmware(upgradeFirmwareDir.GetFiles()[0].FullName))
                {
                    return false;
                }

                // validate FIPS and firmware version
                return (EwsWrapper.Instance().GetFipsOption() && EwsWrapper.Instance().ValidateFirmware(Path.GetFileNameWithoutExtension(upgradeFirmwareDir.GetFiles()[0].ToString())));
            }
            catch
            {
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
                EwsWrapper.Instance().SetTLSOption("TLS1.2", true);
                EwsWrapper.Instance().SetTLSOption("TLS1.1", true);
            }
        }

        /// <summary>
        /// Verify FIPS status across interfaces[678828]
        ///1.	Enable FIPS on Wired interface.
        ///2.	Verify FIPS status on other interface.
        ///3.	FIPS should be in enabled state in all other interfaces.
        ///4.	Configure SNMPV3 account with non FIPS compliant algorithm MD5/DES in one interface.
        ///5.	Enable FIPS from other interface.
        ///6.	Web UI should throw an error while enabling FIPS in other interface, since snmpv3 is configured with non FIPS compliant algorithm.
        ///7.	Configure IPsec with Kerberos with non FIPS compliant algorithm MD5/DES on one interface.
        ///8.	Enable FIPS from other interface.
        ///9.	Web UI should throw an error since Kerberos is configured with non FIPS complaint algorithm.
        ///10.	Enable FIPS on one interface.
        ///11.	Verify Encryption strength across other interfaces.
        ///12.	FIPS should be enabled across all other interfaces and encryption strength should set to high across all other interfaces.
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyFIPSAcrossInterfaces(SecurityActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, true, performFIPSPrerequisites: true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Note: There is a problem in Automation while validating the Fips in second interface, so testing viceversa
                TraceFactory.Logger.Info("Enable FIPS in second interface");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondaryWiredIPv4Address);
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validate FIPS enabled in the first interface");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                if (!EwsWrapper.Instance().GetFipsOption())
                {
                    return false;
                }
                //reverting back the FIPS status to proceed to next step
                EwsWrapper.Instance().SetFipsOption(false);

                TraceFactory.Logger.Info("Configuring SNMPV3 with MD5 in second interface");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondaryWiredIPv4Address);
                EwsWrapper.Instance().SetSNMPV3AuthenticationProtocol("MD5", "admin", "admin123", "admin123", true);

                TraceFactory.Logger.Info("Enabling FIPS in first interface should fail since SNMPV3 MD5 is configured in second interface");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                if (EwsWrapper.Instance().SetFipsOption(true))
                {
                    return true;
                }
                // revert back from MD5
                EwsWrapper.Instance().SetSNMPV3AuthenticationProtocol("SHA1", "admin", "admin123", "admin123", true);

                TraceFactory.Logger.Info("Configuring IPSec with Kerberos-MD5 in first interface");
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);
                KerberosImportSettings importSettings = new KerberosImportSettings(KERBEROS_CONFIGFILE.FormatWith(activityData.ProductFamily), KERBEROS_DES);
                KerberosSettings kerberos = new KerberosSettings(importSettings);

                SecurityRuleSettings settings = CtcUtility.GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity);
                try
                {
                    EwsWrapper.Instance().CreateRule(settings, true);
                }
                catch
                {
                    TraceFactory.Logger.Info("Failed to authenticate Kerberos in FIPs mode with DES");
                }

                TraceFactory.Logger.Info("Enabling FIPS in second interface should fail since IPSec Kerberos with MD5 is configured in first interface");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondaryWiredIPv4Address);
                if (EwsWrapper.Instance().SetFipsOption(true))
                {
                    return true;
                }

                TraceFactory.Logger.Info("Enable FIPS in second interface");
                EwsWrapper.Instance().SetFipsOption(true);

                TraceFactory.Logger.Info("Verifying encryption strength in first interface");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                return EwsWrapper.Instance().GetEncryptionStrength().Equals("high");
            }
            catch
            {
                return false;
            }
            finally
            {
                // disable SNMPV3 option
                EwsWrapper.Instance().SetSNMPV3AuthenticationProtocol("SHA1", "admin", "admin123", "admin123", false);

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                TestPostRequisites(activityData, deleteRules: true);
                EwsWrapper.Instance().ClearKerberosIPsecConfiguration();
            }
        }

        /// <summary>
        /// Verify FIPS on Multiple JDI Networks[577009]
        /// 1.	Enable FIPS and SNMP on the device from WebUI in wireless interface.
        ///2.	Perform the basic operation such as save to network folder from wired interface.
        ///3.	The job should be successful.
        ///4.	Enable FIPS and SNMP on the device from WebUI in wired interface.
        ///5.	Perform the basic operation such as save to network folder from wired interface.
        ///6.	The job should be successful.
        ///7.	Enable FIPS and SNMP on the device from WebUI in wireless interface.
        ///8.	Perform the basic operation such as save to network folder from wireless interface.
        ///9.	The job should be successful.
        ///10.	 Enable FIPS and SNMP on the device from WebUI in wireless and wired interface.
        ///11.	FIPS should be in enabled mode.
        ///12.	Enable FIPS on Wired and Wireless interface.
        ///13.	Access Web UI of the device, go to security tab.
        ///14.	Enter password and reenter the same password, verify and apply.
        ///15.	Open the EWS page of external jet direct connected. Enter user name and password.
        ///16.	Should be able to open EWS page successfully with user name and password.
        ///17.	Telnet to device.
        ///18.	Able to connect to the printer through telnet successfully.
        ///19.	Upgrade the wired interface with FIPS firmware
        ///20.	Validate whether the FIPS is enabled in the device after firmware upgrade in wired interface.
        ///21.	FIPS should be in enabled state.
        ///22.	Upgrade the wireless interface with FIPS firmware
        ///23.	Validate whether the FIPS is enabled in the device after firmware upgrade in wireless interface.
        ///24.	FIPS should be in enabled state.
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyFIPSonMultipleJDINetworks(SecurityActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, true, true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string firmwareUpgradeFilePath = FIRMWAREBASELOCATION + Path.DirectorySeparatorChar + activityData.ProductFamily + Path.DirectorySeparatorChar + activityData.ProductName + Path.DirectorySeparatorChar + "UpgradeFile";
                DirectoryInfo upgradeFirmwareDir = new DirectoryInfo(firmwareUpgradeFilePath);

                TraceFactory.Logger.Info("Enabling FIPS in Wireless interface");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WirelessIPv4Address);
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Performing scan to network folder from wired interface and the job should be success");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                if (!CtcUtility.ScanToNetworkFolder(IPAddress.Parse(activityData.WiredIPv4Address), QUICK_SET_NAME, SHARE_FILE_PATH, testNo))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Enabling FIPS in Wired interface");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Performing scan to network folder from wired interface and the job should be success");
                if (!CtcUtility.ScanToNetworkFolder(IPAddress.Parse(activityData.WiredIPv4Address), QUICK_SET_NAME, SHARE_FILE_PATH, testNo))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Enabling FIPS in Wireless interface");
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Performing scan to network folder from wireless interface and the job should be success");
                if (!CtcUtility.ScanToNetworkFolder(IPAddress.Parse(activityData.WiredIPv4Address), QUICK_SET_NAME, SHARE_FILE_PATH, testNo))
                {
                    return false;
                }

                EwsWrapper.Instance().SetFipsOption(false);
                TraceFactory.Logger.Info("Enable Login with administrator password in wired interface");
                EwsWrapper.Instance().SetAdminPassword("admin");

                TraceFactory.Logger.Info("Validate the access of EWS Page from wireless interface with administrator password");
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WirelessIPv4Address);
                EwsWrapper.Instance().Stop();
                EwsWrapper.Instance().Start();
                if (!EwsWrapper.Instance().Login("admin"))
                {
                    return false;
                }

                EwsWrapper.Instance().DeleteAdminPassword("admin");

                TraceFactory.Logger.Info("Validating the Telnet access");
                if (!printer.IsTelnetAccessible())
                {
                    return false;
                }

                // upgrade and validate
                if (!EwsWrapper.Instance().InstallFirmware(upgradeFirmwareDir.GetFiles()[0].FullName))
                {
                    return false;
                }

                // validate FIPS and firmware version
                if (EwsWrapper.Instance().GetFipsOption())
                {
                    return false;
                }

                if (!EwsWrapper.Instance().ValidateFirmware((Path.GetFileNameWithoutExtension(upgradeFirmwareDir.GetFiles()[0].ToString()))))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                // upgrade and validate
                if (!EwsWrapper.Instance().InstallFirmware(upgradeFirmwareDir.GetFiles()[0].FullName))
                {
                    return false;
                }

                // validate FIPS and firmware version
                // validate FIPS and firmware version
                if (EwsWrapper.Instance().GetFipsOption())
                {
                    return false;
                }

                return EwsWrapper.Instance().ValidateFirmware((Path.GetFileNameWithoutExtension(upgradeFirmwareDir.GetFiles()[0].ToString())));
            }
            catch
            {
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        #endregion

        #region Password

        public static bool VerifyPasswordAsSetcommunityname(SecurityActivityData activityData)
        {
            try
            {
                if (!TestPreRequisites(activityData))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDefaultSnmpCommunityName())
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Setting admin password from Web UI and checking in SNMP.");

                if (!EwsWrapper.Instance().SetAdminPassword(ADMINPASSWORD, setPasswordAsCommunityName: true))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().Login(ADMINPASSWORD))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().isCommunityNameSet(SNMPCommunity.Set))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                if (!CtcUtility.ValidateDeviceServices(printer, snmpSet: DeviceServiceState.Pass, snmpSetCommunityName: ADMINPASSWORD))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().DeleteAdminPassword(ADMINPASSWORD))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Setting admin password from SNMP and checking in Web UI.");

                if (!SnmpWrapper.Instance().SetAdminPassword(ADMINPASSWORD))
                {
                    return false;
                }

                if (!SnmpWrapper.Instance().SetAdminPasswordAsSetCommunityName())
                {
                    return false;
                }

                TraceFactory.Logger.Info("Checking SNMP set without setting the community name.");

                if (!CtcUtility.ValidateDeviceServices(printer, snmpSet: DeviceServiceState.Fail))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Checking SNMP set after setting the community name.");

                if (!CtcUtility.ValidateDeviceServices(printer, snmpSet: DeviceServiceState.Pass, snmpSetCommunityName: ADMINPASSWORD))
                {
                    return false;
                }

                if (EwsWrapper.Instance().Login(ADMINPASSWORD))
                {
                    TraceFactory.Logger.Info("Admin password is available in Web UI.");
                }
                else
                {
                    TraceFactory.Logger.Info("Admin password is not configured in Web UI.");
                    return false;
                }

                if (EwsWrapper.Instance().isCommunityNameSet(SNMPCommunity.Set))
                {
                    TraceFactory.Logger.Info("SNMP set community name is configured in Web UI.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("SNMP set community name is not configured in Web UI.");
                    return false;
                }
            }
            finally
            {
                PostRequisiteAdminPassword(ADMINPASSWORD);
            }
        }

        private static void PostRequisiteAdminPassword(string password)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
            EwsWrapper.Instance().Login(password);
            EwsWrapper.Instance().DeleteAdminPassword(password);
            //EwsWrapper.Instance().SetDefaultSnmpCommunityName();
        }

        public static bool VerifyPasswordInNetworkingPeripheral(SecurityActivityData activityData)
        {
            try
            {
                if (!TestPreRequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetAdminPassword(ADMINPASSWORD))
                {
                    return false;
                }

                return EwsWrapper.Instance().ValidateLogin(ADMINPASSWORD);
            }
            finally
            {
                PostRequisiteAdminPassword(ADMINPASSWORD);
            }
        }

        public static bool VerifyPasswordInNetworkingPeripheralTelnet(SecurityActivityData activityData)
        {
            try
            {
                if (!TestPreRequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetAdminPassword(ADMINPASSWORD))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().Login(ADMINPASSWORD))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                return CtcUtility.CheckTelnetPasswordPrompt(Enum<PrinterFamilies>.Parse(activityData.ProductFamily), activityData.WiredIPv4Address, ADMINPASSWORD, ADMINPASSWORD);
            }
            finally
            {
                PostRequisiteAdminPassword(ADMINPASSWORD);
            }
        }

        public static bool VerifyPasswordSynchronization(SecurityActivityData activityData, string password = "", bool deletePassword = true)
        {
            password = string.IsNullOrEmpty(password) ? ADMINPASSWORD : password;

            try
            {
                if (!TestPreRequisites(activityData, true))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Setting admin password on embedded jet direct interface.");

                if (!EwsWrapper.Instance().SetAdminPassword(password))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().Login(password))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().ValidateHomeScreenPages(true))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating admin password on europa wired interface.");

                if (!ValidateExternalInterfacePasswordSetStatus(activityData, activityData.SecondaryWiredIPv4Address, password))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating admin password on europa wireless interface.");

                return ValidateExternalInterfacePasswordSetStatus(activityData, activityData.WirelessIPv4Address, password);
            }
            finally
            {
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                if (deletePassword)
                {
                    PostRequisiteAdminPassword(password);
                }
            }
        }

        public static bool VerifyPasswordSynchronizationPowercycle(SecurityActivityData activityData)
        {
            INetworkSwitch networkSwitch = null;

            try
            {
                if (!VerifyPasswordSynchronization(activityData, ADMINPASSWORD, deletePassword: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Deleting admin password on embedded jet direct interface.");

                if (!EwsWrapper.Instance().Login(ADMINPASSWORD))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().DeleteAdminPassword(ADMINPASSWORD))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondaryWiredIPv4Address);

                TraceFactory.Logger.Info("Validating if password is deleted from Europa wired interface.");

                if (EwsWrapper.Instance().ValidateHomeScreenPages(true, false))
                {
                    TraceFactory.Logger.Info("Password is deleted from europa wired interface.");
                }
                else
                {
                    TraceFactory.Logger.Info("Password is not deleted from europa wired interface.");
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WirelessIPv4Address);

                TraceFactory.Logger.Info("Validating if password is deleted from Europa wireless interface.");

                if (EwsWrapper.Instance().ValidateHomeScreenPages(true, false))
                {
                    TraceFactory.Logger.Info("Password is deleted from europa wired interface.");
                }
                else
                {
                    TraceFactory.Logger.Info("Password is not deleted from europa wired interface.");
                    return false;
                }

                TraceFactory.Logger.Info("Step III: Check the password status after power cycle");

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

                if (!EwsWrapper.Instance().SetAdminPassword(ADMINPASSWORD))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                printer.PowerCycle();

                TraceFactory.Logger.Info("Validating password status after power cycle.");

                if (EwsWrapper.Instance().ValidateHomeScreenPages(false, true))
                {
                    TraceFactory.Logger.Info("Password is retained after power cycle.");
                }
                else
                {
                    TraceFactory.Logger.Info("Password is not retained after power cycle.");
                    return false;
                }

                EwsWrapper.Instance().Login(ADMINPASSWORD);

                EwsWrapper.Instance().DeleteAdminPassword(ADMINPASSWORD);

                TraceFactory.Logger.Info("Step IV: Bring down external interfaces of the printer.");

                networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIPAddress));

                if (!networkSwitch.DisablePort(activityData.SecondaryWiredPortNo))
                {
                    return false;
                }

                if (!networkSwitch.DisablePort(activityData.WirelessPortNo))
                {
                    return false;
                }

                if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.SecondaryWiredIPv4Address), TimeSpan.FromMinutes(2)))
                {
                    TraceFactory.Logger.Info("Ping failed with ip address: {0}. The interface with ip address: {0} is down.".FormatWith(activityData.SecondaryWiredIPv4Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to bring down the interface with ip address: {0}.".FormatWith(activityData.SecondaryWiredIPv4Address));
                    return false;
                }

                if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WirelessIPv4Address), TimeSpan.FromMinutes(2)))
                {
                    TraceFactory.Logger.Info("Ping failed with ip address: {0}. The interface with ip address: {0} is down.".FormatWith(activityData.WirelessIPv4Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to bring down the interface with ip address: {0}.".FormatWith(activityData.WirelessIPv4Address));
                    return false;
                }

                if (!EwsWrapper.Instance().SetAdminPassword(ADMINPASSWORD))
                {
                    return false;
                }

                if (!networkSwitch.EnablePort(activityData.SecondaryWiredPortNo))
                {
                    return false;
                }

                if (!networkSwitch.EnablePort(activityData.WirelessPortNo))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating admin password on europa wired interface.");

                if (!ValidateExternalInterfacePasswordSetStatus(activityData, activityData.SecondaryWiredIPv4Address, ADMINPASSWORD))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating admin password on europa wireless interface.");

                return ValidateExternalInterfacePasswordSetStatus(activityData, activityData.WirelessIPv4Address, ADMINPASSWORD);
            }
            finally
            {
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                if (null != networkSwitch)
                {
                    networkSwitch.EnablePort(activityData.SecondaryWiredPortNo);
                    networkSwitch.EnablePort(activityData.WirelessPortNo);
                }

                PostRequisiteAdminPassword(ADMINPASSWORD);
            }
        }

        private static bool ValidateExternalInterfacePasswordSetStatus(SecurityActivityData activityData, string ipAddress, string password)
        {
            EwsWrapper.Instance().ChangeDeviceAddress(ipAddress);

            if (EwsWrapper.Instance().ValidateHomeScreenPages(false, true))
            {
                TraceFactory.Logger.Info("Password is set for printer interface IP address: {0}".FormatWith(ipAddress));
            }
            else
            {
                TraceFactory.Logger.Info("Password is not set for printer interface IP address: {0}".FormatWith(ipAddress));
                return false;
            }

            if (!EwsWrapper.Instance().Login(password))
            {
                return false;
            }

            if (!EwsWrapper.Instance().ValidateHomeScreenPages(true))
            {
                return false;
            }

            return CtcUtility.CheckTelnetPasswordPrompt(Enum<PrinterFamilies>.Parse(activityData.ProductFamily), ipAddress, password, password);
        }

        #endregion

        #region Web Encryption

        /// <summary>
        /// Verifies communication in different protocol options between Printer and Browser
        /// Eg: TLS 1.0 on Printer and TLS 1.0 on Browser
        ///     TLS 1.0 on Printer and TLS 1.1 on Browser
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        /// <returns>Test result</returns>
        public static bool VerifySecureProtocolOptions(SecurityActivityData activityData)
        {
            bool result = true;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                EwsWrapper.Instance().StopAdapter();
                EwsWrapper.Instance().Create(family, activityData.ProductName, activityData.WiredIPv4Address, activityData.SitemapsVersion, BrowserModel.Chrome);
                EwsWrapper.Instance().Start();

                // Enable All Web Encryption option before testing Secure Protocols
                TraceFactory.Logger.Info("Enable Encrypt All Web Communication option");
                EwsWrapper.Instance().SetEncryptWebCommunication(true);

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Setup 1: Verifying Secure Protocol option TLS 1.0 (Printer) - TLS 1.0 (Browser)");
                result &= VerifySecureProtocolOption(activityData, SecureProtocol.TLS10, SecureProtocol.TLS10);

                IEBrowserSettingsAutomation.SetSecureProtocols(SecureProtocol.AllTLS);

                TraceFactory.Logger.Info("Setup 2: Verifying Secure Protocol option TLS 1.0 (Printer) - TLS 1.1 (Browser)");
                result &= VerifySecureProtocolOption(activityData, SecureProtocol.TLS10, SecureProtocol.TLS11);

                IEBrowserSettingsAutomation.SetSecureProtocols(SecureProtocol.AllTLS);

                TraceFactory.Logger.Info("Setup 3: Verifying Secure Protocol option TLS 1.1 (Printer) - TLS 1.1 (Browser)");
                result &= VerifySecureProtocolOption(activityData, SecureProtocol.TLS11, SecureProtocol.TLS11);

                IEBrowserSettingsAutomation.SetSecureProtocols(SecureProtocol.AllTLS);

                TraceFactory.Logger.Info("Setup 4: Verifying Secure Protocol option TLS 1.1 (Printer) - TLS 1.2 (Browser)");
                result &= VerifySecureProtocolOption(activityData, SecureProtocol.TLS11, SecureProtocol.TLS12);

                IEBrowserSettingsAutomation.SetSecureProtocols(SecureProtocol.AllTLS);

                TraceFactory.Logger.Info("Setup 5: Verifying Secure Protocol option TLS 1.2 (Printer) - TLS 1.2 (Browser)");
                result &= VerifySecureProtocolOption(activityData, SecureProtocol.TLS12, SecureProtocol.TLS12);

                IEBrowserSettingsAutomation.SetSecureProtocols(SecureProtocol.AllTLS);

                TraceFactory.Logger.Info("Setup 6: Verifying Secure Protocol option TLS 1.2 (Printer) - TLS 1.0 (Browser)");
                result &= VerifySecureProtocolOption(activityData, SecureProtocol.TLS12, SecureProtocol.TLS10);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex.Message);
                result = false;
            }
            finally
            {
                WebEncryptionPostRequiresites(activityData);

                EwsWrapper.Instance().StopAdapter();
                EwsWrapper.Instance().Create(Enum<PrinterFamilies>.Parse(activityData.ProductFamily), activityData.ProductName, activityData.WiredIPv4Address, activityData.SitemapsVersion, BrowserModel.Firefox);
                EwsWrapper.Instance().Start();
            }

            return result;
        }

        /// <summary>
        /// Verifies secure protocol option with the given settings
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        /// <param name="printerProtocol">Secure Protocol on the Printer side</param>
        /// <param name="browserProtocol">Secure Protocol on the Browser side</param>
        /// <returns>Returns connection result</returns>
		public static bool VerifySecureProtocolOption(SecurityActivityData activityData, SecureProtocol printerProtocol, SecureProtocol browserProtocol)
        {
            TraceFactory.Logger.Info("There are no Pre-Requiresites");
            // 1. Set secure protocol on the printer
            TraceFactory.Logger.Info("Setting {0} protocol on printer".FormatWith(printerProtocol));
            EwsWrapper.Instance().SetSecureProtocol(printerProtocol);

            // 2. Set secure protocol on the browser
            TraceFactory.Logger.Info("Setting {0} protocol on Browser".FormatWith(browserProtocol));
            IEBrowserSettingsAutomation.SetSecureProtocols(browserProtocol);

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            bool connection = printer.IsEwsAccessible(activityData.WiredIPv4Address, "http", BrowserModel.Explorer);

            return printerProtocol == browserProtocol ? connection : !connection;
        }

        /// <summary>
        /// Verifies different Web Encryption Strengths
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        /// <returns>Returns test result</returns>
        public static bool VerifyWebEncryptionStrengths(SecurityActivityData activityData)
        {
            bool result = true;

            try
            {
                TraceFactory.Logger.Info("There are no Pre-Requiresites");
                TraceFactory.Logger.Info("Step 1: Verifying Web Encryption Strength in Low");
                if (!EwsWrapper.Instance().IsOmniOpus)
                {
                    result &= VerifyWebEncryptionStrength(activityData, EncryptionStrengths.Low);
                }
                else
                {
                    TraceFactory.Logger.Info("Encryption strength Low is not supported on Omni/Opus.");
                }

                TraceFactory.Logger.Info("Step 2: Verifying Web Encryption Strength in Medium");
                result &= VerifyWebEncryptionStrength(activityData, EncryptionStrengths.Medium);

                TraceFactory.Logger.Info("Step 3: Verifying Web Encryption Strength in High");
                result &= VerifyWebEncryptionStrength(activityData, EncryptionStrengths.High);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex.Message);
                result = false;
            }
            finally
            {
                WebEncryptionPostRequiresites(activityData);
            }

            return result;
        }

        /// <summary>
        /// Verifies with the given Web Encryption strength
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        /// <param name="strength">Web Encryption strength</param>
        /// <returns>Returns test result</returns>
        public static bool VerifyWebEncryptionStrength(SecurityActivityData activityData, EncryptionStrengths strength)
        {
            EwsWrapper.Instance().SetEncryptionStrength(strength);

            // check the browser connection
            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            return printer.IsEwsAccessible(printer.WiredIPv4Address, "http") && printer.IsEwsAccessible(printer.WiredIPv4Address, "https");
        }

        /// <summary>
        /// Verifies Enable/Disable of Encrypt Web Communication
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        /// <returns>Returns test result</returns>
        public static bool VerifyEncryptWebCommunicationOption(SecurityActivityData activityData)
        {
            bool result = true;

            try
            {
                TraceFactory.Logger.Info("There are no Pre-Requiresites");
                TraceFactory.Logger.Info("Step 1: Verifying Encrypt Web Communication in Enable mode");
                EwsWrapper.Instance().SetEncryptWebCommunication(true);
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                result &= printer.IsEwsAccessible(printer.WiredIPv4Address, "http", BrowserModel.Firefox);
                result &= printer.IsEwsAccessible(printer.WiredIPv4Address, "https", BrowserModel.Firefox);

                TraceFactory.Logger.Info("Step 2: Verifying Encrypt Web Communication in Disable mode");
                EwsWrapper.Instance().SetEncryptWebCommunication(false);

                result &= printer.IsEwsAccessible(printer.WiredIPv4Address, "http", BrowserModel.Firefox);
                result &= printer.IsEwsAccessible(printer.WiredIPv4Address, "https", BrowserModel.Firefox);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex.Message);
                result = false;
            }
            finally
            {
                WebEncryptionPostRequiresites(activityData);
            }

            return result;
        }

        /// <summary>
        /// Verifies Web Encryption options on Power Cycle and Cold Reset
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        /// <returns>Returns test result</returns>
        public static bool VerifyWebEncryptionOnResets(SecurityActivityData activityData)
        {
            bool result = true;

            try
            {
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                TraceFactory.Logger.Info("There are no Pre-Requiresites");
                // Power Cycle
                TraceFactory.Logger.Info("Step 1: Verifying Web Encryption options after Power Cycle");

                // set with the following settings
                EwsWrapper.Instance().SetEncryptWebCommunication(false);
                EwsWrapper.Instance().SetSecureProtocol(SecureProtocol.TLS10);
                EwsWrapper.Instance().SetEncryptionStrength(EncryptionStrengths.Medium);

                printer.PowerCycle();

                TraceFactory.Logger.Info("Checking settings after Power Cycle");
                if (!EwsWrapper.Instance().GetEncryptWebCommunication())
                {
                    TraceFactory.Logger.Info("Encrypt All Web Communication is not disabled after power cycle");
                }
                else
                {
                    TraceFactory.Logger.Info("Encrypt All Web Communication is disabled after power cycle");
                }

                if (EwsWrapper.Instance().GetTLSOption("TLS1.0"))
                {
                    TraceFactory.Logger.Info("TLS 1.0 is enabled after power cycle");
                }
                else
                {
                    TraceFactory.Logger.Info("TLS 1.0 is not enabled after power cycle");
                }

                if (!EwsWrapper.Instance().GetTLSOption("TLS1.1") && !EwsWrapper.Instance().GetTLSOption("TLS1.2"))
                {
                    TraceFactory.Logger.Info("TLS 1.1 and TLS 1.2 is disabled after power cycle");
                }
                else
                {
                    TraceFactory.Logger.Info("TLS 1.1 and TLS 1.2 is not disabled after power cycle");
                }

                if (EwsWrapper.Instance().GetEncryptionStrength().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Encryption strength is Medium after power cycle");
                }
                else
                {
                    TraceFactory.Logger.Info("Encryption strength is not Medium after power cycle");
                }

                if (activityData.ProductFamily == ProductFamilies.LFP.ToString())
                {
                    // Cold Reset
                    TraceFactory.Logger.Info("Step 1: Verifying Web Encryption options after Cold Reset");
                    try
                    {
                        printer.ColdReset();
                    }
                    catch (Exception ex)
                    {

                        TraceFactory.Logger.Info("Exception occured while Cold restting : {0}".FormatWith(ex));
                    }

                    if (EwsWrapper.Instance().GetEncryptionStrength().EqualsIgnoreCase("Low"))
                    {
                        TraceFactory.Logger.Info("Entryption Strength is 'Low' after cold reset");

                        if (EwsWrapper.Instance().GetTLSOption("TLS1.0"))
                        {
                            TraceFactory.Logger.Info("TLS 1.0 is enabled");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("TLS 1.0 is not enabled");
                            result = false;
                        }

                        if (EwsWrapper.Instance().GetTLSOption("TLS1.1"))
                        {
                            TraceFactory.Logger.Info("TLS 1.1 is enabled");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("TLS 1.1 is not enabled");
                            result = false;
                        }

                        if (EwsWrapper.Instance().GetTLSOption("TLS1.2"))
                        {
                            TraceFactory.Logger.Info("TLS 1.2 is enabled");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("TLS 1.2 is not enabled");
                            result = false;
                        }

                        if (EwsWrapper.Instance().GetEncryptWebCommunication())
                        {
                            TraceFactory.Logger.Info("Encrypt All Web Communication is enabled");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Encrypt All Web Communication is not enabled");
                            result = false;
                        }
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Entryption Strength must be 'Low' after cold reset");
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex.Message);
                result = false;
            }
            finally
            {
                WebEncryptionPostRequiresites(activityData);
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Shows the error popup
        /// </summary>
        /// <param name="errorMessage">The message to be shown.</param>
        /// <returns>True if the user clicks retry, else false.</returns>
        public static bool ShowErrorPopUp(string errorMessage)
        {
            DialogResult result = MessageBox.Show(errorMessage + @"Click Retry to continue or Cancel to ignore.", @"Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

            if (result == DialogResult.Retry)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Private Methods
        private static bool IsInSameSubnet(IPAddress firstIp, IPAddress secondIp)
        {
            return firstIp.GetAddressBytes()[2] == (uint)secondIp.GetAddressBytes()[2];
        }

        private static bool ChangePort(string switchIPAddress, string destinationNetwork, params int[] portNumbers)
        {
            TraceFactory.Logger.Info("Disconnecting the printer from source dhcp Server network");

            INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(switchIPAddress));

            foreach (var portNumber in portNumbers)
            {
                // Disable the port on first VLAN
                if (networkSwitch.DisablePort(portNumber))
                {
                    TraceFactory.Logger.Debug("Printer is disconnected from the source network.");
                }
                else
                {
                    TraceFactory.Logger.Debug("Failed to disconnect the printer from source network.");
                    return false;
                }
            }

            int destinationVlan = (from vlan in networkSwitch.GetAvailableVirtualLans()
                                   where (null != vlan.IPAddress) && vlan.IPAddress.IsInSameSubnet(IPAddress.Parse(destinationNetwork))
                                   select vlan.Identifier).FirstOrDefault();

            if (destinationVlan == 0)
            {
                TraceFactory.Logger.Info("No VLAN is found in the {0} network.".FormatWith(destinationNetwork));
                return false;
            }

            // Change the ports to second VLAN 
            foreach (var portNumber in portNumbers)
            {
                if (!networkSwitch.ChangeVirtualLan(portNumber, destinationVlan))
                {
                    return false;
                }
            }

            TraceFactory.Logger.Debug("Connecting the printer in destination network: {0}.".FormatWith(destinationNetwork));
            // Enable Port on second VLAN
            foreach (var portNumber in portNumbers)
            {
                if (networkSwitch.EnablePort(portNumber))
                {
                    TraceFactory.Logger.Debug("Printer is connected to destination network: {0}.".FormatWith(destinationNetwork));
                }
                else
                {
                    TraceFactory.Logger.Debug("Failed to connect printer to destination network : {0}.".FormatWith(destinationNetwork));
                    return false;
                }
            }

            Thread.Sleep(TimeSpan.FromMinutes(3));
            return true;
        }

        private static bool ValidateExternalInterface(SecurityActivityData activityData, Dictionary<IPAddress, string> networkDetails, bool isSameSubnet = false)
        {
            try
            {
                IPAddress blockedNetworkIp;

                if (isSameSubnet)
                {
                    // Take the network in the same subnet of europa interface as the blocked network
                    blockedNetworkIp = networkDetails.FirstOrDefault(x => x.Key.IsInSameSubnet(IPAddress.Parse(activityData.SecondaryDhcpServerIPAddress), IPAddress.Parse(activityData.SecondaryDhcpServerIPAddress).GetSubnetMask())).Key;
                }
                else
                {
                    blockedNetworkIp = networkDetails.FirstOrDefault(x => x.Key.IsInSameSubnet(IPAddress.Parse(activityData.SecondaryWiredIPv4Address), IPAddress.Parse(activityData.SecondaryWiredIPv4Address).GetSubnetMask())).Key;
                }

                // Disable all the NICs except the one with same subnet as that of europa wired interface
                foreach (var item in networkDetails.Where(x => !(x.Key.Equals(blockedNetworkIp))))
                {
                    NetworkUtil.DisableNetworkConnection(item.Value);
                }

                TraceFactory.Logger.Info("Checking Secondary wired interface in blocked network: {0}.".FormatWith(blockedNetworkIp));
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.SecondaryWiredIPv4Address);

                if (!CtcUtility.ValidateDeviceServices(printer, telnet: DeviceServiceState.Pass, snmpGet: DeviceServiceState.Pass, snmpSet: DeviceServiceState.Pass, http: DeviceServiceState.Pass, ftp: DeviceServiceState.Pass))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Checking wireless interface in blocked network: {0}.".FormatWith(blockedNetworkIp));
                printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WirelessIPv4Address);

                return CtcUtility.ValidateDeviceServices(printer, telnet: DeviceServiceState.Pass, snmpGet: DeviceServiceState.Pass, snmpSet: DeviceServiceState.Pass, http: DeviceServiceState.Pass, ftp: DeviceServiceState.Pass);
            }
            finally
            {
                foreach (var item in networkDetails)
                {
                    NetworkUtil.EnableNetworkConnection(item.Value);
                }
            }
        }

        /// <summary>
        /// Performs Windows test Pr-requisites, it will check the printer, clients and server status
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="multipleInterfaceCheck"></param>
        /// <param name="performFIPSPrerequisites"></param>
        /// <returns>Returns true if the environment is correct else returns false</returns>
        private static bool TestPreRequisites(SecurityActivityData activityData, bool multipleInterfaceCheck = false, bool performFIPSPrerequisites = false)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);

            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WiredIPv4Address), TimeSpan.FromSeconds(20)))
            {
                TraceFactory.Logger.Info("Ping failed with IP address: {0}.".FormatWith(activityData.WiredIPv4Address));
                return false;
            }

            EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
            // Setting advances option telnet,SNMP, FTP..
            EwsWrapper.Instance().SetAdvancedOptions();

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            if (!printer.IsEwsAccessible(activityData.WiredIPv4Address, "https"))
            {
                return false;
            }

            if (multipleInterfaceCheck)
            {
                IPAddress secondaryInterfaceAddress;

                // Other interfaces check
                if (IPAddress.TryParse(activityData.SecondaryWiredIPv4Address, out secondaryInterfaceAddress) && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.SecondaryWiredIPv4Address), TimeSpan.FromSeconds(20)))
                {
                    TraceFactory.Logger.Info("Ping failed with IP address: {0}.".FormatWith(activityData.SecondaryWiredIPv4Address));
                    return false;
                }

                printer = PrinterFactory.Create(activityData.ProductFamily, activityData.SecondaryWiredIPv4Address);

                if (!printer.IsEwsAccessible(activityData.SecondaryWiredIPv4Address, "https"))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondaryWiredIPv4Address);
                // Setting advances option telnet,SNMP, FTP..
                EwsWrapper.Instance().SetAdvancedOptions();
                //to check SNMP is active or not as Pre-req
                if (!printer.IsSnmpAccessible(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("SNMP is not Active. Please check the printer.");
                }

                if (IPAddress.TryParse(activityData.WirelessIPv4Address, out secondaryInterfaceAddress) && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WirelessIPv4Address), TimeSpan.FromSeconds(20)))
                {
                    TraceFactory.Logger.Info("Ping failed with IP address: {0}.".FormatWith(activityData.WirelessIPv4Address));
                    return false;
                }

                printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WirelessIPv4Address);

                if (!printer.IsEwsAccessible(activityData.WirelessIPv4Address, "https"))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WirelessIPv4Address);
                // Setting advances option telnet,SNMP, FTP..
                EwsWrapper.Instance().SetAdvancedOptions();

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();
            }

            //if (performFIPSPrerequisites)
            //{
               //Removinf this from preq for fips as fips does not need self signed cert.
            //    // EwsWrapper.Instance().InstallSelfSignedCertificate();
            //}

            return true;
        }

        /// <summary>
        /// Performs Windows test post requisites, deletes all the rules on the windows primary and also
        /// it will delete all the certificates
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="deleteIDCertificate"></param>
        /// <param name="deleteCACertificate"></param>
        /// <param name="deleteRules"></param>
        private static void TestPostRequisites(SecurityActivityData activityData, bool deleteIDCertificate = false, bool deleteCACertificate = false, bool deleteRules = false)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            EwsWrapper.Instance().SetFipsOption(false);

            if (deleteIDCertificate)
            {
                EwsWrapper.Instance().UnInstallIDCertificate();
            }
            if (deleteCACertificate)
            {
                EwsWrapper.Instance().UnInstallCACertificate();
            }
            if (deleteRules)
            {
                TraceFactory.Logger.Info("Cleaning up the Rules Created in Printer and Client");

                // Delete all rules in the client
                if (!CtcUtility.DeleteAllIPsecRules())
                {
                    CtcUtility.ShowErrorPopup("Failed to delete Rules in client. \nMake sure rules are deleted from client before proceeding to next test case");
                }

                // Delete all rules in printer
                if (!EwsWrapper.Instance().DeleteAllRules())
                {
                    CtcUtility.ShowErrorPopup("Failed to delete Rules in printer. \nMake sure rules are deleted from printer before proceeding to next test case");
                }
                TraceFactory.Logger.Info("Performing cold reset");
                try
                {
                    printer.ColdReset();
                }
                catch (Exception ex)
                {

                    TraceFactory.Logger.Info("Exception occured while Cold restting : {0}".FormatWith(ex));
                }
            }
        }

        private static void AclPostRequisites(SecurityActivityData activityData, Dictionary<IPAddress, string> networkDetails, bool deleteRule = true, bool isDifferentSubnet = true)
        {
            try
            {
                if (deleteRule)
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                }

                foreach (var item in networkDetails)
                {
                    NetworkUtil.EnableNetworkConnection(item.Value);
                }

                Thread.Sleep(TimeSpan.FromSeconds(30));

                IPAddress localAddress = networkDetails.FirstOrDefault(x => x.Key.IsInSameSubnet(IPAddress.Parse(activityData.WiredIPv4Address))).Key;
                // Setting the system address to the address with which the rule was created in order to avoid disconnection problems
                SystemConfiguration.SetDhcpIPAddress(networkDetails.FirstOrDefault(x => x.Key.Equals(localAddress)).Value);

                SnmpWrapper.Instance().SetCommunityName("public");
                Thread.Sleep(TimeSpan.FromSeconds(90));

                if (deleteRule)
                {
                    EwsWrapper.Instance().DeleteAllAclRules();
                }

                EwsWrapper.Instance().SetDefaultSnmpCommunityName();
            }
            finally
            {
                if (!isDifferentSubnet)
                {
                    ChangePort(activityData.SwitchIPAddress, networkDetails.FirstOrDefault(x => (x.Key.IsInSameSubnet(IPAddress.Parse(activityData.SecondaryDhcpServerIPAddress)))).Key.ToString(), activityData.SecondaryWiredPortNo);
                    ChangePort(activityData.SwitchIPAddress, networkDetails.FirstOrDefault(x => (x.Key.IsInSameSubnet(IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress)))).Key.ToString(), activityData.WiredPortNo);
                }
            }
        }

        private static void WebEncryptionPostRequiresites(SecurityActivityData activityData)
        {
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                TraceFactory.Logger.Info("Enable all Secure Protocol options on the IE browser");
                IEBrowserSettingsAutomation.SetSecureProtocols(SecureProtocol.All);

                TraceFactory.Logger.Info("Enable all Secure Protocol options on the Printer");
                EwsWrapper.Instance().SetSecureProtocol(SecureProtocol.AllTLS);

                TraceFactory.Logger.Info("Enable Encrypt All Web Communication option");
                EwsWrapper.Instance().SetEncryptWebCommunication(true);
            }
            catch
            {
                // ignored
            }
        }

        #endregion
    }
}
