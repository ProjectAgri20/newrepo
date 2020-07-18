using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework.Synchronization;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Manages domain user account reservations.
    /// </summary>
    public static class DomainAccountService
    {
        /// <summary>
        /// Reserves a block of users.  This MUST be called before being able to get user credentials.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="accountQuantity">The account quantity which is separated by account pool.</param>
        /// <returns>A <see cref="DomainAccountReservationSet"/> containing information on the reserved accounts.</returns>
        /// <exception cref="System.ArgumentNullException">accountQuantity</exception>
        /// <exception cref="System.InvalidOperationException">Domain accounts have already been reserved for this Session Id</exception>
        public static DomainAccountReservationSet Reserve(string sessionId, DomainAccountQuantityDictionary accountQuantity)
        {
            if (accountQuantity == null)
            {
                throw new ArgumentNullException("accountQuantity");
            }

            DomainAccountReservationSet reservedBlock = new DomainAccountReservationSet(sessionId);

            Action action = new Action(() =>
            {
                try
                {
                    using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                    {
                        if (context.DomainAccountReservations.Any(e => e.SessionId.Equals(sessionId, StringComparison.OrdinalIgnoreCase)))
                        {
                            //This could be a subsequent call to reserve.  Clear all reservations before proceeding.
                            Release(sessionId);
                        }

                        foreach (var poolType in accountQuantity.Keys)
                        {
                            int userCount = accountQuantity[poolType];
                            DomainAccountPool pool = SelectPool(context, poolType);
                            int startIndex = ReserveBlock(context, sessionId, pool, userCount);

                            TraceFactory.Logger.Debug($"New pool reserved: StartIndex: {startIndex}, UserCount: {userCount}, PoolName: {pool.DomainAccountKey}");

                            reservedBlock.Add(poolType, pool, startIndex, userCount);
                        }
                    }
                }
                catch (InsufficientDomainAccountsException)
                {
                    ReleaseSessionReservations(sessionId);
                    throw;
                }
            });

            var token = new GlobalLockToken("DomainAccountReservation", TimeSpan.FromMinutes(11), TimeSpan.FromMinutes(2));
            ExecutionServices.CriticalSection.Run(token, action);

            return reservedBlock;
        }

        /// <summary>
        /// Releases the reservations for the specified session id.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public static void Release(string sessionId)
        {
            ReleaseSessionReservations(sessionId);
        }

        /// <summary>
        /// Selects the DomainAccountPool with the specified key.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <param name="poolType">The key.</param>
        /// <returns></returns>
        public static DomainAccountPool SelectPool(AssetInventoryContext context, string poolType)
        {
            return (from n in context.DomainAccountPools
                    where n.DomainAccountKey == poolType
                    select n).FirstOrDefault();
        }

        /// <summary>
        /// Reserves a block of User Ids for the given session.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="accountPool">The account pool.</param>
        /// <param name="requestedSize">The size of reservation block being requested.</param>
        /// <returns>The start index for the reserved block</returns>
        private static int ReserveBlock(AssetInventoryContext context, string sessionId, DomainAccountPool accountPool, int requestedSize)
        {
            if (accountPool == null)
            {
                throw new ArgumentNullException("accountPool");
            }

            // Calculate the start index of the new block
            int startIndex = accountPool.MinimumUserNumber;
            foreach (DomainAccountReservation reservation in SelectReservationsByKey(context, accountPool.DomainAccountKey))
            {
                // Check to see if there are any users between reservations that we can utilize.
                // For example, say there were 2 reservations against this DomainAccountKey, but the first one got released
                // Now there is a gap between the minimum user number and the start index of the reservation we are looking at.
                // We want to utilize that block, if possible.
                if ((reservation.StartIndex - startIndex) >= requestedSize)
                {
                    break;
                }
                // Not enough space for the requested size, so check the next one.
                startIndex = reservation.StartIndex + reservation.Count;
            }

            // Now that we have the start index for our new block, we need to see if there is enough room
            // for the block before we hit the "ceiling" of our domain users.
            int available = accountPool.MaximumUserNumber - startIndex + 1;
            if (available < requestedSize)
            {
                throw new InsufficientDomainAccountsException($"There were not enough domain accounts available to run this scenario. PoolName: {accountPool.DomainAccountKey} Available: {available} Number Requested: {requestedSize}");
            }

            DomainAccountReservation newReservation = new DomainAccountReservation
            {
                SessionId = sessionId,
                DomainAccountKey = accountPool.DomainAccountKey,
                StartIndex = startIndex,
                Count = requestedSize
            };

            context.DomainAccountReservations.Add(newReservation);
            context.SaveChanges();

            return startIndex;
        }

        /// <summary>
        /// Releases all reserved user Ids for the given session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        private static void ReleaseSessionReservations(string sessionId)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (DomainAccountReservation reservation in
                         context.DomainAccountReservations.Where(n => n.SessionId == sessionId))
                {
                    context.DomainAccountReservations.Remove(reservation);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Selects all <see cref="DomainAccountReservation"/> for the specified session Id in ascending order.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <param name="sessionId">The SessionId.</param>
        /// <returns></returns>
        public static IQueryable<DomainAccountReservation> SelectReservationsBySession(AssetInventoryContext context, string sessionId)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return from r in context.DomainAccountReservations
                   where r.SessionId == sessionId
                   orderby r.StartIndex
                   select r;
        }

        /// <summary>
        /// Selects all <see cref="DomainAccountReservation"/> with the specified domain account key in ascending order.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <param name="key">The domain account key.</param>
        /// <returns></returns>
        private static IQueryable<DomainAccountReservation> SelectReservationsByKey(AssetInventoryContext context, string key)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return from r in context.DomainAccountReservations
                   where r.DomainAccountKey == key
                   orderby r.StartIndex
                   select r;
        }
    }

    /// <summary>
    /// Exception thrown when more account reservations were requested than were available
    /// </summary>
    [Serializable]
    public class InsufficientDomainAccountsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientDomainAccountsException"/> class.
        /// </summary>
        public InsufficientDomainAccountsException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientDomainAccountsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InsufficientDomainAccountsException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientDomainAccountsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public InsufficientDomainAccountsException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientDomainAccountsException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        ///   
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected InsufficientDomainAccountsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}