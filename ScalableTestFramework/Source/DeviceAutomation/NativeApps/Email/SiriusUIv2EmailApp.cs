using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Authentication;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Email
{
    /// <summary>
    /// Implementation of <see cref="IEmailApp" /> for a <see cref="SiriusUIv2Device" />.
    /// </summary>
    public sealed class SiriusUIv2EmailApp : DeviceWorkflowLogSource, IEmailApp, IEmailJobOptions
    {
        private readonly SiriusUIv2ControlPanel _controlPanel;

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; } = new Pacekeeper(TimeSpan.Zero);

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv2EmailApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public SiriusUIv2EmailApp(SiriusUIv2Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Launches the Email application on the device.
        /// </summary> 
        public void Launch()
        {
            try
            {
                _controlPanel.Press("home_screen_table_menu.2");
                _controlPanel.WaitForActiveScreenLabel("view_scan_home", TimeSpan.FromSeconds(10)); //Scan To Page
                Pacekeeper.Sync();

                RecordEvent(DeviceWorkflowMarker.AppButtonPress);
                _controlPanel.Press("scan_home_screen_table_menu.3");
                _controlPanel.WaitForActiveScreenLabel("view_scan_email_job_menu_home", TimeSpan.FromSeconds(10)); //Email Page
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Sync();
            }
            catch (ElementNotFoundException ex)
            {
                string currentForm = _controlPanel.ActiveScreenLabel();
                if (currentForm == "view_scan_home")
                {
                    throw new DeviceWorkflowException("Email application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Email application from {currentForm}.", ex);
                }
            }
            catch (SiriusInvalidOperationException ex)
            {
                if (_controlPanel.ActiveScreenLabel() == "view_scan_to_email_no_sign_in_select_profile")
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the Email application.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Could not launch Email application: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Launches the Email application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                try
                {
                    _controlPanel.Press("home_screen_table_menu.2");
                    _controlPanel.WaitForActiveScreenLabel("view_scan_home", TimeSpan.FromSeconds(10)); //Scan To Page
                    Pacekeeper.Sync();

                    RecordEvent(DeviceWorkflowMarker.AppButtonPress);
                    _controlPanel.Press("scan_home_screen_table_menu.3");
                    Pacekeeper.Sync();

                    if (_controlPanel.ActiveScreenLabel() != "view_scan_email_job_menu_home")
                    {
                        authenticator.Authenticate();
                    }
                    _controlPanel.WaitForActiveScreenLabel("view_scan_email_job_menu_home", TimeSpan.FromSeconds(30)); //Email Form
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                    Pacekeeper.Sync();
                }
                catch (ElementNotFoundException ex)
                {
                    string currentForm = _controlPanel.ActiveScreenLabel();
                    if (currentForm == "view_scan_home")
                    {
                        throw new DeviceWorkflowException("Email application button was not found on device home screen.", ex);
                    }
                    else
                    {
                        throw new DeviceWorkflowException($"Cannot launch the Email application from {currentForm}.", ex);
                    }
                }
                catch (SiriusInvalidOperationException ex)
                {
                    throw new DeviceWorkflowException($"Could not launch Email application: {ex.Message}", ex);
                }
            }
            else // AuthenticationMode.Eager
            {
                throw new NotImplementedException("Eager Authentication has not been implemented in SiriusUIv2EmailApp.");
            }
        }

        /// <summary>
        /// Launches the Email application using the specified authenticator, authentication mode, and quickset.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quick set.</param>
        /// <exception cref="System.NotImplementedException">LaunchFromQuickset has not been implemented in SiriusUIv2EmailApp.</exception>
        public void LaunchFromQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName)
        {
            throw new NotImplementedException("LaunchFromQuickset has not been implemented in SiriusUIv2EmailApp.");
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="quickSetName">Name of the Quickset if exists or else empty/null</param>
        /// </summary>  
        public void SelectQuickset(string quickSetName)
        {
            throw new NotImplementedException("Qucickset selection has not been implemented in Sirius Email App.");
        }

        /// <summary>
        /// Enters the "To" email address.
        /// </summary>
        /// <param name="emailAddresses">The "To" email addresses.</param>
        public void EnterToAddress(IEnumerable<string> emailAddresses)
        {
            foreach (string address in emailAddresses)
            {
                string displayName = (address ?? string.Empty).Split('@')[0];
                string emailControlName = $"{displayName} {address}".ToLowerInvariant();

                try
                {
                    if (_controlPanel.ActiveScreenLabel() == "view_scan_email_job_menu_home")
                    {
                        _controlPanel.Press("email_menu_value_to");//To Address
                        _controlPanel.WaitForActiveScreenLabel("view_email_Addressbook_Flow_In_STE_list", TimeSpan.FromSeconds(10));

                        _controlPanel.PressByValue(emailControlName); //Check box

                        _controlPanel.Press("Addressbook_Flow_In_STE_footer.0"); //Done
                    }
                }
                catch (ElementNotFoundException ex)
                {
                    throw new DeviceWorkflowException($"Display name is different from To Address {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Enters the email subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        public void EnterSubject(string subject)
        {
            if (_controlPanel.ActiveScreenLabel() == "view_scan_email_job_menu_home")
            {
                _controlPanel.Press("email_menu_value_subject");

                _controlPanel.WaitForActiveScreenLabel("view_scan_email_menu_subject_keyboard", TimeSpan.FromSeconds(10));
                _controlPanel.SetValue("email_menu_subject_keyboard.4", subject);//Keyboard
            }
        }

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void EnterFileName(string fileName)
        {
            try
            {
                if (_controlPanel.ActiveScreenLabel() == "view_scan_email_job_menu_home")
                {
                    _controlPanel.Press("email_job_menu_footer.2");

                    _controlPanel.WaitForActiveScreenLabel("view_scan_email_settings", TimeSpan.FromSeconds(10));
                    _controlPanel.Press("email_settings_menu_list.2");
                    Pacekeeper.Sync();

                    _controlPanel.WaitForActiveScreenLabel("view_scan_email_filename_virtual_keyboard", TimeSpan.FromSeconds(10));
                    _controlPanel.SetValue("email_filename_virtual_keyboard.4", fileName);
                    Pacekeeper.Sync();

                    _controlPanel.Press("email_settings_menu_footer.0");
                }
            }
            catch (ElementNotFoundException ex)
            {
                throw new DeviceWorkflowException($"Display name is different from To Address {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            if (executionOptions == null)
            {
                throw new ArgumentNullException(nameof(executionOptions));
            }

            try
            {
                if (_controlPanel.ActiveScreenLabel() == "view_scan_email_job_menu_home")
                {
                    string documentType = string.Empty;

                    _controlPanel.Press("email_job_menu_footer.2");
                    if (_controlPanel.ActiveScreenLabel() == "view_scan_email_settings")
                    {
                        ScreenInfo info = _controlPanel.GetScreenInfo();
                        Widget documentWidget = info.Widgets.First(n => n.Id == "email_settings_menu_list.0");
                        if (documentWidget.HasValue("Document Type|JPEG Color") || documentWidget.HasValue("Document Type|JPEG Grayscale"))
                        {
                            documentType = "JPEG";
                        }
                        _controlPanel.Press("email_settings_menu_footer.0");
                    }

                    RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                    _controlPanel.Press("email_job_menu_footer.0"); //Start Scan

                    for (int scanCount = 1; scanCount < executionOptions.JobBuildSegments; scanCount++)
                    {
                        if (documentType == "JPEG")
                        {
                            if (!_controlPanel.WaitForActiveScreenLabel("view_scan_email_job_menu_home", TimeSpan.FromSeconds(50)))
                            {
                                throw new DeviceWorkflowException("Start Scan feature was not available");
                            }
                        }
                        else
                        {
                            if (!_controlPanel.WaitForActiveScreenLabel("view_email_scan_another_page", TimeSpan.FromSeconds(50)))
                            {
                                throw new DeviceWorkflowException("Scan repeat function was not found");
                            }
                        }
                    }

                    if (documentType != "JPEG")
                    {
                        if (_controlPanel.WaitForActiveScreenLabel("view_email_scan_another_page", TimeSpan.FromSeconds(50)))
                        {
                            _controlPanel.Press("gr_footer_yes_no.0");
                        }
                        else
                        {
                            throw new DeviceWorkflowException("Scan repeat function was not found");
                        }
                    }
                }
                else
                {
                    throw new DeviceWorkflowException("Scan page is not available");
                }

                Thread.Sleep(TimeSpan.FromSeconds(10));
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            }
            catch (SiriusInvalidOperationException)
            {
                if (_controlPanel.ActiveScreenLabel() == "view_scan_email_error_msg")
                {
                    Widget widget = _controlPanel.GetScreenInfo().Widgets.Find("email_error_msg");
                    const string message = "Cannot connect to server. Check server name and address.";
                    if (widget.HasValue(message))
                    {
                        throw new DeviceWorkflowException($"Could not start job: {message}");
                    }
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the <see cref="IEmailJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IEmailJobOptions Options => this;

        #region IEmailJobOptions Members

        void IEmailJobOptions.SelectFileType(string fileType)
        {
            //File type is set from EWS page
        }

        void IEmailJobOptions.SetJobBuildState(bool enable)
        {
        }

        /// <summary>
        /// Enables print notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        void IEmailJobOptions.EnablePrintNotification(NotifyCondition condition, bool thumbNail)
        {
        }

        /// <summary>
        /// Enables email notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="address">The email address to receive the notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        void IEmailJobOptions.EnableEmailNotification(NotifyCondition condition, string address, bool thumbNail)
        {
        }

        /// <summary>
        /// Selects the resolution for the selected file
        /// </summary>
        /// <param name="resolution">The resolution to select(case sensitive).</param>
        void IEmailJobOptions.SelectResolution(ResolutionType resolution)
        {
            throw new NotImplementedException($"Unable to set job resolution to resolution:{resolution}. Resolution Selection is not implemented on SiriusUIv2 devices.");
        }

        /// <summary>
        /// Selects the Color/Black for the scanned file
        /// </summary>
        /// <param name="colorOrBlack">The Color/Black is selected.</param>
        void IEmailJobOptions.SelectColorOrBlack(ColorType colorOrBlack)
        {
            throw new NotImplementedException($"SelectColor or Black  with setting {colorOrBlack} feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Original Size for the scanned file
        /// </summary>
        /// <param name="originalSize">The original Size to select (case sensitive).</param>
        void IEmailJobOptions.SelectOriginalSize(OriginalSize originalSize)
        {
            throw new NotImplementedException($"SelectOriginalSize with setting {originalSize} feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Content Orientation  for the scanned file
        /// </summary>
        /// <param name="contentOrientation">content orientation.</param>
        void IEmailJobOptions.SelectContentOrientation(ContentOrientation contentOrientation)
        {
            throw new NotImplementedException($"SelectContentOrientation with setting {contentOrientation} feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Set image adjustment for sharpness, darkness, contrast and Background cleanup
        /// </summary>
        /// <param name="imageAdjustSharpness">Sets the value for Image adjustment sharpness </param>
        /// <param name="imageAdjustDarkness">Sets the value for Image adjustment darkness </param>
        /// <param name="imageAdjustContrast">Sets the value for Image adjustment contrast </param>
        /// <param name="imageAdjustBackgroundCleanup">Sets the value for Image adjustment background cleanup </param>
        /// <param name="automaticTone">if set to <c>true</c>enable sharpness only and disable all ;otherwise,enable all</param>
        void IEmailJobOptions.SetImageAdjustments(int imageAdjustSharpness, int imageAdjustDarkness, int imageAdjustContrast, int imageAdjustBackgroundCleanup, bool automaticTone)
        {
            throw new NotImplementedException($"SetImageadjustment feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Optimize text or picture  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">Selects the text or pitcure</param>
        void IEmailJobOptions.SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
        {
            throw new NotImplementedException($"SelectOptimizeTextOrPitcure with setting {optimizeTextOrPicture} feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Sets the Front and back erase edges for Top, Bottom, Left and Right
        /// </summary>
        /// <param name="eraseEdgeList">List cotaining the Key-value pair for the edge. Key, is the erase edge type enum and value is the string to be set for the element. Edges value must be a non empty, mumeric and a non zero string. Zeros will be ignored</param>
        /// <param name="applySameWidth">When set to <c>True</c> it applies same width for all edges for front; otherwise individual widths are applied</param>
        /// <param name="mirrorFrontSide">When set to <c>true</c>, back side will mirror the front side </param>
        /// <param name="useInches">If set to <c>true</c> inches will be used otherwise edges are set in milimeter </param>
        void IEmailJobOptions.SetEraseEdges(Dictionary<EraseEdgesType, decimal> eraseEdgeList, bool applySameWidth, bool mirrorFrontSide, bool useInches)
        {
            throw new NotImplementedException($"SetErase edges feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Crop Option for the scanned file
        /// </summary>
        /// <param name="selectedCropOption">crop optoin type to select(case sensitive)</param>
        void IEmailJobOptions.SelectCropOption(Cropping selectedCropOption)
        {
            throw new NotImplementedException($"SelectCropOption with setting {selectedCropOption} feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Blank Page Supress for scanned file
        /// </summary>
        /// <param name="optionType"> blank page supress to select(case sensitive)</param>
        void IEmailJobOptions.SelectBlankPageSupress(BlankPageSupress optionType)
        {
            throw new NotImplementedException($"SelectBlankPageSupress with setting {optionType} feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Original Sides for the scanned file
        /// </summary>
        /// <param name="originalSides">The original Sides to select (case sensitive).</param>
        /// <param name="pageFlipUp">if set to <c>true</c>enable pageFlipUp ;otherwise,disable it</param>
        void IEmailJobOptions.SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            throw new NotImplementedException($"SelectOriginalSides with setting  feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the multiple files setting for the scanned file
        /// </summary>
        /// <param name="isChecked">isCreatemultipleFiles selected<value><c>true</c>if it is checked;otherwise,<c>false</c></value></param>
        /// <param name="pagesPerFile">pages per file value.</param>
        void IEmailJobOptions.CreateMultipleFiles(bool isChecked, string pagesPerFile)
        {
            throw new NotImplementedException($"CreateMultipleFiles feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the signing or encrypt option for the scanned file
        /// </summary>
        /// <param name="sign">if set to <c>true</c> enable signing.</param>
        /// <param name="encrypt">if set to <c>true</c> enable encryption.</param>
        void IEmailJobOptions.SelectSigningAndEncrypt(bool sign, bool encrypt)
        {
            throw new NotImplementedException($"SelectSigningAndEncrypt feature is not implemented on SiriusUIv2 devices");
        }
        #endregion
    }
}
