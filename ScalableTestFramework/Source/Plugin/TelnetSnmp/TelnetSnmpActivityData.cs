using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.TelnetSnmp
{
    /// <summary>
    /// Contains data needed to execute a Telnet and SNMP activity.
    /// </summary>
    [DataContract]
    public class TelnetSnmpActivityData
    {
        /// <summary>
        /// Product Name
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// Printer IP
        /// </summary>
        [DataMember]
        public string PrinterIP { get; set; }

        /// <summary>
        /// Product Category
        /// </summary>
        [DataMember]
        public string ProductCategory { get; set; }

        /// <summary>
        /// user selected tests
        /// </summary>
        [DataMember]
        public Collection<int> TestNumbers { get; set; }

        /// <summary>
        /// Test Type
        /// </summary>
        [DataMember]
        public bool TestType { get; set; }  // true: Telnet; false: SNMP

        /// <summary>
        /// Printer Type
        /// </summary>
        [DataMember]
        public int PrinterType { get; set; }  // 1 : VEP; 2: LFP; 3: TPS; 4:Inkjet

        /// <summary>
        /// Sitemaps Version number
        /// </summary>
        [DataMember]
        public string SitemapsVersion { get; set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TelnetSnmpActivityData"/> class.
        /// </summary>
        public TelnetSnmpActivityData()
        {
            PrinterIP = string.Empty;
            TestNumbers = new Collection<int>();
            TestType = true;
            SitemapsVersion = string.Empty;
            PrinterType = 1;
            ProductName = string.Empty;
            ProductCategory = string.Empty;
        }
        #endregion
    }
}
