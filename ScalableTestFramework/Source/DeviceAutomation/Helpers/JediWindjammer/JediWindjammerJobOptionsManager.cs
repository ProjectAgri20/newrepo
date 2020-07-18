using System;
using System.Linq;
using HP.DeviceAutomation.Jedi;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer
{
    /// <summary>
    /// Manages advanced job options on a <see cref="JediWindjammerDevice" />.
    /// </summary>
    public class JediWindjammerJobOptionsManager
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly string _appMainForm;

        /// <summary>
        /// Gets or sets the pacekeeper for this options manager.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; } = new Pacekeeper(TimeSpan.Zero);

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerJobOptionsManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="appMainForm">The name of the main form of the associated device application.</param>
        public JediWindjammerJobOptionsManager(JediWindjammerDevice device, string appMainForm)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _appMainForm = appMainForm;
        }

        /// <summary>
        /// Selects the file type for the scanned file.
        /// </summary>
        /// <param name="fileType">The file type to select (case sensitive).</param>
        public void SelectFileType(string fileType)
        {
            //var html = _controlPanel.GetControls();
            ScrollToOption("FileTypeDialogButton");
            Pacekeeper.Pause();

            if (_controlPanel.GetProperty("FileTypeDialogButton", "Text") != fileType)
            {
                _controlPanel.PressToNavigate("FileTypeDialogButton", "DSFileTypeDialog", ignorePopups: false);
                Pacekeeper.Pause();
                _controlPanel.ScrollPress("mFileTypeListBox", "Text", fileType);
                Pacekeeper.Sync();
                _controlPanel.PressToNavigate("m_OKButton", _appMainForm, ignorePopups: false);
                Pacekeeper.Pause();
            }
        }

        /// <summary>
        /// Selects the resolution for the selected file
        /// </summary>
        /// <param name="resolution">The resolution to select(case sensitive).</param>
        public void SelectResolution(ResolutionType resolution)
        {
            throw new NotImplementedException($"Unable to set job resolution to resolution:{resolution}. Resolution Selection is not implemented on JediWindjammer devices.");
        }

        /// <summary>
        /// Selects the resolution for the selected file
        /// </summary>
        /// <param name="resolution">The resolution to select(case sensitive).</param>
        public void SelectFaxResolution(ResolutionType resolution)
        {
            throw new NotImplementedException($"Unable to set fax job resolution to resolution:{resolution}.Fax Resolution Selection is not implemented on JediWindjammer devices.");
        }

        /// <summary>
        /// Selects the Original Sides for the scanned file
        /// </summary>
        /// <param name="originalSides">The original Sides to select (case sensitive).</param>
        /// <param name="pageFlipUp">if set to <c>true</c>enable pageFlipUp ;otherwise,disable it</param>
        public void SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            throw new NotImplementedException($"SelectOriginalSides  with setting {originalSides} and pageflipup has been {(pageFlipUp == true ? "checked" : "not checked")}  feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Selects the Color/Black for the scanned file
        /// </summary>
        /// <param name="colorOrBlack">The Color/Black is selected.</param>
        public void SelectColorOrBlack(ColorType colorOrBlack)
        {
            throw new NotImplementedException($"SelectColor or Black  with setting {colorOrBlack} feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Selects the Original Size for the scanned file
        /// </summary>
        /// <param name="originalSize">The original Size to select (case sensitive).</param>
        public void SelectOriginalSize(OriginalSize originalSize)
        {
            throw new NotImplementedException($"SelectOriginalSize with setting {originalSize} feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Selects the Content Orientation  for the scanned file
        /// </summary>
        /// <param name="contentOrientation">content orientation.</param>
        public void SelectContentOrientation(ContentOrientation contentOrientation)
        {
            throw new NotImplementedException($"SelectContentOrientation with setting {contentOrientation} feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Set image adjustment for sharpness, darkness, contrast and Background cleanup
        /// </summary>
        /// <param name="imageAdjustSharpness">Sets the value for Image adjustment sharpness </param>
        /// <param name="imageAdjustDarkness">Sets the value for Image adjustment darkness </param>
        /// <param name="imageAdjustContrast">Sets the value for Image adjustment contrast </param>
        /// <param name="imageAdjustBackgroundCleanup">Sets the value for Image adjustment background cleanup </param>
        public void SetImageAdjustments(int imageAdjustSharpness, int imageAdjustDarkness, int imageAdjustContrast, int imageAdjustBackgroundCleanup)
        {
            throw new NotImplementedException($"SetImageadjustment feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Set image adjustment for sharpness, darkness, contrast and Background cleanup
        /// </summary>
        /// <param name="imageAdjustSharpness">Sets the value for Image adjustment sharpness </param>
        /// <param name="imageAdjustDarkness">Sets the value for Image adjustment darkness </param>
        /// <param name="imageAdjustContrast">Sets the value for Image adjustment contrast </param>
        /// <param name="imageAdjustBackgroundCleanup">Sets the value for Image adjustment background cleanup </param>
        /// <param name="automaticTone">if set to <c>true</c>enable sharpness only and disable all ;otherwise,enable all</param>
        public void SetImageAdjustments(int imageAdjustSharpness, int imageAdjustDarkness, int imageAdjustContrast, int imageAdjustBackgroundCleanup, bool automaticTone)
        {
            throw new NotImplementedException("SetImageadjustment feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Selects the Optimize text or pitcure  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">Selects the text or pitcure</param>
        public void SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
        {
            ScrollToOption("CopyOptimizeTextPictureDialogButton");
            if (_controlPanel.GetControls().Contains("CopyOptimizeTextPictureDialogButton"))
            {
                _controlPanel.PressWait("CopyOptimizeTextPictureDialogButton", "CopyOptimizeTextPictureDialog");
                var controls = _controlPanel.GetControls().ToList();
                if (controls.Contains("m_RadioButton"))
                {
                    switch (optimizeTextOrPicture)
                    {
                        case OptimizeTextPic.Mixed:
                            _controlPanel.Press("Mixed");
                            break;
                        case OptimizeTextPic.Photo:
                            _controlPanel.Press("Photo");
                            break;
                        case OptimizeTextPic.Text:
                            _controlPanel.Press("Text");
                            break;
                        case OptimizeTextPic.Glossy:
                        default:
                            _controlPanel.Press("Glossy");
                            break;
                    }
                    _controlPanel.Press("m_OKButton");
                }
            }
        }


        /// <summary>
        /// Sets the Front and back erase edges for Top, Bottom, Left and Right
        /// </summary>
        /// <param name="eraseEdgeList">List cotaining the Key-value pair for the edge. Key, is the erase edge type enum and value is the string to be set for the element. Edges value must be a non empty, mumeric and a non zero string. Zeros will be ignored</param>
        /// <param name="applySameWidth">When set to <c>True</c> it applies same width for all edges for front; otherwise individual widths are applied</param>
        /// <param name="mirrorFrontSide">When set to <c>true</c>, back side will mirror the front side </param>
        /// <param name="useInches">If set to <c>true</c> inches will be used otherwise edges are set in milimeter </param>
        public void SetEraseEdges(Dictionary<EraseEdgesType, decimal> eraseEdgeList, bool applySameWidth, bool mirrorFrontSide, bool useInches)
        {
            throw new NotImplementedException("SetErase edges feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Selects the Crop Option for the scanned file
        /// </summary>
        /// <param name="cropOption">crop optoin type to select(case sensitive)</param>
        public void SelectCropOption(Cropping cropOption)
        {
            throw new NotImplementedException($"SelectCropOption with setting {cropOption} feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Selects the Blank Page Supress for scanned file
        /// </summary>
        /// <param name="optionType"> blank page supress to select(case sensitive)</param>
        public void SelectBlankPageSupress(BlankPageSupress optionType)
        {
            throw new NotImplementedException($"SelectBlankPageSupress with setting {optionType} feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Selects the multiple files setting for the scanned file
        /// </summary>
        /// <param name="isChecked">isCreatemultipleFiles selected<value><c>true</c>if it is checked;otherwise,<c>false</c></value></param>
        /// <param name="pagesPerFile">pages per file value.</param>
        public void CreateMultipleFiles(bool isChecked, string pagesPerFile)
        {
            throw new NotImplementedException("CreateMultipleFiles feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Selects the signing or encrypt option for the scanned file
        /// </summary>
        /// <param name="sign">if set to <c>true</c> enable signing.</param>
        /// <param name="encrypt">if set to <c>true</c> enable encryption.</param>
        public void SelectSigningAndEncrypt(bool sign, bool encrypt)
        {
            throw new NotImplementedException("SelectSigningAndEncrypt feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Set the Scan mode
        /// </summary>
        /// <param name="scanModeSelected">Sets the selected scan mode specified in the ScanMode enum</param>
        public virtual void SetScanMode(ScanMode scanModeSelected)
        {
            throw new NotImplementedException("Set scan mode feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Set the booklet format to On-Off
        /// </summary>
        /// <param name="bookletformat">If set to <c>true</c> booklet format is enabaled; otherwise its disabled</param>
        /// <param name="borderOnPage">If set to <c>true</c> border on page is set to true; else it is false</param>                                     
        public virtual void SetBooklet(bool bookletformat, bool borderOnPage)
        {
            throw new NotImplementedException("Set booklet mode feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Sets the state of the Job Build option.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> enable Job Build; otherwise, disable it.</param>
        public void SetJobBuildState(bool enable)
        {
            ScrollToOption("JobBuildDialogButton");
            Pacekeeper.Pause();

            if (_controlPanel.GetProperty("JobBuildDialogButton", "Text") != (enable ? "On" : "Off"))
            {
                _controlPanel.PressToNavigate("JobBuildDialogButton", "JobBuildDialog", ignorePopups: false);
                Pacekeeper.Pause();
                _controlPanel.Press(enable ? "Enabled" : "Disabled");
                Pacekeeper.Sync();
                _controlPanel.PressToNavigate("m_OKButton", _appMainForm, ignorePopups: false);
                Pacekeeper.Pause();
            }
        }

        /// <summary>
        /// Enables print notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>//We have added thumbnail parameter but not yet implemented it
        public void EnablePrintNotification(NotifyCondition condition, bool thumbNail)
        {
            ScrollToOption("NotificationDialogButton");
            Pacekeeper.Pause();

            try
            {
                _controlPanel.PressToNavigate("NotificationDialogButton", "DSNotificationDialog", ignorePopups: false);
            }
            catch (WindjammerInvalidOperationException)
            {
                // Form name is different on some firmware versions
                if (_controlPanel.CurrentForm() != "DSNotificationSettingsDialog")
                {
                    throw;
                }
            }
            Pacekeeper.Pause();
            _controlPanel.Press(condition == NotifyCondition.OnlyIfJobFails ? "AllErrors" : "Always");
            Pacekeeper.Sync();

            // Select the print option and exit
            _controlPanel.Press("Print");
            Pacekeeper.Sync();
            _controlPanel.PressToNavigate("m_OKButton", _appMainForm, ignorePopups: false);
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Enables email notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="address">The email address to receive the notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>//We have added thumbnail parameter but not yet implemented it
        public void EnableEmailNotification(NotifyCondition condition, string address, bool thumbNail)
        {
            ScrollToOption("NotificationDialogButton");
            Pacekeeper.Pause();

            string notificationDialogName = "DSNotificationDialog";
            try
            {
                _controlPanel.PressToNavigate("NotificationDialogButton", "DSNotificationDialog", ignorePopups: false);
            }
            catch (WindjammerInvalidOperationException)
            {
                // Form name is different on some firmware versions
                if (_controlPanel.CurrentForm() == "DSNotificationSettingsDialog")
                {
                    notificationDialogName = "DSNotificationSettingsDialog";
                }
                else
                {
                    throw;
                }
            }
            Pacekeeper.Pause();
            _controlPanel.Press(condition == NotifyCondition.OnlyIfJobFails ? "AllErrors" : "Always");
            Pacekeeper.Sync();

            // On some devices (digital senders) this button doesn't exist, since only email is available
            if (_controlPanel.GetControls().Contains("Email"))
            {
                _controlPanel.Press("Email");
                Pacekeeper.Sync();
            }

            // Enter the email address and exit
            _controlPanel.PressToNavigate("mEmailAddressTextBox", "DataDrivenTextEntryForm", ignorePopups: false);
            Pacekeeper.Pause();
            _controlPanel.TypeOnVirtualKeyboard("mKeyboard", address);
            Pacekeeper.Sync();
            _controlPanel.PressToNavigate("ok", notificationDialogName, ignorePopups: false);
            Pacekeeper.Pause();
            _controlPanel.PressToNavigate("m_OKButton", _appMainForm, ignorePopups: false);
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Scrolls to the specified option on the More Options menu.
        /// </summary>
        /// <param name="controlName">The control name.</param>
        public void ScrollToOption(string controlName)
        {
            string buttonName = "mFirstPageButton";

            if ("CopyAppMainForm" != _device.ControlPanel.CurrentForm())
            {
                buttonName = "mMoreOptionsButton";
            }

            //if (_controlPanel.GetProperty<bool>(controlName, "Visible"))
            //{
            //    return;
            //}
            // If the More Options button is visible, press it to get down into the options menu
            if (_controlPanel.GetProperty<bool>(buttonName, "Visible"))
            {
                _controlPanel.Press(buttonName);
            }
            // Move all the way down the list and back up to the top
            // When the Previous Page button disappears again, we're done
            bool movingDown = true;
            while (movingDown || _controlPanel.GetProperty<bool>("mPreviousPageButton", "Visible") == true)
            {
                // If the control is visible, we are done
                if (_controlPanel.GetControls().Contains(controlName) && _controlPanel.GetProperty<bool>(controlName, "Visible") == true)
                {
                    return;
                }
                else
                {
                    // Check to see if we need to turn around
                    if (movingDown && _controlPanel.GetProperty<bool>("mNextPageButton", "Enabled") == false)
                    {
                        movingDown = false;
                    }

                    // Press the button to go to the next page
                    _controlPanel.Press(movingDown ? "mNextPageButton" : "mPreviousPageButton");
                    // Scan options has no settings on the first screen except the more options button, whereas Copy has settings as well as the more options button
                    //The while logic fails to find the control in the FIRST screen while traversing pages in reverse order. It loops until the first screen and steps out as previous button is not visible. After which Device exception is thrown in the method end
                    if (_controlPanel.GetProperty<bool>(buttonName, "Visible") == true)
                    {
                        if (_controlPanel.GetControls().Contains(controlName) && _controlPanel.GetProperty<bool>(controlName, "Visible") == true)
                        {
                            return;
                        }
                    }
                }
            }

            throw new DeviceWorkflowException($"{controlName} was not found on the more options screen.");
        }

        /// <summary>
        /// Sets the coolor to print
        /// </summary>
        /// <param name="color">No of copies</param>
        public void SetColor(string color)
        {
            try
            {
                bool buttonexists = _device.ControlPanel.WaitForControl("ColorMonoModeDialogButton", TimeSpan.FromSeconds(1));

                if (buttonexists)
                {
                    //Selecting the Color Mode for Copy Job
                    if ("monochrome" == color || "color" == color)
                    {
                        _device.ControlPanel.PressToNavigate("ColorMonoModeDialogButton", "ColorBlackDialog", true);
                        _device.ControlPanel.Press(color);
                        _device.ControlPanel.Press("m_OKButton");
                    }
                }
            }

            catch (Exception ex)
            {
                throw new DeviceWorkflowException(string.Format("Set Color Failed with exception:{0}", ex.Message));
            }
        }

        /// <summary>
        /// Sets the number of copies to print
        /// </summary>
        /// <param name="copies">No of copies</param>
        public void SetNumCopies(int copies)
        {
            // select number of copies
            _device.ControlPanel.PressToNavigate("NumberOfCopies", "HPNumericKeypad", true);
            _device.ControlPanel.Type(copies.ToString());
            _device.ControlPanel.Press("mButtonOK");

        }

        /// <summary>
        /// Performs Copy of Photo with Enlarge/reduction option
        /// </summary>       
        /// <param name="reduceEnlargeOption">if set to <c>true</c> Manual is set ; otherwise Automatic is set</param>
        /// <param name="includeMargin">It set to <c>true</c> enable Include margin; otherwise it is disabled</param>
        /// <param name="zoomSize">Value of the Zoom size, range between 25-100%</param>
        public void SetReduceEnlarge(bool reduceEnlargeOption, bool includeMargin, int zoomSize)
        {
            throw new NotImplementedException("SetReduceEnlarge feature is not implemented on JediWindjammer devices");
        }

        /// <summary>
        /// Sets the value for Collate option.
        /// </summary>
        /// <param name="collate">Either set the job to collate or disbale it</param>
        public virtual void SetCollate(bool collate)
        {
            throw new NotImplementedException("Setcollate fetaure is not implemented on Jediwindjammer devices");
        }

        /// <summary>
        /// Sets the output to either Normal or EdgeToEdge
        /// </summary>
        /// <param name="edgetoedge">if set to <c>true</c> set Edge to edge , otherwise disable it</param>
        public virtual void SetEdgeToEdge(bool edgetoedge)
        {
            throw new NotImplementedException("Setedegetoedge feature is not implemented on Jediwindjammer devices");
        }

        /// <summary>
        /// Set the Pages per Sheet
        /// </summary>
        /// <param name="pages">Set the pages per sheet for the document using the PagesPerSheet enum </param>
        /// <param name="addPageBorders">If set to <c>True</c> Page borders are added ; otherwise page borders are not added </param>
        public virtual void SetPagesPerSheet(PagesPerSheet pages, bool addPageBorders)
        {
            throw new NotImplementedException("Setpagespersheet feature is not implemented on Jediwindjammer devices");
        }

        ///<summary>
        /// Set Paper selection for Size, type and tray 
        /// </summary>
        /// <param name="paperSize">Sets the paper size </param>
        /// <param name="paperType">Sets the paper type</param>
        /// <param name="paperTray">Sets the paper tray to use </param>
        public virtual void SetPaperSelection(PaperSelectionPaperSize paperSize, PaperSelectionPaperType paperType, PaperSelectionPaperTray paperTray)
        {
            throw new NotImplementedException("Setpaperselection feature is not implemented on Jediwindjammer devcies");
        }


        /// <summary>
        /// Sets value for Sides Option (Original and Output and Page flips up)
        /// </summary>       
        /// <param name="originalOnesided">if set to <c>true</c> Orignal side is set to 1-sided ; otherwise 2-sided</param>
        /// <param name="outputOnesided">if set to <c>true</c> Output side is set to 1-sided ; otherwise 2-sided</param>
        /// <param name="originalPageFlipUp">If set to <c>true</c> Page flip is enabled; otherwise it is disabled</param>
        /// <param name="outputPageFlipUp">If set to <c>true</c> Output page flip is enabled ; otherwise it is disabled</param>
        public virtual void SetSides(bool originalOnesided, bool outputOnesided, bool originalPageFlipUp, bool outputPageFlipUp)
        {
            throw new NotImplementedException("Setsides feature is not implemented on Jediwindjammer devices");
        }
    }
}
