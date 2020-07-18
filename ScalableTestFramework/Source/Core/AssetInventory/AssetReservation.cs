using System;
using System.Diagnostics;
using HP.ScalableTest.Core.AssetInventory.Reservation;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A reservation for an <see cref="Asset" />.
    /// </summary>
    [DebuggerDisplay("{AssetId,nq} [{ReservedFor,nq}]")]
    public sealed class AssetReservation
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        public Guid AssetReservationId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the reserved asset.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the owner of the reservation.
        /// </summary>
        public string ReservedBy { get; set; }

        /// <summary>
        /// Gets or sets a description of the task that the device will be used for.
        /// </summary>
        public string ReservedFor { get; set; }

        /// <summary>
        /// Gets or sets the start of the reservation period.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end of the reservation period.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Gets or sets the time this reservation was created.
        /// </summary>
        public DateTime Received { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the STF session that is using this reservation.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user that created this reservation.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how expiration notifications should be handled.
        /// </summary>
        public AssetReservationExpirationNotify Notify { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets the user that should be notified when this reservation is about to expire.
        /// If no user should be notified, returns null.
        /// </summary>
        public string NotificationRecipient
        {
            get
            {
                switch (Notify)
                {
                    case AssetReservationExpirationNotify.ReservedFor:
                        return ReservedFor;

                    case AssetReservationExpirationNotify.ReservedBy:
                        return ReservedBy;

                    default:
                        return null;
                }
            }
        }
    }
}
