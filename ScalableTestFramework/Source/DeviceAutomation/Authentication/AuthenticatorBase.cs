using System;
using System.Collections.Generic;
using System.Text;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Authentication
{
    /// <summary>
    /// Base class for an <see cref="IAuthenticator" /> implementation.
    /// </summary>
    public abstract class AuthenticatorBase : DeviceWorkflowLogSource, IAuthenticator
    {
        /// <summary>
        /// Maps the providers to text that is used to set and retrieve authentication methods on the device.
        /// </summary>
        protected Dictionary<AuthenticationProvider, ProviderMapItem> ProviderMap { get; } = new Dictionary<AuthenticationProvider, ProviderMapItem>();

        /// <summary>
        /// Constructs a new instance of <see cref="AuthenticatorBase" />
        /// </summary>
        /// <param name="credential"></param>
        /// <param name="provider"></param>
        protected AuthenticatorBase(AuthenticationCredential credential, AuthenticationProvider provider)
        {
            Credential = credential;
            Provider = provider;
        }

        /// <summary>
        /// Gets the authentication Credential.
        /// </summary>
        public AuthenticationCredential Credential { get; protected set; }

        /// <summary>
        /// Gets the authentication Provider.
        /// </summary>
        public AuthenticationProvider Provider { get; protected set; }

        /// <summary>
        /// Gets or sets the authentication initialization method.
        /// </summary>
        /// <value>
        /// The authentication initialization method.
        /// </value>
        public AuthenticationInitMethod InitializationMethod { get; set; }

        /// <summary>
        /// Gets or sets the Pace keeper for this authenticator which is used to control pacing of the workflow.
        /// </summary>
        /// <value>The Pace keeper.</value>
        public Pacekeeper Pacekeeper { get; set; } = new Pacekeeper(TimeSpan.Zero);

        /// <summary>
        /// Gets or sets a value indicating whether [lazy authentication only].
        /// This requires that the provider be set with the desired solution.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [lazy authentication only]; otherwise, <c>false</c>.
        /// </value>
        public bool LazyAuthOnly{ get; set; }

        /// <summary>
        /// Enters credentials on the device control panel and initiates the authentication process.
        /// </summary>
        public void Authenticate()
        {
            Authenticate(null);
        }

        /// <summary>
        /// Enters credentials on the device control panel and initiates the authentication process.
        /// Applies the specified parameters before submitting the authentication request.
        /// </summary>
        /// <param name="parameters">Additional configuration parameters presented on the Auth screen.</param>
        public void Authenticate(Dictionary<string, object> parameters)
        {
            StringBuilder providerDescription = new StringBuilder(Provider.GetDescription());
            IAppAuthenticator appAuthenticator = null;

            RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);

            if (InitializationMethod != AuthenticationInitMethod.Badge)
            {
                AuthenticationProvider deviceDefaultProvider = GetDefaultProvider();

                if (Provider == AuthenticationProvider.Auto)
                {
                    // Use the device default
                    Provider = deviceDefaultProvider;
                    providerDescription.Append(" - ");
                    providerDescription.Append(Provider.GetDescription());
                }

                RecordInfo(DeviceWorkflowMarker.AuthType, providerDescription.ToString());

                if (Provider != deviceDefaultProvider)
                {
                    // Set the device provider to the requested provider
                    SetDeviceProvider(Provider);
                }

                //At this point this.Provider should be correctly set, so we should use it from now on.
                appAuthenticator = GetAppAuthenticator(Provider);
            }
            else
            {
                providerDescription.Insert(0, " - ");
                providerDescription.Insert(0, InitializationMethod.GetDescription());
                RecordInfo(DeviceWorkflowMarker.AuthType, providerDescription.ToString());
                appAuthenticator = GetAppAuthenticator(AuthenticationProvider.Card);                
            }

            if (Provider == AuthenticationProvider.Card || InitializationMethod.Equals(AuthenticationInitMethod.Badge))
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, InitializationMethod.GetDescription());
                appAuthenticator.ApplyParameters(null);
            }
            else if (parameters?.Count > 0)
            {
                appAuthenticator.ApplyParameters(parameters);
            }

            RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);
            appAuthenticator.EnterCredentials();
            RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);

            if (Provider != AuthenticationProvider.Card)
            {
                appAuthenticator.SubmitAuthentication();
                if (!appAuthenticator.ValidateAuthentication())
                {
                    throw new DeviceWorkflowException($"{Credential.UserName} login failed. {appAuthenticator.ErrorMessage}");
                }
            }

            RecordEvent(DeviceWorkflowMarker.AuthenticationEnd);
        }

        /// <summary>
        /// Gets the app authenticator for the specified <see cref="AuthenticationProvider" />.
        /// </summary>
        /// <returns><see cref="IAppAuthenticator" /></returns>
        protected abstract IAppAuthenticator GetAppAuthenticator(AuthenticationProvider provider);

        /// <summary>
        /// Gets the default authentication setting from the device.
        /// </summary>
        /// <returns><see cref="AuthenticationProvider" /></returns>
        protected abstract AuthenticationProvider GetDefaultProvider();

        /// <summary>
        /// Applies the Provider to the authentication method on the device.
        /// Does nothing by default.  This is because the majority of child classes do not support setting the provider on the device
        /// </summary>
        /// <param name="provider">The authentication method</param>
        protected virtual void SetDeviceProvider(AuthenticationProvider provider)
        {
        }

    }
}
