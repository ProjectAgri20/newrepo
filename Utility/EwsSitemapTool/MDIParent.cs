using System;
using System.Net;
using System.Windows.Forms;

using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Web;
using System.Reflection;
using System.IO;
using System.Configuration;
using HP.ScalableTest.Tools.Properties;

namespace HP.ScalableTest.Tools
{
	public partial class MDIParent : Form
	{
		#region Local Variables
		
		private TabbedPanel _tabbedPanel;
		private ProductExplorer _productExplorer;
		SeleniumWebDriver _selenium;

		#endregion

		#region Constructor

		public MDIParent()
		{
			InitializeComponent();

			Initilize();
		}

		#endregion

		#region Private Methods

		private void Initilize()
		{
			this.FormClosing += new FormClosingEventHandler(MDIParent_FormClosing);
			_tabbedPanel = new TabbedPanel(siteMapsDetails_TabControl);
			_productExplorer = new ProductExplorer(productExplorer_TreeView, _tabbedPanel);            

			// clear tab items
			_tabbedPanel.ClearTabs();
			CopyExplorerDriverServer();
			_selenium = new SeleniumWebDriver(BrowserModel.Firefox, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

			this.Text += " " + Assembly.GetExecutingAssembly().GetName().Version + " beta";

			FolderBrowserDialog fbd = new FolderBrowserDialog();

			if (DialogResult.OK == fbd.ShowDialog())
			{                
				GlobalSettings.Items.Add("EwsSitemapLocation", fbd.SelectedPath);

				// set the version of the tool
				_productExplorer.LoadProducts(GlobalSettings.Items["EwsSitemapLocation"]);
			}
		}

		void MDIParent_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = !CloseApp();
		}

		private void CopyExplorerDriverServer()
		{
			string newPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "IEDriverServer.exe");

			if (ProcessorArchitectureInfo.Current == ProcessorArchitecture.NTAMD64)
			{
				File.WriteAllBytes(newPath, Resources.IEDriverServer_x64);
			}
			else
			{
				File.WriteAllBytes(newPath, Resources.IEDriverServer_x86);
			}
		}

		private bool CloseApp()
		{
			// check if any tabs are dirty before closing
			if (_tabbedPanel.IsDirty())
			{
				DialogResult result = MessageBox.Show("Some of the Sitemaps are modified, closing the application you may loose the changes. \n Do you wish to close the application ?", "EWS Site Map Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (DialogResult.Yes == result)
				{
					_selenium.Stop();
					return true;
				}
			}
			else
			{
				_selenium.Stop();
				return true;
			}

			return false;
		}

		private void CloseTab(object sender, EventArgs e)
		{
			// Close Tab
			_tabbedPanel.Close();
		}        

		#endregion

		#region Events

		private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_tabbedPanel.SaveAllSiteMapViewer();
		}

		private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CloseApp())
			{
				this.Close();                
			}
		}        

		private void CutToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStrip.Visible = toolBarToolStripMenuItem.Checked;
		}

		private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			statusStrip.Visible = statusBarToolStripMenuItem.Checked;
		}

		private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}

		private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileVertical);
		}

		private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.ArrangeIcons);
		}

		private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_tabbedPanel.CloseAll();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBox about = new AboutBox();
			about.ShowDialog();
		}

		private void saveToolStripButton_Click(object sender, EventArgs e)
		{
			_tabbedPanel.SaveSiteMapViewer();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_tabbedPanel.SaveSiteMapViewer();
		}

		private void newProductToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_productExplorer.AddProduct();
		}

		private void newVersionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_productExplorer.AddVersion();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			IPAddress address;

			if (!IPAddress.TryParse(deviceAddress_toolStripTextBox.Text, out address))
			{
				MessageBox.Show("Enter valid IP Address.", "EWS Sitemap Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			_selenium.Start(BrowserModel.Firefox, new Uri("http://{0}".FormatWith(deviceAddress_toolStripTextBox.Text)));
			_tabbedPanel.Validate(_selenium, deviceAddress_toolStripTextBox.Text);
			_selenium.Stop();
		}

		private void addPage_toolStripButton_Click(object sender, EventArgs e)
		{
			_tabbedPanel.AddPage();
		}

		private void expandAll_toolStripButton_Click(object sender, EventArgs e)
		{
			_tabbedPanel.ExpandAll();
		}

		private void collapseAll_ToolStripButton_Click(object sender, EventArgs e)
		{
			_tabbedPanel.CollapseAll();
		}

		private void deviceAddress_toolStripTextBox_Enter(object sender, EventArgs e)
		{
			if (deviceAddress_toolStripTextBox.Text == "Enter IP Address")
			{
				deviceAddress_toolStripTextBox.Text = "";
			}
		}

		private void deviceAddress_toolStripTextBox_Leave(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(deviceAddress_toolStripTextBox.Text))
			{
				deviceAddress_toolStripTextBox.Text = "Enter IP Address";
			}
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// before editing the settings first close the tabs
			if (_tabbedPanel.Count > 0)
			{
				MessageBox.Show("Close all the sitemaps before editing any settings","EWS Sitemap Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string oldLocation;

			try
			{
				oldLocation = GlobalSettings.Items["EWSSitemapLocation"];
			}
			catch (SettingNotFoundException)
			{
				oldLocation = string.Empty;
			}            

			Settings settings = new Settings();

			if (DialogResult.OK == settings.ShowDialog())
			{
				string newLocation = settings.SimapLocation;

				if (oldLocation != newLocation)
				{
					_productExplorer.Clear();
					_productExplorer.LoadProducts(newLocation);
				}
			}
		}

		#endregion		
	}
}
