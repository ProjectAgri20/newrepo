using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a badge associated with a badge box.
    /// </summary>
    [DataContract]
    public class BadgeInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _badgeId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _userName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _index;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _description;

        /// <summary>
        /// Gets the badge identifier.
        /// </summary>
        public string BadgeId => _badgeId;

        /// <summary>
        /// Gets the name of the user associated with this badge.
        /// </summary>
        public string UserName => _userName;

        /// <summary>
        /// Gets the index of the badge in its associated badge box.
        /// </summary>
        public int Index => _index;

        /// <summary>
        /// Gets a description of the badge.
        /// </summary>
        public string Description => _description;

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeInfo" /> class.
        /// </summary>
        /// <param name="badgeId">The badge identifier.</param>
        /// <param name="user">The name of the user associated with this badge.</param>
        /// <param name="index">The index of the badge in its associated badge box.</param>
        /// <param name="description">The description of the badge.</param>
        public BadgeInfo(string badgeId, string user, int index, string description)
        {
            _badgeId = badgeId;
            _userName = user;
            _index = index;
            _description = description;
        }
    }
}
