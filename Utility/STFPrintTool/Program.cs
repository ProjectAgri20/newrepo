using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HP.ScalableTest.Print;
using System.Printing;
using HP.ScalableTest;

namespace STFPrint
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArguments arguments = new CommandLineArguments(args);

            string serverName = arguments["server"];
            if (string.IsNullOrEmpty(serverName))
            {
                Usage();
                return;
            }

            string queueName = arguments["queue"];
            if (string.IsNullOrEmpty(queueName))
            {
                Usage();
                return;
            }

            string fileName = arguments["file"];
            if (string.IsNullOrEmpty(fileName))
            {
                Usage();
                return;
            }

            int copies = 1;
            if (!string.IsNullOrEmpty(arguments["copies"]))
            {
                if (!int.TryParse(arguments["copies"], out copies))
                {
                    Usage();
                    return;
                }
            }

            PrintServer server = new PrintServer(serverName);
            PrintQueue queue = new PrintQueue(server, queueName);
            PrintJob job = new PrintJob(fileName, queue);
            job.Print(copies);
        }

        static void Usage()
        {
            Console.WriteLine("Usage: stfp [-server hostname] [-queue name] [-file document] [-copies #]");
            Console.WriteLine();
            Console.WriteLine("   Suppported document types include MS Word, Excel, PowerPoint, PDF and Text");
            Console.WriteLine("   Ex: stfp -server \\\\CPRINT01 -queue \"Maui PCL 6\" -file C:\\temp\\file.docx");
            Console.WriteLine();
        }

    }
}
