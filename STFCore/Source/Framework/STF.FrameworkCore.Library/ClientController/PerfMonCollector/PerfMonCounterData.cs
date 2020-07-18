using System;
using System.Globalization;
using System.Xml.Serialization;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// the perfmoncounter data class, which is populated from the serialised data of the manifest
    /// </summary>

    [Serializable]
    public class PerfMonCounterData
    {
       // public int CollectionInterval { get; set; }
       // public string Key { get; set; }
      //  public string Instance { get; set; }
        /// <summary>
        /// The target host name
        /// </summary>
        public string TargetHost { get; set; }

        /// <summary>
        /// the performance category name
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Category's instance
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// the counter name
        /// </summary>
        public string Counter { get; set; }

        /// <summary>
        /// user credentials if the remote machine required authentication to do monitoring
        /// </summary>
        public PerfMonCounterCredential Credentials { get; set; }

        /// <summary>
        /// the period at which collection should be done
        /// </summary>
        public double Interval { get; set; }

        /// <summary>
        /// The time interval in HH MM SS format, for use in the datagrid view
        /// </summary>
        [XmlIgnore]
        public string IntervalString { get { return TimeSpan.FromMilliseconds(Interval).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture); } }

        /// <summary>
        /// The virtual resource metadata ID is used as Unique Identifier correlating between the collection and the grid
        /// </summary>
        [XmlIgnore]
        public Guid VirtualResourceMetadataId { get; set; }

        /// <summary>
        /// Activity Data for the PerfMonCollector virtual resource.
        /// </summary>
        public PerfMonCounterData()
        {
        }
    }

    [Serializable]
    public class PerfMonCounterCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }

        public PerfMonCounterCredential()
        {
        }

        public PerfMonCounterCredential(string userName, string password, string domain)
        {
            UserName = userName;
            Password = password;
            Domain = domain;
        }
    }
}
