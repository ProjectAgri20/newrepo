namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Dss
{
    /// <summary>
    /// Manages advanced job options for an <see cref="IDssWorkflowApp" />.
    /// </summary>
    public interface IDssWorkflowJobOptions
    {
        /// <summary>
        /// Sets the state of the Job Build option.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> enable Job Build; otherwise, disable it.</param>
        void SetJobBuildState(bool enable);
    }
}