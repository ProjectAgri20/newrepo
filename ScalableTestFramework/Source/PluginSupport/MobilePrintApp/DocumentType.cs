using System.ComponentModel;

namespace HP.ScalableTest.PluginSupport.MobilePrintApp
{
    /// <summary>
    /// Document type to print on Mobile print app
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// File (internal stroage)
        /// </summary>
        [Description("File")]
        File,

        /// <summary>
        /// Photo from Gallery
        /// </summary>
        [Description("Photo")]
        Photo,

        /// <summary>
        /// Camera with Camera app
        /// </summary>
        [Description("Camera")]
        Camera,

        /// <summary>
        /// Web page
        /// </summary>
        [Description("Web Page")]
        Web,


        /// <summary>
        /// Email
        /// </summary>
        [Description("Email")]
        Email

    }
}
