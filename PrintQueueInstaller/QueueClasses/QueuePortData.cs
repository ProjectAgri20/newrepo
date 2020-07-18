using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Defines data associated with updating the printer port a queue is pointing to.
    /// </summary>
    public class QueuePortData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueuePortData"/> class.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <param name="portName">The port name</param>
        public QueuePortData(string queueName, string portName)
        {
            QueueName = queueName;
            PortName = portName;
        }

        /// <summary>
        /// Gets or set the Print Queue name.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the Port Name associated with the Print Queue.
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets the Port Address associated with the Print Queue.
        /// </summary>
        public string PortAddress { get; set; }

        /// <summary>
        /// Gets or sets the new port address for this port
        /// </summary>
        public string NewPortAddress { get; set; }

        /// <summary>
        /// Gets or sets the Port Number.
        /// </summary>
        public int PortNumber { get; set; }

    }
}
