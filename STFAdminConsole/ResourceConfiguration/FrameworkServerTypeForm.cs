using System;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Core.AssetInventory;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Displays all FrameworkServerTypes and allows addition of new types.
    /// Very few requests are received to remove FrameworkServerTypes from the database,
    /// hence this form intentionally does not expose Remove and Edit functionality.
    /// Because of the many-to-many relationships between FrameworkServer and FrameworkServerType,
    /// the logistics of Edit and Remove operations would also complicate things significantly.
    /// </summary>
    public partial class FrameworkServerTypeForm : Form
    {
        private SortableBindingList<FrameworkServerType> _serverTypes = new SortableBindingList<FrameworkServerType>();
        private FrameworkServerController _controller = null;
        private int _originalWidth = 437;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkServerTypeForm"/> class.
        /// </summary>
        public FrameworkServerTypeForm()
        {
            InitializeComponent();
            dataGridView_Types.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkServerTypeForm"/> class.
        /// </summary>
        /// <param name="controller">The <see cref="FrameworkServerController"/>.</param>
        public FrameworkServerTypeForm(FrameworkServerController controller) : this()
        {
            _controller = controller;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _originalWidth = this.Width;

            foreach (FrameworkServerType serverType in _controller.ServerTypes)
            {
                _serverTypes.Add(serverType);
            }

            dataGridView_Types.DataSource = null;
            dataGridView_Types.DataSource = _serverTypes;
        }

        /// <summary>
        /// Adds a new <see cref="FrameworkServerType"/> to the system.
        /// </summary>
        private void toolStripButton_Add_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                FrameworkServerType newType = new FrameworkServerType()
                {
                    FrameworkServerTypeId = SequentialGuid.NewGuid(),
                    Name = toolStripTextBox_Name.Text,
                    Description = toolStripTextBox_Descr.Text
                };

                _serverTypes.Add(newType);

                toolStripTextBox_Name.Text = string.Empty;
                toolStripTextBox_Descr.Text = string.Empty;
            }
        }

        /// <summary>
        /// Save all changes
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button_Ok_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            foreach (FrameworkServerType serverType in _serverTypes)
            {
                if (_controller.Context.Entry(serverType).State == EntityState.Added || _controller.Context.Entry(serverType).State == EntityState.Detached)
                {
                    _controller.AddServerType(serverType);
                }
            }
            _controller.SaveChanges();

            Cursor = Cursors.Default;
        }

        private bool ValidateInput()
        {
            StringBuilder message = new StringBuilder();

            message.Append(ValidateControl(toolStripLabel_Name.Text, toolStripTextBox_Name));
            message.Append(ValidateControl(toolStripLabel_Descr.Text, toolStripTextBox_Descr));

            if (message.Length > 0)
            {
                MessageBox.Show(this, message.ToString(), "Add Server Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private string ValidateControl(string fieldLabel, ToolStripTextBox control)
        {
            if (string.IsNullOrEmpty(control.Text.Trim()))
            {
                return $"{fieldLabel} must have a value.\n";
            }

            return string.Empty;
        }

        /// <summary>
        /// Handles the ResizeEnd event of the Form.
        /// Adjusts the Descripton textbox in the toolbar to fill up the extra space (if any).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FrameworkServerTypeForm_ResizeEnd(object sender, EventArgs e)
        {
            int width = this.Width - 312;
            if (this.Width < _originalWidth)
            {
                width = _originalWidth - 312;
            }

            toolStripTextBox_Descr.Size = new Size(width, toolStripTextBox_Descr.Height);
        }


    }
}
