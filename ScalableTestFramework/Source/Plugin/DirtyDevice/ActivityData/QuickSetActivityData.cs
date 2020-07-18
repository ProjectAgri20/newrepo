using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class QuickSetActivityData
    {
        /// <summary>
        /// Gets or sets the quick set title for setting up digital send.
        /// </summary>
        /// <value>The quick set title.</value>
        [DataMember]
        public string QuickSetTitle { get; set; }

        public QuickSetActivityData()
        {
            QuickSetTitle = "Dirty Ews";
        }
    }
}
