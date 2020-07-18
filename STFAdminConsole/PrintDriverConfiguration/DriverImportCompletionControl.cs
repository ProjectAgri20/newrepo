using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Print;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Print.Drivers;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// The Completion control for the importing of drivers into the STF.
    /// Provides a way for the user to select the destination of the drivers selected in the Welcome control.
    /// </summary>
    public partial class DriverImportCompletionControl : UserControl
    {
        private PrintDriverLocation _origination = PrintDriverLocation.CentralRepository;
        private Collection<string> _destinationPaths = null;
        private List<string> _drives = null;
        private int _singleRowHeight;

        private string _rootNodeName = string.Empty;
        private string _pathSuffix = string.Empty;        
        
        public DriverImportCompletionControl()
        {
            _drives = DriveInfo.GetDrives().Select(x => x.Name).ToList();

            InitializeComponent();
            _singleRowHeight = destination_ListBox.Height;

            AdjustListBoxSize();
        }

        /// <summary>
        /// Gets or sets the destination paths for the drivers being imported.
        /// </summary>
        public Collection<string> DestinationPaths
        {
            get { return _destinationPaths; }
        }

        /// <summary>
        /// Gets or sets the location setting for the print driver source.
        /// </summary>
        public PrintDriverLocation DriverOrigination
        {
            get { return _origination; }
            set { _origination = value; }
        }

        /// <summary>
        /// Initializes the control using the specified paths.
        /// </summary>
        /// <param name="paths">The paths.</param>
        public void Initialize(Collection<string> paths)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(paths);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (_origination)
            {
                case PrintDriverLocation.CentralRepository:
                    repoGroupBox.Text = "Driver Repository (readonly) - Driver will be saved off root";
                    repoLabel.Text = "Repository Root";
                    repoPathTextBox.TextChanged -= repoPathTextBox_TextChanged;
                    driverDestination_Control.SelectionChanged -= driverDestination_Control_SelectionChanged;
                    newFolderPictureBox.Visible = false;
                    driverDestination_Control.ExpandRoot();
                    repoPathTextBox.Text = _rootNodeName;
                    repoPathTextBox.ReadOnly = true;
                    destination_ListBox.Visible = true;
                    destination_Label.Visible = true;
                    destination_ListBox.DataSource = null;
                    destination_ListBox.DataSource = _destinationPaths;
                    AdjustListBoxSize();
                    break;
                case PrintDriverLocation.InstalledDriverFolder:
                    UpdateControls(ExtractDriverName(_destinationPaths.First()));
                    break;
                case PrintDriverLocation.DriverPackageFile:
                    string repoPath = _destinationPaths.First();
                    UpdateControls(repoPath);
                    _pathSuffix = Path.GetFileName(repoPath);
                    break;
            }

            if (!string.IsNullOrEmpty(driverDestination_Control.SelectedPath))
            {
                UpdateOnSelectionChange();
            }
        }

        private void UpdateControls(string repoPath)
        {
            repoGroupBox.Text = "Driver Repository - Create subfolder location to save Driver";
            repoLabel.Text = "Repository Subfolder";
            repoPathTextBox.TextChanged += repoPathTextBox_TextChanged;
            driverDestination_Control.SelectionChanged += driverDestination_Control_SelectionChanged;
            newFolderPictureBox.Visible = true;
            //driverDestination_Control.ExpandRoot();
            destination_ListBox.Visible = false;
            destination_Label.Visible = false;
            repoPathTextBox.ReadOnly = false;
            repoPathTextBox.Text = repoPath;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var driverPaths = e.Argument as Collection<string>;
            _destinationPaths = new Collection<string>();

            System.Diagnostics.Debug.WriteLine("Driver Paths:");
            foreach (string path in driverPaths)
            {
                switch (_origination)
                {
                    case PrintDriverLocation.InstalledDriverFolder:
                        _destinationPaths.Add(path);
                        break;
                    default:
                        _destinationPaths.Add(TransformFilePath(path));
                        break;
                }

            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            driverDestination_Control.RepositoryPath = PrintDriversManager.DriverRepositoryLocation;
            driverDestination_Control.LoadTreeControl();

            _rootNodeName = Path.GetFileName(GlobalSettings.Items[Setting.PrintDriverServer]);
            driverDestination_Control.SelectNode(_rootNodeName);
        }

        private void newFolder_Button_Click(object sender, EventArgs e)
        {
            try
            {
                using (InputDialog dlg = new InputDialog("Enter the name of the folder:", "New Folder", "New Folder"))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        StringBuilder path = new StringBuilder(driverDestination_Control.RepositoryPath);
                        path.Append("\\");
                        if (! string.IsNullOrEmpty(driverDestination_Control.SelectedPath))
                        {
                            path.Append(driverDestination_Control.SelectedPath);
                            path.Append("\\");
                        }
                        path.Append(dlg.Value);
                        //System.Diagnostics.Debug.WriteLine(path.ToString());
                        Directory.CreateDirectory(path.ToString());
                        driverDestination_Control.LoadTreeControl();
                        driverDestination_Control.SelectNode(dlg.Value);
                        driverDestination_Control.Select();
                    }
                }
            }
            catch
            {
            }
        }

        private string TransformFilePath(string filePath)
        {
            System.Diagnostics.Debug.WriteLine(filePath);
            var path = Path.Combine
                (
                    Path.GetDirectoryName(filePath.Trim('\\')),
                    Path.GetFileNameWithoutExtension(filePath)
                );

            var drive = _drives.FirstOrDefault(d => filePath.StartsWith(d, StringComparison.OrdinalIgnoreCase));
            if (drive != null)
            {
                return Regex.Replace(path.Replace(drive, string.Empty), @"\s", string.Empty);
            }
            else
            {
                return Regex.Replace(path, @"\s", string.Empty);
            }
        }

        private string ExtractDriverName(string infSoucePath)
        {
            using (DriverInfReader reader = new DriverInfReader(infSoucePath))
            {
                DriverInfParser parser = new DriverInfParser(reader);
                string driverName = parser.GetDiscreteDriverName();

                //if this is a fax driver, ignore it
                if (!driverName.Contains("fax", StringComparison.OrdinalIgnoreCase))
                {
                    return driverName;
                }
            }

            return string.Empty;
        }

        private void AdjustListBoxSize()
        {
            switch (destination_ListBox.Items.Count)
            {
                case 0:
                    break;
                case 1:
                    destination_ListBox.Size = new System.Drawing.Size(destination_ListBox.Width, _singleRowHeight);
                    destination_ListBox.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
                    destination_Label.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                    //driverDestination_Control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                    break;
                default:
                    int newHeight = this.Height - (destination_ListBox.Location.Y + 6);
                    destination_ListBox.Size = new System.Drawing.Size(destination_ListBox.Width, newHeight);
                    break;
            }
        }

        /// <summary>
        /// Changes the color of the Item focus to be the same as the ListBox control.
        /// The effect is that it appears that no selection is made, which is intentional.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void destination_ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                Graphics g = e.Graphics;
                Brush brush = new SolidBrush(destination_ListBox.BackColor);
                g.FillRectangle(brush, e.Bounds);
                e.Graphics.DrawString(destination_ListBox.Items[e.Index].ToString(), e.Font,
                         new SolidBrush(destination_ListBox.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void UpdateOnSelectionChange()
        {
            if (_origination != PrintDriverLocation.CentralRepository)
            {
                var path = driverDestination_Control.SelectedPath;

                if (path.StartsWith(_rootNodeName))
                {
                    path = path.Replace(@"{0}\".FormatWith(_rootNodeName), string.Empty);
                }

                if (_origination == PrintDriverLocation.DriverPackageFile)
                {
                    path = Path.Combine(path, _pathSuffix);
                }

                repoPathTextBox.Text = path;
            }
        }

        private void driverDestination_Control_SelectionChanged(object sender, SelectedNodeEventArgs e)
        {
            UpdateOnSelectionChange();
        }

        private void repoPathTextBox_TextChanged(object sender, EventArgs e)
        {
            _destinationPaths.Clear();
            _destinationPaths.Add(repoPathTextBox.Text);
        }
    }
}
