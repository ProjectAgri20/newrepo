using System;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace HP.ScalableTest.Development.UI
{
    /// <summary>
    /// Provides a way for users to select an STF plugin.
    /// </summary>
    public partial class SelectPluginForm : Form
    {
        /// <summary>
        /// Provides a list of plugins to select.
        /// </summary>
        public SelectPluginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The Assembly name of the loaded plugin.
        /// </summary>
        public string PluginAssemblyName
        {
            get { return comboBox_Plugin.Text; }
        }

        /// <summary>
        /// Load event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            comboBox_Plugin.DataSource = GetPluginsFromFolder();
            string lastAccessedPlugin = UserAppDataRegistry.GetValue("PluginAssemblyName") as string;
            if (!string.IsNullOrEmpty(lastAccessedPlugin))
            {
                comboBox_Plugin.SelectedIndex = comboBox_Plugin.FindString(lastAccessedPlugin);
            }
        }

        private List<string> GetPluginsFromFolder()
        {
            //Build plugin folder path
            string exeRootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pluginFolderPath = Path.Combine(exeRootDir, ConfigurationManager.AppSettings["PluginRelativeLocation"]);

            //Get list of plugins from the path
            List<string> pluginsList = new List<string>();
            DirectoryInfo pluginFolder = new DirectoryInfo(pluginFolderPath);

            foreach (FileInfo file in pluginFolder.GetFiles().ToList())
            {
                if (file.Name.StartsWith("Plugin.") && file.Name.EndsWith(".dll"))
                {
                    pluginsList.Add(file.Name);
                }
            }

            return pluginsList;
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            UserAppDataRegistry.SetValue("PluginAssemblyName", PluginAssemblyName);
        }
    }
}
