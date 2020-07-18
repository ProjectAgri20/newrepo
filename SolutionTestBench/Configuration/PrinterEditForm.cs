using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI;

namespace HP.ScalableTest
{
    /// <summary>
    /// Form for editing Printer information.
    /// </summary>
    public partial class PrinterEditForm : Form
    {
        private Printer _printer = null;
        private AssetInventoryContext _context = null;
        private List<CheckBox> _capabilityControls = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterEditForm"/> class.
        /// Launches form with specified <see cref="Printer"/>.
        /// Treats the printer as new.
        /// </summary>
        /// <param name="printer">The printer to display.</param>
        public PrinterEditForm(Printer printer)
        {
            Initialize();
            _printer = printer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterEditForm"/> class.
        /// Retrieves <see cref="Printer"/> from the database.
        /// </summary>
        /// <param name="printerId">The printer to retrieve and display.</param>
        public PrinterEditForm(string printerId)
        {
            Initialize();
            _printer = _context.Assets.FirstOrDefault(x => x.AssetId == printerId) as Printer;
        }

        private void PrinterEditForm_Load(object sender, EventArgs e)
        {
            if (_context.Assets.OfType<Printer>().Any(x => x.AssetId.Equals(_printer.AssetId, StringComparison.OrdinalIgnoreCase)))
            {
                description_TextBox.Focus();
                assetId_TextBox.ReadOnly = true;
            }
            else
            {
                //Didn't find the passed-in AssetId in the database.  New object, set defaults.
                if (string.IsNullOrEmpty(_printer.AssetId))
                {
                    AssetConfigurationController controller = new AssetConfigurationController(_context);
                    _printer.AssetType = _printer.GetType().Name;
                    _printer.Pool = controller.GetDefaultAssetPool();
                    _printer.Capability = AssetAttributes.Printer | AssetAttributes.ControlPanel;
                }
                assetId_TextBox.Focus();
            }
            
            var printers = _context.Assets.OfType<Printer>();

            foreach (var item in printers.Select(x => x.Owner).Where(x => x != null).Distinct())
            {
                contact_ComboBox.Items.Add(item);
            }

            manufacturer_ComboBox.Items.Add("HP");
            foreach (var item in printers.Select(x => x.Product).Where(x => x != null && !x.Equals("HP")).Distinct())
            {
                manufacturer_ComboBox.Items.Add(item);
            }

            foreach (var item in printers.Select(x => x.Model).Where(x => x != null).Distinct())
            {
                modelName_ComboBox.Items.Add(item);
            }

            foreach (var item in printers.Select(x => x.ModelNumber).Where(x => x != null).Distinct())
            {
                modelNumber_ComboBox.Items.Add(item);
            }

            foreach (var item in printers.Select(x => x.Location).Where(x => x != null).Distinct())
            {
                location_ComboBox.Items.Add(item);
            }

            assetId_TextBox.Text = _printer.AssetId;
            description_TextBox.Text = _printer.Description;
            address1_Control.Text = _printer.Address1;
            address2_Control.Text = _printer.Address2;
            serialNumber_TextBox.Text = _printer.SerialNumber;
            adminPassword_TextBox.Text = _printer.Password;

            if (contact_ComboBox.Items.Cast<string>().Contains(_printer.Owner))
            {
                contact_ComboBox.SelectedItem = _printer.Owner;
            }

            if (manufacturer_ComboBox.Items.Cast<string>().Contains(_printer.Product))
            {
                manufacturer_ComboBox.SelectedItem = _printer.Product;
            }

            if (modelName_ComboBox.Items.Cast<string>().Contains(_printer.Model))
            {
                modelName_ComboBox.SelectedItem = _printer.Model;
            }
            else
            {
                modelName_ComboBox.Text = _printer.Model;
            }

            if (modelNumber_ComboBox.Items.Cast<string>().Contains(_printer.ModelNumber))
            {
                modelNumber_ComboBox.SelectedItem = _printer.ModelNumber;
            }
            else
            {
                modelNumber_ComboBox.Text = _printer.ModelNumber;
            }

            if (location_ComboBox.Items.Cast<string>().Contains(_printer.Location))
            {
                location_ComboBox.SelectedItem = _printer.Location;
            }

            try
            {
                foreach (CheckBox checkBox in _capabilityControls)
                {
                    int checkBoxAttribute = int.Parse(checkBox.Tag.ToString());
                    checkBox.Checked = _printer.Capability.HasFlag((AssetAttributes)checkBoxAttribute);
                }
            }
            catch (FormatException ex)
            {
                TraceFactory.Logger.Error("Unable to Parse Printer Capability.  Make sure the CheckBox.Tag property is set on all Capability Checkboxes.", ex);
                MessageBox.Show("Unable to set Printer Capability.", "Load Printer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Initializes this form instance.
        /// </summary>
        private void Initialize()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            ShowIcon = true;
            _context = DbConnect.AssetInventoryContext();

            // Set up Validation
            fieldValidator.RequireValue(assetId_TextBox, assetId_Label);
            fieldValidator.RequireSelection(manufacturer_ComboBox, manufacturer_Label);
            fieldValidator.RequireSelection(modelName_ComboBox, modelName_Label);
            fieldValidator.RequireCustom(address1_Control, () => ValidateIPAddress(address1_Control.Text), "A valid IP Address is required.");
            fieldValidator.RequireCustom(printerCapabilites_Label, () => _capabilityControls.Any(n => n.Checked), "At least one capability must be selected.");
            fieldValidator.RequireValue(adminPassword_TextBox, adminPassword_Label);

            fieldValidator.SetIconAlignment(assetId_TextBox, ErrorIconAlignment.MiddleRight);
            fieldValidator.SetIconAlignment(manufacturer_ComboBox, ErrorIconAlignment.MiddleRight);
            fieldValidator.SetIconAlignment(modelName_ComboBox, ErrorIconAlignment.MiddleRight);
            fieldValidator.SetIconAlignment(address1_Control, ErrorIconAlignment.MiddleRight);
            fieldValidator.SetIconAlignment(printerCapabilites_Label, ErrorIconAlignment.MiddleRight);
            fieldValidator.SetIconAlignment(adminPassword_TextBox, ErrorIconAlignment.MiddleRight);

            _capabilityControls = new List<CheckBox>() { print_CheckBox, scan_CheckBox, controlPanel_CheckBox };
        }

        private void PrinterEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context.Dispose();
            _context = null;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                _printer.Location = location_ComboBox.Text;
                _printer.Product = manufacturer_ComboBox.Text;
                _printer.Model = modelName_ComboBox.Text;
                _printer.ModelNumber = modelNumber_ComboBox.Text;
                _printer.Owner = contact_ComboBox.Text;
                _printer.AssetId = assetId_TextBox.Text;
                _printer.Description = description_TextBox.Text;
                _printer.Address1 = address1_Control.Text;
                _printer.Address2 = address2_Control.Text;
                _printer.SerialNumber = serialNumber_TextBox.Text;
                _printer.Password = adminPassword_TextBox.Text;

                try
                {
                    int capability = 0;
                    foreach (CheckBox checkBox in _capabilityControls)
                    {
                        if (checkBox.Checked)
                        {
                            capability += int.Parse(checkBox.Tag.ToString());
                        }
                    }
                    _printer.Capability = (AssetAttributes)capability;
                }
                catch (FormatException ex)
                {
                    TraceFactory.Logger.Error("Unable to Parse Printer Capability.  Make sure the CheckBox.Tag property is set on all Capability Checkboxes.", ex);
                    MessageBox.Show("Unable to determine Printer Capability.", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Abort;
                    return;
                }

                if (_context.Entry(_printer).State == EntityState.Detached)
                {
                    // New Printer
                    _context.Assets.Add(_printer);
                }
                _context.SaveChanges();

                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Validates user input.
        /// </summary>
        /// <returns><c>true</c> if input is valid, <c>false</c> otherwise.</returns>
        private bool ValidateInput()
        {
            bool result = fieldValidator.ValidateAll().All(x => x.Succeeded);

            if (result && _context.Entry(_printer).State == EntityState.Detached)
            {
                // New Printer
                if (_context.Assets.Any(x => x.AssetId.Equals(assetId_TextBox.Text)))
                {
                    MessageBox.Show("A Printer with the Id '{0}' already exists in the database.".FormatWith(assetId_TextBox.Text.Trim()), "Unable to Import", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    assetId_TextBox.Focus();
                    result = false;
                }
            }

            return result;
        }

        private bool ValidateIPAddress(string ipAddress)
        {
            IPAddress address;
            return IPAddress.TryParse(ipAddress, out address);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

 
    }
}
