using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using Microsoft.Win32;

namespace HP.ScalableTest.Development.UI
{
    /// <summary>
    /// A graphical display over the top of an <see cref="IPluginFrameworkSimulator" />.
    /// </summary>
    public partial class PluginFrameworkSimulatorForm : Form
    {
        private readonly IPluginFrameworkSimulator _simulator;
        private readonly BindingList<PluginConfigurationListViewItem> _pluginConfigurationList = new BindingList<PluginConfigurationListViewItem>();

        private readonly CriticalSectionMockForm _criticalSectionForm;
        private readonly DataLoggerMockForm _dataLoggerForm;

        private IPluginConfigurationControl _configurationControl;
        private IPluginExecutionEngine _executionEngine;

        static PluginFrameworkSimulatorForm()
        {
            UserInterfaceStyler.Initialize();
        }

        private PluginFrameworkSimulatorForm()
        {
            InitializeComponent();

            pluginConfigurationDataListView.AutoGenerateColumns = false;
            pluginConfigurationDataListView.DataSource = _pluginConfigurationList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginFrameworkSimulatorForm" /> class.
        /// </summary>
        /// <param name="simulator">The <see cref="IPluginFrameworkSimulator" /> that will drive this instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="simulator" /> is null.</exception>
        public PluginFrameworkSimulatorForm(IPluginFrameworkSimulator simulator)
            : this()
        {
            _simulator = simulator ?? throw new ArgumentNullException(nameof(simulator));
            _criticalSectionForm = new CriticalSectionMockForm(simulator);
            _dataLoggerForm = new DataLoggerMockForm(simulator);

            // Initialize config control last, since it is the one that will be displayed initially
            InitializeExecutionEngine();
            InitializeConfigurationControl();
            label_PluginType.Text = simulator.PluginAssemblyName;

            string lastAccessedFileName = UserAppDataRegistry.GetValue("PluginDataLocation") as string;
            if (!string.IsNullOrEmpty(lastAccessedFileName))
            {
                try
                {
                    ImportConfigurationData(lastAccessedFileName, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Couldn't load.  Return to clean state.
                    ClearConfigurationData();
                }
            }
        }

        #region Plugin Commands

        private void PluginInitialize()
        {
            _simulator.SetServiceContext(FrameworkServicesContext.Configuration);
            _configurationControl.Initialize(_simulator.Environment);
            SetUnsavedChanges(false);
        }

        private void PluginInitialize(PluginConfigurationData configurationData)
        {
            _simulator.SetServiceContext(FrameworkServicesContext.Configuration);
            _configurationControl.Initialize(configurationData, _simulator.Environment);
            SetUnsavedChanges(false);
        }

        private PluginConfigurationData PluginGetConfiguration()
        {
            _simulator.SetServiceContext(FrameworkServicesContext.Configuration);
            PluginConfigurationData configuration = _configurationControl.GetConfiguration();
            SetUnsavedChanges(false);
            return configuration;
        }

        private PluginValidationResult PluginValidate()
        {
            _simulator.SetServiceContext(FrameworkServicesContext.Configuration);
            return _configurationControl.ValidateConfiguration();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private PluginExecutionResult PluginExecute(PluginExecutionData executionData)
        {
            _simulator.SetServiceContext(FrameworkServicesContext.Execution);
            try
            {
                return _executionEngine.Execute(executionData);
            }
            catch (Exception ex)
            {
                return new PluginExecutionResult(PluginResult.Error, ex.ToString(), "Unhandled exception.");
            }
        }

        #endregion

        #region UI Routines

        private void InitializeConfigurationControl()
        {
            // Only SystemTrace should be useable during constructor
            _simulator.SetServiceContext(FrameworkServicesContext.ConfigurationConstructor);
            _configurationControl = _simulator.CreateConfigurationControl();

            // Initialize the rest of the configuration services
            _simulator.SetServiceContext(FrameworkServicesContext.Configuration);

            // Call Initialize for the new control
            if (_configurationControl != null)
            {
                _configurationControl.ConfigurationChanged += configurationControl_ConfigurationChanged;
                _configurationControl.Initialize(_simulator.Environment);
            }

            // Add control to the UI
            SetPanelControl(pluginConfigurationPanel, _configurationControl as Control);
            SetUnsavedChanges(false);
        }

        private void InitializeExecutionEngine()
        {
            // Only SystemTrace should be useable during constructor
            _simulator.SetServiceContext(FrameworkServicesContext.ExecutionConstructor);
            _executionEngine = _simulator.CreateExecutionEngine();

            // Initialize the rest of the execution services
            _simulator.SetServiceContext(FrameworkServicesContext.Execution);

            // Add control to the UI
            SetPanelControl(pluginExecutionPanel, _executionEngine as Control);
        }

        private static void SetPanelControl(Panel panel, Control control)
        {
            while (panel.Controls.Count > 0)
            {
                Control c = panel.Controls[0];
                panel.Controls.Remove(c);
                c.Dispose();
            }

            if (control != null)
            {
                panel.Controls.Add(control);
                panel.Controls[0].Dock = DockStyle.Fill;
            }
        }

        private void AddConfigurationData(PluginConfigurationData configurationData)
        {
            AddConfigurationData(new PluginConfigurationListViewItem(configurationData));
        }

        private void AddConfigurationData(PluginConfigurationData configurationData, string label)
        {
            AddConfigurationData(new PluginConfigurationListViewItem(configurationData, label));
        }

        private void AddConfigurationData(PluginConfigurationListViewItem pluginConfigurationListViewItem)
        {
            _pluginConfigurationList.Insert(0, pluginConfigurationListViewItem);
            pluginConfigurationDataListView.ClearSelection();
            pluginConfigurationDataListView.Rows[0].Selected = true;
        }

        private PluginConfigurationData GetSelectedConfigurationData()
        {
            if (pluginConfigurationDataListView.SelectedRows.Count > 0)
            {
                if (pluginConfigurationDataListView.SelectedRows[0].DataBoundItem is PluginConfigurationListViewItem selected)
                {
                    PluginConfigurationData configurationData = selected.ConfigurationData;
                    configurationData.Assets = configurationData.Assets ?? new AssetSelectionData();
                    configurationData.Documents = configurationData.Documents ?? new DocumentSelectionData();
                    configurationData.Servers = configurationData.Servers ?? new ServerSelectionData();
                    configurationData.PrintQueues = configurationData.PrintQueues ?? new PrintQueueSelectionData();
                    return configurationData;
                }
            }

            return null;
        }

        private void ClearConfigurationData()
        {
            _pluginConfigurationList.Clear();
            _configurationControl.ConfigurationChanged -= configurationControl_ConfigurationChanged;
            InitializeConfigurationControl();
        }

        private void ShowConfigurationDataDisplayForm(PluginConfigurationData configurationData)
        {
            using (PluginConfigurationDataDisplayForm displayForm = new PluginConfigurationDataDisplayForm(configurationData))
            {
                displayForm.ShowDialog(this);
            }
        }

        private bool NullConfigurationDataCheck()
        {
            if (pluginConfigurationDataListView.SelectedRows.Count == 0)
            {
                MessageBox.Show("No configuration data is selected.", "No Data Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void DisplayValidationResult(PluginValidationResult result)
        {
            if (result.Succeeded)
            {
                MessageBox.Show("Validation successful.", "Validation Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Validation failed:\n" + string.Join("\n", result.ErrorMessages), "Validation Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetUnsavedChanges(bool hasUnsavedChanges)
        {
            unsavedChanges_ToolStripButton.Enabled = hasUnsavedChanges;
            unsavedChanges_ToolStripButton.Text = hasUnsavedChanges ? "Clear Unsaved Changes" : "No Unsaved Changes";
        }

        private void UpdateExecutionStatus(string status)
        {
            pluginExecutionResultsTextBox.AppendText(status + Environment.NewLine);
        }

        private void DisplayExecutionResult(PluginExecutionResult result)
        {
            UpdateExecutionStatus("Result: " + result.Result);
            if (!string.IsNullOrWhiteSpace(result.Category))
            {
                UpdateExecutionStatus("Category: " + result.Category);
            }
            if (!string.IsNullOrWhiteSpace(result.Message))
            {
                UpdateExecutionStatus("Message: " + result.Message);
            }
        }

        private void ClearExecutionResult()
        {
            pluginExecutionResultsTextBox.Clear();
        }

        #endregion

        #region Event Handlers

        private void initializeToolStripButton_Click(object sender, EventArgs e)
        {
            PluginInitialize();
        }

        private void loadToolStripButton_Click(object sender, EventArgs e)
        {
            if (NullConfigurationDataCheck())
            {
                PluginConfigurationData configurationData = GetSelectedConfigurationData();
                PluginInitialize(configurationData);
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                PluginValidationResult result = PluginValidate();
                if (!result.Succeeded)
                {
                    DisplayValidationResult(result);
                    return;
                }
            }
            catch (NotImplementedException)
            {
                // Do nothing - the plugin does not have validation code yet
            }

            PluginConfigurationData configurationData = PluginGetConfiguration();
            AddConfigurationData(configurationData);
            SetUnsavedChanges(false);
        }

        private void validateToolStripButton_Click(object sender, EventArgs e)
        {
            PluginValidationResult result = PluginValidate();
            DisplayValidationResult(result);
        }

        private void resetConfigurationToolStripButton_Click(object sender, EventArgs e)
        {
            InitializeConfigurationControl();
            SetUnsavedChanges(false);
        }

        private void executeToolStripButton_Click(object sender, EventArgs e)
        {
            executeToolStripButton.Enabled = false;
            resetExecutionToolStripButton.Enabled = false;

            if (NullConfigurationDataCheck())
            {
                executionBackgroundWorker.RunWorkerAsync();
            }
            else
            {
                executeToolStripButton.Enabled = true;
                resetExecutionToolStripButton.Enabled = true;
            }
        }

        private void executionBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            PluginConfigurationData configurationData = GetSelectedConfigurationData();
            PluginExecutionData executionData = _simulator.CreateExecutionData(configurationData);

            int iterations = iterationsNumericUpDown.Value;
            for (int i = 1; i <= iterations; i++)
            {
                executionBackgroundWorker.ReportProgress(0, $"Executing iteration {i} of {iterations}...");
                PluginExecutionResult result = PluginExecute(executionData);
                executionBackgroundWorker.ReportProgress(0, result);
            }
            executionBackgroundWorker.ReportProgress(100, "Execution complete." + Environment.NewLine);
        }

        private void executionBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is PluginExecutionResult result)
            {
                DisplayExecutionResult(result);
            }
            else
            {
                UpdateExecutionStatus(e.UserState.ToString());
            }
        }

        private void executionBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            executeToolStripButton.Enabled = true;
            resetExecutionToolStripButton.Enabled = true;
        }

        private void resetExecutionToolStripButton_Click(object sender, EventArgs e)
        {
            InitializeExecutionEngine();
            ClearExecutionResult();
        }

        private void clearResultToolStripButton_Click(object sender, EventArgs e)
        {
            ClearExecutionResult();
        }

        private void criticalSectionToolStripButton_Click(object sender, EventArgs e)
        {
            _criticalSectionForm.Show();
        }

        private void dataLoggerToolStripButton_Click(object sender, EventArgs e)
        {
            _dataLoggerForm.Show();
        }

        private void unsavedChanges_ToolStripButton_Click(object sender, EventArgs e)
        {
            SetUnsavedChanges(false);
        }

        private void configurationControl_ConfigurationChanged(object sender, EventArgs e)
        {
            SetUnsavedChanges(true);
        }

        private void mainTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // Don't allow tab to change if activity is executing
            e.Cancel = executionBackgroundWorker.IsBusy;
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mainTabControl.SelectedTab == pluginConfigurationTabPage)
            {
                _simulator.SetServiceContext(FrameworkServicesContext.Configuration);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void importConfigurationDataToolStripButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    ImportConfigurationData(openFileDialog.FileName, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file.{Environment.NewLine}{ex.ToString()}", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportConfigurationData(string fileName, bool saveToRegistry)
        {
            PluginConfigurationData data = PluginConfigurationData.LoadFromFile(fileName);
            AddConfigurationData(data, Path.GetFileNameWithoutExtension(fileName));

            // Load metadata unless an activity is executing
            if (!executionBackgroundWorker.IsBusy)
            {
                PluginInitialize(data);
                if (saveToRegistry)
                {
                    UserAppDataRegistry.SetValue("PluginDataLocation", fileName);
                }
            }
        }

        private void exportConfigurationDataToolStripButton_Click(object sender, EventArgs e)
        {
            if (NullConfigurationDataCheck())
            {
                PluginConfigurationData configurationData = GetSelectedConfigurationData();
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    PluginConfigurationData.WriteToFile(configurationData, saveFileDialog.FileName);
                    UserAppDataRegistry.SetValue("PluginDataLocation", saveFileDialog.FileName);
                }
            }
        }

        private void clearConfigurationDataToolStripButton_Click(object sender, EventArgs e)
        {
            if (_pluginConfigurationList.Count > 0)
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure you wish to remove all configuration data?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    ClearConfigurationData();
                }
            }
        }

        private void pluginConfigurationDataListView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex != -1)
            {
                pluginConfigurationDataListView.ClearSelection();
                pluginConfigurationDataListView.Rows[e.RowIndex].Selected = true;
            }
        }

        private void pluginConfigurationDataContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (GetSelectedConfigurationData() == null)
            {
                e.Cancel = true;
            }
        }

        private void pluginConfigurationDataListView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            viewMetadataContextMenuItem_Click(sender, e);
        }

        private void viewMetadataContextMenuItem_Click(object sender, EventArgs e)
        {
            PluginConfigurationData configurationData = GetSelectedConfigurationData();
            ShowConfigurationDataDisplayForm(configurationData);
        }

        private void renameConfigurationDataContextMenuItem_Click(object sender, EventArgs e)
        {
            if (pluginConfigurationDataListView.SelectedRows.Count > 0)
            {
                var selected = pluginConfigurationDataListView.SelectedRows[0].DataBoundItem as PluginConfigurationListViewItem;

                using (ConfigurationDataRenameForm form = new ConfigurationDataRenameForm())
                {
                    form.ConfigurationDataName = selected.Name;
                    if (form.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(form.ConfigurationDataName))
                    {
                        selected.Rename(form.ConfigurationDataName);
                    }
                }
            }
        }

        private void exportConfigurationDataContextMenuItem_Click(object sender, EventArgs e)
        {
            exportConfigurationDataToolStripButton_Click(sender, e);
        }

        private void deleteConfigurationDataContextMenuItem_Click(object sender, EventArgs e)
        {
            if (pluginConfigurationDataListView.SelectedRows.Count > 0)
            {
                var selected = pluginConfigurationDataListView.SelectedRows[0];
                pluginConfigurationDataListView.Rows.Remove(selected);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Helper Classes

        private class PluginConfigurationListViewItem
        {
            public PluginConfigurationData ConfigurationData { get; }

            public string Name { get; private set; }

            public string DisplayText { get; private set; }

            public PluginConfigurationListViewItem(PluginConfigurationData configurationData)
                : this(configurationData, DateTime.Now.ToString())
            {
            }

            public PluginConfigurationListViewItem(PluginConfigurationData configurationData, string name)
            {
                ConfigurationData = configurationData;
                Name = name;
                RefreshDisplayText();
            }

            public void Rename(string name)
            {
                Name = name;
                RefreshDisplayText();
            }

            private void RefreshDisplayText()
            {
                StringBuilder builder = new StringBuilder(Name);
                AddLine(builder, GetAssetDisplayString(ConfigurationData));
                AddLine(builder, GetServerDisplayString(ConfigurationData));
                AddLine(builder, GetDocumentDisplayString(ConfigurationData));
                AddLine(builder, GetPrintQueueDisplayString(ConfigurationData));
                DisplayText = builder.ToString();
            }

            private static void AddLine(StringBuilder builder, string text)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    builder.AppendLine();
                    builder.Append(text);
                }
            }

            private static string GetAssetDisplayString(PluginConfigurationData configurationData)
            {
                AssetSelectionData assets = configurationData.Assets;
                if (assets?.SelectedAssets?.Count + assets?.InactiveAssets?.Count > 0)
                {
                    return $"Assets: {assets.SelectedAssets.Count} selected, {assets.InactiveAssets.Count} inactive";
                }
                else
                {
                    return null;
                }
            }

            private static string GetDocumentDisplayString(PluginConfigurationData configurationData)
            {
                DocumentSelectionData documents = configurationData.Documents;
                if (documents != null)
                {
                    switch (documents.SelectionMode)
                    {
                        case DocumentSelectionMode.SpecificDocuments:
                            return documents?.SelectedDocuments?.Count > 0 ? $"Documents: {documents.SelectedDocuments.Count} documents selected" : null;

                        case DocumentSelectionMode.DocumentSet:
                            return $"Documents: Document Set '{documents.DocumentSetName}' selected";

                        case DocumentSelectionMode.DocumentQuery:
                            return "Documents: Query criteria selection";

                        default:
                            return "Documents: Unknown selection mode";
                    }
                }
                else
                {
                    return null;
                }
            }

            private static string GetServerDisplayString(PluginConfigurationData configurationData)
            {
                ServerSelectionData servers = configurationData.Servers;
                if (servers?.SelectedServers?.Count > 0)
                {
                    return $"Servers: {servers.SelectedServers.Count} selected";
                }
                else
                {
                    return null;
                }
            }

            private static string GetPrintQueueDisplayString(PluginConfigurationData configurationData)
            {
                PrintQueueSelectionData printQueues = configurationData.PrintQueues;
                if (printQueues?.SelectedPrintQueues?.Count > 0)
                {
                    return $"Print Queues: {printQueues.SelectedPrintQueues.Count} selected";
                }
                else
                {
                    return null;
                }
            }

            public override string ToString() => DisplayText;
        }

        private class ToolStripNumericUpDown : ToolStripControlHost
        {
            public ToolStripNumericUpDown()
                : base(new NumericUpDown { Minimum = 1, Maximum = 1000, Value = 1 })
            {
            }

            public int Value => Convert.ToInt32((Control as NumericUpDown).Value);
        }

        #endregion
    }
}
