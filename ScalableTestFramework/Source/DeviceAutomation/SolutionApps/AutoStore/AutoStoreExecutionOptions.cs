namespace HP.ScalableTest.DeviceAutomation.SolutionApps.AutoStore
{
    /// <summary>
    /// Maintains the various AutoStore execution options. Inherits from ScanExecutionOptions
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.ScanExecutionOptions" />
    public class AutoStoreExecutionOptions
    {
        /// <summary>
        /// Gets or sets the AutoStore workflow to be executed.
        /// </summary>
        /// <value>
        /// The automatic store workflow.
        /// </value>
        public string AutoStoreWorkflow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [image preview].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [image preview]; otherwise, <c>false</c>.
        /// </value>
        public bool ImagePreview { get; set; }

        /// <summary>
        /// Gets or sets the number of job build segments to use (if job build is enabled).
        /// </summary>
        /// <value>The number of job build segments.</value>
        public int JobBuildSegments { get; set; }

        /// <summary>
        /// The button titles
        /// </summary>
        public static readonly string[] ButtonTitles = { "Email", "Email (OCR)", "Folder", "Folder (OCR)" };

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoStoreExecutionOptions"/> class.
        /// </summary>
        public AutoStoreExecutionOptions()
        {
            AutoStoreWorkflow = string.Empty;
            ImagePreview = false;
            JobBuildSegments = 1;
        }
        /// <summary>
        /// Sets the AutoStore workflow based on the given parameters.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="useOcr">if set to <c>true</c> [use OCR].</param>
        public void SetAutoStoreWorkflow(string workflow, bool useOcr)
        {
            if (workflow.Contains("Email"))
            {
                AutoStoreWorkflow = (useOcr == false) ? ButtonTitles[0] : ButtonTitles[1];
            }
            else if (workflow.Contains("Folder"))
            {
                AutoStoreWorkflow = (useOcr == false) ? ButtonTitles[2] : ButtonTitles[3];
            }
        }

    }

}
