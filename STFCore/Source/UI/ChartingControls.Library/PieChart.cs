using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Windows.Forms.DataVisualization.Charting;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Basic PieChart Control.
    /// </summary>
    public partial class PieChart : UserControl
    {
        private const string _available = "Available";
        private const string _inUse = "In Use";
        private const int _labelStyleThreshold = 40;
        
        /// <summary>
        /// Constructs a new instance of <see cref="PieChart"/> control.
        /// </summary>
        public PieChart()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Text that describes the display of the Pie Chart.
        /// </summary>
        public string Title 
        {
            get { return Chart.Titles[0].Text; }
            set { Chart.Titles[0].Text = value; } 
        }

        /// <summary>
        /// Connection String for the data source
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The SQL command used to populate the chart.
        /// </summary>
        public string SqlCommand { get; set; }

        /// <summary>
        /// The <see cref="ColorWheel"/> instance used to set the color scheme
        /// </summary>
        public ColorWheel ColorWheel { get; set; }

        /// <summary>
        /// The key/value color pairing of colors already used.
        /// </summary>
        public Dictionary<string, Color> Colors { get; set; }

        /// <summary>
        /// Refresh the data in the chart.
        /// </summary>
        public virtual void RefreshChart()
        {
            // Clear out all the old data values
            Series series = Chart.Series[0];
            series.Points.Clear();

            // Create the stored procedure defined by the virtual property
            using (SqlAdapter adapter = new SqlAdapter(ConnectionString))
            {
                DbDataReader reader = adapter.ExecuteReader(SqlCommand);

                string key = string.Empty;
                while (reader.Read())
                {
                    // Get key and value
                    key = (string)reader["Value"];
                    int count = (int)reader["Count"];

                    // Create the point
                    int addedIndex = series.Points.AddY(count);
                    DataPoint point = series.Points[addedIndex];

                    // Set key and legend text
                    //point.AxisLabel = key;
                    point.Label = BuildPointLabel(count, key);
                    point.LegendText = BuildLegendText(key);
                    if (Colors != null)
                    {
                        point.Color = GetNextColor(key);
                    }

                    // Set the "Available" ForeColor to white since it will be on dark green background
                    if (key == _available && count > _labelStyleThreshold)
                    {
                        point.LabelForeColor = Color.WhiteSmoke;
                    }

                    // If the slice is small, set the label outside
                    if (count < _labelStyleThreshold)
                    {
                        point["PieLabelStyle"] = "Outside";
                    }
                }
            }

            // Redraw the chart.
            Chart.Invalidate();
        }

        /// <summary>
        /// Returns the value for the passed-in display text.
        /// </summary>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public double GetValue(string displayText)
        {
            Series series = Chart.Series[0];
            DataPoint point = series.Points.FirstOrDefault(p => p.AxisLabel == displayText);

            if (point != null && point.YValues.Length > 0)
            {
                return point.YValues[0];
            }

            return 0.0;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            Chart.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the next color for the passed-in key value.
        /// If a color has already been used for the passed-in key value, that same color
        /// is returned.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A new color if this key has not been used before.</returns>
        protected virtual Color GetNextColor(string key)
        {
            if (Colors.Keys.Any(k => k == key))
            {
                return Colors[key];
            }

            Color nextColor = ColorWheel.NextColor(ColorMoods.Failure);
            Colors.Add(key, nextColor);
            return nextColor;
        }

        /// <summary>
        /// Builds a label for a data point, different from the legend text.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        protected virtual string BuildPointLabel(int count, string defaultText)
        {
            return "{0} ({1})".FormatWith(defaultText, count);
        }
        
        /// <summary>
        /// Builds the Legend text for a data point.
        /// </summary>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        protected virtual string BuildLegendText(string defaultText)
        {
            if (defaultText == _inUse)
            {
                return "{0} - No Session".FormatWith(defaultText);
            }

            return defaultText;
        }
    }
}
