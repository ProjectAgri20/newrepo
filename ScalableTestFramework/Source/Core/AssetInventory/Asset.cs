using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// Base class for a test asset tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{AssetId,nq}")]
    public abstract class Asset
    {
        /// <summary>
        /// Gets or sets the unique identifier for the asset.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the type of the asset.
        /// </summary>
        public string AssetType { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="AssetAttributes" /> object representing the capabilities of the asset.
        /// </summary>
        public AssetAttributes Capability { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="AssetPool" /> this asset is assigned to.
        /// </summary>
        public virtual AssetPool Pool { get; set; }

        /// <summary>
        /// Gets or sets the reservations for this asset.
        /// </summary>
        public virtual ICollection<AssetReservation> Reservations { get; set; } = new HashSet<AssetReservation>();

        /// <summary>
        /// Builds an <see cref="AssetInfo" /> from this instance.
        /// If not applicable for this asset type, returns null instead.
        /// </summary>
        /// <returns>An <see cref="AssetInfo" /> with the data from this instance, or null.</returns>
        public abstract AssetInfo ToAssetInfo();

        /// <summary>
        /// Gets the reservation key for the next <see cref="AssetReservation" /> on this <see cref="Asset" />.
        /// If the asset does not have a reservation, returns null.
        /// </summary>
        /// <returns>The reservation key for the active <see cref="AssetReservation" />, or null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method performs a calculation each time it is called.")]
        protected string GetReservationKey()
        {
            AssetReservation current = Reservations.OrderBy(n => n.Start).FirstOrDefault(n => n.End >= DateTime.Now);
            if (current != null)
            {
                return string.IsNullOrEmpty(current.SessionId) ? current.ReservedFor : current.SessionId;
            }
            else
            {
                return null;
            }
        }
    }
}
