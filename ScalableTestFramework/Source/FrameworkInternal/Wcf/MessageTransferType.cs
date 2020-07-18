namespace HP.ScalableTest.Framework.Wcf
{
    /// <summary>
    /// Message transfer types that can be used for WCF communications.
    /// </summary>
    public enum MessageTransferType
    {
        /// <summary>
        /// Messages are transferred via HTTP.
        /// </summary>
        Http,

        /// <summary>
        /// Messages are compressed prior to transfer via HTTP.
        /// </summary>
        CompressedHttp,

        /// <summary>
        /// Messages are sent via local-only named pipes.
        /// </summary>
        NamedPipe
    }
}
