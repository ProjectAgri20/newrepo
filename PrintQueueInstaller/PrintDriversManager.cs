using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Print.Drivers;
using VB = Microsoft.VisualBasic.FileIO;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Contains utility methods for adding print driver packages to the environment
    /// </summary>
    public static class PrintDriversManager
    {
        private static string _universalPrintDriverShareLocation = null;

        /// <summary>
        /// Gets the network share where UPD code drops are placed.
        /// This network share should be available as long as it is being accessed on the HP network.
        /// </summary>
        public static string UniversalPrintDriverShareLocation
        {
            get
            {
                if (string.IsNullOrEmpty(_universalPrintDriverShareLocation))
                {
                    _universalPrintDriverShareLocation = GlobalSettings.Items[Setting.UniversalPrintDriverBaseLocation];
                }

                return _universalPrintDriverShareLocation;
            }
        }

        /// <summary>
        /// Builds a <see cref="PrintDeviceDriverCollection"/> based off of the folder path to a print driver package.
        /// </summary>
        /// <param name="directory">Folder path to a print driver package.</param>
        /// <param name="versionPath">The base path to the driver repository.  Everything after this path in parameter 
        /// <paramref name="directory"/> is considered the version.</param>
        /// <returns>
        /// A <see cref="PrintDeviceDriverCollection"/> based on <paramref name="directory"/>
        /// </returns>
        public static PrintDeviceDriverCollection LoadDrivers(string directory, string versionPath)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException("directory");
            }

            if (string.IsNullOrEmpty(versionPath))
            {
                throw new ArgumentNullException("versionPath");
            }

            PrintDeviceDriverCollection drivers = new PrintDeviceDriverCollection();
            drivers.AddRange(DriverController.LoadFromDirectory(directory, true, SearchOption.AllDirectories).Select(n => new PrintDeviceDriver(n)));

            // The version of a package is defined as the relative path of the directory.
            // This is only used when copying a driver down from the CSL central repository.  If this is a
            // local print driver install then this can be skipped as it won't be used.
            if (directory.StartsWith(versionPath, StringComparison.OrdinalIgnoreCase))
            {
                drivers.Version = directory.Substring(versionPath.Length).Trim(Path.DirectorySeparatorChar);
            }

            return drivers;
        }

        /// <summary>
        /// Copies the driver packages to a local path
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="drivers">The packages.</param>
        /// <returns></returns>
        public static Collection<string> CopyDrivers(string path, Collection<PrintDeviceDriverCollection> drivers)
        {
            Collection<string> driverPaths = new Collection<string>();

            if (drivers == null)
            {
                throw new ArgumentNullException("drivers");
            }

            var query =
                (
                    from driver in drivers
                    from properties in driver
                    where !string.IsNullOrEmpty(properties.InfPath)
                    select new { driver.Version, properties.InfPath, properties.Architecture }
                ).Distinct();

            foreach (var info in query)
            {
                string source = Path.GetDirectoryName(info.InfPath);
                string destination = Path.Combine(path, info.Version, info.Architecture.ToString());

                VB.FileSystem.CopyDirectory(source, destination, VB.UIOption.AllDialogs);

                driverPaths.Add(destination);
            }

            return driverPaths;
        }
    }
}
