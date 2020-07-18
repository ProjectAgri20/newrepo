using HP.ScalableTest.Core;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.LabConsole.Properties;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.LabConsole
{
    public partial class PluginMetadataEditForm : Form
    {
        private EnterpriseTestContext _context = null;
        private MetadataType _metadataType = null;
        private bool _isNewEntry;

        public PluginMetadataEditForm(MetadataType metadataType, EnterpriseTestContext context, bool isNewEntry)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            _metadataType = metadataType;
            _context = context;
            _isNewEntry = isNewEntry;

            if (_isNewEntry)
            {
                CheckForExisting();
            }
        }

        private void PluginMetadataEditForm_Load(object sender, EventArgs e)
        {
            resourceTypes_CheckedListBox.DisplayMember = "Name";

            foreach (var resource in _context.ResourceTypes)
            {
                if (EnumUtil.Parse<VirtualResourceType>(resource.Name).UsesPlugins())
                {
                    int index = resourceTypes_CheckedListBox.Items.Add(resource);

                    if (_metadataType.ResourceTypes.Any(x => x.Name.Equals(resource.Name)))
                    {
                        resourceTypes_CheckedListBox.SetItemChecked(index, true);
                    }
                }
            }

            foreach (string item in MetadataType.SelectGroups(_context))
            {
                group_ComboBox.Items.Add(item);
            }

            if (!string.IsNullOrEmpty(_metadataType.Group))
            {
                group_ComboBox.SelectedItem = _metadataType.Group;
            }


            pluginName_TextBox.DataBindings.Add("Text", _metadataType, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            pluginAssemblyName_TextBox.DataBindings.Add("Text", _metadataType, "AssemblyName", true, DataSourceUpdateMode.OnPropertyChanged);
            pluginTitle_TextBox.DataBindings.Add("Text", _metadataType, "Title", true, DataSourceUpdateMode.OnPropertyChanged);

            if (_metadataType.Icon != null)
            {
                icon_PictureBox.Image = ImageUtil.ReadImage(_metadataType.Icon);
            }
        }

        /// <summary>
        /// If there is an existing metadata type, allow the name textbox to be edited to resolve the conflict.
        /// Otherwise, the plugin name will be used and the user not allowed to change it.
        /// </summary>
        private void CheckForExisting()
        {
            if (_context.MetadataTypes.Any(m => m.Name.Equals(_metadataType.Name)))
            {
                pluginName_TextBox.ReadOnly = false;
            }
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            _metadataType.Name = pluginName_TextBox.Text;
            _metadataType.AssemblyName = pluginAssemblyName_TextBox.Text;
            _metadataType.Group = group_ComboBox.Text.Trim();
            if (icon_PictureBox.Image != null)
            {
                _metadataType.Icon = icon_PictureBox.Image.ToByteArray();
            }
            else
            {
                _metadataType.Icon = null;
            }

            _metadataType.ResourceTypes.Clear();
            foreach (var item in resourceTypes_CheckedListBox.CheckedItems.Cast<ResourceType>())
            {
                _metadataType.ResourceTypes.Add(item);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateInput()
        {
            MetadataType existingType = _context.MetadataTypes.Where(x => x.Name.Equals(_metadataType.Name)).FirstOrDefault();
            // Cannot create a new entry if the name already exists in the context.
            if (existingType != null && _isNewEntry)
            {
                MessageBox.Show(Resources.ExistingPlugin.FormatWith(existingType.Name, existingType.AssemblyName), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Must supply an assembly name.
            if (string.IsNullOrWhiteSpace(pluginAssemblyName_TextBox.Text))
            {
                MessageBox.Show(Resources.MissingAssemblyName, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Must supply at least one Applicable Reference.
            if (resourceTypes_CheckedListBox.CheckedItems.Count == 0)
            {
                MessageBox.Show(Resources.SelectApplicableResource, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void browseImages_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG|*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                icon_PictureBox.Image = Image.FromFile(ofd.FileName);
            }
        }
    }
}
