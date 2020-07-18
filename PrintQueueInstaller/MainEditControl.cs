using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI;
using Microsoft.Win32;
using HP.ScalableTest.Framework;
using RegistryHive = HP.ScalableTest.WindowsAutomation.Registry.RegistryHive;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Edit control for the print queue installer
    /// </summary>
    public partial class MainEditControl : FieldValidatedControl
    {
        private SortableBindingList<QueueInstallationData> _queueDefinitions = new SortableBindingList<QueueInstallationData>();

        private QueueManager _manager = new QueueManager();
        private DriverUpgradeForm _upgradeForm = null;
        private int _threadCount = 1;
        private TimeSpan _installationTimeout = TimeSpan.FromMinutes(5);

        private RegistryAnalyzerDictionary _registryAnalyzers = null;
        private RegistrySizeDictionary _registrySizes = new RegistrySizeDictionary();
        private RegistrySnapshotDictionary _registrySnapshots = new RegistrySnapshotDictionary();

        private int _completedCount = 0;

        internal delegate void InstallStatusHandler(object sender, StatusEventArgs e);
        internal event InstallStatusHandler OnStatusUpdate;

        /// <summary>
        /// Occurs when the an installation is started.
        /// </summary>
        public event EventHandler<EventArgs> OnInstallationStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainEditControl"/> class.
        /// </summary>
        public MainEditControl()
        {
            InitializeComponent();

            // Too lazy to wrap these events into the bundle class, so the Installer is exposed to access them
            _manager.Installer.OnStatusUpdate += new QueueInstaller.InstallStatusHandler(_installer_OnStatusUpdate);
            _manager.Installer.OnError += new QueueInstaller.ErrorEventHandler(_installer_OnError);

            _manager.Installer.OnComplete += new EventHandler(Installer_OnComplete);

            // Define all the locations where registry changes are being made.
            _registryAnalyzers = new RegistryAnalyzerDictionary();

            // Open the registry file and load all of the entries, these will be monitored, and this allows
            // a way for adding more paths to monitor if needed.
            XmlDocument registryXml = new XmlDocument();
            registryXml.Load("RegistryPaths.xml");

            foreach (XmlNode node in registryXml.SelectNodes("//Path"))
            {
                var hive = EnumUtil.Parse<RegistryHive>(node.Attributes["Hive"].Value);
                var analyzer = new RegistryAnalyzer(hive, node.InnerText);
                _registryAnalyzers.Add(analyzer.Key, analyzer);
            }
        }

        private void Installer_OnComplete(object sender, EventArgs e)
        {
            EnableButtons(sender, e);
        }

        private void _installer_OnError(object sender, ErrorEventArgs e)
        {
            MessageBox.Show("Installation Error. Contact the STF team. " + e.Exception.Message, "Fatal Install Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void _installer_OnStatusUpdate(object sender, StatusEventArgs e)
        {
            int count = 0;
            if (int.TryParse(e.Message, out count))
            {
                UpdateCount(count);
            }
            else
            {
                if (OnStatusUpdate != null)
                {
                    OnStatusUpdate(sender, e);
                }
            }

            RefreshGrid(sender, e);
        }

        private void EnableButtons(object sender, EventArgs e)
        {
            if (start_button.InvokeRequired)
            {
                start_button.Invoke(new EventHandler(EnableButtons), null);
            }
            else
            {
                EnableButtons(true);
            }
        }

        private void RefreshGrid(object sender, EventArgs e)
        {
            if (queues_DataGridView.InvokeRequired)
            {
                queues_DataGridView.Invoke(new EventHandler(RefreshGrid), null);
            }
            else
            {
                queues_DataGridView.Refresh();
            }
        }

        private void UpdateCount(int count)
        {
            _completedCount += count;
            UpdateCountLabel(_completedCount, _queueDefinitions.Count);
        }

        private void UpdateCountLabel(int completed, int total)
        {
            if (queueCount_Label.InvokeRequired)
            {
                queueCount_Label.Invoke(new MethodInvoker(() => UpdateCountLabel(completed, total)));
            }
            else
            {
                queueCount_Label.Text = "{0,4} / {1,-4}".FormatWith(completed, total);
            }
        }

        /// <summary>
        /// Gets or sets the thread count.
        /// </summary>
        /// <value>
        /// The thread count.
        /// </value>
        public int ThreadCount
        {
            get { return _threadCount; }
            set { _threadCount = value; }
        }

        /// <summary>
        /// Opens the data saved at a specified path
        /// </summary>
        /// <param name="path">The path.</param>
        public void Open(string path)
        {
            Cursor = Cursors.WaitCursor;
            _queueDefinitions.Clear();
            foreach (QueueInstallationData data in _manager.Open(path))
            {
                data.Progress = string.Empty;
                _queueDefinitions.Add(data);
            }
            Cursor = Cursors.Default;

            queueCount_Label.Text = "0 / {0,-5}".FormatWith(_queueDefinitions.Count);
        }

        /// <summary>
        /// Saves the data to a specfied path
        /// </summary>
        /// <param name="path">The path.</param>
        public void Save(string path)
        {
            Cursor = Cursors.WaitCursor;
            _manager.Save(path, _queueDefinitions);
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            _queueDefinitions.Clear();
            EnableButtons(true);
            _threadCount = 1;
        }

        /// <summary>
        /// Downloads the driver package.
        /// </summary>
        public void DownloadDriverPackage()
        {
            using (PrintDriverLocalCopyForm form = new PrintDriverLocalCopyForm())
            {
                form.ShowDialog();

                if (form.DriverPaths.Count > 0)
                {
                    _manager.DriverPackagePaths.AddRange(form.DriverPaths);
                }
            }
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                return;
            }

            EnableButtons(false);

            queueCount_Label.Text = "0 / {0,-5}".FormatWith(_queueDefinitions.Count);

            // If any of the definitions have CFM default parameter files assigned, then
            // running multiple threads to install could be a problem.  Look through the
            // definitions, then see if the thread count is up, and if so, set it down
            if (_threadCount > 1)
            {
                foreach (QueueInstallationData data in _queueDefinitions)
                {
                    if (data.UseConfigurationFile)
                    {
                        DialogResult result = MessageBox.Show
                            (
                                "You can't combine the use of configuration files with a threaded install. Do you want to continue by installing sequentially with a single thread?",
                                "Threaded Installation",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning
                            );

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            _threadCount = 1;
                        }
                    }
                }
            }


            // First check to see if the current number of queues on the system plus
            // the number of new queues to be created will exceed 100.  If so then
            // let the user know that a registry snapshot will not be taken, but only
            // a registry size change.
            int currentQueueCount = RegistryAnalyzer.RegistryPrinterCount;

            // Exclude the row header in the row count
            int newQueueCount = queues_DataGridView.Rows.Count;

            bool includeRegistryChanges = true;
            if (currentQueueCount + newQueueCount > 100)
            {
                includeRegistryChanges = false;
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(Resource.QueueCountMessage.FormatWith(currentQueueCount, newQueueCount));
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine("Do you want to continue?");

                DialogResult result = MessageBox.Show
                    (
                        builder.ToString(),
                        "Registry Entry Logging",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );

                if (result == DialogResult.No)
                {
                    EnableButtons(true);
                    return;
                }
            }

            if (OnInstallationStarted != null)
            {
                OnInstallationStarted(this, new EventArgs());
            }

            // Capture the initial registry size and also the initial snapshot detail if the 
            // number of queues doesn't exceed the max.
            OnStatusUpdate(this, new StatusEventArgs("Taking a snapshot of the Registry...", StatusEventType.StatusChange));
            _registrySizes.Clear();
            _registrySnapshots.Clear();

            foreach (RegistryAnalyzer analyzer in _registryAnalyzers.Values)
            {
                string key = analyzer.Key;

                _registrySizes.Add(key, new int[2]);

                _registrySizes[key][0] = analyzer.GetRegistrySize(
                    GlobalSettings.Items[Setting.DomainAdminUserName],
                    GlobalSettings.Items[Setting.DomainAdminPassword]);

                if (includeRegistryChanges)
                {
                    _registrySnapshots.Add(key, new RegistrySnapshot[2]);
                    _registrySnapshots[key][0] = analyzer.TakeSnapshot();
                    _registrySnapshots[key][1] = null;
                }
            }

            _manager.InstallQueues(_threadCount, _queueDefinitions, _installationTimeout);
            queues_DataGridView.Refresh();
        }


        private void EnableButtons(bool state)
        {
            start_button.Enabled = state;
            addQueue_Button.Enabled = state;
            clearSelected_Button.Enabled = state;
            abort_Button.Enabled = !state;
        }

        private void MainEditControl_Load(object sender, EventArgs e)
        {
            queueCount_Label.Text = "0 / 0";
            queues_DataGridView.DataSource = _queueDefinitions;
            LoadSettings();

            abort_Button.Enabled = false;
        }

        private void abort_Button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will abort queue creation.  Any queues currently being created may be in an unstable state. Do you want to continue?", "Abort Creation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                _manager.AbortInstallation();
                EnableButtons(true);

                // Clean up any rows that didn't complete.
                foreach (QueueInstallationData data in _queueDefinitions)
                {
                    if (data.Progress == "Working...")
                    {
                        data.Progress = "Aborted";
                    }
                }

                queues_DataGridView.Refresh();

                if (OnStatusUpdate != null)
                {
                    OnStatusUpdate(this, new StatusEventArgs("Queue creation aborted.", StatusEventType.StatusChange));
                }
            }
        }

        /// <summary>
        /// Views the installation log.
        /// </summary>
        public void ViewInstallationLog()
        {
            using (StatusDisplayForm textbox = new StatusDisplayForm())
            {
                textbox.Text = "Installation Status Log";

                textbox.Data = _manager.Installer.LogText;
                textbox.ShowDialog();
            }
        }

        /// <summary>
        /// Loads the settings for the application
        /// </summary>
        public void LoadSettings()
        {
            var registryPath = UserAppDataRegistry.GetValue("PQI Driver Paths");
            if (registryPath != null)
            {
                Collection<string> items = new Collection<string>(registryPath.ToString().Split(new char[] { ',' }));

                // Only load those driver paths that are still there
                foreach (string item in items)
                {
                    if (Directory.Exists(item))
                    {
                        _manager.DriverPackagePaths.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the settings for the application
        /// </summary>
        public void SaveSettings()
        {
            try
            {
                List<string> paths = new List<string>();
                foreach (string path in _manager.DriverPackagePaths.Items)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        paths.Add(path);
                    }
                }

                UserAppDataRegistry.SetValue("PQI Driver Paths", string.Join(",", paths.ToArray()));
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (SecurityException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exit_Button_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit the program?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void clearSelected_Button_Click(object sender, EventArgs e)
        {
            if (queues_DataGridView.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Clear selected definitions?", "Clear Definitions", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    foreach (DataGridViewRow row in queues_DataGridView.SelectedRows)
                    {
                        _queueDefinitions.Remove((QueueInstallationData)row.DataBoundItem);
                    }
                    Cursor.Current = Cursors.Default;
                }
                queues_DataGridView.Refresh();
            }
            else
            {
                MessageBox.Show("Please select a print queue definition to delete.", "Clear Definitions", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void addQueue_Button_Click(object sender, EventArgs e)
        {
            if (_queueDefinitions.Count == 0)
            {
                _manager.Reset();
            }
            _manager.ClearDefinitions();

            using (QueueDefinitionForm form = new QueueDefinitionForm(_manager))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _installationTimeout = form.InstallationTimeout;

                    foreach (QueueInstallationData item in _manager.QueueDefinitions.ToList())
                    {
                        _queueDefinitions.Add(item);
                    }
                }
            }

            UpdateCountLabel(0, _queueDefinitions.Count);
        }

        private void queues_DataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateCountLabel(0, _queueDefinitions.Count);
        }

        /// <summary>
        /// Manages the driver paths.
        /// </summary>
        public void ManageDriverPaths()
        {
            using (ManageDriverPathsForm form = new ManageDriverPathsForm(_manager))
            {
                form.ShowDialog();
            }
        }

        /// <summary>
        /// Upgrades the driver.
        /// </summary>
        public void UpgradeDriver()
        {
            // If this is an XP based machine, the driver name is not stored in the registry
            // so preload the driver data
            if (Environment.OSVersion.Version.Major < 6)
            {
                if (!DriverStoreScanner.Instance.ScanningComplete)
                {
                    using (DriverStoreScanningForm form = new DriverStoreScanningForm())
                    {
                        DialogResult result = form.ShowDialog();

                        if (result != System.Windows.Forms.DialogResult.OK)
                        {
                            return;
                        }
                    }
                }
            }

            if (_upgradeForm == null)
            {
                _upgradeForm = new DriverUpgradeForm(_manager);
            }

            _upgradeForm.LoadDrivers();
            _upgradeForm.ShowDialog();
        }

        /// <summary>
        /// Installs the driver.
        /// </summary>
        public void InstallDriver()
        {
            using (DriverInstallForm form = new DriverInstallForm(_manager))
            {
                if (form.ShowDialog() == DialogResult.OK && _manager.CurrentDriver != null)
                {
                    try
                    {
                        OnStatusUpdate(this, new StatusEventArgs("Installing driver... {0}".FormatWith(_manager.CurrentDriver.Name), StatusEventType.StatusChange));
                        Cursor = Cursors.WaitCursor;
                        if (!_manager.InstallDriver())
                        {
                            Cursor = Cursors.Default;
                            OnStatusUpdate(this, new StatusEventArgs("Driver installation failed...", StatusEventType.StatusChange));
                            MessageBox.Show("Unable to install the print driver", "Installation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            OnStatusUpdate(this, new StatusEventArgs("Installation complete", StatusEventType.StatusChange));
                        }
                    }
                    catch (Win32Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Installation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }
        }

        /// <summary>
        /// Views the registry changes.
        /// </summary>
        public void ViewRegistryChanges()
        {
            var form = new RegistryInformationForm(_registryAnalyzers, _registrySnapshots, _registrySizes);
            using (form)
            {
                form.ShowDialog();
            }
        }

        private void queues_DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
