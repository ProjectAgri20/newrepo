using System;

namespace HP.ScalableTest.Print.Utility
{
    internal class InstallStatusItem : Tuple<string, string, string>
    {
        public InstallStatusItem(string dateTime, string duration, string message)
            : base(dateTime, duration, message)
        {
        }
    }
}
