using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace CreateInstaller
{
    /// <summary>
    /// Class to execute the installer-builder process via command line.
    /// </summary>
    public class CommandLineBuilder
    {
        private string _workingPath = string.Empty;
        private IProcessOutput _process = null;
        private StringBuilder _log = new StringBuilder();

        internal InstallerStatus Status { get; private set; }

        /// <summary>
        /// Instantiates an instance of the CommandLineBuilder class.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="workingPath"></param>
        public CommandLineBuilder(IProcessOutput process, string workingPath)
        {
            _process = process;
            _workingPath = workingPath;
        }

        /// <summary>
        /// Executes the process.
        /// </summary>
        public void Execute()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
            Status = InstallerStatus.Processing;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _process.OnMessageUpdate += Installer_OnMessageUpdate;
            _process.Execute();
        }

        private void Installer_OnMessageUpdate(object sender, InstallEventArgs e)
        {
            _log.AppendLine(e.Message);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WriteToFile();
            Status = InstallerStatus.Completed;
        }

        /// <summary>
        /// Writes this processes output to a file.
        /// </summary>
        private void WriteToFile()
        {
            try
            {
                Installer.WriteLogToFile(_workingPath, _process.Configuration, _log.ToString());
            }
            catch (Exception ex)
            {
                Installer.WriteLogToFile(_workingPath, _process.Label, ex.ToString());
            }
        }

    }
}
