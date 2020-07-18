using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.UI.Reporting
{
    /// <summary>
    /// Contains methods for populating an Excel spreadsheet with STF data.
    /// </summary>
    public static class ReportingEngine
    {
        /// <summary>
        /// Creates the excel report.
        /// </summary>
        /// <param name="templatePath">The report template file path.</param>
        /// <param name="destinationPath">The destination path of the generated report.</param>
        /// <param name="sessionIds">The session ids.</param>
        public static void GenerateReport(string templatePath, string destinationPath, IList<string> sessionIds)
        {
            // Build the list of selected session ID's into a comma delimited string.
            for (int i = 0; i < sessionIds.Count; i++)
            {
                sessionIds[i] = "'" + sessionIds[i] + "'";
            }
            string sessionIdList = string.Join(",", sessionIds);

            // Create a temporary staging Excel file.
            string appDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string stagedFilename = Path.Combine(appDir, Path.GetFileName(destinationPath));

            using (SpreadsheetDocument outputWorkbook = SpreadsheetDocument.Create(stagedFilename, SpreadsheetDocumentType.Workbook, true))
            {
                // Make sure the new workbook is clear.
                outputWorkbook.DeleteParts(outputWorkbook.GetPartsOfType<OpenXmlPart>());

                // Copy all the parts from the report template to the output workbook.
                using (SpreadsheetDocument templateWorkbook = SpreadsheetDocument.Open(templatePath, false))
                {
                    foreach (OpenXmlPart part in templateWorkbook.GetPartsOfType<OpenXmlPart>())
                    {
                        outputWorkbook.AddPart(part);
                    }
                }

                // Process each of the connections in the output workbook.
                if (outputWorkbook?.WorkbookPart?.ConnectionsPart?.Connections != null)
                {
                    foreach (Connection connection in outputWorkbook.WorkbookPart.ConnectionsPart.Connections.Elements<Connection>())
                    {
                        // If this connection points to a database link then perform some additional processing.
                        if (connection?.DatabaseProperties != null && connection.DatabaseProperties.Connection.Value.StartsWith("Provider="))
                        {
                            // Get the database connection string.
                            OleDbConnectionStringBuilder oleDbConnString = new OleDbConnectionStringBuilder(connection.DatabaseProperties.Connection.Value);

                            // Change the user name and password.
                            oleDbConnString.Add("User ID", "report_viewer");
                            oleDbConnString.Add("Password", "report_viewer");

                            // Change the data source and the initial catalog (database).
                            oleDbConnString.Add("Data Source", GlobalSettings.Items[Setting.ReportingDatabaseServer]);
                            oleDbConnString.Add("Initial Catalog", GlobalSettings.Items[Setting.ReportingDatabase]);
                            string appName = GlobalSettings.IsDistributedSystem ? "STF" : "STB";
                            oleDbConnString.Add("Application Name", appName);

                            // Put the modified connection string back into the output workbook.
                            connection.DatabaseProperties.Connection.Value = oleDbConnString.ToString();

                            // Modify the SQL command to include the list of session IDs.
                            string sqlCommand = connection.DatabaseProperties.Command.Value;
                            connection.DatabaseProperties.Command.Value = sqlCommand.Replace("'{SessionId}'", sessionIdList);
                        }

                        if (connection?.DatabaseProperties != null &&
                            connection.DatabaseProperties.Connection.Value.StartsWith("DRIVER="))
                        {
                            DbConnectionStringBuilder dbConnectionString = new DbConnectionStringBuilder(true);
                            dbConnectionString.ConnectionString = connection.DatabaseProperties.Connection.Value;
                            dbConnectionString["SERVER"] = GlobalSettings.Items[Setting.ReportingDatabaseServer];
                            dbConnectionString["UID"] = "report_viewer";
                            dbConnectionString["PWD"] = "report_viewer";

                            connection.DatabaseProperties.Connection.Value = dbConnectionString.ConnectionString;
                            string sqlCommand = connection.DatabaseProperties.Command.Value;
                            connection.DatabaseProperties.Command.Value = sqlCommand.Replace("'{SessionId}'", sessionIdList);
                        }

                       
                    }
                }

                // Modify a few of the workbook package properties.
                outputWorkbook.PackageProperties.Created = DateTime.Now;
                outputWorkbook.PackageProperties.Modified = DateTime.Now;
                outputWorkbook.PackageProperties.Creator = string.Empty;
                outputWorkbook.PackageProperties.LastModifiedBy = string.Empty;
                if (outputWorkbook?.WorkbookPart?.Workbook?.AbsolutePath != null)
                {
                    outputWorkbook.WorkbookPart.Workbook.AbsolutePath = null;
                }

                // Save the output workbook.
                outputWorkbook.WorkbookPart.Workbook.Save();
            }

            // Save the staged Excel file.
            File.Copy(stagedFilename, destinationPath, true);

            // Try to delete the temporary staged Excel file.
            try
            {
                File.Delete(stagedFilename);
            }
            catch (Exception)
            {
            }
        }

    }
}
