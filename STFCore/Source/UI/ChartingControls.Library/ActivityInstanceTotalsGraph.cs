using System;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using HP.ScalableTest.Core.DataLog;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Graph that shows total activity results grouped by instance.
    /// </summary>
    internal class ActivityInstanceTotalsGraph : ActivityTypeTotalsGraph
    {
        /// <summary>
        /// Gets the name of the graph.
        /// </summary>
        public override string GraphName
        {
            get { return "Activity Instance Totals"; }
        }

        /// <summary>
        /// Gets the fields used to group session activities.
        /// </summary>
        protected override SessionActivityGroupFields GroupFields
        {
            get { return base.GroupFields | SessionActivityGroupFields.ActivityName; }
        }

        /// <summary>
        /// Defines the Key to describe unique data points for this graph.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        protected override string CreateKey(SessionActivityGroupKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return $"{key.ActivityName} {key.Status.ToUpperInvariant()}";
        }

        /// <summary>
        /// Adds custom options to the point, such as color or properties.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="point">The point.</param>
        protected override void CustomizePoint(SessionActivityGroupKey key, DataPoint point)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (point == null)
            {
                throw new ArgumentNullException("point");
            }

            base.CustomizePoint(key, point);
            point.SetCustomProperty("Description", key.ActivityName);
        }

        /// <summary>
        /// Gets the error list.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        protected override IQueryable<SessionActivityCount> GetErrorList(DataLogContext context, DataPoint point)
        {
            if (point == null)
            {
                throw new ArgumentNullException("point");
            }

            string description = point.GetCustomProperty("Description");
            string updateType = point.GetCustomProperty("UpdateType");

            return context.SessionData(SessionId)
                          .ActivityCounts(GroupFields | SessionActivityGroupFields.ResultMessage)
                          .Where(n => n.Key.ActivityName == description && n.Key.Status == updateType);
        }
    }
}
