using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.UI.Charting
{
    internal class SubstringFilter
    {
        public string Substring { get; set; }

        public SubstringFilter(string substring)
        {
            Substring = substring;
        }
    }
}
