using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.Plugin.ScanToEmail
{
    /// <summary>
    /// Contains data needed to execute a scan to folder through the ScanToFolder plugin.
    /// </summary>
    [DataContract]
    public class ScanToEmailData
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>The email address.</value>
        [DataMember]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the email address source.
        /// </summary>
        /// <value> The email address source.</value>
        [DataMember]
        public string AddressSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable OCR, if available.
        /// </summary>
        /// <value><c>true</c> if OCR should be enabled; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UseOcr { get; set; }

        /// <summary>
        /// Gets or sets the automation pause for simulators.
        /// </summary>
        /// <value>The automation pause.</value>
        [DataMember]
        public TimeSpan AutomationPause { get; set; }

        /// <summary>
        /// Gets or sets the digital send server.
        /// </summary>
        /// <value>The digital send server.</value>
        [DataMember]
        public string DigitalSendServer { get; set; }

        /// <summary>
        /// Gets or sets the name of the quick set.
        /// </summary>
        /// <value>The quick set name.</value>
        [DataMember]
        public string QuickSetName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use a folder quickset.
        /// </summary>
        /// <value><c>true</c> if a quickset should be used; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UseQuickset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether launch the quickset from App or Homescreen.
        /// </summary>
        /// <value><c>true</c> if a quickset should be used; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool LaunchQuicksetFromApp { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to use the image preview during execution
        /// </summary>
        [DataMember]
        public int ImagePreviewOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating ScanOptions
        /// </summary>
        /// <value>The scan Options</value>
        [DataMember]
        public ScanOptions ScanOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ScanToEmail app auth (lazy)]
        /// </summary>
        [DataMember]
        public bool ApplicationAuthentication { get; set; }

        /// <summary>
        /// Gets or sets what authentication provider to use.
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToEmailData"/> class.
        /// </summary>
        public ScanToEmailData()
        {
            UseQuickset = false;
            QuickSetName = string.Empty;
            UseOcr = false;
            AddressSource = string.Empty;
            
            AutomationPause = TimeSpan.FromSeconds(1);
            ImagePreviewOptions = 0;
            ScanOptions = new ScanOptions();
            ApplicationAuthentication = true;
            AuthProvider = AuthenticationProvider.Auto;
        }

        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            if (ScanOptions == null)
            {
                ScanOptions = new ScanOptions();
            }
        }
    }
}
