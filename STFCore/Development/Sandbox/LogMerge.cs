using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Development
{
    class LogMerge
    {
        static void LogMergeMain(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: LogMerge <directory> <pattern>");
                Console.WriteLine("       directory - path to directory containing log files");
                Console.WriteLine("       pattern - file pattern of log files");
                return;
            }

            string path = args[0];
            string pattern = args[1];
            Console.WriteLine("Merging files in " + path + " with pattern: " + pattern);

            Regex regEx = new Regex("([0-9]{4})");
            Dictionary<string, List<string>> logEntries = new Dictionary<string, List<string>>();
            string[] files = Directory.GetFiles(path, pattern);
            foreach (string file in files)
            {
                string id = Path.GetFileNameWithoutExtension(file).ToUpper();
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    if (line.Length > 23)
                    {
                        string key = line.Substring(0, 24) + "[" + id + "]";
                        if (!logEntries.ContainsKey(key))
                        {
                            List<string> list = new List<string>();
                            list.Add(line.Substring(24));
                            logEntries.Add(key, list);
                        }
                        else
                        {
                            logEntries[key].Add(line.Substring(24));
                        }
                    }
                }
            }

            List<string> keys = GetKeys(logEntries);
            keys.Sort();

            System.IO.
            TextWriter writer = new StreamWriter(Path.Combine(path, "MergedLog.txt"));
            foreach (string key in keys)
            {
                foreach (string value in logEntries[key])
                {
                    writer.WriteLine(key + " " + value);
                }
            }
            writer.Close();
        }

        public static List<T> GetKeys<T, U>(Dictionary<T, U> table)
        {
            return (new List<T>(table.Keys));
        }
    }
}
