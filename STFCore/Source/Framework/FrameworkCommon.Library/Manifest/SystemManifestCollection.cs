using System;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Defines a collection of <see cref="SystemManifest"/> objects
    /// </summary>
    public class SystemManifestCollection : Collection<SystemManifest>
    {
        /// <summary>
        /// Gets or sets the scenario unique identifier.
        /// </summary>
        public Guid ScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the name of the scenario.
        /// </summary>
        public string ScenarioName { get; set; }
    }
}
