using System;
using System.IO;

namespace ConvertBase64String
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Usage();
                return;
            }

            if (!args[0].Equals("to", StringComparison.OrdinalIgnoreCase) && !args[0].Equals("from", StringComparison.OrdinalIgnoreCase))
            {
                Usage();
                return;
            }

            string input = args[1];
            string output = args[2];

            if (args[0].Equals("to", StringComparison.OrdinalIgnoreCase))
            {
                File.WriteAllText(output, Convert.ToBase64String(File.ReadAllBytes(input)));
            }
            else
            {
                File.WriteAllBytes(output, Convert.FromBase64String(File.ReadAllText(input)));
            }
        }

        private static void Usage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("    ConvertBase64String [to|from] <inputfile> <outputFile>");
            Console.WriteLine();
        }
    }
}
