using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Represents a condition that can be applied to a WMI query.
    /// </summary>
    internal sealed class RegistryMonitorCondition
    {
        /// <summary>
        /// Gets the name of the property that this condition filters on.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the list of possible values for this condition.
        /// </summary>
        public IEnumerable<string> Values { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryMonitorCondition" /> class.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The value.</param>
        public RegistryMonitorCondition(string propertyName, string value)
            : this(propertyName, new[] { value })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryMonitorCondition" /> class.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="values">The list of values.</param>
        public RegistryMonitorCondition(string propertyName, IEnumerable<string> values)
        {
            PropertyName = propertyName;
            Values = values;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Join(" OR ", Values.Select(n => $"{PropertyName} = '{n}'"));
        }
    }
}
