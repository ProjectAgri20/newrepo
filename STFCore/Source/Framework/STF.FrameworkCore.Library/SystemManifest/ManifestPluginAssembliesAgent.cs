using System.Collections.Generic;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.Plugin;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework
{
    public class ManifestPluginAssembliesAgent : IManifestComponentAgent
    {
        public IEnumerable<string> RequestedAssets
        {
            get
            {
                yield break;
            }
        }

        public void AssignManifestInfo(SystemManifest manifest)
        {
            manifest.PluginDefinitions.Clear();
            foreach (PluginDefinition definition in SettingsLoader.LoadPluginDefinitions())
            {
                manifest.PluginDefinitions.Add(definition);
            }
        }

        public void LogComponents(string sessionId)
        {

        }
    }
}
