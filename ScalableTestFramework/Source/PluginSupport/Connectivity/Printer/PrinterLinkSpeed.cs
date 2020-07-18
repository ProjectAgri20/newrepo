using System;

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    /// <summary>
    /// Link speed of the printer
    /// Values are equivalent to SNMP values for TPS products
    /// </summary>
    [Flags]
    public enum PrinterLinkSpeed
    {
        /// <summary>
        /// No value
        /// </summary>
        None = 0,
        /// <summary>
        /// 10T Half
        /// </summary>
        [EnumValue("Z10THALF||Z10THALF||Z10THALF||10TX_HALF")]
        Half10T = 3,
        /// <summary>
        /// 10T  Full
        /// </summary>
        [EnumValue("Z10TFULL||Z10TFULL||Z10TFULL||10TX_FULL")]
        Full10T = 2,
        /// <summary>
        /// 100Tx Half
        /// </summary>
        [EnumValue("Z100THALF||Z100THALF||Z100THALF||100TX_HALF")]
        Half100Tx = 5,
        /// <summary>
        /// 100Tx Full
        /// </summary>
        [EnumValue("Z100TFULL||Z100TFULL||Z100TFULL||100TX_FULL")]
        Full100Tx = 4,
        /// <summary>
        /// 1000T Full
        /// </summary>
        [EnumValue("Z1000TFULL")]
        Full1000T = 7,
        /// <summary>
        /// Auto
        /// </summary>
        [EnumValue("ZAUTOS||ZAUTOS||ZAUTOS||AUTO")]
        Auto = 1,
        /// <summary>
        /// 10T Auto
        /// </summary>
        [EnumValue("Z10TAUTO")]
        Auto10T = -1,
        /// <summary>
        /// 100T Auto
        /// </summary>
        [EnumValue("Z100TAUTO")]
        Auto100T = 6
    }
}