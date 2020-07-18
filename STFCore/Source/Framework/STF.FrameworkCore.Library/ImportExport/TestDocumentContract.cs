using System.ComponentModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data contract for TestDocument (used for import/export).
    /// </summary>
    [DataContract(Name = "TestDocument", Namespace = "")]
    public class TestDocumentContract : INotifyPropertyChanged
    {
        private bool _resolved = false;

        [DataMember]
        private string _original = string.Empty;

        [DataMember]
        private string _replacement = string.Empty;

        /// <summary>
        /// Notification when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public TestDocumentContract()
        {
            _original = string.Empty;
            _replacement = string.Empty;
            _resolved = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestDocumentContract(string original)
            : this()
        {
            _original = original;
        }

        /// <summary>
        /// Whether Resolution is required.
        /// </summary>
        public bool ResolutionRequired { get; set; }

        /// <summary>
        /// Whether a conflict has been resolved.
        /// </summary>
        public bool Resolved
        {
            get { return _resolved; }
            set            
            {
                _resolved = value;
                NotifyPropertyChanged("Resolved");
            }
        }

        /// <summary>
        /// The Original document filePath.
        /// </summary>
        public string Original
        {
            get { return _original; }
            set
            {
                _original = value;
                NotifyPropertyChanged("Original");
            }
        }

        /// <summary>
        /// The Replacement document filePath.
        /// </summary>
        public string Replacement
        {
            get { return _replacement; }
            set
            {
                _replacement = value;
                NotifyPropertyChanged("Replacement");
            }
        }

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
