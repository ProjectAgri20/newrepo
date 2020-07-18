using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A pool of domain user accounts tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{DomainAccountKey,nq}")]
    public class DomainAccountPool
    {
        /// <summary>
        /// Gets or sets the unique identifier for this domain account pool.
        /// </summary>
        public string DomainAccountKey { get; set; }

        /// <summary>
        /// Gets or sets the user name format for the users in this pool.
        /// Uses a syntax compatible with <see cref="string.Format(string, object)" />.
        /// </summary>
        public string UserNameFormat { get; set; }

        /// <summary>
        /// Gets or sets the minimum user number.
        /// </summary>
        public int MinimumUserNumber { get; set; }

        /// <summary>
        /// Gets or sets the maximum user number.
        /// </summary>
        public int MaximumUserNumber { get; set; }

        /// <summary>
        /// Gets or sets a description for the domaina account pool.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the reservations for this domain account pool.
        /// </summary>
        public virtual ICollection<DomainAccountReservation> Reservations { get; set; } = new HashSet<DomainAccountReservation>();
    }
}
