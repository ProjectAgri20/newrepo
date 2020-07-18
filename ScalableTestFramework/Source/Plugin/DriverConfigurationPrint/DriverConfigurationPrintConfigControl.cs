using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.PluginSupport.Print;

namespace HP.ScalableTest.Plugin.DriverConfigurationPrint
{
    [ToolboxItem(false)]
    public partial class DriverConfigurationPrintConfigControl : UserControl, IPluginConfigurationControl
    {
        private readonly List<PrintQueueInfo> _printQueues = new List<PrintQueueInfo>();
        private readonly BindingList<PrintQueueRow> _printQueueRows = new BindingList<PrintQueueRow>();

        /// <summary>
        /// Construct the Printer Preference UI.
        /// </summary>
        private PrinterPreferencesForm _printingPreferences = new PrinterPreferencesForm();

        /// <summary>
        /// Printer Preference reference variable.
        /// </summary>
        private PrinterPreferenceData _printerPreferenceData = new PrinterPreferenceData();

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of <see cref="DriverConfigurationPrintConfigControl"/>
        /// </summary>
        public DriverConfigurationPrintConfigControl()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(printQueues_GridView, GridViewStyle.ReadOnly);
            printQueues_GridView.DataSource = _printQueueRows;

            fieldValidator.RequireDocumentSelection(documentSelectionControl);
            fieldValidator.RequireCustom(printQueues_GridView, () => printQueues_GridView.Rows.Any(), "At least one print queue must be selected.");

            shuffle_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            jobseparator_checkBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            documentSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            ConfigureControls(new DriverConfigurationPrintActivityData());
            documentSelectionControl.Initialize();
            _printQueues.Clear();
            RefreshQueueDataGrid();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            ConfigureControls(configuration.GetMetadata<DriverConfigurationPrintActivityData>());
            documentSelectionControl.Initialize(configuration.Documents);
            LoadPrintQueues(configuration.PrintQueues.SelectedPrintQueues);
            RefreshQueueDataGrid();
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            PrintingActivityData data = new DriverConfigurationPrintActivityData()
            {
                ShuffleDocuments = shuffle_CheckBox.Checked,
                PrintJobSeparator = jobseparator_checkBox.Checked,
                PrinterPreference = _printerPreferenceData
            };

            return new PluginConfigurationData(data, "1.0")
            {
                Documents = documentSelectionControl.DocumentSelectionData,
                PrintQueues = new PrintQueueSelectionData(_printQueues)
            };
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        private void ConfigureControls(DriverConfigurationPrintActivityData data)
        {
            shuffle_CheckBox.Checked = data.ShuffleDocuments;
            jobseparator_checkBox.Checked = data.PrintJobSeparator;
            _printerPreferenceData = data.PrinterPreference;
        }

        private void LoadPrintQueues(PrintQueueDefinitionCollection printQueueDefinitions)
        {
            _printQueues.Clear();

            foreach (LocalPrintQueueDefinition local in printQueueDefinitions.OfType<LocalPrintQueueDefinition>())
            {
                _printQueues.Add(new LocalPrintQueueInfo(local.QueueName, local.AssociatedAssetId));
            }
        }

        private void RefreshQueueDataGrid()
        {
            _printQueueRows.Clear();
            foreach (PrintQueueInfo info in _printQueues)
            {
                _printQueueRows.Add(new PrintQueueRow(info));
            }
        }

        private void localQueues_ToolStripButton_Click(object sender, EventArgs e)
        {
            var originalLocalPrintQueues = _printQueues.Except(_printQueues.OfType<RemotePrintQueueInfo>()).ToList();

            using (LocalQueueSelectionForm form = new LocalQueueSelectionForm(originalLocalPrintQueues))
            {
                form.DynamicQueueEnabled = false;
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ReplaceQueues(originalLocalPrintQueues, form.SelectedQueues);
                    RefreshQueueDataGrid();
                    ConfigurationChanged(this, EventArgs.Empty);
                }
            }
        }

        private void remoteQueues_ToolStripButton_Click(object sender, EventArgs e)
        {
            var originalRemotePrintQueues = _printQueues.OfType<RemotePrintQueueInfo>().ToList();
            using (RemoteQueueSelectionForm form = new RemoteQueueSelectionForm(originalRemotePrintQueues))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ReplaceQueues(originalRemotePrintQueues, form.SelectedQueues);
                    RefreshQueueDataGrid();
                    ConfigurationChanged(this, EventArgs.Empty);
                }
            }
        }

        private void ReplaceQueues(IEnumerable<PrintQueueInfo> oldQueues, IEnumerable<PrintQueueInfo> newQueues)
        {
            foreach (PrintQueueInfo queue in oldQueues)
            {
                _printQueues.Remove(queue);
            }
            _printQueues.AddRange(newQueues);
        }

        private class PrintQueueRow
        {
            public string QueueName { get; private set; }
            public string PrintServer { get; private set; }
            public string QueueType { get; private set; }
            public string Device { get; private set; }

            public PrintQueueRow(PrintQueueInfo queue)
            {
                QueueName = queue.QueueName;
                PrintServer = "Client";
                QueueType = "Local";
                Device = queue.AssociatedAssetId;

                RemotePrintQueueInfo remoteQueue = queue as RemotePrintQueueInfo;
                if (remoteQueue != null)
                {
                    PrintServer = remoteQueue.ServerHostName;
                    QueueType = "Remote";
                }

                DynamicLocalPrintQueueInfo dynamicLocalQueue = queue as DynamicLocalPrintQueueInfo;
                if (dynamicLocalQueue != null)
                {
                    QueueName = string.Format("{0}\\{1} on {2}", dynamicLocalQueue.PrintDriver.PackageName, dynamicLocalQueue.PrintDriver.DriverName, dynamicLocalQueue.AssociatedAssetId);
                }
            }
        }

        private void printingPreferences_Button_Click(object sender, EventArgs e)
        {
            using (_printingPreferences = new PrinterPreferencesForm(_printerPreferenceData))
            {
                if (_printingPreferences.ShowDialog() == DialogResult.OK)
                {
                    _printerPreferenceData = _printingPreferences.PrinterPreferenceData;
                }
            }
        }

    }
}
