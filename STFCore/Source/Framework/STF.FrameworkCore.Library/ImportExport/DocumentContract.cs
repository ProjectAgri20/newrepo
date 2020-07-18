using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using HP.ScalableTest.Core.DocumentLibrary;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Document data Contract (used for import/export).
    /// </summary>
    [DataContract(Name = "Document", Namespace = "")]
    public class DocumentContract
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public DocumentContract()
        { }

        /// <summary>
        /// Constructor for exporting document information.
        /// Includes the document Id and filename only.
        /// </summary>
        /// <param name="document"></param>
        public DocumentContract(TestDocument document)
        {
            DocumentId = document.TestDocumentId;
            FileName = document.FileName;
        }

        /// <summary>
        /// Constructor for exporting document information.
        /// Includes the document metadata as well as the serialized bits of the entire document.
        /// </summary>
        /// <param name="document">The document metadata from the library database.</param>
        /// <param name="documentPath">The file path to the actual document that will be serialized.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="SecurityException"></exception>
        public DocumentContract(TestDocument document, string documentPath)
        {
            Application = document.Application;
            AppVersion = document.AppVersion;
            Author = document.Author;
            AuthorType = document.AuthorType;
            ColorMode = document.ColorMode;
            DefectId = document.DefectId;
            DocumentId = document.TestDocumentId;
            Extension = document.Extension;
            FileName = document.FileName;
            FileSize = document.FileSize;
            FileType = document.FileType;
            Notes = document.Notes;
            Orientation = document.Orientation;
            Pages = document.Pages;
            SubmitDate = document.SubmitDate;
            Submitter = document.Submitter;
            Tag = document.Tag;
            Vertical = document.Vertical;

            try
            {
                var bytes = File.ReadAllBytes(documentPath);
                Data = Convert.ToBase64String(bytes);
            }
            catch (ArgumentException ex)
            {
                string msg = "An Argument Exception occured trying to include the following file: {0}. Error Message: {1}".FormatWith(documentPath, ex.Message);
                throw new ArgumentException(msg, ex);
            }
            catch (PathTooLongException ex)
            {
                string msg = "A Path Too Long Exception occured trying to include the following file: {0}. Error Message: {1}".FormatWith(documentPath, ex.Message);
                throw new PathTooLongException(msg, ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                string msg = "The Directory was not found trying to include the following file: {0}. Error Message: {1}".FormatWith(documentPath, ex.Message);
                throw new DirectoryNotFoundException(msg, ex);
            }
            catch (IOException ex)
            {
                string msg = "An IO Exception occured trying to include the following file: {0}. Error Message: {1}".FormatWith(documentPath, ex.Message);
                throw new IOException(msg, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                string msg = "An Unauthorized Access Exception occured trying to include the following file: {0}. Error Message: {1}".FormatWith(documentPath, ex.Message);
                throw new UnauthorizedAccessException(msg, ex);
            }
            catch (NotSupportedException ex)
            {
                string msg = "A Not Supported Exception occured trying to include the following file: {0}. Error Message: {1}".FormatWith(documentPath, ex.Message);
                throw new NotSupportedException(msg, ex);
            }
            catch (SecurityException ex)
            {
                string msg = "A Security Exception occured trying to include the following file: {0}. Error Message: {1}".FormatWith(documentPath, ex.Message);
                throw new SecurityException(msg, ex);
            }
            catch (Exception ex)
            {
                string msg = "An exception occured trying to include the following file: {0}. Error Message: {1}".FormatWith(documentPath, ex.Message);
                throw new SecurityException(msg, ex);
            }
        }

        /// <summary>
        /// Application 
        /// </summary>
        [DataMember]
        public string Application { get; set; }

        /// <summary>
        /// Version of the application
        /// </summary>
        [DataMember]
        public string AppVersion { get; set; }

        /// <summary>
        /// Author of the document
        /// </summary>
        [DataMember]
        public string Author { get; set; }

        /// <summary>
        /// Author type
        /// </summary>
        [DataMember]
        public string AuthorType { get; set; }

        /// <summary>
        /// Indicates if it Color or Mono
        /// </summary>
        [DataMember]
        public string ColorMode { get; set; }

        /// <summary>
        /// Associated Defect (if any)
        /// </summary>
        [DataMember]
        public string DefectId { get; set; }

        /// <summary>
        /// Id for the document
        /// </summary>
        [DataMember]
        public Guid DocumentId { get; set; }

        /// <summary>
        /// File Extension
        /// </summary>
        [DataMember]
        public string Extension { get; set; }

        /// <summary>
        /// File Name
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        [DataMember]
        public long FileSize { get; set; }

        /// <summary>
        /// File Type
        /// </summary>
        [DataMember]
        public string FileType { get; set; }

        /// <summary>
        /// Any Notes associated with the document
        /// </summary>
        [DataMember]
        public string Notes { get; set; }

        /// <summary>
        /// Landscape or Portrait
        /// </summary>
        [DataMember]
        public string Orientation { get; set; }

        /// <summary>
        /// Number of Pages
        /// </summary>
        [DataMember]
        public int Pages { get; set; }

        /// <summary>
        /// Submission Date
        /// </summary>
        [DataMember]
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// Submitter of the document
        /// </summary>
        [DataMember]
        public string Submitter { get; set; }

        /// <summary>
        /// Custom Tags
        /// </summary>
        [DataMember]
        public string Tag { get; set; }

        /// <summary>
        /// Vertical
        /// </summary>
        [DataMember]
        public string Vertical { get; set; }

        /// <summary>
        /// The Base64String of the actual bytes of the document.
        /// </summary>
        [DataMember]
        public string Data { get; set; }

        
    }
}
