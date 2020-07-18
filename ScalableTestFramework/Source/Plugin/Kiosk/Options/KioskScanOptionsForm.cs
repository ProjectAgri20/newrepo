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
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.Kiosk.Options
{
    public partial class KioskScanOptionsForm : Form
    {
        /// <summary>
        /// Gets/Set the members of the<see cref="KioskPrintOptions"/> class
        /// </summary>
        public KioskScanOptions KioskScanOptions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KioskPrintOptions"/> class.
        /// </summary>
        public KioskScanOptionsForm()
        {
            InitializeComponent();
            KioskScanOptions = new KioskScanOptions();
        }

        /// <summary>
        /// CloudPrintOptionsForm constructor with CloudPrintOptions as parameter
        /// </summary>
        /// <param name="kioskScanOptions"></param>
        public KioskScanOptionsForm(KioskScanOptions kioskScanOptions)
        {            
            InitializeComponent();
            KioskScanOptions = kioskScanOptions;

            //combobox items            
            originalsize_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskOriginalSize>().ToList();
            originalorientation_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskOriginalOrientation>().ToList();
            duplex_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskDuplexSided>().ToList();
            colormode_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskColorMode>().ToList();
            fileformat_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskFileFormat>().ToList();
            resolution_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskResolution>().ToList();

            if (kioskScanOptions.ScanDestination.GetDescription().Equals("USB"))
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
            originalsize_ComboBox.Text = KioskScanOptions.OriginalSize.GetDescription();
            originalorientation_ComboBox.Text = KioskScanOptions.OriginalOrientation.GetDescription();
            duplex_ComboBox.Text = KioskScanOptions.Duplex.GetDescription();
            colormode_ComboBox.Text = KioskScanOptions.ColorMode.GetDescription();
            fileformat_ComboBox.Text = KioskScanOptions.FileFormat.GetDescription();
            resolution_ComboBox.Text = KioskScanOptions.Resolution.GetDescription();
            filename_TextBox.Text = KioskScanOptions.StringField;

            base.OnLoad(e);
        }

        private void UpdateScanOptions()
        {                
            KioskScanOptions.OriginalSize = EnumUtil.GetByDescription<KioskOriginalSize>(originalsize_ComboBox.SelectedItem.ToString());
            KioskScanOptions.OriginalOrientation = EnumUtil.GetByDescription<KioskOriginalOrientation>(originalorientation_ComboBox.SelectedItem.ToString());
            KioskScanOptions.Duplex = EnumUtil.GetByDescription<KioskDuplexSided>(duplex_ComboBox.SelectedItem.ToString());
            KioskScanOptions.ColorMode = EnumUtil.GetByDescription<KioskColorMode>(colormode_ComboBox.SelectedItem.ToString());
            KioskScanOptions.FileFormat = EnumUtil.GetByDescription<KioskFileFormat>(fileformat_ComboBox.SelectedItem.ToString());
            KioskScanOptions.Resolution = EnumUtil.GetByDescription < KioskResolution>(resolution_ComboBox.SelectedItem.ToString());
            KioskScanOptions.StringField = filename_TextBox.Text;            
        }
                
        private void ok_Button_Click(object sender, EventArgs e)
        {
            UpdateScanOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
