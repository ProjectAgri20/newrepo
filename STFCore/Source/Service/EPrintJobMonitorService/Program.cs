using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Service.EPrintJobMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            using (EPrintJobMonitorWindowsService service = new EPrintJobMonitorWindowsService())
            {
                service.Run(args);
            }
        }
    }
}
