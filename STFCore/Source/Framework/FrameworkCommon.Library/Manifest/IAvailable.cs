using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Interface IAvailable is used by any class that contains a string identifier and needs to expose availability.
    /// </summary>
    public interface IAvailable
    {
        /// <summary>
        /// Gets the Id of this instance.
        /// </summary>
        /// <value>The Identifier of this instance.</value>
        string Id { get; }

        /// <summary>
        /// Gets the Inventory Id of this instance.
        /// </summary>
        string InventoryId { get; }

        /// <summary>
        /// Gets or sets the availability setting.
        /// </summary>
        /// <value>Whether or not this instance is available.</value>
        bool Available { get; set; }

        DateTime? AvailabilityStartTime { get; set; }

        /// <summary>
        /// Gets the Expiration Date/Time.
        /// </summary>
        DateTime? AvailabilityEndTime { get; set; }
    }
}
