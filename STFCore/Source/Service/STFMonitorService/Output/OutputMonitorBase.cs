using System;
using System.Collections.Concurrent;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.Monitor;

namespace HP.ScalableTest.Service.Monitor.Output
{
    /// <summary>
    /// Base class for STF Output monitors.
    /// </summary>
    public abstract class OutputMonitorBase : StfMonitor
    {
        /// <summary>
        /// The configuration needed to start an output monitor.
        /// </summary>
        protected OutputMonitorConfig Configuration { get; private set; }

        /// <summary>
        /// Constructs a new instance of an OutputMonitorBase.
        /// </summary>
        /// <param name="monitorConfig"></param>
        public OutputMonitorBase(MonitorConfig monitorConfig)
            :base(monitorConfig)
        {
            RefreshConfig(monitorConfig);
        }

        /// <summary>
        /// Refreshes monitor configuration for this instance.
        /// </summary>
        public override void RefreshConfig(MonitorConfig monitorConfig)
        {
            Configuration = LegacySerializer.DeserializeXml<OutputMonitorConfig>(monitorConfig.Configuration);
        }

    }
}
