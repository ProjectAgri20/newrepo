using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeWorkerBootStrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Environment.Exit(-1);
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = args[0],
                Arguments = $"{args[1]} {args[2]}",
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = Path.GetDirectoryName(args[0])
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
            }
        }
    }
}
