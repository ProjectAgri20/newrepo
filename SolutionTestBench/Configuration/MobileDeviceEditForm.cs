using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// User Interface for managing Mobile Device information.
    /// </summary>
    public partial class MobileDeviceEditForm : Form
    {
        private MobileDevice _mobileDevice = null;
        private AssetInventoryContext _context = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileDeviceEditForm"/>.
        /// Launches form with specified <see cref="MobileDevice"/>.
        /// </summary>
        /// <param name="mobileDevice">The <see cref="MobileDevice"/> to display.</param>
        public MobileDeviceEditForm(MobileDevice mobileDevice)
        {
            Initialize();
            _mobileDevice = mobileDevice;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileDeviceEditForm"/>.
        /// Retrieves <see cref="MobileDevice"/> from the database.
        /// </summary>
        /// <param name="assetId">The asset ID of the mobile device to display.</param>
        public MobileDeviceEditForm(string assetId)
        {
            Initialize();
            _mobileDevice = _context.Assets.FirstOrDefault(x => x.AssetId == assetId) as MobileDevice;
        }

        private void MobileDeviceEditForm_Load(object sender, EventArgs e)
        {
            if (_context.Assets.OfType<MobileDevice>().Any(x => x.AssetId.Equals(_mobileDevice.AssetId, StringComparison.OrdinalIgnoreCase)))
            {
                assetId_TextBox.ReadOnly = true;
                connectionId_TextBox.Focus();
            }
            else
            {
                //Didn't find the passed-in AssetId in the database.  New object, set defaults.
                if (string.IsNullOrEmpty(_mobileDevice.AssetId))
                {
                    AssetConfigurationController controller = new AssetConfigurationController(_context);
                    int count = _context.Assets.OfType<MobileDevice>().Count() + 1;
                    _mobileDevice.AssetId = $"MOB-{count.ToString("D5")}";
                    _mobileDevice.AssetType = _mobileDevice.GetType().Name;
                    _mobileDevice.Pool = controller.GetDefaultAssetPool();
                    _mobileDevice.Capability = AssetAttributes.Mobile;
                }
                assetId_TextBox.Focus();
            }

            assetId_TextBox.Text = _mobileDevice.AssetId;
            connectionId_TextBox.Text = _mobileDevice.MobileEquipmentId;
            type_ComboBox.Text = _mobileDevice.MobileDeviceType;
            description_TextBox.Text = _mobileDevice.Description;
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
            fieldValidator.RequireValue(connectionId_TextBox, connectionId_Label);
            fieldValidator.RequireSelection(type_ComboBox, type_Label);

            fieldValidator.SetIconAlignment(assetId_TextBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(connectionId_TextBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(type_ComboBox, ErrorIconAlignment.MiddleLeft);

            foreach (MobileDeviceType deviceType in Enum.GetValues(typeof(MobileDeviceType)))
            {
                type_ComboBox.Items.Add(deviceType.ToString());
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                _mobileDevice.MobileEquipmentId = connectionId_TextBox.Text;
                _mobileDevice.MobileDeviceType = type_ComboBox.Text;
                _mobileDevice.Description = description_TextBox.Text;

                if (_context.Entry(_mobileDevice).State == EntityState.Detached)
                {
                    //New Mobile Device
                    _mobileDevice.AssetId = assetId_TextBox.Text;
                    _context.Assets.Add(_mobileDevice);
                }
                _context.SaveChanges();

                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateInput()
        {
            bool result = fieldValidator.ValidateAll().All(x => x.Succeeded);

            if (result && _context.Entry(_mobileDevice).State == EntityState.Detached)
            {
                // New mobile device
                if (_context.Assets.Any(x => x.AssetId.Equals(assetId_TextBox.Text)))
                {
                    MessageBox.Show($"A Mobile Device with the Id '{assetId_TextBox.Text.Trim()}' already exists in the database.", "Save Mobile Device", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    assetId_TextBox.Focus();
                    result = false;
                }
            }

            return result;
        }
    }
}
