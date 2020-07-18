
using System;
using HP.ScalableTest.Framework.Dispatcher;
namespace HP.ScalableTest.Service.Dispatcher
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (DispatcherWindowsService service = new DispatcherWindowsService())
            {
                service.Run(args);
            }
        }
    }
}
