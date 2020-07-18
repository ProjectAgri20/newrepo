using System.Collections.Generic;
using Microsoft.Win32;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Helper class for querying list of applications installed on the machine
    /// </summary>
    public static class InstalledAppHelper
    {
        const string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        /// <summary>
        /// Class for holding information about an installed application
        /// </summary>
        public class InstalledApp
        {
            public string DisplayName { get; set; }
            public string DisplayVersion { get; set; }

            public override string ToString()
            {
                return string.Format("{0}, {1}", DisplayName, DisplayVersion);
            }
        }

        public static List<InstalledApp> GetInstalledPrograms()
        {
            var result = new List<InstalledApp>();
            result.AddRange(GetInstalledProgramsFromRegistry(RegistryView.Registry32));
            result.AddRange(GetInstalledProgramsFromRegistry(RegistryView.Registry64));
            return result;
        }

        private static IEnumerable<InstalledApp> GetInstalledProgramsFromRegistry(RegistryView registryView)
        {
            var result = new List<InstalledApp>();

            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView).OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        if (IsProgramVisible(subkey))
                        {
                            result.Add(
                                new InstalledApp()
                                {
                                    DisplayName = (string)subkey.GetValue("DisplayName"),
                                    DisplayVersion = (string)subkey.GetValue("DisplayVersion")
                                }
                            );
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsProgramVisible(RegistryKey subkey)
        {
            var name = (string)subkey.GetValue("DisplayName");
            var releaseType = (string)subkey.GetValue("ReleaseType");
            //var unistallString = (string)subkey.GetValue("UninstallString");
            var systemComponent = subkey.GetValue("SystemComponent");
            var parentName = (string)subkey.GetValue("ParentDisplayName");

            return
                !string.IsNullOrEmpty(name)
                && string.IsNullOrEmpty(releaseType)
                && string.IsNullOrEmpty(parentName)
                && (systemComponent == null);
        }
    }
}
