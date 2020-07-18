using System;
using System.Linq;
using HP.ScalableTest;

namespace HP.SolutionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TraceFactory.Logger.Debug("Starting service");

            using (SolutionTestWindowsService service = new SolutionTestWindowsService())
            {
                service.Run(args);
            }
        }

        private static void Start(string[] args)
        {
            using (SolutionTestWindowsService service = new SolutionTestWindowsService())
            {
                service.Run(args);
            }
        }
    }
}
