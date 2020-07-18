using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// Specifies property value requirements that can be used as part of a test document query.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{PropertyName,nq}")]
    public sealed class DocumentQueryCriteria
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _propertyName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly QueryOperator _queryOperator;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Collection<object> _values = new Collection<object>();

        /// <summary>
        /// Gets the criteria property name.
        /// </summary>
        public string PropertyName => _propertyName;

        /// <summary>
        /// Gets the criteria query operator.
        /// </summary>
        public QueryOperator QueryOperator => _queryOperator;

        /// <summary>
        /// Gets the criteria query value(s).
        /// </summary>
        public Collection<object> Values => _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentQueryCriteria" /> class.
        /// </summary>
        /// <param name="property">The <see cref="DocumentQueryProperty" />.</param>
        /// <param name="queryOperator">The <see cref="QueryOperator" />.</param>
        private DocumentQueryCriteria(DocumentQueryProperty property, QueryOperator queryOperator)
        {
            if (!property.Operators.Contains(queryOperator))
            {
                throw new ArgumentException($"{queryOperator} is not a valid operator for query property {property.Name}.");
            }

            _propertyName = property.Name;
            _queryOperator = queryOperator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentQueryCriteria" /> class.
        /// </summary>
        /// <param name="property">The <see cref="DocumentQueryProperty" />.</param>
        /// <param name="queryOperator">The <see cref="QueryOperator" />.</param>
        /// <param name="value">The criteria query value.</param>
        public DocumentQueryCriteria(DocumentQueryProperty property, QueryOperator queryOperator, object value)
            : this(property, queryOperator)
        {
            _values.Add(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentQueryCriteria" /> class.
        /// </summary>
        /// <param name="property">The <see cref="DocumentQueryProperty" />.</param>
        /// <param name="queryOperator">The <see cref="QueryOperator" />.</param>
        /// <param name="values">The criteria query values.</param>
        public DocumentQueryCriteria(DocumentQueryProperty property, QueryOperator queryOperator, IEnumerable<object> values)
            : this(property, queryOperator)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (object value in values)
            {
                _values.Add(value);
            }
        }
    }
}
