namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Defines the interface used by any class that is part of the overall session map
    /// </summary>
    public interface ISessionMapElement
    {
        /// <summary>
        /// Gets the session map element defined in the class inheriting this interface.
        /// </summary>
        /// <value>
        /// The element.
        /// </value>
        SessionMapElement MapElement { get; }
    }
}