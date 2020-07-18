using System;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using HP.ScalableTest.Core.DataLog;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Graph that shows activities over time, grouped by type.
    /// </summary>
    internal class ActivityTypeTimeGraph : TimeGraphBase
    {
        /// <summary>
        /// Gets or sets the synchronized minimum X value.
        /// </summary>
        protected static double SynchronizedMin { get; set; }

        /// <summary>
        /// Gets or sets the synchronized maximum X value.
        /// </summary>
        protected static double SynchronizedMax { get; set; }

        /// <summary>
        /// Gets the name of the graph.
        /// </summary>
        public override string GraphName
        {
            get { return "Activity Types Per Minute"; }
        }

        /// <summary>
        /// Gets the fields used to group session activities.
        /// </summary>
        protected override SessionActivityGroupFields GroupFields
        {
            get { return SessionActivityGroupFields.Status | SessionActivityGroupFields.ActivityType; }
        }

        /// <summary>
        /// Defines the Key to describe unique data points for this graph.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected override string CreateKey(SessionActivityGroupKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return $"{key.ActivityType} {key.Status.ToUpperInvariant()}";
        }

        /// <summary>
        /// Sets the bounds.
        /// </summary>
        protected override void SetBounds()
        {
            base.SetBounds();

            // Store the min and max values for inheriting classes that want to synchronize
            SynchronizedMin = AxisX.Minimum;
            SynchronizedMax = AxisX.Maximum;
        }
    }
}
