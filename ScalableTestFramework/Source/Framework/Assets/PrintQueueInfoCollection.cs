using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// A read-only collection of <see cref="PrintQueueInfo" /> objects.
    /// </summary>
    [DataContract]
    public sealed class PrintQueueInfoCollection : ReadOnlyCollection<PrintQueueInfo>
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueInfoCollection" /> class.
        /// </summary>
        /// <param name="printQueues">The list of <see cref="PrintQueueInfo" /> objects to wrap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printQueues" /> is null.</exception>
        public PrintQueueInfoCollection(IList<PrintQueueInfo> printQueues)
            : base(printQueues)
        {
        }

        /// <summary>
        /// Gets all <see cref="PrintQueueInfo" /> objects of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="PrintQueueInfo" /> objects to return.</typeparam>
        /// <returns>All <see cref="PrintQueueInfo" /> objects of type <typeparamref name="T" />.</returns>
        public IEnumerable<T> OfType<T>() where T : PrintQueueInfo
        {
            return Items.OfType<T>();
        }

        /// <summary>
        /// Gets a random <see cref="PrintQueueInfo" /> from this collection.
        /// </summary>
        /// <returns>An <see cref="PrintQueueInfo" /> selected at random, or null if this collection is empty.</returns>
        public PrintQueueInfo GetRandom()
        {
            return GetRandom(Items);
        }

        /// <summary>
        /// Gets a random <see cref="PrintQueueInfo" /> of the specified type from this collection.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="PrintQueueInfo" /> object to select.</typeparam>
        /// <returns>An <see cref="PrintQueueInfo" /> of type <typeparamref name="T" /> selected at random, or null if this collection is empty.</returns>
        public T GetRandom<T>() where T : PrintQueueInfo
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
