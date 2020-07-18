using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Displays and manages the list of Software Installers that are part of the given Software Installer Package.
    /// Provides methods for adding and removing installers from the package.
    /// </summary>
    public partial class SoftwareInstallerSelectionControl : UserControl
    {
        private EnterpriseTestContext _dataContext = null;
        private SoftwareInstallerPackage _package = null;
        private SortableBindingList<InstallerSettingRow> _settings = new SortableBindingList<InstallerSettingRow>();
        
        public SoftwareInstallerSelectionControl()
        {
            InitializeComponent();
            installers_DataGridView.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Sets the data source for this control.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataContext"></param>
        public void SetDataSource(SoftwareInstallerPackage dataSource, EnterpriseTestContext dataContext)
        {
            _package = dataSource;
            if (_dataContext != dataContext)
            {
                _dataContext = dataContext;
            }
            _settings.Clear();

            if (dataSource != null)
            {
                Refresh();
            }
        }

        /// <summary>
        /// Refreshes the display of Software Installers.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();

            _settings.Clear();
            foreach (SoftwareInstallerSetting setting in _package.SoftwareInstallerSettings.OrderBy(s => s.InstallOrderNumber))
            {
                _settings.Add(new InstallerSettingRow(setting));
            }

            installers_DataGridView.DataSource = null;
            installers_DataGridView.DataSource = _settings;
            installers_DataGridView.Refresh();
        }

        /// <summary>
        /// Returns the selected <see cref="SoftwareInstallerSetting"/>
        /// </summary>
        private InstallerSettingRow SelectedSetting
        {
            get
            {
                if (installers_DataGridView.SelectedRows.Count > 0)
                {
                    InstallerSettingRow dataItem = installers_DataGridView.SelectedRows[0].DataBoundItem as InstallerSettingRow;
                    return dataItem;
                }
                return null;
            }
        }

        private void add_ToolStripButton_Click(object sender, EventArgs e)
        {
            this.Focus();
            if (_dataContext != null && _package != null)
            {
                //System.Diagnostics.Debug.WriteLine(_package.ToString());
                if (_package.PackageId == Guid.Empty)
                {
                    MessageBox.Show("Unable to add an installer to the selected package.", "Invalid Package", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
                using (SoftwareInstallersForm installersForm = new SoftwareInstallersForm(_dataContext))
                {
                    if (installersForm.ShowDialog(this) == DialogResult.OK)
                    {
                        SoftwareInstaller installer = installersForm.Selected;
                        SoftwareInstallerSetting setting = _dataContext.CreateObject<SoftwareInstallerSetting>();

                        setting.InstallerId = installer.InstallerId;
                        setting.PackageId = _package.PackageId;
                        setting.InstallOrderNumber = _package.SoftwareInstallerSettings.Count + 1;
                        setting.SoftwareInstaller = installer;

                        _settings.Add(new InstallerSettingRow(setting));
                        _package.SoftwareInstallerSettings.Add(setting);
                        _dataContext.AddToSoftwareInstallerSettings(setting);
                    }
                }
            }
        }

        private void remove_ToolStripButton_Click(object sender, EventArgs e)
        {
            InstallerSettingRow selected = SelectedSetting;
            if (selected != null && MessageBox.Show("Remove {0} from this package?".FormatWith(selected.InstallerSetting.SoftwareInstaller.Description), "Remove Software Installer",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _settings.Remove(selected);
                _dataContext.DeleteObject(selected.InstallerSetting);
                RebuildInstallOrder();
            }
        }

        private void moveUp_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (installers_DataGridView.SelectedRows.Count > 0)
            {
                MoveItem(installers_DataGridView.SelectedRows[0], -1);
            }
        }

        private void moveDown_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (installers_DataGridView.SelectedRows.Count > 0)
            {
                MoveItem(installers_DataGridView.SelectedRows[0], 1);
            }
        }

        private void MoveItem(DataGridViewRow selected, int direction)
        {
            int newIndex = selected.Index + direction;

            // Make sure we have a selection and that we're not moving out of bounds
            if (selected.Index != -1 && newIndex >= 0 && newIndex < _settings.Count)
            {
                DataGridViewRow affected = (from DataGridViewRow r in installers_DataGridView.Rows 
                                            where r.Index == newIndex select r).FirstOrDefault();

                //Swap the selected and affected Install Order Numbers
                ((InstallerSettingRow)selected.DataBoundItem).InstallerSetting.InstallOrderNumber += direction;
                ((InstallerSettingRow)affected.DataBoundItem).InstallerSetting.InstallOrderNumber -= direction;

                Refresh();
                installers_DataGridView.Rows[newIndex].Selected = true;
            }
        }

        private void RebuildInstallOrder()
        {
            int index = 1;
            foreach (SoftwareInstallerSetting setting in _package.SoftwareInstallerSettings.OrderBy(s => s.InstallOrderNumber))
            {
                setting.InstallOrderNumber = index++;
            }
        }
    }

    /// <summary>
    /// Helper class for displaying Installer information in a grid.
    /// </summary>
    internal class InstallerSettingRow
    {
        public InstallerSettingRow(SoftwareInstallerSetting installerSetting)
        {
            InstallerSetting = installerSetting;
        }

        public SoftwareInstallerSetting InstallerSetting { get; private set; }
        public string Name { get { return InstallerSetting.SoftwareInstaller.Description; } }
        public string RebootMode { get { return InstallerSetting.SoftwareInstaller.RebootSetting; } }
        public int OrderNumber 
        {
            get { return InstallerSetting.InstallOrderNumber; }
            set { InstallerSetting.InstallOrderNumber = value; } 
        }
    }
}
