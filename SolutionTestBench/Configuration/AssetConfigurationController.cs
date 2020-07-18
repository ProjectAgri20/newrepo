using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Central location for common Asset Inventory operations used across all asset configuration forms.
    /// </summary>
    public class AssetConfigurationController : IDisposable
    {
        private readonly AssetInventoryContext _context = null;
        private bool _disposeContext = false;

        /// <summary>
        /// Constructs a <see cref="AssetConfigurationController"/> instance, creating a new <see cref="AssetInventoryContext"/>.
        /// </summary>
        public AssetConfigurationController(): this(DbConnect.AssetInventoryContext())
        {
            _disposeContext = true;
        }

        /// <summary>
        /// Constructs a <see cref="AssetConfigurationController"/> instance, using the specified <see cref="AssetInventoryContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="AssetInventoryContext"/>.</param>
        public AssetConfigurationController(AssetInventoryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the default <see cref="AssetPool"/>.  If default is not specified, uses the first available <see cref="AssetPool"/>
        /// that is specified in the System Settings "AssetInventoryPools" setting.
        /// </summary>
        /// <returns></returns>
        public AssetPool GetDefaultAssetPool()
        {
            AssetPool result = null;
            // Return only the pools that match AssetInventoryPools SystemSetting.  Otherwise the asset won't be visible in asset selection.
            string[] fromSettings = GetAssetPoolsFromSettings();

            //If a pool named DEFAULT exists, return it.
            if (fromSettings.Contains("DEFAULT"))
            {
                result = _context.AssetPools.FirstOrDefault(x => x.Name.Equals("DEFAULT"));
                if (result != null)
                {
                    return result;
                }
            }

            // No AssetPool named "DEFAULT".  Return the first one that matches a settings value
            foreach (string settingName in fromSettings)
            {
                result = _context.AssetPools.FirstOrDefault(x => x.Name.Equals(settingName));
                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the collection of <see cref="AssetPool"/> names specified in System Settings "AssetInventoryPools".
        /// </summary>
        /// <returns></returns>
        public static string[] GetAssetPoolsFromSettings()
        {
            return (GlobalSettings.Items != null && GlobalSettings.Items.ContainsKey(Setting.AssetInventoryPools) ? 
                GlobalSettings.Items[Setting.AssetInventoryPools] : 
                string.Empty
                ).Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }


        /// <summary>
        /// Disposes of the <see cref="AssetInventoryContext" if this class created it on construction./>
        /// </summary>
        public void Dispose()
        {
            if (_disposeContext)
            {
                _context.Dispose();
            }
        }
    }
}
