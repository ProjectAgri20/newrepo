using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.SharePoint.Client;

namespace HP.ScalableTest.SharePoint
{
    /// <summary>
    /// A collection of <see cref="SharePointDocument" /> objects.
    /// </summary>
    public sealed class SharePointDocumentCollection : Collection<SharePointDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDocumentCollection" /> class.
        /// </summary>
        public SharePointDocumentCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDocumentCollection" /> class.
        /// </summary>
        /// <param name="listItems">The collection of SharePoint <see cref="ListItem" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="listItems" /> is null.</exception>
        internal SharePointDocumentCollection(IEnumerable<ListItem> listItems)
        {
            if (listItems == null)
            {
                throw new ArgumentNullException(nameof(listItems));
            }

            foreach (ListItem item in listItems)
            {
                Add(new SharePointDocument(item));
            }
        }
    }
}
