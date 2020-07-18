using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.LabConsole;
using HP.ScalableTest.UI;
using HP.ScalableTest.UI.Framework;
using System;
using System.Windows.Forms;
using HP.ScalableTest.Data;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.UI;

namespace HP.ScalableTest
{
    public partial class AdminControlPanel : Form
    {
        private string _importDirectory = string.Empty;

        static AdminControlPanel()
        {
            UserInterfaceStyler.Initialize();
        }

        public AdminControlPanel()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void systemSettingsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var dialog = new GlobalSettingsListForm(SettingType.SystemSetting))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    GlobalSettings.Refresh();
                }
            }
        }

        private void AdminControlPanel_Load(object sender, EventArgs e)
        {
            try
            {
                if (!ConnectToEnvironment())
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                // Inform the user of the error and allow them to send but do not kill the app unless they choose
                // For example, they may instead want to change environment.
                ApplicationExceptionHandler.UnhandledException(ex);
            }

            decimal version = 0;
            using (SqlAdapter adapter = new SqlAdapter(EnterpriseTestSqlConnection.ConnectionString))
            {
                try
                {
                    string sqlText = "SELECT value FROM fn_listextendedproperty(N'STF Revision', NULL, NULL, NULL, NULL, NULL, NULL)";
                    var reader = adapter.ExecuteReader(sqlText);
                    if (reader != null && reader.Read())
                    {
                        version = decimal.Parse(reader["value"].ToString());
                    }
                }                
                catch (Exception)
                {                    
                }
            }

            if (version != 0)
            {
                versionLabel.Text = "STB v{0:#.##}".FormatWith(version);
            }
            else
            {
                versionLabel.Text = string.Empty;
            }
        }

        private bool ConnectToEnvironment()
        {
            if (!STFLoginManager.Login())
            {
                return false; //User canceled the login
            }
            return true;
        }

        private void LaunchDialog<T>(bool refreshSettings = false) 
            where T : Form, new()
        {
            using (new BusyCursor())
            {
                using (var form = new T())
                {
                    var result = form.ShowDialog();
                    if (refreshSettings && result == DialogResult.OK)
                    {
                        GlobalSettings.Refresh();
                    }
                }
            }
        }

        private void serverInventoryLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<FrameworkServerListForm>();
        }

        private void printServersLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (PrintServerManagementForm form = new PrintServerManagementForm())
            {
                form.ServerType = Framework.ServerType.Print;
                form.ShowDialog(this);
            }
        }

        private void groupManagementLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<UserGroupsConfigForm>();
        }

        private void userManagementLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<UserManagementListForm>();
        }

        private void pluginAssociationsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<PluginMetadataListForm>();
        }

        private void deviceInventoryLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<PrinterListForm>();
        }

        private void testDocumentsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<DocumentListForm>();
        }

        private void printDriverLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<PrintDriverConfigForm>();
        }

        private void softwareInstallers_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<SoftwareInstallersForm>();
        }

        private void packagesLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<InstallerPackagesForm>();
        }

        private void virtualWorkerPoolsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<DomainAccountListForm>(true);
        }

        private void pluginSettingsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var dialog = new GlobalSettingsListForm(SettingType.PluginSetting))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    GlobalSettings.Refresh();
                }
            }
        }

        private void badgeBox_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<BadgeBoxListForm>();
        }

        private void assetPoolsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<AssetPoolListForm>();
        }

        private void simulators_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<SimulatorListForm>();
        }

        private void camera_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<CameraListForm>();
        }

        private void mobileDevice_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchDialog<MobileDeviceListForm>();
        }
    }
}
