using System;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework.Data
{
    /// <summary>
    /// Logs usage of a document by a plugin activity.
    /// </summary>
    public sealed class ActivityExecutionDocumentUsageLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "ActivityExecutionDocumentUsage";

        /// <summary>
        /// Gets the document name.
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string DocumentName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutionDocumentUsageLog" /> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="document">The <see cref="Document" /> used by the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        public ActivityExecutionDocumentUsageLog(PluginExecutionData executionData, Document document)
            : base(executionData)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            DocumentName = document.FileName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutionDocumentUsageLog" /> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="documentName">The name of the document used by the activity.</param>
        public ActivityExecutionDocumentUsageLog(PluginExecutionData executionData, string documentName)
            : base(executionData)
        {
            DocumentName = documentName;
        }
    }
}
