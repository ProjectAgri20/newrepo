using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest.Configuration
{
    /// <summary>
    /// An in-memory representation of the EnterpriseTest database configuration objects.
    /// Tracks basic status of and relationships between those objects.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class EnterpriseTestConfigMap
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly ConfigurationObjectTagCollection _configurationObjectTags = new ConfigurationObjectTagCollection();

        /// <summary>
        /// Gets the number of configuration objects being tracked by this map.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Count => _configurationObjectTags.Count;

        /// <summary>
        /// Gets all configuration object tags being tracked by this map.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IEnumerable<ConfigurationObjectTag> AllObjects => _configurationObjectTags;

        /// <summary>
        /// Adds the specified <see cref="ConfigurationObjectTag" /> to the map.
        /// </summary>
        /// <param name="tag">The <see cref="ConfigurationObjectTag" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tag" /> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="tag" /> has a parent or folder ID not present in the map.</exception>
        public void Add(ConfigurationObjectTag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            if (CanAdd(tag))
            {
                _configurationObjectTags.Add(tag);
            }
            else
            {
                throw new InvalidOperationException($"Tag '{tag.Name}' cannot be added to map without parent/folder.");
            }
        }

        private bool CanAdd(ConfigurationObjectTag tag)
        {
            return (tag.ParentId == null || Contains(tag.ParentId.Value))
                && (tag.FolderId == null || Contains(tag.FolderId.Value));
        }

        /// <summary>
        /// Adds all <see cref="ConfigurationObjectTag" /> objects in the specified collection to the map.
        /// </summary>
        /// <param name="tags">The <see cref="ConfigurationObjectTag" /> collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tags" /> is null.</exception>
        public void AddRange(IEnumerable<ConfigurationObjectTag> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }

            foreach (ConfigurationObjectTag tag in tags)
            {
                Add(tag);
            }
        }

        /// <summary>
        /// Determines whether this map contains a configuration object with the specified ID.
        /// </summary>
        /// <param name="configurationObjectId">The configuration object identifier.</param>
        /// <returns><c>true</c> if this map contains a configuration object with the specified ID; otherwise, <c>false</c>.</returns>
        public bool Contains(Guid configurationObjectId)
        {
            return _configurationObjectTags.Contains(configurationObjectId);
        }

        /// <summary>
        /// Removes all <see cref="ConfigurationObjectTag" /> objects from this map.
        /// </summary>
        public void Clear()
        {
            _configurationObjectTags.Clear();
        }
    }
}
