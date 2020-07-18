using System;
using System.IO;
using System.Net;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Authentication;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder
{
    /// <summary>
    /// Implementation of <see cref="INetworkFolderApp" /> for a <see cref="SiriusUIv2Device" />.
    /// </summary>
    public sealed class SiriusUIv2NetworkFolderApp : DeviceWorkflowLogSource, INetworkFolderApp, INetworkFolderJobOptions
    {
        private readonly SiriusUIv2ControlPanel _controlPanel;

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; } = new Pacekeeper(TimeSpan.Zero);

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv2NetworkFolderApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public SiriusUIv2NetworkFolderApp(SiriusUIv2Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Launches the Network Folder application on the device.
        /// </summary>
        public void Launch()
        {
            try
            {
                _controlPanel.Press("home_screen_table_menu.2");
                _controlPanel.WaitForActiveScreenLabel("view_scan_home", TimeSpan.FromSeconds(10)); //Scan To Page
                Pacekeeper.Sync();

                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "NetworkFolder");
                _controlPanel.Press("scan_home_screen_table_menu.2");
                _controlPanel.WaitForActiveScreenLabel("view_scan_folder_presets", TimeSpan.FromSeconds(10)); //Network folder page
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Sync();
            }
            catch (ElementNotFoundException ex)
            {
                string currentScreen = _controlPanel.ActiveScreenLabel();
                if (currentScreen == "view_scan_home")
                {
                    throw new DeviceWorkflowException("Network Folder application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Network Folder application from {currentScreen}.", ex);
                }
            }
            catch (SiriusInvalidOperationException ex)
            {
                if (_controlPanel.ActiveScreenLabel() == "view_scan_to_email_no_sign_in_select_profile")
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the Network Folder application.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Could not launch Network Folder application: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Launches Scan to Network Folder using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="DeviceWorkflowException">
        /// Network Folder button was not found on device home screen.
        /// or
        /// or
        /// </exception>
        /// <exception cref="System.NotImplementedException">Eager authentication has not been implemented for this solution.</exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                try
                {
                    _controlPanel.Press("home_screen_table_menu.2");
                    _controlPanel.WaitForActiveScreenLabel("view_scan_home", TimeSpan.FromSeconds(10)); //Scan To Page
                    Pacekeeper.Sync();

                    RecordEvent(DeviceWorkflowMarker.AppButtonPress, "NetworkFolder");
                    _controlPanel.Press("scan_home_screen_table_menu.2");
                    Pacekeeper.Sync();

                    if (_controlPanel.ActiveScreenLabel() != "view_scan_folder_presets")
                    {
                        authenticator.Authenticate();
                    }

                    _controlPanel.WaitForActiveScreenLabel("view_scan_folder_presets", TimeSpan.FromSeconds(30)); //Network folder page
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                    Pacekeeper.Sync();
                }
                catch (ElementNotFoundException ex)
                {
                    string currentScreen = _controlPanel.ActiveScreenLabel();
                    if (currentScreen == "view_scan_home")
                    {
                        throw new DeviceWorkflowException("Network Folder button was not found on device home screen.", ex);
                    }
                    else
                    {
                        throw new DeviceWorkflowException($"Cannot launch the Network Folder application from {currentScreen}.", ex);
                    }
                }
                catch (SiriusInvalidOperationException ex)
                {
                    throw new DeviceWorkflowException($"Could not launch Network Folder application: {ex.Message}", ex);
                }
            }
            else
            {
                throw new NotImplementedException("Eager authentication has not been implemented for this solution.");
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
                _controlPanel.Press("scanfolder_home_value3");
                Pacekeeper.Sync();
                if (_controlPanel.WaitForActiveScreenLabel("view_scan_folder_filename_vkbd", TimeSpan.FromSeconds(1)))
                {
                    _controlPanel.SetValue("folder_filename_vkbd.4", fileName);
                }
                Pacekeeper.Sync();
            }
            catch (SiriusInvalidOperationException ex)
            {
                if (_controlPanel.ActiveScreenLabel() == "view_scan_folder_filename_vkbd")
                {
                    throw new DeviceWorkflowException("Error entering file name.", ex);
                }
            }
        }

        /// <summary>
        /// Adds the specified network folder path as a destination for the scanned file.
        /// </summary>
        /// <param name="path">The network folder path.</param>
        /// <param name="credential">The network credential parameter is being added to provide the network credentials to access folder path </param>
        /// /// <param name="applyCredentials"><value><c>true</c> if apply credentials on verification is checked;otherwise, <c>false</c>.</value></param>
        public void AddFolderPath(string path, NetworkCredential credential, bool applyCredentials)////network credential for folder access is supported for Omni device. SiriusUiv2 yet to be supported
        {
            string folder = Path.GetFileName(path);

            try
            {
                if (_controlPanel.WaitForActiveScreenLabel("view_scan_folder_presets", TimeSpan.FromSeconds(1)))
                {
                    _controlPanel.PressByValue(folder);
                    Pacekeeper.Sync();
                }
            }
            catch (ElementNotFoundException ex)
            {
                if (_controlPanel.ActiveScreenLabel() == "view_scan_folder_presets")
                {
                    throw new DeviceWorkflowException("Error entering folder path", ex);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// </summary>
        /// <param name="quicksetName">The quickset name.</param>
        public void SelectQuickset(string quicksetName)
        {
        }

        /// <summary>
        /// Gets the number of folder destinations currently selected.
        /// </summary>
        /// <returns>The number of folder destinations.</returns>
        public int GetDestinationCount()
        {
            return 0;
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool jobCompleted = false;
            if (executionOptions == null)
            {
                throw new ArgumentNullException(nameof(executionOptions));
            }

            try
            {
                if (_controlPanel.ActiveScreenLabel() == "view_scan_to_folder_home")
                {
                    RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                    _controlPanel.Press("scanfolder_home_group_footer.0"); //Start Scan

                    for (int scanCount = 1; scanCount < executionOptions.JobBuildSegments; scanCount++)
                    {
                        if (_controlPanel.WaitForActiveScreenLabel("view_scan_another_page_folder_message", TimeSpan.FromSeconds(50)))
                        {
                            _controlPanel.Press("gr_footer_yes_no.1");
                            if (_controlPanel.WaitForActiveScreenLabel("view_scan_folder_next_page_reply", TimeSpan.FromSeconds(50)))
                            {
                                _controlPanel.Press("scan_to_folder_scan_next_page_footer.0");
                            }
                        }
                        else
                        {
                            throw new DeviceWorkflowException("Waiting to click on Yes button");
                        }
                    }

                    if (_controlPanel.WaitForActiveScreenLabel("view_scan_another_page_folder_message", TimeSpan.FromSeconds(50)))
                    {
                        _controlPanel.Press("gr_footer_yes_no.0");
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Waiting to click on No button");
                    }
                }
                else
                {
                    throw new DeviceWorkflowException("Scan page is not available");
                }

                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                jobCompleted = true;
            }
            catch (SiriusInvalidOperationException)
            {
                if (_controlPanel.ActiveScreenLabel() == "view_scan_to_folder_error_msg")
                {
                    Widget widget = _controlPanel.GetScreenInfo().Widgets.Find("email_error_msg");
                    const string message = "Cannot connect to server. Make sure the shared folder name is correct.";
                    if (widget.HasValue(message))
                    {
                        throw new DeviceWorkflowException($"Could not start job: {message}");
                    }
                }
            }

            return jobCompleted;
        }

        /// <summary>
        /// Gets the <see cref="INetworkFolderJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public INetworkFolderJobOptions Options => this;

        #region INetworkFolderJobOptions Members

        void INetworkFolderJobOptions.SelectFileType(string fileType)
        {
        }

        void INetworkFolderJobOptions.SetJobBuildState(bool enable)
        {
        }

        /// <summary>
        /// Enables print notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        void INetworkFolderJobOptions.EnablePrintNotification(NotifyCondition condition, bool thumbNail)
        {
        }

        /// <summary>
        /// Enables email notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="address">The email address to receive the notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        void INetworkFolderJobOptions.EnableEmailNotification(NotifyCondition condition, string address, bool thumbNail)
        {
        }

        /// <summary>
        /// Selects the resolution for the selected file
        /// </summary>
        /// <param name="resolution">The resolution to select(case sensitive).</param>
        void INetworkFolderJobOptions.SelectResolution(ResolutionType resolution)
        {
            throw new NotImplementedException($"Unable to set job resolution to resolution:{resolution}. Resolution Selection is not implemented on SiriusUIv2 devices.");
        }

        /// <summary>
        /// Selects the Color/Black for the scanned file
        /// </summary>
        /// <param name="colorOrBlack">The Color/Black is selected.</param>
        void INetworkFolderJobOptions.SelectColorOrBlack(ColorType colorOrBlack)
        {
            throw new NotImplementedException($"SelectColor or Black  with setting {colorOrBlack} feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Original Size for the scanned file
        /// </summary>
        /// <param name="originalSize">The original Size to select (case sensitive).</param>
        void INetworkFolderJobOptions.SelectOriginalSize(OriginalSize originalSize)
        {
            throw new NotImplementedException($"SelectOriginalSize with setting {originalSize} feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Content Orientation  for the scanned file
        /// </summary>
        /// <param name="contentOrientation">content orientation.</param>
        void INetworkFolderJobOptions.SelectContentOrientation(ContentOrientation contentOrientation)
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
        void INetworkFolderJobOptions.SetImageAdjustments(int imageAdjustSharpness, int imageAdjustDarkness, int imageAdjustContrast, int imageAdjustBackgroundCleanup)
        {
            throw new NotImplementedException("SetImageadjustment feature is not implemented on SiriusUIv2 devices");
        }


        /// <summary>
        /// Set image adjustment for sharpness, darkness, contrast and Background cleanup
        /// </summary>
        /// <param name="imageAdjustSharpness">Sets the value for Image adjustment sharpness </param>
        /// <param name="imageAdjustDarkness">Sets the value for Image adjustment darkness </param>
        /// <param name="imageAdjustContrast">Sets the value for Image adjustment contrast </param>
        /// <param name="imageAdjustBackgroundCleanup">Sets the value for Image adjustment background cleanup </param>
        /// <param name="automaticTone">if set to <c>true</c>enable sharpness only and disable all ;otherwise,enable all</param>
        void INetworkFolderJobOptions.SetImageAdjustments(int imageAdjustSharpness, int imageAdjustDarkness, int imageAdjustContrast, int imageAdjustBackgroundCleanup, bool automaticTone)
        {
            throw new NotImplementedException("SetImageadjustment feature is not implemented on SiriusUIv2 devices");
        }


        /// <summary>
        /// Selects the Optimize text or pitcure  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">Selects the text or pitcure</param>
        void INetworkFolderJobOptions.SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
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
        void INetworkFolderJobOptions.SetEraseEdges(Dictionary<EraseEdgesType, decimal> eraseEdgeList, bool applySameWidth, bool mirrorFrontSide, bool useInches)
        {
            throw new NotImplementedException("SetErase edges feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Crop Option for the scanned file
        /// </summary>
        /// <param name="selectedCropOption">crop optoin type to select(case sensitive)</param>
        void INetworkFolderJobOptions.SelectCropOption(Cropping selectedCropOption)
        {
            throw new NotImplementedException($"SelectCropOption with setting {selectedCropOption} feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Blank Page Supress for scanned file
        /// </summary>
        /// <param name="optionType"> blank page supress to select(case sensitive)</param>
        void INetworkFolderJobOptions.SelectBlankPageSupress(BlankPageSupress optionType)
        {
            throw new NotImplementedException($"SelectBlankPageSupress with setting {optionType} feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the Original Sides for the scanned file
        /// </summary>
        /// <param name="originalSides">The original Sides to select (case sensitive).</param>
        /// <param name="pageFlipUp">if set to <c>true</c>enable pageFlipUp ;otherwise,disable it</param>
        void INetworkFolderJobOptions.SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            throw new NotImplementedException("SelectOriginalSides with setting  feature is not implemented on SiriusUIv2 devices");
        }

        /// <summary>
        /// Selects the multiple files setting for the scanned file
        /// </summary>
        /// <param name="isChecked">isCreatemultipleFiles selected<value><c>true</c>if it is checked;otherwise,<c>false</c></value></param>
        /// <param name="pagesPerFile">pages per file value.</param>
        void INetworkFolderJobOptions.CreateMultipleFiles(bool isChecked, string pagesPerFile)
        {
            throw new NotImplementedException("CreateMultipleFiles feature is not implemented on SiriusUIv2 devices");
        }


        #endregion
    }
}
