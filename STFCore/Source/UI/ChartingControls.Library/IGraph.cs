using System;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.Charting
{
    internal interface IGraph
    {
        /// <summary>
        /// Gets the name of the graph.
        /// </summary>
        /// <value>
        /// The name of the graph.
        /// </value>
        string GraphName { get; }

        /// <summary>
        /// Occurs when there is a graphing status update.
        /// </summary>
        event EventHandler<StatusChangedEventArgs> StatusUpdate;

        /// <summary>
        /// Refreshes the graph, displaying all available data points.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="scenarioName">Name of the scenario.</param>
        void RefreshGraph(string sessionId, string scenarioName, bool applyFilters = false);

        /// <summary>
        /// Saves the image to clipboard.
        /// </summary>
        void SaveImageToClipboard();
    }
}
