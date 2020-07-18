using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Dispatcher;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Defines basic interface for a control that displays status about a session map element.
    /// This class should not be instantiated directly.
    /// </summary>
    /// <remarks>
    /// This class cannot be marked abstract or the designer will not work with derived classes.
    /// </remarks>
    public class SessionStatusControlBase : UserControl
    {
        /// <summary>
        /// Initializes this instance with the specified <see cref="SessionMapElement"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <exception cref="ControlTypeMismatchException">
        /// Thrown when an object of incorrect type is passed to this instance.
        /// </exception>
        public virtual void Initialize(SessionMapElement element, ControlSessionExecution control)
        {
            throw new NotImplementedException();
        }
    }
}
