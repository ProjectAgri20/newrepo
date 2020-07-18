using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Collection of Asset Contract
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [CollectionDataContract(Name = "Assets", Namespace = "")]
    public class AssetContractCollection<T> : Collection<T> where T : AssetContract, new()
    {
        /// <summary>
        /// Exports the AssetContractCollection to a file with the specified file name.
        /// </summary>
        /// <param name="fileName"></param>
        public void Export(string fileName)
        {
            File.WriteAllText(fileName, LegacySerializer.SerializeDataContract(this).ToString());
        }

        /// <summary>
        /// Loads the AssetContractCollection from the specified Asset collection.
        /// </summary>
        /// <param name="assets"></param>
        public void Load(IEnumerable<Asset> assets)
        {
            foreach (var asset in assets)
            {
                var contract = ContractFactory.Create<T>(asset);
                Add(contract);
            }
        }
    }
}
