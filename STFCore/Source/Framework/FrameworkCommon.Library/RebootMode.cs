
using System.ComponentModel;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public enum RebootMode
    {
        /// <summary>
        /// Reboot immediately after this installation file finishes executing.
        /// </summary>
        [Description("Immediate - Reboot immediately after this installer executes")]
        Immediate,

        /// <summary>
        /// Defers reboot until after all installation files are executed.
        /// </summary>
        [Description("Deferred - Defers reboot until all installers have executed")]
        Deferred,

        /// <summary>
        /// Does not require a reboot after installation
        /// </summary>
        [Description("No Reboot - Does not reboot after installation")]
        NoReboot,
    }
}
