using HP.ScalableTest.Web;
using System;
using System.IO;
using System.Windows.Forms;

namespace HP.ScalableTest.Tools
{
    internal class TabbedPanel
    {
        #region Local Variables

        TabControl _tabControl;

        #endregion

        #region Constructor

        public TabbedPanel(TabControl tabControl)
        {
            _tabControl = tabControl;
        }

        #endregion        

        #region Public Methods

        public void AddTab(TreeNode node)
        {
            try
            {
                if (((NodeType)node.Tag) == NodeType.Version)
                {
                    if (!IsVersionTabExists(node))
                    {
                        TabPage tabPage = new TabPage(GetTabText(node));

                        SiteMapViewer viewer = new SiteMapViewer(Path.Combine(node.Parent.Parent.Parent.Tag.ToString(), node.Parent.Parent.Text, node.Parent.Text, node.Text));

                        viewer.Dock = DockStyle.Fill;

                        tabPage.Controls.Add(viewer);

                        _tabControl.TabPages.Add(tabPage);
                        _tabControl.SelectedTab = tabPage;
                    }
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        public void RemoveTab(TreeNode node)
        {
            //_tabControl.TabPages.Remove()
        }

        public void ClearTabs()
        {
            _tabControl.TabPages.Clear();
        }

        public bool IsVersionTabExists(TreeNode node)
        {
            string tabText = GetTabText(node);

            foreach (TabPage tab in _tabControl.TabPages)
            {
                if (tabText.Equals(tab.Text)) // TODO: Need to support *
                {
                    _tabControl.SelectedTab = tab;
                    return true;
                }
            }

            return false;
        }

        public bool IsProductTabExists(TreeNode node)
        {
            string tabText = node.Parent.Text + " \\ " + node.Text;

            foreach (TabPage tab in _tabControl.TabPages)
            {
                if (tab.Text.StartsWith(tabText))
                {
                    return true;
                }
            }

            return false;
        }

        public string GetTabText(TreeNode node)
        {
            return node.Parent.Parent.Text + " \\ " + node.Parent.Text + " \\ " + node.Text;
        }

        public void BringToFront(TreeNode node)
        {
            foreach (TabPage tab in _tabControl.TabPages)
            {
                if (tab.Text == GetTabText(node))
                {                    
                    _tabControl.SelectedTab = tab;
                }
            }
        }

        public void SaveSiteMapViewer()
        {
            (_tabControl.SelectedTab.Controls[0] as SiteMapViewer).Save();
        }

        public void SaveAllSiteMapViewer()
        {
            foreach(TabPage tab in _tabControl.TabPages)
            {
                (tab.Controls[0] as SiteMapViewer).Save();
            }
        }

        public void Close()
        {
            if (null != _tabControl.SelectedTab)
            {
                if (_tabControl.SelectedTab.Text.EndsWith("*"))
                {
                    DialogResult result = MessageBox.Show("Sitemaps are modified. Do you want to save the changes?", "EWS Sitemap Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                    if (DialogResult.Yes == result)
                    {
                        SaveSiteMapViewer();
                        _tabControl.TabPages.Remove(_tabControl.SelectedTab);
                    }
                    else
                    {
                        _tabControl.TabPages.Remove(_tabControl.SelectedTab);
                    }
                }
                else
                {
                    _tabControl.TabPages.Remove(_tabControl.SelectedTab);
                }
            }            
        }

        public void CloseAll()
        {
            foreach (TabPage tab in _tabControl.TabPages)
            {
                _tabControl.SelectedTab = tab;
                Close();
            }
        }

        public bool IsDirty()
        {
            foreach (TabPage tab in _tabControl.TabPages)
            {
                if (tab.Text.EndsWith("*"))
                {
                    return true;
                }
            }

            return false;
        }

        public void Validate(SeleniumWebDriver selenium, string deviceAddress)
        {
            TabPage page = _tabControl.SelectedTab;

            if (null != page)
            {
                if (page.Controls[0] is SiteMapViewer)
                {
                    (page.Controls[0] as SiteMapViewer).Validate(selenium, deviceAddress);
                }
            }
        }

        public void AddPage()
        {
            TabPage page = _tabControl.SelectedTab;

            if (null != page)
            {
                if (page.Controls[0] is SiteMapViewer)
                {
                    (page.Controls[0] as SiteMapViewer).AddPage();
                }
            }
        }

        public void ExpandAll()
        {
            TabPage page = _tabControl.SelectedTab;

            if (null != page)
            {
                if (page.Controls[0] is SiteMapViewer)
                {
                    (page.Controls[0] as SiteMapViewer).ExpandAll();
                }
            }
        }

        public void CollapseAll()
        {
            TabPage page = _tabControl.SelectedTab;

            if (null != page)
            {
                if (page.Controls[0] is SiteMapViewer)
                {
                    (page.Controls[0] as SiteMapViewer).CollapseAll();
                }
            }
        }

        public int Count
        {
            get
            {
                return _tabControl.TabCount;
            }

        }

        #endregion
    }
}
