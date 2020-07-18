using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.Classes
{
    [DataContract]
    public abstract class AQuickSet // : IComponentData
    {
        /// <summary>
        /// Quickset Name, may vary from name
        /// </summary>
        [DataMember]
        public string QName { get; set; }

        /// <summary>
        /// Datamember name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Quickset Type, Scan,Fax,Email, etc.
        /// </summary>
        [DataMember]
        public string QType { get; set; }

        /// <summary>
        /// Scan settings for quickset type
        /// </summary>
        [DataMember]
        public ScanSettings ScanSetData { get; set; }

        /// <summary>
        /// File Settings for quickset
        /// </summary>
        [DataMember]
        public FileSettings FileSetData { get; set; }

    }
}
