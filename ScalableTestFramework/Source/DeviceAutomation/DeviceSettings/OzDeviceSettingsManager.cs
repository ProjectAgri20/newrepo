using System;
using System.Collections.Generic;
using System.Net;
using HP.DeviceAutomation.Oz;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.DeviceSettings
{
    /// <summary>
    /// Implementation of <see cref="IDeviceSettingsManager" /> for an <see cref="OzDevice" />.
    /// </summary>
    public sealed class OzDeviceSettingsManager : IDeviceSettingsManager
    {
        private static readonly Dictionary<JobMediaMode, string> _commands = new Dictionary<JobMediaMode, string>()
        {
            [JobMediaMode.Paper] = SettingsManagerResource.OzPaperlessDisable,
            [JobMediaMode.Paperless] = SettingsManagerResource.OzPaperlessEnable
        };

        private readonly IPAddress _address;
        private const int _port = 9100;

        /// <summary>
        /// Initializes a new instance of the <see cref="OzDeviceSettingsManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is null.</exception>
        public OzDeviceSettingsManager(OzDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _address = IPAddress.Parse(device.Address);
        }

        /// <summary>
        /// Gets the job media mode for the device.
        /// </summary>
        /// <returns>The current <see cref="JobMediaMode" />.</returns>
        public JobMediaMode GetJobMediaMode() => JobMediaMode.Unknown;

        /// <summary>
        /// Sets the job media mode for the device.
        /// </summary>
        /// <param name="mode">The <see cref="JobMediaMode" /> to set.</param>
        /// <returns><c>true</c> if the job media mode was set successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException"><paramref name="mode" /> is set to <see cref="JobMediaMode.Unknown" />.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool SetJobMediaMode(JobMediaMode mode)
        {
            if (mode == JobMediaMode.Unknown)
            {
                throw new ArgumentException($"Cannot set Job Media Mode to {mode}.", nameof(mode));
            }

            try
            {
                using (PjlMessenger pjl = new PjlMessenger(_address, _port))
                {
                    pjl.Connect();
                    pjl.SendCommand(_commands[mode]);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogWarn("Unable to set job media mode.", ex);
                return false;
            }
        }
    }
}
