namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Interface for the triage collection when a STF activity fails.
    /// </summary>
    public interface ITriage
    {
        /// <summary>
        /// Collects the triage data.
        /// </summary>
        void CollectTriageData();

        /// <summary>
        /// Collects the triage data.
        /// </summary>
        /// <param name="reason">reason why data is collected.</param>
        void CollectTriageData(string reason);

        /// <summary>
        /// Gets the current control ids.
        /// </summary>
        void GetCurrentControlIds();

        /// <summary>
        /// Gets the device warnings.
        /// </summary>
        /// <returns>string</returns>
        string GetDeviceWarnings();

        /// <summary>
        /// Gets the control panel image.
        /// </summary>
        void GetControlPanelImage();
        /// <summary>
        /// Submits this instance.
        /// </summary>
        void Submit();

    }
}
