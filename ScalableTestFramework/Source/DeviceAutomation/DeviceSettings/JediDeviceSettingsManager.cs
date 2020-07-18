using System;
using System.Collections.Generic;
using System.Net;
using HP.DeviceAutomation.Jedi;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.DeviceSettings
{
    /// <summary>
    /// Implementation of <see cref="IDeviceSettingsManager" /> for a <see cref="JediDevice" />.
    /// </summary>
    public sealed class JediDeviceSettingsManager : IDeviceSettingsManager
    {
        private static readonly Dictionary<JobMediaMode, string> _commands = new Dictionary<JobMediaMode, string>()
        {
            [JobMediaMode.Paper] = SettingsManagerResource.JediPaperlessDisable,
            [JobMediaMode.Paperless] = SettingsManagerResource.JediPaperlessEnable
        };

        private readonly IPAddress _address;
        private const int _port = 9100;
        private readonly string _password;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediDeviceSettingsManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is null.</exception>
        public JediDeviceSettingsManager(JediDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _address = IPAddress.Parse(device.Address);
            _password = device.AdminPassword;
        }

        /// <summary>
        /// Gets the job media mode for the device.
        /// </summary>
        /// <returns>The current <see cref="JobMediaMode" />.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public JobMediaMode GetJobMediaMode()
        {
            try
            {
                using (PjlMessenger pjl = new PjlMessenger(_address, _port))
                {
                    pjl.Connect();
                    string response = pjl.SendInquire(SettingsManagerResource.JediPaperlessInquire);
                    return response.Contains("ON") ? JobMediaMode.Paper : JobMediaMode.Paperless;
                }
            }
            catch (Exception ex)
            {
                LogWarn("Unable to get job media mode.", ex);
                return JobMediaMode.Unknown;
            }
        }

        /// <summary>
        /// Sets the job media mode for the device.
        /// </summary>
        /// <param name="mode">The <see cref="JobMediaMode" /> to set.</param>
        /// <returns><c>true</c> if the job media mode was set successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException"><paramref name="mode" /> is set to <see cref="JobMediaMode.Unknown" />.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool SetJobMediaMode(JobMediaMode mode)
        {
            bool success = false;
            if (mode == JobMediaMode.Unknown)
            {
                throw new ArgumentException($"Cannot set Job Media Mode to '{mode}'.", nameof(mode));
            }
            //Get current mode but set to allow PJL
            string currentPJLMode = AllowPJLAnyway();

            JobMediaMode currentState = GetJobMediaMode();
            if (currentState == mode)
            {
                LogDebug($"Job media mode already set to '{mode}' - no change required.");
                success =  true;
            }
            else
            {
                LogDebug($"Job media mode set to '{currentState}', setting to '{mode}'.");
                int tries = 0;
                while (currentState != mode && tries++ < 3)
                {
                    try
                    {
                        using (PjlMessenger pjl = new PjlMessenger(_address, _port))
                        {
                            pjl.Connect();
                            pjl.SendCommand(_commands[mode]);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogWarn("Unable to set job media mode.", ex);
                    }
                    currentState = GetJobMediaMode();

                    if (currentState != mode)
                    {
                        LogDebug($"Job media mode failed to set. Attempt {tries}. Retrying...");
                    }
                }

                if (GetJobMediaMode() == mode)
                {
                    LogDebug($"Job media mode '{mode}' set successfully.");
                    success = true;
                }
                else
                {
                    LogWarn($"Failed to set job media mode '{mode}'.");
                    success =  false;
                }
            }
            RestorePJL(currentPJLMode);
            return success;
        }

        private string AllowPJLAnyway()
        {
            JediDevice device = new JediDevice(_address, _password);
            string urn = "urn:hp:imaging:con:service:security:SecurityService";
            string endpoint = "security";

            WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
            string oldValue = tic.FindElement("PjlDeviceAccess").Value;
            if (oldValue == "disabled")
            {
                LogDebug("Abling PJL");
                tic.FindElement("PjlDeviceAccess").SetValue("enabled");
                device.WebServices.PutDeviceTicket(endpoint, urn, tic);
            }
            return oldValue;
        }
        private void RestorePJL(string pjlMode)
        {
            JediDevice device = new JediDevice(_address, _password);
            string urn = "urn:hp:imaging:con:service:security:SecurityService";
            string endpoint = "security";

            WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
            string oldValue = tic.FindElement("PjlDeviceAccess").Value;
            if (pjlMode != oldValue)
            {
                LogDebug("Disabling PJL");
                tic.FindElement("PjlDeviceAccess").SetValue(pjlMode);
                device.WebServices.PutDeviceTicket(endpoint, urn, tic);
            }
        }
    }
}
