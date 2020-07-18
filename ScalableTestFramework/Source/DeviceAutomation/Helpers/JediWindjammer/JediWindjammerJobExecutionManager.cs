using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer
{
    /// <summary>
    /// Manages job execution on a <see cref="JediWindjammerDevice" />.
    /// </summary>
    public sealed class JediWindjammerJobExecutionManager : DeviceWorkflowLogSource
    {
        private int _jobBuildSegmentsScanned = 1;
        private string _lastStatus = string.Empty;

        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerJobExecutionManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerJobExecutionManager(JediWindjammerDevice device)
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
        /// <param name="appMainForm">The name of the application main form.</param>
        /// <returns><c>true</c> if the job finishes (regardless of its ending status), <c>false</c> otherwise.</returns>
        public bool ExecuteScanJob(ScanExecutionOptions executionOptions, string appMainForm)
        {
            bool done = false;
            if (executionOptions == null)
            {
                throw new ArgumentNullException(nameof(executionOptions));
            }

            try
            {
                if (executionOptions.JobBuildSegments > 1)
                {
                    RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
                }

                _controlPanel.PressToNavigate("mStartButton", "BaseJobStartPopup", ignorePopups: false);
            }
            catch (WindjammerInvalidOperationException)
            {
                if (_controlPanel.CurrentForm() == "ConflictDialog")
                {
                    string message = _controlPanel.GetProperty("mDetailsBrowser", "PageContent");
                    throw new DeviceWorkflowException($"Could not start job: {message}");
                }
                else
                {
                    // Let it flow on through. It might be another dialog we can handle.
                }
            }


            while (!done)
            {
                string currentForm = _controlPanel.CurrentForm();
                switch (currentForm)
                {
                    case "BaseJobStartPopup":
                        done = MonitorExecutingJob();
                        break;

                    case "JobBuildPrompt":
                        if (_jobBuildSegmentsScanned < executionOptions.JobBuildSegments)
                        {                                                        
                            _controlPanel.PressToNavigate("m_OKButton", "BaseJobStartPopup", ignorePopups: true);
                            SetPerformanceMarker(DeviceWorkflowMarker.ScanJobBegin.GetDescription(), "");
                            _jobBuildSegmentsScanned++;
                        }
                        else
                        {
                            try
                            {
                                RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
                                _controlPanel.PressToNavigate("mFinishButton", "BaseJobStartPopup", ignorePopups: false);
                            }
                            catch (WindjammerInvalidOperationException)
                            {
                                // Do nothing - will loop back around and try to handle the dialog
                            }
                        }
                        break;

                    case "FlatbedAutoDetectPrompt":
                        _controlPanel.PressToNavigate("m_OKButton", "BaseJobStartPopup", ignorePopups: true);
                        break;

                    case "AddressBookBatchAddDialog":
                        // Could take us to either the BaseJobStartPopup screen or the FlatbedAutoDetectPrompt
                        _controlPanel.PressWait("m_CancelButton", "BaseJobStartPopup", TimeSpan.FromSeconds(5));
                        break;

                    case "HPProgressPopup":
                        try
                        {
                            _controlPanel.WaitForForm("BaseJobStartPopup", ignorePopups: false);
                        }
                        catch (WindjammerInvalidOperationException)
                        {
                            // Do nothing - will loop back around and try to handle the dialog
                        }
                        break;

                    default:
                        if (currentForm == appMainForm)
                        {
                            // Sometimes the main form flashes in between other screens
                            // Only return if the form stays steady for a few seconds
                            if (!Wait.ForChange(_controlPanel.CurrentForm, appMainForm, TimeSpan.FromSeconds(3)))
                            {
                                done = true;
                            }
                        }
                        else
                        {
                            return done;
                        }
                        break;
                }
            }
            return done;
        }

        /// <summary>
        /// Monitors an executing job.
        /// </summary>
        /// <returns><c>true</c> if the job finishes (regardless of its ending status), <c>false</c> otherwise.</returns>
        public bool MonitorExecutingJob()
        {
            List<string> endingStatuses = new List<string>() { "Success", "Failed", "Partial Success", "Scheduled" };
            string performanceMarker = string.Empty;
            var ctlrs = _controlPanel.GetControls();
            int timeCount = 0;
            DateTime dt = DateTime.Now.AddSeconds(55);
            
            try
            {
                while (true)
                {
                    string status = _controlPanel.GetProperty("mStatusTextLine1", "Text");
                    if (status != _lastStatus || string.IsNullOrEmpty(performanceMarker))
                    {
                        performanceMarker = SetPerformanceMarker(performanceMarker, status);

                        _lastStatus = status;
                    }

                    if (endingStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
                    {
                        SetPerformanceMarkerEnd(performanceMarker);
                        return true;
                    }
                    else
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(250));
                    }
                    if (dt < DateTime.Now && timeCount < 4)
                    {
                        timeCount++;
                        _controlPanel.PressScreen(5, 5);
                        dt = DateTime.Now.AddSeconds(55);
                    }

                }
            }
            catch (ControlNotFoundException)
            {
                // Return false to indicate that another dialog has appeared
                SetPerformanceMarkerEnd(performanceMarker);
                return false;
            }
        }

        private void SetPerformanceMarkerEnd(string performanceMarker)
        {
            if (performanceMarker.Contains("Begin"))
            {
                if (performanceMarker.Equals(DeviceWorkflowMarker.SendingJobBegin.GetDescription()))
                {
                    RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
                }
                else if (performanceMarker.Equals(DeviceWorkflowMarker.ScanJobBegin.GetDescription()))
                {
                    RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                }
                else if (performanceMarker.Equals(DeviceWorkflowMarker.ProcessingJobBegin.GetDescription()))
                {
                    RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
                }
            }
        }

        private string SetPerformanceMarker(string performanceMarker, string status)
        {
            if (status.Contains("Processing"))
            {
                if (string.IsNullOrEmpty(performanceMarker))
                {
                    RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
                    performanceMarker = DeviceWorkflowMarker.ProcessingJobBegin.GetDescription();
                }
                else if (performanceMarker.Equals(DeviceWorkflowMarker.ScanJobBegin.GetDescription()))
                {
                    RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                    RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
                    performanceMarker = DeviceWorkflowMarker.ProcessingJobBegin.GetDescription();
                }
            }
            else if (status.Contains("Scanning"))
            {
                if (string.IsNullOrEmpty(performanceMarker))
                {
                    RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                    performanceMarker = DeviceWorkflowMarker.ScanJobBegin.GetDescription();
                }
                else if (performanceMarker.Equals(DeviceWorkflowMarker.ProcessingJobBegin))
                {
                    RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
                    //RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                    //performanceMarker = DeviceWorkflowMarker.ScanJobBegin.GetDescription();
                }
            }
            else if (status.Contains("Sending"))
            {
                if (performanceMarker.Equals(DeviceWorkflowMarker.ProcessingJobBegin))
                {
                    RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
                    RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
                    performanceMarker = DeviceWorkflowMarker.SendingJobBegin.GetDescription();
                }
                else if (performanceMarker.Equals(DeviceWorkflowMarker.ScanJobBegin.GetDescription()))
                {
                    RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                    RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
                    performanceMarker = DeviceWorkflowMarker.SendingJobBegin.GetDescription();
                }
            }


            return performanceMarker;
        }
    }
}
