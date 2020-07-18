using System;
using System.Threading;
using HP.DeviceAutomation.Phoenix;

namespace HP.ScalableTest.DeviceAutomation.Helpers.PhoenixNova
{
    /// <summary>
    /// Manages advanced job options on a <see cref="PhoenixDevice" />.
    /// </summary>
    public class PhoenixNovaJobOptionsManager : DeviceWorkflowLogSource
    {
        private readonly PhoenixNovaDevice _device;
        private readonly PhoenixNovaControlPanel _controlPanel;
        private readonly PhoenixNovaJobExecutionManager _executionManager;

        /// <summary>
        /// Gets/Sets Color copy string id
        /// </summary>
        public string CopyButtonColorPress { get; set; }

        /// <summary>
        /// Gets or sets the pacekeeper for this options manager.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; } = new Pacekeeper(TimeSpan.Zero);

        /// <summary>
        /// Constructor for Phoenix device taking a PhonixNovaDevice object
        /// </summary>
        /// <param name="device"></param>
        public PhoenixNovaJobOptionsManager(PhoenixNovaDevice device)
        {
            _device = device;
            _controlPanel = _device.ControlPanel;
            _executionManager = new PhoenixNovaJobExecutionManager(_device);
        }

        /// <summary>
        /// Sets CopyButtonColorPress based on short string
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(string color)
        {
            switch (color.ToLower())
            {
                case "color":
                    CopyButtonColorPress = "cColorCopyTouchButton";
                    break;
                case "black":
                    CopyButtonColorPress = "cBlackCopyTouchButton";
                    break;
                default:
                    CopyButtonColorPress = "cBlackCopyTouchButton";
                    break;
            }
        }

        /// <summary>
        /// Literally does nothing.
        /// </summary>
        /// <param name="enable"></param>
        public void SetJobBuildState(bool enable)
        {
            throw new NotImplementedException($"Set job buildstate  feature is not implemented on PhoenixNova devices");
        }

        /// <summary>
        /// Types the number of copies to print in the control panel
        /// </summary>
        /// <param name="copies"></param>
        public void SetNumCopies(int copies)
        {
            _executionManager.PressSolutionButton("Copy", _executionManager.JobSettingsButton);
            _executionManager.PressSolutionButton("Copy Menu", _executionManager.NumberOfCopiesButton);

            _controlPanel.TypeOnVirtualKeyboard(copies.ToString());
            Thread.Sleep(1000);
            _controlPanel.Press(_executionManager.OkayButton);
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Enables print notification for this job. Stub Code as phoenix doesn't implement
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        public void EnablePrintNotification(NotifyCondition condition,bool thumbNail)
        {
            throw new NotImplementedException($"Enable print notification  feature is not implemented on PhoenixNova devices");
        }

        /// <summary>
        /// Enables email notification for this job.Stub Code as Phoenix doesn't Implement
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="address">The email address to receive the notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        public void EnableEmailNotification(NotifyCondition condition, string address,bool thumbNail)
        {
            throw new NotImplementedException($"Enable email notification  feature is not implemented on PhoenixNova devices");
        }

        /// <summary>
        /// Selects the resolution for the selected file
        /// </summary>
        /// <param name="resolution">The resolution to select(case sensitive).</param>
        public void SelectFaxResolution(ResolutionType resolution)
        {
            throw new NotImplementedException($"Select fax resolution has not been implemented for Phoenix Nova devices.");
        }

        /// <summary>
        /// Selects the Original Size for the scanned file
        /// </summary>
        /// <param name="originalSize">The original Size to select (case sensitive).</param>
        public void SelectOriginalSize(OriginalSize originalSize)
        {
            throw new NotImplementedException($"Select original size has not been implemented for Phoenix Nova devices.");
        }

        /// <summary>
        /// Selects the Content Orientation  for the scanned file
        /// </summary>
        /// <param name="contentOrientation">content orientation.</param>
        public void SelectContentOrientation(ContentOrientation contentOrientation)
        {
            throw new NotImplementedException($"Select content orientation has not been implemented for Phoenix Nova devices.");
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
            throw new NotImplementedException($"Select image adjustment has not been implemented for Phoenix Nova devices.");
        }

        /// <summary>
        /// Selects the Optimize text or pitcure  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">Selects the text or pitcure</param>
        public void SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
        {
            throw new NotImplementedException($"Select optimize text or picture has not been implemented for Phoenix Nova devices.");
        }

        /// <summary>
        /// Selects the Blank Page Supress for scanned file
        /// </summary>
        /// <param name="optionType"> blank page supress to select(case sensitive)</param>
        public void SelectBlankPageSupress(BlankPageSupress optionType)
        {
            throw new NotImplementedException($"Select blank page supress has not been implemented for Phoenix Nova devices.");
        }

        /// <summary>
        /// Selects the Original Sides for the scanned file
        /// </summary>
        /// <param name="originalSides">The original Sides to select (case sensitive).</param>
        /// <param name="pageFlipUp">if set to <c>true</c>enable pageFlipUp ;otherwise,disable it</param>
        public void SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            throw new NotImplementedException($"Select original sides has not been implemented for Phoenix Nova devices.");
        }
    }
}
