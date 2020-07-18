using System.ComponentModel;

namespace HP.ScalableTest.PluginSupport.PullPrint
{
    /// <summary>
    /// Legacy Enumeration that is used for converting old VirtualResourceMetadata to a newer version.
    /// </summary>
    public enum PullPrintDocumentProcessActions
    {
        /// <summary>
        /// Prints all documents with probable deleting as well
        /// </summary>
        [Description("Print All Documents")]
        PrintAllDocuments,

        /// <summary>
        /// Prints single document
        /// </summary>
        [Description("Print Single Document")]
        PrintSingleDocument,

        /// <summary>
        /// Deletes all documents
        /// </summary>
        [Description("Delete All Documents")]
        DeleteAllDocuments,

        /// <summary>
        /// Delete a single document
        /// </summary>
        [Description("Delete Single Document")]
        DeleteSingleDocument,

        /// <summary>
        /// Prints and keeps all documents
        /// </summary>
        [Description("Print Keep All Documents")]
        PrintKeepAllDocuments,

        /// <summary>
        /// Prints and keeps single document
        /// </summary>
        [Description("Print Keep Single Document")]
        PrintKeepSingleDocument
    }
}
