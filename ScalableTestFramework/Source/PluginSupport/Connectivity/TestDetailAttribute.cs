using System;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Represents the Protocols IPv4, IPv6 etc.
    /// </summary>
    public enum ProtocolType
    {
        /// <summary>
        /// IPv4 Protocol
        /// </summary>
        IPv4,

        /// <summary>
        /// IPv6 Protocol
        /// </summary>
        IPv6
    }

    /// <summary>
    /// Represents wired or wireless Connectivity 
    /// </summary>
    public enum ConnectivityType
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Wired connection
        /// </summary>
        Wired,

        /// <summary>
        /// Wireless connection
        /// </summary>
        Wireless
    }

    /// <summary>
    /// Defines different product groups
    /// </summary>
    [Flags]
    public enum ProductFamilies
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 1,

        /// <summary>
        /// The VEP group
        /// </summary>
        [EnumValue("VEP")]
        VEP = 2,

        /// <summary>
        /// The TPS group
        /// </summary>
        [EnumValue("TPS")]
        TPS = 4,

        /// <summary>
        /// The LFP group
        /// </summary>
        [EnumValue("LFP")]
        LFP = 8,

        /// <summary>
        /// The ink jet group
        /// </summary>
        [EnumValue("InkJet")]
        InkJet = 16,

        /// <summary>
        /// Represents all the product families
        /// </summary>
        All = VEP | TPS | LFP | InkJet
    }

    /// <summary>
    /// Different print protocols used
    /// </summary>
    public enum PrintProtocolType
    {
        /// <summary>
        /// Represents all the print protocols
        /// </summary>
        All,
        /// <summary>
        /// The RAW protocol
        /// </summary>
        RAW,
        /// <summary>
        /// The LPD protocol
        /// </summary>
        LPD,
        /// <summary>
        /// The IPP protocol
        /// </summary>
        IPP,
        /// <summary>
        /// The FTP protocol
        /// </summary>
        FTP,
        /// <summary>
        /// The WS_PRINT protocol
        /// </summary>
        WS_PRINT
    }

    /// <summary>
    /// This class is used to represent the test case details like id, category and description for every test method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestDetailsAttribute : Attribute
    {
        /// <summary>
        /// ID of the test case
        /// </summary>
        int _id;

        /// <summary>
        /// Category of the test case
        /// </summary>
        string _category;

        /// <summary>
        /// Description of the test case
        /// </summary>
        string _description;

        /// <summary>
        /// Protocol of the test case
        /// </summary>
        ProtocolType _protocol;

        /// <summary>
        /// Connectivity type used for the test case
        /// </summary>
        ConnectivityType _connectivity;

        /// <summary>
        /// Product category (TPS, LFP, VEP, InkJet)
        /// </summary>
        ProductFamilies _productCategory;

        /// <summary>
        /// Print Protocol  (RAW, LPD, IPP, FTP)
        /// </summary>
        PrintProtocolType _printProtocol;

        /// <summary>
        /// Duration for continuous printing
        /// </summary>
        uint _printDuration;

        /// <summary>
        /// Port number for the print protocols RAW, LPD, IPP
        /// </summary>
        int _portNumber;

        /// <summary>
        /// Test Detail Attribute constructor.
        /// </summary>
        public TestDetailsAttribute()
        {
        }

        /// <summary>
        /// Gets or Sets the Id
        /// </summary>
        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// Gets or Sets the category
        /// </summary>
        public string Category
        {
            get { return this._category; }
            set { this._category = value; }
        }

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        /// <summary>
        /// Gets or Sets the Protocol
        /// </summary>
        public ProtocolType Protocol
        {
            get { return this._protocol; }
            set { _protocol = value; }
        }

        /// <summary>
        /// Gets or Sets the Connectivity
        /// </summary>
        public ConnectivityType Connectivity
        {
            get { return _connectivity; }
            set { _connectivity = value; }
        }

        /// <summary>
        /// Gets or Sets the Product Category
        /// </summary>
        public ProductFamilies ProductCategory
        {
            get { return _productCategory; }
            set { _productCategory = value; }
        }

        /// <summary>
        /// Gets or Sets Print Protocol
        /// </summary>
        public PrintProtocolType PrintProtocol
        {
            get { return _printProtocol; }
            set { _printProtocol = value; }
        }

        /// <summary>
        /// Gets or Sets the Id
        /// </summary>
        public uint PrintDuration
        {
            get { return this._printDuration; }
            set { this._printDuration = value; }
        }

        /// <summary>
        /// Gets or sets the port number for the print protocols
        /// </summary>
        public int PortNumber
        {
            get { return this._portNumber; }
            set { this._portNumber = value; }
        }
    }
}
