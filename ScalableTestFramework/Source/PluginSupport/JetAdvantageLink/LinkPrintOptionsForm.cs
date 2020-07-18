using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Utility;
using System;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.PluginSupport.JetAdvantageLink
{
    /// <summary>
    /// Options form for JetAdvantageLink Print job
    /// </summary>
    public partial class LinkPrintOptionsForm : Form
    {
        /// <summary>
        /// Gets/Set the members of the<see cref="LinkPrintOptions"/> class
        /// </summary>
        public LinkPrintOptions LinkPrintOption { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkPrintOptions"/> class.
        /// </summary>
        public LinkPrintOptionsForm()
        {
            InitializeComponent();
            LinkPrintOption = new LinkPrintOptions();
        }

        /// <summary>
        /// CloudPrintOptionsForm constructor with CloudPrintOptions as parameter
        /// </summary>
        /// <param name="linkPrintOptions"></param>
        public LinkPrintOptionsForm(LinkPrintOptions linkPrintOptions)
        {
            InitializeComponent();
            LinkPrintOption = linkPrintOptions;            
            outputSides_comboBox.DataSource = EnumUtil.GetDescriptions<LinkPrintOutputSides>().ToList();
            colorBlack_comboBox.DataSource = EnumUtil.GetDescriptions<LinkPrintColorBlack>().ToList();
            staple_comboBox.DataSource = EnumUtil.GetDescriptions<LinkPrintStaple>().ToList();
            paperSize_comboBox.DataSource = EnumUtil.GetDescriptions<LinkPrintPaperSize>().ToList();
            paperTray_comboBox.DataSource = EnumUtil.GetDescriptions<LinkPrintPaperTray>().ToList();
        }

        /// <summary>
        /// The OnLoad event of the form prebinding the values to controls
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            pageCount_checkBox.Checked = LinkPrintOption.UsePageCount;
            outputSides_checkBox.Checked = LinkPrintOption.UseOutputSides;
            colorBlack_checkBox.Checked = LinkPrintOption.UseColorBlack;
            staple_checkBox.Checked = LinkPrintOption.UseStaple;
            paperSelection_checkBox.Checked = LinkPrintOption.UsePaperSelection;

            pageCount_NumericUpDown.Value = LinkPrintOption.PageCount;
            outputSides_comboBox.Text = LinkPrintOption.OutputSides.GetDescription();
            colorBlack_comboBox.Text = LinkPrintOption.ColorBlack.GetDescription();
            staple_comboBox.Text = LinkPrintOption.Staple.GetDescription();
            paperSize_comboBox.Text = LinkPrintOption.PaperSize.GetDescription();
            paperTray_comboBox.Text = LinkPrintOption.PaperTray.GetDescription();

            base.OnLoad(e);
        }

        private void ok_button_Click(object sender, EventArgs e)
        {   
            UpdatePrintOptions();
            DialogResult = DialogResult.OK;            
        }

        private void UpdatePrintOptions()
        {
            LinkPrintOption.PageCount = (int)pageCount_NumericUpDown.Value;
            LinkPrintOption.OutputSides = EnumUtil.GetByDescription<LinkPrintOutputSides>(outputSides_comboBox.SelectedItem.ToString());
            LinkPrintOption.ColorBlack = EnumUtil.GetByDescription<LinkPrintColorBlack>(colorBlack_comboBox.SelectedItem.ToString());
            LinkPrintOption.Staple = EnumUtil.GetByDescription<LinkPrintStaple>(staple_comboBox.SelectedItem.ToString());
            LinkPrintOption.PaperSize = EnumUtil.GetByDescription<LinkPrintPaperSize>(paperSize_comboBox.SelectedItem.ToString());
            LinkPrintOption.PaperTray = EnumUtil.GetByDescription<LinkPrintPaperTray>(paperTray_comboBox.SelectedItem.ToString());

            LinkPrintOption.UsePageCount = pageCount_checkBox.Checked;
            LinkPrintOption.UseOutputSides = outputSides_checkBox.Checked;
            LinkPrintOption.UseColorBlack = colorBlack_checkBox.Checked;
            LinkPrintOption.UseStaple = staple_checkBox.Checked;
            LinkPrintOption.UsePaperSelection = paperSelection_checkBox.Checked;
        }

        private void pageCount_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (pageCount_checkBox.Checked)
            {
                pageCount_Label.Enabled = true;
                pageCount_NumericUpDown.Enabled = true;
            }
            else
            {
                pageCount_Label.Enabled = false;
                pageCount_NumericUpDown.Enabled = false;
            }
        }

        private void outputSides_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (outputSides_checkBox.Checked)
            {
                outputSides_label.Enabled = true;
                outputSides_comboBox.Enabled = true;
            }
            else
            {
                outputSides_label.Enabled = false;
                outputSides_comboBox.Enabled = false;
            }
        }

        private void colorBlack_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (colorBlack_checkBox.Checked)
            {
                colorBlack_label.Enabled = true;
                colorBlack_comboBox.Enabled = true;
            }
            else
            {
                colorBlack_label.Enabled = false;
                colorBlack_comboBox.Enabled = false;
            }
        }

        private void staple_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (staple_checkBox.Checked)
            {
                staple_label.Enabled = true;
                staple_comboBox.Enabled = true;
            }
            else
            {
                staple_label.Enabled = false;
                staple_comboBox.Enabled = false;
            }
        }

        private void paperSelection_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (paperSelection_checkBox.Checked)
            {
                paperSelection_groupBox.Enabled = true;
            }
            else
            {
                paperSelection_groupBox.Enabled = false;
            }
        }
    }
}
