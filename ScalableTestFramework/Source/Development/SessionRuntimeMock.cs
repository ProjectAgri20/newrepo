using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A mock implementation of <see cref="ISessionRuntime" /> for development.
    /// </summary>
    public sealed class SessionRuntimeMock : ISessionRuntime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionRuntimeMock" /> class.
        /// </summary>
        public SessionRuntimeMock()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Reports a non-functioning asset to the runtime framework.
        /// (This mock does not implement any behavior in response.)
        /// </summary>
        /// <param name="assetInfo">The <see cref="IAssetInfo" /> representing the asset in error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetInfo" /> is null.</exception>
        public void ReportAssetError(IAssetInfo assetInfo)
        {
            if (assetInfo == null)
            {
                throw new ArgumentNullException(nameof(assetInfo));
            }

            // Intentionally left blank for this mock.
        }

        /// <summary>
        /// Collects a memory usage profile from the specified device.
        /// (This mock does not implement any behavior in response.)
        /// </summary>
        /// <param name="deviceInfo">The <see cref="IDeviceInfo" /> representing the device to profile.</param>
        /// <param name="label">The label to apply (for logging purposes).</param>
        /// <exception cref="ArgumentNullException"><paramref name="deviceInfo" /> is null.</exception>
        public void CollectDeviceMemoryProfile(IDeviceInfo deviceInfo, string label)
        {
            if (deviceInfo == null)
            {
                throw new ArgumentNullException(nameof(deviceInfo));
            }

            // Intentionally left blank for this mock.
        }
    }
}
