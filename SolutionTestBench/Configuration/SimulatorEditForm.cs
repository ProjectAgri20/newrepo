using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Form for editing Device Simulator information.
    /// </summary>
    public partial class SimulatorEditForm : Form
    {
        private DeviceSimulator _simulator = null;
        private AssetInventoryContext _context = null;
        private List<CheckBox> _capabilityControls = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorEditForm"/> class.
        /// Launches form with specified <see cref="DeviceSimulator"/>.
        /// Treats the printer as new.
        /// </summary>
        /// <param name="simulator">The simulator to display.</param>
        public SimulatorEditForm(DeviceSimulator simulator)
        {
            Initialize();
            _simulator = simulator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorEditForm"/> class.
        /// Retrieves the <see cref="DeviceSimulator"/> from the database.
        /// </summary>
        /// <param name="simulatorId">The ID of the DeviceSimulator to retrieve and display.</param>
        public SimulatorEditForm(string simulatorId)
        {
            Initialize();
            _simulator = _context.Assets.FirstOrDefault(x => x.AssetId == simulatorId) as DeviceSimulator;
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
            fieldValidator.RequireValue(textBox_AssetId, label_AssetId);
            fieldValidator.RequireValue(textBox_Product, label_Product);
            fieldValidator.RequireValue(textBox_Address, label_Address);
            fieldValidator.RequireValue(textBox_VmName, label_VmName);
            fieldValidator.RequireCustom(label_Capabilites, () => _capabilityControls.Any(n => n.Checked), "At least one capability must be selected.");

            fieldValidator.SetIconAlignment(textBox_AssetId, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(textBox_Product, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(textBox_Address, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(textBox_VmName, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(label_Capabilites, ErrorIconAlignment.MiddleLeft);

            _capabilityControls = new List<CheckBox>() { checkBox_Scan, checkBox_ControlPanel, checkBox_Mobile };
        }

        private void SimulatorEditForm_Load(object sender, EventArgs e)
        {
            textBox_AssetId.Text = _simulator.AssetId;
            textBox_Product.Text = _simulator.Product;
            textBox_Address.Text = _simulator.Address;
            textBox_VmName.Text = _simulator.VirtualMachine;
            textBox_Firmware.Text = _simulator.FirmwareVersion;
            textBox_Password.Text = _simulator.Password;
            comboBox_Type.Text = _simulator.SimulatorType;
            try
            {
                foreach (CheckBox checkBox in _capabilityControls)
                {
                    int checkBoxAttribute = int.Parse(checkBox.Tag.ToString());
                    checkBox.Checked = _simulator.Capability.HasFlag((AssetAttributes)checkBoxAttribute);
                }
            }
            catch (FormatException ex)
            {
                TraceFactory.Logger.Error("Unable to Parse Simulator Capability.  Make sure the CheckBox.Tag property is set on all Capability Checkboxes.", ex);
                MessageBox.Show("Unable to set Simulator Capability.", "Load Simulator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            textBox_AssetId.ReadOnly = !string.IsNullOrEmpty(_simulator.AssetId);
            comboBox_Type.SelectedIndexChanged += comboBox_Type_SelectedIndexChanged;
        }

        private void SimulatorEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context.Dispose();
            _context = null;
        }

        private void comboBox_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox_Mobile.Checked = (comboBox_Type.Text == AssetAttributes.Mobile.ToString());
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            using (new BusyCursor())
            {
                if (ValidateInput())
                {
                    AssetConfigurationController controller = new AssetConfigurationController(_context);
                    _simulator.AssetId = textBox_AssetId.Text;
                    _simulator.Product = textBox_Product.Text;
                    _simulator.Address = textBox_Address.Text;
                    _simulator.VirtualMachine = textBox_VmName.Text;
                    _simulator.FirmwareVersion = textBox_Firmware.Text;
                    _simulator.Password = textBox_Password.Text;
                    _simulator.SimulatorType = comboBox_Type.Text;
                    _simulator.Pool = controller.GetDefaultAssetPool();

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
                        _simulator.Capability = (AssetAttributes)capability;
                    }
                    catch (FormatException ex)
                    {
                        TraceFactory.Logger.Error("Unable to Parse Simulator Capability.  Make sure the CheckBox.Tag property is set on all Capability Checkboxes.", ex);
                        MessageBox.Show("Unable to determine Simulator Capability.", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DialogResult = DialogResult.Abort;
                        return;
                    }

                    if (_context.Entry(_simulator).State == EntityState.Detached)
                    {
                        // New Simulator
                        _context.Assets.Add(_simulator);
                    }
                    _context.SaveChanges();

                    DialogResult = DialogResult.OK;
                }
            }
        }

        /// <summary>
        /// Validates user input.
        /// </summary>
        /// <returns><c>true</c> if input is valid, <c>false</c> otherwise.</returns>
        private bool ValidateInput()
        {
            bool result = fieldValidator.ValidateAll().All(x => x.Succeeded);

            if (result && _context.Entry(_simulator).State == EntityState.Detached)
            {
                // New Simulator
                if (_context.Assets.Any(x => x.AssetId.Equals(textBox_AssetId.Text)))
                {
                    MessageBox.Show($"A Simulator with the Id '{textBox_AssetId.Text.Trim()}' already exists in the database.", "Add Simulator", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    textBox_AssetId.Focus();
                    result = false;
                }
            }

            return result;
        }

    }
}
