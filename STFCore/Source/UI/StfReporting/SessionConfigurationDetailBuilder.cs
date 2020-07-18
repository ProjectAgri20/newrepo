using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.Reporting
{
    /// <summary>
    /// Constructs an array-formatted snapshot of a session configuration.
    /// </summary>
    internal static class SessionConfigurationDetailBuilder
    {
        /// <summary>
        /// Retrieve Session Configuration data formatted as a two-dimentional object array.
        /// Two-dimentional object arrays can be consumed by Excel automation components.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>Two-dimentional object array of Session configuration data</returns>
        public static object[,] GetSessionSettingsArray(string sessionId)
        {
            string sessionXml = GetSessionConfiguration(sessionId);
            int columns = 0, rows = 0;
            object[,] result = null;
            int rowCount = 0;

            if (!string.IsNullOrEmpty(sessionXml))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sessionXml);

                CalculateDimensions(xmlDoc.DocumentElement, 0, ref columns, ref rows);

                result = new object[rows, columns];
                NodeToArray(xmlDoc.DocumentElement, 0, ref rowCount, ref result);
            }

            return result;
        }

        private static string GetSessionConfiguration(string sessionId)
        {
            string sql = String.Format(Resources.SessionConfigurationSQL, sessionId);

            DataTable table = new DataTable();
            using (SqlAdapter adapter = new SqlAdapter(ReportingSqlConnection.ConnectionString))
            {
                using (SqlDataReader reader = adapter.ExecuteReader(sql))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                return reader.GetString(0);
                            }
                            catch (SqlNullValueException)
                            {
                                return null;
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Builder method for XmlNode to 2-dimentional array.
        /// </summary>
        /// <param name="theNode"></param>
        /// <param name="level"></param>
        /// <param name="rowIndex"></param>
        /// <param name="result"></param>
        private static void NodeToArray(XmlNode theNode, int level, ref int rowIndex, ref object[,] result)
        {
            //The name node gets treated like an attribute.  No need to print it again.
            if (theNode.Name != "#text" && theNode.Name != "Name")
            {
                // Iterate the columns, inserting the node data in the correct column
                for (int col = 0; col < result.GetUpperBound(1) + 1; col++)
                {
                    if (col == level)
                    {
                        result[rowIndex, col] = BuildNodeName(theNode) + BuildNodeValue(theNode);
                    }
                    else
                    {
                        result[rowIndex, col] = string.Empty;
                    }
                }
                rowIndex++;

                // Iterate each child node.
                foreach (XmlNode childNode in theNode.ChildNodes)
                {
                    NodeToArray(childNode, level + 1, ref rowIndex, ref result);
                }
            }
        }

        private static void CalculateDimensions(XmlNode theNode, int previousLevel, ref int highestLevel, ref int childCount)
        {
            int thisLevel = previousLevel + 1;

            if (thisLevel > highestLevel)
            {
                highestLevel = thisLevel;
            }
            childCount++;

            foreach (XmlNode childNode in theNode.ChildNodes)
            {
                CalculateDimensions(childNode, thisLevel, ref highestLevel, ref childCount);
            }
        }

        private static string BuildNodeName(XmlNode theNode)
        {
            if (theNode.Name != "string")
            {
                return theNode.Name + ": ";
            }
            return string.Empty;
        }

        private static string BuildNodeValue(XmlNode theNode)
        {
            // See if there is a child node named "Name".
            XmlNode nameNode = theNode.SelectSingleNode("Name");

            if (nameNode != null)
            {
                return nameNode.InnerXml;
            }
            else if (!theNode.InnerXml.StartsWith("<", StringComparison.Ordinal))
            {
                return theNode.InnerXml;
            }
            else
            {
                return BuildNodeAttrString(theNode);
            }
        }

        private static string BuildNodeAttrString(XmlNode theNode)
        {
            int orderIndex = -1, nameIndex = -1, typeIndex = -1;
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < theNode.Attributes.Count; i++)
            {
                switch (theNode.Attributes[i].Name)
                {
                    case "ExecutionOrder":
                        orderIndex = i;
                        break;
                    case "Type":
                        typeIndex = i;
                        break;
                    case "Name":
                        nameIndex = i;
                        break;
                    default:
                        break;
                }
            }

            if (orderIndex > -1)
            {
                result.Append(theNode.Attributes[orderIndex].InnerText).Append(". ");
            }
            if (nameIndex > -1)
            {
                result.Append(theNode.Attributes[nameIndex].InnerText).Append(" ");
            }
            if (typeIndex > -1)
            {
                result.Append("(");
                result.Append(theNode.Attributes[typeIndex].InnerText).Append(")");
            }

            return result.ToString();
        }
    }
}
