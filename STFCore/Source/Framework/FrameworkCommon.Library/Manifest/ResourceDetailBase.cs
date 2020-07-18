using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;
using HP.ScalableTest.Core;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Base abstract class the supports manifest detail for all virtual resources
    /// </summary>
    [DataContract]
    [KnownType(typeof(AdminWorkerDetail))]
    [KnownType(typeof(EventLogCollectorDetail))]
    [KnownType(typeof(OfficeWorkerDetail))]
    [KnownType(typeof(PerfMonCollectorDetail))]
    [KnownType(typeof(MachineReservationDetail))]
    [KnownType(typeof(LoadTesterDetail))]
    [KnownType(typeof(VirtualResourceDetail))]
    public abstract class ResourceDetailBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceDetailBase"/> class.
        /// </summary>
        protected ResourceDetailBase()
        {
            MetadataDetails = new Collection<ResourceMetadataDetail>();
        }

        /// <summary>
        /// Gets or sets the resource unique identifier.
        /// </summary>
        [DataMember]
        public Guid ResourceId { get; set; }

        /// <summary>
        /// Gets the metadata details for this resource.
        /// </summary>
        [DataMember]
        public Collection<ResourceMetadataDetail> MetadataDetails { get; private set; }

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        [DataMember]
        public VirtualResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the description for this resource.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the instance count for this resource.
        /// </summary>
        [DataMember]
        public int InstanceCount { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the platform identifier.
        /// </summary>
        /// <value>The platform identifier.</value>
        [DataMember]
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets whether this resource is enabled.
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets the unique names associated with the specific resource type.
        /// </summary>
        public abstract IEnumerable<string> UniqueNames { get; }

        /// <summary>
        /// Gets the Virtual workers used by a resource
        /// </summary>
        [DataMember]
        public int? ResourcesPerVM { get; set; }
        /// <summary>
        /// Gets a formatted XML representation of the resource details.
        /// </summary>
        /// 
        public string Xml
        {
            get 
            { 
                // Load the document and replace the Activities, which are escaped
                // XML with actual XML.  This will make it display much better.
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(LegacySerializer.SerializeDataContract(this).ToString());

                XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
                manager.AddNamespace("ns", "http://schemas.datacontract.org/2004/07/HP.ScalableTest.Framework.Manifest");

                foreach (XmlNode node in doc.DocumentElement.SelectNodes("//ns:ResourceMetadataDetail/ns:Data", manager))
                {
                    var newNode = XmlUtil.CreateNode(doc, node.InnerText);
                    node.RemoveAll();
                    node.AppendChild((XmlNode)newNode);
                }

                return XmlUtil.CreateXDocument(doc).ToString();
            }
        }
    }
}