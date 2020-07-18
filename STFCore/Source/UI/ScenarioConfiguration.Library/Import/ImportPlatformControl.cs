using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class ImportPlatformControl : UserControl
    {
        private List<ResourceDetailBase> _resources = null;
        public event EventHandler OnAllItemsResolved;

        private class PlatformData : INotifyPropertyChanged
        {
            private ResourceDetailBase _resource = null;
            public event PropertyChangedEventHandler PropertyChanged;

            public PlatformData(ResourceDetailBase resource)
            {
                _resource = resource;
            }

            public string Name
            {
                get { return _resource.Name; }
                set { _resource.Name = value; }
            }

            public string Platform
            {
                get { return _resource.Platform; }
                set { _resource.Platform = value; }
            }

            public ResourceDetailBase Resource
            {
                get { return _resource; }
            }
        }

        public ImportPlatformControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;

            platformComboBox.DisplayMember = "Name";
            platformComboBox.ValueMember = "FrameworkClientPlatformId";

            assignPlatformGridView.CellValueChanged += ResolveDataGridView_CellValueChanged;
            assignPlatformGridView.CurrentCellDirtyStateChanged += ResolveDataGridView_CurrentCellDirtyStateChanged;
        }

        private void ResolveDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (assignPlatformGridView.IsCurrentCellDirty)
            {
                if (assignPlatformGridView.CurrentCell.EditedFormattedValue != null)
                {
                    assignPlatformGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        private void ResolveDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            CheckItemsResolved();

            assignPlatformGridView.Invalidate();
        }

        public bool CheckItemsResolved()
        {
            if (_resources != null && _resources.All(x => !string.IsNullOrEmpty(x.Platform)))
            {
                if (OnAllItemsResolved != null)
                {
                    OnAllItemsResolved(this, new EventArgs());
                }

                return true;
            }

            return false;
        }

        public void Reset()
        {
            _resources = null;
            assignPlatformGridView.DataSource = null;
            resourceTypeComboBox.SelectedIndexChanged -= resourceTypeComboBox_SelectedIndexChanged;
            resourceTypeComboBox.DataSource = null;
            resourceTypeComboBox.SelectedIndexChanged += resourceTypeComboBox_SelectedIndexChanged;
        }

        public void LoadResources(Collection<ResourceDetailBase> resources)
        {
            if (_resources == null)
            {
                _resources = resources.Where(x => string.IsNullOrEmpty(x.Platform)).ToList();
                assignPlatformGridView.AutoGenerateColumns = false;
                platformColumn.DisplayMember = "Name";
                platformColumn.ValueMember = "FrameworkClientPlatformId";
            }

            resourceTypeComboBox.DataSource = _resources.Select(x => x.ResourceType).Distinct().ToList();
        }


        private void resourceTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfigureGrid((VirtualResourceType)resourceTypeComboBox.SelectedItem);
        }

        private void ConfigureGrid(VirtualResourceType type)
        {
            assignPlatformGridView.DataSource = null;

            List<string> allowedPlatforms = new List<string>();
            string stringType = type.ToString();
            using (EnterpriseTestContext context = DbConnect.EnterpriseTestContext())
            {
                allowedPlatforms.AddRange(context.ResourceTypes.Find(stringType).FrameworkClientPlatforms.Select(n => n.FrameworkClientPlatformId));
            }

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                platformComboBox.Items.Clear();
                platformColumn.Items.Clear();


                var allPlatforms = context.FrameworkClientPlatforms.AsEnumerable();
                foreach (var platform in allPlatforms.Where(n => allowedPlatforms.Contains(n.FrameworkClientPlatformId, StringComparer.OrdinalIgnoreCase)))
                {
                    platformColumn.Items.Add(platform);
                    platformComboBox.Items.Add(platform);

                }
            }

            var platforms = new SortableBindingList<PlatformData>();
            foreach (var resource in _resources.Where(x => x.ResourceType.Equals(type)))
            {
                platforms.Add(new PlatformData(resource));
            }

            assignPlatformGridView.DataSource = platforms;

            CheckItemsResolved();
        }

        private void assignPlatformGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void applyToAllButton_Click(object sender, EventArgs e)
        {
            var platform = platformComboBox.SelectedItem as FrameworkClientPlatform;


            foreach (var data in assignPlatformGridView.Rows.Cast<DataGridViewRow>().Select(x => x.DataBoundItem as PlatformData))
            {
                data.Platform = platform.FrameworkClientPlatformId;
            }

            assignPlatformGridView.Refresh();
            CheckItemsResolved();
        }

        private void assignPlatformGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool validRow = (e.RowIndex != -1);
            var assignPlatformGridView = sender as DataGridView;

            // Check to make sure the cell clicked is the cell containing the combobox 
            if (assignPlatformGridView.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validRow)
            {
                assignPlatformGridView.BeginEdit(true);
                ((ComboBox)assignPlatformGridView.EditingControl).DroppedDown = true;
            }
        }
    }
}
