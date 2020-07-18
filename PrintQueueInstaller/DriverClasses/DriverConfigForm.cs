using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace HP.ScalableTest.Print.Utility
{
    public partial class DriverConfigForm : Form
    {
        private string _exePath = string.Empty;
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverConfigForm"/> class.
        /// </summary>
        public DriverConfigForm()
        {
            InitializeComponent();

            // Copy the HPBCFGAP files out into the appdata area
            _exePath = CopyDriverConfigurationUtility();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(_exePath);
        }

        private void DriverConfigForm_Load(object sender, EventArgs e)
        {
            help_TextBox.TabStop = false;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            Process.Start(_exePath);
            Close();
        }

        private static string CopyDriverConfigurationUtility()
        {
            TraceFactory.Logger.Debug("Entering...");
            string exeDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HPBCFGAP");
            if (!Directory.Exists(exeDir))
            {
                Directory.CreateDirectory(exeDir);
            }

            string path = Path.Combine(exeDir, "cfgapp.ico");
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                TraceFactory.Logger.Debug("Copying {0}".FormatWith(path));
                Resource.cfgapp.Save(stream);
            }
            path = Path.Combine(exeDir, "hpbcfgre.dll");
            File.WriteAllBytes(path, Resource.hpbcfgre);
            File.WriteAllBytes(Path.Combine(exeDir, "hpbcfgui.dll"), Resource.hpbcfgui);
            File.WriteAllBytes(Path.Combine(exeDir, "HPCDMC32.dll"), Resource.HPCDMC32);

            string exePath = Path.Combine(exeDir, "hpbcfgap.exe");
            File.WriteAllBytes(exePath, Resource.hpbcfgap);

            return exePath;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
