namespace HP.ScalableTest.PluginSupport.Connectivity.Selenium
{
    /// <summary>
    /// 
    /// </summary>
    public enum BrowserModel
    {
        /// <summary>
        /// Internet Explorer browser with proxy
        /// </summary>
        [EnumValue("*iexploreproxy")]
        ExplorerProxy,

        /// <summary>
        /// Internet Explorer browser
        /// </summary>
        [EnumValue("*iexplore")]
        Explorer,

        /// <summary>
        /// Firefox browser with proxy
        /// </summary>
        [EnumValue("*firefoxproxy")]
        FirefoxProxy,

        /// <summary>
        /// Firefox browser
        /// </summary>
        [EnumValue("*firefox")]
        Firefox,

        /// <summary>
        /// Safari browser with proxy
        /// </summary>
        [EnumValue("*safariproxy")]
        SafariProxy,

        /// <summary>
        /// Safari browser
        /// </summary>
        [EnumValue("*safari")]
        Safari,

        /// <summary>
        /// Google chrome browser
        /// </summary>
        [EnumValue("*googlechrome")]
        Chrome,

        /// <summary>
        /// Opera browser
        /// </summary>
        [EnumValue("*opera")]
        Opera
    }
}
