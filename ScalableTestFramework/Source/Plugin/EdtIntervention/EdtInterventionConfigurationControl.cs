using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.EdtIntervention
{
    /// <summary>
    /// Configuration Control Module for the Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class EdtInterventionConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private EdtInterventionActivityData _pluginData;

        public EdtInterventionConfigurationControl()
        {
            InitializeComponent();
            edtTestType_comboBox.DataSource = Enum.GetNames(typeof(EdtTestType));
            edtTestType_comboBox.SelectedValueChanged += (s, e) => ConfigurationChanged(s,e);
            AlertTextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            testMethod_comboBox.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);
        }

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Gets the Configuration Settings from the UI to the Activity Data
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _pluginData.AlertMessage = AlertTextBox.Text;
            _pluginData.TestType = (EdtTestType)Enum.Parse(typeof(EdtTestType), edtTestType_comboBox.SelectedItem.ToString());
            _pluginData.TestMethod = testMethod_comboBox.SelectedItem?.ToString();
            _pluginData.WakeMethod = wakeMethod_comboBox.SelectedItem?.ToString();
            return new PluginConfigurationData(_pluginData, "1.0");
        }

        /// <summary>
        /// Initialize the Control for new MetaData
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            _pluginData = new EdtInterventionActivityData();
        }

        /// <summary>
        /// Initializes the Control for the Existing MetaData
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _pluginData = configuration.GetMetadata<EdtInterventionActivityData>();
            if (!string.IsNullOrEmpty(_pluginData.AlertMessage))
            {
                AlertTextBox.Text = _pluginData.AlertMessage;
            }

            edtTestType_comboBox.SelectedIndex = edtTestType_comboBox.Items.IndexOf(_pluginData.TestType.ToString());
            if (!string.IsNullOrEmpty(_pluginData.TestMethod))
            {
                testMethod_comboBox.SelectedIndex = testMethod_comboBox.Items.IndexOf(_pluginData.TestMethod);
            }
            if (!string.IsNullOrEmpty(_pluginData.WakeMethod))
            {
                wakeMethod_comboBox.SelectedIndex = wakeMethod_comboBox.Items.IndexOf(_pluginData.WakeMethod);
            }
        }

        /// <summary>
        /// Validates the values Input by user in Configuration Control
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(FieldValidator.HasValue(AlertTextBox, "Alert message"));
        }

        private void edtTestType_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            wakeMethod_label.Visible = false;
            wakeMethod_comboBox.Visible = false;
            switch (edtTestType_comboBox.SelectedIndex)
            {
                case 0:
                    {
                        testMethod_label.Visible = false;
                        testMethod_comboBox.Visible = false;
                        testMethod_comboBox.DataSource = null;
                        testMethod_comboBox.Enabled = false;
                    }
                    break;

                case 1:
                    {
                        testMethod_comboBox.Visible = true;
                        testMethod_comboBox.Enabled = true;
                        testMethod_comboBox.DataSource = Enum.GetNames(typeof(BootMethod));
                        testMethod_label.Visible = true;
                        testMethod_label.Text = @"Boot Method";
                    }
                    break;

                case 2:
                    {
                        testMethod_comboBox.Visible = true;
                        testMethod_comboBox.Enabled = true;
                        testMethod_comboBox.DataSource = Enum.GetNames(typeof(SleepWakeMethod));
                        testMethod_label.Visible = true;
                        testMethod_label.Text = @"Methods";
                    }
                    break;

                case 3:
                {
                    testMethod_comboBox.Visible = true;
                    testMethod_comboBox.Enabled = true;
                    testMethod_comboBox.DataSource = null;
                    testMethod_comboBox.Items.Add("WJA");
                    testMethod_label.Visible = true;
                    testMethod_label.Text = @"Methods";

                }
                    break;
            }
        }

        private void testMethod_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (edtTestType_comboBox.SelectedIndex == 2)
            {
                switch (testMethod_comboBox.SelectedIndex)
                {
                    case 0:
                        wakeMethod_label.Visible = false;
                        wakeMethod_comboBox.Visible = false;
                        break;

                    case 1:
                    case 2:
                        wakeMethod_label.Visible = true;
                        wakeMethod_comboBox.Visible = true;
                        wakeMethod_comboBox.Enabled = true;
                        wakeMethod_comboBox.DataSource = Enum.GetNames(typeof(WakeMethod));
                        break;
                }
            }
        }
    }
}