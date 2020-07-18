using System.ComponentModel;

namespace HP.ScalableTest.Virtualization
{
    /// <summary>
    /// OS type of the VM.
    /// </summary>
    public enum ManagedMachineType
    {
        /// <summary>
        /// Defines the machine type as a virtual machine running windows
        /// </summary>
        [Description("WindowsVirtual")]
        WindowsVirtual,

        /// <summary>
        /// Defines the machine type as a physical machine running windows
        /// </summary>
        [Description("WindowsPhysical")]
        WindowsPhysical,

        /// <summary>
        /// Defines the machine type as a standalone desktop machine running windows
        /// </summary>
        [Description("WindowsDesktop")]
        WindowsDesktop,

        /// <summary>
        /// Legacy value that translates to a virtual machine type
        /// </summary>
        [Description("WindowsVirtual")]
        Windows
    }
}
