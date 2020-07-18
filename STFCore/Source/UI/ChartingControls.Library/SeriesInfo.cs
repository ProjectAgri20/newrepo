
namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Contains information about a series in a graph.
    /// </summary>
    internal class SeriesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesInfo"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public SeriesInfo(string key, bool enabled = true)
        {
            Key = key;
            Enabled = enabled;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SeriesInfo"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }
    }
}
