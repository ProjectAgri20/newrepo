using System;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// 
    /// </summary>
    public class HostMachineEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RuntimeState State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="message"></param>
        public HostMachineEventArgs(RuntimeState state, string message)
        {
            State = state;
            Message = message;
        }
    }
}
