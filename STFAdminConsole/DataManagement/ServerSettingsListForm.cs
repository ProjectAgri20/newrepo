using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Lists the ServerSettings for a given server.
    /// </summary>
    public partial class ServerSettingsListForm : Form
    {
        private string _serverHostName = string.Empty;
        private SortableBindingList<ServerSetting> _settings = null;

        /// <summary>
        /// Creates a new instance of ServerSettingsListForm.
        /// </summary>
        /// <param name="serverHostName">The host name of the server.</param>
        public ServerSettingsListForm(string serverHostName)
        {
            InitializeComponent();

            _settings = new SortableBindingList<ServerSetting>();

            UserInterfaceStyler.Configure(settings_RadGridView, GridViewStyle.ReadOnly);
            settings_RadGridView.AllowRowResize = true;
            _serverHostName = serverHostName;
        }

        private void ServerSettingsListForm_Load(object sender, EventArgs e)
        {
            using (new BusyCursor())
            {
                BindSettingsToGrid();
            }
        }

        private void RefreshSettings(AssetInventoryContext context)
        {
            if (_settings != null)
            {
                _settings.Clear();
            }

            foreach (ServerSetting setting in context.FrameworkServers.Where(n => n.HostName == _serverHostName).SelectMany(n => n.ServerSettings))
            {
                _settings.Add(setting);
            }
        }

        private void BindSettingsToGrid()
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                BindSettingsToGrid(context);
            }
        }

        private void BindSettingsToGrid(AssetInventoryContext context)
        {
            RefreshSettings(context);

            settings_RadGridView.DataSource = null;
            settings_RadGridView.DataSource = _settings;

            settings_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            settings_RadGridView.BestFitColumns();

            if (settings_RadGridView.Rows.Count > 0)
            {
                settings_RadGridView.TableElement.ScrollToRow(0);
            }
        }

        private void add_ToolStripButton_Click(object sender, EventArgs e)
        {
            ServerSetting setting = null;
            try
            {
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    FrameworkServer server = context.FrameworkServers.FirstOrDefault(n => n.HostName.StartsWith(_serverHostName));
                    if (server == null)
                    {
                        MessageBox.Show("Save the Server Properties before adding a Server Setting.", "Server Properties not saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    setting = new ServerSetting()
                    {
                        FrameworkServerId = server.FrameworkServerId,
                        Name = string.Empty,
                        Value = string.Empty
                    };

                    if (EditSetting(setting, true) == DialogResult.OK)
                    {
                        server.ServerSettings.Add(setting);
                        context.SaveChanges();
                    }

                    BindSettingsToGrid(context);
                }
            }
            catch (UpdateException)
            {
                MessageBox.Show($"Setting Name '{setting.Name}' already exists.", "Save Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void edit_ToolStripButton_Click(object sender, EventArgs e)
        {
            GridViewRowInfo row = GetFirstSelectedRow();
            if (row != null)
            {
                ServerSetting setting = row.DataBoundItem as ServerSetting;

                if (EditSetting(setting, false) == DialogResult.OK)
                {
                    using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                    {
                        ServerSetting dbSetting = context.FrameworkServers.First(n => n.FrameworkServerId == setting.FrameworkServerId).ServerSettings.FirstOrDefault(n => n.Name == setting.Name);
                        dbSetting.Value = setting.Value;
                        dbSetting.Description = setting.Description;
                        context.SaveChanges();
                        
                        BindSettingsToGrid(context);                     
                    }
                }                
            }
        }

        private void remove_ToolStripButton_Click(object sender, EventArgs e)
        {
            GridViewRowInfo row = GetFirstSelectedRow();
            if (row != null)
            {
                ServerSetting setting = row.DataBoundItem as ServerSetting;

                DialogResult dialogResult = MessageBox.Show
                    (
                        $"Removing setting '{setting.Name}'.  Do you want to continue?",
                        "Delete Setting",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question

                    );

                if (dialogResult == DialogResult.Yes)
                {
                    using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                    {
                        FrameworkServer server = context.FrameworkServers.First(n => n.FrameworkServerId == setting.FrameworkServerId);
                        server.ServerSettings.Remove(server.ServerSettings.First(n => n.Name == setting.Name));
                        context.SaveChanges();

                        BindSettingsToGrid(context);
                    }
                }
            }
        }

        private void settings_RadGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            edit_ToolStripButton_Click(sender, EventArgs.Empty);
        }

        private DialogResult EditSetting(ServerSetting setting, bool addingNew)
        {
            DialogResult result = DialogResult.None;

            using (ServerSettingsEditForm form = new ServerSettingsEditForm(setting, addingNew))
            {
                result = form.ShowDialog();
            }

            return result;
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return settings_RadGridView.SelectedRows.FirstOrDefault();
        }


    }
}
