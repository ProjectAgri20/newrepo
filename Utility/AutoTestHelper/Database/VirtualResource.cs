using System;
using System.Collections.Generic;

namespace HP.RDL.EDT.AutoTestHelper.Database
{
    /// <summary>
    /// The Virtual Resource
    /// </summary>
    internal class VirtualResource
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public Guid VirtualResourceId { get; set; }

        /// <summary>
        /// Scenario the virtual resource belongs to
        /// </summary>
        public Guid EnterpriseScenarioId { get; set; }

        /// <summary>
        /// Name of the virtual resource
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// SolutionTester usually
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// Instance Count
        /// </summary>
        public int InstanceCount { get; set; }

        /// <summary>
        /// Virtual Machine Platform - LOCAL for STB
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Whether enabled or disabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Resource Per VM - 10 for STB
        /// </summary>
        public int ResourcePerVM { get; set; }

        /// <summary>
        /// Ignore
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// NULL
        /// </summary>
        public Guid FolderId { get; set; }

        /// <summary>
        /// Used for tagging to ALM
        /// </summary>
        public int TestCaseId { get; set; }

        /// <summary>
        /// Repeat Count
        /// </summary>
        public int RepeatCount { get; set; }

        /// <summary>
        /// Randomize startup delay
        /// </summary>
        public bool RandomizeStartupDelay { get; set; }

        /// <summary>
        /// Minimum startup delay
        /// </summary>
        public int MinStartupDelay { get; set; }

        /// <summary>
        /// Max Startup delay
        /// </summary>
        public int MaxStartupDelay { get; set; }

        /// <summary>
        /// Order or Shuffle Mode
        /// </summary>
        public bool RandomizeActivities { get; set; }

        /// <summary>
        /// Randomize activity delay
        /// </summary>
        public bool RandomizeActivityDelay { get; set; }

        /// <summary>
        /// Minimum Activity Delay
        /// </summary>
        public int MinActivityDelay { get; set; }

        /// <summary>
        /// maximum activity delay
        /// </summary>
        public int MaxActivityDelay { get; set; }

        /// <summary>
        /// Iteration, Duration
        /// </summary>
        public string RunMode { get; set; }

        /// <summary>
        /// duration time
        /// </summary>
        public int DurationTime { get; set; }

        /// <summary>
        /// Activities inside this resource
        /// </summary>
        public ICollection<VirtualResourceMetadata> VirtualResourceMetadata { get; set; }

        public VirtualResource()
        {
            VirtualResourceMetadata = new List<VirtualResourceMetadata>();
        }


    }
}
