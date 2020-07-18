using System.Collections.Generic;

namespace HP.ScalableTest.PluginSupport.MobilePrintApp
{
    /// <summary>
    /// Object to print on mobile print app
    /// </summary>
    public class ObjectToPrint
    {
        /// <summary>
        /// Type of object.
        /// refer <see cref="DocumentType"/>
        /// </summary>
        public DocumentType Type { get; set; }

        /// <summary>
        /// Dictionary of Information (i.e. file path, web page address)
        /// </summary>
        public Dictionary<string, object> Infomation;

        /// <summary>
        /// Initialize
        /// </summary>
        public ObjectToPrint()
        {
            Infomation = new Dictionary<string, object>();
        }
    }
}
