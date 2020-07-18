using System.Net;

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    /// <summary>
    /// Contains the JEDI printer specific functionality
    /// </summary>
    public class JediPrinter : Printer
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JediPrinter"/> class.
        /// </summary>
        /// <param name="ipV4Address"></param>
        public JediPrinter(IPAddress ipV4Address)
        {
            _wiredIPv4Address = ipV4Address;
            _defaultIPConfigMethod = IPConfigMethod.BOOTP;
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
            _timeRequiredForPowerCycle = 7;

            // JEDI printers requires maximum of 10 minutes to come up after power cycle.
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

            // JEDI printers requires maximum of 10 minutes to come up after power cycle.
            return base.PowerCycle();
        }

        /// <summary>
        /// JEDI printer doesn't have SNMP OID to perform cold reset, so performing power cycle.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>                
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ColdReset()
        {
            _timeRequiredForPowerCycle = 7;
            _timeRequiredForTCPReset = 2;
            Framework.Logger.LogInfo("VEP Products doesn't support Cold Reset thru SNMP, so performing TCP Rest and Power Cycle.");
            base.TCPReset();
            return base.PowerCycle();
        }

        /// <summary>
        /// JEDI printer doesn't have SNMP OID to perform cold reset, so performing power cycle.
        /// Returns true if the printer is in ready state after the cold reset, else returns false.
        /// </summary>        
        /// <param name="time">wait time for power cycle operation</param>
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ColdReset(int time)
        {
            _timeRequiredForPowerCycle = time;
            Framework.Logger.LogInfo("VEP Products doesn't support Cold Reset thru SNMP, so performing TCP Rest and Power Cycle.");
            base.TCPReset();
            return base.PowerCycle();
        }

        /// <summary>
        /// Resets the NVRAM of the printer
        /// </summary>        
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ResetNVRAM()
        {
            _timeRequiredForNVRAMReset = 7;
            // JEDI printers requires maximum of 10 minutes to come up after Resetting NVRAM.
            return base.ResetNVRAM();
        }

        #endregion
    }
}