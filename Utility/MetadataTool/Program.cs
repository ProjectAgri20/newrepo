using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Utility.BtfMetadataHelper
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
            Application.Run(new MetadataForm());
        }
    }
}
