using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A mock implementation of <see cref="IEnvironmentConfiguration" /> for development.
    /// </summary>
    public sealed class EnvironmentConfigurationMock : IEnvironmentConfiguration
    {
        private readonly Dictionary<string, List<string>> _outputMonitorDestinations = new Dictionary<string, List<string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentConfigurationMock" /> class.
        /// </summary>
        public EnvironmentConfigurationMock()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Adds the specified STF output monitor destination to the environment configuration.
        /// </summary>
        /// <param name="monitorType">The STF monitor type.</param>
        /// <param name="destination">The destination to add to the environment configuration.</param>
        public void AddOutputMonitorDestination(string monitorType, string destination)
        {
            if (!_outputMonitorDestinations.ContainsKey(monitorType))
            {
                _outputMonitorDestinations[monitorType] = new List<string>();
            }
            _outputMonitorDestinations[monitorType].Add(destination);
        }

        /// <summary>
        /// Removes the specified STF output monitor destination from the environment configuration.
        /// </summary>
        /// <param name="monitorType">The STF monitor type.</param>
        /// <param name="destination">The destination to remove from the environment configuration.</param>
        public void RemoveOutputMonitorDestination(string monitorType, string destination)
        {
            if (_outputMonitorDestinations.ContainsKey(monitorType))
            {
                _outputMonitorDestinations[monitorType].Remove(destination);
            }
        }

        /// <summary>
        /// Removes all STF output monitor configuration from the environment configuration.
        /// </summary>
        public void ClearOutputMonitorConfiguration()
        {
            _outputMonitorDestinations.Clear();
        }

        #region IEnvironmentConfiguration Members

        /// <summary>
        /// Retrieves a collection of output monitor destinations of the specified type.
        /// </summary>
        /// <param name="monitorType">The type of output monitor to retrieve.</param>
        /// <returns>A collection of output monitor destinations.</returns>
        public IEnumerable<string> GetOutputMonitorDestinations(string monitorType)
        {
            if (_outputMonitorDestinations.ContainsKey(monitorType))
            {
                // The ToList call ensures we send a copy of the list instead of the actual server list.
                return _outputMonitorDestinations[monitorType].ToList();
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        #endregion
    }
}
