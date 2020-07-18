using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    [DataContract(Name = "Document")]
    public class TitanDocument
    {
        [DataMember]
        public bool isShared { get; set; }

        [DataMember]
        public string folderId { get; set; }

        [DataMember]
        public string documentId { get; set; }

        [DataMember]
        public Int64 fileModifiedWhen { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public Int64 version { get; set; }

        [DataMember]
        public string createdBy { get; set; }

        [DataMember]
        public string extension { get; set; }

        [DataMember]
        public string tags { get; set; }

        [DataMember]
        public string uploadedBy { get; set; }

        [DataMember]
        public Int64 size { get; set; }

        [DataMember]
        public string ownerId { get; set; }

        [DataMember]
        public string library { get; set; }

        [DataMember]
        public string fileReadOnly { get; set; }

        [DataMember]
        public string modifiedBy { get; set; }

        [DataMember]
        public Int64 fileCreatedWhen { get; set; }

        [DataMember]
        public string access { get; set; }

        [DataMember]
        public string datasource { get; set; }

        [DataMember]
        public Int64 createdWhen { get; set; }

        [DataMember]
        public Int64 modifiedWhen { get; set; }
    }
}