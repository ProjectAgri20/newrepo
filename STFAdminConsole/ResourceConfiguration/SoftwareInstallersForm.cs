using System;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Form for displaying and managing the list of Software Installers.
    /// </summary>
    public partial class SoftwareInstallersForm : Form
    {
        private EnterpriseTestContext _dataContext = null;
        private SortableBindingList<SoftwareInstaller> _installers = new SortableBindingList<SoftwareInstaller>();
        private bool _localContext = true;
        
        /// <summary>
        /// Constructs a new instance of SoftwareInstallersForm.
        /// Creates and uses a new data context.
        /// </summary>
        public SoftwareInstallersForm()
        {
            InitializeForm();

            _dataContext = new EnterpriseTestContext();
            LoadInstallers();
        }

        /// <summary>
        /// Constructs a new instance of SoftwareInstallersForm.
        /// Uses the provided data context.
        /// </summary>
        /// <param name="dataContext"></param>
        public SoftwareInstallersForm(EnterpriseTestContext dataContext)
        {
            InitializeForm();

            _dataContext = dataContext;
            LoadInstallers();
            _localContext = false;
        }

        /// <summary>
        /// Gets the selected <see cref="SoftwareInstaller"/>.
        /// </summary>
        public SoftwareInstaller Selected
        {
            get
            {
                if (installers_DataGridView.SelectedRows.Count > 0)
                {
                    return installers_DataGridView.SelectedRows[0].DataBoundItem as SoftwareInstaller;
                }
                return null;
            }
        }

        private void InitializeForm()
        {
            InitializeComponent();
            installers_DataGridView.AutoGenerateColumns = false;
        }

        private void LoadInstallers()
        {
            foreach (SoftwareInstaller installer in _dataContext.SoftwareInstallers.Include("SoftwareInstallerSettings"))
            {
                _installers.Add(installer);
            }

            RefreshInstallerGrid();
        }
        
        private void RefreshInstallerGrid()
        {
            installers_DataGridView.DataSource = null;
            installers_DataGridView.DataSource = _installers;
        }

        private void add_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (SoftwareInstallerForm installerDialog = new SoftwareInstallerForm())
            {
                if (installerDialog.ShowDialog(this) == DialogResult.OK)
                {
                    _installers.Add(installerDialog.SoftwareInstaller);
                    _dataContext.AddToSoftwareInstallers(installerDialog.SoftwareInstaller);                    
                    RefreshInstallerGrid();
                }
            }
        }        
        
        private void edit_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (SoftwareInstallerForm installerDialog = new SoftwareInstallerForm(Selected))
            {
                if (installerDialog.ShowDialog(this) == DialogResult.OK)
                {
                    installers_DataGridView.Refresh();
                }
            }
        }

        private void delete_ToolStripButton_Click(object sender, EventArgs e)
        {
            SoftwareInstaller selected = Selected;
            if (selected != null && MessageBox.Show("Delete {0}?".FormatWith(selected.Description), "Delete Software Installer", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _installers.Remove(selected);
                _dataContext.DeleteObject(selected);
                RefreshInstallerGrid();
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            //Only save changes if this form created the data context.
            if (_localContext)
            {
                _dataContext.SaveChanges();
            }
            this.DialogResult = DialogResult.OK;
        }

    }
}
