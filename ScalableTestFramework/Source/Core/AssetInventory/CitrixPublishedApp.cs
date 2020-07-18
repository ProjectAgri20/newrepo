namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A published application on a Citrix server.
    /// </summary>
    public sealed class CitrixPublishedApp
    {
        /// <summary>
        /// Gets or sets the Citrix server hosting this application.
        /// </summary>
        public string CitrixServer { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        public string ApplicationName { get; set; }
    }
}
