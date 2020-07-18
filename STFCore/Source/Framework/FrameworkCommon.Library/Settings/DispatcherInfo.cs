
namespace HP.ScalableTest.Framework.Settings
{
    /// <summary>
    /// Defines a dispatcher available in the STF environment.
    /// </summary>
    public class DispatcherInfo
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherInfo"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        public DispatcherInfo(string address)
            : this(address, address)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        public DispatcherInfo(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
