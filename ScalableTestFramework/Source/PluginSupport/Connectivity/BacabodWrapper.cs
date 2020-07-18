using System;
using System.IO;
using System.Reflection;
using HP.ScalableTest.PluginSupport.Connectivity.Properties;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    #region enum

    /// <summary>
    /// Source Type
    /// </summary>
    public enum BacaBodSourceType
    {
        /// <summary>
        /// The IP Address source type to discover the printer.
        /// </summary>
        IPAddress = 0,
        /// <summary>
        /// The Host Name source type to discover the printer.
        /// </summary>
        HostName = 1,
        /// <summary>
        /// The MacAddress source type to discover the printer.
        /// </summary>
        MacAddress = 2
    }

    /// <summary>
    /// BacaBod Discovery Type
    /// </summary>
    public enum BacaBodDiscoveryType
    {
        /// <summary>
        /// The SLP Discovery Type/
        /// </summary>
        SLP = 1,
        /// <summary>
        /// The WSDV6 Discovery Type.
        /// </summary>
        WSDV6 = 2,
        /// <summary>
        /// The WSDV4 Discovery Type.
        /// </summary>
        WSDV4 = 4,
        /// <summary>
        /// All the Discovery Types.
        /// </summary>
        All = 7,
    }

    /// <summary>
    /// Packet cast option
    /// </summary>
    public enum CastType
    {
        /// <summary>
        /// Multicast option
        /// </summary>
        Multicast = 1,

        /// <summary>
        /// Unicast option
        /// </summary>
        Unicast = 2
    }

    #endregion

    #region BacaBodWrapper

    /// <summary>
    /// BacaBod Wrapper around the BacaBod tool
    /// </summary>
    public static class BacaBodWrapper
    {
	const string BACABOD_TOOL = "BacabodTool.exe";
        /// <summary>
        /// Discovers the device using the BacaBod tool based on the source type and discovery type
        /// </summary>
        /// <param name="source">source string (IP Address/Hostname/MAC Address)</param>
        /// <param name="sourceType">Source Type</param>
        /// <param name="discoveryType">Discovery Type</param>
        /// <param name="ipAddress">Sets IP Address of the printer if the source is Host Name or the MAC Address</param>
        /// <param name="hostName">Name of the host.</param>
        /// <returns>Returns true if the device is discovered, else returns false</returns>
		public static bool DiscoverDevice(string source, BacaBodSourceType sourceType, BacaBodDiscoveryType discoveryType, ref string ipAddress, ref string hostName, CastType castType = CastType.Multicast)
        {
            string bacabodToolPath = GetBacabodToolPath();

            // Convert MAC address to capital letters (Bacabod tool expects the MAC address to be in capital letters)
            if (sourceType == BacaBodSourceType.MacAddress)
            {
                source = source.ToUpperInvariant();
            }

            TraceFactory.Logger.Debug("Discovering with Bacabod Tool with Source:{0}, Source Type:{1}, Discovery Type{2}".FormatWith(source, sourceType, discoveryType));
            try
            {
                if (File.Exists(bacabodToolPath))
                {
                    var processResult = HP.ScalableTest.Utility.ProcessUtil.Execute(bacabodToolPath, source + " " + sourceType.GetHashCode() + " " + discoveryType.GetHashCode() + " " + castType.GetHashCode());

                string output = processResult.StandardOutput;

                bool result = output.StartsWith("True", StringComparison.CurrentCultureIgnoreCase);

                if (result)
                {
                    string[] arr = output.Split('|');

                    if (arr.Length > 1)
                    {
                        ipAddress = arr[1].Trim();
                    }

					if (arr.Length >= 3)
					{
						hostName = arr[3].Trim();
					}

                    TraceFactory.Logger.Info("Printer Discovered with IP Address {0} using Bacabod tool".FormatWith(ipAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer not Discovered using Bacabod tool");
                }

                return result;
            }
            else
            {
                TraceFactory.Logger.Info("{0} is missing in the Plugin folder".FormatWith(bacabodToolPath));

                    return false;
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception occured in Bacabod Discovery : {0}".FormatWith(ex.Message));
                return false;
            }
        }


        /// <summary>
        /// Discovers the device using the BacaBod tool based on the source type and discovery type
        /// </summary>
        /// <param name="source">source string (IP Address/Hostname/MAC Address)</param>
        /// <param name="sourceType">Source Type</param>
        /// <param name="discoveryType">Discovery Type</param>
        /// <returns>Returns true if the device is discovered, else returns false</returns>
        public static bool DiscoverDevice(string source, BacaBodSourceType sourceType, BacaBodDiscoveryType discoveryType, CastType castType = CastType.Multicast)
        {
            string bacabodToolPath = GetBacabodToolPath();
            TraceFactory.Logger.Info("bacabodtoolpath : {0}".FormatWith(bacabodToolPath));
            // Convert MAC address to capital letters (Bacabod tool expects the MAC address to be in capital letters)
            if (sourceType == BacaBodSourceType.MacAddress)
            {
                source = source.ToUpperInvariant();
                TraceFactory.Logger.Info("Printer MAC address : {0}".FormatWith(source));
            }

            if (File.Exists(bacabodToolPath))
            {
                TraceFactory.Logger.Info("Execute Command: {0}".FormatWith(bacabodToolPath, source + " " + sourceType.GetHashCode() + " " + discoveryType.GetHashCode() + " " + castType.GetHashCode()));
                var result = ProcessUtil.Execute(bacabodToolPath, source + " " + sourceType.GetHashCode() + " " + discoveryType.GetHashCode() + " " + castType.GetHashCode());
                TraceFactory.Logger.Info("Execute Result : {0}".FormatWith(result));
                return result.StandardOutput.StartsWith("True");
            }
            else
            {
                TraceFactory.Logger.Info("{0} is missing in the Plugin folder".FormatWith(bacabodToolPath));

                return false;
            }
        }

        private static string GetBacabodToolPath()
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string bacabodPath = Path.Combine(assemblyPath, "BacabodTool.exe");
            TraceFactory.Logger.Info("bacabod path :{0}".FormatWith(bacabodPath));
            if (!File.Exists(bacabodPath))
            {
                TraceFactory.Logger.Info("bacabod path :{0}".FormatWith(bacabodPath));
                File.WriteAllBytes(bacabodPath, Resource.BacabodTool);
                File.WriteAllBytes(Path.Combine(assemblyPath, "sdisdk.dll"), Resource.sdisdk);
            }

            return bacabodPath;
        }
    }

    #endregion
}
