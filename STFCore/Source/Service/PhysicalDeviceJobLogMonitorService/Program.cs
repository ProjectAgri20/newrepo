using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    static class Program
    {
        static void Main(string[] args)
        {
            RunAsService(args);
        }

        private static void RunAsService(string[] args)
        {
            using (var service = new PhysicalDeviceJobLogMonitorWindowsService())
            {
                service.Run(args);
            }
        }
    }
}
