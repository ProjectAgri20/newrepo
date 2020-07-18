using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A mock data log record.
    /// </summary>
    public sealed class DataLoggerMockRecord
    {
        /// <summary>
        /// The values in the data log record.
        /// </summary>
        public Dictionary<string, object> Values { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMockRecord" /> class.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" /> containing the log data.</param>
        internal DataLoggerMockRecord(ActivityDataLog dataLog)
        {
            Values = ExtractValues(dataLog, false);
        }

        /// <summary>
        /// Updates this record with data from the specified <see cref="ActivityDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" /> containing the log data.</param>
        internal void Update(ActivityDataLog dataLog)
        {
            foreach (var newValue in ExtractValues(dataLog, true))
            {
                Values[newValue.Key] = newValue.Value;
            }
        }

        private static Dictionary<string, object> ExtractValues(ActivityDataLog dataLog, bool updateOnly)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            Type dataLogType = dataLog.GetType();

            var properties = from property in dataLogType.GetProperties()
                             let dataLogAttribute = property.GetCustomAttribute<DataLogPropertyAttribute>()
                             where dataLogAttribute != null
                             orderby dataLogAttribute.Order
                             select new
                             {
                                 Property = property,
                                 Attribute = dataLogAttribute
                             };

            foreach (var value in properties)
            {
                if (!updateOnly || value.Attribute.IncludeInUpdates)
                {
                    if (value.Property.Name == "RecordId")
                    {
                        values.Add(dataLog.TableName + "Id", value.Property.GetValue(dataLog));
                    }
                    else
                    {
                        values.Add(value.Property.Name, GetPropertyValue(dataLog, value.Property, value.Attribute));
                    }
                }
            }

            return values;
        }

        private static object GetPropertyValue(ActivityDataLog dataLog, PropertyInfo property, DataLogPropertyAttribute dataLogAttribute)
        {
            if (dataLogAttribute.MaxLength == 0 || dataLogAttribute.MaxLength < -1)
            {
                throw new DataLoggerMockException("Data log attribute max length must be a positive number or -1 (for unlimited).");
            }

            // If it's a string, we might need some special handling
            if (property.PropertyType == typeof(string))
            {
                string value = property.GetValue(dataLog) as string ?? string.Empty;

                // If there is a maximum length specified and the value exceeds that length...
                if (dataLogAttribute.MaxLength > 0 && value.Length > dataLogAttribute.MaxLength)
                {
                    // If trunctation is allowed, truncate the value and return
                    if (dataLogAttribute.TruncationAllowed)
                    {
                        return value.Substring(0, dataLogAttribute.MaxLength);
                    }
                    else
                    {
                        // The value exceeds the maximum length, but truncation is not allowed.
                        throw new DataLoggerMockException($"Value of property '{property.Name}' exceeds the max length, and truncation is not allowed.");
                    }
                }
            }

            return property.GetValue(dataLog);
        }
    }
}
