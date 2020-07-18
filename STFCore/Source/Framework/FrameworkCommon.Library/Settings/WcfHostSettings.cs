using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Settings
{
    /// <summary>
    /// A collection of WCF host settings.
    /// </summary>
    [DataContract]
    public class WcfHostSettings : SettingsCollectionBase
    {
        /// <summary>
        /// Gets the host address of the specified WCF service.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <returns></returns>
        public string this[string serviceName] => GetSetting(serviceName);

        /// <summary>
        /// Gets the host address of the specified WCF service.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <returns></returns>
        public string this[WcfService serviceName] => GetSetting(serviceName.ToString());

        /// <summary>
        /// Adds the specified service address.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <param name="address">The address.</param>
        public void Add(string serviceName, string address)
        {
            this.AddValue(serviceName, address);
        }

        /// <summary>
        /// Adds the specified service address.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <param name="address">The address.</param>
        public void Add(WcfService serviceName, string address)
        {
            this.Add(serviceName.ToString(), address);
        }
    }
}
