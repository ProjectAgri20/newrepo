using System.Linq;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.Framework.Dispatcher;

namespace HP.ScalableTest.Framework
{
    public class StfLiteCentralHub
    {
        private static readonly SessionDispatcher _dispatcher = new SessionDispatcher();
        private static readonly VirtualClientControllerLite _controller = new VirtualClientControllerLite();

        StfLiteCentralHub()
        {
        }

        public static SessionDispatcher Dispatcher
        {
            get { return _dispatcher; }
        }

        public static VirtualClientControllerLite Controller
        {
            get { return _controller; }
        }
    }
}