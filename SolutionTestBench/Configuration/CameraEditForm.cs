using System;
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
    /// Form for editing Camera information.
    /// </summary>
    public partial class CameraEditForm : Form
    {
        private Camera _camera = null;
        private AssetInventoryContext _context = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraEditForm"/> class.
        /// Launches form with specified <see cref="Camera"/>.
        /// Treats the Camera as new.
        /// </summary>
        /// <param name="camera">The camera to display.</param>
        public CameraEditForm(Camera camera)
        {
            Initialize();
            _camera = camera;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraEditForm"/> class.
        /// Retrieves <see cref="Camera"/> from the database.
        /// </summary>
        /// <param name="assetId">The camera to retrieve and display.</param>
        public CameraEditForm(string assetId)
        {
            Initialize();
            _camera = _context.Assets.FirstOrDefault(x => x.AssetId == assetId) as Camera;
        }

        private void CameraEditForm_Load(object sender, EventArgs e)
        {
            if (_context.Assets.OfType<Camera>().Any(x => x.AssetId.Equals(_camera.AssetId, StringComparison.OrdinalIgnoreCase)))
            {
                assetId_TextBox.ReadOnly = true;
                ipAddress_Control.Focus();
            }
            else
            {
                //Didn't find the passed-in AssetId in the database.  New object, set defaults.
                if (string.IsNullOrEmpty(_camera.AssetId))
                {
                    AssetConfigurationController controller = new AssetConfigurationController(_context);
                    _camera.AssetType = _camera.GetType().Name;
                    _camera.Pool = controller.GetDefaultAssetPool();
                    _camera.Capability = AssetAttributes.None;
                }
                assetId_TextBox.Focus();
            }

            assetId_TextBox.Text = _camera.AssetId;
            ipAddress_Control.Text = _camera.IPAddress;
            cameraServer_TextBox.Text = _camera.CameraServer;
            printerId_TextBox.Text = _camera.PrinterId;
            description_TextBox.Text = _camera.Description;
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
            fieldValidator.RequireCustom(ipAddress_Control, () => ValidateIPAddress(ipAddress_Control.Text), "A valid IP Address is required.");
            fieldValidator.RequireValue(cameraServer_TextBox, cameraServer_Label);

            fieldValidator.SetIconAlignment(assetId_TextBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(ipAddress_Control, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(cameraServer_TextBox, ErrorIconAlignment.MiddleLeft);
        }

        private void CameraEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context.Dispose();
            _context = null;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                _camera.AssetId = assetId_TextBox.Text;
                _camera.Description = description_TextBox.Text;
                _camera.IPAddress = ipAddress_Control.Text;
                _camera.CameraServer = cameraServer_TextBox.Text;
                _camera.PrinterId = printerId_TextBox.Text;

                if (_context.Entry(_camera).State == EntityState.Detached)
                {
                    //New Camera
                    _context.Assets.Add(_camera);
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

            if (result && _context.Entry(_camera).State == EntityState.Detached)
            {
                // New camera
                if (_context.Assets.Any(x => x.AssetId.Equals(assetId_TextBox.Text)))
                {
                    MessageBox.Show($"A Camera with the Id '{assetId_TextBox.Text.Trim()}' already exists in the database.", "Save Camera Properties", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    assetId_TextBox.Focus();
                    result = false;
                }
            }

            return result;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private bool ValidateIPAddress(string ipAddress)
        {
            IPAddress address;
            return IPAddress.TryParse(ipAddress, out address);
        }
    }
}
