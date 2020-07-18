using HP.ScalableTest;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;
using EnterpriseTestContext = HP.ScalableTest.Data.EnterpriseTest.EnterpriseTestContext;

namespace HP.RDL.EDT.AutoTestHelper
{
    /// <summary>
    /// Interaction logic for BulkEditorWindow.xaml
    /// </summary>
    public partial class BulkEditorWindow : Window, IDisposable
    {
        private readonly List<ScenarioQueueItem> _activeScenarios;
        private readonly EnterpriseTestContext _dataContext;
        private static readonly Collection<Assembly> PluginCache = new Collection<Assembly>();

        //represents the changes in the format of plugin name, (plugin data member, value)
        private readonly Dictionary<string, Dictionary<string, string>> _pluginChangesDictionary = new Dictionary<string, Dictionary<string, string>>();

        private ServerComboBox _stfServerComboBox;
        private static string RootPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private const string ApplicationName = "EDT AutoTestHelper";
        public BulkEditorWindow()
        {
            InitializeComponent();
            _activeScenarios = new List<ScenarioQueueItem>();
        }

        public BulkEditorWindow(List<ScenarioQueueItem> activeScenarios)
        {
            _activeScenarios = activeScenarios;
            InitializeComponent();

            _dataContext = new EnterpriseTestContext(STFLoginManager.SystemDatabase);
            PluginComboBox.ItemsSource = _dataContext.MetadataTypes.ToList();

            ScenarioDataGrid.DataContext = activeScenarios;

            _stfServerComboBox = FrameworkServerHost.Child as ServerComboBox;
            _stfServerComboBox?.Initialize();
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MetadataTreeView.SelectedItem == null || (string.IsNullOrEmpty(MetadataMemberTextBox.Text) && MetadataMemberCombobox.SelectedIndex == -1))
            {
                return;
            }

            var selectedTreeNode = MetadataTreeView.SelectedItem as TreeNode;

            if (selectedTreeNode == null)
                return;

            //if the user has selected the class item, just skip and go back
            if (selectedTreeNode.IsClass)
                return;

            if (selectedTreeNode.EnumValueCollection.Count == 0)
            {
                if (!ValidateInput(MetadataMemberTextBox.Text, selectedTreeNode.PropertyType))
                {
                    MessageBox.Show($"Entered data does not match required type. Please enter the data of format {selectedTreeNode.PropertyType}", ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            selectedTreeNode.Text = selectedTreeNode.EnumValueCollection.Count > 0 ? MetadataMemberCombobox.SelectedItem.ToString() : MetadataMemberTextBox.Text;
            selectedTreeNode.Modified = true;

            Dictionary<string, string> pluginChangeDictionary;
            _pluginChangesDictionary.TryGetValue(PluginComboBox.Text, out pluginChangeDictionary);

            if (pluginChangeDictionary == null || pluginChangeDictionary.Count == 0)
            {
                pluginChangeDictionary =
                    new Dictionary<string, string> { { selectedTreeNode.Name, selectedTreeNode.Text } };
            }
            else
            {
                pluginChangeDictionary[selectedTreeNode.Name] = selectedTreeNode.Text;
            }

            if (_stfServerComboBox.HasSelection)
            {
                var serverSelection = new ServerSelectionData(_stfServerComboBox.SelectedServer);
                pluginChangeDictionary["FrameworkServer"] = Serializer.Serialize(serverSelection).ToString();
            }

            _pluginChangesDictionary[PluginComboBox.Text] = pluginChangeDictionary;
        }

        private bool ValidateInput(string text, string propertyType)
        {
            try
            {
                Type type = Type.GetType(propertyType);
                if (type == null)
                    return false;

                var converter = TypeDescriptor.GetConverter(type);
                var result = converter.ConvertFrom(text);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_pluginChangesDictionary.Count == 0)
                return;

            Parallel.ForEach(_pluginChangesDictionary.Keys, BulkEditMetadata);

            var count = _dataContext.SaveChanges();
            MessageBox.Show($"Bulk Editor made {count} changes to {_activeScenarios.Count} scenario(s).", ApplicationName);
        }

        private void BulkEditMetadata(string pluginName)
        {
            Dictionary<string, string> pluginChangeDictionary;
            _pluginChangesDictionary.TryGetValue(pluginName, out pluginChangeDictionary);

            if (pluginChangeDictionary == null)
                return;

            foreach (var activeScenario in _activeScenarios)
            {
                var virtualResources =
                    _dataContext.VirtualResources.Where(x => x.EnterpriseScenarioId == activeScenario.ScenarioId);

                foreach (var virtualResource in virtualResources)
                {
                    var pluginMetadataList = virtualResource.VirtualResourceMetadataSet.Where(x => x.MetadataType == pluginName).ToList();
                    foreach (var pluginMetadata in pluginMetadataList)
                    {
                        var metadataElement = XElement.Parse(pluginMetadata.Metadata);
                        foreach (var pluginDataMemberKey in pluginChangeDictionary.Keys)
                        {
                            var dataMemberElement = metadataElement.Descendants(metadataElement.GetDefaultNamespace() + pluginDataMemberKey).FirstOrDefault();
                            if (dataMemberElement != null)
                                dataMemberElement.Value = pluginChangeDictionary[pluginDataMemberKey];
                        }

                        pluginMetadata.Metadata = metadataElement.ToString();
                        var pluginServerUsage = _dataContext.VirtualResourceMetadataServerUsages.FirstOrDefault(x =>
                            x.VirtualResourceMetadataId == pluginMetadata.VirtualResourceMetadataId);

                        if (pluginChangeDictionary.ContainsKey("FrameworkServer"))
                            pluginServerUsage.ServerSelectionData = pluginChangeDictionary["FrameworkServer"];
                    }
                }
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PluginComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PluginComboBox.SelectedIndex == -1)
                return;

            try
            {
                LoadPlugin(PluginComboBox.SelectedValue.ToString());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, ApplicationName);
            }
        }

        private void LoadPlugin(string pluginName)
        {
            var relativePath = GlobalSettings.Items[Setting.PluginRelativeLocation];
            var location = ParsePath(RootPath, relativePath, pluginName);

            var pluginAssembly = PluginCache.FirstOrDefault(x => x.Location.Equals(location));
            if (pluginAssembly == null)
            {
                pluginAssembly = GetAssembly(location);

                if (pluginAssembly != null)
                {
                    PluginCache.Add(pluginAssembly);
                }
                else
                {
                    throw new Exception("The plugin Assembly '{0}' was not found.".FormatWith(location));
                }
            }
            ReadAssembly(pluginName);
        }

        private void ReadAssembly(string assemblyName)
        {
            var pluginAssembly = PluginCache.FirstOrDefault(x => x.ManifestModule.Name == assemblyName);

            var pluginTree = new TreeNode(pluginAssembly.GetName().Name);

            var types = pluginAssembly.DefinedTypes.Where(x => x.CustomAttributes.Any(n => n.AttributeType == typeof(DataContractAttribute)));
            foreach (var typeInfo in types)
            {
                if (typeInfo.IsClass)
                {
                    var pluginClassNode = new TreeNode(typeInfo.Name) { IsClass = true };
                    ReadDataMembers(typeInfo, pluginClassNode);
                    pluginTree.Add(pluginClassNode);
                }
            }

            MetadataTreeView.ItemsSource = pluginTree;
        }

        private void ReadDataMembers(TypeInfo className, TreeNode pluginTreeNode)
        {
            var datamembers = className.GetProperties().Where(x => x.CustomAttributes.Any(n => n.AttributeType == typeof(DataMemberAttribute)));
            foreach (var propertyInfo in datamembers)
            {
                if (propertyInfo.PropertyType.IsValueType && !propertyInfo.PropertyType.IsPrimitive && !propertyInfo.PropertyType.IsEnum)
                {
                    pluginTreeNode.Add(new TreeNode(propertyInfo.Name) { PropertyType = propertyInfo.PropertyType.FullName, IsReadOnly = true });
                }
                else if (propertyInfo.PropertyType.IsEnum)
                {
                    pluginTreeNode.Add(new TreeNode(propertyInfo.Name)
                    {
                        PropertyType = propertyInfo.PropertyType.FullName,
                        EnumValueCollection = Enum.GetNames(propertyInfo.PropertyType).ToList()
                    });
                }
                else
                {
                    pluginTreeNode.Add(new TreeNode(propertyInfo.Name) { PropertyType = propertyInfo.PropertyType.FullName });
                }
            }
        }

        private static Assembly GetAssembly(string filePath)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(filePath);
            }
            catch (FileLoadException flEx) // filePath is not correct or incomplete
            {
                TraceFactory.Logger.Error(flEx.ToString());
                throw new FileNotFoundException("Incorrect plugin filePath: {0}".FormatWith(filePath), flEx);
            }
            catch (FileNotFoundException fnfEx) // filePath does not exist.
            {
                TraceFactory.Logger.Error(fnfEx.ToString());
                throw new FileNotFoundException("The plugin Assembly was not found: {0}".FormatWith(filePath), fnfEx);
            }

            return assembly;
        }

        private static string ParsePath(params string[] paths)
        {
            var info = new FileInfo(Path.Combine(paths));
            return Path.Combine(info.DirectoryName, info.Name);
        }

        private void MetadataTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedTreeNode = MetadataTreeView.SelectedItem as TreeNode;
            if (selectedTreeNode == null)
                return;

            DataMemberStackPanel.DataContext = selectedTreeNode;

            if (selectedTreeNode.EnumValueCollection.Count > 0)
            {
                MetadataMemberTextBox.Text = string.Empty;
                MetadataMemberCombobox.SelectedItem = selectedTreeNode.Text;
            }
        }

        public void Dispose()
        {
            _dataContext.Dispose();
            PluginCache.Clear();
            _activeScenarios.Clear();
        }
    }

    public class TreeNode : IEnumerable<TreeNode>, INotifyPropertyChanged
    {
        private readonly Dictionary<string, TreeNode> _children =
            new Dictionary<string, TreeNode>();

        public readonly string Id;

        public string Text { get; set; }

        private bool _modified;

        public bool Modified
        {
            get { return _modified; }
            set
            {
                _modified = value;
                OnPropertyChanged("FontStyle");
            }
        }

        public bool IsClass { get; set; }

        public bool IsReadOnly { get; set; }

        public FontStyle FontStyle
        {
            get { return Modified ? FontStyles.Italic : FontStyles.Normal; }
        }

        public FontWeight FontWeight
        {
            get { return IsClass ? FontWeights.Bold : IsReadOnly ? FontWeights.Light : FontWeights.Normal; }
        }

        public string Name
        {
            get { return Id; }
        }

        public string PropertyType { get; set; }

        public string Description
        {
            get
            {
                return IsClass ? @"This is a class and data cannot be entered." :
                    EnumValueCollection.Count > 0 ? @"Please select a value from the list." :
                    @"Please enter the value in the textbox below.";
            }
        }

        public List<string> EnumValueCollection { get; set; }
        public TreeNode Parent { get; private set; }

        public TreeNode(string id)
        {
            Id = id;
            EnumValueCollection = new List<string>();
        }

        public TreeNode GetChild(string id)
        {
            return _children[id];
        }

        public void Add(TreeNode item)
        {
            item.Parent?._children.Remove(item.Id);

            item.Parent = this;
            _children.Add(item.Id, item);
        }

        public IEnumerator<TreeNode> GetEnumerator()
        {
            return _children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _children.Count; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}