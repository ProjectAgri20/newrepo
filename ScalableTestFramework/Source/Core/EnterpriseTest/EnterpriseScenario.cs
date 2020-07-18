using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A test configuration scenario that defines the test setup and execution parameters.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class EnterpriseScenario
    {
        /// <summary>
        /// Gets or sets the unique identifier for the enterprise scenario.
        /// </summary>
        public Guid EnterpriseScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="ConfigurationTreeFolder" /> that contains this scenario.
        /// </summary>
        public Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the name of this enterprise scenario.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of this enterprise scenario.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the user that owns this scenario.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the vertical to which this scenario applies.
        /// </summary>
        public string Vertical { get; set; }

        /// <summary>
        /// Gets or sets the reference company for this scenario.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the XML scenario settings.
        /// </summary>
        public string ScenarioSettings { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="VirtualResource" /> objects associated with this scenario.
        /// </summary>
        public virtual ICollection<VirtualResource> VirtualResources { get; set; } = new HashSet<VirtualResource>();

        /// <summary>
        /// Gets or sets the <see cref="AssociatedProductVersion" /> objects associated with this scenario.
        /// </summary>
        public virtual ICollection<AssociatedProductVersion> AssociatedProductVersions { get; set; } = new HashSet<AssociatedProductVersion>();

        /// <summary>
        /// Gets or sets the <see cref="EnterpriseScenarioSession" /> objects associated with this scenario.
        /// </summary>
        public virtual ICollection<EnterpriseScenarioSession> ScenarioSessions { get; set; } = new HashSet<EnterpriseScenarioSession>();

        /// <summary>
        /// Gets or sets the user groups that have permissions to view or modify this scenario.
        /// </summary>
        public virtual ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();
    }
}
