
using System.ComponentModel;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Options for retaining a digital send output file.
    /// </summary>
    public enum RetentionOption
    {
        /// <summary>
        /// Do not move or delete the file
        /// </summary>
        DoNothing,

        /// <summary>
        /// Always retain the file
        /// </summary>
        [Description("Always retain files")]
        AlwaysRetain,

        /// <summary>
        /// Retain the file if it is corrupt (invalid for file type)
        /// </summary>
        [Description("Retain corrupt files")]
        RetainIfCorrupt,

        /// <summary>
        /// Never retain the file
        /// </summary>
        [Description("Never retain files")]
        NeverRetain
    }
}
