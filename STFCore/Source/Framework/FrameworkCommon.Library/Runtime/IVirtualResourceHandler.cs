using System;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Describes the operations allowed on a Virtual Resource controller
    /// </summary>
    public interface IVirtualResourceHandler
    {
        /// <summary>
        /// Creates the resources.
        /// </summary>
        void Start();
    }
}
