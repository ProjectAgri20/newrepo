using System;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    /// <summary>
    /// Class for prompting the user to choose a file name and location for an exported STF type.
    /// </summary>
    public class ExportSaveFileDialog : IDisposable
    {
        private SaveFileDialog _dialog = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory">The default directory.</param>
        /// <param name="title">The title of the Save dialog.</param>
        /// <param name="type">The STF ImportExportType.</param>
        public ExportSaveFileDialog(string directory, string title, ImportExportType type = ImportExportType.Scenario)
        {
            _dialog = new SaveFileDialog()
            {
                DereferenceLinks = false,
                AutoUpgradeEnabled = false,
                AddExtension = true,
                CheckFileExists = false,
                CheckPathExists = false,
                DefaultExt = type.Extension(),
                Filter = type.Filter(),
                InitialDirectory = directory,
                Title = title,
                ShowHelp = false,
            };
        }

        /// <summary>
        /// The SaveFileDialog Instance.
        /// </summary>
        public SaveFileDialog Base
        {
            get { return _dialog; } 
        }

        /// <summary>
        /// Shows this instance as a modal dialog.
        /// </summary>
        /// <returns></returns>
        public DialogResult ShowDialog()
        {
            return _dialog.ShowDialog();
        }

        /// <summary>
        /// Disposes this instance and cleans up any resources used.
        /// </summary>
        public void Dispose()
        {
            if (_dialog != null)
            {
                _dialog.Dispose();
            }
        }
    }
}
