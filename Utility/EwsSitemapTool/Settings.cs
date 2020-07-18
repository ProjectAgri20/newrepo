using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Tools
{
    public partial class Settings : Form
    {
        #region Constructor

        public Settings()
        {
            InitializeComponent();

            LoadUI();
        }

        #endregion        

        #region Properties

        public string SimapLocation
        {
            get
            {
                return location_TextBox.Text;
            }
        }

        #endregion

        #region Private Methods

        private void LoadUI()
        {
            try
            {
                location_TextBox.Text = GlobalSettings.Items["EWSSitemapLocation"];

                HashSet<string> keys = MasterKeys.Instance().GetKeys();

                int res;

                int rem = Math.DivRem(keys.Count, 3, out res);

                int i = 0;

                for (int row = 0; row < rem; row++)
                {
                    SiteMapDataSet.KeysRow keyRow = siteMapDataSet.Keys.NewKeysRow();

                    keyRow.Key1 = keys.ElementAt(i++);
                    keyRow.Key2 = keys.ElementAt(i++);
                    keyRow.Key3 = keys.ElementAt(i++);

                    siteMapDataSet.Keys.AddKeysRow(keyRow);
                }

                if (res > 0)
                {
                    SiteMapDataSet.KeysRow row2 = siteMapDataSet.Keys.NewKeysRow();

                    if (res == 1)
                    {
                        row2.Key1 = keys.ElementAt(keys.Count - 1);
                    }
                    else if (res == 2)
                    {
                        row2.Key1 = keys.ElementAt(keys.Count - 2);
                        row2.Key2 = keys.ElementAt(keys.Count - 1);
                    }

                    siteMapDataSet.Keys.AddKeysRow(row2);
                }
            }
            catch (SettingNotFoundException)
            {
                keys_DataGridView.Enabled = false;
            }

        }

        #endregion        

        #region Events

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (keys_DataGridView.Enabled)
            {
                HashSet<string> keys = new HashSet<string>();

                foreach (SiteMapDataSet.KeysRow row in siteMapDataSet.Keys.Rows)
                {
                    if (!row.IsKey1Null() && !string.IsNullOrWhiteSpace(row.Key1))
                    {
                        keys.Add(row.Key1);
                    }

                    if (!row.IsKey2Null() && !string.IsNullOrWhiteSpace(row.Key2))
                    {
                        keys.Add(row.Key2);
                    }

                    if (!row.IsKey3Null() && !string.IsNullOrWhiteSpace(row.Key3))
                    {
                        keys.Add(row.Key3);
                    }
                }

                MasterKeys.Instance().SetKeys(keys);
            }
            else
            {
                GlobalSettings.Items.Add("EwsSitemapLocation", location_TextBox.Text); 
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void browse_Button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (DialogResult.OK == fbd.ShowDialog())
            {
                location_TextBox.Text = fbd.SelectedPath;
            }
        }

        #endregion        
    }
}
