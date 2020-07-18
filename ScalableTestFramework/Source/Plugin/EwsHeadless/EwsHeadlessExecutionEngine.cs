using System;
using System.IO;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    ///  EWS for DAT execution controller
    /// </summary>
    public class EwsHeadlessExecutionEngine : IPluginExecutionEngine
    {
        /// <summary>
        /// Processes the activity given to the plugin.
        /// </summary>

        private EwsHeadlessActivityData _activityData = new EwsHeadlessActivityData();

        private IDevice _device;

        /// <summary>
        /// Execution Entry Point
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            if (executionData == null)
                return new PluginExecutionResult(PluginResult.Skipped, "Execution Data is empty!");

            _activityData = executionData.GetMetadata<EwsHeadlessActivityData>();
            var printDeviceInfo = (PrintDeviceInfo)executionData.Assets.First();

            _device = DeviceConstructor.Create(printDeviceInfo);
            IEwsCommunicator communicator = EwsCommunicatorFactory.Create(_device);

            EwsPayloadFactory.AddContractLocation(Path.Combine(executionData.Environment.PluginSettings["DATPayLoadRepository"], "EWSContractFiles"));
            EwsPayloadFactory.AddContractLocation(Path.Combine(executionData.Environment.PluginSettings["DATPayLoadRepository"], "EWSPayLoads"));

            if (_activityData.Operation.Equals(_activityData.DeviceSpecificOperation))
            {
                _activityData.DeviceSpecificOperation = string.Empty; //to match the subfilter type in the XML
            }

            //create request to access EWS DAT payloads
            EwsRequest ewsRequest = communicator.CreateRequest(_activityData.Operation, _activityData.DeviceSpecificOperation);

            foreach (var configurationValue in _activityData.ConfigurationValues)
            {
                string valueToBePassed = configurationValue.Value;
                if (valueToBePassed.Equals("[CurrentUser]", StringComparison.OrdinalIgnoreCase))
                {
                    valueToBePassed = executionData.Credential.UserName;
                }

                if ((_device is SiriusDevice) && (_activityData.Operation.Equals("Administration")) && (configurationValue.Key.Equals("Password")))
                {
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(valueToBePassed);
                    valueToBePassed = Convert.ToBase64String(plainTextBytes);
                }
                ewsRequest.Add(configurationValue.Key, valueToBePassed);
            }

            EwsResult result = communicator.Submit(ewsRequest);

            return result.Exception != null ? new PluginExecutionResult(PluginResult.Failed, result.Exception) : new PluginExecutionResult(PluginResult.Passed);
        }
    }
}