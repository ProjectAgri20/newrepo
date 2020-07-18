using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Provides editing of serialized configuration data for <see cref="DirectoryMonitorConfig"/> type. 
    /// </summary>
    public partial class DirectoryMonitorConfigControl : UserControl, IMonitorConfigControl
    {
        private ErrorProvider _errorProvider = new ErrorProvider();
        private DirectoryMonitorConfig _monitorConfig = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryMonitorConfigControl"/>.
        /// </summary>
        public DirectoryMonitorConfigControl(MonitorConfig monitorConfig)
        {
            InitializeComponent();
            _monitorConfig = (DirectoryMonitorConfig)StfMonitorConfigFactory.Create(monitorConfig.MonitorType, monitorConfig.Configuration);

            // Populate the controls
            textBox_Destination.Text = _monitorConfig.MonitorLocation;
            textBox_DataLogHost.Text = _monitorConfig.LogServiceHostName;

            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        /// <summary>
        /// Gets the serialized string of the <see cref="StfMonitorConfig" /> object.
        /// </summary>
        public string Configuration
        {
            get
            {
                return LegacySerializer.SerializeXml(_monitorConfig).ToString();
            }
        }

        private void TextBox_Destination_Validating(object sender, CancelEventArgs e)
        {
            if (HasValue(textBox_Destination, label_Destination, e))
            {
                _monitorConfig.MonitorLocation = textBox_Destination.Text;
            }
        }

        private void TextBox_DataLogHost_Validating(object sender, CancelEventArgs e)
        {
            if (HasValue(textBox_DataLogHost, label_DataLogHost, e))
            {
                _monitorConfig.LogServiceHostName = textBox_DataLogHost.Text;
            }
        }

        //private bool IsEnterpriseEnabled()
        //{
        //    return GlobalSettings.IsDistributedSystem || GlobalSettings.Items[Setting.EnterpriseEnabled].Equals("true", StringComparison.InvariantCultureIgnoreCase);
        //}

        private bool HasValue(TextBox textBox, Label label, CancelEventArgs e)
        {
            ValidationResult result = FieldValidator.HasValue(textBox, label);

            _errorProvider.SetError(label, result.Message);
            e.Cancel = !result.Succeeded;
            return result.Succeeded;
        }

    }
}
