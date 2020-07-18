using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.SoidBerg
{
    [ToolboxItem(false)]
    public partial class SoidBergConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private SoidBergActivityData _data;
        private readonly List<Oids> _oidCollection;

        public SoidBergConfigurationControl()
        {
            InitializeComponent();

            //Parse XML Data to List of Objects

            XDocument xdoc = XDocument.Parse(Properties.Resources.SNMPData);
            _oidCollection = xdoc.Element("SNMPData").Elements("Data").Select(d =>
           new Oids
           {
               Name = d.Element("Name").Value,
               Value = d.Element("Value").Value,
               IsInteger = Convert.ToBoolean((Convert.ToInt32(d.Element("IsInteger").Value))),
               CommandType = (SnmpCommandTypes)Enum.Parse(typeof(SnmpCommandTypes), d.Element("Command").Value),
               Comment = d.Element("Comments").Value
           }).ToList();

            //UI operations
            oidtype_comboBox.DataSource = Enum.GetNames(typeof(SnmpCommandTypes));
            snmp_comboBox.DataBindings.Add("Enabled", snmpCombo_radioButton, "Checked");
            snmp_textBox.DataBindings.Add("Enabled", snmpCustom_radioButton, "Checked");
            //snmpvalues_groupBox.DataBindings.Add("Enabled", snmpCustom_radioButton, "Checked");
            snmpcomments_textbox.DataBindings.Add("Enabled", snmpCombo_radioButton, "Checked");
            snmp_comboBox.DataSource = _oidCollection;
            snmp_comboBox.DisplayMember = "Name";
            snmp_comboBox.ValueMember = "Value";

            soid_fieldValidator.RequireAssetSelection(soid_assetSelectionControl);
            soid_fieldValidator.RequireSelection(snmp_comboBox, Snmpoid_label, ValidationCondition.IfEnabled);
            soid_fieldValidator.RequireValue(snmp_textBox, Custom_oid_label, ValidationCondition.IfEnabled);
            soid_fieldValidator.RequireValue(oidvalue_textbox, Setvalue_label, ValidationCondition.IfEnabled);

            snmp_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            soid_assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            snmpCombo_radioButton.Checked = true;
        }

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            _data = new SoidBergActivityData();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<SoidBergActivityData>();
            soid_assetSelectionControl.Initialize(configuration.Assets, Framework.Assets.AssetAttributes.None);
            oidtype_comboBox.SelectedIndex = (int)_data.SnmpCommand;
            snmp_textBox.Text = _data.SnmpOid;
            snmpCustom_radioButton.Checked = true;
            if (!string.IsNullOrEmpty(_data.SnmpSetValue))
            {
                oidvalue_textbox.Text = _data.SnmpSetValue;
            }
        }

        public PluginConfigurationData GetConfiguration()
        {
            // Code for SNMPGet or SNMP set selection
            _data.SnmpCommand = (SnmpCommandTypes)Enum.Parse(typeof(SnmpCommandTypes), oidtype_comboBox.Text);
            _data.SnmpOid = snmpCombo_radioButton.Checked ? (string)snmp_comboBox.SelectedValue : snmp_textBox.Text;
            _data.SnmpSetValue = _data.SnmpCommand == SnmpCommandTypes.Set ? oidvalue_textbox.Text : null;
            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = soid_assetSelectionControl.AssetSelectionData
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(soid_fieldValidator.ValidateAll());
        }

        private void snmp_comboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int index = snmp_comboBox.SelectedIndex;
            if (index >= 0)
            {
                snmpcomments_textbox.Text = _oidCollection.ElementAt(index).Comment;
                oidtype_comboBox.SelectedIndex = (int)_oidCollection.ElementAt(index).CommandType;
            }

        }

        private void oidtype_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = oidtype_comboBox.SelectedIndex;
            oidvalue_textbox.Enabled = index == 2;
        }
    }
}