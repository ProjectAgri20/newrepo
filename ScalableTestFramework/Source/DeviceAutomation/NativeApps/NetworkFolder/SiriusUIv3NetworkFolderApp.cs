using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.SiriusUIv3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder
{
    /// <summary>
    /// Implementation of <see cref="INetworkFolderApp" /> for a <see cref="SiriusUIv3Device" />.
    /// </summary>
    public sealed class SiriusUIv3NetworkFolderApp : DeviceWorkflowLogSource, INetworkFolderApp, INetworkFolderJobOptions
    {
        private readonly SiriusUIv3ControlPanel _controlPanel;
        private readonly SiriusUIv3JobExecutionManager _executionManager;
       

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3NetworkFolderApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public SiriusUIv3NetworkFolderApp(SiriusUIv3Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
            _executionManager = new SiriusUIv3JobExecutionManager(device);
        }

        /// <summary>
        /// Launches the Network Folder application on the device.
        /// </summary>
        public void Launch()
        {
            try
            {
                _controlPanel.ScrollPress("sfolderview_p","group.group.scan");
                Pacekeeper.Pause();
                _controlPanel.WaitForScreenLabel("Home", TimeSpan.FromSeconds(10)); //Scan To Page
                Pacekeeper.Pause();
                _controlPanel.ScrollPress("sfolderview_p", "command.scan_folder");

                Pacekeeper.Pause();
                if (_controlPanel.GetScreenInfo().ScreenLabels.Contains("AnA_Login_With_Windows_Authentication") || _controlPanel.GetScreenInfo().ScreenLabels.Contains("AnA_Login_With_LDAP_Authentication"))
                {
                    throw new DeviceInvalidOperationException("Network folder is Protected, launch it with authenticator");
                }
                //_controlPanel.Press("model.0");
                Pacekeeper.Pause();
            }
            catch (ElementNotFoundException ex)
            {
                string currentScreen = _controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault();
                if (currentScreen == "Scan_Status_Error")
                {
                    throw new DeviceWorkflowException("Email application button was not found on device home screen.", ex);
                }
                if (currentScreen == "Scan_NetworkFolder_HomeGlass")
                {
                    // The application launched successfully. This happens sometimes.
                }
                else
                {
                    throw new DeviceInvalidOperationException($"Cannot launch the Email application from {currentScreen}.", ex);
                }
            }
            catch (SiriusInvalidOperationException ex)
            {
                switch (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault())
                {
                    case "Scan_NetworkFolder_HomeGlass":
                    case "Scan_NetworkFolder_HomeADF":
                        // The application launched successfully. This happens sometimes.
                        break;

                    case "AnA_Login_With_Windows_Authentication":
                    case "AnA_Login_With_LDAP_Authentication":
                        {
                            throw new DeviceWorkflowException("Sign-in required to launch the Email application.", ex);
                        }

                    default:
                        {
                            throw new DeviceInvalidOperationException($"Could not launch Email application: {ex.Message}", ex);
                        }
                }
            }
        }

        /// <summary>
        /// Launches Scan to Network Folder using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="DeviceInvalidOperationException">
        /// </exception>
        /// <exception cref="DeviceWorkflowException">
        /// Network Folder button was not found on device home screen.
        /// or
        /// Sign-in required to launch the Email application.
        /// </exception>
        /// <exception cref="System.NotImplementedException">Eager authentication has not been implemented for this solution.</exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                try
                {
                    _controlPanel.ScrollPress("sfolderview_p", "group.group.scan");
                    _controlPanel.WaitForScreenLabel("Home", TimeSpan.FromSeconds(1)); //Scan To Page
                    Pacekeeper.Pause();
                    _controlPanel.ScrollPress("sfolderview_p", "command.scan_folder");
                    Pacekeeper.Pause();

                    if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault() != "Scan_NetworkFolder_FolderList") //Scan_NetworkFolder_HomeGlass;Scan_NetworkFolder_HomeADF
                    {
                        authenticator.Authenticate();
                    }
                    //Veda: This is not needed
                    //_controlPanel.WaitForScreenLabel("view_scan_folder_presets", TimeSpan.FromSeconds(30)); //Network folder page
                    Pacekeeper.Pause();
                }
                catch (ElementNotFoundException ex)
                {
                    string currentScreen = _controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault();
                    if (currentScreen == "Home")
                    {
                        throw new DeviceInvalidOperationException($"Cannot launch the Network application from {currentScreen}.", ex);
                    }
                    if (currentScreen == "Scan_NetworkFolder_HomeGlass")
                    {
                        //Some times this happens
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Network Folder button was not found on device home screen.", ex);
                    }
                }
                catch (SiriusInvalidOperationException ex)
                {
                    switch (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault())
                    {
                        case "Scan_NetworkFolder_HomeGlass":
                        case "Scan_NetworkFolder_HomeADF":
                            // The application launched successfully. This happens sometimes.
                            break;

                        case "AnA_Login_With_Windows_Authentication":
                        case "AnA_Login_With_LDAP_Authentication":
                            {
                                throw new DeviceWorkflowException("Sign-in required to launch the Email application.", ex);
                            }

                        default:
                            {
                                throw new DeviceInvalidOperationException($"Could not launch Email application: {ex.Message}", ex);
                            }
                    }
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
                if (_controlPanel.WaitForScreenLabel("Scan_NetworkFolder_HomeGlass"))
                {
                    _controlPanel.Press("item2");
                    Pacekeeper.Pause();
                    _controlPanel.SetValue("slineedit", fileName);
                    Pacekeeper.Pause();
                    _controlPanel.Press("_done");
                    Pacekeeper.Pause();
                }

                Pacekeeper.Sync();
            }
            catch (SiriusInvalidOperationException ex)
            {
                if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault() == "Scan_Status_Error")
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
        public void AddFolderPath(string path, NetworkCredential credential, bool applyCredentials)////network credential for folder access is supported for Omni device. SiriusUIv3 yet to be supported
        {
            try
            {
                if (_controlPanel.WaitForScreenLabel("Scan_NetworkFolder_FolderList", TimeSpan.FromSeconds(1)))
                {
                    _controlPanel.PressByValue(path);
                    Pacekeeper.Pause();
                    Pacekeeper.Sync();
                }
            }
            catch (ElementNotFoundException ex)
            {
                if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault() == "Scan_Status_Error")
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
            if (_controlPanel.WaitForScreenLabel("Scan_NetworkFolder_FolderList", TimeSpan.FromSeconds(1)))
            {
                try
                {
                    _controlPanel.PressByValue(quicksetName);
                    Pacekeeper.Pause();
                }
                catch (ElementNotFoundException e)
                {
                    throw new DeviceWorkflowException("Quickset not found", e);
                }
            }
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
            _executionManager.WorkflowLogger = WorkflowLogger;
            var jobCompleted = _executionManager.ExecuteScanJob(executionOptions, "Scan_NetworkFolder_HomeGlass");
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
        /// <param name="includeThumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        void INetworkFolderJobOptions.EnablePrintNotification(NotifyCondition condition, bool includeThumbNail)
        {
        }

        /// <summary>
        /// Enables email notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="address">The email address to receive the notification.</param>
        /// <param name="includeThumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        void INetworkFolderJobOptions.EnableEmailNotification(NotifyCondition condition, string address, bool includeThumbNail)
        {
        }

        /// <summary>
        /// Selects the resolution for the selected file
        /// </summary>
        /// <param name="resolution">The resolution to select(case sensitive).</param>
        void INetworkFolderJobOptions.SelectResolution(ResolutionType resolution)
        {
            throw new NotImplementedException($"Unable to set job resolution to resolution:{resolution}. Resolution Selection is not implemented on SiriusUIv3 devices.");
        }

        /// <summary>
        /// Selects the Color/Black for the scanned file
        /// </summary>
        /// <param name="colorOrBlack">The Color/Black is selected.</param>
        void INetworkFolderJobOptions.SelectColorOrBlack(ColorType colorOrBlack)
        {
            throw new NotImplementedException($"SelectColor or Black  with setting {colorOrBlack} feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Original Size for the scanned file
        /// </summary>
        /// <param name="originalSize">The original Size to select (case sensitive).</param>
        void INetworkFolderJobOptions.SelectOriginalSize(OriginalSize originalSize)
        {
            throw new NotImplementedException($"SelectOriginalSize with setting {originalSize} feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Content Orientation  for the scanned file
        /// </summary>
        /// <param name="contentOrientation">content orientation.</param>
        void INetworkFolderJobOptions.SelectContentOrientation(ContentOrientation contentOrientation)
        {
            throw new NotImplementedException($"SelectContentOrientation with setting {contentOrientation} feature is not implemented on SiriusUIv3 devices");
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
            throw new NotImplementedException($"SetImageadjustment feature is not implemented on SiriusUIv3 devices");
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
            throw new NotImplementedException("SetImageadjustment feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Optimize text or pitcure  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">Selects the text or pitcure</param>
        void INetworkFolderJobOptions.SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
        {
            throw new NotImplementedException($"SelectOptimizeTextOrPitcure with setting {optimizeTextOrPicture} feature is not implemented on SiriusUIv3 devices");
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
            throw new NotImplementedException("SetErase edges feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Crop Option for the scanned file
        /// </summary>
        /// <param name="selectedCropOption">crop optoin type to select(case sensitive)</param>
        void INetworkFolderJobOptions.SelectCropOption(Cropping selectedCropOption)
        {
            throw new NotImplementedException($"SelectCropOption with setting {selectedCropOption} feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Blank Page Supress for scanned file
        /// </summary>
        /// <param name="optionType"> blank page supress to select(case sensitive)</param>
        void INetworkFolderJobOptions.SelectBlankPageSupress(BlankPageSupress optionType)
        {
            throw new NotImplementedException($"SelectBlankPageSupress with setting {optionType} feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Original Sides for the scanned file
        /// </summary>
        /// <param name="originalSides">The original Sides to select (case sensitive).</param>
        /// <param name="pageFlipUp">if set to <c>true</c>enable pageFlipUp ;otherwise,disable it</param>
        void INetworkFolderJobOptions.SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            throw new NotImplementedException("SelectOriginalSides with setting  feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the multiple files setting for the scanned file
        /// </summary>
        /// <param name="isChecked">isCreatemultipleFiles selected<value><c>true</c>if it is checked;otherwise,<c>false</c></value></param>
        /// <param name="pagesPerFile">pages per file value.</param>
        void INetworkFolderJobOptions.CreateMultipleFiles(bool isChecked, string pagesPerFile)
        {
            throw new NotImplementedException("CreateMultipleFiles feature is not implemented on SiriusUIv3 devices");
        }

        #endregion INetworkFolderJobOptions Members
    }
}