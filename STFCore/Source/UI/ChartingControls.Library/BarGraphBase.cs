using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Base class for producing a bar graph.
    /// </summary>
    internal partial class BarGraphBase : UserControl, IGraph
    {
        private ColorWheel _colorWheel = new ColorWheel();
        private string _sessionId = string.Empty;
        private Collection<string> _ignoredKeys = new Collection<string>();
        private const int LARGE_SET = 10;
        private Collection<SubstringFilter> _substringFilters = new Collection<SubstringFilter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BarGraphBase"/> class.
        /// </summary>
        protected BarGraphBase()
        {
            InitializeComponent();
            Chart.Titles[0].Text = GraphName;
        }

        /// <summary>
        /// Occurs when there is a graphing status update.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        /// <summary>
        /// Occurs when a data point is clicked.
        /// </summary>
        protected event EventHandler<DataPointClickEventArgs> DataPointClick;

        /// <summary>
        /// Gets the session id.
        /// </summary>
        protected string SessionId
        {
            get { return _sessionId; }
        }

        /// <summary>
        /// Gets the color wheel.
        /// </summary>
        protected ColorWheel ColorWheel
        {
            get { return _colorWheel; }
        }

        /// <summary>
        /// Gets the name of the graph.
        /// </summary>
        public virtual string GraphName { get; }

        /// <summary>
        /// Gets the fields used to group session activities.
        /// </summary>
        protected virtual SessionActivityGroupFields GroupFields { get; }

        /// <summary>
        /// Defines the Key to describe unique data points for this graph.
        /// </summary>
        /// <param name="record">The record.</param>
        protected virtual string CreateKey(SessionActivityGroupKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("groupKey");
            }

            return "BarGraphBase: " + key.Status;
        }

        /// <summary>
        /// Adds custom options to the point, such as color or properties.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="point">The point.</param>
        protected virtual void CustomizePoint(SessionActivityGroupKey key, DataPoint point)
        {
            // Do nothing
        }

        /// <summary>
        /// Refreshes the graph, displaying all available data points.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="scenarioName">Name of the scenario.</param>
        public void RefreshGraph(string sessionId, string scenarioName, bool applyFilters = false)
        {
            bool applyDefaultFilters = applyFilters;

            if (_sessionId != sessionId)
            {
                SetNewSession(sessionId, scenarioName);
                applyDefaultFilters = true;
            }
            else if (applyDefaultFilters)
            {
                ClearFilters();
            }

            RefreshGraph(applyDefaultFilters);
        }

        private void RefreshGraph(bool applyDefaultFilters = false)
        {
            OnStatusUpdate("Loading graph '{0}'...".FormatWith(GraphName));
            Series series = Chart.Series[0];

            // If the number of data points does not change, the y-axis maximum will not be updated.
            // To get around this, we can force a graph refresh, but it will cause a "flicker"
            // to occur, so only do this if the maximum value changes (i.e. we need a new scale).
            double oldMax = double.MaxValue; // MaxValue prevents unnecessary refresh if there are no points initially
            if (series.Points.Count > 0)
            {
                oldMax = series.Points.FindMaxByValue().YValues[0];
            }

            // Zero out all the old data values - this makes sure there will be no stale data
            foreach (DataPoint point in series.Points)
            {
                point.SetValueY(0);
            }

            // Load the points for each value
            LoadSeriesPoints(series);
            if (series.Points.Count > 0 && series.Points.FindMaxByValue().YValues[0] > oldMax)
            {
                // Force the y-axis max to be recalculated
                series.Points.Add(0);
                Application.DoEvents();
            }
            RemoveEmptyValues(series);

            // Apply the default filters, if requested
            if (applyDefaultFilters)
            {
                SetDefaultFilters(series);
            }

            // Reset any zooming
            ResetZoom();

            // Redraw the chart.
            Chart.Invalidate();

            OnStatusUpdate(GraphName + " loaded.");
        }

        private void zoomX_radTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (zoomX_radTrackBar.Value == 1)
            {
                Chart.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            }
            else
            {
                ChartArea area = Chart.ChartAreas[0];
                double diff = Math.Round(area.AxisX.Maximum / zoomX_radTrackBar.Value);
                double start = area.AxisX.ScaleView.Position;

                if (Double.IsNaN(start))
                {
                    start = 0.0;
                }

                if (start == 0.0 && (diff == 0.0 || diff == 1.0)) // Bug fix, labels disappear when zooming with certain settings
                {
                    start = 1.0;
                }

                if (diff == 0.0)
                {
                    diff = 1.0;
                }

                double end = start + diff;
                if (end > area.AxisX.Maximum)
                {
                    end = area.AxisX.Maximum;
                    start = end - diff;
                }

                ZoomX(start, end);
            }
        }

        private void zoomY_radTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (zoomY_radTrackBar.Value == 1)
            {
                Chart.ChartAreas[0].AxisY.ScaleView.ZoomReset();
            }
            else
            {
                ChartArea area = Chart.ChartAreas[0];
                double diff = Math.Round(area.AxisY.Maximum / zoomY_radTrackBar.Value);
                double start = area.AxisY.ScaleView.Position;

                if (Double.IsNaN(start))
                {
                    start = 0;
                }

                if (start == 0.0 && (diff == 0.0 || diff == 1.0)) // Bug fix, labels disappear when zooming with certain settings
                {
                    start = 1.0;
                }

                if (diff == 0.0)
                {
                    diff = 1.0;
                }

                double end = start + diff;
                if (end > area.AxisY.Maximum)
                {
                    end = area.AxisY.Maximum;
                    start = end - diff;
                }

                ZoomY(start, end);
            }
        }

        private void ZoomX(double start, double end)
        {
            Chart.ChartAreas[0].AxisX.ScaleView.Zoom(start, end);
        }

        private void ZoomY(double start, double end)
        {
            Chart.ChartAreas[0].AxisY.ScaleView.Zoom(start, end);
        }

        private void ResetZoom()
        {
            zoomX_radTrackBar.Value = 1;
            zoomY_radTrackBar.Value = 1;
            Chart.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            Chart.ChartAreas[0].AxisY.ScaleView.ZoomReset();
        }

        private void LoadSeriesPoints(Series series)
        {
            using (DataLogContext context = DbConnect.DataLogContext())
            {
                var counts = context.SessionData(_sessionId).ActivityCounts(GroupFields).ToList();
                foreach (var result in counts.OrderByDescending(n => CreateKey(n.Key)))
                {
                    string keyText = CreateKey(result.Key);

                    // If key is empty, skip it
                    if (string.IsNullOrEmpty(keyText))
                    {
                        continue;
                    }

                    // If this is one of our ignored keys, skip it
                    if (_substringFilters.Any(s => keyText.Contains(s.Substring, StringComparison.OrdinalIgnoreCase)) || _ignoredKeys.Contains(keyText))
                    {
                        continue;
                    }

                    // If the point already exists, just update it
                    DataPoint point = series.Points.FirstOrDefault(n => n.AxisLabel == keyText);
                    if (point != null)
                    {
                        point.SetValueY(result.Count);
                    }
                    else
                    {
                        // Create the point
                        int addedIndex = series.Points.AddY(result.Count);
                        point = series.Points[addedIndex];

                        // Add key and custom data
                        point.AxisLabel = keyText;
                        CustomizePoint(result.Key, point);
                    }
                }
            }
        }

        private static void RemoveEmptyValues(Series series)
        {
            // Remove zero-valued points
            int i = 0;
            while (i < series.Points.Count)
            {
                if (series.Points[i].YValues[0] == 0)
                {
                    series.Points.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        private void SetDefaultFilters(Series series)
        {
            // For each point that should be excluded, set the value to 0
            foreach (DataPoint point in series.Points)
            {
                bool isComplete = point.AxisLabel.Contains("complete", StringComparison.OrdinalIgnoreCase);
                bool isFailed = point.AxisLabel.Contains("failed", StringComparison.OrdinalIgnoreCase);
                bool isSkipped = point.AxisLabel.Contains("skipped", StringComparison.OrdinalIgnoreCase);
                bool isError = point.AxisLabel.Contains("error", StringComparison.OrdinalIgnoreCase);
                bool isOther = !(isComplete || isFailed || isSkipped);

                bool filterSeries = false;
                filterSeries |= (isComplete && !Properties.Settings.Default.DisplayCompleted);
                filterSeries |= (isFailed && !Properties.Settings.Default.DisplayFailed);
                filterSeries |= (isSkipped && !Properties.Settings.Default.DisplaySkipped);
                filterSeries |= (isError && !Properties.Settings.Default.DisplayError);
                filterSeries |= (isOther && !Properties.Settings.Default.DisplayOthers);

                if (filterSeries)
                {
                    point.YValues[0] = 0;
                    _ignoredKeys.Add(point.AxisLabel);
                }
            }

            RemoveEmptyValues(series);
            UpdateFilterStatus();
        }

        private void SetNewSession(string sessionId, string scenarioName)
        {
            // Update the stored session id and title
            _sessionId = sessionId;
            SetTitle(sessionId, scenarioName);

            // Disable the timer
            refresh_Timer.Enabled = false;
            ClearChecks(autoRefresh_ToolStripMenuItem);

            // Clear points and reset the filter
            Chart.Series[0].Points.Clear();
            ClearFilters();
            _colorWheel.Reset();
        }

        private void ClearFilters()
        {
            _ignoredKeys.Clear();
            _substringFilters.Clear();
        }

        protected virtual void SetTitle(string sessionId, string scenarioName)
        {
            Chart.Titles[0].Text = GraphName
                + (string.IsNullOrEmpty(sessionId) ? string.Empty : " - " + sessionId)
                + (string.IsNullOrEmpty(scenarioName) ? string.Empty : " - " + scenarioName);
        }

        /// <summary>
        /// Called when there is a graphing status update.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void OnStatusUpdate(string status)
        {
            if (StatusUpdate != null)
            {
                StatusUpdate(this, new StatusChangedEventArgs(status));
            }
        }

        /// <summary>
        /// Called when a data point is clicked.
        /// </summary>
        /// <param name="point">The point.</param>
        private void OnDataPointClick(DataPoint point)
        {
            if (DataPointClick != null)
            {
                DataPointClick(this, new DataPointClickEventArgs(point));
            }
        }

        private void Chart_MouseDown(object sender, MouseEventArgs e)
        {
            // Ignore everything but left clicks
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            DataPoint point;
            Series series = Chart.Series[0];

            // Determine where the user has clicked
            HitTestResult[] results = Chart.HitTest(e.X, e.Y, false, new ChartElementType[] {
                                                                            ChartElementType.DataPoint,
                                                                            ChartElementType.AxisLabels,
                                                                            ChartElementType.ScrollBarLargeDecrement,
                                                                            ChartElementType.ScrollBarLargeIncrement,
                                                                            ChartElementType.ScrollBarSmallDecrement,
                                                                            ChartElementType.ScrollBarSmallIncrement,
                                                                            ChartElementType.ScrollBarThumbTracker
            });

            // If the user clicked on a scrollbar, don't do anything else
            ChartElementType[] scrollBarTypes = new ChartElementType[]
            {
                ChartElementType.ScrollBarLargeDecrement,
                ChartElementType.ScrollBarLargeIncrement,
                ChartElementType.ScrollBarSmallDecrement,
                ChartElementType.ScrollBarSmallIncrement,
                ChartElementType.ScrollBarThumbTracker
            };
            if (!results.Any(r => scrollBarTypes.Contains(r.ChartElementType)))
            {
                HitTestResult dataPoint = results.FirstOrDefault(r => r.ChartElementType == ChartElementType.DataPoint);
                HitTestResult axisLabel = results.FirstOrDefault(r => r.ChartElementType == ChartElementType.AxisLabels);

                if (dataPoint != null)
                {
                    point = Chart.Series[0].Points[dataPoint.PointIndex];
                    OnDataPointClick(point);
                }
                else if (axisLabel != null)
                {
                    CustomLabel label = (CustomLabel)axisLabel.Object;
                    for (int i = 0; i < series.Points.Count; i++)
                    {
                        if (label.Text == series.Points[i].AxisLabel)
                        {
                            point = series.Points[i];
                            OnDataPointClick(point);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves the image to clipboard.
        /// </summary>
        public void SaveImageToClipboard()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Chart.SaveImage(stream, ChartImageFormat.Bmp);
                Bitmap bitmap = new Bitmap(stream);
                Clipboard.SetImage(bitmap);
            }
        }

        private void seriesFilter_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Collection<SeriesInfo> seriesList = new Collection<SeriesInfo>();

            // Build a list of data points
            foreach (DataPoint point in Chart.Series[0].Points)
            {
                seriesList.Add(new SeriesInfo(point.AxisLabel));
            }
            foreach (string key in _ignoredKeys)
            {
                seriesList.Add(new SeriesInfo(key, false));
            }

            // Show the form
            using (SeriesFilterForm form = new SeriesFilterForm(seriesList, _substringFilters))
            {
                form.ShowDialog(this);

                // Add the keys that are being ignored to the list
                ClearFilters();

                foreach (SeriesInfo series in form.SeriesList.Where(n => !n.Enabled))
                {
                    _ignoredKeys.Add(series.Key);
                }

                foreach (SubstringFilter filter in form.SubstringFilters)
                {
                    _substringFilters.Add(filter);
                }
            }

            // Refresh the graph to update the displayed series
            UpdateFilterStatus();
        }

        private void clearFilter_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear points and reset the filter
            ClearFilters();
            UpdateFilterStatus();
        }

        private void UpdateFilterStatus()
        {
            // See if anything was actually caught by the filter
            bool filterApplied = _ignoredKeys.Count > 0 || _substringFilters.Count > 0;
            clearFilter_ToolStripMenuItem.Enabled = filterApplied;
            seriesFilter_ToolStripMenuItem.Checked = filterApplied;

            _colorWheel.Reset();
            Chart.Series[0].Points.Clear();
            RefreshGraph();
        }

        private void refreshNone_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoRefresh_ToolStripMenuItem.Checked = false;
            refresh_Timer.Enabled = false;
        }

        private void refreshByMinute_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoRefresh_ToolStripMenuItem.Checked = true;

            // The sender item should have the tag set to the interval in minutes
            string tag = (sender as ToolStripMenuItem).Tag.ToString();
            int interval = int.Parse(tag, CultureInfo.InvariantCulture);

            // Enable the timer
            refresh_Timer.Enabled = false;
            refresh_Timer.Interval = interval * 60 * 1000;
            refresh_Timer.Enabled = true;
        }

        private void refresh_Timer_Tick(object sender, EventArgs e)
        {
            refresh_Timer.Enabled = false;
            RefreshGraph();
            refresh_Timer.Enabled = true;
        }

        private static void ClearChecks(ToolStripMenuItem menuItem)
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException("menuItem");
            }

            menuItem.Checked = false;
            foreach (ToolStripMenuItem item in menuItem.DropDownItems)
            {
                ClearChecks(item);
            }
        }

        private void zoom_radTrackBar_LabelFormatting(object sender, Telerik.WinControls.UI.LabelFormattingEventArgs e)
        {
            e.LabelElement.Text += "x";
        }
    }
}
