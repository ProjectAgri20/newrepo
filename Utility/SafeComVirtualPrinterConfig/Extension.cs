using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeComVirtualPrinterConfig
{
    public static class Extension
    {
        public static int IndexOf(this StringBuilder stringBuilder, char searchChar)
        {
            if (stringBuilder == null)
            {
                throw new ArgumentNullException();
            }

            for (int idx = 0; idx != stringBuilder.Length; ++idx)
            {
                if (stringBuilder[idx] == searchChar)
                {
                    return idx;
                }
            }
            return -1;
        }

        public static int LastIndexOf(this StringBuilder stringBuilder, char searchChar)
        {
            if (stringBuilder == null)
            {
                throw new ArgumentNullException();
            }

            for (int idx = stringBuilder.Length - 1; idx >= 0; --idx)
            {
                if (stringBuilder[idx] == searchChar)
                {
                    return idx;
                }
            }
            return -1;
        } 

    }
}
