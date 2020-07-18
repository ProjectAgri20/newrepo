using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;
using System.Drawing;

namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    internal partial class WizardDeviceSetupPage : UserControl, IWizardPage
    {
        private AssetDetailCollection _assets;

        /// <summary>
        /// Notification to cancel the wizard.
        /// </summary>
        public event EventHandler Cancel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardDeviceSetupPage"/> class.
        /// </summary>
        public WizardDeviceSetupPage()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(printerSetup_GridView, GridViewStyle.Display);
            UserInterfaceStyler.Configure(cameraSetup_GridView, GridViewStyle.Display);
        }

        /// <summary>
        /// Initializes the wizard page with the specified <see cref="WizardConfiguration"/>.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public bool Initialize(WizardConfiguration configuration)
        {
            _assets = configuration.SessionAssets;

            AvailableDeviceSet<AssetDetail> assetSet = new AvailableDeviceSet<AssetDetail>(_assets);
            var printDevices = assetSet.Devices.OfType<PrintDeviceDetail>().ToList();
            var cameraDevices = assetSet.Devices.OfType<CameraDetail>().ToList();
            printerSetup_GridView.DataSource = printDevices;
            printerSetup_GridView.BestFitColumns();

            cameraSetup_GridView.DataSource = cameraDevices;
            cameraSetup_GridView.BestFitColumns();

            return (printDevices.Any() || cameraDevices.Any());
        }

        /// <summary>
        /// Performs final validation before allowing the user to navigate away from the page.
        /// </summary>
        /// <returns>
        /// True if this page was successfully validated.
        /// </returns>
        public bool Complete()
        {
            return true;
        }

		/// <summary>
		/// Handles the Click event of the CrcPaperLess_ToolStripBtn control.
		/// Turns on or off the CRC paperless mode for the device (s)
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void CrcPaperLess_ToolStripBtn_Click(object sender, EventArgs e)
		{
			ToolStripButton tsb = (ToolStripButton)sender;
			bool bCrcOn = SetCrcPaperLessImage(tsb);
			foreach (GridViewRowInfo row in printerSetup_GridView.Rows)
			{
				row.Cells["crcMode_GridViewColumn"].Value = bCrcOn;
			}
		}

        /// <summary>
		/// Handles the Click event of the CrcPaperLess_ToolStripBtn control.
		/// Turns on or off the CRC paperless mode for the device (s)
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void Recording_ToolStripBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton tsb = (ToolStripButton)sender;
            bool bCameraRecordingOn = SetCameraRecordingImage(tsb);
            foreach (GridViewRowInfo row in cameraSetup_GridView.Rows)
            {
                row.Cells["recordingMode_GridViewColumn"].Value = bCameraRecordingOn;
            }
        }

        /// <summary>
        /// Sets the CRC paper less image, checkbox on or off.
        /// </summary>
        /// <param name="tsb">The TSB.</param>
        /// <returns></returns>
        private bool SetCrcPaperLessImage(ToolStripButton tsb)
		{
			bool bImageOn = true;

			if (tsb.Checked)
			{
				tsb.Image = new Bitmap(Properties.Resources.CheckboxOn);
			}
			else
			{
				tsb.Image = new Bitmap(Properties.Resources.CheckboxOff);
				bImageOn = false;
			}
			return bImageOn;
		}


        /// <summary>
        /// Sets the camera recording image, checkbox on or off.
        /// </summary>
        /// <param name="tsb">The TSB.</param>
        /// <returns></returns>
        private bool SetCameraRecordingImage(ToolStripButton tsb)
        {
            bool bImageOn = true;

            if (tsb.Checked)
            {
                tsb.Image = new Bitmap(Properties.Resources.CheckboxOn);
            }
            else
            {
                tsb.Image = new Bitmap(Properties.Resources.CheckboxOff);
                bImageOn = false;
            }
            return bImageOn;
        }

    }
}
