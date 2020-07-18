using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Framework.Manifest;
using System.Collections.Generic;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Service contract for the Virtual Resource Dispatcher Service.
    /// </summary>
    [ServiceContract]
    public interface ISessionDispatcher
    {
        /// <summary>
        /// Initiates a new session based on the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ownerCallback">The owner callback.</param>
        [OperationContract]
        void Initiate(SessionTicket ticket, Uri ownerCallback);

        /// <summary>
        /// Reserves all assets associated with this session using the specified key.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="reservationKey">The reservation key.</param>
        /// <returns></returns>
        [OperationContract]
        AssetDetailCollection Reserve(string sessionId, string reservationKey);

        /// <summary>
        /// Starts the enterprise scenario based on the provided configuration.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="assets">The assets.</param>
        [OperationContract]
        void Stage(string sessionId, AssetDetailCollection assets);

        /// <summary>
        /// Validates all assets and components used in this session.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        [OperationContract]
        void Validate(string sessionId);

        /// <summary>
        /// Signals the dispatcher to validate again.
        /// </summary>
        [OperationContract]
        void Revalidate(string sessionId);

        /// <summary>
        /// Powers on all assets and components used in this session.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        [OperationContract(Name = "PowerUpImmediate")]
        void PowerUp(string sessionId);

        /// <summary>
        /// Powers up the VM
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="schedule"></param>
        [OperationContract(Name ="PowerUpOnSchedule")]
        void PowerUp(string sessionId, SessionStartSchedule schedule);

        /// <summary>
        /// Runs the specified session.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        [OperationContract]
        void Run(string sessionId);

        /// <summary>
        /// Signals the dispatcher to repeat execution of the loaded session.
        /// </summary>
        [OperationContract]
        void Repeat(string sessionId);

        /// <summary>
        /// Signals the dispatcher to get offline devices for the loaded session.
        /// </summary>
        [OperationContract]
        HashSet<string> GetSessionOfflineDevices(string sessionId);

        /// <summary>
        /// Signals the dispatcher to set offline devices for the loaded session.
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        /// <param name="onlineDevices">Offline Devices</param>
        [OperationContract]
        void SetSessionOfflineDevices(string sessionId, HashSet<string> onlineDevices);


        /// <summary>
        /// Sets the CRC.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="crcOn">if set to <c>true</c> [CRC on].</param>
        [OperationContract]
        void SetCrc(string sessionId, string assetId, bool crcOn);
        /// <summary>
        /// Shuts down the executing session, including resources, etc.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="options">The shutdown options.</param>
        [OperationContract]
        void Shutdown(string sessionId, ShutdownOptions options);

        /// <summary>
        /// Pauses the session.
        /// </summary>
        [OperationContract]
        void Pause(string sessionId);

        /// <summary>
        /// Resumes execution of a paused session.
        /// </summary>
        [OperationContract]
        void Resume(string sessionId);

        /// <summary>
        /// Gets the session log files.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        string GetSessionLogFiles(string sessionId);

        /// <summary>
        /// Closes the specified session and releases all associated resources
        /// </summary>
        [OperationContract]
        void Close(string sessionId);

        /// <summary>
        /// Closes the session and releases all associated resource with the shutdown option
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="options"></param>
        [OperationContract(Name = "CloseWithOptions")]
        void Close(string sessionId, ShutdownOptions options);

        /// <summary>
        /// Restarts the designated machine which can help when it gets stuck during startup
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="replaceMachine">if set to <c>true</c> then replace this machine with a new instance.</param>
        [OperationContract]
        void RestartMachine(string sessionId, string machineName, bool replaceMachine);

        /// <summary>
        /// Restarts the specified asset.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="assetId">The asset id.</param>
        [OperationContract]
        void RestartAsset(string sessionId, string assetId);

        /// <summary>
        /// Pauses execution for a single virtual resource.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="userName">Name of the user.</param>
        [OperationContract]
        void PauseWorker(string sessionId, string userName);

        /// <summary>
        /// Resumes execution for a single virtual resource.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="userName">Name of the user.</param>
        [OperationContract]
        void ResumeWorker(string sessionId, string userName);

        /// <summary>
        /// Halts execution for a single virtual resource.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="userName">Name of the user.</param>
        [OperationContract]
        void HaltWorker(string sessionId, string userName);

        /// <summary>
        /// Suspends activitiy execution to the specified asset.
        /// </summary>
        /// <param name="sessionId">The session unique identifier</param>
        /// <param name="assetId">The asset Id.</param>
        [OperationContract]
        void TakeAssetOffline(string sessionId, string assetId);

        /// <summary>
        /// Resumes activity execution to the specified asset.
        /// </summary>
        /// <param name="sessionId">The session unique identifier</param>
        /// <param name="assetId">The asset Id.</param>
        [OperationContract]
        void BringAssetOnline(string sessionId, string assetId);

        /// <summary>
        /// Subscribes the defined listener to dispatcher updates coming from the status publisher
        /// </summary>
        /// <param name="subscriber">The client listener</param>
        [OperationContract]
        void Subscribe(Uri subscriber);

        /// <summary>
        /// Unsubscribes the defined listener from dispatcher updates coming from the status publisher
        /// </summary>
        /// <param name="subscriber">The client listener</param>
        [OperationContract]
        void Unsubscribe(Uri subscriber);

        /// <summary>
        /// Refreshes the defined listener with current dispatcher status information.
        /// </summary>
        /// <param name="subscriber">The client listener.</param>
        [OperationContract]
        void RefreshSubscriber(Uri subscriber);

        /// <summary>
        /// Checks the subscriber's subscription to see if it's still active.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns>true if the subscription is still active, otherwise false</returns>
        [OperationContract]
        bool SubscriptionActive(Uri subscriber);

        /// <summary>
        /// Lets the dispatcher know that the proxy is started
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        [OperationContract]
        void NotifyProxyStarted(string sessionId);

        /// <summary>
        /// Returns the system database that this service instance is pointing to.
        /// </summary>
        [OperationContract]
        string GetSystemDatabase();

        /// <summary>
        /// Returns the whether there are active sessions running on the dispatcher
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool AreThereActiveSessions();

        /// <summary>
        /// Loads the specified user credential.
        /// </summary>
        /// <param name="credential"></param>
        [OperationContract]
        void SetUserCredential(UserCredential credential);

        /// <summary>
        /// Gets the current state for the session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [OperationContract]
        SessionState GetSessionState(string sessionId);

        /// <summary>
        /// Gets the current state of Session Startup
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [OperationContract]
        SessionStartupTransition GetSessionStartupState(string sessionId);
    }
}
