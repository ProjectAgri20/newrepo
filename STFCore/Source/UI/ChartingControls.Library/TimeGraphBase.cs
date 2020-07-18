using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Charting.Properties;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Base class for producing a graph of points over time.
    /// </summary>
    internal partial class TimeGraphBase : UserControl, IGraph
    {
        private ColorWheel _colorWheel = new ColorWheel();
        private string _sessionId = string.Empty;
        private static MemoryStream _graphTemplate;
        private int _window = 0;
        private List<CalculatedSeries> _calculatedSeries = new List<CalculatedSeries>();
        private List<SubstringFilter> _substringFilters = new List<SubstringFilter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeGraphBase"/> class.
        /// </summary>
        protected TimeGraphBase()
        {
            InitializeComponent();
            Chart.Titles[0].Text = GraphName;

            // Save a copy of the empty graph, so we can use the
            // series as a template
            if (_graphTemplate == null)
            {
                _graphTemplate = new MemoryStream();
                Chart.Serializer.Content = SerializationContents.All;
                Chart.Serializer.Save(_graphTemplate);
            }
            Chart.Series.Clear();

            // Load the Date/Time settings for this graph
            LoadDateTimeSettings();
        }

        /// <summary>
        /// Occurs when there is a graphing status update.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

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
        /// Gets the axis X.
        /// </summary>
        protected Axis AxisX
        {
            get { return Chart.ChartAreas[0].AxisX; }
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
        /// Defines the Key to describe unique data series for this graph.
        /// </summary>
        /// <param name="key">The key.</param>
        protected virtual string CreateKey(SessionActivityGroupKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return "TimeGraphBase: " + key.Status;
        }

        /// <summary>
        /// Adds custom options to the series, such as color or properties.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="series">The series.</param>
        protected virtual void CustomizeSeries(SessionActivityGroupKey key, Series series)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (series == null)
            {
                throw new ArgumentNullException("series");
            }

            string updateType = key.Status;

            switch (updateType)
            {
                case "Failed":
                case "Error":
                    series.Color = ColorWheel.NextColor(ColorMoods.Failure, ColorMoods.Neutral);
                    break;

                case "Passed":
                    series.Color = ColorWheel.NextColor(ColorMoods.Success, ColorMoods.Neutral);
                    break;

                default:
                    series.Color = ColorWheel.NextColor(ColorMoods.Neutral);
                    break;
            }
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

            RefreshGraph(applyDefaultFilters);
        }

        /// <summary>
        /// Refreshes the graph, displaying all available data points.
        /// </summary>
        private void RefreshGraph(bool applyDefaultFilters = false)
        {
            OnStatusUpdate("Loading graph '{0}'...".FormatWith(GraphName));

            // Clear all data points from the existing series
            foreach (Series series in Chart.Series)
            {
                series.Points.Clear();
            }

            // Load the points for all the series
            LoadSeriesPoints();
            LoadStoredCalculatedSeries();
            DisplayCalculatedSeries();
            RemoveEmptySeries();

            // Set series-specific information
            SetBounds();
            SetSeriesTypes();
            if (applyDefaultFilters)
            {
                SetDefaultFilters();
            }

            // Redraw the chart.
            Chart.Invalidate();
            OnStatusUpdate(GraphName + " loaded.");
        }

        private void LoadSeriesPoints()
        {
            using (DataLogContext context = DbConnect.DataLogContext())
            {
                var counts = context.SessionData(_sessionId).ActivityCounts(GroupFields | SessionActivityGroupFields.EndDateTime).ToList();
                foreach (var result in counts.OrderBy(n => CreateKey(n.Key)))
                {
                    string keyText = CreateKey(result.Key);

                    // If key is empty, skip it
                    if (string.IsNullOrEmpty(keyText))
                    {
                        continue;
                    }

                    // If this is one of our ignored keys, skip it
                    if (_substringFilters.Any(s => keyText.Contains(s.Substring, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    // If the series does not exist, create it
                    Series series = Chart.Series.FindByName(keyText);
                    if (series == null)
                    {
                        series = CreateSeries(keyText);
                        Chart.Series.Add(series);
                        CustomizeSeries(result.Key, series);
                    }

                    // Add the point
                    series.Points.AddXY(result.Key.EndDateTime?.LocalDateTime, result.Count);
                }
            }
        }

        private void DisplayCalculatedSeries()
        {
            foreach (CalculatedSeries calculatedSeries in _calculatedSeries)
            {
                // Find the calculated series, or create it if it does not exist
                Series series = Chart.Series.FindByName(calculatedSeries.Name);
                if (series == null)
                {
                    series = CreateSeries(calculatedSeries.Name);
                    Chart.Series.Add(series);
                    series.Color = ColorWheel.NextColor(ColorMoods.Neutral);
                }
                series.Points.Clear();

                // Add up all the points from the included series
                foreach (Series includedSeries in Chart.Series)
                {
                    if (calculatedSeries.IsIncluded(includedSeries.Name))
                    {
                        foreach (DataPoint point in includedSeries.Points)
                        {
                            DataPoint totalsPoint = series.Points.FirstOrDefault(n => n.XValue == point.XValue);
                            if (totalsPoint == null)
                            {
                                series.Points.AddXY(point.XValue, point.YValues[0]);
                            }
                            else
                            {
                                totalsPoint.YValues[0] += point.YValues[0];
                            }
                        }
                    }
                }

                series.Sort(PointSortOrder.Ascending, "X");
            }
        }

        private void LoadStoredCalculatedSeries()
        {
            // Pull the saved calculated series out of the user settings
            var allStoredSeries = LegacySerializer.DeserializeXml<Collection<StoredCalculatedSeries>>(Settings.Default.CalculatedSeries);
            StoredCalculatedSeries storedSeries = allStoredSeries.FirstOrDefault(n => n.SessionId == _sessionId && n.GraphName == this.GraphName);
            if (storedSeries != null)
            {
                _calculatedSeries.Clear();
                _calculatedSeries.AddRange(storedSeries.Series);
            }
        }

        private void StoreCalculatedSeries()
        {
            // Load the saved calculated series so we can update it
            var allStoredSeries = LegacySerializer.DeserializeXml<Collection<StoredCalculatedSeries>>(Settings.Default.CalculatedSeries);
            StoredCalculatedSeries storedSeries = allStoredSeries.FirstOrDefault(n => n.SessionId == _sessionId && n.GraphName == this.GraphName);
            if (storedSeries == null)
            {
                storedSeries = new StoredCalculatedSeries(_sessionId, this.GraphName);
                allStoredSeries.Add(storedSeries);
            }
            storedSeries.Series.Clear();
            _calculatedSeries.ForEach(n => storedSeries.Series.Add(n));
            Settings.Default.CalculatedSeries = LegacySerializer.SerializeXml(allStoredSeries).ToString();
            Settings.Default.Save();
        }

        private void RemoveEmptySeries()
        {
            int i = 0;
            while (i < Chart.Series.Count)
            {
                if (Chart.Series[i].Points.Count == 0)
                {
                    Chart.Series.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        private void SetNewSession(string sessionId, string scenarioName)
        {
            // Update the stored session id and title
            _sessionId = sessionId;
            SetTitle(sessionId, scenarioName);

            // Disable the timer
            refresh_Timer.Enabled = false;
            ClearChecks(autoRefresh_ToolStripMenuItem);
            refreshNone_ToolStripMenuItem.Checked = true;

            // Clear series and reset the filter/zoom
            Chart.Series.Clear();
            _calculatedSeries.Clear();
            _colorWheel.Reset();
            _substringFilters.Clear();
            AxisX.ScaleView.ZoomReset(0);
        }

        protected virtual void SetTitle(string sessionId, string scenarioName)
        {
            Chart.Titles[0].Text = GraphName
                + (string.IsNullOrEmpty(sessionId) ? string.Empty : " - " + sessionId)
                + (string.IsNullOrEmpty(scenarioName) ? string.Empty : " - " + scenarioName);
        }

        protected static Series CreateSeries(string key)
        {
            _graphTemplate.Seek(0, SeekOrigin.Begin);
            using (Chart temp = new Chart())
            {
                temp.Serializer.Load(_graphTemplate);
                Series series = temp.Series[0];
                series.Name = key;
                temp.Series.Remove(series);
                return series;
            }
        }

        protected virtual void SetBounds()
        {
            double minX = double.PositiveInfinity;
            double maxX = double.NegativeInfinity;

            foreach (Series series in Chart.Series)
            {
                minX = Math.Min(minX, series.Points.FindMinByValue("X").XValue);
                maxX = Math.Max(maxX, series.Points.FindMaxByValue("X").XValue);
            }

            AxisX.Minimum = minX;
            AxisX.Maximum = maxX;
        }

        private void SetSeriesTypes()
        {
            foreach (Series series in Chart.Series)
            {
                series.ToolTip = "#SERIESNAME";
                // Use FastLine for series with lots of points
                if (series.Points.Count > 200)
                {
                    series.ChartType = SeriesChartType.FastLine;
                }

                // If there is only one point, we must use a point chart type
                else if (series.Points.Count <= 1)
                {
                    series.ChartType = SeriesChartType.Point;
                }

                // If the largest value is less than 5, use a point chart type
                else if (series.Points.FindMaxByValue("Y1").YValues[0] <= 5)
                {
                    series.ChartType = SeriesChartType.Point;
                }

                // Default to a line chart type
                else
                {
                    series.ChartType = SeriesChartType.Line;
                }
            }
        }

        private void SetDefaultFilters()
        {
            foreach (Series series in Chart.Series)
            {
                if (series.Name.Contains("complete", StringComparison.OrdinalIgnoreCase))
                {
                    series.Enabled = Properties.Settings.Default.DisplayCompleted;
                }
                else if (series.Name.Contains("failed", StringComparison.OrdinalIgnoreCase))
                {
                    series.Enabled = Properties.Settings.Default.DisplayFailed;
                }
                else if (series.Name.Contains("skipped", StringComparison.OrdinalIgnoreCase))
                {
                    series.Enabled = Properties.Settings.Default.DisplaySkipped;
                }
                else if (series.Name.Contains("error", StringComparison.OrdinalIgnoreCase))
                {
                    series.Enabled = Properties.Settings.Default.DisplayError;
                }
                else
                {
                    series.Enabled = Properties.Settings.Default.DisplayOthers;
                }
            }
            UpdateFilterStatus();
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
            foreach (Series series in Chart.Series)
            {
                seriesList.Add(new SeriesInfo(series.Name, series.Enabled));
            }

            // Show the form
            using (SeriesFilterForm form = new SeriesFilterForm(seriesList, _substringFilters))
            {
                form.ShowDialog(this);

                // Apply the filter to the graph
                foreach (SeriesInfo info in form.SeriesList)
                {
                    Chart.Series.FindByName(info.Key).Enabled = info.Enabled;
                }

                _substringFilters.Clear();
                foreach (SubstringFilter filter in form.SubstringFilters)
                {
                    _substringFilters.Add(filter);
                }

                UpdateFilterStatus();
            }

            RefreshGraph();
        }

        private void clearFilter_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Reset the filter
            foreach (Series series in Chart.Series)
            {
                series.Enabled = true;
            }

            _substringFilters.Clear();

            UpdateFilterStatus();
            RefreshGraph();
        }

        private void UpdateFilterStatus()
        {
            // See if anything was actually caught by the filter
            bool filterApplied = Chart.Series.Any(n => !n.Enabled) || _substringFilters.Count > 0;
            clearFilter_ToolStripMenuItem.Enabled = filterApplied;
            seriesFilter_ToolStripMenuItem.Checked = filterApplied;
        }

        private void calculatedSeries_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Collection<string> seriesNames = new Collection<string>();
            foreach (Series series in Chart.Series)
            {
                if (_calculatedSeries.FirstOrDefault(n => n.Name == series.Name) == null)
                {
                    seriesNames.Add(series.Name);
                }
            }

            using (CalculatedSeriesForm form = new CalculatedSeriesForm(_calculatedSeries, seriesNames))
            {
                if (DialogResult.OK == form.ShowDialog(this))
                {
                    _calculatedSeries.Clear();
                    foreach (CalculatedSeries series in form.CalculatedSeries)
                    {
                        _calculatedSeries.Add(series);
                    }
                }
            }

            StoreCalculatedSeries();
            DisplayCalculatedSeries();
            RefreshGraph();
        }

        private void refreshNone_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearChecks(autoRefresh_ToolStripMenuItem);
            refreshNone_ToolStripMenuItem.Checked = true;
            refresh_Timer.Enabled = false;
        }

        private void refreshByMinute_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The sender item should have the tag set to the interval in minutes
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            int interval = int.Parse(clickedItem.Tag.ToString(), CultureInfo.InvariantCulture);

            // Update the checked items
            ClearChecks(autoRefresh_ToolStripMenuItem);
            autoRefresh_ToolStripMenuItem.Checked = true;
            clickedItem.Checked = true;

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

        private void windowMinutes_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The sender should have the tag set to the interval in minutes
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            _window = int.Parse(clickedItem.Tag.ToString(), CultureInfo.InvariantCulture);

            // Update the checked items
            ClearChecks(realTimeWindow_ToolStripMenuItem);
            realTimeWindow_ToolStripMenuItem.Checked = (_window != 0);
            clickedItem.Checked = true;

            // Refresh the graph
            Refresh();
        }

        private void legendOption_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear the checked items
            ClearChecks(legendPosition_ToolStripMenuItem);

            // Check the appropriate item
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            clickedItem.Checked = true;

            // Apply the selected setting
            if (clickedItem == legendOff_ToolStripMenuItem)
            {
                Chart.Legends[0].Enabled = false;
            }
            else if (clickedItem == legendBottom_ToolStripMenuItem)
            {
                Chart.Legends[0].Enabled = true;
                Chart.Legends[0].Docking = Docking.Bottom;
            }
            else if (clickedItem == legendRight_ToolStripMenuItem)
            {
                Chart.Legends[0].Enabled = true;
                Chart.Legends[0].Docking = Docking.Right;
            }
        }

        private void timeOptions_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Load the serialized settings list
            string serializedSettingsList = Properties.Settings.Default.DateFormats;
            var settingsList = LegacySerializer.DeserializeXml<Collection<GraphDateTimeSettings>>(serializedSettingsList);

            // Find the settings that apply to this graph
            GraphDateTimeSettings settings = settingsList.FirstOrDefault(n => n.GraphType == this.GetType().Name.ToString());
            if (settings == null)
            {
                settings = new GraphDateTimeSettings(this);
                settingsList.Add(settings);
            }

            settings.ShowDate = showDate_ToolStripMenuItem.Checked;
            settings.Use24Hour = use24HourTime_ToolStripMenuItem.Checked;
            settings.ShowAMPM = showAMPM_ToolStripMenuItem.Checked;

            ApplyDateTimeSettings(settings);
            Properties.Settings.Default.DateFormats = LegacySerializer.SerializeXml(settingsList).ToString();
            Properties.Settings.Default.Save();
        }

        private void LoadDateTimeSettings()
        {
            // Load the serialized settings list
            string serializedSettingsList = Properties.Settings.Default.DateFormats;
            var settingsList = LegacySerializer.DeserializeXml<Collection<GraphDateTimeSettings>>(serializedSettingsList);

            // Find the settings that apply to this graph
            GraphDateTimeSettings settings = settingsList.FirstOrDefault(n => n.GraphType == this.GetType().Name.ToString());
            if (settings == null)
            {
                settings = new GraphDateTimeSettings(this);
            }

            ApplyDateTimeSettings(settings);
        }

        private void ApplyDateTimeSettings(GraphDateTimeSettings settings)
        {
            showDate_ToolStripMenuItem.Checked = settings.ShowDate;
            use24HourTime_ToolStripMenuItem.Checked = settings.Use24Hour;
            showAMPM_ToolStripMenuItem.Checked = settings.ShowAMPM;
            showAMPM_ToolStripMenuItem.Enabled = !settings.Use24Hour;
            AxisX.LabelStyle.Format = settings.DateTimeFormat;
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
    }
}
