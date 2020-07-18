using System;
using System.ServiceProcess;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Interfaces with the Windows Print Spooler on the local machine.
    /// </summary>
    public static class PrintSpooler
    {
        /// <summary>
        /// Restarts the Print Spooler service.
        /// </summary>
        public static void RestartSpooler()
        {
            LogDebug("Restarting print spooler.");
            using (ServiceController spooler = new ServiceController("spooler"))
            {
                if (spooler.Status != ServiceControllerStatus.Stopped)
                {
                    spooler.Stop();
                    spooler.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                }
                spooler.Start();
                spooler.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
            }
        }
    }
}
