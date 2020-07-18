using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Contention
{
    /// <summary>
    /// Contains data about the Print activity in the Contention Plugin
    /// </summary>
    [ContentionActivity("Print")]
    public class PrintActivityData
    {
        /// <summary>
        /// Gets or sets a value for the Print Queue name
        /// </summary>
        [DataMember]
        public string QueueName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintActivityData"/> class
        /// </summary>
        public PrintActivityData()
        {
            QueueName = string.Empty;
        }
    }
}
