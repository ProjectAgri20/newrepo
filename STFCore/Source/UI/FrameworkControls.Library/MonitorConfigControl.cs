using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Provides editing of serialized configuration data for <see cref="MonitorConfig"/> type. 
    /// </summary>
    public partial class MonitorConfigControl : UserControl, IMonitorConfigControl
    {
        private ErrorProvider _errorProvider = new ErrorProvider();
        private StfMonitorConfig _monitorConfig = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorConfigControl"/>.
        /// </summary>
        public MonitorConfigControl(MonitorConfig monitorConfig)
        {
            InitializeComponent();
            _monitorConfig = StfMonitorConfigFactory.Create(monitorConfig.MonitorType, monitorConfig.Configuration);

            // Populate the controls
            textBox_MonitorLocation.Text = _monitorConfig.MonitorLocation;

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

        private void destination_TextBox_Validating(object sender, CancelEventArgs e)
        {
            if (HasValue(textBox_MonitorLocation, label_Destination, e))
            {
                _monitorConfig.MonitorLocation = textBox_MonitorLocation.Text;
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
