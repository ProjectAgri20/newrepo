using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.LabConsole.ResourceConfiguration
{
    /// <summary>
    /// UI component for managing ResourceWindowsCategory items.
    /// </summary>
    public partial class ResourceWindowsCategoryAddForm : Form
    {
        //_domain used if hostname is not an IP Address
        string _domain = GlobalSettings.Items[Setting.DnsDomain];

        /// <summary>
        /// Instantiates a new instance of ResourceWindowsCategoryAddForm.
        /// </summary>
        public ResourceWindowsCategoryAddForm()
        {
            InitializeComponent();
            children_GridView.AutoGenerateColumns = false;
            LoadResourceTypeTabs();
            tabControl_Types_SelectedIndexChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Returns the selected tab.
        /// </summary>
        private TabPage SelectedTab
        {
            get { return tabControl_Types.SelectedTab; }
        }

        private void LoadResourceTypeTabs()
        {
            // Move all controls out of the first tab before clearing the tabs collection
            listBox_Resource.Parent = this;
            resource_ToolStrip.Parent = this;
            groupBox_Assoc.Parent = this;
            tabControl_Types.TabPages.Clear();

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                foreach (string resourceType in ResourceWindowsCategory.SelectDistinctCategoryTypes(context))
                {
                    tabControl_Types.TabPages.Add(resourceType, resourceType);
                }

            }
        }
        
        /// <summary>
        /// When a resource type is selected the resource list box is populated based on its value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_Types_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selectedTab = SelectedTab;

            if (selectedTab != null)
            {
                // Add all controls into the selected tab
                listBox_Resource.Parent = selectedTab;
                resource_ToolStrip.Parent = selectedTab;
                groupBox_Assoc.Parent = selectedTab;

                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    listBox_Resource.DataSource = ResourceWindowsCategory.SelectParent(context, selectedTab.Text).ToList();
                }
            }
        }

        private void listBox_Resource_SelectedValueChanged(object sender, EventArgs e)
        {
            int selectedId = (int)listBox_Resource.SelectedValue;
            Update_GridView(selectedId);
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addResource_Button_Click(object sender, EventArgs e)
        {
            using (ResourceWindowsConfigurationForm addForm = new ResourceWindowsConfigurationForm(tabControl_Types.SelectedTab.Text))
            {
                addForm.ShowDialog();
                if (addForm.DialogResult == DialogResult.OK)
                {
                    ResourceWindowsCategory newServer = addForm.Resource;
                    string newCategoryType = addForm.CategoryType;
                    LoadResourceTypeTabs();
                    tabControl_Types.SelectedTab = tabControl_Types.TabPages[newServer.CategoryType];
                    listBox_Resource.SelectedItem = newServer.Name;
                    Update_GridView((int)listBox_Resource.SelectedValue);
                }
            }                
        }

        private void removeResource_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deletion is final, are you sure you want to remove?", "Removal confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    ResourceWindowsCategory resource = ResourceWindowsCategory.SelectByName(context, listBox_Resource.Text, tabControl_Types.SelectedTab.Text);
                    if (resource.Children.Count > 0)
                    {
                        MessageBox.Show("Please remove all associations before deleting '{0}'.".FormatWith(resource.Name), "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    context.ResourceWindowsCategories.DeleteObject(resource);
                    context.SaveChanges();

                    listBox_Resource.DataSource = ResourceWindowsCategory.SelectParent(context, SelectedTab.Text).ToList();
                }
            }
        }

        private void addAssociated_Button_Click(object sender, EventArgs e)
        {
            List<string> associatedServices = new List<string>();
            List<ResourceWindowsCategory> services = new List<ResourceWindowsCategory>();
            ResourceWindowsCategory component = new ResourceWindowsCategory();
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                component = ResourceWindowsCategory.SelectById(context, (int)listBox_Resource.SelectedValue);
                services = ResourceWindowsCategory.SelectByParent(context, (int)listBox_Resource.SelectedValue);
            }

            foreach(ResourceWindowsCategory service in services)
            {
                associatedServices.Add(service.Name);
            }

            try
            {
                if (pingServer(component.Name))
                {
                    ResourceWindowsConfigurationForm addForm = new ResourceWindowsConfigurationForm(component, associatedServices);
                    addForm.ShowDialog();
                    if (addForm.DialogResult == DialogResult.OK)
                    {
                        ResourceWindowsCategory newServer = addForm.Resource;
                        string newCategoryType = addForm.CategoryType;
                        LoadResourceTypeTabs();
                        tabControl_Types.SelectedTab = tabControl_Types.TabPages[newServer.CategoryType];
                        listBox_Resource.SelectedItem = newServer.Name;
                        Update_GridView((int)listBox_Resource.SelectedValue);

                    }
                }
            }
            catch
            {
                string errorMessage = "Error: No access to " + component.Name;
                MessageBox.Show(errorMessage, "Server Access Failure", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        
        private void removeAssociated_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deletion is final, are you sure you want to remove?", "Removal confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                children_GridView.EndEdit();
                List<string> services = getSelectedServices();
                foreach (string service in services)
                {
                    using (EnterpriseTestContext context = new EnterpriseTestContext())
                    {
                        // Get service to remove from parent
                        ResourceWindowsCategory serviceToDelete = ResourceWindowsCategory.SelectByName(context, service, tabControl_Types.SelectedTab.Text);

                        //Get Parent
                        ResourceWindowsCategory parent = ResourceWindowsCategory.SelectByName(context, listBox_Resource.Text, tabControl_Types.SelectedTab.Text);

                        //Remove Parent Child relationship
                        ResourceWindowsCategory.RemoveChild(context, parent.CategoryId, serviceToDelete.CategoryId);

                        context.SaveChanges();

                        // Update Grid
                        children_GridView.DataSource = null;
                        children_GridView.DataSource = ResourceWindowsCategory.SelectByParent(context, (int)listBox_Resource.SelectedValue);
                    }
                }
            }
             
        }

        private List<string> getSelectedServices()
        {
            List<string> selectedServices = new List<string>();
            foreach (DataGridViewRow row in children_GridView.Rows)
            {
                DataGridViewCheckBoxCell cell = row.Cells["servicesCheckBox_Column"] as DataGridViewCheckBoxCell;
                if ((cell != null && cell.Value != null && Convert.ToBoolean(cell.Value)))
                {

                    selectedServices.Add(row.Cells["name_Column"].Value.ToString());
                }
            }

            return selectedServices;

        }

        private bool pingServer(string hostName)
        {
            string serverName = validateServerName(hostName);

            Ping ping = new Ping();
            PingReply reply = ping.Send(serverName, 10000);
            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            else
                return false;
        }

        private void Update_GridView(int selectedId)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                children_GridView.DataSource = ResourceWindowsCategory.SelectByParent(context, selectedId);
            }
        }

        private string validateServerName(string server)
        {
            if (!server.Contains("."))
            {
                return server + "." + _domain;
            }
            else
                return server;
        }

    }
}
