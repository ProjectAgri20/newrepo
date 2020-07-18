using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceInspector.Classes
{
    [DataContract]
    public class DataPair<TKey>
    {
        [DataMember]
        public TKey Key { get; set; }
        [DataMember]
        public bool Value { get; set; }


        public Type Type { get; set; }
        public DataPair()
        {
            Value = false;
            Type = null;
        }
    }
}
