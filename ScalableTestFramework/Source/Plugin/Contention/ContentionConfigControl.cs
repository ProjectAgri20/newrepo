using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.Contention
{
    /// <summary>
    /// Control to configure data for a Contention activity.
    /// </summary>
    public partial class ContentionConfigControl : UserControl, IPluginConfigurationControl
    {        
        public const string Version = "1.1";

        private const AssetAttributes DeviceAttributes = AssetAttributes.Printer | AssetAttributes.Scanner | AssetAttributes.ControlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentionConfigControl"/> class.
        /// </summary>
        public ContentionConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireCustom(ControlPanelActivity_groupBox, ValidateControlPanelActivities, "Atleast one Control Panel activity needs to be selected");
            fieldValidator.RequireCustom(ContentionActivity_groupBox, ValidateContentionActivities, "Atleast one Contention activity needs to be selected");

            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireDocumentSelection(documentSelectionControl, print_checkBox);
            fieldValidator.RequireValue(queueNameTextBox, "Queue Name", print_checkBox);
            fieldValidator.RequireValue(faxNumber_textBox, "Fax Number", faxsend_checkBox);

            fieldValidator.RequireCustom(emailAddressTextBox, () => ValidateScanFields(emailAddressTextBox, emailRadioButton), "Email address is required");
            fieldValidator.RequireCustom(folderPathTextBox, () => ValidateScanFields(folderPathTextBox, folderRadioButton), "Folder Path is required");
            fieldValidator.RequireCustom(usbNameTextBox, () => ValidateScanFields(usbNameTextBox, usbRadioButton), "USB Name is required");

            copy_checkBox.CheckedChanged += ConfigurationChanged;
            scan_checkBox.CheckedChanged += ConfigurationChanged;
            faxsend_checkBox.CheckedChanged += ConfigurationChanged;
            Copies_NumericUpDown.ValueChanged += ConfigurationChanged;
            copyPageCount_NumericUpDown.ValueChanged += ConfigurationChanged;
            emailRadioButton.CheckedChanged += ConfigurationChanged;
            folderRadioButton.CheckedChanged += ConfigurationChanged;
            jobStorageRadioButton.CheckedChanged += ConfigurationChanged;
            usbRadioButton.CheckedChanged += ConfigurationChanged;
            emailAddressTextBox.TextChanged += ConfigurationChanged;
            folderPathTextBox.TextChanged += ConfigurationChanged;
            usbNameTextBox.TextChanged += ConfigurationChanged;
            scanPageCount_NumericUpDown.ValueChanged += ConfigurationChanged;
            faxNumber_textBox.TextChanged += ConfigurationChanged;
            faxPageCount_NumericUpDown.ValueChanged += ConfigurationChanged;

            print_checkBox.CheckedChanged += ConfigurationChanged;
            queueNameTextBox.TextChanged += ConfigurationChanged;
            documentSelectionControl.SelectionChanged += ConfigurationChanged;
            assetSelectionControl.SelectionChanged += ConfigurationChanged;
        }

        private bool ValidateScanFields(TextBoxBase textbox, RadioButton radioButton)
        {
            bool validationResult = true;
            if (scan_checkBox.Checked && radioButton.Checked)
            {
                validationResult = !string.IsNullOrEmpty(textbox.Text);
            }
            return validationResult;
        }

        private bool ValidateControlPanelActivities()
        {
            if (copy_checkBox.Checked || scan_checkBox.Checked || faxsend_checkBox.Checked)
            {
                return true;
            }
            return false;
        }

        private bool ValidateContentionActivities()
        {
            if (print_checkBox.Checked)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            ContentionData data = new ContentionData()
            {
                SelectedControlPanelActivities = GetControlPanelData(),
                SelectedContentionActivities = GetContentionActivityData()
            };

            LocalPrintQueueInfo queue = null;
            PrintQueueSelectionData localQueueSelectionData = null;
            if (data.SelectedContentionActivities.OfType<PrintActivityData>().Any())
            {
                PrintActivityData printData = data.SelectedContentionActivities.OfType<PrintActivityData>().Single();
                queue = string.IsNullOrEmpty(printData.QueueName) ? null : new LocalPrintQueueInfo(printData.QueueName);
                localQueueSelectionData = (queue == null) ? null : new PrintQueueSelectionData(queue);
            }
            
            return new PluginConfigurationData(data, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
                Documents = documentSelectionControl.DocumentSelectionData,
                PrintQueues = localQueueSelectionData
            };
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            ConfigureControls(new ContentionData());
            documentSelectionControl.Initialize();
            assetSelectionControl.Initialize(DeviceAttributes);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            var contentionData = configuration.GetMetadata<ContentionData>();
            ConfigureControls(contentionData);
            documentSelectionControl.Initialize(configuration.Documents);
            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        private void ConfigureControls(ContentionData data)
        {
            //Copy Tab Controls
            if (data.SelectedControlPanelActivities.OfType<CopyActivityData>().Any())
            {
                copy_checkBox.Checked = true;
                CopyActivityData copyData = data.SelectedControlPanelActivities.OfType<CopyActivityData>().Single();
                Copies_NumericUpDown.Value = copyData.Copies;
                copyPageCount_NumericUpDown.Value = copyData.PageCount;
            }
            else
            {
                copy_checkBox.Checked = false;
            }

            //Scan Tab Controls
            if (data.SelectedControlPanelActivities.OfType<ScanActivityData>().Any())
            {
                scan_checkBox.Checked = true;
                ScanActivityData scanData = data.SelectedControlPanelActivities.OfType<ScanActivityData>().Single();
                switch (scanData.ScanJobType)
                {
                    case ContentionScanActivityTypes.Email:
                        emailRadioButton.Checked = true;
                        emailAddressTextBox.Text = scanData.EmailAddress;
                        break;

                    case ContentionScanActivityTypes.Folder:
                        folderRadioButton.Checked = true;
                        folderPathTextBox.Text = scanData.FolderPath;
                        break;

                    case ContentionScanActivityTypes.USB:
                        usbRadioButton.Checked = true;
                        usbNameTextBox.Text = scanData.UsbName;
                        break;

                    case ContentionScanActivityTypes.JobStorage:
                        jobStorageRadioButton.Checked = true;
                        break;
                }
                scanPageCount_NumericUpDown.Value = scanData.PageCount;
            }
            else
            {
                scan_checkBox.Checked = false;
            }

            //Fax Tab Controls
            if (data.SelectedControlPanelActivities.OfType<FaxActivityData>().Any())
            {
                faxsend_checkBox.Checked = true;
                FaxActivityData faxData = data.SelectedControlPanelActivities.OfType<FaxActivityData>().Single();
                faxNumber_textBox.Text = faxData.FaxNumber;
                faxPageCount_NumericUpDown.Value = faxData.PageCount;
            }
            else
            {
                faxsend_checkBox.Checked = false;
            }

            //Print Tab Controls
            if (data.SelectedContentionActivities.OfType<PrintActivityData>().Any())
            {
                print_checkBox.Checked = true;
                PrintActivityData printData = data.SelectedContentionActivities.OfType<PrintActivityData>().Single();
                queueNameTextBox.Text = printData.QueueName;
            }
            else
            {
                print_checkBox.Checked = false;
            }
        }

        private List<object> GetControlPanelData()
        {
            List<object> selectedControlPanelActivities = new List<object>();

            if (copy_checkBox.Checked)
            {
                //Add CopyActivityData object to SelectedControlPanelActivities
                selectedControlPanelActivities.Add(GetCopyData());
            }
            if (scan_checkBox.Checked)
            {
                //Add ScanActivityData object to SelectedControlPanelActivities
                selectedControlPanelActivities.Add(GetScanData());
            }
            if (faxsend_checkBox.Checked)
            {
                //Add FaxActivityData object to SelectedControlPanelActivities
                selectedControlPanelActivities.Add(GetFaxData());
            }

            return selectedControlPanelActivities;
        }

        private CopyActivityData GetCopyData()
        {
            CopyActivityData copyData = new CopyActivityData();
            copyData.Copies = (int)Copies_NumericUpDown.Value;
            copyData.PageCount = (int)copyPageCount_NumericUpDown.Value;
            return copyData;
        }

        private ScanActivityData GetScanData()
        {
            ScanActivityData scanData = new ScanActivityData();
            scanData.ScanJobType = emailRadioButton.Checked ? ContentionScanActivityTypes.Email : (folderRadioButton.Checked ? ContentionScanActivityTypes.Folder : (jobStorageRadioButton.Checked ? ContentionScanActivityTypes.JobStorage : ContentionScanActivityTypes.USB));
            scanData.PageCount = (int)scanPageCount_NumericUpDown.Value;
            scanData.EmailAddress = emailAddressTextBox.Text;
            scanData.FolderPath = folderPathTextBox.Text;
            scanData.UsbName = usbNameTextBox.Text;
            return scanData;
        }

        private FaxActivityData GetFaxData()
        {
            FaxActivityData faxData = new FaxActivityData();
            faxData.FaxNumber = faxNumber_textBox.Text;
            faxData.PageCount = (int)faxPageCount_NumericUpDown.Value;
            return faxData;
        }

        private List<object> GetContentionActivityData()
        {
            List<object> selectedContentionActivities = new List<object>();
            if (print_checkBox.Checked)
            {
                selectedContentionActivities.Add(GetPrintData());
            }
            return selectedContentionActivities;
        }

        private PrintActivityData GetPrintData()
        {
            PrintActivityData printData = new PrintActivityData();
            printData.QueueName = queueNameTextBox.Text;
            return printData;
        }

        private void ScanRadioButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (emailRadioButton.Checked)
            {
                EnableGroupBox(emailGroupBox);
                DisableGroupBox(folderGroupBox);
                DisableGroupBox(usbGroupBox);
            }
            else if (folderRadioButton.Checked)
            {
                EnableGroupBox(folderGroupBox);
                DisableGroupBox(emailGroupBox);
                DisableGroupBox(usbGroupBox);
            }
            else if (usbRadioButton.Checked)
            {
                EnableGroupBox(usbGroupBox);
                DisableGroupBox(emailGroupBox);
                DisableGroupBox(folderGroupBox);
            }
            else
            {
                DisableGroupBox(emailGroupBox);
                DisableGroupBox(folderGroupBox);
                DisableGroupBox(usbGroupBox);
            }
        }

        private void EnableGroupBox(GroupBox groupBox)
        {
            groupBox.Enabled = true;
            groupBox.Visible = true;
        }
        private void DisableGroupBox(GroupBox groupBox)
        {
            groupBox.Enabled = false;
            groupBox.Visible = false;
        }
    }
}
