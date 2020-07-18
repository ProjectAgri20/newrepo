namespace HP.ScalableTest.Plugin.Copy
{
    /// <summary>
    /// Copy Preference Data
    /// </summary>
    public class CopyPreferences
    {
        /// <summary>
        /// # of Copies
        /// </summary>
        public int Copies { get; set; }
        /// <summary>
        /// Collate Option
        /// </summary>
        public bool Collate { get; set; }

        /// <summary>
        /// Edge to Edge option
        /// </summary>
        public bool EdgeToEdge { get; set; }

        /// <summary>
        /// Orientation - Portrait or Landscape
        /// </summary>
        public string Orientation { get; set; }

        /// <summary>
        /// Sets the Colour option to Color, Monochrome, GrayScale, AutoDetect
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Sets the size for Reduce/Enlarge option
        /// </summary>
        public int ZoomSize { get; set; }

        /// <summary>
        /// Selects the automatic/Manual option
        /// </summary>
        public bool ReduceEnlargeOptions { get; set; }

        /// <summary>
        /// Selects include margin
        /// </summary>
        public bool IncludeMargin { get; set; }

        /// <summary>
        /// Selects Optimize Text or Picture Options
        /// </summary>
        public string OptimizeTextPicOptions { get; set; }

        /// <summary>
        /// Creates new Copy Preferences Options
        /// </summary>
        public CopyPreferences()
        {
            Collate = true;
            EdgeToEdge = false;
            Orientation = "Portrait";
            Color = "Automatically detect";
            Copies = 1;
            OptimizeTextPicOptions = "Photograph";
            ReduceEnlargeOptions = true;
        }
    }
}
