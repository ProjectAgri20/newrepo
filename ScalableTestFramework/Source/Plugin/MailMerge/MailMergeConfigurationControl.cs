using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.MailMerge
{
    /// <summary>
    /// Mail Merge automation plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class MailMergeConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private MailMergeActivityData _activityData;
        private DynamicLocalPrintQueueInfo _localPrintQueueInfo;

        /// <summary>
        /// constructor
        /// </summary>
        public MailMergeConfigurationControl()
        {
            InitializeComponent();
            mail_fieldValidator.RequireValue(device_textBox, "Please select a device.");
            mail_fieldValidator.RequireValue(sourceName_textBox, sourceName_label);
            mail_fieldValidator.RequireValue(sourceDepartment_textBox, sourceDepartment_label);
            mail_fieldValidator.RequireValue(sourceDesignation_textBox, sourceDesignation_label);
            mail_fieldValidator.RequireValue(sourceCompany_textBox, sourceCompany_label);
            mail_fieldValidator.RequireCustom(recipientsList_listBox, ValidateRecipients, "Please add a recipient");
            mail_fieldValidator.RequireCustom(message_richTextBox, ValidateRichTextMessage, "Please add a personalized message");

            device_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            message_richTextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
        }

        private bool ValidateRecipients()
        {
            if (recipientsList_listBox.Items.Count == 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateRichTextMessage()
        {
            return !string.IsNullOrEmpty(message_richTextBox.Text);
        }

        private void recipientsAdd_button_Click(object sender, EventArgs e)
        {
            recipientsList_listBox.DataSource = null;

            if (string.IsNullOrEmpty(recipientsAddress_richTextBox.Text) || string.IsNullOrEmpty(recipientsName_textBox.Text))
            {
                return;
            }

            _activityData.RecipientCollection.Add(new Recipients() { Name = recipientsName_textBox.Text, Address = recipientsAddress_richTextBox.Text });

            recipientsList_listBox.DataSource = _activityData.RecipientCollection;
            recipientsList_listBox.DisplayMember = "Name";
        }

        private void recipientsRemove_button_Click(object sender, EventArgs e)
        {
            if (recipientsList_listBox.SelectedIndex > -1)
            {
                int index = recipientsList_listBox.SelectedIndex;
                recipientsList_listBox.DataSource = null;
                _activityData.RecipientCollection.RemoveAt(index);
                recipientsList_listBox.DataSource = _activityData.RecipientCollection;
                recipientsList_listBox.DisplayMember = "Name";
            }
        }

        private void letter_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (letter_radioButton.Checked)
            {
                _activityData.Format = MailMergeFormat.Letter;
                message_richTextBox.Enabled = true;
            }
            else
            {
                _activityData.Format = MailMergeFormat.Envelope;
                //disable few UI elements
                message_richTextBox.Enabled = false;
            }
        }

        private void printerSelect_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(device_textBox.Text))
            {
                using (DynamicLocalPrintQueueForm localQueueForm = new DynamicLocalPrintQueueForm(false))
                {
                    localQueueForm.ShowDialog(this);
                    if (localQueueForm.DialogResult == DialogResult.OK)
                    {
                        _localPrintQueueInfo = localQueueForm.PrintQueues.First();
                        device_textBox.Text = _localPrintQueueInfo.AssociatedAssetId + @", " + _localPrintQueueInfo.PrintDriver.DriverName;
                    }
                }
            }
            else
            {
                using (DynamicLocalPrintQueueForm localQueueForm = new DynamicLocalPrintQueueForm(_localPrintQueueInfo, false))
                {
                    localQueueForm.ShowDialog(this);
                    {
                        if (localQueueForm.DialogResult == DialogResult.OK)
                        {
                            _localPrintQueueInfo = localQueueForm.PrintQueues.First();
                            device_textBox.Text = _localPrintQueueInfo.AssociatedAssetId + @", " + _localPrintQueueInfo.PrintDriver.DriverName;
                        }
                    }
                }
            }
        }

        #region IPluginConfigurationControl

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            // Initialize the activity data with a default value
            _activityData = new MailMergeActivityData();
            PopulateMailMergeUi();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from an existing copy of configuration information.
            _activityData = configuration.GetMetadata<MailMergeActivityData>();

            DynamicLocalPrintQueueDefinition definition = configuration.PrintQueues.SelectedPrintQueues.First() as DynamicLocalPrintQueueDefinition;
            IPrinterInfo printDevice = ConfigurationServices.AssetInventory.GetAsset(definition.AssetId) as IPrinterInfo;
            PrintDriverInfo printDriver = ConfigurationServices.AssetInventory.AsInternal().GetPrintDrivers().First(n => n.PrintDriverId == definition.PrintDriverId);
            _localPrintQueueInfo = new DynamicLocalPrintQueueInfo(printDevice, printDriver, definition.PrinterPort, definition.PrintDriverConfiguration);

            PopulateMailMergeUi();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.Originator = new Originator()
            {
                Name = sourceName_textBox.Text,
                Designation = sourceDesignation_textBox.Text,
                Department = sourceDepartment_textBox.Text,
                Company = sourceCompany_textBox.Text
            };
            _activityData.MessageBody = message_richTextBox.Text;

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = new AssetSelectionData(ConfigurationServices.AssetInventory.GetAsset(_localPrintQueueInfo.AssociatedAssetId)),
                PrintQueues = new PrintQueueSelectionData(_localPrintQueueInfo)
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            // This is where you can add any validation for your UI and then
            // return the appropriate validation result when saving the data.

            return new PluginValidationResult(mail_fieldValidator.ValidateAll());
        }

        #endregion IPluginConfigurationControl

        private void PopulateMailMergeUi()
        {
            // Set up data bindings
            jobseparator_checkBox.DataBindings.Clear();
            jobseparator_checkBox.DataBindings.Add("Checked", _activityData, "PrintJobSeparator");

            if (_activityData.Originator != null)
            {
                device_textBox.Text = _localPrintQueueInfo.AssociatedAssetId + @", " +
                                 _localPrintQueueInfo.PrintDriver.DriverName;
                //load up UI
                recipientsList_listBox.DataSource = _activityData.RecipientCollection;
                recipientsList_listBox.DisplayMember = "Name";

                sourceCompany_textBox.Text = _activityData.Originator.Company;
                sourceDepartment_textBox.Text = _activityData.Originator.Department;
                sourceDesignation_textBox.Text = _activityData.Originator.Designation;
                sourceName_textBox.Text = _activityData.Originator.Name;

                if (_activityData.Format == MailMergeFormat.Letter)
                {
                    letter_radioButton.Checked = true;
                }
                else
                {
                    envelope_radioButton.Checked = true;
                }

                message_richTextBox.Text = _activityData.MessageBody;
            }
        }
    }
}