using System;
using System.Net;
using System.Threading;

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    /// <summary>
    /// Contains the JEDI printer specific functionality
    /// </summary>
    public class PhoenixPrinter : Printer
    {
        #region Constants

        private const string SSH_USERNAME = "root";
        private const string SSH_PASSWORD = "socorro!";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JediPrinter"/> class.
        /// </summary>
        /// <param name="ipV4Address"></param>
        public PhoenixPrinter(IPAddress ipV4Address)
        {
            _wiredIPv4Address = ipV4Address;
            _defaultIPConfigMethod = IPConfigMethod.BOOTP;
            _timeRequiredForColdReset = COLD_RESET;
            _timeRequiredForPowerCycle = POWER_CYCLE;
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
            try
            {
                Framework.Logger.LogInfo("Power cyle process using SSH initiated.");
                SSHProtocol sshProtocol = new SSHProtocol(SSH_USERNAME, SSH_PASSWORD, _wiredIPv4Address);
                sshProtocol.Connect();
                sshProtocol.Send("{0}@{1}\n".FormatWith(SSH_USERNAME, _wiredIPv4Address));
                sshProtocol.Send("reboot\n");
                Framework.Logger.LogInfo("Power cycle process using SSH completed.");
            }
            catch (Exception exception)
            {
                Framework.Logger.LogInfo("Power cycle process using SSH failed.");
                Framework.Logger.LogDebug("Exception details: {0}".FormatWith(exception.Message));
                throw exception;
            }

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

            // Phoenix printers requires maximum of 10 minutes to come up after power cycle.
            return PowerCycle();
        }

        /// <summary>
        /// Restore Factory settings asynchronisly on the printer with wired IPv4 address.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override void ColdResetAsync()
        {
            try
            {
                Framework.Logger.LogInfo("Restore Factory Settings process using SSH initiated.");
                SSHProtocol sshProtocol = new SSHProtocol(SSH_USERNAME, SSH_PASSWORD, _wiredIPv4Address);
                sshProtocol.Connect();
                sshProtocol.Send("{0}@{1}\n".FormatWith(SSH_USERNAME, _wiredIPv4Address));
                sshProtocol.Send("telnet localhost 9169\n");
                sshProtocol.Send("set IO-RESET 1\n");
                sshProtocol.Send("set IIO-2-NIC-RESET 1\n");
                sshProtocol.Send("exit\n");
                sshProtocol.Send("reboot\n");

                Framework.Logger.LogInfo("Restore Factory Settings process using SSH completed.");

            }
            catch (Exception exception)
            {
                Framework.Logger.LogInfo("Restore Factory Settings process using SSH failed.");
                Framework.Logger.LogDebug("Exception details: {0}".FormatWith(exception.Message));
                throw exception;
            }
        }

        /// <summary>
        /// Restore Factory settings on the printer with wired IPv4 address.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ColdReset()
        {
            try
            {
                Framework.Logger.LogInfo("Restore Factory Settings process using SSH initiated.");
                SSHProtocol sshProtocol = new SSHProtocol(SSH_USERNAME, SSH_PASSWORD, _wiredIPv4Address);
                sshProtocol.Connect();
                sshProtocol.Send("{0}@{1}\n".FormatWith(SSH_USERNAME, _wiredIPv4Address));
                sshProtocol.Send("telnet localhost 9169\n");
                sshProtocol.Send("set IO-RESET 1\n");
                sshProtocol.Send("set IIO-2-NIC-RESET 1\n");
                sshProtocol.Send("exit\n");
                sshProtocol.Send("reboot\n");

                Thread.Sleep(TimeSpan.FromMinutes(_timeRequiredForColdReset));
                Framework.Logger.LogInfo("Restore Factory Settings process using SSH completed.");

                if (IsPrinterReady(_wiredIPv4Address, COLD_RESET))
                {
                    Framework.Logger.LogInfo("Printer has come to ready state after Restore Factory Settings.");
                    return true;
                }
                else
                {
                    Framework.Logger.LogInfo("Printer failed to acquire ready state after Restore Factory Settings.");
                    return false;
                }
            }
            catch (Exception exception)
            {
                Framework.Logger.LogInfo("Restore Factory Settings process using SSH failed.");
                Framework.Logger.LogDebug("Exception details: {0}".FormatWith(exception.Message));
                throw exception;
            }
        }

        /// <summary>
        /// Power Cycles the printer with wired IPv4 address.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool PowerCycle()
        {
            try
            {
                Framework.Logger.LogInfo("Power cycle process using SSH initiated.");
                SSHProtocol sshProtocol = new SSHProtocol(SSH_USERNAME, SSH_PASSWORD, _wiredIPv4Address);
                sshProtocol.Connect();
                sshProtocol.Send("{0}@{1}\n".FormatWith(SSH_USERNAME, _wiredIPv4Address));
                sshProtocol.Send("reboot\n");

                Thread.Sleep(TimeSpan.FromMinutes(_timeRequiredForPowerCycle));
                Framework.Logger.LogInfo("Power cycle process using SSH completed.");

                if (IsPrinterReady(_wiredIPv4Address, POWER_CYCLE))
                {
                    Framework.Logger.LogInfo("Printer has come to ready state after PowerCycle");
                    return true;
                }
                else
                {
                    Framework.Logger.LogInfo("Printer failed to acquire ready state after PowerCycle");
                    return false;
                }
            }
            catch (Exception exception)
            {
                Framework.Logger.LogInfo("Power cycle process using SSH failed.");
                Framework.Logger.LogDebug("Exception details: {0}".FormatWith(exception.Message));
                throw exception;
            }
        }

        /// <summary>
        /// Restore Factory settings on the printer with wired IPv4 address.
        /// Returns true if the printer is in ready state after the power cycle, else returns false.
        /// </summary>
        /// <param name="time">wait time for Restore Factory Settings.</param>
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ColdReset(int time)
        {
            _timeRequiredForColdReset = time;
            return ColdReset();
        }

        /// <summary>
        /// Resets the NVRAM of the printer
        /// </summary>        
        /// <returns>Returns true if the printer is ready, else return false.</returns>
        public override bool ResetNVRAM()
        {
            _timeRequiredForNVRAMReset = 7;
            // Phoenix printers requires maximum of 10 minutes to come up after Resetting NVRAM.
            return base.ResetNVRAM();
        }

        /// <summary>
        /// Keep Awake the printer. Disables the Sleep Mode.
        /// </summary>
        public override void KeepAwake()
        {
            try
            {
                Framework.Logger.LogInfo("Sleep mode disabled using SSH.");
                SSHProtocol sshProtocol = new SSHProtocol(SSH_USERNAME, SSH_PASSWORD, _wiredIPv4Address);
                sshProtocol.Connect();
                sshProtocol.Send("{0}@{1}\n".FormatWith(SSH_USERNAME, _wiredIPv4Address));
                sshProtocol.Send("telnet localhost 9169\n");
                sshProtocol.Send("set DEV-ENERGY-STAR-ENABLED false\n");
                sshProtocol.Send("exit\n");
                Framework.Logger.LogInfo("Sleep mode disabled successfully using SSH.");
            }
            catch (Exception exception)
            {
                Framework.Logger.LogInfo("Failed to disable Sleep mode using SSH.");
                Framework.Logger.LogDebug("Exception details: {0}".FormatWith(exception.Message));
                throw exception;
            }
        }
    }
    #endregion
}
