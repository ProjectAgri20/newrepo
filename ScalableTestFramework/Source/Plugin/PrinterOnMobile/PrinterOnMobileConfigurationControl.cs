using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.PrinterOnMobile
{
    [ToolboxItem(false)]
    public partial class PrinterOnMobileConfigurationControl : UserControl, IPluginConfigurationControl
    {        
        private PrinterOnMobileActivityData _data;        
        private string _textAllPages = "All Pages";

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterOnMobileConfigurationControl" /> class.
        /// </summary>
        public PrinterOnMobileConfigurationControl()
        {
            InitializeComponent();            

            fieldValidator.RequireAssetSelection(assetSelectionControl);
            fieldValidator.RequireValue(printerId_textBox, printerId_label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(filePath_textBox, filePath_label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(name_textBox, name_label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(email_textBox, email_label, ValidationCondition.IfEnabled);
            
            orientation_comboBox.DataSource = EnumUtil.GetDescriptions<Option_Orientation>().ToList();
            duplex_comboBox.DataSource = EnumUtil.GetDescriptions<Option_Duplex>().ToList();
            color_comboBox.DataSource = EnumUtil.GetDescriptions<Option_Color>().ToList();
            paperSize_comboBox.DataSource = EnumUtil.GetDescriptions<Option_PaperSize>().ToList();            

            printerId_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            filePath_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            name_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            email_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);

            copies_numericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);

            allPages_radioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            pages_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            pages_radioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            
            orientation_comboBox.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);
            duplex_comboBox.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);
            color_comboBox.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);
            paperSize_comboBox.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);            
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            assetSelectionControl.Initialize(AssetAttributes.Mobile);
            _data = new PrinterOnMobileActivityData();
            
            ConfigureControls(_data);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {            
            assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.Mobile);
            _data = configuration.GetMetadata<PrinterOnMobileActivityData>();
            ConfigureControls(_data);
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data = new PrinterOnMobileActivityData()
            {
                PrinterId = printerId_textBox.Text,
                FilePath = filePath_textBox.Text,
                Name = name_textBox.Text,
                Email = email_textBox.Text
            };

            if (copies_numericUpDown.Value == 1)
            {
                _data.Options.Copies = -1;
            }
            else
            {
                _data.Options.Copies = (int)copies_numericUpDown.Value;
            }

            if (allPages_radioButton.Checked)
            {
                _data.Options.Page = null;
            }
            else
            {
                _data.Options.Page = pages_textBox.Text;
            }

            if (EnumUtil.GetDescription(Option_Orientation.Portrait).Equals(orientation_comboBox.Text))
            {
                _data.Options.Orientation = null;
            }
            else
            {
                _data.Options.Orientation = EnumUtil.GetByDescription<Option_Orientation>(orientation_comboBox.Text);
            }

            if (EnumUtil.GetDescription(Option_Duplex.None).Equals(duplex_comboBox.Text))
            {
                _data.Options.Duplex = null;
            }
            else
            {
                _data.Options.Duplex = EnumUtil.GetByDescription<Option_Duplex>(duplex_comboBox.Text);
            }

            if (EnumUtil.GetDescription(Option_Color.Color).Equals(color_comboBox.Text))
            {
                _data.Options.Color = null;
            }
            else
            {
                _data.Options.Color = EnumUtil.GetByDescription<Option_Color>(color_comboBox.Text);
            }

            if (EnumUtil.GetDescription(Option_PaperSize.A4).Equals(paperSize_comboBox.Text))
            {
                _data.Options.PaperSize = null;
            }
            else
            {
                _data.Options.PaperSize = EnumUtil.GetByDescription<Option_PaperSize>(paperSize_comboBox.Text);
            }            

            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
            };
        }

        private void ConfigureControls(PrinterOnMobileActivityData data)
        {
            printerId_textBox.Text = data.PrinterId;
            filePath_textBox.Text = data.FilePath;

            if (data.Options.Copies.Equals(-1))
            {
                copies_numericUpDown.Value = 1;
            }
            
            if (String.IsNullOrEmpty(data.Options.Page))
            {
                allPages_radioButton.Checked = true;
                pages_radioButton.Checked = false;
                pages_textBox.Enabled = false;
            }
            else
            {
                allPages_radioButton.Checked = false;
                pages_radioButton.Checked = true;
                pages_textBox.Enabled = true;
                pages_textBox.Text = data.Options.Page;
            }

            if(data.Options.Orientation == null)
            {
                orientation_comboBox.Text = EnumUtil.GetDescription(Option_Orientation.Portrait);
            }
            else
            {
                orientation_comboBox.Text = data.Options.Orientation.GetDescription();
            }

            if (data.Options.Duplex == null)
            {
                duplex_comboBox.Text = EnumUtil.GetDescription(Option_Duplex.None);
            }
            else
            {
                duplex_comboBox.Text = data.Options.Duplex.GetDescription();
            }

            if(data.Options.Color == null)
            {
                color_comboBox.Text = EnumUtil.GetDescription(Option_Color.Color);
            }
            else
            {
                color_comboBox.Text = data.Options.Color.GetDescription();
            }
            
            if(data.Options.PaperSize == null)
            {
                paperSize_comboBox.Text = EnumUtil.GetDescription(Option_PaperSize.A4);
            }
            else
            {
                paperSize_comboBox.Text = data.Options.PaperSize.GetDescription();
            }

            name_textBox.Text = data.Name;
            email_textBox.Text = data.Email;
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
            
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private void allPages_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            pages_textBox.Enabled = !allPages_radioButton.Checked;            
        }
    }
}
