using System.Runtime.Serialization;
using HP.ScalableTest.Core.AssetInventory;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract for Assets (used for import/export) 
    /// </summary>
    [DataContract(Name="Asset", Namespace="")]
    [KnownType(typeof(PrinterContract))]
    public class AssetContract
    {
        /// <summary>
        /// Loads the AssetContract from the specified Asset object.
        /// </summary>
        /// <param name="asset"></param>
        public virtual void Load(Asset asset)
        {
            AssetId = asset.AssetId;
            AssetType = asset.AssetType;
            PoolName = asset.Pool.Name;
        }

        /// <summary>
        /// Asset Id
        /// </summary>
        [DataMember]
        public string AssetId { get; set; }

        /// <summary>
        /// Asset Type
        /// </summary>
        [DataMember]
        public string AssetType { get; set; }

        /// <summary>
        /// The Pool Name to which the asset belongs
        /// </summary>
        [DataMember]
        public string PoolName { get; set; }
    }
}
