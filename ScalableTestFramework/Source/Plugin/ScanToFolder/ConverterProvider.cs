using HP.ScalableTest.Framework.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.ScanToFolder
{
    internal static class ConverterProvider
    {
        public static IEnumerable<IPluginMetadataConverter> GetMetadataConverters()
        {
            yield return new ScanToFolderDataConverter1_1();
        }
    }
}
