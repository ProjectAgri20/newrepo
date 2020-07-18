using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Contention
{
    /// <summary>
    /// Contains data about the Copy activity in the Contention Plugin
    /// </summary>
    [ControlPanelActivity("Copy")]
    public class CopyActivityData
    {
        /// <summary>
        /// Gets or sets a value for page count
        /// </summary>
        /// <value>Page Count.</value>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the number of copies .
        /// </summary>
        /// <value>Number of copies.</value>
        [DataMember]
        public int Copies { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyActivityData"/> class
        /// </summary>
        public CopyActivityData()
        {
            PageCount = 1;
            Copies = 1;
        }
    }
}
