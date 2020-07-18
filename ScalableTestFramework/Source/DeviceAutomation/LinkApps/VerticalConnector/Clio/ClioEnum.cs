using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.Clio
{
    #region ClioJobType Enums
    /// <summary>
    /// Clio Destination enum
    /// </summary>
    public enum ClioJobType
    {
        /// <summary>
        /// Job Type: Print
        /// </summary>
        [Description("Print")]
        Print = 0,

        /// <summary>
        /// Job Type: Scan
        /// </summary>
        [Description("Scan")]
        Scan,
    }
    #endregion
    #region Kind of Storage locations
    /// <summary>
    /// Kind of Storage locations
    /// </summary>
    public enum StorageLocation
    {
        /// <summary>
        /// Location : Matter
        /// </summary>
        [Description("Recent Matters")]
        RecentMatters = 0,

        /// <summary>
        /// Location : Document
        /// </summary>
        [Description("Matters")]
        Matters,

        /// <summary>
        /// Job Type: Scan
        /// </summary>
        [Description("Document")]
        Document,
    }
    #endregion
}
