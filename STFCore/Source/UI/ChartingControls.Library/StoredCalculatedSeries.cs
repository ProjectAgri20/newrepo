using System;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// A set of calculated series that is persisted in user settings.
    /// </summary>
    [Serializable]
    public class StoredCalculatedSeries
    {
        private Collection<CalculatedSeries> _series = new Collection<CalculatedSeries>();

        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        /// <value>
        /// The session id.
        /// </value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the graph.
        /// </summary>
        /// <value>
        /// The name of the graph.
        /// </value>
        public string GraphName { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public Collection<CalculatedSeries> Series
        {
            get { return _series; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredCalculatedSeries"/> class.
        /// </summary>
        [Obsolete("This constructor is for serialization purposes only and should not be used.")]
        public StoredCalculatedSeries()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredCalculatedSeries"/> class.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="graphName">Name of the graph.</param>
        public StoredCalculatedSeries(string sessionId, string graphName)
        {
            SessionId = sessionId;
            GraphName = graphName;
        }
    }
}
