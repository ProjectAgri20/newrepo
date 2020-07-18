using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data contract (import/export) used to serialize an resource (Asset, Document, Print Queue, Server) usage object.
    /// </summary>
    [DataContract]
    public class ResourceUsageContract
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="virtualResourceMetadataId"></param>
        /// <param name="xmlSelectionData"></param>
        public ResourceUsageContract(Guid virtualResourceMetadataId, string xmlSelectionData)
        {
            VirtualResourceMetadataId = virtualResourceMetadataId;
            XmlSelectionData = xmlSelectionData;
        }

        /// <summary>
        /// Virtual Resource Metadata Id.
        /// </summary>
        [DataMember]
        public Guid VirtualResourceMetadataId { get; set; }

        /// <summary>
        /// XML selection data that defines usage of resources.
        /// </summary>
        [DataMember]
        public string XmlSelectionData { get; set; }
    }
}
