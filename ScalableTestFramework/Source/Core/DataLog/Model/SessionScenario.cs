using System;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// A scenario executed in a session.
    /// </summary>
    public sealed class SessionScenario
    {
        /// <summary>
        /// Gets or sets the identifier for the session that ran this scenario.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the order in which the scenario was run.
        /// </summary>
        public byte RunOrder { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the scenario began running.
        /// </summary>
        public DateTime? ScenarioStart { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the scenario completed running.
        /// </summary>
        public DateTime? ScenarioEnd { get; set; }

        /// <summary>
        /// Gets or sets the configuration data for the scenario.
        /// </summary>
        public string ConfigurationData { get; set; }
    }
}

