using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Implements the <see cref="IComparer&lt;T&gt;"/> interface to compare property values of a class.
    /// </summary>
    /// <typeparam name="T">The type of property object that will be compared</typeparam>
    public class PropertyComparer<T> : IComparer<T>
    {
        private readonly PropertyDescriptor _property;
        private readonly ListSortDirection _direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="property">The property descriptor.</param>
        /// <param name="direction">The direction for list sorting.</param>
        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            _property = property;
            _direction = direction;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.</returns>
        public int Compare(T x, T y)
        {
            // Get property values
            object xValue = GetPropertyValue(x, _property.Name);
            object yValue = GetPropertyValue(y, _property.Name);

            // Determine sort order
            if (_direction == ListSortDirection.Ascending)
            {
                return Compare(xValue, yValue);
            }
            else
            {
                return CompareDescending(xValue, yValue);
            }
        }

        /// <summary>
        /// Checks if x is equal to the y
        /// </summary>
        /// <param name="x">The value for x.</param>
        /// <param name="y">The value for y.</param>
        /// <returns><c>true</c> if they are equal, <c>false</c> otherwise.</returns>
        public bool Equals(T x, T y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        private static int Compare(object xValue, object yValue)
        {
            IComparable xValueAsComparable = xValue as IComparable;
            int result;

            // Account for a null value in the sort
            if (xValue == null && yValue == null)
            {
                result = 0;
            }
            else if (xValue == null)
            {
                result = -1;
            }
            else if (yValue == null)
            {
                result = 1;
            }
            // If values implement IComparer
            else if (xValueAsComparable != null)
            {
                result = xValueAsComparable.CompareTo(yValue);
            }
            // If values don't implement IComparer but are equivalent
            else if (xValue.Equals(yValue))
            {
                result = 0;
            }
            // Values don't implement IComparer and are not equivalent, so compare as string values
            else result = string.Compare(xValue.ToString(), yValue.ToString(), StringComparison.Ordinal);

            // Return result
            return result;
        }

        private static int CompareDescending(object xValue, object yValue)
        {
            // Return result adjusted for ascending or descending sort order ie
            // multiplied by 1 for ascending or -1 for descending
            return Compare(xValue, yValue) * -1;
        }

        private static object GetPropertyValue(T value, string property)
        {
            // Get property
            PropertyInfo propertyInfo = value.GetType().GetProperty(property);

            // Return value
            return propertyInfo.GetValue(value, null);
        }
    }
}
