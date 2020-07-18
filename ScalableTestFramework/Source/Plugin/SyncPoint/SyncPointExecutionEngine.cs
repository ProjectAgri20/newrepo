using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using static HP.ScalableTest.Framework.ExecutionServices;

namespace HP.ScalableTest.Plugin.SyncPoint
{
    public class SyncPointExecutionEngine : IPluginExecutionEngine
    {
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            SyncPointActivityData activityData = executionData.GetMetadata<SyncPointActivityData>();
            string eventName = activityData.EventName;

            switch (activityData.Action)
            {
                case SyncPointAction.Signal:
                    SystemTrace.LogDebug($"Signaling synchronization event: {eventName}");
                    SessionRuntime.AsInternal().SignalSynchronizationEvent(eventName);
                    break;

                case SyncPointAction.Wait:
                    SystemTrace.LogDebug($"Waiting for synchronization event: {eventName}");
                    SessionRuntime.AsInternal().WaitForSynchronizationEvent(eventName);
                    break;

                default:
                    return new PluginExecutionResult(PluginResult.Error, $"Unknown action '{activityData.Action}'.");
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }
    }
}
