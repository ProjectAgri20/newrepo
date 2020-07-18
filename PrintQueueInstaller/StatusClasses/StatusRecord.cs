using System;

namespace HP.ScalableTest.Print.Utility
{
    internal class StatusRecord
    {
        private string _label = "NONE";
        private DateTime _start = DateTime.Now;
        private DateTime _end = DateTime.Now;
        private InstallStatusData _status = null;

        public StatusRecord(InstallStatusData status)
        {
            _status = status;
        }

        public void Post(string message)
        {
            _status.Record(message);
        }

        public InstallStatusData Status
        {
            get { return _status; }
        }

        public void Start(string label)
        {
            _label = label;
            _start = DateTime.Now;
            _status.Record("{0} START".FormatWith(_label), out _start);
        }

        public void End()
        {
            _end = DateTime.Now;
            _status.Record("{0} END".FormatWith(_label), out _end);
            _status.Record("{0} TOTAL".FormatWith(_label), _end.Subtract(_start));
        }
    }
}
