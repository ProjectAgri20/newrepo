using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Service.Monitor
{
    /// <summary>
    /// Runs STFMonitorWindowsService.
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (STFMonitorWindowsService service = new STFMonitorWindowsService())
                {
                    service.Run(args);
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Unhandled Exception.", ex);
            }
        }
    }
}
