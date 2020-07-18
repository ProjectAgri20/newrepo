using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace HP.ScalableTest.PluginSupport.Scan
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class ScanOptionsForm : Form
    {

        /// <summary>
        /// Gets/Set the members of the<see cref="ScanOptions"/> class
        /// </summary>
        public ScanOptions ScanOption { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ScanOptions"/> class.
        /// </summary>
        public ScanOptionsForm()
        {
            InitializeComponent();
            ScanOption = new ScanOptions();

        }

        /// <summary>
        /// ScanOptionsForm constructor with scanOptions as parameter
        /// </summary>
        /// <param name="scanOptions"></param>
        public ScanOptionsForm(ScanOptions scanOptions)
        {
            InitializeComponent();
            ScanOption = scanOptions;
            fieldValidator.RequireValue(email_TextBox, "Email Id", email_RadioButton);
            fieldValidator.SetIconAlignment(email_TextBox, ErrorIconAlignment.MiddleLeft);
            resolutionType_ComboBox.DataSource = EnumUtil.GetDescriptions<ResolutionType>().Where(x => !x.Contains(ResolutionType.Fine.ToString()) && !x.Contains(ResolutionType.Standard.ToString()) && !x.Contains(ResolutionType.SuperFine.ToString())).ToList();
            fileType_ComboBox.DataSource = EnumUtil.GetDescriptions<FileType>().ToList();
            originalSides_ComboBox.DataSource = EnumUtil.GetDescriptions<OriginalSides>().ToList();
            color_ComboBox.DataSource = EnumUtil.GetDescriptions<ColorType>().ToList();
            originalSize_ComboBox.DataSource = EnumUtil.GetDescriptions<OriginalSize>().ToList();
            contentOrientation_ComboBox.DataSource = EnumUtil.GetDescriptions<ContentOrientation>().ToList();
            optimizeTextPic_ComboBox.DataSource = EnumUtil.GetDescriptions<OptimizeTextPic>().ToList();
            croppingOptions_ComboBox.DataSource = EnumUtil.GetDescriptions<Cropping>().ToList();
            blankPageSupress_ComboBox.DataSource = EnumUtil.GetDescriptions<BlankPageSupress>().ToList();
            notification_ComboBox.DataSource = EnumUtil.GetDescriptions<NotifyCondition>().ToList();
            scanMode_ComboBox.DataSource = EnumUtil.GetDescriptions<ScanMode>().ToList();
            paperSize_ComboBox.DataSource = EnumUtil.GetDescriptions<PaperSelectionPaperSize>().ToList();
            paperTray_ComboBox.DataSource = EnumUtil.GetDescriptions<PaperSelectionPaperTray>().ToList();
            paperType_ComboBox.DataSource = EnumUtil.GetDescriptions<PaperSelectionPaperType>().ToList();
        }

        /// <summary>
        /// The OnLoad event of the form prebinding the values to controls
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (ScanOption.ScanJobType == "ScanToEmail")
            {
                signEncrypt_GroupBox.Enabled = true;
                LoadSignOrEncrypt();
            }
            if (ScanOption.ScanJobType == "ScanToFax")
            {
                resolutionType_ComboBox.DataSource = EnumUtil.GetDescriptions<ResolutionType>().Where(x => x.Contains(ResolutionType.None.ToString()) || x.Contains(ResolutionType.Fine.ToString()) || x.Contains(ResolutionType.Standard.ToString()) || x.Contains(ResolutionType.SuperFine.ToString())).ToList();
                fileType_ComboBox.Enabled = false;
                color_ComboBox.Enabled = false;
                croppingOptions_ComboBox.Enabled = false;
                createMultifile_CheckBox.Enabled = false;
                eraseEdges_CheckBox.Enabled = false;
            }
            if (ScanOption.ScanJobType == "ScanToJobStorage")
            {
                scanMode_GroupBox.Enabled = true;
                sides_CheckBox.Enabled = true;
                sides_GroupBox.Enabled = false;
                reduceEnlarge_GroupBox.Enabled = true;
                paperSelection_GroupBox.Enabled = true;
                pagesPerSheet_CheckBox.Enabled = true;
                pagesPerSheet_GroupBox.Enabled = false;
                bookletFormat_GroupBox.Enabled = true;
                edgeToEdge_CheckBox.Enabled = true;
                collate_CheckBox.Enabled = true;
                resolutionType_ComboBox.Enabled = false;
                fileType_ComboBox.Enabled = false;
                originalSides_ComboBox.Enabled = false;
                color_ComboBox.Enabled = false;
                croppingOptions_ComboBox.Enabled = false;
                blankPageSupress_ComboBox.Enabled = false;
                createMultifile_CheckBox.Enabled = false;
                notification_ComboBox.Enabled = false;
                signEncrypt_GroupBox.Enabled = false;
                scanMode_ComboBox.Text = ScanOption.ScanModes.GetDescription();
                bookletFormat_CheckBox.Checked = ScanOption.BookLetFormat;
                borderonPage_CheckBox.Checked = ScanOption.BorderOnEachPage;
                collate_CheckBox.Checked = ScanOption.Collate;
                edgeToEdge_CheckBox.Checked = ScanOption.EdgeToEdge;
                if (ScanOption.ReduceEnlargeOptions)
                {
                    reduceEnlarge_Combobox.Text = "Automatic";
                }
                else
                {
                    reduceEnlarge_Combobox.Text = "Manual";
                }
                includeThumNail_CheckBox.Checked = ScanOption.IncludeThumbNail == true ? true : false;
                //OptimizeTextPic_combox.Text = CopyOptions.OptimizeTextPicOptions;
                includeMargin_checkBox.Checked = ScanOption.IncludeMargin;
                zoomSize_TextBox.Text = Convert.ToString(ScanOption.ZoomSize);
                EnableDisableAutomaticOptions();
                EnableDisableManualOption();
                SetSidePreviousSelection();
                LoadPagesPerSheet();
                LoadPaperSelection();
            }
            resolutionType_ComboBox.Text = ScanOption.ResolutionType.GetDescription();
            fileType_ComboBox.Text = ScanOption.FileType.GetDescription();
            originalSides_ComboBox.Text = ScanOption.OriginalSides.GetDescription();

            if (originalSides_ComboBox.SelectedIndex.Equals(originalSides_ComboBox.FindStringExact(ScanOption.OriginalSides.GetDescription())))
            {
                if (ScanOption.PageFlipup)
                {
                    pagesFlipup_CheckBox.Enabled = true;
                    pagesFlipup_CheckBox.Checked = true;
                }
            }//removed else part as it was disabling the pagefilpup checkbox onloading existing setting even if not checked the pageflipup

            color_ComboBox.Text = ScanOption.Color.GetDescription();
            originalSize_ComboBox.Text = ScanOption.OriginalSize.GetDescription();
            contentOrientation_ComboBox.Text = ScanOption.ContentOrientationOption.GetDescription();
            optimizeTextPic_ComboBox.Text = ScanOption.OptimizeTextorPic.GetDescription();
            croppingOptions_ComboBox.Text = ScanOption.Cropping.GetDescription();
            blankPageSupress_ComboBox.Text = ScanOption.BlankPageSupressoption.GetDescription();
            notification_ComboBox.Text = ScanOption.notificationCondition.GetDescription();
            if (notification_ComboBox.SelectedIndex == 1 || notification_ComboBox.SelectedIndex == 2)
            {
                notification_GroupBox.Enabled = true;
            }
            else
            {
                notification_GroupBox.Enabled = false;
                email_TextBox.Text = "";
            }
            if (ScanOption.PrintorEmailNotificationMethod == "Email")
            {
                email_RadioButton.Checked = true;
                email_TextBox.Text = ScanOption.EmailNotificationText;
                includeThumNail_CheckBox.Checked = ScanOption.IncludeThumbNail ? true : false;
            }
            else
            {
                print_RadioButton.Checked = true;
                includeThumNail_CheckBox.Checked = ScanOption.IncludeThumbNail ? true : false;
            }
            if (ScanOption.CreateMultiFile)
            {
                createMultifile_CheckBox.Checked = true;
                maxpagePerFile_TextBox.Text = ScanOption.MaxPageperFile;
            }
            else
            {
                createMultifile_CheckBox.Checked = false;
                maxpagePerFile_TextBox.Text = "";
            }

            LoadEraseEdges();
            LoadImageAdjustments();

            base.OnLoad(e);
        }

        private void LoadSignOrEncrypt()
        {
            if (signEncrypt_GroupBox.Visible)
            {
                if (ScanOption.SignOrEncrypt == 0)
                {
                    signing_CheckBox.Checked = true;
                    encrypt_CheckBox.Checked = false;
                }
                if (ScanOption.SignOrEncrypt == 1)
                {
                    encrypt_CheckBox.Checked = true;
                    signing_CheckBox.Checked = false;
                }
                if (ScanOption.SignOrEncrypt == 2)
                {
                    signing_CheckBox.Checked = true;
                    encrypt_CheckBox.Checked = true;
                }
                if (ScanOption.SignOrEncrypt == -1)
                {
                    signing_CheckBox.Checked = false;
                    encrypt_CheckBox.Checked = false;
                }
            }
        }

        private void LoadEraseEdges()
        {
            if (ScanOption.SetEraseEdges)
            {
                eraseEdges_CheckBox.Checked = true;
                eraseEdges_GroupBox.Enabled = true;
            }
            else
            {
                eraseEdges_GroupBox.Enabled = false;
                eraseEdges_CheckBox.Checked = false;
            }
            if (ScanOption.ApplySameWidth)
            {
                applySamewidth_CheckBox.Checked = true;
                topEdge_TextBox.Text = ScanOption.EraseEdgesValue.AllEdges;
            }
            else
            {
                topEdge_TextBox.Text = ScanOption.EraseEdgesValue.FrontTop;
            }

            if (ScanOption.MirrorFrontSide)
            {
                backsidemirror_checkBox.Checked = true;
            }
            if (ScanOption.UseInches)
            {
                useinches_checkBox.Checked = true;
            }

            if (String.IsNullOrEmpty(ScanOption.EraseEdgesValue.FrontTop))
                return;

            bottomEdge_TextBox.Text = ScanOption.EraseEdgesValue.FrontBottom;
            rightEdge_TextBox.Text = ScanOption.EraseEdgesValue.FrontRight;
            leftEdge_TextBox.Text = ScanOption.EraseEdgesValue.FrontLeft;

            topedgeback_textBox.Text = ScanOption.EraseEdgesValue.BackTop;
            bottomedgeback_textBox.Text = ScanOption.EraseEdgesValue.BackBottom;
            rightedgeback_textBox.Text = ScanOption.EraseEdgesValue.BackRight;
            leftedgeback_textBox.Text = ScanOption.EraseEdgesValue.BackLeft;
        }
        private void LoadImageAdjustments()
        {
            if (ScanOption.SetImageAdjustment)
            {
                imageAdjustment_CheckBox.Checked = ScanOption.SetImageAdjustment = true ? true : false;
                sharpness_TrackBar.Value = ScanOption.ImageAdjustSharpness;
                darkness_TrackBar.Value = ScanOption.ImageAdjustDarkness;
                contrast_TrackBar.Value = ScanOption.ImageAdjustContrast;
                backGroundCleanup_TrackBar.Value = ScanOption.ImageAdjustbackgroundCleanup;
                automaticTone_CheckBox.Checked = ScanOption.AutomaticTone == true ? true : false;
            }
        }
        private void ok_Button_Click(object sender, EventArgs e)
        {

            var results = fieldValidator.ValidateAll();
            if (results.All(n => n.Succeeded))
            {
                UpdateScanOptions();
                DialogResult = DialogResult.OK;
            }
            else
            {
                var messages = results.Where(n => !n.Succeeded).Select(n => n.Message);
                MessageBox.Show(string.Join("\n", messages), "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateScanOptions()
        {
            ScanOption.FileType = EnumUtil.GetByDescription<FileType>(fileType_ComboBox.SelectedItem.ToString());
            ScanOption.ResolutionType = EnumUtil.GetByDescription<ResolutionType>(resolutionType_ComboBox.SelectedItem.ToString());
            ScanOption.OriginalSides = EnumUtil.GetByDescription<OriginalSides>(originalSides_ComboBox.SelectedItem.ToString());
            if (pagesFlipup_CheckBox.Checked)
            {
                ScanOption.PageFlipup = true;
            }
            else
            {
                ScanOption.PageFlipup = false;
            }
            ScanOption.Color = EnumUtil.GetByDescription<ColorType>(color_ComboBox.SelectedItem.ToString());
            ScanOption.OriginalSize = EnumUtil.GetByDescription<OriginalSize>(originalSize_ComboBox.SelectedItem.ToString());
            ScanOption.ContentOrientationOption = EnumUtil.GetByDescription<ContentOrientation>(contentOrientation_ComboBox.SelectedItem.ToString());
            ScanOption.OptimizeTextorPic = EnumUtil.GetByDescription<OptimizeTextPic>(optimizeTextPic_ComboBox.SelectedItem.ToString());
            ScanOption.Cropping = EnumUtil.GetByDescription<Cropping>(croppingOptions_ComboBox.SelectedItem.ToString());
            ScanOption.BlankPageSupressoption = EnumUtil.GetByDescription<BlankPageSupress>(blankPageSupress_ComboBox.SelectedItem.ToString());
            ScanOption.notificationCondition = EnumUtil.GetByDescription<NotifyCondition>(notification_ComboBox.SelectedItem.ToString());
            ScanOption.CreateMultiFile = createMultifile_CheckBox.Checked ? true : false;
            if (ScanOption.CreateMultiFile)
            {
                ScanOption.MaxPageperFile = maxpagePerFile_TextBox.Text;
            }
            else
            {
                ScanOption.MaxPageperFile = "";
            }
            if (email_RadioButton.Checked)
            {
                ScanOption.PrintorEmailNotificationMethod = "Email";
                ScanOption.EmailNotificationText = email_TextBox.Text;
            }
            else
            {
                ScanOption.PrintorEmailNotificationMethod = "Print";

            }
            ScanOption.IncludeThumbNail = includeThumNail_CheckBox.Checked ? true : false;
            if (eraseEdges_CheckBox.Checked)
            {
                PopulateEraseEdgesData();
                ScanOption.SetEraseEdges = true;
            }
            else
            {
                ScanOption.SetEraseEdges = false;
            }

            if (imageAdjustment_CheckBox.Checked)
            {
                ScanOption.SetImageAdjustment = true;
                ScanOption.ImageAdjustSharpness = sharpness_TrackBar.Value;
                ScanOption.ImageAdjustDarkness = darkness_TrackBar.Value;
                ScanOption.ImageAdjustContrast = contrast_TrackBar.Value;
                ScanOption.ImageAdjustbackgroundCleanup = backGroundCleanup_TrackBar.Value;
                ScanOption.AutomaticTone = automaticTone_CheckBox.Checked;
            }
            else
            {
                ScanOption.SetImageAdjustment = false;
            }
            PopulateSignOrEncryptData();
            ScanOption.ScanModes = EnumUtil.GetByDescription<ScanMode>(scanMode_ComboBox.SelectedItem.ToString());
            ScanOption.BookLetFormat = bookletFormat_CheckBox.Checked;
            ScanOption.BorderOnEachPage = borderonPage_CheckBox.Checked;
            ScanOption.Collate = collate_CheckBox.Checked;
            ScanOption.EdgeToEdge = edgeToEdge_CheckBox.Checked;
            if (reduceEnlarge_Combobox.Text == "Automatic")
            {
                ScanOption.ReduceEnlargeOptions = true;
            }
            else
            {
                ScanOption.ReduceEnlargeOptions = false;
            }
            ScanOption.IncludeMargin = includeMargin_checkBox.Checked;
            ScanOption.ZoomSize = int.Parse(string.IsNullOrEmpty(zoomSize_TextBox.Text) ? "0" : zoomSize_TextBox.Text);

            if (sides_CheckBox.Checked)
            {
                ScanOption.SetSides = true;
                ScanOption.OriginalOneSided = originalOneSide_RadioButton.Checked;
                ScanOption.OutputOneSided = outputOneSided_RadioButton.Checked;
                ScanOption.OriginalPageflip = originalPageFlip_CheckBox.Checked ? true : false;
                ScanOption.OutputPageflip = outputPageFlip_CheckBox.Checked ? true : false;
            }
            else
            {
                ScanOption.SetSides = false;
            }

            ScanOption.BookLetFormat = bookletFormat_CheckBox.Checked;
            ScanOption.BorderOnEachPage = borderonPage_CheckBox.Checked;
            ScanOption.PaperSelectionPaperSize = EnumUtil.GetByDescription<PaperSelectionPaperSize>(paperSize_ComboBox.SelectedItem.ToString());
            ScanOption.PaperSelectionPaperType = EnumUtil.GetByDescription<PaperSelectionPaperType>(paperType_ComboBox.SelectedItem.ToString());
            ScanOption.PaperSelectionPaperTray = EnumUtil.GetByDescription<PaperSelectionPaperTray>(paperTray_ComboBox.SelectedItem.ToString());

            if (pagesPerSheet_CheckBox.Checked)
            {
                ScanOption.SetPagesPerSheet = true;
                PopulatePagesPerSheet();
            }
            else
            {
                ScanOption.SetPagesPerSheet = false;
            }
        }

        private void PopulateSignOrEncryptData()
        {
            if (signEncrypt_GroupBox.Visible)
            {
                if (signing_CheckBox.Checked)
                {
                    ScanOption.SignOrEncrypt = 0;
                }
                if (encrypt_CheckBox.Checked)
                {
                    ScanOption.SignOrEncrypt = 1;
                }
                if (signing_CheckBox.Checked && encrypt_CheckBox.Checked)
                {
                    ScanOption.SignOrEncrypt = 2;
                }

                if (signing_CheckBox.Checked == false && encrypt_CheckBox.Checked == false)
                {
                    ScanOption.SignOrEncrypt = -1;
                }
            }
        }
        private void PopulateEraseEdgesData()
        {
            bool hasValue = false;
            foreach (TextBox txt in backside_groupBox.Controls.OfType<TextBox>())
            {
                if (!String.IsNullOrEmpty(txt.Text))
                {
                    hasValue = true;
                    break;
                }
            }

            foreach (TextBox txt in frontSide_GroupBox.Controls.OfType<TextBox>())
            {
                if (!String.IsNullOrEmpty(txt.Text))
                {
                    hasValue = true;
                    break;
                }
            }

            if (hasValue)
            {
                ScanOption.EraseEdgesValue.FrontTop = topEdge_TextBox.Text;
                ScanOption.EraseEdgesValue.FrontBottom = bottomEdge_TextBox.Text;
                ScanOption.EraseEdgesValue.FrontLeft = leftEdge_TextBox.Text;
                ScanOption.EraseEdgesValue.FrontRight = rightEdge_TextBox.Text;

                ScanOption.EraseEdgesValue.BackTop = topedgeback_textBox.Text;
                ScanOption.EraseEdgesValue.BackBottom = bottomedgeback_textBox.Text;
                ScanOption.EraseEdgesValue.BackLeft = leftedgeback_textBox.Text;
                ScanOption.EraseEdgesValue.BackRight = rightedgeback_textBox.Text;
                ScanOption.ApplySameWidth = applySamewidth_CheckBox.Checked;
                if (applySamewidth_CheckBox.Checked)
                {
                    ScanOption.EraseEdgesValue.AllEdges = topEdge_TextBox.Text;
                    ScanOption.EraseEdgesValue.FrontTop = "0.00";
                }
                else
                {
                    ScanOption.EraseEdgesValue.AllEdges = string.Empty;
                    ScanOption.EraseEdgesValue.FrontTop = topEdge_TextBox.Text;
                }
                ScanOption.MirrorFrontSide = backsidemirror_checkBox.Checked;
                ScanOption.UseInches = useinches_checkBox.Checked;
            }
        }

        private void originalsides_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (originalSides_ComboBox.SelectedIndex == 2)
            {
                pagesFlipup_CheckBox.Enabled = true;
            }
            else
            {
                pagesFlipup_CheckBox.Enabled = false;
                pagesFlipup_CheckBox.Checked = false;
            }
        }

        private void imageadjustment_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (imageAdjustment_CheckBox.Checked && imageAdjustment_CheckBox.Enabled)
            {
                imageAdjustment_GroupBox.Enabled = true;
            }
            else
            {
                imageAdjustment_GroupBox.Enabled = false;
            }
        }

        private void eraseedges_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (eraseEdges_CheckBox.Checked && eraseEdges_CheckBox.Enabled)
            {
                eraseEdges_GroupBox.Enabled = true;
            }
            else
            {
                eraseEdges_GroupBox.Enabled = false;
            }
        }

        private void applysamewidth_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (applySamewidth_CheckBox.Checked)
            {
                frontsidegrpinvisible();
            }
            else if (applySamewidth_CheckBox.Checked && backsidemirror_checkBox.Checked)
            {
                foreach (Control cntrl in backside_groupBox.Controls)
                {
                    if (cntrl is Label || cntrl is TextBox)
                    {
                        cntrl.Visible = false;
                    }
                }
            }
            else if (applySamewidth_CheckBox.Checked == false && backsidemirror_checkBox.Checked)
            {
                foreach (Control cntrl in backside_groupBox.Controls)
                {
                    if (cntrl is Label || cntrl is TextBox)
                    {
                        cntrl.Visible = true;
                    }
                }
                mirrorthefrontedgetobackedges();
                frontsidegrpvisible();
            }
            else
            {
                frontsidegrpvisible();
            }
        }

        private void frontsidegrpinvisible()
        {
            topEdge_Label.Text = "All Edges";
            topEdge_TextBox.Visible = true;
            topEdgeMeasure_Label.Visible = true;
            bottom_Label.Visible = false;
            bottomEdge_TextBox.Visible = false;
            bottomEdgeMeasure_Label.Visible = false;
            leftEdge_Label.Visible = false;
            leftEdge_TextBox.Visible = false;
            leftEdgeMeasure_Label.Visible = false;
            rightEdge_Label.Visible = false;
            rightEdge_TextBox.Visible = false;
            rightEdgeMeasure_Label.Visible = false;
        }
        private void frontsidegrpvisible()
        {
            topEdge_Label.Text = "Top Edge";
            topEdgeMeasure_Label.Visible = true;
            bottom_Label.Visible = true;
            bottomEdge_TextBox.Visible = true;
            bottomEdgeMeasure_Label.Visible = true;
            leftEdge_Label.Visible = true;
            leftEdge_TextBox.Visible = true;
            leftEdgeMeasure_Label.Visible = true;
            rightEdge_Label.Visible = true;
            rightEdge_TextBox.Visible = true;
            rightEdgeMeasure_Label.Visible = true;
        }

        private void useinches_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (useinches_checkBox.Checked)
            {
                foreach (GroupBox grpcntrl in eraseEdges_GroupBox.Controls.OfType<GroupBox>())
                {
                    foreach (Label item in grpcntrl.Controls.OfType<Label>())
                    {
                        if (item.Text == "mm")
                        {
                            item.Text = "inches";
                        }
                    }
                }
            }
            else
            {
                foreach (GroupBox grpcntrl in eraseEdges_GroupBox.Controls.OfType<GroupBox>())
                {
                    foreach (Label item in grpcntrl.Controls.OfType<Label>())
                    {
                        if (item.Text == "inches")
                        {
                            item.Text = "mm";
                        }
                    }
                }
            }
        }

        private void backsidemirror_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (backsidemirror_checkBox.Checked && (applySamewidth_CheckBox.Checked))
            {
                foreach (Control cntrl in backside_groupBox.Controls)
                {
                    if (cntrl is Label || cntrl is TextBox)
                    {
                        cntrl.Visible = false;
                    }
                }
            }

            else if (backsidemirror_checkBox.Checked && applySamewidth_CheckBox.Checked == false)
            {
                foreach (Control cntrl in backside_groupBox.Controls)
                {
                    if (cntrl is Label || cntrl is TextBox)
                    {
                        cntrl.Visible = true;
                        if (cntrl is TextBox)
                        {
                            cntrl.Enabled = false;
                        }
                    }
                }
                mirrorthefrontedgetobackedges();
            }
            if (!backsidemirror_checkBox.Checked)
            {
                foreach (Control cntrl in backside_groupBox.Controls)
                {
                    if (cntrl is Label || cntrl is TextBox)
                    {
                        cntrl.Visible = true;
                        if (cntrl is TextBox)
                        {
                            cntrl.Enabled = true;
                        }
                    }
                }
            }
        }

        private void mirrorthefrontedgetobackedges()
        {

            if (topEdge_TextBox.Text != "")
            {
                topedgeback_textBox.Text = topEdge_TextBox.Text;
            }
            if (bottomEdge_TextBox.Text != "")
            {
                bottomedgeback_textBox.Text = bottomEdge_TextBox.Text;
            }
            if (leftEdge_TextBox.Text != "")
            {
                leftedgeback_textBox.Text = leftEdge_TextBox.Text;
            }
            if (rightEdge_TextBox.Text != "")
            {
                rightedgeback_textBox.Text = rightEdge_TextBox.Text;
            }
        }

        private void createmultifile_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (createMultifile_CheckBox.Checked)
            {
                maxpagePerFile_TextBox.Enabled = true;
            }
            else
            {
                maxpagePerFile_TextBox.Enabled = false;
                maxpagePerFile_TextBox.Text = "";
            }
        }

        private void notification_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (notification_ComboBox.SelectedIndex == 1 || notification_ComboBox.SelectedIndex == 2)
            {
                notification_GroupBox.Enabled = true;
                email_RadioButton.Checked = true;
                includeThumNail_CheckBox.Visible = true;

            }
            else
            {
                notification_GroupBox.Enabled = false;
                email_RadioButton.Checked = false;
                email_TextBox.Text = "";
                includeThumNail_CheckBox.Visible = false;
            }
        }

        private void maxpageperfile_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }


        private void topedge_textBox_Leave(object sender, EventArgs e)
        {
            topEdge_TextBox.Text = FormatDecimalPlaceofText(topEdge_TextBox);
        }

        private string FormatDecimalPlaceofText(TextBox cntrl)
        {
            return string.Format("{0:0.00}", Convert.ToDecimal(cntrl.Text));
        }
        private void topedge_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != Char.Parse("."))
            {
                e.Handled = true;
            }
        }

        private void bottomedge_textBox_Leave(object sender, EventArgs e)
        {
            bottomEdge_TextBox.Text = FormatDecimalPlaceofText(bottomEdge_TextBox);
        }

        private void leftedge_textBox_Leave(object sender, EventArgs e)
        {
            leftEdge_TextBox.Text = FormatDecimalPlaceofText(leftEdge_TextBox);
        }

        private void rightedge_textBox_Leave(object sender, EventArgs e)
        {
            rightEdge_TextBox.Text = FormatDecimalPlaceofText(rightEdge_TextBox);
        }

        private void email_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (email_RadioButton.Checked)
            {
                email_TextBox.Enabled = true;
                email_RadioButton.Checked = true;
            }

            else
            {
                email_TextBox.Enabled = false;
                email_TextBox.Text = "";
                email_RadioButton.Checked = false;
            }
        }

        private void print_radioButton_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void originalsize_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_eraseClearAll_Click(object sender, EventArgs e)
        {
            foreach (TextBox lbl in frontSide_GroupBox.Controls.OfType<TextBox>())
            {
                lbl.Text = "0.00";
            }
            foreach (TextBox lbl in backside_groupBox.Controls.OfType<TextBox>())
            {
                lbl.Text = "0.00";
            }
            backsidemirror_checkBox.Checked = false;
            applySamewidth_CheckBox.Checked = false;
            useinches_checkBox.Checked = true;
        }

        private void eraseEdgesKeyPressNumericOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != Char.Parse("."))
            {
                e.Handled = true;
            }

            TextBox text = (TextBox)sender;
            if (text.Text.Contains(".") && e.KeyChar == Char.Parse("."))
                e.Handled = true;
        }

        private void eraseEdgesKeyUpMaxValue(object sender, KeyEventArgs e)
        {
            TextBox text = (TextBox)sender;
            double dValue = Convert.ToDouble(string.IsNullOrEmpty(text.Text) ? "0" : text.Text);
            if (dValue > 999)
                text.Text = "999";
        }


        private void SetSidePreviousSelection()
        {
            if (ScanOption.SetSides)
            {
                sides_CheckBox.Checked = true;
                sides_GroupBox.Enabled = true;
            }
            else
            {
                sides_CheckBox.Checked = false;
                sides_GroupBox.Enabled = false;
            }

            if (ScanOption.OriginalOneSided == true)
            {
                originalOneSide_RadioButton.Checked = true;
                originalTwoSided_RadioButton.Checked = false;
            }
            else
            {
                originalOneSide_RadioButton.Checked = false;
                originalTwoSided_RadioButton.Checked = true;
            }

            if (ScanOption.OutputOneSided == true)
            {
                outputOneSided_RadioButton.Checked = true;
                outputTwoSided_RadioButton.Checked = false;
            }
            else
            {
                outputOneSided_RadioButton.Checked = false;
                outputTwoSided_RadioButton.Checked = true;
            }

            if (ScanOption.OriginalPageflip == true)
            {
                originalPageFlip_CheckBox.Enabled = true;
                originalPageFlip_CheckBox.Checked = true;
            }

            if (ScanOption.OutputPageflip == true)
            {
                outputPageFlip_CheckBox.Enabled = true;
                outputPageFlip_CheckBox.Checked = true;
            }
        }

        private void EnableDisableAutomaticOptions()
        {
            if (ScanOption.ReduceEnlargeOptions)
            {
                includeMargin_checkBox.Enabled = true;
                includeMargin_checkBox.Visible = true;
                percentage_Label.Visible = false;
                zoomSize_TextBox.Visible = false;

                zoomSize_TextBox.Enabled = false;
                zoomSize_TextBox.Text = "0";
                percentage_Label.Enabled = false;
            }
            else
            {
                includeMargin_checkBox.Enabled = false;
                includeMargin_checkBox.Visible = false;
                percentage_Label.Visible = true;
                zoomSize_TextBox.Visible = true;
                includeMargin_checkBox.Checked = false;
            }
        }
        private void EnableDisableManualOption()
        {
            if (!ScanOption.ReduceEnlargeOptions)
            {
                zoomSize_TextBox.Enabled = true;
                percentage_Label.Enabled = true;
                includeMargin_checkBox.Visible = false;
                percentage_Label.Visible = true;
                zoomSize_TextBox.Visible = true;
                includeMargin_checkBox.Checked = false;
            }
            else
            {
                zoomSize_TextBox.Enabled = false;
                zoomSize_TextBox.Text = "0";
                percentage_Label.Enabled = false;
            }
        }

        private void LoadPagesPerSheet()
        {
            if (ScanOption.SetPagesPerSheet)
            {
                pagesPerSheet_CheckBox.Checked = true;
                pagesPerSheet_GroupBox.Enabled = true;
            }
            else
            {
                pagesPerSheet_CheckBox.Checked = false;
                pagesPerSheet_GroupBox.Enabled = false;
            }

            if (ScanOption.PagesPerSheetElement == PagesPerSheet.OneUp)
                pagePerSheetOne_RadioButton.Checked = true;
            else if (ScanOption.PagesPerSheetElement == PagesPerSheet.TwoUp)
                pagePerSheetTwo_RadioButton.Checked = true;
            else if (ScanOption.PagesPerSheetElement == PagesPerSheet.FourBtoR)
                pagePerSheetdwnrt_RadioButton.Checked = true;
            else if (ScanOption.PagesPerSheetElement == PagesPerSheet.FourRtoB)
                pagePerSheetFourrtdwn_RadioButton.Checked = true;

            if (ScanOption.PagesPerSheetAddBorder)
                pagesPerSheetAddPageBrdr_CheckBox.Checked = true;
        }

        private void LoadPaperSelection()
        {
            //paperSize_ComboBox.Items.Clear();
            //paperTray_ComboBox.Items.Clear();
            //paperType_ComboBox.Items.Clear();

            paperSize_ComboBox.DataSource = EnumUtil.GetDescriptions<PaperSelectionPaperSize>().ToList();
            paperSize_ComboBox.SelectedIndex = paperSize_ComboBox.FindStringExact(ScanOption.PaperSelectionPaperSize.GetDescription());


            paperTray_ComboBox.DataSource = EnumUtil.GetDescriptions<PaperSelectionPaperTray>().ToList();
            paperTray_ComboBox.SelectedIndex = paperTray_ComboBox.FindStringExact(ScanOption.PaperSelectionPaperTray.GetDescription());


            paperType_ComboBox.DataSource = EnumUtil.GetDescriptions<PaperSelectionPaperType>().ToList();
            paperType_ComboBox.SelectedIndex = paperType_ComboBox.FindStringExact(ScanOption.PaperSelectionPaperType.GetDescription());

        }


        private bool IsValidEmail(string emailAddress)
        {
            bool value = true;
            if (emailAddress != "" && print_RadioButton.Checked == false)
            {
                string expresion;
                expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(emailAddress, expresion))
                {
                    if (Regex.Replace(emailAddress, expresion, string.Empty).Length == 0)
                    {
                        return value;
                    }
                    else
                    {
                        value = false;
                    }
                }
                else
                {
                    value = false;
                }
            }
            return value;
        }

        private void email_textBox_TextChanged(object sender, EventArgs e)
        {
            if (!print_RadioButton.Checked == true && email_TextBox.Text != "")
            {
                fieldValidator.RequireCustom(email_TextBox, () => (IsValidEmail(email_TextBox.Text)), "A valid email Address is required.");
            }
        }

        private void rdo_Original_2Sided_CheckedChanged(object sender, EventArgs e)
        {
            if (originalOneSide_RadioButton.Checked)
            {
                originalPageFlip_CheckBox.Enabled = false;
                originalPageFlip_CheckBox.Checked = false;
            }
            else
                originalPageFlip_CheckBox.Enabled = true;
        }

        private void rdo_Output_2Sided_CheckedChanged(object sender, EventArgs e)
        {
            if (outputOneSided_RadioButton.Checked)
            {
                outputPageFlip_CheckBox.Enabled = false;
                outputPageFlip_CheckBox.Checked = false;
            }
            else
            {
                outputPageFlip_CheckBox.Enabled = true;
            }
        }

        private void ReduceEnlargeOptions_Combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reduceEnlarge_Combobox.SelectedIndex == 0)
            {
                ScanOption.ReduceEnlargeOptions = true;
                EnableDisableAutomaticOptions();
            }
            else
            {
                ScanOption.ReduceEnlargeOptions = false;
                EnableDisableManualOption();
            }
        }

        private void chk_bookletFormat_CheckedChanged(object sender, EventArgs e)
        {
            if (bookletFormat_CheckBox.Checked)
            {
                borderonPage_CheckBox.Enabled = true;
            }
            else
            {
                borderonPage_CheckBox.Checked = false;
                borderonPage_CheckBox.Enabled = false;
            }
        }

        private void ValidatePagesElements(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Name == pagePerSheetOne_RadioButton.Name && rdo.Checked)
            {
                pagesPerSheetAddPageBrdr_CheckBox.Enabled = false;
                pagesPerSheetAddPageBrdr_CheckBox.Checked = false;
            }
            else
            {
                pagesPerSheetAddPageBrdr_CheckBox.Enabled = true;
            }
        }
        private void ZoomSize_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void PopulatePagesPerSheet()
        {
            if (pagePerSheetOne_RadioButton.Checked)
            {
                ScanOption.PagesPerSheetElement = PagesPerSheet.OneUp;
            }
            else if (pagePerSheetTwo_RadioButton.Checked)
            {
                ScanOption.PagesPerSheetElement = PagesPerSheet.TwoUp;
            }
            else if (pagePerSheetFourrtdwn_RadioButton.Checked)
            {
                ScanOption.PagesPerSheetElement = PagesPerSheet.FourRtoB;
            }
            else if (pagePerSheetdwnrt_RadioButton.Checked)
            {
                ScanOption.PagesPerSheetElement = PagesPerSheet.FourBtoR;
            }
            if (pagesPerSheetAddPageBrdr_CheckBox.Checked)
                ScanOption.PagesPerSheetAddBorder = true;
            else
                ScanOption.PagesPerSheetAddBorder = false;
        }

        private void automaticTone_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (automaticTone_CheckBox.Checked)
            {
                darkness_TrackBar.Enabled = false;
                contrast_TrackBar.Enabled = false;
                backGroundCleanup_TrackBar.Enabled = false;
            }
            else
            {
                darkness_TrackBar.Enabled = true;
                contrast_TrackBar.Enabled = true;
                backGroundCleanup_TrackBar.Enabled = true;
            }
        }

        private void pagesPerSheet_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (pagesPerSheet_CheckBox.Checked && pagesPerSheet_CheckBox.Enabled)
            {
                ScanOption.SetPagesPerSheet = true;
                pagesPerSheet_GroupBox.Enabled = true;
            }
            else
            {
                ScanOption.SetPagesPerSheet = false;
                pagesPerSheet_GroupBox.Enabled = false;
            }
        }

        private void sides_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sides_CheckBox.Checked && sides_CheckBox.Enabled)
            {
                ScanOption.SetSides = true;
                sides_GroupBox.Enabled = true;
            }
            else
            {
                ScanOption.SetSides = false;
                sides_GroupBox.Enabled = false;
            }
        }
    }

}
