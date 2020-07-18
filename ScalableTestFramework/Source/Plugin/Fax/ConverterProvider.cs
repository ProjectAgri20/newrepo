using HP.ScalableTest.Framework.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.Fax
{
    internal static class ConverterProvider
    {
        public static IEnumerable<IPluginMetadataConverter> GetMetadataConverters()
        {
            yield return new FaxDataConverter0_5();
            yield return new FaxDataConverter1_1();
        }
    }
}
