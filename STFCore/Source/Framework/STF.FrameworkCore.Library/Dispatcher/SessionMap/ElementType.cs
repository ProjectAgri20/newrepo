using System;
using System.Linq;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// All elements that can be present in a session map
    /// </summary>
    public enum ElementType
    {
        /// <summary>
        /// The asset map element
        /// </summary>
        Assets,

        /// <summary>
        /// The asset host element
        /// </summary>
        Device,

        /// <summary>
        /// The resource activity element
        /// </summary>
        Activity,

        /// <summary>
        /// The resource instance element
        /// </summary>
        Worker,

        /// <summary>
        /// The resource map element
        /// </summary>
        Workers,

        /// <summary>
        /// The session map element
        /// </summary>
        Session,

        /// <summary>
        /// The virtual host element
        /// </summary>
        Machine,

        /// <summary>
        /// The remote print queue element
        /// </summary>
        RemotePrintQueue,

        /// <summary>
        /// The remote print queue map element
        /// </summary>
        RemotePrintQueues,
        /// <summary>
        /// Badge Box on a printer
        /// </summary>
        BadgeBox,

        /// <summary>
        /// Camera next to printer
        /// </summary>
        Camera
    }
}
