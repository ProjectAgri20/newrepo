using System;
namespace HP.ScalableTest.Plugin.EWSFirmwarePerformance
{
    /// <summary>
    /// Well known and reused constants for LL CTF.
    /// </summary>
    public static class CommonConstants
    {
        /// <summary>
        /// The default and typical username used to login to a EMU telnet server.
        /// </summary>
        public const String TRIPOLI_USERNAME = "tripoli";

        /// <summary>
        /// The EMU telnet prompt seen and returned when telneted to an EMU when logged on as user tripoli.
        /// </summary>
        public const String DEFAULT_EMU_TRIPOLI_TELNET_PROMPT = TRIPOLI_USERNAME + "@";

        /// <summary>
        /// The telnet prompt seen and returned by EMU telnet service, that requires a login.
        /// </summary>
        public const String DEFAULT_EMU_LOGIN_PROMPT = "login:";

        /// <summary>
        /// The well known FTP server port.
        /// </summary>
        public const Int32 WELL_KNOWN_FTP_PORT = 21;
        
        /// <summary>
        /// The well known JDI IPv4 only FTP server port.
        /// </summary>
        public const Int32 WELL_KNOWN_JDI_IPv4_ONLY_FTP_PORT = 221;
        
        /// <summary>
        /// The well known Telnet server port.
        /// </summary>
        public const Int32 WELL_KNOWN_TELNET_PORT = 23;

        /// <summary>
        /// The well known JDI IPv4 only Telnet server port.
        /// </summary>
        public const Int32 WELL_KNOWN_JDI_IPv4_ONLY_TELNET_PORT = 223;
        
        /// <summary>
        /// The telnet server port that reads/writes the formatter serial port.
        /// </summary>
        public const Int32 EMU_TO_FRMTR_SERIAL_PORT_TELNET_PORT = 8101;

        /// <summary>
        /// Used over EMU telnet to formatter serial port to power off formatter.
        /// </summary>
        public const String EMU_TO_FRMTR_POWER_OFF = "ee formatter power off";

        /// <summary>
        /// Used over EMU telnet to formatter serial port to power on formatter.
        /// </summary>
        public const String EMU_TO_FRMTR_POWER_ON = "ee formatter power on";

        /// <summary>
        /// Used over EMU telnet to formatter serial port to power reset formatter for pentane products.
        /// </summary>
        public const String EMU_TO_FRMTR_POWER_RESET = "ee init";

        /// <summary>
        /// The telnet prompt seen and returned by CE telnet service.
        /// </summary>
        public const String EMU_TO_FRMTR_POWER_OFF_PENTANE = "ee IPower FormatterPowerOff";
        
        /// <summary>
        /// Used over EMU telnet to formatter serial port to power reset formatter.
        /// </summary>
        public const String EMU_TO_FRMTR_POWER_ON_PENTANE = "ee IPower FormatterPowerOn";
        
        /// <summary>
        /// Used over EMU telnet to formatter serial port to power reset formatter.
        /// </summary>
        public const String EMU_TO_FRMTR_POWER_RESET_PENTANE = "ee IPower FormatterPowerReset";
        
        /// <summary>
        /// Used over EMU telnet to formatter serial port to power reset formatter.
        /// </summary>
        public const String DEFAULT_CE_TELNET_PROMPT = "> ";

        /// <summary>
        /// The telnet prompt used to detect the CE Shell prompt.
        /// </summary>
        public const String DEFAULT_CE_SHELL_PROMPT = "Windows CE>";

        /// <summary>
        /// The pattern to look for in telnet client that indicates CEOS has completed boot.
        /// </summary>
        public const String CEOS_BOOT_COMPLETE_PATTERN = "BOOT CHECKPOINT: BOOT COMPLETE";

        /// <summary>
        /// The EMU telnet to formatter serial port EFI prompt.
        /// </summary>
        public const String EFI_PROMPT_PATTERN = @"(Shell|fs[0-9]:\\.*)>";

        /// <summary>
        /// Number of milliseconds in one minute
        /// </summary>
        public const int MILLISECONDS_PER_MINUTE = 60 * 1000;


        #region AsciiCodes

        /// <summary>
        /// Null character
        /// </summary>
        public static readonly Byte bNUL = 0x00;

        /// <summary>
        /// Escape character
        /// </summary>
        public static readonly Byte bESC = 0x1b;

        /// <summary>
        /// End of Text - ctrl-c character
        /// </summary>
        public static readonly Byte bETX = 0x03;

        /// <summary>
        /// Acknowledge - ctrl-f character
        /// </summary>
        public static readonly Byte bACK = 0x06;

        /// <summary>
        /// Carriage Return character
        /// </summary>
        public static readonly Byte bCR = 0x0D;

        /// <summary>
        /// New Line character
        /// </summary>
        public static readonly Byte bLF = 0x0A;

        /// <summary>
        /// Back Space character
        /// </summary>
        public static readonly Byte bBS = 0x08;

        /// <summary>
        /// Space bar character
        /// </summary>
        public static readonly Byte bSP = 0x20;

        /// <summary>
        /// F2 character
        /// </summary>
        public static readonly Byte bF2 = 0x3c;

        /// <summary>
        ///F10, Save character
        ///</summary>
        public static readonly Byte bF10 = 0x5b;

        #endregion
    }
}