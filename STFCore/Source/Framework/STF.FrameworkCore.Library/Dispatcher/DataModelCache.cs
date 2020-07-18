using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    internal sealed class DataModelCache : IDisposable
    {
        private EnterpriseTestContext _context = new EnterpriseTestContext();
        private readonly Dictionary<string, List<SoftwareInstallerDetail>> _typeInstallers = new Dictionary<string, List<SoftwareInstallerDetail>>();
        private readonly Dictionary<Guid, List<SoftwareInstallerDetail>> _instanceInstallers = new Dictionary<Guid, List<SoftwareInstallerDetail>>();

        /// <summary>
        /// Gets <see cref="SoftwareInstallerDetail"/>s for the specified virtual resource type and metadata types.
        /// </summary>
        /// <param name="virtualResourceType"></param>
        /// <param name="metadataTypeNames"></param>
        /// <returns></returns>
        public IEnumerable<SoftwareInstallerDetail> GetSoftwareInstallerDetails(VirtualResourceType virtualResourceType, IEnumerable<string> metadataTypeNames)
        {
            string resourceTypeName = virtualResourceType.ToString();
            List<SoftwareInstallerDetail> installers = null;

            lock (_typeInstallers)
            {
                int i = 1;

                //Cache Resource Type if not already cached
                if (!_typeInstallers.TryGetValue(resourceTypeName, out installers))
                {
                    installers = new List<SoftwareInstallerDetail>();

                    // If the entry is not found, this will cache the entry for reuse
                    // the next time through
                    foreach (SoftwareInstaller installer in SelectInstallersByResourceType(_context, resourceTypeName))
                    {
                        if (!installers.Any(e => e.InstallerId == installer.InstallerId))
                        {
                            installers.Add(CreateSoftwareInstallerDetail(installer, i++));
                        }
                    }

                    _typeInstallers.Add(resourceTypeName, installers);
                }

                // Now iterate over the cached or new installers list and return each detail
                foreach (var installer in installers)
                {
                    yield return installer;
                }

                //Cache Metadata Types if not already cached
                foreach (string metadataTypeName in metadataTypeNames)
                {
                    installers = null;
                    if (!_typeInstallers.TryGetValue(metadataTypeName, out installers))
                    {
                        installers = new List<SoftwareInstallerDetail>();

                        // If the entry is not found, this will cache the entry for reuse
                        // the next time through
                        foreach (var installer in SelectInstallersByMetadataType(_context, metadataTypeName))
                        {
                            if (!installers.Any(e => e.InstallerId == installer.InstallerId))
                            {
                                installers.Add(CreateSoftwareInstallerDetail(installer, i++));
                            }
                        }

                        _typeInstallers.Add(metadataTypeName, installers);
                    }

                    // Now iterate over the cached or new installers list and return each detail
                    foreach (var installer in installers)
                    {
                        yield return installer;
                    }
                }
            }

        }

        /// <summary>
        /// Clean up data context.
        /// </summary>
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }

        private static IQueryable<SoftwareInstaller> SelectInstallersByResourceType(EnterpriseTestEntities entities, string resourceTypeName)
        {
            return from t in entities.ResourceTypes
                   from p in t.SoftwareInstallerPackages
                   from s in p.SoftwareInstallerSettings
                   where t.Name == resourceTypeName
                   orderby s.InstallOrderNumber
                   select s.SoftwareInstaller;
        }

        private static IQueryable<SoftwareInstaller> SelectInstallersByMetadataType(EnterpriseTestEntities entities, string metadataTypeName)
        {
            return from t in entities.MetadataTypes
                   from p in t.SoftwareInstallerPackages
                   from s in p.SoftwareInstallerSettings
                   where t.Name == metadataTypeName
                   orderby s.InstallOrderNumber
                   select s.SoftwareInstaller;
        }

        private static SoftwareInstallerDetail CreateSoftwareInstallerDetail(SoftwareInstaller installer, int orderNumber)
        {
            return new SoftwareInstallerDetail()
            {
                InstallerId = installer.InstallerId,
                Path = installer.FilePath,
                Arguments = installer.Arguments,
                RebootMode = EnumUtil.Parse<RebootMode>(installer.RebootSetting),
                CopyDirectory = installer.CopyDirectory,
                Description = installer.Description,
                InstallOrderNumber = orderNumber,
            };
        }
    }
}