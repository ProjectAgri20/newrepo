using System;
using System.Collections.Concurrent;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Service.Monitor
{
    /// <summary>
    /// Base class for all STF monitors.
    /// </summary>
    public abstract class StfMonitor : IDisposable
    {
        /// <summary>
        /// Constructs a new instance of an StfMonitorBase.
        /// </summary>
        /// <param name="monitorConfig"></param>
        public StfMonitor(MonitorConfig monitorConfig)
        {
            if (monitorConfig == null)
            {
                throw new ArgumentNullException("monitorConfig");
            }

            MonitorId = monitorConfig.MonitorConfigId;
        }

        /// <summary>
        /// Gets the Id for the Output monitor.
        /// </summary>
        public Guid MonitorId { get; private set; }

        /// <summary>
        /// Gets the location to monitor.
        /// </summary>
        public abstract string MonitorLocation { get; }

        /// <summary>
        /// Gets or sets the collection of registered sessions.
        /// </summary>
        public ConcurrentDictionary<string, RegisteredSessionInfo> RegisteredSessions { get; set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public abstract void StartMonitoring();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public abstract void StopMonitoring();

        /// <summary>
        /// Refreshes monitor configuration for this instance.
        /// </summary>
        public abstract void RefreshConfig(MonitorConfig monitorConfig);

        /// <summary>
        /// Cleans up any resources that are being used by the monitors.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Gets the DataLog Server Hostname for the given sessionId.
        /// If none is found, returns empty string.
        /// </summary>
        /// <param name="sessionId">The Session Id.</param>
        /// <returns>The DataLog Server Hostname.</returns>
        protected string GetLogServiceHost(string sessionId)
        {
            try
            {
                RegisteredSessionInfo sessionInfo = RegisteredSessions[sessionId];
                sessionInfo.LastUsed = DateTime.Now;
                return sessionInfo.LogServiceHostName;
            }
            catch (Exception)
            {
                return GlobalSettings.WcfHosts[WcfService.DataLog];
            }
        }
    }
}
