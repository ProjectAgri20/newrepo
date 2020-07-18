using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.SharePoint.Client;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.SharePoint
{
    /// <summary>
    /// Contains methods needed to access and manipulate documents in a SharePoint document library.
    /// </summary>
    public sealed class SharePointDocumentLibrary
    {
        /// <summary>
        /// Gets the library name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the URL of the SharePoint site for this library.
        /// </summary>
        public Uri SiteUrl { get; }

        /// <summary>
        /// Gets or sets the credential to use for accessing the SharePoint library.
        /// </summary>
        public NetworkCredential Credential { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDocumentLibrary" /> class.
        /// </summary>
        /// <param name="documentLibraryUrl">The document library URL.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentLibraryUrl" /> is null.</exception>
        public SharePointDocumentLibrary(string documentLibraryUrl)
            : this(new Uri(documentLibraryUrl))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDocumentLibrary" /> class.
        /// </summary>
        /// <param name="documentLibraryUrl">The document library URL.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentLibraryUrl" /> is null.</exception>
        public SharePointDocumentLibrary(Uri documentLibraryUrl)
        {
            if (documentLibraryUrl == null)
            {
                throw new ArgumentNullException(nameof(documentLibraryUrl));
            }

            SiteUrl = new Uri(documentLibraryUrl.GetLeftPart(UriPartial.Authority));
            Name = SiteUrl.MakeRelativeUri(documentLibraryUrl).ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDocumentLibrary" /> class.
        /// </summary>
        /// <param name="siteUrl">The SharePoint site URL.</param>
        /// <param name="libraryName">The document library name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="siteUrl" /> is null.</exception>
        public SharePointDocumentLibrary(string siteUrl, string libraryName)
            : this(new Uri(siteUrl), libraryName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDocumentLibrary" /> class.
        /// </summary>
        /// <param name="siteUrl">The SharePoint site URL.</param>
        /// <param name="libraryName">The document library name.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="siteUrl" /> is null.
        /// <para>or</para>
        /// <paramref name="libraryName" /> is null.
        /// </exception>
        public SharePointDocumentLibrary(Uri siteUrl, string libraryName)
        {
            SiteUrl = siteUrl ?? throw new ArgumentNullException(nameof(siteUrl));
            Name = libraryName ?? throw new ArgumentNullException(nameof(libraryName));
        }

        /// <summary>
        /// Retrieves all documents from this document library.
        /// </summary>
        /// <returns>A collection of <see cref="SharePointDocument" /> objects.</returns>
        public SharePointDocumentCollection Retrieve()
        {
            return Retrieve(new SharePointDocumentQuery());
        }

        /// <summary>
        /// Retrieves up to the specified number of documents from this document library.
        /// </summary>
        /// <param name="limit">The maximum number of documents to return.</param>
        /// <returns>A collection of <see cref="SharePointDocument" /> objects.</returns>
        public SharePointDocumentCollection Retrieve(int limit)
        {
            SharePointDocumentQuery query = new SharePointDocumentQuery
            {
                DocumentLimit = limit
            };
            return Retrieve(query);
        }

        /// <summary>
        /// Retrieves documents from this document library using the specified <see cref="SharePointDocumentQuery" />.
        /// </summary>
        /// <param name="query">A <see cref="SharePointDocumentQuery" /> object containing query parameters.</param>
        /// <returns>A collection of <see cref="SharePointDocument" /> objects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="query" /> is null.</exception>
        public SharePointDocumentCollection Retrieve(SharePointDocumentQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            LogDebug($"Retrieving documents from {Name} library at {ToString()}");

            // Declare this outside the context so we can close the connection as soon as we are done
            ListItemCollection listItems;

            using (ClientContext context = new ClientContext(SiteUrl))
            {
                context.Credentials = Credential;
                List list = context.Web.Lists.GetByTitle(Name);
                listItems = list.GetItems(query.GenerateCamlQuery());
                context.Load(listItems);
                context.ExecuteQuery();
            }

            return new SharePointDocumentCollection(listItems);
        }

        /// <summary>
        /// Deletes the specified documents from this document library.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documents" /> is null.</exception>
        public void Delete(IEnumerable<SharePointDocument> documents)
        {
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            LogDebug($"Deleting {documents.Count()} documents from {Name} library at {ToString()}");
            using (ClientContext context = new ClientContext(SiteUrl))
            {
                context.Credentials = Credential;
                List list = context.Web.Lists.GetByTitle(Name);
                foreach (SharePointDocument document in documents)
                {
                    ListItem doc = list.GetItemById(document.ItemId);
                    doc.DeleteObject();
                }
                context.ExecuteQuery();
            }
        }

        /// <summary>
        /// Downloads the specified document to the specified directory.
        /// </summary>
        /// <param name="document">The <see cref="SharePointDocument" /> to download.</param>
        /// <param name="directory">The directory to download the file into.</param>
        /// <returns>A <see cref="FileInfo" /> object representing the downloaded file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        public FileInfo Download(SharePointDocument document, string directory)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return Download(document, directory, document.FileName);
        }

        /// <summary>
        /// Downloads the specified document to the specified directory.
        /// </summary>
        /// <param name="document">The <see cref="SharePointDocument" /> to download.</param>
        /// <param name="directory">The directory to download the file into.</param>
        /// <param name="fileName">The file name to give the downloaded file</param>
        /// <returns>A <see cref="FileInfo" /> object representing the downloaded file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        public FileInfo Download(SharePointDocument document, string directory, string fileName)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            using (WebClient client = new WebClient())
            {
                if (Credential != null)
                {
                    client.Credentials = Credential;
                }
                else
                {
                    client.UseDefaultCredentials = true;
                }
                return DownloadFile(client, document, directory, fileName);
            }
        }

        /// <summary>
        /// Downloads the specified documents to the specified directory.
        /// </summary>
        /// <param name="documents">The collection of documents to download.</param>
        /// <param name="directory">The directory to download the documents into.</param>
        /// <returns>A collection of <see cref="FileInfo" /> objects representing the downloaded files.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="documents" /> is null.</exception>
        public IEnumerable<FileInfo> Download(IEnumerable<SharePointDocument> documents, string directory)
        {
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            List<FileInfo> fileInfoCollection = new List<FileInfo>();
            using (WebClient client = new WebClient())
            {
                if (Credential != null)
                {
                    client.Credentials = Credential;
                }
                else
                {
                    client.UseDefaultCredentials = true;
                }
                foreach (SharePointDocument document in documents)
                {
                    fileInfoCollection.Add(DownloadFile(client, document, directory, document.FileName));
                }
            }
            return fileInfoCollection;
        }

        private FileInfo DownloadFile(WebClient client, SharePointDocument document, string directory, string fileName)
        {
            LogDebug($"Downloading '{fileName} to {directory}");
            string filePath = Path.Combine(directory, fileName);
            Uri fileUrl = new Uri(SiteUrl, document.RelativeUrl);
            client.DownloadFile(fileUrl, filePath);
            return new FileInfo(filePath);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() => $@"{SiteUrl.Host}\{Name}";
    }
}
