using HP.ScalableTest.DeviceAutomation.Enums;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.CloudConnector
{
    /// <summary>
    /// Control used to configure the CloudConnector execution data.
    /// </summary>
    public partial class CloudConnectorConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private const AssetAttributes DeviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        private LinkScanOptions _soptions = new LinkScanOptions();
        private LinkPrintOptions _poptions = new LinkPrintOptions();
        private CloudConnectorActivityData _activityData;

        /// <summary>
        /// Event used to signal a change in configuration data has occurred.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public CloudConnectorConfigurationControl()
        {
            InitializeComponent();

            tb_PWD.PasswordChar = '●';

            fieldValidator.RequireAssetSelection(assetSelectionControl);
            fieldValidator.RequireValue(tb_ID, lb_ID, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(tb_PWD, lb_PWD, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(tb_FilePath, lb_FilePath, rb_Print);

            comboBox_SIO.DataSource = EnumUtil.GetDescriptions<SIOMethod>().ToList();
            comboBox_SIO.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);

            cb_AppName.DataSource = EnumUtil.GetDescriptions<ConnectorName>().ToList();
            cb_AppName.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);

            comboBox_Logout.DataSource = EnumUtil.GetDescriptions<LogOutMethod>().ToList();
            comboBox_Logout.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);

            tb_ID.TextChanged += (s, e) => ConfigurationChanged(s, e);
            tb_PWD.TextChanged += (s, e) => ConfigurationChanged(s, e);
            tb_SharePointSites.TextChanged += (s, e) => ConfigurationChanged(s, e);

            nud_PageCount.ValueChanged += (s, e) => ConfigurationChanged(s, e);

            tb_FolderPath.TextChanged += (s, e) => ConfigurationChanged(s, e);
            tb_FilePath.TextChanged += (s, e) => ConfigurationChanged(s, e);
            tb_MP_FileNames.TextChanged += (s, e) => ConfigurationChanged(s, e);
            tb_MP_FolderPath.TextChanged += (s, e) => ConfigurationChanged(s, e);

            rb_Scan.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            rb_Print.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            rb_MultipleFile.CheckedChanged += (s, e) => ConfigurationChanged(s, e);

            lockTimeoutControl.ValueChanged += (s, e) => ConfigurationChanged(s, e);

            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
        }

        /// <summary>
        /// Initializes the configuration control to default values.
        /// </summary>
        /// <param name="environment">Domain and plugin specific environment data.</param
        public void Initialize(PluginEnvironment environment)
        {
            ConfigureControls(new CloudConnectorActivityData());
            assetSelectionControl.Initialize(DeviceAttributes);            
        }

        /// <summary>
        /// Initializes the configuration control with the supplied configuration settings.
        /// </summary>
        /// <param name="configuration">Pre-configured plugin settings.</param>
        /// <param name="environment">Domain and plugin specific environment data.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            var data = configuration.GetMetadata<CloudConnectorActivityData>();
            ConfigureControls(data);
            _soptions = data.CloudScanOptions;
            _poptions = data.CloudPrintOptions;
            lockTimeoutControl.Initialize(data.LockTimeouts);

            if (data.CloudJobType.Equals(ConnectorJobType.Scan.GetDescription()))
            {                
                rb_Scan.Checked = true;
                rb_Print.Checked = false;
                rb_MultipleFile.Checked = false;
            }
            else if(data.CloudJobType.Equals(ConnectorJobType.Print.GetDescription()))
            {
                rb_Scan.Checked = false;
                rb_Print.Checked = true;
                rb_MultipleFile.Checked = false;
            }
            else if(data.CloudJobType.Equals(ConnectorJobType.MultiplePrint.GetDescription()))
            {
                rb_Scan.Checked = false;
                rb_Print.Checked = false;
                rb_MultipleFile.Checked = true;
            }

            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
        }

        /// <summary>
        /// Returns all of the HPInc_STB_Plugin_Template1 configuration data along with a version string.
        /// </summary>
        /// <returns>Configuration data and version string.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            List<string> filelist = new List<string>();
            for(int i=0; i<lbx_FileNames.Items.Count; i++)
            {
                filelist.Add(lbx_FileNames.Items[i].ToString());
            }

            _activityData = new CloudConnectorActivityData()
            {
                SIO = EnumUtil.GetByDescription<SIOMethod>(comboBox_SIO.SelectedItem.ToString()),
                CloudAppType = EnumUtil.GetByDescription<ConnectorName>(cb_AppName.SelectedItem.ToString()),
                ID = tb_ID.Text,
                PWD = tb_PWD.Text,
                LogOut = EnumUtil.GetByDescription<LogOutMethod>(comboBox_Logout.SelectedItem.ToString()),
                PageCount = (int)nud_PageCount.Value,
                
                CloudScanOptions = _soptions,                
                CloudPrintOptions = _poptions,
                LockTimeouts = lockTimeoutControl.Value
            };

            _activityData.CloudScanOptions.PageCount = _activityData.PageCount;

            if (!String.IsNullOrEmpty(tb_SharePointSites.Text))
            {
                _activityData.SharePointSite = tb_SharePointSites.Text;
            }
            
            if (rb_Scan.Checked)
            {
                _activityData.FolderPath = tb_FolderPath.Text;
                _activityData.CloudJobType = ConnectorJobType.Scan.GetDescription();
                _activityData.CloudScanOptions.AppName = _activityData.CloudAppType.ToString();
            }
            else if(rb_Print.Checked)
            {
                _activityData.FilePath = tb_FilePath.Text;
                _activityData.CloudJobType = ConnectorJobType.Print.GetDescription();
                _activityData.CloudPrintOptions.AppName = _activityData.CloudAppType.ToString();
            }
            else if (rb_MultipleFile.Checked)
            {
                _activityData.FilePath = tb_MP_FolderPath.Text;
                _activityData.FileList = filelist;
                _activityData.CloudJobType = ConnectorJobType.MultiplePrint.GetDescription();
                _activityData.CloudPrintOptions.AppName = _activityData.CloudAppType.ToString();
            }

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,                
            };
        }

        /// <summary>
        /// Validate the configuration settings before saving.
        /// </summary>
        /// <returns>Information about the validation results.</returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void ConfigureControls(CloudConnectorActivityData data)
        {
            comboBox_SIO.Text = data.SIO.GetDescription();
            cb_AppName.Text = data.CloudAppType.GetDescription();
            tb_ID.Text = data.ID;
            tb_PWD.Text = data.PWD;
            comboBox_Logout.Text = data.LogOut.GetDescription();
            nud_PageCount.Value = data.PageCount;

            tb_FolderPath.Text = data.FolderPath;
            tb_FilePath.Text = data.FilePath;
            tb_MP_FolderPath.Text = data.FilePath;
            
            if(data.FileList != null)
            {
                foreach (string str in data.FileList)
                {
                    lbx_FileNames.Items.Add(str);
                }
            }            
            
            if(!String.IsNullOrEmpty(data.SharePointSite))
            {
                tb_SharePointSites.Text = data.SharePointSite;
            }

            lockTimeoutControl.Initialize(data.LockTimeouts);

        }

        private void btn_Options_Click(object sender, EventArgs e)
        {            
            if (rb_Scan.Checked)
            {
                using (var preferences = new LinkScanOptionsForm(_soptions))
                {
                    if (preferences.ShowDialog() == DialogResult.OK)
                    {
                        _soptions = preferences.LinkScanOption;
                    }
                }
            }
            else
            {
                using (var preferences = new LinkPrintOptionsForm(_poptions))
                {
                    if (preferences.ShowDialog() == DialogResult.OK)
                    {
                        _poptions = preferences.LinkPrintOption;
                    }
                }
            }

        }

        private void rb_Print_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Print.Checked)
            {
                lb_FilePath.Visible = true;
                tb_FilePath.Visible = true;
                lb_FilePathDesc.Visible = true;                
            }
            else
            {
                lb_FilePath.Visible = false;
                tb_FilePath.Visible = false;
                lb_FilePathDesc.Visible = false;

                tb_FilePath.Text = "";                
            }
        }

        private void rb_Scan_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Scan.Checked)
            {
                lb_FolderPath.Visible = true;
                tb_FolderPath.Visible = true;
                lb_FolderPathDesc.Visible = true;

                lb_PageCount.Visible = true;
                lb_PageCountDes.Visible = true;
                nud_PageCount.Visible = true;

                gb_PageCount.Visible = true;
            }
            else
            {
                lb_FolderPath.Visible = false;
                tb_FolderPath.Visible = false;
                lb_FolderPathDesc.Visible = false;

                lb_PageCount.Visible = false;
                lb_PageCountDes.Visible = false;
                nud_PageCount.Visible = false;

                tb_FolderPath.Text = "";

                gb_PageCount.Visible = false;                
            }
        }

        private void rb_MultipleFile_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_MultipleFile.Checked)
            {
                lb_MP_FolderPath.Visible = true;
                lb_MP_FileNames.Visible = true;
                lb_SelectedFiles.Visible = true;
                lb_MPDesc.Visible = true;
                
                tb_MP_FolderPath.Visible = true;
                tb_MP_FileNames.Visible = true;

                btn_Add.Visible = true;
                btn_clear.Visible = true;
                button_delete.Visible = true;

                lbx_FileNames.Visible = true;

                fieldValidator.RequireCustom(lbx_FileNames, () => lbx_FileNames.Items.Count >= 1, "At least one file must be selected.");
            }
            else
            {
                lb_MP_FolderPath.Visible = false;
                lb_MP_FileNames.Visible = false;
                lb_SelectedFiles.Visible = false;
                lb_MPDesc.Visible = false;

                tb_MP_FolderPath.Visible = false;
                tb_MP_FileNames.Visible = false;

                btn_Add.Visible = false;
                btn_clear.Visible = false;
                button_delete.Visible = false;
                lbx_FileNames.Visible = false;

                tb_MP_FolderPath.Text = "";
                tb_MP_FileNames.Text = "";
                lbx_FileNames.Items.Clear();

                fieldValidator.Remove(lbx_FileNames);                
            }
        }

        private void cb_AppName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectAccountInfo _accountInfo = (ConnectAccountInfo)Enum.Parse(typeof(ConnectAccountInfo), cb_AppName.Text);

            if (cb_AppName.Text.Equals(ConnectorName.SharePoint.GetDescription()))
            {
                lb_SharePointSites.Visible = true;
                tb_SharePointSites.Visible = true;
                lb_SharePointSites_Desc.Visible = true;

                fieldValidator.RequireValue(tb_SharePointSites, lb_SharePointSites);
            }
            else
            {
                lb_SharePointSites.Visible = false;
                tb_SharePointSites.Visible = false;
                lb_SharePointSites_Desc.Visible = false;

                tb_SharePointSites.Text = "";
                fieldValidator.Remove(tb_SharePointSites);
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {            
            if(tb_MP_FileNames.Text != "")
            {
                lbx_FileNames.Items.Add(tb_MP_FileNames.Text);
                tb_MP_FileNames.Text = "";
            }
            
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            lbx_FileNames.Items.Clear();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if(lbx_FileNames.SelectedItem != null)
            {
                lbx_FileNames.Items.Remove(lbx_FileNames.SelectedItem);
            }
        }

        private void comboBox_SIO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_SIO.Text.Equals(SIOMethod.SIOWithoutIDPWD.GetDescription()))
            {
                tb_ID.Enabled = false;
                tb_PWD.Enabled = false;
            }
            else
            {
                tb_ID.Enabled = true;
                tb_PWD.Enabled = true;
            }
        }
    }
}
