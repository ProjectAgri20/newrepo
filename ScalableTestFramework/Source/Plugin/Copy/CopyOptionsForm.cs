using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using HP.ScalableTest.Utility;
using HP.ScalableTest.DeviceAutomation;
using System.ComponentModel;
using System.Reflection;

namespace HP.ScalableTest.Plugin.Copy
{
    public partial class CopyOptionsForm : Form
    {
        public CopyOptions CopyOptions { get; set; }

        public CopyOptionsForm()
        {
            InitializeComponent();
            CopyOptions = new CopyOptions();
        }

        public CopyOptionsForm(CopyOptions options)
        {
            InitializeComponent();
            CopyOptions = options;
        }

        protected override void OnLoad(EventArgs e)
        {
            eraseEdges_CheckBox.Checked = CopyOptions.SetEraseEdges;
            imageAdjustment_CheckBox.Checked = CopyOptions.SetImageAdjustment;
            stamps_CheckBox.Checked = CopyOptions.setStamps;
            sides_CheckBox.Checked = CopyOptions.SetSides;
            pagesPerSheet_CheckBox.Checked = CopyOptions.SetPagesPerSheet;
            collate_checkBox.Checked = CopyOptions.Collate;
            edgetoedge_checkBox.Checked = CopyOptions.EdgeToEdge;
            Color_comboBox.Text = CopyOptions.Color;
            Copies_NumericUpDown.Value = CopyOptions.Copies;
            if (CopyOptions.ReduceEnlargeOptions)
            {
                reduceEnlargeOptions_Combobox.Text = "Automatic";
            }
            else
            {
                reduceEnlargeOptions_Combobox.Text = "Manual";
            }
            IncludeMargin_checkBox.Checked = CopyOptions.IncludeMargin;
            zoomSize_TextBox.Text = Convert.ToString(CopyOptions.ZoomSize);
            EnableDisableAutomaticOptions();
            EnableDisableManualOption();
            SetSidePreviousSelection();
            LoadStampsData();
            LoadScanMode();
            bookletFormat_Checkbox.Checked = CopyOptions.BookLetFormat;
            borderOnpage_Checkbox.Checked = CopyOptions.BorderOnEachPage;
            waterMark_Textbox.Text = CopyOptions.WatermarkText;
            LoadEraseEdges();
            LoadPagesPerSheet();
            LoadOriginalSize();
            LoadPaperSelection();
            LoadImageAdjustments();
            LoadOptimizeTextPicture();
            LoadContentOrientation();
            base.OnLoad(e);
        }

        private void LoadContentOrientation()
        {
            orientation_comboBox.Items.Clear();
            orientation_comboBox.DataSource = EnumUtil.GetDescriptions<ContentOrientation>().ToList();
            orientation_comboBox.SelectedIndex = orientation_comboBox.FindStringExact(CopyOptions.Orientation.GetDescription());
        }

        private void LoadOptimizeTextPicture()
        {
            optimizeTextPic_combox.Items.Clear();
            optimizeTextPic_combox.DataSource = EnumUtil.GetDescriptions<OptimizeTextPic>().ToList();
            optimizeTextPic_combox.SelectedIndex = optimizeTextPic_combox.FindStringExact(CopyOptions.OptimizeTextPicture.GetDescription());
        }

        private void LoadImageAdjustments()
        {
            if (CopyOptions.SetImageAdjustment)
            {
                imageAdjustment_CheckBox.Checked = true;
                imageAdjustment_Groupbox.Enabled = true;
            }
            else
            {
                imageAdjustment_CheckBox.Checked = false;
                imageAdjustment_Groupbox.Enabled = false;
            }
            sharpness_Trackbar.Value = CopyOptions.ImageAdjustSharpness;
            darkness_Trackbar.Value = CopyOptions.ImageAdjustDarkness;
            contrast_Trackbar.Value = CopyOptions.ImageAdjustContrast;
            backgroundcleanup_Trackbar.Value = CopyOptions.ImageAdjustbackgroundCleanup;
        }

        private void LoadPaperSelection()
        {
            psPaperSize_Combobox.Items.Clear();
            psPaperTray_Combobox.Items.Clear();
            psPaperType_Combobox.Items.Clear();

            psPaperSize_Combobox.DataSource = EnumUtil.GetDescriptions<PaperSelectionPaperSize>().ToList();
            psPaperSize_Combobox.SelectedIndex = psPaperSize_Combobox.FindStringExact(CopyOptions.PaperSelectionPaperSize.GetDescription());


            psPaperTray_Combobox.DataSource = EnumUtil.GetDescriptions<PaperSelectionPaperTray>().ToList();
            psPaperTray_Combobox.SelectedIndex = psPaperTray_Combobox.FindStringExact(CopyOptions.PaperSelectionPaperTray.GetDescription());


            psPaperType_Combobox.DataSource = EnumUtil.GetDescriptions<PaperSelectionPaperType>().ToList();
            psPaperType_Combobox.SelectedIndex = psPaperType_Combobox.FindStringExact(CopyOptions.PaperSelectionPaperType.GetDescription());

        }

        private void LoadOriginalSize()
        {
            originalSize_Combobox.Items.Clear();
            originalSize_Combobox.DataSource = EnumUtil.GetDescriptions<OriginalSize>().ToList();
            originalSize_Combobox.SelectedIndex = originalSize_Combobox.FindStringExact(CopyOptions.OriginalSizeType.GetDescription());

        }

        private void LoadPagesPerSheet()
        {
            if (CopyOptions.SetPagesPerSheet)
            {
                pagesPerSheet_CheckBox.Checked = true;
                pagesPerSheet_Groupbox.Enabled = true;
            }
            else
            {
                pagesPerSheet_CheckBox.Checked = false;
                pagesPerSheet_Groupbox.Enabled = false;
            }

            if (CopyOptions.PagesPerSheetElement == PagesPerSheet.OneUp)
            {
                ppsOne_radioButton.Checked = true;
            }
            else if (CopyOptions.PagesPerSheetElement == PagesPerSheet.TwoUp)
            {
                ppsTwo_Radiobutton.Checked = true;
            }
            else if (CopyOptions.PagesPerSheetElement == PagesPerSheet.FourBtoR)
            {
                ppsFourBtoR_Radiobutton.Checked = true;
            }
            else if (CopyOptions.PagesPerSheetElement == PagesPerSheet.FourRtoB)
            {
                ppsFourRtoB_Radiobutton.Checked = true;
            }
            if (CopyOptions.PagesPerSheetAddBorder)
            {
                ppsAddPageBorders_Checkbox.Checked = true;
            }
        }

        private void LoadEraseEdges()
        {
            if (CopyOptions.SetEraseEdges)
            {
                eraseEdges_CheckBox.Checked = true;
                eraseEdges_Groupbox.Enabled = true;
            }
            else
            {
                eraseEdges_Groupbox.Enabled = false;
                eraseEdges_CheckBox.Checked = false;
            }
            frontSameWidht_Checkbox.Checked = CopyOptions.ApplySameWdith;
            backMirrorFront_Checkbox.Checked = CopyOptions.MirrorFrontSide;
            chk_edgesUseInches.Checked = CopyOptions.UseInches;

            if (frontSameWidht_Checkbox.Checked)
            {
                frontTopEdge_Textbox.Text = CopyOptions.EraseEdgesValue.AllEdges;
            }
            else
            {
                frontTopEdge_Textbox.Text = CopyOptions.EraseEdgesValue.FrontTop;
            }
            frontBottomEdge_Textbox.Text = CopyOptions.EraseEdgesValue.FrontBottom;
            frontRightEdge_Textbox.Text = CopyOptions.EraseEdgesValue.FrontRight;
            frontLeftEdge_Textbox.Text = CopyOptions.EraseEdgesValue.FrontLeft;

            backTopEdge_Textbox.Text = CopyOptions.EraseEdgesValue.BackTop;
            backBottomEdge_Textbox.Text = CopyOptions.EraseEdgesValue.BackBottom;
            backRightEdge_Textbox.Text = CopyOptions.EraseEdgesValue.BackRight;
            backLeftEdge_Textbox.Text = CopyOptions.EraseEdgesValue.BackLeft;
        }

        private void LoadScanMode()
        {
            scanMode_Combobox.Items.Clear();
            scanMode_Combobox.DataSource = EnumUtil.GetDescriptions<ScanMode>().ToList();
            scanMode_Combobox.SelectedIndex = scanMode_Combobox.FindStringExact(CopyOptions.ScanModes.GetDescription());
        }

        private void Ok_button_Click(object sender, EventArgs e)
        {
            UpdateCopyPreferences();
            DialogResult = DialogResult.OK;
        }

        private void ZoomSize_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void EnableDisableAutomaticOptions()
        {
            if (CopyOptions.ReduceEnlargeOptions)
            {
                IncludeMargin_checkBox.Enabled = true;
                IncludeMargin_checkBox.Visible = true;
                percentage_Label.Visible = false;
                zoomSize_TextBox.Visible = false;

                zoomSize_TextBox.Enabled = false;
                zoomSize_TextBox.Text = "0";
                percentage_Label.Enabled = false;
            }
            else
            {
                IncludeMargin_checkBox.Enabled = false;
                IncludeMargin_checkBox.Visible = false;
                percentage_Label.Visible = true;
                zoomSize_TextBox.Visible = true;
                IncludeMargin_checkBox.Checked = false;
            }
        }

        private void EnableDisableManualOption()
        {
            if (!CopyOptions.ReduceEnlargeOptions)
            {
                zoomSize_TextBox.Enabled = true;
                percentage_Label.Enabled = true;
                IncludeMargin_checkBox.Visible = false;
                percentage_Label.Visible = true;
                zoomSize_TextBox.Visible = true;
                IncludeMargin_checkBox.Checked = false;
            }
            else
            {
                zoomSize_TextBox.Enabled = false;
                zoomSize_TextBox.Text = "0";
                percentage_Label.Enabled = false;
            }
        }

        private void ReduceEnlargeOptions_Combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reduceEnlargeOptions_Combobox.SelectedIndex == 0)
            {
                CopyOptions.ReduceEnlargeOptions = true;
                EnableDisableAutomaticOptions();
            }
            else
            {
                CopyOptions.ReduceEnlargeOptions = false;
                EnableDisableManualOption();
            }
        }

        private void CheckChangeOriginalSides(object sender, EventArgs e)
        {
            if (original_1Sided_Radiobutton.Checked)
            {
                originalSides_PageFlipup_Checkbox.Enabled = false;
                originalSides_PageFlipup_Checkbox.Checked = false;
            }
            else
                originalSides_PageFlipup_Checkbox.Enabled = true;
        }

        private void CheckChangeOutputSides(object sender, EventArgs e)
        {
            if (output_1Sided_Radiobutton.Checked)
            {
                outputSides_PageFlipup_Checkbox.Enabled = false;
                outputSides_PageFlipup_Checkbox.Checked = false;
            }
            else
            {
                outputSides_PageFlipup_Checkbox.Enabled = true;
            }
        }

        private void SetSidePreviousSelection()
        {
            if (CopyOptions.SetSides)
            {
                sides_CheckBox.Checked = true;
                sides_Groupbox.Enabled = true;
            }
            else
            {
                sides_CheckBox.Checked = false;
                sides_Groupbox.Enabled = false;
            }

            if (CopyOptions.OriginalOneSided == true)
            {
                original_1Sided_Radiobutton.Checked = true;
                original_2Sided_Radiobutton.Checked = false;
            }
            else
            {
                original_1Sided_Radiobutton.Checked = false;
                original_2Sided_Radiobutton.Checked = true;
            }

            if (CopyOptions.OutputOneSided == true)
            {
                output_1Sided_Radiobutton.Checked = true;
                output_2Sided_Radiobutton.Checked = false;
            }
            else
            {
                output_1Sided_Radiobutton.Checked = false;
                output_2Sided_Radiobutton.Checked = true;
            }

            if (CopyOptions.OriginalPageflip == true)
            {
                originalSides_PageFlipup_Checkbox.Enabled = true;
                originalSides_PageFlipup_Checkbox.Checked = true;
            }

            if (CopyOptions.OutputPageflip == true)
            {
                outputSides_PageFlipup_Checkbox.Enabled = true;
                outputSides_PageFlipup_Checkbox.Checked = true;
            }
        }

        private void txt_stampStartingNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void eraseEdgesKeyPressNumericOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != Char.Parse("."))
            {
                e.Handled = true;
            }

            TextBox text = (TextBox)sender;
            //Below condition restricts the user to input just one decimal point .There is a restriction for numbers lesser than 1 , i.e user must enter 0.23 and not just .23 
            if (text.Text.Contains(".") && e.KeyChar == Char.Parse(".") ||
                string.IsNullOrEmpty(text.Text) && e.KeyChar == Char.Parse("."))
            {
                e.Handled = true;
            }
        }

        private void UpdateCopyPreferences()
        {
            CopyOptions.Orientation = EnumUtil.GetByDescription<ContentOrientation>(orientation_comboBox.SelectedItem.ToString());
            CopyOptions.Collate = collate_checkBox.Checked;
            CopyOptions.EdgeToEdge = edgetoedge_checkBox.Checked;
            CopyOptions.Color = Color_comboBox.Text;
            CopyOptions.Copies = (int)Copies_NumericUpDown.Value;
            if (reduceEnlargeOptions_Combobox.Text == "Automatic")
            {
                CopyOptions.ReduceEnlargeOptions = true;
            }
            else
            {
                CopyOptions.ReduceEnlargeOptions = false;
            }
            CopyOptions.IncludeMargin = IncludeMargin_checkBox.Checked;
            CopyOptions.ZoomSize = int.Parse(zoomSize_TextBox.Text);
            CopyOptions.OptimizeTextPicOptions = EnumUtil.GetByDescription<OptimizeTextPic>(optimizeTextPic_combox.SelectedItem.ToString());
            #region------------------- Sides controls start -----------------------
            if (sides_CheckBox.Checked)
            {
                CopyOptions.OriginalOneSided = original_1Sided_Radiobutton.Checked ? true : original_2Sided_Radiobutton.Checked ? false : true;
                CopyOptions.OutputOneSided = output_1Sided_Radiobutton.Checked ? true : output_2Sided_Radiobutton.Checked ? false : true;
                CopyOptions.OriginalPageflip = originalSides_PageFlipup_Checkbox.Checked ? true : false;
                CopyOptions.OutputPageflip = outputSides_PageFlipup_Checkbox.Checked ? true : false;
                CopyOptions.SetSides = true;
            }
            else
            {
                CopyOptions.SetSides = false;
            }
            #endregion

            #region -------------------Stamp Content controls start------------------
            if (stamps_CheckBox.Checked)
            {
                PopulateStampData();
                CopyOptions.setStamps = true;
            }
            else
            {
                CopyOptions.setStamps = false;
            }
            #endregion

            #region Scan mode            
            CopyOptions.ScanModes = EnumUtil.GetByDescription<ScanMode>(scanMode_Combobox.SelectedItem.ToString());
            #endregion

            #region Booklet settings
            CopyOptions.BookLetFormat = bookletFormat_Checkbox.Checked;
            CopyOptions.BorderOnEachPage = borderOnpage_Checkbox.Checked;
            #endregion

            CopyOptions.WatermarkText = waterMark_Textbox.Text;
            if (eraseEdges_CheckBox.Checked)
            {
                PopulateEraseEdgesData();
                CopyOptions.SetEraseEdges = true;
            }
            else
            {
                CopyOptions.SetEraseEdges = false;
            }

            #region Pages Per Sheet
            if (pagesPerSheet_CheckBox.Checked)
            {
                PopulatePagesPerSheet();
                CopyOptions.SetPagesPerSheet = true;
            }
            else
            {
                CopyOptions.SetPagesPerSheet = false;
            }
            #endregion

            if (originalSize_Combobox.SelectedItem != null)
                CopyOptions.OriginalSizeType = EnumUtil.GetByDescription<OriginalSize>(originalSize_Combobox.SelectedItem.ToString());

            CopyOptions.PaperSelectionPaperSize = EnumUtil.GetByDescription<PaperSelectionPaperSize>(psPaperSize_Combobox.SelectedItem.ToString());
            CopyOptions.PaperSelectionPaperType = EnumUtil.GetByDescription<PaperSelectionPaperType>(psPaperType_Combobox.SelectedItem.ToString());
            CopyOptions.PaperSelectionPaperTray = EnumUtil.GetByDescription<PaperSelectionPaperTray>(psPaperTray_Combobox.SelectedItem.ToString());

            if (optimizeTextPic_combox.SelectedItem != null)
            {
                CopyOptions.OptimizeTextPicture = EnumUtil.GetByDescription<OptimizeTextPic>(optimizeTextPic_combox.SelectedItem.ToString());
            }

            if (imageAdjustment_CheckBox.Checked)
            {
                CopyOptions.SetImageAdjustment = true;
                CopyOptions.ImageAdjustSharpness = sharpness_Trackbar.Value;
                CopyOptions.ImageAdjustDarkness = darkness_Trackbar.Value;
                CopyOptions.ImageAdjustContrast = contrast_Trackbar.Value;
                CopyOptions.ImageAdjustbackgroundCleanup = backgroundcleanup_Trackbar.Value;
            }
            else
            {
                CopyOptions.SetImageAdjustment = false;
            }

        }

        private void PopulatePagesPerSheet()
        {
            if (ppsOne_radioButton.Checked)
            {
                CopyOptions.PagesPerSheetElement = PagesPerSheet.OneUp;
            }
            else if (ppsTwo_Radiobutton.Checked)
            {
                CopyOptions.PagesPerSheetElement = PagesPerSheet.TwoUp;
            }
            else if (ppsFourRtoB_Radiobutton.Checked)
            {
                CopyOptions.PagesPerSheetElement = PagesPerSheet.FourRtoB;
            }
            else if (ppsFourBtoR_Radiobutton.Checked)
            {
                CopyOptions.PagesPerSheetElement = PagesPerSheet.FourBtoR;
            }
            if (ppsAddPageBorders_Checkbox.Checked)
            {
                CopyOptions.PagesPerSheetAddBorder = true;
            }
            else
            {
                CopyOptions.PagesPerSheetAddBorder = false;
            }
        }

        private void PopulateEraseEdgesData()
        {
            bool hasValue = false;
            foreach (TextBox txt in edgesBackElemets_Groupbox.Controls.OfType<TextBox>())
            {
                if (!String.IsNullOrEmpty(txt.Text))
                {
                    hasValue = true;
                    break;
                }
            }

            foreach (TextBox txt in edgesFrontElements_Groupbox.Controls.OfType<TextBox>())
            {
                if (!String.IsNullOrEmpty(txt.Text))
                {
                    hasValue = true;
                    break;
                }
            }

            if (hasValue)
            {
                CopyOptions.EraseEdgesValue.FrontTop = frontTopEdge_Textbox.Text;
                CopyOptions.EraseEdgesValue.FrontBottom = frontBottomEdge_Textbox.Text;
                CopyOptions.EraseEdgesValue.FrontLeft = frontLeftEdge_Textbox.Text;
                CopyOptions.EraseEdgesValue.FrontRight = frontRightEdge_Textbox.Text;

                CopyOptions.EraseEdgesValue.BackTop = backTopEdge_Textbox.Text;
                CopyOptions.EraseEdgesValue.BackBottom = backBottomEdge_Textbox.Text;
                CopyOptions.EraseEdgesValue.BackLeft = backLeftEdge_Textbox.Text;
                CopyOptions.EraseEdgesValue.BackRight = backRightEdge_Textbox.Text;
                CopyOptions.ApplySameWdith = frontSameWidht_Checkbox.Checked;
                CopyOptions.MirrorFrontSide = backMirrorFront_Checkbox.Checked;
                CopyOptions.UseInches = chk_edgesUseInches.Checked;
                if (frontSameWidht_Checkbox.Checked)
                {
                    CopyOptions.EraseEdgesValue.AllEdges = frontTopEdge_Textbox.Text;
                    CopyOptions.EraseEdgesValue.FrontTop = string.Empty;
                }
                else
                {
                    CopyOptions.EraseEdgesValue.AllEdges = string.Empty;
                    CopyOptions.EraseEdgesValue.FrontTop = frontTopEdge_Textbox.Text;
                }
            }

        }

        private void LoadStampsData()
        {
            stamps_CheckBox.Checked = CopyOptions.setStamps ? true : false;
            stamps_Groupbox.Enabled = CopyOptions.setStamps ? true : false;

            stamps_Datagrid.Rows.Clear();
            foreach (StampType type in EnumUtil.GetValues<StampType>().ToList())
            {
                StampContents content = CopyOptions.StampContents.FirstOrDefault(x => x.StampType.Equals(type));
                if (content == null)
                {
                    stamps_Datagrid.Rows.Add(type.GetDescription(), "None");
                }
                else
                {
                    StampContents stamp = content;
                    stamps_Datagrid.Rows.Add(stamp.StampType.GetDescription(), stamp.StampContentType);
                }

            }
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="lstSelectedIndex"></param>
        /// 
        private void PopulateStampData()
        {
            CopyOptions.StampContents.Clear();

            if (CopyOptions.setStamps)
            {
                foreach (DataGridViewRow row in stamps_Datagrid.Rows)
                {
                    if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() != string.Empty && row.Cells[1].Value.ToString() != "None")
                    {
                        StampContents stamp = new StampContents();
                        stamp.StampType = EnumUtil.GetByDescription<StampType>(row.Cells[0].Value.ToString());
                        stamp.StampContentType = row.Cells[1].Value.ToString();
                        CopyOptions.StampContents.Add(stamp);
                    }
                }
                CopyOptions.StampContents.TrimExcess();
            }
        }

        private void chk_bookletFormat_CheckedChanged(object sender, EventArgs e)
        {
            if (bookletFormat_Checkbox.Checked)
            {
                borderOnpage_Checkbox.Enabled = true;
            }
            else
            {
                borderOnpage_Checkbox.Checked = false;
                borderOnpage_Checkbox.Enabled = false;
            }
        }

        private void chk_edgesUseInches_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_edgesUseInches.Checked)
            {
                setDimensionValues(edgesFrontElements_Groupbox, "IN");
                setDimensionValues(edgesBackElemets_Groupbox, "IN");

            }
            else
            {
                setDimensionValues(edgesFrontElements_Groupbox, "mm");
                setDimensionValues(edgesBackElemets_Groupbox, "mm");
            }
        }

        private void setDimensionValues(GroupBox grp, string labelText)
        {
            foreach (Label lbl in grp.Controls.OfType<Label>())
            {
                if (lbl.Name.Contains("EED_"))
                {
                    lbl.Text = labelText;
                }
            }
        }

        private void chk_backMirrorFront_CheckedChanged(object sender, EventArgs e)
        {
            if (backMirrorFront_Checkbox.Checked && frontSameWidht_Checkbox.Checked)
            {
                edgesBackElemets_Groupbox.Visible = false;
            }
            else if (backMirrorFront_Checkbox.Checked && frontSameWidht_Checkbox.Checked == false)
            {
                edgesBackElemets_Groupbox.Visible = true;

                foreach (TextBox txt in edgesBackElemets_Groupbox.Controls.OfType<TextBox>())
                {
                    txt.Enabled = false;
                }

                backTopEdge_Textbox.Text = frontTopEdge_Textbox.Text;
                backLeftEdge_Textbox.Text = frontLeftEdge_Textbox.Text;
                backBottomEdge_Textbox.Text = frontBottomEdge_Textbox.Text;
                backRightEdge_Textbox.Text = frontRightEdge_Textbox.Text;
            }
            else if (!backMirrorFront_Checkbox.Checked)
            {
                edgesBackElemets_Groupbox.Visible = true;
                foreach (TextBox txt in edgesBackElemets_Groupbox.Controls.OfType<TextBox>())
                {
                    txt.Enabled = true;
                }
            }
            else
            {
                edgesBackElemets_Groupbox.Visible = true;
            }
        }

        private void chk_frontSameWidht_CheckedChanged(object sender, EventArgs e)
        {
            if (frontSameWidht_Checkbox.Checked)
            {
                edgesBackElemets_Groupbox.Visible = true;
                foreach (Control ctrl in edgesFrontElements_Groupbox.Controls)
                {
                    if (!ctrl.Name.ToUpper().Contains("FRONTTOP"))
                    {
                        ctrl.Visible = false;
                    }
                    if (ctrl is TextBox)
                    {
                        ctrl.Text = "0.00";
                    }
                    if (frontTopEdge_Label.Name == ctrl.Name)
                    {
                        ctrl.Text = "All Edges";
                    }
                }
            }
            else
            {
                foreach (Control ctrl in edgesFrontElements_Groupbox.Controls)
                {
                    ctrl.Visible = true;
                    frontTopEdge_Label.Text = "Top Edge";
                }
            }

            if (backMirrorFront_Checkbox.Checked && !frontSameWidht_Checkbox.Checked)
            {
                backMirrorFront_Checkbox.Checked = false;
            }

            if (backMirrorFront_Checkbox.Checked && frontSameWidht_Checkbox.Checked)
            {
                edgesBackElemets_Groupbox.Visible = false;
            }

        }

        private void btn_eraseClearAll_Click(object sender, EventArgs e)
        {
            foreach (TextBox lbl in edgesBackElemets_Groupbox.Controls.OfType<TextBox>())
            {
                lbl.Text = "0.00";
            }
            foreach (TextBox lbl in edgesFrontElements_Groupbox.Controls.OfType<TextBox>())
            {
                lbl.Text = "0.00";
            }
            backMirrorFront_Checkbox.Checked = false;
            frontSameWidht_Checkbox.Checked = false;
            chk_edgesUseInches.Checked = true;
        }

        private void eraseEdgesKeyUpMaxValue(object sender, KeyEventArgs e)
        {
            TextBox text = (TextBox)sender;
            if (!String.IsNullOrEmpty(text.Text))
            {
                double dValue = Convert.ToDouble(text.Text);
                if (dValue > 999)
                {
                    text.Text = "999";
                }
            }
        }

        private void ValidatePagesElements(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Name == ppsOne_radioButton.Name && rdo.Checked)
            {
                ppsAddPageBorders_Checkbox.Enabled = false;
                ppsAddPageBorders_Checkbox.Checked = false;
            }
            else
            {
                ppsAddPageBorders_Checkbox.Enabled = true;
            }
        }

        private void ZoomSize_TextBox_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void eraseEdges_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            eraseEdges_Groupbox.Enabled = (eraseEdges_CheckBox.Checked && eraseEdges_CheckBox.Enabled) ? true : false;
        }

        private void imageAdjustment_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            imageAdjustment_Groupbox.Enabled = (imageAdjustment_CheckBox.Checked && imageAdjustment_CheckBox.Enabled) ? true : false;
        }

        private void stamps_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            stamps_Groupbox.Enabled = (stamps_CheckBox.Checked && stamps_CheckBox.Enabled) ? true : false;
        }

        private void pagesPerSheet_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pagesPerSheet_Groupbox.Enabled = (pagesPerSheet_CheckBox.Checked && pagesPerSheet_CheckBox.Enabled) ? true : false;
        }

        private void sides_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            sides_Groupbox.Enabled = (sides_CheckBox.Checked && sides_CheckBox.Enabled) ? true : false;
        }
    }
}