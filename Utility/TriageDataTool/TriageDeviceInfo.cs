namespace HP.STF.TriageDataTool
{
    /// <summary>
    /// This class is used to hold the information displayed in the 'Select
    /// Device ID' panel.
    /// </summary>
    internal class TriageDeviceInfo
    {
        public string DeviceName { get; internal set; }

        public string ProductName { get; internal set; }

        public string ModelNumber { get; internal set; }

        public string FirmwareRevision { get; internal set; }

        public string FirmwareDatecode { get; internal set; }

        public string IpAddress { get; internal set; }
    }
}
