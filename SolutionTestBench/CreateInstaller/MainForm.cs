using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateInstaller
{
    public partial class MainForm : Form
    {
        ConcurrentStack<InstallerForm> _processForms = new ConcurrentStack<InstallerForm>();

        private string WorkingPath
        {
            set { textBox_WorkingPath.Text = value; }
            get { return textBox_WorkingPath.Text; }
        }

        public MainForm()
        {
            InitializeComponent();

            string workingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            textBox_WorkingPath.Text = Path.GetDirectoryName(workingFolder);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!ValidateWorkingPath())
            {
                var dr = MessageBox.Show("The required Configuration directory is missing in the deployment package.\nSelect different working folder?", "Missing Directory", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dr == DialogResult.Yes)
                {
                    while (WorkingPath != string.Empty && !ValidateWorkingPath())
                    {
                        WorkingPath = SelectWorkFolder();
                        if (string.IsNullOrEmpty(WorkingPath))
                        {
                            Application.Exit();
                        }
                    }
                }
            }

            string configDirectory = GetConfigurationPath();
            textBox_InstallerPath.Text = GetInstallerPath();

            if (!Directory.Exists(configDirectory))
            {
                MessageBox.Show("The required Configuration directory is missing in the deployment package.", "Missing Directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
                okButton.Enabled = false;
            }

            if (!Directory.Exists(GetAllPluginsPath(configDirectory)))
            {
                MessageBox.Show("The AllPlugins Configuration directory is missing in the deployment package.", "Missing Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Collection<string> directories = new Collection<string>();
            foreach (string directory in Directory.GetDirectories(configDirectory))
            {
                directories.Add(new DirectoryInfo(directory).Name);
            }

            ((ListBox)listBox_BuildConfig).DataSource = directories;

            // Seed default label value
            SetPackageLabel();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string configuration = ValidateBuildConfig();
            if (string.IsNullOrEmpty(configuration))
            {
                return;
            }

            if (checkBox_Build.Checked)
            {
                if (ValidateBuild())
                {
                    List<Task> tasks = new List<Task>();                    

                    if (serverCheckBox.Checked)
                    {
                        //tasks.Add(StartInstallerBuildThread("Server", configuration));
                        tasks.Add(Task.Factory.StartNew(() => BuildInstaller("Server", configuration)));
                    }

                    if (clientCheckBox.Checked)
                    {
                        //tasks.Add(StartInstallerBuildThread("Client", configuration));
                        tasks.Add(Task.Factory.StartNew(() => BuildInstaller("Client", configuration)));
                    }

                    if (tasks.Any())
                    {
                        UpdateStatus("Installer builds started...");
                        Task.Factory.StartNew(() => PostProcessHandler());
                        UpdateStatus("Installer builds finished");
                    }

                    _processForms.Clear();
                    tasks.Clear();
                }
                else
                {
                    //We don't want to try to Sign and Package if build is not valid.
                    return;
                }
            }

            // The Post Process Installation handles signing and packaging if the user has selected to do so.
            // But if the user has chosen to sign and package an existing build, it has to be kicked off
            // from this event handler.
            if (checkBox_SignPackage.Checked)
            {
                if (ValidatePackage())
                {
                    SignAndPackage(textBox_Label.Text, configuration);
                }
            }

            UpdateStatus("Finished");
        }

        private void PostProcessHandler()
        {
            // Wait until we have forms and none of them are processing
            while (_processForms.Any() && _processForms.Any(x => x.Status == InstallerStatus.Processing) == false)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            if (_processForms.All(x => x.Status == InstallerStatus.Completed))
            {
                // close the open windows if all successful
                foreach(InstallerForm form in _processForms)
                {
                    if (form != null && form.InvokeRequired)
                    {
                        form.Invoke(new MethodInvoker(() => form.Close()));
                    }
                }
            }
            else
            {
                UpdateStatus("Error Processing");
            }
        }

        private void SignAndPackage(string fileLabel, string configuration)
        {
            if (checkBox_Authenticate.Checked)
            {
                UpdateStatus("Authenticating...");
                Signer.Authenticate();
            }

            if (checkBox_Sign.Checked)
            {
                UpdateStatus("Signing files...");
                SignFiles(fileLabel);
            }

            if (checkBox_Package.Checked)
            {
                UpdateStatus("Creating packages...");
                CreatePackages(fileLabel, configuration);
            }
        }

        private void BuildInstaller(string installerType, string configuration)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => BuildInstaller(installerType, configuration)));
                return;
            }

            Installer installerBuilder = new Installer(WorkingPath, installerType, textBox_Label.Text, configuration);
            InstallerForm form = new InstallerForm(installerBuilder, WorkingPath);

            // Add the form to the list so that we can monitor it's progress
            _processForms.Push(form);
            form.Show(this);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string GetConfigurationPath()
        {
            return Path.Combine(WorkingPath, "Configuration");
        }

        private string GetInstallerPath()
        {
            return Path.Combine(WorkingPath, "Installers");
        }

        private string GetAllPluginsPath(string configDirectory)
        {
            return Path.Combine(configDirectory, "AllPlugins");
        }

        private bool ValidateWorkingPath()
        {
            bool result = false;
            string configDirectory = GetConfigurationPath();
            if (Directory.Exists(configDirectory) && Directory.Exists(GetAllPluginsPath(configDirectory)))
            {
                result = true;
            }
            return result;
        }

        private bool ValidateBuild()
        {
            if (string.IsNullOrEmpty(textBox_Label.Text))
            {
                MessageBox.Show("You must enter a Package Label", "Missing Package Label", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!serverCheckBox.Checked && !clientCheckBox.Checked)
            {
                MessageBox.Show("Choose one or both installer types", "Missing Installer Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidatePackage()
        {
            if (Directory.GetFiles(GetInstallerPath()).Length < 1)
            {
                MessageBox.Show("No Installer files were found to package.", "Missing Installer Files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates and returns the selected build configuration.
        /// </summary>
        /// <returns>The selected configuration</returns>
        private string ValidateBuildConfig()
        {
            switch (listBox_BuildConfig.CheckedItems.Count)
            {
                case 0:
                    MessageBox.Show("Please choose a Build Configuration", "Missing Build Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                case 1:
                    break;
                default:
                    if (checkBox_SignPackage.Checked)
                    {
                        MessageBox.Show("Unable to sign more than one package per build.  Please select one Build Configuration at a time for signing and packaging.", "Multiple Build Configurations", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return string.Empty;
                    }
                    break;
            }

            string configuration = listBox_BuildConfig.CheckedItems[0].ToString();

            if (checkBox_SignPackage.Checked && !checkBox_Build.Checked)
            {
                // Not building, just signing only.  Make sure the selected configuration matches the existing Installers.
                string installerFolder = textBox_InstallerPath.Text;
                if (! Directory.GetFiles(installerFolder).Any(x => x.Contains(configuration)))
                {
                    MessageBox.Show("Selected Configuration does not exist in the Installers folder.  \nPlease check folder or select a different build configuration.", "Build Configuration Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                }
            }

            return configuration;
        }

        private void SetPackageLabel(string labelText = null)
        {
            try
            {
                if (string.IsNullOrEmpty(labelText))
                {
                    // Set the default
                    labelText = Installer.GetPackageLabel(WorkingPath);
                }

                textBox_Label.Text = labelText;
            }
            catch { }
        }

        private void SignFiles(string fileLabel)
        {
            List<Task> tasks = new List<Task>();
            string fileFolder = Path.Combine(WorkingPath, "Examples\\References");

            UpdateStatus("Signing Sdk files...");
            foreach (string filePath in Directory.GetFiles(fileFolder))
            {
                tasks.Add(Task.Factory.StartNew(() => SignFile(filePath, fileFolder)));
                //Copies the signed files back into the original location
            }

            if (tasks.Any())
            {
                Task.Factory.StartNew(() => PostProcessHandler());
                UpdateStatus("Sdk files signed.");
            }
            else
            {
                UpdateStatus("No Sdk files found to sign.");
            }

            _processForms.Clear();
            tasks.Clear();

            UpdateStatus("Signing STB Installers...");
            fileFolder = textBox_InstallerPath.Text;
            //Sign installer .exes
            foreach (string filePath in Directory.GetFiles(fileFolder).Where(x => x.Contains(fileLabel)))
            {
                tasks.Add(Task.Factory.StartNew(() => SignFile(filePath, fileFolder)));
            }

            if (tasks.Any())            {
                Task.Factory.StartNew(() => PostProcessHandler());
                UpdateStatus("STB Installers signed.");
            }
            else
            {
                UpdateStatus("No Installers found to sign.");
            }

            _processForms.Clear();
            tasks.Clear();
        }

        private void SignFile(string filePathToSign, string outputPath)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => SignFile(filePathToSign, outputPath)));
                return;
            }

            Signer signer = new Signer(filePathToSign, outputPath);
            InstallerForm form = new InstallerForm(signer, WorkingPath);

            // Add the form to the list so that we can monitor it's progress
            _processForms.Push(form);
            form.Show(this);
        }


        /// <summary>
        /// Zip the signed sdk .dlls and supporting files
        /// </summary>
        private void CreatePackages(string fileLabel, string configuration)
        {
            string outputLocation = Path.Combine(WorkingPath, "Packages");

            CreateSdkPackage(fileLabel, configuration, outputLocation);

            UpdateStatus("Creating STB Installer package...");
            //Zip the signed installer files
            List<string> installerFiles = new List<string>();
            foreach (string filePath in Directory.GetFiles(GetInstallerPath()).Where(x=>x.Contains(fileLabel) && x.Contains(configuration)))
            {
                installerFiles.Add(filePath);
            }

            // Add AdminGuide.doc
            foreach (string filePath in Directory.GetFiles(Path.Combine(WorkingPath, "Documentation")))
            {
                installerFiles.Add(filePath);
            }

            CreateSecurePackage(installerFiles, $"STBInstall-{configuration}-{fileLabel}", outputLocation);
        }

        private void CreateSdkPackage(string fileLabel, string configuration, string outputLocation)
        {
            UpdateStatus($"Creating Sdk package for {configuration}-{fileLabel}...");
            List<string> sdkFiles = new List<string>();

            // Add Documentation (.pdf files and .chm file)
            foreach (string filePath in Directory.GetFiles(Path.Combine(WorkingPath, "Documentation\\PluginSdk")))
            {
                sdkFiles.Add(filePath);
            }

            // Add Plugin SDK Example Project folder
            sdkFiles.Add(Path.Combine(WorkingPath, "Examples"));

            CreateSecurePackage(sdkFiles, $"PluginSdk-{fileLabel}", outputLocation); //Create the Zip
        }

        private void CreateSecurePackage(List<string> filePaths, string packageName, string outputFolderPath)
        {
            string packageFilePath = Packager.CreatePackage(filePaths, packageName, outputFolderPath);
            Packager.CreateMd5(packageFilePath);
        }

        private void UpdateStatus(string statusText)
        {
            if (label_Status.InvokeRequired)
            {
                label_StatusDisplay.Invoke(new MethodInvoker(() => UpdateStatus(statusText)));
                return;
            }
            label_StatusDisplay.Text = statusText;
            Application.DoEvents();
        }

        private void buttonFolderBrowse_Click(object sender, EventArgs e)
        {
            SelectWorkFolder();
        }

        private string SelectWorkFolder()
        {
            string result = string.Empty;
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the working path...";
                dialog.SelectedPath = "D:\\Work\\STBBuilds";
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    result = dialog.SelectedPath;
                }
            }
            return result;
        }

        /// <summary>
        /// Make the configuration ListBox Single-select.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_BuildConfig_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < listBox_BuildConfig.Items.Count; ++i)
                {
                    if (i != e.Index)
                    {
                        listBox_BuildConfig.SetItemChecked(i, false);
                    }
                }
            }

        }

        private void checkBox_Build_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_Build.Enabled = checkBox_Build.Checked;
        }

        private void checkBox_SignPackage_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_SignPackage.Enabled = checkBox_SignPackage.Checked;
        }

        private void button_ViewPackage_Click(object sender, EventArgs e)
        {
            string packageLabel = string.Empty;
            string configuration = string.Empty;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "STB Installer|*.exe";
                dialog.FilterIndex = 0;
                dialog.InitialDirectory = textBox_InstallerPath.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var parts = dialog.FileName.Split('-');
                    if (parts.Length >= 2)
                    {
                        packageLabel = parts[2].Substring(0, parts[2].Length - 4);
                        configuration = parts[1];
                    }
                }
            }
            SetPackageLabel(packageLabel);
        }

        private void Sign_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked)
            {
                checkBox_SignPackage.Checked = true;
            }
        }
    }
}
