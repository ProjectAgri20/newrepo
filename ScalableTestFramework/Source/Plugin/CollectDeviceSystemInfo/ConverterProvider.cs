using System;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.CollectDeviceSystemInfo
{
    internal class ConverterProvider
    {
        public static IEnumerable<IPluginMetadataConverter> GetMetadataConverters()
        {
            yield return new CollectDeviceInfoDataConverter1_1();
        }
    }
}
