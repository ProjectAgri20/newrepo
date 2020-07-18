namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    /// <summary>
    /// Defines different printer groups
    /// </summary>
    public enum PrinterFamilies
    {
        /// <summary>
        /// The VEP group
        /// </summary>
        [EnumValue("VEP")]
        VEP = 0,

        /// <summary>
        /// The TPS group
        /// </summary>
        [EnumValue("TPS")]
        TPS = 1,

        /// <summary>
        /// The LFP group
        /// </summary>
        [EnumValue("LFP")]
        LFP = 2,

        /// <summary>
        /// The ink jet group
        /// </summary>
        [EnumValue("InkJet")]
        InkJet = 3,

       ///<summary>
       ///The Apollo group
       ///</summary>
       [EnumValue("Apollo")]
       Apollo = 4,
       
       }
}
