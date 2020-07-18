using System;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Attributes that may be used for classifying or filtering assets.
    /// </summary>
    [Flags]
    public enum AssetAttributes
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Asset has print capability.
        /// </summary>
        Printer = 0x1,

        /// <summary>
        /// Asset has scan capability.
        /// </summary>
        Scanner = 0x2,

        /// <summary>
        /// Asset has a built-in graphical or text-based control panel.
        /// </summary>
        ControlPanel = 0x4,

        /// <summary>
        /// Asset is a mobile device with app capability (Android, iOS, etc.)
        /// </summary>
        Mobile = 0x8
    }
}
