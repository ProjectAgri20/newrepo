using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.mPrint
{
    /// <summary>
    /// Contains data needed to execute a mPrint activity.
    /// </summary>
    [DataContract]
    internal class mPrintActivityData
    {
        /// <summary>
        /// Initializes a new instance of the mPrintActivityData class.
        /// </summary>
        public mPrintActivityData()
        {
            serv = null;
            queueIndex = "";
        }

        [DataMember]
        public ServerInfo serv { get; set; }

        [DataMember]
        public string queueIndex { get; set; }
    }
}
