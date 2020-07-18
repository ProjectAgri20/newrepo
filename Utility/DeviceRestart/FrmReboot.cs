using System;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace HP.STF.DeviceRestart
{
    /// <summary>
    /// Current Power save state, valid values below
 
    // PowerStateAwake = 1,
    // PowerStatePowerSave = 2, ( shallow sleep aka Sleep-1)
    // PowerStateSleep = 3, (Sleep-2, depends upon WakeEvent setting)
    // PowerStateOff = 4, 
    // PowerStateReboot = 9,
    // PowerStateAutoOff = 10
    // PowerState = 11 (Sleep-3, 1W )

    /// </summary>
    public partial class FrmReboot : Form
    {
        private static string ErrorMessage { get; set; }
        public int RebootStyle { get; set; }

        readonly string _printerNameOid = "1.3.6.1.2.1.25.3.2.1.3.1";
        readonly string _rebootOid = "1.3.6.1.2.1.43.5.1.1.3.1";
        readonly string _deviceStatus = "1.3.6.1.4.1.11.2.3.9.1.1.3.0";
        readonly string _wakeDevice = "1.3.6.1.4.1.11.2.3.9.4.2.1.1.1.65.0";
       
        private string PrinterName { get; set; }

        public FrmReboot()
        {
            InitializeComponent();

            PrinterName = string.Empty;
            RlblRebootMessage.Text = "Waiting for power cycle command.";            
        }
        /// <summary>
        /// Property (bool) for error
        /// </summary>
        private static bool IsError
        {
            get
            {
                return (string.IsNullOrEmpty(ErrorMessage) == false) ? true : false;
            }
        }
        private void RbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RrbRebootClicked(object sender, EventArgs e)
        {
            RadRadioButton rb = sender as RadRadioButton;
            RebootStyle = int.Parse(rb.Tag.ToString());
        }

        private void CheckPrinterIPAddress(DatDeviceReboot ddr)
        {
            if (!ddr.ValidIPAddress(RtxtIpAddress.Text))
            {
                ErrorMessage = ddr.GetLastError + ", " + RtxtIpAddress.Text;
                MessageBox.Show(ErrorMessage, "Printer IP Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetDeviceName(DatDeviceReboot ddr)
        {
            int iTime = 0;
            PrinterName = ddr.GetSNMPData(out iTime, _printerNameOid);
        }

        private void SetLabel()
        {
            string msg = "Rebooting " + PrinterName;
            switch (RebootStyle)
            {
                case 4: msg += " with normal power cycle.";
                    break;
                case 5: msg += " and resetting to NTRAM";
                    break;
                case 6: msg += " with NVRAM and factory restore.";
                    break;
            }

            RlblFieldName.Text = PrinterName;
            RlblRebootMessage.Text = msg;

            RlblFieldName.Update();
            RlblRebootMessage.Update();
        }
        /// <summary>
        /// Handles the KeyDown event of the RtxtIpAddress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void RtxtIpAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode.Equals(Keys.Return))
            {
                RbtnReboot_Click(sender, e);
            }
        }
        private void RbtnReboot_Click(object sender, EventArgs e)
        {
            ErrorMessage = string.Empty;
            DatDeviceReboot ddr = new DatDeviceReboot(RtxtIpAddress.Text, string.Empty);

            CheckPrinterIPAddress(ddr);
            int rebootTime = 0;
            int exeTime;
            if (!IsError)
            {
                GetDeviceName(ddr);
                string status = ddr.GetSNMPData(out exeTime, _deviceStatus);

                SetLabel();

                if (status.ToLower().Equals("sleep mode on"))
                {
                    WakeDevice(ddr);
                }

                RebootDevice(ddr, ref exeTime, ref rebootTime);
                
                if (!ddr.IsError)
                {
                    RlblRebootMessage.Text += " - Rebooted in " + rebootTime + " seconds.";
                }
                else
                {
                    MessageBox.Show(ddr.GetLastError, "Printer IP Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else { ErrorMessage = string.Empty; }
        }
 
        private void RebootDevice(DatDeviceReboot ddr, ref int exeTime, ref int rebootTime)
        {
            ddr.RebootDevice(_rebootOid, out exeTime);
            if (exeTime > 0)
            {
                RlblRebootMessage.Text = "Starting the power cycle process";
                RlblRebootMessage.Update();
                DateTime startDt = DateTime.Now;
                Thread.Sleep(5000);
                rebootTime = CheckPrinterStatus(ddr, startDt);
            }
        }
 
        private void WakeDevice(DatDeviceReboot ddr)
        {
            int exeTime = 0;
            string status = ddr.SetDeviceValue(_wakeDevice, 1, out exeTime);
            if (!ddr.IsError)
            {
                status = ddr.GetSNMPData(out exeTime, _deviceStatus);

                while (status.ToLower().Contains("sleep") || status.ToLower().Contains("initializing"))
                {
                    RlblRebootMessage.Text = status + "...Waking device " + RtxtIpAddress.Text;
                    RlblRebootMessage.Update();

                    exeTime = 0;
                    Thread.Sleep(5000);
                    status = ddr.GetSNMPData(out exeTime, _deviceStatus);
                }
            }
            else
            {
                RlblRebootMessage.Text = "Issue waking device. Reboot may still work. " + ddr.GetLastError;
            }
        }

        private int CheckPrinterStatus(DatDeviceReboot ddr, DateTime startDt)
        {
            int exe;

            int rebootTime = 0;
            string state = string.Empty;

            DateTime dt = DateTime.Now.AddSeconds(240);
            while (state != "Ready" && (dt > DateTime.Now))
            {
                state = ddr.GetSNMPData(out exe, _deviceStatus);
                TimeSpan span = DateTime.Now.Subtract(startDt);

                rebootTime = (span.Minutes * 60) + span.Seconds;

                if (state.Equals("04 00"))
                {
                    state = "Still in the process of rebooting: " + rebootTime + " seconds passed.";
                }
                else if (string.IsNullOrWhiteSpace(state))
                {
                    state = "Still in the process of rebooting:" + rebootTime + " seconds passed.";
                }
                else if (state.ToLower().Contains("error:") && state.ToLower().Contains("did not respond to snmp"))
                {
                    state = "Beginning of power cycle process. Printer is unable to respond.";
                }
                RlblRebootMessage.Text = state;
                RlblRebootMessage.Update();

                this.Refresh();
            }
            return rebootTime;
        }

    }
}
