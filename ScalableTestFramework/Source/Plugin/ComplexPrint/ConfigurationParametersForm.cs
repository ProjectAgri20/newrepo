using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.ComplexPrint
{
    public partial class ConfigurationParametersForm : Form
    {
        #region Constants

        /// <summary>
        /// Directory name for the documents.
        /// </summary>
        const string DIRECTORY_DOCUMENTS = "Documents";

        /// <summary>
        /// Common directory name
        /// </summary>
        const string COMMONPATH = "Common";

        /// <summary>
        /// Drivers location
        /// </summary>
        const string DIRECTORY_DRIVERS = "Drivers";

        /// <summary>
        /// Configuration file name
        /// </summary>
        const string CONFIGURATION_FILE = "CNP.xml";

        #endregion

        #region Local Variables

        /// <summary>
        /// Contains Product Family name
        /// </summary>
        ProductFamilies _productFamilies;

        /// <summary>
        /// Used to maintain the state on whether 'OK' button is clicked while closing the form
        /// </summary>
        bool _okButtonClicked = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the form
        /// </summary>
        /// <param name="productFamilies">Product Family name</param>
        public ConfigurationParametersForm(ProductFamilies productFamilies)
        {
            _productFamilies = productFamilies;
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Load the data based on the product family
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigurationParametersForm_Load(object sender, EventArgs e)
        {
            // Load default values on to the form.

            info_Label.Text = "You are trying to edit configuration parameters for '" + _productFamilies.ToString() + "' product family.";

            // Load the printer names based on the printer family
            Collection<string> productNames = CtcSettings.GetProducts(_productFamilies.ToString());

            productName_ComboBox.Items.Clear();

            if (productNames.Count != 0)
            {
                for (int i = 0; i < productNames.Count; i++)
                {
                    productName_ComboBox.Items.Add(productNames[i]);
                }
            }
            else
            {
                MessageBox.Show("There are no Products available for {0} Product Category in the database.\nPlease add the Product and try again.".FormatWith(_productFamilies.ToString()), "Product Loading Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            productName_ComboBox.SelectedIndex = 0;

            // Load the printer driver settings
            print_PrintDriverSelector.PrinterFamily = _productFamilies.ToString();
            print_PrintDriverSelector.PrinterName = productName_ComboBox.SelectedItem.ToString();

            sitemapVersionSelector.PrinterFamily = _productFamilies.ToString();
            sitemapVersionSelector.PrinterName = productName_ComboBox.SelectedItem.ToString();

            // Load the configuration data from CNP.xml on to the form
            LoadConfigurationData();
        }

        /// <summary>
        /// Update the documents path based on the product name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productName_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Load the documents directory
            string documentLocation = Path.Combine(CtcSettings.ConnectivityShare, _productFamilies.ToString());
            documentsPath_ComboBox.Tag = documentLocation;

            documentsPath_ComboBox.Items.Clear();

            if (Directory.Exists(documentLocation))
            {
                List<string> documentPath = Directory.EnumerateDirectories(documentLocation).ToList();

                if (documentPath.Count.Equals(0))
                {
                    MessageBox.Show("No folders exists in '{0}'".FormatWith(documentLocation), "Documents Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    foreach (string directory in documentPath)
                    {
                        if (Directory.Exists(directory + Path.DirectorySeparatorChar))
                        {
                            documentsPath_ComboBox.Items.Add(new DirectoryInfo(directory).Name);
                        }
                    }

                    // select the "common" directory as default
                    if (documentsPath_ComboBox.FindStringExact(COMMONPATH) != -1)
                    {
                        documentsPath_ComboBox.SelectedIndex = documentsPath_ComboBox.FindStringExact(COMMONPATH);
                    }
                }
            }
            else
            {
                MessageBox.Show("The Path '{0}' does not exist or you don't have permissions to this folder.".FormatWith(documentLocation), "Documents Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            print_PrintDriverSelector.PrinterName = productName_ComboBox.SelectedItem.ToString();
            sitemapVersionSelector.PrinterName = productName_ComboBox.SelectedItem.ToString();
        }

        /// <summary>
        /// Validate and save the data into the configuration file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Button_Click(object sender, EventArgs e)
        {
            // Save the data into the CNP.xml file

            // check to see if configuration file exists or not
            string configFile = CtcSettings.ConnectivityShare + Path.DirectorySeparatorChar + _productFamilies.ToString() + Path.DirectorySeparatorChar + CONFIGURATION_FILE;
            string documentPath = CtcSettings.ConnectivityShare + Path.DirectorySeparatorChar + _productFamilies.ToString() + Path.DirectorySeparatorChar + documentsPath_ComboBox.SelectedItem.ToString() + Path.DirectorySeparatorChar + DIRECTORY_DOCUMENTS + "\\CNP";

            // Validate the data
            if (!printer_ipAddressControl.IsValidIPAddress())
            {
                MessageBox.Show("Enter valid IP address", "Printer Address Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                printer_ipAddressControl.Focus();
                return;
            }

            if (!Directory.Exists(documentPath))
            {
                MessageBox.Show("Document folder: {0} doesn't exist.\n\nSelect a valid Document folder".FormatWith(documentPath), "Document folder Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                documentsPath_ComboBox.Focus();
                return;
            }

            try
            {
                XmlDocument doc = new XmlDocument();

                XmlNode xmlNode = doc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                doc.AppendChild(xmlNode);
                XmlNode rootNode = doc.CreateElement("Configuration");
                doc.AppendChild(rootNode);

                // Product Name
                XmlNode productNode = doc.CreateElement("ProductName");
                productNode.InnerText = productName_ComboBox.SelectedItem.ToString();
                rootNode.AppendChild(productNode);

                // IP Address
                XmlNode ipAddressNode = doc.CreateElement("IPv4Address");
                ipAddressNode.InnerText = printer_ipAddressControl.Text;
                rootNode.AppendChild(ipAddressNode);

                // Driver Path
                XmlNode driverPathNode = doc.CreateElement("DriverPath");
                driverPathNode.InnerText = print_PrintDriverSelector.DriverPackagePath;
                rootNode.AppendChild(driverPathNode);

                // Driver Model
                XmlNode driverModelNode = doc.CreateElement("DriverModel");
                driverModelNode.InnerText = print_PrintDriverSelector.DriverModel;
                rootNode.AppendChild(driverModelNode);

                // Documents Path
                XmlNode documentsNode = doc.CreateElement("FilesPath");
                documentsNode.InnerText = documentPath;
                rootNode.AppendChild(documentsNode);

                // Sitemaps
                XmlNode sitemapsNode = doc.CreateElement("Sitemaps");
                sitemapsNode.InnerText = sitemapVersionSelector.SitemapVersion;
                rootNode.AppendChild(sitemapsNode);

                doc.Save(CtcSettings.ConnectivityShare + Path.DirectorySeparatorChar + _productFamilies.ToString() + Path.DirectorySeparatorChar + CONFIGURATION_FILE);

                _okButtonClicked = true;

                // Close the form.
                this.Close();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error occurred while saving the data into configuration file, make sure no else opened the configuration file and try again.", ex);
            }

        }

        /// <summary>
        /// Taking the user confirmation before closing the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigurationParametersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_okButtonClicked)
            {
                if (DialogResult.Yes == MessageBox.Show("Do you want to close without saving the data?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the data from configuration file
        /// </summary>
        private void LoadConfigurationData()
        {
            // check to see if configuration file exists or not
            string configFile = CtcSettings.ConnectivityShare + Path.DirectorySeparatorChar + _productFamilies.ToString() + Path.DirectorySeparatorChar + CONFIGURATION_FILE;

            if (File.Exists(configFile))
            {
                // if the configuration file exists then load the data on to the form
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(configFile);

                    productName_ComboBox.SelectedIndex = productName_ComboBox.FindStringExact(doc.DocumentElement.SelectSingleNode("ProductName").InnerText);
                    printer_ipAddressControl.Text = doc.DocumentElement.SelectSingleNode("IPv4Address").InnerText;
                    string documentsPath = doc.DocumentElement.SelectSingleNode("FilesPath").InnerText;
                    string[] dirs = documentsPath.Split('\\');
                    documentsPath_ComboBox.SelectedIndex = documentsPath_ComboBox.FindStringExact(dirs[dirs.Length - 3]);
                    print_PrintDriverSelector.DriverPackagePath = doc.DocumentElement.SelectSingleNode("DriverPath").InnerText;
                    print_PrintDriverSelector.DriverModel = doc.DocumentElement.SelectSingleNode("DriverModel").InnerText;
                    sitemapVersionSelector.SitemapVersion = doc.DocumentElement.SelectSingleNode("Sitemaps").InnerText;
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error("Error occurred in loading configuration file", ex);
                }
            }
        }

        #endregion
    }
}