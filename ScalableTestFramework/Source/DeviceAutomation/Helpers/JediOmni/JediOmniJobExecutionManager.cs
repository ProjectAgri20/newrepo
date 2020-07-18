using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JediOmni
{
    /// <summary>
    /// Manages job execution on a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniJobExecutionManager : DeviceWorkflowLogSource
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniJobOptionsManager _optionsManager;
        private readonly JediOmniPopupManager _popupManager;
        private readonly JediOmniMasthead _masthead;
        private readonly TimeSpan _idleTimeoutOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniJobExecutionManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniJobExecutionManager(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediOmniJobOptionsManager(_device);
            _popupManager = new JediOmniPopupManager(_device);
            _masthead = new JediOmniMasthead(_device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Executes the currently configured scan job.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        /// <param name="startButton">The name of the start button.</param>
        /// <returns><c>true</c> if the job finishes (regardless of its ending status), <c>false</c> otherwise.</returns>
        public bool ExecuteScanJob(ScanExecutionOptions executionOptions, string startButton)
        {
            if (executionOptions == null)
            {
                throw new ArgumentNullException(nameof(executionOptions));
            }

            // Close the options panel if it is visible (applicable to 4.3" control panels)
            CloseOptionsPanel(startButton);

            // Close the keyboard if it is visible
            if (_controlPanel.CheckState("#hpid-keyboard-key-done", OmniElementState.Useable))
            {
                _controlPanel.PressWait("#hpid-keyboard-key-done", startButton);
            }

            // Check image preview availability and make sure it is compatible with the execution options
            ValidateImagePreviewAvailability(executionOptions, startButton);

            // Determine parameters and start job
            if (UseImagePreview(executionOptions))
            {
                ExecuteImagePreviewScan(executionOptions, startButton);
            }
            else if (executionOptions.JobBuildSegments > 1)
            {
                ExecuteJobBuildScan(executionOptions, startButton);
            }
            else
            {
                // Simple single page scan - no setup, just need to press the button
                _controlPanel.WaitForAvailable(startButton);
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                _controlPanel.Press(startButton);
                WaitForScanToFinish();
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            }

            // Wait for the job to finish and report success
            return WaitForJobFinish(executionOptions.ValidateJobExecution);
        }

        private void ExecuteImagePreviewScan(ScanExecutionOptions executionOptions, string startButton)
        {

            if (_controlPanel.GetScreenSize().Width > 480)
            {
                _controlPanel.WaitForAvailable(".hp-preview-touch-to-start-label");
            }
            else
            {
                _controlPanel.WaitForAvailable("#hpid-preview-button");
            }

            if (executionOptions.JobBuildSegments > 1)
            {
                RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
            }

            for (int i = 1; i <= executionOptions.JobBuildSegments; i++)
            {
                // Different button for 1st page than for the rest
                if (i == 1)
                {
                    _controlPanel.Press((_controlPanel.CheckState("#hpid-preview-button", OmniElementState.Exists)) ? "#hpid-preview-button" : ".hp-preview-touch-to-start-label");
                }
                else
                {
                    _controlPanel.WaitForAvailable("#hpid-preview-button-add-pages");
                    _controlPanel.PressWait("#hpid-preview-button-add-pages", "#hpid-original-size-label");
                    _popupManager.HandleScanMorePrompt(true);
                }
                HandlePopups(TimeSpan.FromSeconds(5));

                // Record timestamps
                RecordEvent(DeviceWorkflowMarker.ImagePreviewBegin);
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

                WaitForScanToFinish();

                // Record timestamps
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);
            }

            if (executionOptions.JobBuildSegments > 1)
            {
                RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
            }

            // Setup is finished - start the scan
            _controlPanel.WaitForAvailable(startButton);
            _controlPanel.Press(startButton);
        }

        private void WaitForScanToFinish()
        {
            //Added this HandleScanMorePrompt for scan job when scan mode is book mode or 2-sided Id, or when original sides is set as 2-sided (The scan more popup is encountered in these 3 cases).
            _popupManager.HandleScanMorePrompt(true);
            // Wait for the scan to finish (either the Add Pages button is enabled again, or the scan more prompt has been closed)
            bool scanDone() => _controlPanel.CheckState("#hpid-preview-button-add-pages", OmniElementState.Enabled) || _popupManager.HandleScanMorePrompt(false) || _popupManager.HandleRetainSettingsPopup(false);
            Wait.ForTrue(scanDone, _idleTimeoutOffset, TimeSpan.FromMilliseconds(250));
        }

        private void ExecuteJobBuildScan(ScanExecutionOptions executionOptions, string startButton)
        {
            _optionsManager.SetPromptForAdditionalPages();
            CloseOptionsPanel(startButton);

            // Start first page
            RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
            _controlPanel.Press(startButton);
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            HandlePopups(TimeSpan.FromSeconds(5));
            for (int i = 2; i <= executionOptions.JobBuildSegments; i++)
            {
                // Wait for the previous page to finish scanning, and press the scan button when prompted
                Wait.ForTrue(() => _popupManager.HandleScanMorePrompt(true), _idleTimeoutOffset, TimeSpan.FromMilliseconds(250));

                // Previous page is done scanning, and the current page has started scanning
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            }

            // Wait for the last page to finish scanning and press Done
            Wait.ForTrue(() => _popupManager.HandleScanMorePrompt(false), _idleTimeoutOffset, TimeSpan.FromMilliseconds(250));
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
        }

        private bool WaitForJobFinish(bool monitorActiveJobIcon)
        {
            // Check to see if the job starts sending - if not, there may be a popup in the way
            if (!_masthead.WaitForActiveJobsButtonState(true, TimeSpan.FromSeconds(5)))
            {
                HandlePopups(TimeSpan.FromSeconds(3));
                _masthead.WaitForActiveJobsButtonState(true, TimeSpan.FromSeconds(5));
            }
            RecordEvent(DeviceWorkflowMarker.SendingJobBegin);

            // Clear any popups and wait for the job to finish sending
            HandlePopups(TimeSpan.FromSeconds(3));
            _popupManager.HandleScanMorePrompt(false);

            // If monitorActiveJobIcon is false, set jobFinished as true, else invoke CheckJobFinished method
            bool jobFinished = (monitorActiveJobIcon == false) ? true : CheckJobFinished();

            return jobFinished;
        }

        private bool CheckJobFinished()
        {
            int count = 0;
            bool jobFinished = false;

            while ((jobFinished = _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset)) == false && count < 1)
            {
                count++;
                _controlPanel.SignalUserActivity();
            }

            if (jobFinished)
            {
                RecordEvent(DeviceWorkflowMarker.SendingJobEnd);

                // Wait for the job to finish processing
                RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
                HandlePopups(TimeSpan.FromSeconds(3));
                RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
            }

            return jobFinished;
        }

        private void ValidateImagePreviewAvailability(ScanExecutionOptions executionOptions, string startButton)
        {
            bool previewDisabled = _controlPanel.CheckState(".hp-preview-disabled", OmniElementState.VisiblePartially);
            bool previewRequired = _controlPanel.GetValue(startButton, "innerText", OmniPropertyType.Property) == "Preview";

            if (previewDisabled && executionOptions.ImagePreview == ImagePreviewOption.GeneratePreview)
            {
                throw new DeviceWorkflowException("Configuration requires image preview be used, but the device is not configured to allow image preview.");
            }
            else if (previewRequired && executionOptions.ImagePreview == ImagePreviewOption.RestrictPreview)
            {
                throw new DeviceWorkflowException("Configuration requires image preview not be used, but the device is configured to require image preview.");
            }
        }

        private bool UseImagePreview(ScanExecutionOptions executionOptions)
        {
            bool imagePreview = false;
            if (executionOptions.ImagePreview == ImagePreviewOption.GeneratePreview)
            {
                imagePreview = true;
            }
            else if (executionOptions.ImagePreview == ImagePreviewOption.RestrictPreview)
            {
                imagePreview = false;
            }
            else if (_controlPanel.CheckState(".hp-preview-touch-to-start-label", OmniElementState.VisiblePartially))
            {
                imagePreview = true;
            }
            else if (_controlPanel.CheckState("#hpid-preview-button", OmniElementState.Useable))
            {
                imagePreview = true;
            }

            return imagePreview;
        }

        /// <summary>
        /// Handles the popups.
        /// </summary>
        /// <param name="waitTime">The wait time.</param>
        private void HandlePopups(TimeSpan waitTime)
        {
            // Set up a list of popups to try to handle
            List<Func<bool>> handlers = new List<Func<bool>>
            {
                () => _popupManager.HandleCancelJobPopup(false),
                () => _popupManager.HandleRetainSettingsPopup(false),
                () => _popupManager.HandleAddContactPopup(false),
                () => _popupManager.HandleFileAlreadyExistsPopup(true),
                () => _popupManager.HandleFlatbedAutodetectPrompt(),
                () => _popupManager.HandleTemporaryPopup("Verifying access", _idleTimeoutOffset),
                () => _popupManager.HandleTemporaryPopup("Please wait", _idleTimeoutOffset),
                () => _popupManager.HandleTemporaryPopup(TimeSpan.FromSeconds(3))  // Keep this one last - handles generic popups that aren't covered above
            };

            // Keep looking for popups until we have gone 3 seconds without clearing one
            bool clearedLast = true;
            while (clearedLast && _popupManager.WaitForPopup(waitTime))
            {
                // If there is a popup, try each handler in turn to see if any of them clear it.
                // Short-circuiting will stop executing handlers as soon as one works.
                clearedLast = handlers.Any(n => n.Invoke() == true);
            }
        }

        /// <summary>
        /// Close the Options panel if it is visible (condition added to handle 4.3inch Control Panels)
        /// </summary>
        private void CloseOptionsPanel(string startButton)
        {
            if (_controlPanel.CheckState(".hp-header-levelup-button", OmniElementState.Useable))
            {
                _controlPanel.PressWait(".hp-header-levelup-button", startButton);
            }
        }
    }
}
