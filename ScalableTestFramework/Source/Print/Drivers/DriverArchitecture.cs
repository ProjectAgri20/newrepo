namespace HP.ScalableTest.Print.Drivers
{
    /// <summary>
    /// Represents the processor architecture of a print driver.
    /// </summary>
    /// <remarks>
    /// The spelling of these enumerated values were chosen based off of the supported values found in an INF file.
    /// </remarks>
    public enum DriverArchitecture
    {
        /// <summary>
        /// x86 (32-bit)
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        NTx86,

        /// <summary>
        /// AMD64 (64-bit)
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        NTAMD64
    }
}
