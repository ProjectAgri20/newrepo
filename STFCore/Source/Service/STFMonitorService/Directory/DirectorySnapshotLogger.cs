using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Service.Monitor.Directory
{
    internal class DirectorySnapshotLogger : FrameworkDataLog
    {
        public override string TableName => "DirectorySnapshot";

        public override string PrimaryKeyColumn => nameof(DirectorySnapshotId);

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectorySnapshotLogger"/> class.
        /// </summary>
        public DirectorySnapshotLogger()
        {
            DirectorySnapshotId = SequentialGuid.NewGuid();
            SnapshotDateTime = DateTime.Now;
            HostName = Environment.MachineName;
        }

        /// <summary>
        /// Gets or sets the directory snapshot ID.
        /// </summary>
        /// <value>The directory snapshot ID.</value>
        [DataLogProperty]
        public Guid DirectorySnapshotId { get; set; }

        /// <summary>
        /// Gets or sets the snapshot date time.
        /// </summary>
        /// <value>The snapshot date time.</value>
        [DataLogProperty]
        public DateTimeOffset SnapshotDateTime { get; set; }

        /// <summary>
        /// Gets or sets the host name.
        /// </summary>
        /// <value>The host name.</value>
        [DataLogProperty]
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the directory name.
        /// </summary>
        /// <value>The directory name.</value>
        [DataLogProperty(MaxLength = 1024)]
        public string DirectoryName { get; set; }

        /// <summary>
        /// Gets or sets the file count.
        /// </summary>
        /// <value>The file count.</value>
        [DataLogProperty]
        public int TotalFiles { get; set; }

        /// <summary>
        /// Gets or sets the total byte count.
        /// </summary>
        /// <value>The byte count.</value>
        [DataLogProperty]
        public long TotalBytes { get; set; }
    }
}
