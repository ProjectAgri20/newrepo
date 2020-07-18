using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Contains extension methods for .NET framework classes.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Removes all whitespace from the specified <see cref="string" />.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>A new <see cref="string" /> that is identical to <paramref name="value" /> but with all whitespace removed.</returns>
        public static string RemoveWhiteSpace(this string value)
        {
            return Regex.Replace(value, @"\s+", string.Empty);
        }

        /// <summary>
        /// Determines whether two specified <see cref="string" /> objects have the same value, ignoring case.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="value">The string to compare to this instance.</param>
        /// <returns><c>true</c> if the two strings have the same value (ignoring case), <c>false</c> otherwise.</returns>
        public static bool EqualsIgnoreCase(this string str, string value)
        {
            return string.Equals(str, value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified substring occurs within the specified <see cref="string" />.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">The <see cref="StringComparison" /> to use.</param>
        /// <returns><c>true</c> if <paramref name="value" /> is found; otherwise, <c>false</c>.</returns>
        public static bool Contains(this string str, string value, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified property selector to compare elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The sequence to get distinct members from.</param>
        /// <param name="property">The selector used to retrieve the property to compare.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains distinct elements from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> source, Func<T, object> property)
        {
            return source.GroupBy(property).Select(n => n.First());
        }

        /// <summary>
        /// Performs an action on a control, marshaling to the creating thread if necessary.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="control" />.</typeparam>
        /// <param name="control">The control.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action" /> is null.</exception>
        public static void InvokeIfRequired<T>(this T control, Action<T> action) where T : ISynchronizeInvoke
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (control.InvokeRequired)
            {
                control.Invoke(action, new object[] { control });
            }
            else
            {
                action(control);
            }
        }

        /// <summary>
        /// Performs an Async action on a control so we don't hit thread locks due to waiting on logging, marshaling to the creating thread if necessary.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="control" />.</typeparam>
        /// <param name="control">The control.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action" /> is null.</exception>
        public static void AsyncInvokeIfRequired<T>(this T control, Action<T> action) where T : ISynchronizeInvoke
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (control.InvokeRequired)
            {
                var waitFor = control.BeginInvoke(action, new object[] { control });
                waitFor.AsyncWaitHandle.WaitOne();
            }
            else
            {
                action(control);
            }
        }

        /// <summary>
        /// Sets the name of the specified <see cref="Thread" />.
        /// If the thread name cannot be set, then this method does nothing.
        /// </summary>
        /// <param name="thread">The thread to be renamed.</param>
        /// <param name="name">The thread name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is null.</exception>
        public static void SetName(this Thread thread, string name)
        {
            if (thread == null)
            {
                throw new ArgumentNullException(nameof(thread));
            }

            try
            {
                if (thread.Name == null)
                {
                    thread.Name = name;
                }
            }
            catch (InvalidOperationException)
            {
                // Race condition - ignore.
            }
        }
    }
}
