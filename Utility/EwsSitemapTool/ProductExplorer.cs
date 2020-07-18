using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace HP.ScalableTest.Tools
{
    enum NodeType
    {
        Root,
        Family,
        Product,
        Version
    }    

    internal class ProductExplorer
    {
        #region Local Variables

        TreeView _treeView;
        TreeNode _editingNode;
        TabbedPanel _tabbedPanel;
        bool _isCopied = false;

        #endregion

        #region Constructor

        public ProductExplorer(TreeView treeView, TabbedPanel tabbedPanel)
        {
            _treeView = treeView;
            _tabbedPanel = tabbedPanel;
            _treeView.LabelEdit = true;
            _treeView.MouseUp += new MouseEventHandler(treeView_MouseUp);
            _treeView.BeforeLabelEdit += new NodeLabelEditEventHandler(treeView_BeforeLabelEdit);
            _treeView.AfterLabelEdit += new NodeLabelEditEventHandler(treeView_AfterLabelEdit);
            _treeView.KeyDown += new KeyEventHandler(treeView_KeyDown);
            _treeView.DoubleClick += new EventHandler(treeView_DoubleClick);
        }

        #endregion

        #region Public Methods

        public void AddVersion()
        {
            try
            {
                TreeNode node = _treeView.SelectedNode;

                if (NodeType.Product == (NodeType)node.Tag)
                {
                    node.Expand();

                    string productPath = Path.Combine((string)node.Parent.Parent.Tag, node.Parent.Text, node.Text);

                    string newVersion = GetNextAvailableVersionName(productPath);

                    // create new directory with the version name
                    Directory.CreateDirectory(newVersion);

                    // create empty site map
                    XmlDocument doc = new XmlDocument();

                    XmlNode root = doc.CreateElement("Sitemaps");
                    doc.AppendChild(root);

                    XmlNode pages = doc.CreateElement("Pages");
                    root.AppendChild(pages);

                    doc.Save(Path.Combine(newVersion, "SiteMaps.xml"));

                    TreeNode versionNode = AddItem(node, newVersion.Substring(newVersion.LastIndexOf('\\') + 1), NodeType.Version);
                    versionNode.EnsureVisible();
                    _treeView.SelectedNode = versionNode;
                    _editingNode = versionNode;
                    versionNode.BeginEdit();
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        public void Clear()
        {
            _treeView.Nodes.Clear();
            _editingNode = null;
            _isCopied = false;
        }

        public void LoadProducts(string siteMapLocation)
        {
            // Create HP Products root node
            TreeNode root = AddItem(null, "HP Printers", NodeType.Root);
            root.Tag = siteMapLocation;

            // walk thru all the product families inside the site map locator
            if (Directory.Exists(siteMapLocation))
            {
                AddFamily(root, siteMapLocation);
            }
            else
            {
                MessageBox.Show("Sitemap Directory {0} couldn't found or you do not have permissions.".FormatWith(siteMapLocation), "EWS Sitemap Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _treeView.ExpandAll();
        }

        public TreeNode SelectedNode
        {
            get
            {
                return _treeView.SelectedNode;
            }
        }

        #endregion

        #region Events

        void treeView_DoubleClick(object sender, EventArgs e)
        {
            _tabbedPanel.AddTab(_treeView.SelectedNode);
        }

        void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                _treeView.SelectedNode.BeginEdit();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                _tabbedPanel.AddTab(_treeView.SelectedNode);
            }
        }

        void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (null != _editingNode)
            {
                string oldName = _editingNode.Text;

                string newName = e.Label;

                if(null != newName && oldName != newName)
                {
                    string newPath = string.Empty;
                    string oldPath = string.Empty;

                    if ((NodeType)_editingNode.Tag == NodeType.Product)
                    {
                        newPath = Path.Combine((string)_editingNode.Parent.Parent.Tag, _editingNode.Parent.Text, newName);
                        oldPath = Path.Combine((string)_editingNode.Parent.Parent.Tag, _editingNode.Parent.Text, oldName);
                    }
                    else if ((NodeType)_editingNode.Tag == NodeType.Version)
                    {
                        newPath = Path.Combine((string)_editingNode.Parent.Parent.Parent.Tag, _editingNode.Parent.Parent.Text, _editingNode.Parent.Text, newName);
                        oldPath = Path.Combine((string)_editingNode.Parent.Parent.Parent.Tag, _editingNode.Parent.Parent.Text, _editingNode.Parent.Text, oldName);
                    }

                    if (!string.IsNullOrEmpty(newPath))
                    {
                        // check if already another directory exists
                        if (Directory.Exists(newPath))
                        {
                            MessageBox.Show("Name already exists, enter another valid name.", "EWS Sitemap Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.CancelEdit = true;
                        }
                        else
                        {
                            if (IsValid(newPath.Substring(newPath.LastIndexOf('\\') + 1)))
                            {
                                // Move the old directory to new directory
                                Directory.Move(oldPath, newPath);
                             
                            }
                            else
                            {
                                MessageBox.Show("Invalid name, enter only alphanumerical and _ - . special characters.", "EWS Sitemap Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                e.CancelEdit = true;
                            }
                        }
                    }
                }
            }
        }

        void treeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            _editingNode = null;
            
            try
            {
                NodeType nodeType = (NodeType)e.Node.Tag;
                
                switch(nodeType)
                {
                    // Family node can't be editable
                    case NodeType.Family:
                        e.CancelEdit = true;
                        return;                        

                    case NodeType.Product:
                        if (_tabbedPanel.IsProductTabExists(e.Node))
                        {
                            MessageBox.Show("All the tabs related to this product must be closed before editing this node.", "EWS Sitemap Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.CancelEdit = true;
                            return;
                        }                        
                        break;

                    case NodeType.Version:
                        if (_tabbedPanel.IsVersionTabExists(e.Node))
                        {
                            MessageBox.Show("The corresponding version tab must be closed before editing this node.", "EWS Sitemap Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.CancelEdit = true;
                            return;
                        }
                        break;
                }

                _editingNode = e.Node;
            }
            catch (Exception)
            {
                e.CancelEdit = true;
            }
        }

        void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                _treeView.SelectedNode = _treeView.GetNodeAt(e.X, e.Y);

                if (_treeView.SelectedNode != null)
                {
                    GetContextMenu().Show(_treeView, e.Location);
                }
            }
        }        

        void pasteSitemap_Click(object sender, EventArgs e)
        {
            TreeNode node = _treeView.SelectedNode;
            string[] siteMaps = Directory.GetFiles(Path.Combine((string)node.Parent.Parent.Parent.Tag, node.Parent.Parent.Text, node.Parent.Text, node.Text), "*.xml");
            File.Copy("CopiedSitemaps.xml", siteMaps[0], true);
        }

        void copySitemap_Click(object sender, EventArgs e)
        {
            TreeNode node = _treeView.SelectedNode;
            string[] siteMaps = Directory.GetFiles(Path.Combine((string)node.Parent.Parent.Parent.Tag, node.Parent.Parent.Text, node.Parent.Text, node.Text), "*.xml");
            File.Copy(siteMaps[0], "CopiedSitemaps.xml", true);
            _isCopied = true;
        }

        void deleteVersion_Click(object sender, EventArgs e)
        {
            TreeNode node = _treeView.SelectedNode;

            if (_tabbedPanel.IsVersionTabExists(node))
            {
                MessageBox.Show("Close the tab before deleting the version.", "EWS Sitemap Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (DialogResult.Yes == MessageBox.Show("This will delete Sitemaps permanently from the disk, Do you still want to delete the version ?","EWS Sitemap Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Directory.Delete(Path.Combine((string)node.Parent.Parent.Parent.Tag, node.Parent.Parent.Text, node.Parent.Text, node.Text), true);
                node.Remove();
            }
        }

        void deleteProduct_Click(object sender, EventArgs e)
        {
            TreeNode node = _treeView.SelectedNode;

            if (_tabbedPanel.IsProductTabExists(node))
            {
                MessageBox.Show("Close all the tabs related to this product before deleting.", "EWS Sitemap Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (DialogResult.Yes == MessageBox.Show("This will delete product permanently from the disk, Do you still want to delete the product ?", "EWS Sitemap Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Directory.Delete(Path.Combine((string)node.Parent.Parent.Tag, node.Parent.Text, node.Text), true);
                node.Remove();
            }
        }

        void expandAll_Click(object sender, EventArgs e)
        {
            _treeView.ExpandAll();
        }

        void collapseAll_Click(object sender, EventArgs e)
        {
            _treeView.CollapseAll();
        }

        void addProduct_Click(object sender, EventArgs e)
        {
            AddProduct();
        }        

        void addVersion_Click(object sender, EventArgs e)
        {
            AddVersion();
        }
        
        #endregion

        #region Private Methods

        public void AddProduct()
        {
            try
            {
                TreeNode node = _treeView.SelectedNode;

                if (NodeType.Family == (NodeType)node.Tag)
                {
                    node.Expand();

                    string familyPath = Path.Combine((string)node.Parent.Tag, node.Text);

                    string newProduct = GetNextAvailableProductName(familyPath);

                    // create new directory with the product name
                    Directory.CreateDirectory(newProduct);

                    TreeNode productNode = AddItem(node, newProduct.Substring(newProduct.LastIndexOf('\\') + 1), NodeType.Product);
                    productNode.EnsureVisible();
                    _treeView.SelectedNode = productNode;
                    _editingNode = productNode;
                    productNode.BeginEdit();
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        private ContextMenuStrip GetContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            try
            {
                switch ((NodeType)_treeView.SelectedNode.Tag)
                {
                    case NodeType.Family:
                        ToolStripItem addProduct = contextMenu.Items.Add("Add Product");
                        addProduct.Click += new EventHandler(addProduct_Click);
                        addProduct.Image = global::HP.ScalableTest.Tools.Properties.Resources.printer_add;

                        break;

                    case NodeType.Product:
                        ToolStripItem addVersion = contextMenu.Items.Add("Add Sitemap");
                        addVersion.Click += new EventHandler(addVersion_Click);
                        addVersion.Image = global::HP.ScalableTest.Tools.Properties.Resources.chart_organisation_add;
                        contextMenu.Items.Add("-");

                        ToolStripItem deleteProduct = contextMenu.Items.Add("Delete Product");
                        deleteProduct.Click += deleteProduct_Click;
                        deleteProduct.Image = global::HP.ScalableTest.Tools.Properties.Resources.printer_delete;
                        break;

                    case NodeType.Version:

                        ToolStripItem copySitemap = contextMenu.Items.Add("Copy");
                        copySitemap.Click += copySitemap_Click;

                        ToolStripItem pasteSitemap = contextMenu.Items.Add("Paste");
                        pasteSitemap.Click += pasteSitemap_Click;

                        if (_isCopied)
                        {
                            pasteSitemap.Enabled = true;
                        }
                        else
                        {
                            pasteSitemap.Enabled = false;
                        }

                        ToolStripItem deleteVersion = contextMenu.Items.Add("Delete Sitemap");
                        deleteVersion.Click += deleteVersion_Click;
                        deleteVersion.Image = global::HP.ScalableTest.Tools.Properties.Resources.chart_organisation_delete;
                        break;
                }
            }
            catch (Exception)
            {
                // do nothing
            }
            finally
            {
                if (contextMenu.Items.Count > 0)
                {
                    contextMenu.Items.Add("-");
                }

                ToolStripItem expandAll = contextMenu.Items.Add("Expand All");
                expandAll.Click += new EventHandler(expandAll_Click);

                ToolStripItem collapseAll = contextMenu.Items.Add("Collapse All");
                collapseAll.Click += new EventHandler(collapseAll_Click);
            }

            return contextMenu;
        }

        private bool IsValid(string input)
        {
            return Regex.IsMatch(input, @"^[A-Za-z0-9_\-\.\s]+$");
        }

        private int GetIconIndex(NodeType type)
        {
            int index = 0;

            switch (type)
            {
                case NodeType.Root:
                    index = 0;
                    break;
                case NodeType.Family:
                    index = 2;
                    break;
                case NodeType.Product:
                    index = 3;
                    break;
                case NodeType.Version:
                    index = 4;
                    break;
            }

            return index;
        }

        private void AddFamily(TreeNode parent, string siteMapLocation)
        {
            foreach (string dir in Directory.GetDirectories(siteMapLocation))
            {
                TreeNode familyNode = AddItem(parent, dir.Substring(dir.LastIndexOf('\\') + 1), NodeType.Family);

                AddProduct(familyNode, dir);
            }
        }

        private void AddProduct(TreeNode parent, string familyDir)
        {
            foreach (string dir in Directory.GetDirectories(familyDir))
            {
                TreeNode productNode = AddItem(parent, dir.Substring(dir.LastIndexOf('\\') + 1), NodeType.Product);

                AddVersion(productNode, dir);
            }
        }

        private void AddVersion(TreeNode parent, string productDir)
        {
            foreach (string dir in Directory.GetDirectories(productDir))
            {
                TreeNode productNode = AddItem(parent, dir.Substring(dir.LastIndexOf('\\') + 1), NodeType.Version);
            }
        }

        private TreeNode AddItem(TreeNode parent, string name, NodeType type)
        {
            TreeNode node = new TreeNode(name, GetIconIndex(type), GetIconIndex(type));


            if (null == parent)
            {
                _treeView.Nodes.Add(node);
            }
            else
            {
                parent.Nodes.Add(node);
                node.Tag = type;
            }


            return node;
        }

        private string GetNextAvailableProductName(string familyPath)
        {
            int count = 1;

            string newProductName = Path.Combine(familyPath, "New Product " + count);

            while (Directory.Exists(newProductName))
            {
                newProductName = Path.Combine(familyPath, "New Product " + count++);
            }

            return newProductName;
        }

        private string GetNextAvailableVersionName(string productPath)
        {
            int count = 1;

            string newVersiontName = Path.Combine(productPath, "New Version " + count++);

            while (Directory.Exists(newVersiontName))
            {
                newVersiontName = Path.Combine(productPath, "New Version " + count++);
            }

            return newVersiontName;
        }

        #endregion
    }
}
