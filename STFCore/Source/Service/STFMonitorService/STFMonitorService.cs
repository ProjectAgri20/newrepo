using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Service.Monitor.Output;
using HP.ScalableTest.Service.Monitor.Directory;
using HP.ScalableTest.Service.Monitor.DigitalSend;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Service.Monitor.AutoStore;
using HP.ScalableTest.Service.Monitor.Hpcr;
using HP.ScalableTest.Service.Monitor.Eprint;

namespace HP.ScalableTest.Service.Monitor
{
    /// <summary>
    /// STF service that hosts all monitor objects.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class STFMonitorService : ISTFMonitorService
    {
        private List<StfMonitor> _monitors = new List<StfMonitor>();
        private ConcurrentDictionary<string, RegisteredSessionInfo> _registeredSessions = new ConcurrentDictionary<string, RegisteredSessionInfo>();
        private Timer _expirationTimer = null;
        private TimeSpan _timerIntervalHours = TimeSpan.FromHours(24.0);
        private int _expirationIntervalHours = 240;

        /// <summary>
        /// Initializes a new instance of the <see cref="STFMonitorService"/> class.
        /// Clears out unused sessions after 10 days.
        /// </summary>
        public STFMonitorService()
        {
            _expirationTimer = new Timer(new TimerCallback(ExpirationTimer_Elapsed), null, _timerIntervalHours, _timerIntervalHours);
            bool created = false;

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (MonitorConfig config in context.MonitorConfigs.Where(n => n.ServerHostName == Environment.MachineName))
                {
                    CreateMonitor(config);
                    created = true;
                }
            }

            if (created == false)
            {
                string message = $"No monitor configuration was found for {Environment.MachineName}";
                TraceFactory.Logger.Error(message);
                throw new ApplicationException(message);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="STFMonitorService"/> class.
        /// </summary>
        /// <param name="expirationIntervalHours">Number of hours the service should wait before clearing out unused sessions.</param>
        public STFMonitorService(int expirationIntervalHours)
            : this()
        {
            if (expirationIntervalHours > 0)
            {
                _expirationIntervalHours = expirationIntervalHours;
            }

            TraceFactory.Logger.Debug($"Expiration Interval set to: {_expirationIntervalHours} hours.");
        }

        private void CreateMonitor(MonitorConfig monitorConfig)
        {
            STFMonitorType monitorType = EnumUtil.Parse<STFMonitorType>(monitorConfig.MonitorType);

            StfMonitor monitor = null;
            switch (monitorType)
            {
                case STFMonitorType.OutputEmail:
                    monitor = new OutputEmailMonitor(monitorConfig);
                    break;

                case STFMonitorType.OutputDirectory:
                    monitor = new OutputDirectoryMonitor(monitorConfig);
                    break;

                case STFMonitorType.SharePoint:
                    monitor = new SharePointMonitor(monitorConfig);
                    break;

                case STFMonitorType.LANFax:
                    monitor = new LANFaxMonitor(monitorConfig);
                    break;

                case STFMonitorType.DigitalSendNotification:
                    monitor = new DigitalSendNotificationMonitor(monitorConfig);
                    break;

                case STFMonitorType.Directory:
                    monitor = new DirectoryMonitor(monitorConfig);
                    break;
                case STFMonitorType.AutoStore:
                    monitor = new AutoStoreMonitor(monitorConfig);
                    break;

                case STFMonitorType.DSSServer:
                    monitor = new DigitalSendDatabaseMonitor(monitorConfig);
                    break;

                case STFMonitorType.Hpcr:
                    monitor = new HpcrDatabaseMonitor(monitorConfig);
                    break;

                case STFMonitorType.EPrint:
                    monitor = new EPrintJobMonitorService(monitorConfig);
                    break;
                default:
                    TraceFactory.Logger.Debug($"Unknown monitor type: {monitorConfig.MonitorType}");
                    return;
            }

            monitor.RegisteredSessions = _registeredSessions;
            _monitors.Add(monitor);
            TraceFactory.Logger.Debug($"Added monitor for '{monitor.MonitorLocation}'.");
            
        }

        /// <summary>
        /// Starts this service, which in turn starts all monitors configured for the service host.
        /// </summary>
        public void Start()
        {
            foreach (StfMonitor monitor in _monitors)
            {
                ThreadPool.QueueUserWorkItem(StartMonitor, monitor);
            }
        }

        private static void StartMonitor(object state)
        {
            StfMonitor monitor = (StfMonitor)state;
            try
            {
                monitor.StartMonitoring();
            }
            catch (Exception ex)
            {
                // If something goes wrong, log the error info, but allow other monitors continue to start.
                TraceFactory.Logger.Debug($"{monitor.ToString()} failed to start.");
                TraceFactory.Logger.Debug(ex.ToString());
            }
        }

        /// <summary>
        /// Stops this Service which stops all monitors configured for the service host.
        /// </summary>
        public void Stop()
        {
            foreach (StfMonitor monitor in _monitors)
            {
                if (monitor != null)
                {
                    monitor.StopMonitoring();
                    monitor.Dispose();
                }
            }
        }

        /// <summary>
        /// Registers the given Session Id and log service host name with the STF monitor service.
        /// </summary>
        /// <param name="sessionId">The session Id</param>
        /// <param name="logServiceHostName">The host name of the server where the data log service is running</param>
        public void Register(string sessionId, string logServiceHostName)
        {
            bool result = Retry.UntilTrue(() => _registeredSessions.TryAdd(sessionId, new RegisteredSessionInfo(logServiceHostName)), 5, TimeSpan.FromSeconds(1));

            if (result)
            {
                TraceFactory.Logger.Debug($"Registered {sessionId}:{logServiceHostName}.");
            }
            else
            {
                TraceFactory.Logger.Debug($"Failed to register {sessionId}:{logServiceHostName}.");
            }
        }

        /// <summary>
        /// Returns whether the specified directory path exists on the service host.
        /// </summary>
        /// <param name="directoryPath">The directory path to validate</param>
        /// <returns>true if the directory path exists, false otherwise.</returns>
        public bool IsValidDirectoryPath(string directoryPath)
        {
            return System.IO.Directory.Exists(directoryPath);
        }

        /// <summary>
        /// Reloads all the MonitorConfig items for each monitor running on the current service host.  
        /// </summary>
        public void RefreshConfig()
        {
            TraceFactory.Logger.Debug("Request received to refresh config.");
            List<MonitorConfig> currentConfiguration = null;
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                currentConfiguration = context.MonitorConfigs.Where(n => n.ServerHostName == Environment.MachineName).ToList();
            }

            lock (_monitors)
            {
                RemoveStaleMonitors(new HashSet<Guid>(currentConfiguration.Select(c => c.MonitorConfigId)));
            }

            foreach (MonitorConfig config in currentConfiguration)
            {
                StfMonitor monitor = _monitors.Where(m => m.MonitorId == config.MonitorConfigId).FirstOrDefault();
                if (monitor != null)
                {
                    // Found existing monitor
                    monitor.RefreshConfig(config);
                }
                else
                {
                    //Monitor exists in the database config, but is not running on the current service host.
                    CreateMonitor(config);
                }
            }

            TraceFactory.Logger.Debug("Config refreshed.");
        }

        private void ExpirationTimer_Elapsed(object state)
        {
            //Turn off timer
            _expirationTimer.Change(-1, -1);

            foreach (string sessionId in _registeredSessions.Keys)
            {
                if (IsSessionInactive(_registeredSessions[sessionId]))
                {
                    UnRegister(sessionId);
                }
            }

            //Turn timer back on
            _expirationTimer.Change(_timerIntervalHours, _timerIntervalHours);
        }

        /// <summary>
        /// Removes the specified sessionId from the list of registered sessions.
        /// </summary>
        /// <param name="sessionId"></param>
        private void UnRegister(string sessionId)
        {
            RegisteredSessionInfo value;
            bool result = Retry.UntilTrue(() => _registeredSessions.TryRemove(sessionId, out value), 5, TimeSpan.FromSeconds(1));

            if (result == false)
            {
                TraceFactory.Logger.Debug($"Failed to un-register {sessionId}.");
            }
        }

        private void RemoveStaleMonitors(HashSet<Guid> activeIDs)
        {
            List<StfMonitor> staleMonitors = _monitors.Where(m => !activeIDs.Contains(m.MonitorId)).ToList();

            TraceFactory.Logger.Debug($"Stale monitor count={staleMonitors.Count}");
            foreach (StfMonitor monitor in staleMonitors)
            {
                _monitors.Remove(monitor);
                TraceFactory.Logger.Debug($"Removed monitor Id='{monitor.MonitorId}' monitoring at {monitor.MonitorLocation}");
            }
        }

        private bool IsSessionInactive(RegisteredSessionInfo sessionInfo)
        {
            // If it has been X hours or more since this sessionInfo was referenced, consider the session inactive.
            return ((DateTime.Now - sessionInfo.LastUsed).TotalHours >= _expirationIntervalHours);
        }

    }
}
