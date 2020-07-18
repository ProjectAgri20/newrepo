using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A record of session information for an <see cref="EnterpriseScenario" /> execution.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public sealed class EnterpriseScenarioSession
    {
        /// <summary>
        /// Gets or sets the unique identifier for the enterprise scenario.
        /// </summary>
        public Guid EnterpriseScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the session name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the session notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the date when this session information was last edited.
        /// </summary>
        public DateTime EditedDate { get; set; }
    }
}
