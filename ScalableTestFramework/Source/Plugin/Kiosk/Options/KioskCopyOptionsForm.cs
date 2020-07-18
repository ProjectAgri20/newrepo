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
    public partial class KioskCopyOptionsForm : Form
    {
        /// <summary>
        /// Gets/Set the members of the<see cref="KioskCopyOptions"/> class
        /// </summary>
        public KioskCopyOptions KioskCopyOptions { get; set; }

        // <summary>
        /// Initializes a new instance of the <see cref="KioskCopyOptions"/> class.
        /// </summary>
        public KioskCopyOptionsForm()
        {
            InitializeComponent();
            KioskCopyOptions = new KioskCopyOptions();
        }

        /// <summary>
        /// CloudPrintOptionsForm constructor with CloudPrintOptions as parameter
        /// </summary>
        /// <param name="kioskCopyOptions"></param>
        public KioskCopyOptionsForm(KioskCopyOptions kioskCopyOptions)
        {            
            InitializeComponent();
            KioskCopyOptions = kioskCopyOptions;
         
            colormode_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskColorMode>().ToList();
            originalsize_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskOriginalSize>().ToList();
            nup_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskNUp>().ToList();
            originalorientation_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskOriginalOrientation>().ToList();
            originalduplex_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskDuplexSided>().ToList();
            outputduplex_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskDuplexSided>().ToList();
            GetResuceEnlargeDataSource();         
        }

        /// <summary>
        /// The OnLoad event of the form prebinding the values to controls
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {            
            pagecount_NumericUpDown.Value = KioskCopyOptions.PageCount;
            colormode_ComboBox.Text = KioskCopyOptions.ColorMode.GetDescription();
            originalsize_ComboBox.Text = KioskCopyOptions.OriginalSize.GetDescription();
            nup_ComboBox.Text = KioskCopyOptions.Nup.GetDescription();
            originalorientation_ComboBox.Text = KioskCopyOptions.OriginalOrientation.GetDescription();
            originalduplex_ComboBox.Text = KioskCopyOptions.DuplexOriginal.GetDescription();
            outputduplex_ComboBox.Text = KioskCopyOptions.DuplexOutput.GetDescription();
            reduceenlarge_ComgoBox.SelectedIndex = KioskCopyOptions.ReduceEnlargeIndex;
            base.OnLoad(e);
        }

        private void UpdateCopyOptions()
        {            
            KioskCopyOptions.PageCount = (int)pagecount_NumericUpDown.Value;
            KioskCopyOptions.ColorMode = EnumUtil.GetByDescription<KioskColorMode>(colormode_ComboBox.SelectedItem.ToString());
            KioskCopyOptions.OriginalSize = EnumUtil.GetByDescription<KioskOriginalSize>(originalsize_ComboBox.SelectedItem.ToString());
            KioskCopyOptions.Nup = EnumUtil.GetByDescription<KioskNUp>(nup_ComboBox.SelectedItem.ToString());
            KioskCopyOptions.OriginalOrientation = EnumUtil.GetByDescription<KioskOriginalOrientation>(originalorientation_ComboBox.SelectedItem.ToString());
            KioskCopyOptions.DuplexOriginal = EnumUtil.GetByDescription<KioskDuplexSided>(originalduplex_ComboBox.SelectedItem.ToString());
            KioskCopyOptions.DuplexOutput = EnumUtil.GetByDescription<KioskDuplexSided>(outputduplex_ComboBox.SelectedItem.ToString());            
            KioskCopyOptions.ReduceEnlargeIndex = reduceenlarge_ComgoBox.SelectedIndex;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            UpdateCopyOptions();
            DialogResult = DialogResult.OK;
        }

        private void GetResuceEnlargeDataSource()
        {
            KioskOriginalSize originalSize = EnumUtil.GetByDescription<KioskOriginalSize>(originalsize_ComboBox.SelectedItem.ToString());

            switch (originalSize)
            {
                case KioskOriginalSize.A3:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<KioskA3ReduceEnlarge>().ToList();
                    break;
                case KioskOriginalSize.A4:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<KioskA4ReduceEnlarge>().ToList();
                    break;
                case KioskOriginalSize.A5:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<KioskA5ReduceEnlarge>().ToList();
                    break;
                case KioskOriginalSize.B4:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<KioskB4ReduceEnlarge>().ToList();
                    break;
                case KioskOriginalSize.B5:
                    reduceenlarge_ComgoBox.DataSource = EnumUtil.GetDescriptions<KioskB5ReduceEnlarge>().ToList();
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
