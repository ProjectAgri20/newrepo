namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A reservation on a block of users in a <see cref="DomainAccountPool" />.
    /// </summary>
    public sealed class DomainAccountReservation
    {
        /// <summary>
        /// Gets or sets the unique identifier for the domain account pool.
        /// </summary>
        public string DomainAccountKey { get; set; }

        /// <summary>
        /// Gets or sets the session ID for which the accounts are reserved.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the (inclusive) start index for the reservation.
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of users reserved.
        /// </summary>
        public int Count { get; set; }
    }
}
