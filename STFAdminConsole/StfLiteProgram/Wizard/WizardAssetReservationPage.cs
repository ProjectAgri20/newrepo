using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest;
using HP.ScalableTest.UI.SessionExecution.Wizard;
using HP.SolutionTest.WorkBench.Properties;
using HP.ScalableTest.Core.AssetInventory.Reservation;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using System.Collections.Generic;

namespace HP.SolutionTest.WorkBench
{
    internal partial class WizardAssetReservationPage : UserControl, IWizardPage
    {
        private WizardConfiguration _configuration;

        /// <summary>
        /// Notification to cancel the wizard.
        /// </summary>
        public event EventHandler Cancel;

        public WizardAssetReservationPage()
        {
            InitializeComponent();
            AssetReservationRow.IconList = availabilityIcons_ImageList;
            UserInterfaceStyler.Configure(assetReservation_GridView, GridViewStyle.ReadOnly);
        }

        /// <summary>
        /// Initializes the wizard page with the specified <see cref="WizardConfiguration"/>.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public bool Initialize(WizardConfiguration configuration)
        {
            header_Label.Text = string.Empty;
            _configuration = configuration;

            // Make an initial reservation
            reserve_Button_Click(null, EventArgs.Empty);
            LoadReservedForComboBox();
            SetDisplay(_configuration.SessionAssets.Any());

            return true;
        }
        /// <summary>
        /// Performs final validation before allowing the user to navigate away from the page.
        /// </summary>
        /// <returns>
        /// True if this page was successfully validated.
        /// </returns>
        public bool Complete()
        {
            // If there are any assets that are not fully available, show a confirm dialog
            if (_configuration.SessionAssets.Any(n => n.Availability != AssetAvailability.Available))
            {
                DialogResult proceed = MessageBox.Show("One or more assets may be unavailable for all or part of the session.\nDo you want to continue?",
                    "Confirm Reservations", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                return (proceed == DialogResult.Yes);
            }

            return true;
        }

        private void reserve_Button_Click(object sender, EventArgs e)
        {
            RefreshReservations(assetReservationKey_ComboBox.Text);
        }

        private void RefreshReservations(string reservationKey)
        {
            // Call the dispatcher service to make reservations
            AssetDetailCollection assets = null;
            if (!string.IsNullOrWhiteSpace(reservationKey))
            {
                assets = SessionClient.Instance.Reserve(_configuration.Ticket.SessionId, reservationKey);
            }
            else
            {
                SessionClient.Instance.Ping();
                assets = SessionClient.Instance.Reserve(_configuration.Ticket.SessionId);
            }

            _configuration.SessionAssets.Replace(assets);

            // Display the results
            assetReservation_GridView.DataSource = null;
            assetReservation_GridView.DataSource = assets.Select(n => new AssetReservationRow(n));
            assetReservation_GridView.BestFitColumns();
        }

        private void SetDisplay(bool anyAssets)
        {
            assetReservation_GridView.Visible = anyAssets;
            assetReservationKey_Label.Visible = anyAssets;
            assetReservationKey_ComboBox.Visible = anyAssets;
            assetReservationRefresh_Button.Visible = anyAssets;
            if (anyAssets)
            {
                header_Label.Text = Resources.AssetsHeader;
                return;
            }

            header_Label.Text = Resources.NoAssetsHeader;

        }

        private void LoadReservedForComboBox()
        {
            List<string> keys = new List<string>() { string.Empty };
            keys.AddRange(ConfigurationServices.AssetInventory.GetAssets().Select(n => n.ReservationKey).Distinct().Where(n => !string.IsNullOrEmpty(n)));

            assetReservationKey_ComboBox.Items.Clear();
            foreach (string item in keys)
            {
                assetReservationKey_ComboBox.Items.Add(item);
            }
            assetReservationKey_ComboBox.SelectedIndex = 0;
        }

        private void assetReservationKey_ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode.Equals(Keys.Return) || e.KeyCode.Equals(Keys.Enter))
            {
                RefreshReservations(assetReservationKey_ComboBox.Text);
            }
        }

        private void assetReservationKey_ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            var key = assetReservationKey_ComboBox.SelectedItem;
            if (key != null)
            {
                RefreshReservations(key.ToString());
            }
        }
        
        /// <summary>
        /// Helper class used to display information in the GridView.
        /// </summary>
        private class AssetReservationRow
        {
            public static ImageList IconList { get; set; }

            public string AssetId { get; private set; }
            public string Description { get; private set; }
            public string Availability { get; private set; }
            public Image Icon { get; private set; }

            public AssetReservationRow(AssetDetail asset)
            {
                AssetId = asset.AssetId;
                Description = asset.Description ?? "<No information available.>";
                Availability = GetDescription(asset.Availability).FormatWith(asset.AvailabilityEndTime.ToString());
                Icon = IconList.Images[asset.Availability.ToString()];
            }

            private static string GetDescription(AssetAvailability availability)
            {
                switch (availability)
                {
                    case AssetAvailability.Available:
                        return "Available.";

                    case AssetAvailability.NotAvailable:
                        return "Not available.";

                    case AssetAvailability.PartiallyAvailable:
                        return "Available until {0}.";

                    case AssetAvailability.Unknown:
                        return "Status unknown.";

                    default:
                        return "Asset availability description undefined.";
                }
            }
        }
    }

}
