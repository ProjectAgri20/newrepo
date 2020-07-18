using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using System.ServiceProcess;
using HP.ScalableTest.Framework.Settings;
using System.Net.NetworkInformation;
using HP.ScalableTest.Data.EnterpriseTest;

namespace HP.ScalableTest.LabConsole.ResourceConfiguration
{
    public partial class ResourceWindowsConfigurationForm : Form
    {
        private string _domain = GlobalSettings.Items[Setting.DnsDomain].ToLowerInvariant();
        private List<string> _associatedServices = new List<string>();

        /// <summary>
        /// Constructor to add services to existing resource
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="associatedServices"></param>
        public ResourceWindowsConfigurationForm(ResourceWindowsCategory resource, List<string> associatedServices)
        {
            InitializeComponent();
            try
            {
                _associatedServices = associatedServices;
                SetupForm(resource);
            }
            catch
            {
                this.Close();
            }
        }

        /// <summary>
        /// Constructor to add new resource
        /// </summary>
        public ResourceWindowsConfigurationForm(string categoryType)
        {
            InitializeComponent();
            services_GridView.AutoGenerateColumns = false;
            customSelection_GroupBox.Enabled = false;
            resourceType_TextBox.Text = categoryType;
        }

        /// <summary>
        /// The Resource.
        /// </summary>
        public ResourceWindowsCategory Resource { get; private set; }

        /// <summary>
        /// The Type.
        /// </summary>
        public string CategoryType { get { return resourceType_TextBox.Text; } }

        /// <summary>
        /// Initial Form Setup
        /// </summary>
        /// <param name="resource"></param>
        private void SetupForm(ResourceWindowsCategory resource)
        {
            resourceType_TextBox.Text = resource.CategoryType;
            serverName_TextBox.Text = resource.Name;
            services_GridView.AutoGenerateColumns = false;
            resource_GroupBox.Enabled = false;

            updateServerGrid(ValidateServerName(resource.Name));
            
        }
    
        private void add_Button_Click(object sender, EventArgs e)
        {
            // End grid edit mode
            services_GridView.EndEdit();

            // Validate resource name is not blank
            if(!ValidateResourceTypeName())
            {
                MessageBox.Show("Pleae enter valid resource name", "Invalid resource name", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Get checkbox services and custom service
            List<string> servicesToAdd = getSelectedServices();
            if (getCustomService() != null)
            {
                servicesToAdd.Add(getCustomService());
            }

            // Add and link services to server and resource type
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {

                // Get resource type and server name
                string categoryType = resourceType_TextBox.Text;
                string validatedServerName = serverName_TextBox.Text;

                int serverId = ResourceWindowsCategory.AddResource(context, validatedServerName, categoryType);
                ResourceWindowsCategory serverResource = ResourceWindowsCategory.SelectById(context, serverId);

                foreach (string service in servicesToAdd)
                {
                    int tempId = ResourceWindowsCategory.AddResource(context, service, categoryType);
                    ResourceWindowsCategory tempService = ResourceWindowsCategory.SelectById(context, tempId);
                    tempService.Parents.Add(serverResource);
                }

                context.SaveChanges();
                Resource = serverResource;
                resourceType_TextBox.Text = categoryType;
                this.DialogResult = DialogResult.OK;
                this.Close(); 
            }

        }
        private void addCustomService_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            serviceName_Label.Enabled = addCustomService_RadioButton.Checked;
            serviceName_TextBox.Enabled = addCustomService_RadioButton.Checked;
        }

        private void retrieveServicesList_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            services_GridView.Enabled = retrieveServicesList_RadioButton.Checked;
            services_GridView.Visible = retrieveServicesList_RadioButton.Checked;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void serverValidation_Button_Click(object sender, EventArgs e)
        {
            string server = ValidateServerName(serverName_TextBox.Text);

            if (isServerReachable(server))
            {
                updateServerGrid(server);
                customSelection_GroupBox.Enabled = true;
            }
        }

        /// <summary>
        /// If server is not an IP Address (contains '.') append _domainName 
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        private string ValidateServerName(string server)
        {
            if (!server.Contains("."))
            {
                return server + "." + _domain;
            }
            else
                return server;
        }
        private bool ValidateResourceTypeName()
        {
            if (string.IsNullOrWhiteSpace(resourceType_TextBox.Text))
            {
                return false;
            }

            return true;
        }
        private bool isServerReachable(string hostName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Ping ping = new Ping();
                PingReply reply = ping.Send(hostName, 10000);
                if (reply.Status == IPStatus.Success)
                {
                    this.Cursor = Cursors.Default;
                    return true;
                }
            }
            catch { } //Swallow any errors and assume hostName is not reachable.

            this.Cursor = Cursors.Default;
            string errorMessage = "Server: " + hostName + " ping unsuccessful.  Server is not available.";
            MessageBox.Show(errorMessage, "Server Unresponsive", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            return false;
        }

        private void updateServerGrid(string serverName)
        {
            try
            {
                List<ServiceController> sortedList = ServiceController.GetServices(serverName).OrderBy(x => x.DisplayName).ToList();
                List<ServiceController> tempList = new List<ServiceController>();
                foreach (ServiceController service in sortedList)
                {
                    if (!_associatedServices.Contains(service.DisplayName))
                    {
                        tempList.Add(service);
                    }
                }
                services_GridView.DataSource = tempList;

            }
            catch
            {
                string errorMessage = "Server: " + serverName + " cannot be reached.";
                MessageBox.Show(errorMessage, "Server Access Failure", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private List<string> getSelectedServices()
        {
            List<string> selectedServices = new List<string>();
            foreach (DataGridViewRow row in services_GridView.Rows)
            {
                DataGridViewCheckBoxCell cell = row.Cells["servicesCheckBox_Column"] as DataGridViewCheckBoxCell;
                if ((cell != null && cell.Value != null && Convert.ToBoolean(cell.Value)))
                {

                    selectedServices.Add(row.Cells["displayName_Column"].Value.ToString());
                }
            }

            return selectedServices;
        }

        /// <summary>
        /// Gets custom service name if custom service button is checked 
        /// and text box is not blank or null
        /// </summary>
        /// <returns></returns>
        private string getCustomService()
        {
            string serviceName = serviceName_TextBox.Text;
            //string blank = serverName.Replace(" ", "");

            if(!string.IsNullOrWhiteSpace(serviceName))
            {
                return serviceName_TextBox.Text;
            }
            else
                return null;

        }

      
    }
}
