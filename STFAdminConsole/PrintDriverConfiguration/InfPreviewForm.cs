using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Print;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.Utility;


namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Provides an encapsulated location for previewing the contents of an .INF file.
    /// </summary>
    public partial class InfPreviewForm : Form
    {
        public InfPreviewForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the filepath of the selected file.
        /// </summary>
        /// <value>The filepath of the selected file.</value>
        public string SelectedFile
        {
            get { return ((KeyValuePair<string, string>)listBox_Files.SelectedItem).Key; }
        }

        /// <summary>
        /// Initializes the form with the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        public void Initialize(Collection<DriverInf> files)
        {
            foreach (DriverInf file in files)
            {
                KeyValuePair<string, string> item = new KeyValuePair<string, string>(file.Location, Path.GetFileName(file.Location));
                listBox_Files.Items.Add(item);
            }
        }

        private void listBox_Files_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox_DriverName.Text = ExtractDriverName(SelectedFile);
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


    }
}
