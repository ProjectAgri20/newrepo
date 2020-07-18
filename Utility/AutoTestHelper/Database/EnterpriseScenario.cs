using System;
using System.Collections.Generic;

namespace HP.RDL.EDT.AutoTestHelper.Database
{
    /// <summary>
    /// Enterprise Scenario Object
    /// </summary>
    internal class EnterpriseScenario
    {
       /// <summary>
       /// Primary Key
       /// </summary>
        public Guid EnterpriseScenarioId { get; set; }

        /// <summary>
        /// Name of the scenario
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Vertical field
        /// </summary>
        public string Vertical { get; set; }

        /// <summary>
        /// Company used by Test Type
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// ignore
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Folder Id where the scenario resides
        /// </summary>
        public Guid FolderId { get; set; }

        /// <summary>
        /// Scenario Owner
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Scenario settings blob
        /// </summary>
        public string ScenarioSettings { get; set; }

        /// <summary>
        /// VirtualResource in this scenario
        /// </summary>
        public ICollection<VirtualResource> VirtualResources { get; set; }

        internal EnterpriseScenario()
        {
            VirtualResources = new List<VirtualResource>();
        }

    }


}
