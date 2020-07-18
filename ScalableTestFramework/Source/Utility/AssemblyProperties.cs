using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Provides information about the application entry assembly.
    /// </summary>
    public static class AssemblyProperties
    {
        private static readonly Assembly _assembly = Assembly.GetEntryAssembly();

        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        public static string Title
        {
            get
            {
                string assemblyTitle = _assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
                if (string.IsNullOrEmpty(assemblyTitle))
                {
                    assemblyTitle = Path.GetFileNameWithoutExtension(_assembly.CodeBase);
                }
                return assemblyTitle;
            }
        }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        public static string Version
        {
            get
            {
                string version = _assembly.GetName().Version.ToString();
                if (version == "1.0.0.0")
                {
                    FileInfo fileInfo = new FileInfo(_assembly.Location);
                    DateTime lastModified = fileInfo.LastWriteTime;
                    version = "*DEV* " + lastModified.ToString("g", CultureInfo.CurrentCulture);
                }
                return version;
            }
        }

        /// <summary>
        /// Gets the assembly description.
        /// </summary>
        public static string Description => _assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? string.Empty;

        /// <summary>
        /// Gets the assembly product.
        /// </summary>
        public static string Product => _assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? string.Empty;

        /// <summary>
        /// Gets the assembly copyright.
        /// </summary>
        public static string Copyright => _assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? string.Empty;

        /// <summary>
        /// Gets the assembly company.
        /// </summary>
        public static string Company => _assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? string.Empty;
    }
}
