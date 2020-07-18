using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using System.ComponentModel;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class AssignPlatformDialog : Form
    {
        private IEnumerable<VirtualResource> _resources = null;

        public class Item : INotifyPropertyChanged
        {
            private VirtualResource _resource = null;

            public event PropertyChangedEventHandler PropertyChanged;

            public Item(VirtualResource resource)
            {
                _resource = resource;
            }

            public string Name
            {
                get { return _resource.Name; }
                set { }
            }

            public string Platform
            { 
                get { return _resource.Platform; }
                set 
                { 
                    _resource.Platform = value; 

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Platform"));
                    }
                }
            }

            public VirtualResource Resource
            {
                get { return _resource;  }
            }
        }

        public AssignPlatformDialog(IEnumerable<VirtualResource> resources)
        {
            InitializeComponent();

            // Clear the platform value to nothing before databinding to avoid
            // and error where the value is not valid with respect to the 
            // dropdown combobox.
            foreach (var resource in resources)
            {
                resource.Platform = string.Empty;
            }

            _resources = resources;
            assignPlatformGridView.AutoGenerateColumns = false;
            platformColumn.DisplayMember = "Name";
            platformColumn.ValueMember = "FrameworkClientPlatformId";
        }


        private void AssignPlatformDialog_Load(object sender, EventArgs e)
        {
            var types = _resources.Select(x => x.ResourceType).Distinct().ToList();
            resourceTypeComboBox.DataSource = types;
        }

        private void resourceTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfigureGrid((string)resourceTypeComboBox.SelectedItem);
        }

        private void ConfigureGrid(string type)
        {
            assignPlatformGridView.DataSource = null;

            List<string> allowedPlatforms = new List<string>();
            using (var context = DbConnect.EnterpriseTestContext())
            {
                allowedPlatforms.AddRange(context.ResourceTypes.Find(type).FrameworkClientPlatforms.Select(n => n.FrameworkClientPlatformId));
            }

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                platformColumn.Items.Clear();

                var allPlatforms = context.FrameworkClientPlatforms.AsEnumerable();
                foreach (var platform in allPlatforms.Where(n => allowedPlatforms.Contains(n.FrameworkClientPlatformId, StringComparer.OrdinalIgnoreCase)))
                {
                    platformColumn.Items.Add(platform);
                }
            }

            SortableBindingList<Item> items = new SortableBindingList<Item>();
            foreach (var item in _resources.Where(x => x.ResourceType.Equals(type)))
            {
                items.Add(new Item(item));
            }

            assignPlatformGridView.DataSource = items;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (_resources.Any(x => string.IsNullOrEmpty(x.Platform)))
            {
                var result = MessageBox.Show("Not all resources have a Platform assigned.  Do you want to continue?", "Missing Platform Assignments", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
