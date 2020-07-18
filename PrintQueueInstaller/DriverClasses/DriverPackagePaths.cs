using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class DriverPackagePathSet
    {
        private Collection<string> _paths = new Collection<string>();

        /// <summary>
        /// Adds the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public string Add(string path)
        {
            string newPath = TrimPath(path);

            if (!_paths.Contains(newPath) && !string.IsNullOrEmpty(newPath))
            {
                _paths.Add(newPath);
            }

            return newPath;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="paths">The paths.</param>
        public void AddRange(Collection<string> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }

            foreach (string path in paths)
            {
                if (!_paths.Contains(path) && !string.IsNullOrEmpty(path))
                {
                    _paths.Add(path);
                }
            }

        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public Collection<string> Items
        {
            get { return _paths; }
        }

        private static string TrimPath(string path)
        {
            string x86 = @"\\{0}$".FormatWith(ProcessorArchitecture.NTx86);
            string x64 = @"\\{0}$".FormatWith(ProcessorArchitecture.NTAMD64);

            string newPath = path;

            newPath = Regex.Replace(newPath, x86, "");
            newPath = Regex.Replace(newPath, x64, "");

            return newPath;
        }
    }
}
