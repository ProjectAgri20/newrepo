using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;

namespace HP.ScalableTest.SharePoint
{
    /// <summary>
    /// Defines the query used for retrieval of SharePoint documents.
    /// </summary>
    /// <remarks>
    /// SharePoint uses a schema called CAML to execute queries against list data.
    /// This class forms a wrapper around the CAML format that can generate queries
    /// against a document library based on the set property values.
    /// </remarks>
    public sealed class SharePointDocumentQuery
    {
        /// <summary>
        /// Gets or sets the maximum number of documents to return.
        /// </summary>
        public int DocumentLimit { get; set; } = 0;

        /// <summary>
        /// Gets or sets the filter to use for the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the minimum amount of time the file must have existed on the file server.
        /// </summary>
        public TimeSpan CreationDelay { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDocumentQuery" /> class.
        /// </summary>
        public SharePointDocumentQuery()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Generates the corresponding CAML query for this instance.
        /// </summary>
        /// <returns>A <see cref="CamlQuery" /> object.</returns>
        internal CamlQuery GenerateCamlQuery()
        {
            XElement query =
                new XElement("View",
                    new XElement("Query",
                        new XElement("Where")
                    )
                );

            if (DocumentLimit > 0)
            {
                query.Add(new XElement("RowLimit", DocumentLimit.ToString()));
            }

            if (CreationDelay > TimeSpan.Zero)
            {
                DateTime earliestTimeUtc = DateTime.Now - CreationDelay;

                query.Element("Query").Element("Where").Add(
                    new XElement("Leq",
                        new XElement("FieldRef", new XAttribute("Name", "Created")),
                        new XElement("Value", new XAttribute("Type", "DateTime"), new XAttribute("IncludeTimeValue", "True"), earliestTimeUtc.ToString("s"))
                    )
                );
            }

            if (!string.IsNullOrEmpty(FileName))
            {
                query.Element("Query").Element("Where").Add(
                    new XElement("Contains",
                        new XElement("FieldRef", new XAttribute("Name", "FileLeafRef")),
                        new XElement("Value", new XAttribute("Type", "Text"), FileName)
                    )
                );
            }

            return new CamlQuery
            {
                ViewXml = query.ToString()
            };
        }
    }
}
