using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using HP.DeviceAutomation.Phoenix;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.DeviceSettings
{
    /// <summary>
    /// Implementation of <see cref="IDeviceSettingsManager" /> for a <see cref="PhoenixDevice" />.
    /// </summary>
    public sealed class PhoenixDeviceSettingsManager : IDeviceSettingsManager
    {
        private readonly PhoenixDevice _device;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixDeviceSettingsManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is null.</exception>
        public PhoenixDeviceSettingsManager(PhoenixDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
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
        public bool SetJobMediaMode(JobMediaMode mode)
        {
            if (mode == JobMediaMode.Unknown)
            {
                throw new ArgumentException($"Cannot set Job Media Mode to {mode}.", nameof(mode));
            }

            // We need to have debug telnet enabled on this device, to do that, we will send a payload to the printer opening it up
            if (EnableTelnetDebug())
            {

                // Now that debug telnet is open, we open telnet session and send the command
                const string crcCommandDirectory = "cd print/debug";
                string crcCommand = (mode == JobMediaMode.Paperless)
                    ? "VideoCRCMode true"
                    : "VideoCRCMode false";

                using (Telnet telnet = new Telnet(_device.Address, 23000))
                {
                    try
                    {
                        telnet.ReceiveUntilMatch(">");
                        telnet.SendLine(crcCommandDirectory);
                        telnet.ReceiveUntilMatch(">");
                        telnet.SendLine(crcCommand);
                        telnet.ReceiveUntilMatch(">");
                        return true;
                    }
                    catch (SocketException ex)
                    {
                        LogWarn($"Printer does not have telnet enabled, skipping paperless mode. {ex.Message}");
                        return false;
                    }
                }
            }
            return false;
        }

        private bool EnableTelnetDebug()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.Expect100Continue = false;

            HttpWebRequest crcHttpWebRequest = WebRequest.CreateHttp($"http://{_device.Address}/DevMgmt/ProductServiceDyn.xml");
            crcHttpWebRequest.Method = WebRequestMethods.Http.Put;
            crcHttpWebRequest.ContentType = "text/xml; charset=us-ascii";
            string postData = SettingsManagerResource.PhoenixTelnetDebugEnable;

            if (!string.IsNullOrEmpty(_device.AdminPassword))
            {
                var plainTextBytes = Encoding.UTF8.GetBytes("admin:" + _device.AdminPassword);
                string authorization = Convert.ToBase64String(plainTextBytes);
                crcHttpWebRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authorization + "");
            }
            try
            {
                var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(postData);
                crcHttpWebRequest.ContentLength = bytes.Length;

                using (var writeStream = crcHttpWebRequest.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }

                crcHttpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                using (var response = (HttpWebResponse)crcHttpWebRequest.GetResponse())
                {
                    // Should this check the return code?
                    return true;
                }
            }
            catch (WebException ex)
            {
                LogWarn("Could not enable telnet debug", ex);
                return false;
            }
        }
    }
}
