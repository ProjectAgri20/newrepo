using System.Net;

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    public class SiriusPrinter : Printer
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusPrinter"/> class.
        /// </summary>
        /// <param name="ipV4Address"></param>
        public SiriusPrinter(IPAddress ipv4Address)
        {
            _wiredIPv4Address = ipv4Address;
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
            return base.PowerCycle();
        }

        /// <summary>
        /// Performs cold reset on the printer.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>                
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ColdReset()
        {
            _timeRequiredForColdReset = 3;
            return base.ColdReset();
        }

        /// <summary>
        /// Performs cold reset on the printer.
        /// Returns true if the printer is in ready state after the cold reset, else returns false.
        /// </summary>        
        /// <param name="time">wait time for power cycle operation</param>
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ColdReset(int time)
        {
            _timeRequiredForColdReset = time;
            return base.ColdReset();
        }

        /// <summary>
        /// Resets the NVRAM of the printer
        /// </summary>        
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ResetNVRAM()
        {
            _timeRequiredForNVRAMReset = 7;
            return base.ResetNVRAM();
        }

        #endregion
    }
}
