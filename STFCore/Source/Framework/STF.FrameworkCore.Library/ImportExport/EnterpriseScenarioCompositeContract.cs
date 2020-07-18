using System.IO;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Contract for Enterprise Scenario Composite
    /// </summary>
    [DataContract(Name = "Composite", Namespace = "")]
    public class EnterpriseScenarioCompositeContract
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scenario"></param>
        public EnterpriseScenarioCompositeContract(EnterpriseScenarioContract scenario)
        {
            Scenario = scenario;
            Documents = new DocumentContractCollection();
            Printers = new AssetContractCollection<PrinterContract>();
        }

        /// <summary>
        /// Scenario
        /// </summary>
        [DataMember]
        public EnterpriseScenarioContract Scenario { get; set; }

        /// <summary>
        /// Documents
        /// </summary>
        [DataMember]
        public DocumentContractCollection Documents { get; set; }

        /// <summary>
        /// Printers
        /// </summary>
        [DataMember]
        public AssetContractCollection<PrinterContract> Printers { get; set; }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            File.WriteAllText(fileName, Save());
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <returns></returns>
        public string Save()
        {
            return Serializer.Serialize(this).ToString();
        }
    }
}
