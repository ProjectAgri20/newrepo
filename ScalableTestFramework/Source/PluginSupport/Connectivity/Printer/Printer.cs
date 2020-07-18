using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Printing;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.PluginSupport.Connectivity.Discovery;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.Print;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.Print.Monitor;
using HP.ScalableTest.Utility;
using Microsoft.Win32;

// removed references to HP.ScalableTest.Network.Snmp and Lextm.SharpSnmpLib:
// Don Anderson, 4/17/2015

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    /// <summary>
    /// Event Handler to notify Web Service Printer Addition
    /// </summary>
    public delegate void NotifyWSPrinterEventHandler();

    /// <summary>
    /// Structure to maintain LPR Print Job details
    /// </summary>
    public struct LprPrintDetail
    {
        /// <summary>
        /// Gets or sets Job Id
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// Gets or sets Job Status
        /// </summary>
        public bool JobStatus { get; set; }

        /// <summary>
        /// Gets or sets Job Thread
        /// </summary>
        public Thread JobThread { get; set; }
    }

    /// <summary>
    /// This is an abstract class provides functionalities which are common to all the 
    /// VEP, TPS, Inkjet and LFP printers. Derived classes should override if any functionality is different    
    /// </summary>
    public abstract class Printer
    {
        #region Public Events

        /// <summary>
        /// Event handler for PrintJobSpooling
        /// </summary>
        public event EventHandler<PrintJobDataEventArgs> PrintJobSpooling;

        /// <summary>
        /// Event handler for PrintJobPrinting
        /// </summary>
        public event EventHandler<PrintJobDataEventArgs> PrintJobPrinting;

        /// <summary>
        /// Event handler for PrintJobFinished
        /// </summary>
        public event EventHandler<PrintJobDataEventArgs> PrintJobFinished;

        /// <summary>
        /// Event Handler for FTPJobEvent
        /// </summary>
        public event EventHandler<FtpPrintingEventArgs> FTPJobPrinting;

        /// <summary>
        /// Event Handler for Notification for Web Service Printer Addition
        /// </summary>
        public event NotifyWSPrinterEventHandler NotifyWSPrinter;

        /// <summary>
        /// Event Handler for PrintQueue Error
        /// </summary>
        public event EventHandler<PrintJobDataEventArgs> PrintQueueError;

        #endregion

        #region Enum

        /// <summary>
        /// Print Protocol enumerator
        /// </summary>
        public enum PrintProtocol
        {
            RAW,
            IPP,
            IPPS,
            LPD,
            WSP
        }

        #region enum

        /// <summary>
        /// Source Type
        /// </summary>
        public enum BacaBodSourceType
        {
            /// <summary>
            /// The IP Address source type to discover the printer.
            /// </summary>
            IPAddress = 0,
            /// <summary>
            /// The Host Name source type to discover the printer.
            /// </summary>
            HostName = 1,
            /// <summary>
            /// The MacAddress source type to discover the printer.
            /// </summary>
            MacAddress = 2
        }

        /// <summary>
        /// BacaBod Discovery Type
        /// </summary>
        public enum BacaBodDiscoveryType
        {
            /// <summary>
            /// The SLP Discovery Type/
            /// </summary>
            SLP = 1,
            /// <summary>
            /// The WSDV6 Discovery Type.
            /// </summary>
            WSDV6 = 2,
            /// <summary>
            /// The WSDV4 Discovery Type.
            /// </summary>
            WSDV4 = 4,
            /// <summary>
            /// All the Discovery Types.
            /// </summary>
            All = 7,
        }

        /// <summary>
        /// Packet cast option
        /// </summary>
        public enum CastType
        {
            /// <summary>
            /// Multicast option
            /// </summary>
            Multicast = 1,

            /// <summary>
            /// Unicast option
            /// </summary>
            Unicast = 2
        }

        #endregion

        #endregion Enum

        #region Constants

        /// <summary>
        /// Constant for Power Cycle
        /// </summary>
        protected const int POWER_CYCLE = 4;

        /// <summary>
        /// Constant for Resetting NVRAM
        /// </summary>
        protected const int RESET_NVRAM = 5;

        /// <summary>
        /// Constant for TCPReset
        /// </summary>
        protected const int TCP_RESET = 1;

        /// <summary>
        /// Constant for Cold Reset
        /// </summary>
        protected const int COLD_RESET = 6;

        /// <summary>
        /// Telnet Port Number
        /// </summary>
        protected const int TELENET_PORTNO = 23;

        /// <summary>
        /// Port 9100 
        /// </summary>
        protected const int P9100_PORTNO = 9100;

        /// <summary>
        /// Line Printer Daemon Port Number
        /// </summary>
        protected const int LPD_PORTNO = 515;

        /// <summary>
        /// Internet Printing Protocol Port Numbers
        /// </summary>
        protected readonly int[] IPP_PORTNO = { 80, 631 };

        /// <summary>
        /// Secure Internet Printing Protocol Port Number
        /// </summary>
        protected const int IPPS_PORTNO = 443;

        /// <summary>
        /// Timeout for pinging printer
        /// </summary>
        protected const int PING_TIMEOUT = 10;

        /// <summary>
        /// File Transfer Protocol prefix URL
        /// </summary>
        protected const string FTP_URL_PREFIX = "ftp://";

        /// <summary>
        /// Printer Driver Registry Path
        /// </summary>
        protected const string PRINTERDRIVER_KEYPATH = @"SYSTEM\CurrentControlSet\Control\Print\Printers";

        /// <summary>
        /// Time format for log display
        /// </summary>
        protected const string TIME_FORMAT = "hh\\:mm\\:ss";

        /// <summary>
        /// Supported file types for print
        /// </summary>
        protected readonly string[] SUPPORTED_FILES = { ".txt", ".rtf", ".png", ".gif", ".jpg", ".jpeg", ".doc", ".docx", ".ppt", ".pptx", ".tif", ".tiff", ".bmp", ".pdf", ".html" };

        /// <summary>
        /// Supported file types for FTP print
        /// </summary>
        protected readonly string[] FTP_SUPPORTED_FILES = { ".pcl", ".ps", ".txt", ".gl2", ".hpgl2", ".pdf", ".jpg", ".jpeg", ".tif", ".tiff" };

        /// <summary>
        /// Files to send to PrintQueue
        /// </summary>
        protected const int MAX_FILES_TO_PRINT = 5;

        /// <summary>
        /// SNMP OID for getting Firmware Version
        /// </summary>
        protected const string FIRMWARE_OID = "1.3.6.1.4.1.11.2.3.9.4.2.1.1.3.6.0";

        /// <summary>
        /// SNMP OID for general reset
        /// </summary>
        protected const string GENERAL_RESET_OID = "1.3.6.1.2.1.43.5.1.1.3.1";

        /// <summary>
        /// SNMP OID for getting device status
        /// </summary>
        protected const string DEVICE_STATUS_OID = "1.3.6.1.2.1.25.3.2.1.5.1";

        /// <summary>
        /// Gets the current status of a printer device.  Possible values are defined in <see cref="PrinterStatus"/>.
        /// </summary>
        protected const string PRINTER_STATUS_OID = "1.3.6.1.2.1.25.3.5.1.1.1";

        #endregion

        #region Local Variables

        /// <summary>
        /// Contains Wired IPv4 address
        /// </summary>
        protected IPAddress _wiredIPv4Address;

        /// <summary>
        /// Contains Wired IPv6 Address
        /// </summary>
        protected IPAddress _wiredIPv6Address;

        /// <summary>
        /// Contains Wireless IPv4 Address
        /// </summary>
        protected IPAddress _wirelessIPv4Address;

        /// <summary>
        /// Contains Wireless IPv6 Address
        /// </summary>
        protected IPAddress _wirelessIPv6Address;

        /// <summary>
        /// Contains Host Name
        /// </summary>
        protected string _hostName;

        /// <summary>
        /// Contains MAC Address
        /// </summary>
        protected string _macAddress;

        /// <summary>
        /// Contains FW Build Number
        /// </summary>
        protected string _fwBuildNumber;

        /// <summary>
        /// Contains Page Count
        /// </summary>
        protected int _pageCount;

        /// <summary>
        /// Device Status
        /// </summary>
        protected DeviceStatus _deviceStatus;

        /// <summary>
        /// Time required to come up the printer after power cycle
        /// </summary>
        protected int _timeRequiredForPowerCycle;

        /// <summary>
        /// Time required to come up the printer after cold reset
        /// </summary>
        protected int _timeRequiredForColdReset;

        /// <summary>
        /// Time required to come up the printer after NVRAM Reset
        /// </summary>
        protected int _timeRequiredForNVRAMReset;

        /// <summary>
        /// Time required to come up the printer after TCP Reset
        /// </summary>
        protected int _timeRequiredForTCPReset;

        /// <summary>
        /// PrintQueue 
        /// </summary>
        protected PrintQueue _printQueue;

        /// <summary>
        /// Print Job Monitor
        /// </summary>
        private PrintJobMonitor _printJobMonitor;

        /// <summary>
        /// PrintJobData collection
        /// </summary>
        private Dictionary<long, PrintJobData> _printJobData;

        /// <summary>
        /// Link local address
        /// </summary>
        private IPAddress _linkLocalAddress;

        /// <summary>
        /// Stateful address
        /// </summary>
        private IPAddress _statefullAddress;

        /// <summary>
        /// Stateless address
        /// </summary>
        private Collection<IPAddress> _statelessAddress;

        /// <summary>
        /// Printer Discovery
        /// </summary>
        private bool _isPrinterDiscovered = false;

        /// <summary>
        /// Printer IP configuration method
        /// </summary>
        protected IPConfigMethod _defaultIPConfigMethod;

        /// <summary>
        /// Contains LPR print details like Job ID, Status and Thread
        /// </summary>
        protected Collection<LprPrintDetail> _lprPrintDetail;

        #endregion

        #region Properties

        /// <summary>
        /// Gets Wired IPv4 address
        /// </summary>
        public IPAddress WiredIPv4Address
        {
            get { return _wiredIPv4Address; }
        }

        /// <summary>
        /// Gets Wireless IPv4 Address
        /// </summary>
        public IPAddress WirelessIPv4Address
        {
            get { return _wirelessIPv4Address; }
        }

        /// <summary>
        /// Gets Wireless IPv6 Address
        /// </summary>
        public IPAddress WirelessIPv6Address
        {
            get { return _wirelessIPv6Address; }
        }

        /// <summary>
        /// Gets MAC Address
        /// </summary>
        public string MacAddress
        {
            get
            {
                PopulatePrinterProperties();
                return _macAddress;
            }
        }

        /// <summary>
        /// Gets Host Name
        /// </summary>
        public string HostName
        {
            get
            {
                PopulatePrinterProperties();
                return _hostName;
            }
        }

        /// <summary>
        /// Gets Firmware Build Number
        /// </summary>
        public string FWBuildNumber
        {
            get { return _fwBuildNumber; }
        }

        /// <summary>
        /// Gets Page Count
        /// </summary>
        public int PageCount
        {
            get { return _pageCount; }
        }

        /// <summary>
        /// Gets Device Status
        /// </summary>
        public DeviceStatus DeviceStatus
        {
            get { return GetDeviceStatus(_wiredIPv4Address); }
        }

        /// <summary>
        /// Gets Printer Status
        /// </summary>
        public PrinterStatus PrinterStatus
        {
            get { return GetPrinterStatus(_wiredIPv4Address); }
        }

        /// <summary>
        /// PrintQueue object for printer installed
        /// </summary>
        public PrintQueue PrintQueue
        {
            get { return _printQueue; }
        }

        /// <summary>
        /// <see cref="PrintQueueStatus"/>
        /// </summary>
        public PrintQueueStatus PrintQueueStatus
        {
            get { return GetPrintQueueStatus(); }
        }

        /// <summary>
        /// Gets Print queue error is in error state or not
        /// </summary>        
        public bool IsPrintQueueInError
        {
            get
            {
                if (null == _printQueue)
                {
                    Framework.Logger.LogInfo("Print Queue is not initialized.");
                    return false;
                }

                return IsQueueInError();
            }
        }

        /// <summary>
        /// Gets number of jobs to be printed
        /// This will not include the jobs which are added to the queue externally.
        /// </summary>        
        public int NumberOfJobsToPrint
        {
            get
            {
                if (null != _printJobData)
                {
                    return _printJobData.Count;
                }

                return -1;
            }
        }

        /// <summary>
        /// Gets number of jobs in the current print queue.
        /// </summary>
        public int NumberOfJobsInPrintQueue
        {
            get
            {
                // Note: Create new print queue object based on the current print queue name.
                // We are doing this because Microsoft throws an exception. 
                // If this method is called in a different thread in case of Asynchronous calls
                PrintQueue printQueue = PrintQueueController.GetPrintQueue(_printQueue.Name);
                return printQueue.NumberOfJobs;
            }
        }

        /// <summary>
        /// IPv6 Link Local Address
        /// </summary>
        public IPAddress IPv6LinkLocalAddress
        {
            get
            {
                PopulatePrinterProperties();
                return _linkLocalAddress;
            }
        }

        /// <summary>
        /// IPv6 Stateful Address
        /// </summary>
        public IPAddress IPv6StateFullAddress
        {
            get
            {
                PopulatePrinterProperties();
                return _statefullAddress;
            }
        }

        /// <summary>
        /// IPv6 Stateless Address
        /// </summary>
        public Collection<IPAddress> IPv6StatelessAddresses
        {
            get
            {
                PopulatePrinterProperties();
                return _statelessAddress;
            }
        }

        /// <summary>
        /// Firmware Version of the Printer
        /// Note: This works for Jedi, Phoenix and Sirius family
        /// </summary>
        public virtual string FirmwareVersion
        {
            get
            {
                IDevice device = DeviceFactory.Create(_wiredIPv4Address);
                return device.GetDeviceInfo().FirmwareRevision;
            }
        }

        public IPConfigMethod DefaultIPConfigMethod
        {
            get
            {
                return _defaultIPConfigMethod;
            }
        }

        /// <summary>
        /// Gets the Legacy IP Address of the printer
        /// </summary>
        public static IPAddress LegacyIPAddress
        {
            get { return IPAddress.Parse("192.0.0.192"); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Power cycles the printer asynchronously without checking the printer status.
        /// <remarks>The method starts power cycling the printer.
        /// It is the users responsibility to ensure that the printer is come back to ready state after the power cycle</remarks>
        /// </summary>
        public virtual void PowerCycleAsync()
        {
            Framework.Logger.LogInfo("Power Cycle started.");

            // create SNMP object and set the printer reset to power cycle
            Snmp snmp = new Snmp(_wiredIPv4Address);
            snmp.Set(GENERAL_RESET_OID, POWER_CYCLE);
        }

        /// <summary>
        /// Power Cycles the printer with Wired IPv4 address.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>        
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public virtual bool PowerCycle()
        {
            Framework.Logger.LogInfo("Power Cycle started.");

            // create SNMP object and set the printer reset to power cycle
            Snmp snmp = new Snmp(_wiredIPv4Address);
            snmp.Set(GENERAL_RESET_OID, POWER_CYCLE);

            // once the printer started doing power cycle wait for the specified time
            // and check the printer ready state.
            Thread.Sleep(TimeSpan.FromMinutes(_timeRequiredForPowerCycle));
            if (IsPrinterReady(_wiredIPv4Address, _timeRequiredForPowerCycle))
            {
                Framework.Logger.LogInfo("Printer has come to ready state after Power Cycle");
                return true;
            }
            else
            {
                Framework.Logger.LogInfo("Printer failed to acquire ready state after Power Cycle");
                return false;
            }
        }

        /// <summary>
        /// Power Cycles the printer with Wired IPv4 address.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>        
        /// <param name="time">wait time after the power cycle operation</param>
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public virtual bool PowerCycle(int time)
        {
            _timeRequiredForPowerCycle = time;

            Framework.Logger.LogInfo("Power Cycle started.");

            // create SNMP object and set the printer reset to power cycle
            Snmp snmp = new Snmp(_wiredIPv4Address);
            snmp.Set(GENERAL_RESET_OID, POWER_CYCLE);

            // once the printer started doing power cycle wait for the specified time
            // and check the printer ready state.
            Thread.Sleep(TimeSpan.FromMinutes(_timeRequiredForPowerCycle));
            if (IsPrinterReady(_wiredIPv4Address, _timeRequiredForPowerCycle))
            {
                Framework.Logger.LogInfo("Printer has come to ready state after Power Cycle");
                return true;
            }
            else
            {
                Framework.Logger.LogInfo("Printer failed to acquire ready state after Power Cycle");
                return false;
            }
        }

        /// <summary>
        /// Cold Resets the printer asynchronously without checking the printer status.
        /// <remarks>The method starts power cycling the printer.
        /// It is the users responsibility to ensure that the printer is come back to ready state after the cold reset</remarks>
        /// </summary>
        public virtual void ColdResetAsync()
        {
            Framework.Logger.LogInfo("Cold reset started.");

            // create SNMP object
            Snmp snmp = new Snmp(_wiredIPv4Address);
            snmp.Set(GENERAL_RESET_OID, COLD_RESET);
        }

        /// <summary>
        /// Cold Resets the printer with Wired IPv4 address.
        /// Returns true if the printer is in ready state after the Cold reset, else returns false.
        /// </summary>        
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public virtual bool ColdReset()
        {
            Framework.Logger.LogInfo("Cold reset started.");

            // create SNMP object
            Snmp snmp = new Snmp(_wiredIPv4Address);
            snmp.Set(GENERAL_RESET_OID, COLD_RESET);

            // wait for the specified time and check the printer status.
            Thread.Sleep(TimeSpan.FromMinutes(_timeRequiredForColdReset));
            if (IsPrinterReady(_wiredIPv4Address, _timeRequiredForColdReset))
            {
                Framework.Logger.LogInfo("Printer has acquired IP after Cold reset");
               // return true;
            }
            else
            {
                Framework.Logger.LogInfo("Printer failed to acquire IP and is not pinging after ColdReset");
               // return true; //changed it to true, as executionwas getting ended for this test case if printer is not ready
            }
            return true;
        }

        /// <summary>
        /// Cold Resets the printer with Wired IPv4 address.
        /// Returns true if the printer is in ready state after the Cold reset, else returns false.
        /// </summary> 
        /// <param name="time">wait time after the cold reset operation</param>
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public virtual bool ColdReset(int time)
        {
            Framework.Logger.LogInfo("Cold reset started.");

            _timeRequiredForColdReset = time;

            // create SNMP object
            Snmp snmp = new Snmp(_wiredIPv4Address);
            snmp.Set(GENERAL_RESET_OID, COLD_RESET);

            // wait for the specified time and check the printer status.
            Thread.Sleep(TimeSpan.FromMinutes(_timeRequiredForColdReset));
            if (IsPrinterReady(_wiredIPv4Address, _timeRequiredForColdReset))
            {
                Framework.Logger.LogInfo("Printer has acquired IP after Cold reset ");
                //return true;
            }
            else
            {
                Framework.Logger.LogInfo("Printer failed to acquire IP and is not pinging after ColdReset");
               // return false;
            }
            return true;
        }

        /// <summary>
        /// Resets the NVRAM of the printer
        /// </summary>        
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public virtual bool ResetNVRAM()
        {
            Framework.Logger.LogInfo("Resetting NVRAM started.");

            // create SNMP object
            Snmp snmp = new Snmp(_wiredIPv4Address);
            snmp.Set(GENERAL_RESET_OID, RESET_NVRAM);

            // wait for the specified time and check the printer status.
            Thread.Sleep(TimeSpan.FromMinutes(_timeRequiredForNVRAMReset));
            return IsPrinterReady(_wiredIPv4Address, _timeRequiredForNVRAMReset);
        }

        /// <summary>
        /// TCPReset
        /// </summary>        
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public virtual void TCPReset()
        {
            Framework.Logger.LogInfo("TCP Reset started.");
            string tcpReset = "1.3.6.1.4.1.11.2.4.3.5.20.0";

            // create SNMP object
            Snmp snmp = new Snmp(_wiredIPv4Address);
            snmp.Set(tcpReset, TCP_RESET);


            // wait for the specified time and check the printer status.            
            Thread.Sleep(TimeSpan.FromMinutes(_timeRequiredForTCPReset));
            Framework.Logger.LogInfo("TCP Reset: Success");
        }

        /// <summary>
        /// Install Printer driver and Add printer
        /// </summary>
        /// <param name="ipAddress">IP Address of the Printer</param>
        /// <param name="protocol"><see cref="PrintProtocol"/></param>
        /// <param name="driverPath">Path to the Printer driver</param>
        /// <param name="driverModel">Printer driver model</param>
        /// <param name="portNo">Port no</param>
        /// <returns>True if installed and added successfully, false otherwise</returns>
        public bool Install(IPAddress ipAddress, PrintProtocol protocol, string driverPath, string driverModel, int portNo = -1, string printerHostName = null, bool isPingRequired = true)
        {
            if (isPingRequired)
            {
                if (!PingUntilTimeout(ipAddress, TimeSpan.FromSeconds(PING_TIMEOUT)))
                {
                    Framework.Logger.LogInfo("Printer installation failed, Printer IP: {0} is not pinging.".FormatWith(ipAddress));
                    return false;
                }
            }

            if (string.IsNullOrEmpty(driverPath))
            {
                Framework.Logger.LogInfo("Printer installation failed, Driver Path can not be empty.");
                return false;
            }

            if (string.IsNullOrEmpty(driverModel))
            {
                Framework.Logger.LogInfo("Printer installation failed, Driver Model can not be empty.");
                return false;
            }

            switch (protocol)
            {
                case PrintProtocol.RAW:
                    if (-1 == portNo)
                    {
                        portNo = P9100_PORTNO;
                    }
                    break;
                case PrintProtocol.IPP:
                    if (!Array.Exists(IPP_PORTNO, item => item == portNo))
                    {
                        Framework.Logger.LogInfo("Printer installation failed, specified port: {0} for IPP protocol is not valid.".FormatWith(portNo));
                        return false;
                    }
                    break;
                case PrintProtocol.IPPS:
                    if (-1 == portNo)
                    {
                        portNo = IPPS_PORTNO;
                    }
                    break;
                case PrintProtocol.LPD:
                    if (-1 == portNo)
                    {
                        portNo = LPD_PORTNO;
                        break;
                    }
                    else if (LPD_PORTNO != portNo)
                    {
                        Framework.Logger.LogInfo("Printer installation failed, specified port: {0} for LPD protocol is not valid.".FormatWith(portNo));
                        return false;
                    }
                    break;
                case PrintProtocol.WSP:
                    break;
            }

            _printQueue = null;
            Framework.Logger.LogInfo("Installing Printer with {0} protocol with {1} port number.".FormatWith(protocol, portNo));
            return Install(ipAddress.ToString(), protocol, driverPath, driverModel, portNo, printerHostName);
        }

        /// <summary>
        /// Print file for specified time duration        
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="duration">Time duration</param>
        /// <returns>True if print job is successful, false otherwise</returns>
        public bool Print(string filePath, TimeSpan duration)
        {
            if (!File.Exists(filePath))
            {
                Framework.Logger.LogInfo("Printing failed, specified file : {0} doesn't exist.".FormatWith(filePath));
                return false;
            }

            if (null == _printQueue)
            {
                Framework.Logger.LogInfo("Printing failed, make sure Printer is installed successfully.");
                return false;
            }

            return Print(filePath, duration, true);
        }

        /// <summary>
        /// Print file for specified minutes
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="duration">Time duration in minutes</param>
        /// <returns>True if print job is successful, false otherwise</returns>        
        public bool Print(string filePath, int duration = -1)
        {
            TimeSpan timeDuration = TimeSpan.Zero;
            if (-1 != duration)
            {
                timeDuration = new TimeSpan(0, duration, 0);
            }

            return Print(filePath, timeDuration);
        }

        /// <summary>
        /// Print collection of files for specified time duration
        /// Note: All supported jobs in collection will be printed at least once even if time duration is less
        /// </summary>
        /// <param name="files">Collection of files</param>
        /// <param name="duration">Time duration</param>
        /// <returns>True if all print jobs are successful, false otherwise</returns>
        public bool Print(string[] files, TimeSpan duration, string printerName = null)
        {
            if (printerName != null)
            {
                _printQueue = PrintQueueController.GetPrintQueue(printerName);
            }

            if (null == _printQueue)
            {
                Framework.Logger.LogInfo("Printing failed, make sure Printer is installed successfully.");
                return false;
            }

            string[] availableFiles, missingFiles;
            int fileAvailable, fileMissing;
            availableFiles = missingFiles = new string[files.Length];
            fileAvailable = fileMissing = 0;

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    availableFiles[fileAvailable] = file;
                    fileAvailable++;
                }
                else
                {
                    missingFiles[fileMissing] = file;
                    fileMissing++;
                }
            }

            if (0 == fileAvailable)
            {
                Framework.Logger.LogInfo("Printing failed, no files available for Print.");
                return false;
            }

            if (0 != fileMissing)
            {
                Framework.Logger.LogInfo("Following files are not available : {0}.".FormatWith(string.Join(", ", missingFiles)));
            }

            return Print(availableFiles, duration, true);
        }

        /// <summary>
        /// Print collection of files for specified minutes
        /// Note: All supported jobs in collection will be printed at least once even if time duration is less
        /// </summary>
        /// <param name="files">Collection of files</param>
        /// <param name="duration">Time duration in minutes</param>
        /// <returns>True if all print jobs are successful, false otherwise</returns>
        public bool Print(string[] files, int duration = -1, string printerName = null)
        {
            TimeSpan timeDuration = TimeSpan.Zero;

            if (-1 != duration)
            {
                timeDuration = new TimeSpan(0, duration, 0);
            }

            return Print(files, timeDuration, printerName);
        }

        /// <summary>
        /// Print file using FTP protocol
        /// Note: Print job will be printed at least once even if time duration is less
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="filePath">File path</param>
        /// <param name="duration">Duration for printing</param>
        /// <returns>true if successful, false otherwise</returns>
        public bool PrintWithFtp(IPAddress ipAddress, string userName, string password, string filePath, TimeSpan duration, bool isPassiveMode = false)
        {
            if (!PingUntilTimeout(ipAddress, TimeSpan.FromSeconds(PING_TIMEOUT)))
            {
                Framework.Logger.LogInfo("Printing with FTP failed, Printer IP: {0} is not pingable.".FormatWith(ipAddress));
                return false;
            }

            if (!File.Exists(filePath))
            {
                Framework.Logger.LogInfo("Printing with FTP failed, specified file : {0} doesn't exist.".FormatWith(filePath));
                return false;
            }

            bool result = true;

            if (TimeSpan.Zero != duration)
            {
                Framework.Logger.LogInfo("Printing for duration of {0}.".FormatWith(duration.ToString(TIME_FORMAT, CultureInfo.CurrentCulture)));
            }

            DateTime startTime = DateTime.Now;

            do
            {
                result &= PrintFtp(ipAddress.ToString(), userName, password, filePath, isPassiveMode);
            } while (result && (DateTime.Now.Subtract(startTime).TotalSeconds <= duration.TotalSeconds));

            return result;
        }

        /// <summary>
        /// Print file using FTP protocol
        /// Note: Print job will be printed at least once even if time duration is less
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="filePath">File path</param>
        /// <param name="duration">Duration in minutes</param>
        /// <returns>true if successful, false otherwise</returns>
        public bool PrintWithFtp(IPAddress ipAddress, string userName, string password, string filePath, int duration = -1, bool isPassiveMode = false)
        {
            TimeSpan timeDuration = TimeSpan.Zero;
            if (-1 != duration)
            {
                timeDuration = new TimeSpan(0, duration, 0);
            }

            return PrintWithFtp(ipAddress, userName, password, filePath, timeDuration, isPassiveMode);
        }

        /// <summary>
        /// Print list of files using FTP protocol
        /// Note: All supported jobs in collection will be printed at least once even if time duration is less
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="files">List of files</param>
        /// <param name="duration">Duration for printing</param>        
        /// <returns>true if successful, false otherwise</returns>
        public bool PrintWithFtp(IPAddress ipAddress, string userName, string password, string[] files, TimeSpan duration, bool isPassiveMode = false)
        {
            if (!PingUntilTimeout(ipAddress, TimeSpan.FromSeconds(10)))
            {
                Framework.Logger.LogInfo("Printing with FTP failed, Printer IP: {0} is not pingable.".FormatWith(ipAddress));
                return false;
            }

            if (null == files || 0 == files.Length)
            {
                Framework.Logger.LogInfo("No files available for print.");
                return false;
            }

            string[] availableFiles, missingFiles;
            int fileAvailable, fileMissing;
            availableFiles = missingFiles = new string[files.Length];
            fileAvailable = fileMissing = 0;

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    availableFiles[fileAvailable] = file;
                    fileAvailable++;
                }
                else
                {
                    missingFiles[fileMissing] = file;
                    fileMissing++;
                }
            }

            if (0 == fileAvailable)
            {
                Framework.Logger.LogInfo("Printing failed, no files available for Print.");
                return false;
            }

            if (0 != fileMissing)
            {
                Framework.Logger.LogInfo("Following files are not available : {0}.".FormatWith(string.Join(", ", missingFiles)));
            }

            return PrintFtp(ipAddress.ToString(), userName, password, availableFiles, duration, isPassiveMode);

        }

        /// <summary>
        /// Print list of files using FTP protocol
        /// Note: All supported jobs in collection will be printed at least once even if time duration is less
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="files">List of files</param>
        /// <param name="duration">Duration in minutes</param>        
        /// <returns>true if successful, false otherwise</returns>
        public bool PrintWithFtp(IPAddress ipAddress, string userName, string password, string[] files, int duration = -1, bool isPassiveMode = false)
        {
            TimeSpan timeDuration = TimeSpan.Zero;
            if (-1 != duration)
            {
                timeDuration = new TimeSpan(0, duration, 0);
            }

            return PrintWithFtp(ipAddress, userName, password, files, timeDuration, isPassiveMode);
        }

        /// <summary>
        /// Print file asynchronously
        /// </summary>
        /// <param name="filePath">File Path</param>
        public void PrintAsynchronously(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Framework.Logger.LogInfo("Printing failed, specified file : {0} doesn't exist.".FormatWith(filePath));
                return;
            }

            if (null == _printQueue)
            {
                Framework.Logger.LogInfo("Printing failed, make sure Printer is installed successfully.");
                return;
            }

            PrintAsync(filePath);
        }

        /// <summary>
        /// Print list of files asynchronously
        /// </summary>
        /// <param name="files"></param>
        public void PrintAsynchronously(string[] files)
        {
            if (null == _printQueue)
            {
                Framework.Logger.LogInfo("Printing failed, make sure Printer is installed successfully.");
                return;
            }

            if (null == files || 0 == files.Length)
            {
                Framework.Logger.LogInfo("No files available for print.");
                return;
            }

            string[] availableFiles, missingFiles;
            int fileAvailable, fileMissing;
            availableFiles = missingFiles = new string[files.Length];
            fileAvailable = fileMissing = 0;

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    availableFiles[fileAvailable] = file;
                    fileAvailable++;
                }
                else
                {
                    missingFiles[fileMissing] = file;
                    fileMissing++;
                }
            }

            if (0 == fileAvailable)
            {
                Framework.Logger.LogInfo("Printing failed, no files available for Print.");
                return;
            }

            if (0 != fileMissing)
            {
                Framework.Logger.LogInfo("Following files are not available : {0}.".FormatWith(string.Join(", ", missingFiles)));
            }

            PrintAsync(availableFiles);
        }

        /// <summary>
        /// Print file using FTP protocol Asynchronously
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="filePath">File path</param>        
        public void PrintWithFtpAsynchronously(IPAddress ipAddress, string userName, string password, string filePath)
        {
            if (!PingUntilTimeout(ipAddress, TimeSpan.FromSeconds(PING_TIMEOUT)))
            {
                Framework.Logger.LogInfo("Printing with FTP failed, Printer IP: {0} is not accessible.".FormatWith(ipAddress));
                return;
            }

            if (!File.Exists(filePath))
            {
                Framework.Logger.LogInfo("Printing with FTP failed, specified file : {0} doesn't exist.".FormatWith(filePath));
                return;
            }

            PrintFtpAsync(ipAddress.ToString(), userName, password, filePath);
        }

        /// <summary>
        /// Pause printing job
        /// </summary>
        /// <param name="jobId">Print job Id</param>
        /// <returns>true if paused successfully, false otherwise</returns>
        public bool PausePrintJob(int jobId)
        {
            if (null == _printQueue)
            {
                Framework.Logger.LogInfo("Print job pause failed, Print Queue is not accessible.");
                return false;
            }

            if (jobId <= 0)
            {
                Framework.Logger.LogInfo("Print job pause failed, Job Id can not be less than 0.");
                return false;
            }

            // Query to get print job properties
            string searchQuery = "SELECT * FROM Win32_PrintJob";
            ManagementObjectSearcher searchPrintJobs = new ManagementObjectSearcher(searchQuery);

            try
            {
                ManagementObjectCollection printJobCollection = searchPrintJobs.Get();

                foreach (ManagementObject printJob in printJobCollection)
                {
                    // jobName will be of format: [Printer_Name], [Job_ID]
                    string jobName = printJob.Properties["Name"].Value.ToString();
                    char[] splitCharacter = { ',' };
                    string printerName = jobName.Split(splitCharacter)[0];
                    int printJobId = Convert.ToInt32(jobName.Split(splitCharacter)[1], CultureInfo.CurrentCulture);

                    if (String.Compare(printerName, _printQueue.Name, true, CultureInfo.CurrentCulture) == 0)
                    {
                        if (printJobId == jobId)
                        {
                            printJob.InvokeMethod("Pause", null);
                            Framework.Logger.LogDebug("Print job is paused successfully.");
                            return true;
                        }
                    }
                }

            }
            catch (Exception defaultException)
            {
                Framework.Logger.LogInfo("Print job pause failed, Exception : {0}.".FormatWith(defaultException.JoinAllErrorMessages()));
            }
            finally
            {
                searchPrintJobs.Dispose();
            }

            return false;
        }

        /// <summary>
        /// Resume paused print job
        /// </summary>
        /// <param name="jobId">Print job Id</param>
        /// <returns>true if print job resumed successfully, false otherwise</returns>
        public bool ResumeJob(int jobId)
        {
            if (null == _printQueue)
            {
                Framework.Logger.LogInfo("Paused job resume failed, Print Queue is not accessible.");
                return false;
            }

            if (jobId <= 0)
            {
                Framework.Logger.LogInfo("Paused job resume failed, Job Id can not be less than 0.");
                return false;
            }

            // Query to get print job properties
            string searchQuery = "SELECT * FROM Win32_PrintJob";
            ManagementObjectSearcher searchPrintJobs = new ManagementObjectSearcher(searchQuery);

            try
            {
                ManagementObjectCollection printJobCollection = searchPrintJobs.Get();

                foreach (ManagementObject printJob in printJobCollection)
                {
                    // jobName will be of format: [Printer_Name], [Job_ID]
                    string jobName = printJob.Properties["Name"].Value.ToString();
                    char[] splitCharacter = { ',' };
                    string printerName = jobName.Split(splitCharacter)[0];
                    int printJobId = Convert.ToInt32(jobName.Split(splitCharacter)[1], CultureInfo.CurrentCulture);

                    if (String.Compare(printerName, _printQueue.Name, true, CultureInfo.CurrentCulture) == 0)
                    {
                        if (printJobId == jobId)
                        {
                            printJob.InvokeMethod("Resume", null);
                            Framework.Logger.LogDebug("Print job is resume successfully.");
                            return true;
                        }
                    }
                }

            }
            catch (Exception defaultException)
            {
                Framework.Logger.LogInfo("Job was not able to be resumed. Print job might be small and is completed before resuming.");
                Framework.Logger.LogInfo("Pause job resuming failed, Exception : {0}.".FormatWith(defaultException.JoinAllErrorMessages()));
            }
            finally
            {
                searchPrintJobs.Dispose();
            }

            return false;
        }

        /// <summary>
        /// Delete All existing Jobs in current print Queue
        /// </summary>
        public void DeleteAllPrintQueueJobs()
        {
            if (null == _printQueue)
            {
                Framework.Logger.LogInfo("Deletion of files failed, Print Queue is not accessible.");
                return;
            }

            DeleteAllQueueJobs();
        }

        /// <summary>
        /// Get SNMP connection status		
        /// </summary>
        /// <param name="ipAddress">IP Address of the device</param>
        /// <returns>true if SNMP operation is successful, false otherwise</returns>    
        /// TODO: Needs to be modified to Static method with AddressType as parameter    
        public bool IsSnmpAccessible(IPAddress ipAddress)
        {
            bool snmpConnect = false;
            DeviceStatus deviceStatus = DeviceStatus.Unknown;

            try
            {
                Snmp snmp = new Snmp(ipAddress);
                string deviceStatusVariable = snmp.Get(DEVICE_STATUS_OID);

                deviceStatus = (DeviceStatus)int.Parse(deviceStatusVariable, CultureInfo.InvariantCulture);
                if (!deviceStatus.Equals(DeviceStatus.Unknown))
                {
                    snmpConnect = true;
                }
            }
            catch
            {
                Framework.Logger.LogInfo("SNMP Connection with {0} IP Address is not successful".FormatWith(ipAddress));
                return snmpConnect;
            }

            Framework.Logger.LogInfo("SNMP Connection with {0} IP Address is successful".FormatWith(ipAddress));
            return snmpConnect;
        }

        /// <summary>
        /// Get Telnet connection status		
        /// </summary>
        /// <param name="ipAddress">IP Address of the device</param>
        /// <returns>true is connection successful, false otherwise</returns>
        /// TODO: Needs to be modified to Static method with AddressType as parameter        
        public bool IsTelnetAccessible(IPAddress ipAddress)
        {
            bool telnetConnect = false;
            TelnetIpc telnetIpc = null;

            try
            {
                telnetIpc = new TelnetIpc(ipAddress.ToString(), TELENET_PORTNO);
                telnetIpc.Connect();
                telnetConnect = true;
            }
            catch
            {
                Framework.Logger.LogInfo("Telnet Connection with {0} IP Address is not successful.".FormatWith(ipAddress));
                return telnetConnect;
            }
            finally
            {
                if (null != telnetIpc)
                {
                    telnetIpc.Dispose();
                }
            }

            Framework.Logger.LogInfo("Telnet Connection with {0} IP Address is successful.".FormatWith(ipAddress));
            return telnetConnect;
        }

        /// <summary>
        /// Get Telnet connection status		
        /// </summary>		
        /// <returns>Return true if the connection successful, else returns false</returns>
        public bool IsTelnetAccessible()
        {
            return IsTelnetAccessible(_wiredIPv4Address);
        }

        /// <summary>
        /// Check whether Web UI is accessible with http/ https hypertext
        /// Note: Currently Firefox is used as default browser to validate
        /// </summary>
        /// <param name="ipAddress">Host name/ IP Address of the device</param>
        /// <param name="hypertext">http/ https</param>
        /// <param name="model">Optional browser model</param>
        /// <returns>true if EWS page is accessible, false otherwise</returns>
        public bool IsEwsAccessible(string ipAddress, string hypertext = "http", BrowserModel model = BrowserModel.Firefox)
        {
            bool isAccessible = false;

            EwsSeleniumSettings seleniumSettings = new EwsSeleniumSettings();

            seleniumSettings.SeleniumChromeDriverPath = @"\\etlhubrepo\boi\CTC\SeleniumFiles\chromedriver.exe";
            seleniumSettings.SeleniumIEDriverPath32 = @"\\etlhubrepo\boi\CTC\SeleniumFiles\IEDriverServer-x86.exe";
            seleniumSettings.SeleniumIEDriverPath64 = @"\\etlhubrepo\boi\CTC\SeleniumFiles\IEDriverServer-x64.exe";
            SeleniumWebDriver seleniumWebDriver = new SeleniumWebDriver(model, EwsAdapter.CopyWebDriverEXEFiles(model, seleniumSettings));

            try
            {
                Uri uri = new Uri("{0}://{1}".FormatWith(hypertext, ipAddress));
                seleniumWebDriver.Start(model, uri);
                // In Negative scenario, page load event is taking more time hence a delay is added
                Thread.Sleep(TimeSpan.FromSeconds(10));
                string pageTitle = seleniumWebDriver.Title;
                if (!(pageTitle.Contains("Problem loading page") || pageTitle.Contains("This page") || seleniumWebDriver.Body.Contains("Access Denied.")))
                {
                    isAccessible = true;
                }
            }
            catch
            {
                Framework.Logger.LogInfo("Printer with {0} hostname/ IP Address is not accessible with {1} hypertext".FormatWith(ipAddress, hypertext));
                return isAccessible;
            }
            finally
            {
                StopWebDriver(seleniumWebDriver, model);
            }

            if (isAccessible)
            {
                Framework.Logger.LogInfo("Printer with {0} hostname/ IP Address is accessible with {1} hypertext".FormatWith(ipAddress, hypertext));
            }
            else
            {
                Framework.Logger.LogInfo("Printer with {0} hostname/ IP Address is not accessible with {1} hypertext".FormatWith(ipAddress, hypertext));
            }
            return isAccessible;
        }

        /// <summary>
        /// Check whether Web UI is accessible with http/ https hypertext
        /// Note: Currently Firefox is used as default browser to validate
        /// </summary>
        /// <param name="ipAddress">IP Address of the device</param>
        /// <param name="hypertext">http/ https</param>
        /// <returns>true if EWS page is accessible, false otherwise</returns>
        public bool IsEwsAccessible(IPAddress ipAddress, string hypertext = "http", BrowserModel model = BrowserModel.Firefox)
        {
            string localAddress = ipAddress.ToString();

            if (localAddress.Contains(":"))
            {
                localAddress = string.Concat("[", ipAddress.ToString(), "]");
            }

            return IsEwsAccessible(localAddress, hypertext, model);
        }

        /// <summary>
        /// Check whether Web UI is accessible with http/ https hypertext for primary address
        /// Note: Currently Firefox is used as default browser to validate
        /// </summary>		
        /// <param name="hypertext">http/ https</param>
        /// <returns>true if EWS page is accessible, false otherwise</returns>		
        public bool IsEwsAccessible(string hypertext = "http")
        {
            return IsEwsAccessible(_wiredIPv4Address, hypertext);
        }

        /// <summary>
        /// Check whether FTP connection is accessible or not
        /// </summary>
        /// <param name="ftpUri">Ftp Uri to be checked</param>
        /// <returns>true if <see cref=" ftpUri"/> is accessible, false otherwise</returns>
        public bool IsFTPAccessible(string ftpUri)
        {
            Uri ftpUrl = new Uri(ftpUri);
            FtpWebRequest ftpWebRequest = null;

            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(ftpUrl);

                // If below options are removed, FtpWebRequest throws up an exception while requesting for stream connection
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = false;
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;

                Stream ftpStream = ftpWebRequest.GetRequestStream();
                Framework.Logger.LogInfo("FTP is accessible.");
                return true;
            }
            catch (Exception exception)
            {
                Framework.Logger.LogInfo("FTP is not accessible.");
                Framework.Logger.LogDebug("Exception details: {0}".FormatWith(exception.Message));
                return false;
            }
            finally
            {
                ftpWebRequest.Abort();
            }
        }

        /// <summary>
        /// Check whether FTP connection is accessible or not with primary IP address
        /// </summary>		
        /// <returns>true if <see cref=" ftpUri"/> is accessible, false otherwise</returns>
        public bool IsFTPAccessible()
        {
            return IsFTPAccessible("ftp://{0}".FormatWith(_wiredIPv4Address.ToString()));
        }

        /// <summary>
        /// Check if SSH is accessible or not
        /// </summary>
        /// <param name="address">IP Address</param>
        /// <param name="userName">User Name</param>
        /// <param name="password">Password</param>
        /// <returns>true if accessible, false otherwise</returns>
        public bool IsSSHAccessible(IPAddress address, string userName, string password)
        {
            try
            {
                SSHProtocol sshProtocol = new SSHProtocol(userName, password, address);
                sshProtocol.Connect();
                Framework.Logger.LogInfo("SSH Connection with {0} IP Address is successful.".FormatWith(address));
                return true;
            }
            catch (Exception exception)
            {
                Framework.Logger.LogInfo("SSH Connection with {0} IP Address is not successful.".FormatWith(address));
                Framework.Logger.LogDebug("Exception details: {0}".FormatWith(exception.Message));
                return false;
            }
        }

        /// <summary>
        /// Print file using LPR command line
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="filePath">File path</param>
        /// <returns>true if successful, false otherwise</returns>
        public bool PrintLpr(IPAddress ipAddress, string filePath, Guid guid = default(Guid))
        {
            try
            {
                string result = ProcessUtil.Execute(@"lpr.exe", " -S {0} -P auto {1}".FormatWith(ipAddress.ToString(), filePath)).StandardOutput;

                if (string.IsNullOrEmpty(result))
                {
                    if (Guid.Empty != guid)
                    {
                        // Get the matching item from collection
                        var lprPrint = _lprPrintDetail.FirstOrDefault(i => i.JobId == guid);

                        // Remove the item from the collection, update the status and add modified item back to collection
                        _lprPrintDetail.Remove(lprPrint);
                        lprPrint.JobStatus = true;
                        _lprPrintDetail.Add(lprPrint);
                    }

                    Framework.Logger.LogInfo("Successfully printed the file: {0}".FormatWith(filePath));
                    return true;
                }
                else
                {
                    Framework.Logger.LogInfo("Failed to print the file: {0}".FormatWith(filePath));
                    Framework.Logger.LogInfo(result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Framework.Logger.LogInfo("Failed to print the file: {0}".FormatWith(filePath));
                Framework.Logger.LogInfo("Exception details: {0}".FormatWith(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// LPR Print Asynchronously.
        /// Note: To track the status of the job, use GetLPRPrintStatus() and match using Guid returned while calling this function.
        /// </summary>
        /// <param name="ipAddress">IP Address of printer</param>
        /// <param name="filePath">File Path</param>
        /// <returns>Guid created for the LPR Print Job</returns>
        public Guid PrintLprAsync(IPAddress ipAddress, string filePath)
        {
            // Guid will be ID for LPR print job
            Guid guid = Guid.NewGuid();

            if (null == _lprPrintDetail)
            {
                _lprPrintDetail = new Collection<LprPrintDetail>();
            }

            // Create a thread to call PrintLpr
            Thread lprThread = new Thread(() => PrintLpr(ipAddress, filePath, guid));

            // Build the structure for LPR print 
            LprPrintDetail lprPrintDetail = new LprPrintDetail();
            lprPrintDetail.JobId = guid;
            lprPrintDetail.JobThread = lprThread;
            lprPrintDetail.JobStatus = false;

            _lprPrintDetail.Add(lprPrintDetail);
            lprThread.Start();

            return guid;
        }

        /// <summary>
        /// Get the LPR Print Job details
        /// </summary>
        /// <returns>Collection of LPR Print Job details</returns>
        public virtual void KeepAwake()
        {
            Framework.Logger.LogInfo("Not implemented for VEP/TPS/Inkjet products");
        }

        /// <summary>
        /// Get the LPR Print Job details
        /// </summary>
        /// <returns>Collection of LPR Print Job details</returns>
        public Collection<LprPrintDetail> GetLprPrintStatus()
        {
            return _lprPrintDetail;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Tells whether printer is ready or not
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="timeout">Number of minutes to wait before it returns</param>
        /// <returns>Returns true if the printer is ready else returns false</returns>
        protected bool IsPrinterReady(IPAddress ipAddress, int timeout)
        {
            // first check the printer is in ping able state
            if (PingUntilTimeout(ipAddress, timeout))
            {
                // Some times printer  for the device to report that it is ready
                DateTime endTime = DateTime.Now + TimeSpan.FromMinutes(timeout);
                while (DateTime.Now < endTime)
                {
                    DeviceStatus status = GetDeviceStatus(ipAddress);

                    if (DeviceStatus.Running == status || DeviceStatus.Warning == status)
                    {
                        // After the printer reports "Running" or "Warning", it takes another minute to bring all the services up
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        return true;
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }

            return false;
        }

        // TODO: Based on our understanding there are two status of the printer 1. Device status 2. Printer status
        // we may need to write one more function to get the printer status.

        /// <summary>
        /// Gets the device status of the Printer
        /// </summary>
        /// <param name="ipAddress">IP address of printer</param>
        /// <returns>Returns device status</returns>
        private DeviceStatus GetDeviceStatus(IPAddress ipAddress)
        {
            DeviceStatus deviceStatus = DeviceStatus.Unknown;

            try
            {
                Snmp snmp = new Snmp(ipAddress);
                string deviceStatusVariable = snmp.Get(DEVICE_STATUS_OID);

                deviceStatus = (DeviceStatus)int.Parse(deviceStatusVariable, CultureInfo.InvariantCulture);
            }
            catch
            {
                Framework.Logger.LogError("Printer state was not able to be retrieved");
                return deviceStatus;
            }

            return deviceStatus;
        }

        /// <summary>
        /// Get Printer Status
        /// </summary>
        /// <param name="ipAddress">Printer IPv4 Address</param>
        /// <returns><see cref=" PrinterStatus"/></returns>
        private PrinterStatus GetPrinterStatus(IPAddress ipAddress)
        {
            PrinterStatus printerStatus = PrinterStatus.Unknown;

            try
            {
                Snmp snmp = new Snmp(ipAddress);
                string printerStatusVariable = snmp.Get(PRINTER_STATUS_OID);
                printerStatus = (PrinterStatus)int.Parse(printerStatusVariable, CultureInfo.InvariantCulture);
            }
            catch
            {
                Framework.Logger.LogError("Printer state was not able to be retrieved");
                return printerStatus;
            }

            return printerStatus;
        }

        /// <summary>
        /// Pings the printer until it is success or time out happens
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="timeout">Number of minutes to wait</param>
        /// <returns>Returns true if the printer is pinging in the timeout else return false</returns>
        public bool PingUntilTimeout(IPAddress ipAddress, int timeout)
        {
            return NetworkUtil.PingUntilTimeout(ipAddress, timeout);
        }

        /// <summary>
        /// Pings the printer until it is success or time out happens
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="timeout">Number of minutes to wait</param>
        /// <returns>Returns true if the printer is pinging in the timeout else return false</returns>
        public bool PingUntilTimeout(IPAddress ipAddress, TimeSpan timeout)
        {
            return NetworkUtil.PingUntilTimeout(ipAddress, timeout);
        }

        /// <summary>
        /// Pings the printer with embedded IPv4 address until it is success or time out of 30 seconds
        /// </summary>
        /// <returns>Returns true if the printer is pinging in the timeout else return false</returns>
        public bool Ping()
        {
            return NetworkUtil.PingUntilTimeout(_wiredIPv4Address, TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Checks Wireless option is available through Control Panel
        /// [applies only for VEP Products]
        /// </summary>
        /// <returns>returns true if success</returns>
        public bool IsWirelessOptionAvailable(string printerIP)
        {
            Framework.Logger.LogInfo("Validating the Wireless is enabled through Front Panel");

            try
            {
                JediWindjammerDevice device = new JediWindjammerDevice(printerIP, "admin");

                //Waking up the printer to enable the controls
                device.PowerManagement.Wake();

                // load the admin app
                device.ControlPanel.ScrollPressNavigate("mAccessPointDisplay", "AdminApp", "AdminAppMainForm", true);
                Thread.Sleep(TimeSpan.FromSeconds(1));

                //scroll button to get networking control
                device.ControlPanel.ScrollPress("mMenuTree", "NetworkingAndIOMenu");
                Thread.Sleep(TimeSpan.FromSeconds(1));

                if (device.ControlPanel.GetControls().Contains("JetDirectMenu2"))
                {
                    Framework.Logger.LogInfo("Wireless option is enabled through Front Panel on the Printer:{0}".FormatWith(printerIP));
                    return true;
                }
                else
                {
                    Framework.Logger.LogInfo("Wireless option is not enabled through Front Panel on the Printer:{0}".FormatWith(printerIP));
                    return false;
                }
            }
            catch (Exception controlException)
            {
                Framework.Logger.LogError(controlException.Message);
                return false;
            }
        }

        /// <summary>
        /// Get <see cref="PrintQueueStatus"/>
        /// </summary>
        /// <returns></returns>
        private PrintQueueStatus GetPrintQueueStatus()
        {
            if (null != _printQueue)
            {
                return _printQueue.QueueStatus;
            }

            return PrintQueueStatus.None;
        }

        /// <summary>
        /// Gets the DHCP Server IP on Which Printer is configured
        /// </summary>		
        /// <param name="ipAddress">IP address of printer</param>
        /// <returns>Returns DHCP Server IP</returns>
        public static IPAddress GetDHCPServerIP(IPAddress ipAddress)
        {
            IPAddress serverIP = IPAddress.Any;
            string dhcpServerIP = "1.3.6.1.4.1.11.2.4.3.16.2.0";

            try
            {
                Snmp snmp = new Snmp(ipAddress);
                string deviceStatusVariable = snmp.Get(dhcpServerIP);
                serverIP = IPAddress.Parse(deviceStatusVariable);
            }
            catch (Exception exception)
            {
                Framework.Logger.LogInfo("DHCP Server IP was not retrieved");
                Framework.Logger.LogError("Exception details : {0}.".FormatWith(exception));
                return serverIP;
            }

            return serverIP;
        }

        /// <summary>
        /// Gets the RouterIP on Which Printer is configured
        /// </summary>		
        /// <param name="ipAddress">IP address of printer</param>
        /// <returns>Returns Router IP address</returns>
        public static IPAddress GetRouterIPAddress(IPAddress ipAddress)
        {
            IPAddress routerIP = IPAddress.Any;
            string defaultGateway = "1.3.6.1.4.1.11.2.4.3.5.13.0";

            try
            {
                Snmp snmp = new Snmp(ipAddress);
                string deviceStatusVariable = snmp.Get(defaultGateway);
                routerIP = IPAddress.Parse(deviceStatusVariable);
            }
            catch (Exception exception)
            {
                Framework.Logger.LogInfo("Router IP was not retrieved");
                Framework.Logger.LogError("Exception details : {0}.".FormatWith(exception));
                return routerIP;
            }

            return routerIP;
        }

        /// <summary>
        /// Install print driver and add printer for different protocols
        /// </summary>
        /// <param name="ipAddress">Printer IP Address</param>
        /// <param name="protocol"><see cref="PrintProtocol"/></param>        
        /// <param name="driverPackPath">Printer Driver Path</param>
        /// <param name="driverModel">Printer Driver Model</param>
        /// <param name="printerPort">Printer Port number</param>                
        /// <param name="printQueue">Print Queue</param>
        /// <param name="snmpEnabled">Enable/ Disable SNMP option when Printer is added</param>
        /// <returns>true if Printer installed & added successfully, false otherwise</returns>
        private bool Install(string ipAddress, Printer.PrintProtocol protocol, string driverPackPath, string driverModel, int printerPort, string printerHostName, bool snmpEnabled = true)
        {
            try
            {
                var drivers = DriverController.LoadFromDirectory(driverPackPath, true, SearchOption.AllDirectories);
                // Select the first driver that matches the current architecture
                var driver =
                    (
                        from d in drivers
                        where d.Architecture == DriverController.LocalArchitecture &&
                            d.Name.Equals(driverModel, StringComparison.OrdinalIgnoreCase)
                        select d
                    ).FirstOrDefault();

                // Exit if current architecture doesn't match
                if (driver == null)
                {
                    Framework.Logger.LogInfo("Printer installation failed, unable to find current architecture for this driver");
                    return false;
                }

                string portName = null;
                string wsPrinterName = string.Empty;

                switch (protocol)
                {
                    case Printer.PrintProtocol.RAW:
                        portName = string.Format("IP_{0}:{1}", ipAddress, printerPort);
                        PrintPortManager.AddRawPort(portName, printerPort, ipAddress, snmpEnabled, "public", 1);
                        break;

                    case Printer.PrintProtocol.IPP:
                        // Check in case of IPv6 address
                        if (ipAddress.ToString().Contains(':'))
                        {
                            ipAddress = string.Concat("[", ipAddress.ToString(), "]");
                        }

                        portName = "http://{0}:{1}".FormatWith(ipAddress, printerPort);
                        break;

                    case Printer.PrintProtocol.IPPS:
                        portName = "https://{0}:{1}".FormatWith(printerHostName, printerPort);
                        break;

                    case Printer.PrintProtocol.LPD:
                        portName = string.Format("IP_{0}:{1}", ipAddress, printerPort);
                        PrintPortManager.AddLprPort(portName, Framework.Assets.LprPrinterPortInfo.DefaultPortNumber, ipAddress, "auto", snmpEnabled, "public", 1);
                        break;

                    case Printer.PrintProtocol.WSP:
                        DriverInstaller.Install(driver);
                        wsPrinterName = GetWSPrinterName(driverModel);
                        Framework.Logger.LogInfo("Printername : {0}".FormatWith(wsPrinterName));
                        if (string.IsNullOrEmpty(wsPrinterName) && null != NotifyWSPrinter)
                        {
                            NotifyWSPrinter();
                        }

                        wsPrinterName = GetWSPrinterName(driverModel);
                        Framework.Logger.LogInfo("Printername : {0}".FormatWith(wsPrinterName));
                        if (!string.IsNullOrEmpty(wsPrinterName))
                        {
                            _printQueue = PrintQueueController.GetPrintQueue(wsPrinterName);
                            return true;
                        }
                        Framework.Logger.LogInfo("Printer installation failed, WS Printer was not found.");
                        return false;

                    default:
                        Framework.Logger.LogError("Inappropriate Print protocol.");
                        return false;
                }

                string queueName = "{0} ({1})".FormatWith(driver.Name, portName);
                if (!PrintQueueInstaller.IsInstalled(queueName))
                {
                    DriverInstaller.Install(driver);
                    PrintQueueInstaller.CreatePrintQueue(queueName, driver.Name, portName, driver.PrintProcessor);
                    Framework.Logger.LogInfo("creating queue for port name: {0}".FormatWith(portName));
                    PrintQueueInstaller.WaitForInstallationComplete(queueName, driver.Name);
                    Framework.Logger.LogInfo("installation completed: {0}".FormatWith(queueName));
                }
                _printQueue = PrintQueueController.GetPrintQueue(queueName);
            }
            catch (Exception defaultException)
            {
                Framework.Logger.LogInfo("Printer installation failed due to one the following errors:");
                Framework.Logger.LogInfo("1. Windows couldn't connect to the printer");
                Framework.Logger.LogInfo("2. Specified Port is Unknown.\nException  details- {0}.".FormatWith(defaultException.JoinAllErrorMessages()));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Print File
        /// </summary>        
        /// <param name="printQueue"><see cref="PrintQueue"/></param>
        /// <param name="filePath">File Path</param>
        /// <param name="duration">Time duration</param>
        /// <param name="clearJobsOnError">Delete all jobs in case of printer error if true, ignore if false</param>
        /// <returns>true if all print jobs are successful, false otherwise</returns>
        private bool Print(string filePath, TimeSpan duration, bool clearJobsOnError = true)
        {
            string file = Path.GetFileName(filePath);

            if (!IsSupportedFile(filePath))
            {
                Framework.Logger.LogInfo("Printing failed, {0} is not supported for printing.".FormatWith(file));
                Framework.Logger.LogInfo("Supported file types : {0}.".FormatWith(string.Join(", ", SUPPORTED_FILES)));
                return false;
            }

            if (TimeSpan.Zero != duration)
            {
                Framework.Logger.LogInfo("Printing for duration of {0}.".FormatWith(duration.ToString(TIME_FORMAT, CultureInfo.CurrentCulture)));
            }

            _printJobMonitor = new PrintJobMonitor();
            _printJobData = new Dictionary<long, PrintJobData>();

            try
            {
                // Subscribe for synchronous Events
                _printJobMonitor.PrintJobSpooling += printJobMonitor_PrintJobSpooling;
                _printJobMonitor.PrintJobPrinting += printJobMonitor_PrintJobPrinting;
                _printJobMonitor.PrintJobMonitoringFinished += printJobMonitor_PrintJobFinished;

                _printJobMonitor.StartMonitor();

                PrintController controller = new PrintController();
                DateTime startTime = DateTime.Now;

                do
                {
                    Framework.Logger.LogInfo("Printing file : {0}.".FormatWith(Path.GetFileName(file)));
                    controller.Print(_printQueue, filePath);

                    do
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));

                        if (null != PrintQueueError && IsQueueInError())
                        {
                            PrintQueueError(this, null);
                        }

                        if (0 == _printJobData.Count || IsQueueInError())
                        {
                            break;
                        }
                    } while (true);

                    Framework.Logger.LogDebug("PrintQueue {0} State : {1}.".FormatWith(_printQueue.Name, _printQueue.QueueStatus));

                    if (_printJobData.Count != 0)
                    {
                        if (clearJobsOnError)
                        {
                            DeleteAllQueueJobs();
                        }

                        Framework.Logger.LogInfo("Print file : {0} failed to print.".FormatWith(Path.GetFileName(file)));

                        return false;
                    }

                } while (DateTime.Now.Subtract(startTime).TotalSeconds <= duration.TotalSeconds);

                Framework.Logger.LogDebug("Total time taken to print file : {0}.".FormatWith(DateTime.Now.Subtract(startTime).ToString(TIME_FORMAT, CultureInfo.CurrentCulture)));

                return true;
            }
            catch (Exception defaultException)
            {
                Framework.Logger.LogInfo("Printing failed, Exception : {0}.".FormatWith(defaultException.JoinAllErrorMessages()));
                return false;
            }
            finally
            {
                if (clearJobsOnError)
                {
                    DeleteAllQueueJobs();
                }

                _printJobData.Clear();
                _printJobMonitor.StopMonitor();
                _printJobMonitor = null;
            }
        }

        /// <summary>
        /// Print All files in collection
        /// </summary>        
        /// <param name="filesList">List of files to be printed</param>
        /// <param name="duration">Time duration</param>        
        /// <param name="clearJobsOnError">Delete all jobs in case of printer error if true, ignore if false</param>
        /// <returns>true if all print jobs are successful, false otherwise</returns>
        private bool Print(string[] filesList, TimeSpan duration, bool clearJobsOnError = true)
        {
            Collection<string> files = GetSupportedFiles(filesList);
            bool result = false;

            if (files.Count > 0)
            {
                if (TimeSpan.Zero == duration)
                {
                    DateTime startTime = DateTime.Now;
                    Framework.Logger.LogInfo("Printing all files.");

                    // print all the files once
                    foreach (string file in files)
                    {
                        if (Print(file, TimeSpan.Zero, clearJobsOnError))
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                            break;
                        }
                    }

                    Framework.Logger.LogInfo("Total time taken to print files : {0}.".FormatWith(DateTime.Now.Subtract(startTime).ToString(TIME_FORMAT, CultureInfo.CurrentCulture)));
                }
                else
                {
                    PrintContinuously(ref duration, clearJobsOnError, files, ref result);
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Print continuously for specified duration
        /// </summary>
        /// <param name="duration">Time duration</param>
        /// <param name="clearJobsOnError">Clear all jobs on failure</param>
        /// <param name="files">Collection of files to print</param>
        /// <param name="result"></param>
        private void PrintContinuously(ref TimeSpan duration, bool clearJobsOnError, Collection<string> files, ref bool result)
        {
            Framework.Logger.LogInfo("Printing for duration of {0} (hh:mm:ss).".FormatWith(duration.ToString(TIME_FORMAT, CultureInfo.CurrentCulture)));
            Collection<string> sendMaxFiles = new Collection<string>();
            int currentFilePosition = 0;

            // Check if files is less than MAX_FILES_TO_PRINT, add if required.
            // If exceeds, send only MAX_FILES_TO_PRINT

            if (files.Count < MAX_FILES_TO_PRINT)
            {
                // TODO : need to look for different logic
                for (int i = 0; i < MAX_FILES_TO_PRINT; i++)
                {
                    if (i >= files.Count)
                    {
                        i = (i % files.Count);
                    }

                    sendMaxFiles.Add(files[i]);

                    if (MAX_FILES_TO_PRINT == sendMaxFiles.Count)
                    {
                        currentFilePosition = i + 1;
                        break;
                    }
                }

            }
            else if (files.Count > MAX_FILES_TO_PRINT)
            {
                for (int i = 0; i < MAX_FILES_TO_PRINT; i++)
                {
                    sendMaxFiles.Add(files[i]);
                }
                currentFilePosition = MAX_FILES_TO_PRINT;

            }
            else
            {
                sendMaxFiles = files;
            }

            _printJobMonitor = new PrintJobMonitor();
            _printJobData = new Dictionary<long, PrintJobData>();

            try
            {
                _printJobMonitor.PrintJobSpooling += printJobMonitor_PrintJobSpooling_ContinuousPrinting;
                _printJobMonitor.PrintJobPrinting += printJobMonitor_PrintJobPrinting_ContinuousPrinting;
                _printJobMonitor.PrintJobMonitoringFinished += printJobMonitor_PrintJobFinished_ContinuousPrinting;

                _printJobMonitor.StartMonitor();

                PrintController controller = new PrintController();
                DateTime startTime = DateTime.Now;

                // Print MAX_FILES_TO_PRINT files
                foreach (string file in sendMaxFiles)
                {
                    Framework.Logger.LogInfo("Printing file : {0}.".FormatWith(Path.GetFileName(file)));
                    controller.Print(_printQueue, file);
                }

                // Check if duration is not exceeded and all jobs are completed
                while ((DateTime.Now.Subtract(startTime).TotalSeconds <= duration.TotalSeconds))
                {
                    if (null != PrintQueueError && IsQueueInError())
                    {
                        PrintQueueError(this, null);
                    }

                    // If PrintQueue reports an error again, return failure
                    if (IsQueueInError())
                    {
                        result = false;
                        break;
                    }

                    // Check if PrintQueue has MAX_FILES_TO_PRINT files, if not send 1 more job
                    if (_printJobData.Count < MAX_FILES_TO_PRINT)
                    {
                        Framework.Logger.LogInfo("Printing file : {0}.".FormatWith(Path.GetFileName(files[currentFilePosition])));
                        controller.Print(_printQueue, files[currentFilePosition]);

                        currentFilePosition += 1;
                        if (files.Count <= currentFilePosition)
                        {
                            currentFilePosition = currentFilePosition % files.Count;
                        }
                    }
                };

                // Wait till all the jobs in PrintQueue is completed
                if (0 != _printJobData.Count)
                {
                    do
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));

                        if (null != PrintQueueError && IsQueueInError())
                        {
                            PrintQueueError(this, null);
                        }

                        // If PrintQueue reports an error again, return failure
                        if (IsQueueInError())
                        {
                            result = false;
                            break;
                        }

                        // If all jobs are completed, return true
                        if (0 == _printJobData.Count)
                        {
                            result = true;
                            break;
                        }

                    } while (true);
                }

                Framework.Logger.LogInfo("Total time taken to print files : {0} (hh:mm:ss).".FormatWith(DateTime.Now.Subtract(startTime).ToString(TIME_FORMAT, CultureInfo.CurrentCulture)));
            }
            finally
            {
                if (clearJobsOnError)
                {
                    DeleteAllQueueJobs();
                }

                _printJobData.Clear();
                _printJobMonitor.StopMonitor();
                _printJobMonitor = null;
            }
        }

        /// <summary>
        /// Print using FTP
        /// </summary>
        /// <param name="ipAddress">IP Address of the Printer</param>
        /// <param name="userName">User Name</param>
        /// <param name="password">Password</param>
        /// <param name="filePath">File path</param>        
        /// <returns>true if print successful, false otherwise</returns>
        private bool PrintFtp(string ipAddress, string userName, string password, string filePath, bool isPassiveMode = false)
        {
            FtpWebRequest ftpWebRequest = null;
            Stream ftpStream = null;
            int bufferSize = 1024;

            if (!IsSupportedFile(filePath, true))
            {
                Framework.Logger.LogInfo("Printing with FTP failed, {0} is not supported for printing.".FormatWith(filePath));
                Framework.Logger.LogInfo("Supported file types : {0}.".FormatWith(string.Join(", ", FTP_SUPPORTED_FILES)));
                return false;
            }

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long uploadSize = fileInfo.Length;

                // Enclose address with square brackets if IPv6 address
                if (ipAddress.Contains(":"))
                {
                    ipAddress = string.Concat("[", ipAddress.ToString(), "]");
                }

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(@"{0}{1}/{2}".FormatWith(FTP_URL_PREFIX, ipAddress, Path.GetFileName(filePath)));
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = isPassiveMode;
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;

                ftpStream = ftpWebRequest.GetRequestStream();

                FileStream localFileStream = new FileStream(filePath, FileMode.Open);
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                long bytesTransferred = 0;

                Framework.Logger.LogInfo("Sending file {0} over FTP with {1} mode.".FormatWith(Path.GetFileName(filePath), isPassiveMode ? "passive" : "active"));

                // Upload the file by sending the buffered data until it reach the size of the file.
                try
                {
                    do
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesTransferred += bytesSent;
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                        if (bytesTransferred.Equals(uploadSize))
                        {
                            break;
                        }
                    } while (bytesTransferred <= uploadSize);

                }
                catch (Exception defaultException)
                {
                    Console.WriteLine(defaultException.ToString());
                    return false;
                }
                // Resource cleanup
                localFileStream.Close();
                localFileStream.Dispose();
                ftpStream.Close();
                FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse();
                Framework.Logger.LogDebug("Upload file Complete, status : {0}.".FormatWith(response.StatusDescription));
                Framework.Logger.LogInfo("Print file : {0} printed successfully.".FormatWith(Path.GetFileName(filePath)));
                ftpWebRequest.Abort();
                response.Close();
                ftpWebRequest = null;
            }
            catch (WebException webException)
            {
                Framework.Logger.LogInfo("Printing with FTP failed, Exception : {0}.".FormatWith(webException.Message));
                return false;
            }
            catch (Exception defaultException)
            {
                Framework.Logger.LogInfo("Printing with FTP failed, Exception : {0}.".FormatWith(defaultException.JoinAllErrorMessages()));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Print list of files with FTP
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="userName">UserName</param>
        /// <param name="password">Password</param>
        /// <param name="fileList">List of files to be printed</param>
        /// <param name="duration">time period for job printing</param>
        /// <returns>true if all the listed jobs are printed, false otherwise</returns>
        private bool PrintFtp(string ipAddress, string userName, string password, string[] fileList, TimeSpan duration, bool isPassiveMode = false)
        {
            bool result = false;
            string[] filesToPrint = new string[fileList.Length];
            string[] unsupportedFiles = new string[fileList.Length];
            int supportingFiles = 0;
            int unsupportingFiles = 0;

            for (int i = 0; i < fileList.Length; i++)
            {
                if (IsSupportedFile(fileList[i], true))
                {
                    filesToPrint[supportingFiles] = fileList[i];
                    supportingFiles++;
                }
                else
                {
                    unsupportedFiles[unsupportingFiles] = fileList[i];
                    unsupportingFiles++;
                }
            }

            if (supportingFiles.Equals(0))
            {
                Framework.Logger.LogInfo("Printing with FTP failed, no supported files were available for print.");
                Framework.Logger.LogInfo("Supported file types : {0}.".FormatWith(string.Join(", ", FTP_SUPPORTED_FILES)));
                return false;
            }

            if (supportingFiles != fileList.Length)
            {
                Framework.Logger.LogInfo("Following files are not supported for printing : {0}.".FormatWith(string.Join(", ", unsupportedFiles)));
                Framework.Logger.LogInfo("Supported file types : {0}.".FormatWith(string.Join(", ", FTP_SUPPORTED_FILES)));
            }

            if (TimeSpan.Zero != duration)
            {
                Framework.Logger.LogInfo("Printing for duration of {0} (hh:mm:ss).".FormatWith(duration.ToString(TIME_FORMAT, CultureInfo.CurrentCulture)));
            }

            filesToPrint = filesToPrint.Where(item => item != null).ToArray();
            DateTime startTime = DateTime.Now;
            do
            {
                foreach (string file in filesToPrint)
                {
                    result = PrintFtp(ipAddress, userName, password, file, isPassiveMode);
                }
            } while (result && (DateTime.Now.Subtract(startTime).TotalSeconds <= duration.TotalSeconds));

            return result;
        }

        /// <summary>
        /// Print file Asynchronously 
        /// </summary>        
        /// <param name="printQueue"><see cref="PrintQueue"/></param>
        /// <param name="filePath">File Path</param>        
        private void PrintAsync(string filePath)
        {
            string file = Path.GetFileName(filePath);

            if (!IsSupportedFile(filePath))
            {
                Framework.Logger.LogInfo("Printing failed, {0} is not supported for printing.".FormatWith(file));
                Framework.Logger.LogInfo("Supported file types: {0}.".FormatWith(string.Join(", ", SUPPORTED_FILES)));
            }

            _printJobMonitor = new PrintJobMonitor();
            _printJobData = new Dictionary<long, PrintJobData>();

            try
            {
                // Subscribe for Events
                _printJobMonitor.PrintJobSpooling += printJobMonitor_PrintJobSpooling_Async;
                _printJobMonitor.PrintJobPrinting += printJobMonitor_PrintJobPrinting_Async;
                _printJobMonitor.PrintJobMonitoringFinished += printJobMonitor_PrintJobFinished_Async;

                _printJobMonitor.StartMonitor();

                Framework.Logger.LogInfo("Printing file : {0}.".FormatWith(file));

                PrintController controller = new PrintController();
                controller.Print(_printQueue, filePath);

                Framework.Logger.LogDebug("PrintQueue {0} State : {1}.".FormatWith(_printQueue.Name, _printQueue.QueueStatus));
            }
            catch (Exception defaultException)
            {
                Framework.Logger.LogInfo("Printing failed, Exception : {0}.".FormatWith(defaultException.JoinAllErrorMessages()));
            }
        }

        /// <summary>
        /// Print all files Asynchronously 
        /// </summary>        
        /// <param name="printQueue"><see cref="PrintQueue"/></param>
        /// <param name="fileList">List of all files</param>        
        private void PrintAsync(string[] fileList)
        {
            Collection<string> filesToPrint = new Collection<string>();
            Collection<string> unsupportedFiles = new Collection<string>();
            int supportingFiles = 0;
            int unsupportingFiles = 0;

            for (int i = 0; i < fileList.Length; i++)
            {
                if (IsSupportedFile(fileList[i]))
                {
                    filesToPrint.Add(fileList[i]);
                    supportingFiles++;
                }
                else
                {
                    unsupportedFiles.Add(fileList[i]);
                    unsupportingFiles++;
                }
            }

            if (supportingFiles.Equals(0))
            {
                Framework.Logger.LogInfo("Printing failed, no supported files were available for print.");
                Framework.Logger.LogInfo("Supported file types : {0}.".FormatWith(string.Join(", ", SUPPORTED_FILES)));
                return;
            }

            if (supportingFiles != fileList.Length)
            {
                Framework.Logger.LogInfo("Following files are not supported for printing : {0}.".FormatWith(string.Join(", ", unsupportedFiles)));
            }

            _printJobMonitor = new PrintJobMonitor();
            _printJobData = new Dictionary<long, PrintJobData>();

            try
            {
                // Subscribe for Events
                _printJobMonitor.PrintJobSpooling += printJobMonitor_PrintJobSpooling_Async;
                _printJobMonitor.PrintJobPrinting += printJobMonitor_PrintJobPrinting_Async;
                _printJobMonitor.PrintJobMonitoringFinished += printJobMonitor_PrintJobFinished_Async;

                _printJobMonitor.StartMonitor();

                PrintController controller = new PrintController();

                // Send all jobs
                foreach (string file in filesToPrint)
                {
                    Framework.Logger.LogInfo("Printing file : {0}.".FormatWith(file));
                    controller.Print(_printQueue, file);
                }

                Framework.Logger.LogDebug("PrintQueue {0} State : {1}.".FormatWith(_printQueue.Name, _printQueue.QueueStatus));
            }
            catch (Exception defaultException)
            {
                Framework.Logger.LogInfo("Printing failed, Exception : {0}.".FormatWith(defaultException.JoinAllErrorMessages()));
            }
        }

        /// <summary>
        /// Print using FTP
        /// </summary>
        /// <param name="ipAddress">IP Address of the Printer</param>
        /// <param name="userName">User Name</param>
        /// <param name="password">Password</param>
        /// <param name="filePath">File path</param>        
        private void PrintFtpAsync(string ipAddress, string userName, string password, string filePath, bool isPassiveMode = false)
        {
            FtpWebRequest ftpWebRequest = null;
            Stream ftpStream = null;
            int bufferSize = 1024;
            FtpPrintingEventArgs eventArgs = new FtpPrintingEventArgs();

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long uploadSize = fileInfo.Length;

                // Enclose address with square brackets if IPv6 address
                if (ipAddress.Contains(":"))
                {
                    ipAddress = string.Concat("[", ipAddress.ToString(), "]");
                }

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(@"{0}{1}/{2}".FormatWith(FTP_URL_PREFIX, ipAddress, Path.GetFileName(filePath)));
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = isPassiveMode;
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;

                ftpStream = ftpWebRequest.GetRequestStream();

                FileStream localFileStream = new FileStream(filePath, FileMode.Open);
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                long bytesTransferred = 0;

                Framework.Logger.LogInfo("Sending file {0} over FTP with {1} mode.".FormatWith(Path.GetFileName(filePath), isPassiveMode ? "passive" : "active"));

                // Upload the file by sending the buffered data until it reach the size of the file.
                try
                {
                    do
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesTransferred += bytesSent;
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                        if (null != FTPJobPrinting)
                        {
                            eventArgs.SentBytes = bytesTransferred;
                            eventArgs.TotalBytes = uploadSize;
                            eventArgs.Abort = false;
                            FTPJobPrinting(this, eventArgs);
                        }

                        if (true == eventArgs.Abort)
                        {
                            localFileStream.Close();
                            localFileStream.Dispose();
                            ftpStream.Close();
                            ftpWebRequest.Abort();
                            ftpWebRequest = null;
                            Framework.Logger.LogInfo("FTP job {0} is aborted successfully. File size : {1} byte(s), transferred size : {2} byte(s)."
                                .FormatWith(Path.GetFileName(filePath), uploadSize, bytesTransferred));
                            return;
                        }

                        if (bytesTransferred.Equals(uploadSize))
                        {
                            break;
                        }
                    } while (bytesTransferred <= uploadSize);
                }
                catch (Exception defaultException)
                {
                    Console.WriteLine(defaultException.ToString());
                }
                // Resource cleanup
                localFileStream.Close();
                localFileStream.Dispose();
                ftpStream.Close();
                FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse();
                Framework.Logger.LogDebug("Upload file Complete, status : {0}.".FormatWith(response.StatusDescription));
                Framework.Logger.LogInfo("Print file : {0} printed successfully.".FormatWith(Path.GetFileName(filePath)));
                ftpWebRequest.Abort();
                response.Close();
                ftpWebRequest = null;
            }
            catch (WebException webException)
            {
                Framework.Logger.LogInfo("Printing with FTP failed, Exception : {0}".FormatWith(webException.Message));
            }
            catch (Exception defaultException)
            {
                Framework.Logger.LogInfo("Printing with FTP failed, Exception : {0}.".FormatWith(defaultException.JoinAllErrorMessages()));
            }
        }

        /// <summary>
        /// Cancel print job
        /// </summary>
        /// <param name="printQueue"><see cref=" PrintQueue"/></param>
        /// <param name="jobId">Id of print job</param>
        /// <returns></returns>
        public bool CancelPrintJob(long jobId)
        {
            if (null == _printQueue)
            {
                Framework.Logger.LogInfo("Cancel job failed, Print Queue is not accessible.");
                return false;
            }

            if (jobId <= 0)
            {
                Framework.Logger.LogInfo("Cancel job failed, Job Id can not be less than 0.");
                return false;
            }

            // Query to get print job properties
            string searchQuery = "SELECT * FROM Win32_PrintJob";
            ManagementObjectSearcher searchPrintJobs = new ManagementObjectSearcher(searchQuery);

            try
            {
                ManagementObjectCollection printJobCollection = searchPrintJobs.Get();

                foreach (ManagementObject printJob in printJobCollection)
                {
                    // jobName will be of format: [Printer_Name], [Job_ID]
                    string jobName = printJob.Properties["Name"].Value.ToString();
                    char[] splitCharacter = { ',' };
                    string printerName = jobName.Split(splitCharacter)[0];
                    int printJobId = Convert.ToInt32(jobName.Split(splitCharacter)[1], CultureInfo.CurrentCulture);
                    string documentName = printJob.Properties["Document"].Value.ToString();

                    if (String.Compare(printerName, _printQueue.Name, true, CultureInfo.CurrentCulture) == 0)
                    {
                        if (printJobId == jobId)
                        {
                            // cancel job
                            printJob.Delete();
                            Framework.Logger.LogDebug("Print file : {0} is cancelled successfully.".FormatWith(documentName));
                            return true;
                        }
                    }
                }

            }
            catch (Exception defaultException)
            {
                Framework.Logger.LogInfo("Print job cancellation failed, error in cancel print job operation. Exception : {0}."
                    .FormatWith(defaultException.Message));
            }
            finally
            {
                searchPrintJobs.Dispose();
            }

            return false;
        }

        /// <summary>
        /// Get the Web Service Printer name from registry based on driver model name 
        /// and Port name of the printer
        /// </summary>
        /// <param name="driverModel">Driver Model name of the printer</param>
        /// <returns>Printer Name if found, null otherwise</returns>
        private static string GetWSPrinterName(string driverModel)
        {
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
                        if (driverName.Equals(driverModel) && portName[0].StartsWith("WSD", StringComparison.CurrentCulture))
                        {
                            return printerName = dsSpooler.GetValue("printerName").ToString();
                        }
                    }
                }
            }
            Framework.Logger.LogInfo("Printer name : {0}".FormatWith(printerName));
            return printerName;
        }

        /// <summary>
        /// Check whether file is supported for printing
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="isFTPFile">true to check for FTP file, false otherwise </param>
        /// <returns>true if file is supported, false otherwise</returns>
        private bool IsSupportedFile(string filePath, bool isFTPSupported = false)
        {
            string[] supportedFiles = isFTPSupported == true ? FTP_SUPPORTED_FILES : SUPPORTED_FILES;

            return supportedFiles.Contains(Path.GetExtension(filePath), StringComparer.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Gets the supported files list based on the FTP option flag.
        /// This method also displays the log details.
        /// </summary>
        /// <param name="files">list of files</param>
        /// <returns>contains supported files</returns>
        private Collection<string> GetSupportedFiles(string[] files, bool isFtpSupported = false)
        {
            Collection<string> supportedFiles = new Collection<string>();

            // walk thru the files collection
            foreach (string file in files)
            {
                if (IsSupportedFile(file, isFtpSupported))
                {
                    supportedFiles.Add(file);
                }
                else
                {
                    Framework.Logger.LogInfo("File {0} not supported for this print operation.".FormatWith(file));
                }
            }

            if (supportedFiles.Count == 0)
            {
                Framework.Logger.LogInfo("Printing failed, no supported files were available for print.");

                if (isFtpSupported)
                {
                    Framework.Logger.LogInfo("Supported file types : {0}.".FormatWith(string.Join(", ", FTP_SUPPORTED_FILES)));
                }
                else
                {
                    Framework.Logger.LogInfo("Supported file types : {0}.".FormatWith(string.Join(", ", SUPPORTED_FILES)));
                }
            }

            return supportedFiles;
        }

        ///<summary>
        /// Delete all the jobs present in the print Queue
        /// Note: PrintQueue.Purge(), won't clear all the jobs sometime hence the below approach was taken
        /// </summary>
        /// <param name="printQueue"><see cref=" PrintQueue"/></param>
        private void DeleteAllQueueJobs()
        {
            string searchQuery;
            ManagementObjectSearcher searchPrinters;
            ManagementObjectCollection printerCollection;
            searchQuery = "SELECT * FROM Win32_Printer" + @" Where Name='{0}'".FormatWith(_printQueue.FullName);
            searchPrinters = new ManagementObjectSearcher(searchQuery);

            try
            {
                printerCollection = searchPrinters.Get();
                foreach (ManagementObject printer in printerCollection)
                {
                    printer.InvokeMethod("CancelAllJobs", null);
                }
            }
            catch (Exception defaultException)
            {
                Framework.Logger.LogError("Exception : {0}.".FormatWith(defaultException.Message));
            }
            finally
            {
                searchPrinters.Dispose();
            }
        }

        /// <summary>
        /// Check if <see cref=" PrintQueue"/> is in error state
        /// </summary>
        /// <param name="printQueue"><see cref="PrintQueue"/></param>
        /// <returns>true if in error, false otherwise</returns>
        private bool IsQueueInError()
        {
            // Note: Create new print queue object based on the current print queue name.
            // We are doing this because Microsoft throws an exception "TODO: Ram write the exception details". 
            // If this method is called in a different thread in case of asynchronous calls
            PrintQueue printQueue = PrintQueueController.GetPrintQueue(_printQueue.Name);
            printQueue.Refresh();

            if (printQueue.QueueStatus == PrintQueueStatus.DoorOpen | printQueue.QueueStatus == PrintQueueStatus.Error |
                printQueue.QueueStatus == PrintQueueStatus.NoToner | printQueue.QueueStatus == PrintQueueStatus.Offline |
                printQueue.QueueStatus == PrintQueueStatus.OutputBinFull | printQueue.QueueStatus == PrintQueueStatus.PaperJam |
                printQueue.QueueStatus == PrintQueueStatus.PaperOut | printQueue.QueueStatus == PrintQueueStatus.TonerLow |
                printQueue.QueueStatus == PrintQueueStatus.UserIntervention)
            {
                Framework.Logger.LogInfo("PrintQueue {0} experienced an error while printing the document.".FormatWith(printQueue.Name));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Stop Print Job Monitoring 
        /// </summary>
        private void StopJobMonitoring()
        {
            Framework.Logger.LogDebug("Checking all jobs are completed.");

            if (null != _printQueue)
            {
                if (_printJobData.Count == 0)
                {
                    Framework.Logger.LogDebug("Print Job Monitor is stopped.");

                    _printJobData.Clear();
                    _printJobMonitor.StopMonitor();
                }
            }
        }

        /// <summary>
        /// Set all Printer Properties via discovering the printer using bacabodtool
        /// Note: All properties which are dependent on discovery method, need to call this function before usage
        /// </summary>
        private void PopulatePrinterProperties()
        {
            if (!_isPrinterDiscovered)
            {
                DeviceInfo deviceInfo = PrinterDiscovery.ProbeDevice(_wiredIPv4Address);
                _statelessAddress = new Collection<IPAddress>();
                TraceFactory.Logger.Info("Probed device :{0}".FormatWith(deviceInfo));

                if (null != deviceInfo)
                {
                    _isPrinterDiscovered = true;

                    // Set All Properties

                    _linkLocalAddress = deviceInfo.LinkLocalAddress;
                    _statefullAddress = deviceInfo.StateFullAddress;
                    _statelessAddress = deviceInfo.StateLessAddress;
                    _macAddress = deviceInfo.MacAddress;
                    Framework.Logger.LogDebug("Printer MAC Address from DEvice infor.Printer.cs = {0}".FormatWith(_macAddress));
                    _hostName = deviceInfo.HostName;
                }
                //This is not working so trying to  if with old 4.7 it works on not ->It works 

                //if (!DiscoverPrinterDetails())
                //{
                //    // Discovering twice to get the printer detaisl.
                //    Framework.Logger.LogDebug("Bacabod discovery failed. Discovering again.");
                //    if (!DiscoverPrinterDetails())
                //    {
                //        throw new Exception("Bacabod discovery failed.".ToUpperInvariant());
                //    }
                //}
            }
        }

        private bool DiscoverPrinterDetails()
        {
            List<IPAddress> ipv6Address = new List<IPAddress>();
            _statelessAddress = new Collection<IPAddress>();

            if (DiscoverDevice(WiredIPv4Address.ToString(), ref _macAddress, ref _hostName, ref ipv6Address))
            {
                _linkLocalAddress = ipv6Address.FirstOrDefault(x => x.IsIPv6LinkLocal);

                //remove the link local address from address list
                ipv6Address.Remove(_linkLocalAddress);

                // Stateless address ends with part of link local address excluding fe80::
                string linklocalSuffix = _linkLocalAddress.ToString().Replace("fe80::", string.Empty);
                _statelessAddress = new Collection<IPAddress>(ipv6Address.Where(x => x.ToString().EndsWith(linklocalSuffix)).ToList());
                _statefullAddress = ipv6Address.Except(_statelessAddress).FirstOrDefault();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get document name for the Unique file name generated in PrintEventAgrs
        /// </summary>
        /// <param name="fullDocumentName"></param>
        /// <returns></returns>
        private static string GetDocumentName(string fullDocumentName)
        {
            string documentName = string.Empty;
            char[] splitChar = { '_' };

            string[] splitArray = fullDocumentName.Split(splitChar);

            if (splitChar.Length > 0)
            {
                documentName = splitArray[2] == null ? string.Empty : splitArray[2];
            }

            return documentName;
        }

        /// <summary>
        /// Stop Ews adapter
        /// </summary>
        /// <param name="webDriver"><see cref="SeleniumWebDriver"/></param>
        /// <param name="browser"><see cref="BrowserModel"/></param>
        public void StopWebDriver(SeleniumWebDriver webDriver, BrowserModel browser)
        {
            try
            {
                webDriver.Stop();
                webDriver.Dispose();
            }
            catch (Exception generalException)
            {
                Framework.Logger.LogDebug("Exception details: {0}".FormatWith(generalException.Message));
                KillBrowser(browser);
            }
        }

        /// <summary>
        /// Kill browser
        /// </summary>
        /// <param name="browser"><see cref="BrowserModel"/></param>
        private void KillBrowser(BrowserModel browser)
        {
            Framework.Logger.LogDebug("Killing all {0} browsers.".FormatWith(browser));
            Process[] browserProcess = Process.GetProcessesByName(browser.ToString());

            try
            {
                foreach (Process process in browserProcess)
                {
                    process.Kill();
                }

                for (int i = 0; i < 4; i++)
                {
                    browserProcess = Process.GetProcessesByName(browser.ToString());

                    if (0 == browserProcess.Length)
                    {
                        return;
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(30));
                    foreach (Process process in browserProcess)
                    {
                        process.Kill();
                    }
                }
            }
            catch
            {
                // do nothing
            }
        }
        
        /// <summary>
        /// Discovers the device using the BacaBod tool based on the source type and discovery type
        /// </summary>
        /// <param name="address">The IP address</param>
        /// <param name="macAddress">Mac address</param>
        /// <param name="ipv6Address">IPv6 Address</param>
        /// <param name="hostName">Host Name</param>
        /// <returns>Returns true if the device is discovered, else returns false</returns>
		private bool DiscoverDevice(string address, ref string macAddress, ref string hostName, ref List<IPAddress> ipv6Address)
        {
            var bacabodToolPath = "BacabodTool.exe";
            var dllPath = "sdisdk.dll";

            TraceFactory.Logger.Info("Path :{0}".FormatWith(bacabodToolPath));
            if (!File.Exists(bacabodToolPath))
            {
                File.WriteAllBytes(bacabodToolPath, Properties.Resource.BacabodTool);
            }

            if (!File.Exists(dllPath))
            {
                File.WriteAllBytes(dllPath, Properties.Resource.sdisdk);
            }

            Framework.Logger.LogDebug($"Discovering device with IPAddress:{address} using bacabod tool.");

            var processResult = HP.ScalableTest.Utility.ProcessUtil.Execute(bacabodToolPath, $"{address} {BacaBodSourceType.IPAddress.GetHashCode()} {BacaBodDiscoveryType.All.GetHashCode()} {CastType.Unicast.GetHashCode()}");

            // Output Format: DiscoveryStatus(True/False)|IPv4Address|Mac Address|Hostname|IPv6 Addresses(space separated)
            string output = processResult.StandardOutput;

            bool result = output.StartsWith("True", StringComparison.CurrentCultureIgnoreCase);

            if (result)
            {
                _isPrinterDiscovered = true;

                string[] arr = output.Split('|');

                if (arr.Length > 2)
                {
                    macAddress = arr[2].Trim();
                }

                if (arr.Length > 3)
                {
                    hostName = arr[3].Trim();
                }

                if (arr.Length > 4)
                {
                    foreach(var item in arr[4].Trim().Split(' '))
                    {
                        ipv6Address.Add(IPAddress.Parse(item));
                    }
                }

                Framework.Logger.LogDebug($"Discovering device with IPAddress:{address} using bacabod tool.");
                Framework.Logger.LogDebug($"Mac Address:{macAddress}");
                Framework.Logger.LogDebug($"Host Name: {hostName}");
                Framework.Logger.LogDebug($"IPv6 Address: {ipv6Address}");
            }
            else
            {
                Framework.Logger.LogDebug("Printer not Discovered using Bacabod tool");
            }

            return result;
        }

        #endregion

        #region Print Job Event Handler

        #region Synchronous Events

        /// <summary>
        /// PrintJobMonitor for Job Spooling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printJobMonitor_PrintJobSpooling(object sender, PrintJobDataEventArgs e)
        {
            try
            {
                _printJobData.Add(e.Job.Id, e.Job);
                //Framework.Logger.LogDebug("Spooling event raised. Adding PrintJobData, Id : {0}, Document : {1}".
                //	FormatWith(e.Job.Id, e.Job.Document == null ? "Empty" : e.Job.Document));

                if (null != PrintJobSpooling)
                {
                    PrintJobSpooling(sender, e);
                }
            }
            catch (Exception ex)
            {
                Framework.Logger.LogInfo("Exception occured at PrintJob spoolings : {0}".FormatWith(ex.Message));
            }
        }

        /// <summary>
        /// PrintJobMonitor for Job Printing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printJobMonitor_PrintJobPrinting(object sender, PrintJobDataEventArgs e)
        {
            //Framework.Logger.LogDebug("Printing event raised. Id : {0}, Document : {1}".
            //	FormatWith(e.Job.Id, e.Job.Document == null ? "Empty" : e.Job.Document));
            try
            {
                if (null != PrintJobPrinting)
                {
                    PrintJobPrinting(sender, e);
                }
            }
            catch (Exception ex)
            {
                Framework.Logger.LogInfo("Exception occured at PrintJob printing : {0}".FormatWith(ex.Message));
            }
        }

        /// <summary>
        /// PrintJobMonitor for Job Finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printJobMonitor_PrintJobFinished(object sender, PrintJobDataEventArgs e)
        {
            try
            {
                if (e.Job.Status.HasFlag(PrintJobStatus.Deleted))
                {
                    _printJobData.Remove(e.Job.Id);

                    //Framework.Logger.LogDebug("Finished event raised. Id : {0}, Document : {1}".
                    //	FormatWith(e.Job.Id, e.Job.Document == null ? "Empty" : e.Job.Document));
                    Framework.Logger.LogInfo("Print file : {0} printed {1} page(s) successfully.".
                        FormatWith(e.Job.Document == null ? "Empty" : e.Job.Document, e.Job.TotalPages));

                    if (null != PrintJobFinished)
                    {
                        PrintJobFinished(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Framework.Logger.LogInfo("Exception occured at PrintJob Finished : {0}".FormatWith(ex.Message));

            }
        }

        #endregion

        #region Continous Printing Events

        /// <summary>
        /// Print Job Monitor for spooling event for Continuous Printing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printJobMonitor_PrintJobSpooling_ContinuousPrinting(object sender, PrintJobDataEventArgs e)
        {
            try
            {
                _printJobData.Add(e.Job.Id, e.Job);
                //Framework.Logger.LogDebug("Spooling event raised. Adding PrintJobData, Id : {0}, Document : {1}".
                //	FormatWith(e.Job.Id, e.Job.Document == null ? "Empty" : e.Job.Document));

                if (null != PrintJobSpooling)
                {
                    PrintJobSpooling(sender, e);
                }
            }
            catch (Exception ex)
            {
                Framework.Logger.LogInfo("Exception occured at PrintJob spooling Continuous printing : {0}".FormatWith(ex.Message));

            }
        }

        /// <summary>
        /// Print Job Monitor for printing event for Continuous Printing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printJobMonitor_PrintJobPrinting_ContinuousPrinting(object sender, PrintJobDataEventArgs e)
        {
            try
            {
                //Framework.Logger.LogDebug("Printing event raised. Id : {0}, Document : {1}".
                //	FormatWith(e.Job.Id, e.Job.Document == null ? "Empty" : e.Job.Document));

                if (null != PrintJobPrinting)
                {
                    PrintJobPrinting(sender, e);
                }
            }
            catch (Exception ex)
            {
                Framework.Logger.LogInfo("Exception occured at PrintJob printing conitnuous printing  : {0}".FormatWith(ex.Message));
            }
        }

        /// <summary>
        /// Print Job Monitor for finished event for Continuous Printing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printJobMonitor_PrintJobFinished_ContinuousPrinting(object sender, PrintJobDataEventArgs e)
        {
            try
            {
                if (e.Job.Status.HasFlag(PrintJobStatus.Deleted))
                {
                    _printJobData.Remove(e.Job.Id);

                    //Framework.Logger.LogDebug("Finished event raised. Id : {0}, Document : {1}".
                    //	FormatWith(e.Job.Id, e.Job.Document == null ? "Empty" : e.Job.Document));
                    Framework.Logger.LogInfo("Print file : {0} printed {1} page(s) successfully.".
                        FormatWith(e.Job.Document == null ? "Empty" : e.Job.Document, e.Job.TotalPages));

                    if (null != PrintJobFinished)
                    {
                        PrintJobFinished(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Framework.Logger.LogInfo("Exception occured at PrintJob Finished continuous printing: {0}".FormatWith(ex.Message));

            }
        }

        #endregion

        #region Asynchronous Events

        /// <summary>
        /// PrintJobMonitor for Job Spooling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printJobMonitor_PrintJobSpooling_Async(object sender, PrintJobDataEventArgs e)
        {
            try
            {
                _printJobData.Add(e.Job.Id, e.Job);
                //Framework.Logger.LogDebug("Spooling event raised. Adding PrintJobData, Id : {0}, Document : {1}".
                //	FormatWith(e.Job.Id, e.Job.Document == null ? "Empty" : e.Job.Document));

                if (null != PrintJobSpooling && null != e)
                {
                    PrintJobSpooling(this, e);
                }
            }
            catch (Exception ex)
            {
                Framework.Logger.LogInfo("Exception occured at Print Job Spooling  : {0}".FormatWith(ex.Message));

            }
        }

        /// <summary>
        /// PrintJobMonitor for Job Printing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printJobMonitor_PrintJobPrinting_Async(object sender, PrintJobDataEventArgs e)
        {
            //Framework.Logger.LogDebug("Printing event raised. Id : {0}, Document : {1}".
            //	FormatWith(e.Job.Id, e.Job.Document == null ? "Empty" : e.Job.Document));
            try
            {
                if (null != PrintJobPrinting && null != e)
                {
                    PrintJobPrinting(this, e);
                }
            }
            catch (Exception ex)
            {
                Framework.Logger.LogInfo("Exception occured at PrintJob Printing : {0}".FormatWith(ex.Message));

            }
        }

        /// <summary>
        /// PrintJobMonitor for Job Finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printJobMonitor_PrintJobFinished_Async(object sender, PrintJobDataEventArgs e)
        {
            try
            {
                if (e.Job.Status.HasFlag(JobStatus.Deleted))
                {
                    _printJobData.Remove(e.Job.Id);

                    //Framework.Logger.LogDebug("Finished event raised. Id : {0}, Document : {1}".
                    //	FormatWith(e.Job.Id, e.Job.Document == null ? "Empty" : e.Job.Document));
                    Framework.Logger.LogDebug("Print file : {0} printed {1} page(s) successfully with status {2}.".
                        FormatWith(e.Job.Document == null ? "Empty" : e.Job.Document, e.Job.TotalPages, e.Job.Status));

                    // For Asynchronous print jobs, check if all jobs are completed and Stop Print Job Monitoring
                    StopJobMonitoring();

                    if (null != PrintJobFinished && null != e)
                    {
                        PrintJobFinished(this, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Framework.Logger.LogInfo("Exception occured at PrintJob Finished : {0}".FormatWith(ex.Message));

            }
        }

        #endregion

        #endregion
    }
}
