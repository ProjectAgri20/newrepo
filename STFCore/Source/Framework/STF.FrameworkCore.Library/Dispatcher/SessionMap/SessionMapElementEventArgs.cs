using System;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Event args class for changes to a session map element
    /// </summary>
    public class SessionMapElementEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the map element for this change event.
        /// </summary>
        public SessionMapElement MapElement { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionMapElementEventArgs"/> class.
        /// </summary>
        /// <param name="mapElement">The element.</param>
        public SessionMapElementEventArgs(SessionMapElement mapElement)
        {
            MapElement = mapElement;
        }
    }
}