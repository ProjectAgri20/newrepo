using System;
using System.Net;
using System.Threading;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.WebProxy
{

    /// <summary>
    /// Web Proxy Templates
    /// </summary>
    internal static class WebProxyTemplates
    {
        #region Constants
        private const int PROXYEXPIRYTIME = 3900;
        #endregion
        #region Web Proxy Templates

        #region  Unsecure Web Proxy

        /// <summary>
        /// Unsecure Web Proxy
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool UnsecureWebProxy(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Manual,
                    IPAddress = activityData.UnsecureWebProxyServerIPAddress,
                    PortNo = activityData.UnsecureWebProxyServerPortNumber,
                };
                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Secure Web Proxy

        /// <summary>
        /// Secure Web Proxy
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool SecureWebProxy(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Manual,
                    IPAddress = activityData.SecureWebProxyServerIPAddress,
                    PortNo = activityData.SecureWebProxyServerPortNumber,
                    UserName = activityData.SecureWebProxyServerUsername,
                    Password = activityData.SecureWebProxyServerPassword,
                    AuthType = WebProxyAuthType.Both
                };
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.SecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Invalid Web Proxy

        /// <summary>
        /// Invalid Web Proxy
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool InvalidWebProxy(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    TraceFactory.Logger.Info("Step 01: Invalid IP Address");
                    WebProxyConfigurationDetails invalidConfigDetails = new WebProxyConfigurationDetails
                    {
                        ProxyType = WebProxyType.Manual,
                        IPAddress = activityData.PrimaryDHCPServerIPAddress,
                        PortNo = activityData.UnsecureWebProxyServerPortNumber,
                    };
                    if (!EwsWrapper.Instance().SetWebProxy(invalidConfigDetails))
                    {
                        return false;
                    }
                    if (EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));

                    TraceFactory.Logger.Info("Step 02: Invalid Port Number");
                    invalidConfigDetails.IPAddress = activityData.UnsecureWebProxyServerIPAddress;
                    invalidConfigDetails.PortNo = 8888;

                    if (!EwsWrapper.Instance().SetWebProxy(invalidConfigDetails))
                    {
                        return false;
                    }
                    if (EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));

                    TraceFactory.Logger.Info("Step 03: Invalid Credentials");
                    if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.SecureWebProxyServerIPAddress))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));

                    invalidConfigDetails.IPAddress = activityData.SecureWebProxyServerIPAddress;
                    invalidConfigDetails.PortNo = activityData.SecureWebProxyServerPortNumber;
                    invalidConfigDetails.UserName = activityData.SecureWebProxyServerUsername;
                    invalidConfigDetails.Password = "xyz";
                    invalidConfigDetails.AuthType = WebProxyAuthType.Both;

                    if (!EwsWrapper.Instance().SetWebProxy(invalidConfigDetails))
                    {
                        return false;
                    }
                    if (EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Manual Proxy Power Cycle

        /// <summary>
        /// Manual Proxy Power Cycle
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool ManualProxyPowerCycle(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.WiredIPv4Address));
                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Manual,
                    IPAddress = activityData.SecureWebProxyServerIPAddress,
                    PortNo = activityData.SecureWebProxyServerPortNumber,
                    UserName = activityData.SecureWebProxyServerUsername,
                    Password = activityData.SecureWebProxyServerPassword,
                    AuthType = WebProxyAuthType.Both
                };
                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.SecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    printer.PowerCycle();
                    if (EwsWrapper.Instance().GetWebProxy().Equals(configDetails))
                    {
                        TraceFactory.Logger.Info("Web Proxy {0} is retained after Power Cycle".FormatWith(configDetails.ProxyType));
                        TraceFactory.Logger.Debug("Web Proxy Parameters Configured: {0}".FormatWith(configDetails.ToString()));
                        if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Web Proxy {0} is not retained after Power Cycle".FormatWith(configDetails.ProxyType));
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Manual Proxy Restore Factory Settings

        /// <summary>
        /// Manual Proxy Restore Factory Settings
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool ManualProxyColdReset(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.WiredIPv4Address));

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Manual,
                    IPAddress = activityData.SecureWebProxyServerIPAddress,
                    PortNo = activityData.SecureWebProxyServerPortNumber,
                    UserName = activityData.SecureWebProxyServerUsername,
                    Password = activityData.SecureWebProxyServerPassword,
                    AuthType = WebProxyAuthType.Both
                };
                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.SecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    printer.ColdReset();
                    if (EwsWrapper.Instance().GetWebProxy().ProxyType == WebProxyType.Auto)
                    {
                        TraceFactory.Logger.Info("Web Proxy {0} is restored to Auto after Restore Factory Settings".FormatWith(configDetails.ProxyType));
                        TraceFactory.Logger.Debug("Web Proxy Parameters Configured: {0}".FormatWith(configDetails.ToString()));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Web Proxy {0} is not restored to Disable after Restore Factory Settings".FormatWith(configDetails.ProxyType));
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Web Proxy - FQDN

        /// <summary>
        /// FQDN Web Proxy
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool FQDNWebProxy(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Manual,
                    IPAddress = activityData.SecureWebProxyServerFQDN,
                    PortNo = activityData.SecureWebProxyServerPortNumber,
                    UserName = activityData.SecureWebProxyServerUsername,
                    Password = activityData.SecureWebProxyServerPassword,
                    AuthType = WebProxyAuthType.Both
                };
                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.SecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Secure Web Proxy - SNMP

        /// <summary>
        /// Secure Web Proxy - SNMP
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool SecureWebProxySNMP(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Manual,
                    IPAddress = activityData.SecureWebProxyServerIPAddress,
                    PortNo = activityData.SecureWebProxyServerPortNumber,
                    UserName = activityData.SecureWebProxyServerUsername,
                    Password = activityData.SecureWebProxyServerPassword,
                    AuthType = WebProxyAuthType.Both
                };
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.SecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!SnmpWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Disable Web Proxy

        /// <summary>
        /// Disable Web Proxy
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool DisableWebProxy(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Disable,
                };
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Disable Web Proxy - SNMP

        /// <summary>
        /// Disable Web Proxy - SNMP
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool DisableWebProxySNMP(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Disable,
                };
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!SnmpWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  cURL Web Proxy

        /// <summary>
        /// cURL Web Proxy
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool cURLWebProxy(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Curl,
                    cURL = activityData.cURLPathIPAddress,
                };
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Invalid cURL

        /// <summary>
        /// Invalid cURL
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool InvalidcURL(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Curl,
                    cURL = activityData.UnsecureWebProxyServerIPAddress,
                };
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  cURL FQDN

        /// <summary>
        /// cURL FQDN
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool cURLFQDN(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Curl,
                    cURL = activityData.cURLPathFQDN,
                };
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  cURL Web Proxy - SNMP

        /// <summary>
        /// cURL Web Proxy - SNMP
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool cURLWebProxySNMP(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Curl,
                    cURL = activityData.cURLPathIPAddress,
                };
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(120));
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!SnmpWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy

        /// <summary>
        /// Auto Web Proxy
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxy(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };
                if (!SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathIPAddress))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }

                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathFQDN);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy FQDN

        /// <summary>
        /// Auto Web Proxy FQDN
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyFQDN(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - BOOTP to DHCP

        /// <summary>
        /// Auto Web Proxy - BOOTP to DHCP
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyBOOTPDHCP(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - DHCP to BOOTP

        /// <summary>
        /// Auto Web Proxy - DHCP to BOOTP
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyDHCPBOOTP(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Manual to DHCP

        /// <summary>
        /// Auto Web Proxy - Manual to DHCP
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyManualDHCP(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - DHCP to Manual

        /// <summary>
        /// Auto Web Proxy - DHCP to Manual
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyDHCPManual(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Stateless DHCPv4 Enabled

        /// <summary>
        /// Auto Web Proxy- Stateless DHCPv4 Enabled
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyStatelessDHCPv4Enabled(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    EwsWrapper.Instance().SetStatelessDHCPv4(true);
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetStatelessDHCPv4(false);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Stateless DHCPv4 Disabled

        /// <summary>
        /// Auto Web Proxy- Stateless DHCPv4 Disabled
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyStatelessDHCPv4Disabled(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress);
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    EwsWrapper.Instance().SetStatelessDHCPv4(false);
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetStatelessDHCPv4(false);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Lease Time Expiry - Proxy Retention

        /// <summary>
        /// Auto Web Proxy - Lease Time Expiry - Proxy Retention
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyLeaseTimeProxyRetention(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!SetLeaseTime(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, 300))
                    {
                        return false;
                    }

                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        return false;
                    }
                    TraceFactory.Logger.Info("Waiting for 180 seconds for lease renewal...");
                    Thread.Sleep(TimeSpan.FromSeconds(240));

                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetLeaseTime(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, 691200);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Lease Time Expiry - No Proxy

        /// <summary>
        /// Auto Web Proxy - Lease Time Expiry - No Proxy
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyLeaseTimeNoProxy(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!SetLeaseTime(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, 300))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        return false;
                    }
                    if (!DeleteWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress))
                    {
                        return false;
                    }
                    TraceFactory.Logger.Info("Waiting for 180 seconds for lease renewal...");
                    Thread.Sleep(TimeSpan.FromSeconds(240));
                    if (EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetLeaseTime(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, 691200);
                    SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathIPAddress);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Lease Time Expiry - New Proxy

        /// <summary>
        /// Auto Web Proxy - Lease Time Expiry - New Proxy
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyLeaseTimeNewProxy(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!DeleteWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!SetLeaseTime(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, 300))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        return false;
                    }
                    if (!SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathIPAddress))
                    {
                        return false;
                    }
                    TraceFactory.Logger.Info("Waiting for 180 seconds for lease renewal...");

                    Thread.Sleep(TimeSpan.FromSeconds(240));

                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetLeaseTime(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, 691200);
                    SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathIPAddress);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Lease Time Expiry - DHCP to DNS Discovery

        /// <summary>
        /// Auto Web Proxy - Lease Time Expiry - DHCP to DNS Discovery
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyLeaseTimeDHCPToDNS(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!SetLeaseTime(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, 300))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetDomainName(activityData.DomainName))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDHCPServerIPAddress))
                    {
                        return false;
                    }
                    if (!DeleteWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress))
                    {
                        return false;
                    }
                    TraceFactory.Logger.Info("Waiting for 180 seconds for lease renewal...");
                    Thread.Sleep(TimeSpan.FromSeconds(240));
                    if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathFQDN);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Lease Time Expiry - DNS to DHCP Discovery

        /// <summary>
        /// Auto Web Proxy - Lease Time Expiry - DNS to DHCP Discovery
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyLeaseTimeDNSToDHCP(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!DeleteWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!ConfigureDNSParameters(activityData.PrimaryDHCPServerIPAddress))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetDomainName(activityData.DomainName))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!SetLeaseTime(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, 300))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        return false;
                    }
                    if (!SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathFQDN))
                    {
                        return false;
                    }
                    string domainName = CtcUtility.GetUniqueDomainName();
                    if (!EwsWrapper.Instance().SetDomainName(domainName))
                    {
                        return false;
                    }
                    TraceFactory.Logger.Info("Waiting for 180 seconds for lease renewal...");
                    Thread.Sleep(TimeSpan.FromSeconds(240));
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathFQDN);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - DNS Discovery - Domain Name

        /// <summary>
        /// Auto Web Proxy - DNS Discovery - Domain Name
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyDNSDiscoveryDomainName(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!DeleteWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!ConfigureDNSParameters(activityData.PrimaryDHCPServerIPAddress))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetDomainName(activityData.DomainName))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }

                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathFQDN);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - DNS Discovery - DNS Suffix

        /// <summary>
        /// Auto Web Proxy - DNS Discovery - DNS Suffix
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyDNSDiscoveryDNSSuffix(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!DeleteWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!ConfigureDNSParameters(activityData.PrimaryDHCPServerIPAddress))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetDNSSuffixList(activityData.DomainName))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }

                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().DeleteAllSuffixes();
                    SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathFQDN);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Proxy Expiry Time - Proxy Retention

        /// <summary>
        /// Auto Web Proxy - Proxy Expiry Time - Proxy Retention
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyExpiryTimeProxyRetention(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    TraceFactory.Logger.Info("Waiting for 60 minutes for proxy expiry time renewal....");
                    Thread.Sleep(TimeSpan.FromSeconds(PROXYEXPIRYTIME));

                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetLeaseTime(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, 691200);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Proxy Expiry Time - New Proxy

        /// <summary>
        /// Auto Web Proxy - Proxy Expiry Time - New Proxy
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyExpiryTimeNewProxy(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!DeleteWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDHCPServerIPAddress))
                    {
                        return false;
                    }
                    if (!SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathIPAddress))
                    {
                        return false;
                    }
                    TraceFactory.Logger.Info("Waiting for 60 minutes for proxy expiry time renewal...");
                    Thread.Sleep(TimeSpan.FromSeconds(PROXYEXPIRYTIME));
                    if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetLeaseTime(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, 691200);
                    SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathIPAddress);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - Discovery Priority

        /// <summary>
        /// Auto Web Proxy - Discovery Priority
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxyDiscoveryPriority(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!ConfigureDNSParameters(activityData.PrimaryDHCPServerIPAddress))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetDomainName(activityData.DomainName))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!EwsWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.UnsecureWebProxyServerIPAddress))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(120));
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }

                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathFQDN);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region  Auto Web Proxy - SNMP

        /// <summary>
        /// Auto Web Proxy - SNMP
        /// </summary>
        /// <param name="activityData">WebProxyActivityData</param>
        /// <returns>true if template passes
        ///          false if template fails </returns>        
        public static bool AutoWebProxySNMP(WebProxyActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
                {
                    ProxyType = WebProxyType.Auto,
                };
                if (!SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathIPAddress))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP))
                {
                    return false;
                }
                if (!ConfigureDNSParameters(activityData.UnsecureWebProxyServerIPAddress))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    if (!SnmpWrapper.Instance().SetWebProxy(configDetails))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().ValidateConnectivity(ConnectivityTest.Internet))
                    {
                        return false;
                    }
                    return true;
                }

                catch (Exception webProxyException)
                {
                    TraceFactory.Logger.Info(webProxyException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    SetWPADServerEntry(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathFQDN);
                    WebProxyPostRequisites();
                }
            }
        }

        #endregion

        #region Private Methods

        private static bool WebProxyPostRequisites()
        {
            EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP);
            WebProxyConfigurationDetails configDetails = new WebProxyConfigurationDetails
            {
                ProxyType = WebProxyType.Disable
            };
            if (SnmpWrapper.Instance().SetWebProxy(configDetails))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool SetWPADServerEntry(string dhcpServer, string scope, string wpadServer)
        {
            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(dhcpServer);
            if (serviceMethod.Channel.SetWPADServer(dhcpServer, scope, wpadServer))
            {
                TraceFactory.Logger.Info("Successfully set WPAD Server details {0} on DHCP Server {1}".FormatWith(wpadServer, dhcpServer));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set WPAD Server details {0} on DHCP Server {1}".FormatWith(wpadServer, dhcpServer));
                return false;
            }
        }

        private static bool DeleteWPADServerEntry(string dhcpServer, string scope)
        {
            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(dhcpServer);
            if (serviceMethod.Channel.DeleteWPADServer(dhcpServer, scope))
            {
                TraceFactory.Logger.Info("Successfully deleted WPAD Server details on DHCP Server {0}".FormatWith(dhcpServer));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to delete WPAD Server details on DHCP Server {0}".FormatWith(dhcpServer));
                return false;
            }
        }

        private static bool SetDomainNameEntry(string dhcpServer, string scope, string domainName)
        {
            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(dhcpServer);
            if (serviceMethod.Channel.SetDomainName(dhcpServer, scope, domainName))
            {
                TraceFactory.Logger.Info("Successfully set Domain Name {0} on DHCP Server {1}".FormatWith(domainName, dhcpServer));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set Domain Name {0} on DHCP Server {1}".FormatWith(domainName, dhcpServer));
                return false;
            }
        }

        private static bool SetLeaseTime(string dhcpServer, string dhcpScope, int leaseTime)
        {
            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(dhcpServer);
            if (serviceMethod.Channel.SetDhcpLeaseTime(dhcpServer, dhcpScope, leaseTime))
            {
                TraceFactory.Logger.Info("Successfully set lease time on DHCP Server {0} to {1}".FormatWith(dhcpServer, leaseTime));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set lease time on DHCP Server {0}".FormatWith(dhcpServer));
                return false;
            }

        }

        private static bool ConfigureDNSParameters(string dnsServer)
        {
            string domainName = CtcUtility.GetUniqueDomainName();
            if (!EwsWrapper.Instance().SetDomainName(domainName))
            {
                return false;
            }
            if (!EwsWrapper.Instance().DeleteAllSuffixes())
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetPrimaryDnsServer(dnsServer))
            {
                return false;
            }
            Thread.Sleep(TimeSpan.FromSeconds(120));
            return true;
        }

        #endregion
    }
}
#endregion



