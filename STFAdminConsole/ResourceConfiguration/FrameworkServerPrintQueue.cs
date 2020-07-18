using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// Defines the queues that are being used
    /// </summary>
    public class PrintQueueInUse
    {
        /// <summary>
        /// Gets or sets the name of the scenario.
        /// </summary>
        /// <value>
        /// The name of the scenario.
        /// </value>
        public string ScenarioName { get; set; }

        /// <summary>
        /// Gets or sets the virtual resource.
        /// </summary>
        /// <value>
        /// The virtual resource.
        /// </value>
        public string VirtualResource { get; set; }

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the metadata description.
        /// </summary>
        /// <value>
        /// The metadata description.
        /// </value>
        public string MetadataDescription { get; set; }

        /// <summary>
        /// Gets or sets the type of the metadata.
        /// </summary>
        /// <value>
        /// The type of the metadata.
        /// </value>
        public string MetadataType { get; set; }

        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>
        /// The name of the server.
        /// </value>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; set; }

        /// <summary>
        /// Returns ScenarioName to make it easier to display which scenarios are using the queue.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ScenarioName;
        }
    }
}
