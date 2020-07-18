using System;

namespace HP.ScalableTest.DeviceAutomation.DeviceSettings
{
    /// <summary>
    /// Interface for setting configuration on a device.
    /// </summary>
    public interface IDeviceSettingsManager
    {
        /// <summary>
        /// Gets the job media mode for the device.
        /// </summary>
        /// <returns>The current <see cref="JobMediaMode" />.</returns>
        JobMediaMode GetJobMediaMode();

        /// <summary>
        /// Sets the job media mode for the device.
        /// </summary>
        /// <param name="mode">The <see cref="JobMediaMode" /> to set.</param>
        /// <returns><c>true</c> if the job media mode was set successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException"><paramref name="mode" /> is set to <see cref="JobMediaMode.Unknown" />.</exception>
        bool SetJobMediaMode(JobMediaMode mode);
    }
}
