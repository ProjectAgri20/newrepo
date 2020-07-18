namespace HP.ScalableTest.Plugin.DirtyDevice
{
    /// <summary>
    /// A custom page hosted by the HttpListenerServerSim
    /// </summary>
    public class CustomServerPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name (uri) of the page></param>
        /// <param name="contentType">Content type of the page (e.g. text/html)</param>
        /// <param name="content">Page content</param>
        public CustomServerPage(string name, string contentType, byte[] content)
        {
            Name = name;
            ContentType = contentType;
            Content = content;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name (uri) of the page></param>
        /// <param name="contentType">Content type of the page (e.g. text/html)</param>
        /// <param name="pathToContent">file path to content</param>
        public CustomServerPage(string name, string contentType, string pathToContent)
        {
            Name = name;
            ContentType = contentType;
            FilePath = pathToContent;
        }

        /// <summary>
        /// Name (uri) of the page
        /// </summary>
        public string Name;

        /// <summary>
        /// Content type of the page (e.g. text/html)
        /// </summary>
        public string ContentType;

        /// <summary>
        /// Page content
        /// </summary>
        public byte[] Content;

        /// <summary>
        /// File path to content
        /// </summary>
        public string FilePath;
    }
}
