using System;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public class ExportOpenFileDialog : IDisposable
    {
        private OpenFileDialog _dialog = null;

        public ExportOpenFileDialog(string directory, string title, ImportExportType type)
        {
            _dialog = new OpenFileDialog();

            _dialog.DereferenceLinks = false;
            _dialog.AutoUpgradeEnabled = false;
            _dialog.Multiselect = false;
            _dialog.AddExtension = true;
            _dialog.CheckFileExists = false;
            _dialog.CheckPathExists = false;
            _dialog.DefaultExt = type.Extension();
            _dialog.Filter = type.Filter();
            _dialog.InitialDirectory = directory;
            _dialog.Title = title;
            _dialog.ShowHelp = false;
        }

        public OpenFileDialog Base
        {
            get { return _dialog; } 
        }

        public DialogResult ShowDialog()
        {
            return _dialog.ShowDialog();
        }

        public void Dispose()
        {
            if (_dialog != null)
            {
                _dialog.Dispose();
            }
        }
    }
}
