using System;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class VirtualResourceEventBusShutdownArgs : EventArgs
    {
        /// <summary>
        /// Gets the Copy Logs flag
        /// </summary>
        public bool CopyLogs { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="copyLogs"></param>
        public VirtualResourceEventBusShutdownArgs(bool copyLogs)
        {
            CopyLogs = copyLogs;
        }
    }
}
