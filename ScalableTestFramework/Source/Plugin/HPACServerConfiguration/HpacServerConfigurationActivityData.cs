using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// Contains data needed to execute a HpacServerConfiguration through the HpacServer plugin.
    /// </summary>
    [DataContract]
    public class HpacServerConfigurationActivityData
    {
        /// <summary>
        /// Gets or sets the type of the HpacConfigTile.
        /// </summary>
        /// <value>The type of the HpacConfigTile.</value>
        [DataMember]
        public HpacTile HpacConfigTile { get; set; }

        /// <summary>
        /// Gets or sets the type of the Settings Tab Data.
        /// </summary>
        /// <value>Settings Tab Data.</value>
        [DataMember]
        public SettingsTabData SettingsData { get; set; }

        /// <summary>
        /// Gets or sets the type of the IRM Tab Data.
        /// </summary>
        /// <value>IRM Tab Data.</value>
        [DataMember]
        public IRMTabData IRMData { get; set; }

        /// <summary>
        /// Gets or sets the type of the Device Tab Data.
        /// </summary>
        /// <value>Device Tab Data.</value>
        [DataMember]
        public DeviceTabData DeviceData { get; set; }

        /// <summary>
        /// Gets or sets the type of the PrintServer Tab Data.
        /// </summary>
        /// <value>PrintServer Tab Data.</value>
        [DataMember]
        public PrintServerTabData PrintServerData { get; set; }

        /// <summary>
        /// Gets or sets the type of the JobAccounting Tab Data.
        /// </summary>
        /// <value>JobAccounting Tab Data.</value>
        [DataMember]
        public JobAccountingTabData JobAccountingData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HpacServerConfigurationActivityData"/> class.
        /// </summary>
        public HpacServerConfigurationActivityData()
        {
            HpacConfigTile = HpacTile.Devices;
            SettingsData = new SettingsTabData();
            IRMData = new IRMTabData();
            DeviceData = new DeviceTabData();
            PrintServerData = new PrintServerTabData();
            JobAccountingData = new JobAccountingTabData();
        }

    }
}

