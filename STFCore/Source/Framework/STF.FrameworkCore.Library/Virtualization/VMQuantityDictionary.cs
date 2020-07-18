using System;
using System.Collections.Generic;
using System.Text;

namespace HP.ScalableTest.Virtualization
{
    /// <summary>
    /// Class that captures the required number of Virtual Machines by
    /// platform.  This is used to help start the correct number of
    /// VMs
    /// </summary>
    [Serializable]
    public class VMQuantityDictionary : Dictionary<string, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VMQuantityDictionary"/> class.
        /// </summary>
        public VMQuantityDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VMQuantityDictionary"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public VMQuantityDictionary(IEnumerable<KeyValuePair<string, int>> items)
        {
            foreach (var item in items)
            {
                Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string key in Keys)
            {
                builder.Append("Platform {0}, Machine Count {1}".FormatWith(key, this[key]));
            }

            return builder.ToString();

        }
    }
}