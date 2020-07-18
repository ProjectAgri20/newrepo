using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Collection of virtual resource manifest details at the base class level
    /// </summary>
    [DataContract]
    public class ResourceDetailCollection : Collection<ResourceDetailBase>
    {
        /// <summary>
        /// Selects the name of the activity by metadata Id.
        /// </summary>
        /// <param name="metadataId">The metadata unique identifier.</param>
        /// <returns></returns>
        public string SelectActivityName(Guid metadataId)
        {
            var metadata =
                (
                    from r in this
                    from m in r.MetadataDetails
                    where m.Id == metadataId
                    select m
                ).FirstOrDefault();

            string name = metadataId.ToString();

            if (metadata != null)
            {
                name = metadata.Name;
            }

            return name;
        }

        /// <summary>
        /// Gets the office worker by the given username, only applies to OfficeWorker types.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public OfficeWorkerDetail GetByUsername(string userName)
        {
            return
            (
                from r in this.OfType<OfficeWorkerDetail>()
                select r into worker
                from w in worker.UserCredentials
                where w.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                select worker
            ).FirstOrDefault();
        }

        /// <summary>
        /// Gets virtual resource details of a given resource Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourceId">The resource unique identifier.</param>
        /// <returns>A <see cref="ResourceDetailBase"/> that represents the virtual resource.</returns>
        public T GetResource<T>(Guid resourceId) where T : ResourceDetailBase
        {
            return
            (
                from r in this
                where r.ResourceId == resourceId
                select (T)r
            ).FirstOrDefault();
        }

        /// <summary>
        /// Gets the worker
        /// </summary>
        /// <param name="instanceId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetWorker<T>(string instanceId) where T : OfficeWorkerDetail
        {
            return
            (
                from r in this.OfType<T>()
                select r into worker
                from w in worker.UserCredentials
                where w.ResourceInstanceId.Equals(instanceId)
                select worker
            ).FirstOrDefault();
        }

        /// <summary>
        /// Gets the user count from the collection of user credentials.
        /// </summary>
        public int UserCount
        {
            get
            {
                var credentials = Credentials;
                return credentials.Count() > 0 ? credentials.Count() : 1;
            }
        }

        /// <summary>
        /// Gets the credentials for all Office Workers.
        /// </summary>
        public IEnumerable<OfficeWorkerCredential> Credentials
        {
            get
            {
                IEnumerable<OfficeWorkerDetail> workers = this.OfType<OfficeWorkerDetail>();
                return workers != null ? workers.SelectMany(x => x.UserCredentials).ToList() : new List<OfficeWorkerCredential>();
            }
        }

        /// <summary>
        /// Gets the external credentials for all Office Workers.
        /// </summary>
        public IEnumerable<ExternalCredentialDetail> ExternalCredentials
        {
            get
            {
                IEnumerable<OfficeWorkerDetail> workers = this.OfType<OfficeWorkerDetail>();
                return workers != null ? workers.SelectMany(x => x.ExternalCredentials).ToList() : new List<ExternalCredentialDetail>();
            }
        }

        /// <summary>
        /// Gets a collection of manifest details by resource type.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns></returns>
        public IEnumerable<ResourceDetailBase> GetByType(VirtualResourceType resourceType)
        {
            return
            (
                from r in this
                where r.ResourceType == resourceType
                select r
            );
        }
    }
}
