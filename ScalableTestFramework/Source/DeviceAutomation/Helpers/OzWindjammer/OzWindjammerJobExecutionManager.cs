using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Oz;

namespace HP.ScalableTest.DeviceAutomation.Helpers.OzWindjammer
{
    /// <summary>
    /// Manages job execution on an <see cref="OzWindjammerDevice" />.
    /// </summary>
    public sealed class OzWindjammerJobExecutionManager : DeviceWorkflowLogSource
    {
        private const int _jobBuildPrompt = 36;
        private const int _jobBuildScanningScreen = -776;
        private const int _jobProcessingPopup = -791;

        private readonly OzWindjammerDevice _device;
        private readonly OzWindjammerControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="OzWindjammerJobExecutionManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public OzWindjammerJobExecutionManager(OzWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
        }

        /// <summary>
        /// Executes the currently configured scan job.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        /// <returns><c>true</c> if the job finishes (regardless of its ending status), <c>false</c> otherwise.</returns>
        public bool ExecuteScanJob(ScanExecutionOptions executionOptions)
        {
            bool completedJob = false;
            if (executionOptions == null)
            {
                throw new ArgumentNullException(nameof(executionOptions));
            }

            if (executionOptions.JobBuildSegments > 1)
            {
                RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
            }
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            _controlPanel.PressKey(OzHardKey.Start);

            if (executionOptions.JobBuildSegments > 1)
            {
                for (int i = 1; i <= executionOptions.JobBuildSegments; i++)
                {
                    _controlPanel.WaitForScreen(_jobBuildScanningScreen, TimeSpan.FromSeconds(10));
                    _controlPanel.WaitForScreen(_jobBuildPrompt, TimeSpan.FromSeconds(60));
                    Wait.ForNotNull(() => _controlPanel.GetWidgets().Find("Scan"), TimeSpan.FromSeconds(5));

                    // If this is the last time, press "Finish" instead of "Scan"
                    if (i == executionOptions.JobBuildSegments)
                    {
                        RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                        _controlPanel.Press("Finish");
                    }
                    else
                    {
                        RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                        RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                        _controlPanel.Press("Scan");
                    }
                }
                RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
            }

            // Wait for the prompt asking whether to stay logged in - this means the job is done scanning.
            if (_controlPanel.WaitForScreen(_jobProcessingPopup, TimeSpan.FromSeconds(30)))
            {
                RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
                Widget yesWidget = _controlPanel.GetWidgets().First(n => n.Text == "Yes" || n.Text == "OK");
                _controlPanel.Press(yesWidget);
            }

            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
            completedJob = true;
            return completedJob;
        }
    }
}
