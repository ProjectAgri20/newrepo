using System;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using HP.ScalableTest.Core.DataLog;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Graph that shows total activity errors grouped by message.
    /// </summary>
    internal class ActivityErrorTotalsGraph : ActivityTypeTotalsGraph
    {
        /// <summary>
        /// Gets the name of the graph.
        /// </summary>
        public override string GraphName
        {
            get { return "Error Totals"; }
        }

        /// <summary>
        /// Gets the fields used to group session activities.
        /// </summary>
        protected override SessionActivityGroupFields GroupFields
        {
            get { return SessionActivityGroupFields.Status | SessionActivityGroupFields.ResultCategory; }
        }

        /// <summary>
        /// Creates the key.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
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

            string category = key.ResultCategory;
            string updateType = key.Status;

            point.SetCustomProperty("Category", category);
            point.SetCustomProperty("UpdateType", updateType);

            switch (updateType)
            {
                case "Failed":
                case "Error":
                    point.Color = ColorWheel.NextColor(ColorMoods.Failure, ColorMoods.Neutral);
                    break;

                case "Passed":
                    point.Color = ColorWheel.NextColor(ColorMoods.Success, ColorMoods.Neutral);
                    break;

                default:
                    point.Color = ColorWheel.NextColor(ColorMoods.Neutral);
                    break;
            }
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

            string category = point.GetCustomProperty("Category");
            string updateType = point.GetCustomProperty("UpdateType");

            return context.SessionData(SessionId)
                          .ActivityCounts(GroupFields | SessionActivityGroupFields.ResultMessage)
                          .Where(n => n.Key.ResultCategory == category && n.Key.Status == updateType);
        }
    }
}
