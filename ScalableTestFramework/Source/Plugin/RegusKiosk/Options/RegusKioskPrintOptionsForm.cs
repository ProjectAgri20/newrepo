using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.ScalableTest.DeviceAutomation.LinkApps.Kiosk.RegusKiosk;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.RegusKiosk.Options
{
    public partial class RegusKioskPrintOptionsForm : Form
    {
        /// <summary>
        /// Gets/Set the members of the<see cref="KioskPrintOptions"/> class
        /// </summary>
        public RegusKioskPrintOptions RegusKioskPrintOptions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KioskPrintOptions"/> class.
        /// </summary>
        public RegusKioskPrintOptionsForm()
        {
            InitializeComponent();
            RegusKioskPrintOptions = new RegusKioskPrintOptions();
        }

        /// <summary>
        /// RegsKioskPrintOptionsForm constructor with RegusKioskPrintOptions as parameter
        /// </summary>
        /// <param name="kioskPrintOptions"></param>
        public RegusKioskPrintOptionsForm(RegusKioskPrintOptions regusKioskPrintOptions)
        {         
            InitializeComponent();
            RegusKioskPrintOptions = regusKioskPrintOptions;

            //combobox imtes            
            colormode_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskColorMode>().ToList();
            duplex_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskDuplexPrint>().ToList();
            papersource_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskOriginalSize>().ToList();
        }

        /// <summary>
        /// The OnLoad event of the form prebinding the values to controls
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {            
            copies_NumericUpDown.Value = RegusKioskPrintOptions.PageCount;            
            colormode_ComboBox.Text = RegusKioskPrintOptions.ColorMode.GetDescription();
            duplex_ComboBox.Text = RegusKioskPrintOptions.Duplex.GetDescription();
            papersource_ComboBox.Text = RegusKioskPrintOptions.PaperSource.GetDescription();
            autofit_CheckBox.Checked = RegusKioskPrintOptions.AutoFit;

            base.OnLoad(e);
        }

        private void UpdatePrintOptions()
        {
            RegusKioskPrintOptions.PageCount = (int)copies_NumericUpDown.Value;
            RegusKioskPrintOptions.ColorMode = EnumUtil.GetByDescription<RegusKioskColorMode>(colormode_ComboBox.SelectedItem.ToString());
            RegusKioskPrintOptions.Duplex = EnumUtil.GetByDescription<RegusKioskDuplexPrint>(duplex_ComboBox.SelectedItem.ToString());
            RegusKioskPrintOptions.PaperSource = EnumUtil.GetByDescription<RegusKioskOriginalSize>(papersource_ComboBox.SelectedItem.ToString());
            RegusKioskPrintOptions.AutoFit = autofit_CheckBox.Checked;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            UpdatePrintOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
