using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HP.ScalableTest.Data.EnterpriseTest.Model;

namespace HP.ScalableTest.LabConsole
{
    internal class ResourceTypeRow
    {
        /// <summary>
        /// Helper class for displaying Resource Types in a grid.
        /// </summary>
        /// <param name="resourceType"></param>
        public ResourceTypeRow(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }

        public ResourceType ResourceType { get; private set; }
        public bool Selected { get; set; }
        public string Name { get { return ResourceType.Name; } }
    }

    internal class MetadataTypeRow
    {
        /// <summary>
        /// Helper class for displaying Metadata Types in a grid.
        /// </summary>
        /// <param name="metadataType"></param>
        public MetadataTypeRow(MetadataType metadataType)
        {
            MetadataType = metadataType;
        }

        public MetadataType MetadataType { get; private set; }
        public bool Selected { get; set; }
        public string Name { get { return MetadataType.Name; } }
    }
}
