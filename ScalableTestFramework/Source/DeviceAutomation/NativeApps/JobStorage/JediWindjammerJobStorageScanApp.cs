using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage
{
    /// <summary>
    /// Implementation of <see cref="IJobStorageScanApp" /> for a <see cref="JediWindjammerDevice" />.
    /// </summary>
    public sealed class JediWindjammerJobStorageScanApp : DeviceWorkflowLogSource, IJobStorageScanApp
    {
        private readonly JediWindjammerJobStorageJobOptions _optionsManager;
        private Pacekeeper _pacekeeper;

        /// <summary>
        /// Constructor for Jediwindjammer device
        /// </summary>
        /// <param name="device">Jediwindjammer Device</param>
        public JediWindjammerJobStorageScanApp(JediWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _optionsManager = new JediWindjammerJobStorageJobOptions(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }
        
        /// <summary>
        /// Adds the specified name for the job
        /// </summary>
        /// <param name="jobname">The job name.</param> 
        public void AddJobName(string jobname)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Adds the specified name for the job along with the pin 
        /// </summary>
        /// <param name="jobname">The job name.</param>
        /// <param name="pin">Pin for the file.</param> 
        public void AddJobName(string jobname, string pin)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteScanJob(ScanExecutionOptions executionOptions)
        {
            //Implementation needs to be done
            throw new NotImplementedException($"Execution of  scan to jobstorage/save to device memory app is not implementedon windjammer device");
        }

        /// <summary>
        /// Launches the Scan to Job Storage application on the device.
        /// </summary>
        public void Launch()
        {
            throw new NotImplementedException($"Launch of scan to jobstorage/save to device memory app is not implemented on windjammer device");
        }

        /// <summary>
        /// Launches the Scan to Job Storage using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            throw new NotImplementedException($"Launch of  scan to jobstorage/save to device memory app is not implemented on windjammer device");
        }

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper
        {
            get
            {
                return _pacekeeper;
            }
            set
            {
                _pacekeeper = value;
                _optionsManager.Pacekeeper = _pacekeeper;
            }
        }

        /// <summary>
        /// Gets the <see cref="IJobStorageJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IJobStorageJobOptions Options => _optionsManager;

        private class JediWindjammerJobStorageJobOptions : JediWindjammerJobOptionsManager, IJobStorageJobOptions
        {
            public JediWindjammerJobStorageJobOptions(JediWindjammerDevice device)
                : base(device, "")
            {
            }

            
        }
    }
}
