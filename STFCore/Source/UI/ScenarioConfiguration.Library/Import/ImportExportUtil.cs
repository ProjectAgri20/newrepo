using System;
using System.IO;
using System.Linq;
using System.Text;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    /// <summary>
    /// Defines the entities that are importable and/or exportable.
    /// </summary>
    public enum ImportExportType
    {
        Scenario,
        Document,
        Printer,
        Installer,
        Metadata
    }

    /// <summary>
    /// Builds file type information for import/export files.
    /// </summary>
    public static class ImportExportFile
    {
        /// <summary>
        /// Returns the import/export file extension for the specified ImportExportType.
        /// </summary>
        /// <param name="type">The ImportExportType.</param>
        /// <returns>The respective file extension.</returns>
        public static string Extension(this ImportExportType type)
        {
            var ext = "stbs";
            switch (type)
            {
                case ImportExportType.Document:
                    ext = "stbd";
                    break;
                case ImportExportType.Printer:
                    ext = "stbp";
                    break;
                case ImportExportType.Installer:
                    ext = "stbi";
                    break;
                case ImportExportType.Metadata:
                    ext = "plugindata";
                    break;
            }

            return ext;
        }

        /// <summary>
        /// Returns the import/export file filter for the specified ImportExportType.
        /// </summary>
        /// <param name="type">The ImportExportType</param>
        /// <returns>The respective file filter.</returns>
        public static string Filter(this ImportExportType type)
        {
            var filter = "STB Test Scenario Export File|*.stbs|All files (*.*)|*.*";
            switch (type)
            {
                case ImportExportType.Document:
                    filter = "STB Test Document Export File|*.stbd|All files (*.*)|*.*";
                    break;
                case ImportExportType.Printer:
                    filter = "STB Print Device Export File|*.stbp|All files (*.*)|*.*";
                    break;
                case ImportExportType.Installer:
                    filter = "STB Software Installer Export File|*.stbi|All files (*.*)|*.*";
                    break;
                case ImportExportType.Metadata:
                    filter = "Plugin Data File|*.plugindata|All Files (*.*)|*.*";
                    break;
            }

            return filter;
        }
    }

    /// <summary>
    /// Utility class for Import/Export operations.
    /// </summary>
    public static class ImportExportUtil
    {
        /// <summary>
        /// Writes the specified TestDocument to a shared location on the Document Library Server.
        /// </summary>
        /// <param name="document">The test document to be written.</param>
        /// <param name="rootShare">The document library "root" folder share location.</param>
        /// <param name="data">The document data.</param>
        public static void WriteDocumentToServer(TestDocument document, string rootShare, string data)
        {
            var destination = Path.Combine
                (
                    rootShare,
                    document.TestDocumentExtension.Location,
                    document.FileName
                );

            File.WriteAllBytes(destination, Convert.FromBase64String(data));
        }

        /// <summary>
        /// Copies the specified TestDocument to a shared location on the Document Library Server.
        /// </summary>
        /// <param name="document">The test document to be copied.</param>
        /// <param name="shareRoot">The document library "root" folder share location.</param>
        /// <param name="sourceFile">The filepath of the source file.</param>
        /// <param name="overwrite">Whether to overwrite if the file already exists.</param>
        public static void CopyDocumentToServer(TestDocument document, string shareRoot, string sourceFile, bool overwrite = true)
        {
            var destination = Path.Combine
                (
                    shareRoot,
                    document.TestDocumentExtension.Location,
                    document.FileName
                );

            File.Copy(sourceFile, destination, overwrite);
        }

        /// <summary>
        /// Saves documents from the specified composite contract to the Document Library.
        /// </summary>
        /// <param name="composite">The EnterpriseScenarioCompositeContract.</param>
        /// <returns>true if the operation completed successfully.</returns>
        public static bool ProcessCompositeContractFile(EnterpriseScenarioCompositeContract composite)
        {
            using (DocumentLibraryContext context = DbConnect.DocumentLibraryContext())
            {
                bool changesMade = false;

                foreach (DocumentContract documentContract in composite.Documents)
                {
                    if (CanInsertDocument(documentContract, context))
                    {
                        TestDocument documentEntity = ContractFactory.Create(context, documentContract);
                        WriteDocumentToServer(documentEntity, GlobalSettings.Items[Setting.DocumentLibraryServer], documentContract.Data);
                        context.TestDocuments.Add(documentEntity);
                        changesMade = true;
                    }
                }

                if (changesMade)
                {
                    context.SaveChanges();
                }
            }

            return true;
        }

        /// <summary>
        /// Can insert the document if:
        /// 1.  DocumentContract.Data is not empty AND
        /// 2.  A document with the same file name doesn't already exist in the database.
        /// </summary>
        /// <param name="contract">The DocumentContract.</param>
        /// <param name="context">The document Database context.</param>
        /// <returns></returns>
        private static bool CanInsertDocument(DocumentContract contract, DocumentLibraryContext context)
        {
            return (!string.IsNullOrEmpty(contract.Data) && 
                !context.TestDocuments.Any(x => x.FileName.Equals(contract.FileName, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
