using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;

namespace Sandbox
{
#pragma warning disable 0162

    internal class Program
    {
        static Program()
        {
            Logger.Initialize(new SystemTraceLogger(typeof(Logger)));
        }

        internal static void Main(string[] args)
        {
            Logger.LogDebug("Test");
        }
    }

#pragma warning restore
}
