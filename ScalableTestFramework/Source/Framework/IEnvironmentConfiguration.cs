using System.Collections.Generic;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Provides access to test environment configuration information.
    /// </summary>
    public interface IEnvironmentConfiguration
    {
        /// <summary>
        /// Retrieves a collection of output monitor destinations of the specified type.
        /// </summary>
        /// <param name="monitorType">The type of output monitor to retrieve.</param>
        /// <returns>A collection of output monitor destinations.</returns>
        IEnumerable<string> GetOutputMonitorDestinations(string monitorType);
    }
}
