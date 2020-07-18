using System;

namespace HP.ScalableTest.Plugin.SafeComSimulation
{
    public class SafeComSessionException : Exception
    {
        public SafeComSessionException() : base()
        {
        }

        public SafeComSessionException(string message) : base(message)
        {
        }

        public SafeComSessionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
