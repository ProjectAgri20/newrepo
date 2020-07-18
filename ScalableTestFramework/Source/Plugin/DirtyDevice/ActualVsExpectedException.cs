using System;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class ActualVsExpectedException : Exception
    {
        public ActualVsExpectedException(string message) : base(message)
        {
        }

        public ActualVsExpectedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
