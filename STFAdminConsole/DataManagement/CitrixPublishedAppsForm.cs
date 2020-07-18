using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Class CitrixPublishedAppsForm used for managing published applications on Citrix Servers
    /// </summary>
    public partial class CitrixPublishedAppsForm : Form
    {
        private AssetInventoryContext _context = null;
        private Dictionary<String, SortableBindingList<StringValue>> _publishedApps = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CitrixPublishedAppsForm"/> class.
        /// </summary>
        public CitrixPublishedAppsForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            _context = DbConnect.AssetInventoryContext();
            _publishedApps = new Dictionary<string, SortableBindingList<StringValue>>();
        }

        /// <summary>
        /// Populates the dropdown with Citrix Server hostnames and associated application names.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CitrixPublishedAppsForm_Load(object sender, EventArgs e)
        {
            publishedApp_DataGridView.AutoGenerateColumns = false;

            citrixServer_ComboBox.DisplayMember = "HostName";

            IEnumerable<FrameworkServer> servers = null;

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                string serverType = ServerType.Citrix.ToString();
                servers = context.FrameworkServers.Where(n => n.ServerTypes.Any(m => m.Name == serverType) && n.Active).OrderBy(n => n.HostName).ToList();
                foreach (FrameworkServer server in servers)
                {
                    _publishedApps.Add(server.HostName, new SortableBindingList<StringValue>());
                    foreach (var app in _context.CitrixPublishedApps.Where(n => n.CitrixServer == server.HostName).Select(n => n.ApplicationName).Distinct())
                    {
                        _publishedApps[server.HostName].Add(new StringValue(app));
                    }
                }

                citrixServer_ComboBox.DataSource = servers;
            }
        }

        /// <summary>
        /// Updates the Citrix published application table from the DataSource and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void publishedApp_OK_Click(object sender, EventArgs e)
        {
            UpdateApplications(_context, _publishedApps);
            Close();
        }
        /// <summary>
        /// /// Updates the CitrixPublishedApp table from the DataSource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void publishedApp_Apply_Click(object sender, EventArgs e)
        {
            UpdateApplications(_context, _publishedApps);
        }

        /// <summary>
        /// Cancel the operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void publishedApp_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Changes the Datagrid based on product name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void citrixServer_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = citrixServer_ComboBox.SelectedItem as FrameworkServer;

            publishedApp_DataGridView.DataSource = _publishedApps[item.HostName];

            publishedApp_DataGridView.Refresh();
        }

        /// <summary>
        /// Cell Validation event to allow only valid characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productName_dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            String text = (String)e.FormattedValue;
            if ((text.Length == 0) || !Regex.IsMatch(text, "^[A-Za-z0-9]*$"))
            {
                publishedApp_DataGridView.Rows[e.RowIndex].ErrorText = "Enter valid characters";
                e.Cancel = true;
            }
            else
            {
                publishedApp_DataGridView.Rows[e.RowIndex].ErrorText = "";
            }
        }

        private static void UpdateApplications(AssetInventoryContext entities, Dictionary<String, SortableBindingList<StringValue>> applicationNames)
        {
            AddApplications(entities, applicationNames);
            DeleteApplications(entities, applicationNames);
        }

        private static void AddApplications(AssetInventoryContext entities, Dictionary<String, SortableBindingList<StringValue>> applicationNames)
        {
            //From the data source retrieve each entry and check if it exists in table. If no entry found, add an entry.
            foreach (KeyValuePair<String, SortableBindingList<StringValue>> appNamePair in applicationNames)
            {
                foreach (StringValue applicationName in appNamePair.Value)
                {
                    //Add all the entries if the name does not exist in the table
                    if (!ApplicationNameExists(entities, appNamePair.Key, applicationName.Value))
                    {
                        CitrixPublishedApp app = new CitrixPublishedApp();
                        app.CitrixServer = appNamePair.Key;
                        app.ApplicationName = applicationName.Value;

                        entities.CitrixPublishedApps.Add(app);
                    }
                }
            }

            entities.SaveChanges();
        }

        private static bool ApplicationNameExists(AssetInventoryContext entities, string hostName, string name)
        {
            return entities.CitrixPublishedApps.Any(n => n.CitrixServer == hostName && n.ApplicationName == name);
        }

        private static void DeleteApplications(AssetInventoryContext entities, Dictionary<String, SortableBindingList<StringValue>> applicationNames)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            if (applicationNames == null)
            {
                throw new ArgumentNullException("applicationNames");
            }

            foreach (CitrixPublishedApp app in entities.CitrixPublishedApps)
            {
                SortableBindingList<StringValue> appList = null;

                // Remove each entry in the system that is found in the target applicationNames by hostname
                // but where the application names is not included in the target set of application names
                // for that host.
                if (applicationNames.TryGetValue(app.CitrixServer, out appList) && !appList.Any(n => n.Value.EqualsIgnoreCase(app.ApplicationName)))
                {
                    entities.CitrixPublishedApps.Remove(app);
                }
            }

            entities.SaveChanges();
        }
    }
}
