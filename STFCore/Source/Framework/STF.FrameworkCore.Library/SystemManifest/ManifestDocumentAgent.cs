using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml.Linq;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.ClientController.ActivityExecution;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework
{
    public class ManifestDocumentAgent : IManifestComponentAgent
    {
        private readonly List<Document> _documents = new List<Document>();
        private readonly Dictionary<Guid, DocumentIdCollection> _activityDocuments = new Dictionary<Guid, DocumentIdCollection>();

        public IEnumerable<string> RequestedAssets
        {
            get { yield break; }
        }

        public ManifestDocumentAgent(Guid scenarioId)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                // Retrieve document usage data for all enabled activities in the specified session
                var activities = (from documentUsage in context.VirtualResourceMetadataDocumentUsages
                                  let data = documentUsage.DocumentSelectionData
                                  let metadata = documentUsage.VirtualResourceMetadata
                                  let resource = metadata.VirtualResource
                                  where resource.EnterpriseScenarioId == scenarioId
                                     && resource.Enabled == true
                                     && metadata.Enabled == true
                                     && data != null
                                  select new { Id = metadata.VirtualResourceMetadataId, Documents = data }).ToList();

                foreach (var activity in activities)
                {
                    DocumentSelectionData documentSelectionData = GetSelectionData(activity.Documents);
                    _activityDocuments.Add(activity.Id, GetDocuments(documentSelectionData));
                }
            }

            var documentIds = _activityDocuments.Values.SelectMany(n => n).Distinct().ToList();

            using (DocumentLibraryContext context = DbConnect.DocumentLibraryContext())
            {
                var documents = context.TestDocuments.Where(n => documentIds.Contains(n.TestDocumentId)).ToDocumentCollection();
                _documents.AddRange(documents);
            }
        }

        private static DocumentSelectionData GetSelectionData(string data)
        {
            return Serializer.Deserialize<DocumentSelectionData>(XElement.Parse(data));
        }

        private DocumentIdCollection GetDocuments(DocumentSelectionData documentSelectionData)
        {
            switch (documentSelectionData.SelectionMode)
            {
                case DocumentSelectionMode.SpecificDocuments:
                    return documentSelectionData.SelectedDocuments;

                case DocumentSelectionMode.DocumentSet:
                    return GetDocumentsFromSet(documentSelectionData.DocumentSetName);

                case DocumentSelectionMode.DocumentQuery:
                    return GetDocumentsFromQuery(documentSelectionData.DocumentQuery);

                default:
                    return new DocumentIdCollection();
            }
        }

        private static DocumentIdCollection GetDocumentsFromSet(string documentSetName)
        {
            using (DocumentLibraryContext context = DbConnect.DocumentLibraryContext())
            {
                var documentSet = context.TestDocumentSets.Include(n => n.TestDocumentSetItems).FirstOrDefault(n => n.Name == documentSetName);
                return new DocumentIdCollection(documentSet.TestDocumentSetItems.OrderBy(n => n.SortOrder).Select(n => n.TestDocumentId));
            }
        }

        private DocumentIdCollection GetDocumentsFromQuery(DocumentQuery queryCriteria)
        {
            DocumentLibraryController controller = new DocumentLibraryController(DbConnect.DocumentLibraryConnectionString);
            return new DocumentIdCollection(controller.GetDocuments(queryCriteria).Select(n => n.DocumentId));
        }

        public void AssignManifestInfo(SystemManifest manifest)
        {
            manifest.Documents.Clear();
            manifest.ActivityDocuments.Clear();

            foreach (Document document in _documents)
            {
                manifest.Documents.Add(document);
            }

            foreach (var activityDocument in _activityDocuments)
            {
                manifest.ActivityDocuments.Add(activityDocument.Key, activityDocument.Value);
            }
        }

        public void LogComponents(string sessionId)
        {
            var documentIds = _activityDocuments.Values.SelectMany(n => n).Distinct().ToList();
            using (DocumentLibraryContext context = DbConnect.DocumentLibraryContext())
            {
                foreach (var doc in context.TestDocuments.Where(n => documentIds.Contains(n.TestDocumentId)))
                {
                    SessionDocumentLogger logger = new SessionDocumentLogger
                    {
                        SessionDocumentId = SequentialGuid.NewGuid(),
                        SessionId = sessionId,
                        DocumentId = doc.TestDocumentId,
                        FileName = doc.FileName,
                        Extension = doc.Extension,
                        FileType = doc.FileType,
                        FileSizeKilobytes = doc.FileSize,
                        Pages = (short)doc.Pages,
                        ColorMode = doc.ColorMode,
                        Orientation = doc.Orientation,
                        DefectId = doc.DefectId,
                        Tag = doc.Tag
                    };
                    ExecutionServices.DataLogger.AsInternal().SubmitAsync(logger);
                }
            }
        }
    }
}
