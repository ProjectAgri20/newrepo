namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Enumeration of top-level registry hives.
    /// </summary>
    public enum RegistryHive
    {
        /// <summary>
        /// The HKEY_CLASSES_ROOT registry hive.
        /// </summary>
        ClassesRoot,

        /// <summary>
        /// The HKEY_CURRENT_USER registry hive.
        /// </summary>
        CurrentUser,

        /// <summary>
        /// The HKEY_LOCAL_MACHINE registry hive.
        /// </summary>
        LocalMachine,

        /// <summary>
        /// The HKEY_USERS registry hive.
        /// </summary>
        Users,

        /// <summary>
        /// The HKEY_CURRENT_CONFIG registry hive.
        /// </summary>
        CurrentConfig
    }
}
