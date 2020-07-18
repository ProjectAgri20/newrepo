using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HP.ScalableTest.DeviceAutomation.LinkApps.Kiosk.RegusKiosk;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.RegusKiosk.Options
{
    public partial class RegusKioskScanOptionsForm : Form
    {
        /// <summary>
        /// Gets/Set the members of the<see cref="KioskPrintOptions"/> class
        /// </summary>
        public RegusKioskScanOptions RegusKioskScanOptions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegusKioskPrintOptions"/> class.
        /// </summary>
        public RegusKioskScanOptionsForm()
        {
            InitializeComponent();
            RegusKioskScanOptions = new RegusKioskScanOptions();
        }

        /// <summary>
        /// CloudPrintOptionsForm constructor with CloudPrintOptions as parameter
        /// </summary>
        /// <param name="kioskScanOptions"></param>
        public RegusKioskScanOptionsForm(RegusKioskScanOptions regusKioskScanOptions)
        {            
            InitializeComponent();
            RegusKioskScanOptions = regusKioskScanOptions;

            //combobox items            
            originalsize_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskOriginalSize>().ToList();
            originalorientation_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskOriginalOrientation>().ToList();
            duplex_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskDuplexSided>().ToList();
            imageRotation_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskImageRotation>().ToList();
            colormode_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskColorMode>().ToList();
            fileformat_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskFileFormat>().ToList();
            resolution_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskResolution>().ToList();

            if (regusKioskScanOptions.ScanDestination.GetDescription().Equals("USB"))
            {
                filename_Label.Text = "File Name";
            }
            else
            {
                filename_Label.Text = "To";
            }
        }

        /// <summary>
        /// The OnLoad event of the form prebinding the values to controls
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {            
            originalsize_ComboBox.Text = RegusKioskScanOptions.OriginalSize.GetDescription();
            originalorientation_ComboBox.Text = RegusKioskScanOptions.OriginalOrientation.GetDescription();
            duplex_ComboBox.Text = RegusKioskScanOptions.Duplex.GetDescription();
            imageRotation_ComboBox.Text = RegusKioskScanOptions.ImageRotation.GetDescription();
            colormode_ComboBox.Text = RegusKioskScanOptions.ColorMode.GetDescription();
            fileformat_ComboBox.Text = RegusKioskScanOptions.FileFormat.GetDescription();
            resolution_ComboBox.Text = RegusKioskScanOptions.Resolution.GetDescription();
            filename_TextBox.Text = RegusKioskScanOptions.StringField;

            base.OnLoad(e);
        }

        private void UpdateScanOptions()
        {
            RegusKioskScanOptions.OriginalSize = EnumUtil.GetByDescription<RegusKioskOriginalSize>(originalsize_ComboBox.SelectedItem.ToString());
            RegusKioskScanOptions.OriginalOrientation = EnumUtil.GetByDescription<RegusKioskOriginalOrientation>(originalorientation_ComboBox.SelectedItem.ToString());
            RegusKioskScanOptions.Duplex = EnumUtil.GetByDescription<RegusKioskDuplexSided>(duplex_ComboBox.SelectedItem.ToString());
            RegusKioskScanOptions.ImageRotation = EnumUtil.GetByDescription<RegusKioskImageRotation>(imageRotation_ComboBox.SelectedItem.ToString());
            RegusKioskScanOptions.ColorMode = EnumUtil.GetByDescription<RegusKioskColorMode>(colormode_ComboBox.SelectedItem.ToString());
            RegusKioskScanOptions.FileFormat = EnumUtil.GetByDescription<RegusKioskFileFormat>(fileformat_ComboBox.SelectedItem.ToString());
            RegusKioskScanOptions.Resolution = EnumUtil.GetByDescription <RegusKioskResolution>(resolution_ComboBox.SelectedItem.ToString());
            RegusKioskScanOptions.StringField = filename_TextBox.Text;            
        }
                
        private void ok_Button_Click(object sender, EventArgs e)
        {
            UpdateScanOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
