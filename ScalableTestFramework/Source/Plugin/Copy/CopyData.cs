using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.Plugin.Copy
{
    /// <summary>
    /// Contains data needed to execute the Copy plugin.
    /// </summary>
    [DataContract]
    public class CopyData
    {
        /// <summary>
        /// Gets or sets a value for number of copies
        /// </summary>
        /// <value>Number of copies.</value>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the color .
        /// </summary>
        /// <value>Color to be printed.</value>
        [DataMember]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the number of copies .
        /// </summary>
        /// <value>Number of copies.</value>
        [DataMember]
        public int Copies { get; set; }

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
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets the automation pause for simulators.
        /// </summary>
        /// <value>The automation pause.</value>
        [DataMember]
        public TimeSpan AutomationPause { get; set; }

        [DataMember]
        //public CopyPreferences Preferences { get; set; } Renamed to CopyOptions August 9 2017
        public CopyOptions Options { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether [Copy app auth (lazy)]
        /// </summary>
        [DataMember]
        public bool ApplicationAuthentication { get; set; }

        /// <summary>
        /// Gets or sets what authentication provider to use.
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Job Separator needs to be printed or not.
        /// </summary>
        [DataMember]
        public bool PrintJobSeparator { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyData"/> class.
        /// </summary>
        public CopyData()
        {
            UseQuickset = false;
            QuickSetName = string.Empty;
            Color = string.Empty;
            PageCount = 2;
            Copies = 1;
            LaunchQuicksetFromApp = true;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));
            AutomationPause = TimeSpan.FromSeconds(1);
            Options = new CopyOptions();
            ApplicationAuthentication = true;
            AuthProvider = AuthenticationProvider.Auto;
        }
        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            if (Options == null)
            {
                Options = new CopyOptions();
            }
        }
    }
}
