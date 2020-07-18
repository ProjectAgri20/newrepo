using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Control for selecting a print driver.
    /// </summary>
    public partial class PrintDriverSelectionControl : UserControl
    {
        private readonly List<PrintDriverInfo> _drivers = new List<PrintDriverInfo>();
        private readonly List<string> _driverPackages = new List<string>();
        private bool _suppressSelectionChanged = false;

        /// <summary>
        /// Occurs when this control's selection is changed.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the selection of the control changes.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDriverSelectionControl" /> class.
        /// </summary>
        public PrintDriverSelectionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the <see cref="PrintDriverInfo" /> selected in this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PrintDriverInfo SelectedPrintDriver
        {
            get
            {
                if (driverModel_ComboBox.SelectedIndex != -1)
                {
                    return driverModel_ComboBox.SelectedItem as PrintDriverInfo;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a selected print driver.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasSelection
        {
            get { return driverModel_ComboBox.SelectedIndex != -1; }
        }

        /// <summary>
        /// Initializes this control by loading all drivers from the asset inventory.
        /// </summary>
        public void Initialize()
        {
            LoadPackageComboBox();
        }

        /// <summary>
        /// Initializes this control by loading all drivers from the asset inventory
        /// with the specified driver selected.
        /// </summary>
        /// <param name="printDriverId">The ID of the print driver to select.</param>
        public void Initialize(Guid printDriverId)
        {
            Initialize();
            SetSelectedPrintDriver(printDriverId);
        }

        /// <summary>
        /// Initializes this control by loading all drivers from the asset inventory
        /// with the specified driver selected.
        /// </summary>
        /// <param name="printDriver">The print driver to select.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printDriver" /> is null.</exception>
        public void Initialize(PrintDriverInfo printDriver)
        {
            if (printDriver == null)
            {
                throw new ArgumentNullException(nameof(printDriver));
            }

            Initialize();
            SetSelectedPrintDriver(printDriver.PrintDriverId);
        }

        private void LoadPackageComboBox()
        {
            _suppressSelectionChanged = true;

            _drivers.Clear();
            _driverPackages.Clear();

            _drivers.AddRange(ConfigurationServices.AssetInventory.AsInternal().GetPrintDrivers());
            _driverPackages.AddRange(_drivers.Select(n => n.PackageName).Distinct());

            driverPackage_ComboBox.DataSource = null;
            driverPackage_ComboBox.DataSource = _driverPackages;
            driverPackage_ComboBox.SelectedIndex = -1;

            _suppressSelectionChanged = false;
        }

        private void SetSelectedPrintDriver(Guid printDriverId)
        {
            _suppressSelectionChanged = true;

            PrintDriverInfo driverToSelect = _drivers.FirstOrDefault(n => n.PrintDriverId == printDriverId);
            if (driverToSelect != null)
            {
                driverPackage_ComboBox.SelectedItem = driverToSelect.PackageName;
                driverModel_ComboBox.SelectedItem = driverToSelect;
            }

            _suppressSelectionChanged = false;
        }

        private void driverPackage_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            driverModel_ComboBox.DataSource = null;

            if (driverPackage_ComboBox.SelectedIndex != -1)
            {
                string selectedPackage = (string)driverPackage_ComboBox.SelectedItem;
                driverModel_ComboBox.DataSource = _drivers.Where(n => n.PackageName == selectedPackage).ToList();
                driverModel_ComboBox.DisplayMember = "DriverName";

                driverModel_Label.Enabled = true;
                driverModel_ComboBox.Enabled = true;
            }
            else
            {
                driverModel_Label.Enabled = false;
                driverModel_ComboBox.Enabled = false;
            }
        }

        private void driverModel_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            if (!_suppressSelectionChanged)
            {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
