using System;
using System.Linq;

namespace HP.SolutionTest.Install
{
    internal class ProgressEventArgs : EventArgs
    {
        public ProgressState State { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }
    }
}
