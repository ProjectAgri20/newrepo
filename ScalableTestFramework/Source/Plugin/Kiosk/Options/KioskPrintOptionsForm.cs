using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.Kiosk.Options
{
    public partial class KioskPrintOptionsForm : Form
    {
        /// <summary>
        /// Gets/Set the members of the<see cref="KioskPrintOptions"/> class
        /// </summary>
        public KioskPrintOptions KioskPrintOptions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KioskPrintOptions"/> class.
        /// </summary>
        public KioskPrintOptionsForm()
        {
            InitializeComponent();
            KioskPrintOptions = new KioskPrintOptions();
        }

        /// <summary>
        /// CloudPrintOptionsForm constructor with CloudPrintOptions as parameter
        /// </summary>
        /// <param name="kioskPrintOptions"></param>
        public KioskPrintOptionsForm(KioskPrintOptions kioskPrintOptions)
        {         
            InitializeComponent();
            KioskPrintOptions = kioskPrintOptions;

            //combobox imtes            
            colormode_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskColorMode>().ToList();
            duplex_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskDuplexPrint>().ToList();
            papersource_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskOriginalSize>().ToList();
        }

        /// <summary>
        /// The OnLoad event of the form prebinding the values to controls
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {            
            copies_NumericUpDown.Value = KioskPrintOptions.PageCount;            
            colormode_ComboBox.Text = KioskPrintOptions.ColorMode.GetDescription();
            duplex_ComboBox.Text = KioskPrintOptions.Duplex.GetDescription();
            papersource_ComboBox.Text = KioskPrintOptions.PaperSource.GetDescription();
            autofit_CheckBox.Checked = KioskPrintOptions.AutoFit;

            base.OnLoad(e);
        }

        private void UpdatePrintOptions()
        {            
            KioskPrintOptions.PageCount = (int)copies_NumericUpDown.Value;            
            KioskPrintOptions.ColorMode = EnumUtil.GetByDescription<KioskColorMode>(colormode_ComboBox.SelectedItem.ToString());
            KioskPrintOptions.Duplex = EnumUtil.GetByDescription<KioskDuplexPrint>(duplex_ComboBox.SelectedItem.ToString());
            KioskPrintOptions.PaperSource = EnumUtil.GetByDescription<KioskOriginalSize>(papersource_ComboBox.SelectedItem.ToString());
            KioskPrintOptions.AutoFit = autofit_CheckBox.Checked;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            UpdatePrintOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
