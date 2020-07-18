using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Data.AssetInventory;
using HP.ScalableTest.Data.AssetInventory.Model;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.SessionExecution.Wizard;

namespace HP.ScalableTest.STFU
{
    public class TreeViewItemViewModel : INotifyPropertyChanged
    {
        #region Data

        static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();

        readonly ObservableCollection<TreeViewItemViewModel> _children;
        readonly TreeViewItemViewModel _parent;

        bool _isExpanded;
        bool _isSelected;

        #endregion // Data

        #region Constructors

        protected TreeViewItemViewModel(TreeViewItemViewModel parent, bool lazyLoadChildren)
        {
            _parent = parent;

            _children = new ObservableCollection<TreeViewItemViewModel>();

            if (lazyLoadChildren)
                _children.Add(DummyChild);
        }

        // This is used to create the DummyChild instance.
        private TreeViewItemViewModel()
        {
        }

        #endregion // Constructors

        #region Presentation Members

        #region Children

        /// <summary>
        /// Returns the logical child items of this object.
        /// </summary>
        public ObservableCollection<TreeViewItemViewModel> Children
        {
            get { return _children; }
        }

        #endregion // Children

        #region HasLoadedChildren

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild
        {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }

        #endregion // HasLoadedChildren

        #region IsExpanded

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;

                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }

        #endregion // IsExpanded

        #region IsSelected

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected

        #region LoadChildren

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
        }

        #endregion // LoadChildren

        #region Parent

        public TreeViewItemViewModel Parent
        {
            get { return _parent; }
        }

        #endregion // Parent

        #endregion // Presentation Members

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }

    public class ScenarioFolderViewModel : TreeViewItemViewModel
    {
        readonly ConfigurationTreeFolder _folder;

        public ScenarioFolderViewModel(ConfigurationTreeFolder folder): base(null, true)
        {
            _folder = folder;           
        }

        public ScenarioFolderViewModel(ConfigurationTreeFolder folder, ScenarioFolderViewModel parentFolder): base(parentFolder, true)
        {
            _folder = folder;
        }

        public string FolderName { get {return _folder.Name; } }

        protected override void LoadChildren()
        {
            //foreach (State state in Database.GetStates(_region))
            //    base.Children.Add(new StateViewModel(state, this));


            using(EnterpriseTestContext context = new EnterpriseTestContext())
            {     
                foreach(ConfigurationTreeFolder folder in GetSubFolders(context, this._folder.ConfigurationTreeFolderId) )
                {
                    base.Children.Add(new ScenarioFolderViewModel(folder, this));
                }
                foreach (EnterpriseScenario scenario in ScenariosInFolder(context, _folder.ConfigurationTreeFolderId))
                {
                    //base.Children.Add(new scen)
                    base.Children.Add(new ScenarioViewModel(scenario, this));
                }
            }
        }

        public static IEnumerable<ConfigurationTreeFolder> GetSubFolders(EnterpriseTestEntities entities, Guid folderId)
        {

            var folders = entities.ConfigurationTreeFolders.Where(n => n.ParentId == folderId);

            return folders;

        }

        public static IEnumerable<EnterpriseScenario> ScenariosInFolder(EnterpriseTestEntities entities, Guid folderId)
        {
            var scenarioIds = entities.EnterpriseScenarios.Where(n => n.FolderId == folderId);
            return scenarioIds;
        }
    }

    public class ScenarioViewModel: TreeViewItemViewModel
    {
        readonly EnterpriseScenario _scenario;

        public ScenarioViewModel(EnterpriseScenario scenario, ScenarioFolderViewModel folder): base(folder, true)
        {
            _scenario = scenario;
        }

        public string ScenarioName { get { return _scenario.Name; } }
        public Guid ScenarioId { get { return _scenario.EnterpriseScenarioId; } }

    }

    public class ScenarioModel
    {
        readonly ReadOnlyCollection<ScenarioFolderViewModel> _folders;

        public ScenarioModel(ConfigurationTreeFolder[] folders)
        {
            _folders = new ReadOnlyCollection<ScenarioFolderViewModel>(
                (from folder in folders
                 select new ScenarioFolderViewModel(folder))
                .ToList());
        }

        public ReadOnlyCollection<ScenarioFolderViewModel> Folders
        {
            get { return _folders; }
        }
    }
}
