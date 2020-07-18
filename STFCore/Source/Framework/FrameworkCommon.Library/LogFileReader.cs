using System;
using System.Reflection;
using System.IO;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Provides functionality for reading log files.
    /// </summary>
    public class LogFileReader
    {
        public static string Read(string fileName)
        {
            string data = string.Empty;

            using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        data = reader.ReadToEnd();
                    }
                    catch (OutOfMemoryException ex)
                    {
                        TraceFactory.Logger.Error("Memory error reading {0}: {1}".FormatWith(fileName, ex.Message));
                    }
                    catch (IOException ex)
                    {
                        TraceFactory.Logger.Error("I/O error reading {0}: {1}".FormatWith(fileName, ex.Message));
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// The well-known filepath of the data log files.
        /// </summary>
        /// <returns>The filepath of the data log directory</returns>
        public static string DataLogPath()
        {
            string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(location, "Logs");

        }

        /// <summary>
        /// The well-known path of the data log files with the specified name appended to the file path.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The filepath of the data log directory</returns>
        public static string DataLogPath(string name)
        {
            string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(location, "Logs", name);
        }

    }
}
