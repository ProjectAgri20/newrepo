using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Event data for clicking on a chart data point.
    /// </summary>
    internal class DataPointClickEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the data point that was clicked.
        /// </summary>
        public DataPoint Point { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointClickEventArgs"/> class.
        /// </summary>
        /// <param name="point">The point.</param>
        public DataPointClickEventArgs(DataPoint point)
        {
            Point = point;
        }
    }
}
