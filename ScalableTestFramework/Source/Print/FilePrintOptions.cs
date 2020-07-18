namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Specifies options to use when printing a file using a <see cref="FilePrinter" />.
    /// </summary>
    public sealed class FilePrintOptions
    {
        /// <summary>
        /// Gets or sets the number of copies to print.
        /// The default value is 1.
        /// </summary>
        public int Copies { get; set; } = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePrintOptions" /> class.
        /// </summary>
        public FilePrintOptions()
        {
            // Constructor explicitly declared for XML doc.
        }
    }
}
