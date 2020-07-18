using System;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Factory class for creating session status controls.
    /// </summary>
    public static class SessionStatusControlFactory
    {
        /// <summary>
        /// Creates a session status control for the specified <see cref="SessionMapElement"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static SessionStatusControlBase Create(SessionMapElement element)
        {
            if (element == null)
            {
                // No element provided.  Return null.
                return null;
            }

            switch (element.ElementType)
            {
                default:
                    if (GlobalSettings.IsDistributedSystem)
                    {
                        return new GenericMapElementControl();
                    }
                    else
                    {
                        return new GenericMapElementControlLite();
                    }
            }
        }

        public static T CreateElementInfoControl<T>()
            where T : ElementInfoControlBase, new()
        {
            return new T();
        }
    }
}
