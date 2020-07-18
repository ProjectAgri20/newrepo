namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Defines the location where a print job is rendered.
    /// </summary>
    public enum PrintJobRenderLocation
    {
        /// <summary>
        /// Print jobs are rendered on the client.
        /// </summary>
        Client = 0,

        /// <summary>
        /// Print jobs are rendered on the server.
        /// </summary>
        Server = 1,

        /// <summary>
        /// Unknown where the job is being rendered.
        /// </summary>
        Unknown = 2
    }
}
