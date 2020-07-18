namespace HP.ScalableTest.DeviceAutomation.Wireless
{
    /// <summary>
    /// The wireless modes
    /// </summary>
    public enum WirelessModes
    {
        /// <summary>
        /// The BG mode
        /// </summary>
        Bg,
        /// <summary>
        /// The BGN mode
        /// </summary>
        Bgn
    }

    /// <summary>
    /// The adoc and infrastructure modes
    /// </summary>
    public enum WirelessStaModes
    {
        /// <summary>
        /// The adhoc mode
        /// </summary>
        Adhoc,
        /// <summary>
        /// The infrastructure mode
        /// </summary>
        Infrastructure
    }

    /// <summary>
    /// The wireless authentication modes
    /// </summary>
    public enum WirelessAuthentications
    {
        /// <summary>
        /// No Security
        /// </summary>
        NoSecurity = 0,
        /// <summary>
        /// WPA Personal
        /// </summary>
        Wpa = 1,
        /// <summary>
        /// WEP Personal
        /// </summary>
        Wep = 2
    }

    /// <summary>
    /// The wireless frequency bands
    /// </summary>
    public enum WirelessBands
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// The 2.4 GHz band
        /// </summary>        
        TwoDotFourGHz = 1,
        /// <summary>
        /// 5GHz band
        /// </summary>
        FiveGHz = 2,
        /// <summary>
        /// Both 2.4 and 5 GHz
        /// </summary>
        Both = TwoDotFourGHz | FiveGHz
    }

    /// <summary>
    /// Enumerator for WiFiDirectConnectionMode
    /// </summary>
    public enum WiFiDirectConnectionMode
    {
        /// <summary>
        /// Auto
        /// </summary>
        Auto,
        /// <summary>
        /// Manual
        /// </summary>
        Manual,
        /// <summary>
        /// Advanced
        /// </summary>
        Advanced,
        /// <summary>
        /// No Security
        /// </summary>
        NoSecurity,
        /// <summary>
        /// With Security
        /// </summary>
        WithSecurity,
        /// <summary>
        /// Automatic
        /// </summary>
        Automatic
    }

    /// <summary>
    /// The device interface type. Applicable only for Jedi devices.
    /// </summary>
    public enum DeviceInterfaceType
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// The Single Interface
        /// </summary>
        SingleInterface,
        /// <summary>
        /// The multiple interface
        /// </summary>
        MultipleInterface
    }
}
