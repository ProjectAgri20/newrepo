using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Contains details about badges used in a <see cref="BadgeBoxDetail"/>.
    /// </summary>
    [DataContract]
    public class BadgeDetail
    {
        /// <summary>
        /// Access or Mutate UniqueIdentifier of badge
        /// </summary>
        [DataMember]
        public Guid BadgeId { get; set; }
        /// <summary>
        /// Access or Mutate UniqueIdentifier of badgebox
        /// </summary>
        [DataMember]
        public Guid BadgeBoxId { get; set; }

        /// <summary>
        /// Access or Mutate Username
        /// </summary>
        [DataMember]
        public string Username { get; set; }
        /// <summary>
        /// Access or Mutate Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Access or Mutate Index
        /// </summary>
        [DataMember]
        public int Index { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeDetail"/> class.
        /// </summary>
        public BadgeDetail()
        {
            BadgeId = Guid.Empty;
            BadgeBoxId = Guid.Empty;
            Username = string.Empty;
            Description = string.Empty;
            Index = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeDetail"/> class.
        /// </summary>
        /// <param name="badgeId">The Badge identifier.</param>
        /// <param name="badgeBoxId">The BadgeBox identifier.</param>
        /// <param name="user">The Badge UserName</param>
        /// <param name="description">The Badge description</param>
        /// <param name="index">The Badge index.</param>
        public BadgeDetail(Guid badgeId, Guid badgeBoxId, string user, string description, int index)
        {
            BadgeId = badgeId;
            BadgeBoxId = badgeBoxId;
            Username = user;
            Description = description;
            Index = index;
        }

    }
}
