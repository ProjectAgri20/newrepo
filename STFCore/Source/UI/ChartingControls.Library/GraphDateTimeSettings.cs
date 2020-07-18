using System;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Contains date/time settings for the X-axis of a time-based graph.
    /// </summary>
    [Serializable]
    public class GraphDateTimeSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphDateTimeSettings"/> class.
        /// </summary>
        public GraphDateTimeSettings()
        {
            ShowDate = true;
            Use24Hour = true;
            ShowAMPM = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphDateTimeSettings"/> class.
        /// </summary>
        /// <param name="graphType">Type of the graph.</param>
        internal GraphDateTimeSettings(string graphType)
            : this()
        {
            GraphType = graphType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphDateTimeSettings"/> class.
        /// </summary>
        /// <param name="graphType">Type of the graph.</param>
        internal GraphDateTimeSettings(Type graphType)
            : this()
        {
            GraphType = graphType.Name.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphDateTimeSettings"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        internal GraphDateTimeSettings(TimeGraphBase graph)
            : this()
        {
            GraphType = graph.GetType().Name.ToString();
        }

        /// <summary>
        /// Gets or sets the type of the graph.
        /// </summary>
        /// <value>
        /// The type of the graph.
        /// </value>
        public string GraphType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show date.
        /// </summary>
        public bool ShowDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use 24 hour time.
        /// </summary>
        public bool Use24Hour { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show AM/PM.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "AMPM")]
        public bool ShowAMPM { get; set; }

        /// <summary>
        /// Gets the date time format.
        /// </summary>
        public string DateTimeFormat
        {
            get
            {
                return (ShowDate ? "MM/dd/yy " : string.Empty)
                     + (Use24Hour ? "H:mm" : "h:mm")
                     + (ShowAMPM && !Use24Hour ? " tt" : string.Empty);
            }
        }
    }
}
