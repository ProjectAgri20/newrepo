using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (import/export) for a ConfigurationTreeFolder.
    /// </summary>
    [DataContract]
    public class FolderContract
    {
        private Collection<Guid> _childIds;

        /// <summary>
        /// Folder Name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Folder Type.
        /// </summary>
        [DataMember]
        public string FolderType { get; set; }

        /// <summary>
        /// Collection of Child Ids.
        /// </summary>
        [DataMember]
        public Collection<Guid> ChildIds
        {
            get { return _childIds ?? (_childIds = new Collection<Guid>()); }
            set { _childIds = value; }
        }
    }
}