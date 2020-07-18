using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.Framework
{
    /// <summary>
    /// Provides the UI for displaying and navigating driver repository locations.
    /// </summary>
    public partial class PrintDriverDownloadControl : UserControl
    {
        private const string _directoryNameX64 = "winxp_vista_x64";
        private const string _shortDirectoryNameX64 = "64bit";
        private TreeNode _rootNode = null;
        private RepositoryUri _repositoryPath = null;

        public event EventHandler<StatusChangedEventArgs> StatusChanged;
        public event EventHandler<SelectedNodeEventArgs> SelectionChanged;

        [BrowsableAttribute(true), EditorBrowsableAttribute(EditorBrowsableState.Always)]
        public bool DisplayRoot { get; set; }

        /// <summary>
        /// Default Constructor.  Uses repository path from SystemSettings.
        /// </summary>
        public PrintDriverDownloadControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor for connecting to an explicit repository path.
        /// </summary>
        /// <param name="repositoryPath">The repository path</param>
        public PrintDriverDownloadControl(string repositoryPath)
        {
            InitializeComponent();
            RepositoryPath = repositoryPath;
        }

        /// <summary>
        /// Handles the Load event of the PrintDriverAddControl control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_repositoryPath == null && !DesignMode) //This allows consumers of this control to display correctly in the VS designer
            {
                RepositoryPath = GlobalSettings.Items[Setting.UniversalPrintDriverBaseLocation];
            }
            Refresh();
        }

        /// <summary>
        /// Gets or sets the repository path.
        /// </summary>
        public string RepositoryPath
        {
            get
            {
                return DesignMode ? string.Empty : LocalPath; //_repositoryPath.FullPath
            }
            set
            {
                if (value.Length > 0)
                {
                    _repositoryPath = new RepositoryUri(value);
                }
            }
        }

        /// <summary>
        /// Loads the UI tree display of the repository path.
        /// </summary>
        public void LoadTreeControl()
        {
            UpdateStatus("Loading...");

            this.Cursor = Cursors.WaitCursor;

            if (printDrivers_TreeView.Nodes.Count > 0)
            {
                printDrivers_TreeView.Nodes.Clear();
            }
            printDrivers_TreeView.BeginUpdate();
            LoadPrintDriverShare();
            printDrivers_TreeView.Sort();
            printDrivers_TreeView.EndUpdate();

            UpdateStatus("Loading complete.");
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Gets the paths of all driver packages contained in the current selection (recursive).
        /// </summary>
        public IEnumerable<string> PackagePaths
        {
            get { return GetPackagePaths(printDrivers_TreeView.SelectedNode); }
        }

        /// <summary>
        /// Gets the path of the seleted node.
        /// </summary>
        public string SelectedPath
        {
            get { return printDrivers_TreeView.SelectedNode != null ? printDrivers_TreeView.SelectedNode.FullPath : string.Empty; }
        }

        public void ExpandRoot()
        {
            _rootNode.Expand();
        }

        public void CollapseRoot()
        {
            _rootNode.Collapse();
        }

        /// <summary>
        /// Selects the first node containing the specified text.
        /// </summary>
        /// <param name="nodeText">The text to search for.</param>
        public void SelectNode(string nodeText)
        {
            TreeNode result = RootNodes.Find(nodeText, searchAllChildren: true).FirstOrDefault();

            if (result != null)
            {
                printDrivers_TreeView.SelectedNode = result;
                printDrivers_TreeView.Select();
            }
        }

        private string LocalPath
        {
            get { return DisplayRoot ? _repositoryPath.GetPartialPath(_repositoryPath.DirectoryCount - 1) : _repositoryPath.FullPath; }
        }

        private TreeNodeCollection RootNodes
        {
            get { return DisplayRoot ? _rootNode.Nodes : printDrivers_TreeView.Nodes; }
        }


        private void UpdateStatus(string status)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this, new StatusChangedEventArgs(status));
            }
        }

        private void LoadPrintDriverShare()
        {
            try
            {
                if (DisplayRoot)
                {
                    SetRootNode();
                }

                foreach (string directory in Directory.GetDirectories(_repositoryPath.FullPath))
                {
                    AddNodeByPath(directory);
                }

                // Add all the child nodes to the initial directories
                TreeNodeCollection rootNodes = DisplayRoot ? _rootNode.Nodes : printDrivers_TreeView.Nodes;
                foreach (TreeNode node in rootNodes)
                {
                    AddChildNodes(node);
                }

                if (DisplayRoot)
                {
                    _rootNode.Expand();
                }

            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                if (PromptForCredentials() == DialogResult.OK)
                {
                    LoadPrintDriverShare();
                }
            }
        }

        private void SetRootNode()
        {
            //string stringPath = _repositoryPath.FullPath;
            //int lastIndex = stringPath.LastIndexOf('\\');
            //string pathPart = stringPath.Substring(lastIndex + 1, stringPath.Length - (lastIndex + 1));
            // string pathPart = _repositoryPath.GetPartialPath(_repositoryPath.DirectoryCount - 1);
            string pathPart = _repositoryPath.GetDirectory(_repositoryPath.DirectoryCount - 1);

            _rootNode = printDrivers_TreeView.Nodes.Add(pathPart);
            _rootNode.Name = pathPart;
        }

        private void AddChildNodes(TreeNode node)
        {
            string path = Path.Combine(LocalPath, node.FullPath);

            //if (_repositoryPath.Host.Contains("STFGlobal"))
            //{
            //    System.Diagnostics.Debug.WriteLine("Stop");
            //}

            try
            {
                Collection<string> subDirectories = new Collection<string>(Directory.GetDirectories(path));
                // Check to see if the X64 folder is found
                if (subDirectories.Any(n => n.EndsWith(Path.DirectorySeparatorChar + _directoryNameX64, StringComparison.OrdinalIgnoreCase) ||
                                            n.EndsWith(Path.DirectorySeparatorChar + _shortDirectoryNameX64, StringComparison.OrdinalIgnoreCase)))
                {
                    // If it is, the parent node represents a driver package
                    node.ImageIndex = node.SelectedImageIndex = 1;
                }
                else
                {
                    foreach (string subDirectory in subDirectories)
                    {
                        AddNodeByPath(subDirectory);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Error Loading Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void printDrivers_TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode expandingNode = e.Node;
            foreach (TreeNode node in expandingNode.Nodes)
            {
                AddChildNodes(node);
            }
        }

        private DialogResult PromptForCredentials()
        {
            DialogResult result = DialogResult.Cancel;

            // Prompt the user for credentials.
            using (PrintDriverCredentialForm credentialForm = new PrintDriverCredentialForm())
            {
                credentialForm.ShareLocation = RepositoryPath;
                result = credentialForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    RepositoryPath = credentialForm.ShareLocation; //The user may have changed the driver share location.
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the node by path.
        /// </summary>
        /// <param name="path">The path.</param>
        private void AddNodeByPath(string path)
        {
            //TreeNodeCollection currentSet = DisplayRoot ? _rootNode.Nodes : printDrivers_TreeView.Nodes;
            TreeNodeCollection currentSet = printDrivers_TreeView.Nodes;
            string shortPath = path.Replace(LocalPath, string.Empty);

            //if (_repositoryPath.Host.Contains("STFGlobal"))
            //{
            //    System.Diagnostics.Debug.WriteLine("Stop");
            //}

            // Walk through each path part, locating (or creating) the node that corresponds to that part
            foreach (string pathPart in shortPath.Split('\\'))
            {
                if (!string.IsNullOrEmpty(pathPart))
                {
                    // Look to see if the current set contains the node we're looking for
                    TreeNode[] results = currentSet.Find(pathPart, searchAllChildren: false);

                    if (results.Length == 0)
                    {
                        // The node could not be found - create it
                        TreeNode node = currentSet.Add(pathPart);
                        node.Name = pathPart;
                        // The current set is now the children of the node we created
                        currentSet = node.Nodes;
                    }
                    else
                    {
                        // The current set is now the children of the node we found
                        currentSet = results[0].Nodes;
                    }
                }
            }
        }

        private List<string> GetPackagePaths(TreeNode node)
        {
            List<string> packageSetNames = new List<string>();

            if (IsDriverPackage(node))
            {
                packageSetNames.Add(node.FullPath);
            }
            else
            {
                foreach (TreeNode child in node.Nodes)
                {
                    // Make sure the next level is loaded
                    AddChildNodes(child);

                    // Append the list of print driver packages
                    GetPackagePaths(child).ForEach(x => packageSetNames.Add(x));
                }
            }

            return packageSetNames;
        }

        private static bool IsDriverPackage(TreeNode node)
        {
            // As each node was loaded into the tree view,
            // the icon was changed for all print driver package
            // nodes.  This image index is an easy way to check to see
            // if the given icon is a print driver package, as opposed to a folder.
            return node.ImageIndex == 1;
        }

        ///// <summary>
        ///// Copies all drivers from the specified paths to the destination path.
        ///// </summary>
        ///// <param name="destinationPath"></param>
        ///// <param name="drivers"></param>
        ///// <returns></returns>
        //public Collection<string> CopyDrivers(string destinationPath, Collection<PrintDeviceDriverCollection> drivers)
        //{
        //    Collection<string> driverPaths = null;

        //    try
        //    {
        //        driverPaths = PrintDriversManager.CopyDrivers(destinationPath, drivers);
        //    }
        //    catch (IOException)
        //    {
        //        // If we were trying to overwrite and it failed, or the user opted not to overwrite,
        //        // return a failure condition.
        //        UpdateStatus("Failed to download drivers.");
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        // The user canceled the copy operation.
        //        UpdateStatus("User cancelled the copy.");
        //    }
        //    finally
        //    {
        //        UpdateStatus("Done.");
        //    }

        //    return driverPaths;
        //}

        private void printDrivers_TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (SelectionChanged != null)
            {
                string nodePath = string.Empty;
                if (e.Node.FullPath != _rootNode.Text)
                {
                    nodePath = DisplayRoot ? e.Node.FullPath.Substring(_rootNode.Text.Length + 1) : e.Node.FullPath;
                }
                SelectionChanged(this, new SelectedNodeEventArgs(e.Node.Text, string.Empty, nodePath));
            }
        }
    }

    /// <summary>
    /// Class used to manage the repository path within the PrintDriverDownloadControl.
    /// </summary>
    internal class RepositoryUri
    {
        private List<string> _parsedPath = new List<string>();

        public RepositoryUri(string repositoryPath)
        {
            foreach (string pathPart in repositoryPath.Split('\\'))
            {
                if (pathPart.Trim() != string.Empty)
                {
                    _parsedPath.Add(pathPart);
                }
            }
        }

        /// <summary>
        /// The Host Name.
        /// </summary>
        public string Host
        {
            get { return GetDirectory(0); }
        }

        /// <summary>
        /// Name of the last directory.
        /// </summary>
        public string LastDirectory
        {
            get { return GetDirectory(_parsedPath.Count - 1); }
        }

        /// <summary>
        /// The number of directories in this instance, including host name.
        /// </summary>
        public int DirectoryCount
        {
            get { return _parsedPath.Count; }
        }

        /// <summary>
        /// The full path.
        /// </summary>
        public string FullPath
        {
            get
            {
                StringBuilder result = new StringBuilder("\\\\");
                _parsedPath.ForEach(x => result.Append(x).Append("\\"));
                return result.ToString(0, result.Length - 1);
            }
        }

        /// <summary>
        /// Get a directory name by index.
        /// </summary>
        /// <param name="index">The index of the desired directory name.</param>
        /// <returns></returns>
        public string GetDirectory(int index)
        {
            if (index < _parsedPath.Count)
            {
                return _parsedPath[index];
            }
            return string.Empty;
        }

        /// <summary>
        /// Get the partial path up to, but not including, the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetPartialPath(int index)
        {
            StringBuilder result = new StringBuilder("\\\\");
            for (int i = 0; i < index; i++)
            {
                result.Append(_parsedPath[i]).Append("\\");
            }
            return result.ToString(0, result.Length - 1);
        }
    }
}
