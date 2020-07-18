using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// A read-only collection of <see cref="Document" /> objects.
    /// </summary>
    public sealed class DocumentCollection : ReadOnlyCollection<Document>
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentCollection" /> class.
        /// </summary>
        /// <param name="documents">The list of <see cref="Document" /> objects to wrap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documents" /> is null.</exception>
        public DocumentCollection(IList<Document> documents)
            : base(documents)
        {
        }

        /// <summary>
        /// Gets a random <see cref="Document" /> from this collection.
        /// </summary>
        /// <returns>A <see cref="Document" /> selected at random, or null if this collection is empty.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method returns a different object each time it is called.")]
        public Document GetRandom()
        {
            if (Items.Count > 0)
            {
                return Items[_random.Next(Items.Count)];
            }
            else
            {
                return null;
            }
        }
    }
}
