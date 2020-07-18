using System;
using System.Windows.Forms;

namespace HP.RDL.STF.DeviceSettings
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
            Application.Run(new FrmDeviceSettings());
        }
    }
}
