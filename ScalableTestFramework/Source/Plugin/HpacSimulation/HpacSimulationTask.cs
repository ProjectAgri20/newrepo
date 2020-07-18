using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.PluginSupport.PullPrint.Simulation;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpacSimulation
{
    public class HpacSimulationTask
    {
        private HpacSimulationController _hpacController = null;
        private HpacSimulationActivityData _taskConfig = null;
        private IPrinterInfo _device = null;
        private List<File> _printJobs = null;

        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        public HpacSimulationTask(HpacSimulationController controller, HpacSimulationActivityData activityData, IPrinterInfo device)
            : base()
        {
            _hpacController = controller;
            _taskConfig = activityData;
            _device = device;
        }

        public void Execute()
        {
            Authenticate();
            PerformPullPrintAction();
            Thread.Sleep(5000);
            ValidatePull();
        }

        private void PerformPullPrintAction()
        {
            GetHpacJobListing();

            if (_taskConfig.PullAllDocuments)
            {
                PullAllJobs(_device.Address);
            }
            else
            {
                PullSingleJob(_device.Address);
            }
        }

        private void Authenticate()
        {
            switch (_taskConfig.HpacAuthenticationMode)
            {
                case HpacAuthenticationMode.Pic:
                    string code = new string(_hpacController.UserCredential.UserName.Where(Char.IsDigit).ToArray());
                    _hpacController.AuthenticateUser(code);
                    UpdateStatus($"User Authenticated: { (object)_hpacController.UserCredential.UserName}  PIN: { (object)code}");
                    break;
                case HpacAuthenticationMode.SmartCard:
                    throw new NotImplementedException("Smart Card Authentication is not enabled for HPAC simulation.");
                default: //Defaults to HpacAuthenticationMode.DomainCredentials 
                    UpdateStatus($"Using Windows Credentials for: {_hpacController.UserCredential.UserName}");
                    break;
            }
        }

        /// <summary>
        /// Pulls the first print job available in the queue.
        /// Note that a document that enters the queue as multiple print jobs (Excel) is treated as a single pull operation,
        /// even though multiple jobs are pulled.
        /// </summary>
        /// <param name="deviceAddress"></param>
        /// <returns></returns>
        private void PullSingleJob(string deviceAddress)
        {
            List<string> failedJobs = new List<string>();

            ExecutionServices.SystemTrace.LogDebug($"Pulling '{_printJobs[0].JobName}'. {_printJobs.Count} print jobs.");

            // Go through each job and attempt to pull it
            foreach (File file in _printJobs)
            {
                if (!PullSingleJob(deviceAddress, file))
                {
                    failedJobs.Add(file.Guid);
                }
                // Give the server a chance to process the pull request.  If this is done too quickly, we will get what appears to be the cached job list
                // with the requested job removed.
                Thread.Sleep(5000);
            }

            // If there were any errors when pulling, throw an exception
            if (failedJobs.Count > 0)
            {
                throw new PullPrintingSimulationException($"Error detected in pulling {failedJobs.Count} jobs, see log for more details.");
            }
        }

        private void GetHpacJobListing()
        {
            _printJobs = _hpacController.GetFirstJob();
            if (_printJobs == null || _printJobs.Count == 0)
            {
                string logText = "No jobs available.";
                throw new EmptyQueueException(logText);
            }
        }

        /// <summary>
        /// Pulls a single print job by Job ID.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool PullSingleJob(string deviceAddress, File file)
        {
            UpdateStatus($"Pulling Job '{file.JobName}'  JobId: {file.Guid}");

            string pullResponse = _hpacController.PullJob(deviceAddress, file.Guid, true); // Pulls the specified job from the print queue

            if (pullResponse != string.Empty)
            {
                string logText = $"Error: {pullResponse}";
                UpdateStatus(logText);
                ExecutionServices.SystemTrace.LogError(logText);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Pulls all print jobs for the given user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="deviceAddress"></param>
        private void PullAllJobs(string deviceAddress)
        {
            string logText = $"Pulling all Jobs for {_hpacController.UserCredential.UserName}";
            UpdateStatus(logText);
            ExecutionServices.SystemTrace.LogDebug(logText);

            string response = _hpacController.PullAllJobs(deviceAddress);

            if (response != string.Empty)
            {
                throw new PullPrintingSimulationException(response);
            }
        }

        /// <summary>
        /// 7/22/2014 - kyoungman : Validation of PullAll has proven to be problematic,
        /// 1. Because there's no good way to determine the list of jobs pulled, and
        /// 2. Because the validation code constantly grabs the list of print jobs causing a lot of churn on the HPAC server.
        /// For now validation will only occur for single pull jobs.
        /// </summary>
        /// <param name="userName"></param>
        private void ValidatePull()
        {
            if (!_taskConfig.PullAllDocuments)
            {
                ValidatePullSingle();
            }
            else
            {
                ValidatePullAll();
            }
        }

        /// <summary>
        /// Validates a pull operation by job name.
        /// This allows a document that enters the queue as multiple print jobs (Excel) to be validated as a single pull operation.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="filesPulled">The print jobs to be pulled.</param>
        /// <returns></returns>
        private void ValidatePullSingle()
        {
            List<string> failedJobs = new List<string>();

            ExecutionServices.SystemTrace.LogDebug($"Validating Pull for '{_printJobs[0].JobName}'. {_printJobs.Count} print jobs.");

            // Go through each job and validate it
            foreach (File file in _printJobs)
            {
                if (!ValidatePullSingle(file))
                {
                    failedJobs.Add(file.Guid);
                }
                // Give the server a chance to process the pull request.  If this is done too quickly, we will get what appears to be the cached job list
                // with the requested job removed.
                Thread.Sleep(5000);
            }

            // If there were any errors, throw an exception
            if (failedJobs.Count > 0)
            {
                throw new PullPrintingSimulationException($"One or more Print Jobs associated with '{_printJobs[0].JobName}' failed to validate.");
            }
        }

        /// <summary>
        /// Determines if a single pull print operation was successful. Checks the Guid of the pulled file against those in the collection.
        /// Retries several times if needed to compensate for possible lag from the server.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="file">The print job to validate.</param>
        /// <returns>True if the print job was not found on the server.</returns>
        private bool ValidatePullSingle(File file)
        {
            StringBuilder logText = new StringBuilder();

            // Grab a collection of jobs available.
            List<File> jobList = _hpacController.GetJobCollection();

            bool result = !jobList.Any(j => j.Guid == file.Guid); // Means that there are still jobs in the print queue that should have been removed

            // If the job is still present in the list, retry for up to 30 min,
            // waiting an additional 5 seconds with each subsequent attempt.
            int i = 1, seconds = 0, elapsed = 0;
            while (result == false && elapsed <= 900) //Validate for 15 minutes
            {
                seconds = i * 5;
                logText.Clear();
                logText.Append("Print Job '").Append(file.Guid).Append("' was not removed from the server.  ");
                logText.Append("Waiting ").Append(seconds).Append(" seconds before re-validating.");
                //TraceFactory.Logger.Debug(logText.ToString());
                UpdateStatus(logText.ToString());

                Thread.Sleep(seconds * 1000);
                elapsed += seconds;

                logText.Clear();
                logText.Append("Re-checking the server. Retry Attempt #").Append(i);
                UpdateStatus(logText.ToString());
                ExecutionServices.SystemTrace.LogDebug(logText.ToString());

                jobList = _hpacController.GetJobCollection();
                result = !jobList.Any(j => j.Guid == file.Guid);
                i++;
            }

            if (result)
            {
                UpdateStatus($"Pull print activity successful, Job removed from queue: '{file.JobName}'  JobId: {file.Guid}");
            }
            else
            {
                logText.Clear();
                logText.Append("Job was not removed after the pull operation. Deleting '");
                logText.Append(file.JobName);
                logText.Append("'  JobId: ");
                logText.Append(file.Guid);

                _hpacController.DeleteJob(file.Guid);

                ExecutionServices.SystemTrace.LogError(logText);
                UpdateStatus(logText.ToString());
            }

            return result;
        }

        /// <summary>
        /// Validates a pull-all operation.
        /// 7/22/2014 - kyoungman : Validation of PullAll has proven to be problematic,
        /// 1. Because there's no good way to determine the list of jobs pulled, and
        /// 2. Because the validation code constantly grabs the list of print jobs causing a lot of churn on the HPAC server.
        /// For now this method is not being called.
        /// </summary>
        /// <param name="userName"></param>
        private void ValidatePullAll()
        {
            StringBuilder logText = new StringBuilder("Validating Pull All...");
            UpdateStatus(logText.ToString());
            ExecutionServices.SystemTrace.LogDebug(logText);

            int cnt = _hpacController.GetJobCollection().Count;
            bool result = cnt == 0;
           // bool result = _hpacController.GetJobCollection().Count < 1;
            logText.Clear();

            int i = 1, seconds = 0, elapsed = 0;
            while (result == false && elapsed <= 900) //Validate for 15 minutes
            {
                seconds = i * 5;
                logText.Clear();
                logText.Append("Print jobs still found in queue, waiting ").Append(seconds).Append(" seconds before re-validating.");
                UpdateStatus(logText.ToString());

                Thread.Sleep(seconds * 1000);
                elapsed += seconds;

                logText.Clear();
                logText.Append("Re-checking the server. Retry Attempt #").Append(i);
                UpdateStatus(logText.ToString());
                ExecutionServices.SystemTrace.LogDebug(logText.ToString());

                result = _hpacController.GetJobCollection().Count < 1;
                i++;
            }

            if (result)
            {
                UpdateStatus("Pull all successful, no print jobs left in the queue.");
            }
            else
            {
                logText.Clear();
                logText.Append("Pull all unsuccessful, print jobs are still in the queue.");
                ExecutionServices.SystemTrace.LogDebug("Pull all unsuccessful. Clearing all print jobs.");
                _hpacController.DeleteAllJobs();
                UpdateStatus(logText.ToString());
                throw new PullPrintingSimulationException(logText.ToString());
            }
        }

        private void UpdateStatus(string message)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }
    }
}
