namespace HP.ScalableTest.DeviceAutomation.DeviceSettings
{
    /// <summary>
    /// Printer media modes that define how the device processes jobs.
    /// </summary>
    public enum JobMediaMode
    {
        /// <summary>
        /// The job media mode is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The device prints jobs to physical media.
        /// </summary>
        Paper,

        /// <summary>
        /// The device processes the job and performs a CRC on the generated image, but prints no paper.
        /// </summary>
        Paperless
    }
}
