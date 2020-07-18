namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A record specifying a printer and its associated product family.
    /// </summary>
    public sealed class PrinterProduct
    {
        /// <summary>
        /// Gets or sets the product family.
        /// </summary>
        public string Family { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Name { get; set; }
    }
}
