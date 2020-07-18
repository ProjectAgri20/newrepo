using System;
using System.Collections.Generic;
using System.Linq;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.AssetInventory.Reservation
{
    /// <summary>
    /// Handles <see cref="DomainAccountReservation" /> creation and management.
    /// </summary>
    public sealed class DomainAccountReservationManager
    {
        private readonly AssetInventoryConnectionString _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainAccountReservationManager" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="AssetInventoryConnectionString" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public DomainAccountReservationManager(AssetInventoryConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Releases all reservations for the specified sessions.
        /// </summary>
        /// <param name="sessionIds">The session IDs to release.</param>
        public void ReleaseSessionReservations(IEnumerable<string> sessionIds)
        {
            LogInfo($"Releasing domain account reservations for session(s): {string.Join(", ", sessionIds)}");

            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                foreach (DomainAccountReservation reservation in context.DomainAccountReservations.Where(n => sessionIds.Contains(n.SessionId)))
                {
                    context.DomainAccountReservations.Remove(reservation);
                }
                context.SaveChanges();
            }
        }
    }
}
