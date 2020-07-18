using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework
{
    public class ManifestRetrySettingsAgent : IManifestComponentAgent
    {
        private readonly Dictionary<Guid, PluginRetrySettingDictionary> _activityRetrySettings = new Dictionary<Guid, PluginRetrySettingDictionary>();

        public IEnumerable<string> RequestedAssets
        {
            get { yield break; }
        }

        public ManifestRetrySettingsAgent(Guid scenarioId)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                // Retrieve retry settings data for all enabled activities in the specified session
                var activities = (from retrySetting in context.VirtualResourceMetadataRetrySettings
                                  let metadata = retrySetting.VirtualResourceMetadata
                                  let resource = metadata.VirtualResource
                                  where resource.EnterpriseScenarioId == scenarioId
                                     && resource.Enabled == true
                                     && metadata.Enabled == true
                                  group retrySetting by retrySetting.VirtualResourceMetadataId into g
                                  select new { Id = g.Key, RetrySettings = g });

                foreach (var activity in activities)
                {
                    var list = new List<PluginRetrySetting>();
                    foreach (var setting in activity.RetrySettings)
                    {
                        var retrySetting = new PluginRetrySetting(
                            state: EnumUtil.Parse<PluginResult>(setting.State),
                            retryAction: EnumUtil.Parse<PluginRetryAction>(setting.Action),
                            retryLimit: setting.RetryLimit,
                            delayBeforeRetry: new TimeSpan(0, 0, setting.RetryDelay),
                            limitExceededAction: EnumUtil.Parse<PluginRetryAction>(setting.LimitExceededAction)
                            );
                        list.Add(retrySetting);
                    }
                    _activityRetrySettings.Add(activity.Id, new PluginRetrySettingDictionary(list));
                }
            }
        }

        public void AssignManifestInfo(SystemManifest manifest)
        {
            manifest.ActivityRetrySettings.Clear();

            foreach (var retrySetting in _activityRetrySettings)
            {
                manifest.ActivityRetrySettings.Add(retrySetting.Key, retrySetting.Value);
            }
        }

        public void LogComponents(string sessionId)
        {
            // Nothing to do here
        }
    }
}
