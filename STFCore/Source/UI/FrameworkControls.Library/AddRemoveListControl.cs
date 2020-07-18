using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Add remove List control
    /// </summary>
    public partial class AddRemoveListControl : UserControl
    {
        private string _sourceListDisplayMember = string.Empty;
        private string _destinationListDisplayMember = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public AddRemoveListControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moves items from the source list to the destination list.
        /// </summary>
        /// <param name="source">Source listbox</param>
        /// <param name="dest">Destination listbox</param>
        /// <param name="itemsToMove">List of items to move.</param>
        private void MoveFromSourceToDestination(ListBox source, ListBox dest, IList itemsToMove)
        {
            IList selectedItems = new ArrayList();

            IList sourceList = (IList)source.DataSource;
            IList destinationList = (IList)dest.DataSource;

            // First collect the index values in the collection
            foreach (object item in itemsToMove)
            {
                destinationList.Add(item);
                selectedItems.Add(item);
            }

            foreach (object item in selectedItems)
            {
                sourceList.Remove(item);
            }

            source.DataSource = null;
            source.DataSource = sourceList;

            dest.DataSource = null;
            dest.DataSource = destinationList;

            SetSourceListDisplayMember();
            SetDestinationListDisplayMember();
        }

        private void SingleAddButtonClick(object sender, EventArgs e)
        {
            MoveFromSourceToDestination(sourceListBox, destinationListBox, sourceListBox.SelectedItems);
        }

        private void SingleRemoveButtonClick(object sender, EventArgs e)
        {
            MoveFromSourceToDestination(destinationListBox, sourceListBox, destinationListBox.SelectedItems);
        }

        private void AllAddButtonClick(object sender, EventArgs e)
        {
            MoveFromSourceToDestination(sourceListBox, destinationListBox, sourceListBox.Items);
        }

        private void AllRemoveButtonClick(object sender, EventArgs e)
        {
            MoveFromSourceToDestination(destinationListBox, sourceListBox, destinationListBox.Items);
        }

        /// <summary>
        /// 
        /// </summary>
        public string SourceListDisplayMember
        {
            get 
            {
                return _sourceListDisplayMember;  
            }
            set 
            {
                _sourceListDisplayMember = value;
                SetSourceListDisplayMember();
            }
        }

        private void SetSourceListDisplayMember()
        {
            if (!string.IsNullOrEmpty(_sourceListDisplayMember))
            {
                sourceListBox.DisplayMember = _sourceListDisplayMember;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DesinationListDisplayMember
        {
            get
            {
                return _destinationListDisplayMember;
            }
            set
            {
                _destinationListDisplayMember = value;
                SetDestinationListDisplayMember();
            }
        }

        private void SetDestinationListDisplayMember()
        {
            if (!string.IsNullOrEmpty(_destinationListDisplayMember))
            {
                destinationListBox.DisplayMember = _destinationListDisplayMember;
            }
        }

        /// <summary>
        /// Gets or sets the source items list.
        /// </summary>
        /// <remarks>
        /// When you set this property, it acts like a binding.
        /// If the Source Items change, then the object that was passed into this property will also be changed.
        /// </remarks>
        public IList SourceItems
        {
            get
            {
                return (IList)sourceListBox.DataSource;
            }
            
            set
            {
                sourceListBox.DataSource = value;
                RemoveDuplicates();
            }
        }

        /// <summary>
        /// Gets or sets the destination items list.
        /// </summary>
        /// <remarks>
        /// When you set this property, it acts like a binding.
        /// If the Destination Items change, then the object that was passed into this property will also be changed.
        /// </remarks>
        public IList DestinationItems
        {
            get
            {
                return (IList)destinationListBox.DataSource;
            }

            set
            {
                destinationListBox.DataSource = value;
                RemoveDuplicates();
            }
        }

        /// <summary>
        /// Scans both listboxes and removes duplicate entries found in both listboxes.  The duplicate
        /// found will be removed from the source list.
        /// </summary>
        private void RemoveDuplicates()
        {
            IList source =      (IList)sourceListBox.DataSource;
            IList destination = (IList)destinationListBox.DataSource;

            // If either list is null, then there is nothing more to do.
            if (source == null || destination == null)
            {
                return;
            }

            // The generl rule is that any entry duplicate from destination found in source will be removed from the 
            // source.
            // NOTE: The IEquatable interface should be implemented if what's been listed in this control
            // is a class.  This will ensure the correct comparisons are taking place when adding/removing.
            foreach (object destinationItem in destination)
            {
                if (source.Contains(destinationItem))
                {
                    source.Remove(destinationItem);
                }
            }

            // Source is the only thing that has changed, so reset it.
            sourceListBox.DataSource = null;
            sourceListBox.DataSource = source;
            SetSourceListDisplayMember();
        }

        /// <summary>
        /// Gets or sets the label that sits above the Source ListBox
        /// </summary>
        public string SourceLabelText
        {
            get
            {
                return source_Label.Text;
            }
            set
            {
                source_Label.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the label that sits above the Destination ListBox
        /// </summary>
        public string DestinationLabelText
        {
            get
            {
                return destination_Label.Text;
            }
            set
            {
                destination_Label.Text = value;
            }
        }
    }
}
