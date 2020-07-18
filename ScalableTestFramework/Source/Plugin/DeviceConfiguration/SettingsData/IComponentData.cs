using System;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;

namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    public interface IComponentData
    {
        /// <summary>
        /// Using the pluginExecution data, determines which fields should be updated/set.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="pluginExecutionData"></param>
        /// <returns></returns>
        bool ExecuteJob(JediDevice device, AssetInfo assetInfo, Framework.Plugin.PluginExecutionData pluginExecutionData);

        /// <summary>
        /// Entry function to setting fields on a device
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="pluginExecutionData"></param>
        /// <returns>success data/error message</returns>
        DataPair<string> SetFields(JediDevice device, AssetInfo assetInfo, Framework.Plugin.PluginExecutionData pluginExecutionData);
        /// <summary>
        /// Interface function to update and log device fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ChangeValue"></param>
        /// <param name="device"></param>
        /// <param name="data"></param>
        /// <param name="urn"></param>
        /// <param name="endpoint"></param>
        /// <param name="assetInfo"></param>
        /// <param name="activity"></param>
        /// <param name="pluginExecutionData"></param>
        /// <returns>Success bool</returns>
        bool UpdateField<T>(Func<WebServiceTicket, WebServiceTicket> ChangeValue, JediDevice device, DataPair<T> data, string urn, string endpoint, AssetInfo assetInfo, string activity, Framework.Plugin.PluginExecutionData pluginExecutionData);
    }
}
