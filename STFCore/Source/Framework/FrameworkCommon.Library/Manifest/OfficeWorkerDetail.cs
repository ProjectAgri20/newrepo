using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using HP.ScalableTest.Core;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Manifest details for the Office Worker virtual resource
    /// </summary>
    [DataContract]
    [ObjectFactory(VirtualResourceType.OfficeWorker)]
    [KnownType(typeof(CitrixWorkerDetail))]
    [KnownType(typeof(SolutionTesterDetail))]
    public class OfficeWorkerDetail : ResourceDetailBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorkerDetail"/> class.
        /// </summary>
        public OfficeWorkerDetail()
        {
            UserCredentials = new Collection<OfficeWorkerCredential>();
            ExternalCredentials = new Collection<ExternalCredentialDetail>();
        }

        /// <summary>
        /// Gets the user credentials.
        /// </summary>
        [DataMember]
        public Collection<OfficeWorkerCredential> UserCredentials { get; private set; }

        /// <summary>
        /// Gets the external credentials.
        /// </summary>
        [DataMember]
        public Collection<ExternalCredentialDetail> ExternalCredentials { get; private set; }

        /// <summary>
        /// Gets or sets the command port offset.
        /// </summary>
        [DataMember]
        public int CommandPortOffset { get; set; }

        /// <summary>
        /// Gets or sets the duration time.
        /// </summary>
        [DataMember]
        public int DurationTime { get; set; }

        /// <summary>
        /// Gets or sets the execution mode.
        /// </summary>
        [DataMember]
        public ExecutionMode ExecutionMode { get; set; }

        /// <summary>
        /// Gets or sets the execution schedule.
        /// </summary>
        [DataMember]
        public string ExecutionSchedule { get; set; }

        /// <summary>
        /// Gets or sets the maximum activity delay.
        /// </summary>
        [DataMember]
        public int MaxActivityDelay { get; set; }

        /// <summary>
        /// Gets or sets the maximum startup delay.
        /// </summary>
        [DataMember]
        public int MaxStartupDelay { get; set; }

        /// <summary>
        /// Gets or sets the minimum activity delay.
        /// </summary>
        [DataMember]
        public int MinActivityDelay { get; set; }

        /// <summary>
        /// Gets or sets the minimum startup delay.
        /// </summary>
        [DataMember]
        public int MinStartupDelay { get; set; }

        /// <summary>
        /// <summary>
        /// Gets or sets a value indicating whether to randomize the delay after each activity execution.
        /// </summary>
        /// <value>
        /// <c>true</c> if delay between activity execution should be randomized; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool RandomizeActivities { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [randomize activity delay].
        /// </summary>
        /// <value>
        /// <c>true</c> if activity delay should be randomized; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool RandomizeActivityDelay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [randomize startup delay].
        /// </summary>
        /// <value>
        /// <c>true</c> if startup delay should be randomized; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool RandomizeStartupDelay { get; set; }

        /// <summary>
        /// Gets or sets the repeat count.
        /// </summary>
        [DataMember]
        public int RepeatCount { get; set; }

        ///// <summary>
        ///// Gets or sets the resources per vm.
        ///// </summary>
        //[DataMember]
        //public int ResourcesPerVM { get; set; }

        /// <summary>
        /// Gets or sets the security groups.
        /// </summary>
        [DataMember]
        public string SecurityGroups { get; set; }

        /// <summary>
        /// Gets or sets the office worker count.
        /// </summary>
        [DataMember]
        public int OfficeWorkerCount { get; set; }

        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        [DataMember]
        public int StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the user name format.
        /// </summary>
        [DataMember]
        public string UserNameFormat { get; set; }

        /// <summary>
        /// Gets the unique names associated with the specific resource type.
        /// </summary>
        public override IEnumerable<string> UniqueNames
        {
            get
            {
                return UserCredentials.Select(x => x.ResourceInstanceId);
            }
        }
    }
}