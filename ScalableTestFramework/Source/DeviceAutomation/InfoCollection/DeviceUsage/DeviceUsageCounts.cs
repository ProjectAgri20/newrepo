namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceUsage
{
    /// <summary>
    /// Contains data about device usage, such as the number of images printed or scanned.
    /// </summary>
    public sealed class DeviceUsageCounts
    {
        /// <summary>
        /// Gets or sets the total number of images printed.
        /// </summary>
        /// <value>The total number of images printed.</value>
        public int PrintTotalImages { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total number of images scanned from the flatbed.
        /// </summary>
        /// <value>The total number of images scanned from the flatbed.</value>
        public int FlatbedTotalImages { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total number of images scanned from the ADF.
        /// </summary>
        /// <value>The total number of images scanned from the ADF.</value>
        public int AdfTotalImages { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total mono page count.
        /// </summary>
        /// <value>The total mono page count.</value>
        public int TotalMonoPageCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total color page count.
        /// </summary>
        /// <value>The total color page count.</value>
        public int TotalColorPageCount { get; set; } = 0;

        /// <summary>
        /// Gets the total number of images scanned from either the flatbed or the ADF.
        /// </summary>
        public int ScanTotalImages => FlatbedTotalImages + AdfTotalImages;

        /// <summary>
        /// Gets the total usage count, e.g. the total number of images scanned and printed.
        /// </summary>
        public int TotalImageCount => PrintTotalImages + ScanTotalImages;

        /// <summary>
        /// Gets or sets the total fax count.
        /// </summary>
        /// <value>The total fax count.</value>
        public int TotalFaxCount { get; set; } = 0;
    }
}
