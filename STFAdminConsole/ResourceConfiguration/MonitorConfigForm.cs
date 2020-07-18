using System;
using System.Windows.Forms;
using System.Data;
using System.ServiceModel;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Xml;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Framework.Settings;
using System.Linq;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Form that displays list of monitor config items.
    /// </summary>
    public partial class MonitorConfigForm : Form
    {
        private MonitorConfigDataSet _dataSet = new MonitorConfigDataSet();
        private AssetInventoryContext _context = DbConnect.AssetInventoryContext();

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorConfigForm"/> form.
        /// </summary>
        public MonitorConfigForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            monitor_DataGridView.AutoGenerateColumns = false;
        }

        private void MonitorConfigForm_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            monitor_DataGridView.DataSource = null;
            _dataSet.Clear();

            foreach (MonitorConfig monitor in _context.MonitorConfigs.OrderBy(n => n.ServerHostName))
            {
                _dataSet.AddRow(monitor);
            }

            monitor_DataGridView.DataSource = new BindingSource(_dataSet, "MonitorConfig");
        }

        private void new_ToolStripButton_Click(object sender, EventArgs e)
        {
            MonitorConfig configItem = new MonitorConfig()
            {
                MonitorConfigId = SequentialGuid.NewGuid(),
                MonitorType = string.Empty
            };

            SetCursor(true);
            if (DialogResult.OK == DisplayConfigurationEditor(configItem))
            {
                _context.MonitorConfigs.Add(configItem);
                _context.SaveChanges();
                RefreshServerService(configItem.ServerHostName);
                RefreshData();
            }
        }

        private void edit_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (monitor_DataGridView.SelectedRows.Count > 0)
            {
                SetCursor(true);
                DataRowView dataItem = (DataRowView)monitor_DataGridView.SelectedRows[0].DataBoundItem;
                Guid monitorConfigId = (Guid)dataItem[MonitorConfigColumn.MonitorConfigId];
                MonitorConfig configItem = _context.MonitorConfigs.FirstOrDefault(n => n.MonitorConfigId == monitorConfigId);

                if (DialogResult.OK == DisplayConfigurationEditor(configItem))
                {
                    _context.SaveChanges();
                    RefreshServerService(configItem.ServerHostName);
                    RefreshData();
                }
                SetCursor(false);
            }
        }

        private void RefreshServerService(string serverHostName)
        {
            if (serverHostName.EqualsIgnoreCase(Environment.MachineName))
            {
                // A remote monitor service will never be running on the same machine as this form.
                return;
            }

            try
            {
                using (var stfMonitorSvcCxn = STFMonitorServiceConnection.Create($"{serverHostName}.{GlobalSettings.Items[Setting.DnsDomain]}"))
                {
                    stfMonitorSvcCxn.Channel.RefreshConfig();
                }
            }
            catch (CommunicationException ex)
            {
                TraceFactory.Logger.Debug(ex.ToString());
                MessageBox.Show($"Failed to refresh service config on {serverHostName}.{Environment.NewLine}Manually refresh the STFMonitor service for changes to take effect.", "Refresh Server Service", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Monitor_DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                edit_ToolStripButton_Click(sender, e);
            }
        }

        private DialogResult DisplayConfigurationEditor(MonitorConfig configItem)
        {
            using (MonitorConfigDetailForm form = new MonitorConfigDetailForm(configItem))
            {
                SetCursor(false);
                return form.ShowDialog();
            }
        }

        private void remove_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (monitor_DataGridView.SelectedRows.Count > 0)
            {
                DataRowView dataItem = (DataRowView)monitor_DataGridView.SelectedRows[0].DataBoundItem;
                Guid monitorConfigId = (Guid)dataItem[MonitorConfigColumn.MonitorConfigId];
                MonitorConfig configItem = _context.MonitorConfigs.FirstOrDefault(n => n.MonitorConfigId == monitorConfigId);

                DialogResult dialogResult = MessageBox.Show
                (
                    $"Delete {configItem.MonitorType} monitor configuration item on {configItem.ServerHostName}?",
                    "Delete Monitor Configuration Item",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (DialogResult.Yes == dialogResult)
                {
                    _context.MonitorConfigs.Remove(configItem);
                    _context.SaveChanges();
                    RefreshServerService(configItem.ServerHostName);
                    RefreshData();
                }
            }
        }

        private void SetCursor(bool wait)
        {
            if (wait)
            {
                Cursor = Cursors.WaitCursor;
                return;
            }

            Cursor = Cursors.Default;
        }


        private class MonitorConfigDataSet : DataSet
        {
            public MonitorConfigDataSet()
                : base("MonitorConfig")
            {
                DataTable table = new DataTable("MonitorConfig");
                table.Columns.Add(new DataColumn(MonitorConfigColumn.MonitorConfigId, typeof(Guid)));
                table.Columns.Add(new DataColumn(MonitorConfigColumn.ServerHostName, typeof(string)));
                table.Columns.Add(new DataColumn(MonitorConfigColumn.MonitorType, typeof(string)));
                table.Columns.Add(new DataColumn(MonitorConfigColumn.Configuration, typeof(StfMonitorConfig)));

                table.PrimaryKey = new DataColumn[] { table.Columns["MonitorConfig"] };

                Tables.Add(table);
            }

            public void AddRow(MonitorConfig monitorConfig)
            {
                STFMonitorType monitorType;
                try
                {
                    monitorType = EnumUtil.Parse<STFMonitorType>(monitorConfig.MonitorType);
                }
                catch (ArgumentException)
                {
                    // If we can't parse the STFMonitorType, don't add it to the collection.
                    // For development, this will resolve itself by adding the new value to STFMonitorType enum.
                    return;
                }

                DataRow row = Tables[0].NewRow();
                row[MonitorConfigColumn.MonitorConfigId] = monitorConfig.MonitorConfigId;
                row[MonitorConfigColumn.ServerHostName] = monitorConfig.ServerHostName;
                row[MonitorConfigColumn.MonitorType] = monitorConfig.MonitorType;
                row[MonitorConfigColumn.Configuration] = StfMonitorConfigFactory.Create(monitorType, monitorConfig.Configuration);

                Tables[0].Rows.Add(row);
            }
        }

        private struct MonitorConfigColumn
        {
            public const string MonitorConfigId = "MonitorConfigId";
            public const string ServerHostName = "ServerHostName";
            public const string MonitorType = "MonitorType";
            public const string Configuration = "Configuration";
        }

    }
}
