using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Form for filtering graph series.
    /// </summary>
    internal partial class SeriesFilterForm : Form
    {
        private SortableBindingList<SeriesInfo> _bindingList = new SortableBindingList<SeriesInfo>();
        private SortableBindingList<SubstringFilter> _filters = new SortableBindingList<SubstringFilter>();

        /// <summary>
        /// Gets the series list.
        /// </summary>
        public IEnumerable<SeriesInfo> SeriesList
        {
            get { return _bindingList; }
        }

        public IEnumerable<SubstringFilter> SubstringFilters
        {
            get { return _filters; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesFilterForm"/> class.
        /// </summary>
        /// <param name="seriesList">The series list.</param>
        public SeriesFilterForm(IEnumerable<SeriesInfo> seriesList, IEnumerable<SubstringFilter> substringFilters)
        {
            if (seriesList == null)
            {
                throw new ArgumentNullException("seriesList");
            }

            InitializeComponent();

            UserInterfaceStyler.Configure(series_radGridView, GridViewStyle.FullEdit);
            UserInterfaceStyler.Configure(hidden_radGridView, GridViewStyle.ReadOnly);

            foreach (SeriesInfo series in seriesList)
            {
                _bindingList.Add(series);
            }

            foreach (SubstringFilter filter in substringFilters)
            {
                _filters.Add(filter);
            }

            hidden_radGridView.DataSource = _filters;
            
            series_radGridView.DataSource = _bindingList;

            ApplySeriesFilters();
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(filter_textBox.Text))
            {
                _filters.Add(new SubstringFilter(filter_textBox.Text));
                filter_textBox.Clear();

                ApplySeriesFilters();
            }
        }

        private void ApplySeriesFilters()
        {
            // Disable data containing the substring
            foreach (var data in _bindingList)
            {
                if (data.Enabled)
                {
                    data.Enabled = IsFiltered(data);
                }
            }

            RefreshAll();
        }

        private void RefreshAll()
        {
            ForceRefreshGrid();
            SetReadOnlyRows();
        }

        private void ForceRefreshGrid()
        {
            series_radGridView.DataSource = null;
            series_radGridView.DataSource = _bindingList;
        }

        private void SetReadOnlyRows()
        {
            foreach (var row in series_radGridView.Rows)
            {
                row.Cells[0].ReadOnly = !IsFiltered((SeriesInfo)row.DataBoundItem);
            }
        }

        private bool IsFiltered(SeriesInfo info)
        {
            return !_filters.Any(f => info.Key.Contains(f.Substring, StringComparison.OrdinalIgnoreCase));
        }

        private void remove_button_Click(object sender, EventArgs e)
        {
            List<SubstringFilter> toRemove = new List<SubstringFilter>();
            foreach (var row in hidden_radGridView.SelectedRows)
            {
                toRemove.Add((SubstringFilter)row.DataBoundItem);
            }
            foreach (var filter in toRemove)
            {
                _filters.Remove(filter);
            }

            foreach (var data in _bindingList)
            {
                // Check if data was filtered before
                if (toRemove.Any(f => data.Key.Contains(f.Substring, StringComparison.OrdinalIgnoreCase)))
                {
                    data.Enabled = IsFiltered(data);
                }
            }

            RefreshAll();
        }

        private void filter_textBox_Click(object sender, EventArgs e)
        {
            add_Button.Show();
        }

        private void enabledAll_button_Click(object sender, EventArgs e)
        {
            foreach (var row in series_radGridView.Rows)
            {
                if (row.Cells[0].ReadOnly == false)
                {
                    row.Cells[0].Value = true;
                }
            }
        }

        private void disableAll_button_Click(object sender, EventArgs e)
        {
            foreach (var row in series_radGridView.Rows)
            {
                row.Cells[0].Value = false;
            }
        }
    }
}
