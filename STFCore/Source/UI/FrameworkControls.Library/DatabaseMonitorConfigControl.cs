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
    /// Provides editing of serialized configuration data for <see cref="DatabaseMonitorConfig"/> type. 
    /// </summary>
    public partial class DatabaseMonitorConfigControl : UserControl, IMonitorConfigControl
    {
        private ErrorProvider _errorProvider = new ErrorProvider();
        private DatabaseMonitorConfig _monitorConfig = null;

        /// <summary>
        /// Provides editing of serialized configuration data for <see cref="DatabaseMonitorConfig"/> type. 
        /// </summary>
        /// <param name="monitorConfig"></param>
        public DatabaseMonitorConfigControl(MonitorConfig monitorConfig)
        {
            InitializeComponent();
            _monitorConfig = (DatabaseMonitorConfig)StfMonitorConfigFactory.Create(monitorConfig.MonitorType, monitorConfig.Configuration);

            // Populate the controls
            textBox_DbHostName.Text = _monitorConfig.MonitorLocation;
            textBox_DbInstanceName.Text = _monitorConfig.DatabaseInstanceName;
            textBox_Port.Text = _monitorConfig.ConnectionPort > 0 ? _monitorConfig.ConnectionPort.ToString() : string.Empty;

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

        private void textBox_DbHostName_Validating(object sender, CancelEventArgs e)
        {
            if (HasValue(textBox_DbHostName, label_DbHostName, e))
            {
                _monitorConfig.MonitorLocation = textBox_DbHostName.Text;
            }
        }

        private void textBox_DbInstanceName_Validating(object sender, CancelEventArgs e)
        {
            if (HasValue(textBox_DbInstanceName, label_DbInstanceName, e))
            {
                _monitorConfig.DatabaseInstanceName = textBox_DbInstanceName.Text;
            }
        }

        private bool HasValue(TextBox textBox, Label label, CancelEventArgs e)
        {
            ValidationResult result = FieldValidator.HasValue(textBox, label);

            _errorProvider.SetError(label, result.Message);
            e.Cancel = !result.Succeeded;
            return result.Succeeded;
        }
    }
}
