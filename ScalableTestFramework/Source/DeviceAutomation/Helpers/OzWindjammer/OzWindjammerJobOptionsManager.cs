using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Oz;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.Helpers.OzWindjammer
{
    /// <summary>
    /// Manages advanced job options on an <see cref="OzWindjammerDevice" />.
    /// </summary>
    public class OzWindjammerJobOptionsManager
    {
        private readonly OzWindjammerDevice _device;
        private readonly OzWindjammerControlPanel _controlPanel;

        /// <summary>
        /// Gets or sets the pacekeeper for this options manager.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; } = new Pacekeeper(TimeSpan.Zero);

        /// <summary>
        /// Initializes a new instance of the <see cref="OzWindjammerJobOptionsManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public OzWindjammerJobOptionsManager(OzWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
        }

        /// <summary>
        /// Selects the file type for the scanned file.
        /// </summary>
        /// <param name="fileType">The file type to select (case sensitive).</param>
        public void SelectFileType(string fileType)
        {
            Widget serviceButton = ScrollToOption("Document File Type");
            Pacekeeper.Sync();

            if (serviceButton.Properties["StringContent1"] != fileType)
            {
                _controlPanel.Press(serviceButton);
                Pacekeeper.Sync();
                Widget fileTypeWidget = _controlPanel.ScrollToItem("StringContent1", fileType);
                Pacekeeper.Sync();
                _controlPanel.Press(fileTypeWidget);
                Pacekeeper.Sync();
                _controlPanel.Press("OK");
                Pacekeeper.Sync();
            }
        }

        /// <summary>
        /// Sets the state of the Job Build option.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> enable Job Build; otherwise, disable it.</param>
        public void SetJobBuildState(bool enable)
        {
            Widget serviceButton = ScrollToOption("Job Build");
            Pacekeeper.Sync();

            if (serviceButton.Properties["StringContent1"] != (enable ? "On" : "Off"))
            {
                _controlPanel.Press(serviceButton);
                Pacekeeper.Sync();
                _controlPanel.Press(enable ? "Job Build On" : "Job Build Off");
                Pacekeeper.Sync();
                _controlPanel.Press("OK");
                Pacekeeper.Sync();
            }
        }

        /// <summary>
        /// Enables print notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>//We have added thumbnail parameter but not yet implemented it
        public void EnablePrintNotification(NotifyCondition condition, bool thumbNail)
        {
            Widget serviceButton = ScrollToOption("Notification");
            Pacekeeper.Sync();
            _controlPanel.Press(serviceButton);
            Pacekeeper.Sync();

            // Press the notify option, then wait for the other widgets to load
            string notifyOption = (condition == NotifyCondition.OnlyIfJobFails ? "On error" : "This job");
            _controlPanel.Press(notifyOption);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Pacekeeper.Sync();

            _controlPanel.Press("Print");
            Pacekeeper.Sync();
            _controlPanel.Press("OK");
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Enables email notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="address">The email address to receive the notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>//We have added thumbnail parameter but not yet implemented it
        public void EnableEmailNotification(NotifyCondition condition, string address, bool thumbNail)
        {
            Widget serviceButton = ScrollToOption("Notification");
            Pacekeeper.Sync();
            _controlPanel.Press(serviceButton);
            Pacekeeper.Sync();

            // Press the notify option, then wait for the other widgets to load
            string notifyOption = (condition == NotifyCondition.OnlyIfJobFails ? "On error" : "This job");
            _controlPanel.Press(notifyOption);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Pacekeeper.Sync();

            // On some devices (digital senders) this widget doesn't exist, since only email is available
            WidgetCollection widgets = _controlPanel.GetWidgets();
            Widget emailWidget = widgets.Find("E-mail");
            if (emailWidget != null)
            {
                _controlPanel.Press(emailWidget);
                Pacekeeper.Sync();
            }

            // Enter the email address
            Widget label = widgets.Find("E-mail Address", StringMatch.StartsWith);
            Widget emailStringBox = widgets.FindByLabel(label, WidgetType.StringBox);
            _controlPanel.Press(emailStringBox);
            Pacekeeper.Sync();
            _controlPanel.PressKey(OzHardKey.Clear);
            _controlPanel.TypeOnVirtualKeyboard(address);
            Pacekeeper.Sync();
            _controlPanel.Press("OK");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Pacekeeper.Sync();
            _controlPanel.Press("OK");
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Selects the resolution for the selected file
        /// </summary>
        /// <param name="resolution">The resolution to select(case sensitive).</param>
        public void SelectResolution(ResolutionType resolution)
        {
            throw new NotImplementedException($"Unable to set job resolution to resolution:{resolution}. Resolution Selection is not implemented on OzWindjammer devices.");
        }

        /// <summary>
        /// Selects the resolution for the selected file
        /// </summary>
        /// <param name="resolution">The resolution to select(case sensitive).</param>
        public void SelectFaxResolution(ResolutionType resolution)
        {
            throw new NotImplementedException($"Unable to set fax resolution to resolution:{resolution}.Fax Resolution Selection is not implemented on OzWindjammer devices.");
        }

        /// <summary>
        /// Selects the Original Sides for the scanned file
        /// </summary>
        /// <param name="originalSides">The original Sides to select (case sensitive).</param>
        /// <param name="pageFlipUp">if set to <c>true</c>enable pageFlipUp ;otherwise,disable it</param>
        public void SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            throw new NotImplementedException($"SelectOriginalSides  with setting {originalSides} and pageflipup has been {(pageFlipUp == true ? "checked" : "not checked")}  feature is not implemented on OzWindjammer devices");
        }

        /// <summary>
        /// Selects the Color/Black for the scanned file
        /// </summary>
        /// <param name="colorOrBlack">The Color/Black is selected.</param>
        public void SelectColorOrBlack(ColorType colorOrBlack)
        {
            throw new NotImplementedException($"SelectColor/Black  with setting {colorOrBlack} feature is not implemented on OzWindjammer devices");
        }

        /// <summary>
        /// Selects the Original Size for the scanned file
        /// </summary>
        /// <param name="originalSize">The original Size to select (case sensitive).</param>
        public void SelectOriginalSize(OriginalSize originalSize)
        {
            throw new NotImplementedException($"SelectOriginalSize with setting {originalSize} feature is not implemented on OzWindjammer devices");
        }

        /// <summary>
        /// Selects the Content Orientation  for the scanned file
        /// </summary>
        /// <param name="contentOrientation">content orientation.</param>
        public void SelectContentOrientation(ContentOrientation contentOrientation)
        {
            throw new NotImplementedException($"SelectContentOrientation with setting {contentOrientation} feature is not implemented on OzWindjammer devices");
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
            throw new NotImplementedException("SetImageadjustment feature is not implemented on OzWindjammer devices");
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
            throw new NotImplementedException("SetImageadjustment feature is not implemented on OzWindjammer devices");
        }

        /// <summary>
        /// Selects the Optimize text or pitcure  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">Selects the text or pitcure</param>
        public void SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
        {
            throw new NotImplementedException($"SelectOptimizeTextOrPitcure with setting {optimizeTextOrPicture} feature is not implemented on OzWindjammer devices");
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
            throw new NotImplementedException("SetErase edges feature is not implemented on OzWindjammer devices");
        }

        /// <summary>
        /// Selects the Crop Option for the scanned file
        /// </summary>
        /// <param name="cropOption">crop optoin type to select(case sensitive)</param>
        public void SelectCropOption(Cropping cropOption)
        {
            throw new NotImplementedException($"SelectCropOption with setting {cropOption} feature is not implemented on OzWindjammer devices");
        }

        /// <summary>
        /// Selects the Blank Page Supress for scanned file
        /// </summary>
        /// <param name="optionType"> blank page supress to select(case sensitive)</param>
        public void SelectBlankPageSupress(BlankPageSupress optionType)
        {
            throw new NotImplementedException($"SelectBlankPageSupress with setting {optionType} feature is not implemented on OzWindjammer devices");
        }

        /// <summary>
        /// Selects the multiple files setting for the scanned file
        /// </summary>
        /// <param name="isChecked">isCreatemultipleFiles selected<value><c>true</c>if it is checked;otherwise,<c>false</c></value></param>
        /// <param name="pagesPerFile">pages per file value.</param>
        public void CreateMultipleFiles(bool isChecked, string pagesPerFile)
        {
            throw new NotImplementedException("CreateMultipleFiles feature is not implemented on OzWindjammer devices");
        }

        /// <summary>
        /// Selects the signing or encrypt option for the scanned file
        /// </summary>
        /// <param name="sign">if set to <c>true</c> enable signing.</param>
        /// <param name="encrypt">if set to <c>true</c> enable encryption.</param>
        public void SelectSigningAndEncrypt(bool sign, bool encrypt)
        {
            throw new NotImplementedException("SelectSigningAndEncrypt feature is not implemented on OzWindjammer devices");
        }

        /// <summary>
        /// Scrolls to the specified option on the More Options menu.
        /// </summary>
        /// <param name="option">The title of the option button to scroll to.</param>
        /// <returns>The option button.</returns>
        public Widget ScrollToOption(string option)
        {
            // This function only scrolls down through the list - could be enhanced to scroll up too
            while (true)
            {
                Widget optionWidget = _controlPanel.GetWidgets().Find("Title", option);
                if (optionWidget != null)
                {
                    return optionWidget;
                }

                Widget moreOptionsButton = FindMoreOptionsButton();
                if (moreOptionsButton.State != WidgetState.Disabled)
                {
                    _controlPanel.Press(moreOptionsButton);
                }
                else
                {
                    throw new DeviceWorkflowException($"{option} was not found on the more options screen.");
                }
            }
        }

        private Widget FindMoreOptionsButton()
        {
            // This doesn't always work the first time.  Give it a couple of tries.
            for (int i = 0; i < 3; i++)
            {
                // The more options button has a different position and id for different
                // apps and different devices.  The most reliable way to find it without a
                // list of cases is to look for the bottom-most button widget.
                Widget moreOptions = _controlPanel.GetWidgets().OfType(WidgetType.Button).OrderByDescending(n => n.Location.Y).FirstOrDefault();

                // To confirm that we have the right one, make sure there is no text
                if (moreOptions != null && string.IsNullOrEmpty(moreOptions.Text))
                {
                    return moreOptions;
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }

            throw new DeviceWorkflowException("Could not find 'More Options' button.");
        }
    }
}
