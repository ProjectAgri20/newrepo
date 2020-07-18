using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Utility.VisualStudio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Form mainForm = new MainForm())
            {
                Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Application.Run(mainForm);
            }
        }
    }
}
