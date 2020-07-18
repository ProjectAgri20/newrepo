using System.Collections.Generic;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.Printing
{
    internal static class ConverterProvider
    {
        public static IEnumerable<IPluginMetadataConverter> GetMetadataConverters()
        {
            yield return new PrintingDataConverter1_1();
        }
    }
}
