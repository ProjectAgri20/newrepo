using Vim25Api;

namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// The result of a <see cref="VSphereClient.WaitForTaskCompletion" /> operation.
    /// </summary>
    public sealed class VSphereTaskResult
    {
        /// <summary>
        /// Gets a value indicating whether the task completed successfully.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets the error message returned by the task, if any.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereTaskResult"/> class.
        /// </summary>
        /// <param name="taskObject">The <see cref="VSphereManagedObject" /> representing the task object.</param>
        internal VSphereTaskResult(VSphereManagedObject taskObject)
        {
            Success = (TaskInfoState)taskObject.Properties["info.state"] == TaskInfoState.success;
            if (taskObject.Properties.ContainsKey("info.error"))
            {
                Error = ((LocalizedMethodFault)taskObject.Properties["info.error"]).localizedMessage;
            }
        }
    }
}
