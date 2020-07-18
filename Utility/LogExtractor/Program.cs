using System;
using System.IO;
using System.Linq;
using HP.ScalableTest;

namespace LogExtractor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            string rootPath = args[0];

            // There may be zip files - unzip and delete them
            foreach (string zipFile in Directory.GetFiles(rootPath, "*.zip"))
            {
                string zipName = Path.GetFileNameWithoutExtension(zipFile);
                Console.WriteLine("Unzipping {0} ...".FormatWith(zipName));
                ZipUtil.Unzip(zipFile, Path.Combine(rootPath, zipName), string.Empty, true);
            }

            foreach (string machineDirectory in Directory.GetDirectories(rootPath))
            {
                string machineName = new DirectoryInfo(machineDirectory).Name;

                foreach (string logDirectory in Directory.GetDirectories(machineDirectory, "*Logs"))
                {
                    // Move all the user logs, since they should be unique
                    foreach (string file in Directory.GetFiles(logDirectory, "u???????.*"))
                    {
                        string fileName = Path.GetFileName(file);
                        Console.WriteLine(fileName);
                        File.Move(file, Path.Combine(rootPath, fileName));
                    }

                    // Any other files, prepend the machine name and move them out
                    foreach (string file in Directory.GetFiles(logDirectory))
                    {
                        string fileName = machineName + " " + Path.GetFileName(file);
                        Console.WriteLine(fileName);
                        File.Move(file, Path.Combine(rootPath, fileName));
                    }
                }

                // Delete the machine directory if it is empty
                if (!Directory.GetFiles(machineDirectory).Any(n => new FileInfo(n).Length > 0))
                {
                    Directory.Delete(machineDirectory, true);
                }
            }
        }
    }
}
