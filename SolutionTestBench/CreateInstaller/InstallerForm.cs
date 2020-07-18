using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Text;

namespace CreateInstaller
{
    internal enum InstallerStatus
    {
        Processing,
        Canceled,
        Completed
    }

    public partial class InstallerForm : Form
    {
        private string _workingPath = null;
        private IProcessOutput _process = null;

        internal InstallerStatus Status { get; private set; }

        public InstallerForm(IProcessOutput process, string workingPath)
        {
            InitializeComponent();
            _process = process;
            _workingPath = workingPath;
            Status = InstallerStatus.Processing;
            Text = $"{_process.Label} Log";
        }

        private void InstallerForm_Shown(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _process.OnMessageUpdate += Installer_OnMessageUpdate;
            _process.Execute();
        }

        private void Installer_OnMessageUpdate(object sender, InstallEventArgs e)
        {
            UpdateDisplay(e.Message);
        }

        private void UpdateDisplay(string message)
        {
            if (!logTextBox.IsDisposed)
            {
                if (logTextBox.InvokeRequired)
                {
                    logTextBox.Invoke(new MethodInvoker(() => UpdateDisplay(message)));
                    return;
                }

                if (!string.IsNullOrEmpty(message))
                {
                    logTextBox.AppendText(message);
                    logTextBox.AppendText(Environment.NewLine);
                }
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WriteToFile();
            mainButton.Text = "Close";
            mainButton.ForeColor = SystemColors.ControlText;
            Status = InstallerStatus.Completed;
        }

        private void mainButton_Click(object sender, EventArgs e)
        {
            if (_process != null && _process.Processing)
            {
                _process.OnMessageUpdate -= Installer_OnMessageUpdate;
                _process.Cancel();
                UpdateDisplay(Environment.NewLine);
                UpdateDisplay("Installer Build Cancelled!");
                Status = InstallerStatus.Canceled;
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Writes this log to a file.
        /// </summary>
        private void WriteToFile()
        {
            try
            {
                Installer.WriteLogToFile(_workingPath, _process.Configuration, logTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Write Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
