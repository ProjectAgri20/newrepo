using System;
using System.Net;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;

namespace HP.ScalableTest.Plugin.TelnetSnmp
{
    public class TelnetSnmpExecutionEngine : IPluginExecutionEngine
    {
        private TelnetTests _tests;

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            TelnetSnmpActivityData activityData = executionData.GetMetadata<TelnetSnmpActivityData>(CtcMetadataConverter.Converters);

            if (null == _tests)
            {
                _tests = new TelnetTests(activityData);
            }
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductCategory);
            // create instance of ews adapter
            EwsWrapper.Instance().Create(family, activityData.ProductName, activityData.PrinterIP, activityData.SitemapsVersion, BrowserModel.Firefox);

            EwsWrapper.Instance().Start();

            // Enable telnet as after cold reset telnet will be disabled by default in the new product versions.
            EwsWrapper.Instance().SetTelnet();

            foreach (int testNumber in activityData.TestNumbers)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _tests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.PrinterIP), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductCategory));
                }
                catch (Exception pauseException)
                {
                    TraceFactory.Logger.Fatal("Error while pausing: " + pauseException.Message);
                }
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }
    }
}
