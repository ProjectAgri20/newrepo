namespace HP.ScalableTest.Plugin.DirtyDevice
{
    /// <summary>
    /// A file uploaded via a HTTP Post
    /// </summary>
    public class UploadedFile
    {
        /// <summary>
        /// Recommended filename
        /// </summary>
        public string Filename;
        /// <summary>
        /// Recommended Content Type
        /// </summary>
        public string ContentType;
        /// <summary>
        /// Raw Contents
        /// </summary>
        public byte[] Contents;
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="filename">Recommended filename</param>
        /// <param name="contentType">Recommended Content Type</param>
        /// <param name="contents">Raw Contents</param>
        public UploadedFile(string filename, string contentType, byte[] contents)
        {
            Filename = filename;
            ContentType = contentType;
            Contents = contents;
        }
    }
}
