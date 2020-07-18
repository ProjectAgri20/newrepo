using System;
using System.Text;
using System.Printing;
using HP.ScalableTest.Framework.Plugin;
using TopCat.TestApi.GUIAutomation;
using HP.ScalableTest.Plugin.DriverConfigurationPrint.UIMaps;
using System.Diagnostics;
using HP.ScalableTest.Plugin.DriverConfigurationPrint.Enum;
using HP.ScalableTest.Utility;
using HP.ScalableTest.PluginSupport.Print;
using System.Threading;
using TopCat.TestApi.GUIAutomation.Enums;

namespace HP.ScalableTest.Plugin.DriverConfigurationPrint
{
    /// <summary>
    /// DriverConfigurationPrint class Prints the Printer Preferences Settings.
    /// </summary>
    public class DriverConfigPrintingEngine : PrintingEngine
    {
        public StringBuilder _strContentToPrint = new StringBuilder();

        /// <summary>
        /// Occurs when the activity status changes.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusUpdate;
        private const int shortTimeout = 5;
        private const int windowsTimeout = 10;
        private const int screenWaitTimeout = 20;
        private static readonly TimeSpan humanTimeSpan = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Sets the driver settings using Topcat.
        /// </summary>
        /// <param name="printerPreferenceData"></param>
        /// <param name="queueName"></param>
        public void ConfigureDriverSettings(PrinterPreferenceData printerPreferenceData, string queueName)
        {
            TopCatUIAutomation.Initialize();

            //Launching the Printer preferences Window
            UpdateStatus("Launching the Printer preferences Window");
            Process launch = new Process();
            string command = $"/c rundll32 printui.dll,PrintUIEntry /e /n \"{queueName}\"";
            launch.StartInfo.Arguments = command;
            launch.StartInfo.Verb = "runas";
            launch.StartInfo.FileName = "cmd.exe";
            launch.StartInfo.CreateNoWindow = true;
            launch.StartInfo.UseShellExecute = false;
            launch.Start();
            PrintPreferences printPreferences = new PrintPreferences(queueName);

            if (printPreferences.DriverWindow.IsAvailable(screenWaitTimeout))
            {
                UpdateStatus("The Printer preferences window has been Launched");
                PaperQualityTab(printerPreferenceData, printPreferences);
                EffectsTab(printerPreferenceData, printPreferences);
                FinishingTab(printerPreferenceData, printPreferences);
                OutputTab(printerPreferenceData, printPreferences);
                JobStorageTab(printerPreferenceData, printPreferences);
                ColorTab(printerPreferenceData, printPreferences);
                AdvanceTab(printerPreferenceData, printPreferences);
                printPreferences.OKButtonOkButton.WaitForAvailable(windowsTimeout);
                printPreferences.OKButtonOkButton.PerformHumanAction(x => x.Click(windowsTimeout));
                //handling selections conflicts popup window
                if (printPreferences.SelectionsconflWindow.IsEnabled(windowsTimeout))
                {
                    printPreferences.YesButton6Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, windowsTimeout));
                }
            }
            else
            {
                throw new ArgumentException("The Queue name is not valid");
            }
        }

        private void PaperQualityTab(PrinterPreferenceData printerPreferenceData, PrintPreferences printPreferences)
        {

            if (printPreferences.PaperQualityTabTabItem.IsVisible())
            {
                UpdateStatus("The Paper/Quality Tab operations begins");
                //Print Tab Quality
                printPreferences.PaperQualityTabTabItem.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                //Paper Size             
                TopCatUiHelper.ComboBoxSetValue(printPreferences.ComboBoxcboPageDup0ComboBox, printerPreferenceData.PaperSize, shortTimeout);
                UpdateStatus($"The Print size option activity with type {printerPreferenceData.PaperSize} is set");
                //Paper Source          
                TopCatUiHelper.ComboBoxSetValue(printPreferences.ComboBoxcboDocuComboBox, printerPreferenceData.PaperSource, shortTimeout);
                UpdateStatus($"The Paper Sorce option activity with type {printerPreferenceData.PaperSource} is set");
                //Paper Type          
                TopCatUiHelper.ComboBoxSetValue(printPreferences.ComboBoxcboPageDup1ComboBox, printerPreferenceData.PaperType, shortTimeout);
                UpdateStatus($"The Paper Type option activity with type {printerPreferenceData.PaperType} is set");

                //Special Pages
                printPreferences.ListBoxlistSpecList.ClickWithMouse(windowsTimeout);
                try
                {
                    switch (printerPreferenceData.SpecialPages)
                    {
                        case SpecialPages.FrontCoverNoCovers:
                            printPreferences.FrontCoverNoCovListItem.Select(shortTimeout);
                            break;

                        case SpecialPages.BackCoverNoCovers:
                            printPreferences.BackCoverNoCoveListItem.Select(shortTimeout);
                            break;

                        case SpecialPages.PrintPagesondifferentPaper:
                            printPreferences.PrintpagesondifListItem.Select(shortTimeout);
                            break;

                        case SpecialPages.Insertblankorpreprintedsheets:
                            printPreferences.InsertblankorprListItem.Select(shortTimeout);
                            break;
                    }
                }
                catch
                {
                    throw new ArgumentException(printerPreferenceData.SpecialPages + " not found on the device ");
                }
                Thread.Sleep(humanTimeSpan);
                UpdateStatus($"The Special pages option activity with type {printerPreferenceData.SpecialPages} is set");
                //Print Quality 
                try
                {
                    printPreferences.ComboBoxcboResoComboBox.ClickWithMouse(windowsTimeout);
                    switch (printerPreferenceData.PrintQuality)
                    {
                        case PrintQuality.Normal:
                            printPreferences.NormalTextBlockDup1Text.ClickWithMouse(shortTimeout);
                            break;

                        case PrintQuality.FineLines:
                            printPreferences.FineLinesTextBlDup1Text.ClickWithMouse(shortTimeout);
                            break;

                        case PrintQuality.QuickView:
                            printPreferences.QuickViewTextBlDup1Text.ClickWithMouse(shortTimeout);
                            break;

                        case PrintQuality.FastRes:
                            printPreferences.FastRes1200TextDup1Text.ClickWithMouse(shortTimeout);
                            break;

                        case PrintQuality.A12001X200:
                            printPreferences.A1200x1200TextBDup1Text.ClickWithMouse(shortTimeout);
                            break;

                        case PrintQuality.GeneralOffice:
                            printPreferences.GeneralOfficeTeText.ClickWithMouse(shortTimeout);
                            break;

                        case PrintQuality.Professional:
                            printPreferences.ProfessionalTexText.ClickWithMouse(shortTimeout);
                            break;

                        case PrintQuality.Presentation:
                            printPreferences.PresentationTexText.ClickWithMouse(shortTimeout);
                            break;
                    }

                    Thread.Sleep(humanTimeSpan);
                    UpdateStatus($"The Print Quality option activity with type {printerPreferenceData.PrintQuality.GetDescription()} is set");
                }
                catch
                {
                    throw new ArgumentException(printerPreferenceData.PrintQuality + " not found on the device ");
                }
                UpdateStatus("The Paper/Quality Tab operations ends");
            }
        }

        private void EffectsTab(PrinterPreferenceData printerPreferenceData, PrintPreferences printPreferences)
        {
            //Effects Tab
            if (printPreferences.EffectsTabItemETabItem.IsVisible())
            {
                UpdateStatus("The Effect Tab operations begins");
                printPreferences.EffectsTabItemETabItem.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                if (printerPreferenceData.ActualSize)
                {
                    printPreferences.ActualsizeRadioRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                    UpdateStatus("The Actual Size option is selected");
                }
                else if (printerPreferenceData.PrintDocumentOn)
                {
                    printPreferences.PrintdocumentonRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                    UpdateStatus("The Print Documents On option is selected");
                    TopCatUiHelper.ComboBoxSetValue(printPreferences.ComboBoxcboPrinComboBox, printerPreferenceData.PrintDocumentOnValue, shortTimeout);
                    if (printerPreferenceData.ScaleToFit)
                    {
                        printPreferences.ScaletofitCheckCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                        UpdateStatus("The Scale to Fit option is enabled");
                    }
                    else
                    {
                        printPreferences.ScaletofitCheckCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
                    }
                }
                else
                {
                    printPreferences.ofactualsizeRadRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                    UpdateStatus("The Percentage of Actual Size option is selected");
                    printPreferences.TextBoxedZoomEdit.PerformHumanAction(x => x.EnterText(printerPreferenceData.PerActualSizeNo, shortTimeout));
                    UpdateStatus($"The Actual Size value {printerPreferenceData.PerActualSizeNo} is set");
                }

                TopCatUiHelper.ComboBoxSetValue(printPreferences.ComboBoxcboWateComboBox, printerPreferenceData.Watermark, shortTimeout);
                UpdateStatus($"The Watermark option {printerPreferenceData.Watermark} is set");
                if (printerPreferenceData.Watermark != WaterMark.none.GetDescription())
                {
                    if (printerPreferenceData.FirstPageOnly)
                    {
                        printPreferences.FirstpageonlyChCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                        UpdateStatus("The First Page Only option is enabled");
                    }
                    else
                    {
                        printPreferences.FirstpageonlyChCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
                    }
                }
                UpdateStatus("The Effect Tab operations ends");
            }
        }

        private void FinishingTab(PrinterPreferenceData printerPreferenceData, PrintPreferences printPreferences)
        {
            //Finishing Tab Item
            if (printPreferences.FinishingTabIteTabItem.IsVisible())
            {
                UpdateStatus("The Finishing Tab operations begins");
                printPreferences.FinishingTabIteTabItem.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));               
                if (printerPreferenceData.PrintOnBothSides)
                {
                    printPreferences.PrintonbothsideCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                    UpdateStatus("The Print On both side option is enabled");
                    TopCatUiHelper.ComboBoxSetValue(printPreferences.ComboBoxcboBookComboBox, printerPreferenceData.BookletLayout, shortTimeout);
                    UpdateStatus($"The BookletLayout option activity with type {printerPreferenceData.BookletLayout} is set");
                    if (printPreferences.FlipPagesUpChecCheckBox.IsEnabled())
                    {
                        if (printerPreferenceData.FlipPagesUp)
                        {
                            printPreferences.FlipPagesUpChecCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                            UpdateStatus("The flip page up option is enabled");
                        }
                        else
                        {
                            printPreferences.FlipPagesUpChecCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
                        }
                    }
                }
                else
                {
                    printPreferences.PrintonbothsideCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
                }

                if (printerPreferenceData.BookletLayout == BookletLayout.Off.GetDescription())
                {
                    printPreferences.ComboBoxcboDocuDup0ComboBox.Expand(windowsTimeout);
                    switch (printerPreferenceData.PagesPerSheet)
                    {
                        case PagesPerSheet.A1pagepersheet:
                            printPreferences.MonotypePrinterDup6I0XListItem.Select(windowsTimeout);
                            break;

                        case PagesPerSheet.A2pagespersheet:
                            printPreferences.MonotypePrinterDup7I1XListItem.Select(windowsTimeout);
                            break;

                        case PagesPerSheet.A4pagespersheet:
                            printPreferences.MonotypePrinterDup8I2XListItem.Select(windowsTimeout);
                            break;

                        case PagesPerSheet.A6pagespersheet:
                            printPreferences.MonotypePrinterDup9I3XListItem.Select(windowsTimeout);
                            break;

                        case PagesPerSheet.A9pagespersheet:
                            printPreferences.MonotypePrinterDup10I4XListItem.Select(windowsTimeout);
                            break;

                        case PagesPerSheet.A16pagespersheet:
                            printPreferences.MonotypePrinterDup11I5XListItem.Select(windowsTimeout);
                            break;
                    }
                    printPreferences.ComboBoxcboDocuDup0ComboBox.Collapse(windowsTimeout);
                    Thread.Sleep(humanTimeSpan);
                    UpdateStatus($"The PagesPerSheet option activity with type {printerPreferenceData.PagesPerSheet.GetDescription()} is set");

                    if (printerPreferenceData.PagesPerSheet != PagesPerSheet.A1pagepersheet)
                    {
                        if (printerPreferenceData.PrintPageBorders)
                        {
                            printPreferences.PrintpageborderCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                            UpdateStatus("The Print page border option is enabled");
                        }
                        else
                        {
                            printPreferences.PrintpageborderCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
                        }

                        printPreferences.ComboBoxcboJobPComboBox_Finishing.Expand(windowsTimeout);
                        switch (printerPreferenceData.PageOrder)
                        {
                            case PageOrder.RightthenDown:
                                printPreferences.MonotypePrinterDup10I0XListItem.Select(windowsTimeout);
                                break;

                            case PageOrder.Confidential:
                                printPreferences.MonotypePrinterDup11I1XListItem.Select(windowsTimeout);
                                break;

                            case PageOrder.LeftthenDown:
                                printPreferences.MonotypePrinterDup12I2XListItem.Select(windowsTimeout);
                                break;

                            case PageOrder.DownthenLeft:
                                printPreferences.MonotypePrinterDup13I3XListItem.Select(windowsTimeout);
                                break;
                        }
                        printPreferences.ComboBoxcboJobPComboBox_Finishing.Collapse(windowsTimeout);
                        Thread.Sleep(humanTimeSpan);
                        UpdateStatus($"The Page Order option activity with type {printerPreferenceData.PageOrder.GetDescription()} is set");
                    }
                }

                if (printerPreferenceData.OrientationPortrait)
                {
                    printPreferences.PortraitRadioBuRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                    UpdateStatus("The Portrait option is selected");
                }
                else
                {
                    printPreferences.LandscapeRadioBRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                    UpdateStatus("The Landscape option is selected");
                }

                if (printerPreferenceData.RotateBy180Degree)
                {
                    printPreferences.Rotateby180degrCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                    UpdateStatus("The Rotation option is enabled");
                }
                else
                {
                    printPreferences.Rotateby180degrCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
                }
                UpdateStatus("The Finishing Tab operations ends");
            }
        }

        private void OutputTab(PrinterPreferenceData printerPreferenceData, PrintPreferences printPreferences)
        {
            //Output Tab
            if (printPreferences.OutputTabItemOuTabItem.IsVisible())
            {
                UpdateStatus("The Output Tab operations begins");

                printPreferences.OutputTabItemOuTabItem.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                try
                {
                    if (printPreferences.ComboBoxcboStapComboBox.IsEnabled() && printPreferences.ComboBoxcboStapComboBox.IsVisible())
                    {
                        TopCatUiHelper.ComboBoxSetValue(printPreferences.ComboBoxcboStapComboBox, printerPreferenceData.Staple.GetDescription(), shortTimeout);
                        UpdateStatus($"The Output Staple option activity with type {printerPreferenceData.Staple.GetDescription()} is set");
                    }
                }
                catch
                {
                    throw new ArgumentException(printerPreferenceData.Staple + " not found on the device ");
                }
                try
                {
                    if (printPreferences.ComboBoxcboHoleComboBox.IsEnabled() && printPreferences.ComboBoxcboHoleComboBox.IsVisible())
                    {
                        TopCatUiHelper.ComboBoxSetValue(printPreferences.ComboBoxcboHoleComboBox, printerPreferenceData.Punch.GetDescription(), shortTimeout);
                        UpdateStatus($"The Output Hole punch option activity with type {printerPreferenceData.Punch.GetDescription()} is set");
                    }
                }
                catch
                {
                    throw new ArgumentException(printerPreferenceData.Punch + " not found on the device ");
                }
                try
                {
                    if (printPreferences.ComboBoxcboFoldComboBox.IsEnabled() && printPreferences.ComboBoxcboFoldComboBox.IsVisible())
                    {
                        printPreferences.ComboBoxcboFoldComboBox.Expand(shortTimeout);
                        switch (printerPreferenceData.Fold)
                        {
                            case Fold.InwardCfoldOpensToLeftorUp:
                                printPreferences.InwardCfoldopenDup1I1XListItem.Select(shortTimeout);
                                break;

                            case Fold.InwardCfoldOpensToRightorDown:
                                printPreferences.InwardCfoldopenDup3I1XListItem.Select(shortTimeout);
                                break;

                            case Fold.OutwardCfoldOpensToLeftorUp:
                                printPreferences.OutwardCfoldopeDup1I1XListItem.Select(shortTimeout);
                                break;

                            case Fold.OutwardCfoldOpensToRightorDown:
                                printPreferences.OutwardCfoldopeDup3I1XListItem.Select(shortTimeout);
                                break;

                            case Fold.InwardVfold:
                                printPreferences.InwardVfoldListDup1I1XListItem.Select(shortTimeout);
                                break;

                            case Fold.OutwardVfold:
                                printPreferences.OutwardVfoldLisDup1I1XListItem.Select(shortTimeout);
                                break;

                            case Fold.None:
                                printPreferences.NoneListBoxItemDup2ListItem.Select(shortTimeout);
                                break;
                        }
                        printPreferences.ComboBoxcboFoldComboBox.Collapse(shortTimeout);
                        UpdateStatus($"The Output Fold  option activity with type {printerPreferenceData.Fold.GetDescription()} is set");
                    }
                }
                catch
                {
                    throw new ArgumentException(printerPreferenceData.Fold + " not found on the device ");
                }
                if (printPreferences.ComboBoxcboFoldComboBox.IsEnabled() && printPreferences.ComboBoxcboFoldComboBox.IsVisible())
                {
                    printPreferences.TextBoxedMaxSheEdit.PerformHumanAction(x => x.EnterText(printerPreferenceData.MaxSheetPerSet, shortTimeout));
                    UpdateStatus($"The Max sheets per set value {printerPreferenceData.MaxSheetPerSet} is set");
                }
                if (printPreferences.ComboBoxcboOutpComboBox.IsEnabled())
                {
                    TopCatUiHelper.ComboBoxSetValue(printPreferences.ComboBoxcboOutpComboBox, printerPreferenceData.OutputBin, shortTimeout);
                    UpdateStatus($"The output bin option activity with type {printerPreferenceData.OutputBin} is set");
                }
                UpdateStatus("The Output Tab operations ends");
            }
        }

        private void JobStorageTab(PrinterPreferenceData printerPreferenceData, PrintPreferences printPreferences)
        {
            //Job Storage Tab 
            if (printPreferences.JobStorageTabItTabItem.IsVisible())
            {
                UpdateStatus("The JobStorage Tab operations begins");
                printPreferences.JobStorageTabItTabItem.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                if (printerPreferenceData.JobStorageOff)
                {
                    printPreferences.OffRadioButtonrRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                    UpdateStatus("The Job Storage Mode option Off is selected");
                }
                if (printerPreferenceData.ProofandHold)
                {
                    printPreferences.ProofandHoldRadRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                    UpdateStatus("The Job Storage Mode option Proof AND Hold is selected");
                }
                if (printerPreferenceData.PersonalJob)
                {
                    printPreferences.PersonalJobRadiRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                    UpdateStatus("The Job Storage Mode option Personal Job is selected");
                }
                if (printerPreferenceData.QuickJob)
                {
                    printPreferences.QuickCopyRadioBRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                    UpdateStatus("The Job Storage Mode option Quick Copy is selected");
                }
                if (printerPreferenceData.StoredJob)
                {
                    printPreferences.StoredJobRadioBRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                    UpdateStatus("The Job Storage Mode option Stored Job is selected");
                }

                if (printerPreferenceData.PersonalJob || printerPreferenceData.StoredJob)
                {
                    TopCatUiHelper.ComboBoxSetValue(printPreferences.ComboBoxcboJobPComboBox_JobStorage, printerPreferenceData.MakeJobPrivateSecure, shortTimeout);
                    UpdateStatus($"The Make Job Private Secure option activity with type {printerPreferenceData.MakeJobPrivateSecure} is set");
                    if (printerPreferenceData.MakeJobPrivateSecure == MakeJobPrivateSecure.PINtoprint.GetDescription())
                    {
                        printPreferences.PasswordBoxedPIDup0Edit.PerformHumanAction(x => x.EnterText(printerPreferenceData.PinNumber.ToString(), shortTimeout));
                        UpdateStatus("The Password is set");
                    }
                    else if (printerPreferenceData.MakeJobPrivateSecure == MakeJobPrivateSecure.EncryptJobwithpassword.GetDescription())
                    {
                        printPreferences.PasswordBoxedPaDup0Edit.PerformHumanAction(x => x.EnterText(printerPreferenceData.Password, shortTimeout));
                        UpdateStatus("The Password is set");
                        printPreferences.PasswordBoxedCoDup0Edit.PerformHumanAction(x => x.EnterText(printerPreferenceData.ConfirmPassword, shortTimeout));
                        UpdateStatus("The Confirm Password is set");
                        if (printerPreferenceData.SaveAsDefaultPass)
                        {
                            printPreferences.SaveasdefaultpaDup0CheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                            UpdateStatus("The Save as default is enabled");
                        }
                        else
                        {
                            printPreferences.SaveasdefaultpaDup0CheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
                        }
                    }
                }

                if (!printerPreferenceData.JobStorageOff)
                {
                    if (printerPreferenceData.UserName)
                    {
                        printPreferences.UserNameRadioBuRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                        UpdateStatus("The user name is enabled");
                    }
                    else
                    {
                        printPreferences.CustomRadioButtDup0RadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                        UpdateStatus($"The Custom name is selected");
                        printPreferences.TextBoxedUserNaEdit.PerformHumanAction(x => x.EnterText(printerPreferenceData.CustomText, shortTimeout));
                        UpdateStatus($"The Custom name {printerPreferenceData.CustomText} is set");
                    }
                    if (printerPreferenceData.JobNameAutomatic)
                    {
                        printPreferences.AutomaticRadioBRadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                        UpdateStatus("The Automatic Job name is enabled");
                    }
                    else
                    {
                        printPreferences.CustomRadioButtDup1RadioButton.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                        UpdateStatus($"The Custom job name is selected");
                        printPreferences.TextBoxedJobNamEdit.PerformHumanAction(x => x.EnterText(printerPreferenceData.JobNameCustomText, shortTimeout));
                        UpdateStatus($"The Custom job name {printerPreferenceData.JobNameCustomText} is set");
                    }
                    if (printerPreferenceData.JobNameExists == IfJobNameExists.UseJobName199.GetDescription())
                    {
                        printPreferences.ComboBoxcboJobNComboBox.Expand(windowsTimeout);
                        printPreferences.MonotypePrinterDup0I0XListItem.Select(shortTimeout);
                        printPreferences.ComboBoxcboJobNComboBox.Collapse(windowsTimeout);
                    }
                    else
                    {
                        printPreferences.ComboBoxcboJobNComboBox.Expand(windowsTimeout);
                        printPreferences.MonotypePrinterDup3I1XListItem1.Select(shortTimeout);
                        printPreferences.ComboBoxcboJobNComboBox.Collapse(windowsTimeout);
                    }
                    Thread.Sleep(humanTimeSpan);
                    UpdateStatus($"The option {printerPreferenceData.JobNameExists} is set");
                }
                UpdateStatus("The JobStorage Tab operations ends");
            }
        }

        private void ColorTab(PrinterPreferenceData printerPreferenceData, PrintPreferences printPreferences)
        {
            //Color Tab
            if (printPreferences.ColorTabItemColTabItem.IsVisible())
            {
                UpdateStatus($"The Color Tab operations begins");
                printPreferences.ColorTabItemColTabItem.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
                printPreferences.ComboBoxcboPrinComboBoxColor.WaitForVisible(shortTimeout);
                printPreferences.ComboBoxcboPrinComboBoxColor.ClickWithMouse(shortTimeout);
                switch (printerPreferenceData.PrintInGrayScaleText)
                {
                    case GrayScale.off:
                        printPreferences.OffListBoxItemListItem.Select(shortTimeout);
                        break;

                    case GrayScale.Blackonly:
                        printPreferences.BlackOnlyListBoListItem.Select(shortTimeout);
                        break;

                    case GrayScale.HighQualityCMYK:
                        printPreferences.HighQualityCMYKListItem.Select(shortTimeout);
                        break;

                    case GrayScale.On:
                        printPreferences.OnListBoxItemListItem.Select(shortTimeout);
                        break;
                }
                UpdateStatus($"The Print In GrayScale option { printerPreferenceData.PrintInGrayScaleText.ToString() } is set");
                printPreferences.ComboBoxcboRGBCComboBox.WaitForAvailable(shortTimeout);
                printPreferences.ComboBoxcboRGBCComboBox.Expand(shortTimeout);
                switch (printerPreferenceData.RGBColor)
                {
                    case RGBColor.DefaultsRGB:
                        printPreferences.MonotypePrinterDup6I0XListItemRGB.Select(shortTimeout);
                        break;

                    case RGBColor.PhotosRGB:
                        printPreferences.MonotypePrinterDup7I1XListItemRGB.Select(shortTimeout);
                        break;

                    case RGBColor.PhotoAdobeRGB1998:
                        printPreferences.MonotypePrinterDup8I2XListItemRGB.Select(shortTimeout);
                        break;

                    case RGBColor.VividsRGB:
                        printPreferences.MonotypePrinterDup9I3XListItemRGB.Select(shortTimeout);
                        break;

                    case RGBColor.None:
                        printPreferences.MonotypePrinterDup10I4XListItemRGB.Select(shortTimeout);
                        break;

                    case RGBColor.CustomProfile:
                        printPreferences.MonotypePrinterDup11I5XListItemRGB.Select(shortTimeout);
                        break;
                }
                printPreferences.ComboBoxcboRGBCComboBox.Collapse(shortTimeout);
                UpdateStatus($"The RGB color option {printerPreferenceData.RGBColor.ToString()} is set");
                Thread.Sleep(humanTimeSpan);
                UpdateStatus($"The Color Tab operations ends");
            }
        }

        private void AdvanceTab(PrinterPreferenceData printerPreferenceData, PrintPreferences printPreferences)
        {
            //Advanced Tab
            UpdateStatus("The Advanced Tab operations begins");
            printPreferences.AdvancedTabItemTabItem.PerformHumanAction(x => x.ClickWithMouse(shortTimeout));
            printPreferences.TextBoxedCopiesEdit.WaitForAvailable(shortTimeout);
            printPreferences.TextBoxedCopiesEdit.PerformHumanAction(x => x.EnterText(printerPreferenceData.Copies.ToString(), shortTimeout));
            UpdateStatus($"The Copy option value {printerPreferenceData.Copies} is set");
            if (printerPreferenceData.Copies > 1)
            {
                if (printerPreferenceData.Collate)
                {
                    printPreferences.CollateCheckBoxCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                    UpdateStatus("The collate option is enabled");
                }
                else
                {
                    printPreferences.CollateCheckBoxCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
                }
            }
            if (printerPreferenceData.ReversePageOrder)
            {
                printPreferences.ReversepageordeCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                UpdateStatus("The Reverse Page order option is enabled");
            }
            else
            {
                printPreferences.ReversepageordeCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
            }
            if (printerPreferenceData.PrintTextAsBlack)
            {
                printPreferences.PrintAllTextasBCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                UpdateStatus("The print text as black option is enabled");
            }
            else
            {
                printPreferences.PrintAllTextasBCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
            }
            if (printerPreferenceData.HPEasyColor)
            {
                printPreferences.HPEasyColorChecCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                UpdateStatus("The HP Easy Color option is enabled");
            }
            else
            {
                printPreferences.HPEasyColorChecCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
            }

            if (printPreferences.EdgetoEdgeCheckCheckBox.IsAvailable())
            {
                if (printerPreferenceData.EdgeToEdge)
                {
                    printPreferences.EdgetoEdgeCheckCheckBox.PerformHumanAction(x => x.Check(shortTimeout));
                    UpdateStatus("The Edge To Edge option is enabled");
                }
                else
                {
                    printPreferences.EdgetoEdgeCheckCheckBox.PerformHumanAction(x => x.Uncheck(shortTimeout));
                }
            }

            if (printPreferences.ComboBoxcboRastComboBox.IsVisible() && printPreferences.ComboBoxcboRastComboBox.IsEnabled())
            {
                printPreferences.ComboBoxcboRastComboBox.WaitForAvailable(shortTimeout);
                printPreferences.ComboBoxcboRastComboBox.Expand(shortTimeout);
                switch (printerPreferenceData.RasterCompression)
                {
                    case RasterCompression.Automatic:
                        printPreferences.MonotypePrinterDup3I0XListItemRast.Select(shortTimeout);
                        break;

                    case RasterCompression.BestQuality:
                        printPreferences.MonotypePrinterDup4I1XListItemRast.Select(shortTimeout);
                        break;

                    case RasterCompression.MaximumCompression:
                        printPreferences.MonotypePrinterDup5I2XListItemRast.Select(shortTimeout);
                        break;
                }
                UpdateStatus($"The Raster Compression option {printerPreferenceData.RasterCompression.ToString()} is set");
            }
            UpdateStatus("The Advanced Tab operations ends");
        }

        private void PrintPreferenceOptions(PrinterPreferenceData printerPreferenceData)
        {
            try
            {
                if (printerPreferenceData != null)
                {
                    _strContentToPrint.AppendLine("Printer Preferences");
                    _strContentToPrint.AppendLine();
                    _strContentToPrint.AppendLine($"Paper Size: {printerPreferenceData.PaperSize}");
                    _strContentToPrint.AppendLine($"Paper Quality: {EnumUtil.GetDescription(printerPreferenceData.PrintQuality)}");
                    _strContentToPrint.AppendLine($"Paper Type : {printerPreferenceData.PaperType}");
                    _strContentToPrint.AppendLine($"Paper Source: {printerPreferenceData.PaperSource}");
                    _strContentToPrint.AppendLine($"Special Pages: {printerPreferenceData.SpecialPages}");
                    _strContentToPrint.AppendLine($"Actual Size : {printerPreferenceData.ActualSize}");
                    _strContentToPrint.AppendLine($"Print Document On : {printerPreferenceData.PrintDocumentOn}");
                    _strContentToPrint.AppendLine($"Print Document On Value : {printerPreferenceData.PrintDocumentOnValue}");
                    _strContentToPrint.AppendLine($"Scale To Fit : {printerPreferenceData.ScaleToFit}");
                    _strContentToPrint.AppendLine($"Percentage Actual Size :{printerPreferenceData.PerActualSize}");
                    _strContentToPrint.AppendLine($"Percentage Actual Size in Number :{printerPreferenceData.PerActualSizeNo}");
                    _strContentToPrint.AppendLine($"Watermark :{printerPreferenceData.Watermark}");
                    _strContentToPrint.AppendLine($"First Page Only :{printerPreferenceData.FirstPageOnly}");

                    _strContentToPrint.AppendLine($"Print On Both Sides : {printerPreferenceData.PrintOnBothSides}");
                    _strContentToPrint.AppendLine($"Flip Pages Up : {printerPreferenceData.FlipPagesUp}");
                    _strContentToPrint.AppendLine($"Booklet Layout : {printerPreferenceData.BookletLayout}");
                    _strContentToPrint.AppendLine($"Pages Per Sheet : {EnumUtil.GetDescription(printerPreferenceData.PagesPerSheet)}");
                    _strContentToPrint.AppendLine($"Print Page Borders : {printerPreferenceData.PrintPageBorders}");
                    _strContentToPrint.AppendLine($"Page Order : {printerPreferenceData.PageOrder}");
                    _strContentToPrint.AppendLine($"Orientation Portrait : {printerPreferenceData.OrientationPortrait}");
                    _strContentToPrint.AppendLine($"Orientation Lanscape : {printerPreferenceData.OrientationLanscape}");
                    _strContentToPrint.AppendLine($"Rotate By 180 Degree : {printerPreferenceData.RotateBy180Degree}");
                    if (!printerPreferenceData.Staple.Equals(Staple.None))
                    {
                        _strContentToPrint.AppendLine($"Staple  :{EnumUtil.GetDescription(printerPreferenceData.Staple)}");
                    }
                    if (!printerPreferenceData.Punch.Equals(Punch.None))
                    {
                        _strContentToPrint.AppendLine($"Punch   : {EnumUtil.GetDescription(printerPreferenceData.Punch)}");
                    }
                    if (!printerPreferenceData.Fold.Equals(Fold.None))
                    {
                        _strContentToPrint.AppendLine($"Fold    : {EnumUtil.GetDescription(printerPreferenceData.Fold)}");
                    }
                    _strContentToPrint.AppendLine($"Max sheets per set    : {printerPreferenceData.MaxSheetPerSet}");
                    _strContentToPrint.AppendLine($"Output Bin : {printerPreferenceData.OutputBin}");

                    _strContentToPrint.AppendLine($"Job Storage Off : {printerPreferenceData.JobStorageOff}");
                    _strContentToPrint.AppendLine($"Proofand Hold  : {printerPreferenceData.ProofandHold}");
                    _strContentToPrint.AppendLine($"Personal Job  : {printerPreferenceData.PersonalJob}");
                    _strContentToPrint.AppendLine($"Quick Copy : {printerPreferenceData.QuickJob}");
                    _strContentToPrint.AppendLine($"Stored Job : {printerPreferenceData.StoredJob}");
                    _strContentToPrint.AppendLine($"Make Job Private Secure : {printerPreferenceData.MakeJobPrivateSecure}");
                    _strContentToPrint.AppendLine($"User Name : {printerPreferenceData.UserName}");
                    _strContentToPrint.AppendLine($"Custom  : {printerPreferenceData.Custom}");
                    if (!string.IsNullOrEmpty(printerPreferenceData.CustomText))
                        _strContentToPrint.AppendLine($"Custom Text : {printerPreferenceData.CustomText}");
                    _strContentToPrint.AppendLine($"JobName Automatic : {printerPreferenceData.JobNameAutomatic}");
                    _strContentToPrint.AppendLine($"JobName Custom : {printerPreferenceData.JobNameCustom}");
                    if (!string.IsNullOrEmpty(printerPreferenceData.JobNameCustomText))
                        _strContentToPrint.AppendLine($"JobName CustomText : {printerPreferenceData.JobNameCustomText}");
                    _strContentToPrint.AppendLine($"JobName Exists : {printerPreferenceData.JobNameExists}");
                    if (!string.IsNullOrEmpty(printerPreferenceData.PinNumber.ToString()))
                        _strContentToPrint.AppendLine($"Pin Number : {printerPreferenceData.PinNumber}");
                    if (!string.IsNullOrEmpty(printerPreferenceData.Password))
                        _strContentToPrint.AppendLine($"Password : {printerPreferenceData.Password}");
                    if (!string.IsNullOrEmpty(printerPreferenceData.ConfirmPassword))
                        _strContentToPrint.AppendLine($"Confirm Password : {printerPreferenceData.ConfirmPassword}");
                    _strContentToPrint.AppendLine($"Save As Default Password : {printerPreferenceData.SaveAsDefaultPass}");

                    _strContentToPrint.AppendLine($"Print in GrayScale : {EnumUtil.GetDescription(printerPreferenceData.PrintInGrayScaleText)}");
                    _strContentToPrint.AppendLine($"RGB Color : {EnumUtil.GetDescription(printerPreferenceData.RGBColor)}");

                    _strContentToPrint.AppendLine($"Copies : {printerPreferenceData.Copies}");
                    _strContentToPrint.AppendLine($"Collate : {printerPreferenceData.Collate}");
                    _strContentToPrint.AppendLine($"Reverse PageOrder : {printerPreferenceData.ReversePageOrder}");
                    _strContentToPrint.AppendLine($"Print Text as Black : {printerPreferenceData.PrintTextAsBlack}");
                    _strContentToPrint.AppendLine($"HP Easy Color : {printerPreferenceData.HPEasyColor}");
                    _strContentToPrint.AppendLine($"Edge To Edge : {printerPreferenceData.EdgeToEdge}");
                    _strContentToPrint.AppendLine($"Raster Compression : {EnumUtil.GetDescription(printerPreferenceData.RasterCompression)}");

                    _strContentToPrint.AppendLine();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This is an overridden method, from the Print Support PrintingEngine class
        /// PrintTag Virtual Method.
        /// </summary>
        /// <param name="printQueue"></param>
        /// <param name="executionData"></param>
        public override StringBuilder PrintTag(PrintQueue printQueue, PluginExecutionData executionData)
        {
            DriverConfigurationPrintActivityData data = executionData.GetMetadata<DriverConfigurationPrintActivityData>();
            PrintPreferenceOptions(data.PrinterPreference);
            _strContentToPrint.AppendLine();
            _strContentToPrint.AppendLine();
            _strContentToPrint.AppendLine($"UserName: {Environment.UserName}");
            _strContentToPrint.AppendLine($"Session ID: {executionData.SessionId}");
            _strContentToPrint.AppendLine($"Activity ID:{executionData.ActivityExecutionId}");
            _strContentToPrint.AppendLine($"Date: {DateTime.Now.ToShortDateString()}");
            _strContentToPrint.AppendLine($"Time: {DateTime.Now.ToShortTimeString()}");
            return _strContentToPrint;
        }

        /// <summary>
        /// Updates the status of the plugin execution.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void UpdateStatus(string status)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(status));
        }
    }
}
