using System;
using System.Net;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.Ews
{
    public class EwsExecutionEngine : IPluginExecutionEngine
    {
        private EwsTests _tests;

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            EwsActivityData data = executionData.GetMetadata<EwsActivityData>(CtcMetadataConverter.Converters);

            if (null == _tests)
            {
                _tests = new EwsTests(data);
            }

            foreach (int testNumber in data.TestNumbers)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _tests.RunTest(executionData, testNumber, IPAddress.Parse(data.PrinterIP), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), data.ProductCategory));
                }
                catch (Exception pluginException)
                {
                    TraceFactory.Logger.Error("Error while pausing: " + pluginException.Message);
                }
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }
    }
}

