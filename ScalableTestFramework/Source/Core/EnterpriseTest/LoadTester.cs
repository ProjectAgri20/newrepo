namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Configuration data for a Load Tester virtual resource.
    /// </summary>
    public class LoadTester : VirtualResource
    {
        /// <summary>
        /// Gets or sets the number of threads to execute per VM.
        /// </summary>
        public int ThreadsPerVM { get; set; }
    }
}
