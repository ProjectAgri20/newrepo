using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HP.ScalableTest;

namespace HP.SolutionTest.Install
{
    internal class DatabaseScripts
    {
        private static readonly string _resourcePath = "HP.SolutionTest.DatabaseScripts";

        public DatabaseScripts()
        {
        }

        public string GetValue(string version)
        {
            string streamId = "{0}.{1}-Script.sql".FormatWith(_resourcePath, version);
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(streamId);
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static string Read(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
