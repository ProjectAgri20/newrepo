using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Provides editing of serialized configuration data for <see cref="MonitorConfig"/> item. 
    /// </summary>
    public partial class MonitorConfigDetailForm : Form
    {
        private MonitorConfig _monitorConfig = null;
        private ErrorProvider _errorProvider = new ErrorProvider();
        private IMonitorConfigControl _monitorConfigControl = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorConfigDetailForm"/> form.
        /// </summary>
        /// <param name="monitorConfig">The location.</param>
        public MonitorConfigDetailForm(MonitorConfig monitorConfig)
        {
            InitializeComponent();
            serverComboBox.Initialize(new List<string>() { "FileServer", "Solution" });
            _monitorConfig = monitorConfig;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            monitorType_ComboBox.DataSource = GetMonitorTypes();
            monitorType_ComboBox.SelectedIndex = -1;

            //Wire up events
            serverComboBox.Validating += serverComboBox_Validating;
            monitorType_ComboBox.Validating += monitorType_ComboBox_Validating;
            monitorType_ComboBox.SelectedIndexChanged += monitorType_ComboBox_SelectedIndexChanged;

            // Initialize controls with data from MonitorConfig
            ServerInfo server = ConfigurationServices.AssetInventory.GetServers().FirstOrDefault(n => n.HostName == _monitorConfig.ServerHostName);
            if (server != null)
            {
                serverComboBox.SelectedServer = server;
            }

            // If this is a "create new" operation, the monitor type will be blank
            if (string.IsNullOrEmpty(monitorConfig.MonitorType) == false)
            {
                monitorType_ComboBox.SelectedValue = _monitorConfig.MonitorType;
            }

            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
        }

        private List<KeyValuePair<string, string>> GetMonitorTypes()
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (STFMonitorType item in EnumUtil.GetValues<STFMonitorType>())
            {
                result.Add(new KeyValuePair<string, string>(item.ToString(), EnumUtil.GetDescription(item)));
            }

            return result;
        }

        private void CreateMonitorConfigControl(MonitorConfig monitorConfig)
        {
            switch (EnumUtil.Parse<STFMonitorType>(monitorConfig.MonitorType))
            {
                case STFMonitorType.DigitalSendNotification:
                case STFMonitorType.OutputDirectory:
                case STFMonitorType.OutputEmail:
                case STFMonitorType.LANFax:
                case STFMonitorType.SharePoint:
                    _monitorConfigControl = new OutputMonitorConfigControl(monitorConfig);
                    break;
                case STFMonitorType.Directory:
                    _monitorConfigControl = new DirectoryMonitorConfigControl(monitorConfig);
                    break;
                case STFMonitorType.DSSServer:
                case STFMonitorType.Hpcr:
                case STFMonitorType.EPrint:
                    _monitorConfigControl = new DatabaseMonitorConfigControl(monitorConfig);
                    break;
                default:
                    _monitorConfigControl = new MonitorConfigControl(monitorConfig);
                    break;
            }

            Control control = (Control)_monitorConfigControl;
            Point controlLocation = new Point(12, 90);
            int formHeight = control.Height + 160;

            control.Location = controlLocation;
            control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            this.Controls.Add(control);
            this.Size = new Size(this.Width, formHeight);
        }

        private void RefreshMonitorConfigControl()
        {
            if (_monitorConfigControl != null)
            {
                RemoveMonitorConfigControl((Control)_monitorConfigControl);
                _monitorConfigControl = null;
            }

            CreateMonitorConfigControl(_monitorConfig);
        }

        private void RemoveMonitorConfigControl(Control control)
        {
            this.Controls.Remove(control);
            control.Dispose();
            control = null;
        }

        private bool ValidateMonitorType()
        {
            if (monitorType_ComboBox.SelectedItem == null)
            {
                _errorProvider.SetError(label_MonitorType, "Monitor Type is required.");
                return false;
            }
            else
            {
                _errorProvider.SetError(label_MonitorType, string.Empty);
                _monitorConfig.MonitorType = (string)monitorType_ComboBox.SelectedValue;
                return true;
            }
        }

        private void monitorType_ComboBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !ValidateMonitorType();
        }

        private void serverComboBox_Validating(object sender, CancelEventArgs e)
        {
            if (!serverComboBox.HasSelection)
            {
                _errorProvider.SetError(label_Server, "You must select a server.");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(label_Server, string.Empty);
                e.Cancel = false;
                _monitorConfig.ServerHostName = serverComboBox.SelectedServer.HostName;
            }
        }

        private void monitorType_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValidateMonitorType())
            {
                RefreshMonitorConfigControl();
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                _monitorConfig.Configuration = _monitorConfigControl.Configuration;
                DialogResult = DialogResult.OK;
            }
        }

    }
}
