using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    /// <summary>
    /// Titan parent class data contract.
    /// </summary>
    [DataContract]
    internal class Parent
    {
        [DataMember]
        public string createdBy { get; set; }

        [DataMember]
        public string isChildHidden { get; set; }

        [DataMember]
        public string ownerId { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string library { get; set; }

        [DataMember]
        public string modifiedBy { get; set; }

        [DataMember]
        public string folderId { get; set; }

        [DataMember]
        public string parentId { get; set; }

        [DataMember]
        public string datasource { get; set; }

        [DataMember]
        public Int64 version { get; set; }

        [DataMember]
        public bool isShared { get; set; }

        [DataMember]
        public Int64 modifiedWhen { get; set; }

        [DataMember]
        public string access { get; set; }
    }
}