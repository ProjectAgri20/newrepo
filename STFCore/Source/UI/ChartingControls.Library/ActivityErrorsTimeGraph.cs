using System;
using System.Data;
using HP.ScalableTest.Core.DataLog;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Graph that shows activity errors over time.
    /// </summary>
    internal class ActivityErrorsTimeGraph : ActivityTypeTimeGraph
    {
        /// <summary>
        /// Gets the name of the graph.
        /// </summary>
        public override string GraphName
        {
            get { return "Errors Per Minute"; }
        }

        /// <summary>
        /// Gets the fields used to group session activities.
        /// </summary>
        protected override SessionActivityGroupFields GroupFields
        {
            get { return SessionActivityGroupFields.Status | SessionActivityGroupFields.ResultCategory; }
        }

        protected override string CreateKey(SessionActivityGroupKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (string.IsNullOrEmpty(key.ResultCategory))
            {
                return null;
            }

            return $"[{key.Status.ToUpperInvariant()}] {key.ResultCategory}";
        }

        /// <summary>
        /// Sets the bounds.
        /// </summary>
        protected override void SetBounds()
        {
            AxisX.Minimum = SynchronizedMin;
            AxisX.Maximum = SynchronizedMax;
        }
    }
}
