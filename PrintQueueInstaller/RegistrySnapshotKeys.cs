using System.Collections.ObjectModel;

namespace HP.ScalableTest.Print.Utility
{
    public class RegistrySnapshotKeys
    {
        private Collection<RegistrySnapshotKey> _registryKeys = null;
        private RegistrySnapshotState _state = RegistrySnapshotState.None;

        public RegistrySnapshotKeys(RegistrySnapshotState state)
        {
            _state = state;
            _registryKeys = new Collection<RegistrySnapshotKey>();
        }

        public Collection<RegistrySnapshotKey> Entries
        {
            get { return _registryKeys; }
        }

        public RegistrySnapshotState State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}
