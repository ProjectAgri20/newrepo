using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace HP.SolutionTest.Install
{
    internal partial class SchemaInstallerForm : Form
    {
        private MainInstaller _installer = null;
        private bool _inErrorState = false;

        public SchemaInstallerForm(SchemaTicket ticket)
        {
            InitializeComponent();

            EnterpriseTestHostname = string.Empty;
            _installer = new MainInstaller(ticket);

            _installer.OnStatusUpdate += _schemaInstaller_OnStatusUpdate;
            _installer.OnProgressUpdate += _schemaInstaller_OnProgressUpdate;
            _installer.OnInstallationComplete += _schemaInstaller_OnInstallationComplete;
            _installer.OnCancellation += _schemaInstaller_OnCancellation;
            _installer.OnError += _schemaInstaller_OnError;
            
        }

        public string EnterpriseTestHostname { get; set; }

        private void CloseForm()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => Close()));
            }
            else
            {
                Close();
            }
        }

        private void _schemaInstaller_OnError(object sender, StatusEventArgs e)
        {
            UpdateStatus(e.Message);
            Environment.ExitCode = 1;
            _inErrorState = true;
            //CloseForm();
        }

        private void _schemaInstaller_OnCancellation(object sender, EventArgs e)
        {
            Environment.ExitCode = -1;
            CloseForm();
        }

        private void _schemaInstaller_OnInstallationComplete(object sender, EventArgs e)
        {
            Environment.ExitCode = 0;
            CloseForm();
        }

        private void _schemaInstaller_OnProgressUpdate(object sender, ProgressEventArgs e)
        {
            UpdateProgress(e);
        }

        private void _schemaInstaller_OnStatusUpdate(object sender, StatusEventArgs e)
        {
            UpdateStatus(e.Message);
        }

        private void SchemaInstallerForm_Load(object sender, EventArgs e)
        {
            ClearProgress();
            _installer.Run(EnterpriseTestHostname);
        }

        public void ClearProgress()
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new MethodInvoker(() => ClearProgress()));
            }
            else
            {
                progressBar.Value = 0;
                progressBar.Visible = false;
            }
        }

        public void UpdateStatus(string message)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new MethodInvoker(() => UpdateStatus(message)));
            }
            else
            {
                statusLabel.Text = message;
                statusLabel.Refresh();
            }
        }

        public void UpdateProgress(ProgressEventArgs args)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new MethodInvoker(() => UpdateProgress(args)));
            }
            else
            {
                switch (args.State)
                {
                    case ProgressState.Start:
                        progressBar.Visible = true;
                        progressBar.Maximum = args.Total;
                        progressBar.Value = args.Current;
                        break;
                    case ProgressState.Running:
                        if (args.Current <= progressBar.Maximum)
                        {
                            progressBar.Value = args.Current;
                        }
                        break;
                    case ProgressState.End:
                        progressBar.Visible = false;
                        break;
                }
            }
        }

        public ProgressBar ProgressIndicator
        {
            get
            {
                return progressBar;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _installer.Cancel();
            if (_inErrorState)
            {
                Close();
            }
        }

        private void linkLabel_LogFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //Wrapping in a using just to ensure everything gets cleaned up properly.
                using (Process p = Process.Start(Path.GetTempPath())) { };
            }
            catch (Exception ex)
            {
                SystemTrace.Instance.Error("Error Opening Temp folder location.", ex);
            }
        }
    }
}
