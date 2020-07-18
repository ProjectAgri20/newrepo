using System;
using System.Linq;

namespace HP.SolutionTest.Install
{
    internal class StatusEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public StatusEventArgs(string message)
        {
            Message = message;
        }
    }
}
