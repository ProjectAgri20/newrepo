using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Contains information about a series that is calculated based on other series
    /// </summary>
    [Serializable]
    public class CalculatedSeries
    {
        private Collection<string> _includedSeries = new Collection<string>();

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the included series.
        /// </summary>
        public Collection<string> IncludedSeries
        {
            get { return _includedSeries; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedSeries"/> class.
        /// </summary>
        [Obsolete("This constructor is for serialization purposes only and should not be used.")]
        public CalculatedSeries()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedSeries"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public CalculatedSeries(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedSeries"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="includedSeries">The included series.</param>
        public CalculatedSeries(string name, IEnumerable<string> includedSeries)
        {
            if (includedSeries == null)
            {
                throw new ArgumentNullException("includedSeries");
            }

            Name = name;
            foreach (string series in includedSeries)
            {
                _includedSeries.Add(series);
            }
        }

        /// <summary>
        /// Returns a bool value indicating whether this instance includes a series with the specified name.
        /// </summary>
        /// <param name="seriesName">Name of the series.</param>
        /// <returns></returns>
        public bool IsIncluded(string seriesName)
        {
            return _includedSeries.Contains(seriesName);
        }

        /// <summary>
        /// Includes the specified series name.
        /// </summary>
        /// <param name="seriesName">Name of the series.</param>
        public void Include(string seriesName)
        {
            if (!IsIncluded(seriesName))
            {
                _includedSeries.Add(seriesName);
            }
        }

        /// <summary>
        /// Excludes the specified series name.
        /// </summary>
        /// <param name="seriesName">Name of the series.</param>
        public void Exclude(string seriesName)
        {
            if (IsIncluded(seriesName))
            {
                _includedSeries.Remove(seriesName);
            }
        }
    }
}
