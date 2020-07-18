using HP.DeviceAutomation.Sirius;
using System;
using System.Linq;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.Helpers.SiriusUIv3
{
    /// <summary>
    /// Manages job execution on a <see cref="SiriusUIv3Device" />.
    /// </summary>
    public sealed class SiriusUIv3JobExecutionManager : DeviceWorkflowLogSource
    {
        private readonly SiriusUIv3ControlPanel _controlPanel;
        private readonly TimeSpan _waitTimeSpan = TimeSpan.FromSeconds(10);
        private readonly TimeSpan _shortWaitTimeSpan = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3JobExecutionManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public SiriusUIv3JobExecutionManager(SiriusUIv3Device device)
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
        /// <param name="executionOptions">The execution options.</param>
        /// <param name="appMainForm">The application main form.</param>
        /// <returns><c>true</c> if the job finishes (regardless of its ending status), <c>false</c> otherwise.</returns>
        public bool ExecuteScanJob(ScanExecutionOptions executionOptions, string appMainForm)
        {
            string documentType = string.Empty;
            try
            {
                //   if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault().Equals(appMainForm))
                if (_controlPanel.WaitForScreenLabel(appMainForm))
                {
                    if (_controlPanel.WaitForWidget("fb_settings", _shortWaitTimeSpan) != null)
                    {
                        _controlPanel.Press("fb_settings");
                        Thread.Sleep(_shortWaitTimeSpan);
                    }

                    if (_controlPanel.GetScreenInfo().ScreenLabels.Contains("Scan_Glass_Settings") || _controlPanel.GetScreenInfo().ScreenLabels.Contains("Scan_ADF_Settings"))
                    {
                        //_controlPanel.Press("command.save_as");
                        ScreenInfo screen = _controlPanel.GetScreenInfo();
                        Widget widget = screen.Widgets.First(n => n.Id == "command.save_as");
                        if (widget.Values["secondarytext"] == "JPEG")
                        {
                            documentType = "JPEG";
                        }
                        _controlPanel.PressKey(SiriusSoftKey.Back);
                        Thread.Sleep(_shortWaitTimeSpan);
                    }

                    RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                    _controlPanel.Press("fb_start_scan");
                    Thread.Sleep(_shortWaitTimeSpan);

                    while (_controlPanel.WaitForScreenLabel("Scan_Status_Connecting", _shortWaitTimeSpan))
                    {
                        //wait for this screen to get over
                        Thread.Sleep(200);
                    }

                    while (_controlPanel.WaitForScreenLabel("Scan_Status_ScanningPage", _shortWaitTimeSpan))
                    {
                        Thread.Sleep(200);
                    }

                    if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault().Equals("Scan_Status_Error"))
                    {
                        string exceptionMessage = string.Empty;
                        try
                        {
                            Widget controlInfo = _controlPanel.GetScreenInfo().Widgets.Find("text");
                            if (controlInfo != null)
                            {
                                exceptionMessage = controlInfo.Values.FirstOrDefault().Value;
                            }
                        }
                        catch (Exception)
                        {
                            //ignore
                        }

                        throw new DeviceWorkflowException($"Scan Error occurred.{exceptionMessage}");
                    }

                    ExecuteScan(executionOptions, appMainForm, documentType);

                    RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                }
                CheckForScanError();
            }
            catch (SiriusInvalidOperationException)
            {
                if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault().Equals("Scan_Status_Error"))
                {
                    Widget controlInfo = _controlPanel.GetScreenInfo().Widgets.Find("email_error_msg");
                    string message = "Cannot connect to server. Check server name and address.";
                    if (controlInfo.HasValue(message))
                    {
                        throw new DeviceWorkflowException($"Could not start job: {message}");
                    }
                }
                return false;
            }
            return true;
        }

        private void ExecuteScan(ScanExecutionOptions executionOptions, string appMainForm, string documentType)
        {
            for (int scanCount = 1; scanCount < executionOptions.JobBuildSegments; scanCount++)
            {
                if (documentType.Equals("JPEG"))
                {
                    if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault().Equals(appMainForm))
                    {
                        _controlPanel.Press("fb_start_scan");//start Scan
                        while (_controlPanel.WaitForScreenLabel("Scan_Status_Connecting", _shortWaitTimeSpan))
                        {
                            //wait for this screen to get over
                            Thread.Sleep(200);
                        }

                        while (_controlPanel.WaitForScreenLabel("Scan_Status_ScanningPage", _shortWaitTimeSpan))
                        {
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Waiting to Click on Start Scan");
                    }
                }
                else
                {
                    if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault().Equals("Scan_AnotherPage"))
                    {
                        //Start Scan
                        _controlPanel.Press("mdlg_action_button");
                        Thread.Sleep(_shortWaitTimeSpan);
                        if (_controlPanel.WaitForScreenLabel("Scan_PlacePage", _shortWaitTimeSpan))
                        {
                            _controlPanel.Press("mdlg_action_button");
                            Thread.Sleep(_shortWaitTimeSpan);
                        }
                        while (_controlPanel.WaitForScreenLabel("Scan_Status_Connecting", _shortWaitTimeSpan))
                        {
                            //wait for this screen to get over
                            Thread.Sleep(200);
                        }

                        while (_controlPanel.WaitForScreenLabel("Scan_Status_ScanningPage", _shortWaitTimeSpan))
                        {
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Waiting to click on Yes button");
                    }
                }
            }

            if (!documentType.Equals("JPEG"))
            {
                if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault().Equals("Scan_Status_Error"))
                {
                    throw new DeviceWorkflowException($"Scan Error occurred.");
                }

                if (_controlPanel.WaitForScreenLabel("Scan_AnotherPage", _waitTimeSpan))
                {
                    _controlPanel.Press("mdlg_option_button");
                    Thread.Sleep(_shortWaitTimeSpan);
                }
                else
                {
                    throw new DeviceWorkflowException("Failed while waiting to click on No button");
                }
            }
        }

        private void CheckForScanError()
        {
            if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault().Equals("Scan_Status_Error"))
            {
                string exceptionMessage = string.Empty;
                try
                {
                    Widget controlInfo = _controlPanel.GetScreenInfo().Widgets.Find("text");
                    if (controlInfo != null)
                    {
                        exceptionMessage = controlInfo.Values.FirstOrDefault().Value;
                    }
                }
                catch (Exception)
                {
                    //ignore
                }

                throw new DeviceWorkflowException($"Scan Error occurred.{exceptionMessage}");
            }
        }
    }
}