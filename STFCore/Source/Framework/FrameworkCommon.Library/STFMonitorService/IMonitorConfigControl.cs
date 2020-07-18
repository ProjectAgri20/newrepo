using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Framework.Monitor
{
    public interface IMonitorConfigControl
    {
        /// <summary>
        /// Gets the serialized string of the <see cref="StfMonitorConfig" /> object.
        /// </summary>
        string Configuration { get; }
    }
}
