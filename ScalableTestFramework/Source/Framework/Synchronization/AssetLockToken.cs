using System;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Framework.Synchronization
{
    /// <summary>
    /// A <see cref="LockToken" /> that represents a system-wide lock on an asset.
    /// </summary>
    public class AssetLockToken : LockToken
    {
        /// <summary>
        /// Gets the <see cref="IAssetInfo" /> representing the asset to lock.
        /// </summary>
        public IAssetInfo AssetInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetLockToken" /> class with the specified lock timeouts.
        /// </summary>
        /// <param name="asset">The <see cref="IAssetInfo" /> representing the asset to lock.</param>
        /// <param name="acquireTimeout">The amount of time to wait to acquire the lock.</param>
        /// <param name="holdTimeout">The amount of time to hold the lock.</param>
        /// <exception cref="ArgumentNullException"><paramref name="asset" /> is null.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="asset" /> ID is an empty string.
        /// <para>or</para>
        /// <paramref name="acquireTimeout" /> is less than <see cref="TimeSpan.Zero" />.
        /// <para>or</para>
        /// <paramref name="holdTimeout" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        public AssetLockToken(IAssetInfo asset, TimeSpan acquireTimeout, TimeSpan holdTimeout)
            : base(GetKey(asset), acquireTimeout, holdTimeout)
        {
            AssetInfo = asset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetLockToken" /> class with the specified lock timeouts.
        /// </summary>
        /// <param name="asset">The <see cref="IAssetInfo" /> representing the asset to lock.</param>
        /// <param name="timeouts">The <see cref="LockTimeoutData" /> that defines timeout behavior.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="asset" /> is null.
        /// <para>or</para>
        /// <paramref name="timeouts" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="asset" /> ID is an empty string.</exception>
        public AssetLockToken(IAssetInfo asset, LockTimeoutData timeouts)
            : base(GetKey(asset), timeouts)
        {
            AssetInfo = asset;
        }

        private static string GetKey(IAssetInfo asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            if (string.IsNullOrEmpty(asset.AssetId))
            {
                throw new ArgumentException("Asset ID cannot be an empty string.", nameof(asset));
            }

            return asset.AssetId;
        }
    }
}
