using System;


namespace HP.RDL.EDT.AutoTestHelper.Database
{
    /// <summary>
    /// Virtual Resource Metadata class
    /// </summary>
    internal class VirtualResourceMetadata
    {
       /// <summary>
       /// Primary Key
       /// </summary>
        public Guid VirtualResourceMetadataId { get; set; }
        /// <summary>
        /// Name of the activity
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Solution Tester usually
        /// </summary>
        public string ResourceType { get; set; }
        /// <summary>
        /// Plugin Type
        /// </summary>
        public string MetadataType { get; set; }

        /// <summary>
        /// Plugin metadata stored in xml
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// Solution tester Id this belongs to
        /// </summary>
        public Guid VirtualResourceId { get; set; }

        /// <summary>
        /// Whether enabled or disabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// ignore
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// NULL
        /// </summary>
        public Guid FolderId { get; set; }

        /// <summary>
        /// Execution Plan
        /// </summary>
        public string ExecutionPlan { get; set; }
        /// <summary>
        /// Used for converters
        /// </summary>
        public string MetadataVersion { get; set; }

       
    }
}
