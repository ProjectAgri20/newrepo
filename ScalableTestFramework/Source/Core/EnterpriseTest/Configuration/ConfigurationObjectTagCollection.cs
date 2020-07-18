using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.Core.EnterpriseTest.Configuration
{
    /// <summary>
    /// A keyed collection of <see cref="ConfigurationObjectTag" /> objects.
    /// </summary>
    public sealed class ConfigurationObjectTagCollection : KeyedCollection<Guid, ConfigurationObjectTag>
    {
        /// <summary>
        /// Adds all <see cref="ConfigurationObjectTag" /> items from the specified collection.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="ArgumentNullException">items</exception>
        public void Add(IEnumerable<ConfigurationObjectTag> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (ConfigurationObjectTag tag in items)
            {
                Add(tag);
            }
        }

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>The key for the specified element.</returns>
        protected override Guid GetKeyForItem(ConfigurationObjectTag item) => item.Id;
    }
}
