using System.Net;

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    /// <summary>
    /// Contains the JEDI printer specific functionality
    /// </summary>
    public class MarvellPrinter : Printer
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MarvellPrinter"/> class.
        /// </summary>
        /// <param name="ipV4Address"></param>
        public MarvellPrinter(IPAddress ipV4Address)
        {
            _wiredIPv4Address = ipV4Address;
            _defaultIPConfigMethod = IPConfigMethod.DHCP;
        }

        #endregion

        #region Public Overridden methods

        /// <summary>
        /// Power cycles the printer asynchronously without checking the printer status.
        /// <remarks>The method starts power cycling the printer.
        /// It is the users responsibility to ensure that the printer is come back to ready state after the power cycle</remarks>
        /// </summary>
        public override void PowerCycleAsync()
        {
            base.PowerCycleAsync();
        }

        /// <summary>
        /// Power Cycles the printer with Wired IPv4 address.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>        
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool PowerCycle()
        {
            _timeRequiredForPowerCycle = 3;
            // Marvell printers requires maximum of 5 minutes to come up after power cycle.
            return base.PowerCycle();
        }

        /// <summary>
        /// Power Cycles the printer with Wired IPv4 address.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>    
        /// <param name="time">wait time for power cycle operation</param>
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool PowerCycle(int time)
        {
            _timeRequiredForPowerCycle = time;
            // Marvell printers requires maximum of 5 minutes to come up after power cycle.
            return base.PowerCycle();
        }

        /// <summary>
        /// Cold reset the printer with wired IPv4 address.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>        
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ColdReset()
        {
            _timeRequiredForColdReset = 5;
            // Marvell printers requires maximum of 5 minutes to come up after cold reset.
            return base.ColdReset();
        }

        /// <summary>
        /// Cold reset the printer with wired IPv4 address.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary> 
        /// <param name="time">wait time for cold reset operation</param>
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ColdReset(int time)
        {
            _timeRequiredForColdReset = time;
            // Marvell printers requires maximum of 5 minutes to come up after cold reset.
            return base.ColdReset();
        }

        /// <summary>
        /// Resets the NVRAM of the printer
        /// </summary>        
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ResetNVRAM()
        {
            _timeRequiredForNVRAMReset = 5;
            // Marvell printers requires maximum of 5 minutes to come up after Resetting NVRAM.
            return base.ResetNVRAM();
        }

        /// <summary>
        /// Firmware Version of the Printer
        /// Note: SNMP OID used for other printer family is not available for Marvell printer
        /// TODO: Find an alternative to get Firmware version
        /// </summary>
        public override string FirmwareVersion
        {
            get
            {
                return "Not Available";
            }
        }

        #endregion
    }
}