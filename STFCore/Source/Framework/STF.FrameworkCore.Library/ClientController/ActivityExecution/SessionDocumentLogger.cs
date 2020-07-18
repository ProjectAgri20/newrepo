using System;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Framework.ClientController.ActivityExecution
{
    /// <summary>
    /// A class for logging properities of documents used in a scenario.
    /// </summary>
    public class SessionDocumentLogger : FrameworkDataLog
    {
        public override string TableName => "SessionDocument";

        public override string PrimaryKeyColumn => nameof(SessionDocumentId);

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionDocumentLogger" /> class.
        /// </summary>
        public SessionDocumentLogger()
        {
        }

        [DataLogProperty]
        public Guid SessionDocumentId { get; set; }

        /// <summary>
        /// Gets or sets the Session Id.
        /// </summary>
        /// <value>The Session Id.</value>
        [DataLogProperty]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the Document Id.
        /// </summary>
        /// <value>The Document Id.</value>
        [DataLogProperty]
        public Guid DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        [DataLogProperty(MaxLength = 255)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>The file extension.</value>
        [DataLogProperty(MaxLength = 10)]
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>The type of the file.</value>
        [DataLogProperty]
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>The size of the file.</value>
        [DataLogProperty]
        public long FileSizeKilobytes { get; set; }

        /// <summary>
        /// Gets or sets the pages.
        /// </summary>
        /// <value>The pages.</value>
        [DataLogProperty]
        public short Pages { get; set; }

        /// <summary>
        /// Gets or sets the color mode.
        /// </summary>
        /// <value>The color mode.</value>
        [DataLogProperty(MaxLength = 10)]
        public string ColorMode { get; set; }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        [DataLogProperty(MaxLength = 10)]
        public string Orientation { get; set; }

        /// <summary>
        /// Gets or sets the Defect Id.
        /// </summary>
        /// <value>The Defect Id.</value>
        [DataLogProperty]
        public string DefectId { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        [DataLogProperty(MaxLength = 255)]
        public string Tag { get; set; }
    }
}
