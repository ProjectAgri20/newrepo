
using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.JetAdvantageScan
{
    /// <summary>
    /// JetAdvantage Data class.
    /// </summary>
    [DataContract]
    public class JetAdvantageScanActivityData
    {
        /// <summary>
        /// Gets or sets the admin password.
        /// </summary>
        /// <value>The admin password.</value>
        [DataMember]
        public string AdminPassword { get; set; }
        /// <summary>
        /// Gets or sets the JetAdvantage login Id. (Email address)
        /// </summary>
        /// <value>The JetAdvantage Login ID</value>
        [DataMember]
        public string JetAdvantageLoginId { get; set; }
        /// <summary>
        /// Gets or sets the JetAdvantage password
        /// </summary>
        /// <value>The JetAdvantage password</value>
        [DataMember]
        public string JetAdvantagePassword { get; set; }
        /// <summary>
        /// Settings for configuring scan
        /// </summary>
        [DataMember]
        public ScanSettings Settings { get; set; }
        /// <summary>
        ///The JetAdvantage Login pin value.
        /// </summary>
        [DataMember]
        public string JetAdvantageLoginPin { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether or not to use login PIN.
        /// </summary>
        /// <value><c>true</c> if [authenticate with Login Pin]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UseLoginPin { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to use the ADF for simulators.
        /// </summary>
        /// <value><c>true</c> if the simulator ADF should be used; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UseAdf { get; set; }
        /// <summary>
        /// Gets or sets the automation pause for simulators.
        /// </summary>
        /// <value>The automation pause.</value>
        [DataMember]
        public TimeSpan AutomationPause { get; set; }
        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>The page count.</value>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Initializes class members
        /// </summary>
        public JetAdvantageScanActivityData()
        {
            AdminPassword = "admin";
            JetAdvantageLoginId = "";
            JetAdvantagePassword = "";
            Settings = new ScanSettings();
            UseLoginPin = false;
            JetAdvantageLoginPin = "";
            UseAdf = false;
            AutomationPause = TimeSpan.FromSeconds(1);
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));
            PageCount = 1;
        }
    }

    /// <summary>
    /// Scan Settings 
    /// </summary>
    [DataContract]
    public class ScanSettings
    {
        /// <summary>
        /// Gets or sets a value indicating what size of the paper to be selected.
        /// </summary>
        [DataMember]
        public string PaperSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating in which orientation scan should perform
        /// </summary>
        [DataMember]
        public string Orientation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating which mode scan should perform.
        /// </summary>
        [DataMember]
        public string DuplexMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the FileType of the Scanned Document
        /// </summary>
        [DataMember]
        public string FileType { get; set; }

        /// <summary>
        /// Initializes class members
        /// </summary>
        public ScanSettings()
        {
            PaperSize = "Letter";
            Orientation = "Portrait";
            DuplexMode = "Simplex";
            FileType = "Pdf";
        }

        /// <summary>
        /// Overriding the Tostring method.
        /// </summary>
        public override string ToString()
        {
            return string.Format("PaperSize: {0}, Orientation: {1}, Sides: {2}, Filetype: {3}",
                    PaperSize, Orientation, DuplexMode, FileType);
        }
    }
}
