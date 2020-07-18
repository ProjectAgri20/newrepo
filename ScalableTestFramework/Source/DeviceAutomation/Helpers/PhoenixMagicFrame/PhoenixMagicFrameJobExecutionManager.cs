using System;
using System.Threading;
using HP.DeviceAutomation.Phoenix;

namespace HP.ScalableTest.DeviceAutomation.Helpers.PhoenixMagicFrame
{
    /// <summary>
    /// Manages job execution on a <see cref="PhoenixMagicFrameDevice" />.
    /// </summary>
    public sealed class PhoenixMagicFrameJobExecutionManager : DeviceWorkflowLogSource
    {
        private readonly PhoenixMagicFrameControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixMagicFrameJobExecutionManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public PhoenixMagicFrameJobExecutionManager(PhoenixMagicFrameDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Executes the currently configured scan job.
        /// </summary>
        /// <param name="scanForm">The scan form.</param>
        /// <returns><c>true</c> if the job finishes (regardless of its ending status), <c>false</c> otherwise.</returns>
        public bool ExecuteScanJob(string scanForm)
        {
            bool done = false;
            try
            {
                if (_controlPanel.WaitForDisplayedText(scanForm, TimeSpan.FromSeconds(1)))
                {
                    _controlPanel.Press("cScanTouchButton");
                    RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                    Thread.Sleep(TimeSpan.FromSeconds(25));
                    done = true;
                }
            }
            catch (PhoenixInvalidOperationException ex)
            {
                throw new DeviceWorkflowException($"Could not start job: {ex.Message}");
            }

            return done;
        }
    }
}
