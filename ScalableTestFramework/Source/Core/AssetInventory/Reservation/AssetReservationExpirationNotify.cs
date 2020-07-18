namespace HP.ScalableTest.Core.AssetInventory.Reservation
{
    /// <summary>
    /// Defines the notification setting for an expiring <see cref="AssetReservation" />.
    /// </summary>
    public enum AssetReservationExpirationNotify
    {
        /// <summary>
        /// No notification of the expiration will be sent.
        /// </summary>
        DoNotNotify,

        /// <summary>
        /// Notify the "ReservedFor" user.
        /// </summary>
        ReservedFor,

        /// <summary>
        /// Notify the "ReservedBy" user.
        /// </summary>
        ReservedBy
    }
}
