using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Development.UI
{
    /// <summary>
    /// A form that shows output from an <see cref="IDataLoggerMock" />.
    /// </summary>
    public partial class DataLoggerMockForm : Form
    {
        private readonly IPluginFrameworkSimulator _simulator;

        private DataLoggerMockForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMockForm" /> class.
        /// </summary>
        /// <param name="simulator">The <see cref="IPluginFrameworkSimulator" /> managing the data to display.</param>
        /// <exception cref="ArgumentNullException"><paramref name="simulator" /> is null.</exception>
        public DataLoggerMockForm(IPluginFrameworkSimulator simulator)
            : this()
        {
            _simulator = simulator ?? throw new ArgumentNullException(nameof(simulator));

            if (_simulator.DataLogger is IDataLoggerMock mock)
            {
                mock.TableAdded += DataLogger_TableAdded;
            }
        }

        private void DataLogger_TableAdded(object sender, DataLoggerMockTableEventArgs e)
        {
            if (dataLoggerOutputTabControl.InvokeRequired)
            {
                dataLoggerOutputTabControl.Invoke(new MethodInvoker(() => DataLogger_TableAdded(sender, e)));
            }
            else
            {
                TabPage page = new TabPage(e.Table.TableName)
                {
                    AutoScroll = true
                };

                DataLoggerMockTableGridView gridView = new DataLoggerMockTableGridView(e.Table);
                page.Controls.Add(gridView);
                gridView.Dock = DockStyle.Fill;
                dataLoggerOutputTabControl.TabPages.Add(page);
            }
        }

        private void DataLoggerMockForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
