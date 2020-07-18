using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// This class displays data and operations for a user with regard to print queues 
    /// that are out of sync with the database or the print server on which they reside.
    /// </summary>
    public partial class PrintQueueRefreshForm : Form
    {
        private Collection<string> _selectedQueues;
        private const int SELECT_COLUMN = 0;
        private const int QUEUE_NAME_COLUMN = 1;
        private const int SCENARIO_NAME_COLUMN = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueRefreshForm"/> class.
        /// </summary>
        public PrintQueueRefreshForm()
        {
            InitializeComponent();
            _selectedQueues = new Collection<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueRefreshForm"/> class.
        /// </summary>
        /// <param name="instructions">The instructions for the user.</param>
        /// <param name="okButtonText">The OK button text.</param>
        /// <param name="windowTitle">The window title.</param>
        public PrintQueueRefreshForm(string instructions, string okButtonText, string windowTitle)
            : this()
        {
            instructions_Label.Text = instructions;
            ok_Button.Text = okButtonText;
            this.Text = windowTitle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueRefreshForm"/> class.
        /// </summary>
        /// <param name="queueNames">The print queue names.</param>
        /// <param name="instructions">The instructions.</param>
        /// <param name="okButtonText">The OK button text.</param>
        /// <param name="windowTitle">The window title.</param>
        public PrintQueueRefreshForm(Collection<string> queueNames, string instructions, string okButtonText, string windowTitle)
            : this(instructions, okButtonText, windowTitle)
        {
            foreach (string name in queueNames)
            {
                printQueue_DataGridView.Rows.Add(false, name);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueRefreshForm"/> class.
        /// </summary>
        /// <param name="queuesAndScenarios">The print queues and their associated scenarios.</param>
        /// <param name="instructions">The instructions for the user.</param>
        /// <param name="okButtonText">The OK button text.</param>
        /// <param name="windowTitle">The window title.</param>
        /// <param name="selectColumnHeaderText">The select column header text.</param>
        public PrintQueueRefreshForm(Collection<Tuple<string,string>> queuesAndScenarios, string instructions, string okButtonText, string windowTitle, string selectColumnHeaderText)
            : this(instructions, okButtonText, windowTitle)
        {
            printQueue_DataGridView.Columns[SCENARIO_NAME_COLUMN].Visible = true;
            printQueue_DataGridView.Columns[SELECT_COLUMN].HeaderText = selectColumnHeaderText;

            foreach (Tuple<string, string> queueAndScenario in queuesAndScenarios)
            {
                printQueue_DataGridView.Rows.Add();

                DataGridViewRow row = printQueue_DataGridView.Rows[printQueue_DataGridView.RowCount - 1];

                if (queueAndScenario.Item2 == string.Empty)
                {
                    row.Cells[SELECT_COLUMN].Value = true;
                    row.ReadOnly = true;
                }
                else
                {
                    row.Cells[SELECT_COLUMN].Value = false;
                }

                row.Cells[QUEUE_NAME_COLUMN].Value = queueAndScenario.Item1;
                row.Cells[SCENARIO_NAME_COLUMN].Value = queueAndScenario.Item2;
            }

            printQueue_DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            foreach (DataGridViewRow row in printQueue_DataGridView.Rows)
            {
                if ((bool)row.Cells[SELECT_COLUMN].Value == true)
                {
                    _selectedQueues.Add(row.Cells[QUEUE_NAME_COLUMN].Value.ToString());
                }
            }

            this.Close();
        }

        /// <summary>
        /// Gets the selected print queues.
        /// </summary>
        /// <value>The selected print queues.</value>
        public Collection<string> SelectedQueues
        {
            get { return _selectedQueues; }
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
