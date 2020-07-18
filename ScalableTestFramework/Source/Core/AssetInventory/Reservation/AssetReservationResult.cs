using System;

namespace HP.ScalableTest.Core.AssetInventory.Reservation
{
    /// <summary>
    /// The end result of a call to <see cref="AssetReservationManager.ReserveAssets" />.
    /// </summary>
    public sealed class AssetReservationResult
    {
        /// <summary>
        /// Gets the ID of the reserved asset.
        /// </summary>
        public string AssetId { get; }

        /// <summary>
        /// Gets the <see cref="AssetAvailability" />.
        /// </summary>
        public AssetAvailability Availability { get; }

        /// <summary>
        /// Gets the beginning of the time period when the asset is available.
        /// If availability information is not available, returns null.
        /// </summary>
        public DateTime? AvailabilityStart { get; }

        /// <summary>
        /// Gets the end of the time period when the asset is available.
        /// If availability information is not available, returns null.
        /// </summary>
        public DateTime? AvailabilityEnd { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetReservationResult" /> class.
        /// </summary>
        /// <param name="assetId">The ID of the reserved asset.</param>
        /// <param name="availability">The <see cref="AssetAvailability" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetId" /> is null.</exception>
        public AssetReservationResult(string assetId, AssetAvailability availability)
        {
            AssetId = assetId ?? throw new ArgumentNullException(nameof(assetId));
            Availability = availability;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetReservationResult" /> class.
        /// </summary>
        /// <param name="assetId">The ID of the reserved asset.</param>
        /// <param name="availability">The <see cref="AssetAvailability" />.</param>
        /// <param name="availabilityStart">The beginning of the time period when the asset is available.</param>
        /// <param name="availabilityEnd">The end of the time period when the asset is available.</param>
        /// <exception cref="ArgumentException"><paramref name="availabilityEnd" /> is before <paramref name="availabilityStart" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="assetId" /> is null.</exception>
        public AssetReservationResult(string assetId, AssetAvailability availability, DateTime availabilityStart, DateTime availabilityEnd)
            : this(assetId, availability)
        {
            if (availabilityEnd <= availabilityStart)
            {
                throw new ArgumentException("Availability end must be after availability start.", nameof(availabilityEnd));
            }

            AvailabilityStart = availabilityStart;
            AvailabilityEnd = availabilityEnd;
        }
    }
}
