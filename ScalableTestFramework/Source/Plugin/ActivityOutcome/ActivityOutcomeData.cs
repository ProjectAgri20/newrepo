using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ActivityOutcome
{
    /// <summary>
    /// Contains data needed to execute an ActivityOutcome activity.
    /// </summary>
    [DataContract]
    public class ActivityOutcomeData
    {
        /// <summary>
        /// Gets or sets the result value.
        /// </summary>
        /// <value>
        /// The outcome.
        /// </value>
        [DataMember]
        public PluginResult Result { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [random outcome].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [random outcome]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool RandomResult { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityOutcomeData" /> class.
        /// </summary>
        public ActivityOutcomeData()
        {
            Result = PluginResult.Passed;
            RandomResult = false;
        }
    }
}
