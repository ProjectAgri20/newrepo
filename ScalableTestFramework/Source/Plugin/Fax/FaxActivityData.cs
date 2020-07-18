using System;
using System.Runtime.Serialization;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.Plugin.Fax
{
    /// <summary>
    /// Contains data needed to execute the Fax activity.
    /// </summary>
    [DataContract]
    internal class FaxActivityData
    {
        /// <summary>
        /// Initializes a new instance of the FaxActivityData class.
        /// </summary>
        public FaxActivityData()
        {
            EnableNotification = false;
            AutomationPause = TimeSpan.FromSeconds(1);
            FaxType = FaxConfiguration.AnalogFax;
            FaxOperation = FaxTask.SendFax;
            FaxNumber = null;
            FaxReceiveTimeout = new TimeSpan(0, 1, 0);
            UseSpeedDial = false;
            ScanOptions = new ScanOptions();
            ApplicationAuthentication = true;
            AuthProvider = AuthenticationProvider.Auto;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enable notification.
        /// </summary>
        /// <value><c>true</c> if notification should be enabled; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool EnableNotification { get; set; }

        /// <summary>
        /// Gets or Sets the Fax type(Analog Fax/Lan Fax/Internet Fax)
        /// </summary>
        [DataMember]
        public FaxConfiguration FaxType { get; set; }

        /// <summary>
        ///  Gets or Sets the Fax operation Type(Send/receive)
        /// </summary>
        [DataMember]
        public FaxTask FaxOperation { get; set; }

        /// <summary>
        /// Gets or Sets the Fax Number
        /// </summary>
        [DataMember]
        public String FaxNumber { get; set; }

        /// <summary>
        /// Timeout for Fax Receiving.Time to wait before checking the fax Report 
        /// </summary>
        [DataMember]
        public TimeSpan FaxReceiveTimeout { get; set; }

        /// <summary>
        /// Gets or sets the notification email address.
        /// </summary>
        /// <value>The notification email address.</value>
        [DataMember]
        public string NotificationEmail { get; set; }

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

        [DataMember]
        public bool UseSpeedDial { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the image preview during execution
        /// </summary>
        [DataMember]
        public int ImagePreviewOptions { get; set; }

        /// <summary>
        /// Gets or set the PIN to use.
        /// </summary>
        [DataMember]
        public string PIN { get; set; }

        /// <summary>
        /// Gets or sets a value indicating ScanOptions
        /// </summary>
        /// <value>The scan Options</value>
        [DataMember]
        public ScanOptions ScanOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [Fax app auth (lazy)]
        /// </summary>
        [DataMember]
        public bool ApplicationAuthentication { get; set; }

        /// <summary>
        /// Gets or sets what authentication provider to use.
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Job Separator needs to be printed or not
        /// </summary>
        [DataMember]
        public bool PrintJobSeparator { get; set; }

        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            if (ScanOptions == null)
            {
                ScanOptions = new ScanOptions();
            }
            if (PIN == null)
            {
                PIN = string.Empty;
            }
        }
    }

    /// <summary>
    /// Enumerator for Type of the Fax operation(Send/Receive)
    /// </summary>
    public enum FaxTask
    {
        /// <summary>
        /// Send Fax
        /// </summary>
        SendFax,

        /// <summary>
        /// Receive Fax
        /// </summary>
        ReceiveFax
    }

    /// <summary>
    /// Enumerator for Type of the Fax Configuration
    /// </summary>
    public enum FaxConfiguration
    {
        /// <summary>
        /// Analog Fax
        /// </summary>
        AnalogFax,

        /// <summary>
        /// LAN Fax
        /// </summary>
        LANFax,

        /// <summary>
        /// InternetFax
        /// </summary>
        InternetFax
    }
}
