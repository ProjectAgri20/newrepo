using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using HP.ScalableTest.Framework.Manifest;
using System.Collections.Generic;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Service contract for the Session Proxy.
    /// </summary>
    [ServiceContract]
    public interface ISessionProxy
    {
        /// <summary>
        /// Initiates a new session based on the specified session ticket
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ownerCallback">The owner callback.</param>
        [OperationContract]
        void Initiate(SessionTicket ticket, Uri ownerCallback);

        /// <summary>
        /// Reserves all assets associated with this session using the specified key.
        /// </summary>
        /// <param name="reservationKey">The reservation key.</param>
        /// <returns></returns>
        [OperationContract]
        AssetDetailCollection Reserve(string reservationKey);

        /// <summary>
        /// Starts the enterprise scenario based on the provided configuration.
        /// </summary>
        /// <param name="assets">The assets.</param>
        [OperationContract]
        void Stage(AssetDetailCollection assets);

        /// <summary>
        /// Validates all assets and components used in this session.
        /// </summary>
        [OperationContract]
        void Validate();

        /// <summary>
        /// Signals the dispatcher to validate again.
        /// </summary>
        [OperationContract]
        void Revalidate();

        /// <summary>
        /// Powers on all assets and components used in this session.
        /// </summary>
        [OperationContract(Name = "PowerUpImmediate")]
        void PowerUp();

        /// <summary>
        /// Powers on all assets and components used in this session on a defined schedule
        /// </summary>
        /// <param name="schedule">The delay.</param>
        [OperationContract(Name ="PowerUpOnSchedule")]
        void PowerUp(SessionStartSchedule schedule);

        /// <summary>
        /// Runs the specified session.
        /// </summary>
        [OperationContract]
        void Run();

        /// <summary>
        /// Signals the dispatcher to repeat execution of the loaded session.
        /// </summary>
        [OperationContract]
        void Repeat();

        /// <summary>
        /// Signals the dispatcher to get the offline devices of the loaded session.
        /// </summary>
        [OperationContract]
        HashSet<string> GetSessionOfflineDevices();

        /// <summary>
        /// Signals the dispatcher to set the offline devices of the loaded session. 
        /// </summary>
        /// <param name="onlineDevices">Offline Devices</param>
        [OperationContract]
        void SetSessionOfflineDevices(HashSet<string> onlineDevices);

        /// <summary>
        /// Shuts down the executing session, including resources, etc.
        /// </summary>
        /// <param name="options">The shutdown options.</param>
        [OperationContract]
        void Shutdown(ShutdownOptions options);

        /// <summary>
        /// Pauses the session.
        /// </summary>
        [OperationContract]
        void Pause();

        /// <summary>
        /// Resumes execution of a paused session.
        /// </summary>
        [OperationContract]
        void Resume();

        /// <summary>
        /// Restarts the designated machine which can help when it gets stuck during startup
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="replaceMachine">if set to <c>true</c> then replace this machine with a new instance.</param>
        [OperationContract]
        void RestartMachine(string machineName, bool replaceMachine);

        /// <summary>
        /// Restarts the specified asset.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        [OperationContract]
        void RestartAsset(string assetId);

        /// <summary>
        /// Suspends operations to an Asset so that no jobs are being sent to it.
        /// </summary>
        [OperationContract]
        void TakeOffline(string assetId);

        /// <summary>
        /// Resumes normal operations to an Asset.
        /// </summary>
        [OperationContract]
        void BringOnline(string assetId);

        /// <summary>
        /// Sets the CRC on or off.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="crcOn">if set to <c>true</c> [CRC on].</param>
        [OperationContract]
        void SetCrc(string sessionId, string assetId, bool crcOn);

        /// <summary>
        /// Pauses execution for a single virtual resource.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        [OperationContract]
        void PauseWorker(string userName);

        /// <summary>
        /// Resumes execution for a single virtual resource.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        [OperationContract]
        void ResumeWorker(string userName);

        /// <summary>
        /// Halts execution for a single virtual resource.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        [OperationContract]
        void HaltWorker(string userName);

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
        void RefreshSubscription(Uri subscriber);

        /// <summary>
        /// Refreshes status for all subscribed listeners
        /// </summary>
        [OperationContract]
        void RefreshAllSubscriptions();

        /// <summary>
        /// Checks the subscriber's subscription to see if it's still active.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns>true if the subscription is still active, otherwise false</returns>
        [OperationContract]
        bool CheckSubscription(Uri subscriber);

        /// <summary>
        /// Pings this instance.
        /// </summary>
        [OperationContract]
        void Ping();


        /// <summary>
        /// Gets the current Session State
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        SessionState GetSessionState();

        /// <summary>
        /// Gets the current Session Startup State
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        SessionStartupTransition GetSessionStartupState();

    }
}
