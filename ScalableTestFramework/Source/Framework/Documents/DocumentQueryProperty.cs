using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// A document property that can be used in <see cref="DocumentQueryCriteria" />.
    /// </summary>
    /// <remarks>
    /// The document library supports a finite number of querying properties, which are exposed
    /// via static members on this class. Consumers should not attempt to create their own querying properties
    /// as the framework <see cref="IDocumentLibrary" /> implementation will not work with them.
    /// </remarks>
    [DebuggerDisplay("{Name,nq}")]
    public sealed class DocumentQueryProperty
    {
        #region Operator Sets

        private static readonly List<QueryOperator> _numberOperators = new List<QueryOperator>
        {
            QueryOperator.Equal,
            QueryOperator.LessThan,
            QueryOperator.GreaterThan,
            QueryOperator.LessThanOrEqual,
            QueryOperator.GreaterThanOrEqual,
            QueryOperator.NotEqual,
            QueryOperator.IsBetween
        };

        private static readonly List<QueryOperator> _stringOperators = new List<QueryOperator>
        {
            QueryOperator.Equal,
            QueryOperator.NotEqual,
            QueryOperator.Contains,
            QueryOperator.BeginsWith,
            QueryOperator.EndsWith
        };

        private static readonly List<QueryOperator> _listOperators = new List<QueryOperator>
        {
            QueryOperator.IsIn,
            QueryOperator.IsNotIn
        };

        #endregion

        // Operator sets must appear above this line - static intializers are executed in textual order
        private static readonly Dictionary<string, DocumentQueryProperty> _queryProperties = LoadProperties();

        private ReadOnlyCollection<object> _values;
        private readonly Func<IEnumerable<object>> _loadValues;

        /// <summary>
        /// The query property name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The query property label.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// The applicable query operators.
        /// </summary>
        public ReadOnlyCollection<QueryOperator> Operators { get; }

        /// <summary>
        /// Gets the available values for list-based properties.
        /// </summary>
        public ReadOnlyCollection<object> Values
        {
            get
            {
                if (_values == null && _loadValues != null)
                {
                    _values = new ReadOnlyCollection<object>(_loadValues().ToList());
                }
                return _values;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentQueryProperty"/> class.
        /// </summary>
        /// <param name="name">The name of the query property.</param>
        /// <param name="label">The label to display in a UI.</param>
        /// <param name="operators">The valid query operators for this property.</param>
        private DocumentQueryProperty(string name, string label, IList<QueryOperator> operators)
        {
            Name = name;
            Label = label;
            Operators = new ReadOnlyCollection<QueryOperator>(operators);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentQueryProperty"/> class.
        /// </summary>
        /// <param name="name">The name of the query property.</param>
        /// <param name="label">The label to display in a UI.</param>
        /// <param name="operators">The valid query operators for this property.</param>
        /// <param name="values">The finite collection of values that can be selected for this property.</param>
        private DocumentQueryProperty(string name, string label, IList<QueryOperator> operators, IEnumerable<object> values)
            : this(name, label, operators)
        {
            _values = new ReadOnlyCollection<object>(values.ToList());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentQueryProperty"/> class.
        /// </summary>
        /// <param name="name">The name of the query property.</param>
        /// <param name="label">The label to display in a UI.</param>
        /// <param name="operators">The valid query operators for this property.</param>
        /// <param name="loadValues">A delegate to retrieve the finite collection of values that can be selected for this property.</param>
        private DocumentQueryProperty(string name, string label, IList<QueryOperator> operators, Func<IEnumerable<object>> loadValues)
            : this(name, label, operators)
        {
            _loadValues = loadValues;
        }

        #region Static Query Properties

        /// <summary>
        /// Specifies the file name allowed for documents.
        /// </summary>
        public static DocumentQueryProperty FileName => _queryProperties["FileName"];

        /// <summary>
        /// Specifies the extensions allowed for documents.
        /// </summary>
        public static DocumentQueryProperty Extension => _queryProperties["Extension"];

        /// <summary>
        /// Specifies the tags allowed for documents.
        /// </summary>
        public static DocumentQueryProperty Tag => _queryProperties["Tag"];

        /// <summary>
        /// Specifies the file sizes allowed for documents.
        /// </summary>
        public static DocumentQueryProperty FileSize => _queryProperties["FileSize"];

        /// <summary>
        /// Specifies the number of pages allowed for documents.
        /// </summary>
        public static DocumentQueryProperty Pages => _queryProperties["Pages"];

        /// <summary>
        /// Specifies the allowed color mode(s) for documents.
        /// </summary>
        public static DocumentQueryProperty ColorMode => _queryProperties["ColorMode"];

        /// <summary>
        /// Specifies the allowed document orientation(s) for documents.
        /// </summary>
        public static DocumentQueryProperty Orientation => _queryProperties["Orientation"];

        private static Dictionary<string, DocumentQueryProperty> LoadProperties()
        {
            List<object> colorModes = new List<object>() { Documents.ColorMode.Color, Documents.ColorMode.Mono };
            List<object> orientations = new List<object>() { Documents.Orientation.Landscape, Documents.Orientation.Portrait, Documents.Orientation.Mixed };

            IEnumerable<object> getExtensions() => ConfigurationServices.DocumentLibrary.GetExtensions().Select(n => n.Extension);
            IEnumerable<object> getTags() => ConfigurationServices.DocumentLibrary.GetTags();

            List<DocumentQueryProperty> properties = new List<DocumentQueryProperty>
            {
                new DocumentQueryProperty("FileName", "File Name", _stringOperators),
                new DocumentQueryProperty("FileSize", "Size (KB)", _numberOperators),
                new DocumentQueryProperty("Pages", "Page Count", _numberOperators),
                new DocumentQueryProperty("ColorMode", "Color Mode", _listOperators, colorModes),
                new DocumentQueryProperty("Orientation", "Orientation", _listOperators, orientations),
                new DocumentQueryProperty("Extension", "Extension", _listOperators, getExtensions),
                new DocumentQueryProperty("Tag", "Tag", _listOperators, getTags)
            };

            return properties.ToDictionary(n => n.Name, n => n);
        }

        #endregion
    }
}
