using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// A collection of <see cref="PrintQueueDefinition" /> objects.
    /// </summary>
    [DataContract]
    public sealed class PrintQueueDefinitionCollection : Collection<PrintQueueDefinition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueDefinitionCollection" /> class.
        /// </summary>
        public PrintQueueDefinitionCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueDefinitionCollection"/> class.
        /// </summary>
        /// <param name="printQueueDefinitions">The print queue definitions to include in the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printQueueDefinitions" /> is null.</exception>
        public PrintQueueDefinitionCollection(IEnumerable<PrintQueueDefinition> printQueueDefinitions)
        {
            if (printQueueDefinitions == null)
            {
                throw new ArgumentNullException(nameof(printQueueDefinitions));
            }

            foreach (PrintQueueDefinition printQueueDefinition in printQueueDefinitions)
            {
                Add(printQueueDefinition);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueDefinitionCollection" /> class.
        /// </summary>
        /// <param name="printQueues">The <see cref="PrintQueueInfo" /> objects whose definitions will be included in the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printQueues" /> is null.</exception>
        public PrintQueueDefinitionCollection(IEnumerable<PrintQueueInfo> printQueues)
        {
            if (printQueues == null)
            {
                throw new ArgumentNullException(nameof(printQueues));
            }

            foreach (PrintQueueInfo printQueue in printQueues.Where(n => n != null))
            {
                Add(printQueue.CreateDefinition());
            }
        }
    }
}
