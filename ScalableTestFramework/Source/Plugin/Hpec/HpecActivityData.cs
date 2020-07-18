using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.Hpec
{
    /// <summary>
    /// Contains data needed to execute a scan to workflow through the Hpec plugin.
    /// </summary>
    [DataContract]
    public class HpecActivityData
    {
        /// <summary>
        /// Gets or sets the name of the workflow.
        /// </summary>
        /// <value>
        /// The name of the workflow.
        /// </value>
        [DataMember]
        public string WorkflowName { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>The page count.</value>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [application authentication] or Sign In via device sign in button.
        /// </summary>
        /// <value>
        /// <c>true</c> if [application authentication]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ApplicationAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HpecActivityData"/> class.
        /// </summary>
        public HpecActivityData()
        {
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));
            PageCount = 1;
            ApplicationAuthentication = false;
        }
    }
}
