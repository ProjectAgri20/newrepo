using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Utility;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HP.ScalableTest.PluginSupport.JetAdvantageLink
{
    /// <summary>
    /// Options form for JetAdvantageLink Scan job
    /// </summary>
    public partial class LinkScanOptionsForm : Form
    {
        /// <summary>
        /// Gets/Set the members of the<see cref="LinkPrintOptions"/> class
        /// </summary>
        public LinkScanOptions LinkScanOption { get; set; }

        /// <summary>
        /// Using ScantoEmail or not
        /// </summary>
        public bool IsUsingEmail = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkPrintOptions"/> class.
        /// </summary>
        public LinkScanOptionsForm()
        {
            InitializeComponent();
            LinkScanOption = new LinkScanOptions();
        }

        /// <summary>
        /// CloudPrintOptionsForm constructor with CloudPrintOptions as parameter
        /// </summary>
        /// <param name="linkScanOptions"></param>
        public LinkScanOptionsForm(LinkScanOptions linkScanOptions)
        {
            InitializeComponent();
            LinkScanOption = linkScanOptions;
            originalSides_comboBox.DataSource = EnumUtil.GetDescriptions<LinkScanOriginalSides>().ToList();
            fileType_comboBox.DataSource = EnumUtil.GetDescriptions<LinkScanFileType>().ToList();
            resolution_comboBox.DataSource = EnumUtil.GetDescriptions<LinkScanResolution>().ToList();
            colorBlack_comboBox.DataSource = EnumUtil.GetDescriptions<LinkScanColorBlack>().ToList();
            originalSize_comboBox.DataSource = EnumUtil.GetDescriptions<LinkScanOriginalSize>().ToList();
            contentOrientation_comboBox.DataSource = EnumUtil.GetDescriptions<LinkScanContentOrientation>().ToList();            
        }
        
        /// <summary>
        /// The OnLoad event of the form prebinding the values to controls
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {           
            originalSides_comboBox.Text = LinkScanOption.OriginalSides.GetDescription();
            fileType_comboBox.Text = LinkScanOption.FileType.GetDescription();
            resolution_comboBox.Text = LinkScanOption.Resolution.GetDescription();
            colorBlack_comboBox.Text = LinkScanOption.ColorBlack.GetDescription();
            originalSize_comboBox.Text = LinkScanOption.OriginalSize.GetDescription();
            contentOrientation_comboBox.Text = LinkScanOption.ContentOrientation.GetDescription();

            originalSides_checkBox.Checked = LinkScanOption.UseOriginalSides;
            fileTypeandResolution_checkBox.Checked = LinkScanOption.UseFileTypeandResolution;
            colorBlack_checkBox.Checked = LinkScanOption.UseColorBlack;
            originalSize_checkBox.Checked = LinkScanOption.UseOriginalSize;
            contentOrientation_checkBox.Checked = LinkScanOption.UseContentOrientation;
                                   
            base.OnLoad(e);
        }

        private void UpdateScanOptions()
        {
            LinkScanOption.OriginalSides = EnumUtil.GetByDescription<LinkScanOriginalSides>(originalSides_comboBox.SelectedItem.ToString());
            LinkScanOption.FileType = EnumUtil.GetByDescription<LinkScanFileType>(fileType_comboBox.SelectedItem.ToString());
            LinkScanOption.Resolution = EnumUtil.GetByDescription<LinkScanResolution>(resolution_comboBox.SelectedItem.ToString());
            LinkScanOption.ColorBlack = EnumUtil.GetByDescription<LinkScanColorBlack>(colorBlack_comboBox.SelectedItem.ToString());
            LinkScanOption.OriginalSize = EnumUtil.GetByDescription<LinkScanOriginalSize>(originalSize_comboBox.SelectedItem.ToString());
            LinkScanOption.ContentOrientation = EnumUtil.GetByDescription<LinkScanContentOrientation>(contentOrientation_comboBox.SelectedItem.ToString());

            LinkScanOption.UseOriginalSides = originalSides_checkBox.Checked;
            LinkScanOption.UseFileTypeandResolution = fileTypeandResolution_checkBox.Checked;
            LinkScanOption.UseColorBlack = colorBlack_checkBox.Checked;
            LinkScanOption.UseOriginalSize = originalSize_checkBox.Checked;
            LinkScanOption.UseContentOrientation = contentOrientation_checkBox.Checked;
        }
        
        private void ok_button_Click(object sender, EventArgs e)
        {
            var results = fieldValidator.ValidateAll();
            if (results.All(n => n.Succeeded))
            {
                UpdateScanOptions();
                DialogResult = DialogResult.OK;
            }
        }

        private void originalSides_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (originalSides_checkBox.Checked)
            {
                originalSides_label.Enabled = true;
                originalSides_comboBox.Enabled = true;                
            }
            else
            {
                originalSides_label.Enabled = false;
                originalSides_comboBox.Enabled = false;
            }
        }

        private void fileTypeandResolution_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (fileTypeandResolution_checkBox.Checked)
            {                
                fileTypeandResolution_groupBox.Enabled = true;
            }
            else
            {
                fileTypeandResolution_groupBox.Enabled = false;
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

        private void originalSize_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (originalSize_checkBox.Checked)
            {
                originalSize_label.Enabled = true;
                originalSize_comboBox.Enabled = true;
            }
            else
            {
                originalSize_label.Enabled = false;
                originalSize_comboBox.Enabled = false;
            }
        }

        private void contentOrientation_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (contentOrientation_checkBox.Checked)
            {
                contentorientation_label.Enabled = true;
                contentOrientation_comboBox.Enabled = true;
            }
            else
            {
                contentorientation_label.Enabled = false;
                contentOrientation_comboBox.Enabled = false;
            }
        }
    }
}
