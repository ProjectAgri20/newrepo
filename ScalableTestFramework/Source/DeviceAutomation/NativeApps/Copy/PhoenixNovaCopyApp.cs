using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation.Phoenix;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.PhoenixNova;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Copy
{
    /// <summary>
    /// Implementation of <see cref="ICopyApp" /> for a <see cref="PhoenixDevice" />.
    /// </summary>
    public sealed class PhoenixNovaCopyApp : DeviceWorkflowLogSource, ICopyApp
    {
        private readonly PhoenixNovaDevice _device;
        private readonly PhoenixNovaControlPanel _controlPanel;
        private readonly PhoenixNovaJobExecutionManager _executionManager;
        private readonly PhoenixNovaCopyJobOptionsManager _optionsManager;

        /// <summary>
        /// Available options for copy jobs
        /// </summary>
        public ICopyJobOptions Options => _optionsManager;

        /// <summary>
        /// Constructor for Copy App using a Phoenix Device
        /// </summary>
        /// <param name="device"></param>
        public PhoenixNovaCopyApp(PhoenixNovaDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new PhoenixNovaCopyJobOptionsManager(device);
            _executionManager = new PhoenixNovaJobExecutionManager(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Launches the Copy application on the device. Not Implemented    
        /// </summary>
        /// <exception cref="DeviceWorkflowException">Could not launch Copy application: {ex.Message}</exception>
        public void Launch()
        {
            throw new NotImplementedException("Basic Launch not implemented in PhoenixNovaCopyApp");
        }

        /// <summary>
        /// Launches the Copy application using the specified authenticator, authentication mode, and quickset.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quick set.</param>
        /// <exception cref="System.NotImplementedException">LaunchFromQuickset has not been implemented in PhoenixNovaCopyApp.</exception>
        public void LaunchFromQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName)
        {
            throw new NotImplementedException("LaunchFromQuickset has not been implemented in PhoenixNovaCopyApp.");
        }

        /// <summary>
        /// Keep in mind that the Phoenix Nova doesn't use the authentication.
        /// Launches the Copy application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="DeviceWorkflowException"></exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            try
            {
                _executionManager.PressApplicationButton(_executionManager.CopyButton);
            }
            catch (PhoenixInvalidOperationException ex)
            {
                throw new DeviceWorkflowException($"Could not launch Copy application: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="quickSetName">Name of the Quickset</param>
        /// </summary> 
        public void SelectQuickSet(string quickSetName)
        {
            throw new NotSupportedException("Not supported in Phoenix device");
        }

        /// <summary>
        /// If the StartCopyButton is present than the Nova device is mono.
        /// </summary>
        private void CheckNovaDevice()
        {
            if (_executionManager.ButtonExist(_executionManager.StartCopyButton))
            {
                _optionsManager.CopyButtonColorPress = _executionManager.StartCopyButton;
            }
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        /// <exception cref="DeviceWorkflowException">Could not launch Copy application at this time</exception>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            IEnumerable<string> data = _controlPanel.GetDisplayedStrings().ToList();

            CheckNovaDevice();
            if (data.ElementAt(0).StartsWith("Copy"))
            {
                // on a copy menu and can press a copy button
                _executionManager.PressSolutionButton(data.ElementAt(0), _optionsManager.CopyButtonColorPress);
                if (_executionManager.ButtonExist(_executionManager.OkayButton))
                {
                    _executionManager.PressApplicationButton(_executionManager.OkayButton);
                }
            }
            else
            {
                throw new DeviceWorkflowException($"Could not launch Copy application at this time");
            }
            var completedJob = _executionManager.DoneProcessingJob("Copying");
            return completedJob;
        }


    }
}
