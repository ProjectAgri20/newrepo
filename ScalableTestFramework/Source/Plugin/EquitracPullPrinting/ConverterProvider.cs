using System.Collections.Generic;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.EquitracPullPrinting
{
    internal static class ConverterProvider
    {
        public static IEnumerable<IPluginMetadataConverter> GetMetadataConverters()
        {
            yield return new EquitracDataConverter1_1();
            yield return new EquitracDataConverter1_2();
            yield return new EquitracDataConverter1_3();
        }
    }
}
