using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.DeviceAutomation.Authentication
{
    /// <summary>
    /// A factory class for creating Solution Authenticators based on an <see cref="AuthenticationProvider" />.
    /// </summary>
    public class AppAuthenticatorFactoryCore
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly Dictionary<AuthenticationProvider, Type> _map = new Dictionary<AuthenticationProvider, Type>();

        /// <summary>
        /// Gets the number of key/value pairs in the <see cref="AppAuthenticatorFactoryCore" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Count => _map.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceFactoryCore{TFactory}" /> class.
        /// </summary>
        public AppAuthenticatorFactoryCore()
        {
            // Constructor declared for XML doc.
        }

        /// <summary>
        /// Adds the specified <see cref="AuthenticationProvider" /> and mapped type.
        /// </summary>
        /// <param name="provider">The <see cref="AuthenticationProvider" /> to map.</param>
        /// <typeparam name="TMappedType">The type that should be mapped to the specified <see cref="AuthenticationProvider" /> value.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void Add<TMappedType>(AuthenticationProvider provider)
        {
            Type mappedType = typeof(TMappedType);

            if (!typeof(IAppAuthenticator).IsAssignableFrom(mappedType))
            {
                throw new InvalidOperationException($"{mappedType.Name} is not compatible with IAppAuthenticator.");
            }

            _map.Add(provider, mappedType);
        }

        /// <summary>
        /// Creates the appropriate <see cref="IAppAuthenticator" /> object from the specified <see cref="AuthenticationProvider" /> using this instance's mappings.
        /// </summary>
        /// <param name="provider">The <see cref="AuthenticationProvider" /> enum value.</param>
        /// <param name="constructorParameters">The parameters required by the constructor of the <see cref="IAppAuthenticator" /> object.</param>
        /// <returns>The constructed <see cref="AuthenticationProvider" /> object.</returns>
        public IAppAuthenticator FactoryCreate(AuthenticationProvider provider, object[] constructorParameters)
        {
            if (provider == AuthenticationProvider.Auto)
            {
                throw new InvalidOperationException("Auto not supported?");
            }

            if (_map.TryGetValue(provider, out Type mappedType))
            {
                try
                {
                    return (IAppAuthenticator)Activator.CreateInstance(mappedType, constructorParameters);
                }
                catch (MissingMethodException ex)
                {
                    throw new DeviceFactoryCoreException($"{mappedType.Name} does not have a default constructor.", ex);
                }
            }

            throw new DeviceFactoryCoreException($"AuthenticationProvider {provider} does not have a mapped IAppAuthenticator implementation.");
        }
    }
}
