using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.Plugin.ScanToWorkflow
{
    /// <summary>
    /// Contains data needed to execute a scan to folder through the ScanToWorkflow plugin.
    /// </summary>
    [DataContract]
    public class ScanToWorkflowData
    {
        /// <summary>
        /// Gets or sets the name of the workflow.
        /// </summary>
        /// <value>The name of the workflow.</value>
        [DataMember]
        public string WorkflowName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable OCR, if available.
        /// </summary>
        /// <value>Gets or sets a value indicating whether the workflow is expected to use OCR.</value>
        [DataMember]
        public bool UseOcr { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>The page count.</value>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets the automation pause for simulators.
        /// </summary>
        /// <value>The automation pause.</value>
        [DataMember]
        public TimeSpan AutomationPause { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the ADF for simulators.
        /// </summary>
        /// <value><c>true</c> if the simulator ADF should be used; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UseAdf { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to exclude the prompt for file name.
        /// </summary>
        /// <value><c>true</c> if the OCR prompts to be excluded; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool ExcludeFileNamePrompt { get; set; }

        /// <summary>
        /// Gets or sets the type of the destination.
        /// </summary>
        /// <value>The destination type.</value>
        [DataMember]
        public string DestinationType { get; set; }

        /// <summary>
        /// Gets or sets the digital send server.
        /// </summary>
        /// <value>The digital send server.</value>
        [DataMember]
        public string DigitalSendServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ScanToWorkflowAuth app auth (lazy)]
        /// </summary>
        [DataMember]
        public bool ApplicationAuthentication { get; set; }

        /// <summary>
        /// Gets or sets what authentication provider to use.
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets the prompt values.
        /// </summary>
        /// <value>The prompt values.</value>
        [DataMember]
        public Collection<WorkflowPromptValue> PromptValues { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToWorkflowData"/> class.
        /// </summary>
        public ScanToWorkflowData()
        {
            UseOcr = false;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));
            PageCount = 1;
            UseAdf = false;
            ExcludeFileNamePrompt = false;
            AutomationPause = TimeSpan.FromSeconds(1);
            DestinationType = "Workflow";
            PromptValues = new Collection<WorkflowPromptValue>();
            ApplicationAuthentication = true;
            AuthProvider = AuthenticationProvider.Auto;
        }
    }
}
