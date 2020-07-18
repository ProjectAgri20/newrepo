using System;
using System.Text;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Monitor
{
    /// <summary>
    /// Configuration options for validation and retention of Solution Destination files.
    /// </summary>
    [Serializable]
    public class OutputMonitorConfig : StfMonitorConfig
    {
        /// <summary>
        /// Gets or sets the file retention location.
        /// </summary>
        public string RetentionLocation { get; set; }

        /// <summary>
        /// Gets or sets the retention filename. If null, the original filename is preserved.
        /// </summary>
        public string RetentionFileName { get; set; }

        /// <summary>
        /// Gets or sets the file retention option.
        /// </summary>
        public RetentionOption Retention { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to look for a metadata file.
        /// </summary>
        public bool LookForMetadataFile { get; set; }

        /// <summary>
        /// Gets or sets the metadata file extension.
        /// </summary>
        public string MetadataFileExtension { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputMonitorConfig"/> class.
        /// </summary>
        public OutputMonitorConfig() 
            : base()
        {
            RetentionLocation = null;
            RetentionFileName = null;
            Retention = RetentionOption.NeverRetain;
            LookForMetadataFile = false;
            MetadataFileExtension = null;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(".  ");

            switch (Retention)
            {
                case RetentionOption.AlwaysRetain:
                case RetentionOption.RetainIfCorrupt:
                    result.Append(EnumUtil.GetDescription(this.Retention));
                    result.Append(" at ");
                    result.Append(RetentionLocation);
                    break;
                case RetentionOption.NeverRetain:
                    result.Append(EnumUtil.GetDescription(this.Retention));
                    break;
                default: //RetentionOption.DoNothing
                    result.Append("Do not move or retain files.");
                    break;
            }

            return result.ToString();
        }
    }
}
