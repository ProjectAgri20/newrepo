using System.ServiceModel;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Interface for the client-side callback of a distributed <see cref="ILockManager" /> communication channel.
    /// </summary>
    /// <remarks>
    /// When an <see cref="ILockService" /> implementation is ready to grant a lock to a client,
    /// it calls back using this interface.  This interface also includes a method to check for
    /// dead/expired clients so that it can stop tracking them.
    /// 
    /// In the client/server model, this interface should be implemented on the client side.
    /// </remarks>
    [ServiceContract]
    public interface ILockCallback
    {
        /// <summary>
        /// Signals the client that the lock request has been granted.
        /// </summary>
        /// <param name="resourceId">The identifier for the locked resource.</param>
        [OperationContract]
        void GrantLock(string resourceId);

        /// <summary>
        /// Pings this instance.
        /// </summary>
        /// <returns><c>true</c> if the ping was successful.</returns>
        [OperationContract]
        bool Ping();
    }
}
