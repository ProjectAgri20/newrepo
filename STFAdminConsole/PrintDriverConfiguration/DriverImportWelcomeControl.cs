using HP.ScalableTest.Print;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Print.Drivers;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// The Welcome control for the importing of drivers into the STF.
    /// Provides a way for the user to select the source location of the drivers they want to import.
    /// </summary>
    public partial class DriverImportWelcomeControl : UserControl
    {
        private ErrorProvider _errorProvider = new ErrorProvider();
        private int _singleRowHeight;
        
        public DriverImportWelcomeControl()
        {
            InitializeComponent();
            _singleRowHeight = location_ListBox.Height;

            InitializeErrorProvider();
            AdjustListBoxSize();
        }

        /// <summary>
        /// Gets or sets whether or not to allow multiple source selection options.
        /// If multiple are not allowed, only a driver package file is allowed as the source.
        /// (Self-extracting .exe or .zip file)
        /// </summary>
        public bool AllowSelectionOptions
        {
            get { return folder_RadioButton.Visible && package_RadioButton.Visible && central_RadioButton.Visible; }
            set 
            {
                folder_RadioButton.Visible = value;
                package_RadioButton.Visible = value;
                central_RadioButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets the location setting for the print driver source.
        /// </summary>
        public PrintDriverLocation PrintDriverLocation
        {
            get
            {
                if (folder_RadioButton.Checked)
                {
                    return PrintDriverLocation.InstalledDriverFolder;
                }
                else if (package_RadioButton.Checked)
                {
                    return PrintDriverLocation.DriverPackageFile;
                }

                return PrintDriverLocation.CentralRepository;
            }
        }

        /// <summary>
        /// Gets the Driver Paths for the selected node (recursive).
        /// </summary>
        public Collection<string> PrintDriverPaths
        {
            get
            {
                Collection<string> result = new Collection<string>();
                foreach (string path in location_ListBox.Items)
                {
                    result.Add(path);
                }
                return result;
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                locationButton.PerformClick();
            }
        }

        private void InitializeErrorProvider()
        {
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.ContainerControl = this;
            _errorProvider.SetIconAlignment(location_ListBox, ErrorIconAlignment.TopLeft);
        }

        private void location_ListBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (location_ListBox.Items.Count == 0)
            {
                _errorProvider.SetError(location_ListBox, "Click 'Browse' to select driver location.");
                e.Cancel = true;
                return;
            }

            _errorProvider.SetError(location_ListBox, string.Empty);
            e.Cancel = false;
        }

        private void AdjustListBoxSize()
        {
            if (location_ListBox.Items.Count > 1)
            {
                int newHeight = this.Height - (location_ListBox.Location.Y + 6);
                location_ListBox.Size = new System.Drawing.Size(location_ListBox.Width, newHeight);
            }
            else if (location_ListBox.Height != _singleRowHeight)
            {
                location_ListBox.Size = new System.Drawing.Size(location_ListBox.Width, _singleRowHeight);
            }
        }

        /// <summary>
        /// Changes the color of the Item focus to be the same as the ListBox control.
        /// The effect is that it appears that no selection is made, which is intentional.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void location_ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                Graphics g = e.Graphics;
                Brush brush = new SolidBrush(location_ListBox.BackColor);
                g.FillRectangle(brush, e.Bounds);
                e.Graphics.DrawString(location_ListBox.Items[e.Index].ToString(), e.Font,
                         new SolidBrush(location_ListBox.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void GetInfFile(string folderPath)
        {
            //only searching the top level directory for INF files, the discrete driver has lot of INF files in various directory, which is unnecessary to go through.
            Collection<DriverInf> files = new Collection<DriverInf>();
            foreach (string fileName in Directory.GetFiles(folderPath, "*.inf", SearchOption.TopDirectoryOnly))
            {
                using (DriverInfReader reader = new DriverInfReader(fileName))
                {
                    DriverInfParser parser = new DriverInfParser(reader);
                    DriverInf inf = parser.BuildInf();
                    if (inf.DriverClass == "Printer")
                    {
                        files.Add(inf);
                    }
                }
            }

            switch (files.Count())
            {
                case 0:
                    MessageBox.Show("The selected directory does not contain any print drivers. \nPlease select the directory which has the print driver setup information file (*.inf).", "Print Driver not found",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
                case 1:
                    location_ListBox.Items.Add(files[0].Location);
                    break;
                default:
                    MessageBox.Show("The selected directory contains more than one print driver setup information file (*.inf).\nPlease choose the file you want to use.", "Multiple .INF Files Found",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    using (InfPreviewForm previewForm = new InfPreviewForm())
                    {
                        previewForm.Text = folderPath;
                        previewForm.Initialize(files);
                        if (previewForm.ShowDialog(this) == DialogResult.OK)
                        {
                            location_ListBox.Items.Add(previewForm.SelectedFile);
                        }
                    }
                    break;
            }
        }

        private void locationButton_Click(object sender, EventArgs e)
        {
            if (folder_RadioButton.Checked)
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Select the driver package directory...";
                    dialog.ShowNewFolderButton = false;
                    dialog.SelectedPath = "C:\\Windows";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        location_ListBox.Items.Clear();
                        GetInfFile(dialog.SelectedPath);
                        AdjustListBoxSize();
                    }
                }
            }
            else if (package_RadioButton.Checked)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Filter = "Print Driver Installer|*.exe|Zipped Files|*.zip|All Files (*.*)|*.*";
                    dialog.FilterIndex = 1;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        location_ListBox.Items.Clear();
                        location_ListBox.Items.Add(dialog.FileName);
                        AdjustListBoxSize();
                    }
                }
            }
            else if (central_RadioButton.Checked)
            {
                using (PrintDriverAddForm addForm = new PrintDriverAddForm())
                {
                    if (addForm.ShowDialog(this) == DialogResult.OK)
                    {
                        location_ListBox.Items.Clear();
                        foreach (string path in addForm.SelectedDestinationPaths)
                        {
                            location_ListBox.Items.Add(path);
                        }
                        AdjustListBoxSize();
                    }
                }
            }
        }



    }
}
