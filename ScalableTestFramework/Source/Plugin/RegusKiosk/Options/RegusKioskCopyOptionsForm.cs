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
    public partial class RegusKioskCopyOptionsForm : Form
    {
        /// <summary>
        /// Gets/Set the members of the<see cref="RegusKioskCopyOptions"/> class
        /// </summary>
        public RegusKioskCopyOptions RegusKioskCopyOptions { get; set; }

        // <summary>
        /// Initializes a new instance of the <see cref="RegusKioskCopyOptions"/> class.
        /// </summary>
        public RegusKioskCopyOptionsForm()
        {
            InitializeComponent();
            RegusKioskCopyOptions = new RegusKioskCopyOptions();
        }

        /// <summary>
        /// RegusKioskPrintOptionsForm constructor with RegusKioskPrintOptions as parameter
        /// </summary>
        /// <param name="kioskCopyOptions"></param>
        public RegusKioskCopyOptionsForm(RegusKioskCopyOptions regusKioskCopyOptions)
        {            
            InitializeComponent();
            RegusKioskCopyOptions = regusKioskCopyOptions;
         
            colormode_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskColorMode>().ToList();
            originalsize_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskOriginalSize>().ToList();
            nup_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskNUp>().ToList();
            originalorientation_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskOriginalOrientation>().ToList();
            originalduplex_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskDuplexSided>().ToList();
            outputduplex_ComboBox.DataSource = EnumUtil.GetDescriptions<RegusKioskDuplexSided>().ToList();
            GetResuceEnlargeDataSource();         
        }

        /// <summary>
        /// The OnLoad event of the form prebinding the values to controls
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {            
            pagecount_NumericUpDown.Value = RegusKioskCopyOptions.NumberOfCopies;
            colormode_ComboBox.Text = RegusKioskCopyOptions.ColorMode.GetDescription();
            originalsize_ComboBox.Text = RegusKioskCopyOptions.OriginalSize.GetDescription();
            nup_ComboBox.Text = RegusKioskCopyOptions.Nup.GetDescription();
            originalorientation_ComboBox.Text = RegusKioskCopyOptions.OriginalOrientation.GetDescription();
            originalduplex_ComboBox.Text = RegusKioskCopyOptions.DuplexOriginal.GetDescription();
            outputduplex_ComboBox.Text = RegusKioskCopyOptions.DuplexOutput.GetDescription();
            reduceenlarge_ComgoBox.Text = RegusKioskCopyOptions.ReduceEnlarge;
            base.OnLoad(e);
        }

        private void UpdateCopyOptions()
        {            
            RegusKioskCopyOptions.NumberOfCopies = (int)pagecount_NumericUpDown.Value;
            RegusKioskCopyOptions.ColorMode = EnumUtil.GetByDescription<RegusKioskColorMode>(colormode_ComboBox.SelectedItem.ToString());
            RegusKioskCopyOptions.OriginalSize = EnumUtil.GetByDescription<RegusKioskOriginalSize>(originalsize_ComboBox.SelectedItem.ToString());
            RegusKioskCopyOptions.Nup = EnumUtil.GetByDescription<RegusKioskNUp>(nup_ComboBox.SelectedItem.ToString());
            RegusKioskCopyOptions.OriginalOrientation = EnumUtil.GetByDescription<RegusKioskOriginalOrientation>(originalorientation_ComboBox.SelectedItem.ToString());
            RegusKioskCopyOptions.DuplexOriginal = EnumUtil.GetByDescription<RegusKioskDuplexSided>(originalduplex_ComboBox.SelectedItem.ToString());
            RegusKioskCopyOptions.DuplexOutput = EnumUtil.GetByDescription<RegusKioskDuplexSided>(outputduplex_ComboBox.SelectedItem.ToString());            
            RegusKioskCopyOptions.ReduceEnlarge = reduceenlarge_ComgoBox.SelectedItem.ToString();
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            UpdateCopyOptions();
            DialogResult = DialogResult.OK;
        }

        private void GetResuceEnlargeDataSource()
        {
            RegusKioskOriginalSize originalSize = EnumUtil.GetByDescription<RegusKioskOriginalSize>(originalsize_ComboBox.SelectedItem.ToString());

            switch (originalSize)
            {
                case RegusKioskOriginalSize.A3:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<RegusKioskA3ReduceEnlarge>().ToList();
                    break;
                case RegusKioskOriginalSize.A4:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<RegusKioskA4ReduceEnlarge>().ToList();
                    break;
                case RegusKioskOriginalSize.A5:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<RegusKioskA5ReduceEnlarge>().ToList();
                    break;
                case RegusKioskOriginalSize.B4:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<RegusKioskB4ReduceEnlarge>().ToList();
                    break;
                case RegusKioskOriginalSize.B5:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<RegusKioskB5ReduceEnlarge>().ToList();
                    break;
                case RegusKioskOriginalSize.Executive:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<RegusKioskExecutiveReduceEnlarge>().ToList();
                    break;
                case RegusKioskOriginalSize.Ledger:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<RegusKioskLedgerReduceEnlarge>().ToList();
                    break;
                case RegusKioskOriginalSize.Legal:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<RegusKioskLegalReduceEnlarge>().ToList();
                    break;
                case RegusKioskOriginalSize.Letter:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<RegusKioskLetterReduceEnlarge>().ToList();
                    break;
                case RegusKioskOriginalSize.Statement:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<RegusKioskStatementReduceEnlarge>().ToList();
                    break;
            }
        }

        private void originalsize_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetResuceEnlargeDataSource();
            reduceenlarge_ComgoBox.SelectedIndex = 0;
        }
    }
}
