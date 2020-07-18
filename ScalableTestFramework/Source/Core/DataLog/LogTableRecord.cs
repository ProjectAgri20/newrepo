using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Represents a log data record that will be used in an insert or update to a database table.
    /// </summary>
    [DataContract]
    public sealed class LogTableRecord : IEnumerable<KeyValuePair<string, object>>
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        public object this[string key]
        {
            get => _values[key];
            set => _values[key] = value;
        }

        /// <summary>
        /// Gets the number of key/value pairs for this record.
        /// </summary>
        public int Count => _values.Count;

        /// <summary>
        /// Gets a collection of keys for this record.
        /// </summary>
        public IEnumerable<string> Keys => _values.Keys;

        /// <summary>
        /// Determines whether the record contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if this records contains the specified key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(string key) => _values.ContainsKey(key);

        /// <summary>
        /// Adds the specified key and value to the record data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, object value) => _values.Add(key, value);

        /// <summary>
        /// Removes the value with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key) => _values.Remove(key);

        /// <summary>
        /// Removes all keys and values from this instance.
        /// </summary>
        public void Clear() => _values.Clear();

        #region IEnumerable Members

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() => _values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

        #endregion
    }
}
