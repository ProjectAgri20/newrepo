using System;
using System.Text;
using System.ComponentModel;
using System.Linq;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    internal class WorkerActivityConfiguration : INotifyPropertyChanged
    {
        public WorkerActivityConfiguration(VirtualResourceMetadata metadata, WorkerExecutionPlan plan)
        {
            Metadata = metadata;
            ExecutionPlan = plan;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public VirtualResourceMetadata Metadata { get; private set; }
        public WorkerExecutionPlan ExecutionPlan { get; private set; }

        public bool Enabled
        {
            get { return Metadata.Enabled; }
            set
            {
                Metadata.Enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        public string MetadataType
        {
            get { return Metadata.MetadataType; }
            set {}
        }

        public string Name 
        {
            get { return Metadata.Name; }
            set {}
        }

        public int Order
        {
            get { return ExecutionPlan.Order; }

            set
            {
                ExecutionPlan.Order = value;
                OnPropertyChanged("Order");
            }
        }

        public ExecutionMode Mode
        {
            get { return ExecutionPlan.Mode; }

            set
            {
                ExecutionPlan.Mode = value;
                OnPropertyChanged("Mode");
            }
        }

        public string Value
        {
            get
            {
                if (ExecutionPlan.Mode == ExecutionMode.Iteration)
                {
                    return ExecutionPlan.Value.ToString();
                }
                else
                {
                    return TimeSpanFormat.ToTimeSpanString(ExecutionPlan.Value);
                }
            }

            set 
            {
                if (ExecutionPlan.Mode == ExecutionMode.Iteration)
                {
                    ExecutionPlan.Value = int.Parse(value);
                }
                else
                {
                    ExecutionPlan.Value = (int)TimeSpanFormat.Parse(value).TotalMinutes;
                }

                OnPropertyChanged("Value");
            }
        }

        public string Pacing
        {
            get
            {
                return ExecutionPlan.ActivityPacing.ToString();
            }
            set { }
        }

        public string RetryHandling
        {
            get
            {
                if (Metadata.VirtualResourceMetadataRetrySettings == null || !Metadata.VirtualResourceMetadataRetrySettings.Any())
                {
                    return "None";
                }

                StringBuilder builder = new StringBuilder();
                Metadata.VirtualResourceMetadataRetrySettings.Where(x => x.Action != PluginRetryAction.Continue.ToString()).OrderBy(x => x.State).ToList().ForEach(x => builder.AppendLine(string.Format("{0} -> {1}", x.State, x.Action)));
                return builder.ToString();
            }

            set { }
        }

        public void RemoveAllRetrySettings()
        {
            if (Metadata != null && Metadata.VirtualResourceMetadataRetrySettings != null)
            {
                Metadata.VirtualResourceMetadataRetrySettings.ToList().ForEach(x => x.VirtualResourceMetadataId = Guid.Empty);
                Metadata.VirtualResourceMetadataRetrySettings.Clear();
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
