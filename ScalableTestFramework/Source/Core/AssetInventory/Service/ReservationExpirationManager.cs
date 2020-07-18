using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.AssetInventory.Service
{
    /// <summary>
    /// Manages asset reservations that are expiring or have already expired.
    /// </summary>
    public sealed class ReservationExpirationManager : IAssetInventoryMaintenanceManager
    {
        private readonly AssetInventoryConnectionString _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationExpirationManager" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="AssetInventoryConnectionString" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public ReservationExpirationManager(AssetInventoryConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Sends notifications for all expiring and expired asset reservations.
        /// </summary>
        /// <param name="expirationNotifier">The <see cref="ExpirationNotifier" />.</param>
        public void SendExpirationNotifications(ExpirationNotifier expirationNotifier)
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                List<AssetReservation> reservations = context.AssetReservations.Where(n => n.End < DbFunctions.AddHours(DateTime.Now, 24)).ToList();
                LogDebug($"Found {reservations.Count} expiring or expired reservations.");

                foreach (AssetReservation reservation in reservations.Where(n => n.NotificationRecipient != null))
                {
                    SendExpirationNotification(expirationNotifier, reservation);
                }

                LogDebug("Finished processing asset reservation notifications.");
            }
        }

        private static void SendExpirationNotification(ExpirationNotifier notifier, AssetReservation reservation)
        {
            try
            {
                MailAddress recipient = new MailAddress(reservation.NotificationRecipient);
                string reservationText = (reservation.End < DateTime.Now ? Resource.ExpiredReservationText : Resource.ExpiringReservationText);
                string subject = string.Format(reservationText, reservation.AssetId);
                string body = string.Format(Resource.ReservationEmailBody, subject, reservation.End.ToString("f"));
                notifier.SendNotification(recipient, subject, body);
            }
            catch (FormatException)
            {
                LogWarn($"Ignoring invalid notification recipient: {reservation.NotificationRecipient}");
            }
        }

        /// <summary>
        /// Cleans up all expired reservations.
        /// </summary>
        public void CleanupExpiredReservations()
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                List<AssetReservation> reservations = context.AssetReservations.Where(n => n.End < DateTime.Now).ToList();
                LogDebug($"Cleaning up {reservations.Count} expired reservations.");

                context.AssetReservations.RemoveRange(reservations);
                context.SaveChanges();
            }
        }

        #region Explicit IAssetInventoryMaintenanceManager Members

        void IAssetInventoryMaintenanceManager.RunMaintenance()
        {
            CleanupExpiredReservations();
        }

        #endregion
    }
}
