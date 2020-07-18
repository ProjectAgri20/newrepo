using System.Runtime.Serialization;
using HP.ScalableTest.Core.AssetInventory;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract for Printers (used for import/export) 
    /// </summary>
    [DataContract(Name = "Printer", Namespace = "")]
    public class PrinterContract : AssetContract
    {
        /// <summary>
        /// Constructor for PrinterContract.
        /// </summary>
        public PrinterContract()
        { }

        /// <summary>
        /// Loads the PrinterContract from the specified Asset object.
        /// </summary>
        public override void Load(Asset asset)
        {
            base.Load(asset);

            var printer = asset as Printer;

            Product = printer.Product;
            Model = printer.Model;
            Address1 = printer.Address1;
            Address2 = printer.Address2;
            Description = printer.Description;
            Location = printer.Location;
            PortNumber = printer.PortNumber;
            SnmpEnabled = printer.SnmpEnabled;
            Owner = printer.Owner;
            PrinterType = printer.PrinterType;
            SerialNumber = printer.SerialNumber;
            ModelNumber = printer.ModelNumber;
            EngineType = printer.EngineType;
            FirmwareType = printer.FirmwareType;
        }

        /// <summary>
        /// The Product.
        /// </summary>
        [DataMember]
        public string Product { get; set; }

        /// <summary>
        /// The Model.
        /// </summary>
        [DataMember]
        public string Model { get; set; }

        /// <summary>
        /// The Address.
        /// </summary>
        [DataMember]
        public string Address1 { get; set; }

        /// <summary>
        /// Additional Address.
        /// </summary>
        [DataMember]
        public string Address2 { get; set; }

        /// <summary>
        /// The Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// The Location.
        /// </summary>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// The Port Number.
        /// </summary>
        [DataMember]
        public int PortNumber { get; set; }

        /// <summary>
        /// Is Snmp Enabled.
        /// </summary>
        [DataMember]
        public bool SnmpEnabled { get; set; }

        /// <summary>
        /// The Owner.
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// The Printer Type.
        /// </summary>
        [DataMember]
        public string PrinterType { get; set; }

        /// <summary>
        /// The Serial Number.
        /// </summary>
        [DataMember]
        public string SerialNumber { get; set; }

        /// <summary>
        /// The Model Number.
        /// </summary>
        [DataMember]
        public string ModelNumber { get; set; }

        /// <summary>
        /// The Engine Type.
        /// </summary>
        [DataMember]
        public string EngineType { get; set; }

        /// <summary>
        /// The Firmware Type.
        /// </summary>
        [DataMember]
        public string FirmwareType { get; set; }
    }
}
