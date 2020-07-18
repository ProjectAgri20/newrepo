using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Base class for a minimal identifier used to save <see cref="PrintQueueInfo" />.
    /// </summary>
    [DataContract]
    public abstract class PrintQueueDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueDefinition" /> class.
        /// </summary>
        protected PrintQueueDefinition()
        {
            // Constructor explicitly declared for XML doc.
        }
    }
}
