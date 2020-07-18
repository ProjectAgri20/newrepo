using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    public static class CtcSettings
    {
        private static PluginEnvironment _environmentData;

        public static string Domain
        {
            get { return _environmentData.UserDomain; }
        }

        public static string ConnectivityShare
        {
            get { return _environmentData.PluginSettings["ConnectivityShare"]; }
        }

        public static string EwsSiteMapLocation
        {
            get { return _environmentData.PluginSettings["EwsSiteMapLocation"]; }
        }

        public static string SeleniumChromeDriverPath
        {
            get { return _environmentData.PluginSettings["SeleniumChromeDriverPath"]; }
        }

        public static string SeleniumIEDriverPath32
        {
            get { return _environmentData.PluginSettings["SeleniumIEDriverPath32"]; }
        }

        public static string SeleniumIEDriverPath64
        {
            get { return _environmentData.PluginSettings["SeleniumIEDriverPath64"]; }
        }

        public static string GetSetting(string name)
        {
            return _environmentData.PluginSettings[name];
        }

        public static Collection<string> ProductFamilies
        {
            get
            {
                var families = ConfigurationServices.EnvironmentConfiguration.AsInternal().GetPrinterFamilies();
                return new Collection<string>(families.ToList());
            }
        }

        public static Collection<string> GetProducts(string productFamily)
        {
            var products = ConfigurationServices.EnvironmentConfiguration.AsInternal().GetPrinterProducts(productFamily);
            return new Collection<string>(products.ToList());
        }

        public static void Initialize(PluginExecutionData executionData)
        {
            Initialize(executionData.Environment);
        }

        public static void Initialize(PluginEnvironment environment)
        {
            _environmentData = environment;
        }
    }
}
