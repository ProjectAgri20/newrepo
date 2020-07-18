using System;
using System.Data;
using HP.ScalableTest.Core.DataLog;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Graph that shows activities over time, grouped by instance.
    /// </summary>
    internal class ActivityInstanceTimeGraph : ActivityTypeTimeGraph
    {
        /// <summary>
        /// Gets the name of the graph.
        /// </summary>
        public override string GraphName
        {
            get { return "Activity Instances Per Minute"; }
        }

        /// <summary>
        /// Gets the fields used to group session activities.
        /// </summary>
        protected override SessionActivityGroupFields GroupFields
        {
            get { return SessionActivityGroupFields.Status | SessionActivityGroupFields.ActivityName; }
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

            return $"{key.ActivityName} {key.Status.ToUpperInvariant()}";
        }
    }
}
