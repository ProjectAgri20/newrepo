using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using HP.DeviceAutomation.AccessPoint;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;

namespace HP.ScalableTest.Plugin.Wireless
{
    /// <summary>
    /// Contains data needed to execute a wireless activity.
    /// </summary>
    [DataContract]
    public class WirelessActivityData
    {
        [DataMember]
        public ProductFamilies ProductFamily { get; set; } = ProductFamilies.VEP;

        [DataMember]
        public string ProductName { get; set; } = string.Empty;

        [DataMember]
        public string PrimaryInterfaceAddress { get; set; } = string.Empty;

        [DataMember]
        public int PrimaryInterfaceAddressPortNumber { get; set; }

        [DataMember]
        public string WirelessInterfaceAddress { get; set; } = string.Empty;

        [DataMember]
        public IPAddress SwitchIpAddress { get; set; } = IPAddress.None;

        [DataMember]
        public string AccessPointVendor { get; set; } = string.Empty;

        [DataMember]
        public ProductType PrinterInterfaceType { get; set; } = ProductType.None;

        [DataMember]
        public string DriverPath { get; set; } = string.Empty;

        [DataMember]
        public string DriverModel { get; set; } = string.Empty;

        [DataMember]
        public string SitemapPath { get; set; } = string.Empty;

        [DataMember]
        public string SitemapVersion { get; set; } = string.Empty;

        [DataMember]
        public string DhcpServerIp { get; set; } = "192.168.{0}.254";

        [DataMember]
        public string WirelessMacAddress { get; set; } = string.Empty;

        [DataMember]
        public string SessionId { get; set; } = string.Empty;

        [DataMember]
        public RadiusServerTypes RadiusServerType { get; set; } = RadiusServerTypes.RootSha1;

        [DataMember]
        public string RootSha1ServerIp { get; set; } = string.Empty;

        [DataMember]
        public string RootSha2ServerIp { get; set; } = string.Empty;

        [DataMember]
        public string RadiusServerIp { get; set; } = string.Empty;

		[DataMember]
		public List<AccessPointInfo> AccessPointDetails = new List<AccessPointInfo>();

        [DataMember]
        public bool EnableDebug { get; set; } = false;

        [DataMember]
        public Radio WirelessRadio { get; set; } = Radio._2dot4Ghz;

        [DataMember]
        public string PowerSwitchIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the selected test numbers
        /// </summary>
        [DataMember]
        public Collection<int> SelectedTests { get; set; } = new Collection<int>();
    }

    public struct AccessPointInfo
    {
        public IPAddress Address { get; set; }

        public AccessPointManufacturer Vendor { get; set; }

        public string Model { get; set; }

        public string UserName { get; set; }

        public string password { get; set; }

        public int PortNumber { get; set; }
    }
}