using System;
using System.ComponentModel;
using System.Linq;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    internal class LoadTesterConfiguration : INotifyPropertyChanged
    {
        public VirtualResourceMetadata Metadata { get; set; }
        public LoadTesterExecutionPlan ExecutionPlan { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Enabled
        {
            get
            {
                if (Metadata != null)
                {
                    return Metadata.Enabled;
                }

                return false;
            }

            set
            {
                if (Metadata != null)
                {
                    Metadata.Enabled = value;
                    OnPropertyChanged("Enabled");
                }
            }
        }

        public string MetadataType
        {
            get
            {
                if (Metadata != null)
                {
                    return Metadata.MetadataType;
                }

                return string.Empty;
            }

            set {}
        }

        public string Name 
        {
            get
            {
                if (Metadata != null)
                {
                    return Metadata.Name;
                }

                return string.Empty;
            }

            set {}
        }

        public int ThreadCount
        {
            get
            {
                if (ExecutionPlan != null)
                {
                    return ExecutionPlan.ThreadCount;
                }

                return 1;
            }

            set
            {
                if (ExecutionPlan != null)
                {
                    ExecutionPlan.ThreadCount = value;
                    OnPropertyChanged("ThreadCount");
                }
            }
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
