using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Data used for selecting print queues for use in a test.
    /// </summary>
    [DataContract]
    public sealed class PrintQueueSelectionData
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PrintQueueDefinitionCollection _selectedPrintQueues;

        /// <summary>
        /// Gets the selected print queues.
        /// </summary>
        public PrintQueueDefinitionCollection SelectedPrintQueues => _selectedPrintQueues;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueSelectionData" /> class.
        /// </summary>
        public PrintQueueSelectionData()
        {
            _selectedPrintQueues = new PrintQueueDefinitionCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueSelectionData"/> class.
        /// </summary>
        /// <param name="selectedPrintQueue">The selected print queue.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectedPrintQueue" /> is null.</exception>
        public PrintQueueSelectionData(PrintQueueInfo selectedPrintQueue)
        {
            if (selectedPrintQueue == null)
            {
                throw new ArgumentNullException(nameof(selectedPrintQueue));
            }

            _selectedPrintQueues = new PrintQueueDefinitionCollection(new List<PrintQueueInfo> { selectedPrintQueue });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueSelectionData" /> class.
        /// </summary>
        /// <param name="selectedPrintQueues">The selected print queues.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectedPrintQueues" /> is null.</exception>
        public PrintQueueSelectionData(IEnumerable<PrintQueueInfo> selectedPrintQueues)
        {
            _selectedPrintQueues = new PrintQueueDefinitionCollection(selectedPrintQueues);
        }
    }
}
