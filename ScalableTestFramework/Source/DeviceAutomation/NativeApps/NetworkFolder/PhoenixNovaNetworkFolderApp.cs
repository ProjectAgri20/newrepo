using System;
using System.Net;
using System.Threading;
using HP.DeviceAutomation.Phoenix;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.PhoenixNova;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder
{
    /// <summary>
    /// Implementation of <see cref="INetworkFolderApp" /> for a <see cref="PhoenixNovaDevice" />.
    /// </summary>
    public sealed class PhoenixNovaNetworkFolderApp : DeviceWorkflowLogSource, INetworkFolderApp, INetworkFolderJobOptions
    {
        private readonly PhoenixNovaControlPanel _controlPanel;
        private readonly PhoenixNovaJobExecutionManager _executionManager;

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixNovaNetworkFolderApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public PhoenixNovaNetworkFolderApp(PhoenixNovaDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
            _executionManager = new PhoenixNovaJobExecutionManager(device);
        }

        /// <summary>
        /// Launches the Network Folder application on the device.
        /// </summary>
        public void Launch()
        {
            try
            {
                _controlPanel.Press(_executionManager.ScanButton);
                if (_controlPanel.WaitForDisplayedText("Scan Menu", TimeSpan.FromSeconds(3)))
                {
                    _controlPanel.Press("cScanToNetworkFolder");
                }

                Pacekeeper.Pause();
            }
            catch (PhoenixInvalidOperationException ex)
            {
                throw new DeviceWorkflowException($"Could not launch Select network folder page: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Launches Scan to Network Folder using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="DeviceWorkflowException"></exception>
        /// <exception cref="System.NotImplementedException">Eager authentication has not been implemented for this solution.</exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                try
                {
                    _executionManager.PressApplicationButton(_executionManager.ScanButton);
                    _executionManager.PressSolutionButton("Scan Menu", "cScanToNetworkFolder");
                }
                catch (PhoenixInvalidOperationException ex)
                {
                    throw new DeviceWorkflowException($"Could not launch Select network folder page: {ex.Message}", ex);
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
            _executionManager.PressSolutionButton("Scan to Network Folder", _executionManager.JobSettingsButton);
            _controlPanel.ScrollDown(50);

            _executionManager.PressSolutionButton("File Name Prefix", "cFilenamePrefix");
            _executionManager.DeleteFieldData();

            _controlPanel.TypeOnVirtualKeyboard(fileName);
            Thread.Sleep(1000);

            _controlPanel.Press(_executionManager.OkayButton);
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Adds the specified network folder path as a destination for the scanned file.
        /// </summary>
        /// <param name="path">The network folder path.</param>
        /// <param name="credential">The network credential parameter is being added to provide the network credentials to access folder path </param>
        /// /// <param name="applyCredentials"><value><c>true</c> if apply credentials on verification is checked;otherwise, <c>false</c>.</value></param>
        public void AddFolderPath(string path, NetworkCredential credential, bool applyCredentials)////network credential for folder access is supported for Omni device.PhoenixNova yet to be supported
        {
            try
            {
                // Phoenix does not support but can just press the first button
                var buttons = _controlPanel.GetVirtualButtons();
                if (buttons.Count == 1)
                {
                    _executionManager.PressSolutionButton("Select network folder.", buttons[0].Name);
                }

                Pacekeeper.Sync();
            }
            catch (PhoenixInvalidOperationException ex)
            {
                throw new DeviceWorkflowException($"Error selecting folder path: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// </summary>
        /// <param name="quicksetName">The quickset name.</param>
        public void SelectQuickset(string quicksetName)
        {
            _executionManager.PressSolutionButton("Select network folder.", quicksetName);
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
            var jobCompleted = _executionManager.ExecuteScanJob("Scan Settings");
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
            throw new NotImplementedException($"Unable to set job resolution to resolution:{resolution}. Resolution Selection is not implemented on PhoenixNova devices.");
        }

        /// <summary>
        /// Selects the Color/Black for the scanned file
        /// </summary>
        /// <param name="colorOrBlack">The Color/Black is selected.</param>
        void INetworkFolderJobOptions.SelectColorOrBlack(ColorType colorOrBlack)
        {
            throw new NotImplementedException($"SelectColor or Black  with setting {colorOrBlack} feature is not implemented on PhoenixNova devices");
        }

        /// <summary>
        /// Selects the Original Size for the scanned file
        /// </summary>
        /// <param name="originalSize">The original Size to select (case sensitive).</param>
        void INetworkFolderJobOptions.SelectOriginalSize(OriginalSize originalSize)
        {
            throw new NotImplementedException($"SelectOriginalSize with setting {originalSize} feature is not implemented on PhoenixNova devices");
        }

        /// <summary>
        /// Selects the Content Orientation  for the scanned file
        /// </summary>
        /// <param name="contentOrientation">content orientation.</param>
        void INetworkFolderJobOptions.SelectContentOrientation(ContentOrientation contentOrientation)
        {
            throw new NotImplementedException($"SelectContentOrientation with setting {contentOrientation} feature is not implemented on PhoenixNova devices");
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
            throw new NotImplementedException($"SetImageadjustment feature is not implemented on PhoenixNova devices");
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
            throw new NotImplementedException($"SetImageadjustment feature is not implemented on PhoenixNova devices");
        }

        /// <summary>
        /// Selects the Optimize text or pitcure  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">Selects the text or pitcure</param>
        void INetworkFolderJobOptions.SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
        {
            throw new NotImplementedException($"SelectOptimizeTextOrPitcure with setting {optimizeTextOrPicture} feature is not implemented on PhoenixNova devices");
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
            throw new NotImplementedException("SetErase edges feature is not implemented on PhoenixNova devices");
        }

        /// <summary>
        /// Selects the Crop Option for the scanned file
        /// </summary>
        /// <param name="selectedCropOption">crop optoin type to select(case sensitive)</param>
        void INetworkFolderJobOptions.SelectCropOption(Cropping selectedCropOption)
        {
            throw new NotImplementedException($"SelectCropOption with setting {selectedCropOption} feature is not implemented on PhoenixNova devices");
        }

        /// <summary>
        /// Selects the Blank Page Supress for scanned file
        /// </summary>
        /// <param name="optionType"> blank page supress to select(case sensitive)</param>
        void INetworkFolderJobOptions.SelectBlankPageSupress(BlankPageSupress optionType)
        {
            throw new NotImplementedException($"SelectBlankPageSupress with setting {optionType} feature is not implemented on PhoenixNova devices");
        }

        /// <summary>
        /// Selects the Original Sides for the scanned file
        /// </summary>
        /// <param name="originalSides">The original Sides to select (case sensitive).</param>
        /// <param name="pageFlipUp">if set to <c>true</c>enable pageFlipUp ;otherwise,disable it</param>
        void INetworkFolderJobOptions.SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            throw new NotImplementedException("SelectOriginalSides with setting feature is not implemented on PhoenixNova devices");
        }

        /// <summary>
        /// Selects the multiple files setting for the scanned file
        /// </summary>
        /// <param name="isChecked">isCreatemultipleFiles selected<value><c>true</c>if it is checked;otherwise,<c>false</c></value></param>
        /// <param name="pagesPerFile">pages per file value.</param>
        void INetworkFolderJobOptions.CreateMultipleFiles(bool isChecked, string pagesPerFile)
        {
            throw new NotImplementedException("CreateMultipleFiles feature is not implemented on PhoenixNova devices");
        }

        #endregion
    }
}
