using System;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A record storing history of an <see cref="AssetReservation" /> object.
    /// </summary>
    public sealed class ReservationHistory
    {
        /// <summary>
        /// Gets or sets the unique identifier for the history record.
        /// </summary>
        internal int ReservationHistoryId { get; set; }

        /// <summary>
        /// Corresponds to <see cref="AssetReservation.AssetId" />.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Corresponds to <see cref="AssetReservation.ReservedFor" />.
        /// </summary>
        public string ReservedFor { get; set; }

        /// <summary>
        /// Corresponds to <see cref="AssetReservation.Start" />.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Corresponds to <see cref="AssetReservation.End" />.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationHistory" /> class.
        /// </summary>
        public ReservationHistory()
        {
            // Parameterless constructor required for Entity Framework
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationHistory" /> class.
        /// </summary>
        /// <param name="reservation">The <see cref="AssetReservation" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reservation" /> is null.</exception>
        public ReservationHistory(AssetReservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            AssetId = reservation.AssetId;
            ReservedFor = reservation.ReservedFor.Contains("@") ? reservation.ReservedFor : reservation.ReservedBy;
            Start = reservation.Start;
            End = reservation.End;
        }
    }
}
