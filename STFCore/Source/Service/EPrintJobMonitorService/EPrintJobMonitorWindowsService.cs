using System;
using System.Collections.Generic;
using System.IO;
using HP.ScalableTest.Framework.Service;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Service.EPrintJobMonitor
{
    internal class EPrintJobMonitorWindowsService : FrameworkServiceBase
    {
        private EPrintJobMonitorEndpoint _serviceEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="EPrintJobMonitorWindowsService"/> class.
        /// </summary>
        public EPrintJobMonitorWindowsService()
            : base("EPrintJobMonitorWindowsService", "STF ePrint Job Monitor")
        {
            // Create the Endpoint
            _serviceEndpoint = new EPrintJobMonitorEndpoint();
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The args.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override void StartService(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            if (args.Length == 0)
            {
                args = LoadCachedStartupArgs();
            }
            TraceFactory.Logger.Debug(string.Join(",", args));

            TraceFactory.Logger.Debug(args);

            LoadSettings(args);

            if (_serverSettings.ContainsKey("ConnectionPort"))
            {
                int connectionPort = int.Parse(_serverSettings["ConnectionPort"]);
                TraceFactory.Logger.Debug($"Using database port {connectionPort}");
                // Open the endpoint.
                _serviceEndpoint.Open(_serverSettings["DatabaseHostName"], _serverSettings["DatabaseInstanceName"], connectionPort);
            }
            else
            {
                TraceFactory.Logger.Debug("No database port specified.  Using default.");
                // Open the endpoint.
                _serviceEndpoint.Open(_serverSettings["DatabaseHostName"], _serverSettings["DatabaseInstanceName"]);
            }

        }

        /// <summary>
        /// Stops this service instance.
        /// </summary>
        protected override void StopService()
        {
            if (_serviceEndpoint != null)
            {
                _serviceEndpoint.Close();
                _serviceEndpoint = null;
            }
        }
    }
}
