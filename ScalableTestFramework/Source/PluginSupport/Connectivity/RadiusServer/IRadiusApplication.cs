using System.Collections.ObjectModel;
using System.ServiceModel;

namespace HP.ScalableTest.PluginSupport.Connectivity.RadiusServer
{
    [ServiceContract]
    public interface IRadiusApplication
    {
        /// <summary>
        /// Adds a client on the radius server.
        /// </summary>
        /// <param name="clientName">The name of the client.</param>
        /// <param name="address">The IP address of the client.</param>
        /// <param name="sharedSecret">The shared secret.</param>
        /// <returns>True if the client is added, else false.</returns>
        [OperationContract]
        bool AddClient(string clientName, string address, string sharedSecret);

        /// <summary>
        /// Deletes a client on the radius server.
        /// </summary>
        /// <param name="clientName">The name of the client.</param>
        /// <returns>True if the client is deleted, else false.</returns>
        [OperationContract]
        bool DeleteClient(string clientName);

        /// <summary>
        /// Get all the clients available in the radius server.
        /// </summary>
        /// <returns><see cref="RadiusClient"/></returns>
        [OperationContract]
        Collection<RadiusClient> GetAllClients();

        /// <summary>
        /// Deletes all the clients available in the radius server.
        /// </summary>
        /// <returns>True if all the clients are deleted, else false.</returns>
        [OperationContract]
        bool DeleteAllClients();

        /// <summary>
        /// Clears the network policy from the radius server.
        /// </summary>
        /// <returns>True if the network policy is cleared from the server.</returns>
        [OperationContract]
        bool ClearNetworkPolicy();

        /// <summary>
        /// Adds network policy on the radius server.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool AddNetworkPolicy();

        /// <summary>
        /// Sets the authentication mode on the radius server for the specified network policy.
        /// </summary>
        /// <param name="networkPolicy">The network policy name.</param>
        /// <param name="authenticationModes"><see cref="AuthenticationMode"/></param>
        /// <param name="priorityMode">The priority <see cref="AuthenticationMode"/> to be set on the server.</param>
        /// <returns>True if the configuration is successful, else false.</returns>
        [OperationContract]
        bool SetAuthenticationMode(string networkPolicy, AuthenticationMode authenticationModes, AuthenticationMode priorityMode = AuthenticationMode.None);

        /// <summary>
        /// Map Id certificate to the specified user.
        /// </summary>
        /// <param name="userName">The user name for which the certificate is to be mapped.</param>
        /// <param name="certificatePath">The path of the certificate.</param>
        /// <param name="certificatePassword">Password of the certificate.</param>
        /// <returns>True if the operation is successful, else false.</returns>
        [OperationContract]
        bool MapIdCertificate(string userName, string certificatePath, string certificatePassword = "");

        /// <summary>
        /// Delete Id certificate mapped to the specified user.
        /// </summary>
        /// <param name="userName">The user name for which the certificate is to be deleted.</param>
        /// <param name="certificatePath">The path of the certificate.</param>
        /// <param name="certificatePassword">Password of the certificate.</param>
        /// <returns>True if the operation is successful, else false.</returns>
        [OperationContract]
        bool DeleteIdCertificate(string userName, string certificatePath, string certificatePassword = "");

        /// <summary>
        /// Adds CA certificate to the certificate store.
        /// </summary>
        /// <param name="certificatePath">The path of the certificate.</param>
        /// <returns></returns>
        [OperationContract]
        bool AddCACertificate(string certificatePath);

        /// <summary>
        /// Deletes the CA certificate from certificate store.
        /// </summary>
        /// <param name="certificatePath">The path of the certificate.</param>
        /// <param name="certificatePassword"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCACertificate(string certificatePath);

        /// <summary>
        /// Restarts the Extensible Authentication Protocol and Network Policy Server services.
        /// </summary>
        [OperationContract]
        void RestartRadiusServices();

        /// <summary>
        /// Gets the SamAccountName of the specified active directory user.
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <returns>The SamAccountName of the specified user.</returns>
        [OperationContract]
        string GetADUserSamAccountName(string userName);

        /// <summary>
        /// Renmaes the SamAccountName of the specified active directory user.
        /// </summary>
        /// <param name="userName">The user name to be modified.</param>
        /// <param name="newName">The new user name</param>
        /// <returns>True if the SamAccountName is set, else false.</returns>
        [OperationContract]
        bool RenameADUser(string userName, string newName);
    }
}
