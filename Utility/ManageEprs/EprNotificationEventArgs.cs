using System;
using System.Linq;
using System.Xml;

namespace HP.ScalableTest.EndpointResponder
{
    public class EprNotificationEventArgs : EventArgs
    {
        public XmlElement Data { get; private set; }
        public EprNotificationEventArgs(XmlElement data)
        {
            Data = data;
        }
    }
}
