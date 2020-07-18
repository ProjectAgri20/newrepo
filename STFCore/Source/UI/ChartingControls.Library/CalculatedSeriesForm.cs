using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Form for configuring calculated graph series.
    /// </summary>
    internal partial class CalculatedSeriesForm : Form
    {
        private Collection<CalculatedSeries> _calculatedSeries = new Collection<CalculatedSeries>();

        /// <summary>
        /// Gets the calculated series.
        /// </summary>
        public IEnumerable<CalculatedSeries> CalculatedSeries
        {
            get { return _calculatedSeries; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedSeriesForm"/> class.
        /// </summary>
        /// <param name="calculatedSeries">The calculated series.</param>
        /// <param name="seriesNames">The series names.</param>
        public CalculatedSeriesForm(IEnumerable<CalculatedSeries> calculatedSeries, IEnumerable<string> seriesNames)
        {
            if (calculatedSeries == null)
            {
                throw new ArgumentNullException("calculatedSeries");
            }

            if (seriesNames == null)
            {
                throw new ArgumentNullException("seriesNames");
            }

            InitializeComponent();

            foreach (CalculatedSeries series in calculatedSeries)
            {
                _calculatedSeries.Add(series);
                calculatedSeries_ListBox.Items.Add(series.Name);
            }

            foreach (string name in seriesNames)
            {
                includedSeries_ListBox.Items.Add(name);
            }
        }

        private void calculatedSeries_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabledControls();

            if (calculatedSeries_ListBox.SelectedItem != null)
            {
                string selectedName = calculatedSeries_ListBox.SelectedItem.ToString();
                CalculatedSeries calculatedSeries = _calculatedSeries.First(n => n.Name == selectedName);
                for (int i = 0; i < includedSeries_ListBox.Items.Count; i++)
                {
                    string itemName = includedSeries_ListBox.Items[i].ToString();
                    includedSeries_ListBox.SetItemChecked(i, calculatedSeries.IsIncluded(itemName));
                }
            }
        }

        private void UpdateEnabledControls()
        {
            if (calculatedSeries_ListBox.SelectedItem == null)
            {
                remove_Button.Enabled = false;
                includedSeries_ListBox.Enabled = false;
                foreach (int i in includedSeries_ListBox.CheckedIndices)
                {
                    includedSeries_ListBox.SetItemChecked(i, false);
                }
            }
            else
            {
                remove_Button.Enabled = true;
                includedSeries_ListBox.Enabled = true;
            }
        }

        private void includedSeries_ListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (calculatedSeries_ListBox.SelectedItem == null)
            {
                return;
            }

            string selectedName = calculatedSeries_ListBox.SelectedItem.ToString();
            CalculatedSeries calculatedSeries = _calculatedSeries.First(n => n.Name == selectedName);
            string checkedName = includedSeries_ListBox.Items[e.Index].ToString();
            if (e.NewValue == CheckState.Checked)
            {
                calculatedSeries.Include(checkedName);
            }
            else
            {
                calculatedSeries.Exclude(checkedName);
            }
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            string selectedName = InputDialog.Show("Enter a name for the new calculated series:", "New Calculated Series", "Total");
            if (selectedName == null)
            {
                // User cancelled
                return;
            }
            
            if (string.IsNullOrWhiteSpace(selectedName) || _calculatedSeries.Any(n => n.Name == selectedName))
            {
                MessageBox.Show("Invalid name.", "Invalid Name", MessageBoxButtons.OK);
            }
            else
            {
                CalculatedSeries series = new CalculatedSeries(selectedName);
                _calculatedSeries.Add(series);
                calculatedSeries_ListBox.SelectedIndex = calculatedSeries_ListBox.Items.Add(series.Name);
            }

            UpdateEnabledControls();
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            if (calculatedSeries_ListBox.SelectedItem != null)
            {
                string selectedName = calculatedSeries_ListBox.SelectedItem.ToString();
                CalculatedSeries series = _calculatedSeries.First(n => n.Name == selectedName);
                _calculatedSeries.Remove(series);
                calculatedSeries_ListBox.Items.Remove(calculatedSeries_ListBox.SelectedItem);
            }

            UpdateEnabledControls();
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            foreach (CalculatedSeries series in _calculatedSeries)
            {
                if (series.IncludedSeries.Count == 0)
                {
                    MessageBox.Show("Please ensure all calculated series have at least one series selected.",
                                    "Validation Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
