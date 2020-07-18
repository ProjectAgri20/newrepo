using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Class that supports selection of an STF system 
    /// </summary>
    public partial class SystemSelectionDialog : Form
    {
        private string _database = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSelectionDialog"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public SystemSelectionDialog(string database)
        {
            InitializeComponent();

            _database = database;
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string Database
        {
            get { return _database; }
        }

        private string FindName()
        {
            string name = string.Empty;

            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (ConfigurationManager.AppSettings[key].Equals(_database, StringComparison.OrdinalIgnoreCase))
                {
                    name = key;
                    break;
                }
            }

            return name;
        }

        private void SystemSelectionDialog_Load(object sender, EventArgs e)
        {
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                env_ComboBox.Items.Add(key);
            }

            var name = FindName();

            env_ComboBox.SelectedIndexChanged += new EventHandler(env_ComboBox_SelectedIndexChanged);


            if (!string.IsNullOrEmpty(name) && env_ComboBox.Items.Contains(name))
            {
                env_ComboBox.SelectedItem = name;
            }
            else if (env_ComboBox.Items.Count > 0)
            {
                env_ComboBox.SelectedIndex = 0;
            }
            else
            {
                ok_Button.Enabled = false;
            }
        }

        void env_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _database = ConfigurationManager.AppSettings[env_ComboBox.Text];
        }
    }
}
