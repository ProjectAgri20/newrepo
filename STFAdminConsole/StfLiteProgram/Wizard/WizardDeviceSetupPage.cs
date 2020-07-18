using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;
using HP.ScalableTest.UI.ScenarioConfiguration;
using System.Drawing;
using HP.ScalableTest.UI.SessionExecution.Wizard;
using System.Collections.Generic;

namespace HP.SolutionTest.WorkBench
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
            UserInterfaceStyler.Configure(deviceSetup_GridView, GridViewStyle.Display);
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
            deviceSetup_GridView.DataSource = printDevices;
            //deviceSetup_GridView.BestFitColumns();

            return printDevices.Any();
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
        /// Retrieves the asset ID from the row that has a selected cell.
        /// </summary>
        /// <returns>List of asset Ids</returns>
        private List<string> SelectedAssetIds()
        {
            List<string> assetIds = new List<string>();

            foreach (GridViewRowInfo row in deviceSetup_GridView.SelectedRows)
            {
                string assetId = row.Cells["deviceId_GridViewColumn"].Value.ToString().Trim();
                if (!assetIds.Exists(x => x.Equals(assetId)))
                {
                    assetIds.Add(assetId);
                }
            }

            return assetIds;
        }

		/// <summary>
		/// Handles the Click event of the CrcPaperLess_ToolStripBtn control.
		/// checks or un-checks the CRC check box value
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void CrcPaperLess_ToolStripBtn_Click(object sender, EventArgs e)
		{
			ToolStripButton tsb = (ToolStripButton)sender;
			bool bCrcOn = SetCrcPaperLessImage(tsb);
			foreach (GridViewRowInfo row in deviceSetup_GridView.Rows)
			{
				row.Cells["crcMode_GridViewColumn"].Value = bCrcOn;
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


    }
}
