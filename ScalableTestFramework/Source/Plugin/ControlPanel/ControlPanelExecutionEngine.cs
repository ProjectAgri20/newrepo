using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    /// <summary>
    /// This is the execution engine for control panel plugin
    /// </summary>
    public class ControlPanelExecutionEngine : IPluginExecutionEngine
    {
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            var activityData = executionData.GetMetadata<ControlPanelActivityData>();

            ConcurrentDictionary<string, PluginExecutionResult> results = new ConcurrentDictionary<string, PluginExecutionResult>();
            TimeSpan lockTimeout = TimeSpan.FromMinutes(5);
            TimeSpan holdTimeout = TimeSpan.FromMinutes(5);

            //if the assets are not found then skip it
            if (!executionData.Assets.OfType<IDeviceInfo>().Any())
                return new PluginExecutionResult(PluginResult.Skipped, "No Assets found for execution");

            Parallel.ForEach(executionData.Assets.OfType<IDeviceInfo>(),
                asset =>
                {
                    if (!asset.Attributes.HasFlag(AssetAttributes.ControlPanel))
                    {
                        results.AddOrUpdate(asset.AssetId,
                            new PluginExecutionResult(PluginResult.Skipped, "Device does not have control panel"),
                            (key, oldValue) =>
                                new PluginExecutionResult(PluginResult.Skipped, "Device does not have control panel"));
                        return;
                    }
                    AssetLockToken assetToken = new AssetLockToken(asset, lockTimeout, holdTimeout);
                    ExecutionServices.CriticalSection.Run(assetToken, () =>
                    {
                        var printDevice = DeviceConstructor.Create(asset);
                        ControlPanelWrapper wrapper = new ControlPanelWrapper(printDevice, executionData.Credential,
                        activityData.ControlPanelAction, activityData.ParameterValues);
                        var result = wrapper.Execute();
                        results.AddOrUpdate(asset.AssetId, result, (key, oldValue) => wrapper.Execute());
                        printDevice.Dispose();
                    });
                });
            
            return results.Any(x => x.Value.Result != PluginResult.Passed) ? new PluginExecutionResult(PluginResult.Failed) : new PluginExecutionResult(PluginResult.Passed);
        }
    }

    public enum Language
    {
        Dansk, Deutsch, English, Espanol, French, Italiano, Norsk, Netherlands, Svensk, Suomi, Portugues, Turkey, Poliski, Russian, Cestina, Magyar, Japan, ChineseTraditional, ChineseSimplified, Korean, Greek, Croatian, Romanian, Slovak, Slovenian, Catalan, Thai, Indonesia
    };
}