namespace HP.ScalableTest.DeviceAutomation.NativeApps.Email
{
    // TODO: This is probably not the best way to handle the extra info needed for Sirius/Phoenix.

    /// <summary>
    /// Extension to <see cref="IEmailApp" /> that includes an address source.
    /// </summary>
    public interface IEmailAppWithAddressSource : IEmailApp
    {
        /// <summary>
        /// Gets or sets the address source.
        /// </summary>
        /// <value>The address source.</value>
        string AddressSource { get; set; }

        /// <summary>
        /// Gets or sets from address.
        /// </summary>
        /// <value>The from address.</value>
        string FromAddress { get; set; }
    }
}
