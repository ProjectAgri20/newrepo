using HP.ScalableTest.Framework.Dispatcher;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Generic Map element control, which displays the status for virtual resource, session, etc.,
    /// but does not display realtime trace log data.
    /// </summary>
    public partial class GenericMapElementControlLite : SessionStatusControlBase
    {
        public GenericMapElementControlLite()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="sessionControl">The session control.</param>
        public override void Initialize(SessionMapElement element, ControlSessionExecution sessionControl)
        {
            elementInfoCompositeControl.Initialize(element, sessionControl);
        }
    }
}
