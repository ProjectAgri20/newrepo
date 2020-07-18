using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.Classes
{
    /// <summary>
    /// File Settings for job/quickset configuration
    /// </summary>
    public class FileSettings
    {
        /// <summary>
        /// File Name Setting.
        /// </summary>
        public DataPair<string> FileName { get; set; }
        /// <summary>
        /// File Name Prefix Setting.
        /// </summary>
        public DataPair<string> FileNamePrefix { get; set; }
        /// <summary>
        /// File Name Suffix Setting
        /// </summary>
        public DataPair<string> FileNameSuffix { get; set; }
        /// <summary>
        /// File Name Setting
        /// </summary>
        public DataPair<string> FileType { get; set; }
        /// <summary>
        /// Resolution Setting.
        /// </summary>
        public DataPair<string> Resolution { get; set; }
        /// <summary>
        /// File Size Setting.
        /// </summary>
        public DataPair<string> FileSize { get; set; }
        /// <summary>
        /// File Color Setting.
        /// </summary>
        public DataPair<string> FileColor { get; set; }
        /// <summary>
        /// File Numbering Setting.
        /// </summary>
        public DataPair<string> FileNumbering { get; set; }
        /// <summary>
        /// Creates instance of FileSettings Class.
        /// </summary>
        public FileSettings()
        {
            FileName = new DataPair<string> {Key = string.Empty};
            FileNamePrefix = new DataPair<string> { Key = string.Empty };
            FileNameSuffix = new DataPair<string> { Key = string.Empty };
            FileType = new DataPair<string> { Key = string.Empty };
            Resolution = new DataPair<string> { Key = string.Empty };
            FileSize = new DataPair<string> { Key = string.Empty };
            FileColor = new DataPair<string> { Key = string.Empty };
            FileNumbering = new DataPair<string> { Key = string.Empty };
        }
    }
}
