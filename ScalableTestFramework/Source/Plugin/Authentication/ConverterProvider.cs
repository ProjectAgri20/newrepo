using System.Collections.Generic;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.Authentication
{
    internal static class ConverterProvider
    {
        public static IEnumerable<IPluginMetadataConverter> GetMetadataConverters()
        {
            yield return new AuthDataConverter1_1();
        }
    }

}
