using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Defines a column for a database table containing log data.
    /// </summary>
    [DataContract, DebuggerDisplay("{Name,nq} ({DataType,nq})")]
    public sealed class LogTableColumn
    {
        private static readonly Dictionary<Type, string> _typeMappings = new Dictionary<Type, string>
        {
            [typeof(bool)] = "bit",
            [typeof(int)] = "int",
            [typeof(long)] = "bigint",
            [typeof(short)] = "smallint",
            [typeof(byte)] = "tinyint",
            [typeof(float)] = "float",
            [typeof(double)] = "float",
            [typeof(decimal)] = "decimal",
            [typeof(Guid)] = "uniqueidentifier",
            [typeof(TimeSpan)] = "time",
            [typeof(DateTime)] = "datetime",
            [typeof(DateTimeOffset)] = "datetime",
            [typeof(byte[])] = "varbinary(max)"
        };

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _name;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _dataType;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _allowNulls;

        /// <summary>
        /// Gets the column name.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the column data type.
        /// </summary>
        public string DataType => _dataType;

        /// <summary>
        /// Gets a value indicating whether this column can contain NULL values.
        /// </summary>
        public bool AllowNulls => _allowNulls;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogTableColumn" /> class.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="dataType">The column data type.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> is null or empty.
        /// <para>or</para>
        /// <paramref name="dataType" /> is null or empty.
        /// </exception>
        public LogTableColumn(string name, string dataType)
            : this(name, dataType, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogTableColumn" /> class.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="dataType">The column data type.</param>
        /// <param name="allowNulls">Indicates whether this column can contain NULL values.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> is null or empty.
        /// <para>or</para>
        /// <paramref name="dataType" /> is null or empty.
        /// </exception>
        public LogTableColumn(string name, string dataType, bool allowNulls)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Column name must be defined.", nameof(name));
            }

            if (string.IsNullOrEmpty(dataType))
            {
                throw new ArgumentException("Data type must be defined.", nameof(dataType));
            }

            _name = name;
            _dataType = dataType;
            _allowNulls = allowNulls;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogTableColumn"/> class.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo" /> to base this column on.</param>
        /// <param name="maxLength">The maximum length to allow for this field (ignored for types other than string).</param>
        /// <param name="allowNulls">Indicates whether this column can contain NULL values.</param>
        /// <exception cref="ArgumentNullException"><paramref name="property" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="property"/> is of type <see cref="string" /> and <paramref name="maxLength" /> is invalid.</exception>
        public LogTableColumn(PropertyInfo property, int maxLength, bool allowNulls)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            _name = property.Name;
            _dataType = GetDataType(property, maxLength);
            _allowNulls = allowNulls;
        }

        private static string GetDataType(PropertyInfo property, int maxLength)
        {
            // Check to see if this is a nullable value type; if so, get the underlying type
            Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            if (type == typeof(string))
            {
                if (maxLength == -1 || maxLength > 8000)
                {
                    return "nvarchar(max)";
                }
                else if (maxLength <= 0)
                {
                    throw new ArgumentException("Max Length must be either greater than 0, or set to -1 to represent the maximum length.");
                }
                else
                {
                    return $"nvarchar({maxLength})";
                }
            }
            else
            {
                if (_typeMappings.TryGetValue(type, out string result))
                {
                    return result;
                }
                else
                {
                    throw new NotSupportedException($"{type} is not supported.");
                }
            }
        }
    }
}
