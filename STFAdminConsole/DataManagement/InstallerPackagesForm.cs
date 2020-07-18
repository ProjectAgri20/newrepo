using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.UI;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using System.IO;
using HP.ScalableTest.Xml;
using HP.ScalableTest.Framework.Settings;
using System.Text;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Utility for maintaining Software Installer Package relationships in the STF.
    /// </summary>
    public partial class InstallerPackagesForm : Form
    {
        private const int SelectColumn = 0;
        private EnterpriseTestContext _dataContext = null;
        private IEnumerable<SoftwareInstallerPackage> _packages = null;
        private SortableBindingList<ResourceTypeRow> _resourceTypes = new SortableBindingList<ResourceTypeRow>();
        private SortableBindingList<MetadataTypeRow> _metadataTypes = new SortableBindingList<MetadataTypeRow>();
        private BindingManagerBase _packagesCurrencyManager = null;

        private string _directory = string.Empty;

        /// <summary>
        /// Constructor for InstallerPackagesForm.
        /// </summary>
        public InstallerPackagesForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
        }

        private SoftwareInstallerPackage SelectedPackage
        {
            get 
            {
                if (_packagesCurrencyManager != null)
                {
                    return _packagesCurrencyManager.Current as SoftwareInstallerPackage;
                }
                return null;
            }
        }

        private void InstallerPackagesForm_Load(object sender, EventArgs e)
        {
            packages_DataGridView.AutoGenerateColumns = false;
            resourceTypes_DataGridView.AutoGenerateColumns = false;
            metadataTypes_DataGridView.AutoGenerateColumns = false;

            Cursor = Cursors.WaitCursor;
            _dataContext = new EnterpriseTestContext();

            LoadGrids();
        }

        private void LoadGrids()
        {
            //Thread the loading of all the data
            BackgroundWorker loadPackagesWorker = new BackgroundWorker();
            loadPackagesWorker.DoWork += new DoWorkEventHandler(LoadPackagesWorker_DoWork);
            loadPackagesWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadPackagessWorker_RunWorkerCompleted);
            loadPackagesWorker.RunWorkerAsync();
            
            BackgroundWorker loadResourcesWorker = new BackgroundWorker();
            loadResourcesWorker.DoWork += new DoWorkEventHandler(LoadResourcesWorker_DoWork);
            loadResourcesWorker.RunWorkerAsync(packages_DataGridView);

            BackgroundWorker loadMetadataWorker = new BackgroundWorker();
            loadMetadataWorker.DoWork += new DoWorkEventHandler(LoadMetadataWorker_DoWork);
            loadMetadataWorker.RunWorkerAsync(packages_DataGridView);
        }

        private void LoadPackagesWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            packages_DataGridView.Invoke(new MethodInvoker(LoadPackages));
        }

        private void LoadResourcesWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            resourceTypes_DataGridView.Invoke(new MethodInvoker(LoadResourceTypes));
        }

        private void LoadMetadataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            metadataTypes_DataGridView.Invoke(new MethodInvoker(LoadMetadataTypes));
        }

        private void LoadPackages()
        {
           packages_DataGridView.DataSource = null; 

            _packages = _dataContext.SoftwareInstallerPackages
                                    .Include("ResourceTypes")
                                    .Include("MetadataTypes")
                                    .OrderBy(n => n.Description);
            packages_DataGridView.DataSource = _packages;

            _packagesCurrencyManager = packages_DataGridView.BindingContext[_packages];
        }

        private void LoadResourceTypes()
        {
            foreach (ResourceType resourceType in _dataContext.ResourceTypes)
            {
                _resourceTypes.Add(new ResourceTypeRow(resourceType));
            }

            resourceTypes_DataGridView.DataSource = null;
            resourceTypes_DataGridView.DataSource = _resourceTypes;
        }

        private void LoadMetadataTypes()
        {
            foreach (MetadataType metadataType in _dataContext.MetadataTypes)
            {
                _metadataTypes.Add(new MetadataTypeRow(metadataType));
            }
            
            metadataTypes_DataGridView.DataSource = null;
            metadataTypes_DataGridView.DataSource = _metadataTypes;
        }

        private void LoadPackagessWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            packages_DataGridView_SelectionChanged(sender, EventArgs.Empty);
            Cursor = Cursors.Default;
        }

        private void RefreshSelection(SoftwareInstallerPackage selectedPackage)
        {
            Cursor = Cursors.WaitCursor;

            if (selectedPackage != null)
            {
                softwareInstallerSelectionControl.SetDataSource(selectedPackage, _dataContext);

                foreach (ResourceTypeRow row in _resourceTypes)
                {
                    row.Selected = selectedPackage.ResourceTypes.Contains(row.ResourceType);
                }

                foreach (MetadataTypeRow row in _metadataTypes)
                {
                    row.Selected = selectedPackage.MetadataTypes.Contains(row.MetadataType);
                }

                softwareInstallerSelectionControl.Refresh();
                resourceTypes_DataGridView.Refresh();
                metadataTypes_DataGridView.Refresh();

                ScrollToFirstSelection(metadataTypes_DataGridView);
            }            

            Cursor = Cursors.Default;
        }

        private void ClearCheckboxSelection(DataGridView dataGrid)
        {
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                row.Cells[SelectColumn].Value = false;
            }
        }

        private void packagesCurrencyManager_CurrentChanged(object sender, EventArgs e)
        {
            // Check for a newly added package
            SoftwareInstallerPackage package = SelectedPackage;
            if (package != null && package.PackageId == Guid.Empty)
            {
                package.PackageId = SequentialGuid.NewGuid();
            }
        }

        private void packages_DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("packages_DataGridView_SelectionChanged.");
            if (packages_DataGridView.SelectedRows.Count > 0)
            {
                RefreshSelection(SelectedPackage);
            }
        }

        /// <summary>
        /// Forces the DataGrid to end edit when it loses focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void packages_DataGridView_Leave(object sender, EventArgs e)
        {
            packages_DataGridView.EndEdit();
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            SaveChanges();
            Cursor = Cursors.Default;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (SaveChanges())
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool SaveChanges()
        {
            foreach (SoftwareInstallerPackage package in _packages)
            {
                if (package.EntityState == EntityState.Added || package.EntityState == EntityState.Detached)
                {
                    _dataContext.SoftwareInstallerPackages.AddObject(package);
                }
            }
            try
            {
                _dataContext.SaveChanges();
                return true;
            }
            catch (UpdateException ex)
            {
                TraceFactory.Logger.Error(ex);
                MessageBox.Show("Only one instance of an installer can be added to an installer package.", "Update Installer Package", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //If we end up here, the save failed.
            return false;
        }

        private void ScrollToFirstSelection(DataGridView dataGrid)
        {
            int scrollIndex = 0;
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (((bool)row.Cells[SelectColumn].Value))
                {
                    scrollIndex = row.Index;
                    break;
                }
            }

            if (dataGrid.Rows.Count > 0)
            {
                dataGrid.FirstDisplayedScrollingRowIndex = scrollIndex;
            }
        }

        private void resourceTypes_DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == SelectColumn)
                {
                    // The boolean value here is swapped, because the checked value of the control has not been updated yet.
                    bool isChecked = !(bool)resourceTypes_DataGridView.Rows[e.RowIndex].Cells[SelectColumn].Value;
                    ResourceTypeRow resourceTypeRow = resourceTypes_DataGridView.Rows[e.RowIndex].DataBoundItem as ResourceTypeRow;

                    //System.Diagnostics.Debug.WriteLine("Resource.  Control Value:{0}  Data Item Value:{1}".FormatWith(isChecked, resourceTypeRow.Selected));
                    UpdateResourceTypeList(isChecked, SelectedPackage, resourceTypeRow.ResourceType);
                }
            }
        }

        private void metadataTypes_DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == SelectColumn)
                {
                    // The boolean value here is swapped, because the checked value of the control has not been updated yet.
                    bool isChecked = !(bool)metadataTypes_DataGridView.Rows[e.RowIndex].Cells[SelectColumn].Value;
                    MetadataTypeRow metadataTypeRow = metadataTypes_DataGridView.Rows[e.RowIndex].DataBoundItem as MetadataTypeRow;

                    //System.Diagnostics.Debug.WriteLine("Metadata.  Control Value:{0}  Data Item Value:{1}".FormatWith(isChecked, metadataTypeRow.Selected));
                    UpdateMetadataTypeList(isChecked, SelectedPackage, metadataTypeRow.MetadataType);
                }
            }
        }

        private static void UpdateResourceTypeList(bool isSelected, SoftwareInstallerPackage selectedPackage, ResourceType resourceType)
        {
            if (resourceType != null)
            {
                if (isSelected && !selectedPackage.ResourceTypes.Contains(resourceType))
                {
                    selectedPackage.ResourceTypes.Add(resourceType);
                }
                else
                {
                    selectedPackage.ResourceTypes.Remove(resourceType);
                }
            }
        }

        private static void UpdateMetadataTypeList(bool isSelected, SoftwareInstallerPackage selectedPackage, MetadataType metadataType)
        {
            if (metadataType != null)
            {
                if (isSelected && !selectedPackage.MetadataTypes.Contains(metadataType))
                {
                    selectedPackage.MetadataTypes.Add(metadataType);
                }
                else
                {
                    selectedPackage.MetadataTypes.Remove(metadataType);
                }
            }
        }

        private void DisplayValidationError(string message)
        {
            if (packages_DataGridView.CurrentCell.IsInEditMode)
            {
                MessageBox.Show(this, message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                packages_DataGridView.CurrentCell.ErrorText = message;
            }
        }

        private void packages_DataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // Clear all associations to SoftwareInstallerSettings before deleting.
            SoftwareInstallerPackage package = e.Row.DataBoundItem as SoftwareInstallerPackage;
            package.SoftwareInstallerSettings.Clear();
        }

        private void packages_DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Cell EndEdit ({0},{1})".FormatWith(e.RowIndex, e.ColumnIndex));
            SoftwareInstallerPackage package = SelectedPackage;
            if (ValidatePackage(package))
            {
                RefreshSelection(package);
            }
        }

        private void packages_DataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Cell Validating ({0},{1})".FormatWith(e.RowIndex, e.ColumnIndex));
            string packageName = e.FormattedValue as string;

            if (! string.IsNullOrEmpty(packageName))
            {
                ValidatePackage(SelectedPackage);
            }
        }

        /// <summary>
        /// Clear out the error text when the user leaves the cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void packages_DataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            packages_DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = string.Empty;
        }

        private bool ValidatePackage(SoftwareInstallerPackage package)
        {
            if (package == null || string.IsNullOrEmpty(package.Description))
            {
                DisplayValidationError("The installer package must have a name.");
                return false;
            }

            if (package.PackageId == Guid.Empty)
            {
                package.PackageId = SequentialGuid.NewGuid();
            }

            return true;
        }

        private void exportSoftwarePackagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string metadataTypeName = string.Empty;
            if (metadataTypes_DataGridView.SelectedRows.Count > 0)
            {
                metadataTypeName = (string)metadataTypes_DataGridView.SelectedRows[0].Cells["metadataTypeName_Column"].Value;
            }
            else
            {
                return;
            }

            using (var dialog = new ExportSaveFileDialog(_directory, "Export Software Installer Package", ImportExportType.Installer))
            {
                string exportFile = string.Empty;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    exportFile = dialog.Base.FileName;
                    _directory = Path.GetDirectoryName(exportFile);
                }
                else
                {
                    return;
                }

                try
                {
                    Cursor = Cursors.WaitCursor;
                    var contract = new MetadataTypeInstallerContract();
                    contract.Export(metadataTypeName, exportFile);
                    Cursor = Cursors.Default;

                    MessageBox.Show("Data successfully exported", "STB Data Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            var file = string.Empty;
            using (var dialog = new ExportOpenFileDialog(_directory, "Open Software Installer Export File", ImportExportType.Installer))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    file = dialog.Base.FileName;
                    _directory = Path.GetDirectoryName(file);
                }
            }

            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            var contract = LegacySerializer.DeserializeDataContract<MetadataTypeInstallerContract>(File.ReadAllText(file));

            using (var context = new EnterpriseTestContext())
            {
                var metadataType = context.MetadataTypes.FirstOrDefault(x => x.Name.Equals(contract.Name));

                if (metadataType == null)
                {
                    MessageBox.Show("The plugin activity ({0}) dependent on this software installer does not exist in this system.".FormatWith(contract.Name));
                    return;
                }

                try
                {
                    Cursor = Cursors.WaitCursor;

                    var currentPackages = context.SoftwareInstallerPackages
                        .Where(x => x.MetadataTypes.Any(y => y.Name.Equals(contract.Name)))
                        .Select(x => x.Description);

                    if (currentPackages != null && currentPackages.Count() > 0)
                    {
                        var packageNames = string.Join(Environment.NewLine, currentPackages.ToArray());
                        StringBuilder builder = new StringBuilder();
                        builder.Append("The following packages are already associated with the {0} plugin.".FormatWith(contract.Name));
                        builder.AppendLine(" Do you want to continue with the import?");
                        builder.AppendLine();
                        builder.AppendLine(packageNames);

                        var result = MessageBox.Show(builder.ToString(), "Installers Already Exist for {0}".FormatWith(contract.Name), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }

                try
                {
                    Cursor = Cursors.WaitCursor;
                    contract.Import(context);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }

            LoadGrids();

            MessageBox.Show("The software installer was successfully imported", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
