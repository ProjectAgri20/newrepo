using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using System.Collections.Generic;

namespace HP.ScalableTest.PluginSupport.Connectivity.UI
{
    /// <summary>
    /// Control for displaying the sitemap details such as sitemap location and version.
    /// </summary>
    public partial class SitemapVersionSelector : UserControl
    {

        #region Local Variables

        private string _productCategory;
        private string _productName;
        string _sitemapPath = string.Empty;

        /// <summary>
        /// Occurs when a property value is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor
        /// <summary>
        /// Creates an instance of SitemapVersionSelector.
        /// </summary>
        public SitemapVersionSelector()
        {
            InitializeComponent();

            
        }

        #endregion

        #region Public Methods

        /// <summary>
		/// Validating the control elements
		/// </summary>
        /// <returns></returns>
		public ValidationResult ValidateControls()
        {
            bool status = true;
        
            foreach (ValidationResult result in sitemap_FieldValidator.ValidateAll())
            {
                status &= result.Succeeded;
            }

            return new ValidationResult(status);
        }

        public IEnumerable<ValidationResult> ValidateControl()
        {
            return sitemap_FieldValidator.ValidateAll();
        }

        /// <summary>
        /// Loads the UI, call this after initialising the plugin
        /// </summary>
        public void Initialize()
        {
            //if (!DesignMode)
            //{
            //     //folderBrowserDialog1.SelectedPath = SitemapBaseLocation;
                
            //}

            PrinterFamily = ProductFamilies.InkJet.ToString();

            sitemap_FieldValidator.RequireValue(sitemapLocation_TextBox, sitemapLocation_Label);
            sitemap_FieldValidator.RequireCustom(sitemapLocation_TextBox, () => (DesignMode ? true : sitemapLocation_TextBox.Text.Contains(SitemapBaseLocation)) && Directory.Exists(sitemapLocation_TextBox.Text), $"The path does not exist or Sitemaps should be in {SitemapBaseLocation}.");
            sitemap_FieldValidator.RequireValue(sitemapVersion_ComboBox, sitemapVersion_Label);

            sitemap_FieldValidator.SetIconAlignment(sitemapLocation_TextBox, ErrorIconAlignment.MiddleRight);
            sitemap_FieldValidator.SetIconAlignment(sitemapVersion_ComboBox, ErrorIconAlignment.MiddleRight);

        }

        #endregion

        #region Public Properties

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
                    PropertyChanged += SitemapVersionSelector_PropertyChanged;

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
                PropertyChanged += SitemapVersionSelector_PropertyChanged;

                // Fire the event only if both the ProductType & ProducName are set
                if (!string.IsNullOrEmpty(PrinterName) && !string.IsNullOrEmpty(PrinterFamily))
                {
                    OnPropertyChanged(PrinterFamily);
                }
            }
        }

        /// <summary>
        /// Gets or sets the sitemap version
        /// </summary>
        public string SitemapVersion
        {
            get
            {
                if (!DesignMode)
                {
                    return sitemapVersion_ComboBox?.SelectedItem?.ToString();
                }
                return string.Empty;
            }
            set
            {
                sitemapVersion_ComboBox.SelectedItem = value;
            }
        }

        public string SitemapPath
        {
            get { return !DesignMode ? sitemapLocation_TextBox.Text : string.Empty; }
            set
            {
                if (DesignMode || string.IsNullOrEmpty(value)) return;
                sitemapLocation_TextBox.Text = Directory.Exists(value) ? value : string.Empty;
            }
            //get { return _sitemapPath; }
            //set { _sitemapPath = sitemapLocation_TextBox.Text = value; }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the sitemap base location path
        /// </summary>
        private static string SitemapBaseLocation
        {
            get { return CtcSettings.EwsSiteMapLocation; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event Handler for property change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SitemapVersionSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!DesignMode)
            {
                if (e.PropertyName.Equals("SiteMapVersion"))
                {
                    sitemapVersion_ComboBox.SelectedItem = SitemapVersion;
                }
                else if(!e.PropertyName.Equals("Sitemap Location"))
                {
                    sitemapLocation_TextBox.Text = Path.Combine(SitemapBaseLocation, PrinterFamily, PrinterName);
                }
            }
        }        

        private void sitemapLocation_TextBox_TextChanged(object sender, EventArgs e)
        {
           // SitemapPath = sitemapLocation_TextBox.Text;
            LoadVersions();

            OnPropertyChanged(new PropertyChangedEventArgs("Sitemap Location"));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the version numbers into the version drop down box. This will discard the "Version " string.
        /// eg: 1.0, 1.1 etc...
        /// </summary>        
        private void LoadVersions()
        {
            string sitemapPath = sitemapLocation_TextBox.Text;

            sitemapVersion_ComboBox.Items.Clear();

            // Check to see if the directory exists
            if (Directory.Exists(sitemapPath))
            {
                // walk thru the version directories and load only the version numbers                
                foreach (string directory in Directory.GetDirectories(sitemapPath))
                {
                    DirectoryInfo directoryName = new DirectoryInfo(directory);
                    if (!directoryName.Name.EqualsIgnoreCase(".svn"))
                    {
                        sitemapVersion_ComboBox.Items.Add(directoryName.Name);
                    }
                }

                if (sitemapVersion_ComboBox.Items.Count > 0)
                {
                    sitemapVersion_ComboBox.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Occurs on property changed
        /// </summary>
        /// <param name="e">PropertyChangedEventArgs</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
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
        private void sitemapVersion_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(SitemapVersion));
        }

		#endregion

		private void button1_Click(object sender, EventArgs e)
		{
			if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
                SitemapPath = folderBrowserDialog1.SelectedPath;
			}
		}
    }
}
