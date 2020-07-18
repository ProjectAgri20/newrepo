using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A folder used for grouping items when displayed in the configuration tree.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public sealed class ConfigurationTreeFolder
    {
        /// <summary>
        /// Gets or sets the unique identifier for the configuration tree folder.
        /// </summary>
        public Guid ConfigurationTreeFolderId { get; set; }

        /// <summary>
        /// Gets or sets the name of this folder.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of objects that are contained in this folder.
        /// </summary>
        public string FolderType { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for this folder's parent.
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
