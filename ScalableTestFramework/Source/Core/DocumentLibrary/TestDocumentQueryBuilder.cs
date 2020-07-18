using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// Builds ad-hoc SQL queries from <see cref="DocumentQuery" /> objects.
    /// </summary>
    internal static class TestDocumentQueryBuilder
    {
        /// <summary>
        /// Builds an ad-hoc SQL query from the specified <see cref="DocumentQuery" />.
        /// </summary>
        /// <param name="query">The <see cref="DocumentQuery" />.</param>
        /// <returns>A SQL query that returns documents based on the specified <see cref="DocumentQuery" />.</returns>
        public static string BuildQuery(DocumentQuery query)
        {
            List<string> whereExpressions = new List<string>();
            foreach (DocumentQueryCriteria criteria in query.Criteria)
            {
                whereExpressions.Add(BuildWhereExpression(criteria));
            }

            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT TestDocumentId, FileName, Location, FileSize, Pages, ColorMode, Orientation ");
            sqlQuery.Append("FROM TestDocument INNER JOIN TestDocumentExtension ON TestDocument.Extension = TestDocumentExtension.Extension ");

            if (whereExpressions.Any())
            {
                sqlQuery.Append($"WHERE {string.Join(" AND ", whereExpressions)}");
            }

            return sqlQuery.ToString();
        }

        private static string BuildWhereExpression(DocumentQueryCriteria criteria)
        {
            if (!criteria.Values.Any())
            {
                throw new InvalidOperationException($"No values provided for where clause expression for {criteria.PropertyName}.");
            }

            string condition = null;
            switch (criteria.QueryOperator)
            {
                case QueryOperator.Equal:
                case QueryOperator.LessThan:
                case QueryOperator.GreaterThan:
                case QueryOperator.LessThanOrEqual:
                case QueryOperator.GreaterThanOrEqual:
                case QueryOperator.NotEqual:
                    condition = BuildOperatorCondition(criteria);
                    break;

                case QueryOperator.Contains:
                    condition = BuildLikeCondition(criteria, "%", "%");
                    break;

                case QueryOperator.BeginsWith:
                    condition = BuildLikeCondition(criteria, null, "%");
                    break;

                case QueryOperator.EndsWith:
                    condition = BuildLikeCondition(criteria, "%", null);
                    break;

                case QueryOperator.IsIn:
                    condition = BuildInCondition(criteria, true);
                    break;

                case QueryOperator.IsNotIn:
                    condition = BuildInCondition(criteria, false);
                    break;

                case QueryOperator.IsBetween:
                    condition = BuildBetweenCondition(criteria);
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported query operator {criteria.QueryOperator}");
            }

            return $"TestDocument.{criteria.PropertyName} {condition}";
        }

        private static string BuildOperatorCondition(DocumentQueryCriteria criteria)
        {
            var operatorStrings = new Dictionary<QueryOperator, string>
            {
                [QueryOperator.Equal] = "=",
                [QueryOperator.LessThan] = "<",
                [QueryOperator.GreaterThan] = ">",
                [QueryOperator.LessThanOrEqual] = "<=",
                [QueryOperator.GreaterThanOrEqual] = ">=",
                [QueryOperator.NotEqual] = "<>"
            };
            string operatorString = operatorStrings[criteria.QueryOperator];

            object value = criteria.Values[0];
            string paramValue = (value is string) ? $"'{value}'" : value.ToString();
            return $"{operatorString} {paramValue}";
        }

        private static string BuildLikeCondition(DocumentQueryCriteria criteria, string prefix, string suffix)
        {
            return $"LIKE '{prefix}{criteria.Values[0]}{suffix}'";
        }

        private static string BuildInCondition(DocumentQueryCriteria criteria, bool include)
        {
            string inOperator = include ? "IN" : "NOT IN";
            var quotedValues = criteria.Values.Select(n => $"'{n}'");
            string joinedValues = string.Join(", ", quotedValues);
            return $"{inOperator} ({joinedValues})";
        }

        private static string BuildBetweenCondition(DocumentQueryCriteria criteria)
        {
            if (criteria.Values.Count < 2)
            {
                throw new InvalidOperationException("Two values must be provided for a BETWEEN where condition.");
            }

            return $"BETWEEN {criteria.Values[0]} AND {criteria.Values[1]}";
        }
    }
}
