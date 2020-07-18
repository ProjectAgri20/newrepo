using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceInspector.Classes
{
    [DataContract]
    public abstract class AQuickSet // : IComponentData
    {
        [DataMember]
        public string QName { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string QType { get; set; }
        [DataMember]
        public ScanSettings ScanSetData { get; set; }

        [DataMember]
        public FileSettings FileSetData { get; set; }

    }
}
