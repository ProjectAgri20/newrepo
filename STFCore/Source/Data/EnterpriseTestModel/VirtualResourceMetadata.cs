using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    public partial class VirtualResourceMetadata
    {
        /// <summary>
        /// Selects a <see cref="VirtualResourceMetadata"/> by its id.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static VirtualResourceMetadata Select(EnterpriseTestEntities entities, Guid id)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from n in entities.VirtualResourceMetadataSet
                    where n.VirtualResourceMetadataId == id
                    select n).FirstOrDefault();
        }

        /// <summary>
        /// Selects all <see cref="VirtualResourceMetadata"/> objects with ids in the specified list.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public static IQueryable<VirtualResourceMetadata> Select(EnterpriseTestEntities entities, IEnumerable<Guid> ids)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return from n in entities.VirtualResourceMetadataSet
                   where ids.Contains(n.VirtualResourceMetadataId)
                   select n;
        }

        /// <summary>
        /// Selects a collection of <see cref="VirtualResourceMetadata"/> by its VirtualResourceType
        /// </summary>
        /// <param name="entities">The data context</param>
        /// <param name="resourceType">The VirtualResourceType.</param>
        /// <returns></returns>
        public static IQueryable<VirtualResourceMetadata> Select(EnterpriseTestEntities entities, VirtualResourceType resourceType)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            string resourceTypeString = resourceType.ToString();
            return from n in entities.VirtualResourceMetadataSet
                   where n.ResourceType.Equals(resourceTypeString, StringComparison.OrdinalIgnoreCase)
                   select n;
        }

        /// <summary>
        /// Selects a <see cref="VirtualResourceMetadata"/> by its id.
        /// Loads all Asset, Document, Server and PrintQueue usages before returning.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static VirtualResourceMetadata SelectWithUsage(EnterpriseTestEntities entities, Guid id)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }
            
            return (from n in entities.VirtualResourceMetadataSet
                    .Include("AssetUsage")
                    .Include("DocumentUsage")
                    .Include("PrintQueueUsage")
                    .Include("ServerUsage")
                    where n.VirtualResourceMetadataId == id
                    select n).FirstOrDefault();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResourceMetadata"/> class.
        /// </summary>
        internal VirtualResourceMetadata()
        {
            // We don't use this constructor, but entity framework does
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResourceMetadata"/> class.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="metadataType">Type of the metadata.</param>
        public VirtualResourceMetadata(string resourceType, string metadataType)
        {
            VirtualResourceMetadataId = SequentialGuid.NewGuid();
            Name = "{0} {1}".FormatWith(metadataType, DateTime.Now.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture));
            ResourceType = resourceType;
            MetadataType = metadataType;
            Metadata = string.Empty;
            Enabled = true;
        }

        /// <summary>
        /// Creates a copy of this <see cref="VirtualResourceMetadata"/>.
        /// </summary>
        /// <returns></returns>
        public VirtualResourceMetadata Copy(bool includeRetrySettings)
        {
            VirtualResourceMetadata metadata = new VirtualResourceMetadata
            {
                VirtualResourceMetadataId = SequentialGuid.NewGuid(),
                Name = Name,
                ResourceType = ResourceType,
                MetadataType = MetadataType,
                Metadata = Metadata,
                MetadataVersion = MetadataVersion,
                Enabled = Enabled,
                ExecutionPlan = ExecutionPlan
            };

            if (this.AssetUsage != null)
            {
                metadata.AssetUsage = new VirtualResourceMetadataAssetUsage();
                metadata.AssetUsage.AssetSelectionData = this.AssetUsage.AssetSelectionData;
            }

            if (this.DocumentUsage != null)
            {
                metadata.DocumentUsage = new VirtualResourceMetadataDocumentUsage();
                metadata.DocumentUsage.DocumentSelectionData = this.DocumentUsage.DocumentSelectionData;
            }

            if (this.ServerUsage != null)
            {
                metadata.ServerUsage = new VirtualResourceMetadataServerUsage();
                metadata.ServerUsage.ServerSelectionData = this.ServerUsage.ServerSelectionData;
            }

            if (this.PrintQueueUsage != null)
            {
                metadata.PrintQueueUsage = new VirtualResourceMetadataPrintQueueUsage();
                metadata.PrintQueueUsage.PrintQueueSelectionData = this.PrintQueueUsage.PrintQueueSelectionData;
            }

            if (includeRetrySettings)
            {
                foreach (var retrySetting in this.VirtualResourceMetadataRetrySettings)
                {
                    VirtualResourceMetadataRetrySetting copiedSetting = new VirtualResourceMetadataRetrySetting
                    {
                        SettingId = SequentialGuid.NewGuid(),
                        State = retrySetting.State,
                        Action = retrySetting.Action,
                        RetryLimit = retrySetting.RetryLimit,
                        RetryDelay = retrySetting.RetryDelay,
                        LimitExceededAction = retrySetting.LimitExceededAction
                    };
                    metadata.VirtualResourceMetadataRetrySettings.Add(copiedSetting);
                }
            }

            return metadata;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}