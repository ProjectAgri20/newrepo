using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    /// <summary>
    /// Implementation of the PhysicalDeviceJobLogMonitorService
    /// Used to provide to query the device for job accounting data
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class PhysicalDeviceJobLogMonitorService : IDisposable, IPhysicalDeviceJobLogMonitorService
    {
        /// <summary>
        /// Logs that a ping was received by the service
        /// </summary>
        /// <returns></returns>
        public string Ping()
        {           
            TraceFactory.Logger.Debug("Ping received");
            return "Ping acknowledged";
        }

        /// <summary>
        /// Spins a task to Monitor a device for speciific document ids
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="activityId"></param>
        /// <param name="transactionId"></param>
        /// <param name="deviceId"></param>
        /// <param name="deviceIpAddress"></param>
        /// <param name="documentIdentifiers"></param>
        /// <param name="role"></param>
        /// <param name="password"></param>
        /// <param name="minutesToMonitorBeforeTimeout"></param>
        public void MonitorDeviceForDocumentIds(
            string sessionId, Guid activityId, Guid transactionId, string deviceId, string deviceIpAddress
            ,List<string> documentIdentifiers, string role, string password, int minutesToMonitorBeforeTimeout = 30)
        {
            TraceFactory.Logger.Info("Starting monitor...");
            Task.Factory.StartNew(() =>
                {
                    var deviceJobLogger = new HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor.DeviceJobLogMonitor(
                        sessionId,
                        activityId,
                        transactionId,
                        deviceId,
                        deviceIpAddress
                        );

                    deviceJobLogger.Monitor(documentIdentifiers, role, password, minutesToMonitorBeforeTimeout);
                });
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TraceFactory.Logger.Debug("PhysicalDeviceJobLogMonitorService instance has been disposed");
            }
        }
    }
}
