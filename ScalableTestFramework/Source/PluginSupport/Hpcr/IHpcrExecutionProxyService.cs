using System.Collections.ObjectModel;
using System.ServiceModel;

namespace HP.ScalableTest.PluginSupport.Hpcr
{
    /// <summary>
    /// Service contract for the HPCR execution proxy service.
    /// </summary>
    [ServiceContract]
    public interface IHpcrExecutionProxyService
    {
        /// <summary>
        /// Gets the distributions available to the specified user.
        /// </summary>
        /// <param name="hpcrServerAddress">The HPCR server address.</param>
        /// <param name="userEmailAddress">The user email address.</param>
        /// <returns>A collection of distributions.</returns>
        [OperationContract]
        Collection<Distribution> GetDistributions(string hpcrServerAddress, string userEmailAddress);

        /// <summary>
        /// Delivers a document to the specified HPCR distribution.
        /// </summary>
        /// <param name="hpcrServerAddress">The HPCR server address.</param>
        /// <param name="documentFilePath">The document file path.</param>
        /// <param name="originatorEmail">The originator email.</param>
        /// <param name="distributionTitle">The HPCR distribution title.</param>
        [OperationContract]
        void DeliverToDistribution(string hpcrServerAddress, string documentFilePath, string originatorEmail, string distributionTitle);

        /// <summary>
        /// Delivers a document to the specified email address.
        /// </summary>
        /// <param name="hpcrServerAddress">The HPCR server address.</param>
        /// <param name="documentFilePath">The document file path.</param>
        /// <param name="originatorEmail">The originator email.</param>
        /// <param name="destinationEmail">The destination email.</param>
        [OperationContract]
        void DeliverToEmailByDocument(string hpcrServerAddress, string documentFilePath, string originatorEmail, string destinationEmail);
    }
}
