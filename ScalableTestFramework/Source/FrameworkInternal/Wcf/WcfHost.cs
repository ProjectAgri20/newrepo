using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace HP.ScalableTest.Framework.Wcf
{
    /// <summary>
    /// A base WCF service host.
    /// </summary>
    /// <typeparam name="T">The contract for this WCF service host.</typeparam>
    public class WcfHost<T> : ServiceHost where T : class
    {
        /// <summary>
        /// Gets the contract name for this WCF host.
        /// </summary>
        public string ContractName { get; } = typeof(T).FullName;

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfHost{T}" /> class.
        /// </summary>
        /// <param name="serviceType">The type that will be used to implement the service.</param>
        /// <param name="messageTransferType">The <see cref="MessageTransferType" /> to use for WCF communication.</param>
        /// <param name="uri">The URI at which the service will be hosted.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> is null.
        /// <para>or</para>
        /// <paramref name="uri" /> is null.
        /// </exception>
        public WcfHost(Type serviceType, MessageTransferType messageTransferType, Uri uri)
            : this(serviceType, BindingFactory.CreateBinding(messageTransferType), uri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfHost{T}" /> class.
        /// </summary>
        /// <param name="singletonInstance">The singleton instance of the service implementation.</param>
        /// <param name="messageTransferType">The <see cref="MessageTransferType" /> to use for WCF communication.</param>
        /// <param name="uri">The URI at which the service will be hosted.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="singletonInstance" /> is null.
        /// <para>or</para>
        /// <paramref name="uri" /> is null.
        /// </exception>
        public WcfHost(T singletonInstance, MessageTransferType messageTransferType, Uri uri)
            : this(singletonInstance, BindingFactory.CreateBinding(messageTransferType), uri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfHost{T}" /> class.
        /// </summary>
        /// <param name="serviceType">The type that will be used to implement the service.</param>
        /// <param name="binding">The <see cref="Binding" /> to use for WCF communication.</param>
        /// <param name="uri">The URI at which the service will be hosted.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> is null.
        /// <para>or</para>
        /// <paramref name="binding" /> is null.
        /// <para>or</para>
        /// <paramref name="uri" /> is null.
        /// </exception>
        private WcfHost(Type serviceType, Binding binding, Uri uri)
            : base(serviceType, uri)
        {
            SetHostBehavior(binding);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfHost{T}" /> class.
        /// </summary>
        /// <param name="singletonInstance">The singleton instance of the service implementation.</param>
        /// <param name="binding">The <see cref="Binding" /> to use for WCF communication.</param>
        /// <param name="uri">The URI at which the service will be hosted.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="singletonInstance" /> is null.
        /// <para>or</para>
        /// <paramref name="binding" /> is null.
        /// <para>or</para>
        /// <paramref name="uri" /> is null.
        /// </exception>
        private WcfHost(T singletonInstance, Binding binding, Uri uri)
            : base(singletonInstance, uri)
        {
            SetHostBehavior(binding);
        }

        private void SetHostBehavior(Binding binding)
        {
            AddServiceEndpoint(ContractName, binding, string.Empty);

            ServiceMetadataBehavior smb = GetBehavior<ServiceMetadataBehavior>();
            if (!(binding is NetNamedPipeBinding))
            {
                smb.HttpGetEnabled = true;
            }

            ServiceDebugBehavior sdb = GetBehavior<ServiceDebugBehavior>();
            sdb.IncludeExceptionDetailInFaults = true;

            ServiceThrottlingBehavior stb = GetBehavior<ServiceThrottlingBehavior>();
            stb.MaxConcurrentCalls = int.MaxValue;
            stb.MaxConcurrentInstances = int.MaxValue;
            stb.MaxConcurrentSessions = int.MaxValue;
        }

        private TBehavior GetBehavior<TBehavior>() where TBehavior : IServiceBehavior, new()
        {
            TBehavior behavior = Description.Behaviors.Find<TBehavior>();
            if (behavior == null)
            {
                behavior = new TBehavior();
                Description.Behaviors.Add(behavior);
            }
            return behavior;
        }
    }
}
