namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// The overall "health" of a managed virtual component.
    /// </summary>
    public enum VirtualComponentStatus
    {
        /// <summary>
        /// The status is unknown.
        /// </summary>
        Gray,

        /// <summary>
        /// The component is OK.
        /// </summary>
        Green,

        /// <summary>
        /// The component might have a problem.
        /// </summary>
        Yellow,

        /// <summary>
        /// The component definitely has a problem.
        /// </summary>
        Red,
    }
}
