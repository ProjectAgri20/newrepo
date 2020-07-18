using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Print.Drivers;

namespace HP.ScalableTest.PluginSupport.Connectivity.UI
{
    /// <summary>
    /// Control for selecting the driver details such as driver package path and driver model.
    /// </summary>
    public partial class PrintDriverSelector : UserControl
    {
        #region Constants

        const string DIRECTORY_DRIVERS = "Drivers";

        #endregion

        #region Local Variables

        /// <summary>
        /// Contains product category
        /// </summary>
        private string _productCategory;

        /// <summary>
        /// Contains product name
        /// </summary>
        private string _productName;

        /// <summary>
        /// Occurs when a property value is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of PrintDriverSelector.
        /// </summary>
        public PrintDriverSelector()
        {
            InitializeComponent();

            //driverDetails_FieldValidator.RequireValue(driverPackage_TextBox, package_Label);
            //driverDetails_FieldValidator.RequireValue(driverModel_ComboBox, model_Label);

            //driverDetails_FieldValidator.SetIconAlignment(driverPackage_TextBox, ErrorIconAlignment.MiddleRight);
            //driverDetails_FieldValidator.SetIconAlignment(driverModel_ComboBox, ErrorIconAlignment.MiddleRight);
        }

        #endregion

        #region Properties
		/// <summary>
        /// Initialize driver package Path in Plugin UI
        /// </summary>
        public void Initialize()
        {
            if (!DesignMode)
            {
                driverPackage_TextBox.Text = string.Empty;
            }

            PrinterFamily = ProductFamilies.InkJet.ToString();
           
            driverDetails_FieldValidator.RequireValue(driverPackage_TextBox, package_Label);
            driverDetails_FieldValidator.RequireValue(driverModel_ComboBox, model_Label);

            driverDetails_FieldValidator.SetIconAlignment(driverPackage_TextBox, ErrorIconAlignment.MiddleRight);
            driverDetails_FieldValidator.SetIconAlignment(driverModel_ComboBox, ErrorIconAlignment.MiddleRight);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the driver package path
        /// </summary>
        public string DriverPackagePath
        {
            get { return !DesignMode ? driverPackage_TextBox.Text : string.Empty; }
            set
            {
                if (DesignMode || string.IsNullOrEmpty(value)) return;
                driverPackage_TextBox.Text = Directory.Exists(value) ? value : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the category to which the printer belongs
        /// </summary>
        public string PrinterFamily
        {
            get { return _productCategory; }

            set
            {
                ProductFamilies family;

                if (Enum.TryParse(value, out family))
                {
                    _productCategory = value;
                    PropertyChanged += PrintDriverSelector_PropertyChanged;

                    // Fire the event only if both the ProductType & ProducName are set
                    if (!string.IsNullOrEmpty(PrinterFamily) && !string.IsNullOrEmpty(PrinterName))
                    {
                        OnPropertyChanged(PrinterFamily);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets name of the product
        /// </summary>
        public string PrinterName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                PropertyChanged += PrintDriverSelector_PropertyChanged;

                // Fire the event only if both the ProductType & ProducName are set
                if (!string.IsNullOrEmpty(PrinterName) && !string.IsNullOrEmpty(PrinterFamily))
                {
                    OnPropertyChanged(PrinterFamily);
                }
            }
        }

        /// <summary>
        /// Gets the driver model selected on the control
        /// </summary>
        public string DriverModel
        {
            get
            {
                return !DesignMode ? driverModel_ComboBox.SelectedItem.ToString() : string.Empty;
            }
            set
            {
                driverModel_ComboBox.SelectedItem = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
		/// Validating the control elements
		/// </summary>
        /// <returns></returns>
		public ValidationResult ValidateControls()
        {
            bool status = driverDetails_FieldValidator.ValidateAll().Aggregate(true, (current, result) => current & result.Succeeded);

            return new ValidationResult(status);
        }

        #endregion

        #region Events

        /// <summary>
        /// Event Handler for property change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PrintDriverSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DriverModel"))
            {
                driverModel_ComboBox.SelectedItem = DriverModel;
            }
            else
            {
                driverPackage_TextBox.Text = Path.Combine(CtcSettings.ConnectivityShare, PrinterFamily, PrinterName, DIRECTORY_DRIVERS);
            }
        }

        /// <summary>
        /// On change of Driver package load the driver models.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void driverPackage_TextBox_TextChanged(object sender, EventArgs e)
        {
            LoadDrivers();
            OnPropertyChanged(new PropertyChangedEventArgs(DriverPackagePath));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the drivers into driver model drop down box.         
        /// </summary>        
        private void LoadDrivers()
        {
            string driverPackagePath = driverPackage_TextBox.Text;

            if (Directory.Exists(driverPackagePath))
            {
                // walk thru the model directories and load only the models              
                int filesCount = Directory.EnumerateFiles(driverPackagePath, "*.inf", SearchOption.TopDirectoryOnly).Count();

                if (filesCount > 0)
                {
                    var drivers = DriverController.LoadFromDirectory(driverPackage_TextBox.Text, true);

                    // Populates the driver model combo box only if the count of driver models are available
                    if (drivers.Any())
                    {
                        driverModel_ComboBox.DataSource = drivers.Select(x => x.Name).Distinct().ToList();
                    }
                }
            }
        }

        /// <summary>
        /// Occurs on property changed
        /// </summary>
        /// <param name="e">PropertyChangedEventArgs</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs on property changed
        /// </summary>
        /// <param name="propertyName">propertyName</param>
        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Occurs when the selected index changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"><see cref="EventArgs"/></param>
        private void driverModel_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(DriverModel));
        }

        #endregion
        private void driverModel_ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            driver_toolTip.Hide(driverModel_ComboBox);
        }

        private void driverModel_ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) { return; }

            string text = driverModel_ComboBox.GetItemText(driverModel_ComboBox.Items[e.Index]);
            e.DrawBackground();
            using (SolidBrush br = new SolidBrush(e.ForeColor))
            { e.Graphics.DrawString(text, e.Font, br, e.Bounds); }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            { driver_toolTip.Show(text, driverModel_ComboBox, e.Bounds.Right, e.Bounds.Bottom); }
            e.DrawFocusRectangle();
        }

        private void driverModel_ComboBox_Enter(object sender, EventArgs e)
        {
            driverModel_ComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            driverModel_ComboBox.DrawItem += driverModel_ComboBox_DrawItem;
            driverModel_ComboBox.DropDownClosed += driverModel_ComboBox_DropDownClosed;
        }
    }
}
