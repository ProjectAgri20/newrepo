using HP.ScalableTest;
using HP.ScalableTest.PluginSupport.Connectivity.Discovery;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;


namespace WSPrintTemplate1
{
    public partial class WSDiscoverForm : Form
    {
        int _currentDeviceIndex = 0;
        Collection<DeviceInfo> _discoveredDevices = new Collection<DeviceInfo>();

        public WSDiscoverForm()
        {
            InitializeComponent();            
        }
      
        
        void DisplayDeviceInfo()
        {
            if (_discoveredDevices.Count > 0)
            {
                label27.Text = "{0} / {1}".FormatWith(_currentDeviceIndex + 1, _discoveredDevices.Count);

                DeviceInfo deviceInfo = _discoveredDevices[_currentDeviceIndex];

                StringBuilder output = new StringBuilder();

                label1.Text = "Device is found using the WS-Discovery";
                label17.Text = deviceInfo.HostName;
                label16.Text = deviceInfo.Port.ToString();
                // Condition to check whether ipv4 address is empty holds good when DHCP server provides only ipv6 address or ipv4 is disabled on the printer
                label15.Text = deviceInfo.IPv4Address == null ? "NULL" : deviceInfo.IPv4Address; 
                label14.Text = deviceInfo.LinkLocalAddress.ToString();
                label21.Text = deviceInfo.StateFullAddress != null ? deviceInfo.StateFullAddress.ToString() : "NULL";
                label23.Text = string.Join(", ", deviceInfo.StateLessAddress);
                label13.Text = deviceInfo.Uuid == null ? "NULL" : deviceInfo.Uuid;
                label12.Text = deviceInfo.Model;
                label11.Text = deviceInfo.Make;
                label10.Text = deviceInfo.Description;
                label25.Text = deviceInfo.MacAddress;
                label19.Text = string.Join(", ", deviceInfo.DeviceTypes.ToArray());

                richTextBox1.Text = deviceInfo.ProbeMatchString;
            }           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            --_currentDeviceIndex;
            if (_currentDeviceIndex < 0) _currentDeviceIndex = 0;
            DisplayDeviceInfo();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ++_currentDeviceIndex;
            if (_currentDeviceIndex >= _discoveredDevices.Count ) _currentDeviceIndex = _discoveredDevices.Count - 1;
            DisplayDeviceInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _discoveredDevices = PrinterDiscovery.Discover();
            _currentDeviceIndex = 0;
           
            DisplayDeviceInfo();

        }
    }
}
