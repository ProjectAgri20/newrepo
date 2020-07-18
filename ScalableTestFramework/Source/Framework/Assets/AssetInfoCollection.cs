using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// A read-only collection of <see cref="AssetInfo" /> objects.
    /// </summary>
    public sealed class AssetInfoCollection : ReadOnlyCollection<AssetInfo>
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInfoCollection" /> class.
        /// </summary>
        /// <param name="assets">The list of <see cref="AssetInfo" /> objects to wrap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assets" /> is null.</exception>
        public AssetInfoCollection(IList<AssetInfo> assets)
            : base(assets)
        {
        }

        /// <summary>
        /// Gets all <see cref="AssetInfo" /> objects of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="AssetInfo" /> objects to return.</typeparam>
        /// <returns>All <see cref="AssetInfo" /> objects of type <typeparamref name="T" />.</returns>
        public IEnumerable<T> OfType<T>() where T : IAssetInfo
        {
            return Items.OfType<T>();
        }

        /// <summary>
        /// Gets a random <see cref="AssetInfo" /> from this collection.
        /// </summary>
        /// <returns>An <see cref="AssetInfo" /> selected at random, or null if this collection is empty.</returns>
        public AssetInfo GetRandom()
        {
            return GetRandom(Items);
        }

        /// <summary>
        /// Gets a random <see cref="AssetInfo" /> of the specified type from this collection.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="AssetInfo" /> object to select.</typeparam>
        /// <returns>An <see cref="AssetInfo" /> of type <typeparamref name="T" /> selected at random, or null if this collection is empty.</returns>
        public T GetRandom<T>() where T : IAssetInfo
        {
            return GetRandom(Items.OfType<T>().ToList());
        }

        private T GetRandom<T>(IList<T> items)
        {
            if (items.Count > 0)
            {
                return items[_random.Next(items.Count)];
            }
            else
            {
                return default;
            }
        }
    }
}
