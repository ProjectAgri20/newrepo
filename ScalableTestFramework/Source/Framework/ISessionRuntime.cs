using System;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Provides access to runtime components available when executing a test session.
    /// </summary>
    public interface ISessionRuntime
    {
        /// <summary>
        /// Reports a non-functioning asset to the runtime framework.
        /// </summary>
        /// <param name="assetInfo">The <see cref="IAssetInfo" /> representing the asset in error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetInfo" /> is null.</exception>
        void ReportAssetError(IAssetInfo assetInfo);

        /// <summary>
        /// Collects a memory usage profile from the specified device.
        /// </summary>
        /// <param name="deviceInfo">The <see cref="IDeviceInfo" /> representing the device to profile.</param>
        /// <param name="label">The label to apply (for logging purposes).</param>
        /// <exception cref="ArgumentNullException"><paramref name="deviceInfo" /> is null.</exception>
        void CollectDeviceMemoryProfile(IDeviceInfo deviceInfo, string label);
    }
}
