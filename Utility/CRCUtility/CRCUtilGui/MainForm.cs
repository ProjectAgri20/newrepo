using System;
using System.Net;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DeviceSettings;

namespace PaperlessUtilityGui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void onButton_Click(object sender, EventArgs e)
        {
            SetPaperlessMode(textBox_Address.Text, textBox_AdminPwd.Text, true);
        }

        private void offButton_Click(object sender, EventArgs e)
        {
            SetPaperlessMode(textBox_Address.Text, textBox_AdminPwd.Text, false);
        }

        public const int port9100 = 9100;

        public static void SetPaperlessMode(string address, string adminPassword, bool paperlessModeOn)
        {
            JobMediaMode mode = paperlessModeOn ? JobMediaMode.Paperless : JobMediaMode.Paper;

            IPAddress ipaddress;
            if (IPAddress.TryParse(address, out ipaddress))
            {
                using (IDevice device = DeviceFactory.Create(ipaddress, adminPassword))
                {
                    IDeviceSettingsManager manager = DeviceSettingsManagerFactory.Create(device);
                    manager.SetJobMediaMode(mode);
                }
                MessageBox.Show(string.Format("Paperless mode was turned {0}.", paperlessModeOn ? "on" : "off"));
            }
            else
            {
                MessageBox.Show("IP address was invalid.");
            }
        }
    }
}
