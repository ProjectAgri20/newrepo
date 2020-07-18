using System.Collections.Generic;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework
{
    public class ManifestSettingsAgent : IManifestComponentAgent
    {
        public IEnumerable<string> RequestedAssets
        {
            get { yield break; }
        }

        public void AssignManifestInfo(SystemManifest manifest)
        {
            manifest.PullFromGlobalSettings();
        }

        public void LogComponents(string sessionId)
        {
            // Nothing to do here
        }
    }
}
