using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.ScanToWorkflow
{
    /// <summary>
    /// Class containing the text and value for a workflow prompt.
    /// </summary>
    [DataContract]
    public class WorkflowPromptValue
    {
        /// <summary>
        /// Gets or sets the prompt text.
        /// </summary>
        /// <value>
        /// The prompt text.
        /// </value>
        [DataMember]
        public string PromptText { get; set; }

        /// <summary>
        /// Gets or sets the prompt value.
        /// </summary>
        /// <value>
        /// The prompt value.
        /// </value>
        [DataMember]
        public string PromptValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowPromptValue"/> class.
        /// </summary>
        /// <param name="promptText">The prompt text.</param>
        /// <param name="promptValue">The prompt value.</param>
        public WorkflowPromptValue(string promptText, string promptValue)
        {
            PromptText = promptText;
            PromptValue = promptValue;
        }
    }
}
