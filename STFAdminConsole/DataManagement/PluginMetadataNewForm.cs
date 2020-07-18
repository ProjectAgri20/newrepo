using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Reflection;
using System.Drawing;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.LabConsole.Properties;
using HP.ScalableTest.Core.Plugin;

namespace HP.ScalableTest.LabConsole.DataManagement
{
    /// <summary>
    /// Class for adding new plugin 
    /// </summary>
    public partial class PluginMetadataNewForm : Form
    {
        /// <summary>
        /// Name of the Plugin
        /// </summary>
        public string PluginName { get; set; }
        /// <summary>
        /// Plugin assembly name
        /// </summary>
        public string PluginAssemblyName { get; set; }
        /// <summary>
        /// Name of the plugin displayed in Console
        /// </summary>
        public string PluginDisplayName { get; set; }
        /// <summary>
        /// The image associated with the plugin
        /// </summary>
        public Image PluginIcon { get; set; }



        /// <summary>
        /// constructs new form for adding new plugins
        /// </summary>
        public PluginMetadataNewForm()
        {
            InitializeComponent();

            string pluginFolderPath = GetPluginFolderPath();
            StringBuilder prompt = new StringBuilder(Resources.AddPluginPrompt);
            prompt.Append(Environment.NewLine);
            prompt.Append(pluginFolderPath);
            NewFormLabel.Text = prompt.ToString();

            PluginComboBox.DataSource = GetAvailablePlugins(pluginFolderPath);
        }


        /// <summary>
        /// Checks Plugin folder and grabs list of plugin candidates from it.
        /// </summary>
        private List<string> GetPluginsFromFolder(string folderPath)
        {
            List<string> pluginsList = new List<string>();
            DirectoryInfo pluginDir = new DirectoryInfo(folderPath);

            foreach (FileInfo file in pluginDir.GetFiles().ToList())
            {
                if (file.Name.EndsWith(".dll"))
                {
                    pluginsList.Add(file.Name);
                }
            }

            return pluginsList;
        }

        /// <summary>
        /// Cross checks plugin list from folder to plugins already in the database
        /// and removes them from candidate list
        /// </summary>
        private void RemovePluginsAlreadyInDatabase(ref List<string> plugins)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                foreach (MetadataType item in context.MetadataTypes)
                {
                    if (plugins.Contains(item.AssemblyName, StringComparer.OrdinalIgnoreCase))
                    {
                        plugins.RemoveAt(plugins.FindIndex(n => n.Equals(item.AssemblyName, StringComparison.OrdinalIgnoreCase)));
                    }
                }
            }
        }

        /// <summary>
        /// Checks plugins in candidate list to see which plugins implement
        /// IPluginConfigurationControl and keeps those to finalize candidate list
        /// </summary>
        private List<string> GetAvailablePlugins(string pluginFolderPath)
        {
            List<string> result = GetPluginsFromFolder(pluginFolderPath);
            RemovePluginsAlreadyInDatabase(ref result);
            return result;
        }

        /// <summary>
        /// Removes "Plugin" and ".dll" from assembly name to create Plugin Name
        /// </summary>
        /// <param name="pluginAssemblyName"></param>
        /// <returns></returns>
        public string GetMetadataTypeName(string pluginAssemblyName)
        {
            StringBuilder newText = new StringBuilder(pluginAssemblyName);

            newText.Replace("Plugin", string.Empty);
            newText.Replace(".", string.Empty);
            newText.Replace("dll", string.Empty);
            return newText.ToString();
        }

        /// <summary>
        /// Checks to see if one class inherits from another 
        /// </summary>
        /// <param name="testClass"></param>
        /// <param name="baseClass"></param>
        /// <returns></returns>
        private bool Inherits(Type testClass, Type baseClass)
        {
            return baseClass.IsAssignableFrom(testClass);
        }

        /// <summary>
        /// Returns a Type from a given assembly and namespace 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="fullNamespace"></param>
        /// <returns></returns>
        private Type GetType(string assembly, string fullNamespace)
        {
            Assembly a = Assembly.Load(assembly);
            return a.GetType(fullNamespace);
        }

        private void okButton_Click(object sender, EventArgs e)
        {            
            string pluginName = GetMetadataTypeName(PluginComboBox.SelectedItem.ToString());
            string pluginAssembly = PluginComboBox.SelectedItem.ToString();

            if (PluginFactory.GetPluginByAssemblyName(pluginAssembly).Implements<IPluginConfigurationControl>())
            {
                SetPluginInfo(pluginAssembly, pluginName);
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show($"'{pluginAssembly}' is not an STB or STE plugin.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Sets plugin info for PluginMetadataListForm 
        /// to display on PluginMetadataEditForm
        /// </summary>
        /// <param name="pluginAssemblyName"></param>
        /// <param name="pluginName"></param>
        private void SetPluginInfo(string pluginAssemblyName, string pluginName)
        {
            PluginName = pluginName;
            PluginDisplayName = pluginName;
            PluginAssemblyName = pluginAssemblyName;
            PluginIcon = null;
        }

        /// <summary>
        /// Gets the plugin folder absolute path.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetPluginFolderPath()
        {
            string exeRootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string tempPath = Path.Combine(exeRootDir, GlobalSettings.Items[Setting.PluginRelativeLocation]);
            Uri uri = new Uri(tempPath);

            return uri.LocalPath;
        }

        // Used to write icon update script for database update
        //private void iconUpdateScript()
        //{
        //    STFIcons pluginIcons = new STFIcons();
        //    ImageList iconList = pluginIcons.ResourceMetadataIcons;
        //    string script = string.Empty;
        //    IconConverter iconConverter = new IconConverter();
        //    byte[] byteArray;
        //    List<string> lines = new List<string>();

        //    string location = @"C:\PluginIcons\updateScript.txt";
        //    foreach (string key in iconList.Images.Keys)
        //    {
              
        //        Bitmap Image = new Bitmap(iconList.Images[key]);                // get image
        //        byteArray = iconConverter.imageToByteArray(Image);

        //        string byteString = "0x" + BitConverter.ToString(byteArray).Replace("-", "");

        //        script = "update MetadataType\nset Icon = convert(varbinary(max),'" + byteString + "',1)\nwhere Name = '" + key + "'\n";
        //        string goodScript = script.Replace("\n", Environment.NewLine);
        //        lines.Add(goodScript);
                           
        //    }

        //    using (StreamWriter file = new StreamWriter(location))
        //    {
        //        foreach(string line in lines)
        //        {
        //            file.WriteLine(line);
        //        }
        //    }
        //}
        
    }
}
