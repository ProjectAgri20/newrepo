using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Contract for Document Collection
    /// </summary>
    [CollectionDataContract(Name = "Documents", Namespace = "")]
    public class DocumentContractCollection : Collection<DocumentContract>
    {
        /// <summary>
        /// Exports the DocumentContractCollection to a file with the specified file name.
        /// </summary>
        /// <param name="fileName"></param>
        public void Export(string fileName)
        {
            File.WriteAllText(fileName, LegacySerializer.SerializeDataContract(this).ToString());
        }

        /// <summary>
        /// Loads the document.
        /// </summary>
        /// <param name="document">The Test docuement metadata from the document library database.</param>
        /// <param name="loadDocumentBits">Whether or not to serialize the document bits along with the metadata.</param>
        public void Load(TestDocument document, bool loadDocumentBits)
        {
            DocumentContract contract = null;

            if (loadDocumentBits)
            {
                // Find the path to the actual file
                string rootPath = GlobalSettings.Items[Setting.DocumentLibraryServer];
                string location = document.TestDocumentExtension.Location;
                string fileName = document.FileName;
                string fullPath = Path.Combine(rootPath, location, fileName);
                contract = ContractFactory.Create(document, fullPath);
            }
            else
            {
                contract = ContractFactory.Create(document);
            }

            Add(contract);
        }

        /// <summary>
        /// Loads the list of documents
        /// </summary>
        /// <param name="documents">Collection of Test docuement metadata from the document library database.</param>
        /// <param name="loadDocumentBits">Whether or not to serialize the document bits along with the metadata.</param>
        public void Load(IEnumerable<TestDocument> documents, bool loadDocumentBits)
        {
            foreach (var document in documents)
            {
                Load(document, loadDocumentBits);
            }
        }
    }
}
