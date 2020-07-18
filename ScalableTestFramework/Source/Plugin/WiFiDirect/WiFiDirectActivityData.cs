using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;

using HP.DeviceAutomation.AccessPoint;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.WiFiDirect
{
    /// <summary>
    /// Contains data needed to execute a wireless activity.
    /// </summary>
    [DataContract]
    public class WiFiDirectActivityData
    {
        [DataMember]
        public ProductFamilies ProductFamily { get; set; } = ProductFamilies.VEP;

        [DataMember]
        public string ProductName { get; set; } = string.Empty;

        [DataMember]
        public string PrimaryInterfaceAddress { get; set; } = string.Empty;

        [DataMember]
        public string SecondaryInterfaceAddress { get; set; } = string.Empty;

        [DataMember]
        public int PrimaryInterfaceAddressPortNumber { get; set; }

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

        /// <summary>
        /// Gets or sets the selected test numbers
        /// </summary>
        [DataMember]
        public Collection<int> SelectedTests { get; set; } = new Collection<int>();

        [DataMember]
        public List<AccessPointInfo> AccessPointDetails = new List<AccessPointInfo>();

        [DataMember]
        public bool EnableDebug { get; set; }

        [DataMember]
        public string WiFiSsid { get; set; } = string.Empty;

        [DataMember]
        public bool IsWifiTestsSelected { get; set; }
    }

    public struct AccessPointInfo
    {
        public IPAddress Address { get; set; }

        public AccessPointManufacturer Vendor { get; set; }

        public string Model { get; set; }
    }
}