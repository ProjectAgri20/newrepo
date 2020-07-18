using System.ServiceModel;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Service.EPrintJobMonitor
{
    internal class EPrintJobMonitorEndpoint
    {
        private ServiceHost _service = null;
        private EPrintJobMonitorService _ePrintJobMonitor = null;

        /// <summary>
        /// Opens the endpoint for a local database.
        /// </summary>
        public void Open()
        {
            TraceFactory.Logger.Debug("Local database installation");
            _ePrintJobMonitor = new EPrintJobMonitorService();
            OpenEndpoint();
        }

        /// <summary>
        /// Opens the endpoint for a remote database.
        /// </summary>
        /// <param name="dbHostName">Name of the database host.</param>
        /// <param name="instanceName">Name of the database instance.</param>
        /// <param name="dbPort">The connection port.</param>
        public void Open(string dbHostName, string dbInstanceName, int dbPort = -1)
        {
            TraceFactory.Logger.Debug("Remote database installation");
            _ePrintJobMonitor = new EPrintJobMonitorService(dbHostName, dbInstanceName, dbPort);
            OpenEndpoint();
        }

        /// <summary>
        /// Closes the endpoint.
        /// </summary>
        public void Close()
        {
            if (_service.State != CommunicationState.Closed)
            {
                _service.Close();
                TraceFactory.Logger.Debug("EPrintJobMonitor endpoint - Closed");
            }

            if (_ePrintJobMonitor != null)
            {
                _ePrintJobMonitor.Dispose();
                _ePrintJobMonitor = null;
            }
        }

        private void OpenEndpoint()
        {
            TraceFactory.Logger.Debug("Opening EPrintJobMonitor endpoint");

            // Load and start the data log service
            _service = new WcfHost<IEPrintJobMonitorService>(
                    _ePrintJobMonitor,
                    MessageTransferType.Http,
                    WcfService.EPrintJobMonitor.GetLocalHttpUri());

            _service.Open();

            TraceFactory.Logger.Debug("EPrintJobMonitor endpoint - Opened");
        }
    }
}
