using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.WindowsAutomation;
using VB = Microsoft.VisualBasic.FileIO;

namespace HP.ScalableTest
{
    /// <summary>
    /// Contains utility methods for adding print driver packages to the environment
    /// </summary>
    public static class PrintDriversManager
    {
        private static string _driverRepositoryLocation = null;

        /// <summary>
        /// The Location of Print Drivers defined in system setting
        /// </summary>
        public static string DriverRepositoryLocation
        {
            get
            {
                if (string.IsNullOrEmpty(_driverRepositoryLocation))
                {
                    _driverRepositoryLocation = GlobalSettings.Items[Setting.PrintDriverServer];
                }

                return _driverRepositoryLocation;
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
            drivers.AddRange(DriverController.LoadFromDirectory(directory, true, SearchOption.AllDirectories));

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
        /// Adds a collection of <see cref="PrintDeviceDriverCollection"/> to the testing environment.
        /// </summary>
        /// <param name="drivers">Collection of print driver packages to add to the testing environment.</param>
        /// <param name="overwrite">Specifies whether to overwrite the driver packages, should duplications be found.
        /// The default is false.</param>
        /// <returns>
        /// A new collection of print driver packages with INF file locations updated.
        /// </returns>
        public static void AddToFrameworkRepository(Collection<PrintDeviceDriverCollection> drivers, bool overwrite = false)
        {
            if (drivers == null)
            {
                throw new ArgumentNullException("drivers");
            }

            string baseLocation = GlobalSettings.Items[Setting.PrintDriverServer];
            NetworkCredential admin = GlobalSettings.Items.DomainAdminCredential;

            NetworkConnection.AddConnection(baseLocation, admin);

            try
            {
                foreach (PrintDeviceDriverCollection driver in drivers)
                {
                    driver.AddToFrameworkRepository(baseLocation, overwrite);
                }
            }
            finally
            {
                NetworkConnection.RemoveConnection(baseLocation, forceRemoval: true);
            }
        }

        /// <summary>
        /// Copis the specified print driver package to the environment and adds all necessary database entires to the database.
        /// </summary>
        /// <param name="drivers">The package to be added.</param>
        /// <param name="basePath">The path of the print driver package network store.</param>
        /// <param name="overwrite">if set to <c>true</c>, overwrite existing files.</param>
        /// <returns></returns>
        public static void AddToFrameworkRepository(this PrintDeviceDriverCollection drivers, string basePath, bool overwrite = false)
        {
            if (drivers == null)
            {
                throw new ArgumentNullException("drivers");
            }

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                var dbDriverPackage = GetDatabaseDriverPackage(context, drivers, basePath, overwrite);

                CopyDrivers(drivers, basePath);

                // Return a new instance of PrintDriverPackage that contains the correct INF file locations.
                PrintDeviceDriverCollection reloadedDrivers = LoadDrivers(Path.Combine(basePath, drivers.Version), basePath);

                // Get a distinct list of x86 and x64 INF paths joined by the common driver name
                var infPathQuery =
                    (
                        from d1 in reloadedDrivers
                        where d1.Architecture == DriverArchitecture.NTAMD64
                        join d2 in reloadedDrivers on d1.Name equals d2.Name into driverItems
                        from d3 in driverItems
                        where d3.Architecture == DriverArchitecture.NTx86
                        select new { x64Path = d1.InfPath, x86Path = d3.InfPath }
                    ).Distinct();

                // For each distinct combination of INF paths, create a new entry in the print driver package table.
                // NOTE: that for now if more than one row is added for the same version, this could be a problem
                // as the backend won't support it.  But the database will be fixed in the future to support 
                // multiple entries.
                foreach (var item in infPathQuery)
                {
                    // Create new database entries for this print driver package.
                    dbDriverPackage = new PrintDriverPackage
                    {
                        PrintDriverPackageId = SequentialGuid.NewGuid(),
                        Name = reloadedDrivers.Version,
                        InfX86 = TrimPath(item.x86Path, basePath),
                        InfX64 = TrimPath(item.x64Path, basePath)
                    };

                    context.PrintDriverPackages.Add(dbDriverPackage);

                    var driverQuery =
                        (
                            from d in reloadedDrivers
                            select new { d.Name, Processor = d.PrintProcessor.ToLowerInvariant() }
                        ).Distinct();

                    foreach (var properties in driverQuery)
                    {
                        var printDriver = new PrintDriver
                        {
                            PrintDriverId = SequentialGuid.NewGuid(),
                            PrintDriverPackage = dbDriverPackage,
                            Name = properties.Name,
                            PrintProcessor = properties.Processor
                        };
                        dbDriverPackage.PrintDrivers.Add(printDriver);
                    }
                }

                context.SaveChanges();
            }
        }

        private static PrintDriverPackage GetDatabaseDriverPackage(AssetInventoryContext context, PrintDeviceDriverCollection drivers, string basePath, bool overwrite)
        {
            // Check to see if this entry already exists.
            var dbDriverPackage = context.PrintDriverPackages.Include("PrintDrivers").FirstOrDefault(n => n.Name == drivers.Version);

            if (dbDriverPackage != null)
            {
                if (overwrite)
                {
                    // Delete the files.
                    string packagePath = Path.GetDirectoryName(dbDriverPackage.InfX86.TrimEnd('\\'));
                    string deletePath = Path.Combine(basePath, packagePath);
                    if (Directory.Exists(deletePath))
                    {
                        Directory.Delete(deletePath, recursive: true);
                    }

                    // Remove database entries.
                    while (dbDriverPackage.PrintDrivers.Count != 0)
                    {
                        context.PrintDrivers.Remove(dbDriverPackage.PrintDrivers.First());
                    }
                    context.PrintDriverPackages.Remove(dbDriverPackage);
                }
                else
                {
                    throw new IOException("Print Driver Package version already exists: " + drivers.Version);
                }
            }
            return dbDriverPackage;
        }

        private static void CopyDrivers(PrintDeviceDriverCollection drivers, string basePath)
        {
            var query =
                (
                    from d in drivers
                    where !string.IsNullOrEmpty(d.InfPath)
                    select new { drivers.Version, d.InfPath, d.Architecture }
                ).Distinct();

            foreach (var item in query)
            {
                string source = Path.GetDirectoryName(item.InfPath);
                string destination = Path.Combine(basePath, drivers.Version, item.Architecture.ToString());

                // Create the destination path.
                Directory.CreateDirectory(destination);

                VB.FileSystem.CopyDirectory(source, destination, VB.UIOption.AllDialogs);
            }
        }

        private static string TrimPath(string driverPath, string rootPath)
        {
            return string.IsNullOrEmpty(driverPath) ? string.Empty
                : driverPath.Substring(rootPath.Length).Trim(Path.DirectorySeparatorChar);
        }
    }
}
