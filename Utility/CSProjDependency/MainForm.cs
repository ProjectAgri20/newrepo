using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace HP.ScalableTest.Utility.VisualStudio
{
    public partial class MainForm : Form
    {
        private DependencyGraphBuilder _graph;
        private Bitmap _currentImage;
        private Cursor _savedCursor = Cursors.Default;

        public MainForm()
        {
            InitializeComponent();
        }

        private void LoadGraphButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("You must provide a path", "CS Project Path Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error, 
                    MessageBoxDefaultButton.Button1, 
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return;
            }

            listBox1.Items.Clear();
            root_ComboBox.Items.Clear();
            scaleNumericUpDown.Value = 100;
            _graph.Load(textBox1.Text);
            foreach (string assembly in _graph.AssemblyList)
            {
                root_ComboBox.Items.Add(assembly);
            }
            mainStatusLabel.Text = string.Empty;    
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DirectoryInfo directory = new DirectoryInfo(Environment.CurrentDirectory);
            while (directory.Name != "EnterpriseTestLab")
            {
                directory = directory.Parent;
            }
            textBox1.Text = directory.FullName;


            _graph = new DependencyGraphBuilder();
            _graph.Cleanup();

            _graph.OnProcessingProject += new EventHandler<StringEventArgs>(Graph_OnProcessingProject);
            _graph.OnGraphCreated += new EventHandler<StringEventArgs>(Graph_OnGraphCreated);
            _graph.OnUpdateStatus += new EventHandler<StringEventArgs>(Graph_OnUpdateStatus);
            _graph.OnUpdateProcessing += new EventHandler<BoolEventArgs>(Graph_OnUpdateProcessing);
        }

        void Graph_OnUpdateProcessing(object sender, BoolEventArgs args)
        {
            if (args.Data)
            {
                _savedCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = _savedCursor;
            }
        }

        void Graph_OnUpdateStatus(object sender, StringEventArgs args)
        {
            mainStatusLabel.Text = args.Data;
            mainStatusLabel.GetCurrentParent().Refresh();
        }

        void Graph_OnGraphCreated(object sender, StringEventArgs args)
        {
            _currentImage = new Bitmap(args.Data);
            extendedPictureBox.PictureBitmap = _currentImage;
        }

        void Graph_OnProcessingProject(object sender, StringEventArgs args)
        {
            listBox1.Items.Add(args.Data);
            listBox1.TopIndex = listBox1.Items.Count - 1;
            listBox1.Refresh();
        }

        private void BrowseDirectoryButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "CS Project Root Directory";

                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    dialog.SelectedPath = textBox1.Text;
                }
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    textBox1.Text = dialog.SelectedPath;
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _graph.Dispose();
        }

        private void ApplyScaleButton_Click(object sender, EventArgs e)
        {
            int width = _currentImage.Width;
            int height = _currentImage.Height;

            decimal factor = scaleNumericUpDown.Value / 100;
            width = (int)Math.Ceiling(width * factor);
            height = (int)Math.Ceiling(height * factor);
            extendedPictureBox.PictureBitmap = ResizeImage(_currentImage, width, height);
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            dialog.Title = "Save Dependency Graph as Image File";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(dialog.FileName))
                {
                    // Saves the Image via a FileStream created by the OpenFile method.
                    FileStream fileStream = (FileStream)dialog.OpenFile();

                    // Saves the Image in the appropriate ImageFormat based upon the
                    // File type selected in the dialog box.
                    // NOTE that the FilterIndex property is one-based.
                    switch (dialog.FilterIndex)
                    {
                        case 1:
                            _currentImage.Save(fileStream, ImageFormat.Jpeg);
                            break;

                        case 2:
                            _currentImage.Save(fileStream, ImageFormat.Bmp);
                            break;

                        case 3:
                            _currentImage.Save(fileStream, ImageFormat.Gif);
                            break;
                    }

                    fileStream.Close();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            IEnumerable<string> projectList = _graph.ShownProjectList;
            if (_graph.ShownProjectList.Count == 0)
            {
                projectList = _graph.ProjectList;
            }

            StringBuilder builder = new StringBuilder();
            foreach (string project in projectList)
            {
                builder.Append(project);
                builder.Append(Environment.NewLine);                
            }
            Clipboard.SetText(builder.ToString());
        }

        private void filterOptions_Changed(object sender, EventArgs e)
        {
            _graph.Filter(root_ComboBox.SelectedItem.ToString(), recursive_CheckBox.Checked);
        }
    }
}
