namespace HP.ScalableTest.Core.AssetInventory.Reservation
{
    /// <summary>
    /// The availability of an asset over a specified time period, as determined by a reservation attempt.
    /// </summary>
    public enum AssetAvailability
    {
        /// <summary>
        /// Asset availability could not be determined.
        /// </summary>
        Unknown,

        /// <summary>
        /// Asset is available for requested time period.
        /// </summary>
        Available,

        /// <summary>
        /// Asset is available for a portion of the requested time period.
        /// </summary>
        PartiallyAvailable,

        /// <summary>
        /// Asset is unavailable for the requested time period.
        /// </summary>
        NotAvailable
    }
}
