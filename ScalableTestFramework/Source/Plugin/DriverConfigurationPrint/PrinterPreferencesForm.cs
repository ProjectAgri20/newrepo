using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DriverConfigurationPrint.Enum;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.DriverConfigurationPrint
{
    public partial class PrinterPreferencesForm : Form
    {
        public PrinterPreferenceData PrinterPreferenceData { get; set; }

        public event EventHandler ConfigurationChanged;
        /// <summary>
        /// Constructs the Printing Preference Controls
        /// </summary>
        public PrinterPreferencesForm()
        {
            InitializeComponent();
            PrinterPreferenceData = new PrinterPreferenceData();
            noOfCopies_NumericUpDown.ValueChanged += ConfigurationChanged;
        }

        /// <summary>
        /// Constructs and updates the Printing Preference Controls 
        /// </summary>
        /// <param name="printerPreferenceData"></param>
        public PrinterPreferencesForm(PrinterPreferenceData printerPreferenceData)
        {
            InitializeComponent();
            PrinterPreferenceData = printerPreferenceData;
            LoadPrinterPreferencesComboBoxValues();
            UpdatePrinterPreferences(PrinterPreferenceData);
            fieldValidator.RequireValue(customPaperSizeName_TextBox, customPaperSizeName_Label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(customWaterMark_TextBox, customWaterMark_Label, ValidationCondition.IfEnabled);
        }

        /// <summary>
        /// Loads the Printing Preferences Control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void LoadPrinterPreferencesComboBoxValues()
        {
            paperSize_ComboBox.DataSource = EnumUtil.GetDescriptions<PaperSize>().ToList();
            paperType_ComboBox.DataSource = EnumUtil.GetDescriptions<PaperType>().ToList();
            paperQuality_ComboBox.DataSource = EnumUtil.GetDescriptions<PrintQuality>().ToList();
            paperSource_ComboBox.DataSource = EnumUtil.GetDescriptions<PaperSource>().ToList();
            specialPages_ComboBox.DataSource = EnumUtil.GetDescriptions<SpecialPages>().ToList();
            printDocument_ComboBox.DataSource = EnumUtil.GetDescriptions<PaperSize>().ToList();
            watermarks_ComboBox.DataSource = EnumUtil.GetDescriptions<WaterMark>().ToList();
            bookletLayout_ComboBox.DataSource = EnumUtil.GetDescriptions<BookletLayout>().ToList();
            pagesPerSheet_ComboBox.DataSource = EnumUtil.GetDescriptions<PagesPerSheet>().ToList();
            pageOrder_ComboBox.DataSource = EnumUtil.GetDescriptions<PageOrder>().ToList();
            staple_ComboBox.DataSource = EnumUtil.GetDescriptions<Staple>().ToList();
            punch_ComboBox.DataSource = EnumUtil.GetDescriptions<Punch>().ToList();
            fold_ComboBox.DataSource = EnumUtil.GetDescriptions<Fold>().ToList();
            outputBin_ComboBox.DataSource = EnumUtil.GetDescriptions<OutputBin>().ToList();
            makeJob_ComboBox.DataSource = EnumUtil.GetDescriptions<MakeJobPrivateSecure>().ToList();
            jobNameExists_ComboBox.DataSource = EnumUtil.GetDescriptions<IfJobNameExists>().ToList();
            printInGrayScale_ComboBox.DataSource = EnumUtil.GetDescriptions<GrayScale>().ToList();
            rgbColor_ComboBox.DataSource = EnumUtil.GetDescriptions<RGBColor>().ToList();
            rasterCompression_ComboBox.DataSource = EnumUtil.GetDescriptions<RasterCompression>().ToList();
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            var results = fieldValidator.ValidateAll();
            if (results.All(n => n.Succeeded))
            {
                UpdatePrinterPeferenceOptions();
                DialogResult = DialogResult.OK;
            }
            else
            {
                var messages = results.Where(n => !n.Succeeded).Select(n => n.Message);
                MessageBox.Show(string.Join("\n", messages), @"Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void JobStorageMode_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton jobStorageRdBtn = (RadioButton)sender;

            string jobStorageRdBtnText = jobStorageRdBtn.Text;
            switch (jobStorageRdBtnText)
            {
                case "Off":
                    if (off_RadioButton.Checked)
                    {
                        ResetJobStorageTxtBoxes();
                        privateSecure_GroupBox.Enabled = false;
                        userName_GroupBox.Enabled = false;
                        jobName_GroupBox.Enabled = false;
                        makeJob_ComboBox.SelectedIndex = 0;
                    }
                    break;
                case "Proof and Hold":
                    if (proofAndHold_RadioButton.Checked)
                    {
                        ResetJobStorageTxtBoxes();
                        privateSecure_GroupBox.Enabled = false;
                        userName_GroupBox.Enabled = true;
                        jobName_GroupBox.Enabled = true;
                        makeJob_ComboBox.SelectedIndex = 0;
                    }
                    break;
                case "Personal Job":
                    if (personalJob_RadioButton.Checked)
                    {
                        ResetJobStorageTxtBoxes();
                        privateSecure_GroupBox.Enabled = true;
                        userName_GroupBox.Enabled = true;
                        jobName_GroupBox.Enabled = true;
                        makeJob_ComboBox.SelectedIndex = 0;
                    }
                    break;
                case "Quick Copy":
                    if (quickCopy_RadioButton.Checked)
                    {
                        ResetJobStorageTxtBoxes();
                        privateSecure_GroupBox.Enabled = false;
                        userName_GroupBox.Enabled = true;
                        jobName_GroupBox.Enabled = true;
                        makeJob_ComboBox.SelectedIndex = 0;
                    }
                    break;
                case "Stored Job":
                    if (storedJob_RadioButton.Checked)
                    {
                        ResetJobStorageTxtBoxes();
                        privateSecure_GroupBox.Enabled = true;
                        userName_GroupBox.Enabled = true;
                        jobName_GroupBox.Enabled = true;
                        makeJob_ComboBox.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private void UserName_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton userNameRdBtn = (RadioButton)sender;

            string userNameRdBtnText = userNameRdBtn.Text;
            switch (userNameRdBtnText)
            {
                case "User Name":
                    if (userName_RadioButton.Checked)
                    {
                        custom_TextBox.Text = "Administrator";
                        custom_TextBox.Enabled = false;
                    }
                    break;
                case "Custom":
                    if (custom_RadioButton.Checked)
                    {
                        custom_TextBox.Enabled = true;
                    }
                    break;
                default:
                    custom_TextBox.Enabled = false;
                    break;
            }
        }

        private void JobName_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton jobNameRdBtn = (RadioButton)sender;
            string jobNameRdBtnText = jobNameRdBtn.Text;
            switch (jobNameRdBtnText)
            {
                case "Automatic":
                    if (automatic_RadioButton.Checked)
                    {
                        jobNameCustom_TextBox.Text = "<Automatic>";
                        jobNameCustom_TextBox.Enabled = false;
                    }
                    break;
                case "Custom":
                    if (jobNameCustom_RadioButton.Checked)
                    {
                        jobNameCustom_TextBox.Enabled = true;
                    }
                    break;
                default:
                    jobNameCustom_TextBox.Enabled = false;
                    break;
            }
        }

        private void PrintOnBothSides_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            int pagesPerSheetIndex = 0;
            if (printOnBothSides_CheckBox.Checked)
            {
                flipPagesUp_CheckBox.Enabled = true;
                bookletLayout_ComboBox.Enabled = true;
            }
            else
            {
                flipPagesUp_CheckBox.Checked = false;
                flipPagesUp_CheckBox.Enabled = false;
                if (bookletLayout_ComboBox.SelectedIndex == 0)
                {
                    bookletLayout_ComboBox.SelectedIndex = 0;
                    bookletLayout_ComboBox.Enabled = false;
                }
                else
                {
                    pagesPerSheetIndex = pagesPerSheet_ComboBox.SelectedIndex;
                    bookletLayout_ComboBox.SelectedIndex = 0;
                    bookletLayout_ComboBox.Enabled = false;
                    pageOrder_ComboBox.Enabled = true;
                    pagesPerSheet_ComboBox.Enabled = true;
                    printPageBorders_CheckBox.Checked = false;
                    printPageBorders_CheckBox.Enabled = true;
                    pagesPerSheet_ComboBox.SelectedIndex = pagesPerSheetIndex;
                }
            }
        }
        private void CheckPrintOnBothSidesDisable()
        {
            flipPagesUp_CheckBox.Checked = false;
            flipPagesUp_CheckBox.Enabled = false;
            if (bookletLayout_ComboBox.SelectedIndex == 0)
            {
                bookletLayout_ComboBox.SelectedIndex = 0;
                bookletLayout_ComboBox.Enabled = false;
            }
        }

        private void BookletLayout_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bookletLayout_ComboBox.SelectedIndex == 0)
            {
                pagesPerSheet_ComboBox.Enabled = true;
                if (pagesPerSheet_ComboBox.Items.Count > 0)
                {
                    pagesPerSheet_ComboBox.SelectedIndex = 0;
                    if (printOnBothSides_CheckBox.Checked)
                        flipPagesUp_CheckBox.Enabled = true;
                }
            }
            else
            {
                flipPagesUp_CheckBox.Checked = false;
                flipPagesUp_CheckBox.Enabled = false;
                pagesPerSheet_ComboBox.SelectedIndex = 1;
                pagesPerSheet_ComboBox.Enabled = false;
                printPageBorders_CheckBox.Enabled = false;
                pageOrder_ComboBox.SelectedIndex = 0;
                pageOrder_ComboBox.Enabled = false;
            }
        }

        private void PagesPerSheet_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pagesPerSheet_ComboBox.SelectedIndex == 0)
            {
                printPageBorders_CheckBox.Checked = false;
                printPageBorders_CheckBox.Enabled = false;
                if (pageOrder_ComboBox.Items.Count > 0)
                {
                    pageOrder_ComboBox.SelectedIndex = 0;
                    pageOrder_ComboBox.Enabled = false;
                }
            }
            else
            {
                printPageBorders_CheckBox.Enabled = true;
                pageOrder_ComboBox.Enabled = true;
            }
        }

        private void ResizingOptions_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton resizingOptionsRdBtn = (RadioButton)sender;
            string resizingOptionsText = resizingOptionsRdBtn.Text;
            switch (resizingOptionsText)
            {
                case "Actual Size":
                    if (resizingOptionsRdBtn.Checked)
                    {
                        printDocument_ComboBox.SelectedIndex = 0;
                        printDocument_ComboBox.Enabled = false;
                        scaleToFit_CheckBox.Enabled = false;
                        scaleToFit_CheckBox.Checked = true;
                        perOnActualSize_RadioButton.Checked = false;
                        percentage_TextBox.Enabled = false;
                    }
                    break;
                case "Print document on:":
                    if (resizingOptionsRdBtn.Checked)
                    {
                        printDocument_ComboBox.Enabled = true;
                        scaleToFit_CheckBox.Enabled = true;
                        scaleToFit_CheckBox.Checked = true;
                        perOnActualSize_RadioButton.Checked = false;
                        percentage_TextBox.Enabled = false;
                    }
                    break;
                case "% on actual size:":
                    if (resizingOptionsRdBtn.Checked)
                    {
                        printDocument_ComboBox.SelectedIndex = 0;
                        printDocument_ComboBox.Enabled = false;
                        scaleToFit_CheckBox.Checked = true;
                        scaleToFit_CheckBox.Enabled = false;
                        percentage_TextBox.Enabled = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// Updates the values on Printing Preference UI. This method is responsible for setting
        /// the Printing Preference UI values.
        /// </summary>
        public void UpdatePrinterPeferenceOptions()
        {
            PrinterPreferenceData = new PrinterPreferenceData();
            // Page Quality Tab Values are set below
            if (enableCustomName_CheckBox.Checked)
            {
                PrinterPreferenceData.EnableCustomPaperSize = enableCustomName_CheckBox.Checked;
                PrinterPreferenceData.CustomPaperSizeName = PrinterPreferenceData.PaperSize = customPaperSizeName_TextBox.Text;
            }
            else
            {
                PrinterPreferenceData.EnableCustomPaperSize = enableCustomName_CheckBox.Checked;
                PrinterPreferenceData.PaperSize = paperSize_ComboBox.SelectedValue.ToString();
            }
            PrinterPreferenceData.PrintQuality = EnumUtil.GetByDescription<PrintQuality>(paperQuality_ComboBox.SelectedValue.ToString());
            PrinterPreferenceData.PaperType = paperType_ComboBox.SelectedValue.ToString();
            PrinterPreferenceData.PaperSource = paperSource_ComboBox.SelectedValue.ToString();
            PrinterPreferenceData.SpecialPages = EnumUtil.GetByDescription<SpecialPages>(specialPages_ComboBox.SelectedValue.ToString());
            // Effects Tab Values are set below
            PrinterPreferenceData.ActualSize = actualSize_RadioButton.Checked;
            PrinterPreferenceData.PrintDocumentOn = printDocumentOn_RadioButton.Checked;
            PrinterPreferenceData.PrintDocumentOnValue = printDocument_ComboBox.SelectedValue.ToString();
            PrinterPreferenceData.ScaleToFit = scaleToFit_CheckBox.Checked;
            PrinterPreferenceData.PerActualSize = perOnActualSize_RadioButton.Checked;
            PrinterPreferenceData.PerActualSizeNo = percentage_TextBox.Text.ToString();
            if (enableCustomWatermark_CheckBox.Checked)
            {
                PrinterPreferenceData.EnableCustomWaterMark = enableCustomWatermark_CheckBox.Checked;
                PrinterPreferenceData.CustomWaterMarkName = PrinterPreferenceData.Watermark = customWaterMark_TextBox.Text;
            }
            else
            {
                PrinterPreferenceData.EnableCustomWaterMark = enableCustomWatermark_CheckBox.Checked;
                PrinterPreferenceData.Watermark = watermarks_ComboBox.SelectedValue.ToString();
            }
            PrinterPreferenceData.FirstPageOnly = firstPageOnly_CheckBox.Checked;
            // Finishing Tab Values are set below
            PrinterPreferenceData.PrintOnBothSides = printOnBothSides_CheckBox.Checked;
            PrinterPreferenceData.FlipPagesUp = flipPagesUp_CheckBox.Checked;
            PrinterPreferenceData.BookletLayout = bookletLayout_ComboBox.SelectedValue.ToString();
            PrinterPreferenceData.PagesPerSheet = EnumUtil.GetByDescription<PagesPerSheet>(pagesPerSheet_ComboBox.SelectedValue.ToString());
            PrinterPreferenceData.PrintPageBorders = printPageBorders_CheckBox.Checked;
            PrinterPreferenceData.PageOrder = EnumUtil.GetByDescription<PageOrder>(pageOrder_ComboBox.SelectedValue.ToString());
            PrinterPreferenceData.OrientationPortrait = portrait_RadioButton.Checked;
            PrinterPreferenceData.OrientationLanscape = landscape_RadioButton.Checked;
            PrinterPreferenceData.RotateBy180Degree = rotateBy180Degrees_CheckBox.Checked;
            // Output Tab Values are set below
            PrinterPreferenceData.Staple = EnumUtil.GetByDescription<Staple>(staple_ComboBox.SelectedValue.ToString());
            PrinterPreferenceData.Punch = EnumUtil.GetByDescription<Punch>(punch_ComboBox.SelectedValue.ToString());
            PrinterPreferenceData.Fold = EnumUtil.GetByDescription<Fold>(fold_ComboBox.SelectedValue.ToString());
            PrinterPreferenceData.MaxSheetPerSet = string.IsNullOrEmpty(maxSheetPerSet_TextBox.Text) ? string.Empty : maxSheetPerSet_TextBox.Text;
            PrinterPreferenceData.OutputBin = outputBin_ComboBox.SelectedValue.ToString();
            // Job Storage Tab Values are set below
            PrinterPreferenceData.JobStorageOff = off_RadioButton.Checked;
            PrinterPreferenceData.ProofandHold = proofAndHold_RadioButton.Checked;
            PrinterPreferenceData.PersonalJob = personalJob_RadioButton.Checked;
            PrinterPreferenceData.QuickJob = quickCopy_RadioButton.Checked;
            PrinterPreferenceData.StoredJob = storedJob_RadioButton.Checked;
            PrinterPreferenceData.MakeJobPrivateSecure = makeJob_ComboBox.SelectedValue.ToString();
            PrinterPreferenceData.UserName = userName_RadioButton.Checked;
            PrinterPreferenceData.Custom = custom_RadioButton.Checked;
            PrinterPreferenceData.CustomText = string.IsNullOrEmpty(custom_TextBox.Text) ? string.Empty : custom_TextBox.Text;
            PrinterPreferenceData.JobNameAutomatic = automatic_RadioButton.Checked;
            PrinterPreferenceData.JobNameCustom = jobNameCustom_RadioButton.Checked;
            PrinterPreferenceData.JobNameCustomText = string.IsNullOrEmpty(jobNameCustom_TextBox.Text) ? string.Empty : jobNameCustom_TextBox.Text;
            PrinterPreferenceData.JobNameExists = jobNameExists_ComboBox.SelectedValue.ToString();
            PrinterPreferenceData.PinNumber = pin_TextBox.Text;
            PrinterPreferenceData.Password = password_TextBox.Text;
            PrinterPreferenceData.ConfirmPassword = confirm_TextBox.Text;
            PrinterPreferenceData.SaveAsDefaultPass = saveDefaultPass_CheckBox.Checked;
            //Color Tab Values are set below
            PrinterPreferenceData.PrintInGrayScaleText = EnumUtil.GetByDescription<GrayScale>(printInGrayScale_ComboBox.SelectedValue.ToString());
            PrinterPreferenceData.RGBColor = EnumUtil.GetByDescription<RGBColor>(rgbColor_ComboBox.SelectedValue.ToString());
            // Advance Tab Values are set below
            PrinterPreferenceData.Copies = Convert.ToInt32(noOfCopies_NumericUpDown.Value);
            PrinterPreferenceData.Collate = collate_CheckBox.Checked;
            PrinterPreferenceData.ReversePageOrder = reversePageOrder_CheckBox.Checked;
            PrinterPreferenceData.PrintTextAsBlack = printAllText_CheckBox.Checked;
            PrinterPreferenceData.HPEasyColor = hpEasyColor_CheckBox.Checked;
            PrinterPreferenceData.EdgeToEdge = edgeToEdge_CheckBox.Checked;
            PrinterPreferenceData.RasterCompression = EnumUtil.GetByDescription<RasterCompression>(rasterCompression_ComboBox.SelectedValue.ToString());
        }

        /// <summary>
        /// Updates the values on Printing Preference UI. This method is responsible for setting
        /// the Printing Preference UI values from the saved configurations.
        /// </summary>
        /// <param name="_printerPreferenceData"></param>
        public void UpdatePrinterPreferences(PrinterPreferenceData printerPreferenceData)
        {
            if (printerPreferenceData != null)
            {
                // Paper Quality Tab
                if (PrinterPreferenceData.EnableCustomPaperSize)
                {
                    enableCustomName_CheckBox.Checked = PrinterPreferenceData.EnableCustomPaperSize;
                    customPaperSizeName_TextBox.Text = printerPreferenceData.PaperSize = printerPreferenceData.CustomPaperSizeName;
                }
                else
                {
                    enableCustomName_CheckBox.Checked = PrinterPreferenceData.EnableCustomPaperSize;
                    paperSize_ComboBox.SelectedIndex = paperSize_ComboBox.FindStringExact(printerPreferenceData.PaperSize);
                }
                paperQuality_ComboBox.SelectedIndex = paperQuality_ComboBox.FindStringExact(printerPreferenceData.PrintQuality.GetDescription());
                paperType_ComboBox.SelectedIndex = paperType_ComboBox.FindStringExact(printerPreferenceData.PaperType);
                paperSource_ComboBox.SelectedIndex = paperSource_ComboBox.FindString(printerPreferenceData.PaperSource);
                specialPages_ComboBox.SelectedIndex = specialPages_ComboBox.FindStringExact(PrinterPreferenceData.SpecialPages.GetDescription());
                //Effects Tab
                actualSize_RadioButton.Checked = printerPreferenceData.ActualSize ? true : false;
                printDocumentOn_RadioButton.Checked = printerPreferenceData.PrintDocumentOn ? true : false;
                printDocument_ComboBox.SelectedIndex = printDocument_ComboBox.FindStringExact(printerPreferenceData.PrintDocumentOnValue);
                scaleToFit_CheckBox.Checked = printerPreferenceData.ScaleToFit ? true : false;
                perOnActualSize_RadioButton.Checked = printerPreferenceData.PerActualSize ? true : false;
                percentage_TextBox.Text = string.IsNullOrEmpty(printerPreferenceData.PerActualSizeNo) ? "100" : printerPreferenceData.PerActualSizeNo;
                if (printerPreferenceData.EnableCustomWaterMark)
                {
                    enableCustomWatermark_CheckBox.Checked = PrinterPreferenceData.EnableCustomWaterMark;
                    customWaterMark_TextBox.Text = printerPreferenceData.Watermark = printerPreferenceData.CustomWaterMarkName;
                }
                else
                {
                    enableCustomWatermark_CheckBox.Checked = PrinterPreferenceData.EnableCustomWaterMark;
                    watermarks_ComboBox.SelectedIndex = watermarks_ComboBox.FindStringExact(printerPreferenceData.Watermark);
                }
                firstPageOnly_CheckBox.Checked = printerPreferenceData.FirstPageOnly ? true : false;
                // Finishing Tab
                printOnBothSides_CheckBox.Checked = printerPreferenceData.PrintOnBothSides ? true : false;
                if (!printerPreferenceData.PrintOnBothSides)
                {
                    CheckPrintOnBothSidesDisable();
                }
                flipPagesUp_CheckBox.Checked = printerPreferenceData.FlipPagesUp ? true : false;
                bookletLayout_ComboBox.SelectedIndex = bookletLayout_ComboBox.FindStringExact(printerPreferenceData.BookletLayout);
                pagesPerSheet_ComboBox.SelectedIndex = pagesPerSheet_ComboBox.FindStringExact(printerPreferenceData.PagesPerSheet.GetDescription());
                printPageBorders_CheckBox.Checked = printerPreferenceData.PrintPageBorders ? true : false;
                pageOrder_ComboBox.SelectedIndex = pageOrder_ComboBox.FindStringExact(printerPreferenceData.PageOrder.GetDescription());
                portrait_RadioButton.Checked = printerPreferenceData.OrientationPortrait ? true : false;
                landscape_RadioButton.Checked = printerPreferenceData.OrientationLanscape ? true : false;
                rotateBy180Degrees_CheckBox.Checked = printerPreferenceData.RotateBy180Degree ? true : false;
                // Output tab
                staple_ComboBox.SelectedIndex = staple_ComboBox.FindStringExact(printerPreferenceData.Staple.GetDescription());
                punch_ComboBox.SelectedIndex = punch_ComboBox.FindStringExact(printerPreferenceData.Punch.GetDescription());
                fold_ComboBox.SelectedIndex = fold_ComboBox.FindStringExact(printerPreferenceData.Fold.GetDescription());
                maxSheetPerSet_TextBox.Text = string.IsNullOrEmpty(PrinterPreferenceData.MaxSheetPerSet) ? "1" : printerPreferenceData.MaxSheetPerSet;
                outputBin_ComboBox.SelectedIndex = outputBin_ComboBox.FindStringExact(printerPreferenceData.OutputBin);
                // Job Storage
                off_RadioButton.Checked = printerPreferenceData.JobStorageOff ? true : false;
                proofAndHold_RadioButton.Checked = printerPreferenceData.ProofandHold ? true : false;
                personalJob_RadioButton.Checked = printerPreferenceData.PersonalJob ? true : false;
                storedJob_RadioButton.Checked = printerPreferenceData.StoredJob ? true : false;
                quickCopy_RadioButton.Checked = printerPreferenceData.QuickJob ? true : false;
                makeJob_ComboBox.SelectedIndex = makeJob_ComboBox.FindStringExact(printerPreferenceData.MakeJobPrivateSecure);
                makeJob_ComboBox.SelectedIndex = makeJob_ComboBox.FindStringExact(printerPreferenceData.MakeJobPrivateSecure);
                if (makeJob_ComboBox.SelectedIndex == 1)
                    pin_TextBox.Text = printerPreferenceData.PinNumber.ToString();
                if (makeJob_ComboBox.SelectedIndex == 2)
                {
                    password_TextBox.Text = printerPreferenceData.Password;
                    confirm_TextBox.Text = printerPreferenceData.ConfirmPassword;
                    saveDefaultPass_CheckBox.Checked = printerPreferenceData.SaveAsDefaultPass;
                }
                userName_RadioButton.Checked = printerPreferenceData.UserName ? true : false;
                custom_RadioButton.Checked = printerPreferenceData.Custom ? true : false;
                custom_TextBox.Text = string.IsNullOrEmpty(printerPreferenceData.CustomText) ? string.Empty : printerPreferenceData.CustomText;
                automatic_RadioButton.Checked = printerPreferenceData.JobNameAutomatic ? true : false;
                jobNameCustom_RadioButton.Checked = printerPreferenceData.JobNameCustom ? true : false;
                jobNameCustom_TextBox.Text = string.IsNullOrEmpty(printerPreferenceData.JobNameCustomText) ? string.Empty : printerPreferenceData.JobNameCustomText;
                jobNameExists_ComboBox.SelectedIndex = jobNameExists_ComboBox.FindStringExact(printerPreferenceData.JobNameExists);
                //Color Tab
                printInGrayScale_ComboBox.SelectedIndex = printInGrayScale_ComboBox.FindStringExact(printerPreferenceData.PrintInGrayScaleText.GetDescription());
                rgbColor_ComboBox.SelectedIndex = rgbColor_ComboBox.FindStringExact(printerPreferenceData.RGBColor.GetDescription());
                // Advance Tab
                noOfCopies_NumericUpDown.Value = Convert.ToDecimal(printerPreferenceData.Copies);
                collate_CheckBox.Checked = printerPreferenceData.Collate ? true : false;
                reversePageOrder_CheckBox.Checked = printerPreferenceData.ReversePageOrder ? true : false;
                printAllText_CheckBox.Checked = printerPreferenceData.PrintTextAsBlack ? true : false;
                hpEasyColor_CheckBox.Checked = printerPreferenceData.HPEasyColor ? true : false;
                edgeToEdge_CheckBox.Checked = printerPreferenceData.EdgeToEdge ? true : false;
                rasterCompression_ComboBox.SelectedIndex = rasterCompression_ComboBox.FindStringExact(printerPreferenceData.RasterCompression.GetDescription());
            }
        }

        private void Watermarks_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (watermarks_ComboBox.SelectedIndex > 0)
            {
                firstPageOnly_CheckBox.Enabled = true;
                customWaterMark_TextBox.Text = "";
            }
            else
            {
                firstPageOnly_CheckBox.Checked = false;
                firstPageOnly_CheckBox.Enabled = false;
            }
        }

        private void MakeJob_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (makeJob_ComboBox.SelectedIndex == 1)
            {
                password_Label.Visible = false;
                password_TextBox.Visible = false;
                confirm_Label.Visible = false;
                confirm_TextBox.Visible = false;
                passwordCharacteSizer_Label.Visible = false;
                fieldValidator.RequireValue(pin_TextBox, pin_Label);
                fieldValidator.Remove(password_TextBox);
                fieldValidator.Remove(confirm_TextBox);
                saveDefaultPass_CheckBox.Visible = false;
                pin_Label.Visible = true;
                pin_TextBox.Visible = true;
                pinLimit_Label.Visible = true;

            }
            else if (makeJob_ComboBox.SelectedIndex == 2)
            {
                pin_Label.Visible = false;
                pin_TextBox.Visible = false;
                pinLimit_Label.Visible = false;
                fieldValidator.Remove(pin_TextBox);
                fieldValidator.RequireValue(password_TextBox, password_Label);
                fieldValidator.RequireValue(confirm_TextBox, confirm_Label);
                password_Label.Visible = true;
                password_TextBox.Visible = true;
                confirm_Label.Visible = true;
                confirm_TextBox.Visible = true;
                passwordCharacteSizer_Label.Visible = true;
                saveDefaultPass_CheckBox.Visible = true;
            }
            else
            {
                pin_Label.Visible = false;
                pin_TextBox.Visible = false;
                pinLimit_Label.Visible = false;
                fieldValidator.Remove(password_TextBox);
                fieldValidator.Remove(confirm_TextBox);
                fieldValidator.Remove(pin_TextBox);
                password_Label.Visible = false;
                password_TextBox.Visible = false;
                confirm_Label.Visible = false;
                confirm_TextBox.Visible = false;
                passwordCharacteSizer_Label.Visible = false;
                saveDefaultPass_CheckBox.Visible = false;
            }
        }

        private void ResetJobStorageTxtBoxes()
        {
            password_TextBox.Text = string.Empty;
            confirm_TextBox.Text = string.Empty;
            pin_TextBox.Text = string.Empty;
        }

        private bool ValidatePassword(string password)
        {
            if (password.Length < 4 || password.Length > 32)
            {
                return false;
            }
            return true;
        }

        private bool ComparePassword(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return false;
            }
            return true;
        }
        private bool ValidatePinNumber(string pinNumber)
        {
            return pinNumber.Length < 4 ? false : true;
        }

        private bool ValidateComparePasswordLength(string comparePassword)
        {
            return comparePassword.Length < 4 ? false : true;
        }

        private void Pin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void Percentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void Percentage_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(percentage_TextBox.Text))
            {
                percentage_TextBox.Text = "25";
            }
            else if (Convert.ToInt32(percentage_TextBox.Text) > 400)
            {
                percentage_TextBox.Text = "400";
            }
            else if (Convert.ToInt32(percentage_TextBox.Text) < 25)
            {
                percentage_TextBox.Text = "25";
            }
        }

        private void password_TextBox_TextChanged(object sender, EventArgs e)
        {
            fieldValidator.RequireCustom(password_TextBox, () => ValidatePassword(password_TextBox.Text), "Please enter the correct Password");
            fieldValidator.RequireCustom(password_TextBox, () => ComparePassword(password_TextBox.Text, confirm_TextBox.Text), "Password and Confirm password does not match");
        }

        private void confirm_TextBox_TextChanged(object sender, EventArgs e)
        {
            fieldValidator.RequireCustom(password_TextBox, () => ValidatePassword(password_TextBox.Text), "Please enter the correct Password");
            fieldValidator.RequireCustom(password_TextBox, () => ComparePassword(password_TextBox.Text, confirm_TextBox.Text), "Password and Confirm password does not match");
        }

        private void pin_TextChanged(object sender, EventArgs e)
        {
            fieldValidator.RequireCustom(pin_TextBox, () => ValidatePinNumber(pin_TextBox.Text), "Please enter 4 digits PIN Number");
        }

        private void compare_TextBox_Leave(object sender, EventArgs e)
        {
            fieldValidator.RequireCustom(confirm_TextBox, () => ValidateComparePasswordLength(confirm_TextBox.Text), "Compare Password Length cannot be less than 4 characters");
            fieldValidator.Validate(confirm_TextBox);
        }

        private void password_TextBox_Leave(object sender, EventArgs e)
        {
            fieldValidator.RequireCustom(password_TextBox, () => ValidatePassword(password_TextBox.Text), "Password Length cannot be less than 4 characters");
            fieldValidator.Validate(password_TextBox);
            fieldValidator.RequireCustom(password_TextBox, () => ComparePassword(password_TextBox.Text, confirm_TextBox.Text), "Password and Confirm password does not match");
        }

        private void maxSheetPerSet_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            int n0 = (int)(e.KeyChar);
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
            if (n0 == 48)
            {
                e.Handled = true;
            }
        }

        private void fold_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fold_ComboBox.SelectedIndex == 0)
            {
                maxSheetsPerSet_Label.Visible = false;
                maxSheetPerSet_TextBox.Visible = false;
                maxsheet_Label.Visible = false;
                maxSheetPerSet_TextBox.Text = "";
            }
            else
            {
                PrinterPreferenceData.Fold = EnumUtil.GetByDescription<Fold>(fold_ComboBox.SelectedValue.ToString());
                maxSheetsPerSet_Label.Visible = true;
                maxSheetPerSet_TextBox.Visible = true;
                maxsheet_Label.Visible = true;
            }
            if (fold_ComboBox.SelectedIndex == 5 || fold_ComboBox.SelectedIndex == 6)
            {
                maxsheet_Label.Text = "(1-5)";
            }
            else
            {
                maxsheet_Label.Text = "(1-3)";
            }
        }

        private void noOfCopies_NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (noOfCopies_NumericUpDown.Value > 1)
            {
                collate_CheckBox.Enabled = true;
                collate_CheckBox.Checked = true;
            }
            else
            {
                collate_CheckBox.Checked = false;
                collate_CheckBox.Enabled = false;
            }
        }

        private void paperSize_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (paperSize_ComboBox.SelectedIndex > 0)
            {
                customPaperSizeName_TextBox.Text = "";
            }
        }

        private void enableCustomName_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (enableCustomName_CheckBox.Checked)
            {
                customPaperSizeName_TextBox.Enabled = true;
                fieldValidator.RequireValue(customPaperSizeName_TextBox, customPaperSizeName_Label, ValidationCondition.IfEnabled);
            }
            else
            {
                customPaperSizeName_TextBox.Enabled = false;
                customPaperSizeName_TextBox.Text = "";
                fieldValidator.Remove(customPaperSizeName_TextBox);
            }
        }

        private void enableCustomWatermark_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (enableCustomWatermark_CheckBox.Checked)
            {
                customWaterMark_TextBox.Enabled = true;
                firstPageOnly_CheckBox.Enabled = true;
                fieldValidator.RequireValue(customWaterMark_TextBox, customWaterMark_Label, ValidationCondition.IfEnabled);
            }
            else
            {
                customWaterMark_TextBox.Enabled = false;
                customWaterMark_TextBox.Text = "";
                if (watermarks_ComboBox.SelectedIndex > 0)
                {
                    firstPageOnly_CheckBox.Enabled = true;
                }
                else
                {
                    firstPageOnly_CheckBox.Enabled = false;
                }
                fieldValidator.Remove(customWaterMark_TextBox);
            }
        }

        private void maxSheetPerSet_TextBox_TextChanged(object sender, EventArgs e)
        {
            if ((int.Parse(string.IsNullOrEmpty(maxSheetPerSet_TextBox.Text) ? "0" : maxSheetPerSet_TextBox.Text) > 3) && (!(PrinterPreferenceData.Fold.GetDescription().Equals(Fold.InwardVfold.GetDescription().ToString()) || (PrinterPreferenceData.Fold.GetDescription().Equals(Fold.OutwardVfold.GetDescription().ToString())))))
            {
                maxSheetPerSet_TextBox.Text = "3";
            }
            else if ((int.Parse(string.IsNullOrEmpty(maxSheetPerSet_TextBox.Text) ? "0" : maxSheetPerSet_TextBox.Text) > 5) && (PrinterPreferenceData.Fold.GetDescription().Equals(Fold.InwardVfold.GetDescription().ToString()) || (PrinterPreferenceData.Fold.GetDescription().Equals(Fold.OutwardVfold.GetDescription().ToString()))))
            {
                maxSheetPerSet_TextBox.Text = "5";
            }
        }
    }
}
