using System;
using Microsoft.SharePoint.Client;

namespace HP.ScalableTest.SharePoint
{
    /// <summary>
    /// A document retrieved from a SharePoint document library.
    /// </summary>
    public sealed class SharePointDocument
    {
        /// <summary>
        /// Gets the SharePoint ID for this item.
        /// </summary>
        internal int ItemId { get; }

        /// <summary>
        /// Gets the file URL on the SharePoint server.
        /// </summary>
        internal string RelativeUrl { get; }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Gets the date the file was created.
        /// </summary>
        public DateTimeOffset Created { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDocument" /> class.
        /// </summary>
        /// <param name="item">The SharePoint <see cref="ListItem" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item" /> is null.</exception>
        internal SharePointDocument(ListItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            ItemId = item.Id;
            RelativeUrl = (string)item["FileRef"];
            FileName = (string)item["FileLeafRef"];

            // SharePoint stores the created date in UTC
            // Convert this to local time to be consistent with the rest of the framework
            Created = ((DateTime)item["Created"]).ToLocalTime();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() => FileName;
    }
}
