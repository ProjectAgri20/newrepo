using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Contention
{
    /// <summary>
    /// Contains data about the Fax activity in the Contention Plugin
    /// </summary>
    [ControlPanelActivity("Fax")]
    public class FaxActivityData
    {
        /// <summary>
        /// Gets or sets a value for Page Count
        /// </summary>
        /// <value>Page Count</value>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the destination Fax number (for Fax send)
        /// </summary>
        /// <value>The Fax number</value>
        [DataMember]
        public string FaxNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaxActivityData"/> class
        /// </summary>
        public FaxActivityData()
        {
            PageCount = 1;
            FaxNumber = string.Empty;
        }
    }
}
