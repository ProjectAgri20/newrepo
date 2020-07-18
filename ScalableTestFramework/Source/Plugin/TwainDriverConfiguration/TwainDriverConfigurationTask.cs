using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.ScalableTest.Plugin.TwainDriverConfiguration.UIMaps;
using TopCat.TestApi.GUIAutomation.Enums;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Utility;
using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.TwainDriverConfiguration
{
    /// <summary>
    /// Defines and implements the tasks to be performed by the TwainDriverConfiguration activity.
    /// </summary>
    public static class TwainDriverConfigurationTask
    {

        private const int ShortTimeout = 5;
        private const int WindowsTimeout = 20;
        private const int ShortWaitWindowsTimeout = 3000;
        private static HPScanTwain _hpscanConfiguration;
        private static SaveOperations _saveOperations;
        private static string _modelName;
        private static string ScanType => "Twain";

        /// <summary>
        /// Configuration Settings for Twain Scan
        /// </summary>
        /// <param name="twainDriver"></param> 
        /// <param name="deviceDetails"></param>   
        public static bool ConfigureSettings(IDevice deviceDetails, TwainDriverActivityData twainDriver)
        {
            // Get the common desktop directory
            var shortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
                "HP Scan Twain.lnk");
            if (File.Exists(shortcut))
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(shortcut)
                {
                    UseShellExecute = true,
                    Verb = "runas"
                };

                Process.Start(processInfo);
            }
            else
            {
                throw new Exception("Desktop shortcut not found");
            }

            _modelName = deviceDetails.GetDeviceInfo().ModelName;
            _hpscanConfiguration = new HPScanTwain(UIAFramework.ManagedUIA);
            _saveOperations = new SaveOperations(_modelName);
            CommunicationError communicationError = new CommunicationError(_modelName);
            try
            {
                Retry.WhileThrowing(() => _hpscanConfiguration.SaveasPDFListItem.IsAvailable(),
                                            15,
                                            TimeSpan.FromSeconds(3),
                                            new List<Type>() { typeof(Exception) });


                //Selecting the Configuration
                switch (twainDriver.TwainConfigurations)
                {
                    case TwainConfiguration.SavesAsPdf:
                        _hpscanConfiguration.SaveasPDFListItem.Select(WindowsTimeout);
                        _hpscanConfiguration.SaveasPDFListItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                        break;
                    case TwainConfiguration.SaveAsJpeg:
                        _hpscanConfiguration.SaveasJPEGListItem.Select(WindowsTimeout);
                        _hpscanConfiguration.SaveasJPEGListItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                        break;
                    case TwainConfiguration.EmailAsPdf:
                        _hpscanConfiguration.EmailasPDFListItem.Select(WindowsTimeout);
                        _hpscanConfiguration.EmailasPDFListItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                        break;
                    case TwainConfiguration.EmailAsJpeg:
                        _hpscanConfiguration.EmailasJPEGListItem.Select(WindowsTimeout);
                        _hpscanConfiguration.EmailasJPEGListItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                        break;
                    case TwainConfiguration.EveryDayScan:
                        _hpscanConfiguration.EverydayScanListItem.Select(WindowsTimeout);
                        _hpscanConfiguration.EverydayScanListItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                        break;
                    case TwainConfiguration.NewScanShortCut:
                        _hpscanConfiguration.CreateNewScanShButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                        break;
                }

                //Creating Shortcut
                if (twainDriver.TwainConfigurations.ToString() == TwainConfiguration.NewScanShortCut.ToString())
                {
                    //Creating Scan Shortcut Button                
                    CreateShortcut createShortcut = new CreateShortcut(_modelName);
                    Thread.Sleep(ShortWaitWindowsTimeout);
                    createShortcut.EnterthenameoftEdit.PerformHumanAction(x => x.ClickWithMouse(WindowsTimeout));
                    switch (twainDriver.ShortcutSettings)
                    {
                        case ShortcutSettings.SavesAsPdf:
                            createShortcut.EnterthenameoftEdit.PerformHumanAction(x => x.EnterText("SaveAsPDF_shortcut", WindowsTimeout));
                            createShortcut.NewScanShortcutComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                            createShortcut.SaveasPDFDup0ListItem.Select(WindowsTimeout);
                            break;
                        case ShortcutSettings.SaveAsJpeg:
                            createShortcut.EnterthenameoftEdit.PerformHumanAction(x => x.EnterText("SaveAsJPEG_shortcut", WindowsTimeout));
                            createShortcut.NewScanShortcutComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                            createShortcut.SaveasJPEGDup0ListItem.Select(WindowsTimeout);
                            break;
                        case ShortcutSettings.EmailAsPdf:
                            createShortcut.EnterthenameoftEdit.PerformHumanAction(x => x.EnterText("EmailAsPDF_shortcut", WindowsTimeout));
                            createShortcut.NewScanShortcutComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                            createShortcut.EmailasPDFDup0ListItem.Select(WindowsTimeout);
                            break;
                        case ShortcutSettings.EmailAsJpeg:
                            createShortcut.EnterthenameoftEdit.PerformHumanAction(x => x.EnterText("EmailAsJPEG_shortcut", WindowsTimeout));
                            createShortcut.NewScanShortcutComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                            createShortcut.EmailasJPEGDup0ListItem.Select(WindowsTimeout);
                            break;
                    }

                    createShortcut.CreateButton1Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    if (createShortcut.OKButton1Button.IsAvailable())
                    {
                        createShortcut.OKButton1Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                        throw new Exception("Shortcut already exists");
                    }

                    _hpscanConfiguration.ExitButton1007Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                }
                else
                {
                    //Enabling or disabling Reservation
                    JobReservationSettings(twainDriver);

                    //Selecting the Item Type
                    _hpscanConfiguration.ItemTypeComboBoComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    switch (twainDriver.ItemType)
                    {
                        case ItemType.Document:
                            _hpscanConfiguration.DocumentListItem.Select(WindowsTimeout);
                            break;
                        case ItemType.Photo:
                            _hpscanConfiguration.PhotoListItem.Select(WindowsTimeout);
                            break;

                    }

                    //Selecting the Page Size
                    _hpscanConfiguration.PageSizeComboBoComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    switch (twainDriver.PageSize)
                    {
                        case PageSize.DetectContentOnPage:
                            if (twainDriver.ItemType.ToString() == ItemType.Document.ToString())
                            {
                                _hpscanConfiguration.DetectContentonListItem.Select(WindowsTimeout);
                                break;
                            }
                            else
                            {
                                _hpscanConfiguration.DetectPhotosListItem.Select(WindowsTimeout);
                                break;
                            }

                        case PageSize.EntireScanArea:
                            _hpscanConfiguration.EntireScanAreaListItem.Select(WindowsTimeout);
                            break;
                        case PageSize.Letter11Inches:
                            _hpscanConfiguration.Letter85x11inchListItem.Select(WindowsTimeout);
                            break;
                        case PageSize.Legal14Inches:
                            _hpscanConfiguration.Legal85x14incheListItem.Select(WindowsTimeout);
                            break;
                        case PageSize.A4:
                            _hpscanConfiguration.A4210x297mmListItem.Select(WindowsTimeout);
                            break;
                        case PageSize.TwentyCrossTwentyFive:
                            _hpscanConfiguration.A8x10in20x25cmListItem.Select(WindowsTimeout);
                            break;
                    }

                    //Selecting the Source                    
                    _hpscanConfiguration.SourceComboBox1ComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    switch (twainDriver.Source)
                    {
                        case Source.DocumentFeederifloaded:
                            if (twainDriver.IsReservation)
                            {
                                _hpscanConfiguration.AutomaticDocumeListItem.Select(WindowsTimeout);
                                break;
                            }
                            else
                            {
                                _hpscanConfiguration.DocumentFeederiListItem.Select(WindowsTimeout);
                                break;
                            }
                        case Source.Flatbed:
                            _hpscanConfiguration.FlatbedListItem.Select(WindowsTimeout);
                            break;
                    }

                    //Selecting the Page Sides
                    _hpscanConfiguration.PageSidesComboBComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    switch (twainDriver.PageSides)
                    {
                        case PageSides.OneSided:
                            _hpscanConfiguration.A1sidedListItem.Select(WindowsTimeout);
                            break;
                        case PageSides.TwoSidedBook:
                            _hpscanConfiguration.A2sidedbookListItem.Select(WindowsTimeout);
                            break;
                        case PageSides.TwoSidedTablet:
                            _hpscanConfiguration.A2sidedtabletListItem.Select(WindowsTimeout);
                            break;
                    }

                    //Selecting the ColorMode
                    _hpscanConfiguration.ColorModeComboBComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    switch (twainDriver.ColorMode)
                    {
                        case ColorMode.BlackWhite:
                            _hpscanConfiguration.BlackWhiteListItem.Select(WindowsTimeout);
                            break;
                        case ColorMode.Gray:
                            _hpscanConfiguration.GrayListItem.Select(WindowsTimeout);
                            break;
                        case ColorMode.Halftone:
                            _hpscanConfiguration.HalftoneListItem.Select(WindowsTimeout);
                            break;
                        case ColorMode.Color:
                            _hpscanConfiguration.ColorListItem.Select(WindowsTimeout);
                            break;
                        case ColorMode.AutoDetectColor:
                            _hpscanConfiguration.AutoDetectColorListItem.Select(WindowsTimeout);
                            break;
                    }

                    //Selecting the Filetype and SendTo if the selected configuration is EveryDayScan
                    if (twainDriver.TwainConfigurations.ToString() == TwainConfiguration.EveryDayScan.ToString())
                    {
                        _hpscanConfiguration.FileTypeComboBoComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                        switch (twainDriver.FileType)
                        {
                            case FileType.Bmp:
                                _hpscanConfiguration.BMPListItem.Select(WindowsTimeout);
                                break;
                            case FileType.Jpeg:
                                _hpscanConfiguration.JPEGListItem.Select(WindowsTimeout);
                                break;
                            case FileType.Pdf:
                                _hpscanConfiguration.PDFListItem.Select(WindowsTimeout);
                                break;
                            case FileType.Png:
                                _hpscanConfiguration.PNGListItem.Select(WindowsTimeout);
                                break;
                            case FileType.Tif:
                                _hpscanConfiguration.TIFListItem.Select(WindowsTimeout);
                                break;
                        }

                        //Ensure the FileType is selected
                        Thread.Sleep(ShortWaitWindowsTimeout);

                        _hpscanConfiguration.SendToComboBox1ComboBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                        switch (twainDriver.SendTo)
                        {
                            case SendTo.Email:
                                _hpscanConfiguration.EmailListItem.Select(WindowsTimeout);
                                break;
                            case SendTo.LocalorNetworkfolder:
                                _hpscanConfiguration.LocalorNetworkfListItem.Select(WindowsTimeout);
                                break;
                        }
                    }

                    _hpscanConfiguration.ScanButton1012Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    if (communicationError.OKButton1Button.IsAvailable(WindowsTimeout))
                    {
                        throw new Exception(
                            "Scanner Communication cannot be established/Communication with the device ended prematurely");
                    }
                }

                return _hpscanConfiguration.PreparingtoscanText.IsAvailable();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException(ex.Message);
            }
        }

        public static void ScanOperation(TwainDriverActivityData twainDriver, string sessionId, string userName)
        {
            if ((twainDriver.TwainConfigurations.ToString() == TwainConfiguration.SavesAsPdf.ToString()) ||
                (twainDriver.TwainConfigurations.ToString() == TwainConfiguration.SaveAsJpeg.ToString()))
            {
                SaveToFolder(twainDriver.SharedFolderPath, sessionId, userName);
            }
            else if ((twainDriver.TwainConfigurations.ToString() == TwainConfiguration.EmailAsPdf.ToString()) ||
                     (twainDriver.TwainConfigurations.ToString() == TwainConfiguration.EmailAsJpeg.ToString()))
            {
                SendToEmail(twainDriver.EmailAddress, twainDriver.TwainConfigurations.ToString());
            }
            else
            {
                if (twainDriver.SendTo.ToString() == SendTo.LocalorNetworkfolder.ToString())
                {
                    SaveToFolder(twainDriver.SharedFolderPath, sessionId, userName);
                }
                else
                {
                    SendToEmail(twainDriver.EmailAddress, twainDriver.TwainConfigurations.ToString());
                }
            }
        }

        private static void SaveToFolder(string folderDetails, string sessionId, string userName)
        {
            try
            {
                ScanFilePrefix filePrefix = new ScanFilePrefix(sessionId, userName, ScanType);
                //Maxmium retry is set to 500 in order to ensure scan is completed in case of a large document.
                Retry.WhileThrowing(() => _hpscanConfiguration.Button1000Button.IsEnabled(),
                                            500,
                                            TimeSpan.FromSeconds(3),
                                            new List<Type>() { typeof(Exception) });
                Thread.Sleep(ShortWaitWindowsTimeout);
                _hpscanConfiguration.Button1000Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                _saveOperations.DestinationButtButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                _saveOperations.FileTypeButton1Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                _saveOperations.ScanEdit1869Edit.PerformHumanAction(x => x.EnterText(filePrefix.ToString().ToLowerInvariant(), WindowsTimeout));
                _saveOperations.OKButton1Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                _saveOperations.SendToEdit1159Edit.PerformHumanAction(x => x.EnterText(folderDetails, WindowsTimeout));
                _saveOperations.DoNothingButtonRadioButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                //Checking for Incorrect Folder Path
                SaveToFolderPath saveToFolderPath = new SaveToFolderPath(_modelName);
                if (saveToFolderPath.OKButton1Button.IsEnabled(ShortTimeout))
                {
                    throw new Exception("Save To Folder Path is Incorrect");
                }
                _saveOperations.ShowSaveAsdialoCheckBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                _hpscanConfiguration.Button1000Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                Retry.WhileThrowing(() => _hpscanConfiguration.SaveButton1006Button.IsEnabled(),
                                            10,
                                            TimeSpan.FromSeconds(3),
                                            new List<Type>() { typeof(Exception) });
                Thread.Sleep(ShortWaitWindowsTimeout);
                _hpscanConfiguration.SaveButton1006Button.PerformHumanAction(x => x.Click(WindowsTimeout));
                if (_saveOperations.YesButton1ButtonReplace.IsAvailable())
                {
                    _saveOperations.YesButton1ButtonReplace.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                }
                ExitTwain();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException(ex.Message);
            }
        }

        private static void SendToEmail(string emailAddress, string twainConfiguration)
        {
            try
            {
                if (twainConfiguration == TwainConfiguration.EveryDayScan.ToString())
                {
                    //Maxmium retry is set to 500 in order to ensure scan in case of a large document is completed
                    Retry.WhileThrowing(() => _hpscanConfiguration.Button1000Button.IsEnabled(),
                                            500,
                                            TimeSpan.FromSeconds(3),
                                            new List<Type>() { typeof(Exception) });
                    Thread.Sleep(ShortWaitWindowsTimeout);
                    _hpscanConfiguration.Button1000Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    _saveOperations.DestinationButtButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    _saveOperations.SavealocalcopyBCheckBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                }

                //Maxmium retry is set to 200 in order to ensure scan is completed
                Retry.WhileThrowing(() => _hpscanConfiguration.SendButton1006Button.IsEnabled(),
                                            200,
                                            TimeSpan.FromSeconds(3),
                                            new List<Type>() { typeof(Exception) });
                Thread.Sleep(ShortWaitWindowsTimeout);
                _hpscanConfiguration.SendButton1006Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));

                //Sending Email
                Outlook outlookConfiguration = new Outlook(_modelName);
                if (!outlookConfiguration.SendButton4256Button.IsAvailable(WindowsTimeout))
                {
                    throw new Exception("The outlook window did not pop out in the alloted time");
                }
                SendKeys.SendWait(emailAddress);
                Thread.Sleep(ShortWaitWindowsTimeout);
                outlookConfiguration.SubjectRichEditEdit.PerformHumanAction(x => x.EnterText("Twain Attachment", WindowsTimeout));
                outlookConfiguration.SendButton4256Button.WaitForAvailable();
                outlookConfiguration.SendButton4256Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                ExitTwain();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException(ex.Message);
            }
        }

        private static void JobReservationSettings(TwainDriverActivityData twainDriver)
        {
            _hpscanConfiguration.Button1903Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
            string machineName = Environment.MachineName;
            JobReservation jobReservation = new JobReservation(_modelName, machineName);

            if (twainDriver.IsReservation)
            {
                if (!jobReservation.UsePINButton190CheckBox.IsEnabled())
                {
                    jobReservation.EnableReservatiCheckBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                }

                if (twainDriver.IsPinRequired)
                {
                    if (!jobReservation.JobNameHOLLAEdiEdit.IsEnabled())
                    {
                        jobReservation.UsePINButton190CheckBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    }
                    jobReservation.JobNameHOLLAEdiEdit.PerformHumanAction(x => x.EnterText(twainDriver.Pin, WindowsTimeout));
                }
                else
                {
                    if (jobReservation.JobNameHOLLAEdiEdit.IsEnabled())
                    {
                        jobReservation.UsePINButton190CheckBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                    }
                }
            }
            else
            {
                if (jobReservation.UsePINButton190CheckBox.IsEnabled())
                {
                    jobReservation.EnableReservatiCheckBox.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                }
            }

            jobReservation.OKButton1156Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
        }

        private static void ExitTwain()
        {
            try
            {
                Retry.WhileThrowing(() => _hpscanConfiguration.ExitButton1007Button.IsEnabled(),
                                        10,
                                        TimeSpan.FromSeconds(3),
                                        new List<Type>() { typeof(Exception) });
                _hpscanConfiguration.ExitButton1007Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                if (_saveOperations.NoButton1845Button.IsAvailable(WindowsTimeout))
                {
                    _saveOperations.NoButton1845Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Exit button did not appear within the alloted time {ex.Message}");
            }
        }
    }
}
