using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.PluginSupport.PullPrint.Simulation;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.SafeComSimulation
{
    public class SafeComSimulationTask
    {
        private SafeComSimulationController _safecomController = null;
        private SafeComSimulationActivityData _taskConfig = null;
        private IPrinterInfo _device = null;
        private List<int> _printJobs = null;

        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        public SafeComSimulationTask(SafeComSimulationController controller, SafeComSimulationActivityData activityData, IPrinterInfo device)
            : base()
        {
            _safecomController = controller;
            _taskConfig = activityData;
            _device = device;
        }

        public void Execute()
        {
            _safecomController.Authenticate();
            PerformPullPrintAction();
            Thread.Sleep(5000);
            // 7/22/2014:  Removed all validation.  See note in ValidatePull() header.
        }

        private void PerformPullPrintAction()
        {
            if (_taskConfig.PullAllDocuments)
            {
                PullAllJobs();
            }
            else
            {
                PullSingleJob();
            }
        }

        private void PullSingleJob()
        {
            List<int> failedJobs = new List<int>();
            string logText = string.Empty;

            _printJobs = _safecomController.GetFirstJob();
            if (_printJobs == null || _printJobs.Count == 0)
            {
                throw new EmptyQueueException("No jobs available.");
            }

            ExecutionServices.SystemTrace.LogDebug(string.Format("Pulling {0} print jobs.", _printJobs.Count));

            // Go through each job and attempt to pull it
            foreach (int jobId in _printJobs)
            {
                if (!PullSingleJob(jobId))
                {
                    failedJobs.Add(jobId);
                }
                // Give the server a chance to process the pull request.
                Thread.Sleep(5000);
            }

            // If there were any errors during the pull operation, throw an exception
            if (failedJobs.Count > 0)
            {
                throw new PullPrintingSimulationException(string.Format("Error detected while pulling {0} jobs, see log for more details.", failedJobs.Count));
            }
        }

        private bool PullSingleJob(int jobId)
        {
            UpdateStatus(string.Format("Pulling JobId: {0}", jobId));

            try
            {
                _safecomController.PullJob(jobId);
            }
            catch (SafeComSessionException ex)
            {
                string logText = string.Format("Error pulling JobId:{0}.  {1}", jobId, ex.ToString());
                UpdateStatus(logText);
                return false;
            }

            return true;
        }

        private void PullAllJobs()
        {
            UpdateStatus("Pulling All Jobs");

            _printJobs = _safecomController.GetJobCollection().Keys.ToList();
            _safecomController.PullAllJobs();
        }

        /// <summary>
        /// 7/22/2014 - kyoungman : Validation has proven to be problematic mostly because the SafeCom server
        /// doesn't respond very quickly once a print job has been pulled.  Because of that, the validation code
        /// causes a lot of churn on the server which is causes more trouble than the validation is worth.
        /// For now validation has been discontinued for pulling single jobs and pull all.
        /// </summary>
        /// <param name="userName"></param>
        private void ValidatePullOperation()
        {
            foreach (int jobId in _printJobs)
            {
                ValidatePullSingle(jobId);
            }
        }

        private bool ValidatePullSingle(int jobId)
        {
            string logText = string.Empty;
            Dictionary<int, string> jobList = _safecomController.GetJobCollection(true);
            bool result = !jobList.ContainsKey(jobId);

            if (!result)
            {
                UpdateStatus("Re-Validating...");
            }
            int i = 1, limit = 360; //Try for 30 min.
            while (result == false && i <= limit)
            {
                if (limit % 12 == 0)
                {
                    UpdateStatus(string.Format("Retry Attempt #{0}", i));
                }
                jobList = _safecomController.GetJobCollection(true);
                result = !jobList.ContainsKey(jobId);
                i++;
            }

            logText = string.Format("Pull print activity {0}. Id: {1}", result ? "successful" : "failed", jobId);
            UpdateStatus(logText);

            return result;
        }

        private void UpdateStatus(string message)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }
    }
}
