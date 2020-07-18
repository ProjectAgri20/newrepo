using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.UI.Reporting
{
    /// <summary>
    /// Defines parameters for extraction of a data set for a report.
    /// </summary>
    public class ReportDataSetDefinition
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the report SQL.
        /// </summary>
        public string ReportSql { get; private set; }

        /// <summary>
        /// Gets the starting cell.
        /// </summary>
        public string StartingCell { get; private set; }

        /// <summary>
        /// Gets the starting cell row.
        /// </summary>
        public int StartingCellRow { get; private set; }

        /// <summary>
        /// Gets the starting cell column.
        /// </summary>
        public string StartingCellColumn { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportDataSetDefinition" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="reportSql">The report SQL.</param>
        /// <param name="startingCell">The starting cell.</param>
        public ReportDataSetDefinition(string name, string reportSql, string startingCell = "A2")
        {
            Name = name;
            ReportSql = reportSql;
            StartingCell = startingCell;

            // Parse the starting cell into row and column values
            Match startingCellMatch = Regex.Match(startingCell, @"\b(\D+)(\d+)\b");
            if (!startingCellMatch.Success)
            {
                throw new ArgumentException("Starting cell "+ startingCell + " was not in the proper format.");
            }
            StartingCellColumn = startingCellMatch.Groups[1].Value;
            StartingCellRow = int.Parse(startingCellMatch.Groups[2].Value, CultureInfo.InvariantCulture);
        }
    }
}
