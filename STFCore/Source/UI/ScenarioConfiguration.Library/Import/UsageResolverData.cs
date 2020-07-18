using System.ComponentModel;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public class UsageResolverData : INotifyPropertyChanged
    {
        private bool _resolved = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public UsageResolverData(ResourceUsageAgent<ResourceUsageBase> agent)
        {
            Agent = agent;
            _resolved = agent.ResolutionCompleted;
        }

        public ResourceUsageAgent<ResourceUsageBase> Agent { get; private set; }

        public bool Resolved
        {
            get { return _resolved; }
            set
            {
                _resolved = value;
                NotifyPropertyChanged("Resolved");
            }
        }

        public ResourceUsageType Type
        {
            get { return Agent.UsageType; }
            set {}
        }

        public string Original
        {
            get { return Agent.ExportData.Name; }
            set {}
        }

        public string Replacement
        {
            get { return Agent.ImportData.Name; }
            set
            {
                Agent.ImportData.Name = value;
                NotifyPropertyChanged("ImportName");
            }
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
