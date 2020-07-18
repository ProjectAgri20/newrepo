using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.CertificateInstaller
{
    [ToolboxItem(false)]
    public partial class CertificateInstallerConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private CertificateInstallerActivityData _certificateInstallerActivityData;
        private string _certificateRepository;

        public CertificateInstallerConfigurationControl()
        {
            InitializeComponent();
            Intermediate_CA.DataBindings.Add("Enabled", Intermediate_CA, "Checked");
            certificate_fieldValidator.RequireSelection(browser_comboBox, browser_label);
            certificate_fieldValidator.RequireCustom(certificates_checkedListBox, ValidateCertificateCheckListBox, "Please select a certificate");
            certificate_fieldValidator.RequireAssetSelection(certificate_assetSelectionControl);

            certificate_assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            browser_comboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            certificates_checkedListBox.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            InstallPrinter_CA.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            DeletePrinter_CA.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            InstallVM_CA.CheckedChanged += (s, e) => ConfigurationChanged(s, e);

        }

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            _certificateInstallerActivityData = new CertificateInstallerActivityData();

            try
            {
                _certificateRepository = environment.PluginSettings["CertificationStore"];
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Certification Store path not defined. Please define the setting and try again.");
                return;
            }
           
            LoadUi();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _certificateInstallerActivityData = configuration.GetMetadata<CertificateInstallerActivityData>();
            certificate_assetSelectionControl.Initialize(configuration.Assets, Framework.Assets.AssetAttributes.None);
            try
            {
                _certificateRepository = environment.PluginSettings["CertificationStore"];
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Certification Store path not defined. Please define the setting and try again.");
                return;
            }
            LoadUi();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _certificateInstallerActivityData.InstallPrinterCA = InstallPrinter_CA.Checked;
            _certificateInstallerActivityData.ClientVMCA = InstallVM_CA.Checked;
            _certificateInstallerActivityData.DeletePrinterCA = DeletePrinter_CA.Checked;
            _certificateInstallerActivityData.IntermediateCA = Intermediate_CA.Checked;
            _certificateInstallerActivityData.BrowserType = browser_comboBox.Text;
            _certificateInstallerActivityData.CACertificate = certificates_checkedListBox.SelectedItem.ToString();

            return new PluginConfigurationData(_certificateInstallerActivityData, "1.0")
            {
                Assets = certificate_assetSelectionControl.AssetSelectionData
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(certificate_fieldValidator.ValidateAll());
        }

        private void LoadUi()
        {
            certificates_checkedListBox.Items.Clear();

            foreach (var file in Directory.GetFiles(_certificateRepository, "*.*", SearchOption.AllDirectories))
            {
                certificates_checkedListBox.Items.Add(file);
            }

            InstallPrinter_CA.Checked = _certificateInstallerActivityData.InstallPrinterCA;
            DeletePrinter_CA.Checked = _certificateInstallerActivityData.DeletePrinterCA;
            InstallVM_CA.Checked = _certificateInstallerActivityData.ClientVMCA;
            Intermediate_CA.Checked = _certificateInstallerActivityData.IntermediateCA;

            var items = certificates_checkedListBox.Items.Cast<string>().Select((item, i) => new { FileName = item, Index = i });
            var filename = items.FirstOrDefault(x => x.FileName.Equals(_certificateInstallerActivityData.CACertificate));
            if (filename != null)
            {
                certificates_checkedListBox.SelectedIndex = filename.Index;
                certificates_checkedListBox.SetItemChecked(filename.Index, true);
            }
            browser_comboBox.SelectedItem = _certificateInstallerActivityData.BrowserType;
        }

        private void Certificates_CheckChanged(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                CheckedListBox.CheckedIndexCollection coll = certificates_checkedListBox.CheckedIndices;

                foreach (int index in coll)
                {
                    certificates_checkedListBox.SetItemCheckState(index, CheckState.Unchecked);
                }

                foreach (string item in certificates_checkedListBox.CheckedItems)
                {
                    string certificateName = item;
                    _certificateInstallerActivityData.CACertificate = certificateName;
                }
            }
        }

        private bool ValidateCertificateCheckListBox()
        {
            CheckedListBox.CheckedIndexCollection coll = certificates_checkedListBox.CheckedIndices;
            if (coll.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}