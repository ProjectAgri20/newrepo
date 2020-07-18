using System;
using System.IO;
using Microsoft.Win32;

namespace HP.ScalableTest.PluginSupport.Connectivity.Selenium
{
    /// <summary>
    /// Used to provide helper methods associated with external third party applications and utilities.
    /// </summary>
    public static class JavaUtil
    {
        /// <summary>
        /// Gets the java installation path with double quotes appended.
        /// </summary>
        /// <value>
        /// The <see cref="System.String"/> value of the Java installation path.
        /// </value>
        public static string JavaExePath
        {
            get
            {
                string path = string.Empty;

                string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
                if (!string.IsNullOrEmpty(environmentPath))
                {
                    path = environmentPath;
                }
                else
                {
                    using (RegistryKey javaKey =
                        Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Javasoft\Java Runtime Environment") ??
                        Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Javasoft\Java Runtime Environment"))
                    {
                        if (javaKey != null)
                        {
                            string currentVersion = javaKey.GetValue("CurrentVersion").ToString();
                            using (RegistryKey version = javaKey.OpenSubKey(currentVersion))
                            {
                                path = Path.Combine(Path.Combine(version.GetValue("JavaHome").ToString(), "bin"), "java.exe");
                            }
                        }
                    }
                }

                return string.Concat("\"", path, "\"");
            }
        }
    }
}