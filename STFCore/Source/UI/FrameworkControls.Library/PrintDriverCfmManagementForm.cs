using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.UI.Framework
{
    public partial class PrintDriverCfmManagementForm : Form
    {
        private SortableBindingList<PrintDriverConfig> _configData = new SortableBindingList<PrintDriverConfig>();
        private AssetInventoryContext _context = null;
        private PrintDriverConfig _current = null;

        public PrintDriverCfmManagementForm()
        {
            InitializeComponent();

            configFiles_DataGridView.AutoGenerateColumns = false;
        }

        private void upload_Button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.AddExtension = true;
                dialog.AutoUpgradeEnabled = true;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "cfm";
                dialog.Multiselect = false;
                dialog.Title = "Select CFM File";
                dialog.Filter = "CFM files (*.cfm)|*.cfm";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    AddFile(dialog.FileName);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Lots of things could go wrong when uploading the file, and we want to catch all of them.")]
        private void AddFile(string filePath)
        {
            bool copyFileToServer = true;

            try
            {
                Cursor = Cursors.WaitCursor;

                string fileName = Path.GetFileName(filePath);
                string repository = GlobalSettings.Items[Setting.PrintDriverConfigFileLocation];
                string destinationPath = Path.Combine(repository, fileName);

                if (EnvironmentFileServer.FileExists(destinationPath))
                {
                    var result = MessageBox.Show(
                            "File [{0}] already exists on the file server.\nClick \"Yes\" to overwrite, \"No\" to keep existing, or \"Cancel\"".FormatWith(fileName), 
                            "Overwrite File",
                            MessageBoxButtons.YesNoCancel, 
                            MessageBoxIcon.Exclamation
                       );

                    switch (result)
                    {
                        case DialogResult.Cancel:
                            return;

                        case DialogResult.No:
                            copyFileToServer = false;
                            break;
                        case DialogResult.Yes:
                            copyFileToServer = true;
                            break;
                    }
                }

                // Copy to file server if not already exist or user has chosen to overwrite
                if (copyFileToServer)
                {
                    EnvironmentFileServer.CopyFile(filePath, destinationPath);
                }

                // Add entry to database if it doesn't already exist
                PrintDriverConfig config = _configData.FirstOrDefault(e => Path.GetFileName(e.ConfigFile).Equals(fileName, StringComparison.OrdinalIgnoreCase));
                if (config == null)
                {
                    config = new PrintDriverConfig
                    {
                        PrintDriverConfigId = SequentialGuid.NewGuid(),
                        ConfigFile = destinationPath
                    };
                    _configData.Add(config);
                    _context.PrintDriverConfigs.Add(config);
                }

                // Set the newly uploaded item as current
                SelectedConfig = config;
            }
            catch (Exception ex)
            {
                MessageBox.Show
                    (
                        "Error communicating with remote server.{0}{1}".FormatWith(Environment.NewLine, ex.Message),
                        "Error Adding File",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private PrintDriverConfig SelectedConfig
        {
            get { return configFiles_DataGridView.SelectedRows[0].DataBoundItem as PrintDriverConfig; }
            set
            {
                int index = _configData.ToList<PrintDriverConfig>().FindIndex(x => x.PrintDriverConfigId == value.PrintDriverConfigId);
                if (index >= 0)
                {
                    configFiles_DataGridView.FirstDisplayedScrollingRowIndex = index;
                    configFiles_DataGridView.Rows[index].Selected = true;
                    _current = _configData[index];
                    RefreshCheckLists();
                }
            }
        }

        private void PrintDriverCFMManagementForm_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                _context = DbConnect.AssetInventoryContext();
                LoadConfigDataFile();
                LoadCheckBoxes();

                if (_configData.Count > 0)
                {
                    _current = configFiles_DataGridView.Rows[0].DataBoundItem as PrintDriverConfig;
                    configFiles_DataGridView.Rows[0].Selected = true;
                    RefreshCheckLists();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void LoadConfigDataFile()
        {
            foreach (var item in _context.PrintDriverConfigs
                .Include("PrintDriverProducts")
                .Include("PrintDriverVersions")
                .OrderBy(e => e.ConfigFile))
            {
                _configData.Add(item);
            }

            configFiles_DataGridView.DataSource = _configData;
        }

        private void LoadCheckBoxes()
        {
            LoadProductCheckList();
            LoadVersionCheckList();
        }

        private void LoadVersionCheckList()
        {
            version_CheckedListBox.Items.Clear();

            foreach (var item in _context.PrintDriverVersions.Distinct())
            {
                version_CheckedListBox.Items.Add(item.Value);
            }
            version_CheckedListBox.Sorted = true;
        }

        private void LoadProductCheckList()
        {
            product_CheckedListBox.Items.Clear();

            foreach (var item in _context.PrintDriverProducts.Distinct())
            {
                product_CheckedListBox.Items.Add(item.Name);
            }
            product_CheckedListBox.Sorted = true;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _context.SaveChanges();
            Cursor = Cursors.Default;

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void version_CheckedListBox_Click(object sender, EventArgs e)
        {
            ProcessVersionCheckList();
        }

        private void ProcessVersionCheckList()
        {
            if (_current != null)
            {
                var checkedItem = version_CheckedListBox.SelectedItem as string;
                var version = _context.PrintDriverVersions.FirstOrDefault(x => x.Value.Equals(checkedItem));

                if (version != null)
                {
                    if (version_CheckedListBox.GetItemChecked(version_CheckedListBox.SelectedIndex))
                    {
                        _current.PrintDriverVersions.Remove(version);
                    }
                    else
                    {
                        _current.PrintDriverVersions.Add(version);
                    }
                }
            }
        }

        private void product_CheckedListBox_Click(object sender, EventArgs e)
        {
            ProcessProductCheckedList();
        }

        private void ProcessProductCheckedList()
        {
            if (_current != null)
            {
                var checkedItem = product_CheckedListBox.SelectedItem as string;
                var product = _context.PrintDriverProducts.FirstOrDefault(x => x.Name.Equals(checkedItem));

                if (product != null)
                {
                    if (product_CheckedListBox.GetItemChecked(product_CheckedListBox.SelectedIndex))
                    {
                        _current.PrintDriverProducts.Remove(product);
                    }
                    else
                    {
                        _current.PrintDriverProducts.Add(product);
                    }
                }
            }
        }

        private void configFiles_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _current = configFiles_DataGridView.Rows[e.RowIndex].DataBoundItem as PrintDriverConfig;

                RefreshCheckLists();
            }
        }

        private void RefreshCheckLists()
        {
            RefreshVersionCheckList();
            RefreshProductCheckList();
        }

        private void RefreshVersionCheckList()
        {
            ClearList(version_CheckedListBox);
            if (_current != null)
            {
                foreach (var version in _current.PrintDriverVersions)
                {
                    SetCheck(version_CheckedListBox, version.Value);
                }
            }
        }

        private void RefreshProductCheckList()
        {
            ClearList(product_CheckedListBox);
            if (_current != null)
            {
                foreach (var product in _current.PrintDriverProducts)
                {
                    SetCheck(product_CheckedListBox, product.Name);
                }
            }
        }

        private static void SetCheck(CheckedListBox list, object value)
        {
            int index = list.Items.IndexOf(value);
            list.SetItemChecked(index, true);
        }

        private static void ClearList(CheckedListBox list)
        {
            foreach (int index in list.CheckedIndices)
            {
                list.SetItemCheckState(index, CheckState.Unchecked);
            }
        }

        private void productSelectAll_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (_current != null)
            {
                for (int i = 0; i < product_CheckedListBox.Items.Count; i++)
                {
                    if (!product_CheckedListBox.GetItemChecked(i))
                    {
                        product_CheckedListBox.SetItemCheckState(i, CheckState.Checked);
                        var item = product_CheckedListBox.Items[i].ToString();
                        var product = _context.PrintDriverProducts.FirstOrDefault(x => x.Name.Equals(item));

                        if (product != null)
                        {
                            _current.PrintDriverProducts.Add(product);
                        }
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void productUnselectAll_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            foreach (int index in product_CheckedListBox.CheckedIndices)
            {
                product_CheckedListBox.SetItemCheckState(index, CheckState.Unchecked);
            }

            if (_current != null)
            {
                _current.PrintDriverProducts.Clear();
            }

            Cursor = Cursors.Default;
        }

        private void versionSelectAll_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (_current != null)
            {
                for (int i = 0; i < version_CheckedListBox.Items.Count; i++)
                {
                    if (!version_CheckedListBox.GetItemChecked(i))
                    {
                        version_CheckedListBox.SetItemCheckState(i, CheckState.Checked);
                        var item = version_CheckedListBox.Items[i].ToString();
                        var version = _context.PrintDriverVersions.FirstOrDefault(x => x.Value.Equals(item));

                        if (version != null)
                        {
                            _current.PrintDriverVersions.Add(version);
                        }
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void versionUnselectAll_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            foreach (int index in version_CheckedListBox.CheckedIndices)
            {
                version_CheckedListBox.SetItemCheckState(index, CheckState.Unchecked);
            }

            if (_current != null)
            {
                _current.PrintDriverVersions.Clear();
            }

            Cursor = Cursors.Default;
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _context.SaveChanges();
            Cursor = Cursors.Default;
        }

        private void refreshProduct_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                // Compare that list to what's already in the PrintDriverProduct table, and add any new entries
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    // Using List<> as opposed to IQueryable<> or IEnumerable<> as they are attached
                    // to the context and causing problems across different contexts
                    List<string> existing = null;
                    List<string> missing = null;

                    existing = context.PrintDriverProducts.Select(x => x.Name).Distinct().ToList();

                    // Go to the asset inventory database and get a list of products
                    using (var assetContext = DbConnect.AssetInventoryContext())
                    {
                        var printers = assetContext.Assets.OfType<Core.AssetInventory.Printer>().Select(x => x.Product).Distinct().ToList();
                        missing = printers.Except(existing).ToList();
                    }

                    foreach (var product in missing)
                    {
                        // Add the new product to the database
                        PrintDriverProduct newProduct = new PrintDriverProduct();
                        newProduct.Name = product;
                        newProduct.PrintDriverProductId = SequentialGuid.NewGuid();
                        context.PrintDriverProducts.Add(newProduct);
                    }

                    if (missing.Count > 0)
                    {
                        context.SaveChanges();
                        LoadProductCheckList();
                        RefreshProductCheckList();
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static string GetVersion(string item)
        {
            return item.Split('\\').First();
        }

        private void refreshVersion_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                // Compare that list to what's already in the PrintDriverProduct table, and add any new entries
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    // Using List<> as opposed to IQueryable<> or IEnumerable<> as they are attached
                    // to the context and causing problems across different contexts
                    List<string> existing = null;
                    List<string> missing = null;

                    existing = context.PrintDriverVersions.Select(x => x.Value).Distinct().ToList();

                    List<string> versions = new List<string>();
                    foreach (var item in context.PrintDriverPackages.Select(x => x.Name).Distinct())
                    {
                        string splitItem = item.Split('\\').First();

                        if (!versions.Contains<string>(splitItem))
                        {
                            versions.Add(splitItem);
                        }
                    }

                    missing = versions.Except(existing).ToList();

                    foreach (var version in missing)
                    {
                        // Add the new product to the database
                        PrintDriverVersion newVersion = new PrintDriverVersion();
                        newVersion.Value = version;
                        newVersion.PrintDriverVersionId = SequentialGuid.NewGuid();
                        context.PrintDriverVersions.Add(newVersion);
                    }

                    if (missing.Count > 0)
                    {
                        context.SaveChanges();
                        LoadVersionCheckList();
                        RefreshVersionCheckList();
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void refreshVersion_LinkLabel_LinkClickedMombi(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (!LoadPrintDriverVersionsMombi())
                {
                    MessageBox.Show("Unable to update available driver versions", "Print Driver Repository Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private bool LoadPrintDriverVersionsMombi()
        {
            string pattern = @"evo\\.*?([\d\.]+).*$";
            bool successful = true;

            try
            {
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    Collection<string> versions = new Collection<string>();

                    foreach (var directory in Directory.GetDirectories(GlobalSettings.Items[Setting.UniversalPrintDriverBaseLocation]))
                    {
                        // Parse the directory name and find the version number
                        Match match = Regex.Match(directory, pattern);

                        if (match.Success)
                        {
                            string version = match.Groups[1].Value;
                            if (!context.PrintDriverVersions.Any(x => x.Value.Equals(version)) &&
                                !versions.Any(x => x.Equals(version)))
                            {
                                versions.Add(version);
                            }
                        }
                    }

                    if (versions.Count > 0)
                    {
                        foreach (var version in versions)
                        {
                            PrintDriverVersion newVersion = new PrintDriverVersion();
                            newVersion.Value = version;
                            newVersion.PrintDriverVersionId = SequentialGuid.NewGuid();
                            context.PrintDriverVersions.Add(newVersion);
                        }

                        context.SaveChanges();
                        LoadVersionCheckList();
                        RefreshVersionCheckList();
                    }
                }
            }
            catch (IOException)
            {
                // Prompt the user for credentials.
                using (PrintDriverCredentialForm credentialForm = new PrintDriverCredentialForm())
                {
                    credentialForm.ShareLocation = GlobalSettings.Items[Setting.UniversalPrintDriverBaseLocation];
                    if (credentialForm.ShowDialog() == DialogResult.OK)
                    {
                        successful = LoadPrintDriverVersionsMombi();
                    }
                    else
                    {
                        successful = false;
                    }
                }
            }

            return successful;
        }
    }
}