using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// The default implementation of <see cref="T:BindingList`1"/> does not support sorting.
    /// This class fixes that problem. However it does not support filtering.
    /// </summary>
    /// <typeparam name="T">Type of object that will be stored</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool _sorted = false;
        private PropertyDescriptor _sortProperty = null;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableBindingList&lt;T&gt;"/> class.
        /// </summary>
        public SortableBindingList()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableBindingList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public SortableBindingList(IList<T> list)
            : base(list)
        {
        }

        /// <summary>
        /// Returns whether this object supports Sorting
        /// </summary>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        /// Whether the collection has been sorted.
        /// </summary>
        protected override bool IsSortedCore
        {
            get { return _sorted; }
        }

        /// <summary>
        /// Direction in which sorting was last performed (Ascending/Descending).
        /// </summary>
        protected override ListSortDirection SortDirectionCore
        {
            get { return _sortDirection; }
        }

        /// <summary>
        /// Property on which the sort has been last performed
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return _sortProperty; }
        }

        /// <summary>
        /// Sort the collection.
        /// </summary>
        /// <param name="prop">Property on which the sort will be performed.</param>
        /// <param name="direction">The direction in which the collection should be sorted.</param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            // Get list to sort
            List<T> items = this.Items as List<T>;

            // Apply and set the sort
            if (items != null)
            {
                PropertyComparer<T> pc = new PropertyComparer<T>(prop, direction);
                items.Sort(pc);
                _sorted = true;

                _sortProperty = prop;
                _sortDirection = direction;

                // Let bound controls know they should refresh their views
                //OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
            else
            {
                _sorted = false;
            }
        }

        /// <summary>
        /// Removes the sort core.
        /// </summary>
        protected override void RemoveSortCore()
        {
            throw new NotSupportedException();
            //_Sorted = false;
            //this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
    }
}