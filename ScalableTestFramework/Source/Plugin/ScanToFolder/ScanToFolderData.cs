using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.Plugin.ScanToFolder
{
    /// <summary>
    /// Contains data needed to execute a scan to folder through the ScanToFolder plugin.
    /// </summary>
    [DataContract]
    public class ScanToFolderData
    {
        /// <summary>
        /// Gets or sets the folder path.
        /// </summary>
        /// <value>The folder path.</value>
        [DataMember]
        public string FolderPath { get; set; }

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
        /// Gets or sets the type of the destination.
        /// </summary>
        /// <value>The destination type.</value>
        [DataMember]
        public string DestinationType { get; set; }

        /// <summary>
        /// Gets or sets the destination count.
        /// </summary>
        /// <value>The destination count.</value>
        [DataMember]
        public int DestinationCount { get; set; }

        /// <summary>
        /// Gets or sets the digital send server.
        /// </summary>
        /// <value>The digital send server.</value>
        [DataMember]
        public string DigitalSendServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the image preview during execution
        /// </summary>
        [DataMember]
        public int ImagePreviewOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the apply credentials on verification
        /// <value><c>true</c> if apply credentials on verification is checked;otherwise, <c>false</c>.</value>
        /// </summary>
        [DataMember]
        public bool ApplyCredentialsOnVerification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating ScanOptions
        /// </summary>
        /// <value>The scan Options</value>
        [DataMember]
        public ScanOptions ScanOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ScanToFolder app auth (lazy)]
        /// </summary>
        [DataMember]
        public bool ApplicationAuthentication { get; set; }

        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

      
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToFolderData"/> class.
        /// </summary>
        public ScanToFolderData()
        {
            UseQuickset = false;
            UseOcr = false;
            AutomationPause = TimeSpan.FromSeconds(1);
            DestinationType = "Folder";
            DestinationCount = 1;
            ImagePreviewOptions = 0;
            ApplyCredentialsOnVerification = false;
            ScanOptions = new ScanOptions();
            AuthProvider = AuthenticationProvider.Auto;
            ApplicationAuthentication = true;
            
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
