using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using HP.ScalableTest.Web;

namespace HP.ScalableTest.Tools
{
    public partial class SiteMapViewer : UserControl
    {
        XmlDocument _doc;
        string _siteMapPath;

        public SiteMapViewer(string versionPath)
        {
            InitializeComponent();

            this.AutoScroll = true;
            this.DoubleBuffered = true;
            
            _doc = new XmlDocument();

            LoadSiteMap(versionPath);
        }

        private void LoadSiteMap(string versionPath)
        {
            string[] siteMaps = Directory.GetFiles(versionPath, "*.xml");

            //TODO: if it is more than one physical fine it needs to be merged into a single xml

            _doc.Load(siteMaps[0]);

            _siteMapPath = siteMaps[0];

            // walk thru all the pages and load on the UI
            foreach(XmlNode pageNode in _doc.DocumentElement.SelectNodes("//Pages/Page"))
            {
                LoadPageDetails(pageNode);
            }
        }

        private void LoadPageDetails(XmlNode pageNode)
        {
            PageViewer page = new PageViewer(pageNode);
            page.Height = 300;
            page.Dock = DockStyle.Top;
            page.Collapse();
            page.PageDataModified += new EventHandler(page_PageDataModified);
            this.Controls.Add(page);
        }

        void page_PageDataModified(object sender, EventArgs e)
        {
            if (this.Parent is TabPage)
            {
                TabPage tabPage = (this.Parent as TabPage);

                if (!tabPage.Text.EndsWith(" *"))
                {
                    tabPage.Text = tabPage.Text + " *";
                }
            }
        }

        public void Save()
        {
            foreach (Control page in this.Controls)
            {
                if (page is PageViewer)
                {
                    (page as PageViewer).Save();
                }
            }

            // save the site map into the disc
            _doc.Save(_siteMapPath);

            if (this.Parent is TabPage)
            {
                TabPage tabPage = (this.Parent as TabPage);

                if (tabPage.Text.EndsWith(" *"))
                {
                    tabPage.Text = tabPage.Text.Substring(0, tabPage.Text.Length - 2);
                }
            }
        }

        private void addPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPage();
        }

        public void AddPage()
        {
            // create new page node to xml and add the page viewer control
            XmlNode pageNode = _doc.CreateElement("Page");
            pageNode.Attributes.Append(_doc.CreateAttribute("key"));
            pageNode.Attributes.Append(_doc.CreateAttribute("relative_path"));
            pageNode.AppendChild(_doc.CreateElement("Elements"));

            _doc.DocumentElement.SelectSingleNode("Pages").AppendChild(pageNode);
            LoadPageDetails(pageNode);

            page_PageDataModified(null, null);
        }

        public void Validate(SeleniumWebDriver selenium, string deviceAddress)
        {
            foreach (Control page in this.Controls)
            {
                if (page is PageViewer)
                {
                    (page as PageViewer).Validate(selenium, deviceAddress);
                }
            }
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandAll();
        }

        public void ExpandAll()
        {
            foreach (Control page in this.Controls)
            {
                if (page is PageViewer)
                {
                    (page as PageViewer).Expand();
                }
            }
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollapseAll();
        }

        public void CollapseAll()
        {
            foreach (Control page in this.Controls)
            {
                if (page is PageViewer)
                {
                    (page as PageViewer).Collapse();
                }
            }
        }
    }
}
