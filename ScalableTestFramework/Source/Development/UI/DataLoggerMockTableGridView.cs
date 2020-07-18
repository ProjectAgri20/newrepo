using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Development.UI
{
    /// <summary>
    /// Grid view for displaying data from a <see cref="DataLoggerMockTable" />.
    /// </summary>
    public partial class DataLoggerMockTableGridView : UserControl
    {
        private readonly DataLoggerMockTable _dataLoggerTable;
        private readonly DataTable _dataTable = new DataTable();

        private DataLoggerMockTableGridView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMockTableGridView" /> class.
        /// </summary>
        /// <param name="table">The <see cref="DataLoggerMockTable" /> to display in this grid view.</param>
        /// <exception cref="ArgumentNullException"><paramref name="table" /> is null.</exception>
        public DataLoggerMockTableGridView(DataLoggerMockTable table)
            : this()
        {
            _dataLoggerTable = table ?? throw new ArgumentNullException(nameof(table));
            _dataLoggerTable.DataUpdated += DataLoggerTable_DataUpdated;

            BuildTable();
            RefreshGridView();
        }

        private void DataLoggerTable_DataUpdated(object sender, EventArgs e)
        {
            if (_dataTable.Columns.Count == 0)
            {
                BuildTable();
            }
            RefreshGridView();
        }

        private void BuildTable()
        {
            _dataTable.Columns.Clear();

            if (_dataLoggerTable.Records.Count > 0)
            {
                foreach (string key in _dataLoggerTable.Records.Values.FirstOrDefault().Values.Keys)
                {
                    _dataTable.Columns.Add(key);
                }
            }

            dataGridView.DataSource = _dataTable;
        }

        private void RefreshGridView()
        {
            _dataTable.Clear();
            foreach (DataLoggerMockRecord record in _dataLoggerTable.Records.Values)
            {
                DataRow row = _dataTable.NewRow();
                foreach (var item in record.Values)
                {
                    row[item.Key] = item.Value;
                }
                _dataTable.Rows.Add(row);
            }
        }
    }
}
