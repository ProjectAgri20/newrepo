using System;
using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    /// <summary>
    /// A request received by the http listener
    /// </summary>
    public class RequestInstance
    {
        /// <summary>
        /// Http Headers
        /// </summary>
        public Dictionary<string, string> Headers;

        /// <summary>
        /// Relative uri
        /// </summary>
        public string Uri;

        /// <summary>
        /// Request Method
        /// </summary>
        public string Method;

        /// <summary>
        /// The GET Query String
        /// </summary>
        public Dictionary<string, string> GetQueryString;

        /// <summary>
        /// The POST Query String, formatted as a dictionary
        /// </summary>
        public Dictionary<string, string> PostQueryString;

        /// <summary>
        /// The POST Query String, no formatting
        /// </summary>
        public string RawPostQueryString;

        /// <summary>
        /// Uploaded Files
        /// </summary>
        public List<UploadedFile> Files;

        /// <summary>
        /// Timestamp of request
        /// </summary>
        public DateTime TimeStamp;
    }
}
