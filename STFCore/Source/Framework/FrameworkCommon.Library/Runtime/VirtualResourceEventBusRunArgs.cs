using System;
using System.Linq;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class VirtualResourceEventBusRunArgs : EventArgs
    {
        /// <summary>
        /// Gets the Phase
        /// </summary>
        public ResourceExecutionPhase Phase { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phase"></param>
        public VirtualResourceEventBusRunArgs(ResourceExecutionPhase phase)
        {
            Phase = phase;
        }
    }
}
