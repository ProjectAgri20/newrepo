using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Printing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.Print.Monitor;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;
using TopCat.TestApi.GUIAutomation;
using System.Diagnostics;
using HP.ScalableTest.Plugin.ConnectivityPrint.UIMaps;
using TopCat.TestApi.GUIAutomation.Enums;
using Microsoft.Win32;
using System.Security;

namespace HP.ScalableTest.Plugin.ConnectivityPrint
{
    /// <summary>
    /// Templates for Print
    /// </summary>
    internal class ConnectivityPrintTemplates
    {
        #region Local Variable

        ConnectivityPrintActivityData _activityData;

        // Network Switch object
        INetworkSwitch _networkSwitch = null;

        // File name for cancellation, hose break & pause
        string _cancelFileName = string.Empty;
        string _hosebreakFileName = string.Empty;
        string _pauseFileName = string.Empty;

        // User input status on PrintQueue error
        bool _abort = false;

        // Job completion status for hose break and pause resume
        bool _jobCompleted = false;

        // Timeout for hose break
        int _tcpipTimeOut = 0;

        private static string IDCERTIFICATEPATH = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\IPPS\ID\SPIJetdirectCert.pfx");
        private static string CACERTIFICATEPATH = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\IPPS\CA\SPIRootCA.cer");
        private static string FIRMWAREBASELOCATION = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\FirmwareFiles");
        private static string PRINTERDRIVER_KEYPATH = @"SYSTEM\CurrentControlSet\Control\Print\Printers";
        private const string IDCERTIFICATE_PSWD = "JetDirect";

        #endregion

        #region Constants

        // Cancel file position
        const int INACTIVE_CANCEL_FILE = 4;
        const int ACTIVE_CANCEL_FILE = 2;
        const int SPOOLING_CANCEL_FILE = 0;
        // Timeout for hose break
        const int SHORT_TIMEOUT_TPS_IWS_LFP = 30;
        const int LONG_TIMEOUT_TPS_IWS_LFP = 90;
        // Timeout for pause job
        const int PAUSE_JOB_TIMEOUT = 5;
        // Printer Error message box title
        const string PRINTER_ERROR_WINDOW_TITLE = "Printer Error";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to load <see cref=" ConnectivityPrintActivityData"/>
        /// </summary>
        /// <param name="activityData"><see cref=" ConnectivityPrintActivityData"/></param>
        public ConnectivityPrintTemplates(ConnectivityPrintActivityData activityData)
        {
            _activityData = activityData;
        }

        #endregion

        #region Public Functions

        #region Cancel Jobs

        /// <summary>
        /// Cancel Inactive job.
        /// Send 5 jobs and cancel the last job.
        /// </summary>       
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <returns>true if inactive job cancelled and subsequent jobs printed successfully, false otherwise</returns>
        public bool CancelInactiveJob(Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo, bool isIPv6 = false)
        {
            if (!isIPv6)
            {
                TraceFactory.Logger.Info("Executing Cancel Inactive job test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));
                return CancelInactiveJob(_activityData.Ipv4Address, printProtocol, portNo, testNo);
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Cancel Inactive job test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        results[resultIndex++] = CancelInactiveJob(ipAddress, printProtocol, portNo, testNo);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        /// <summary>
        /// Cancel Active job.
        /// Send 5 jobs and cancel 3rd job while printing.
        /// </summary>        
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <returns>true if active job cancelled and subsequent jobs printed successfully, false otherwise</returns>
        public bool CancelActiveJob(Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo, bool isIPv6 = false)
        {
            if (!isIPv6)
            {
                TraceFactory.Logger.Info("Executing Cancel Active job test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));
                return CancelActiveJob(_activityData.Ipv4Address, printProtocol, portNo, testNo);
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Cancel Active job test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        results[resultIndex++] = CancelActiveJob(ipAddress, printProtocol, portNo, testNo);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        /// <summary>
        /// Cancel Spooling job.
        /// Send 5 jobs and cancel the 1st job while spooling.
        /// </summary>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <returns>true if spooling job cancelled and subsequent jobs printed successfully, false otherwise</returns>
        public bool CancelSpoolingJob(Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo, bool isIPv6 = false)
        {
            if (!isIPv6)
            {
                TraceFactory.Logger.Info("Executing Cancel Spooling job test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));
                return CancelSpoolingJob(_activityData.Ipv4Address, printProtocol, portNo, testNo);
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Cancel Spooling job test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        results[resultIndex++] = CancelSpoolingJob(ipAddress, printProtocol, portNo, testNo);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        /// <summary>
        /// Cancel FTP job
        /// </summary>
        /// <param name="testNo">Test number</param>
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <returns>true if FTP job is cancelled and subsequent job is printed successfully, false otherwise</returns>
        public bool CancelFtpJob(int testNo, bool isIPv6 = false)
        {
            if (!isIPv6)
            {
                TraceFactory.Logger.Info("Executing Cancel Ftp job test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));
                return CancelFtpJob(_activityData.Ipv4Address, testNo);
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Cancel Ftp job test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        results[resultIndex++] = CancelFtpJob(ipAddress, testNo);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        #endregion

        #region  Continuous, Simple, Various File Size, Different Application

        /// <summary>
        /// Print all files for specified duration
        /// </summary>        
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <param name="duration">Time duration in minutes</param>
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <returns>true if all files printed successfully, false otherwise</returns>
        public bool ContinuousPrinting(Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo, int duration, bool isIPv6 = false)
        {
            if (!isIPv6)
            {
                TraceFactory.Logger.Info("Executing Continuous Printing test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));
                return ContinuousPrinting(_activityData.Ipv4Address, printProtocol, portNo, testNo, duration);
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Continuous Printing test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        results[resultIndex++] = ContinuousPrinting(ipAddress, printProtocol, portNo, testNo, duration);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        /// <summary>
        /// Print all files in specified folder
        /// </summary>        
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>        
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <returns>true if all files printed successfully, false otherwise</returns>
        public bool SimplePrinting(Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo, bool isIPv6 = false)
        {
            if (!isIPv6)
            {
                string printerHostName = EwsWrapper.Instance().GetHostname();
                TraceFactory.Logger.Debug("Executing Simple Printing test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));
                if (portNo == 443)
                {
                    string outHostName = string.Empty;
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                    if (!SimplePrinting(_activityData.Ipv4Address, printProtocol, portNo, testNo, outHostName))
                    {
                        return false;
                    }
                    return IPPS_PostRequisite(printerHostName);
                }
                else
                {
                    return SimplePrinting(_activityData.Ipv4Address, printProtocol, portNo, testNo);
                }
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Simple Printing test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        results[resultIndex++] = SimplePrinting(ipAddress, printProtocol, portNo, testNo);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        /// <summary>
        /// Print all files in specified folder for specified duration
        /// </summary>        
        /// <param name="testNo">Test number</param>
        /// <param name="duration">Duration for printing in minutes</param>
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <param name="isPassiveMode">Passive/ Active mode for FTP</param>
        /// <returns>true if all files printed successfully, false otherwise</returns>
        public bool ContinuousPrintingFtp(int testNo, int duration, bool isIPv6 = false, bool isPassiveMode = false)
        {
            if (!isIPv6)
            {
                TraceFactory.Logger.Info("Executing Continuous Printing with Ftp test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));
                return ContinuousPrintingFtp(_activityData.Ipv4Address, testNo, duration);
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Continuous Printing with Ftp test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        results[resultIndex++] = ContinuousPrintingFtp(ipAddress, testNo, duration, isPassiveMode: isPassiveMode);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        /// <summary>
        /// Print all files in specified folder
        /// </summary>        
        /// <param name="testNo">Test number</param>        
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <param name="isPassiveMode">Passive/ Active mode for FTP</param>
        /// <returns>true if all files printed successfully, false otherwise</returns>
        public bool SimplePrintingFtp(int testNo, bool isIPv6 = false, bool isPassiveMode = false)
        {
            if (!isIPv6)
            {
                TraceFactory.Logger.Info("Executing Simple Printing with Ftp test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));
                return SimplePrintingFtp(_activityData.Ipv4Address, testNo);
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Simple Printing with Ftp test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        results[resultIndex++] = SimplePrintingFtp(ipAddress, testNo, isPassiveMode: isPassiveMode);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        #endregion

        #region Hose Break

        /// <summary>
        /// Perform hose break while job printing
        /// </summary>        
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <param name="isShortHoseBreak">true for short hose break, false for long hose break</param>
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <returns>true if job printed successfully after hose break functionality, false otherwise</returns>
        public bool HoseBreak(Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo, bool isShortHoseBreak = true, bool isIPv6 = false)
        {
            if (!isIPv6)
            {
                TraceFactory.Logger.Info("Executing Hosebreak test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));

                if (Printer.Printer.PrintProtocol.IPP == printProtocol)
                {
                    TraceFactory.Logger.Info("IPP");
                    return HoseBreakWithIPP(_activityData.Ipv4Address, portNo, testNo, isShortHoseBreak);
                  // return HoseBreak(_activityData.Ipv4Address, printProtocol, portNo, testNo, isShortHoseBreak);
                }
                else if (portNo == 443)
                {
                    TraceFactory.Logger.Info("IPPS");
                    string printerHostName = EwsWrapper.Instance().GetHostname();
                    string outHostName = string.Empty;
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                    if (!HoseBreakWithIPP(_activityData.Ipv4Address, portNo, testNo, isShortHoseBreak, outHostName))
                    {
                        return false;
                    }
                    return IPPS_PostRequisite(printerHostName);
                }
                else
                {
                    TraceFactory.Logger.Info("IPP-Other");
                    return HoseBreak(_activityData.Ipv4Address, printProtocol, portNo, testNo, isShortHoseBreak);
                }
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Hosebreak test for {0} Address : {1}.".FormatWith(addressType, ipAddress));

                        if (Printer.Printer.PrintProtocol.IPP == printProtocol)
                        {
                            TraceFactory.Logger.Info("IPP-IPv6");
                            results[resultIndex++] = HoseBreakWithIPP(ipAddress, portNo, testNo, isShortHoseBreak);
                        }
                        else
                        {
                            TraceFactory.Logger.Info("IPP-IPv6-Other");
                            results[resultIndex++] = HoseBreak(ipAddress, printProtocol, portNo, testNo, isShortHoseBreak);
                        }
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        /// <summary>
        /// Perform hose break for FTP job
        /// </summary>        
        /// <param name="testNo">Test number</param>
        /// <param name="isShortHoseBreak">true for short hose break, false for long hose break</param>
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <returns>true if job printed successfully after hose break functionality, false otherwise</returns>
        public bool HoseBreakFtp(int testNo, bool isShortHoseBreak = true, bool isIPv6 = false)
        {
            if (!isIPv6)
            {
                TraceFactory.Logger.Info("Executing Hosebreak test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));
                return HoseBreakFtp(_activityData.Ipv4Address, testNo, isShortHoseBreak);
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Hosebreak with Ftp test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        results[resultIndex++] = HoseBreakFtp(ipAddress, testNo, isShortHoseBreak);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        #endregion

        #region Pause Resume

        /// <summary>
        /// Pause printing job and resume it
        /// </summary>        
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <param name="isIPv6">true if Ipv6 test, false otherwise</param>
        /// <returns>true if printed successfully after pause-resume, false otherwise</returns>
        public bool PauseJob(Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo, bool isIPv6 = false)
        {
            if (!isIPv6)
            {
                TraceFactory.Logger.Info("Executing Pause Job test for IPv4 Address : {0}.".FormatWith(_activityData.Ipv4Address));
                return PauseJob(_activityData.Ipv4Address, printProtocol, portNo, testNo);
            }
            else
            {
                BitArray results = new BitArray(3, true);
                bool returnResult = true;
                int resultIndex = 0;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (Ipv6AddressTypes.LinkLocal == address)
                    {
                        addressType = "LinkLocal";
                        ipAddress = _activityData.Ipv6LinkLocalAddress;
                    }
                    else if (Ipv6AddressTypes.Stateful == address)
                    {
                        addressType = "Statefull";
                        ipAddress = _activityData.Ipv6StateFullAddress;
                    }
                    else if (Ipv6AddressTypes.Stateless == address)
                    {
                        addressType = "Stateless";
                        ipAddress = _activityData.Ipv6StatelessAddress;
                    }
                    else
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Pause Job test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        results[resultIndex++] = PauseJob(ipAddress, printProtocol, portNo, testNo);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        results[resultIndex++] = false;
                    }
                }

                // Incase user has selected different IPv6 address types, combine all results
                foreach (bool result in results)
                {
                    returnResult &= result;
                }

                return returnResult;
            }
        }

        #endregion

        #region Dynamic Raw Port Printing

        /// <summary>
        /// Prints with specified Raw port number with different scenarios
        /// 1. Printing without setting the ports - It should fail
        /// 2. Printing after setting dynamic port - It should pass
        /// 3. Printing after dynamic port deletion - It should fail
        /// </summary>
        /// <param name="port1Number">Dynamic Port 1</param>
        /// <param name="port2Number">Dynamic Port 2</param>
        /// <param name="testNo">Test number</param>
        /// <param name="isIpv6">True if it is IPv6 else false</param>
        /// <returns>Returns true if all the test steps passes else returns fails.</returns>
        public bool DynamicRawPortPrinting(int port1Number, int port2Number, int testNo, bool isIpv6 = false)
        {
            // Test Feature
            /*
			 * Pre-requisite: Clear the Dynamic ports
			 * 1. Try installing and print - It should fail
			 * 2. Set the two dynamic ports and Install and Print with both the ports - It should print the files
			 * 3. Clear the Dynamic ports and Print with any one dynamic port - It should fail
			 * Post-requisites: Clear the Dynamic ports
			*/

            bool result = true;

            try
            {
                // Updated the Template based on the input provided by chintu dated: 10/2/2016                
                TraceFactory.Logger.Info("Test Pre-requisites");

                string[] files = GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles);
                if (null == files || 0 == files.Length)
                {
                    TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                    return false;
                }

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(_activityData.Ipv4Address));

                if (!printer.PingUntilTimeout(IPAddress.Parse(_activityData.Ipv4Address), TimeSpan.FromSeconds(20)))
                {
                    TraceFactory.Logger.Info("Printer is not pingable {0}".FormatWith(_activityData.Ipv4Address));
                    return false;
                }

                // Set the RAW Port number on the EWS page
                TraceFactory.Logger.Info("Step 1: Printing after setting Dynamic Raw Ports.");

                ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
                if (EwsWrapper.Instance().SetDynamicRawPorts(port1Number, port2Number))
                {
                    TraceFactory.Logger.Info("Printing after setting 'Dynamic Raw Port 1' with {0}".FormatWith(port1Number));
                    result &= printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.RAW, port1Number, testNo, isIpv6);

                    TraceFactory.Logger.Info("Printing after setting 'Dynamic Raw Port 2' with {0}".FormatWith(port2Number));
                    result &= printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.RAW, port2Number, testNo, isIpv6);

                    if (result)
                    {
                        TraceFactory.Logger.Info("Step 1 Test Result: Printing after setting Dynamic Raw Ports {0} and {1} are passed.".FormatWith(port1Number, port2Number));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Step 1 Test Result: Printing after setting Dynamic Raw Ports {0} and {1} failed.".FormatWith(port1Number, port2Number));
                        return false;
                    }
                }

                // Test steps
                TraceFactory.Logger.Info("Step 2: Installing and Printing without setting the Dynamic Raw Ports");
                TraceFactory.Logger.Info("Clearing any existing Dynamic Raw Ports.");

                if (EwsWrapper.Instance().ClearDynamicRawPorts())
                {
                    result = !printer.Print(files);
                }

                if (result)
                {
                    TraceFactory.Logger.Info("Step 2 Test Result: Printing with Raw Port {0} failed without setting the Dynamic Raw Ports.".FormatWith(port1Number));
                }
                else
                {
                    TraceFactory.Logger.Info("Step 2 Test Result: Printing with Raw Port {0} passed without setting the Dynamic Raw Ports.".FormatWith(port1Number));
                    return false;
                }
            }
            finally
            {
                // post requisites
                // delete the port numbers
                TraceFactory.Logger.Info("");
                TraceFactory.Logger.Info("Test Post-requisites");
                TraceFactory.Logger.Info("Clearing any existing Dynamic Raw Ports.");
                EwsWrapper.Instance().ClearDynamicRawPorts();
            }

            return true;
        }
        #endregion

        #region FTP Printing After IP change

        public bool FtpPrintingAfterIPChange(bool isIpv6 = false)
        {
            if (!isIpv6)
            {
                return FtpPrintingAfterIPChange(_activityData.Ipv4Address);
            }
            else
            {
                bool returnResult = true;

                foreach (Ipv6AddressTypes address in _activityData.Ipv6AddressTypes)
                {
                    string addressType = string.Empty;
                    string ipAddress = string.Empty;

                    if (!SetIPAddress(address, ref addressType, ref ipAddress))
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        TraceFactory.Logger.Info("Executing Ftp Printing After IP change test for {0} Address : {1}.".FormatWith(addressType, ipAddress));
                        returnResult &= FtpPrintingAfterIPChange(ipAddress);
                    }
                    else
                    {
                        // This is required if IPv6 address is not available
                        TraceFactory.Logger.Info("{0} IPv6 address was not available.".FormatWith(addressType));
                        returnResult &= false;
                    }
                }

                return returnResult;
            }
        }

        #endregion

        #endregion

        #region Templates

        #region Cancel jobs

        /// <summary>
        /// Cancel Inactive job.
        /// Send 5 jobs and cancel the last job.
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <returns>true if inactive job cancelled and subsequent jobs printed successfully, false otherwise</returns>
        private bool CancelInactiveJob(string ipAddress, Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo)
        {
            #region Input Validation
            // For Cancel jobs, we pick 5 files. If this condition is not satisfied, return failure.            

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.CancelFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Cancelling Inactive job failed. No files available for cancel jobs.");
                return false;
            }

            if (4 >= files.Length)
            {
                TraceFactory.Logger.Info("Cancelling Inactive job failed. No of files is less than 5.");
                return false;
            }

            #endregion

            // Mark the file for cancelling
            _cancelFileName = files[INACTIVE_CANCEL_FILE];

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (printer.Install(IPAddress.Parse(ipAddress), printProtocol, _activityData.DriverPackagePath, _activityData.DriverModel, portNo))
            {
                // Subscribe for PrintQueue error and Spooling Events
                printer.PrintQueueError += printer_PrintQueueError;
                printer.PrintJobSpooling += printer_PrintJobSpooling;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }
                printer.PrintAsynchronously(files);

                // Wait till all jobs are Completed/ PrintQueue reports an error
                while (printer.NumberOfJobsInPrintQueue != 0 && !_abort)
                { }

                // If PrintQueue error is not cleared, delete all jobs and return failure
                if (_abort)
                {
                    TraceFactory.Logger.Info("Cancelling Inactive job failed.");
                    TraceFactory.Logger.Info("Deleting all jobs in PrintQueue.");
                    printer.PrintQueueError -= printer_PrintQueueError;
                    printer.DeleteAllPrintQueueJobs();
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("Cancelling Inactive job failed.");
                return false;
            }

            TraceFactory.Logger.Info("Cancelling Inactive job is successfully, all subsequent jobs printed.");
            return true;
        }

        /// <summary>
        /// Cancel Active job.
        /// Send 5 jobs and cancel 3rd job while printing.
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <returns>true if active job cancelled and subsequent jobs printed successfully, false otherwise</returns>
        private bool CancelActiveJob(string ipAddress, Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo)
        {
            #region Input Validation
            // For Cancel jobs, we pick 5 files. If this condition is not satisfied, return failure.            

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.CancelFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Cancelling Active job failed. No files available for cancel jobs.");
                return false;
            }

            if (4 >= files.Length)
            {
                TraceFactory.Logger.Info("Cancelling Active job failed. No of files is less than 5.");
                return false;
            }

            #endregion

            // Mark the file for cancelling
            _cancelFileName = files[ACTIVE_CANCEL_FILE];

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (printer.Install(IPAddress.Parse(ipAddress), printProtocol, _activityData.DriverPackagePath, _activityData.DriverModel, portNo))
            {
                // Subscribe for PrintQueue error and Printing Events
                printer.PrintQueueError += printer_PrintQueueError;
                printer.PrintJobPrinting += printer_PrintJobPrinting;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }
                printer.PrintAsynchronously(files);

                // Wait till all jobs are Completed/ PrintQueue reports an error
                while (printer.NumberOfJobsInPrintQueue != 0 && !_abort)
                { }

                // If PrintQueue error is not cleared, delete all jobs and return failure
                if (_abort)
                {
                    TraceFactory.Logger.Info("Cancelling Active job failed.");
                    TraceFactory.Logger.Info("Deleting all jobs in PrintQueue.");
                    printer.PrintQueueError -= printer_PrintQueueError;
                    printer.DeleteAllPrintQueueJobs();
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("Cancelling Active job failed.");
                return false;
            }

            TraceFactory.Logger.Info("Cancelling Active job is successfully, all subsequent jobs printed.");
            return true;
        }

        /// <summary>
        /// Cancel Spooling job.
        /// Send 5 jobs and cancel the 1st job while spooling.
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <returns>true if spooling job cancelled and subsequent jobs printed successfully, false otherwise</returns>
        private bool CancelSpoolingJob(string ipAddress, Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo)
        {
            #region Input Validation
            // For Cancel jobs, we pick 5 files. If this condition is not satisfied, return failure.            

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.CancelFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Cancelling Spooling job failed. No files available for cancel jobs.");
                return false;
            }

            if (4 >= files.Length)
            {
                TraceFactory.Logger.Info("Cancelling Spooling job failed. No of files is less than 5.");
                return false;
            }

            #endregion

            // Mark the file for cancelling
            _cancelFileName = files[SPOOLING_CANCEL_FILE];

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (printer.Install(IPAddress.Parse(ipAddress), printProtocol, _activityData.DriverPackagePath, _activityData.DriverModel, portNo))
            {
                // Subscribe for PrintQueue error and Spooling Events
                printer.PrintJobSpooling += printer_PrintJobSpooling;
                printer.PrintQueueError += printer_PrintQueueError;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }
                printer.PrintAsynchronously(files);

                // Wait till all jobs are Completed/ PrintQueue reports an error
                while (printer.NumberOfJobsInPrintQueue != 0 && !_abort)
                { }

                // If PrintQueue error is not cleared, delete all jobs and return failure
                if (_abort)
                {
                    TraceFactory.Logger.Info("Cancelling Spooling job failed.");
                    TraceFactory.Logger.Info("Deleting all jobs in PrintQueue.");
                    printer.PrintQueueError -= printer_PrintQueueError;
                    printer.DeleteAllPrintQueueJobs();
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("Cancelling Spooling job failed.");
                return false;
            }

            TraceFactory.Logger.Info("Cancelling Spooling job is successfully, all subsequent jobs printed.");
            return true;
        }

        /// <summary>
        /// Cancel FTP job
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="testNo">Test number</param>
        /// <returns>true if FTP job is cancelled and subsequent job is printed successfully, false otherwise</returns>
        private bool CancelFtpJob(string ipAddress, int testNo)
        {
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.FTPCancelFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Cancelling FTP job failed. No files available for cancel jobs.");
                return false;
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (!_activityData.PaperlessMode)
            {
                printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
            }

            printer.FTPJobPrinting += printer_FTPJobPrinting;
            printer.PrintWithFtpAsynchronously(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files.FirstOrDefault());

            if (!printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files.Length > 1 ? files[1] : files.FirstOrDefault()))
            {
                TraceFactory.Logger.Info("Cancelling FTP job failed. FTP job didn't print successfully.");
                return false;
            }

            TraceFactory.Logger.Info("Cancelling FTP job is successfully, next job printed successfully.");
            return true;
        }

        #endregion

        #region Continuous, Simple

        /// <summary>
        /// Print all files for specified duration
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <param name="duration">Time duration in minutes</param>
        /// <returns>true if all files printed successfully, false otherwise</returns>
        private bool ContinuousPrinting(string ipAddress, Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo, int duration)
        {
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.ContinousFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Continuous Printing failed. No files available.");
                return false;
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (printer.Install(IPAddress.Parse(ipAddress), printProtocol, _activityData.DriverPackagePath, _activityData.DriverModel, portNo))
            {
                // Clear PrintQueue before starting test
                printer.DeleteAllPrintQueueJobs();
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }
                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                if (!printer.Print(files, duration))
                {
                    TraceFactory.Logger.Info("Continuous Printing failed. All jobs didn't print.");
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("Continuous Printing failed.");
                return false;
            }

            TraceFactory.Logger.Info("All jobs for Continuous Printing printed successfully.");
            return true;
        }

        /// <summary>
        /// Print all files in specified folder
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>        
        /// <returns>true if all files printed successfully, false otherwise</returns>
        private bool SimplePrinting(string ipAddress, Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo, string certificateHostName = null)
        {
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }

			PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
			Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));
                        
            if ((portNo == 80 || portNo == 631))
            {
                string printerName = InstallPrinterUsingTopCat(_activityData, portNo);
                if (printerName != null)
                {
                    if (!printer.Print(files, TimeSpan.FromMinutes(2), printerName))
                    {
                        TraceFactory.Logger.Info("Simple Printing failed. All jobs didn't print.");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (printer.Install(IPAddress.Parse(ipAddress), printProtocol, _activityData.DriverPackagePath, _activityData.DriverModel, portNo, certificateHostName))
                {
                    // Subscribing for Print Queue Event
                    printer.PrintQueueError += printer_PrintQueueError;
                    if (!_activityData.PaperlessMode)
                    {
                        printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                    }
                    // Subscribing for Print Queue Event
                    printer.PrintQueueError += printer_PrintQueueError;

                    if (!printer.Print(files))
                    {
                        TraceFactory.Logger.Info("Simple Printing failed. All jobs didn't print.");
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Simple Printing failed.");
                    return false;
                }
            }

            TraceFactory.Logger.Info("All jobs for Simple Printing printed successfully.");
            return true;
        }

        /// <summary>
        /// Print all files in specified folder for specified duration
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="testNo">Test number</param>
        /// <param name="duration">Duration for printing in minutes</param>
        /// <param name="isPassiveMode">Passive/ Active mode for FTP</param>
        /// <returns>true if all files printed successfully, false otherwise</returns>
        private bool ContinuousPrintingFtp(string ipAddress, int testNo, int duration, bool isPassiveMode = false)
        {
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.FTPContinousFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Continuous Printing failed. No files available.");
                return false;
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (!_activityData.PaperlessMode)
            {
                printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
            }
            if (printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files, duration, isPassiveMode: isPassiveMode))
            {
                TraceFactory.Logger.Info("All jobs for Continuous Printing printed successfully.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Continuous Printing failed. All jobs didn't print successfully.");
                return false;
            }
        }

        /// <summary>
        /// Print all files in specified folder
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="testNo">Test number</param>        
        /// <param name="isPassiveMode">Passive/ Active mode for FTP</param>
        /// <returns>true if all files printed successfully, false otherwise</returns>
        private bool SimplePrintingFtp(string ipAddress, int testNo, bool isPassiveMode = false)
        {
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.FTPSimpleFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (!_activityData.PaperlessMode)
            {
                printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
            }
            if (printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files.FirstOrDefault(), isPassiveMode: isPassiveMode))
            {
                TraceFactory.Logger.Info("All jobs for Simple Printing printed successfully.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Simple Printing failed. All jobs didn't print successfully.");
                return false;
            }
        }

        #endregion

        #region Hose break

        /// <summary>
        /// Perform hose break while job printing
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <param name="isShortHoseBreak">true for short hose break, false for long hose break</param>
        /// <returns>true if job printed successfully after hose break functionality, false otherwise</returns>
        private bool HoseBreak(string ipAddress, Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo, bool isShortHoseBreak = true)
        {
            _hosebreakFileName = string.Empty;
            _jobCompleted = false;
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.HoseBreakFiles);
            // Create network switch object before starting printing the page
            _networkSwitch = SwitchFactory.Create(IPAddress.Parse(_activityData.SwitchIPAddress));

            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Hose break test failed. No files available.");
                return false;
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (printer.Install(IPAddress.Parse(ipAddress), printProtocol, _activityData.DriverPackagePath, _activityData.DriverModel, portNo))
            {
                if (ProductFamilies.VEP == _activityData.ProductFamily)
                {
                    // Set TCPIP timeout based on Product family
                    SnmpWrapper.Instance().Create(_activityData.Ipv4Address);

                    int tcpipTimeOut = SnmpWrapper.Instance().GetTcpIpTimeout();

                    if (isShortHoseBreak)
                    {
                        _tcpipTimeOut = tcpipTimeOut / 2;
                    }
                    else
                    {
                        _tcpipTimeOut = Convert.ToInt32(tcpipTimeOut * 1.5);
                    }
                }
                else
                {
                    if (isShortHoseBreak)
                    {
                        _tcpipTimeOut = SHORT_TIMEOUT_TPS_IWS_LFP;
                    }
                    else
                    {
                        _tcpipTimeOut = LONG_TIMEOUT_TPS_IWS_LFP;
                    }
                }
                TraceFactory.Logger.Info("Timeout:{0}".FormatWith(_tcpipTimeOut));
                // Mark file for hose break
                _hosebreakFileName = files.FirstOrDefault();

                // Clear PrintQueue before starting test
                printer.DeleteAllPrintQueueJobs();
                // Subscribe for PrintQueue error, printing and completion events
                printer.PrintQueueError += printer_PrintQueueError;
                printer.PrintJobPrinting += printer_PrintJobPrinting_HoseBreak;
                printer.PrintJobFinished += printer_PrintJobFinished_HoseBreak;

                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                printer.PrintAsynchronously(_hosebreakFileName);

                Thread.Sleep(TimeSpan.FromMinutes(4));
                DateTime startTime = DateTime.Now;
                TimeSpan timeOut = new TimeSpan(0, 30, 0);

                /* Scenario handled after print job is sent:
				 * 
				 * 1. Wait for print job to attain 'Printing' status, disable port , wait for timeout and enable the port
				 *      case a: print job is sent to printer after enabling the port and no of jobs is 0.
				 *      case b: print job is lost and no of jobs is 0.
				 *      case c: print job is stuck in spooler and no of jobs is 1.
				 * Case a and b: Switch port is enabled (as a pre-requisite) and 2nd job is sent.
				 * Case c: Wait for 30 minutes maximum, clear all jobs and fail the test
				 * 
				 * 2. Print job attained 'Printing' status, disabled switch port; after timeout, enable port and print job is cleared. 2nd job is sent
				 * 
				 * 3. Check whether hose break was really performed by checking the flag: _jobCompleted.
				 * 
				 * */

                try
                {
                   // MessageBox.Show("Print3");
                    TraceFactory.Logger.Info("no of jobs in queue :{0}".FormatWith(printer.NumberOfJobsInPrintQueue));
                    TraceFactory.Logger.Info("Time out : {0}".FormatWith(timeOut));

                    // Wait till job is completed or timeout
                    while (printer.NumberOfJobsInPrintQueue != 0 && (DateTime.Now.Subtract(timeOut) <= startTime))
                    { }

                    // Check whether loop ended due to maxmimum timeout
                    if (DateTime.Now.Subtract(timeOut) >= startTime)
                    {
                        TraceFactory.Logger.Info("Print job didnt clear after waiting for {0} minutes.".FormatWith(timeOut.TotalMinutes));
                        TraceFactory.Logger.Info("Hose break test failed.");
                        return false;
                    }

                    // Check if hose break was performed
                    if (!_jobCompleted)
                    {
                        TraceFactory.Logger.Info("Printer is still active after disabling printer port.");
                        TraceFactory.Logger.Info("Hose break test failed.");
                        return false;
                    }

                    if (printer.NumberOfJobsInPrintQueue == 0)
                    {
                        // If job is cleared from PrintQueue, make sure the port is enabled.									
                        _networkSwitch.EnablePort(_activityData.SwitchPortNumber);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        TraceFactory.Logger.Info("Print file : {0} printed successfully.".FormatWith(Path.GetFileNameWithoutExtension(_hosebreakFileName)));
                        if (printer.Print(files.Length > 1 ? files[1] : files.FirstOrDefault()))
                        {
                            TraceFactory.Logger.Info("Hose break test passed, next job was printed successfully.");
                            return true;
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Hose break test failed, next job didn't print successfully.");
                            return false;
                        }
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Hose break test failed. Print job didn't complete within {0} minutes.".FormatWith(timeOut.TotalMinutes));
                        return false;
                    }
                }
                finally
                {
                    // Adding a post requisite to make sure the Switch port is enabled.
                    _networkSwitch.EnablePort(_activityData.SwitchPortNumber);
                    _networkSwitch = null;
                    printer.DeleteAllPrintQueueJobs();
                }
            }
            else
            {
                TraceFactory.Logger.Info("Hose break test failed.");
                return false;
            }
        }

        /// <summary>
        /// Perform hose break for FTP job
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>        
        /// <param name="testNo">Test number</param>
        /// <param name="isShortHoseBreak">true for short hose break, false for long hose break</param>
        /// <returns>true if job printed successfully after hose break functionality, false otherwise</returns>
        private bool HoseBreakFtp(string ipAddress, int testNo, bool isShortHoseBreak = true)
        {
            _jobCompleted = false;
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.FTPHoseBreakFiles);

            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Hose break test failed. No files available.");
                return false;
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            // Set TCPIP timeout based on Product family
            int tcpipTimeOut = CtcUtility.GetTCPIPConnectionTimeout(_activityData.Ipv4Address);

            if (ProductFamilies.VEP == _activityData.ProductFamily)
            {
                if (isShortHoseBreak)
                {
                    _tcpipTimeOut = tcpipTimeOut / 2;
                }
                else
                {
                    _tcpipTimeOut = Convert.ToInt32(tcpipTimeOut * 1.5);
                }
            }
            else
            {
                if (isShortHoseBreak)
                {
                    _tcpipTimeOut = SHORT_TIMEOUT_TPS_IWS_LFP;
                }
                else
                {
                    _tcpipTimeOut = LONG_TIMEOUT_TPS_IWS_LFP;
                }
            }

            if (printer.PingUntilTimeout(IPAddress.Parse(ipAddress), TimeSpan.FromSeconds(10)))
            {
                if (!_activityData.PaperlessMode)
                {
                    printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                printer.FTPJobPrinting += printer_FTPJobPrinting_HoseBreak;
                printer.PrintWithFtpAsynchronously(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files.FirstOrDefault());

                DateTime startTime = DateTime.Now;
                TimeSpan timeOut = new TimeSpan(0, 10, 0);

                // Wait till job is completed or timeout
                while (!_jobCompleted && (DateTime.Now.Subtract(timeOut) <= startTime))
                { }

                if (_jobCompleted)
                {
                    printer.FTPJobPrinting -= printer_FTPJobPrinting_HoseBreak;
                    if (printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty,
                        files.Length > 1 ? files[1] : files.FirstOrDefault()))
                    {
                        TraceFactory.Logger.Info("Hose break test with FTP passed, next job printed successfully.");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Hose break test with FTP failed, next job didn't print successfully.");
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Hose break test failed.");
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("Hose break test failed. Printer is not pingable.");
                return false;
            }
        }

        #endregion

        #region Pause resume

        /// <summary>
        /// Pause printing job and resume it
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <returns>true if printed successfully after pause-resume, false otherwise</returns>
        private bool PauseJob(string ipAddress, Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo)
        {
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.PauseFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Pause job test failed. No files available.");
                return false;
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (printer.Install(IPAddress.Parse(ipAddress), printProtocol, _activityData.DriverPackagePath, _activityData.DriverModel, portNo))
            {
                // Mark file for Pause job
                _pauseFileName = files.FirstOrDefault();

                // Subscribe for PrintQueue error, Printing and completion events
                printer.PrintQueueError += printer_PrintQueueError;
                printer.PrintJobPrinting += printer_PrintJobPrinting_PauseJob;
                printer.PrintJobFinished += printer_PrintJobFinished_PauseJob;

                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }
                printer.PrintAsynchronously(_pauseFileName);

                DateTime startTime = DateTime.Now;
                TimeSpan timeOut = new TimeSpan(0, 30, 0);

                // Wait till job completed or timeout
                while (printer.NumberOfJobsInPrintQueue != 0 && (DateTime.Now.Subtract(timeOut) <= startTime))
                { }

                if (printer.NumberOfJobsInPrintQueue == 0)
                {
                    TraceFactory.Logger.Info("Pause job test passed, job printed successfully.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Pause job test failed, print job didn't complete in {0} minutes.".FormatWith(timeOut.TotalMinutes));
                    return false;
                }

            }
            else
            {
                TraceFactory.Logger.Info("Pause job test failed.");
                return false;
            }
        }

        #endregion

        #region FTP Printing After IP Change

        private bool FtpPrintingAfterIPChange(string ipAddress, bool isIpv6 = false)
        {
            _jobCompleted = false;
            bool result = true;

            // use the hose break files for IP change test
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.FTPHoseBreakFiles);

            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("IP change test failed. No files available in the {0} folder.".FormatWith(FolderType.FTPHoseBreakFiles.ToString()));
                return false;
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (printer.PingUntilTimeout(IPAddress.Parse(ipAddress), TimeSpan.FromSeconds(10)))
            {
                printer.FTPJobPrinting += printer_FTPJobAfterIPChange;
                printer.PrintWithFtpAsynchronously(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files.FirstOrDefault());

                // once the FTP printing is in progress _jobCompleted will become to true
                if (_jobCompleted)
                {
                    IPAddress newIPAddress = IPAddress.None;

                    // Change the IP address (with next available non pingable IP address) of the printer
                    if (isIpv6)
                    {
                        throw new NotImplementedException("Changing IPv6 address is not yet implemented.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Fetching the next non-ping able IP Address of the printer");
                        newIPAddress = NetworkUtil.FetchNextIPAddress(EwsWrapper.Instance().GetSubnetMask(), printer.WiredIPv4Address);

                        TraceFactory.Logger.Info("Changing IPAddress of the printer to : " + newIPAddress);
                        EwsWrapper.Instance().SetIPaddress(newIPAddress.ToString());
                    }

                    // create new printer object with new IP address
                    Printer.Printer newPrinter = PrinterFactory.Create(family, newIPAddress);

                    result = newPrinter.PrintWithFtp(newIPAddress, string.Empty, string.Empty, files.FirstOrDefault());

                    if (result)
                    {
                        TraceFactory.Logger.Info("Ftp printing after IP change passed, job printed successfully.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Ftp printing after IP change failed, job didn't printed successfully.");
                    }

                    //Post-requisite, resetting the old ip address.
                    if (isIpv6)
                    {
                        throw new NotImplementedException("Changing IPv6 address is not yet implemented.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Post-requisite, setting the old ip address to printer.");
                        EwsWrapper.Instance().SetIPaddress(ipAddress.ToString());
                        return result;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Ftp printing after IP change test failed.");
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("Ftp Printing after IP change test failed. Printer is not pingable.");
                return false;
            }
        }

        #endregion

        #region IPP

        /// <summary>
        /// Perform hose break while job printing
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>
        /// <param name="isShortHoseBreak">true for short hose break, false for long hose break</param>
        /// <returns>true if job printed successfully after hose break functionality, false otherwise</returns>
        private bool HoseBreakWithIPP(string ipAddress, int portNo, int testNo, bool isShortHoseBreak = true, string certificateHostName = null)
        {
            _hosebreakFileName = string.Empty;
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.HoseBreakFiles);
            TraceFactory.Logger.Info("*** Switch Ip address : {0}".FormatWith(_activityData.SwitchIPAddress));
            MessageBox.Show("Check Switch IP created");
            _networkSwitch = SwitchFactory.Create(IPAddress.Parse(_activityData.SwitchIPAddress));

            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Hose break test failed. No files available.");
                return false;
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (printer.Install(IPAddress.Parse(ipAddress), portNo.Equals(443) ? Printer.Printer.PrintProtocol.IPPS : Printer.Printer.PrintProtocol.IPP, _activityData.DriverPackagePath, _activityData.DriverModel, portNo, certificateHostName))
            {
                // Set TCPIP timeout based on Product family
                int tcpipTimeOut = CtcUtility.GetTCPIPConnectionTimeout(_activityData.Ipv4Address);

                if (ProductFamilies.VEP == _activityData.ProductFamily)
                {
                    if (isShortHoseBreak)
                    {
                        _tcpipTimeOut = tcpipTimeOut / 2;
                    }
                    else
                    {
                        _tcpipTimeOut = Convert.ToInt32(tcpipTimeOut * 1.5);
                    }
                }
                else
                {
                    if (isShortHoseBreak)
                    {
                        _tcpipTimeOut = SHORT_TIMEOUT_TPS_IWS_LFP;
                    }
                    else
                    {
                        _tcpipTimeOut = LONG_TIMEOUT_TPS_IWS_LFP;
                    }
                }

                // Mark file for hose break
                _hosebreakFileName = files.FirstOrDefault();

                // Clear PrintQueue before starting test
                printer.DeleteAllPrintQueueJobs();

              //  INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(_activityData.SwitchIPAddress));
               
                printer.PrintQueueError += printer_PrintQueueError;
                printer.PrintJobPrinting += printer_PrintJobPrinting_HoseBreak;
                printer.PrintJobFinished += printer_PrintJobFinished_HoseBreak;

                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }
                printer.PrintAsynchronously(_hosebreakFileName);

                DateTime startTime = DateTime.Now;
                TimeSpan timeOut = new TimeSpan(0, 30, 0);
                try
                {
                    // Wait till job is completed or timeout
                    while (printer.NumberOfJobsInPrintQueue != 0 && (DateTime.Now.Subtract(timeOut) <= startTime))
                    { }
                   
                    // Check whether loop ended due to maxmimum timeout
                    if (DateTime.Now.Subtract(timeOut) >= startTime)
                    {
                        TraceFactory.Logger.Info("Print job didnt clear after waiting for {0} minutes.".FormatWith(timeOut.TotalMinutes));
                        TraceFactory.Logger.Info("Hose break test failed.");
                        return false;
                    }
                  
                    // Check if hose break was performed
                    if (!_jobCompleted)
                    {
                        TraceFactory.Logger.Info("Printer is still active after disabling printer port.");
                        TraceFactory.Logger.Info("Hose break test failed.");
                        return false;
                    }
                   
                    if (printer.NumberOfJobsInPrintQueue == 0)
                    {
                        TraceFactory.Logger.Info("No jobs in Queue after Hose break. Printing Simple file **** ");
                        // If job is cleared from PrintQueue, make sure the port is enabled.									
                       // _networkSwitch.EnablePort(_activityData.SwitchPortNumber);
                       
                        TraceFactory.Logger.Info("Checking if Port is enabled : {0}".FormatWith(_activityData.SwitchPortNumber));

                        if (!_networkSwitch.IsPortDisabled(_activityData.SwitchPortNumber)) ;
                        {
                            TraceFactory.Logger.Info("Port is already enabled");
                        }
                        _networkSwitch.EnablePort(_activityData.SwitchPortNumber);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        TraceFactory.Logger.Info("Print Simple file : {0} printed successfully.".FormatWith(GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles)));
                        files = GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles);
                        if (null == files || 0 == files.Length)
                        {
                            TraceFactory.Logger.Info("No Simple file Available.");
                            return false;
                        }
                        TraceFactory.Logger.Info("printingStatus Simple file");
                        bool result = printer.Print(files);
                        if (result)
                        {

                            TraceFactory.Logger.Info("***Hose break test passed, next job was printed successfully.");
                            return true;
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Hose break test failed, next job didn't print successfully.");
                            return false;
                        }
                        //return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Hose break test failed. Print job didn't complete within {0} minutes.".FormatWith(timeOut.TotalMinutes));
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Info("Exception Occured in Hose Break Test : {0}".FormatWith(ex.Message));
                    return false;
                }

                finally
                {
                    // Adding a post requisite to make sure the Switch port is enabled.
                    _networkSwitch.EnablePort(_activityData.SwitchPortNumber);
                    _networkSwitch = null;
                    printer.DeleteAllPrintQueueJobs();
                }
                //Neha has commented this
                // Wait for printer to attain 'Printing' status and perform hose break
                ////do
                ////{
                ////    TraceFactory.Logger.Debug("Waiting for printer to attain 'Printing' status.");
                ////    // Checking every half second for printer status
                ////    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                ////} while ((PrinterStatus.Printing != printer.PrinterStatus) && (DateTime.Now.Subtract(timeOut) <= startTime));

                // Check whether printer didn't record Printing status
                ////if ((DateTime.Now.Subtract(timeOut) >= startTime))
                ////{
                ////    TraceFactory.Logger.Info("Printer status didn't attain to 'Printing' status after waiting for {0} minutes".FormatWith(timeOut.TotalMinutes));
                ////    TraceFactory.Logger.Debug("Current printer status: {0}".FormatWith(printer.PrinterStatus));
                ////    TraceFactory.Logger.Info("Hose break test failed.");
                ////    TraceFactory.Logger.Debug("Deleting all PrintQueue files as post-requisites.");
                ////    printer.DeleteAllPrintQueueJobs();
                ////    return false;
                ////}

                ////// Perform hose break				
                ////if (networkSwitch.DisablePort(_activityData.SwitchPortNumber))
                ////{
                ////    if (!printer.PingUntilTimeout(IPAddress.Parse(_activityData.Ipv4Address), TimeSpan.FromSeconds(10)))
                ////    {
                ////        TraceFactory.Logger.Info("Switch IP : {0} with port no : {1} is disabled.".FormatWith(_activityData.SwitchIPAddress, _activityData.SwitchPortNumber));
                ////        TraceFactory.Logger.Info("Waiting for timeout of {0} seconds.".FormatWith(_tcpipTimeOut));
                ////        Thread.Sleep(TimeSpan.FromSeconds(_tcpipTimeOut));
                ////        networkSwitch.EnablePort(_activityData.SwitchPortNumber);
                ////        TraceFactory.Logger.Info("Switch IP : {0} with port no : {1} is enabled.".FormatWith(_activityData.SwitchIPAddress, _activityData.SwitchPortNumber));
                ////    }
                ////    else
                ////    {
                ////        TraceFactory.Logger.Info("Printer is still active after disabling printer port.");
                ////        TraceFactory.Logger.Info("Hose break test failed.");
                ////        return false;
                ////    }
                ////}
                ////else
                ////{
                ////    TraceFactory.Logger.Info("Unable to disable switch port.");
                ////    TraceFactory.Logger.Info("Hose break test failed.");
                ////    return false;
                ////}

                ////TraceFactory.Logger.Debug("Waiting for a minute.");
                ////Thread.Sleep(TimeSpan.FromMinutes(1));

                ////TraceFactory.Logger.Debug("Waiting for printer to finish print job");
                ////startTime = DateTime.Now;

                ////do
                ////{
                ////    TraceFactory.Logger.Debug("Waiting for printer to come out from 'Printing' status.");
                ////    // Checking every half second for printer status
                ////    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                ////} while ((PrinterStatus.Printing == printer.PrinterStatus) && (DateTime.Now.Subtract(timeOut) <= startTime));

                ////if (printer.NumberOfJobsInPrintQueue == 0)
                ////{
                ////    // If job is cleared from PrintQueue, make sure the port is enabled.
                ////    // This scenario may occur for IPP protocol hose break tests where job is cleared from Queue after idle timeout
                ////    networkSwitch.EnablePort(_activityData.SwitchPortNumber);

                ////    TraceFactory.Logger.Info("Print file : {0} printed successfully.".FormatWith(Path.GetFileNameWithoutExtension(_hosebreakFileName)));
                ////    if (printer.Print(files.Length > 1 ? files[1] : files.FirstOrDefault()))
                ////    {
                ////        TraceFactory.Logger.Info("Hose break test passed, next job was printed successfully.");
                ////        return true;
                ////    }
                ////    else
                ////    {
                ////        TraceFactory.Logger.Info("Hose break test failed, next job didn't print successfully.");
                ////        return false;
                ////    }
                ////}
                ////else
                ////{
                ////    // Adding a post requisite to make sure the Switch port is enabled.
                ////    networkSwitch.EnablePort(_activityData.SwitchPortNumber);
                ////    TraceFactory.Logger.Info("Hose break test failed. Print job didn't complete within {0} minutes.".FormatWith(timeOut.TotalMinutes));
                ////    return false;
                ////}
            }
            else
            {
                TraceFactory.Logger.Info("Hose break test failed.");
                return false;
            }
        }

        #endregion

        #region Print Simple Files with All Protocols[Complex Printing]

        /// <summary>
        /// Verify printing with LAA configured (all protocol)
        /// -	Install the printer 
        /// -	Set the IP configuration method to Manual 
        /// -	Change the LAA of the printer
        /// -	Send few print jobs
        /// -	Print jobs should be successful
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool PrintWithLAA(ConnectivityPrintActivityData activityData, Printer.Printer.PrintProtocol printProtocol, int portNumber, int testNo, bool isIPv6 = false)
        {
            string currentDeviceAddress = string.Empty;
            string ipAddress = string.Empty;
            try
            {
                TraceFactory.Logger.Info("Configuring LAA and setting IP Configuration method to Manual");

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.Ipv4Address));

                string serverIP = Printer.Printer.GetDHCPServerIP(IPAddress.Parse(activityData.Ipv4Address)).ToString();
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(serverIP);
                string scopeIP = serviceMethod.Channel.GetDhcpScopeIP(serverIP);
                string macAddress = printer.MacAddress.Replace(":", string.Empty);

                string[] files = GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles);
                if (null == files || 0 == files.Length)
                {
                    TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                    return false;
                }
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = _activityData.Ipv6StateFullAddress;
                    currentDeviceAddress = ipAddress.Remove((ipAddress.Length) - (ipAddress.Split(':').Last().Count())) + 1;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                // Delete Reservation is not validated to handle cases where reservation is not present
                serviceMethod.Channel.DeleteReservation(serverIP, scopeIP, activityData.Ipv4Address, macAddress);

                if (serviceMethod.Channel.CreateReservation(serverIP, scopeIP, activityData.Ipv4Address, macAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Succeeded");
                }
                else
                {
                    TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
                }

                if (!isIPv6)
                {
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, IPAddress.Parse(activityData.Ipv4Address)))
                    {
                        return false;
                    }
                    ipAddress = activityData.Ipv4Address;
                }
                else
                {
                    if (!EwsWrapper.Instance().SetManualIPv6Address(true, currentDeviceAddress))
                    {
                        return false;
                    }
                    ipAddress = currentDeviceAddress;
                }

                printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));
                TraceFactory.Logger.Info("Installing the print driver with Protocol: {0} PortNumber: {1} and IPAddress: {2}".FormatWith(printProtocol, portNumber, ipAddress));
                printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber);

                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));
                string laa = macAddress.Remove(macAddress.Length - 2, 2).Insert(0, "02");
                EwsWrapper.Instance().SetLAA(laa);

                currentDeviceAddress = CtcUtility.GetPrinterIPAddress(laa);
                if (!isIPv6)
                {
                    if (string.IsNullOrEmpty(currentDeviceAddress))
                    {
                        TraceFactory.Logger.Info("No printer was discovered with the MAC address: {0}.".FormatWith(laa));
                        return false;
                    }

                    if (currentDeviceAddress.EqualsIgnoreCase(activityData.Ipv4Address))
                    {
                        TraceFactory.Logger.Info("Printer retained the manual IP address:{0} after changing LAA.".FormatWith(activityData.Ipv4Address));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer failed to retain the manual IP address after changing LAA.");
                        return false;
                    }
                }

                TraceFactory.Logger.Info("Sending Print Job");
                if (!printer.Print(files))
                {
                    TraceFactory.Logger.Info("Failed to Print the Simple Files");
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));
                TraceFactory.Logger.Info("Successfully Printed all the Documents in Simple Files");
                return true;
            }
            finally
            {
                TraceFactory.Logger.Info("Performing PostRequisite for LAA");
                // Cold reset the printer to set the LAA to default value.
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily.ToString(), currentDeviceAddress);
                printer.ColdReset();

                // Check for printer availability
                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), 1))
                {
                    TraceFactory.Logger.Info("Ping succeeded with Printer IP Address:{0}".FormatWith(activityData.Ipv4Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Ping failed with Printer IP Address:{0}".FormatWith(activityData.Ipv4Address));
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.Ipv4Address);

                //Enabling IPV6 startup[After cold reset the startup option will be in disabled state] 
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                EwsWrapper.Instance().SetIPv6(false);
                EwsWrapper.Instance().SetIPv6(true);
            }
        }

        /// <summary>
        ///  Verify printing a job submitted from single client before power on
        ///-	Install the printer (all protocols)
        ///-	Switch off the printer
        ///-	Send few jobs to the printer
        ///-	Switch on the printer and the print jobs should print successfully.
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool PrintOffline(ConnectivityPrintActivityData activityData, Printer.Printer.PrintProtocol printProtocol, int portNumber, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.ContinousFiles);
            string outHostName = string.Empty;
            string addressType = string.Empty;
            string printerHostName = EwsWrapper.Instance().GetHostname();

            try
            {
                if (null == files || 0 == files.Length)
                {
                    TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                    return false;
                }
                if (portNumber == 443)
                {
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                }
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                TraceFactory.Logger.Info("Installing the print driver with Protocol: {0} and PortNumber: {1}".FormatWith(printProtocol, portNumber));
                printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, outHostName);

                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                TraceFactory.Logger.Info("Reboot the Printer, and when the printer goes Off line, firing the Print job");
                printer.PowerCycleAsync();

                TraceFactory.Logger.Info("Validating the print job that has been fired when the printer is in off line state");

                Thread printerStatus = new Thread(() => printingStatus(printer));
                printerStatus.Start();
                printer.PrintAsynchronously(files);
                Thread.Sleep(TimeSpan.FromMinutes(2));

                // Validating the Jobs in Print Queue
                while (printer.NumberOfJobsInPrintQueue != 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(20));
                }
                printerStatus.Abort();

                TraceFactory.Logger.Info("Printer resumed the print job successfully");
                return true;
            }
            finally
            {
                if (portNumber == 443)
                {
                    IPPS_PostRequisite(printerHostName);
                }
            }
        }

        /// <summary>
        ///  Verify printing a job submitted from single client before power on
        ///-	Install the printer (all protocols)
        ///-	Send few jobs to the printer
        ///-	Reboot the printer and send few jobs.
        ///-	The print jobs should print successfully.
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool PrintOnReboot(ConnectivityPrintActivityData activityData, Printer.Printer.PrintProtocol printProtocol, int portNumber, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.ContinousFiles);
            string outHostName = string.Empty;
            string addressType = string.Empty;
            string printerHostName = EwsWrapper.Instance().GetHostname();

            try
            {
                if (null == files || 0 == files.Length)
                {
                    TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                    return false;
                }

                if (portNumber == 443)
                {
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                }
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                TraceFactory.Logger.Info("Installing the print driver with Protocol: {0} and PortNumber: {1}".FormatWith(printProtocol, portNumber));
                printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, outHostName);

                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                TraceFactory.Logger.Info("Sending the Print job before rebooting the printer");
                printer.PrintAsynchronously(files);

                TraceFactory.Logger.Info("Reboot the Printer");
                printer.PowerCycle();

                // Validating the Jobs in Print Queue
                while (printer.NumberOfJobsInPrintQueue != 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(20));
                }
                files = GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles);
                TraceFactory.Logger.Info("Sending the Print job after rebooting the printer when the Printer is in Ready State");
                if (!printer.Print(files))
                {
                    TraceFactory.Logger.Info("Failed to Print the Simple Files");
                    return false;
                }
                TraceFactory.Logger.Info("Successfully Printed all the Documents in Simple Files");
                return true;
            }
            finally
            {
                if (portNumber == 443)
                {
                    IPPS_PostRequisite(printerHostName);
                }
            }
        }

        /// <summary>
        ///  Verify printing a job submitted from single client before power on[FTP]
        ///-	Install the printer (all protocols)
        ///-	Send few jobs to the printer
        ///-	Reboot the printer and send few jobs.
        ///-	The print jobs should print successfully.
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool PrintOnReboot_FTP(ConnectivityPrintActivityData activityData, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            string addressType = string.Empty;

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.ContinousFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }
            if (isIPv6)
            {
                TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                ipAddress = activityData.Ipv6StateFullAddress;
            }
            else
            {
                ipAddress = activityData.Ipv4Address;
            }

            // Print Start page and send all jobs to PrintQueue
            if (!_activityData.PaperlessMode)
            {
                printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
            }

            TraceFactory.Logger.Info("Sending the Print job before rebooting the printer");
            printer.PrintWithFtpAsynchronously(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files.FirstOrDefault());

            TraceFactory.Logger.Info("Reboot the Printer");
            printer.PowerCycle();

            TraceFactory.Logger.Info("Sending the Print job after rebooting the printer with printer in ready state");
            printer.PrintWithFtpAsynchronously(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files.FirstOrDefault());

            // Validating the Printer with Printer Status
            while (printer.PrinterStatus != PrinterStatus.Printing)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            TraceFactory.Logger.Info("Successfully Printed");
            return true;
        }

        /// <summary>
        ///  Verify printing after IP change and re-installation
        ///-	Install the printer (all protocol)
        ///-	Send print jobs
        ///-	Change the ip address of the printer
        ///-	Re install the printer and send few print jobs.
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool PrintOnIPChangeAndReinstall(ConnectivityPrintActivityData activityData, Printer.Printer.PrintProtocol printProtocol, int portNumber, int testNo, bool isIPv6 = false)
        {
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            string currentDeviceAddress = string.Empty;
            string outHostName = string.Empty;
            string addressType = string.Empty;
            string printerHost = EwsWrapper.Instance().GetHostname();

            currentDeviceAddress = NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.Ipv4Address).GetSubnetMask(), IPAddress.Parse(activityData.Ipv4Address)).ToString();

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }
            try
            {
                if (portNumber == 443)
                {
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                }
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                    currentDeviceAddress = ipAddress.Remove((ipAddress.Length) - (ipAddress.Split(':').Last().Count())) + 1;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                TraceFactory.Logger.Info("Installing the print driver with Protocol: {0} and PortNumber: {1}".FormatWith(printProtocol, portNumber));
                printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, outHostName);

                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                TraceFactory.Logger.Info("Printing the File with IPAddress: {0}".FormatWith(ipAddress));
                printer.Print(files);

                TraceFactory.Logger.Info("Changing Printer IP Address to:{0}".FormatWith(currentDeviceAddress));

                if (!isIPv6)
                {
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, IPAddress.Parse(currentDeviceAddress)))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!EwsWrapper.Instance().SetManualIPv6Address(true, currentDeviceAddress))
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Info("Reinstalling the driver with changed IPAddress: {0}".FormatWith(currentDeviceAddress));
                printer = PrinterFactory.Create(family, IPAddress.Parse(currentDeviceAddress));
                printer.Install(IPAddress.Parse(currentDeviceAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, outHostName);

                TraceFactory.Logger.Info("Sending Print Job after reinstalling the Driver");
                if (printer.Print(files))
                {
                    TraceFactory.Logger.Info("Print Job is Success");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to Print");
                    return false;
                }
            }
            finally
            {
                TraceFactory.Logger.Info("Performing Post requisite: Setting back DHCP Config method");
                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);
                if (isIPv6)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.Ipv4Address);
                    EwsWrapper.Instance().SetManualIPv6Address(false);
                }
                else
                {
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address));
                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.Ipv4Address);
                }
                if (portNumber == 443)
                {
                    IPPS_PostRequisite(printerHost);
                }
            }
        }

        /// <summary>
        ///  Verify printing with host name, after host name change and re-installation, after host name modification
        ///-	Install the printer using host name
        ///-	Send print jobs, printing should be successful
        ///-	Change the host name in the printer
        ///-	Modify the host name in the driver and printing should be successful
        ///-	Again change the host name in the printer, re install the printer using new host name
        ///-	Send few print jobs and printing should be successful.
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool PrintOnHostNameChangeAndReinstall(ConnectivityPrintActivityData activityData, Printer.Printer.PrintProtocol printProtocol, int portNumber, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            string hostName = string.Empty;
            string currentDeviceAddress = string.Empty;
            string outHostName = string.Empty;
            string addressType = string.Empty;
            string printerHost = string.Empty;

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }
            try
            {
                if (portNumber == 443)
                {
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                }
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                hostName = printer.HostName;
                TraceFactory.Logger.Info("Installing the print driver with Protocol: {0},PortNumber: {1} and HostName: {2}".FormatWith(printProtocol, portNumber, hostName));
                printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, hostName);

                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                TraceFactory.Logger.Info("Sending Print Jobs with IPAddress: {0}".FormatWith(ipAddress));
                printer.Print(files);

                TraceFactory.Logger.Info("Changing Printer hostName to:{0}".FormatWith("DefaultHostName"));
                EwsWrapper.Instance().SetHostname("DefaultHostName");

                //TODO: Editing Printer HostName in driver
                TraceFactory.Logger.Info("Reinstalling the driver with host name: {0}".FormatWith("DefaultHostName"));
                printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, "DefaultHostName");

                TraceFactory.Logger.Info("Sending print job after reinstalling driver");
                if (printer.Print(files))
                {
                    TraceFactory.Logger.Info("Print Job is Success");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to Print");
                    return false;
                }
            }
            finally
            {
                TraceFactory.Logger.Info("Performing Post requisite: Setting back the default host name");
                if (portNumber == 443)
                {
                    IPPS_PostRequisite(hostName);
                }
            }
        }

        /// <summary>
        ///  Verify job pause of an active print job on the printer through Front Panel
        ///-	Install the printer 
        ///-	Send few print jobs
        ///-	Pause the job from the front panel.
        ///-	The printer should be paused state and no further jobs should be printed (all protocol)
        ///-	Resume the jobs from the front panel.
        ///-	Printer should start printing successfully.
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool VerifyJobPausethroughFrontPanel(ConnectivityPrintActivityData activityData, Printer.Printer.PrintProtocol printProtocol, int portNumber, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            IDevice device = DeviceFactory.Create(activityData.Ipv4Address);
            string addressType = string.Empty;
            string outHostName = string.Empty;
            string printerHostName = string.Empty;
            if (portNumber == 443)
            {
                printerHostName = EwsWrapper.Instance().GetHostname();
            }
            TraceFactory.Logger.Info("***Sending the Print jobs from location :{0}****".FormatWith("Continuous job files"));
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.ContinousFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Pause job test failed. No files available.");
                return false;
            }

            try
            {
                if (portNumber == 443)
                {
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                }
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                TraceFactory.Logger.Info("Installing the print driver with Protocol: {0} and PortNumber: {1}".FormatWith(printProtocol, portNumber));
                if (!printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, outHostName))
                {
                    TraceFactory.Logger.Info("Failed to Install the Print Driver");
                    return false;
                }

                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;
                //Neha added this part here check 
                Thread printerStatus = new Thread(() => printingStatus(printer));
                printerStatus.Start();
                PrintJobPauseFrontPanel(device, "Pause");
                TraceFactory.Logger.Info("************** Sending Print Job***************8");
                printer.PrintAsynchronously(files);
               
                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

               // TraceFactory.Logger.Info("Sending Print Job");
               // printer.PrintAsynchronously(files);

              //  TraceFactory.Logger.Info("Pausing the Print Job through Front Panel");
             //   PrintJobPauseFrontPanel(device, "Pause");

                TraceFactory.Logger.Info("Validating the Print Job");
                if (printer.PrinterStatus == PrinterStatus.Printing)
                {
                    TraceFactory.Logger.Info("Printing successfully, even after the job is paused through front panel");
                    return false;
                }

                if (printer.NumberOfJobsInPrintQueue == 0)
                {
                    TraceFactory.Logger.Info("No jobs in Print queue, even after the job is paused");
                    return false;
                }

                TraceFactory.Logger.Info("Since the Job is Paused, no jobs printed");

                //Thread printerStatusnew = new Thread(() => printingStatus(printer));
                //printerStatus.Start();
                TraceFactory.Logger.Info("Resuming back the Printer through Front Panel");
                PrintJobPauseFrontPanel(device, "Resume");

                Thread.Sleep(TimeSpan.FromMinutes(2));

                // Validating the Jobs in Print Queue
                while (printer.NumberOfJobsInPrintQueue != 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(20));
                }
                printerStatus.Abort();

                TraceFactory.Logger.Info("Since the Job is Resumed back, successfully printed");
                return true;
            }
            finally
            {
                if (portNumber == 443)
                {
                    IPPS_PostRequisite(printerHostName);
                }
            }
        }

        /// <summary>
        ///  Verify job cancel of an active print job on the printer through Front Panel
        ///-	Install the printer 
        ///-	Send few print jobs
        ///-	Cancel the job from the front panel.
        ///-	The printer should be canceled state and no further jobs should be printed (all protocol)        
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool VerifyJobCancelthroughFrontPanel(ConnectivityPrintActivityData activityData, Printer.Printer.PrintProtocol printProtocol, int portNumber, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            IDevice device = DeviceFactory.Create(activityData.Ipv4Address);
            string addressType = string.Empty;
            string outHostName = string.Empty;
            string printerHostName = string.Empty;
            if (portNumber == 443)
            {
                printerHostName = EwsWrapper.Instance().GetHostname();
            }
            TraceFactory.Logger.Info("Cancel from front panel Folder");
            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.CancelFromFrontPanel);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }

            try
            {
                if (portNumber == 443)
                {
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                }

                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                TraceFactory.Logger.Info("Installing the print driver with Protocol: {0} and PortNumber: {1}".FormatWith(printProtocol, portNumber));
                if (!printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, outHostName))
                {
                    TraceFactory.Logger.Info("Failed to Install the Print Driver");
                    return false;
                }

                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                Thread printerStatus = new Thread(() => printingStatus(printer));

                printerStatus.Start();


                TraceFactory.Logger.Info("Sending Print Jobs");
                DateTime printstarttime = EwsWrapper.Instance().GetDateTime();
                printer.PrintAsynchronously(files);

                if (!PrintJobCancelFrontPanel(device))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(4));
                printerStatus.Abort();
                if (EwsWrapper.Instance().IsOmniOpus)
                {
                    if (!ValidatecanceljobOmniOpusFrontPanel(device, printstarttime))
                    {
                        TraceFactory.Logger.Info("Test Failed.");
                        return false;
                    }
                }
                TraceFactory.Logger.Info("Validating the Printer status");
                if (printer.PrinterStatus == PrinterStatus.Printing)
                {
                    TraceFactory.Logger.Info("Printing successfully, even after the job is canceled through front panel");
                    return false;
                }

                TraceFactory.Logger.Info("Since the Print Job is canceled through Front Panel, Print job is not success");
                return true;
            }
            finally
            {
                if (portNumber == 443)
                {
                    IPPS_PostRequisite(printerHostName);
                }
            }
        }

        /// <summary>
        ///  Verify job cancel of an active print job on the printer through Front Panel[FTP]
        ///-	Install the printer 
        ///-	Send few print jobs
        ///-	Cancel the job from the front panel.
        ///-	The printer should be canceled state and no further jobs should be printed (all protocol)        
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool VerifyJobCancelthroughFrontPanel_FTP(ConnectivityPrintActivityData activityData, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            IDevice device = DeviceFactory.Create(activityData.Ipv4Address);
            string addressType = string.Empty;

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.FTPContinousFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }
            if (isIPv6)
            {
                TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                ipAddress = activityData.Ipv6StateFullAddress;
            }
            else
            {
                ipAddress = activityData.Ipv4Address;
            }
            // Print Start page and send all jobs to PrintQueue
            if (!_activityData.PaperlessMode)
            {
                printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
            }

            Thread printerStatus = new Thread(() => PrintJobCancelFrontPanel(device));
            printerStatus.Start();

            TraceFactory.Logger.Info("Sending Print Jobs");
            printer.PrintWithFtpAsynchronously(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files.FirstOrDefault());

            Thread.Sleep(TimeSpan.FromMinutes(1));

            TraceFactory.Logger.Info("Validating the Printer status");
            if (printer.PrinterStatus == PrinterStatus.Printing)
            {
                TraceFactory.Logger.Info("Printing successfully, even after the job is canceled through front panel");
                return false;
            }

            TraceFactory.Logger.Info("Since the Print Job is canceled through Front Panel, Print job is not success");
            return true;
        }

        /// <summary>
        ///  Verify printing after IP change and re-connection
        ///-	FTP to the Printer
        ///-	Send few Large size jobs to the Printer
        ///-	Change the IP Address of the Printer
        ///-    Reconnect to the Printer with new IP using FTP
        ///-    Send few more Print Jobs
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool PrintAfterIPChangeAndReconnection_FTP(ConnectivityPrintActivityData activityData, int testNo, bool isIPv6 = false)
        {
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            string currentDeviceAddress = string.Empty;
            string addressType = string.Empty;

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.FTPContinousFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }

            currentDeviceAddress = NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.Ipv4Address).GetSubnetMask(), IPAddress.Parse(activityData.Ipv4Address)).ToString();
            try
            {
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.PrintWithFtp(IPAddress.Parse(activityData.Ipv4Address), string.Empty, string.Empty, CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                EwsWrapper.Instance().SetAdvancedOptions();
                if (!printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files.FirstOrDefault()))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Changing Printer IP Address to:{0}".FormatWith(currentDeviceAddress));
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, IPAddress.Parse(currentDeviceAddress)))
                {
                    return false;
                }

                //Reconnecting to the Printer with new IP using FTP
                printer = PrinterFactory.Create(family, IPAddress.Parse(currentDeviceAddress));
                return printer.PrintWithFtp(IPAddress.Parse(currentDeviceAddress), string.Empty, string.Empty, files.FirstOrDefault());
            }
            finally
            {
                TraceFactory.Logger.Info("Performing Post requisite: Setting back DHCP Configuration method");
                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address));
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.Ipv4Address);
            }
        }

        /// <summary>
        ///  Verify printing across Subnets and Routers
        ///-	Install the Printer
        ///-	Send few print jobs
        ///-	Print should go through successfully across subnets
        ///-Note: Since there is no infra ready for more than one routers, across Routers not handled
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool PrintAcrossSubnets(ConnectivityPrintActivityData activityData, Printer.Printer.PrintProtocol printProtocol, int portNumber, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string serverIP = Printer.Printer.GetDHCPServerIP(IPAddress.Parse(activityData.Ipv4Address)).ToString();
            string ipAddress = string.Empty;
            string outHostName = string.Empty;
            string clientNetworkName = string.Empty;
            string addressType = string.Empty;

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles);
            string printerHostName = EwsWrapper.Instance().GetHostname();
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }
            try
            {
                if (portNumber == 443)
                {
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                }
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, outHostName);

                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }
                // Keeping the printer in a different subnet by disabling the network .
                TraceFactory.Logger.Info("Disabling the network corresponding to DHCP server: {0}".FormatWith(serverIP));
                // Get the NIC having IP address in the server IP range so that the NIC can be disabled.
                clientNetworkName = CtcUtility.GetClientNetworkName(serverIP);
                NetworkUtil.DisableNetworkConnection(clientNetworkName);

                Thread.Sleep(TimeSpan.FromMinutes(1));
                // Sending Print Job from different[Secondary] subnet
                return printer.Print(files);
            }
            finally
            {
                Thread.Sleep(TimeSpan.FromMinutes(2));
                TraceFactory.Logger.Info("Performing Post requisite: Enabling back the disabled nic[Primary]");
                NetworkUtil.EnableNetworkConnection(clientNetworkName);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (portNumber == 443)
                {
                    IPPS_PostRequisite(printerHostName);
                }
            }
        }

        /// <summary>
        ///  Verify printing across Subnets and Routers[FTP]
        ///-	Install the Printer
        ///-	Send few print jobs
        ///-	Print should go through successfully across subnets
        ///-Note: Since there is no infra ready for more than one routers, across Routers not handled
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool PrintAcrossSubnets_FTP(ConnectivityPrintActivityData activityData, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string serverIP = Printer.Printer.GetDHCPServerIP(IPAddress.Parse(activityData.Ipv4Address)).ToString();
            string ipAddress = string.Empty;
            string clientNetworkName = string.Empty;
            string addressType = string.Empty;

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.FTPContinousFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }
            try
            {
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                // Keeping the printer in a different subnet by disabling the network .
                TraceFactory.Logger.Info("Disabling the network corresponding to DHCP server: {0}".FormatWith(serverIP));
                // Get the NIC having IP address in the server IP range so that the NIC can be disabled.
                clientNetworkName = CtcUtility.GetClientNetworkName(serverIP);
                NetworkUtil.DisableNetworkConnection(clientNetworkName);

                // Sending Print Job from different[Secondary] subnet
                return printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files.FirstOrDefault());
            }
            finally
            {
                TraceFactory.Logger.Info("Performing Post requisite: Enabling back the disabled nic[Primary]");
                NetworkUtil.EnableNetworkConnection(clientNetworkName);
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        /// <summary>
        ///  Verify printer behavior when a firmware upgrade is performed while printing 
        ///-	Install the printer 
        ///-	Send print jobs using the spooler. 
        ///-	Upgrade the firmware.
        ///-	Spooler returns to online state post reboot and the pending jobs print successfully
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool VerifyPrinterBehaviourduringFirmwareUpgrade(ConnectivityPrintActivityData activityData, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string firmwareUpgradeFilePath = FIRMWAREBASELOCATION + Path.DirectorySeparatorChar + activityData.ProductFamily + Path.DirectorySeparatorChar + activityData.ProductName + Path.DirectorySeparatorChar + "UpgradeFile";
            string firmwareDowngradeFilePath = FIRMWAREBASELOCATION + Path.DirectorySeparatorChar + activityData.ProductFamily + Path.DirectorySeparatorChar + activityData.ProductName + Path.DirectorySeparatorChar + "DowngradeFile";
            DirectoryInfo upgradeFirmwareDir = new DirectoryInfo(firmwareUpgradeFilePath);
            DirectoryInfo downgradeFirmwareDir = new DirectoryInfo(firmwareDowngradeFilePath);
            string ipAddress = string.Empty;
            string addressType = string.Empty;

            TraceFactory.Logger.Info("Downgrading the firmware as part of Prerequisite");
            EwsWrapper.Instance().InstallFirmware(downgradeFirmwareDir.GetFiles()[0].FullName);
            Thread.Sleep(TimeSpan.FromMinutes(6));

            if (isIPv6)
            {
                TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                ipAddress = activityData.Ipv6StateFullAddress;
            }
            else
            {
                ipAddress = activityData.Ipv4Address;
            }

            // Installing all the printers before upgrading Firmware as required by the test case
            TraceFactory.Logger.Info("Installing the print driver with Protocol: {0} and PortNumber: {1}".FormatWith(Printer.Printer.PrintProtocol.RAW, 9100));
            printer.Install(IPAddress.Parse(ipAddress), Printer.Printer.PrintProtocol.RAW, activityData.DriverPackagePath, activityData.DriverModel, 9100);

            TraceFactory.Logger.Info("Installing the print driver with Protocol: {0} and PortNumber: {1}".FormatWith(Printer.Printer.PrintProtocol.LPD, 515));
            printer.Install(IPAddress.Parse(ipAddress), Printer.Printer.PrintProtocol.LPD, activityData.DriverPackagePath, activityData.DriverModel, 515);

            TraceFactory.Logger.Info("Installing the print driver with Protocol: {0}".FormatWith(Printer.Printer.PrintProtocol.WSP));
            printer.NotifyWSPrinter += printer_NotifyWSPAddition;
            if (printer.Install(IPAddress.Parse(ipAddress), Printer.Printer.PrintProtocol.WSP, activityData.DriverPackagePath, activityData.DriverModel))
            {
                MessageBox.Show("WS Printer was added successfully.", "WS Printer Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("WS Printer was not added successfully. All WS Print related tests will fail.", "WS Printer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Subscribing for Print Queue Event
            printer.PrintQueueError += printer_PrintQueueError;

            // Print Start page and send all jobs to PrintQueue
            if (!_activityData.PaperlessMode)
            {
                printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
            }

            // Upgrading the Firmware
            EwsWrapper.Instance().InstallFirmware(upgradeFirmwareDir.GetFiles()[0].FullName);
            Thread.Sleep(TimeSpan.FromMinutes(6));

            return PrintWithAllProtocols(ipAddress, activityData.ProductFamily, activityData.DriverPackagePath, activityData.DriverModel, testNo, isIPv6);
        }

        /// <summary>
        ///  Verify printer behavior when navigating through Front Panel while printing
        ///-	Install the printer (all protocol)
        ///-	Send few jobs to the printer
        ///-	While printing, navigate the menus in the front panel.
        ///-	Printing should be successful, printer should not hang/crash.
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool VerifyPrinterBehaviourduringFrontPanelNavigation(ConnectivityPrintActivityData activityData, Printer.Printer.PrintProtocol printProtocol, int portNumber, int testNo, bool isIPv6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            string outHostName = string.Empty;
            IDevice device = DeviceFactory.Create(activityData.Ipv4Address);
            string addressType = string.Empty;
            string printerHostName = string.Empty;
            if (portNumber == 443)
            {
                printerHostName = EwsWrapper.Instance().GetHostname();
            }

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }
            try
            {
                if (portNumber == 443)
                {
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                }
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                TraceFactory.Logger.Info("Installing the print driver with Protocol: {0} and PortNumber: {1}".FormatWith(printProtocol, portNumber));
                if (!printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, outHostName))
                {
                    TraceFactory.Logger.Info("Failed to Install the Print Driver");
                    return false;
                }

                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                TraceFactory.Logger.Info("Sending Print Job");
                printer.PrintAsynchronously(files);

                TraceFactory.Logger.Info("Disturbing the Print Job by navigating the Front Panel to disable IPSec option");
                FrontPanelNavigation(device, false);

                Thread.Sleep(TimeSpan.FromMinutes(1));
                TraceFactory.Logger.Info("Again sending Print Job after navigation");
                Thread printerStatus = new Thread(() => printingStatus(printer));
                printerStatus.Start();
                printer.PrintAsynchronously(files);
                Thread.Sleep(TimeSpan.FromMinutes(2));

                // Validating the Jobs in Print Queue
                while (printer.NumberOfJobsInPrintQueue != 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(20));
                }
                printerStatus.Abort();

                TraceFactory.Logger.Info("Successfully Printed the Job even after disturbing the Front Panel by navigating to disable IPSec option");
                return true;
            }
            finally
            {
                if (portNumber == 443)
                {
                    IPPS_PostRequisite(printerHostName);
                }
            }
        }

        /// <summary>
        ///  Verify printing after IP change and re-installation
        ///-	Install the printer (all 9100, LPD)        
        ///-	Change the ip address of the printer
        ///-	Modify the ipaddress of the Printer
        ///-    Send few print jobs
        ///-    Printing should be successful
        /// </summary>
        /// <param name="activityData"><see cref="ConnectivityPrintActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public bool PrintOnIPChangeAndEditPrinter(ConnectivityPrintActivityData activityData, Printer.Printer.PrintProtocol printProtocol, int portNumber, int testNo, bool isIPv6 = false)
        {
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.Ipv4Address));
            string ipAddress = string.Empty;
            string currentDeviceAddress = string.Empty;
            string outHostName = string.Empty;

            string[] files = GetFiles(_activityData.DocumentsPath, FolderType.SimpleFiles);
            string printerHostName = EwsWrapper.Instance().GetHostname();
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Simple Printing failed. No files available.");
                return false;
            }
            currentDeviceAddress = NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.Ipv4Address).GetSubnetMask(), IPAddress.Parse(activityData.Ipv4Address)).ToString();
            try
            {
                if (portNumber == 443)
                {
                    if (!IPPS_Prerequisite(IPAddress.Parse(_activityData.Ipv4Address), out outHostName))
                    {
                        return false;
                    }
                }
                if (isIPv6)
                {
                    TraceFactory.Logger.Info("Statefull IPAddress has been taken as IPV6 address to execute the test case");
                    ipAddress = activityData.Ipv6StateFullAddress;
                    currentDeviceAddress = ipAddress.Remove((ipAddress.Length) - (ipAddress.Split(':').Last().Count())) + 1;
                }
                else
                {
                    ipAddress = activityData.Ipv4Address;
                }

                TraceFactory.Logger.Info("Installing the print driver with Protocol: {0} and PortNumber: {1}".FormatWith(printProtocol, portNumber));
                if (!printer.Install(IPAddress.Parse(ipAddress), printProtocol, activityData.DriverPackagePath, activityData.DriverModel, portNumber, outHostName))
                {
                    TraceFactory.Logger.Info("Failed to install print driver");
                    return false;
                }

                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                // Print Start page and send all jobs to PrintQueue
                if (!_activityData.PaperlessMode)
                {
                    printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                }

                TraceFactory.Logger.Info("Printing the File with IPAddress: {0}".FormatWith(ipAddress));
                printer.Print(files);

                TraceFactory.Logger.Info("Changing Printer IP Address to:{0}".FormatWith(currentDeviceAddress));
                if (!isIPv6)
                {
                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, IPAddress.Parse(currentDeviceAddress)))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!EwsWrapper.Instance().SetManualIPv6Address(true, currentDeviceAddress))
                    {
                        return false;
                    }
                }

                if (!EditPrinterIPaddress(ipAddress, currentDeviceAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Sending Print Job after modifying the printer IPaddress");

                LocalPrintServer localPrintServer = new LocalPrintServer();
                PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();

                PrintSystemJobInfo myPrintJob = defaultPrintQueue.AddJob("SimpleFiles");
                StreamReader myStreamReader = new StreamReader(files.FirstOrDefault());

                // Write a Byte buffer to the JobStream and close the stream
                Stream anotherStream = myPrintJob.JobStream;
                Byte[] anotherByteBuffer = UnicodeEncoding.Unicode.GetBytes(myStreamReader.ReadToEnd());
                anotherStream.Write(anotherByteBuffer, 0, anotherByteBuffer.Length);
                anotherStream.Close();
                myPrintJob.Refresh();

                if (myPrintJob.IsPrinting || myPrintJob.IsDeleted)
                {
                    TraceFactory.Logger.Info("Successfully printed the simple jobs after editing printer ipaddress");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to print the simple jobs after editing the printer ipaddress");
                    return false;
                }
            }
            finally
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
                TraceFactory.Logger.Info("Performing Post requisite: Setting back DHCP Configuration method");
                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address));
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.Ipv4Address);

                EditPrinterIPaddress(currentDeviceAddress, ipAddress);
                if (portNumber == 443)
                {
                    IPPS_PostRequisite(printerHostName);
                }
            }
        }

        #endregion

        #endregion

        #region Event Handler

        /// <summary>
        /// Notify user on PrintQueue error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printer_PrintQueueError(object sender, PrintJobDataEventArgs e)
        {
            // Check added for object type
            if (null != sender)
            {
                Printer.Printer printer = sender as Printer.Printer;
                Thread confirmationMsgBoxThread = new Thread(new ThreadStart(CheckUserConfirmation));
                TimeSpan timeOut = new TimeSpan(0, 30, 0);

                confirmationMsgBoxThread.Start();
                DateTime startTime = DateTime.Now;

                do
                {
                    // Case 1: Wait for timeOut and kill the thread if no response from the user
                    if (startTime.Add(timeOut) <= DateTime.Now)
                    {
                        // Used to notify that PrintQueue error was not cleared
                        _abort = true;

                        KillMessageBox(confirmationMsgBoxThread);
                        return;
                    }
					//changin the time t0 60 sec. Wait from 30 seconds and check the status of PrintQueue
					Thread.Sleep(TimeSpan.FromSeconds(60));
                    //Wait from 30 seconds and check the status of PrintQueue
                    //Thread.Sleep(TimeSpan.FromSeconds(30));

                    // Case 2: If PrintQueue error is rectified from user or if PrintQueue recovers from error, kill message box
                    if (!printer.IsPrintQueueInError)
                    {
                        KillMessageBox(confirmationMsgBoxThread);
                        return;
                    }

                } while (!_abort);

                // If user has clicked Cancel button, abort the thread
                if (!_abort)
                {
                    confirmationMsgBoxThread.Abort();
                }
            }
        }

        /// <summary>
        /// Notify user on job printing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printer_PrintJobPrinting(object sender, PrintJobDataEventArgs e)
        {
            // Check added for object type
            if (null != sender)
            {
                if (e.Job.Document.Contains(Path.GetFileNameWithoutExtension(_cancelFileName)))
                {
                    Printer.Printer printer = sender as Printer.Printer;
                    if (printer.CancelPrintJob(e.Job.Id))
                    {
                        TraceFactory.Logger.Info("Print job : {0} is cancelled successfully.".
                            FormatWith(Path.GetFileNameWithoutExtension(_cancelFileName)));
                    }
                }
            }
        }

		/// <summary>
		/// Notify user on job spooling
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void printer_PrintJobSpooling(object sender, PrintJobDataEventArgs e)
		{
			// Document name will not be updated as soon as Spooling event occurs
			// Wait for a second for document name to be refreshed
			Thread.Sleep(TimeSpan.FromSeconds(20));

            // Check added for object type
            if (null != sender)
			{
                try
                {
                    if (e.Job.Document.Contains(Path.GetFileNameWithoutExtension(_cancelFileName)))
                    {
                        Printer.Printer printer = sender as Printer.Printer;
                        if (printer.CancelPrintJob(e.Job.Id))
                        {
                            TraceFactory.Logger.Info("Print job : {0} is cancelled successfully.".
                                FormatWith(Path.GetFileNameWithoutExtension(_cancelFileName)));
                        }
                    }
                }
                catch(Exception spoolerError)
                {
                    TraceFactory.Logger.Info("Exception caught:{0}".FormatWith(spoolerError.Message));
                }
            }
        }

        /// <summary>
        /// Notify user on FTP job printing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printer_FTPJobPrinting(object sender, FtpPrintingEventArgs e)
        {
            if (e.TotalBytes / 2 <= e.SentBytes)
            {
                e.Abort = true;
            }
        }

		/// <summary>
		/// Notify user on printing to perform hose break
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void printer_PrintJobPrinting_HoseBreak(object sender, PrintJobDataEventArgs e)
		{
			// Check added for object type
			if (null != sender)
			{                                		                
				Printer.Printer printer = sender as Printer.Printer;
				if (!_jobCompleted)
				{
					 try
                    {
                       MessageBox.Show("in the hose break-try");
                        if (_networkSwitch.DisablePort(_activityData.SwitchPortNumber))
                        {
                            Thread.Sleep(TimeSpan.FromMinutes(1));                        

                           MessageBox.Show("Disabled port ");
                            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.Ipv4Address), TimeSpan.FromSeconds(10)))
                            {
                               MessageBox.Show(" Port");
                                TraceFactory.Logger.Info("Switch IP : {0} with port no : {1} is disabled.".FormatWith(_activityData.SwitchIPAddress, _activityData.SwitchPortNumber));
                                TraceFactory.Logger.Info("Waiting for timeout for {0} seconds.".FormatWith(_tcpipTimeOut));
                                Thread.Sleep(TimeSpan.FromSeconds(_tcpipTimeOut));
                                TraceFactory.Logger.Info("Enabled port ***************");
                                _networkSwitch.EnablePort(_activityData.SwitchPortNumber);
                                TraceFactory.Logger.Info("Switch IP : {0} with port no : {1} is enabled.".FormatWith(_activityData.SwitchIPAddress, _activityData.SwitchPortNumber));
                                _jobCompleted = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Debug("Exception occured *** while hose break: ".FormatWith(ex.Message));
                        //do noting 
                    }
					//if (_networkSwitch.DisablePort(_activityData.SwitchPortNumber))
					//{
                       // Thread.Sleep(TimeSpan.FromSeconds(25));                        
					//	//if (!printer.PingUntilTimeout(IPAddress.Parse(_activityData.Ipv4Address), TimeSpan.FromSeconds(10)))
                      //  if(!NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.Ipv4Address), TimeSpan.FromSeconds(10)))
					//	{
					//		TraceFactory.Logger.Info("Switch IP : {0} with port no : {1} is disabled.".FormatWith(_activityData.SwitchIPAddress, _activityData.SwitchPortNumber));
					//		TraceFactory.Logger.Info("Waiting for timeout for {0} seconds.".FormatWith(_tcpipTimeOut));
					//		Thread.Sleep(TimeSpan.FromSeconds(_tcpipTimeOut));
					//		_networkSwitch.EnablePort(_activityData.SwitchPortNumber);
					//		TraceFactory.Logger.Info("Switch IP : {0} with port no : {1} is enabled.".FormatWith(_activityData.SwitchIPAddress, _activityData.SwitchPortNumber));
					//		_jobCompleted = true;
					//	}
				//	}
				}			
			}
		}

        /// <summary>
        /// Notify user on job completion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printer_PrintJobFinished_HoseBreak(object sender, PrintJobDataEventArgs e)
        {
            // Check added for object type
            if (null != sender)
            {
                if (e.Job.Document.Contains(Path.GetFileNameWithoutExtension(_hosebreakFileName)))
                {
                    _jobCompleted = true;
                }
            }
        }

        /// <summary>
        /// Notify user on job printing to pause job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printer_PrintJobPrinting_PauseJob(object sender, PrintJobDataEventArgs e)
        {
            // Check added for object type
            if (null != sender)
            {
                if (e.Job.Document.Contains(Path.GetFileNameWithoutExtension(_pauseFileName)))
                {
                    Printer.Printer printer = sender as Printer.Printer;

                    if (printer.PausePrintJob((int)e.Job.Id))
                    {
                        TraceFactory.Logger.Info("Waiting for {0} seconds after job pause.".FormatWith(PAUSE_JOB_TIMEOUT));
                        Thread.Sleep(TimeSpan.FromSeconds(PAUSE_JOB_TIMEOUT));
                        TraceFactory.Logger.Info("Resuming paused job.");
                        printer.ResumeJob((int)e.Job.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Notify user on job completion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printer_PrintJobFinished_PauseJob(object sender, PrintJobDataEventArgs e)
        {
            // Check added for object type
            if (null != sender)
            {
                if (e.Job.Document.Contains(Path.GetFileNameWithoutExtension(_pauseFileName)))
                {
                    _jobCompleted = true;
                }
            }
        }

        /// <summary>
        /// Notify user on FTP job printing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printer_FTPJobPrinting_HoseBreak(object sender, FtpPrintingEventArgs e)
        {
            // Check added for object type
            try
            {
                if (null != sender)
                {
                    // Perform hose break when half the job is transferred 
                    if (e.TotalBytes / 2 <= e.SentBytes)
                    {
                        Printer.Printer printer = sender as Printer.Printer;
                        INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(_activityData.SwitchIPAddress));
                        if (networkSwitch.DisablePort(_activityData.SwitchPortNumber))
                        {
                            if (!printer.PingUntilTimeout(IPAddress.Parse(_activityData.Ipv4Address), TimeSpan.FromSeconds(10)))
                            {
                                TraceFactory.Logger.Info("Switch IP : {0} with port no : {1} is disabled.".FormatWith(_activityData.SwitchIPAddress, _activityData.SwitchPortNumber));
                                TraceFactory.Logger.Info("Waiting for timeout for {0} seconds.".FormatWith(_tcpipTimeOut));
                                Thread.Sleep(TimeSpan.FromSeconds(_tcpipTimeOut));

                                // Unsubscribe to event so as to avoid multiple hose break
                                printer.FTPJobPrinting -= printer_FTPJobPrinting_HoseBreak;
                                networkSwitch.EnablePort(_activityData.SwitchPortNumber);
                                TraceFactory.Logger.Info("Switch IP : {0} with port no : {1} is enabled.".FormatWith(_activityData.SwitchIPAddress, _activityData.SwitchPortNumber));
                                _jobCompleted = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception occured : {0}".FormatWith(ex.Message));
            }
        }

        void printer_FTPJobAfterIPChange(object sender, FtpPrintingEventArgs e)
        {
            // Check added for object type
            if (null != sender)
            {
                _jobCompleted = true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get all files available in folder path
        /// </summary>
        /// <param name="folderPath">Path from where files need to be retrieved</param>
        /// <param name="folder"><see cref=" FolderType"/></param>
        /// <returns>Collection of files</returns>
        private static string[] GetFiles(string folderPath, FolderType folder)
        {
            string relativePath = Path.Combine(folderPath, Enum<FolderType>.Value(folder));
            if (Directory.Exists(relativePath))
            {
              if(Directory.GetFiles(relativePath)==null)
                {
                    TraceFactory.Logger.Info("No files found. TRying once again to access the location");
                    return Directory.GetFiles(relativePath);
                }
                return Directory.GetFiles(relativePath);
            }
            else
            {
                TraceFactory.Logger.Info("Directory {0} is not found.".FormatWith(relativePath));
                return null;
            }
        }

        /// <summary>
        /// Get user input on PrintQueue error
        /// </summary>
        private void CheckUserConfirmation()
        {
            DialogResult result = MessageBox.Show("Make sure Printer is in Ready state without any warnings.\nSelect one of the options below: \n 1. Retry to try once again.\n 2. Cancel to Fail the current test.\n ", PRINTER_ERROR_WINDOW_TITLE, System.Windows.Forms.MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);

            if (result == DialogResult.Retry)
            {
                _abort = false;
            }

            if (result == DialogResult.Cancel)
            {
                _abort = true;
            }

        }

        /// <summary>
        /// Kill the Windows message box and stop the thread
        /// </summary>
        /// <param name="confirmationMsgBoxThread"></param>
        private static void KillMessageBox(Thread confirmationMsgBoxThread)
        {
            PopupAssassin.KillWindow(PRINTER_ERROR_WINDOW_TITLE);
            confirmationMsgBoxThread.Abort();
        }

        /// <summary>
        /// Sets IPv6 Address type and IPv6 Address based on the IPv6 address type
        /// </summary>
        /// <param name="address">IPv6 address type</param>
        /// <param name="addressType">Address type</param>
        /// <param name="ipAddress">IPv6 address</param>
        /// <returns>Returns true if the address is one of the valid type else returns false</returns>
        private bool SetIPAddress(Ipv6AddressTypes address, ref string addressType, ref string ipAddress)
        {
            bool result = false;

            switch (address)
            {
                case Ipv6AddressTypes.LinkLocal:
                    addressType = "LinkLocal";
                    ipAddress = _activityData.Ipv6LinkLocalAddress;
                    result = true;
                    break;

                case Ipv6AddressTypes.Stateful:
                    addressType = "Statefull";
                    ipAddress = _activityData.Ipv6StateFullAddress;
                    result = true;
                    break;

                case Ipv6AddressTypes.Stateless:
                    addressType = "Stateless";
                    ipAddress = _activityData.Ipv6StatelessAddress;
                    result = true;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Prerequistes for IPPS Protocol related testcases
        /// 1. Installation of CA and ID certificates in Printer and Client.[Pre Generated Certificate should have hostname]
        /// 2. The hostname of the certificate has to be set in the Printer and in the server.
        /// 3. WinServerIP Address has to be set in the printer and in the server.
        /// 4. Make sure the Win server is up and running in DHCP server.       
        /// </summary>
        /// <param name="serverIP">DHCP Server IP Address</param>              
        /// <returns>Returns true prerequisites passed else returns false</returns>
        private bool IPPS_Prerequisite(IPAddress printerIPAddress, out string outHostName)
        {
            string serverIP = Printer.Printer.GetDHCPServerIP(printerIPAddress).ToString();

            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(serverIP);
            string scopeIP = serviceMethod.Channel.GetDhcpScopeIP(serverIP);

            // Getting hostname from the certificate
            CertificateDetails certParameters = CertificateUtility.GetCertificateDetails(IDCERTIFICATEPATH, IDCERTIFICATE_PSWD);
            outHostName = certParameters.IssuedTo;

            // Installing CA and ID Certificate in Printer and in the client
            if (!EwsWrapper.Instance().InstallCACertificate(CACERTIFICATEPATH, false))
            {
                return false;
            }
            if (!EwsWrapper.Instance().InstallIDCertificate(IDCERTIFICATEPATH, IDCERTIFICATE_PSWD))
            {
                return false;
            }
            if (!CtcUtility.InstallCACertificate(CACERTIFICATEPATH))
            {
                return false;
            }
            if (!CtcUtility.InstallIDCertificate(IDCERTIFICATEPATH, IDCERTIFICATE_PSWD))
            {
                return false;
            }

            // Setting hostname and winserver ip address in printer and in the server
            if (!EwsWrapper.Instance().SetHostname(outHostName))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetPrimaryWinServerIP(serverIP))
            {
                return false;
            }

            if (!serviceMethod.Channel.SetWinsServer(serverIP, scopeIP, serverIP))
            {
                return false;
            }

            //check whether the Wins is up and running in DHCP Server
            return CtcUtility.StartService("WINS", serverIP);
        }

        /// <summary>
        /// PostRequisites for IPPS Protocol related testcases
        /// 1. Uninstall CA and ID certificates in Printer and Client.               
        /// </summary>                     
        /// <returns>Returns true if postrequisites passed else returns false</returns>
        private bool IPPS_PostRequisite(string defaultHostName)
        {
            // Setting back the value for hostname
            EwsWrapper.Instance().SetHostname(defaultHostName);
            if (!EwsWrapper.Instance().UnInstallCACertificate(CACERTIFICATEPATH))
            {
                return false;
            }
            return EwsWrapper.Instance().UnInstallIDCertificate(IDCERTIFICATEPATH, IDCERTIFICATE_PSWD);
        }

        /// <summary>
        /// Prints with all Protocols
        /// </summary>
        /// <param name="folderPath">Path from where files need to be retrieved</param>
        /// <param name="folder"><see cref=" FolderType"/></param>
        /// <returns>Collection of files</returns>
        private bool PrintWithAllProtocols(string ipAddress, ProductFamilies family, string driverPath, string driverModel, int testNo, bool isIPv6)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), family.ToString()), IPAddress.Parse(ipAddress));

            string outHostName = string.Empty;
            BitArray results = new BitArray(3, true);
            bool returnResult = true;
            int resultIndex = 0;
            string printerHost = EwsWrapper.Instance().GetHostname();

            if (isIPv6)
            {
                TraceFactory.Logger.Info("Stateless IPAddress has been taken as IPV6 address to execute the test case");
                ipAddress = _activityData.Ipv6StatelessAddress;
            }

            if (!string.IsNullOrEmpty(ipAddress))
            {
                TraceFactory.Logger.Debug("Executing Printing test for IPv4 Address : {0} for Port P9100".FormatWith(ipAddress));
                printer.Install(IPAddress.Parse(ipAddress), Printer.Printer.PrintProtocol.RAW, driverPath, driverModel, 9100);
                results[resultIndex++] = printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));

                TraceFactory.Logger.Debug("Executing Printing test for IPv4 Address : {0} for LPD".FormatWith(ipAddress));
                printer.Install(IPAddress.Parse(ipAddress), Printer.Printer.PrintProtocol.LPD, driverPath, driverModel, 515);
                results[resultIndex++] = printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));

                TraceFactory.Logger.Debug("Executing Printing test for IPv4 Address : {0} for WSP".FormatWith(ipAddress));
                if (printer.Install(IPAddress.Parse(ipAddress), Printer.Printer.PrintProtocol.WSP, driverPath, driverModel))
                    results[resultIndex++] = printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
            }
            else
            {
                // This is required if IPv6 address is not available
                TraceFactory.Logger.Info("IPv6 address was not available.");
                results[resultIndex++] = false;
            }

            foreach (bool result in results)
            {
                returnResult &= result;
            }

            return returnResult;
        }

        /// <summary>
        /// Pausing Print Job through FrontPanel
        /// Administration --> JobStatus--> Pause
        /// </summary>
        /// <param name="device">printer</param>
        /// <param name="status">pause/resume</param>
        /// <returns>True if paused/resumed, else false.</returns>        
        private bool PrintJobPauseFrontPanel(IDevice device, string status)
        {
            TraceFactory.Logger.Info("{0} Print Job through Front Panel".FormatWith(status));
            try
            {
                if (EwsWrapper.Instance().IsOmniOpus)
                {
                    return PrintJobPauseFrontPanelOmniOpus(device, status);
                }
                //Navigating the control back to home page if the control panel has error screen			
                JediWindjammerDevice jd = device as JediWindjammerDevice;
                JediWindjammerControlPanel cp = jd.ControlPanel;

                //validating whether the current page is in Home
                //while (!cp.CurrentForm().Contains("Home") || !cp.CurrentForm().Contains("Pause"))
                //{
                //    //Pressing hide button to move the page to home
                //    cp.Press("mButton1");
                //}
               // MessageBox.Show("You are at home");
                if (cp.GetControls().Contains("mStopButton"))
                {
                    cp.Press("mStopButton");
                    //Thread.Sleep(TimeSpan.FromSeconds(15));
                }

                if (status == "Resume")
                {
                    if ((cp.GetControls().Contains("mStopButton")))
                    {
                        cp.Press("m_OKButton");
                    }
                }

                TraceFactory.Logger.Info("Successfully {0} the Print Job through Front Panel".FormatWith(status));
                return true;
            }
            catch
            {
                //returning false if the error message is other than missing Cartridge errors                
                return false;
            }
        }

        public static bool PrintJobPauseFrontPanelOmniOpus(IDevice device, string status)
        {
            using (JediOmniDevice _device = new JediOmniDevice(device.Address))
            {
                try
                {
                    _device.PowerManagement.Wake();
                    if (_device.ControlPanel.WaitForState(".hp-button-active-jobs", OmniElementState.Useable))
                    {

                        TraceFactory.Logger.Info("Clicked hp active jobs");
                        //Thread.Sleep(TimeSpan.FromSeconds(5));
                        // Press active print jobs button
                        _device.ControlPanel.PressWait(".hp-button-active-jobs", "#hpid-active-jobs-screen");

                       
                        if (status == "Pause")
                        {
                            // Click Pause
                            _device.ControlPanel.WaitForState("#hpid-button-pause-all", OmniElementState.Useable);
                            _device.ControlPanel.Press("#hpid-button-pause-all");
                            TraceFactory.Logger.Info("Pressed Pause All");
                        
                            //exit to home screen
                            if (_device.ControlPanel.CheckState("#hpid-active-jobs-exit", OmniElementState.Useable))
                            {
                                _device.ControlPanel.PressWait("#hpid-active-jobs-exit", "#hpid-pause-resume-popup");
                                _device.ControlPanel.CheckState("#hpid-button-stay-paused", OmniElementState.Useable);
                                _device.ControlPanel.Press("#hpid-button-stay-paused");
                                TraceFactory.Logger.Info("State paused");
                            }

                        }
                        else if (status == "Resume")
                        {
                            _device.ControlPanel.WaitForState("#hpid-button-resume-all", OmniElementState.Useable);
                            _device.ControlPanel.Press("#hpid-button-resume-all");
                            TraceFactory.Logger.Info("Resume all");
                            //exit to home screen
                            if (_device.ControlPanel.CheckState("#hpid-active-jobs-exit", OmniElementState.Useable))
                            {
                                _device.ControlPanel.Press("#hpid-active-jobs-exit");
                                TraceFactory.Logger.Info("Job Exit");
                            }

                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch
                {
                    return false;
                }
            }

        }

        public static bool ValidatepausejobOmniOpusFrontPanel(IDevice device)
        {
            string filename;
            string status = "";
            string time;
            //int count = 0;
            using (JediOmniDevice _device = new JediOmniDevice(device.Address))
            {

                _device.PowerManagement.Wake();
                _device.ControlPanel.ScrollToItem("#hpid-jobLog-homescreen-button");
                _device.ControlPanel.CheckState("#hpid-jobLog-homescreen-button", OmniElementState.Useable);
                _device.ControlPanel.Press("#hpid-jobLog-homescreen-button");
                Thread.Sleep(5000);

                var data = _device.ControlPanel.GetValue("#hpid-active-jobs-setting-list", "innerText", OmniPropertyType.Property);
                string joblog = data;
                //TraceFactory.Logger.Info("JobLog {0}".FormatWith(joblog));
                TraceFactory.Logger.Info("Copied the JOB LOG Data Successfully, tracking the Paused job");

                //Reading the job log from front panel to find out the name and time of the canceled job for validation.
                using (StringReader read = new StringReader(joblog))
                {
                    while ((filename = read.ReadLine()) != null)
                    {
                        if (filename.Contains("Pause"))
                        {

                            time = filename.Remove(0, 7);
                            status = status.Remove(0, 14);
                            //DateTime canceltime = Convert.ToDateTime(time);
                            TraceFactory.Logger.Info("File {0} is Paused at {1}".FormatWith(status, time));
                            return true;

                        }
                        status = read.ReadLine();
                        if (status.Contains("Pause"))
                        {
                            time = status.Remove(0, 7);
                            filename = filename.Remove(0, 14);
                            //DateTime canceltime = Convert.ToDateTime(time);
                            //if ((DateTime.Compare(canceltime, printstarttime) > 0))
                            //{
                            TraceFactory.Logger.Info("File {0} is Paused at {1}".FormatWith(status, time));
                            return true;
                            //}

                        }
                        //if (count == 5)
                        //    return false;
                        //count++;


                    }
                    return false;
                }

            }
        }

        /// <summary>
        /// Cancelling Print Job through FrontPanel
        /// Administration --> JobStatus--> Cancel
        /// </summary>
        /// <param name="device">printer</param>        
        /// <returns>True if Cancelled, else false.</returns>        
        public static bool PrintJobCancelFrontPanel(IDevice device)
        {
            TraceFactory.Logger.Info("Cancelling Print Job through Front Panel");

            //check if the device is Omni opus or not
            if (EwsWrapper.Instance().IsOmniOpus)
            {
                return FrontPanelCancelPrintJobOmniOpus(device.Address);
            }

            //Navigating the control back to home page if the control panel has error screen			
            JediWindjammerDevice jd = device as JediWindjammerDevice;
            JediWindjammerControlPanel cp = jd.ControlPanel;
            try
            {
                //validating whether the current page is in Home
                while (!cp.CurrentForm().Contains("Home"))
                {
                    //Pressing hide button to move the page to home
                    cp.Press("mButton1");
                }

                if (cp.GetControls().Contains("mStopButton"))
                {
                    cp.Press("mStopButton");
                    //Thread.Sleep(TimeSpan.FromSeconds(20));
                }

                cp.Press("m_CancelButton");     //Cancelling the job 
                Thread.Sleep(TimeSpan.FromSeconds(3));
                if (cp.GetControls().Contains("mYesButton"))
                {
                    TraceFactory.Logger.Info("mYes");
                    cp.Press("mYesButton");
                }
                if (cp.GetControls().Contains("mConfirmButton"))
                {
                    TraceFactory.Logger.Info("mConform");
                    cp.Press("mConfirmButton");
                }
                if (cp.GetControls().Contains("mOKButton"))
                {
                    TraceFactory.Logger.Info("mOK");
                    cp.Press("mOKButton");
                }
               // cp.PressToNavigate("m_OKButton", "HomeScreenForm", true);
                Thread.Sleep(TimeSpan.FromMinutes(1));
                TraceFactory.Logger.Info("Successfully Cancelled the Print Job through Front Panel");
                return true;
            }
           	catch(Exception ex)
            {
                //returning false if the error message is other than missing Cartridge errors                
				    TraceFactory.Logger.Info("Exception : {0}".FormatWith(ex.Message));
                return false;
            }
        }

        private static bool FrontPanelCancelPrintJobOmniOpus(string address)
        {

            using (JediOmniDevice _device = new JediOmniDevice(address))
            {
                try
                {
                    _device.PowerManagement.Wake();
                   //Added 30sec Timpespan wait in below function ...since it was ending sooner 
                    if (_device.ControlPanel.WaitForState(".hp-button-active-jobs", OmniElementState.Useable,TimeSpan.FromSeconds(30)))
                    {
                        // Press active print jobs button
                        _device.ControlPanel.PressWait(".hp-button-active-jobs", "#hpid-active-jobs-screen");
                        // Click Cancel print Job
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        _device.ControlPanel.WaitForState("#hpid-button-cancel-job", OmniElementState.Useable);
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        _device.ControlPanel.Press("#hpid-button-cancel-job");

                        // Click yes to confirm
                        Thread.Sleep(5000);
                        _device.ControlPanel.Press("#hpid-button-yes");

                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        // Exit to Home Screen
                        if (_device.ControlPanel.CheckState("#hpid-active-jobs-exit", OmniElementState.Useable))
                        {
                            _device.ControlPanel.PressWait("#hpid-active-jobs-exit", "#hpid-pause-resume-popup");
                        }
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        if (_device.ControlPanel.WaitForState("#hpid-button-resume", OmniElementState.Useable))
                        {
                            _device.ControlPanel.Press("#hpid-button-resume");
                        }
                        //_device.Dispose();
                        return true;
                    }

                    else
                    {
                        //_device.Dispose();
                        return false;
                    }
                }
                catch
                {
                    TraceFactory.Logger.Info("Exception occured, Unable to cancel job");
                    return false;

                }
            }
        }

        public static bool ValidatecanceljobOmniOpusFrontPanel(IDevice device, DateTime printstarttime)
        {
            string filename;
            string status = "";
            string time;
            int count = 0;
            using (JediOmniDevice _device = new JediOmniDevice(device.Address))
            {
                _device.PowerManagement.Wake();
                _device.ControlPanel.ScrollToItem("#hpid-jobLog-homescreen-button");
                _device.ControlPanel.CheckState("#hpid-jobLog-homescreen-button", OmniElementState.Useable);
                _device.ControlPanel.Press("#hpid-jobLog-homescreen-button");
                Thread.Sleep(5000);

                var data = _device.ControlPanel.GetValue("#hpid-active-jobs-setting-list", "innerText", OmniPropertyType.Property);
                string joblog = data;
                //TraceFactory.Logger.Info("JobLog {0}".FormatWith(joblog));
                TraceFactory.Logger.Info("Copied the JOB LOG Data Successfully, tracking the canceled job");
                if (joblog.Equals(null))
                {
                    TraceFactory.Logger.Info("Data from job Log is NULL");
                    return false;
                }
                //Reading the job log from front panel to find out the name and time of the canceled job for validation.
                using (StringReader read = new StringReader(joblog))
                {
                    while ((filename = read.ReadLine()) != null)
                    {
                        if (filename.Contains("Canceled"))
                        {
                            TraceFactory.Logger.Info("Filename : Cancel Found");
                            time = filename.Remove(0, 9);
                            status = status.Remove(0, 14);
                            DateTime canceltime = Convert.ToDateTime(time);
                            TraceFactory.Logger.Info("Cancel Time :{0}".FormatWith(canceltime));
                            TraceFactory.Logger.Info("Print start time: {0} ".FormatWith(printstarttime));
                            //if ((DateTime.Compare(canceltime, printstarttime) > 0))
                            //{
                              //  TraceFactory.Logger.Info("File {0} is Canceled at {1}".FormatWith(status, time));
                                return true;
                            //}
                        }
                        status = read.ReadLine();
                        if (status.Contains("Canceled"))
                        {
                            TraceFactory.Logger.Info("Status : Cancel Found");

                            time = status.Remove(0, 9);
                            filename = filename.Remove(0, 14);
                            DateTime canceltime = Convert.ToDateTime(time);
                            TraceFactory.Logger.Info("Cancel Time :{0}".FormatWith(canceltime));
                            TraceFactory.Logger.Info("Print start time: {0} ".FormatWith(printstarttime));

                          //  if ((DateTime.Compare(canceltime, printstarttime) > 0))
                            //{
                              //  TraceFactory.Logger.Info("File {0} is Canceled at {1}".FormatWith(status, time));
                                return true;
                            //}

                        }
                        if (count == 5)
                            return false;
                        count++;


                    }
                    return false;
                }

            }
        }

        /// <summary>
        ///Disable IPsec through FrontPanel
        ///To disturb a print job, navigating the Front Panel to disable ipsec
        /// Administration --> Network Settings--> Embedded Jet direct Menu--> Security---> Select IPSEC --->
        /// </summary>
        /// <param name="device">printer</param>
        /// <param name="state">true/false</param>
        /// <returns>True if disabled, else false.</returns>        
        public static bool FrontPanelNavigation(IDevice device, bool state)
        {
            //check if the device is Omni opus or not
            if (EwsWrapper.Instance().IsOmniOpus)
            {
                return NavigateFrontPanelOmniOpus(device.Address, state);
            }

            TraceFactory.Logger.Debug("Enabling IPSecurity through Front Panel");
            try
            {
                //Navigating the control back to home page if the control panel has error screen			
                JediWindjammerDevice jd = device as JediWindjammerDevice;
                JediWindjammerControlPanel cp = jd.ControlPanel;

                //validating whether the current page is in Home
                while (!cp.CurrentForm().Contains("Home"))
                {
                    //Pressing hide button to move the page to home
                    cp.Press("mButton1");
                }

                cp.Press("AdminApp");

                //scroll IncrementButton to get networking control
                while (!cp.GetControls().Contains("NetworkingAndIOMenu"))
                {
                    // Even after the increment button is pressed in the front panel exception occurred, so added try catch block
                    try
                    {
                        cp.Press("IncrementButton");
                    }
                    catch
                    {
                        // Do Nothing
                    }
                }

                //Enabling IPSec through Front Panel                
                cp.Press("NetworkingAndIOMenu");
                cp.Press("JetDirectMenu4");     //Embedded jet direct option
                cp.Press("0x9");                // Security option	
                cp.PressToNavigate("0x46_4", "IOMgrMenuDataSelection", true);
                if (state)
                {
                    cp.Press("KEEP");
                }
                else
                {
                    cp.Press("DISABLE");
                }
                cp.Press("m_OKButton");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                TraceFactory.Logger.Info("Successfully {0} IPSec through Front Panel".FormatWith(state ? "enabled" : "disabled"));
                return true;
            }
            catch
            {
                //returning false if the error message is other than missing Cartridge errors                
                return false;
            }
        }


        public static bool NavigateFrontPanelOmniOpus(string address, bool state)
        {
            using (JediOmniDevice _device = new JediOmniDevice(address))
            {

                TraceFactory.Logger.Debug("Enabling IPSecurity through Front Panel");

                try
                {
                    _device.PowerManagement.Wake();
                    _device.ControlPanel.ScrollToItem("#hpid-settings-homescreen-button");
                    _device.ControlPanel.PressWait("#hpid-settings-homescreen-button", "#hpid-keyboard", TimeSpan.FromSeconds(2));


                    //click networking
                    _device.ControlPanel.PressWait("#hpid-tree-node-listitem-networkingandiomenu", "#hpid-keyboard", TimeSpan.FromSeconds(2));

                    //Click on Ethernet
                    _device.ControlPanel.PressWait("#hpid-tree-node-listitem-jetdirectmenu4", "#hpid-keyboard", TimeSpan.FromSeconds(2));

                    //Click on Security
                    _device.ControlPanel.PressWait("#hpid-tree-node-listitem-0x9", "#hpid-keyboard", TimeSpan.FromSeconds(2));

                    //Click on IPSec
                    _device.ControlPanel.PressWait("#hpid-tree-node-listitem-0x46_4", "#hpid-keyboard", TimeSpan.FromSeconds(2));
                    //Enable IPSec

                    if (state)
                    {
                        _device.ControlPanel.Press("#0");
                    }
                    else
                    {
                        _device.ControlPanel.Press("#1");
                    }

                    //Click Done
                    _device.ControlPanel.PressWait("#hpid-ok-setting-button", "#hpid-keyboard", TimeSpan.FromSeconds(2));
                    EwsWrapper.Instance().Stop();
                    TraceFactory.Logger.Info("Successfully {0} IPSec through Front Panel".FormatWith(state ? "enabled" : "disabled"));
                    return true;
                }
                catch
                {
                    //returning false if the error message is other than missing Cartridge errors                
                    return false;
                }
            }
        }

        // Installing Printer using TopCat Tool for IPP Protocol
        public static string InstallPrinterUsingTopCat(ConnectivityPrintActivityData activityData, int portNo)
        {
            TraceFactory.Logger.Info("Installing Printer using TopCat tool");
            TopCatUIAutomation.Initialize();

            string controlpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "control.exe");
            Process.Start(controlpath, "/name Microsoft.DevicesAndPrinters");            

            AddPrinter addprint = new AddPrinter();
            addprint.CommandModuleFoToolBar.WaitForAvailable(20);
            addprint.AddaprinterAJOSButton.Click();

            ClickHyperlink choosePrint = new ClickHyperlink();
            choosePrint.ElementeClassicPane.IsAvailable();
            choosePrint.TheprinterthatIHyperlink.ClickWithMouse(MouseButton.Left, 30);

            SelectPrinter selectPrinter = new SelectPrinter();
            if (selectPrinter.NetworkprintertPane.IsAvailable())
            {
                selectPrinter.SelectasharedprRadioButton.Select(5);
            }
            if (selectPrinter.aAddingprintersEdit.IsEnabled())
            {                
                selectPrinter.aAddingprintersEdit.ClickWithMouse(MouseButton.Left, 30);
                               
                SendKeys.SendWait("h"); Thread.Sleep(TimeSpan.FromSeconds(1));
                string text = selectPrinter.aAddingprintersEdit.Text();
                if (text.Contains("hh"))
                {
                    SendKeys.SendWait("{BACKSPACE}");
                }
                Thread.Sleep(TimeSpan.FromSeconds(1)); SendKeys.SendWait("t"); Thread.Sleep(TimeSpan.FromSeconds(1));
                SendKeys.SendWait("t"); Thread.Sleep(TimeSpan.FromSeconds(1)); SendKeys.SendWait("p"); Thread.Sleep(TimeSpan.FromSeconds(1));
                SendKeys.SendWait(":"); Thread.Sleep(TimeSpan.FromSeconds(1));
                SendKeys.SendWait("//{0}:{1}".FormatWith(activityData.Ipv4Address, portNo));                
                selectPrinter.NextCCPushButtoDup0Button.Click();
            }

            InstallDriver driverInstall = new InstallDriver();
            driverInstall.AddPrinterWizarWindow.WaitForAvailable(50);
            if (driverInstall.AddPrinterWizarWindow.IsAvailable())
            {
                driverInstall.HaveDiskButton1Button.Click();
            }

            InstallFromDisk diskInstall = new InstallFromDisk();
            diskInstall.InstallFromDiskWindow.WaitForAvailable(30);
            if (diskInstall.CopymanufactureEdit.IsAvailable())
            {
                diskInstall.CopymanufactureEdit.ClickWithMouse(MouseButton.Left, 30);
                diskInstall.CopymanufactureEdit.Highlight(1);
                SendKeys.SendWait("{BACKSPACE}"); SendKeys.SendWait("{BACKSPACE}"); SendKeys.SendWait("{BACKSPACE}");                
                foreach (char driverChar in activityData.DriverPackagePath)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(600));
                    SendKeys.SendWait(driverChar.ToString());
                }
                diskInstall.OKButton1Dup0Button.Click();
            }

            SelectModel selectModel = new SelectModel();
            selectModel.SetDeviceInfo(activityData.DriverModel);
            selectModel.SelectedDeviceDataItem.PerformHumanAction(x => x.Select(10));
            selectModel.OKButton1I0XButton.Click();

            SuccessfulInstall success = new SuccessfulInstall();
            success.YouvesuccessfulPane.IsAvailable();
            try
            {
                success.NextCCPushButtoDup0Button.Click();
            }
            catch(Exception clickException)
            {
                TraceFactory.Logger.Info(clickException.Message);
            }

            WindowsSecurityToInstall securityPage = new WindowsSecurityToInstall();
            if (securityPage.WindowsSecurityWindow.IsAvailable(30))
            {                
                securityPage.InstallCCPushBuButton.Click(20);               
            }

            SuccessfulInstall successInstall = new SuccessfulInstall();
            if (successInstall.YouvesuccessfulPane.IsAvailable(40))
            {
                successInstall.NextCCPushButtoDup0Button.Click();
            }

            FinishInstall completeInstall = new FinishInstall();
            if (completeInstall.YouvesuccessfulPane.IsAvailable(40))
            {
                completeInstall.FinishCCPushButButton.Click();
            }
            
            Thread.Sleep(TimeSpan.FromSeconds(30));
            return ValidatePrinterInstalledUsingTopcat(activityData.DriverModel, portNo);
        }

        //Validation
        public static string ValidatePrinterInstalledUsingTopcat(string driverModel, int portNo)
        {
            TraceFactory.Logger.Info("Validating the installed Printer");

            string printerName = string.Empty;
            RegistryKey key = Registry.LocalMachine.OpenSubKey(PRINTERDRIVER_KEYPATH);
            foreach (string subKey in key.GetSubKeyNames())
            {
                RegistryKey folders = Registry.LocalMachine.OpenSubKey(@"{0}\{1}".FormatWith(PRINTERDRIVER_KEYPATH, subKey));
                foreach (string subFolder in folders.GetSubKeyNames())
                {
                    if (subFolder.Equals("DsSpooler"))
                    {
                        RegistryKey dsSpooler = Registry.LocalMachine.OpenSubKey(@"{0}\{1}\{2}".FormatWith(PRINTERDRIVER_KEYPATH, subKey, subFolder));
                        string driverName = dsSpooler.GetValue("driverName") == null ? string.Empty : dsSpooler.GetValue("driverName").ToString();
                        object portDetails = dsSpooler.GetValue("portName");
                        string[] portName = Array.ConvertAll((object[])portDetails, s => (string)s);
                        if (driverName.Equals(driverModel) && portName[0].Contains(portNo.ToString()))
                        {
                            TraceFactory.Logger.Info("Successfully installed the Printer");
                            return printerName = dsSpooler.GetValue("printerName").ToString();                            
                        }
                    }
                }
            }
            TraceFactory.Logger.Info("Failed to install the Printer");
            return printerName;
        }

        // Notify User to Add Web Service Printer
        /// </summary>
        private void printer_NotifyWSPAddition()
        {
            MessageBox.Show("Printer driver is installed.\nAdd WS printer and press OK to continue.", "Add WS Printer", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Get all files available in folder path
        /// </summary>
        /// <param name="folderPath">Path from where files need to be retrieved</param>
        /// <param name="folder"><see cref=" FolderType"/></param>
        /// <returns>Collection of files</returns>
        private static bool EditPrinterIPaddress(string oldIPAddress, string newIPAddress)
        {
            TraceFactory.Logger.Info("Modifying the IPaddress of the Printer: {0}".FormatWith(newIPAddress));

            // Editing the printer ip address				
            System.Management.ConnectionOptions options = new System.Management.ConnectionOptions();
            ManagementScope mscope = new ManagementScope(@"\root\CIMV2", options);
            mscope.Connect();
            System.Management.ObjectQuery oQuery = new ObjectQuery("Select * from Win32_TCPIPPrinterPort");
            System.Management.ManagementObjectSearcher searcher = new ManagementObjectSearcher(mscope, oQuery);
            ManagementObjectCollection moCollection = searcher.Get();

            foreach (ManagementObject mo in moCollection)
            {
                string name = mo["Name"].ToString();

                if (name.Contains(oldIPAddress) || name.Contains(newIPAddress))
                {
                    System.Threading.Thread.Sleep(10000);
                    mo["HostAddress"] = newIPAddress;
                    mo.Put();
                    TraceFactory.Logger.Info("Edited the Printer Port to new IP address " + newIPAddress);
                }
            }

            Thread.Sleep(TimeSpan.FromMinutes(1));
            return true;
        }

        private static void printingStatus(Printer.Printer printer)
        {
            while (printer.PrinterStatus != PrinterStatus.Printing)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }
        }

        #endregion
    }
}
