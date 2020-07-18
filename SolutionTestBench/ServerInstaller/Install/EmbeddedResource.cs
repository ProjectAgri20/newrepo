using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using HP.ScalableTest;

namespace HP.SolutionTest.Install
{
    internal class EmbeddedResource
    {
        public EmbeddedResource(string rootPath)
        {
            Root = rootPath.TrimEnd('.');
        }

        public string Root { get; private set; }

        public string Read(string resourcePath)
        {
            string path = resourcePath;

            // Determine if the provided path starts with the root, if it
            // doesn't then prepend the root to what will be considered
            // the child or relative path.
            if (!path.StartsWith(Root))
            {
                path = "{0}.{1}".FormatWith(Root, resourcePath);
            }

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public string GetSubName(string fullName)
        {
            return fullName.Substring(Root.Length);
        }

        public void Save(string filePath, string resourceName, bool isSubName = true, bool overwrite = false)
        {
            if (!File.Exists(filePath) || (File.Exists(filePath) && overwrite))
            {
                SystemTrace.Instance.Debug("Saving: {0}".FormatWith(filePath));

                // Prepend the Root onto the resource name if it is relative, otherwise leave it alone.
                var resourcePath = isSubName ? "{0}.{1}".FormatWith(Root, resourceName) : resourceName;

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
        }

        public IEnumerable<string> FullNames
        {
            get
            {
                foreach (var name in Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(x => x.StartsWith(Root)).OrderBy(x => x))
                {
                    yield return name;
                }
            }
        }

        public IEnumerable<string> SubNames
        {
            get
            {
                foreach (var name in Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(x => x.StartsWith(Root)).OrderBy(x => x))
                {
                    yield return name.Substring(Root.Length + 1);
                }
            }
        }

        public IEnumerable<SqlUpdateScript> GetScripts()
        {
            var names = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(x => x.StartsWith(Root)).OrderBy(x => x);

            foreach (var name in names)
            {
                // The embedded resource option changes the path and puts an underscore in the version number,
                // "DatabaseUpdates.v3.16.DataLog.sql" will appear as "DatabaseUpdates.v3._16.DataLog.sql".  So
                // This code is to compensate for that and to extract out the version correctly.
                var match = Regex.Match(name, @"\.v([0-9]+)\._([0-9]+)\.*_*([0-9]*)\.(\w+)\.", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    throw new InvalidDataException("SQL Resource Update Script Name is invalid: {0}".FormatWith(Path.GetFileName(name)));
                }
                string value3 = match.Groups[3].Length == 0 ? "0" : match.Groups[3].Value;
                Version version = new Version($"{match.Groups[1].Value}.{match.Groups[2].Value}.{value3}");
                var database = match.Groups[4].Value;

                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    SqlUpdateScript resourceStream = new SqlUpdateScript()
                    {
                        Database = database,
                        Version = version,
                        SqlText = reader.ReadToEnd()
                    };

                    yield return resourceStream;
                }
            }
        }
    }
}
