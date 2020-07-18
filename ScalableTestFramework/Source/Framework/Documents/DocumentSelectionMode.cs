namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// Methods that can be used for document selection.
    /// </summary>
    public enum DocumentSelectionMode
    {
        /// <summary>
        /// Select documents explicitly from a given list.
        /// </summary>
        SpecificDocuments,

        /// <summary>
        /// Select documents based on a predefined document set.
        /// </summary>
        DocumentSet,

        /// <summary>
        /// Query for documents based on filter criteria.
        /// </summary>
        DocumentQuery
    }
}
