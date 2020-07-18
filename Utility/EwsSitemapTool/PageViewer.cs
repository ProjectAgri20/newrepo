using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using HP.ScalableTest.Web;
using OpenQA.Selenium;

namespace HP.ScalableTest.Tools
{
    public partial class PageViewer : UserControl
    {
        #region Local Variables

        XmlNode _pageNode;
        bool _expanded;
        int _height;

        #endregion        

        #region Constructor

        public PageViewer(XmlNode pageNode)
        {
            InitializeComponent();

            _pageNode = pageNode;
            _height = this.Height;

            LoadPageDetails();

            keys_ComboBox.TextChanged += new EventHandler(key_ComboBox_TextChanged);
            path_TextBox.TextChanged += new EventHandler(path_TextBox_TextChanged);
            siteMapDataSet.SiteMaps.ColumnChanged += new DataColumnChangeEventHandler(SiteMaps_ColumnChanged);
            siteMapDataSet.SiteMaps.RowChanged += new DataRowChangeEventHandler(SiteMaps_RowChanged);
            siteMapDataSet.SiteMaps.RowDeleted += new DataRowChangeEventHandler(SiteMaps_RowDeleted);
            siteMapDataSet.SiteMaps.TableNewRow += new DataTableNewRowEventHandler(SiteMaps_TableNewRow);
        }

        #endregion

        public event EventHandler PageDataModified;

        #region Private Methods

        private void LoadPageDetails()
        {
            try
            {
                foreach (string key in MasterKeys.Instance().GetKeys())
                {
                    keys_ComboBox.Items.Add(key);
                }

                keys_ComboBox.SelectedItem = _pageNode.Attributes["key"].Value;
                path_TextBox.Text = _pageNode.Attributes["relative_path"].Value;

                foreach (XmlNode node in _pageNode.SelectNodes("Elements/Element"))
                {
                    SiteMapDataSet.SiteMapsRow row = siteMapDataSet.SiteMaps.NewSiteMapsRow();
                    row.Key = node.Attributes["key"].Value;
                    row.ID = node.Attributes["id"].Value;
                    row.Name = node.Attributes["name"].Value;

                    if (null != node.Attributes["xpath"])
                    {
                        row.XPath = node.Attributes["xpath"].Value;
                    }
                    else
                    {
                        row.XPath = string.Empty;
                    }

                    if (null != node.Attributes["class"])
                    {
                        row.Class = node.Attributes["class"].Value;
                    }
                    else
                    {
                        row.Class = string.Empty;
                    }

                    row.Type = node.Attributes["type"].Value;

                    siteMapDataSet.SiteMaps.AddSiteMapsRow(row);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error in loading page {0} details".FormatWith(_pageNode.Attributes["relative_path"].Value));
            }
        }

        private void page_Panel_Click(object sender, EventArgs e)
        {
            if (_expanded)
            {
                this.Height = page_Panel.Height;
            }
            else
            {
                this.Height = _height;
            }

            _expanded = !_expanded;
        }        

        private XmlNode GetPageNode()
        {
            XmlNode pageNode = _pageNode.OwnerDocument.CreateElement("Page");
            XmlAttribute attr = _pageNode.OwnerDocument.CreateAttribute("key");
            attr.Value = keys_ComboBox.Text;
            pageNode.Attributes.Append(attr);

            attr = _pageNode.OwnerDocument.CreateAttribute("relative_path");
            attr.Value = path_TextBox.Text;
            pageNode.Attributes.Append(attr);

            XmlNode elementsNode = _pageNode.OwnerDocument.CreateElement("Elements");

            pageNode.AppendChild(elementsNode);

            foreach (SiteMapDataSet.SiteMapsRow row in siteMapDataSet.SiteMaps.Rows)
            {
                XmlNode elementNode = _pageNode.OwnerDocument.CreateElement("Element");

                attr = _pageNode.OwnerDocument.CreateAttribute("key");
                attr.Value = row.Key;
                elementNode.Attributes.Append(attr);

                attr = _pageNode.OwnerDocument.CreateAttribute("id");
                attr.Value = row.ID;
                elementNode.Attributes.Append(attr);

                attr = _pageNode.OwnerDocument.CreateAttribute("name");
                attr.Value = row.Name;
                elementNode.Attributes.Append(attr);

                attr = _pageNode.OwnerDocument.CreateAttribute("xpath");
                attr.Value = row.XPath;
                elementNode.Attributes.Append(attr);

                attr = _pageNode.OwnerDocument.CreateAttribute("class");
                attr.Value = row.Class;
                elementNode.Attributes.Append(attr);

                attr = _pageNode.OwnerDocument.CreateAttribute("type");
                attr.Value = row.Type;
                elementNode.Attributes.Append(attr);

                elementsNode.AppendChild(elementNode);
            }

            return pageNode;
        }        

        private bool IsElementExists(SeleniumWebDriver selenium, string locator, FindType type)
        {
            if (string.IsNullOrWhiteSpace(locator)) return false;

			try
			{
				IWebElement element = selenium.FindElement(locator, type, retry: false);

				if (null == element)
				{
					return false;
				}

				return true;
			}
			catch
			{
				return false;
			}
        }

        #endregion

        #region Public Methods

        public void Expand()
        {
            this.Height = _height;
            _expanded = true;
        }

        public void Collapse()
        {
            this.Height = page_Panel.Height;
            _expanded = false;
        }

        public void Save()
        {
            // Remove the existing page node and append the new page node
            XmlNode newPageNode = GetPageNode();
            XmlDocument doc = _pageNode.OwnerDocument;
            _pageNode.ParentNode.RemoveChild(_pageNode);
            _pageNode = newPageNode;
            doc.DocumentElement.SelectSingleNode("Pages").AppendChild(newPageNode);
        }

        public void Validate(SeleniumWebDriver selenium, string deviceAddress)
        {
            this.Expand();

            // validate Page Key
            if (MasterKeys.Instance().IsExists(keys_ComboBox.Text))
            {
                keys_ComboBox.BackColor = Color.White;
            }
            else
            {
                keys_ComboBox.BackColor = Color.Red;
            }

            // validate Page relative path
            try
            {
                string url = path_TextBox.Text;

                if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    url = "http://{0}/{1}".FormatWith(deviceAddress, url);
                }

                selenium.Open(new Uri(url));
                path_TextBox.BackColor = Color.White;
            }
            catch (Exception)
            {
                path_TextBox.BackColor = Color.Red;
            }

            int index = 0;

            // validate all the page elements
            foreach (SiteMapDataSet.SiteMapsRow row in siteMapDataSet.SiteMaps.Rows)
            {
                DataGridViewRow gridRow = pageDetails_DataGridView.Rows[index++];

                gridRow.Cells[1].Style.BackColor = Color.White;
                gridRow.Cells[2].Style.BackColor = Color.White;
                gridRow.Cells[3].Style.BackColor = Color.White;

                // validate they key
                if (MasterKeys.Instance().IsExists(row.Key))
                {
                    gridRow.Cells[0].Style.BackColor = Color.White;
                }
                else
                {
                    gridRow.Cells[0].Style.BackColor = Color.Red;
                }

                // validate id
                if (IsElementExists(selenium, row.ID, FindType.ById))
                {
                    gridRow.Cells[1].Style.BackColor = Color.White;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(row.ID))
                    {
                        gridRow.Cells[1].Style.BackColor = Color.Red;
                    }
                    else
                    {
                        // validate name
                        if (IsElementExists(selenium, row.Name, FindType.ByName))
                        {
                            gridRow.Cells[2].Style.BackColor = Color.White;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(row.Name))
                            {
                                gridRow.Cells[2].Style.BackColor = Color.Red;
                            }
                            else
                            {
                                // validate xpath
                                if (IsElementExists(selenium, row.XPath, FindType.ByXPath))
                                {
                                    gridRow.Cells[3].Style.BackColor = Color.White;
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(row.XPath))
                                    {
                                        gridRow.Cells[3].Style.BackColor = Color.Red;
                                    }
                                    else
                                    {
                                        // validate class
                                        if (IsElementExists(selenium, row.Class, FindType.ByClassName))
                                        {
                                            gridRow.Cells[4].Style.BackColor = Color.White;
                                        }
                                        else
                                        {
                                            gridRow.Cells[4].Style.BackColor = Color.Red;
                                        }
                                    }                                    
                                }
                            }
                        }
                    }                    
                }                
            }
        }

        #endregion

        #region Events

        void SiteMaps_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            PageDataModified(this, new EventArgs());
        }

        void SiteMaps_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            PageDataModified(this, new EventArgs());
        }

        void SiteMaps_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            PageDataModified(this, new EventArgs());
        }

        void SiteMaps_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            PageDataModified(this, new EventArgs());
        }

        void path_TextBox_TextChanged(object sender, EventArgs e)
        {
            PageDataModified(this, new EventArgs());
        }

        void key_ComboBox_TextChanged(object sender, EventArgs e)
        {
            PageDataModified(this, new EventArgs());
        }

        private void deletePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you want to delete the current page ?", "EWS Site Map Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                _pageNode.ParentNode.RemoveChild(_pageNode);
                this.Parent.Controls.Remove(this);
                PageDataModified(this, new EventArgs());
            }
        }        

        #endregion        
        
    }
}
