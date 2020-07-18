using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.ScalableTest.Utility.VisualStudio
{
    public class BoolEventArgs : EventArgs
    {
        public bool Data { set; get; }

        public BoolEventArgs(bool data)
        {
            Data = data;
        }
    }
}
