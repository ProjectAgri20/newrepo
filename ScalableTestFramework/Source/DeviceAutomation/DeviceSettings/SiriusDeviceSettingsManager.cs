using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.DeviceSettings
{
    /// <summary>
    /// Implementation of <see cref="IDeviceSettingsManager" /> for a <see cref="SiriusDevice" />.
    /// </summary>
    public sealed class SiriusDeviceSettingsManager : IDeviceSettingsManager
    {
        private readonly SiriusDevice _device;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusDeviceSettingsManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is null.</exception>
        public SiriusDeviceSettingsManager(SiriusDevice device)
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

            //previously we were using telnet to send the UDW command, turns out the port was not fixed for a printer product and can 
            //vary for each one. 
            //the same can also be achieved by a REST command and works easily over port 80, the code has been modified to achieve the same
            //Veda - 16/04/2018

            string crcCommand = (mode == JobMediaMode.Paperless)
                ? "smgr_pe.multi_button_push+58"
                : "smgr_pe.multi_button_push+59";
            try
            {
                WebClient client = new WebClient();
                client.QueryString.Add("entry", crcCommand);
                client.DownloadString($"http://{_device.Address}:80/UDW/Command");
                return true;
            }
            catch (Exception e)
            {
                LogWarn($"Printer is not on supported firmware, skipping paperless mode. {e.Message}");
                return false;
            }
          

           

        }

        private bool EnableTelnetDebug()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.Expect100Continue = false;

            HttpWebRequest crcHttpWebRequest = WebRequest.CreateHttp($"http://{_device.Address}/ePrint/SecureCfg");
            crcHttpWebRequest.Method = WebRequestMethods.Http.Post;
            crcHttpWebRequest.ContentType = "application/octet-stream";
            string postData = SettingsManagerResource.SiriusTelnetDebugEnable;

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
