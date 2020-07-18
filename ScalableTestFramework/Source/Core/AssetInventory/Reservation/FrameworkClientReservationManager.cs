using System;
using System.Collections.Generic;
using System.Linq;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.AssetInventory.Reservation
{
    /// <summary>
    /// Handles reservation of <see cref="FrameworkClient" /> virtual machines.
    /// </summary>
    public sealed class FrameworkClientReservationManager
    {
        private readonly AssetInventoryConnectionString _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkClientReservationManager" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="AssetInventoryConnectionString" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public FrameworkClientReservationManager(AssetInventoryConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Releases all reservations for the specified sessions.
        /// </summary>
        /// <param name="sessionIds">The session IDs to release.</param>
        public void ReleaseSessionReservations(IEnumerable<string> sessionIds)
        {
            LogInfo($"Releasing framework client reservations for session(s): {string.Join(", ", sessionIds)}");

            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                DateTime updated = DateTime.Now;
                foreach (FrameworkClient client in context.FrameworkClients.Where(n => sessionIds.Contains(n.SessionId)))
                {
                    client.SessionId = null;
                    client.Environment = string.Empty;
                    client.PlatformUsage = string.Empty;
                    client.LastUpdated = updated;
                }
                context.SaveChanges();
            }
        }
    }
}
