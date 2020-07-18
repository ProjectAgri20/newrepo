using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class DigitalSendOutputFolderActivityData
    {
        /// <summary>
        /// Gets or sets the UNC Path for setting up digital send.
        /// </summary>
        /// <value>The UNC Path.</value>
        [DataMember]
        public DigitalSendOutputLocation OutputFolder { get; set; }

        public DigitalSendOutputFolderActivityData()
        {
            OutputFolder = null;
        }
    }
}
