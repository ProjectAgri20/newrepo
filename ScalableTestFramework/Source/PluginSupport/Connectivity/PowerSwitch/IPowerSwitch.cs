namespace HP.ScalableTest.PluginSupport.Connectivity.PowerSwitch
{
    /// <summary>
    /// Interface for different functionalities available for power Switch
    /// </summary>
    public interface IPowerSwitch
    {
        /// <summary>
        /// Power off the specified port
        /// </summary>
        /// <param name="portNumber">The port number</param>
        /// <returns>True if the operation is successful.</returns>
        bool PowerOff(int portNumber);

        /// <summary>
        /// Power on the specified port
        /// </summary>
        /// <param name="portNumber">The port number</param>
        /// <returns>True if the operation is successful.</returns>
        bool PowerOn(int portNumber);
    }
}
