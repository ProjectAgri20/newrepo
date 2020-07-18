using System.ServiceModel;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface IAssetInventoryDispatcherQueryService

    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        [OperationContract]
        bool AreThereActiveSessions(string dispatcher);
    }
}
