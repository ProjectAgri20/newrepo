using System.Collections.ObjectModel;
using System.ServiceModel;

namespace HP.ScalableTest.PluginSupport.Hpcr
{
    /// <summary>
    /// Service contract for the HPCR configuration proxy service.
    /// </summary>
    [ServiceContract]
    public interface IHpcrConfigurationProxyService
    {
        /// <summary>
        /// Gets group memberships for the specified user.
        /// </summary>
        /// <param name="hpcrServerAddress">The HPCR server address.</param>
        /// <param name="userEmailAddress">The user email address.</param>
        /// <returns>A collection of group memberships.</returns>
        [OperationContract]
        Collection<Membership> GetGroupMemberships(string hpcrServerAddress, string userEmailAddress);

        /// <summary>
        /// Gets the distributions available to the specified user.
        /// </summary>
        /// <param name="hpcrServerAddress">The HPCR server address.</param>
        /// <param name="userEmailAddress">The user email address.</param>
        /// <returns>A collection of distributions.</returns>
        [OperationContract]
        Collection<Distribution> GetDistributions(string hpcrServerAddress, string userEmailAddress);

        /// <summary>
        /// Gets a list of configured users from the HPCR server.
        /// </summary>
        /// <param name="hpcrServerAddress">The HPCR server address.</param>
        /// <returns>A collection of configured users.</returns>
        [OperationContract]
        Collection<string> GetConfiguredUsers(string hpcrServerAddress);
    }
}
