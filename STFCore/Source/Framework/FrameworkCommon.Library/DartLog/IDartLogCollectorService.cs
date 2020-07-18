using System.Collections.ObjectModel;
using System.ServiceModel;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Service Contract for Dart Log Collector Service
    /// </summary>
    [ServiceContract]
    public interface IDartLogCollectorService
    {

        /// <summary>
        /// Collects the Dart Log and places it in the System Setting Location
        /// </summary>
        /// <returns>Returns Successful Completion</returns>
        [OperationContract]
        void CollectLog(string device, string sessionID, string email);

        [OperationContract]
        bool Start(string IP);

        [OperationContract]
        bool Stop(string IP);

        [OperationContract]
        bool Flush(string IP);

    }
}
