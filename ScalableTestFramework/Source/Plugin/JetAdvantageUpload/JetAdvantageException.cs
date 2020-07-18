using System;
using System.Runtime.Serialization;
using System.Security;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    [Serializable]
    public class JetAdvantageException : Exception
    {
        public string ProxyRootUrl { get; set; }

        public string PullPrintUrl { get; set; }

        public JetAdvantageException(string message) : base(message)
        {
        }

        public JetAdvantageException(string message, Exception ex) : base(message, ex)
        {
        }

        protected JetAdvantageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Used when serializing this object.
        /// </summary>
        /// <param name="info">Container where to store serialized data.</param>
        /// <param name="context">Streaming context</param>
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ProxyRootUrl", ProxyRootUrl);
            info.AddValue("PullPrintUrl", PullPrintUrl);
        }
    }
}