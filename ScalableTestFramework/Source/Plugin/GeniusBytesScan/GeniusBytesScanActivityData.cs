using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.GeniusBytes;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;

namespace HP.ScalableTest.Plugin.GeniusBytesScan
{
    [DataContract]
    internal class GeniusBytesScanActivityData
    {
        public GeniusBytesScanActivityData()
        {
            ImagePreview = false;
            ReleaseOnSignIn = false;
            ScanCount = 1;
            AppName= GeniusBytesScanType.Scan2ME;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
        }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets the Scan Count
        /// </summary>
        /// <value>The document process action.</value>
        [DataMember]
        public int ScanCount { get; set; }

        /// <summary>
        /// Gets or sets the AuthProvider 
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets the Selected AppName 
        /// </summary>
        [DataMember]
        public GeniusBytesScanType AppName { get; set; }

        /// <summary>
        /// Gets or sets the Selected File type Options 
        /// </summary>
        [DataMember]
        public GeniusByteScanFileTypeOption FileType { get; set; }

        /// <summary>
        /// Gets or sets the Selected Color Options 
        /// </summary>
        [DataMember]
        public GeniusByteScanColorOption ColourOption { get; set; }

        /// <summary>
        /// Gets or sets the Selected Sides Options 
        /// </summary>
        [DataMember]
        public GeniusByteScanSidesOption SidesOption { get; set; }

        /// <summary>
        /// Gets or sets the Selected ResolutionOption
        /// </summary>
        [DataMember]
        public GeniusByteScanResolutionOption ResolutionOption { get; set; }

        /// <summary>
        /// Gets or sets whether to pull all documents immediately after sign-in.
        /// </summary>
        [DataMember]
        public bool ReleaseOnSignIn { get; set; }

        [DataMember]
        public bool ImagePreview { get; set; }
    }
}
