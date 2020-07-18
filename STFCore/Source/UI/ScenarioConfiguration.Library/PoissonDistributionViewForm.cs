using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using Telerik.Charting;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    public partial class PoissonDistributionViewForm : Form
    {
        private readonly int _threadCount = 1;
        private readonly TimeSpan _duration = TimeSpan.Zero;
        private BarSeries _barSeries = null;

        public PoissonDistributionViewForm(int threadCount, TimeSpan duration)
        {
            InitializeComponent();

            _threadCount = threadCount;
            _duration = duration;
        }

        private void Display()
        {
            Cursor = Cursors.WaitCursor;

            var distribution = new PoissonDistribution().GetNormalizedValues(_threadCount);

            // Get the time delta between each sample point.
            var delta = TimeSpan.FromTicks(_duration.Ticks / (distribution.Count() - 1));

            _barSeries.DataPoints.Clear();

            for (int i = 0; i < distribution.Count(); i++)
            {
                var time = TimeSpan.FromSeconds(delta.TotalSeconds * i);
                var dataPoint = new CategoricalDataPoint(distribution.ElementAt(i), time.ToString(@"hh\:mm\:ss"));
                _barSeries.DataPoints.Add(dataPoint);
            }

            Cursor = Cursors.Default;
        }

        private void PoissonDistributionViewForm_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _barSeries = new BarSeries();
            poisson_ChartView.Series.Add(_barSeries);

            CategoricalAxis xAxis = poisson_ChartView.Axes[0] as CategoricalAxis;
            xAxis.PlotMode = AxisPlotMode.OnTicksPadded;
            xAxis.LabelFitMode = AxisLabelFitMode.Rotate;
            xAxis.LabelRotationAngle = 310;
            xAxis.LabelOffset = 0;

            LinearAxis yAxis = poisson_ChartView.Axes[1] as LinearAxis;
            yAxis.Title = "Threads";

            Display();
        }

        private class PoissonData
        {
            public TimeSpan Delta { get; set; }
            public int ThreadCount { get; set; }
        }

        private void refresh_Button_Click(object sender, EventArgs e)
        {
            Display();
        }
    }
}
