using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation.Properties;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Wrapper class for the dureg.exe registry size estimator tool.
    /// </summary>
    internal static class DuregUtility
    {
        private static string _duregPath;

        private static readonly Dictionary<RegistryHive, string> _hiveParameters = new Dictionary<RegistryHive, string>
        {
            [RegistryHive.ClassesRoot] = "/cr",
            [RegistryHive.CurrentUser] = "/cu",
            [RegistryHive.LocalMachine] = "/lm",
            [RegistryHive.Users] = "/u"
        };

        /// <summary>
        /// Gets the approximate size of the specified registry subtree in the specified hive.
        /// </summary>
        /// <param name="hive">The <see cref="RegistryHive" />.</param>
        /// <param name="subTree">The subtree whose size will be estimated.</param>
        /// <returns>The approximate size of the specified subtree, in bytes.</returns>
        public static int GetRegistrySize(RegistryHive hive, string subTree)
        {
            if (!_hiveParameters.ContainsKey(hive))
            {
                throw new ArgumentException($"Registry hive {hive} does not support size estimation.", nameof(hive));
            }

            InstallDureg();

            string hiveParameter = _hiveParameters[hive];
            string arguments = $"{hiveParameter} \"{subTree}\"";
            string output = ProcessUtil.Execute(_duregPath, arguments, TimeSpan.FromMinutes(5)).StandardOutput.TrimEnd();
            string size = Regex.Match(output, "([0-9]+)$").Groups[1].Value;
            return int.Parse(size);
        }

        private static void InstallDureg()
        {
            if (_duregPath == null)
            {
                string installDirectory = Path.Combine(Path.GetTempPath(), "dureg");
                if (!Directory.Exists(installDirectory))
                {
                    Directory.CreateDirectory(installDirectory);
                }

                _duregPath = Path.Combine(installDirectory, "dureg.exe");
                if (!File.Exists(_duregPath))
                {
                    File.WriteAllBytes(_duregPath, Resources.Dureg);
                }
            }
        }
    }
}
