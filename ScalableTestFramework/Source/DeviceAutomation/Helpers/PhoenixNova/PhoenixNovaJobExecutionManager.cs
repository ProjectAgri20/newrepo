using System;
using System.Collections;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation.Phoenix;

namespace HP.ScalableTest.DeviceAutomation.Helpers.PhoenixNova
{
    /// <summary>
    /// Manages job execution on a <see cref="PhoenixNovaDevice" />.
    /// </summary>
    public sealed class PhoenixNovaJobExecutionManager : DeviceWorkflowLogSource
    {
        private readonly PhoenixNovaControlPanel _controlPanel;

        /// <summary>
        /// Copy button on the control panel
        /// </summary>
        public string CopyButton { get; } = "cCopyHomeTouchButton";
        /// <summary>
        /// 
        /// </summary>
        public string FaxButton { get; } = "cFaxHomeTouchButton";
        /// <summary>
        /// 
        /// </summary>
        public string FaxStartButton { get; } = "cStartFaxTouchButton";
        /// <summary>
        /// 
        /// </summary>
        public string FaxMenuButton { get; } = "cFaxMenuTouchButton";
        /// <summary>
        /// 
        /// </summary>
        public string InfoButton { get; } = "InfoButton";
        /// <summary>
        /// 
        /// </summary>
        public string ScanButton { get; } = "cScanHomeTouchButton";
        /// <summary>
        /// 
        /// </summary>
        public string NumberOfCopiesButton { get; } = "cNumCopies";
        /// <summary>
        /// 
        /// </summary>
        public string JobSettingsButton { get; } = "cJobSettings";
        /// <summary>
        /// 
        /// </summary>
        public string OkayButton { get; } = "cOKTouchButton";
        /// <summary>
        /// 
        /// </summary>
        public string StartCopyButton { get; } = "cStartCopyTouchButton";
        /// <summary>
        /// 
        /// </summary>
        public string YesButton { get; } = "cYESString";
        /// <summary>
        /// 
        /// </summary>
        public string NoButton { get; } = "cNOString";


        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixNovaJobExecutionManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public PhoenixNovaJobExecutionManager(PhoenixNovaDevice device)
        {
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
                if(_controlPanel.WaitForVirtualButton("cScanTouchButton", TimeSpan.FromSeconds(1)))
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
        /// <summary>
        /// Deletes the field data located in the textbox
        /// </summary>
        public void DeleteFieldData()
        {
            Thread.Sleep(5000);

            var keyTexts = _controlPanel.GetDisplayedStrings();

            int count = keyTexts.ElementAt(0).Length;
            for (int idx = 0; idx < count; idx++)
            {
                if (ButtonExist("Del"))
                {
                    _controlPanel.Press("Del");
                    Thread.Sleep(300);
                }
            }
            Thread.Sleep(500);
        }
        /// <summary>
        /// Determines if the given button name exists on the current page.
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        /// <returns>bool</returns>
        public bool ButtonExist(string buttonName)
        {
            var buttons = _controlPanel.GetVirtualButtons();

            return buttons.Any(btn => btn.Name.Equals(buttonName));
        }
        /// <summary>
        /// Will continue to loop while the given job type is displayed on the control panel.
        /// A scan job will have "Scanning Page #" and a copy job, Copying Page ...
        /// </summary>
        /// <param name="jobType">Type of the job.</param>
        /// <returns></returns>
        public bool DoneProcessingJob(string jobType)
        {
            bool doneProcessing = false;

            while (!doneProcessing)
            {
                Thread.Sleep(250);
                doneProcessing = !FoundJobType(jobType);
            }

            return true;
        }
        /// <summary>
        /// Determines if the given job type is displayed on the screen
        /// </summary>
        /// <param name="jobType">Type of the job.</param>
        /// <returns></returns>
        private bool FoundJobType(string jobType)
        {
            IEnumerable jobTypes = _controlPanel.GetDisplayedStrings();

            return jobTypes.Cast<string>().Any(jt => jt.Contains(jobType));
        }
        /// <summary>
        /// Gets the name of the button based on the partial string
        /// </summary>
        /// <param name="partial">The partial.</param>
        /// <returns></returns>
        public string GetButtonName(string partial)
        {
            string btnName = string.Empty;

            var buttons = _controlPanel.GetVirtualButtons();
            foreach (VirtualButton btn in buttons.Where(btn => btn.Name.StartsWith(partial)))
            {
                btnName = btn.Name;
                break;
            }

            return btnName;
        }

        /// <summary>
        /// Presses the application button.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <exception cref="DeviceWorkflowException">Unable to find and press the application button " + applicationName</exception>
        public void PressApplicationButton(string applicationName)
        {
            bool foundButton = false;
            var buttons = _controlPanel.GetVirtualButtons();

            if (buttons.Any(btn => btn.Name.Equals(applicationName)))
            {
                _controlPanel.Press(applicationName);
                foundButton = true;
            }
            if (!foundButton)
            {
                throw new DeviceWorkflowException("Unable to find and press the application button " + applicationName);
            }
        }

        /// <summary>
        /// Presses the solution button.
        /// </summary>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="solutionName">Name of the solution.</param>
        /// <exception cref="DeviceWorkflowException">Unable to find and press the solution button " + solutionName</exception>
        public void PressSolutionButton(string menuName, string solutionName)
        {
            if (_controlPanel.WaitForDisplayedText(menuName, TimeSpan.FromSeconds(6)))
            {
                _controlPanel.WaitForVirtualButton(solutionName, TimeSpan.FromSeconds(6));
                _controlPanel.Press(solutionName);
                Thread.Sleep(1000);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to find and press the solution button " + solutionName);
            }
        }
    }
}
