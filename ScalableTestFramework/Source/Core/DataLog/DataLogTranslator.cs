using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Generates <see cref="LogTableDefinition" /> and <see cref="LogTableRecord" /> classes from types that
    /// use <see cref="DataLogPropertyAttribute" />, such as <see cref="ActivityDataLog" /> and <see cref="FrameworkDataLog" />.
    /// </summary>
    public static class DataLogTranslator
    {
        /// <summary>
        /// Creates a <see cref="LogTableDefinition" /> for the specified <see cref="ActivityDataLog" /> object.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" /> object.</param>
        /// <returns>A <see cref="LogTableDefinition" /> for the specified <see cref="ActivityDataLog" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        public static LogTableDefinition BuildTableDefinition(ActivityDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableDefinition definition = new LogTableDefinition(dataLog.TableName);
            foreach (DataLogPropertyInfo dataLogProperty in GetDataLogProperties(dataLog))
            {
                switch (dataLogProperty.Name)
                {
                    // Special cases for base class properties
                    case nameof(ActivityDataLog.RecordId):
                        definition.Add(new LogTableColumn(definition.PrimaryKey, "uniqueidentifier", false));
                        break;

                    case nameof(ActivityDataLog.SessionId):
                        definition.Add(new LogTableColumn(dataLogProperty.Name, "varchar(50)", false));
                        break;

                    case nameof(ActivityDataLog.ActivityExecutionId):
                        definition.Add(new LogTableColumn(dataLogProperty.Name, "uniqueidentifier", false));
                        break;

                    default:
                        definition.Add(new LogTableColumn(dataLogProperty.PropertyInfo, dataLogProperty.MaxLength, true));
                        break;
                }
            }
            return definition;
        }

        /// <summary>
        /// Creates a <see cref="LogTableDefinition" /> for the specified <see cref="FrameworkDataLog" /> object.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" /> object.</param>
        /// <returns>A <see cref="LogTableDefinition" /> for the specified <see cref="FrameworkDataLog" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        public static LogTableDefinition BuildTableDefinition(FrameworkDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableDefinition definition = new LogTableDefinition(dataLog.TableName, dataLog.PrimaryKeyColumn);
            foreach (DataLogPropertyInfo dataLogProperty in GetDataLogProperties(dataLog))
            {
                // Special case for session ID
                if (dataLogProperty.Name == "SessionId" && dataLogProperty.PropertyType == typeof(string))
                {
                    definition.Add(new LogTableColumn(dataLogProperty.Name, "varchar(50)", false));
                }
                else
                {
                    // Primary key should be non-null; everything else is nullable
                    bool nullable = (dataLogProperty.Name != dataLog.PrimaryKeyColumn);
                    definition.Add(new LogTableColumn(dataLogProperty.PropertyInfo, dataLogProperty.MaxLength, nullable));
                }
            }
            return definition;
        }

        /// <summary>
        /// Builds a <see cref="LogTableRecord" /> for an insert operation from the specified <see cref="ActivityDataLog" /> object.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" /> object.</param>
        /// <returns>A <see cref="LogTableRecord" /> for an insert operation from the specified <see cref="ActivityDataLog" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="dataLog" /> contains data that exceeds the maximum allowable length and cannot be truncated.</exception>
        public static LogTableRecord BuildInsertRecord(ActivityDataLog dataLog)
        {
            return BuildRecord(dataLog, true);
        }

        /// <summary>
        /// Builds a <see cref="LogTableRecord" /> for an update operation from the specified <see cref="ActivityDataLog" /> object.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" /> object.</param>
        /// <returns>A <see cref="LogTableRecord" /> for an update operation from the specified <see cref="ActivityDataLog" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="dataLog" /> contains data that exceeds the maximum allowable length and cannot be truncated.</exception>
        public static LogTableRecord BuildUpdateRecord(ActivityDataLog dataLog)
        {
            return BuildRecord(dataLog, false);
        }

        private static LogTableRecord BuildRecord(ActivityDataLog dataLog, bool insert)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            // Build the table definition so that we can use the primary key
            LogTableDefinition tableDefinition = new LogTableDefinition(dataLog.TableName);

            LogTableRecord record = new LogTableRecord();
            foreach (DataLogPropertyInfo dataLogProperty in GetDataLogProperties(dataLog))
            {
                bool isPrimaryKey = dataLogProperty.Name.Equals(nameof(ActivityDataLog.RecordId));
                if (insert || dataLogProperty.IncludeInUpdates || isPrimaryKey)
                {
                    // Special case: change "RecordId" to the primary key name
                    string columnName = isPrimaryKey ? tableDefinition.PrimaryKey : dataLogProperty.Name;
                    record.Add(columnName, GetPropertyValue(dataLogProperty, dataLog));
                }
            }
            return record;
        }

        /// <summary>
        /// Builds a <see cref="LogTableRecord" /> for an insert operation from the specified <see cref="FrameworkDataLog" /> object.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" /> object.</param>
        /// <returns>A <see cref="LogTableRecord" /> for an insert operation from the specified <see cref="FrameworkDataLog" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="dataLog" /> contains data that exceeds the maximum allowable length and cannot be truncated.</exception>
        public static LogTableRecord BuildInsertRecord(FrameworkDataLog dataLog)
        {
            return BuildRecord(dataLog, true);
        }

        /// <summary>
        /// Builds a <see cref="LogTableRecord" /> for an update operation from the specified <see cref="FrameworkDataLog" /> object.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" /> object.</param>
        /// <returns>A <see cref="LogTableRecord" /> for an update operation from the specified <see cref="FrameworkDataLog" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="dataLog" /> contains data that exceeds the maximum allowable length and cannot be truncated.</exception>
        public static LogTableRecord BuildUpdateRecord(FrameworkDataLog dataLog)
        {
            return BuildRecord(dataLog, false);
        }

        private static LogTableRecord BuildRecord(FrameworkDataLog dataLog, bool insert)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableRecord record = new LogTableRecord();
            foreach (DataLogPropertyInfo dataLogProperty in GetDataLogProperties(dataLog))
            {
                bool isPrimaryKey = dataLogProperty.Name.Equals(dataLog.PrimaryKeyColumn);
                if (insert || dataLogProperty.IncludeInUpdates || isPrimaryKey)
                {
                    record.Add(dataLogProperty.Name, GetPropertyValue(dataLogProperty, dataLog));
                }
            }
            return record;
        }

        private static object GetPropertyValue(DataLogPropertyInfo dataLogProperty, object dataLog)
        {
            object result = dataLogProperty.PropertyInfo.GetValue(dataLog);

            // Check to see if it is a string; if so, check to see if it needs to be truncated
            if (result is string stringResult && stringResult.Length > dataLogProperty.MaxLength && dataLogProperty.MaxLength != -1)
            {
                if (dataLogProperty.TruncationAllowed)
                {
                    result = stringResult.Substring(0, dataLogProperty.MaxLength - 3) + "...";
                }
                else
                {
                    throw new InvalidOperationException($"String data is too large for column {dataLogProperty.Name} and truncation is disallowed.");
                }
            }

            // Coerce DateTimeOffset into UtcDateTime
            if (result is DateTimeOffset)
            {
                result = ((DateTimeOffset)result).UtcDateTime;
            }

            return result;
        }

        private static IEnumerable<DataLogPropertyInfo> GetDataLogProperties(object dataLog)
        {
            return from propertyInfo in dataLog.GetType().GetProperties()
                   let dataLogPropertyAttribute = propertyInfo.GetCustomAttribute<DataLogPropertyAttribute>()
                   where dataLogPropertyAttribute != null
                   orderby dataLogPropertyAttribute.Order
                   select new DataLogPropertyInfo(propertyInfo, dataLogPropertyAttribute);
        }

        private sealed class DataLogPropertyInfo
        {
            public string Name => PropertyInfo.Name;
            public Type PropertyType => PropertyInfo.PropertyType;
            public int MaxLength => DataLogPropertyAttribute.MaxLength;
            public bool TruncationAllowed => DataLogPropertyAttribute.TruncationAllowed;
            public bool IncludeInUpdates => DataLogPropertyAttribute.IncludeInUpdates;

            public PropertyInfo PropertyInfo { get; }
            public DataLogPropertyAttribute DataLogPropertyAttribute { get; }

            public DataLogPropertyInfo(PropertyInfo propertyInfo, DataLogPropertyAttribute dataLogPropertyAttribute)
            {
                PropertyInfo = propertyInfo;
                DataLogPropertyAttribute = dataLogPropertyAttribute;
            }
        }
    }
}
