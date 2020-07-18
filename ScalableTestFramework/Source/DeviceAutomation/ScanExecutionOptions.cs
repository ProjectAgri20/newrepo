namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Options to use when executing a scan job on a device.
    /// </summary>
    public class ScanExecutionOptions
    {
        /// <summary>
        /// Gets or sets the number of job build segments to use (if job build is enabled).
        /// </summary>
        public int JobBuildSegments { get; set; } = 1;

        /// <summary>
        /// Gets or sets the image preview requirement for the scan execution.
        /// </summary>
        public ImagePreviewOption ImagePreview { get; set; } = ImagePreviewOption.Optional;

        /// <summary>
        /// Gets or sets a value indicating whether to monitor the active job to verify that the job completes.
        /// </summary>
        public bool ValidateJobExecution { get; set; } = true;
    }

    /// <summary>
    /// Options for the way automation will utilize image preview as part of a scan.
    /// </summary>
    public enum ImagePreviewOption
    {
        /// <summary>
        /// Image preview is optional; automation will use image preview if the device presents
        /// the option, but will still proceed if image preview is disabled.
        /// </summary>
        Optional = 0,

        /// <summary>
        /// Image preview is required; automation will fail if the device is configured with image preview disabled.
        /// </summary>
        GeneratePreview = 1,

        /// <summary>
        /// Image preview cannot be used; automation will fail if the device is configured to require image preview.
        /// </summary>
        RestrictPreview = 2
    }
}
