using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about an automated badge box.
    /// </summary>
    [DataContract]
    public class BadgeBoxInfo : AssetInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _address;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _printerId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ReadOnlyCollection<BadgeInfo> _badges;

        /// <summary>
        /// Gets the network address (IP or hostname) of the badge box.
        /// </summary>
        public string Address => _address;

        /// <summary>
        /// Gets the ID of the printer to which the Badge Box is attached.
        /// </summary>
        public string PrinterId => _printerId;

        /// <summary>
        /// Gets the badges that are available to the badge box.
        /// </summary>
        public ReadOnlyCollection<BadgeInfo> Badges => _badges;

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeBoxInfo" /> class.
        /// </summary>
        /// <param name="assetId">The unique identifier for the badge box.</param>
        /// <param name="attributes">The <see cref="AssetAttributes" /> of the badge box.</param>
        /// <param name="assetType">The asset type.</param>
        /// <param name="address">The network address of the badge box.</param>
        /// <param name="printerId">The ID of the printer the badge box is connected to.</param>
        /// <param name="badges">The available badges for this badge box.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetId" /> is null.
        /// <para>or</para>
        /// <paramref name="address" /> is null.
        /// </exception>
        public BadgeBoxInfo(string assetId, AssetAttributes attributes, string assetType, string address, string printerId, IList<BadgeInfo> badges)
            : base(assetId, attributes, assetType)
        {
            _address = address ?? throw new ArgumentNullException(nameof(address));
            _printerId = printerId;
            _badges = new ReadOnlyCollection<BadgeInfo>(badges);
        }
    }
}
