using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data contract for OfficeWorker (used for import/export).
    /// </summary>
    [DataContract(Name = "OfficeWorker", Namespace = "")]
    [KnownType(typeof(SolutionTesterContract))]
    [KnownType(typeof(CitrixWorkerContract))]
    [KnownType(typeof(AdminWorkerContract))]
    [ObjectFactory(VirtualResourceType.OfficeWorker)]
    public class OfficeWorkerContract : ResourceContract
    {
        /// <summary>
        /// Loads the OfficeWorkerContract from the specified VirtualResource object.
        /// </summary>
        /// <param name="resource"></param>
        public override void Load(VirtualResource resource)
        {
            base.Load(resource);

            var worker = resource as OfficeWorker;

            RepeatCount = worker.RepeatCount;
            RandomizeStartupDelay = worker.RandomizeStartupDelay;
            MinStartupDelay = worker.MinStartupDelay;
            MaxStartupDelay = worker.MaxStartupDelay;
            RandomizeActivities = worker.RandomizeActivities;
            RandomizeActivityDelay = worker.RandomizeActivityDelay;
            MinActivityDelay = worker.MinActivityDelay;
            MaxActivityDelay = worker.MaxActivityDelay;
            ExecutionMode = worker.ExecutionMode;
            DurationTime = worker.DurationTime;
            SecurityGroups = worker.SecurityGroups;
            ExecutionSchedule = worker.ExecutionSchedule;
            UserPool = worker.UserPool;
        }

        /// <summary>
        /// Defines how many times the activity is repeated.
        /// </summary>
        [DataMember]
        public int RepeatCount { get; set; }

        /// <summary>
        /// Whether to randomize startup delay using the Max and Min delay values.
        /// </summary>
        [DataMember]
        public bool RandomizeStartupDelay { get; set; }

        /// <summary>
        /// The mininum delay before starting.
        /// </summary>
        [DataMember]
        public int MinStartupDelay { get; set; }

        /// <summary>
        /// The Maximum delay before starting.
        /// </summary>
        [DataMember]
        public int MaxStartupDelay { get; set; }

        /// <summary>
        /// Whether to randomize activity execution.
        /// </summary>
        [DataMember]
        public bool RandomizeActivities { get; set; }

        /// <summary>
        /// Whether to randomize activity delay using the Max and Min delay values.
        /// </summary>
        [DataMember]
        public bool RandomizeActivityDelay { get; set; }

        /// <summary>
        /// The minimum delay between activities.
        /// </summary>
        [DataMember]
        public int MinActivityDelay { get; set; }

        /// <summary>
        /// The Maximum delay between activities.
        /// </summary>
        [DataMember]
        public int MaxActivityDelay { get; set; }

        /// <summary>
        /// The execution mode.
        /// </summary>
        [DataMember]
        public ExecutionMode ExecutionMode { get; set; }

        /// <summary>
        /// The duration time.
        /// </summary>
        [DataMember]
        public int DurationTime { get; set; }

        /// <summary>
        /// NT Security Group Data.
        /// </summary>
        [DataMember]
        public string SecurityGroups { get; set; }

        /// <summary>
        /// The execution schedule data.
        /// </summary>
        [DataMember]
        public string ExecutionSchedule { get; set; }

        /// <summary>
        /// The name of the User Pool.
        /// </summary>
        [DataMember]
        public string UserPool { get; set; }
    }
}
