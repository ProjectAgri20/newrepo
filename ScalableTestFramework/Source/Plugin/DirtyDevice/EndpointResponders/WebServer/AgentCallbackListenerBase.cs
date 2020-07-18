using System;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    /// <summary>
    /// Base class for listeners and callbacks
    /// </summary>
    public class AgentCallbackListenerBase
    {
        /// <summary>
        /// Endpoint address using IP
        /// </summary>
        public virtual string EndpointAddress { get; protected set; }

        /// <summary>
        /// Returns an EndpointAddress with the maximum allowable size.
        /// </summary>
        /// <param name="endpointAddition">String to append to the end of the EndpointAddress</param>
        /// <returns>Max length endpoint</returns>
        protected string GetMaxEndpoint(string endpointAddition)
        {
            const int MaxEndpointSize = 256;
            int padSize = MaxEndpointSize - EndpointAddress.Length - endpointAddition.Length;
            endpointAddition = String.Empty.PadRight(padSize, 'a') + endpointAddition;
            return EndpointAddress + endpointAddition;
        }
    }
}
