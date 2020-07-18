using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// A set of <see cref="DocumentQueryCriteria" /> used for selecting documents for use in a test.
    /// </summary>
    [DataContract]
    public sealed class DocumentQuery
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Collection<DocumentQueryCriteria> _criteria = new Collection<DocumentQueryCriteria>();

        /// <summary>
        /// Gets the set of <see cref="DocumentQueryCriteria" /> for this query.
        /// </summary>
        public Collection<DocumentQueryCriteria> Criteria => _criteria;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentQuery" /> class.
        /// </summary>
        public DocumentQuery()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentQuery" /> class.
        /// </summary>
        /// <param name="criteria">The <see cref="DocumentQueryCriteria" /> to add to this instance.</param>
        public DocumentQuery(DocumentQueryCriteria criteria)
        {
            _criteria.Add(criteria);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentQuery" /> class.
        /// </summary>
        /// <param name="criteria">The collection of <see cref="DocumentQueryCriteria" /> to add to this instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="criteria" /> is null.</exception>
        public DocumentQuery(IEnumerable<DocumentQueryCriteria> criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            foreach (DocumentQueryCriteria item in criteria)
            {
                _criteria.Add(item);
            }
        }
    }
}
