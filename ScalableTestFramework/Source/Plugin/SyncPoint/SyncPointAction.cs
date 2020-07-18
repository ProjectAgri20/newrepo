namespace HP.ScalableTest.Plugin.SyncPoint
{
    /// <summary>
    /// Actions that can be performed by the SyncPoint plugin.
    /// </summary>
    internal enum SyncPointAction
    {
        /// <summary>
        /// Sends a synchronization event signal.
        /// </summary>
        Signal,

        /// <summary>
        /// Waits for a synchronization event signal.
        /// </summary>
        Wait
    }
}
