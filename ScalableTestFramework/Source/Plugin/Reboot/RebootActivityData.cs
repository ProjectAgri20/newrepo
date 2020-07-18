using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Reboot
{
    [DataContract]
    public class RebootActivityData
    {
        [DataMember]
        public bool SetPaperless { get; set; }

        public RebootActivityData()
        {
            SetPaperless = false;
        }
    }
}
