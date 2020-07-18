using System;
using System.Collections.Generic;
using System.Globalization;

namespace HP.ScalableTest.Print.Utility
{
    internal class InstallStatusData
    {
        private List<InstallStatusItem> _statusData = new List<InstallStatusItem>();
        private const string DTFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private const string DurationFormat = "{0:D2}:{1:D2}.{2:D3}";

        public bool Installed { get; set; }

        public InstallStatusData()
        {
            Installed = false;
        }

        public void Record(string message, DateTime dateTime)
        {
            _statusData.Add(new InstallStatusItem
                (
                    dateTime.ToString(DTFormat, CultureInfo.CurrentCulture),
                    DurationFormat.FormatWith(0, 0, 0),
                    message
                ));
        }

        public void Record(string message)
        {
            DateTime now = DateTime.Now;
            Record(message, out now);
        }

        public void Record(string message, out DateTime now)
        {
            now = DateTime.Now;
            _statusData.Add(new InstallStatusItem
                (
                    now.ToString(DTFormat, CultureInfo.CurrentCulture),
                    DurationFormat.FormatWith(0, 0, 0),
                    message
                ));
        }

        public void Record(string message, TimeSpan time)
        {
            // Reset to zero if there is currently a negative value.
            if (time.Ticks < 0)
            {
                time = new TimeSpan();
            }

            _statusData.Add(new InstallStatusItem
                (
                    DateTime.Now.ToString(DTFormat, CultureInfo.CurrentCulture),
                    DurationFormat.FormatWith(time.Minutes, time.Seconds, time.Milliseconds),
                    message
                ));
        }

        public List<InstallStatusItem> Items
        {
            get { return _statusData; }
        }
    }

}
