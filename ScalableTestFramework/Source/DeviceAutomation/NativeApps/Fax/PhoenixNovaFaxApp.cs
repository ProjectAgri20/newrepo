using System;
using System.Linq;
using HP.DeviceAutomation.Phoenix;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.PhoenixNova;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Fax
{
    /// <summary>
    /// Implementation of <see cref="IFaxApp" /> for a <see cref="PhoenixDevice" />.
    /// </summary>
    public sealed class PhoenixNovaFaxApp : DeviceWorkflowLogSource, IFaxApp
    {
        private readonly PhoenixNovaControlPanel _controlPanel;
        private readonly PhoenixNovaJobExecutionManager _executionManager;
        private readonly PhoenixNovaFaxJobOptions _JobOptionsManager;

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PhoenixNovaFaxApp(PhoenixNovaDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
            _executionManager = new PhoenixNovaJobExecutionManager(device);
            _JobOptionsManager = new PhoenixNovaFaxJobOptions(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Fax application on the device. Not Immp
        /// </summary>
        public void Launch()
        {
            throw new NotImplementedException("Basic Launch method not implemented in PhoenixNovaFaxApp");
        }

        /// <summary>
        /// Launches the FAX solution with the specified authenticator using the given authentication mode.
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
                    _executionManager.PressApplicationButton(_executionManager.FaxButton);
                }
                catch (PhoenixInvalidOperationException ex)
                {
                    throw new DeviceWorkflowException($"Could not launch Copy application: {ex.Message}", ex);
                }
            }
            else
            {
                throw new NotImplementedException("Eager authentication has not been implemented for this solution.");
            }
        }

        /// <summary>
        /// Traverse to Fax Report  on Control Panel and Fetch the Html Fax Report
        /// </summary>
        /// <returns></returns>
        public string RetrieveFaxReport()
        {
            return "stuff";
        }

        /// <summary>
        /// Adds the specified recipient/s for the fax.
        /// </summary>
        /// <param name="recipients">The recipients. Contains PINs, if used.</param>
        /// <param name="useSpeedDial">Uses the #s as speed dials</param>
        public void AddRecipients(Dictionary<string, string> recipients, bool useSpeedDial)
        {
            _controlPanel.WaitForVirtualButton(_executionManager.FaxStartButton);
            _controlPanel.TypeOnVirtualKeyboard(recipients.First().Key);
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool completedJob = false;

            if (_executionManager.ButtonExist(_executionManager.FaxStartButton))
            {
                _executionManager.PressApplicationButton(_executionManager.FaxStartButton);
                _executionManager.PressSolutionButton("fax from flatbed scanner?", _executionManager.YesButton);

                var data = _controlPanel.GetDisplayedStrings();
                if (data.Any(display => display.Contains("Load page")))
                {
                    _executionManager.PressApplicationButton(_executionManager.OkayButton);
                    _executionManager.PressSolutionButton("Scan another page?", _executionManager.NoButton);
                }
                completedJob = _executionManager.DoneProcessingJob("Dialing");
            }
            return completedJob;
        }

        /// <summary>
        /// Adds the recipient.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        public void AddRecipient(string recipient)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the <see cref="IFaxJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IFaxJobOptions Options => _JobOptionsManager;

        private class PhoenixNovaFaxJobOptions : PhoenixNovaJobOptionsManager, IFaxJobOptions
        {
            public PhoenixNovaFaxJobOptions(PhoenixNovaDevice device)
                : base(device)
            {
            }
        }
    }
}
