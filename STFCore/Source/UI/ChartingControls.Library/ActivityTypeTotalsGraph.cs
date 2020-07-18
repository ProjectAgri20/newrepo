using System;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Graph that shows total activity results grouped by type.
    /// </summary>
    internal class ActivityTypeTotalsGraph : BarGraphBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityTypeTotalsGraph"/> class.
        /// </summary>
        public ActivityTypeTotalsGraph()
        {
            this.DataPointClick += new EventHandler<DataPointClickEventArgs>(ActivityTotalsGraph_DataPointClick);
        }

        /// <summary>
        /// Gets the name of the graph.
        /// </summary>
        public override string GraphName
        {
            get { return "Activity Totals"; }
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
        /// <param name="record">The record.</param>
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

            string metadataType = key.ActivityType;
            string updateType = key.Status;

            point.SetCustomProperty("ActivityType", metadataType);
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

        private void ActivityTotalsGraph_DataPointClick(object sender, DataPointClickEventArgs e)
        {
            SortableBindingList<ErrorCount> errorList = new SortableBindingList<ErrorCount>();
            using (DataLogContext context = DbConnect.DataLogContext())
            {
                foreach (SessionActivityCount result in GetErrorList(context, e.Point))
                {
                    errorList.Add(new ErrorCount(result.Key.ResultMessage, result.Count));
                }
            }

            if (errorList.Count > 0 && !(errorList.Count == 1 && string.IsNullOrEmpty(errorList[0].Error)))
            {
                using (ErrorListForm form = new ErrorListForm(errorList, e.Point.AxisLabel))
                {
                    form.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// Gets the error list.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        protected virtual IQueryable<SessionActivityCount> GetErrorList(DataLogContext context, DataPoint point)
        {
            if (point == null)
            {
                throw new ArgumentNullException("point");
            }

            string activityType = point.GetCustomProperty("ActivityType");
            string updateType = point.GetCustomProperty("UpdateType");

            return context.SessionData(SessionId)
                          .ActivityCounts(GroupFields | SessionActivityGroupFields.ResultMessage)
                          .Where(n => n.Key.ActivityType == activityType && n.Key.Status == updateType);
        }
    }
}
