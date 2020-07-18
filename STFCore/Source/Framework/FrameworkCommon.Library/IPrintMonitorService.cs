using System;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Service contract for the Print Monitor Service.
    /// </summary>
    [ServiceContract]
    public interface IPrintMonitorService
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        [OperationContract]
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        [OperationContract]
        void Stop();

        /// <summary>
        /// Requests to send job.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="maxJobCount">The max job count.</param>
        /// <returns></returns>
        [OperationContract]
        bool RequestToSendJob(string queueName, int maxJobCount);

        /// <summary>
        /// Waits for print job to be finished and ready to be pulled..
        /// </summary>
        /// <param name="printJobId">The print job identifier.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [OperationContract]
        bool WaitForPrintJobFinished(Guid printJobId, TimeSpan timeout);

        /// <summary>
        /// Determines whether job is rendered on client for each queue name.
        /// </summary>
        /// <param name="queues">The list of queues.</param>
        /// <returns>
        ///   <c>true</c> if the job is rendered on the client for each queue name; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        SerializableDictionary<string, string> GetJobRenderLocation(Collection<string> queues);

        /// <summary>
        /// Retrieves the log files for the PrintMonitorService.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        LogFileDataCollection GetLogFiles();
    }
}
