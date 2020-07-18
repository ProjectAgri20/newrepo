using System.Collections.Generic;
using System.Text;
using System;

namespace HP.ScalableTest.Print.Utility
{
    internal class PrinterInstallStatus : Dictionary<string, InstallStatusData>
    {
        private QueueInstallationData _queueInstallData = null;
        private Dictionary<string, InstallStatusData> _queueInstallStatus = new Dictionary<string, InstallStatusData>();
        private static object _lock = new object();

        public void Reset()
        {
            this.Clear();
            _queueInstallStatus.Clear();
            _queueInstallData = null;
        }

        public InstallStatusData Create(QueueInstallationData queueData)
        {
            string key = queueData.QueueName;

            lock (_lock)
            {
                if (!ContainsKey(key))
                {
                    _queueInstallData = queueData;

                    InstallStatusData statusData = new InstallStatusData();

                    base.Add(key, statusData);
                }
            }

            return this[key];
        }

        public InstallStatusData Create(string driverModel)
        {
            string key = driverModel;

            lock (_lock)
            {
                if (!ContainsKey(key))
                {
                    _queueInstallData = new QueueInstallationData();

                    InstallStatusData statusData = new InstallStatusData();

                    base.Add(key, statusData);
                }
            }
            return this[key];
        }

        public QueueInstallationData QueueInstallData
        {
            get { return _queueInstallData; }
            set { _queueInstallData = value; }
        }

        public IEnumerable<InstallStatusData> Queues
        {
            get { return _queueInstallStatus.Values; }
        }

        public override string ToString()
        {
            //string installStart = DateTime.MinValue.ToString(_dateTimeFormat);
            //string installEnd = DateTime.MinValue.ToString(_dateTimeFormat);

            StringBuilder builder = new StringBuilder();
            foreach (string key in this.Keys)
            {
                //builder.Append("{0} ({1})".FormatWith(key, _installStatus.QueueInstallData.Address));
                builder.Append(Environment.NewLine);
                foreach (InstallStatusItem statusData in this[key].Items)
                {
                    builder.Append("{0,23} | {1,8} | {2,40} | {3}".FormatWith
                        (
                            statusData.Item1,
                            statusData.Item2,
                            key,
                            statusData.Item3
                        ));
                    builder.Append(Environment.NewLine);
                }
            }

            return builder.ToString();
        }

    }
}
