using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JediOmni
{
    /// <summary>
    /// Manages advanced job options on a <see cref="JediOmniDevice" />.
    /// </summary>
    public class JediOmniJobOptionsManager : DeviceWorkflowLogSource
    {
        private readonly JediOmniControlPanel _controlPanel;

        protected readonly JediOmniDevice _device;
        protected readonly DeviceScreen _deviceScreen;

        /// <summary>
        /// Gets or sets the pacekeeper for this options manager.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; } = new Pacekeeper(TimeSpan.Zero);

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniJobOptionsManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniJobOptionsManager(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _deviceScreen = new DeviceScreen(device.ControlPanel);
            _controlPanel = _device.ControlPanel;
        }

        /// <summary>
        /// Selects the file type for the scanned file.
        /// </summary>
        /// <param name="fileType">The file type to select (case sensitive).</param>
        public void SelectFileType(string fileType)
        {
            OpenOptionsPanel();

            _controlPanel.ScrollPress("#hpid-option-file-type");
            if (_controlPanel.WaitForState("#hpid-file-type-button", OmniElementState.Exists, TimeSpan.FromSeconds(5)))
            {
                _controlPanel.ScrollPressWait("#hpid-file-type-button", "#hpid-file-type-list-selection");

                string fileTypeListItem = FindFileTypeListItem(fileType, $"#hpid-file-type-list-selection .hp-listitem:contains({fileType})");
                _controlPanel.WaitForAvailable(fileTypeListItem);
                _controlPanel.ScrollPressWait(fileTypeListItem, ".hp-button-done");
            }
            else
            {
                // Pre-24.6 firmware
                string fileTypeListItem = FindFileTypeListItem(fileType, $".hp-listitem:contains({fileType})");
                _controlPanel.WaitForAvailable(fileTypeListItem);
                _controlPanel.ScrollPressWait(fileTypeListItem, fileTypeListItem);
            }

            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        private string FindFileTypeListItem(string fileType, string listItemSelector)
        {
            var matchingIds = _controlPanel.GetIds(listItemSelector, OmniIdCollectionType.Self).Select(n => $"#{n}");

            switch (matchingIds.Count())
            {
                case 0:
                    throw new DeviceWorkflowException($"Could not find file type option for '{fileType}'.");

                case 1:
                    return matchingIds.First();

                default:
                    // If there is more than one match, find the one whose text is an exact match.
                    return matchingIds.First(n => fileType == _controlPanel.GetValue($"{n} .hp-listitem-text", "innerText", OmniPropertyType.Property));
            }
        }

        /// <summary>
        /// Sets the state of the Job Build option.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> enable Job Build; otherwise, disable it.</param>
        public void SetJobBuildState(bool enable)
        {
            // Job build is managed through job preview and does not need to be enabled explicitly.
            // This method must exist to fulfill interface implementation, but for Omni there is nothing to do.
        }

        /// <summary>
        /// Enables print notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        public void EnablePrintNotification(NotifyCondition condition, bool thumbNail)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-notification", "#hpid-option-notification-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select notification option.");
            }

            Pacekeeper.Pause();
            EnableNotification(condition, "#hpid-notification-mode-selection-print", thumbNail);
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Enables email notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="address">The email address to receive the notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        public void EnableEmailNotification(NotifyCondition condition, string address, bool thumbNail)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-notification", "#hpid-option-notification-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select notification option.");
            }

            Pacekeeper.Pause();
            EnableNotification(condition, "#hpid-notification-mode-selection-email", thumbNail);

            // Enter the email address
            _controlPanel.WaitForAvailable("#hpid-notify-textbox");
            _controlPanel.ScrollPress("#hpid-notify-textbox");
            Pacekeeper.Sync();

            // Clear any value already in the text box
            _controlPanel.Type(SpecialCharacter.Backspace);
            _controlPanel.TypeOnVirtualKeyboard(address);
            _controlPanel.PressWait("#hpid-keyboard-key-done", ".hp-button-done");
            Pacekeeper.Sync();

            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
            Pacekeeper.Pause();
        }

        private void EnableNotification(NotifyCondition condition, string notificationRadioButton, bool includeThumbnail)
        {
            string conditionSelector = condition == NotifyCondition.OnlyIfJobFails ? "#hpid-notification-condition-selection-allerrors" : "#hpid-notification-condition-selection-always";
            _controlPanel.ScrollPressWait(conditionSelector, notificationRadioButton);
            Pacekeeper.Sync();

            if (_controlPanel.CheckState(notificationRadioButton, OmniElementState.Exists))
            {
                _controlPanel.ScrollToItem(notificationRadioButton);
                if (_controlPanel.CheckState(notificationRadioButton, OmniElementState.Useable))
                {
                    _controlPanel.ScrollPress(notificationRadioButton);
                    SetCheckBoxState("#hpid-thumbnail-checkbox", includeThumbnail);
                    Pacekeeper.Sync();
                }
            }
        }

        /// <summary>
        /// Selects the resolution type for the selected file type
        /// </summary>
        /// <param name="resolution">The resolution type to select (case sensitive).</param>
        public void SelectResolution(ResolutionType resolution)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-file-type", "#hpid-file-type-resolution-button", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select file resolution option.");
            }

            _controlPanel.ScrollPressWait("#hpid-file-type-resolution-button", "#hpid-resolution-selection");
            Pacekeeper.Sync();

            _controlPanel.ScrollPressWait($"#hpid-resolution-selection-{resolution.ToString().ToLower()}", ".hp-button-done");

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        /// <summary>
        /// Selects the resolution type for the selected file type
        /// </summary>
        /// <param name="resolution">The resolution type to select (case sensitive).</param>
        public void SelectFaxResolution(ResolutionType resolution)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-fax-resolution", "#hpid-option-fax-resolution-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select fax resolution option.");
            }

            Pacekeeper.Sync();
            _controlPanel.ScrollPressWait($"#hpid-fax-resolution-selection-{resolution.ToString().ToLower()}", ".hp-option-list");
        }

        /// <summary>
        /// Selects the Original Sides for the scanned file
        /// </summary>
        /// <param name="originalSides">The original Sides to select (case sensitive).</param>
        /// <param name="pageFlipUp">if set to <c>true</c>enable pageFlipUp ;otherwise,disable it</param>
        public void SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            OpenOptionsPanel();

            if (_controlPanel.CheckState("#hpid-option-original-sides", OmniElementState.Useable))
            {
                if (!_controlPanel.ScrollPressWait("#hpid-option-original-sides", "#hpid-option-original-sides-screen", TimeSpan.FromSeconds(5)))
                {
                    throw new DeviceWorkflowException("Unable to select original sides option.");
                }

                switch (originalSides)
                {
                    case OriginalSides.Onesided:
                        _controlPanel.ScrollPress("#hpid-original-sides-selection-simplex");
                        break;

                    case OriginalSides.Twosided:
                        _controlPanel.ScrollPress("#hpid-original-sides-selection-duplex");
                        SetCheckBoxState("#hpid-flippagesup-checkbox", pageFlipUp);
                        break;
                }

                Pacekeeper.Sync();
                _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
            }
        }

        /// <summary>
        /// Selects the Color/Black for the scanned file
        /// </summary>
        /// <param name="colorOrBlack">The color/black is selected.</param>
        public void SelectColorOrBlack(ColorType colorOrBlack)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-color-black", "#hpid-option-color-black-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select color/black option.");
            }

            Pacekeeper.Sync();
            _controlPanel.ScrollPressWait($"#hpid-color-black-selection-{colorOrBlack.ToString().ToLower()}", ".hp-option-list");
        }

        /// <summary>
        /// Selects the Original Size for the scanned file
        /// </summary>
        /// <param name="originalSize">The original size to select (case sensitive).</param>
        public void SelectOriginalSize(OriginalSize originalSize)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-original-size", "#hpid-option-original-size-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select original size option.");
            }

            Pacekeeper.Sync();
            _controlPanel.ScrollPressWait($"#hpid-original-size-selection-{originalSize.ToString().ToLower()}", ".hp-option-list");
        }

        /// <summary>
        /// Selects the Content Orientation  for the scanned file
        /// </summary>
        /// <param name="contentOrientation">content orientation to select(case sensitive).</param>
        public void SelectContentOrientation(ContentOrientation contentOrientation)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-content-orientation", "#hpid-option-content-orientation-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select content orientation option.");
            }

            Pacekeeper.Sync();
            _controlPanel.ScrollPressWait($"#hpid-content-orientation-selection-{contentOrientation.ToString().ToLower()}", ".hp-option-list");
        }

        /// <summary>
        /// Selects the Optimize text or pitcure  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">the text or pitcure to select(case sensitive)</param>
        public void SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-optimize-text-picture", "#hpid-option-optimize-text-picture-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select optimize text/picture option.");
            }

            Pacekeeper.Sync();
            _controlPanel.ScrollPressWait($"#hpid-optimize-text-picture-selection-{optimizeTextOrPicture.ToString().ToLower()}", ".hp-option-list");
        }

        /// <summary>
        /// Selects the Crop Option for the scanned file
        /// </summary>
        /// <param name="cropOption">crop optoin type to select(case sensitive)</param>
        public void SelectCropOption(Cropping cropOption)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-auto-crop", "#hpid-option-auto-crop-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select cropping option.");
            }

            _controlPanel.ScrollPress($"#hpid-auto-crop-selection-{cropOption.ToString().ToLower()}");

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        /// <summary>
        /// Selects the Blank Page Supress for scanned file
        /// </summary>
        /// <param name="optionType"> blank page supress to select(case sensitive)</param>
        public void SelectBlankPageSupress(BlankPageSupress optionType)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-blank-page-suppression", "#hpid-option-blank-page-suppression-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select blank page suppression option.");
            }

            Pacekeeper.Sync();
            _controlPanel.ScrollPressWait($"#hpid-blank-page-suppression-selection-{optionType.ToString().ToLower()}", ".hp-option-list");
        }

        /// <summary>
        /// Selects the multiple files setting for the scanned file
        /// </summary>
        /// <param name="isChecked">isCreatemultipleFiles selected<value><c>true</c>if it is checked;otherwise,<c>false</c></value></param>
        /// <param name="pagesPerFile">pages per file value.</param>
        public void CreateMultipleFiles(bool isChecked, string pagesPerFile)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-create-multiple-files", "#hpid-option-create-multiple-files-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select create multiple files option.");
            }

            SetCheckBoxState("#hpid-create-multiple-files-checkbox", isChecked);
            if (isChecked)
            {
                SetNumericValue("#hpid-max-pages-per-file-textbox", pagesPerFile);
            }

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
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
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-erase-edges", "#hpid-option-erase-edges-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select erase edges option.");
            }

            // Clear settings, then set check boxes
            _controlPanel.ScrollPress("#hpid-settings-clear-button");

            SetCheckBoxState("#hpid-apply-same-width-checkbox", applySameWidth);
            Pacekeeper.Sync();
            SetCheckBoxState("#hpid-back-side-mirrors-front-checkbox", mirrorFrontSide);
            Pacekeeper.Sync();
            SetCheckBoxState("#hpid-use-inches-width-checkbox", useInches);
            Pacekeeper.Sync();

            // Fill in text values
            if (applySameWidth)
            {
                SetNumericValue("#hpid-all-front-edges-textbox", eraseEdgeList[EraseEdgesType.AllEdges].ToString());
            }
            else
            {
                foreach (EraseEdgesType edge in eraseEdgeList.Keys.Where(n => n != EraseEdgesType.AllEdges))
                {
                    if (!string.IsNullOrEmpty(eraseEdgeList[edge].ToString()))
                    {
                        string textBox = $"#hpid-{edge.GetDescription()}-edge-textbox";
                        if (!_controlPanel.CheckState(textBox, OmniElementState.Disabled))
                        {
                            SetNumericValue(textBox, eraseEdgeList[edge].ToString());
                            Pacekeeper.Sync();
                        }
                    }
                }
            }

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
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
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-image-adjustment", "#hpid-option-image-adjustment-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select image adjustment option.");
            }

            SetImageAdjustmentSliders(imageAdjustSharpness, imageAdjustDarkness, imageAdjustContrast, imageAdjustBackgroundCleanup);

            Pacekeeper.Pause();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
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
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-image-adjustment", "#hpid-option-image-adjustment-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select image adjustment option.");
            }

            SetCheckBoxState("#hpid-auto-tone-checkbox", automaticTone);
            Pacekeeper.Sync();

            if (automaticTone)
            {
                ScrollSetSliderValue("#hpid-sharpness-slider-input", imageAdjustSharpness);
            }
            else
            {
                SetImageAdjustmentSliders(imageAdjustSharpness, imageAdjustDarkness, imageAdjustContrast, imageAdjustBackgroundCleanup);
            }

            Pacekeeper.Pause();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        private void SetImageAdjustmentSliders(int imageAdjustSharpness, int imageAdjustDarkness, int imageAdjustContrast, int imageAdjustBackgroundCleanup)
        {
            ScrollSetSliderValue("#hpid-sharpness-slider-input", imageAdjustSharpness);
            ScrollSetSliderValue("#hpid-darkness-slider-input", imageAdjustDarkness);
            ScrollSetSliderValue("#hpid-contrast-slider-input", imageAdjustContrast);
            ScrollSetSliderValue("#hpid-background-cleanup-slider-input", imageAdjustBackgroundCleanup);
        }

        private void ScrollSetSliderValue(string slider, int value)
        {
            _controlPanel.ScrollToItem(slider);
            _controlPanel.SetSliderValue(slider, value);
        }

        /// <summary>
        /// Selects the signing or encrypt option for the scanned file
        /// </summary>
        /// <param name="sign">if set to <c>true</c> enable signing.</param>
        /// <param name="encrypt">if set to <c>true</c> enable encryption.</param>
        public void SelectSigningAndEncrypt(bool sign, bool encrypt)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-signed-encrypted", "#hpid-option-signed-encrypted-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select signing and encryption option.");
            }

            SetCheckBoxState("#hpid-signing-checkbox", sign);
            SetCheckBoxState("#hpid-encryption-checkbox", encrypt);

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        /// <summary>
        /// Sets the prompt for additional pages.
        /// </summary>
        /// <exception cref="DeviceWorkflowException">Unable to select scan mode option.</exception>
        public void SetPromptForAdditionalPages()
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-scan-mode", "#hpid-option-scan-mode-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select scan mode option.");
            }

            _controlPanel.ScrollPress("#hpid-prompt-add-page-checkbox");

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        /// <summary>
        /// Set the Scan mode
        /// </summary>
        /// <param name="scanModeSelected">Sets the selected scan mode specified in the ScanMode enum</param>
        public void SetScanMode(ScanMode scanModeSelected)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-scan-mode", "#hpid-option-scan-mode-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select scan mode option.");
            }

            _controlPanel.ScrollPress($"#hpid-scan-mode-selection-{scanModeSelected.ToString().ToLower()}");

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        /// <summary>
        /// Set the booklet format to On-Off
        /// </summary>
        /// <param name="bookletformat">If set to <c>True</c> booklet format is enabaled; otherwise it is disabled</param>
        /// <param name="borderOnPage">If set to <c>True</c> border on page is set to true; else it is false</param>        
        public void SetBooklet(bool bookletformat, bool borderOnPage)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-booklet", "#hpid-option-booklet-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select booklet mode option.");
            }

            SetCheckBoxState("#hpid-booklet-format-checkbox", bookletformat);
            if (bookletformat)
            {
                SetCheckBoxState("#hpid-booklet-borders-checkbox", borderOnPage);
            }

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        /// <summary>
        /// Sets value for Sides Option (Original and Output and Page flips up)
        /// </summary>       
        /// <param name="originalOnesided">if set to <c>true</c> Orignal side is set to 1-sided ; otherwise 2-sided</param>
        /// <param name="outputOnesided">if set to <c>true</c> Output side is set to 1-sided ; otherwise 2-sided</param>
        /// <param name="originalPageFlipUp">If set to <c>true</c> Page flip is enabled; otherwise it is disabled</param>
        /// <param name="outputPageFlipUp">If set to <c>true</c> Output page flip is enabled ; otherwise it is disabled</param>
        public void SetSides(bool originalOnesided, bool outputOnesided, bool originalPageFlipUp, bool outputPageFlipUp)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-sides", "#hpid-option-sides-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select sides option.");
            }

            if (originalOnesided)
            {
                _controlPanel.ScrollPress("#hpid-original-sides-selection-simplex");
            }
            else
            {
                _controlPanel.ScrollPress("#hpid-original-sides-selection-duplex");
                SetCheckBoxState("#hpid-original-sides-flippagesup-checkbox", originalPageFlipUp);
            }

            if (outputOnesided)
            {
                _controlPanel.ScrollPress("#hpid-output-sides-selection-simplex");
            }
            else
            {
                _controlPanel.ScrollPress("#hpid-output-sides-selection-duplex");
                SetCheckBoxState("#hpid-output-sides-flippagesup-checkbox", outputPageFlipUp);
            }

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        /// <summary>
        /// Performs Copy of Photo with Enlarge/reduction option
        /// </summary>       
        /// <param name="reduceEnlargeOption">if set to <c>true</c> Manual is set ; otherwise Automatic is set</param>
        /// <param name="includeMargin">It set to <c>true</c> enable Include margin; otherwise it is disabled</param>
        /// <param name="zoomSize">Value of the Zoom size, range between 25-100%</param>
        public void SetReduceEnlarge(bool reduceEnlargeOption, bool includeMargin, int zoomSize)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-reduce-enlarge", "#hpid-option-reduce-enlarge-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select reduce/enlarge option.");
            }

            if (reduceEnlargeOption)
            {
                _controlPanel.ScrollPress("#hpid-reduce-enlarge-selection-true .hp-radio-button");
                SetCheckBoxState("#hpid-include-margins-checkbox", includeMargin);
            }
            else
            {
                _controlPanel.ScrollPressWait("#hpid-reduce-enlarge-selection-false .hp-radio-button", "#hpid-scale-x-percent-textbox");
                SetNumericValue("#hpid-scale-x-percent-textbox", zoomSize.ToString());
            }

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        /// <summary>
        /// Sets the paper selection.
        /// </summary>
        /// <param name="paperSize">Size of the paper.</param>
        /// <param name="paperType">Type of the paper.</param>
        /// <param name="paperTray">The paper tray.</param>
        public void SetPaperSelection(PaperSelectionPaperSize paperSize, PaperSelectionPaperType paperType, PaperSelectionPaperTray paperTray)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-paper-select", "#hpid-option-paper-select-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select paper selection option.");
            }

            _controlPanel.ScrollPressWait("#hpid-media-size", "#hpid-media-size-details", TimeSpan.FromSeconds(5));
            _controlPanel.ScrollPressWait($"#hpid-media-size-selection-{paperSize.ToString().ToLower()}", ".hp-button-done");
            Pacekeeper.Sync();

            _controlPanel.ScrollPressWait("#hpid-media-type", "#hpid-media-type-details", TimeSpan.FromSeconds(5));
            _controlPanel.ScrollPressWait($"#hpid-media-type-selection-{paperType.ToString().ToLower()}", ".hp-button-done");
            Pacekeeper.Sync();

            _controlPanel.ScrollPressWait("#hpid-media-source", "#hpid-media-source-details", TimeSpan.FromSeconds(5));
            _controlPanel.ScrollPressWait($"#hpid-media-source-selection-{paperTray.ToString().ToLower()}", ".hp-button-done");
            Pacekeeper.Sync();

            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        /// <summary>
        /// Set the Pages per Sheet
        /// </summary>
        /// <param name="pages">Set the pages per sheet for the document using the PagesPerSheet enum </param>
        /// <param name="addPageBorders">If set to <c>True</c> Page borders are added ; otherwise page borders are not added </param>
        public void SetPagesPerSheet(PagesPerSheet pages, bool addPageBorders)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-pages-per-sheet", "#hpid-option-pages-per-sheet-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select pages per sheet option.");
            }

            _controlPanel.ScrollPress($"#hpid-option-pages-per-sheet-selection-{pages.GetDescription()}");

            if (pages != PagesPerSheet.OneUp)
            {
                _controlPanel.WaitForState("#hpid-option-pages-per-sheet-add-borders", OmniElementState.Constrained, false, TimeSpan.FromSeconds(2));
                SetCheckBoxState("#hpid-option-pages-per-sheet-add-borders", addPageBorders);
            }

            Pacekeeper.Sync();
            _controlPanel.PressWait(".hp-button-done", ".hp-option-list");
        }

        /// <summary>
        /// Sets the output to either Normal or EdgeToEdge
        /// </summary>
        /// <param name="edgetoedge">if set to <c>true</c> set Edge to edge , otherwise disable it</param>
        public void SetEdgeToEdge(bool edgetoedge)
        {
            OpenOptionsPanel();
            if (_controlPanel.CheckState("#hpid-option-edge-to-edge", OmniElementState.Exists))
            {
                if (!_controlPanel.ScrollPressWait("#hpid-option-edge-to-edge", "#hpid-option-edge-to-edge-screen", TimeSpan.FromSeconds(5)))
                {
                    throw new DeviceWorkflowException("Unable to select edge-to-edge option.");
                }

                Pacekeeper.Sync();
                _controlPanel.ScrollPressWait(edgetoedge ? "#hpid-edge-to-edge-selection-true" : "#hpid-edge-to-edge-selection-false", ".hp-option-list");
            }
        }

        /// <summary>
        /// Sets the value for Collate option.
        /// </summary>
        /// <param name="collate">Either set the job to collate or disbale it</param>
        public void SetCollate(bool collate)
        {
            OpenOptionsPanel();

            if (!_controlPanel.ScrollPressWait("#hpid-option-collate", "#hpid-option-collate-screen", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Unable to select collate option.");
            }

            Pacekeeper.Sync();
            _controlPanel.ScrollPressWait(collate ? "#hpid-collate-selection-collated" : "#hpid-collate-selection-uncollated", ".hp-option-list");
        }

        /// <summary>
        /// Opens the options panel for job
        /// </summary>
        public void OpenOptionsPanel()
        {
            if (_controlPanel.CheckState("#hpid-keyboard-key-done", OmniElementState.Useable))
            {
                _controlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-button-options-show-hide");
            }

            if (!_controlPanel.CheckState(".hp-option-list", OmniElementState.VisiblePartially))
            {
                _controlPanel.WaitForAvailable("#hpid-button-options-show-hide", TimeSpan.FromSeconds(5));
                _controlPanel.PressWait("#hpid-button-options-show-hide", ".hp-option-list");
                Pacekeeper.Sync();
            }
        }

        private void SetNumericValue(string selector, string value)
        {
            _controlPanel.ScrollPressWait(selector, "#hpid-keypad-key-close", TimeSpan.FromSeconds(5));
            _controlPanel.TypeOnNumericKeypad(value);
            _controlPanel.PressWait("#hpid-keypad-key-close", ".hp-button-done", TimeSpan.FromSeconds(5));
        }

        private void SetCheckBoxState(string checkBox, bool selected)
        {
            if (selected != _controlPanel.CheckState(checkBox, OmniElementState.Selected))
            {
                _controlPanel.ScrollPress(checkBox);
            }
        }
    }
}
