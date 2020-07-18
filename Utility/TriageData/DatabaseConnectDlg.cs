using log4net;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace HP.ScalableTestTriageData.Data
{
    public partial class DatabaseConnectDlg : Form
    {
        private static ILog log = LogManager.GetLogger("Program");
        public DatabaseConnectDlg()
        {
            InitializeComponent();
            InitializeDropDown();

            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Initializes the Database drop down
        /// </summary>
        private void InitializeDropDown()
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                cbDbServer.Items.Add(key);
            }

            // select the first item
            cbDbServer.SelectedIndex = 1;
        }

        /// <summary>
        /// Event when the connect button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalSettings.Database = ConfigurationManager.AppSettings[cbDbServer.SelectedItem.ToString()];
                GlobalSettings.EnvironmentName = ConfigurationManager.AppSettings.AllKeys[cbDbServer.SelectedIndex].ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Event when the database selection changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnConnect.Enabled = true;
        }
    }
}
