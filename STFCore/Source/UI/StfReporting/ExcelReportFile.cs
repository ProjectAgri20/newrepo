using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace HP.ScalableTest.UI.Reporting
{
    /// <summary>
    /// Represents an Excel report file.
    /// </summary>
    public class ExcelReportFile
    {
        private Application _application;
        private Workbook _workbook;
        private bool _statusBar;

        public ExcelReportFile(string file) : this(file, false, new Microsoft.Office.Interop.Excel.Application() {Visible = true})
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelReportFile"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public ExcelReportFile(string file, bool currentFile, Application excelApp)
        {
            _application = excelApp;

            // Load the specified file
            // Note: If the file gets corrupted a lot using xlNormalLoad, you can pass in xlRepairFile for the last parameter.
            // This will cause Excel to attempt to repair the corrupted file, but users will see the
            // following dialog every time:  "Excel completed file level validation and repair."
            if (currentFile)
            {
                _workbook = _application.ActiveWorkbook;
            }
            else
            {
                _workbook = _application.Workbooks.Open(file, XlCorruptLoad.xlNormalLoad);
            }
        }

        /// <summary>
        /// Returns a value indicating whether the specified worksheet exists in this template.
        /// </summary>
        /// <param name="worksheetName">The tab name.</param>
        /// <returns></returns>
        public bool WorksheetExists(string worksheetName)
        {
            foreach (Worksheet worksheet in _workbook.Worksheets)
            {
                if (String.Equals(worksheetName, worksheet.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the data set definitions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportDataSetDefinition> ExtractDataSetDefinitions(string worksheetName = "Report Definition", bool hideDefinitionWorksheet = true)
        {
            List<ReportDataSetDefinition> definitions = new List<ReportDataSetDefinition>();

            if (!WorksheetExists(worksheetName))
            {
                throw new InvalidOperationException("Cannot find worksheet '" + worksheetName + "' with report definitions.");
            }

            Worksheet worksheet = _workbook.Worksheets[worksheetName];
            if (hideDefinitionWorksheet)
            {
                worksheet.Visible = XlSheetVisibility.xlSheetHidden;
            }

            Range range = worksheet.Range["A2"];
            int row = 1;
            do
            {
                string name = range.Cells[row, 1].Value;
                string startCell = range.Cells[row, 2].Value;
                string reportSql = range.Cells[row, 3].Value;

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(startCell) && !string.IsNullOrEmpty(reportSql))
                {
                    definitions.Add(new ReportDataSetDefinition(name, reportSql, startCell));
                }
            } while (range.Cells[++row, 1].Value != null);

            return definitions as IEnumerable<ReportDataSetDefinition>;
        }

        /// <summary>
        /// Sets the status bar.
        /// </summary>
        /// <param name="text">The status bar text.</param>
        public void SetStatusBar(string text)
        {
            _statusBar = _application.DisplayStatusBar;
            _application.DisplayStatusBar = true;
            _application.StatusBar = text;
        }

        /// <summary>
        /// Resets the status bar.
        /// </summary>
        public void ResetStatusBar()
        {
            _application.StatusBar = false;
            _application.DisplayStatusBar = _statusBar;
        }

        private string ExtractRefreshSetting(string name, string worksheetName = "Scenario Settings")
        {
            if (!WorksheetExists(worksheetName))
            {
                throw new InvalidOperationException("Cannot find worksheet '" + worksheetName + "' with session settings.");
            }

            Worksheet worksheet = _workbook.Worksheets[worksheetName];

            Range range = worksheet.Range["A1"];
            int row = 0;

            while (range.Cells[++row, 1].Value != null)
            {
                if (range.Cells[row, 1].Value == name)
                {
                    return range.Cells[row, 2].Value;
                }
            }

            return null;
        }

        public string ExtractSystemDatabase()
        {
            return ExtractRefreshSetting("System Database");
        }

        /// <summary>
        /// Extracts the type of the database.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ExtractDatabaseServer()
        {
            return ExtractRefreshSetting("Database Server");
        }

        /// <summary>
        /// Extracts the environment.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ExtractDatabase()
        {
            return ExtractRefreshSetting("Database");
        }

        /// <summary>
        /// Extracts the session ids.
        /// </summary>
        /// <param name="worksheetName">Name of the worksheet.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        /// <exception cref="InvalidOperationException">Cannot find worksheet ' + worksheetName + ' with session settings.</exception>
        public List<string> ExtractSessionIds(string worksheetName = "Scenario Settings")
        {
            List<string> sessionIds = new List<string>();

            if (!WorksheetExists(worksheetName))
            {
                throw new InvalidOperationException("Cannot find worksheet '" + worksheetName + "' with session settings.");
            }

            Worksheet worksheet = _workbook.Worksheets[worksheetName];

            Range range = worksheet.Range["A1"];
            int row = 0;

            while (range.Cells[++row, 1].Value != null)
            {
                if (range.Cells[row, 1].Value == "Session Id")
                {
                    sessionIds.Add(range.Cells[row, 2].Value);
                }
            }

            return sessionIds;
        }

        /// <summary>
        /// Writes the specified data to the specified worksheet starting at the specified row and column.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="worksheetName">The worksheet name.</param>
        /// <param name="rangeStartCol">The range start column.</param>
        /// <param name="rangeStartRow">The range start row.</param>
        public void WriteData(object[,] data, string worksheetName, string rangeStartCol, int rangeStartRow)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (rangeStartCol == null)
            {
                throw new ArgumentNullException("rangeStartCol");
            }

            Worksheet worksheet = _workbook.Worksheets[worksheetName];
            string rangeStart = rangeStartCol.ToUpperInvariant() + rangeStartRow.ToString(CultureInfo.InvariantCulture);
            Range range = worksheet.Range[rangeStart];
            range = range.Resize[data.GetUpperBound(0) + 1, data.GetUpperBound(1) + 1];
            range.Value = data;
        }

        /// <summary>
        /// Saves the workbook and refresh all the formulas, pivot tables, etc.
        /// </summary>
        public void Save()
        {
            _workbook.RefreshAll();
            _workbook.Save();
        }

        /// <summary>
        /// Closes the workbook without saving, and kills the process
        /// </summary>
        public void Close()
        {
            _workbook.Close();
            _application.Quit();
            Marshal.FinalReleaseComObject(_application);
            _application = null;

        }
    }
}
