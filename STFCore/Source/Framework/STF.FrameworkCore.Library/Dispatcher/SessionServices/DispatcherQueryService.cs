using HP.ScalableTest.Framework.Wcf;


namespace HP.ScalableTest.Framework.Dispatcher.SessionServices
{
    /// <summary>
    /// 
    /// </summary>
    public class DispatcherQueryService : IAssetInventoryDispatcherQueryService
    {

        /// <summary>
        /// To find if there are any active sessions
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        public bool AreThereActiveSessions(string dispatcher)
        {
            using (var client = new WcfClient<ISessionDispatcher>(MessageTransferType.CompressedHttp, WcfService.SessionServer.GetHttpUri(dispatcher)))
            {
                return client.Channel.AreThereActiveSessions();
            }
        }


    }
}
