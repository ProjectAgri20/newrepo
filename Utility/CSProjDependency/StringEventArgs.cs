using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.ScalableTest.Utility.VisualStudio
{
    public class StringEventArgs : EventArgs
    {
        public string Data { set; get; }

        public StringEventArgs(string data)
        {
            Data = data;
        }
    }
}
