using System.Collections.Generic;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.HpacPullPrinting
{
    internal static class ConverterProvider
    {
        public static IEnumerable<IPluginMetadataConverter> GetMetadataConverters()
        {
            yield return new HpacDataConverter1_1();
            yield return new HpacDataConverter1_2();
        }
    }
}
