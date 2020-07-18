using System;
using System.Collections.Generic;
using System.IO;

namespace HP.ScalableTest.Virtualization
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.WriteLine("Usage: VMCopyTool [TargetPrefix] [StartNumber] [EndNumber] [Source] [Destination(No Host info)]");
                Console.WriteLine(@"example: VMCopyTool CLI 16 115 File.exe C$\temp\file.exe");
                return;
            }

            string prefix = args[0];
            int start = int.Parse(args[1]);
            int end = int.Parse(args[2]);            
            string source = args[3];
            string destination = args[4];

            // Build a list of sources to copy to.
            List<string> hosts = new List<string>();
            for (int i = start; i <= end; i++)
            {
                hosts.Add(string.Format("{0}{1:00}", prefix, i));
            }

            while (hosts.Count != 0)
            {
                Console.WriteLine("Press enter to begin copying...");
                Console.ReadLine();

                List<string> failedHosts = new List<string>();

                foreach (string host in hosts)
                {
                    try
                    {
                        Console.WriteLine(string.Format(@"{0} -> \\{1}\{2}", source, host, destination));
                        File.Copy(source, string.Format(@"\\{0}\{1}", host, destination), true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to copy to {0}: {1}", host, ex.Message);
                        failedHosts.Add(host);
                    }
                }

                // Give a quick summary of the failed hosts.
                Console.WriteLine("\n\nThe following hosts failed to copy.");
                foreach (string host in failedHosts)
                {
                    Console.WriteLine(host);
                }

                hosts = failedHosts;
            }
        }
    }
}
