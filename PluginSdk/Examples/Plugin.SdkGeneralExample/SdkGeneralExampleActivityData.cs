/*
* © Copyright 2016 HP Development Company, L.P.
*/
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Plugin.SdkGeneralExample
{
    [DataContract]
    internal class SdkGeneralExampleActivityData
    {
        [DataMember]
        public string Label { get; set; }

        public SdkGeneralExampleActivityData(string label)
        {
            Label = label;
        }
    }
}
