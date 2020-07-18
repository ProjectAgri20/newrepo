namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// A security package used by SSPI.
    /// </summary>
    public enum SecurityPackage
    {
        /// <summary>
        /// The Negotiate security package.
        /// </summary>
        Negotiate,

        /// <summary>
        /// The Kerberos security package.
        /// </summary>
        Kerberos,

        /// <summary>
        /// The NTLM security package.
        /// </summary>
        Ntlm
    }
}
