using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using ScSimulation;

namespace HP.ScalableTest.Plugin.SafeComSimulation
{
    public class SafeComSimulationController : IDisposable
    {
        // The SafeCom assembly *should* allow the following path to be set to anything, but for some reason, if it's not set to c:\safecom_trace, no log files get created.
        private const string LogPath = "C:\\safecom_trace";
        private Simulation _safeComSimulator = null;
        private bool _sessionOpen = false;


        public SafeComSimulationController(NetworkCredential credential, SafeComAuthenticationMode authMode, string deviceIP, string deviceMacAddress, string safecomServer)
        {
            InitializeSimulator(authMode, credential, deviceIP, deviceMacAddress, safecomServer);
        }

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass 
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_sessionOpen && disposing)
            {
                EndSession();
            }
        }

        public void Authenticate()
        {
            if (!StartSession())
            {
                throw new SafeComSessionException("Failed to start a session.");
            }

        }

        public Dictionary<int, string> GetJobCollection(bool refreshSession = false)
        {
            if (StartSession(refreshSession))
            {
                Dictionary<int, string> jobList = _safeComSimulator.SessionGetJobList();
                if (jobList != null)
                {
                    return jobList;
                }
                else
                {
                    return new Dictionary<int, string>();
                }
            }
            else
            {
                throw new SafeComSessionException("Failed to start a session.");
            }
        }

        /// Finds and returns the oldest job in the list.
        /// Note: Excel often creates multiple print jobs for a single document.
        /// This method relies on the fact that STF embeds a GUID into the print job filename
        /// thus allowing us to find all print jobs containing the same unique filename.
        public List<int> GetFirstJob()
        {
            List<int> printJobIds = new List<int>();

            if (StartSession())
            {
                Dictionary<int, string> allJobs = GetJobCollection();
                if (allJobs.Count > 0)
                {
                    // Gets the name of the first print job and finds all other jobs that share that name (addresses issue of Excel documents being split up into multiple print jobs)
                    foreach (KeyValuePair<int, string> pair in allJobs.Where(j => j.Value == allJobs[allJobs.Keys.First()]))
                    {
                        printJobIds.Add(pair.Key);
                    }
                }
            }
            else
            {
                throw new SafeComSessionException("Failed to start a session.");
            }

            return printJobIds;
        }

        public void PullJob(int printJobId)
        {
            if (StartSession())
            {
                int result = _safeComSimulator.SessionPrint(printJobId);
                CheckOperationResult(result, "SessionPrint");
            }
            else
            {
                throw new SafeComSessionException("Failed to start a session.");
            }
        }

        public void DeleteJob(int printJobId)
        {
            if (StartSession())
            {
                int result = _safeComSimulator.SessionDeleteJob(printJobId);
                CheckOperationResult(result, "SessionDeleteJob");
            }
            else
            {
                throw new SafeComSessionException("Failed to start a session.");
            }
        }

        public void PullAllJobs()
        {
            if (StartSession())
            {
                int result = _safeComSimulator.SessionPrintAll();
                CheckOperationResult(result, "SessionPrintAll");
            }
            else
            {
                throw new SafeComSessionException("Failed to start a session.");
            }
        }

        public void DeleteAllJobs()
        {
            if (StartSession(true))
            {
                foreach (int jobId in GetJobCollection().Keys)
                {
                    DeleteJob(jobId);
                }
            }
            else
            {
                throw new SafeComSessionException("Failed to start a session.");
            }
        }

        private void InitializeSimulator(SafeComAuthenticationMode authMode, NetworkCredential userCredential, string deviceIP, string deviceMacAddress, string safecomServer)
        {
            _safeComSimulator = null;
            VerifyLogFolder();
            _safeComSimulator = new Simulation(LogPath + "\\" + userCredential.UserName + "_.log");

            bool result = _safeComSimulator.SetupSession(GetAuthenticationMethod(authMode), userCredential.UserName, userCredential.Password, userCredential.Domain, deviceMacAddress, safecomServer, 7500, deviceIP, false, true);

            if (!result)
            {
                throw new SafeComSessionException("Failed to initialize the SafeCom simulation engine.");
            }
        }

        private static void VerifyLogFolder()
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
        }

        private bool StartSession(bool refresh = false)
        {
            if (refresh)
            {
                EndSession();
                Thread.Sleep(5000);
            }

            if (!_sessionOpen)
            {
                int result = _safeComSimulator.SessionStart();
                CheckOperationResult(result, "SessionStart");
                _sessionOpen = (result == 0);
            }
            return _sessionOpen;
        }

        private void EndSession()
        {
            if (_sessionOpen)
            {
                try
                {
                    // Note:  Calling _safeComSimulator.SessionStop() multiple times will cause a System.AccessViolationException.
                    int result = _safeComSimulator.SessionStop();
                    CheckOperationResult(result, "SessionStop");
                    _sessionOpen = (result != 0);
                }
                catch (AccessViolationException ex)
                {
                    throw new SafeComSessionException("Unable to execute SessionStop without an open session.", ex);
                }
                catch (Exception ex)
                {
                    throw new SafeComSessionException("Error stopping session.", ex);
                }
            }
        }

        private void CheckOperationResult(int result, string methodName)
        {
            if (result != 0)
            {
                string logText = string.Format("{0} Result: {1}", methodName, result);
                Framework.ExecutionServices.SystemTrace.LogDebug(logText);
            }
        }

        private int GetAuthenticationMethod(SafeComAuthenticationMode pluginAuthMode)
        {
            switch (pluginAuthMode)
            {
                case SafeComAuthenticationMode.NameAndPin:
                    return Simulation.SC_LOGIN_NAME_AND_PIN;
                case SafeComAuthenticationMode.CardAndPin:
                    return Simulation.SC_LOGIN_CARD_AND_PIN;
                case SafeComAuthenticationMode.WindowsCredentials:
                    return Simulation.SC_LOGIN_WINDOWS;
                default:
                    return Simulation.SC_LOGIN_NAME_AND_PWD;
            }
        }


    }
}
