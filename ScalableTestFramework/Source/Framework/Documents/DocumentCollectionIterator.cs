using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// Selects elements from a <see cref="DocumentCollection" /> based on the specified <see cref="CollectionSelectorMode" />.
    /// </summary>
    public sealed class DocumentCollectionIterator
    {
        private readonly CollectionSelectorMode _selectorMode;
        private readonly Random _random = new Random();
        private readonly Queue<Guid> _documentsLeft = new Queue<Guid>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentCollectionIterator" /> class.
        /// </summary>
        /// <param name="selectorMode">The selection mode.</param>
        public DocumentCollectionIterator(CollectionSelectorMode selectorMode)
        {
            _selectorMode = selectorMode;
        }

        /// <summary>
        /// Gets the next <see cref="Document" /> from the specified <see cref="DocumentCollection" />.
        /// </summary>
        /// <param name="documents">The <see cref="DocumentCollection" /> to select from.</param>
        /// <returns>The next <see cref="Document" /> from the collection, or null if the collection is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="documents" /> is null.</exception>
        public Document GetNext(DocumentCollection documents)
        {
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            if (documents.Count == 0)
            {
                return null;
            }

            switch (_selectorMode)
            {
                case CollectionSelectorMode.RoundRobin:
                    return GetNextInQueue(documents, false);

                case CollectionSelectorMode.ShuffledRoundRobin:
                    return GetNextInQueue(documents, true);

                case CollectionSelectorMode.Random:
                    return documents.GetRandom();

                default:
                    throw new ArgumentException($"Collection selector mode '{_selectorMode}' is not supported.");
            }
        }

        private Document GetNextInQueue(DocumentCollection documents, bool shuffle)
        {
            Document found = FindAvailableDocument(documents);
            if (found == null)
            {
                RefillQueue(documents, shuffle);
                found = FindAvailableDocument(documents);
            }
            return found;
        }

        private Document FindAvailableDocument(DocumentCollection documents)
        {
            while (_documentsLeft.Count > 0)
            {
                Guid nextId = _documentsLeft.Dequeue();
                Document found = documents.FirstOrDefault(n => n.DocumentId == nextId);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        private void RefillQueue(DocumentCollection documents, bool shuffle)
        {
            List<Guid> ids = documents.Select(n => n.DocumentId).ToList();
            while (ids.Count > 0)
            {
                int index = shuffle ? _random.Next(ids.Count) : 0;
                _documentsLeft.Enqueue(ids[index]);
                ids.RemoveAt(index);
            }
        }
    }
}
