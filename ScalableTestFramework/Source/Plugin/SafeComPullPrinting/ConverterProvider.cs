using System.Collections.Generic;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.SafeComPullPrinting
{
    internal static class ConverterProvider
    {
        public static IEnumerable<IPluginMetadataConverter> GetMetadataConverters()
        {
            yield return new SafeComDataConverter1_1();
            yield return new SafeComDataConverter1_2();
        }
    }
}
