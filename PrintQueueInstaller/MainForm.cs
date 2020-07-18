using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI;

namespace HP.ScalableTest.Print.Utility
{
    public partial class MainForm : Form
    {
        private string _fileName = string.Empty;
        private MainEditControl mainEditControl = new MainEditControl();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            registryChangesToolStripMenuItem.Enabled = false;
            installationLogDataToolStripMenuItem.Enabled = false;

            mainEditControl.OnStatusUpdate += new MainEditControl.InstallStatusHandler(mainEditControl_OnStatusUpdate);
            mainEditControl.OnInstallationStarted += new EventHandler<EventArgs>(mainEditControl_OnInstallationStarted);
        }

        private void mainEditControl_OnInstallationStarted(object sender, EventArgs e)
        {
            registryChangesToolStripMenuItem.Enabled = true;
            installationLogDataToolStripMenuItem.Enabled = true;
        }

        private void mainEditControl_OnStatusUpdate(object sender, StatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MainEditControl.InstallStatusHandler(mainEditControl_OnStatusUpdate), sender, e);
                this.Invoke(new EventHandler(UpdateForm), null);
            }

            toolStripStatusLabel.Text = e.Message;
        }

        private void UpdateForm(object sender, EventArgs e)
        {
            Update();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox box = new AboutBox())
            {
                box.ShowDialog();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                SaveAs();
            }
            else
            {
                Cursor = Cursors.WaitCursor;
                mainEditControl.Save(_fileName);
                Cursor = Cursors.Default;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.AutoUpgradeEnabled = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "xml";
                dialog.Filter = "PQI Files(*.pqi)|*.pqi";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.Title = "Save Print Queue Installer Configuration Data";
                dialog.ValidateNames = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Cursor = Cursors.WaitCursor;

                _fileName = dialog.FileName;

                try
                {
                    mainEditControl.Open(_fileName);
                }
                catch (QueueInstallException ex)
                {
                    MessageBox.Show("Failed to open. " + ex.Message, "Open Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void SaveAs()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.AutoUpgradeEnabled = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "xml";
                dialog.Filter = "PQI Files(*.pqi)|*.pqi";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.OverwritePrompt = true;
                dialog.Title = "Save Print Queue Installer Configuration Data";
                dialog.ValidateNames = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Cursor = Cursors.WaitCursor;

                _fileName = dialog.FileName;
                if (!Directory.Exists(Path.GetDirectoryName(_fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
                }

                mainEditControl.Save(_fileName);

                Cursor = Cursors.Default;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainEditControl.SaveSettings();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            mainPanel.Controls.Add(mainEditControl);
            mainEditControl.Dock = DockStyle.Fill;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainEditControl.Reset();
        }

        private void downloadDriverPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainEditControl.DownloadDriverPackage();
        }

        private void driverConfigurationUtilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueueManager.RunDriverConfigurationUtility();
        }

        private void installNewDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainEditControl.InstallDriver();
        }

        private void multiThreadedInstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ThreadedInstallForm form = new ThreadedInstallForm(mainEditControl.ThreadCount))
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mainEditControl.ThreadCount = form.ThreadCount;
                }
            }
        }

        private void manageDriverPathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainEditControl.ManageDriverPaths();
        }

        private void upgradeDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainEditControl.UpgradeDriver();
        }

        private void installationLogDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainEditControl.ViewInstallationLog();
        }

        private void registryChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainEditControl.ViewRegistryChanges();
        }

        private void applicationLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = Path.Combine("Logs", "PrintQueueInstaller-{0}.log".FormatWith(Process.GetCurrentProcess().Id));
            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var textReader = new StreamReader(fileStream);
                    using (TextDisplayDialog textBox = new TextDisplayDialog(textReader.ReadToEnd()))
                    {
                        textBox.ShowDialog();
                    }
                }
            }
            else
            {
                MessageBox.Show
                    (
                        "Log file not found",
                        "View Application Log File",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
            }
        }

        private void printQueuePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PortUpdateForm form = new PortUpdateForm())
            {
                form.ShowDialog(this);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var database = LoadDatabaseName();

            using (SystemSelectionDialog dialog = new SystemSelectionDialog(database))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.Database != database)
                    {
                        SaveDatabaseName(dialog.Database);
                    }

                    GlobalSettings.Load(dialog.Database);
                    FrameworkServicesInitializer.InitializeConfiguration();
                    toolStripStatusLabel.Text = "Connected to STF database: {0}".FormatWith(dialog.Database);
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        /// <summary>
        /// Loads the settings for the application
        /// </summary>
        private static string LoadDatabaseName()
        {
            string database = string.Empty;

            object registryPath = UserAppDataRegistry.GetValue("PQI Database");
            if (registryPath != null)
            {
                database = registryPath.ToString();
            }

            return database;
        }

        /// <summary>
        /// Saves the settings for the application
        /// </summary>
        private static void SaveDatabaseName(string database)
        {
            try
            {
                UserAppDataRegistry.SetValue("PQI Database", database);
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
    }
}